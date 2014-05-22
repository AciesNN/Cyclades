using UnityEngine;
using System.Collections;

using Shmipl.Unity;

//TODO - подумать над идеей: отличие св-ва от значения соот-ещего виджета, особенно при а) изначальном создании, б) реинициализации
//по идее, от этого должна спасать UpdateView, но не спасает (возможно, не вызывается родителем)
namespace Shmipl.GameScene
{
	public class AuctionPlayerInformationController : UIController {

		public GameObject changeAnim;

		public override void UpdateView() {
			Player_UpdateView();
			ActivePlayer_UpdateView();
			Income_UpdateView();
			Priests_UpdateView();
			Philosophers_UpdateView();

			GodColor_UpdateView();
			GodVisible_UpdateView();
		}

		/* God color*/
		public UISprite godSprite;
		private Color god_color;
		public Color GodColor {
			get { return god_color; }
			set {
				if (god_color != value) {
					god_color = value;
					GodColor_UpdateView();
				}
			}
		}
		
		public void GodColor_UpdateView() {
			godSprite.color = god_color;
		}

		private bool god_visible = true;
		public bool GodVisible {
			get { return god_visible; }
			set {
				if (god_visible != value) {
					god_visible = value;
					GodVisible_UpdateView();
				}
			}
		}
		
		public void GodVisible_UpdateView() {
			godSprite.gameObject.SetActive(god_visible);
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
		private long gold = 0;
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
			Instantiate(changeAnim, incomeView.transform.position, Quaternion.identity);
		}


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