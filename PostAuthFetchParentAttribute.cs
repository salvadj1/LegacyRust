using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PostAuthFetchParentAttribute : PostAuthAttribute
{
	private const AuthOptions kFixedOptions = AuthOptions.SearchUp;

	private PostAuthFetchParentAttribute(AuthTarg target, bool includeThisGameObject, string nameMask) : base(target, (!includeThisGameObject ? AuthOptions.SearchUp : AuthOptions.SearchUp | AuthOptions.SearchInclusive), nameMask)
	{
	}

	public PostAuthFetchParentAttribute(AuthTarg target, string nameMask) : this(target, false, nameMask)
	{
	}

	public PostAuthFetchParentAttribute(AuthTarg target, bool includeThisGameObject) : this(target, includeThisGameObject, null)
	{
	}
}