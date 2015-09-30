using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RPOSWindowScrollable : RPOSWindow
{
	public UIDraggablePanel myDraggablePanel;

	public bool horizontal;

	public bool vertical = true;

	protected bool autoResetScrolling = true;

	private bool didManualStart;

	private bool queuedCalculationNextFrame;

	private bool cancelCalculationNextFrame;

	protected Vector2 initialScrollValue;

	public RPOSWindowScrollable()
	{
	}

	protected void NextFrameRecalculateBounds()
	{
		this.cancelCalculationNextFrame = false;
		if (!this.queuedCalculationNextFrame)
		{
			base.StartCoroutine(this.Routine_NextFrameRecalculateBounds());
		}
	}

	protected override void OnWindowShow()
	{
		base.OnWindowShow();
		if (this.autoResetScrolling)
		{
			this.ResetScrolling();
		}
	}

	protected void ResetScrolling()
	{
		this.ResetScrolling(false);
	}

	protected virtual void ResetScrolling(bool retainCurrentValue)
	{
		UIScrollBar componentInChildren;
		UIScrollBar uIScrollBar;
		UIScrollBar uIScrollBar1;
		UIScrollBar uIScrollBar2;
		UIScrollBar uIScrollBar3 = null;
		UIScrollBar uIScrollBar4 = null;
		if (this.myDraggablePanel)
		{
			if (!retainCurrentValue)
			{
				if (!this.vertical)
				{
					uIScrollBar1 = null;
				}
				else
				{
					uIScrollBar1 = this.myDraggablePanel.verticalScrollBar;
				}
				uIScrollBar3 = uIScrollBar1;
				if (!this.horizontal)
				{
					uIScrollBar2 = null;
				}
				else
				{
					uIScrollBar2 = this.myDraggablePanel.horizontalScrollBar;
				}
				uIScrollBar4 = uIScrollBar2;
			}
			if (!this.didManualStart)
			{
				this.myDraggablePanel.ManualStart();
				this.didManualStart = true;
			}
			this.myDraggablePanel.calculateBoundsEveryChange = false;
			this.NextFrameRecalculateBounds();
		}
		else if (!retainCurrentValue)
		{
			if (!this.vertical || this.horizontal)
			{
				componentInChildren = null;
			}
			else
			{
				componentInChildren = base.GetComponentInChildren<UIScrollBar>();
			}
			uIScrollBar3 = componentInChildren;
			if (!this.horizontal || this.vertical)
			{
				uIScrollBar = null;
			}
			else
			{
				uIScrollBar = base.GetComponentInChildren<UIScrollBar>();
			}
			uIScrollBar4 = uIScrollBar;
		}
		if (!retainCurrentValue)
		{
			if (this.vertical && uIScrollBar3)
			{
				uIScrollBar3.scrollValue = this.initialScrollValue.y;
				uIScrollBar3.ForceUpdate();
			}
			if (this.horizontal && uIScrollBar4)
			{
				uIScrollBar4.scrollValue = this.initialScrollValue.x;
				uIScrollBar4.ForceUpdate();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator Routine_NextFrameRecalculateBounds()
	{
		RPOSWindowScrollable.<Routine_NextFrameRecalculateBounds>c__Iterator34 variable = null;
		return variable;
	}
}