using System;

public class DeployableCorpse : IDMain
{
	private float lifeTime = 300f;

	public DeployableCorpse() : this(IDFlags.Unknown)
	{
	}

	protected DeployableCorpse(IDFlags flags) : base(flags)
	{
	}
}