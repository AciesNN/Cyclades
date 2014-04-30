using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Shmipl.Unity;
using Cyclades.Game;

namespace Shmipl.GameScene
{
	public class AuctionPlayerInformationPanelController : UICollectionController
	{		
		public override void UpdateView ()
		{
			long players_number = main.instance.context.GetLong ("/players_number");
			List<long> player_order = GetPlayerInformationOrder();

			ForEachChild<AuctionPlayerInformationController> ((i, ch) => {
				ch.gameObject.SetActive (i < players_number);

				if (i < players_number && player_order.Count > 0) {
					long player = player_order[i];
					if (Cyclades.Game.Client.Messanges.cur_player == player) {
						ch.Income = "" + main.instance.context.GetLong ("/markers/gold/[{0}]", player) + "/" + main.instance.context.GetLong ("/markers/income/[{0}]", player);
					} else {
						ch.Income = "?/" + main.instance.context.GetLong ("/markers/income/[{0}]", player);
					}
					ch.Priests = main.instance.context.GetLong ("/markers/priest/[{0}]", player);
					ch.Philosophers = main.instance.context.GetLong ("/markers/philosopher/[{0}]", player);
					ch.Color_ = main.instance.GetColor(i);
					ch.Player = player;

					ch.ActivePlayer = (Library.GetCurrentPlayer(main.instance.context) == player);
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
				return new List<long>();
			}
		}
	}
}