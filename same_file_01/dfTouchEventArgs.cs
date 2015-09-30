using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class dfTouchEventArgs : dfMouseEventArgs
{
	public bool IsMultiTouch
	{
		get
		{
			return this.Touches.Count > 1;
		}
	}

	public UnityEngine.Touch Touch
	{
		get;
		private set;
	}

	public List<UnityEngine.Touch> Touches
	{
		get;
		private set;
	}

	public dfTouchEventArgs(dfControl Source, UnityEngine.Touch touch, UnityEngine.Ray ray) : base(Source, dfMouseButtons.Left, touch.tapCount, ray, touch.position, 0f)
	{
		this.Touch = touch;
		this.Touches = new List<UnityEngine.Touch>()
		{
			touch
		};
		if (touch.deltaTime > 1.401298E-45f)
		{
			base.MoveDelta = touch.deltaPosition * (Time.deltaTime / touch.deltaTime);
		}
	}

	public dfTouchEventArgs(dfControl source, List<UnityEngine.Touch> touches, UnityEngine.Ray ray) : this(source, touches.First<UnityEngine.Touch>(), ray)
	{
		this.Touches = touches;
	}

	public dfTouchEventArgs(dfControl Source) : base(Source)
	{
		base.Position = Vector2.zero;
	}
}