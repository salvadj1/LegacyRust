using Facepunch;
using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

[AddComponentMenu("")]
internal sealed class NGCInternalView : uLinkNetworkView
{
	[NonSerialized]
	private NGC ngc;

	public NGCInternalView()
	{
	}

	private new void Awake()
	{
		this.ngc = base.GetComponent<NGC>();
		this.ngc.networkView = this;
		try
		{
			this.observed = this.ngc;
			this.rpcReceiver = RPCReceiver.OnlyObservedComponent;
			this.stateSynchronization = uLink.NetworkStateSynchronization.Off;
			this.securable = NetworkSecurable.None;
		}
		finally
		{
			try
			{
				base.Awake();
			}
			finally
			{
				this.ngc.networkViewID = base.viewID;
			}
		}
	}

	internal NGC GetNGC()
	{
		return this.ngc;
	}

	protected override bool OnRPC(string rpcName, uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		string str;
		char chr = rpcName[0];
		if (!NGCInternalView.Hack.actionToRPCName.TryGetValue(chr, out str))
		{
			Dictionary<char, string> chrs = NGCInternalView.Hack.actionToRPCName;
			string str1 = string.Concat("NGC:", chr);
			str = str1;
			chrs[chr] = str1;
		}
		return base.OnRPC(str, stream, info);
	}

	private static class Hack
	{
		public static Dictionary<char, string> actionToRPCName;

		static Hack()
		{
			NGCInternalView.Hack.actionToRPCName = new Dictionary<char, string>();
		}
	}
}