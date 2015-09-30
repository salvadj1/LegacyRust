using System;
using uLink;
using UnityEngine;

public class RPOSRepairBenchWindow : RPOSLootWindow
{
	private RepairBench _bench;

	private IInventoryItem _benchItem;

	public UILabel[] _amountLabels;

	public UIButton repairButton;

	public UILabel conditionLabel;

	public UILabel needsLabel;

	public RPOSRepairBenchWindow()
	{
	}

	public void ClearRepairItem()
	{
		this._benchItem = null;
		this.UpdateGUIAmounts();
	}

	private void RepairButtonClicked(GameObject go)
	{
		if (this._benchItem != null)
		{
			NetCull.RPC(this._bench, "DoRepair", uLink.RPCMode.Server);
		}
	}

	public override void SetLootable(LootableObject lootable, bool doInit)
	{
		base.SetLootable(lootable, doInit);
		this._bench = lootable.GetComponent<RepairBench>();
	}

	public void SetRepairItem(IInventoryItem item)
	{
		if (item == null || !item.datablock.isRepairable)
		{
			this.ClearRepairItem();
			return;
		}
		this._benchItem = item;
		this.UpdateGUIAmounts();
	}

	public void Update()
	{
		IInventoryItem inventoryItem = null;
		if (this._bench)
		{
			this._bench.GetComponent<Inventory>().GetItem(0, out inventoryItem);
		}
		this.SetRepairItem(inventoryItem);
	}

	public void UpdateGUIAmounts()
	{
		BlueprintDataBlock blueprintDataBlock;
		if (this._benchItem != null)
		{
			Controllable localPlayer = PlayerClient.GetLocalPlayer().controllable;
			if (localPlayer == null)
			{
				return;
			}
			Inventory component = localPlayer.GetComponent<Inventory>();
			int num = 0;
			if (!this._benchItem.IsDamaged())
			{
				this.needsLabel.text = "Does not need repairs";
				this.needsLabel.color = Color.green;
				this.needsLabel.enabled = true;
				float single = this._benchItem.condition * 100f;
				string str = single.ToString("0");
				float single1 = this._benchItem.maxcondition * 100f;
				string str1 = single1.ToString("0");
				this.conditionLabel.text = string.Concat("Condition : ", str, "/", str1);
				this.conditionLabel.color = Color.green;
				this.conditionLabel.enabled = true;
				this.repairButton.gameObject.SetActive(false);
				UILabel[] uILabelArray = this._amountLabels;
				for (int i = 0; i < (int)uILabelArray.Length; i++)
				{
					UILabel empty = uILabelArray[i];
					empty.text = string.Empty;
					empty.color = Color.white;
				}
			}
			else
			{
				if (BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(this._benchItem.datablock, out blueprintDataBlock))
				{
					int num1 = 0;
					while (num1 < (int)blueprintDataBlock.ingredients.Length)
					{
						if (num < (int)this._amountLabels.Length)
						{
							BlueprintDataBlock.IngredientEntry ingredientEntry = blueprintDataBlock.ingredients[num1];
							int num2 = Mathf.CeilToInt((float)blueprintDataBlock.ingredients[num1].amount * this._bench.GetResourceScalar());
							if (num2 > 0)
							{
								bool flag = component.CanConsume(blueprintDataBlock.ingredients[num1].Ingredient, num2) > 0;
								this._amountLabels[num].text = string.Concat(num2, " ", blueprintDataBlock.ingredients[num1].Ingredient.name);
								this._amountLabels[num].color = (!flag ? Color.red : Color.green);
								num++;
							}
							num1++;
						}
						else
						{
							break;
						}
					}
				}
				this.needsLabel.color = Color.white;
				this.needsLabel.enabled = true;
				this.conditionLabel.enabled = true;
				this.repairButton.gameObject.SetActive(true);
				float single2 = this._benchItem.condition * 100f;
				string str2 = single2.ToString("0");
				float single3 = this._benchItem.maxcondition * 100f;
				string str3 = single3.ToString("0");
				this.conditionLabel.text = string.Concat("Condition : ", str2, "/", str3);
				this.conditionLabel.color = (this._benchItem.condition >= 0.6f ? Color.green : Color.yellow);
				if (this._benchItem.IsBroken())
				{
					this.conditionLabel.color = Color.red;
				}
			}
		}
		else
		{
			UILabel[] uILabelArray1 = this._amountLabels;
			for (int j = 0; j < (int)uILabelArray1.Length; j++)
			{
				UILabel uILabel = uILabelArray1[j];
				uILabel.text = string.Empty;
				uILabel.color = Color.white;
			}
			this.needsLabel.enabled = false;
			this.conditionLabel.enabled = false;
			this.repairButton.gameObject.SetActive(false);
		}
	}

	protected override void WindowAwake()
	{
		base.WindowAwake();
		UIEventListener.Get(this.repairButton.gameObject).onClick += new UIEventListener.VoidDelegate(this.RepairButtonClicked);
		this.ClearRepairItem();
	}
}