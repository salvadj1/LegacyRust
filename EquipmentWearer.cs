using System;
using uLink;
using UnityEngine;

public class EquipmentWearer : IDLocalCharacter
{
	[NonSerialized]
	private CacheRef<ArmorModelRenderer> _armorModelRenderer;

	[NonSerialized]
	private CacheRef<ProtectionTakeDamage> _protectionTakeDamage;

	[NonSerialized]
	private CacheRef<InventoryHolder> _inventoryHolder;

	public ArmorModelRenderer armorModelRenderer
	{
		get
		{
			if (!this._armorModelRenderer.cached)
			{
				this._armorModelRenderer = base.GetLocal<ArmorModelRenderer>();
			}
			return this._armorModelRenderer.@value;
		}
	}

	public InventoryHolder inventoryHolder
	{
		get
		{
			if (!this._inventoryHolder.cached)
			{
				this._inventoryHolder = base.GetLocal<InventoryHolder>();
			}
			return this._inventoryHolder.@value;
		}
	}

	public new ProtectionTakeDamage takeDamage
	{
		get
		{
			if (!this._protectionTakeDamage.cached)
			{
				this._protectionTakeDamage = base.takeDamage as ProtectionTakeDamage;
			}
			return this._protectionTakeDamage.@value;
		}
	}

	public EquipmentWearer()
	{
	}

	[RPC]
	protected void ArmorData(byte[] data)
	{
		DamageTypeList damageTypeList = new DamageTypeList();
		uLink.BitStream bitStream = new uLink.BitStream(data, false);
		for (int i = 0; i < 6; i++)
		{
			damageTypeList[i] = bitStream.ReadSingle();
		}
		ProtectionTakeDamage protectionTakeDamage = this.takeDamage;
		if (protectionTakeDamage)
		{
			protectionTakeDamage.SetArmorValues(damageTypeList);
		}
		if (base.localPlayerControlled)
		{
			RPOS.SetEquipmentDirty();
		}
	}

	public void CalculateArmor()
	{
		IInventoryItem inventoryItem;
		InventoryHolder inventoryHolder = this.inventoryHolder;
		ProtectionTakeDamage protectionTakeDamage = this.takeDamage;
		if (inventoryHolder && protectionTakeDamage)
		{
			DamageTypeList damageTypeList = new DamageTypeList();
			for (int i = 36; i < 40; i++)
			{
				if (inventoryHolder.inventory.GetItem(i, out inventoryItem))
				{
					ArmorDataBlock armorDataBlock = inventoryItem.datablock as ArmorDataBlock;
					ArmorDataBlock armorDataBlock1 = armorDataBlock;
					if (armorDataBlock)
					{
						armorDataBlock1.AddToDamageTypeList(damageTypeList);
					}
				}
			}
			if (protectionTakeDamage)
			{
				protectionTakeDamage.SetArmorValues(damageTypeList);
			}
		}
	}

	public void EquipmentUpdate()
	{
		this.CalculateArmor();
	}
}