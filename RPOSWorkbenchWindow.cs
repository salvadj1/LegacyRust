using System;
using uLink;
using UnityEngine;

public class RPOSWorkbenchWindow : RPOSWindow
{
	private WorkBench _myWorkBench;

	public UIButton actionButton;

	public UIButton takeAllButton;

	public UISlider progressBar;

	public UILabel percentLabel;

	public AudioClip startSound;

	public AudioClip finishedSound;

	public RPOSWorkbenchWindow()
	{
	}

	private void ActionButtonClicked(GameObject go)
	{
		Debug.Log("Action button clicked");
		this._myWorkBench.networkView.RPC("DoAction", uLink.RPCMode.Server, new object[0]);
		Debug.Log("Action post");
	}

	public void BenchUpdate()
	{
		if (!this._myWorkBench)
		{
			Debug.Log("NO BENCH!?!?!");
		}
		if (!this._myWorkBench.IsWorking())
		{
			this.actionButton.GetComponentInChildren<UILabel>().text = "Start";
			this.takeAllButton.enabled = true;
			this.SetCellsLocked(false);
			if (this._myWorkBench._inventory.IsSlotOccupied(12))
			{
				this.finishedSound.Play();
			}
		}
		else
		{
			this.startSound.Play();
			this.actionButton.GetComponentInChildren<UILabel>().text = "Cancel";
			this.takeAllButton.enabled = false;
			this.SetCellsLocked(true);
		}
	}

	public void Initialize()
	{
		base.GetComponentInChildren<RPOSInvCellManager>().SetInventory(this._myWorkBench.GetComponent<Inventory>(), false);
	}

	protected override void OnExternalClose()
	{
		this.WorkbenchClosed();
	}

	protected override void OnRPOSClosed()
	{
		base.OnRPOSClosed();
		this.WorkbenchClosed();
	}

	protected override void OnWindowClosed()
	{
		base.OnWindowClosed();
		this.WorkbenchClosed();
	}

	private void SetCellsLocked(bool isLocked)
	{
		RPOSInvCellManager componentInChildren = base.GetComponentInChildren<RPOSInvCellManager>();
		for (int i = 0; i < (int)componentInChildren._inventoryCells.Length; i++)
		{
			RPOSInventoryCell rPOSInventoryCell = componentInChildren._inventoryCells[i];
			if (rPOSInventoryCell)
			{
				rPOSInventoryCell.SetItemLocked(isLocked);
			}
		}
	}

	public void SetWorkbench(WorkBench workbenchObj)
	{
		this._myWorkBench = workbenchObj;
		this.Initialize();
	}

	private void TakeAllButtonClicked(GameObject go)
	{
		this._myWorkBench.networkView.RPC("TakeAll", uLink.RPCMode.Server, new object[0]);
	}

	public void Update()
	{
		if (!this._myWorkBench || !this._myWorkBench.IsWorking())
		{
			this.percentLabel.enabled = false;
			this.progressBar.sliderValue = 0f;
		}
		else
		{
			this.percentLabel.enabled = true;
			this.progressBar.sliderValue = this._myWorkBench.GetFractionComplete();
			float single = Mathf.Clamp01(this._myWorkBench.GetFractionComplete()) * 100f;
			this.percentLabel.text = string.Concat(single.ToString("N0"), "%");
		}
	}

	protected override void WindowAwake()
	{
		base.WindowAwake();
		UIEventListener.Get(this.actionButton.gameObject).onClick += new UIEventListener.VoidDelegate(this.ActionButtonClicked);
		UIEventListener.Get(this.takeAllButton.gameObject).onClick += new UIEventListener.VoidDelegate(this.TakeAllButtonClicked);
	}

	public virtual void WorkbenchClosed()
	{
		if (this._myWorkBench)
		{
			this._myWorkBench.ClientClosedWorkbenchWindow();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}