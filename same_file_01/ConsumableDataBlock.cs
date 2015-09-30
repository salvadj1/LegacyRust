using System;
using uLink;
using UnityEngine;

public class ConsumableDataBlock : ItemDataBlock
{
	public float litresOfWater;

	public float calories;

	public float antiRads;

	public float healthToHeal;

	public float poisonAmount;

	public bool cookable;

	public int numToCookPerTick;

	public ItemDataBlock cookedVersion;

	public int burnTemp = 10;

	public int cookHeatRequirement = 1;

	public ConsumableDataBlock()
	{
	}

	protected override IInventoryItem ConstructItem()
	{
		return new ConsumableDataBlock.ITEM_TYPE(this);
	}

	public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
	{
		if (option == this.GetConsumeMenuItem())
		{
			return InventoryItem.MenuItemResult.DoneOnServer;
		}
		return base.ExecuteMenuOption(option, item);
	}

	public InventoryItem.MenuItem GetConsumeMenuItem()
	{
		if (this.calories > 0f && this.litresOfWater <= 0f)
		{
			return InventoryItem.MenuItem.Eat;
		}
		if (this.litresOfWater > 0f && this.calories <= 0f)
		{
			return InventoryItem.MenuItem.Drink;
		}
		return InventoryItem.MenuItem.Consume;
	}

	public override string GetItemDescription()
	{
		string empty = string.Empty;
		if (this.calories > 0f && this.litresOfWater > 0f)
		{
			empty = string.Concat(empty, "This is a food item, consuming it (via right click) will replenish your food and water. ");
		}
		else if (this.calories > 0f)
		{
			empty = string.Concat(empty, "This is a food item, eating it will satisfy some of your hunger. ");
		}
		else if (this.litresOfWater > 0f)
		{
			empty = string.Concat(empty, "This is a beverage, drinking it will quench some of your thirst. ");
		}
		if (this.antiRads > 0f)
		{
			empty = string.Concat(empty, "This item has some anti-radioactive properties, consuming it will lower your radiation level. ");
		}
		if (this.healthToHeal > 0f)
		{
			empty = string.Concat(empty, "It will also provide minor healing");
		}
		return empty;
	}

	public override void PopulateInfoWindow(ItemToolTip infoWindow, IInventoryItem tipItem)
	{
		infoWindow.AddItemTitle(this, tipItem, 0f);
		infoWindow.AddSectionTitle("Consumable", 15f);
		if (this.calories > 0f)
		{
			infoWindow.AddBasicLabel(string.Concat(this.calories, " Calories"), 15f);
		}
		if (this.litresOfWater > 0f)
		{
			infoWindow.AddBasicLabel(string.Concat(this.litresOfWater, "L Water"), 15f);
		}
		if (this.antiRads > 0f)
		{
			infoWindow.AddBasicLabel(string.Concat("-", this.antiRads, " Rads"), 15f);
		}
		if (this.healthToHeal != 0f)
		{
			infoWindow.AddBasicLabel(string.Concat((this.healthToHeal <= 0f ? string.Empty : "+"), this.healthToHeal, " Health"), 15f);
		}
		infoWindow.AddItemDescription(this, 15f);
		infoWindow.FinishPopulating();
	}

	public override int RetreiveMenuOptions(IInventoryItem item, InventoryItem.MenuItem[] results, int offset)
	{
		offset = base.RetreiveMenuOptions(item, results, offset);
		if (item.isInLocalInventory)
		{
			int num = offset;
			offset = num + 1;
			results[num] = this.GetConsumeMenuItem();
		}
		return offset;
	}

	public virtual void UseItem(IConsumableItem item)
	{
		Inventory inventory = item.inventory;
		Metabolism local = inventory.GetLocal<Metabolism>();
		if (local == null)
		{
			return;
		}
		if (!local.CanConsumeYet())
		{
			return;
		}
		local.MarkConsumptionTime();
		float single = Mathf.Min(local.GetRemainingCaloricSpace(), this.calories);
		if (this.calories > 0f)
		{
			local.AddCalories(single);
		}
		if (this.litresOfWater > 0f)
		{
			local.AddWater(this.litresOfWater);
		}
		if (this.antiRads > 0f)
		{
			local.AddAntiRad(this.antiRads);
		}
		if (this.healthToHeal != 0f)
		{
			HumanBodyTakeDamage humanBodyTakeDamage = inventory.GetLocal<HumanBodyTakeDamage>();
			if (humanBodyTakeDamage != null)
			{
				if (this.healthToHeal <= 0f)
				{
					TakeDamage.HurtSelf(inventory.idMain, Mathf.Abs(this.healthToHeal), null);
				}
				else
				{
					humanBodyTakeDamage.HealOverTime(this.healthToHeal);
				}
			}
		}
		if (this.poisonAmount > 0f)
		{
			local.AddPoison(this.poisonAmount);
		}
		int num = 1;
		if (item.Consume(ref num))
		{
			inventory.RemoveItem(item.slot);
		}
	}

	private sealed class ITEM_TYPE : ConsumableItem<ConsumableDataBlock>, IConsumableItem, ICookableItem, IInventoryItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(ConsumableDataBlock BLOCK) : base(BLOCK)
		{
		}

		// privatescope
		internal bool ICookableItem.GetCookableInfo(out int consumeCount, out ItemDataBlock cookedVersion, out int cookedCount, out int cookTempMin, out int burnTemp)
		{
			return base.GetCookableInfo(out consumeCount, out cookedVersion, out cookedCount, out cookTempMin, out burnTemp);
		}

		// privatescope
		internal int IInventoryItem.AddUses(int count)
		{
			return base.AddUses(count);
		}

		// privatescope
		internal bool IInventoryItem.Consume(ref int count)
		{
			return base.Consume(ref count);
		}

		// privatescope
		internal void IInventoryItem.Deserialize(uLink.BitStream stream)
		{
			base.Deserialize(stream);
		}

		// privatescope
		internal bool IInventoryItem.get_active()
		{
			return base.active;
		}

		// privatescope
		internal Character IInventoryItem.get_character()
		{
			return base.character;
		}

		// privatescope
		internal float IInventoryItem.get_condition()
		{
			return base.condition;
		}

		// privatescope
		internal Controllable IInventoryItem.get_controllable()
		{
			return base.controllable;
		}

		// privatescope
		internal Controller IInventoryItem.get_controller()
		{
			return base.controller;
		}

		// privatescope
		internal bool IInventoryItem.get_dirty()
		{
			return base.dirty;
		}

		// privatescope
		internal bool IInventoryItem.get_doNotSave()
		{
			return base.doNotSave;
		}

		// privatescope
		internal IDMain IInventoryItem.get_idMain()
		{
			return base.idMain;
		}

		// privatescope
		internal Inventory IInventoryItem.get_inventory()
		{
			return base.inventory;
		}

		// privatescope
		internal bool IInventoryItem.get_isInLocalInventory()
		{
			return base.isInLocalInventory;
		}

		// privatescope
		internal float IInventoryItem.get_lastUseTime()
		{
			return base.lastUseTime;
		}

		// privatescope
		internal float IInventoryItem.get_maxcondition()
		{
			return base.maxcondition;
		}

		// privatescope
		internal int IInventoryItem.get_slot()
		{
			return base.slot;
		}

		// privatescope
		internal int IInventoryItem.get_uses()
		{
			return base.uses;
		}

		// privatescope
		internal float IInventoryItem.GetConditionPercent()
		{
			return base.GetConditionPercent();
		}

		// privatescope
		internal bool IInventoryItem.IsBroken()
		{
			return base.IsBroken();
		}

		// privatescope
		internal bool IInventoryItem.IsDamaged()
		{
			return base.IsDamaged();
		}

		// privatescope
		internal bool IInventoryItem.MarkDirty()
		{
			return base.MarkDirty();
		}

		// privatescope
		internal void IInventoryItem.Serialize(uLink.BitStream stream)
		{
			base.Serialize(stream);
		}

		// privatescope
		internal void IInventoryItem.set_lastUseTime(float value)
		{
			base.lastUseTime = value;
		}

		// privatescope
		internal void IInventoryItem.SetCondition(float condition)
		{
			base.SetCondition(condition);
		}

		// privatescope
		internal void IInventoryItem.SetMaxCondition(float condition)
		{
			base.SetMaxCondition(condition);
		}

		// privatescope
		internal void IInventoryItem.SetUses(int count)
		{
			base.SetUses(count);
		}

		// privatescope
		internal bool IInventoryItem.TryConditionLoss(float probability, float percentLoss)
		{
			return base.TryConditionLoss(probability, percentLoss);
		}
	}
}