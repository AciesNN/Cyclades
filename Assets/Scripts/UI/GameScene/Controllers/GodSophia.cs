using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodSophia : UIController {

		public override void UpdateView () {

		}

		public void BuyPhilosopher() {
			Hashtable msg = Client.BuyPhilosopher();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}
	}
}
