using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class dfDragEventArgs : dfControlEventArgs
{
	public object Data
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

	public dfDragDropState State
	{
		get;
		set;
	}

	public dfControl Target
	{
		get;
		set;
	}

	internal dfDragEventArgs(dfControl source) : base(source)
	{
		this.State = dfDragDropState.None;
	}

	internal dfDragEventArgs(dfControl source, dfDragDropState state, object data, UnityEngine.Ray ray, Vector2 position) : base(source)
	{
		this.Data = data;
		this.State = state;
		this.Position = position;
		this.Ray = ray;
	}
}