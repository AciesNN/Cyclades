using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class CardPanelController : UIController {

		public UIButton[] cards;
		public UILabel[] cards_labels;

		void OpenCard_UpdateView(int index) {
			List<string> open_cards = data.context.GetList<string>("/cards/open");

			if (open_cards.Count > index) {
				cards_labels[index].text = open_cards[index];
			}

			if (Client.cur_player != Library.GetCurrentPlayer(data.context))
				cards[index].isEnabled = false;
			else if (open_cards.Count <= index)
				cards[index].isEnabled = false;	
			else if (open_cards[index] == Constants.cardNone)
				cards[index].isEnabled = false;

			cards[index].isEnabled = true;
		}

		void OpenCards_UpdateView () {
			for (int i = 0; i < cards.Length; ++i) {
				OpenCard_UpdateView(i);
			}
		}

		public override void UpdateView () {
			OpenCards_UpdateView();
		}
		
		public void BuyCard(long card) {
			Hashtable msg = Client.BuyCard(card);
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		/*сигналы*/
		void BuyCard0() {
			BuyCard(0);
		}

		void BuyCard1() {
			BuyCard(1);
		}

		void BuyCard2() {
			BuyCard(2);
		}
	}
}
