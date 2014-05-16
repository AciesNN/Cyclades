using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Shmipl.Unity;
using Cyclades.Game;

namespace Shmipl.GameScene
{
	public class AuctionPlayerInformationPanelController : UICollectionController
	{		
		static readonly Color colorNoneGod = Color.black;

		public override void UpdateView ()
		{
			if (!main.instance.isContextReady(main.instance.context)) 
				return;

			long players_number = main.instance.context.GetLong ("/players_number");
			List<long> player_order = GetPlayerInformationOrder();
			List<Color> player_gods_order = GetPlayerGodsInformationOrder(players_number);

			if (player_order == null || player_gods_order == null)
				return;

			ForEachChild<AuctionPlayerInformationController> ((i, ch) => {
				ch.gameObject.SetActive (i < players_number);

				if (i < players_number && player_order.Count > 0) {
					long player = player_order[i];
					if (player == -1)
						return;

					if (Cyclades.Game.Client.Messanges.cur_player == player) {
						ch.Income = "" + main.instance.context.GetLong ("/markers/gold/[{0}]", player) + "/" + main.instance.context.GetLong ("/markers/income/[{0}]", player);
					} else {
						ch.Income = "?/" + main.instance.context.GetLong ("/markers/income/[{0}]", player);
					}
					ch.Priests = main.instance.context.GetLong ("/markers/priest/[{0}]", player);
					ch.Philosophers = main.instance.context.GetLong ("/markers/philosopher/[{0}]", player);
					ch.Color_ = main.instance.GetColor(player);
					ch.Player = player;

					ch.ActivePlayer = (Library.GetCurrentPlayer(main.instance.context) == player);

					try {
						ch.GodColor = player_gods_order[i];
					} catch (System.Exception ex) {
						int a = 1;
					}
					ch.GodVisible = (player_gods_order[i] != colorNoneGod);
				}
			});
		}

		private List<long> GetPlayerInformationOrder() {
			Cyclades.Game.Phase phase = Library.GetPhase(main.instance.context);

			if (phase == Phase.AuctionPhase) {
				return main.instance.context.GetList<long>("/auction/start_order");
			} else if (phase == Phase.TurnPhase) {
				List<long> res = new List<long>();
				res.Add(main.instance.context.Get<long>("/turn/current_player"));
				res.AddRange(main.instance.context.GetList<long>("/turn/player_order"));
				res.AddRange(main.instance.context.GetList<long>("/auction/player_order"));
				return res;
			} else {
				return null;
			}
		}

		private List<Color> GetPlayerGodsInformationOrder(long players_number) {
			Cyclades.Game.Phase phase = Library.GetPhase(main.instance.context);
			
			if (phase == Phase.AuctionPhase) {
				List<Color> res = new List<Color>();
				for (int i = 0; i < players_number; ++i) {//набиваем результат черным цветом - знаком того, что это аукционный игрок
					res.Add(colorNoneGod);
				}
				return res;
			} else if (phase == Phase.TurnPhase) {
				if (main.instance.context.Get<long>("/turn/current_player") == -1)
					return null;
				List<Color> res = new List<Color>();
				res.Add(GetGodColorForPlayer(main.instance.context.Get<long>("/turn/current_player")));
				List<long> players_order = main.instance.context.GetList<long>("/turn/player_order");
				foreach(long pl in players_order)
					res.Add(GetGodColorForPlayer(pl));
				for (int i = res.Count; i < players_number; ++i) {//остаток набиваем результат черным цветом - знаком того, что это аукционный игрок
					res.Add(colorNoneGod);
				}
				return res;
			} else {
				return null;
			}
		}

		Color GetGodColorForPlayer(long player) {
			return main.instance.GetGodColor(
				Cyclades.Game.Library.Auction_GetGodByNumber(
					main.instance.context,
					Cyclades.Game.Library.Auction_GetCurrentGodBetForPlayer(main.instance.context, player))
				);
		}
	}
}