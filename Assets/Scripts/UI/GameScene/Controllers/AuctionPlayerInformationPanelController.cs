using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AuctionPlayerInformationPanelController : UICollectionController
	{		
		public override void UpdateView ()
		{
			long players_number = data.context.Get<long> ("/players_number");
			
			ForEachChild<AuctionPlayerInformationController> ((i, ch) => {
				ch.gameObject.SetActive (i < players_number);

				if (i < players_number) {
					ch.Gold = data.context.Get<long> ("/markers/gold/[{0}]", i);
					ch.Priests = data.context.Get<long> ("/markers/priest/[{0}]", i);
					ch.Philosophers = data.context.Get<long> ("/markers/philosopher/[{0}]", i);
				}
			});
		}
	}
}