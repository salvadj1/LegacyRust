using InventoryExtensions;
using System;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public abstract class HeldItem<T> : InventoryItem<T>
where T : HeldItemDataBlock
{
	protected ItemModDataBlock[] _itemMods;

	private ViewModel _vm;

	private ItemRepresentation _itemRep;

	public bool canActivate
	{
		get
		{
			return this.CanSetActivate(true);
		}
	}

	public bool canAim
	{
		get
		{
			return this.CanAim();
		}
	}

	public bool canDeactivate
	{
		get
		{
			return this.CanSetActivate(false);
		}
	}

	public int freeModSlots
	{
		get
		{
			return this.totalModSlots - this.usedModSlots;
		}
	}

	public ItemModDataBlock[] itemMods
	{
		get
		{
			return this._itemMods;
		}
	}

	public ItemRepresentation itemRepresentation
	{
		get
		{
			return this._itemRep;
		}
		set
		{
			this.SetItemRepresentation(value);
		}
	}

	public ItemModFlags modFlags
	{
		get
		{
			ItemModFlags itemModFlag = ItemModFlags.Other;
			if (this._itemMods != null)
			{
				ItemModDataBlock[] itemModDataBlockArray = this._itemMods;
				for (int i = 0; i < (int)itemModDataBlockArray.Length; i++)
				{
					ItemModDataBlock itemModDataBlock = itemModDataBlockArray[i];
					if (itemModDataBlock != null)
					{
						itemModFlag = itemModFlag | itemModDataBlock.modFlag;
					}
				}
			}
			return itemModFlag;
		}
	}

	public int totalModSlots
	{
		get;
		private set;
	}

	public int usedModSlots
	{
		get;
		private set;
	}

	public ViewModel viewModelInstance
	{
		get
		{
			return this._vm;
		}
		protected set
		{
			this._vm = value;
		}
	}

	public HeldItem(T datablock) : base(datablock)
	{
	}

	public void AddMod(ItemModDataBlock mod)
	{
		this.RecalculateMods();
		int num = this.usedModSlots;
		this._itemMods[num] = mod;
		this.RecalculateMods();
		this.OnModAdded(mod);
		base.MarkDirty();
	}

	protected virtual bool CanAim()
	{
		return true;
	}

	protected virtual bool CanSetActivate(bool value)
	{
		if (value && base.IsBroken())
		{
			return false;
		}
		return true;
	}

	public override void ConditionChanged(float oldCondition)
	{
	}

	protected virtual void CreateViewModel()
	{
		this.DestroyViewModel();
		if ((T)this.datablock._viewModelPrefab == null || actor.forceThirdPerson)
		{
			return;
		}
		this._vm = (ViewModel)UnityEngine.Object.Instantiate((T)this.datablock._viewModelPrefab);
		this._vm.PlayDeployAnimation();
		if ((T)this.datablock.deploySound)
		{
			(T)this.datablock.deploySound.Play(1f);
		}
		CameraFX.ReplaceViewModel(this._vm, this._itemRep, this.iface as IHeldItem, false);
	}

	protected virtual void DestroyViewModel()
	{
		if (this._vm)
		{
			CameraFX.RemoveViewModel(ref this._vm, true, false);
		}
	}

	public int FindMod(ItemModDataBlock mod)
	{
		if (mod)
		{
			for (int i = 0; i < 5; i++)
			{
				if (this._itemMods[i] == mod)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public virtual void ItemPostFrame(ref HumanController.InputSample sample)
	{
	}

	public virtual void ItemPreFrame(ref HumanController.InputSample sample)
	{
		if (sample.attack2 && (T)this.datablock.secondaryFireAims && this.CanAim())
		{
			sample.attack2 = false;
			sample.aim = true;
			sample.yaw = sample.yaw * (T)this.datablock.aimSensitivtyPercent;
			sample.pitch = sample.pitch * (T)this.datablock.aimSensitivtyPercent;
		}
	}

	public void OnActivate()
	{
		this.OnSetActive(true);
	}

	protected override void OnBitStreamRead(uLink.BitStream stream)
	{
		base.OnBitStreamRead(stream);
		this.SetTotalModSlotCount(stream.ReadInvInt());
		this.SetUsedModSlotCount(stream.ReadInvInt());
		int num = this.usedModSlots;
		for (int i = 0; i < 5; i++)
		{
			if (i >= num)
			{
				this._itemMods[i] = null;
			}
			else
			{
				this._itemMods[i] = DatablockDictionary.GetByUniqueID(stream.ReadInt32()) as ItemModDataBlock;
			}
		}
	}

	protected override void OnBitStreamWrite(uLink.BitStream stream)
	{
		base.OnBitStreamWrite(stream);
		stream.WriteInvInt(this.totalModSlots);
		int num = this.usedModSlots;
		stream.WriteInvInt(num);
		for (int i = 0; i < num; i++)
		{
			stream.WriteInt32(this._itemMods[i].uniqueID);
		}
	}

	public void OnDeactivate()
	{
		this.OnSetActive(false);
	}

	protected virtual void OnModAdded(ItemModDataBlock mod)
	{
	}

	public override void OnMovedTo(Inventory toInv, int toSlot)
	{
		if (base.active)
		{
			base.inventory.DeactivateItem();
		}
	}

	protected virtual void OnSetActive(bool isActive)
	{
		if (!isActive)
		{
			this.DestroyViewModel();
		}
		else
		{
			this.CreateViewModel();
		}
	}

	public virtual void PreCameraRender()
	{
	}

	private void RecalculateMods()
	{
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			if (this._itemMods[i] != null)
			{
				num++;
			}
		}
		this.usedModSlots = num;
	}

	protected virtual void SetItemRepresentation(ItemRepresentation itemRep)
	{
		this._itemRep = itemRep;
		if (this._itemRep)
		{
			if (this._itemRep.datablock != (T)this.datablock)
			{
				Debug.Log("yea the code below wasn't pointless..");
				this._itemRep.SetDataBlockFromHeldItem<T>(this);
			}
			this._itemRep.SetParent(base.inventory.gameObject);
		}
	}

	public void SetTotalModSlotCount(int count)
	{
		this.totalModSlots = count;
	}

	public void SetUsedModSlotCount(int count)
	{
		this.usedModSlots = count;
	}
}