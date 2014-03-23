using UnityEngine;
using System.Collections;

public class AllShipsController : MonoBehaviour {
		
	public Transform active_ship;	
	GridController grid_controller;
	GameObject [,] ships;
	int ships_x, ships_y;
	public GameObject ship_prefub;
	
	public GameObject goal_marker_prefab;
	public GameObject goal_marker = null;
	
	void Awake() {
		Shmipl.Base.Messenger<Shmipl.FrmWrk.Library.Coords>.AddListener("BoardClick", OnBoardClick);
	}
	
	// Use this for initialization
	void Start () {
		grid_controller = GameObject.Find("grid").GetComponent<GridController>(); 
		
		ships_x = grid_controller.GetCellSizeX();
		ships_y = grid_controller.GetCellSizeY();
		
		ships = new GameObject[ships_x, ships_y];
		
		/*AddShip(0, 0);
		AddShip(0, 1);
		AddShip(1, 0);
		AddShip(1, 1);*/

		AddShip(5, 8);
		AddShip(4, 4);
		AddShip(7, 7);

	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void LateUpdate() {
		bool delete_goal_marker = false;

		if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && active_ship && goal_marker_prefab) {
			Vector3 m_pos;
			if (grid_controller.MousePointToColliderHitPosition(out m_pos)) {
				Vector2 cell = grid_controller.WorldPositionToCell(m_pos);
				
				Shmipl.FrmWrk.Library.Coords coord = GridController.Vector2ToCoords(cell);
				if (ships[coord.x, coord.y]) {
					delete_goal_marker = true;
				} else {				
					Vector3 pos = grid_controller.CellToWorldPositionOfCenter(cell);
					
					if (!goal_marker) {
						goal_marker = Instantiate(goal_marker_prefab) as GameObject;
					}
					if (!goal_marker.activeSelf) {
						goal_marker.SetActive(true);
					}

					goal_marker.transform.position = pos;
				}
				
			} else {
				delete_goal_marker = true;
			}
		} else {
			delete_goal_marker = true;
		}
		
		if (delete_goal_marker && goal_marker){
			Destroy(goal_marker);
			goal_marker = null;
		}	
	}
	
	void OnBoardClick(Shmipl.FrmWrk.Library.Coords coord) {
		if (ships[coord.x, coord.y]) {
			SetActiveShip(ships[coord.x, coord.y].transform);
		} else {
			if (active_ship) {
				Ship ship = active_ship.GetComponent<Ship>();
				Vector3 goal = grid_controller.CellToWorldPositionOfCenter( GridController.CoordsToVector2(coord) );
				ship.MoveTo(goal);
				
				ships[coord.x, coord.y] = active_ship.gameObject;
				ships[ship.coord.x, ship.coord.y] = null;
				ship.coord = coord;
			}
		}	
	}
	
	void OnGUI () {
		GUILayout.BeginHorizontal();		
		
		if (GUILayout.Button("Добавить случайный"))
			TestAddRandomShip();
			
		if (GUILayout.Button("Утопить") && active_ship) {
			active_ship.gameObject.AddComponent<DestroyShip>();
			SetActiveShip(null);
		}
			
		if (GUILayout.Button("Вернуться в меню"))
			Application.LoadLevel(0);
		
		GUILayout.EndHorizontal();
	}
	
	void TestAddRandomShip() {
		
		AddShip((int)Random.Range(0, ships_x), (int)Random.Range(0, ships_y));

	}
	
	void AddShip(int x, int y) {
		if (ships[x, y] == null) {
			Vector3 new_ship_pos = grid_controller.CellToWorldPositionOfCenter(new Vector2(x, y));
			GameObject new_ship = GameObject.Instantiate(ship_prefub, new_ship_pos, Quaternion.identity) as GameObject;
			ships[x, y] = new_ship;
			
			Ship ship = new_ship.GetComponent<Ship>();
			ship.coord = new Shmipl.FrmWrk.Library.Coords(x, y);
			
			SetActiveShip(new_ship.transform);
		}
	}
	
	void SetActiveShip(Transform new_active_ship) {
		if (active_ship)
			SetActiveShipParams(active_ship, false);
		
		if (new_active_ship)
			SetActiveShipParams(new_active_ship, true);
		
		active_ship = new_active_ship;
	}
	
	void SetActiveShipParams(Transform ship, bool onoff) {
		ship.Find("active_light").gameObject.SetActive(onoff);
		ship.Find("active_ring").gameObject.SetActive(onoff);	
	}
}
