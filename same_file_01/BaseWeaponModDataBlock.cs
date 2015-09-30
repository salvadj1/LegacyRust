using System;
using uLink;
using UnityEngine;

public class BaseWeaponModDataBlock : ItemModDataBlock
{
	public string attachSocketName = "muzzle";

	public GameObject attachObjectVM;

	public GameObject attachObjectRep;

	public bool isMesh;

	public string socketOverrideName = string.Empty;

	public float punchScalar = 1f;

	public float zoomOffsetZ;

	public bool modifyZoomOffset;

	protected BaseWeaponModDataBlock(Type minimumItemModRepresentationType) : base(minimumItemModRepresentationType)
	{
		if (!typeof(WeaponModRep).IsAssignableFrom(minimumItemModRepresentationType))
		{
			throw new ArgumentOutOfRangeException("minimumItemModRepresentationType", minimumItemModRepresentationType, "!typeof(WeaponModRep).IsAssignableFrom(minimumItemModRepresentationType)");
		}
	}

	public BaseWeaponModDataBlock() : this(typeof(WeaponModRep))
	{
	}

	protected override IInventoryItem ConstructItem()
	{
		return new BaseWeaponModDataBlock.ITEM_TYPE(this);
	}

	public Socket GetSocketByName(Socket.Mapped vm, string name)
	{
		return vm.socketMap[name].socket;
	}

	protected override void InstallToItemModRepresentation(ItemModRepresentation modRep)
	{
		base.InstallToItemModRepresentation(modRep);
		if (this.attachObjectRep != null)
		{
			GameObject gameObject = modRep.itemRep.muzzle.InstantiateAsChild(this.attachObjectRep, false);
			gameObject.name = this.attachObjectRep.name;
			((WeaponModRep)modRep).SetAttached(gameObject, false);
		}
	}

	protected override bool InstallToViewModel(ref ModViewModelAddArgs a)
	{
		GameObject gameObject;
		if (this.isMesh && !a.isMesh)
		{
			return base.InstallToViewModel(ref a);
		}
		if (!this.isMesh && a.isMesh)
		{
			return base.InstallToViewModel(ref a);
		}
		if (a.vm == null)
		{
			Debug.Log("Viewmodel null for item attachment...");
		}
		if (this.attachObjectVM != null)
		{
			WeaponModRep weaponModRep = (WeaponModRep)a.modRep;
			if (!a.isMesh)
			{
				gameObject = this.GetSocketByName(a.vm, this.attachSocketName).InstantiateAsChild(this.attachObjectVM, true);
			}
			else
			{
				Socket socketByName = this.GetSocketByName(a.vm, this.attachSocketName);
				gameObject = UnityEngine.Object.Instantiate(this.attachObjectVM, socketByName.offset, Quaternion.Euler(socketByName.eulerRotate)) as GameObject;
				gameObject.transform.parent = socketByName.parent;
				gameObject.transform.localPosition = socketByName.offset;
				gameObject.transform.localEulerAngles = socketByName.eulerRotate;
			}
			gameObject.name = this.attachObjectVM.name;
			weaponModRep.SetAttached(gameObject, true);
			ViewModelAttachment component = gameObject.GetComponent<ViewModelAttachment>();
			if (component)
			{
				if (this.socketOverrideName != string.Empty && component is VMAttachmentSocketOverride)
				{
					VMAttachmentSocketOverride vMAttachmentSocketOverride = (VMAttachmentSocketOverride)component;
					this.SetSocketByname(a.vm, this.socketOverrideName, vMAttachmentSocketOverride.socketOverride);
					if (this.modifyZoomOffset)
					{
						a.vm.punchScalar = this.punchScalar;
						a.vm.zoomOffset.z = this.zoomOffsetZ;
					}
				}
				component.viewModel = a.vm;
			}
		}
		return true;
	}

	protected override void SecureWriteMemberValues(uLink.BitStream stream)
	{
		base.SecureWriteMemberValues(stream);
		stream.Write<string>(this.socketOverrideName, new object[0]);
		stream.Write<float>(this.zoomOffsetZ, new object[0]);
		stream.Write<bool>(this.isMesh, new object[0]);
		stream.Write<float>(this.punchScalar, new object[0]);
		stream.Write<bool>(this.modifyZoomOffset, new object[0]);
	}

	public void SetSocketByname(Socket.Mapped vm, string name, Socket newSocket)
	{
		vm.socketMap.ReplaceSocket(name, newSocket);
	}

	protected override void UninstallFromItemModRepresentation(ItemModRepresentation rep)
	{
		WeaponModRep weaponModRep = (WeaponModRep)rep;
		GameObject gameObject = weaponModRep.attached;
		if (gameObject)
		{
			weaponModRep.SetAttached(null, false);
			UnityEngine.Object.Destroy(gameObject);
		}
		base.UninstallFromItemModRepresentation(rep);
	}

	protected override void UninstallFromViewModel(ref ModViewModelRemoveArgs a)
	{
		if (this.attachObjectVM != null)
		{
			WeaponModRep weaponModRep = (WeaponModRep)a.modRep;
			GameObject gameObject = weaponModRep.attached;
			ViewModelAttachment component = gameObject.GetComponent<ViewModelAttachment>();
			if (component)
			{
				component.viewModel = null;
			}
			Socket socketByName = this.GetSocketByName(a.vm, this.attachSocketName);
			if (socketByName.attachParent == null)
			{
				Transform transforms = socketByName.parent;
			}
			if (gameObject)
			{
				weaponModRep.SetAttached(null, true);
				UnityEngine.Object.Destroy(gameObject.gameObject);
			}
		}
	}

	private sealed class ITEM_TYPE : ItemModItem<BaseWeaponModDataBlock>, IInventoryItem, IItemModItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(BaseWeaponModDataBlock BLOCK) : base(BLOCK)
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
}