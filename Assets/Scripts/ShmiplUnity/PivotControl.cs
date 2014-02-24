using UnityEngine;
using System.Collections;

namespace Shmipl.Unity
{
	[ExecuteInEditMode]
	public class PivotControl : MonoBehaviour {
		public UIWidget widget;

		// Update is called once per frame
		void Update () {
			switch (widget.GetComponent<UIAnchor>().side)
			{
			case UIAnchor.Side.Bottom:
			{
				widget.pivot=UIWidget.Pivot.Bottom;
				break;
			}
			
			case UIAnchor.Side.Top:
			{
				widget.pivot=UIWidget.Pivot.Top;
				break;
			}
			case UIAnchor.Side.Left:
			{
				widget.pivot=UIWidget.Pivot.Left;
				break;
			}
			case UIAnchor.Side.Right:
			{
				widget.pivot=UIWidget.Pivot.Right;
				break;
			}
			case UIAnchor.Side.TopRight:
			{
				widget.pivot=UIWidget.Pivot.TopRight;
				break;
			}
			case UIAnchor.Side.TopLeft:
			{
				widget.pivot=UIWidget.Pivot.TopLeft;
				break;
			}
			case UIAnchor.Side.BottomLeft:
			{
				widget.pivot=UIWidget.Pivot.BottomLeft;
				break;
			}
			case UIAnchor.Side.BottomRight:
			{
				widget.pivot=UIWidget.Pivot.BottomRight;
				break;
			}
			case UIAnchor.Side.Center:
			{
				widget.pivot=UIWidget.Pivot.Center;
				break;
			}
			}


		}
	}
}
