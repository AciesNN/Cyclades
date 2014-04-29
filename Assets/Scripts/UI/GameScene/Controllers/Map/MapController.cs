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
		public GameObject pr;
		public Texture2D texture;

		public GameObject objPrefab;
		public GameObject hornPrefab;
		public GameObject ownerPrefab;
		public GameObject buildPrefab;

		readonly float mapObjectHeight = 20.0f;

		MapObjectController[] horns_objects;
		MapObjectController[] army_objects;
		MapObjectController[] buildings_objects;
		MapObjectController[] owners_objects;

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
			List<long> owners = main.instance.context.GetList<long>("/map/islands/owners");
			for(int ch = 0; ch < owners.Count; ++ch) {
				buildings_objects[ch].gameObject.SetActive(false);
			}
		}

		public void OnClick() {
			//Debug.Log("OnClick");
			Vector3 pos;
			if (grid.MousePointToColliderHitPosition(out pos)) {	//TODO вообще-то лучше что-нибудь универсальное клик/тач			
				Vector2 cell = grid.WorldPositionToCell(pos);
				Coords coords = CellToCycladesCoord(cell);

				Shmipl.Base.Messenger<Coords>.Broadcast("Shmipl.Map.Click", coords);

				//Debug.Log(coords);
				//Vector3 cell_pos = grid.CellToWorldPositionOfCenter(cell);
				//GameObject.Instantiate(pr, cell_pos, Quaternion.identity);
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
			//...
			InitSeaHorns();
			InitIslandsObjects();

			//
		}

		void InitSeaHorns() {
			UnityEngine.Transform parent = terrain.transform;

			List<Coords> horns = main.instance.context.GetListCoords("/map/seas/horns");
			int ch = 0;
			foreach(Coords horn in horns) {
				Vector3 horn_coord = grid.CellToWorldPositionOfCenter(CycladesCoordToCell(horn));
				Vector3 horn_coord3 = new Vector3(horn_coord.x, mapObjectHeight, horn_coord.z);
				GameObject go = GameObject.Instantiate(hornPrefab, horn_coord3, Quaternion.identity) as GameObject;
				go.transform.parent = parent;
				go.name = "sea horn " + ch;

				ch = ch + 1;
			}
		}

		void InitIslandsObjects() {

			//1. сначала создадим, куда засовывать создаваемые объекты
			UnityEngine.Transform parent = terrain.transform;


			List<object> islands = main.instance.context.GetList("/map/islands/coords");

			//2. создадим списки ссылок 
			int l_lenght = islands.Count;
			horns_objects = new MapObjectController[l_lenght];
			army_objects = new MapObjectController[l_lenght];
			buildings_objects = new MapObjectController[l_lenght];
			owners_objects = new MapObjectController[l_lenght];

			//2. теперь создадим
			int ch = 0;
			foreach(List<object> island in islands) {

				Coords coord = new Coords((long)((List<object>)island[0])[0], (long)((List<object>)island[0])[1]); //координаты первой точки каждого острова

				//на каждом острове создадим: рога, воинов, принадлежность, 
				horns_objects[ch] = CreateObject(hornPrefab, parent, "horn " + ch, coord, 0, -10, -10);
				army_objects[ch] = CreateObject(objPrefab, parent, "army " + ch, coord, 0, 10, -10);
				owners_objects[ch] = CreateObject(ownerPrefab, parent, "owners " + ch, coord, 0, 10, -10);
				buildings_objects[ch] = CreateObject(buildPrefab, parent, "buildings " + ch, coord, 0, -10, 10);

				ch = ch + 1;
			}
		}

		public MapObjectController CreateObject(GameObject prefab, UnityEngine.Transform parent, string name, Coords coord, long count, float dx, float dy) {
			Vector3 _coord = grid.CellToWorldPositionOfCenter(CycladesCoordToCell(coord));
			Vector3 obj_coord3 = new Vector3(_coord.x + dx, mapObjectHeight, _coord.z + dy);			
			GameObject go_ = GameObject.Instantiate(prefab, obj_coord3, Quaternion.identity) as GameObject;
			go_.name = name;
			go_.transform.parent = parent;

			MapObjectController go = go_.GetComponent<MapObjectController>();
			go.SetCount(count);
			
			return go;
		}
	}
}