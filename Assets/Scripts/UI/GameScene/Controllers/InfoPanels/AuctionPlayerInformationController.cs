using UnityEngine;
using System.Collections;

using Shmipl.Unity;

namespace Shmipl.GameScene
{
	public class AuctionPlayerInformationController : UIController {
		
		public override void UpdateView() {
			Player_UpdateView();
			ActivePlayer_UpdateView();
			Income_UpdateView();
			Priests_UpdateView();
			Philosophers_UpdateView();
		}

		/* Player */
		public UILabel playerSign;
		private long player;
		public long Player {
			get { return player; }
			set {
				if (player != value) {
					player = value;
					Player_UpdateView();
				}
			}
		}
		
		public void Player_UpdateView() {
			playerSign.text = "" + player;
		}

		/* Active player */
		public UISprite playerSprite;
		private bool activePlayer;
		public bool ActivePlayer {
			get { return activePlayer; }
			set {
				if (activePlayer != value) {
					activePlayer = value;
					ActivePlayer_UpdateView();
				}
			}
		}
		
		public void ActivePlayer_UpdateView() {
			playerSprite.color = (activePlayer ? Color.green : Color.white);
		}

		/* Color */
		private Color color;
		public Color Color_ {
			get { return color; }
			set {
				if (color != value) {
					color = value;
					Color_UpdateView();
				}
			}
		}
		
		public void Color_UpdateView() {
			playerSign.color = color;
		}

		/* Income */
		public UILabel incomeView;
		private string income = "";
		public string Income {
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