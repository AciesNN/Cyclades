using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodSophia : UIController {

		public override void UpdateView () {
			isEnabled = (Client.cur_player == Library.GetCurrentPlayer(data.context));
		}

		public void BuyPhilosopher() {
			Hashtable msg = Client.BuyPhilosopher();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}

		void BuyBuild() {
			//TODO
		}
	}
}
