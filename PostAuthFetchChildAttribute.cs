using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PostAuthFetchChildAttribute : PostAuthAttribute
{
	private const AuthOptions kFixedOptions = AuthOptions.SearchDown;

	private PostAuthFetchChildAttribute(AuthTarg target, bool includeThisGameObject, string nameMask) : base(target, (!includeThisGameObject ? AuthOptions.SearchDown : AuthOptions.SearchDown | AuthOptions.SearchInclusive), nameMask)
	{
	}

	public PostAuthFetchChildAttribute(AuthTarg target, string nameMask) : this(target, false, nameMask)
	{
	}

	public PostAuthFetchChildAttribute(AuthTarg target, bool includeThisGameObject) : this(target, includeThisGameObject, null)
	{
	}
}