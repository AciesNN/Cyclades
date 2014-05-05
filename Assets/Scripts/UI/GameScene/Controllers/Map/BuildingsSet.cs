﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl.Unity;
using Shmipl.FrmWrk.Library;

namespace Shmipl.GameScene
{
	public class BuildingsSet : UIController {
		public TextMesh[] buildingsSign;

		void Awake() {
			if (buildingsSign.Length != 4)
				Debug.LogError("не заполнены объекты зданий для контроллера набора зданий");
		}

		public void SetInfo(List<object> buildings, bool isMetro, long metroSize) {
			for(int i = 0; i < buildingsSign.Length; ++i) {
				if (i >= buildings.Count) {
					buildingsSign[i].gameObject.SetActive(false);
				} else {
					buildingsSign[i].gameObject.SetActive(true);
					string bld = (string)buildings[i];
					buildingsSign[i].text = (bld == "" ? "-" : bld);
					buildingsSign[i].color = main.instance.GetBuildColor(bld);
				}
			}

			if (isMetro) {
				for (int t = 0; t < metroSize; ++t) {
					buildingsSign[t].text = "!!";
					buildingsSign[t].color = Color.black;
				}
			}
		}
	}
}