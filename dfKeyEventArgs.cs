using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class dfKeyEventArgs : dfControlEventArgs
{
	public bool Alt
	{
		get;
		set;
	}

	public char Character
	{
		get;
		set;
	}

	public bool Control
	{
		get;
		set;
	}

	public UnityEngine.KeyCode KeyCode
	{
		get;
		set;
	}

	public bool Shift
	{
		get;
		set;
	}

	internal dfKeyEventArgs(dfControl source, UnityEngine.KeyCode Key, bool Control, bool Shift, bool Alt) : base(source)
	{
		this.KeyCode = Key;
		this.Control = Control;
		this.Shift = Shift;
		this.Alt = Alt;
	}

	public override string ToString()
	{
		return string.Format("Key: {0}, Control: {1}, Shift: {2}, Alt: {3}", new object[] { this.KeyCode, this.Control, this.Shift, this.Alt });
	}
}