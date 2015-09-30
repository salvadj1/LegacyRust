using Facepunch;
using System;
using uLink;
using UnityEngine;

public sealed class Context : UnityEngine.MonoBehaviour
{
	private const string kButtonName = "WorldUse";

	public const ulong kQuickTapMillisecondLimit = 300L;

	private const string kRPCPrefix = "Context:";

	private const string kRPC_RequestFromClient = "Context:A";

	private const string kRPC_QuickTapFromClient = "Context:B";

	private const string kRPC_SelectedOptionFromClient = "Context:C";

	private const string kRPC_NoSelectionFromClient = "Context:D";

	private const string kRPC_ReadOptionsFromServer = "Context:E";

	private const string kRPC_NoOpFromServer = "Context:F";

	private const string kRPC_CancelFromServer = "Context:G";

	private const string kRPC_FailedImmediateFromServer = "Context:H";

	private const string kRPC_SuccessImmediateFromServer = "Context:I";

	private const string kRPC_FailedSelectionFromServer = "Context:J";

	private const string kRPC_SuccessSelectionFromServer = "Context:K";

	private const string kRPC_StaleSelectionFromServer = "Context:L";

	private const string kRPC_RetryFromServer = "Context:M";

	private static Context self;

	private static uLinkNetworkView network;

	private static ContextUI ui;

	private static int swallowInputCount;

	public static bool ButtonDown
	{
		get
		{
			if (Input.GetButtonDown("WorldUse"))
			{
				if (Context.swallowInputCount == 0)
				{
					return !ChatUI.IsVisible();
				}
				Context.swallowInputCount = Context.swallowInputCount - 1;
			}
			return false;
		}
	}

	public static bool ButtonUp
	{
		get
		{
			return Input.GetButtonUp("WorldUse");
		}
	}

	public static bool Working
	{
		get
		{
			return (!Context.self ? false : Context.ui._clientState != ContextClientState.Off);
		}
	}

	public static bool WorkingInMenu
	{
		get
		{
			return (!Context.self || Context.ui._clientState <= ContextClientState.Off ? false : Context.ui._clientState < ContextClientState.Validating);
		}
	}

	public Context()
	{
	}

	[RPC]
	private void A(NetEntityID hit, uLink.NetworkMessageInfo info)
	{
	}

	private void Awake()
	{
		if (Context.self && Context.self != this)
		{
			Debug.LogError("More than one", this);
			return;
		}
		Context.self = this;
		Context.network = base.GetComponent<uLinkNetworkView>();
		Context.ui = base.GetComponent<ContextUI>();
	}

	[RPC]
	private void B(uLink.NetworkMessageInfo info)
	{
	}

	public static bool BeginQuery(Contextual contextual)
	{
		NetEntityID netEntityID;
		if (!Context.self)
		{
			Debug.LogWarning("Theres no instance", Context.self);
		}
		else if (Context.ui._clientState != ContextClientState.Off)
		{
			Debug.LogWarning("Client is already in a context menu. Wait", contextual);
		}
		else if (!contextual)
		{
			Debug.LogWarning("null", Context.self);
		}
		else if (contextual.exists)
		{
			Facepunch.MonoBehaviour monoBehaviour = contextual.implementor;
			if ((int)NetEntityID.Of(contextual, out netEntityID) != 0)
			{
				Context.ui.OnServerQuerySent(monoBehaviour, netEntityID);
				return true;
			}
			Debug.LogWarning("requestable has no network view", monoBehaviour);
		}
		else
		{
			Debug.LogWarning("requestable destroyed or did not implement monobehaviour", Context.self);
		}
		return false;
	}

	[RPC]
	private void C(int name, uLink.NetworkMessageInfo info)
	{
	}

	[RPC]
	private void D(uLink.NetworkMessageInfo info)
	{
	}

	[RPC]
	private void E(ContextMenuData options, uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerMenu(options);
	}

	public static void EndQuery()
	{
		if (Context.self && Context.ui._clientState > ContextClientState.Off && Context.ui._clientState < ContextClientState.Validating)
		{
			if (NetCull.localTimeInMillis - Context.ui.clientQueryTime > (long)300)
			{
				Context.ui.OnServerCancelSent();
			}
			else
			{
				Context.ui.OnServerQuickTapSent();
			}
		}
	}

	[RPC]
	private void F(uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerNoOp();
	}

	[RPC]
	private void G(uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerCancel();
	}

	[RPC]
	private void H(uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerImmediate(false);
	}

	[RPC]
	private void I(uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerImmediate(true);
	}

	[RPC]
	private void J(uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerSelection(false);
	}

	[RPC]
	private void K(uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerSelection(true);
	}

	[RPC]
	private void L(uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerSelectionStale();
	}

	[RPC]
	private void M(uLink.NetworkMessageInfo info)
	{
		Context.ui.OnServerRestartPolling();
	}

	private void OnDestroy()
	{
		if (Context.self == this)
		{
			Context.self = null;
			Context.network = null;
			Context.swallowInputCount = 0;
			Context.ui = null;
		}
	}

	public static event ContextClientWorkingCallback OnClientWorking
	{
		add
		{
			ContextUI.clientWorkingCallbacks += value;
			if (Context.Working)
			{
				value(true);
			}
		}
		remove
		{
			ContextUI.clientWorkingCallbacks -= value;
			if (Context.Working)
			{
				value(false);
			}
		}
	}

	internal static class UICommands
	{
		internal static bool IsButtonHeld(bool swallow)
		{
			if (!Input.GetButton("WorldUse"))
			{
				return false;
			}
			if (swallow)
			{
				Context.swallowInputCount = 1;
			}
			return true;
		}

		internal static void Issue_Cancel()
		{
			Context.network.RPC("Context:D", uLink.RPCMode.Server, new object[0]);
		}

		internal static void Issue_QuickTap()
		{
			Context.network.RPC("Context:B", uLink.RPCMode.Server, new object[0]);
		}

		internal static void Issue_Request(NetEntityID clientQueryEntID)
		{
			Context.network.RPC<NetEntityID>("Context:A", uLink.RPCMode.Server, clientQueryEntID);
		}

		internal static void Issue_Selection(int name)
		{
			Context.network.RPC<int>("Context:C", uLink.RPCMode.Server, name);
		}
	}
}