using System;

public class BaseHitBox : IDRemote
{
	public new Character idMain
	{
		get
		{
			return (Character)base.idMain;
		}
	}

	public BaseHitBox()
	{
	}
}