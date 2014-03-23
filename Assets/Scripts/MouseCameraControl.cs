using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse camera control")]
public class MouseCameraControl : MonoBehaviour {
	
	public float xAngleSpeed = 20f;
	public float yAngleSpeed = 20f;
	
	public float yMinAngleLimit = 10f;
	public float yMaxAngleLimit = 90f;
	
	public float yMinLimit = 0.5f;
	public float yMaxLimit = 2.5f;
	public float mw_speed = 1f;
	
	public float xSpeed = 1f;
	public float ySpeed = 1f;
	
	public Vector2 lastMousePos;
	public bool bMouseIsPressed;
	public Camera mapCamera;

	GridController grid_controller;
	public Rect coordLimit;

	void Start () {	
		mapCamera=gameObject.camera;
		CameraOrbit(0f, 0f);
	   	if (rigidbody)
			rigidbody.freezeRotation = true;
		
		grid_controller = GameObject.Find("grid").GetComponent<GridController>(); 
		coordLimit = grid_controller.GetRect(-1, -1, grid_controller.GetCellSizeX(), grid_controller.GetCellSizeY());
	}
	
	void LateUpdate () {
		
	    if (Input.GetMouseButton(1)) {			
			CameraOrbit(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		}
		
		CameraScale(Input.GetAxis("Mouse ScrollWheel"));
		
        int count = Input.touchCount;
		Touch touch; 
        for (int i = 0; i < count && i < 1; i++) {
            touch = Input.GetTouch (i); 
			if (touch.phase == TouchPhase.Moved) {
				 CameraTransform(lastMousePos, touch.position);
			}
			lastMousePos = touch.position;
		}
		
		if (Input.GetMouseButton(0)) {
			if (bMouseIsPressed)
				CameraTransform(lastMousePos, Input.mousePosition); //new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
			bMouseIsPressed = true;
			lastMousePos = Input.mousePosition;
		} else {
			bMouseIsPressed = false;
		}
	}
	
	void CameraScale(float mw) {
		
		if (mw == 0f)
			return;
		
		float y = Mathf.Clamp(-mw * mw_speed + transform.position.y, yMinLimit, yMaxLimit);
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
		
	}
	
	void CameraTransform(Vector2 old_m_pos, Vector2 m_pos) {
		
		Ray ray = mapCamera.ScreenPointToRay(m_pos);
    	RaycastHit hit;
    	
		if (!Physics.Raycast(ray, out hit))
			return;
		if (!hit.collider)
			return;
		
		Vector3 pos = hit.point;

		Ray old_ray = mapCamera.ScreenPointToRay(old_m_pos);
    	RaycastHit old_hit;
    	
		if (!Physics.Raycast(old_ray, out old_hit))
			return;
		if (!old_hit.collider)
			return;
		
		Vector3 old_pos = old_hit.point;
		
		if (Vector3.Equals(old_pos, pos))
			return;
		
		Vector3 d_pos = pos - old_pos;
		
		float new_x = Mathf.Clamp(transform.position.x - d_pos.x, coordLimit.xMin, coordLimit.xMax);
		float new_z = Mathf.Clamp(transform.position.z - d_pos.z, coordLimit.yMin, coordLimit.yMax);		
		
		transform.position = new Vector3(new_x, transform.position.y, new_z);
		
	}
	
	void CameraOrbit(float dx, float dy) {
		
	    float x = transform.eulerAngles.y;
	    float y = transform.eulerAngles.x;
		
		x -= dx * xAngleSpeed;
        y += dy * yAngleSpeed;
 		
 		y = ClampAngle(y, yMinAngleLimit, yMaxAngleLimit);
        
        transform.rotation = Quaternion.Euler(y, x, 0f);
		
	}
	
	float ClampAngle (float angle, float min, float max) {
		if (angle < -360f)
			angle += 360f;
		if (angle > 360f)
			angle -= 360f;
		return Mathf.Clamp (angle, min, max);
	}
}
