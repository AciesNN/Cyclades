using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shmipl.Unity
{
	public delegate void ForEachDo<ChildType> (int i, ChildType ch) where ChildType : UIController;

	public class UICollectionController : UIController {

		public UIController[] childs;

		// Use this for initialization
		void Start () {
			RegisterChilds();
		}

		public override void UpdateView() {
			ForEachChild<UIController>(
				(i, ch) => { ch.UpdateView(); });
		}

		public void RegisterChilds() {
			ForEachChild<UIController>(
				(i, ch) => { ch.RegisterParent(this, i); });
		}

		public void Childrens_UpdateView() {
			ForEachChild<UIController>(
				(i, ch) => { ch.UpdateView(); });
		}

		public IEnumerable<int> ChildsRange() {
			return Enumerable.Range(0, childs.Length);
		}

		public ChildType GetChild<ChildType>(int index) where ChildType : UIController {
			return (ChildType)childs[index];
		}

		public void ForEachChild<ChildType>(ForEachDo<ChildType> f) where ChildType : UIController {
			foreach(int i in ChildsRange()) {
				f(i, (ChildType)childs[i]);
			}
		}
	}
}
