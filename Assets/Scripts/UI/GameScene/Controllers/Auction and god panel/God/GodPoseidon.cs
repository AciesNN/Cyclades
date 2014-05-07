using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class GodPoseidon : UIController {
		
		Coords move_navy_from_coords; //для запоминания координат, с которого игрок хочет двинуть флот
		public long move_navy_count;

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

		void BuyNavy() {
			switch (main.instance.game.gameMode) {
			case(GameMode.simple): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_BuyNavy);
				main.instance.game.gameMode = GameMode.buyNavy;
				break;
			case(GameMode.buyNavy):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_BuyNavy);
				main.instance.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}

		void StartMoveNavy() {
			switch (main.instance.game.gameMode) {
			case(GameMode.simple): 
				main.instance.SendSrv( Cyclades.Game.Client.Messanges.StartMoveNavy() );
				main.instance.game.gameMode = GameMode.moveNavyStart;
				break;
			case(GameMode.moveNavyStart): 
			case(GameMode.moveNavyFrom): 
			case(GameMode.moveNavyTo):
				main.instance.SendSrv( Cyclades.Game.Client.Messanges.CancelMoveNavy() );
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
				Shmipl.Base.Messenger<Coords, long>.AddListener("Shmipl.Map.ClickOnBuildSlot", OnMapClick_Build);
				main.instance.game.gameMode = GameMode.buyBuilding;
				break;
			case(GameMode.buyBuilding):
				Shmipl.Base.Messenger<Coords, long>.RemoveListener("Shmipl.Map.ClickOnBuildSlot", OnMapClick_Build);
				main.instance.game.gameMode = GameMode.simple;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}

		void MoveNavy() {
			switch (main.instance.game.gameMode) {
			case(GameMode.moveNavyStart): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_MoveNavy);
				main.instance.game.gameMode = GameMode.moveNavyFrom;
				break;
			case(GameMode.moveNavyFrom):
			case(GameMode.moveArmyTo):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_MoveNavy);
				main.instance.game.gameMode = GameMode.moveNavyStart;
				break;
			default:
				Debug.Log ("NOT ENABLED"); //по идее, надо ограничивать доступность
				return;
			}
		}

		void OnMapClick_BuyNavy(Coords coords) {
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.BuyNavy() );
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.PlaceNavy(coords.x, coords.y) );
			
			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_BuyNavy);
			main.instance.game.gameMode = GameMode.simple;
		}

		void OnMapClick_Build(Coords coords, long slot) {
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.BuyBuild() );	
			main.instance.SendSrv( Cyclades.Game.Client.Messanges.PlaceBuilding(Library.Map_GetIslandByPoint(main.instance.context, coords.x, coords.y), slot) );

			Shmipl.Base.Messenger<Coords, long>.RemoveListener("Shmipl.Map.ClickOnBuildSlot", OnMapClick_Build);
			main.instance.game.gameMode = GameMode.simple;
		}

		void OnMapClick_MoveNavy(Coords coords) {
			switch (main.instance.game.gameMode) {
			case(GameMode.moveArmyFrom): 
				NavyCountDialog.ShowDilog(this);
				
				main.instance.game.gameMode = GameMode.moveNavyTo;
				move_navy_from_coords = coords;
				break;
			case(GameMode.moveArmyTo):
				main.instance.SendSrv( Cyclades.Game.Client.Messanges.MoveNavy(move_navy_from_coords.x, move_navy_from_coords.y, coords.x, coords.y, move_navy_count) );
				
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_MoveNavy);
				main.instance.game.gameMode = GameMode.moveNavyStart;
				break;
			}
		}
	}
}
