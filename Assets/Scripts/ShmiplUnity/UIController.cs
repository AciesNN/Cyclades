using UnityEngine;
using System.Collections;

namespace Shmipl.Unity
{
	public class UIController : MonoBehaviour {
		public UIController parentController = null;
		public int index = -1;

		public UIButton[] buttons;

		private bool _isEnable = true;
		public bool isEnable 
		{
			get { return _isEnable; }
			set { 
				_isEnable = value;
				foreach(UIButton b in buttons) {
					b.isEnabled = _isEnable;
				}
			}
		}

		public void RegisterParent(UIController parent, int _index) {
			parentController = parent;
			index = _index;
		}

		public virtual void UpdateView() {

		}

		private void UpdateButtons() {
			buttons = gameObject.GetComponentsInChildren<UIButton>();
		}

		void Start() {
			UpdateButtons();
			UpdateView();
		}
	}
}