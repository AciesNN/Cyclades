using UnityEngine;
using System.Collections;

namespace Shmipl.Unity
{
	public class UIController : MonoBehaviour {

		public UIController parentController = null;
		public int index = -1;

		public void RegisterParent(UIController parent, int _index) {
			parentController = parent;
			index = _index;
		}

		public virtual void UpdateView() {

		}

		void Start() {
			UpdateView();
		}
	}
}