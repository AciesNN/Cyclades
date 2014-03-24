using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	
	public const float ship_speed = 1f;
	Vector3 goal;
	bool isMoved = false;
	
	public Shmipl.FrmWrk.Library.Coords coord;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoved) { 
			transform.position = Vector3.Lerp(transform.position, goal, Time.deltaTime * ship_speed);
			transform.LookAt(goal);
		}	
	}
	
	public void MoveTo(Vector3 goal) {
		this.goal = goal;
		this.isMoved = true;
	}
	public void StopMove() {
		this.isMoved = false;
	}
}
