using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PostAuthFetchAttribute : PostAuthAttribute
{
	public PostAuthFetchAttribute(AuthTarg target, string nameMask) : base(target, 0, nameMask)
	{
	}

	public PostAuthFetchAttribute(AuthTarg target) : this(target, null)
	{
	}
}