using UnityEngine;
using System.Collections;

using Shmipl.Unity;

public class LoadData : MonoBehaviour {
	public UIInput fileName;

	void UpdateView() {
		GetComponent<UICollectionController>().UpdateView();
	}

	void DataLoad() {
		data.LoadData((fileName.value == "" ? fileName.defaultText : fileName.value));
		UpdateView();
	}

	void SetCurPlayer(long p) {
		data.cur_player = p; 
		UpdateView();
	}

	public void SetCurPlayer1() {
		SetCurPlayer(1);
	}

	public void SetCurPlayer2() {
		SetCurPlayer(2);
	}

	public void SetCurPlayer3() {
		SetCurPlayer(3);
	}

	public void SetCurPlayer4() {
		SetCurPlayer(4);
	}

	public void SetCurPlayer5() {
		SetCurPlayer(5);
	}
}
