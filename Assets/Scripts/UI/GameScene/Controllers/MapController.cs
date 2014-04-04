using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Shmipl.FrmWrk.Library;

public class MapController : MonoBehaviour {
	Shmipl.Unity.GridController grid;
	Terrain terrain;
	public GameObject pr;
	public Texture2D texture;
	public GameObject hornPrefab;

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
		Debug.Log("OnClick");
		Vector3 pos;
		if (grid.MousePointToColliderHitPosition(out pos)) {	//TODO вообще-то лучше что-нибудь универсальное клик/тач			
			Vector2 cell = grid.WorldPositionToCell(pos);
			Coords coords = CellToCycladesCoord(cell);
			Debug.Log(coords);

			//Vector3 cell_pos = grid.CellToWorldPositionOfCenter(cell);
			//GameObject.Instantiate(pr, cell_pos, Quaternion.identity);
		}
	}

	Vector2 CycladesCoordToCell(Coords coords) {
		Vector2 res;

		//1. учтем перевернутый ыгрик 
		res = new Vector2(coords.x, grid.cells_count.y - coords.y);

		//2. учтем бордюры
		res = new Vector2(res.x + 1f, res.y + 0f);

		//3. учтем разную длину разных линий
		res = new Vector2(res.x + System.Math.Abs(coords.y - 5)/2, res.y);

		return res;
	}
	
	Coords CellToCycladesCoord(Vector2 coords) {
		Coords res = Shmipl.Unity.GridController.Vector2ToCoords(coords);;
		
		//2. учтем перевернутый ыгрик 
		res = new Coords(res.x, (int)grid.cells_count.y - res.y - 2); //TODO - тут непонятная двойка из-за того, что размер решетки "не честный"

		//2. учтем бордюры
		res = new Coords(res.x - 1, res.y - 0);

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
		InitHorns();

		//
	}

	void InitHorns() {
		List<Coords> horns = data.context.GetListCoords("/map/seas/horns");
		foreach(Coords horn in horns) {
			Vector3 horn_coord = grid.CellToWorldPositionOfCenter(CycladesCoordToCell(horn));
			Vector3 horn_coord3 = new Vector3(horn_coord.x, mapObjectHeight, horn_coord.z);
			GameObject.Instantiate(hornPrefab, horn_coord3, Quaternion.identity);
		}
	}
}
