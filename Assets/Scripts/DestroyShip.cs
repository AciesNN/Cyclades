using UnityEngine;
using System.Collections;


public class DestroyShip : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 2);
		animation.enabled = true;
		animation.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
