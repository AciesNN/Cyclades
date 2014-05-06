using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class ArmyCountDialog : MonoBehaviour {
		public static ArmyCountDialog instance;

		public static void ShowDilog() {
			if (!instance) {
				instance = FindObjectOfType<ArmyCountDialog>();
			}

			instance.ShowDialog();
		
		}

		void ShowDialog() {
			transform.position = Vector3.zero;
			gameObject.SetActive(true);
		}

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
