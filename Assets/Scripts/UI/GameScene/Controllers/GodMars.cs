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
			isEnabled = (Client.cur_player == Library.GetCurrentPlayer(data.context));
		}
		
		public void EndTurn() {
			if (data.game.gameMode != GameMode.simple) {
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
			Hashtable msg = Client.EndPlayerTurn();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		void BuyArmy() {
			switch (data.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_BuyArmy);
				data.game.gameMode = GameMode.buyArmy;
				break;
			case(GameMode.buyArmy):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_BuyArmy);
				data.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}
		
		void MoveArmy() {
			switch (data.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_MoveArmy);
				data.game.gameMode = GameMode.buyArmy;
				break;
			case(GameMode.moveArmyFrom):
			case(GameMode.moveArmyTo):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_MoveArmy);
				data.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}
		
		void BuyBuild() {
			switch (data.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_Build);
				data.game.gameMode = GameMode.buyBuilding;
				break;
			case(GameMode.buyBuilding):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
				data.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}

		void OnMapClick_Build(Coords coords) {
			Hashtable 
				msg = Client.BuyBuild();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
			
			msg = Client.PlaceBuilding(Library.Map_GetIslandByPoint(data.context, coords.x, coords.y), 0);
			Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));
			
			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
			data.game.gameMode = GameMode.simple;
		}

		void OnMapClick_BuyArmy(Coords coords) {
			Hashtable 
				msg = Client.BuyArmy();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
			
			msg = Client.PlaceArmy(Library.Map_GetIslandByPoint(data.context, coords.x, coords.y));
			Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));
			
			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_BuyArmy);
			data.game.gameMode = GameMode.simple;
		}

		void OnMapClick_MoveArmy(Coords coords) {
			switch (data.game.gameMode) {
			case(GameMode.moveArmyFrom): 
				data.game.gameMode = GameMode.moveArmyTo;
				move_army_from_coords = coords;
				break;
			case(GameMode.moveArmyTo):
				Hashtable 
				
				msg = Client.MoveArmy(Library.Map_GetIslandByPoint(data.context, move_army_from_coords.x, move_army_from_coords.y), Library.Map_GetIslandByPoint(data.context, coords.x, coords.y), 1);
				Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));
				
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_MoveArmy);
				data.game.gameMode = GameMode.simple;
				break;
			}
		}
	}
}
