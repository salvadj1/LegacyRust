using System;
using System.Runtime.CompilerServices;

public class dfFocusEventArgs : dfControlEventArgs
{
	public dfControl GotFocus
	{
		get
		{
			return base.Source;
		}
	}

	public dfControl LostFocus
	{
		get;
		private set;
	}

	internal dfFocusEventArgs(dfControl GotFocus, dfControl LostFocus) : base(GotFocus)
	{
		this.LostFocus = LostFocus;
	}
}