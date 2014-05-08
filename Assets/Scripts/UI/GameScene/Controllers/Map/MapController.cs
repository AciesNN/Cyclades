using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class MapController : UIController {
		Shmipl.Unity.GridController grid;
		Terrain terrain;
		public Texture2D texture;

		public GameObject objPrefab;
		public GameObject hornPrefab;
		public GameObject ownerPrefab;
		public GameObject buildPrefab;

		readonly float mapObjectHeight = 20.0f;

		MapObjectController[] horns_objects;
		MapObjectController[] army_objects;
		BuildingsSet[] buildings_objects;
		MapObjectController[] owners_objects;
		Dictionary<Coords, MapObjectController> ship_objects;

		bool isInit = false;

		// Use this for initialization
		void Start () {
			grid = GameObject.Find("grid").GetComponent<Shmipl.Unity.GridController>();
			terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
		}
		
		public override void UpdateView () {
			Update_WhoesObjects();
			Update_ArmyObjects();
			Update_HornsObjects();
			Update_BuildingsObjects();
			Update_ShipsObjects();
		}

		void Update_HornsObjects() {
			List<long> horns = main.instance.context.GetList<long>("/map/islands/horn");
			for(int ch = 0; ch < horns.Count; ++ch) {
				horns_objects[ch].SetCount(horns[ch]);
				horns_objects[ch].gameObject.SetActive(horns[ch] > 0);
			}
		}

		void Update_WhoesObjects() {
			List<long> owners = main.instance.context.GetList<long>("/map/islands/owners");
			List<long> army = main.instance.context.GetList<long>("/map/islands/army");
			for(int ch = 0; ch < owners.Count; ++ch) {
				owners_objects[ch].renderer.material.color = main.instance.GetColor(owners[ch]);
				owners_objects[ch].gameObject.SetActive(army[ch] == 0 && owners[ch] >= 0);
			}
		}

		void Update_ArmyObjects() {
			List<long> owners = main.instance.context.GetList<long>("/map/islands/owners");
			List<long> army = main.instance.context.GetList<long>("/map/islands/army");
			for(int ch = 0; ch < army.Count; ++ch) {
				army_objects[ch].renderer.material.color = main.instance.GetColor(owners[ch]);
				army_objects[ch].SetCount(army[ch]);
				army_objects[ch].gameObject.SetActive(army[ch] > 0);
			}
		}

		void Update_BuildingsObjects() {
			List<object> buildings = main.instance.context.GetList("/map/islands/buildings");
			List<bool> is_metro = main.instance.context.GetList<bool>("/map/islands/is_metro");

			for(int ch = 0; ch < is_metro.Count; ++ch) {
				List<object> cur_buildings = buildings[ch] as List<object>;
				buildings_objects[ch].SetInfo(cur_buildings, is_metro[ch], Cyclades.Game.Library.Map_IslandMetroSize(main.instance.context, ch));
			}
		}

		void Update_ShipsObjects() {
			foreach(Coords coord in ship_objects.Keys) {
				long owner = Cyclades.Game.Library.Map_GetPointOwner(main.instance.context, coord.x, coord.y);
				long count = Cyclades.Game.Library.Map_GetShipCountByPoint(main.instance.context, coord.x, coord.y);

				ship_objects[coord].renderer.material.color = main.instance.GetColor(owner);
				ship_objects[coord].SetCount(count);
				ship_objects[coord].gameObject.SetActive(count > 0);
			}
		}

		public void OnClick() { //OnClick вызывается NGUI само, просто по имени
			Vector3 pos;
			if (grid.MousePointToColliderHitPosition(out pos)) {
				Vector2 cell = grid.WorldPositionToCell(pos);
				Coords coords = CellToCycladesCoord(cell);

				Shmipl.Base.Messenger<Coords>.Broadcast("Shmipl.Map.Click", coords);
			}

			//можем еще обработать попадание не просто в карту, а в слоты зданий
			//TODO - надо бы куда-нить перенести, и плюс - очень плохая связка с grid.камера
			Ray ray = grid.mapCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int mask = 1 << LayerMask.NameToLayer("MapObject");
			
			if (Physics.Raycast(ray, out hit, grid.mapCamera.farClipPlane, mask))	{
				BuildingsSet buildingsSet = hit.collider.gameObject.transform.parent.parent.gameObject.GetComponent<BuildingsSet>();
				if (buildingsSet) {
					buildingsSet.OnClick(hit.collider.gameObject);
				}
			} 
		}

		Vector2 CycladesCoordToCell(Coords coords) {
			Vector2 res;

			//1. учтем перевернутый ыгрик 
			res = new Vector2(coords.x, (grid.cells_count.y - 1) - coords.y); //-1 потому что: например, высота 13, координата 0, надо получить 12, т.к. индексация с нуля

			//2. учтем бордюры
			res = new Vector2(res.x + 1f, res.y - 1f); //+ и - потому, что в одном случае мы отступаем от начала координат, а потом - от конца (координаты Киклад идут от верхнего угла, а сетки - от нижнего)

			//3. учтем разную длину разных линий
			res = new Vector2(res.x + System.Math.Abs(coords.y - 5)/2, res.y);

			return res;
		}
		
		Coords CellToCycladesCoord(Vector2 coords) {
			Coords res = Shmipl.Unity.GridController.Vector2ToCoords(coords);;
			
			//2. учтем перевернутый ыгрик 
			res = new Coords(res.x, ((int)grid.cells_count.y - 1) - res.y);

			//2. учтем бордюры
			res = new Coords(res.x - 1, res.y - 1); 

			//3. учтем разную длину разных линий
			res = new Coords(res.x - System.Math.Abs(res.y - 5)/2, res.y);

			return res;
		}

		public void InitMap() {
			if (isInit)
				return;
			isInit = true;

			//1. загрузим карту высот, соответствующую кол-ву игроков
			//TODO выбор картинки карты высот
			Shmipl.Unity.TerrainHeightsLoader.LoadHeighMapFromTexture(texture, terrain);

			//2. инициализируем начальные объекты: маркеры принадлежности островов, корабли, рога и т.д.
			InitObjects();
		}

		void InitObjects() {

			//- сначала создадим, куда засовывать создаваемые объекты
			UnityEngine.Transform parent = terrain.transform;

			//- создадим списки ссылок 
			List<object> islands = main.instance.context.GetList("/map/islands/coords");
			int l_lenght = islands.Count;
			horns_objects = new MapObjectController[l_lenght];
			army_objects = new MapObjectController[l_lenght];
			buildings_objects = new BuildingsSet[l_lenght];
			owners_objects = new MapObjectController[l_lenght];

			//- теперь создадим
			int ch = 0;
			foreach(List<object> island in islands) {

				Coords coord = new Coords((long)((List<object>)island[0])[0], (long)((List<object>)island[0])[1]); //координаты первой точки каждого острова

				//на каждом острове создадим: рога, воинов, принадлежность, 
				horns_objects[ch] = CreateObject(hornPrefab, parent, "horn " + ch, coord, -10, -10).GetComponent<MapObjectController>();
				army_objects[ch] = CreateObject(objPrefab, parent, "army " + ch, coord, 10, -10).GetComponent<MapObjectController>();
				owners_objects[ch] = CreateObject(ownerPrefab, parent, "owners " + ch, coord, 10, -10).GetComponent<MapObjectController>();

				buildings_objects[ch] = CreateObject(buildPrefab, parent, "buildings " + ch, coord, -10, 10).GetComponent<BuildingsSet>();
				buildings_objects[ch].coord = coord;

				ch = ch + 1;
			}

			//- создадим корабли
			ship_objects = new Dictionary<Coords, MapObjectController>();
			long map_x = main.instance.context.GetLong ("/map/size_x");
			long map_y = main.instance.context.GetLong ("/map/size_y");
			long y_limit = Cyclades.Game.Library.Map_GetYLimit(map_y);
			for (long y = 0; y <= y_limit; ++y) {
				long x_limit = Cyclades.Game.Library.Map_GetXLimit(map_x, map_y, y);
				for (long x = 0; x <= x_limit; ++x) {
					if (Cyclades.Game.Library.Map_IsPointOnMap(main.instance.context, x, y) && Cyclades.Game.Library.Map_GetIslandByPoint(main.instance.context, x, y) == -1) {
						Coords coord = new Coords(x, y);
						ship_objects[coord] = CreateObject(objPrefab, parent, "ship " + x + " " + y, coord, 0, 0).GetComponent<MapObjectController>();
					}
				}
			}

			//- создадим морские рога (их мы не пишем ни в какие массивы, потому что они неизменны)
			List<Coords> horns = main.instance.context.GetListCoords("/map/seas/horns");
			foreach(Coords coord in horns) {
				CreateObject(hornPrefab, parent, "sea horn " + coord.x + " " + coord.y, coord, 10, -10).GetComponent<MapObjectController>();
			}
		}

		public GameObject CreateObject(GameObject prefab, UnityEngine.Transform parent, string name, Coords coord, float dx, float dy) {
			Vector3 _coord = grid.CellToWorldPositionOfCenter(CycladesCoordToCell(coord));
			Vector3 obj_coord3 = new Vector3(_coord.x + dx, mapObjectHeight, _coord.z + dy);			
			GameObject go = GameObject.Instantiate(prefab, obj_coord3, Quaternion.identity) as GameObject;
			go.name = name;
			go.transform.parent = parent;

			return go;
		}
	}
}