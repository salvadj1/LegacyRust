using System;
using UnityEngine;

public class RPOSLootWindow : RPOSWindowScrollable
{
	[NonSerialized]
	public LootableObject myLootable;

	[NonSerialized]
	public bool initalized;

	public UIButton TakeAllButton;

	public RPOSLootWindow()
	{
	}

	public void Initialize()
	{
		base.GetComponentInChildren<RPOSInvCellManager>().SetInventory(this.myLootable.GetComponent<Inventory>(), true);
		base.ResetScrolling();
	}

	public virtual void LootClosed()
	{
		if (this.myLootable)
		{
			this.myLootable.ClientClosedLootWindow();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected override void OnExternalClose()
	{
		this.LootClosed();
	}

	protected override void OnRPOSClosed()
	{
		base.OnRPOSClosed();
		this.LootClosed();
	}

	protected override void OnWindowHide()
	{
		try
		{
			base.OnWindowHide();
		}
		finally
		{
			this.LootClosed();
		}
	}

	public virtual void SetLootable(LootableObject lootable, bool doInit)
	{
		this.myLootable = lootable;
		this.Initialize();
	}

	public void TakeAllButtonClicked(GameObject go)
	{
		RPOS.ChangeRPOSMode(false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected override void WindowAwake()
	{
		this.autoResetScrolling = false;
		base.WindowAwake();
		if (!this.initalized && this.myLootable)
		{
			this.Initialize();
		}
		if (this.TakeAllButton)
		{
			UIEventListener.Get(this.TakeAllButton.gameObject).onClick += new UIEventListener.VoidDelegate(this.TakeAllButtonClicked);
		}
	}
}