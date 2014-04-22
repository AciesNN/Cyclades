using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodApollo : UIController {

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

		void PlaceApollo() {
			//TODO
		}
	}
}
