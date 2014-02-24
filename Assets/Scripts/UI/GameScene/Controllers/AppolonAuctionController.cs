using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AppolonAuctionController : UIController {
		public UILabel godLabel;
		public UILabel[] bets;

		public override void UpdateView () {
			long appolo_god_number = data.context.Get<long> ("/players_number") - 1;

			List<int> appoloBets = Library.Auction_GetAllOrderBetPlayersForGod(data.context, appolo_god_number);
			for (int i = 0; i < bets.Length; ++i) {
				bets[i].gameObject.SetActive( i < appoloBets.Count );
				if (i < appoloBets.Count) {
					bets[i].text = "" + appoloBets[i];
				}
			}
		}

		/*события*/
		public void ConfirmActiveGodBet() {
			long appolo_god_number = data.context.Get<long> ("/players_number") - 1;
			int bet = Library.Auction_GetAllOrderBetPlayersForGod(data.context, appolo_god_number).Count + 1;
			((AuctionGods)parentController).ConfirmAppoloBet(bet);
		}
	}
}