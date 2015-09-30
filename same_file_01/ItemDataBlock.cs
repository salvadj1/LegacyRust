using Facepunch;
using System;
using uLink;
using UnityEngine;

public class ItemDataBlock : Datablock, IComparable<ItemDataBlock>
{
	public const string kUnknownIconPath = "content/item/tex/unknown";

	public string icon;

	[HideInInspector]
	[NonSerialized]
	public Texture iconTex;

	public int _maxUses = 1;

	public int _spawnUsesMin = 1;

	public int _spawnUsesMax = 1;

	public int _minUsesForDisplay = 1;

	public float _maxCondition = 1f;

	public bool _splittable;

	[HideInInspector]
	public Inventory.SlotFlags _itemFlags;

	public bool isResearchable = true;

	public bool isRepairable = true;

	public bool isRecycleable = true;

	public bool doesLoseCondition;

	public ItemDataBlock.ItemCategory category = ItemDataBlock.ItemCategory.Misc;

	public string itemDescriptionOverride = string.Empty;

	public AudioClip equippedSound;

	public AudioClip unEquippedSound;

	public AudioClip combinedSound;

	public AudioClip UsedSound;

	public ItemDataBlock.CombineRecipe[] Combinations;

	public ItemDataBlock.TransientMode transientMode;

	public bool doesNotSave
	{
		get
		{
			return (this.transientMode & ItemDataBlock.TransientMode.DoesNotSave) == ItemDataBlock.TransientMode.DoesNotSave;
		}
	}

	public bool saves
	{
		get
		{
			return (this.transientMode & ItemDataBlock.TransientMode.DoesNotSave) != ItemDataBlock.TransientMode.DoesNotSave;
		}
	}

	public bool transferable
	{
		get
		{
			return (this.transientMode & ItemDataBlock.TransientMode.Untransferable) != ItemDataBlock.TransientMode.Untransferable;
		}
	}

	public bool untransferable
	{
		get
		{
			return (this.transientMode & ItemDataBlock.TransientMode.Untransferable) == ItemDataBlock.TransientMode.Untransferable;
		}
	}

	public ItemDataBlock()
	{
	}

	public void ConfigureItemPickup(ItemPickup pickup, int amount)
	{
	}

	protected virtual IInventoryItem ConstructItem()
	{
		return new ItemDataBlock.ITEM_TYPE(this);
	}

	public IInventoryItem CreateItem()
	{
		IInventoryItem inventoryItem = this.ConstructItem();
		this.InstallData(inventoryItem);
		return inventoryItem;
	}

	public bool DoesLoseCondition()
	{
		return this.doesLoseCondition;
	}

	public virtual InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
	{
		InventoryItem.MenuItem menuItem = option;
		if (menuItem == InventoryItem.MenuItem.Info)
		{
			RPOS.OpenInfoWindow(this);
			return InventoryItem.MenuItemResult.DoneOnClient;
		}
		if (menuItem != InventoryItem.MenuItem.Split)
		{
			return InventoryItem.MenuItemResult.Unhandled;
		}
		item.inventory.SplitStack(item.slot);
		return InventoryItem.MenuItemResult.Complete;
	}

	public Texture GetIconTexture()
	{
		if (!this.iconTex && !Bundling.Load<Texture>(this.icon, out this.iconTex))
		{
			Bundling.Load<Texture>("content/item/tex/unknown", out this.iconTex);
		}
		return this.iconTex;
	}

	public virtual string GetItemDescription()
	{
		if (this.itemDescriptionOverride.Length <= 0)
		{
			return "No item description available";
		}
		return this.itemDescriptionOverride;
	}

	public ItemDataBlock.CombineRecipe GetMatchingRecipe(ItemDataBlock db)
	{
		if (this.Combinations == null || (int)this.Combinations.Length == 0)
		{
			return null;
		}
		ItemDataBlock.CombineRecipe[] combinations = this.Combinations;
		for (int i = 0; i < (int)combinations.Length; i++)
		{
			ItemDataBlock.CombineRecipe combineRecipe = combinations[i];
			if (combineRecipe.droppedOnType == db)
			{
				return combineRecipe;
			}
		}
		return null;
	}

	public virtual byte GetMaxEligableSlots()
	{
		return (byte)0;
	}

	public int GetMinUsesForDisplay()
	{
		return this._minUsesForDisplay;
	}

	public int GetRandomSpawnUses()
	{
		return UnityEngine.Random.Range(this._spawnUsesMin, this._spawnUsesMax + 1);
	}

	public virtual void InstallData(IInventoryItem item)
	{
		item.SetUses(1);
		item.SetMaxCondition(1f);
		item.SetCondition(1f);
	}

	public virtual bool IsSplittable()
	{
		return this._splittable;
	}

	public static bool LoadIconOrUnknown<TTex>(string iconPath, ref TTex tex)
	where TTex : Texture
	{
		return (tex ? true : ItemDataBlock.LoadIconOrUnknownForced<TTex>(iconPath, out tex));
	}

	public static bool LoadIconOrUnknownForced<TTex>(string iconPath, out TTex tex)
	where TTex : Texture
	{
		if (Bundling.Load<TTex>(iconPath, out tex))
		{
			return true;
		}
		return Bundling.Load<TTex>("content/item/tex/unknown", out tex);
	}

	public virtual void OnItemEvent(InventoryItem.ItemEvent itemEvent)
	{
		switch (itemEvent)
		{
			case InventoryItem.ItemEvent.Equipped:
			{
				if (this.equippedSound)
				{
					this.equippedSound.Play(1f);
				}
				break;
			}
			case InventoryItem.ItemEvent.UnEquipped:
			{
				if (this.unEquippedSound)
				{
					this.unEquippedSound.Play(1f);
				}
				break;
			}
			case InventoryItem.ItemEvent.Combined:
			{
				if (this.combinedSound)
				{
					this.combinedSound.Play(1f);
				}
				break;
			}
			case InventoryItem.ItemEvent.Used:
			{
				if (this.UsedSound)
				{
					this.UsedSound.Play(1f);
				}
				break;
			}
		}
	}

	public virtual void PopulateInfoWindow(ItemToolTip infoWindow, IInventoryItem item)
	{
		infoWindow.AddItemTitle(this, item, 0f);
		infoWindow.AddConditionInfo(item);
		infoWindow.AddItemDescription(this, 15f);
		infoWindow.FinishPopulating();
	}

	protected virtual void PostInstallJsonProperties(IInventoryItem item)
	{
	}

	protected virtual void PreInstallJsonProperties(IInventoryItem item)
	{
	}

	public virtual int RetreiveMenuOptions(IInventoryItem item, InventoryItem.MenuItem[] results, int offset)
	{
		if (this._splittable && item.uses > 1 && item.isInLocalInventory)
		{
			int num = offset;
			offset = num + 1;
			results[num] = InventoryItem.MenuItem.Split;
		}
		return offset;
	}

	protected override void SecureWriteMemberValues(uLink.BitStream stream)
	{
		base.SecureWriteMemberValues(stream);
		stream.Write<int>(this._itemFlags, new object[0]);
		stream.Write<int>(this._maxUses, new object[0]);
		stream.Write<bool>(this._splittable, new object[0]);
		stream.Write<byte>((byte)this.transientMode, new object[0]);
		stream.Write<bool>(this.isResearchable, new object[0]);
		stream.Write<bool>(this.isResearchable, new object[0]);
		stream.Write<bool>(this.isRecycleable, new object[0]);
	}

	int System.IComparable<ItemDataBlock>.CompareTo(ItemDataBlock other)
	{
		return this.CompareTo(other);
	}

	[Serializable]
	public class CombineRecipe
	{
		public ItemDataBlock droppedOnType;

		public ItemDataBlock resultItem;

		public int amountToLose;

		public int amountToLoseOther;

		public int amountToGive;

		public CombineRecipe()
		{
		}
	}

	private sealed class ITEM_TYPE : InventoryItem<ItemDataBlock>, IInventoryItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(ItemDataBlock BLOCK) : base(BLOCK)
		{
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

	[Serializable]
	public enum ItemCategory
	{
		Survival,
		Weapons,
		Ammo,
		Misc,
		Medical,
		Armor,
		Blueprint,
		Food,
		Tools,
		Mods,
		Parts,
		Resource
	}

	public enum TransientMode
	{
		Full,
		DoesNotSave,
		Untransferable,
		None
	}
}