using System;
using UnityEngine;

public abstract class ArmorModel<TArmorModel> : ArmorModel
where TArmorModel : ArmorModel<TArmorModel>, new()
{
	[SerializeField]
	protected TArmorModel censored;

	protected sealed override ArmorModel _censored
	{
		get
		{
			return (object)this.censored;
		}
	}

	public new TArmorModel censoredModel
	{
		get
		{
			return this.censored;
		}
	}

	public new bool hasCensoredModel
	{
		get
		{
			return this.censored;
		}
	}

	internal ArmorModel(ArmorModelSlot slot) : base(slot)
	{
	}
}