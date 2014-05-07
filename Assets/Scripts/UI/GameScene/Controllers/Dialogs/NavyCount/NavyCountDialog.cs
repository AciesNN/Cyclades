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
		bool isActive = false;

		void Awake() {
			instance = FindObjectOfType<NavyCountDialog>();
		}

		public static void ShowDilog(GodPoseidon godPoseiden) {
			instance.ShowDialog(godPoseiden);
		}

		public static void SetValue(string val) {
			instance.inputLabel.text = val;
		}

		void ShowDialog(GodPoseidon godPoseiden) {
			if (isActive)
				return;
			isActive = true;

			transform.position = Vector3.zero;
			gameObject.SetActive(true);
			this.godPoseiden = godPoseiden;
		}

		public static void CloseDilog() {
			instance.CloseDialog();
		}

		void CloseDialog() {
			isActive = false;

			gameObject.SetActive(false);
		}

		void PressCancel() {
			if (!isActive)
				return;

			Debug.Log ("cancel");
			CloseDilog();
		}

		void PressOK() {
			if (!isActive)
				return;

			godPoseiden.move_navy_count = System.Convert.ToInt64(inputLabel.text);
			CloseDilog();
		}
	}
}
