using Facepunch.Prefetch;
using System;
using UnityEngine;

public class RPOS_Craft_IngredientPlaque : MonoBehaviour
{
	[PrefetchChildComponent(NameMask="ItemName")]
	public UILabel itemName;

	[PrefetchChildComponent(NameMask="NeedLabel")]
	public UILabel need;

	[PrefetchChildComponent(NameMask="HaveLabel")]
	public UILabel have;

	[PrefetchChildComponent(NameMask="checkmark")]
	public UISprite checkIcon;

	[PrefetchChildComponent(NameMask="xmark")]
	public UISprite xIcon;

	public RPOS_Craft_IngredientPlaque()
	{
	}

	public void Bind(BlueprintDataBlock.IngredientEntry ingredient, int needAmount, int haveAmount)
	{
		Color color;
		ItemDataBlock itemDataBlock = ingredient.Ingredient;
		if (needAmount > haveAmount)
		{
			this.checkIcon.enabled = false;
			this.xIcon.enabled = true;
			color = Color.red;
		}
		else
		{
			this.checkIcon.enabled = true;
			this.xIcon.enabled = false;
			color = Color.green;
		}
		UILabel uILabel = this.need;
		Color color1 = color;
		this.have.color = color1;
		uILabel.color = color1;
		this.itemName.text = itemDataBlock.name;
		this.need.text = needAmount.ToString("N0");
		this.have.text = haveAmount.ToString("N0");
	}
}