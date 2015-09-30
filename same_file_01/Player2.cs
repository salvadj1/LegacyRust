using System;
using uLink;
using UnityEngine;

public class Player : IDLocalCharacter
{
	public Player()
	{
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		if (base.networkView.isMine)
		{
			GameTip componentInChildren = base.GetComponentInChildren<GameTip>();
			if (componentInChildren)
			{
				componentInChildren.enabled = false;
			}
		}
		if (!base.networkView.isMine)
		{
			GameTip gameTip = base.GetComponentInChildren<GameTip>();
			if (gameTip && base.playerClient)
			{
				gameTip.text = base.playerClient.userName;
			}
		}
	}
}