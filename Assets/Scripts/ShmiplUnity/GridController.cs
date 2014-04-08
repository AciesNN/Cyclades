using UnityEngine;
using System.Collections;

namespace Shmipl.Unity
{
	public enum CellMode
	{
		QuadCell, HexCell
	}

	public enum HexModeOrientation
	{
		Horizontal, Vertical
	}

	public class GridController : MonoBehaviour {
		public CellMode cellMode;
		HexModeOrientation hexModeOrientation;

		//количество клеток
		public Vector2 cells_count;

		public Vector2 cell_size;
		
		public Vector2 texture_scale;
		public Vector2 realTextureSize;

		public Camera mapCamera;
			
		// Use this for initialization
		void Awake () {

			if (cellMode == CellMode.HexCell) {
				hexModeOrientation = ( realTextureSize.x > realTextureSize.y ? HexModeOrientation.Horizontal : HexModeOrientation.Vertical );
			}

			if (cellMode == CellMode.HexCell) {
				if (hexModeOrientation == HexModeOrientation.Horizontal) {
					cell_size = Vector2.Scale(new Vector2(realTextureSize.x / 2f, realTextureSize.y), texture_scale);
				} else {
					cell_size = Vector2.Scale(new Vector2(realTextureSize.x, realTextureSize.y / 2f), texture_scale);
				}
			} else {
				realTextureSize = new Vector2(gameObject.renderer.material.mainTexture.width, gameObject.renderer.material.mainTexture.height);
				cell_size = Vector2.Scale(new Vector2(realTextureSize.x, realTextureSize.y), texture_scale);
			}

			float localScaleX = cell_size.x * cells_count.x;
			float localScaleY = cell_size.y * cells_count.y;		
			
			/*if (cellMode == CellMode.HexCell) {
				if (hexModeOrientation == HexModeOrientation.Horizontal) {
					localScaleX = localScaleX + 0f;
					localScaleY = localScaleY + 0.5f;
				} else {
					localScaleX = localScaleX + 0.5f;
					localScaleY = localScaleY + 0f;
				}
			}*/
			
			transform.localScale = new Vector3(localScaleX, localScaleY, 1f);

			//сместимся относительно корня на половину
			if (transform.root.gameObject != gameObject) {
				transform.localPosition = new Vector3(transform.localScale.x/2f, transform.localPosition.y, transform.localScale.y/2f);
			}
			
			//поправим текстуру
			if (cellMode == CellMode.HexCell) {
				if (hexModeOrientation == HexModeOrientation.Horizontal) {
					gameObject.renderer.material.mainTextureScale = new Vector2(cells_count.x/2f + 0.5f, cells_count.y + 0.5f);
				} else {
					gameObject.renderer.material.mainTextureScale = new Vector2(cells_count.x + 0.5f, cells_count.y/2f + 0.5f);
				}
				//gameObject.renderer.material.mainTextureOffset = new Vector2(1f/6f, 0);
			} else {
				gameObject.renderer.material.mainTextureScale = cells_count;
			}
		
		}
		
		public bool MousePointToColliderHitPosition(out Vector3 pos) {

			Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);
	    	RaycastHit hit;
	    	int mask = 1 << LayerMask.NameToLayer("Map");
			
			if (Physics.Raycast(ray, out hit, mapCamera.farClipPlane, mask))	{
				if (hit.collider == transform/*.root*/.gameObject.collider) {
					pos = hit.point;
					return true;			
				}
			} 
			
			pos = Vector3.zero;
			return false;
			
		}

		/*
		public bool TouchPointToColliderHitPosition(Touch touch, out Vector3 pos) {

			Ray ray = mapCamera.ScreenPointToRay(touch.position);
	    	RaycastHit hit;
			int mask = 1 << LayerMask.NameToLayer("Map");
			
			if (Physics.Raycast(ray, out hit, mapCamera.farClipPlane, mask))	{
				if (hit.collider == collider) {
					pos = hit.point;
					return true;			
				}
			} 
			
			pos = Vector3.zero;
			return false;
			
		}
		*/

		public Vector2 WorldPositionToCell(Vector3 pos) {
					
			Vector3 root_pos = new Vector3(transform.position.x - transform.localScale.x/2f, 0f, transform.position.z - transform.localScale.y/2f);
			Vector3 local_grid_pos = pos - root_pos/*transform.root.position*/;
			Vector2 grid_pos = new Vector2(local_grid_pos.x / transform.localScale.x, local_grid_pos.z / transform.localScale.y);
			
			Vector2 cell_ = Vector2.Scale(cells_count, grid_pos);
			Vector2 cell;
			if (hexModeOrientation == HexModeOrientation.Horizontal) {
				cell = new Vector2(cell_.x - 0.5f, cell_.y);
			} else {
				cell = new Vector2(cell_.x, cell_.y - 0.5f);
			}
			return cell;
					
		}
		
		public Vector3 CellToWorldPositionOfCenter(Vector2 cell) {
			
			Shmipl.FrmWrk.Library.Coords coord = Vector2ToCoords(cell);
			float normCellX;
			float normCellY;

			if (cellMode == CellMode.HexCell) {
				if (hexModeOrientation == HexModeOrientation.Horizontal) {
					normCellX = ((float)coord.x) + 0.5f/* + 1f/6f*/;
					normCellY = (float)coord.y + 0.5f + (coord.x%2 == 0 ? 0 : 0.5f);
				} else {
					normCellX = (float)coord.x + 0.5f + (coord.y%2 == 0 ? 0 : 0.5f);
					normCellY = ((float)coord.y) + 1f /*+ 1f/6f*/;
				}

			} else {
				normCellX = coord.x + 0.5f;
				normCellY = coord.y + 0.5f;
			}
			
			Vector2 norm_cell = new Vector2(normCellX, normCellY);
			Vector2 cell_pos = Vector2.Scale(norm_cell, cell_size);

			Vector3 root_pos = new Vector3(transform.position.x - transform.localScale.x/2f, 0f, transform.position.z - transform.localScale.y/2f);
			Vector3 pos = /*transform.root.position*/ root_pos + new Vector3(cell_pos.x, 0f, cell_pos.y);
			return pos;
			
		}
		
		public static Vector2 CoordsToVector2(Shmipl.FrmWrk.Library.Coords cell) {
			return new Vector2(cell.x, cell.y);
		}
		
		public static Shmipl.FrmWrk.Library.Coords Vector2ToCoords(Vector2 cell) {
			return new Shmipl.FrmWrk.Library.Coords((int)System.Math.Floor(cell.x)
			                                        , (int)System.Math.Floor(cell.y));
		}

		/*public int GetCellSizeX() {
			return (int)System.Math.Floor(cells_count.x);
		}
		
		public int GetCellSizeY() {
			return (int)System.Math.Floor(cells_count.y);
		}

		public Rect GetRect(int x1, int y1, int x2, int y2) {
			
			Vector2 cell1 = new Vector2(x1, y1);
			Vector3 pos1 = CellToWorldPositionOfCenter(cell1);
			
			Rect rect = new Rect (pos1.x, pos1.z, (x2-x1+1)*cell_size.x, (y2-y1+1)*cell_size.y);
			return rect;
		}*/
	}
}