﻿using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AuctionGods : UICollectionController {
		//public UIController AppolonWidget;
		private int activeGodIndex = -1;
		private int activeBet = 0;

		public override void UpdateView ()
		{
			long random_gods_number = data.context.Get<long> ("/players_number") - 1;
			
			ForEachChild<GodPanel> ((i, ch) => {
				ch.gameObject.SetActive (i < random_gods_number); 

				if (i < random_gods_number) {
					ch.God = data.context.Get<string>("/auction/gods_order/[{0}]", i);
					ch.Player = Library.Aiction_GetCurrentBetPlayerForGod(data.context, i);
					if (i == activeGodIndex) {
						ch.Bet = activeBet;
					} else {
						if(ch.Player >= 0 )
							ch.Bet = data.context.GetLong("/auction/bets/[{0}]/[{1}]", i, ch.Player);
						else
							ch.Bet = 0;
					}
				}
			});	
		}

		public void ConfirmActiveGodBet(int index) {
			Debug.Log("Player made bet " + GetChild<GodPanel>(index).Bet + " on god " + GetChild<GodPanel>(index).God);
		}

		public void ChangeBet(int index, int change) {
			if (activeGodIndex != index) {
				activeBet = (int)GetChild<GodPanel>(index).Bet;
				activeGodIndex = index;
			}
			activeBet += change;
			UpdateView();
		}
	}
}