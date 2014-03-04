using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodPoseidon : UIController {
		
		public override void UpdateView () {
			isEnabled = (Client.cur_player == Library.GetCurrentPlayer(data.context));			
		}

		void BuyNavy() {
			//TODO
		}

		void StartMoveNavy() {
			//TODO
		}

		void BuyBuild() {
			//TODO
		}
	}
}
