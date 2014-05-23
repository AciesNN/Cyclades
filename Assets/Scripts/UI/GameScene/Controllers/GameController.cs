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
		moveNavyStart,
		moveNavyFrom,
		moveNavyTo,
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
			string dt = Shmipl.Base.json.dumps(main.instance.context.data);
			if (dt == "{}") {
				int a = 1;
			}
			if (main.instance.context.GetStr("/cur_state") == "Turn.PlaceMetroPhilosopher" && main.instance.game.gameMode != GameMode.placeMetro4Philosopher) {

				main.instance.game.gameMode = GameMode.placeMetro4Philosopher;
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_PlaceMetro4Philosopher);

			}

			if (main.instance.context.GetStr("/cur_state") == "Turn.PlaceMetroBuilding"  && main.instance.game.gameMode != GameMode.placeMetroBuilding) {
				
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
			List<object> slots = GetListOfSlotsOfPlaceMetroBuilding(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y), Cyclades.Game.Client.Messanges.cur_player); //TODO пока как-либо определяются сносимые здания
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.PlaceMetro4Buildings(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y), slots) );
			
			main.instance.game.gameMode = GameMode.simple;
			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_PlaceMetroBuilding);
		}





		/*TODO временная ф-ция дает список зданий, которые можно снести под метрополию*/
		List<object> GetListOfSlotsOfPlaceMetroBuilding(long firstIsland, long owner) {
			List<object> res = new List<object>();
			Dictionary<string, bool> _get_builds = new Dictionary<string, bool>() { {Cyclades.Game.Constants.buildMarina, false}, {Cyclades.Game.Constants.buildUniver, false}, {Cyclades.Game.Constants.buildFortres, false}, {Cyclades.Game.Constants.buildTemple, false}}; //временная, для отметки уже полученных зданий

			List<object> buildings = main.instance.context.GetList("/map/islands/buildings");
			List<long> owners = main.instance.context.GetList<long>("/map/islands/owners");

			List<object> builds_on_island;
			long slot;

			//1. подчистим все с самого острова
			builds_on_island = buildings[(int)firstIsland] as List<object>;

			slot = 0;
			foreach(string b in builds_on_island) {
				if (b != Cyclades.Game.Constants.buildNone && _get_builds[b] != true) {
					res.Add ( new List<object>() {firstIsland, slot} );
					_get_builds[b] = true;
				}
				++slot;
			}

			//2. тоже самое для всех остальных
			for(long island = 0; island < owners.Count; ++island) {
				if (island == firstIsland)
					continue;
				if (owners[(int)island] != owner)
					continue;

				builds_on_island = buildings[(int)island] as List<object>;
				slot = 0;
				foreach(string b in builds_on_island) {
					if (b != Cyclades.Game.Constants.buildNone && _get_builds[b] != true) {
						res.Add ( new List<object>() {island, slot} );
						_get_builds[b] = true;
					}
					++slot;
				}			
			}

			return res;
		}
	}
}