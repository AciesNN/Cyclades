using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodZeus : UIController {

		public CardPanelController cardPanelController; //ссылка нужна для того, чтобы предупреждать о смене режима выбора карт

		public override void UpdateView () {
			
		}
		
		void BuyPriest() {
			Hashtable msg = Client.BuyPriest();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		void ChangeCard() {
			cardPanelController.ChangeCard();
		}

		void BuyBuild() {
			//TODO
		}
	}
}
