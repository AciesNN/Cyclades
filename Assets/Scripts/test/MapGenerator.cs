using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {
	public UISprite[] mapTile;
	public string[] tileID;
	// Use this for initialization
	void Start () {
		for (int i=0; i<mapTile.Length; i++) {
			mapTile[i].spriteName=tileID[i];


				}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
