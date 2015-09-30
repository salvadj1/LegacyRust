using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPOS_Craft_BlueprintList : MonoBehaviour
{
	public GameObject CategoryHeaderPrefab;

	public GameObject ItemPlaquePrefab;

	public RPOSCraftWindow craftWindow;

	private int lastNumBoundBPs;

	public RPOS_Craft_BlueprintList()
	{
	}

	public void AddItemCategoryHeader(ItemDataBlock.ItemCategory category)
	{
	}

	public int AddItemsOfCategory(ItemDataBlock.ItemCategory category, List<BlueprintDataBlock> checkList, int yPos)
	{
		if (!this.AnyOfCategoryInList(category, checkList))
		{
			return yPos;
		}
		GameObject vector3 = NGUITools.AddChild(base.gameObject, this.CategoryHeaderPrefab);
		vector3.transform.localPosition = new Vector3(0f, (float)yPos, -1f);
		vector3.GetComponentInChildren<UILabel>().text = category.ToString();
		yPos = yPos - 16;
		foreach (BlueprintDataBlock blueprintDataBlock in checkList)
		{
			if (blueprintDataBlock.resultItem.category == category)
			{
				GameObject gameObject = NGUITools.AddChild(base.gameObject, this.ItemPlaquePrefab);
				gameObject.GetComponentInChildren<UILabel>().text = blueprintDataBlock.resultItem.name;
				gameObject.transform.localPosition = new Vector3(10f, (float)yPos, -1f);
				UIEventListener.Get(gameObject).onClick += new UIEventListener.VoidDelegate(this.craftWindow.ItemClicked);
				gameObject.GetComponent<RPOSCraftItemEntry>().actualItemDataBlock = blueprintDataBlock.resultItem;
				gameObject.GetComponent<RPOSCraftItemEntry>().blueprint = blueprintDataBlock;
				gameObject.GetComponent<RPOSCraftItemEntry>().craftWindow = this.craftWindow;
				gameObject.GetComponent<RPOSCraftItemEntry>().SetSelected(false);
				yPos = yPos - 16;
			}
		}
		return yPos;
	}

	public bool AnyOfCategoryInList(ItemDataBlock.ItemCategory category, List<BlueprintDataBlock> checkList)
	{
		bool flag;
		List<BlueprintDataBlock>.Enumerator enumerator = checkList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				BlueprintDataBlock current = enumerator.Current;
				if (current != null)
				{
					if (current.resultItem.category != category)
					{
						continue;
					}
					flag = true;
					return flag;
				}
				else
				{
					Debug.Log("WTFFFF");
					flag = false;
					return flag;
				}
			}
			return false;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return flag;
	}

	private void Awake()
	{
	}

	public RPOSCraftItemEntry GetEntryByBP(BlueprintDataBlock bp)
	{
		RPOSCraftItemEntry rPOSCraftItemEntry;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				RPOSCraftItemEntry component = (enumerator.Current as Transform).GetComponent<RPOSCraftItemEntry>();
				if (!component || !(component.blueprint == bp))
				{
					continue;
				}
				rPOSCraftItemEntry = component;
				return rPOSCraftItemEntry;
			}
			return null;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		return rPOSCraftItemEntry;
	}

	public void UpdateItems()
	{
		List<BlueprintDataBlock> boundBPs = RPOS.ObservedPlayer.GetComponent<PlayerInventory>().GetBoundBPs();
		int count = boundBPs.Count;
		if (boundBPs == null)
		{
			Debug.Log("BOUND BP LIST EMPTY!!!!!");
			return;
		}
		if (this.lastNumBoundBPs == count)
		{
			return;
		}
		this.lastNumBoundBPs = count;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UnityEngine.Object.Destroy((enumerator.Current as Transform).gameObject);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		int num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Survival, boundBPs, 0);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Resource, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Medical, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Ammo, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Weapons, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Armor, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Tools, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Mods, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Parts, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Food, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Blueprint, boundBPs, num);
		num = this.AddItemsOfCategory(ItemDataBlock.ItemCategory.Misc, boundBPs, num);
		base.GetComponent<UIDraggablePanel>().calculateNextChange = true;
	}
}