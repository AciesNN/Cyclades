using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class GodMars : UIController {
		
		BuyBuildState buyBuildState = BuyBuildState.notBuy;

		public override void UpdateView () {
			isEnabled = (Client.cur_player == Library.GetCurrentPlayer(data.context));
		}
		
		void BuyArmy() {
			//TODO
		}
		
		void MoveArmy() {
			//TODO
		}
		
		void BuyBuild() {
			switch (buyBuildState) {
			case(BuyBuildState.notBuy): 
				Shmipl.Base.Messenger<Coords>.AddListener("Shmipl.Map.Click", OnMapClick_Build);
				buyBuildState = BuyBuildState.buy;
				break;
			case(BuyBuildState.buy):
				Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
				buyBuildState = BuyBuildState.notBuy;				
				break;
			}
		}

		void OnMapClick_Build(Coords coords) {
			Hashtable 
				msg = Client.BuyBuild();
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
			
			msg = Client.PlaceBuilding(Library.Map_GetIslandByPoint(data.context, coords.x, coords.y), 0);
			Debug.Log ("msg: " + Shmipl.Base.json.dumps(msg));

			Shmipl.Base.Messenger<Coords>.RemoveListener("Shmipl.Map.Click", OnMapClick_Build);
			buyBuildState = BuyBuildState.notBuy;
		}
	}
}
