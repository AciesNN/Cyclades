using UnityEngine;
using System.Collections;

using Shmipl.Unity;

public class LoadData : MonoBehaviour {
	public UIInput fileName;
	public UISprite[] buttonsSprites;

	void UpdateView() {
		GetComponent<UICollectionController>().UpdateView();
	}

	void DataLoad() {
		data.LoadData((fileName.value == "" ? fileName.defaultText : fileName.value));
		UpdateView();
	}

	void SetCurPlayer(long p) {
		Cyclades.Game.Client.cur_player = p; 
		for (int i = 0; i < buttonsSprites.Length; ++i) {
			buttonsSprites[i].color = (i == (int)p ? Color.green : Color.white);
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
