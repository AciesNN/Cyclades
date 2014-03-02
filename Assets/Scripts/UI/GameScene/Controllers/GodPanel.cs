using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodPanel : UIController {
		public UIButton okButton;
		public UIButton increaseButton;
		public UIButton discreaseButton;

		public override void UpdateView() {
			DisableBet_UpdateView();
			God_UpdateView();
			Player_UpdateView();
			Bet_UpdateView();
		}
		
		/* disable bet */
		private bool disableBet = false;
		public bool DisableBet {
			get { return disableBet; }
			set {
				if (disableBet != value) {
					disableBet = value;
					DisableBet_UpdateView();
				}
			}
		}
		
		public void DisableBet_UpdateView() {
			okButton.isEnabled = !disableBet;
		}

		/* max bet */
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
			increaseButton.isEnabled = (Bet < MaxBet) && !disableBet;
		}
		
		/* min bet */
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
			discreaseButton.isEnabled = (Bet > MinBet) && !disableBet;
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