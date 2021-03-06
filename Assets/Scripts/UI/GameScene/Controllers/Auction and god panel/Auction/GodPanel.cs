﻿using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodPanel : UIController {
		public UIButton okButton;
		public UIButton increaseButton;
		public UIButton discreaseButton;

		public override void UpdateView() {
			God_UpdateView();
			Player_UpdateView();
			Bet_UpdateView();
		}

		/* enable bet */
		private bool enableBet = true;
		public bool EnableBet {
			get { return enableBet; }
			set {
				if (enableBet != value) {
					enableBet = value;
					Bet_UpdateView();
				}
			}
		}
		
		public void EnableBet_UpdateView() {
			okButton.isEnabled = enableBet && (Bet < MaxBet) && (Bet > MinBet);
		}

		/* max bet */
		private long maxBet;
		public long MaxBet {
			get { return maxBet; }
			set {
				maxBet = value;
				MaxBet_UpdateView();
			}
		}
		
		public void MaxBet_UpdateView() {
			increaseButton.isEnabled = (Bet < MaxBet) && enableBet;
		}
		
		/* min bet */
		private long minBet;
		public long MinBet {
			get { return minBet; }
			set {
				minBet = value;
				MinBet_UpdateView();
			}
		}
		
		public void MinBet_UpdateView() {
			discreaseButton.isEnabled = (Bet > MinBet) && enableBet;
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
			playerView.text = (player == -1 ? "" : "" + player);
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
			EnableBet_UpdateView();
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