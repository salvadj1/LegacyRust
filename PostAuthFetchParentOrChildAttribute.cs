using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PostAuthFetchParentOrChildAttribute : PostAuthAttribute
{
	private const AuthOptions kFixedOptions = AuthOptions.SearchDown | AuthOptions.SearchUp | AuthOptions.SearchReverse;

	private PostAuthFetchParentOrChildAttribute(AuthTarg target, bool includeThisGameObject, string nameMask) : base(target, (!includeThisGameObject ? AuthOptions.SearchDown | AuthOptions.SearchUp | AuthOptions.SearchReverse : AuthOptions.SearchDown | AuthOptions.SearchUp | AuthOptions.SearchInclusive | AuthOptions.SearchReverse), nameMask)
	{
	}

	public PostAuthFetchParentOrChildAttribute(AuthTarg target, string nameMask) : this(target, false, nameMask)
	{
	}

	public PostAuthFetchParentOrChildAttribute(AuthTarg target, bool includeThisGameObject) : this(target, includeThisGameObject, null)
	{
	}
}