using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class GodMars : UIController {

		Coords move_army_from_coords; //для запоминания острова, с которого игрок хочет двинуть войска

		public override void UpdateView () {
			isEnabled = (Cyclades.Game.Client.Messanges.cur_player == Library.GetCurrentPlayer(main.instance.context));
		}
		
		public void EndTurn() {
			if (main.instance.game.gameMode != GameMode.simple) {
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.EndPlayerTurn() );
		}

		void BuyArmy() {
			switch (main.instance.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_BuyArmy);
				main.instance.game.gameMode = GameMode.buyArmy;
				break;
			case(GameMode.buyArmy):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_BuyArmy);
				main.instance.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}
		
		void MoveArmy() {
			switch (main.instance.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_MoveArmy);
				main.instance.game.gameMode = GameMode.buyArmy;
				break;
			case(GameMode.moveArmyFrom):
			case(GameMode.moveArmyTo):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_MoveArmy);
				main.instance.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}
		
		void BuyBuild() {
			switch (main.instance.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_Build);
				main.instance.game.gameMode = GameMode.buyBuilding;
				break;
			case(GameMode.buyBuilding):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
				main.instance.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}

		void OnMapClick_Build(Coords coords) {
			Hashtable 
				msg = Cyclades.Game.Client.Messanges.BuyBuild();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
			
			msg = Cyclades.Game.Client.Messanges.PlaceBuilding(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y), 0);
			Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));
			
			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
			main.instance.game.gameMode = GameMode.simple;
		}

		void OnMapClick_BuyArmy(Coords coords) {
			Hashtable 
				msg = Cyclades.Game.Client.Messanges.BuyArmy();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
			
			msg = Cyclades.Game.Client.Messanges.PlaceArmy(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y));
			Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));
			
			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_BuyArmy);
			main.instance.game.gameMode = GameMode.simple;
		}

		void OnMapClick_MoveArmy(Coords coords) {
			switch (main.instance.game.gameMode) {
			case(GameMode.moveArmyFrom): 
				main.instance.game.gameMode = GameMode.moveArmyTo;
				move_army_from_coords = coords;
				break;
			case(GameMode.moveArmyTo):
				Hashtable 
				
					msg = Cyclades.Game.Client.Messanges.MoveArmy(Library.Map_GetIslandByPoint(main.instance.context, move_army_from_coords.x, move_army_from_coords.y), Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y), 1);
				Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));
				
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_MoveArmy);
				main.instance.game.gameMode = GameMode.simple;
				break;
			}
		}
	}
}
