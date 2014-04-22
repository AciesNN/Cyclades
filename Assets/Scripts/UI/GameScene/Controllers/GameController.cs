using UnityEngine;
using System.Collections;

using Shmipl;

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
		useCard
	}

	public class GameController  {

		public GameMode gameMode { get; set;}
		//Shmipl.Base.FSM fsm;

		public GameController() {
			gameMode = GameMode.simple;
			//fsm = new Shmipl.Base.FSM();

			//fsm.AddStringToTable(
		}
		 
	}
}