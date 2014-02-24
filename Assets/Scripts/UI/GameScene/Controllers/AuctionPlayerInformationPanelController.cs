using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Shmipl.Unity;
using Cyclades.Game;

namespace Shmipl.GameScene
{
	public class AuctionPlayerInformationPanelController : UICollectionController
	{		
		public override void UpdateView ()
		{
			long players_number = data.context.GetLong ("/players_number");
			
			ForEachChild<AuctionPlayerInformationController> ((i, ch) => {
				ch.gameObject.SetActive (i < players_number);

				if (i < players_number) {
					ch.Income = data.context.GetLong ("/markers/income/[{0}]", i);
					ch.Priests = data.context.GetLong ("/markers/priest/[{0}]", i);
					ch.Philosophers = data.context.GetLong ("/markers/philosopher/[{0}]", i);
				}
			});
		}
	}
}