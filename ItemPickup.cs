using Facepunch;
using System;
using UnityEngine;

[NGCAutoAddScript]
[RequireComponent(typeof(Inventory))]
public class ItemPickup : RigidObj, IContextRequestable, IContextRequestableQuick, IContextRequestableText, IContextRequestablePointText, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	private const string ItemInfoOne_RPC = "PKIS";

	private const string ItemInfo_RPC = "PKIF";

	[NonSerialized]
	private ItemPickup.PickupInfo? info;

	[NonSerialized]
	private ItemPickup.PickupInfo? lastInfo;

	[NonSerialized]
	private string lastString;

	public ItemPickup() : base(RigidObj.FeatureFlags.StreamInitialVelocity)
	{
	}

	public string ContextText(Controllable localControllable)
	{
		if (!base.renderer.enabled)
		{
			return string.Empty;
		}
		if (!this.info.HasValue)
		{
			return "Loading...";
		}
		if (!this.lastInfo.HasValue || !this.lastInfo.Value.Equals(this.info.Value))
		{
			this.lastInfo = this.info;
			this.lastString = string.Format("Take '{0}'", this.info.Value);
		}
		return this.lastString;
	}

	bool IContextRequestablePointText.ContextTextPoint(out Vector3 worldPoint)
	{
		ContextRequestable.PointUtil.SpriteOrOrigin(this, out worldPoint);
		return true;
	}

	protected override void OnDone()
	{
	}

	protected override void OnHide()
	{
		if (base.renderer)
		{
			base.renderer.enabled = false;
		}
	}

	protected override void OnShow()
	{
		if (base.renderer)
		{
			base.renderer.enabled = true;
		}
	}

	[RPC]
	protected void PKIF(int itemName, byte itemAmount)
	{
		this.StoreItemInfo(DatablockDictionary.GetByUniqueID(itemName), (int)itemAmount);
	}

	[RPC]
	protected void PKIS(int itemName)
	{
		this.StoreItemInfo(DatablockDictionary.GetByUniqueID(itemName), 1);
	}

	private void StoreItemInfo(ItemDataBlock datablock, int uses)
	{
		ItemPickup.PickupInfo pickupInfo = new ItemPickup.PickupInfo();
		pickupInfo.datablock = datablock;
		pickupInfo.amount = uses;
		this.info = new ItemPickup.PickupInfo?(pickupInfo);
		pickupInfo.datablock.ConfigureItemPickup(this, uses);
	}

	private struct PickupInfo : IEquatable<ItemPickup.PickupInfo>
	{
		public ItemDataBlock datablock;

		public int amount;

		public bool Equals(ItemPickup.PickupInfo other)
		{
			return (this.datablock != other.datablock ? false : this.amount == other.amount);
		}

		public override bool Equals(object obj)
		{
			return (!(obj is ItemPickup.PickupInfo) ? false : this.Equals((ItemPickup.PickupInfo)obj));
		}

		public override int GetHashCode()
		{
			return (!this.datablock ? this.amount : this.datablock.GetHashCode() ^ this.amount);
		}

		public override string ToString()
		{
			if (!this.datablock)
			{
				if (this.amount <= 1)
				{
					return "null";
				}
				return string.Format("null x{0}", this.amount);
			}
			if (this.amount <= 1 || !this.datablock.IsSplittable())
			{
				return this.datablock.name;
			}
			return string.Format("{0} x{1}", this.datablock.name, this.amount);
		}
	}
}