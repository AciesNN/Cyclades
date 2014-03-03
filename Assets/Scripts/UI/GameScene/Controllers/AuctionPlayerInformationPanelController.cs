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
			long players_number = data.context.GetLong ("/players_number");
			List<long> player_order = GetPlayerInformationOrder();

			ForEachChild<AuctionPlayerInformationController> ((i, ch) => {
				ch.gameObject.SetActive (i < players_number);

				if (i < players_number && player_order.Count > 0) {
					long player = player_order[i];
					ch.Income = data.context.GetLong ("/markers/income/[{0}]", player);
					ch.Priests = data.context.GetLong ("/markers/priest/[{0}]", player);
					ch.Philosophers = data.context.GetLong ("/markers/philosopher/[{0}]", player);

					ch.Player = player;

					ch.ActivePlayer = (Library.GetCurrentPlayer(data.context) == player);
				}
			});
		}

		private List<long> GetPlayerInformationOrder() {
			Cyclades.Game.Phase phase = Library.GetPhase(data.context);

			if (phase == Phase.AuctionPhase) {
				return data.context.GetList<long>("/auction/start_order");
			} else if (phase == Phase.TurnPhase) {
				List<long> res = new List<long>();
				res.Add(data.context.Get<long>("/turn/current_player"));
				res.AddRange(data.context.GetList<long>("/turn/player_order"));
				res.AddRange(data.context.GetList<long>("/auction/player_order"));
				return res;
			} else {
				return new List<long>();
			}
		}
	}
}