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
		
		/* max bet */
		public UIButton increaseButton;
		private long maxBet;
		public long MaxBet {
			get { return maxBet; }
			set {
				if (maxBet != value) {
					maxBet = value;
					MaxBet_UpdateView();
				}
			}
		}
		
		public void MaxBet_UpdateView() {
			increaseButton.isEnabled = (Bet < MaxBet);
		}
		
		/* min bet */
		public UIButton discreaseButton;
		private long minBet;
		public long MinBet {
			get { return minBet; }
			set {
				if (minBet != value) {
					minBet = value;
					MinBet_UpdateView();
				}
			}
		}
		
		public void MinBet_UpdateView() {
			discreaseButton.isEnabled = (Bet > MinBet);
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
			MinBet_UpdateView();
			MaxBet_UpdateView();
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