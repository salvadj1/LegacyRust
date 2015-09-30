using System;
using UnityEngine;

public class RPOSCraftItemEntry : MonoBehaviour
{
	public ItemDataBlock actualItemDataBlock;

	public BlueprintDataBlock blueprint;

	public RPOSCraftWindow craftWindow;

	public RPOSCraftItemEntry()
	{
	}

	public void OnTooltip(bool show)
	{
		ItemDataBlock itemDataBlock;
		if (!show || !(this.actualItemDataBlock != null))
		{
			itemDataBlock = null;
		}
		else
		{
			itemDataBlock = this.actualItemDataBlock;
		}
		ItemToolTip.SetToolTip(itemDataBlock, null);
	}

	public void SetSelected(bool selected)
	{
		Color color;
		color = (!selected ? Color.white : Color.yellow);
		base.GetComponentInChildren<UILabel>().color = color;
	}

	public void Update()
	{
		if (!RPOS.IsOpen)
		{
			return;
		}
		if (!this.blueprint || !(this.blueprint == this.craftWindow.selectedItem))
		{
			this.SetSelected(false);
		}
		else
		{
			this.SetSelected(true);
		}
	}
}