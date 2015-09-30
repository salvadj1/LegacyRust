using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PostAuthFetchChildOrParentAttribute : PostAuthAttribute
{
	private const AuthOptions kFixedOptions = AuthOptions.SearchDown | AuthOptions.SearchUp;

	private PostAuthFetchChildOrParentAttribute(AuthTarg target, bool includeThisGameObject, string nameMask) : base(target, (!includeThisGameObject ? AuthOptions.SearchDown | AuthOptions.SearchUp : AuthOptions.SearchDown | AuthOptions.SearchUp | AuthOptions.SearchInclusive), nameMask)
	{
	}

	public PostAuthFetchChildOrParentAttribute(AuthTarg target, string nameMask) : this(target, false, nameMask)
	{
	}

	public PostAuthFetchChildOrParentAttribute(AuthTarg target, bool includeThisGameObject) : this(target, includeThisGameObject, null)
	{
	}
}