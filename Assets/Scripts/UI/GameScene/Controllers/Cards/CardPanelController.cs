﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	enum CardPanelController_CardChoose_Mode {
		Buy,
		Change
	}

	public class CardPanelController : UIController {
		CardPanelController_CardChoose_Mode card_choose_mode = CardPanelController_CardChoose_Mode.Buy;

		public UIButton[] cards;
		public UILabel[] cards_labels;

		void OpenCard_UpdateView(int index) {
			List<string> open_cards = main.instance.context.GetList<string>("/cards/open");

			if (open_cards.Count > index) {
				cards_labels[index].text = open_cards[index];
			}

			if (open_cards.Count <= index || open_cards[index] == Constants.cardNone)
				cards[index].isEnabled = false;
			else
				cards[index].isEnabled = true;
		}

		void OpenCards_UpdateView () {
			for (int i = 0; i < cards.Length; ++i) {
				OpenCard_UpdateView(i);
			}
		}

		public override void UpdateView () {
			if (!main.instance.isContextReady(main.instance.context)) 
				return;

			OpenCards_UpdateView();
			if (Cyclades.Game.Client.Messanges.cur_player != Library.GetCurrentPlayer(main.instance.context))
				isEnabled = false;
			if (Library.GetPhase(main.instance.context) != Phase.TurnPhase)
				isEnabled = false;
		}
		
		public void BuyCard(long card) {
			Hashtable msg;
			if (card_choose_mode == CardPanelController_CardChoose_Mode.Change) {
				msg = Cyclades.Game.Client.Messanges.ChangeCard(card);
				card_choose_mode = CardPanelController_CardChoose_Mode.Buy;
			} else {
				msg = Cyclades.Game.Client.Messanges.BuyCard(card);
			}
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

		public void ChangeCard() {
			card_choose_mode = (card_choose_mode == CardPanelController_CardChoose_Mode.Change ? CardPanelController_CardChoose_Mode.Buy : CardPanelController_CardChoose_Mode.Change);
		}
	}
}
