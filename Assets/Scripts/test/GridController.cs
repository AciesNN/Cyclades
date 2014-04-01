using UnityEngine;
using System.Collections;

public enum CellMode
{
	QuadCell, HexCell
}

public class GridController : MonoBehaviour {
	public CellMode cellMode;

	//количество клеток
	public Vector2 cells_count;

	public Vector2 cell_size;
	
	public Vector2 texture_scale;
	public Vector2 realTextureSize;

	public Camera mapCamera;
		
	Vector3 lastMousePos;
	bool was_drag = false;
		
	// Use this for initialization
	void Awake () {
			

		if (cellMode == CellMode.HexCell) {
			cell_size = Vector2.Scale(new Vector2(realTextureSize.x / 2f, realTextureSize.y), texture_scale);
		} else {
			realTextureSize = new Vector2(gameObject.renderer.material.mainTexture.width, gameObject.renderer.material.mainTexture.height);
			cell_size = Vector2.Scale(new Vector2(realTextureSize.x , realTextureSize.y), texture_scale);
		}

		float localScaleX = cell_size.x * cells_count.x;
		float localScaleY = cell_size.y * cells_count.y;		
		
		if (cellMode == CellMode.HexCell) {
			localScaleX = localScaleX + 0f;
			localScaleY = localScaleY + 0.5f;
		}
		
		transform.localScale = new Vector3(localScaleX, localScaleY, 1f);

		//сместимся относительно корня на половину
		if (transform.root.gameObject != gameObject) {
			transform.localPosition = new Vector3(transform.localScale.x/2f, transform.localPosition.y, transform.localScale.y/2f);
		}
		
		//поправим текстуру
		if (cellMode == CellMode.HexCell) {
			gameObject.renderer.material.mainTextureScale = new Vector2(cells_count.x/2f, cells_count.y + 0.5f);
			gameObject.renderer.material.mainTextureOffset = new Vector2(1f/6f, 0);
		} else {
			gameObject.renderer.material.mainTextureScale = cells_count;
		}
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		if (Input.GetMouseButtonDown(0)) {
			was_drag = false;	
			lastMousePos = Input.mousePosition;
		} 
		
		if (Input.GetMouseButton(0)) {
			was_drag = was_drag || !Vector3.Equals(lastMousePos, Input.mousePosition);
			lastMousePos = Input.mousePosition;	
		}
		
		if (Input.GetMouseButtonUp(0)) {
			if (!was_drag) {
				Vector3 pos;
				if (MousePointToColliderHitPosition(out pos)) {				
					Vector2 cell = WorldPositionToCell(pos);		
					Shmipl.Base.Messenger<Shmipl.FrmWrk.Library.Coords>.Broadcast("BoardClick", Vector2ToCoords(cell));			
				} 
			}
		}
		
		
		/**/
		
        int count = Input.touchCount;
		Touch touch; 
        for (int i = 0; i < count && i < 1; i++) {
            touch = Input.GetTouch (i);
			
			if (touch.phase == TouchPhase.Began) {
				was_drag = false;	
				lastMousePos = touch.position;
			} 
			
			if (touch.phase == TouchPhase.Moved) {
				was_drag = was_drag || !Vector3.Equals(lastMousePos, Input.mousePosition);
				lastMousePos = touch.position;	
			}
			
			if (touch.phase == TouchPhase.Ended) {
				if (!was_drag) {
					Vector3 pos;
					if (TouchPointToColliderHitPosition(touch, out pos)) {				
						Vector2 cell = WorldPositionToCell(pos);		
						Shmipl.Base.Messenger<Shmipl.FrmWrk.Library.Coords>.Broadcast("BoardClick", Vector2ToCoords(cell));			
					} 
				}
			}
		}			
	}
	
	public bool MousePointToColliderHitPosition(out Vector3 pos) {

		Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);
    	RaycastHit hit;
    	int mask = 1 << 8; //world layer #8 !!!
		
		if (Physics.Raycast(ray, out hit, mapCamera.farClipPlane, mask))	{
			if (hit.collider == collider) {
				pos = hit.point;
				return true;			
			}
		} 
		
		pos = Vector3.zero;
		return false;
		
	}
	
	public bool TouchPointToColliderHitPosition(Touch touch, out Vector3 pos) {

		Ray ray = mapCamera.ScreenPointToRay(touch.position);
    	RaycastHit hit;
    	int mask = 1 << 8; //world layer #8 !!!
		
		if (Physics.Raycast(ray, out hit, mapCamera.farClipPlane, mask))	{
			if (hit.collider == collider) {
				pos = hit.point;
				return true;			
			}
		} 
		
		pos = Vector3.zero;
		return false;
		
	}
	
	public Vector2 WorldPositionToCell(Vector3 pos) {
				
		Vector3 local_grid_pos = pos - transform.root.position;
		Vector2 grid_pos = new Vector2(local_grid_pos.x / transform.localScale.x, local_grid_pos.z / transform.localScale.y);
		
		Vector2 cell = Vector2.Scale(cells_count, grid_pos);
		return cell;
				
	}
	
	public Vector3 CellToWorldPositionOfCenter(Vector2 cell) {
		
		Shmipl.FrmWrk.Library.Coords coord = Vector2ToCoords(cell);
		float normCellX;
		float normCellY;

		if (cellMode == CellMode.HexCell) {
			normCellX = ((float)coord.x) + 0.5f + 1f/6f;
			normCellY = (float)coord.y + 0.5f + (coord.x%2 == 0 ? 0 : 0.5f);	
		} else {
			normCellX = coord.x + 0.5f;
			normCellY = coord.y + 0.5f;
		}
		
		Vector2 norm_cell = new Vector2(normCellX, normCellY);
		Vector2 cell_pos = Vector2.Scale(norm_cell, cell_size);
		
		Vector3 pos = transform.root.position + new Vector3(cell_pos.x, 0f, cell_pos.y);
		return pos;
		
	}
	
	public int GetCellSizeX() {
		return (int)System.Math.Floor(cells_count.x);
	}
	
	public int GetCellSizeY() {
		return (int)System.Math.Floor(cells_count.y);
	}
	
	public static Shmipl.FrmWrk.Library.Coords Vector2ToCoords(Vector2 cell) {
		return new Shmipl.FrmWrk.Library.Coords((int)System.Math.Floor(cell.x)
												, (int)System.Math.Floor(cell.y));
	}

	public static Vector2 CoordsToVector2(Shmipl.FrmWrk.Library.Coords cell) {
		return new Vector2(cell.x, cell.y);
	}
	
	public Rect GetRect(int x1, int y1, int x2, int y2) {
		
		Vector2 cell1 = new Vector2(x1, y1);
		Vector3 pos1 = CellToWorldPositionOfCenter(cell1);
		
		Rect rect = new Rect (pos1.x, pos1.z, (x2-x1+1)*cell_size.x, (y2-y1+1)*cell_size.y);
		return rect;
	}
}
