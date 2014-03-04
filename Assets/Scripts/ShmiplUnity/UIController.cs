using UnityEngine;
using System.Collections;

namespace Shmipl.Unity
{
	public class UIController : MonoBehaviour {
		public UIController parentController = null;
		public int index = -1;

		public UIButton[] buttons;

		private bool _isEnabled = true;
		public bool isEnabled 
		{
			get { return _isEnabled; }
			set { 
				_isEnabled = value;
				foreach(UIButton b in buttons) {
					b.isEnabled = _isEnabled;
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