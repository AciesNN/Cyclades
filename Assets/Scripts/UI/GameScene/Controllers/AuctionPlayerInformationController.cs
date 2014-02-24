using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AuctionPlayerInformationController : UIController {
		
		public override void UpdateView() {
			Income_UpdateView();
			Priests_UpdateView();
			Philosophers_UpdateView();
		}
		
		/* Gold */
		public UILabel incomeView;
		private long income;
		public long Income {
			get { return income; }
			set {
				if (income != value) {
					income = value;
					Income_UpdateView();
				}
			}
		}
		
		public void Income_UpdateView() {
			incomeView.text = "" + income;
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