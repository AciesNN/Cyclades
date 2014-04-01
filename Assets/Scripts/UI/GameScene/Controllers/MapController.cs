using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
	GridController grid;
	public GameObject pr;

	// Use this for initialization
	void Start () {
		grid = GameObject.Find("grid").GetComponent<GridController>();
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
}
