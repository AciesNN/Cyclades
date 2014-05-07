using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class NavyCountDialog : MonoBehaviour {
		public static NavyCountDialog instance;
		GodPoseidon godPoseiden;
		public UILabel inputLabel;

		public static void ShowDilog(GodPoseidon godPoseiden) {
			if (!instance) {
				instance = FindObjectOfType<NavyCountDialog>();
			}
		
			instance.ShowDialog(godPoseiden);
		}

		void ShowDialog(GodPoseidon godPoseiden) {
			transform.position = Vector3.zero;
			gameObject.SetActive(true);
			this.godPoseiden = godPoseiden;
		}

		public static void CloseDilog() {
			if (!instance) {
				instance = FindObjectOfType<NavyCountDialog>();
			}
			instance.CloseDialog();
		}

		void CloseDialog() {
			gameObject.SetActive(false);
		}

		void PressCancel() {
			Debug.Log ("cancel");
			CloseDilog();
		}

		void PressOK() {
			godPoseiden.move_navy_count = System.Convert.ToInt64(inputLabel.text);
			CloseDilog();
		}
	}
}
