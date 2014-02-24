using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodPanel : UIController {

		public override void UpdateView() {
			God_UpdateView();
			Player_UpdateView();
			Bet_UpdateView();
		}
		
		/* God */
		public UILabel godView;
		private string god;
		public string God {
			get { return god; }
			set {
				if (god != value) {
					god = value;
					God_UpdateView();
				}
			}
		}
		
		public void God_UpdateView() {
			godView.text = god;
		}
		
		/* Player */
		public UILabel playerView;
		private long player;
		public long Player {
			get { return player; }
			set { 
				if (player != value) {
					player = value;
					Player_UpdateView();
				}
			}
		}
		
		public void Player_UpdateView() {
			playerView.text = "" + player;
		}
		
		/* Bet */
		public UILabel betView;
		private long bet;
		public long Bet {
			get { return bet; }
			set { 
				if (bet != value) {
					bet = value;
					Bet_UpdateView();
				}
			}
		}
		
		public void Bet_UpdateView() {
			betView.text = "" + bet;
		}

		/*события*/
		void ActiveGodBetIncrease()	{	
			((AuctionGods)parentController).ChangeBet(index, +1);
		}

		void ActiveGodBetDecrease()	{
			((AuctionGods)parentController).ChangeBet(index, -1);
		}

		void ConfirmBet() {
			((AuctionGods)parentController).ConfirmActiveGodBet(index);
		}
	}
}