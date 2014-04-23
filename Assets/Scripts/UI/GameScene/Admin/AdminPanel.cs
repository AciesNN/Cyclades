using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AdminPanel : MonoBehaviour {
		public UIInput fileName;
		public UIButton[] buttons;
		MapController mapController;

		void Awake () {
			//TODO это не должно быть тут
			Application.LoadLevelAdditive("Map");
		}

		void Start() {
			//TODO это не должно быть тут
			UIDragObject mapDnDController = GameObject.FindObjectOfType<UIDragObject>();
			GameObject mapCamera = GameObject.Find ("Map Camera");
			mapDnDController.target = mapCamera.transform;

			mapController = GameObject.FindObjectOfType<MapController>();
			mapController.InitMap();

			Shmipl.Base.Messenger.AddListener("UnityShmipl.UpdateView", UpdateView);
		}

		void UpdateView() {
			GetComponent<UICollectionController>().UpdateView();
		}

		void DataLoad() {
			main.instance.LoadData((fileName.value == "" ? fileName.defaultText : fileName.value));
			main.instance.game.gameMode = GameMode.simple;
			UpdateView();
		}

		void SetCurPlayer(long p) {
			Cyclades.Game.Client.Messanges.cur_player = p; 
			for (int i = 0; i < buttons.Length; ++i) {
				buttons[i].defaultColor = (i == (int)p ? Color.green : Color.red);
				buttons[i].UpdateColor(true, true);
			}
			main.instance.game.gameMode = GameMode.simple;
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
}