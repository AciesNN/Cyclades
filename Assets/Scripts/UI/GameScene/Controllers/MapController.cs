﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class MapController : MonoBehaviour {
		Shmipl.Unity.GridController grid;
		Terrain terrain;
		public GameObject pr;
		public Texture2D texture;

		public GameObject hornPrefab;
		public GameObject objPrefab;

		readonly float mapObjectHeight = 20.0f;

		bool isInit = false;

		// Use this for initialization
		void Start () {
			grid = GameObject.Find("grid").GetComponent<Shmipl.Unity.GridController>();
			terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
		}
		
		// Update is called once per frame
		void Update () {
		
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
			List<Coords> horns = data.context.GetListCoords("/map/seas/horns");
			foreach(Coords horn in horns) {
				Vector3 horn_coord = grid.CellToWorldPositionOfCenter(CycladesCoordToCell(horn));
				Vector3 horn_coord3 = new Vector3(horn_coord.x, mapObjectHeight, horn_coord.z);
				GameObject.Instantiate(hornPrefab, horn_coord3, Quaternion.identity);
			}
		}

		void InitIslandsObjects() {

			//1. сначала создадим, куда засовывать создаваемые объекты
			Transform parent = terrain.transform;

			//2. теперь создадим
			List<object> islands = data.context.GetList("/map/islands/coords");
			foreach(List<object> island in islands) {

				Coords coord = new Coords((long)((List<object>)island[0])[0], (long)((List<object>)island[0])[1]); //координаты первой точки каждого острова

				//на каждом острове создадим: рога, воинов, принадлежность, 
				CreateObject(parent, "horn", coord, 0, -10, -10);
				CreateObject(parent, "army", coord, 0, 10, 10);
				CreateObject(parent, "whos", coord, 0, 10, -10);
				CreateObject(parent, "buildings", coord, 0, -10, 10);
			}
		}

		public MapObjectController CreateObject(Transform parent, string name, Coords coord, long count, float dx, float dy) {
			Vector3 _coord = grid.CellToWorldPositionOfCenter(CycladesCoordToCell(coord));
			Vector3 obj_coord3 = new Vector3(_coord.x + dx, mapObjectHeight, _coord.z + dy);			
			GameObject go_ = GameObject.Instantiate(objPrefab, obj_coord3, Quaternion.identity) as GameObject;
			go_.name = name;
			go_.transform.parent = parent;

			MapObjectController go = go_.GetComponent<MapObjectController>();
			go.SetCount(count);
			
			return go;
		}
	}
}