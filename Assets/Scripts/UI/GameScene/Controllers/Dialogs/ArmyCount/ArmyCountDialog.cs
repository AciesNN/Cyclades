using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

//TODO явная чушь с тем, что один диалог обслуживается двумя скриптами, которые оба принимают сигналы от отдних кнопок
//TODO инициализация instance основана только на том, что элемент активен
namespace Shmipl.GameScene
{
	public class ArmyCountDialog : MonoBehaviour {
		public static ArmyCountDialog instance;
		GodMars godMars;
		public UILabel inputLabel;
		bool isActive = false;

		void Awake() {
			instance = FindObjectOfType<ArmyCountDialog>();
		}

		public static void ShowDilog(GodMars godMars) {
			instance.ShowDialog(godMars);
		}

		public static void SetValue(string val) {
			instance.inputLabel.text = val;
		}

		void ShowDialog(GodMars godMars) {
			if (isActive)
				return;
			isActive = true;

			transform.position = Vector3.zero;
			gameObject.SetActive(true);
			this.godMars = godMars;
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

			godMars.move_army_count = System.Convert.ToInt64(inputLabel.text);
			CloseDilog();
		}
	}
}
