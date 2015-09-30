using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class dfMouseEventArgs : dfControlEventArgs
{
	public dfMouseButtons Buttons
	{
		get;
		private set;
	}

	public int Clicks
	{
		get;
		private set;
	}

	public Vector2 MoveDelta
	{
		get;
		set;
	}

	public Vector2 Position
	{
		get;
		set;
	}

	public UnityEngine.Ray Ray
	{
		get;
		set;
	}

	public float WheelDelta
	{
		get;
		private set;
	}

	public dfMouseEventArgs(dfControl Source, dfMouseButtons button, int clicks, UnityEngine.Ray ray, Vector2 location, float wheel) : base(Source)
	{
		this.Buttons = button;
		this.Clicks = clicks;
		this.Position = location;
		this.WheelDelta = wheel;
		this.Ray = ray;
	}

	public dfMouseEventArgs(dfControl Source) : base(Source)
	{
		this.Buttons = dfMouseButtons.None;
		this.Clicks = 0;
		this.Position = Vector2.zero;
		this.WheelDelta = 0f;
	}
}