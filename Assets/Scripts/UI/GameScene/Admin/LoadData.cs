using UnityEngine;
using System.Collections;

using Shmipl.Unity;

public class LoadData : MonoBehaviour {
	public UIInput fileName;
		
	void DataLoad() {
		data.LoadData((fileName.value == "" ? fileName.defaultText : fileName.value));
		GetComponent<UICollectionController>().UpdateView();
	}
}
