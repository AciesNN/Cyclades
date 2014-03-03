using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class CardPanelController : UIController {

		public UIButton[] cards;

		private void OpenCardEnable_UpdateView () {

		}

		public override void UpdateView () {
			OpenCardEnable_UpdateView();
		}
		
		public void BuyCard(long card) {
			Hashtable msg = Client.BuyCard(card);
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		/*сигналы*/
		void BuyCard0() {
			BuyCard(0);
		}

		void BuyCard1() {
			BuyCard(1);
		}

		void BuyCard2() {
			BuyCard(2);
		}
	}
}
