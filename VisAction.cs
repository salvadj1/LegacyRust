using System;
using UnityEngine;

public abstract class VisAction : ScriptableObject
{
	protected VisAction()
	{
	}

	public abstract void Accomplish(IDMain self, IDMain instigator);

	public abstract void UnAcomplish(IDMain self, IDMain instigator);
}