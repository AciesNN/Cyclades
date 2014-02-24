using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AuctionPlayerInformationController : UIController {
		
		public override void UpdateView() {
			Gold_UpdateView();
			Priests_UpdateView();
			Philosophers_UpdateView();
		}
		
		/* Gold */
		public UILabel goldView;
		private long gold;
		public long Gold {
			get { return gold; }
			set {
				if (gold != value) {
					gold = value;
					Gold_UpdateView();
				}
			}
		}
		
		public void Gold_UpdateView() {
			goldView.text = "" + gold;
		}
		
		/* Priests */
		public UILabel priestsView;
		private long priests;
		public long Priests {
			get { return priests; }
			set { 
				if (priests != value) {
					priests = value;
					Priests_UpdateView();
				}
			}
		}
		
		public void Priests_UpdateView() {
			priestsView.text = "" + priests;
		}
		
		/* Philosophers */
		public UILabel philosophersView;
		private long philosophers;
		public long Philosophers {
			get { return philosophers; }
			set { 
				if (philosophers != value) {
					philosophers = value;
					Philosophers_UpdateView();
				}
			}
		}
		
		public void Philosophers_UpdateView() {
			philosophersView.text = "" + philosophers;
		}
	}
}