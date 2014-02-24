using UnityEngine;
using System.Collections;

public class AddControl : MonoBehaviour {
	public UIWidget control;
	public UIWidget parent;
	public Transform[] placementArray;
	public int num;
	Transform temp;
	// Use this for initialization
	void Start () {
		Spawn();
	}
	
	void Spawn() 
	{ 
		for(int i=0;i<num;i++) 
		{
			int j= Random.Range(0, placementArray.Length-1-i);
			Transform pos= placementArray[j] as Transform;	
			
			UIWidget obj = Instantiate(control, pos.position, pos.rotation) as UIWidget;
			obj.transform.parent=parent.transform;
			obj.transform.localScale=control.transform.localScale;

			temp=placementArray[j];
			placementArray[j]=placementArray[placementArray.Length-1-i];
			placementArray[placementArray.Length-1-i]=temp;
			
			
		}
		
		
		
		
	}
}
