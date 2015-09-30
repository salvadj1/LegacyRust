using Facepunch;
using System;
using System.Collections;
using uLink;
using UnityEngine;

[NGCAutoAddScript]
[RequireComponent(typeof(Inventory))]
public class WorkBench : IDMain, IUseable, IContextRequestable, IContextRequestableQuick, IContextRequestableText, IComponentInterface<IUseable, Facepunch.MonoBehaviour, Useable>, IComponentInterface<IUseable, Facepunch.MonoBehaviour>, IComponentInterface<IUseable>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	[HideInInspector]
	public Inventory _inventory;

	private Useable _useable;

	private uLink.NetworkPlayer _currentlyUsingPlayer;

	private double _startTime_network;

	private float _workDuration = -1f;

	private static bool _debug_workbench;

	private static bool debug_workbench
	{
		get
		{
			return true;
		}
	}

	static WorkBench()
	{
	}

	public WorkBench() : base(IDFlags.Unknown)
	{
	}

	protected void Awake()
	{
		this.SharedAwake();
	}

	public void CancelWork()
	{
		IToolItem tool = this.GetTool();
		if (tool != null)
		{
			tool.CancelWork();
		}
		this._inventory.locked = false;
		this._startTime_network = 0;
		this._workDuration = -1f;
		base.CancelInvoke("CompleteWork");
		this.SendWorkStatusUpdate();
	}

	public void ClientClosedWorkbenchWindow()
	{
		if (this.IsLocalUsing())
		{
			NetCull.RPC(this, "StopUsing", uLink.RPCMode.Server);
		}
	}

	public void CompleteWork()
	{
	}

	public ContextExecution ContextQuery(Controllable controllable, ulong timestamp)
	{
		return ContextExecution.Quick;
	}

	public ContextResponse ContextRespondQuick(Controllable controllable, ulong timestamp)
	{
		return ContextRequestable.UseableForwardFromContextRespond(this, controllable, this._useable);
	}

	public string ContextText(Controllable localControllable)
	{
		if (this._currentlyUsingPlayer == uLink.NetworkPlayer.unassigned)
		{
			return "Use";
		}
		if (this._currentlyUsingPlayer != NetCull.player)
		{
			return "Occupied";
		}
		return string.Empty;
	}

	[RPC]
	private void DoAction()
	{
		if (!this.IsWorking())
		{
			this.StartWork();
		}
		else
		{
			this.TryCancel();
		}
	}

	public bool EnsureWorkExists()
	{
		IToolItem tool = this.GetTool();
		if (tool != null && tool.canWork)
		{
			return true;
		}
		return false;
	}

	public float GetFractionComplete()
	{
		if (!this.IsWorking())
		{
			return 0f;
		}
		return (float)(this.GetTimePassed() / (double)this._workDuration);
	}

	public virtual BlueprintDataBlock GetMatchingDBForItems()
	{
		ArrayList arrayLists = new ArrayList();
		ItemDataBlock[] all = DatablockDictionary.All;
		for (int i = 0; i < (int)all.Length; i++)
		{
			ItemDataBlock itemDataBlock = all[i];
			if (itemDataBlock is BlueprintDataBlock)
			{
				BlueprintDataBlock blueprintDataBlock = itemDataBlock as BlueprintDataBlock;
				bool flag = true;
				BlueprintDataBlock.IngredientEntry[] ingredientEntryArray = blueprintDataBlock.ingredients;
				int num = 0;
				while (num < (int)ingredientEntryArray.Length)
				{
					BlueprintDataBlock.IngredientEntry ingredientEntry = ingredientEntryArray[num];
					int num1 = 0;
					if (this._inventory.FindItem(ingredientEntry.Ingredient, out num1) == null || num1 < ingredientEntry.amount)
					{
						flag = false;
						break;
					}
					else
					{
						num++;
					}
				}
				if (flag)
				{
					arrayLists.Add(blueprintDataBlock);
				}
			}
		}
		if (arrayLists.Count <= 0)
		{
			return null;
		}
		BlueprintDataBlock blueprintDataBlock1 = null;
		int length = -1;
		IEnumerator enumerator = arrayLists.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				BlueprintDataBlock current = (BlueprintDataBlock)enumerator.Current;
				if ((int)current.ingredients.Length <= length)
				{
					continue;
				}
				blueprintDataBlock1 = current;
				length = (int)current.ingredients.Length;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		return blueprintDataBlock1;
	}

	public double GetTimePassed()
	{
		if (this._workDuration == -1f)
		{
			return -1;
		}
		return NetCull.time - this._startTime_network;
	}

	public virtual IToolItem GetTool()
	{
		return this._inventory.FindItemType<IToolItem>();
	}

	public float GetWorkDuration()
	{
		IToolItem tool = this.GetTool();
		if (tool == null)
		{
			return 0f;
		}
		return tool.workDuration;
	}

	public virtual bool HasTool()
	{
		return this.GetTool() != null;
	}

	public bool IsLocalUsing()
	{
		return this._currentlyUsingPlayer == NetCull.player;
	}

	public bool IsWorking()
	{
		return this._workDuration != -1f;
	}

	public static void Log<T>(T a, UnityEngine.Object b)
	{
		if (WorkBench.debug_workbench)
		{
			Debug.Log(a, b);
		}
	}

	public static void Log<T>(T a)
	{
		if (WorkBench.debug_workbench)
		{
			Debug.Log(a);
		}
	}

	public static void LogError<T>(T a, UnityEngine.Object b)
	{
		if (WorkBench.debug_workbench)
		{
			Debug.LogError(a, b);
		}
	}

	public static void LogError<T>(T a)
	{
		if (WorkBench.debug_workbench)
		{
			Debug.LogError(a);
		}
	}

	public static void LogWarning<T>(T a, UnityEngine.Object b)
	{
		if (WorkBench.debug_workbench)
		{
			Debug.LogWarning(a, b);
		}
	}

	public static void LogWarning<T>(T a)
	{
		if (WorkBench.debug_workbench)
		{
			Debug.LogWarning(a);
		}
	}

	public void OnUseEnter(Useable use)
	{
	}

	public void OnUseExit(Useable use, UseExitReason reason)
	{
	}

	public void RadialCheck()
	{
		if (this._useable.user && Vector3.Distance(this._useable.user.transform.position, base.transform.position) > 5f)
		{
			this._useable.Eject();
			base.CancelInvoke("RadialCheck");
		}
	}

	private void SendWorkStatusUpdate()
	{
		if (this._currentlyUsingPlayer == uLink.NetworkPlayer.unassigned)
		{
			return;
		}
		float _startTimeNetwork = (float)this._startTime_network;
		NetCull.RPC<float, float>(this, "WorkStatusUpdate", this._currentlyUsingPlayer, _startTimeNetwork, this._workDuration);
	}

	[RPC]
	private void SetUser(uLink.NetworkPlayer ply)
	{
		if (ply == NetCull.player)
		{
			RPOS.OpenWorkbenchWindow(this);
		}
		else if (this._currentlyUsingPlayer == NetCull.player && ply != this._currentlyUsingPlayer)
		{
			this._currentlyUsingPlayer = uLink.NetworkPlayer.unassigned;
			RPOS.CloseWorkbenchWindow();
		}
		this._currentlyUsingPlayer = ply;
	}

	private void SharedAwake()
	{
		this._inventory = base.GetComponent<Inventory>();
	}

	private void StartWork()
	{
		if (!this.EnsureWorkExists())
		{
			return;
		}
		IToolItem tool = this.GetTool();
		if (tool == null)
		{
			return;
		}
		this._startTime_network = NetCull.time;
		this._workDuration = this.GetWorkDuration();
		base.Invoke("CompleteWork", this._workDuration);
		this._inventory.locked = true;
		tool.StartWork();
		this.SendWorkStatusUpdate();
	}

	[RPC]
	private void StopUsing(uLink.NetworkMessageInfo info)
	{
		if (this._currentlyUsingPlayer == info.sender)
		{
			this._useable.Eject();
		}
	}

	[RPC]
	public void TakeAll()
	{
	}

	public void TryCancel()
	{
		this.CancelWork();
	}

	[RPC]
	private void WorkStatusUpdate(float startTime, float newWorkDuration)
	{
		this._startTime_network = (double)startTime;
		this._workDuration = newWorkDuration;
		RPOS.GetWindowByName("Workbench").GetComponent<RPOSWorkbenchWindow>().BenchUpdate();
	}
}