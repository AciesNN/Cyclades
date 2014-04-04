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
			Debug.Log(cell);
			Vector3 cell_pos = grid.CellToWorldPositionOfCenter(cell);

			GameObject.Instantiate(pr, cell_pos, Quaternion.identity);
		}
	}

	void InitMap() {
		Shmipl.Unity.TerrainHeightsLoader.LoadHeighMapFromTexture(texture, terrain);
	}
}
