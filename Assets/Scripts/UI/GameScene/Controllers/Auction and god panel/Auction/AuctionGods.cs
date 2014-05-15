using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AuctionGods : UICollectionController {
		public AppolonAuctionController AppolonWidget;
		private int activeGodIndex = -1;
		private int activeBet = 0;

		public override void UpdateView ()
		{
			long random_gods_number = main.instance.context.Get<long> ("/players_number") - 1;
			
			ForEachChild<GodPanel> ((god, ch) => {
				ch.gameObject.SetActive (god < random_gods_number); 

				if (god < random_gods_number) {

					ch.EnableBet = Cyclades.Game.Client.Messanges.cur_player != -1 && (god != Library.Auction_GetCurrentGodBetForPlayer(main.instance.context, Cyclades.Game.Client.Messanges.cur_player)) && (Cyclades.Game.Client.Messanges.cur_player == Library.GetCurrentPlayer(main.instance.context));

					ch.God = main.instance.context.Get<string>("/auction/gods_order/[{0}]", god);
					ch.Player = Library.Aiction_GetCurrentBetPlayerForGod(main.instance.context, god);
				
					if (ch.EnableBet) {
						if (ch.Player >= 0)
							ch.MinBet = main.instance.context.GetLong("/auction/bets/[{0}]/[{1}]", god, ch.Player);
						else 
							ch.MinBet = 0;

						if (main.instance.context.GetLong("/markers/gold/[{0}]", Cyclades.Game.Client.Messanges.cur_player) > 0)
							ch.MaxBet = main.instance.context.GetLong("/markers/gold/[{0}]", Cyclades.Game.Client.Messanges.cur_player) + main.instance.context.GetLong("/markers/priest/[{0}]", Cyclades.Game.Client.Messanges.cur_player);
						else
							ch.MaxBet = 0;
					}

					if (god == activeGodIndex) {
						ch.Bet = activeBet;
					} else {
						if(ch.Player >= 0)
							ch.Bet = main.instance.context.GetLong("/auction/bets/[{0}]/[{1}]", god, ch.Player);
						else
							ch.Bet = 0;
					}

					ch.EnableBet_UpdateView();
				}
			});	

			AppolonWidget.EnableBet = (Cyclades.Game.Client.Messanges.cur_player == Library.GetCurrentPlayer(main.instance.context));
			AppolonWidget.UpdateView();
		}
		
		private void ConfirmBet(string god, long bet) {
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.MakeBet(bet, god) );

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