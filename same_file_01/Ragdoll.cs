using System;

public class Ragdoll : Character
{
	[NonSerialized]
	public IDMain sourceMain;

	public Ragdoll() : this(IDFlags.Unknown)
	{
	}

	protected Ragdoll(IDFlags flags) : base(flags)
	{
	}

	protected new void Awake()
	{
		base.LoadTraitMapNonNetworked();
		base.Awake();
	}
}