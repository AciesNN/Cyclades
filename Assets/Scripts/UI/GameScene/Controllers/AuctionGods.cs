using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AuctionGods : UICollectionController {
		public UIController AppolonWidget;
		private int activeGodIndex = -1;
		private int activeBet = 0;

		public override void UpdateView ()
		{
			long random_gods_number = data.context.Get<long> ("/players_number") - 1;
			
			ForEachChild<GodPanel> ((god, ch) => {
				ch.gameObject.SetActive (god < random_gods_number); 

				if (god < random_gods_number) {

					ch.God = data.context.Get<string>("/auction/gods_order/[{0}]", god);
					ch.Player = Library.Aiction_GetCurrentBetPlayerForGod(data.context, god);
					if (god == activeGodIndex) {
						ch.Bet = activeBet;
					} else {
						if(ch.Player >= 0)
							ch.Bet = data.context.GetLong("/auction/bets/[{0}]/[{1}]", god, ch.Player);
						else
							ch.Bet = 0;
					}
				
					ch.DisableBet = (god == Library.Auction_GetCurrentGodBetForPlayer(data.context, data.cur_player));
					if (!ch.DisableBet) {
						if (ch.Player >= 0)
							ch.MinBet = data.context.GetLong("/auction/bets/[{0}]/[{1}]", god, ch.Player);
						else 
							ch.MinBet = 0;

						if (data.context.GetLong("/markers/gold/[{0}]", data.cur_player) > 0)
							ch.MaxBet = data.context.GetLong("/markers/gold/[{0}]", data.cur_player) + data.context.GetLong("/markers/priest/[{0}]", data.cur_player);
						else
							ch.MaxBet = 0;
					}
				}
			});	

			AppolonWidget.UpdateView();
		}
		
		private void ConfirmBet(string god, long bet) {
			Debug.Log("Player made bet " + bet + " on god " + god);
			ResetActiveGod();
			UpdateView();
		}

		public void ConfirmAppoloBet(int bet) {
			ConfirmBet(Constants.godAppolon, (long)bet);
		}

		public void ConfirmActiveGodBet(int index) {
			ConfirmBet(GetChild<GodPanel>(index).God, GetChild<GodPanel>(index).Bet);
		}

		public void ChangeBet(int index, int change) {
			if (activeGodIndex != index) {
				activeBet = (int)GetChild<GodPanel>(index).Bet;
				activeGodIndex = index;
			}
			activeBet += change;
			UpdateView();
		}

		public void ResetActiveGod() {
			activeGodIndex = -1;
			activeBet = 0;
		}
	}
}