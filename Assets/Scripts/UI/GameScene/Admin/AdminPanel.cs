using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AdminPanel : MonoBehaviour {
		public UIInput fileName;
		public UIButton[] buttons;
		MapController mapController;
		public UILabel gameModeLabel;
		public UILabel curStateLabel;

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

			Shmipl.Base.Messenger.AddListener("UnityShmipl.UpdateView", UpdateViewOutside);
			Shmipl.Base.Messenger<GameMode>.AddListener("UnityShmipl.ChangeGameMode", ChangeGameMode);
		}

		void UpdateViewOutside() {
			SetCurPlayer(Cyclades.Game.Library.GetCurrentPlayer(main.instance.context), false);
			UpdateView();
		}

		void ChangeGameMode (GameMode gameMode) {
			gameModeLabel.text = gameMode.ToString();
		}

		void UpdateView() {

			main.instance.game.Update();
			GetComponent<UICollectionController>().UpdateView();

			try {
				curStateLabel.text = main.instance.context.GetStr("/cur_state");
			} catch {
				curStateLabel.text = "<error>";
			}
		}

		void DataLoad() {
			main.instance.LoadData((fileName.value == "" ? fileName.defaultText : fileName.value));
			main.instance.game.gameMode = GameMode.simple;
			main.instance.game.Update();
			UpdateView();
		}

		void SetCurPlayer(long p, bool update) {
			if (p == -1 && !update)
				return;
			
			Cyclades.Game.Client.Messanges.cur_player = p; 
			for (int i = 0; i < buttons.Length; ++i) {
				buttons[i].defaultColor = (i == (int)p ? Color.green : Color.red);
				buttons[i].UpdateColor(true, true);
			}

			if (update) {
				main.instance.game.gameMode = GameMode.simple;
				UpdateView();
				main.instance.game.Update();
			}
		}

		public void SetCurPlayer0() {
			SetCurPlayer(0, true);
		}

		public void SetCurPlayer1() {
			SetCurPlayer(1, true);
		}

		public void SetCurPlayer2() {
			SetCurPlayer(2, true);
		}

		public void SetCurPlayer3() {
			SetCurPlayer(3, true);
		}

		public void SetCurPlayer4() {
			SetCurPlayer(4, true);
		}
	}
}