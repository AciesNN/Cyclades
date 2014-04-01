using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
	GridController grid;

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
		if (grid.MousePointToColliderHitPosition(out pos)) {				
			Vector2 cell = grid.WorldPositionToCell(pos);
			Debug.Log(cell);
		}
	}
}
