using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
	Shmipl.Unity.GridController grid;
	Terrain terrain;
	public GameObject pr;
	public Texture2D texture;

	// Use this for initialization
	void Start () {
		grid = GameObject.Find("grid").GetComponent<Shmipl.Unity.GridController>();
		terrain = GameObject.Find("Terrain").GetComponent<Terrain>();

		InitMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() {
		Debug.Log("OnClick");
		Vector3 pos;
		if (grid.MousePointToColliderHitPosition(out pos)) {	//TODO вообще-то лучше что-нибудь универсальное клик/тач			
			Vector2 cell = grid.WorldPositionToCell(pos);
			Shmipl.FrmWrk.Library.Coords coords = CellToCycladesCoord(cell);
			Debug.Log(coords);

			//Vector3 cell_pos = grid.CellToWorldPositionOfCenter(cell);
			//GameObject.Instantiate(pr, cell_pos, Quaternion.identity);
		}
	}

	Vector2 CycladesCoordToCell(Shmipl.FrmWrk.Library.Coords coords) {
		Vector2 res;

		//1. учтем перевернутый ыгрик 
		res = new Vector2(coords.x, grid.cells_count.y - coords.y);

		//2. учтем бордюры
		res = new Vector2(res.x + 1f, res.y + 0f);

		//3. учтем разную длину разных линий
		res = new Vector2(res.x + System.Math.Abs(coords.y - 5)/2, res.y);

		return res;
	}
	
	Shmipl.FrmWrk.Library.Coords CellToCycladesCoord(Vector2 coords) {
		Shmipl.FrmWrk.Library.Coords res = Shmipl.Unity.GridController.Vector2ToCoords(coords);;
		
		//2. учтем перевернутый ыгрик 
		res = new Shmipl.FrmWrk.Library.Coords(res.x, (int)grid.cells_count.y - res.y - 2); //TODO - тут непонятная двойка из-за того, что размер решетки "не честный"

		//2. учтем бордюры
		res = new Shmipl.FrmWrk.Library.Coords(res.x - 1, res.y - 0);

		//3. учтем разную длину разных линий
		res = new Shmipl.FrmWrk.Library.Coords(res.x - System.Math.Abs(res.y - 5)/2, res.y);

		return res;
	}

	void InitMap() {
		//1. загрузим карту высот, соответствующую кол-ву игроков
		//TODO выбор картинки карты высот
		Shmipl.Unity.TerrainHeightsLoader.LoadHeighMapFromTexture(texture, terrain);

		//2. инициализируем начальные объекты: маркеры принадлежности островов, корабли, рога и т.д.
		//...

		//
	}
}
