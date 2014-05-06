using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public enum GameMode {
		simple,
		buyBuilding,
		buyArmy,
		buyNavy,
		moveArmyFrom,
		moveArmyTo,
		moveNavy,
		useCard,
		placeMetro4Philosopher,
		placeMetroBuilding,
		placeHorn
	}

	public class GameController  {

		GameMode gameMode_ = GameMode.simple;
		public GameMode gameMode 
		{ 
			get {
				return gameMode_;
			}

			set {
				gameMode_ = value;
				main.instance.SetGameMode(gameMode_);
			}
		}


		public void Update() {
			if (main.instance.context.GetStr("/cur_state") == "Turn.PlaceMetroPhilosopher" && main.instance.game.gameMode != GameMode.placeMetro4Philosopher) {

				main.instance.game.gameMode = GameMode.placeMetro4Philosopher;
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_PlaceMetro4Philosopher);

			}

			if (main.instance.context.GetStr("/cur_state") == "Turn.Turn.PlaceMetroBuilding"  && main.instance.game.gameMode != GameMode.placeMetroBuilding) {
				
				main.instance.game.gameMode = GameMode.placeMetroBuilding;
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_PlaceMetroBuilding);
				
			}
		}

		void OnMapClick_PlaceMetro4Philosopher(Coords coords) {
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.PlaceMetro4Philosopher(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y)) );

			main.instance.game.gameMode = GameMode.simple;
			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_PlaceMetro4Philosopher);
		}

		void OnMapClick_PlaceMetroBuilding(Coords coords) {
			List<object> slots = new List<object>(); //TODO пока никак не определяются сносимые здания
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.PlaceMetro4Buildings(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y), slots) );
			
			main.instance.game.gameMode = GameMode.simple;
			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_PlaceMetroBuilding);
		}
	}
}