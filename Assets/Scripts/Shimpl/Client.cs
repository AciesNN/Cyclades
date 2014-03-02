using System;
using System.Collections;
using System.Collections.Generic;

namespace Cyclades.Game
{
	class Client
	{
		public static long cur_player = -1L;
		public static string fsm = "Game";

		private static Hashtable MakeMsg(string action, Hashtable h) {
			Hashtable msg = new Hashtable() { 
				{"action", action},
				{"type", "action"},
				{"to", fsm},
				{"from", cur_player},
			};

			foreach(object key in h.Keys) {
				msg[key] = h[key];
			}

			return msg;
		}

		#region Сообщения
		public static Hashtable MakeBet(long bet, string god) {
			Hashtable h = new Hashtable() { 
				{"bet", bet},
				{"god", god}
			};
			return MakeMsg("MakeBet", h);
		}

		public static Hashtable BuyPhilosopher() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("BuyPhilosopher", h);
		}

		public static Hashtable PlaceMetro4Philosopher(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("PlaceMetro4Philosopher", h);
		}

		public static Hashtable BuyBuild() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("BuyBuild", h);
		}

		public static Hashtable PlaceBuilding(long island, long slot) {
			Hashtable h = new Hashtable() { 
				{"island", island},
				{"slot", slot}
			};
			return MakeMsg("PlaceBuilding", h);
		}

		public static Hashtable PlaceMetro4Buildings(long island, List<object> buildings) {
			Hashtable h = new Hashtable() { 
				{"island", island},
				{"buildings", buildings}
			};
			return MakeMsg("PlaceMetro4Buildings", h);
		}

		public static Hashtable BuyPriest() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("BuyPriest", h);
		}

		public static Hashtable ChangeCard(long open_slot_number) {
			Hashtable h = new Hashtable() { 
				{"open_slot_number", open_slot_number}
			};
			return MakeMsg("ChangeCard", h);
		}

		public static Hashtable BuyArmy() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("BuyArmy", h);
		}

		public static Hashtable PlaceArmy(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("PlaceArmy", h);
		}

		public static Hashtable BuyNavy() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("BuyNavy", h);
		}

		public static Hashtable PlaceNavy(long x, long y) {
			Hashtable h = new Hashtable() { 
				{"x", x},
				{"y", y}
			};
			return MakeMsg("PlaceNavy", h);
		}

		public static Hashtable PlaceHorn(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("PlaceHorn", h);
		}

		public static Hashtable Fight() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("Fight", h);
		}

		public static Hashtable PassFight() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("PassFight", h);
		}

		public static Hashtable MoveArmy(long from_island, long to_island, long units_count) {
			Hashtable h = new Hashtable() { 
				{"from_island", from_island},
				{"to_island", to_island},
				{"units_count", units_count}
			};
			return MakeMsg("MoveArmy", h);
		}

		public static Hashtable StartMoveNavy() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("StartMoveNavy", h);
		}

		public static Hashtable CancelMoveNavy() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("CancelMoveNavy", h);
		}

		public static Hashtable MoveNavy(long x_from, long y_from, long x_to, long y_to, long units_count) {
			Hashtable h = new Hashtable() { 
				{"x_from", x_from},
				{"y_from", y_from},
				{"x_to", x_to},
				{"y_to", y_to},
				{"units_count", units_count}
			};
			return MakeMsg("MoveNavy", h);
		}

		public static Hashtable BuyCard(long open_slot_number) {
			Hashtable h = new Hashtable() { 
				{"open_slot_number", open_slot_number}
			};
			return MakeMsg("BuyCard", h);
		}

		public static Hashtable PassCard() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("PassCard", h);
		}

		public static Hashtable EndPlayerTurn() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("EndPlayerTurn", h);
		}

		/* карты */
		public static Hashtable UseCardMer(long x, long y, bool change) {
			Hashtable h = new Hashtable() { 
				{"x", x},
				{"y", y},
				{"change", change}
			};
			return MakeMsg("UseCardMer", h);
		}

		public static Hashtable UseCardPeg(long from_island, long to_island, long units_count) {
			Hashtable h = new Hashtable() { 
				{"from_island", from_island},
				{"to_island", to_island},
				{"units_count", units_count}
			};
			return MakeMsg("UseCardPeg", h);
		}

		public static Hashtable UseCardGig(long island, long slot) {
			Hashtable h = new Hashtable() { 
				{"island", island},
				{"slot", slot}
			};
			return MakeMsg("UseCardGig", h);
		}

		public static Hashtable UseCardChm(string card, long index) {
			Hashtable h = new Hashtable() { 
				{"card", card},
				{"index", index}
			};
			return MakeMsg("UseCardChm", h);
		}

		public static Hashtable UseCardCyc(long island, long slot, string build) {
			Hashtable h = new Hashtable() { 
				{"island", island},
				{"slot", slot},
				{"build", build}
			};
			return MakeMsg("UseCardCyc", h);
		}

		public static Hashtable UseCardSphSellArmy(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("UseCardSphSellArmy", h);
		}

		public static Hashtable UseCardSphSellNavy(long x, long y) {
			Hashtable h = new Hashtable() { 
				{"x", x},
				{"y", y}
			};
			return MakeMsg("UseCardSphSellNavy", h);
		}

		public static Hashtable UseCardSphSellPriest() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("UseCardSphSellPriest", h);
		}

		public static Hashtable UseCardSphSellPhilosopher() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("UseCardSphSellPhilosopher", h);
		}

		public static Hashtable UseCardSyl() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("UseCardSyl", h);
		}

		public static Hashtable UseCardHar(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("UseCardHar", h);
		}

		public static Hashtable UseCardGri(long player_from) {
			Hashtable h = new Hashtable() { 
				{"player_from", player_from}
			};
			return MakeMsg("UseCardGri", h);
		}

		public static Hashtable UseCardMoy() {
			Hashtable h = new Hashtable() {
			};
			return MakeMsg("UseCardMoy", h);
		}

		public static Hashtable UseCardSat(long player_from) {
			Hashtable h = new Hashtable() { 
				{"player_from", player_from}
			};
			return MakeMsg("UseCardSat", h);
		}

		public static Hashtable UseCardDry(long player_from) {
			Hashtable h = new Hashtable() { 
				{"player_from", player_from}
			};
			return MakeMsg("UseCardDry", h);
		}

		public static Hashtable UseCardKra(long x, long y) {
			Hashtable h = new Hashtable() { 
				{"x", x},
				{"y", y}
			};
			return MakeMsg("UseCardKra", h);
		}

		public static Hashtable UseCardMin(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("UseCardMin", h);
		}

		public static Hashtable UseCardChr(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("UseCardChr", h);
		}

		public static Hashtable UseCardGor(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("UseCardGor", h);
		}

		public static Hashtable UseCardPol(long island) {
			Hashtable h = new Hashtable() { 
				{"island", island}
			};
			return MakeMsg("UseCardPol", h);
		}
		#endregion
	}
}
