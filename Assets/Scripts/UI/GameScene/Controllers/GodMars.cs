using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodMars : UIController {
		
		public override void UpdateView () {
			isEnable = (Client.cur_player == Library.GetCurrentPlayer(data.context));
		}
		
		void BuyArmy() {
			//TODO
		}
		
		void MoveArmy() {
			//TODO
		}
		
		void BuyBuild() {
			//TODO
		}
	}
}
