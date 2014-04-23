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
			isEnabled = (Cyclades.Game.Client.Messanges.cur_player == Library.GetCurrentPlayer(main.instance.context));
		}
		
		public void EndTurn() {
			if (main.instance.game.gameMode != GameMode.simple) {
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
			Hashtable msg = Cyclades.Game.Client.Messanges.EndPlayerTurn();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		void BuyPriest() {
			if (main.instance.game.gameMode != GameMode.simple) {
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
			Hashtable msg = Cyclades.Game.Client.Messanges.BuyPriest();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		void ChangeCard() {
			cardPanelController.ChangeCard();
		}

		void BuyBuild() {
			switch (main.instance.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_Build);
				main.instance.game.gameMode = GameMode.buyBuilding;
				break;
			case(GameMode.buyBuilding):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
				main.instance.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}

		void OnMapClick_Build(Coords coords) {
			Hashtable 
				msg = Cyclades.Game.Client.Messanges.BuyBuild();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));

			msg = Cyclades.Game.Client.Messanges.PlaceBuilding(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y), 0);
			Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));

			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
			main.instance.game.gameMode = GameMode.simple;
		}
	}
}
