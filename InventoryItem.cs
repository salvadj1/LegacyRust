using InventoryExtensions;
using System;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public abstract class InventoryItem
{
	public const int MAX_SUPPORTED_ITEM_MODS = 5;

	public readonly IInventoryItem iface;

	public readonly int maxUses;

	public readonly int datablockUniqueID;

	protected abstract ItemDataBlock __infrastructure_db
	{
		get;
	}

	public bool active
	{
		get
		{
			Inventory inventory = this.inventory;
			return (!inventory ? false : inventory.activeItem == this);
		}
	}

	public Character character
	{
		get
		{
			Character character;
			Inventory inventory = this.inventory;
			if (!inventory)
			{
				character = null;
			}
			else
			{
				character = inventory.idMain as Character;
			}
			return character;
		}
	}

	public float condition
	{
		get;
		private set;
	}

	public Controllable controllable
	{
		get
		{
			Controllable controllable;
			Inventory inventory = this.inventory;
			if (inventory)
			{
				Character character = inventory.idMain as Character;
				Character character1 = character;
				if (!character)
				{
					controllable = null;
					return controllable;
				}
				controllable = character1.controllable;
				return controllable;
			}
			controllable = null;
			return controllable;
		}
	}

	public Controller controller
	{
		get
		{
			Controller controller;
			Inventory inventory = this.inventory;
			if (inventory)
			{
				Character character = inventory.idMain as Character;
				Character character1 = character;
				if (!character)
				{
					controller = null;
					return controller;
				}
				controller = character1.controller;
				return controller;
			}
			controller = null;
			return controller;
		}
	}

	public ItemDataBlock datablock
	{
		get
		{
			return this.__infrastructure_db;
		}
	}

	public bool dirty
	{
		get
		{
			return (!this.inventory ? false : this.inventory.IsSlotDirty(this.slot));
		}
	}

	public IDMain idMain
	{
		get
		{
			IDMain dMain;
			Inventory inventory = this.inventory;
			if (!inventory)
			{
				dMain = null;
			}
			else
			{
				dMain = inventory.idMain;
			}
			return dMain;
		}
	}

	public Inventory inventory
	{
		get;
		private set;
	}

	public bool isInLocalInventory
	{
		get
		{
			bool flag;
			Inventory inventory = this.inventory;
			if (inventory)
			{
				Character character = inventory.idMain as Character;
				Character character1 = character;
				if (!character)
				{
					flag = false;
					return flag;
				}
				flag = character1.localPlayerControlled;
				return flag;
			}
			flag = false;
			return flag;
		}
	}

	public float lastUseTime
	{
		get;
		set;
	}

	public float maxcondition
	{
		get;
		private set;
	}

	public int slot
	{
		get;
		private set;
	}

	public abstract string toolTip
	{
		get;
	}

	public int uses
	{
		get;
		private set;
	}

	internal InventoryItem(ItemDataBlock datablock)
	{
		this.maxUses = datablock._maxUses;
		this.datablockUniqueID = datablock.uniqueID;
		this.iface = this as IInventoryItem;
	}

	public int AddUses(int count)
	{
		if (count > 0)
		{
			int num = this.uses;
			int num1 = num;
			if (num != this.maxUses)
			{
				int num2 = num1 + count;
				int num3 = num2;
				if (num2 < this.maxUses)
				{
					this.uses = num3;
					this.MarkDirty();
					return count;
				}
				this.uses = this.maxUses;
				this.MarkDirty();
				return this.maxUses - num1;
			}
		}
		return 0;
	}

	public void BreakIntoPieces()
	{
	}

	public virtual bool CanMoveToSlot(Inventory toinv, int toslot)
	{
		return true;
	}

	public virtual void ConditionChanged(float oldCondition)
	{
	}

	public bool Consume(ref int numWant)
	{
		int num = this.uses;
		if (num == 0)
		{
			return true;
		}
		if (numWant == 0)
		{
			return false;
		}
		if (num <= numWant)
		{
			numWant = numWant - num;
			this.uses = 0;
			this.MarkDirty();
			return true;
		}
		this.uses = num - numWant;
		numWant = 0;
		this.MarkDirty();
		return false;
	}

	public void Deserialize(uLink.BitStream stream)
	{
		this.OnBitStreamRead(stream);
	}

	protected static void DeserializeSharedProperties(uLink.BitStream stream, InventoryItem item, ItemDataBlock db)
	{
		item.uses = stream.ReadInvInt();
		if (item.datablock.DoesLoseCondition())
		{
			item.condition = stream.ReadSingle();
			item.maxcondition = stream.ReadSingle();
		}
	}

	public float GetConditionForBreak()
	{
		return 0f;
	}

	public float GetConditionPercent()
	{
		return this.condition / this.maxcondition;
	}

	public virtual string GetConditionString()
	{
		if (!this.datablock.doesLoseCondition)
		{
			return string.Empty;
		}
		if (this.condition > 1f)
		{
			return "Artifact";
		}
		if (this.condition >= 0.8f)
		{
			return "Perfect";
		}
		if (this.condition >= 0.6f)
		{
			return "Quality";
		}
		if (this.condition >= 0.5f)
		{
			return string.Empty;
		}
		if (this.condition >= 0.4f)
		{
			return "Shoddy";
		}
		if ((double)this.condition > 0)
		{
			return "Bad";
		}
		if (this.IsBroken())
		{
			return "Broken";
		}
		return "ERROR";
	}

	public bool IsBroken()
	{
		return this.condition <= this.GetConditionForBreak();
	}

	public bool IsDamaged()
	{
		return this.maxcondition - this.condition > 0.001f;
	}

	public bool MarkDirty()
	{
		Inventory inventory = this.inventory;
		if (!inventory)
		{
			return false;
		}
		return inventory.MarkSlotDirty(this.slot);
	}

	public virtual void MaxConditionChanged(float oldCondition)
	{
	}

	public virtual void OnAddedTo(Inventory inv, int slot)
	{
		this.inventory = inv;
		this.slot = slot;
	}

	protected abstract void OnBitStreamRead(uLink.BitStream stream);

	protected abstract void OnBitStreamWrite(uLink.BitStream stream);

	public abstract InventoryItem.MenuItemResult OnMenuOption(InventoryItem.MenuItem option);

	public abstract void OnMovedTo(Inventory inv, int slot);

	public void Serialize(uLink.BitStream stream)
	{
		this.OnBitStreamWrite(stream);
	}

	protected static void SerializeSharedProperties(uLink.BitStream stream, InventoryItem item, ItemDataBlock db)
	{
		stream.WriteInvInt(item.uses);
		if (item.datablock.DoesLoseCondition())
		{
			stream.WriteSingle(item.condition);
			stream.WriteSingle(item.maxcondition);
		}
	}

	public void SetCondition(float newcondition)
	{
		float single = this.condition;
		this.condition = Mathf.Clamp(newcondition, 0f, this.maxcondition);
		this.ConditionChanged(single);
		this.MarkDirty();
	}

	public void SetMaxCondition(float newmaxcondition)
	{
		float single = this.maxcondition;
		this.maxcondition = Mathf.Clamp(newmaxcondition, 0.01f, 1f);
		this.MaxConditionChanged(single);
		this.MarkDirty();
	}

	public void SetUses(int count)
	{
		int num = this.uses;
		if (count < 0 || count > this.maxUses)
		{
			count = this.maxUses;
		}
		if (count != num)
		{
			this.uses = count;
			this.MarkDirty();
		}
	}

	public abstract InventoryItem.MergeResult TryCombine(IInventoryItem other);

	public bool TryConditionLoss(float probability, float percentLoss)
	{
		return false;
	}

	public abstract InventoryItem.MergeResult TryStack(IInventoryItem other);

	public enum ItemEvent
	{
		None,
		Equipped,
		UnEquipped,
		Combined,
		Used
	}

	public enum MenuItem : byte
	{
		Info = 1,
		Status = 2,
		Use = 3,
		Study = 4,
		Split = 5,
		Eat = 6,
		Drink = 7,
		Consume = 8,
		Unload = 9
	}

	public enum MenuItemResult : byte
	{
		Unhandled = 0,
		DoneOnServer = 1,
		DoneOnServerNotYetClient = 2,
		DoneOnClient = 3,
		Complete = 4
	}

	public enum MergeResult
	{
		Failed,
		Merged,
		Combined
	}
}