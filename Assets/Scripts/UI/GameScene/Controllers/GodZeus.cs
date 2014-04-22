using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class GodZeus : UIController {

		public CardPanelController cardPanelController; //ссылка нужна для того, чтобы предупреждать о смене режима выбора карт

		public override void UpdateView () {
			isEnabled = (Client.cur_player == Library.GetCurrentPlayer(data.context));
		}
		
		public void EndTurn() {
			if (data.game.gameMode != GameMode.simple) {
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
			Hashtable msg = Client.EndPlayerTurn();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		void BuyPriest() {
			if (data.game.gameMode != GameMode.simple) {
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
			Hashtable msg = Client.BuyPriest();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		void ChangeCard() {
			cardPanelController.ChangeCard();
		}

		void BuyBuild() {
			switch (data.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_Build);
				data.game.gameMode = GameMode.buyBuilding;
				break;
			case(GameMode.buyBuilding):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
				data.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}

		void OnMapClick_Build(Coords coords) {
			Hashtable 
			msg = Client.BuyBuild();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));

			msg = Client.PlaceBuilding(Library.Map_GetIslandByPoint(data.context, coords.x, coords.y), 0);
			Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));

			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
			data.game.gameMode = GameMode.simple;
		}
	}
}
