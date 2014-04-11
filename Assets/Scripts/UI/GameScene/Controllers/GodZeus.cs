using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	enum BuyBuildState {
		buy, notBuy
	}

	public class GodZeus : UIController {

		public CardPanelController cardPanelController; //ссылка нужна для того, чтобы предупреждать о смене режима выбора карт

		BuyBuildState buyBuildState = BuyBuildState.notBuy;

		public override void UpdateView () {
			isEnabled = (Client.cur_player == Library.GetCurrentPlayer(data.context));
		}
		
		void BuyPriest() {
			Hashtable msg = Client.BuyPriest();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		void ChangeCard() {
			cardPanelController.ChangeCard();
		}

		void BuyBuild() {
			switch (buyBuildState) {
			case(BuyBuildState.notBuy): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick);
				buyBuildState = BuyBuildState.buy;
				break;
			case(BuyBuildState.buy):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick);
				buyBuildState = BuyBuildState.notBuy;				
				break;
			}
		}

		void OnMapClick(Coords coords) {
			Hashtable 
			msg = Client.BuyBuild();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));

			msg = Client.PlaceBuilding(Library.Map_GetIslandByPoint(data.context, coords.x, coords.y), 0);
			Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));

			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick);
			buyBuildState = BuyBuildState.notBuy;
		}
	}
}
