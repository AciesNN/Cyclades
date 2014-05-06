using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class ArmyCountDialog : MonoBehaviour {
		public static ArmyCountDialog instance;
		GodMars godMars;
		public UILabel inputLabel;

		public static void ShowDilog(GodMars godMars) {
			if (!instance) {
				instance = FindObjectOfType<ArmyCountDialog>();
			}
		
			instance.ShowDialog(godMars);
		}

		void ShowDialog(GodMars godMars) {
			transform.position = Vector3.zero;
			gameObject.SetActive(true);
			this.godMars = godMars;
		}

		public static void CloseDilog() {
			if (!instance) {
				instance = FindObjectOfType<ArmyCountDialog>();
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
			godMars.move_army_count = System.Convert.ToInt64(inputLabel.text);
			CloseDilog();
		}
	}
}
