using System;
using UnityEngine;

public class AuthorPalletObject
{
	public AuthorPalletObject.Validator validator;

	public AuthorPalletObject.Creator creator;

	public GUIContent guiContent;

	public AuthorPalletObject()
	{
	}

	public bool Create(AuthorCreation creation, out AuthorPeice peice)
	{
		if (this.creator == null)
		{
			peice = null;
			return false;
		}
		peice = this.creator(creation, this);
		return peice;
	}

	public bool Validate(AuthorCreation creation)
	{
		return (this.validator == null ? true : this.validator(creation, this));
	}

	public delegate AuthorPeice Creator(AuthorCreation creation, AuthorPalletObject obj);

	public delegate bool Validator(AuthorCreation creation, AuthorPalletObject obj);
}