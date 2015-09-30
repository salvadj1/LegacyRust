using System;
using UnityEngine;

public class ItemModRepresentation : MonoBehaviour
{
	protected const ItemModRepresentation.Caps kAllCaps = ItemModRepresentation.Caps.Initialize | ItemModRepresentation.Caps.BindStateFlags | ItemModRepresentation.Caps.Shutdown;

	protected const ItemModRepresentation.Caps kNoCaps = 0;

	private ItemRepresentation _itemRep;

	private int _modSlot = -1;

	[NonSerialized]
	public GameObject instantiatedThing;

	[NonSerialized]
	protected readonly ItemModRepresentation.Caps caps;

	private CharacterStateFlags? _lastFlags;

	public bool destroyed
	{
		get
		{
			return this._modSlot == -2;
		}
	}

	public bool initialized
	{
		get
		{
			return this._modSlot != -1;
		}
	}

	public HeldItemDataBlock itemDatablock
	{
		get
		{
			HeldItemDataBlock heldItemDataBlock;
			if (!this._itemRep)
			{
				heldItemDataBlock = null;
			}
			else
			{
				heldItemDataBlock = this._itemRep.datablock;
			}
			return heldItemDataBlock;
		}
	}

	public ItemRepresentation itemRep
	{
		get
		{
			return this._itemRep;
		}
	}

	public ItemModDataBlock modDataBlock
	{
		get
		{
			return this._itemRep._itemMods.ItemModDataBlock(this._modSlot);
		}
	}

	public int modSlot
	{
		get
		{
			return this._modSlot;
		}
	}

	public ItemModRepresentation()
	{
		if (base.GetType() == typeof(ItemModRepresentation))
		{
			this.caps = (ItemModRepresentation.Caps)0;
		}
		else
		{
			this.caps = ItemModRepresentation.Caps.Initialize | ItemModRepresentation.Caps.BindStateFlags | ItemModRepresentation.Caps.Shutdown;
		}
	}

	protected ItemModRepresentation(ItemModRepresentation.Caps caps)
	{
		this.caps = caps;
	}

	protected virtual void BindStateFlags(CharacterStateFlags flags, ItemModRepresentation.Reason reason)
	{
	}

	internal void HandleChangedStateFlags(CharacterStateFlags flags, bool notFromLoading)
	{
		if ((byte)(this.caps & ItemModRepresentation.Caps.BindStateFlags) == 2 && (!this._lastFlags.HasValue || !this._lastFlags.Value.Equals(flags)))
		{
			this.BindStateFlags(flags, (!notFromLoading ? ItemModRepresentation.Reason.Initialization : ItemModRepresentation.Reason.Explicit));
			this._lastFlags = new CharacterStateFlags?(flags);
		}
	}

	internal void Initialize(ItemRepresentation itemRep, int modSlot, CharacterStateFlags flags)
	{
		if (this._modSlot != -1)
		{
			if (this._modSlot == -2)
			{
				throw new InvalidOperationException("This ItemModRepresentation has been destroyed");
			}
			if (itemRep != this._itemRep || modSlot < 0 && modSlot < 5 && modSlot != this._modSlot)
			{
				throw new InvalidOperationException(string.Format("The ItemModRepresentation was already initialized with {{\"item\":\"{0}\",\"slot\":{1}}} and cannot be re-initialized to use {{\"item\":\"{2|\",\"slot\":{3}}}", new object[] { this._itemRep, this._modSlot, itemRep, modSlot }));
			}
		}
		else
		{
			if (!itemRep)
			{
				throw new ArgumentOutOfRangeException("itemRep", itemRep, "!itemRep");
			}
			if (modSlot < 0 || modSlot >= 5)
			{
				throw new ArgumentOutOfRangeException("modSlot", (object)modSlot, "modSlot<0||modSlot>=MAX_SUPPORTED_ITEM_MODS");
			}
			this._itemRep = itemRep;
			this._modSlot = modSlot;
			if ((byte)(this.caps & ItemModRepresentation.Caps.Initialize) == 1)
			{
				try
				{
					this.Initialize();
				}
				catch (Exception exception)
				{
					this._itemRep = null;
					this._modSlot = -1;
					throw;
				}
			}
			this.HandleChangedStateFlags(flags, false);
		}
	}

	protected virtual void Initialize()
	{
	}

	[Obsolete("Do not use OnDestroy in implementing classes. Instead override Shutdown() and specify Caps.Shutdown in the constructor!")]
	private void OnDestroy()
	{
		if (this._modSlot != -2)
		{
			try
			{
				if (this._modSlot != -1)
				{
					if (!this._itemRep)
					{
						this._itemRep = null;
					}
					else
					{
						try
						{
							if ((byte)(this.caps & ItemModRepresentation.Caps.Shutdown) == 128)
							{
								try
								{
									this.Shutdown();
								}
								catch (Exception exception)
								{
									Debug.LogError(exception, this);
								}
							}
							try
							{
								this._itemRep.ItemModRepresentationDestroyed(this);
							}
							catch (Exception exception1)
							{
								Debug.LogError(exception1, this);
							}
						}
						finally
						{
							this._itemRep = null;
						}
					}
				}
			}
			finally
			{
				this._modSlot = -2;
			}
		}
	}

	protected virtual void Shutdown()
	{
	}

	[Flags]
	protected enum Caps : byte
	{
		Initialize = 1,
		BindStateFlags = 2,
		Shutdown = 128
	}

	protected enum Reason
	{
		Initialization,
		Implicit,
		Explicit
	}
}