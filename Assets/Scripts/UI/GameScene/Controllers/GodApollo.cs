using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodApollo : UIController {

		public override void UpdateView () {
			isEnable = (Client.cur_player != Library.GetCurrentPlayer(data.context));
		}
		
		void PlaceApollo() {
			//TODO
		}
	}
}
