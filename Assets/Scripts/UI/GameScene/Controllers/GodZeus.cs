using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class GodZeus : UIController {
		
		public override void UpdateView () {
			
		}
		
		public void BuyPriest() {
			Hashtable msg = Client.BuyPriest();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
		}
	}
}
