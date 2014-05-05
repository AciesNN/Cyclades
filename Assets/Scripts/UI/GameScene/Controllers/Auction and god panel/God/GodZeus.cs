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
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.EndPlayerTurn() );
		}

		void BuyPriest() {
			if (main.instance.game.gameMode != GameMode.simple) {
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.BuyPriest() );
		}

		void ChangeCard() {
			cardPanelController.ChangeCard();
		}

		void BuyBuild() {
			switch (main.instance.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords, long>.AddListener("Shmipl.Map.ClickOnBuildSlot", OnMapClick_Build);
				main.instance.game.gameMode = GameMode.buyBuilding;
				break;
			case(GameMode.buyBuilding):
				Shmipl.Base.Messenger<Coords, long>.RemoveListener("Shmipl.Map.ClickOnBuildSlot", OnMapClick_Build);
				main.instance.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}

		void OnMapClick_Build(Coords coords, long slot) {
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.BuyBuild() );

			main.instance.SendSrv( Cyclades.Game.Client.Messanges.PlaceBuilding(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y), slot) );

			Shmipl.Base.Messenger<Coords, long>.RemoveListener("Shmipl.Map.ClickOnBuildSlot", OnMapClick_Build);
			main.instance.game.gameMode = GameMode.simple;
		}
	}
}
