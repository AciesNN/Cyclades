using UnityEngine;
using System.Collections;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class MapObjectController : UIController {
		public TextMesh countText;

		public void SetCount(long count) {
			if (count == 0)
				countText.text = "";
			else
				countText.text = "" + count;
		}

		public void SetText(string text) {
			countText.text = text;
		}
	}
}