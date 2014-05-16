using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl.Unity;

namespace Shmipl.GameScene
{
	enum PanelIndex {
		Zeus = 0,
		Sophia = 1,
		Mars = 2,
		Poseidon = 3,
		Appolon = 4,

		Auction = 5
	}

	public class AuctionPanelController : UICollectionController {
		public GameObject [] panels;

		public override void UpdateView () {
			if (!main.instance.isContextReady(main.instance.context)) 
				return;

			switch(Library.GetPhase(main.instance.context)) {
			case(Phase.AuctionPhase):
				SetActivePanel((int) PanelIndex.Auction);
				break;
			case(Phase.TurnPhase):
				string current_god = main.instance.context.GetStr("/turn/current_god");
				if (Constants.gods.IndexOf(current_god) != -1)
					SetActivePanel(Constants.gods.IndexOf(current_god));
				break;
			}
		}

		private void SetActivePanel (int index) {

			for (int i = 0; i < panels.Length; ++i) {
				panels[i].SetActive(i == index);
			}

			if(childs[index]) {
				childs[index].UpdateView();
			}
		}

	}


}
