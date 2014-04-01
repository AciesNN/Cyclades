using UnityEngine;
using System.Collections;

using Shmipl.Unity;

public class AdminPanel : MonoBehaviour {
	public UIInput fileName;
	public UIButton[] buttons;

	void Awake () {
		//TODO это не должно быть тут
		Application.LoadLevelAdditive("Map");
	}

	void Start() {
		//TODO это не должно быть тут
		UIDragObject mapController = GameObject.FindObjectOfType<UIDragObject>();
		GameObject mapCamera = GameObject.Find ("Map Camera");
		mapController.target = mapCamera.transform;
	}

	void UpdateView() {
		GetComponent<UICollectionController>().UpdateView();
	}

	void DataLoad() {
		data.LoadData((fileName.value == "" ? fileName.defaultText : fileName.value));
		UpdateView();
	}

	void SetCurPlayer(long p) {
		Cyclades.Game.Client.cur_player = p; 
		for (int i = 0; i < buttons.Length; ++i) {
			buttons[i].defaultColor = (i == (int)p ? Color.green : Color.red);
			buttons[i].UpdateColor(true, true);
		}
		UpdateView();
	}

	public void SetCurPlayer0() {
		SetCurPlayer(0);
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
}
