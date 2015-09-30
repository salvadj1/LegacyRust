using System;

public abstract class PostAuthAttribute : Attribute
{
	public const AuthOptions kOption_None = 0;

	public const AuthOptions kOption_Down = AuthOptions.SearchDown;

	public const AuthOptions kOption_Up = AuthOptions.SearchUp;

	public const AuthOptions kOption_NameMask = 4;

	public const AuthOptions kOption_Include = AuthOptions.SearchInclusive;

	public const AuthOptions kOption_Reverse = AuthOptions.SearchReverse;

	private readonly AuthOptions _options;

	private readonly AuthTarg _target;

	private readonly string _nameMask;

	public string nameMask
	{
		get
		{
			return this._nameMask;
		}
	}

	public AuthOptions options
	{
		get
		{
			return this._options;
		}
	}

	public AuthTarg target
	{
		get
		{
			return this._target;
		}
	}

	internal PostAuthAttribute(AuthTarg target, AuthOptions options, string nameMask)
	{
		this._target = target;
		if (string.IsNullOrEmpty(nameMask))
		{
			this._options = options;
			this._nameMask = string.Empty;
		}
		else
		{
			this._options = (AuthOptions)((int)options | 4);
			this._nameMask = nameMask;
		}
	}
}