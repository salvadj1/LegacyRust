using System;
using uLink;
using UnityEngine;

public class ArmorDataBlock : EquipmentDataBlock
{
	public DamageTypeList armorValues;

	[SerializeField]
	protected ArmorModel armorModel;

	public ArmorDataBlock()
	{
	}

	public void AddToDamageTypeList(DamageTypeList damageList)
	{
		for (int i = 0; i < 6; i++)
		{
			DamageTypeList item = damageList;
			DamageTypeList damageTypeList = item;
			int num = i;
			float single = damageTypeList[num];
			item[num] = single + this.armorValues[i];
		}
	}

	protected override IInventoryItem ConstructItem()
	{
		return new ArmorDataBlock.ITEM_TYPE(this);
	}

	public TArmorModel GetArmorModel<TArmorModel>()
	where TArmorModel : ArmorModel, new()
	{
		return (TArmorModel)this.GetArmorModel(ArmorModelSlotUtility.GetArmorModelSlotForClass<TArmorModel>());
	}

	public ArmorModel GetArmorModel(ArmorModelSlot slot)
	{
		if (!this.armorModel)
		{
			Debug.LogWarning(string.Concat("No armorModel set to datablock ", this), this);
			return null;
		}
		if (this.armorModel.slot == slot)
		{
			return this.armorModel;
		}
		Debug.LogError(string.Format("The armor model for {0} is {1}. Its not for slot {2}", this, this.armorModel.slot, slot), this);
		return null;
	}

	public bool GetArmorModelSlot(out ArmorModelSlot slot)
	{
		if (this.armorModel)
		{
			slot = this.armorModel.slot;
		}
		else
		{
			slot = (ArmorModelSlot)4;
		}
		return (byte)slot < 4;
	}

	public override string GetItemDescription()
	{
		return "This is an piece of armor. Drag it to it's corresponding slot in the armor window and it will provide additional protection";
	}

	public override void OnEquipped(IEquipmentItem item)
	{
	}

	public override void OnUnEquipped(IEquipmentItem item)
	{
	}

	public override void PopulateInfoWindow(ItemToolTip infoWindow, IInventoryItem tipItem)
	{
		infoWindow.AddItemTitle(this, tipItem, 0f);
		infoWindow.AddConditionInfo(tipItem);
		infoWindow.AddSectionTitle("Protection", 0f);
		for (int i = 0; i < 6; i++)
		{
			if (this.armorValues[i] != 0f)
			{
				float contentHeight = infoWindow.GetContentHeight();
				GameObject gameObject = infoWindow.AddBasicLabel(TakeDamage.DamageIndexToString((DamageTypeIndex)i), 0f);
				int item = (int)this.armorValues[i];
				GameObject gameObject1 = infoWindow.AddBasicLabel(string.Concat("+", item.ToString("N0")), 0f);
				gameObject1.transform.SetLocalPositionX(145f);
				gameObject1.GetComponentInChildren<UILabel>().color = Color.green;
				gameObject.transform.SetLocalPositionY(-(contentHeight + 10f));
				gameObject1.transform.SetLocalPositionY(-(contentHeight + 10f));
			}
		}
		infoWindow.AddSectionTitle("Equipment Slot", 20f);
		string str = "Head, Chest, Legs, Feet";
		if ((this._itemFlags & Inventory.SlotFlags.Head) == Inventory.SlotFlags.Head)
		{
			str = "Head";
		}
		else if ((this._itemFlags & Inventory.SlotFlags.Chest) == Inventory.SlotFlags.Chest)
		{
			str = "Chest";
		}
		infoWindow.AddBasicLabel(str, 10f);
		infoWindow.AddItemDescription(this, 15f);
		infoWindow.FinishPopulating();
	}

	private sealed class ITEM_TYPE : ArmorItem<ArmorDataBlock>, IArmorItem, IEquipmentItem, IInventoryItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(ArmorDataBlock BLOCK) : base(BLOCK)
		{
		}

		// privatescope
		internal void IEquipmentItem.OnEquipped()
		{
			base.OnEquipped();
		}

		// privatescope
		internal void IEquipmentItem.OnUnEquipped()
		{
			base.OnUnEquipped();
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