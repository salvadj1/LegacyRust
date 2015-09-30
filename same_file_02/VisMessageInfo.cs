using System;

public sealed class VisMessageInfo : IDisposable
{
	private VisNode _other;

	private VisReactor _self;

	private VisMessageInfo next;

	private VisMessageInfo.Kind _kind;

	private static VisMessageInfo dump;

	public bool isSeeEvent
	{
		get
		{
			return (this._kind & VisMessageInfo.Kind.SeeEnter) == VisMessageInfo.Kind.SeeEnter;
		}
	}

	public bool isSpectatingEvent
	{
		get
		{
			return ((byte)this._kind - (byte)VisMessageInfo.Kind.SeeEnter & (byte)VisMessageInfo.Kind.SeeEnter) == (byte)VisMessageInfo.Kind.SeeEnter;
		}
	}

	public VisReactor issuer
	{
		get
		{
			return this._self;
		}
	}

	public bool isTwoNodeEvent
	{
		get
		{
			return this._kind > VisMessageInfo.Kind.SpectatorExit;
		}
	}

	public VisMessageInfo.Kind kind
	{
		get
		{
			return this._kind;
		}
	}

	public VisNode other
	{
		get
		{
			return this._other;
		}
	}

	public VisNode seeer
	{
		get
		{
			return this.spectator;
		}
	}

	public VisNode seenNode
	{
		get
		{
			return this.spectated;
		}
	}

	public VisNode self
	{
		get
		{
			return this._self.node;
		}
	}

	public VisNode sender
	{
		get
		{
			return this._self.node;
		}
	}

	public VisNode spectated
	{
		get
		{
			return (!this.isSeeEvent ? this.self : this._other);
		}
	}

	public VisNode spectator
	{
		get
		{
			return (!this.isSpectatingEvent ? this.self : this._other);
		}
	}

	public VisNode target
	{
		get
		{
			return this._other;
		}
	}

	private VisMessageInfo()
	{
	}

	public static VisMessageInfo Create(VisReactor issuer, VisNode other, VisMessageInfo.Kind kind)
	{
		VisMessageInfo visMessageInfo;
		if (VisMessageInfo.dump == null)
		{
			visMessageInfo = new VisMessageInfo();
		}
		else
		{
			visMessageInfo = VisMessageInfo.dump;
			VisMessageInfo.dump = visMessageInfo.next;
			visMessageInfo.next = null;
		}
		visMessageInfo._self = issuer;
		visMessageInfo._other = other;
		visMessageInfo._kind = kind;
		return visMessageInfo;
	}

	public static VisMessageInfo Create(VisReactor issuer, VisMessageInfo.Kind kind)
	{
		VisMessageInfo visMessageInfo;
		if (VisMessageInfo.dump == null)
		{
			visMessageInfo = new VisMessageInfo();
		}
		else
		{
			visMessageInfo = VisMessageInfo.dump;
			VisMessageInfo.dump = visMessageInfo.next;
			visMessageInfo.next = null;
		}
		visMessageInfo._self = issuer;
		visMessageInfo._other = null;
		visMessageInfo._kind = kind;
		return visMessageInfo;
	}

	void System.IDisposable.Dispose()
	{
		if ((int)this._kind != 0)
		{
			this._kind = (VisMessageInfo.Kind)0;
			this.next = VisMessageInfo.dump;
			VisMessageInfo.dump = this;
			this._other = null;
			this._self = null;
		}
	}

	public enum Kind : byte
	{
		SeeEnter = 1,
		SpectatedEnter = 2,
		SeeExit = 3,
		SpectatorExit = 4,
		SeeAdd = 5,
		SeeRemove = 7,
		SpectatorAdd = 8,
		SpectatorRemove = 10
	}
}