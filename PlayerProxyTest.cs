using Facepunch.Prefetch;
using System;
using uLink;
using UnityEngine;

public class PlayerProxyTest : UnityEngine.MonoBehaviour
{
	[PrefetchChildComponent(NameMask="Soldier")]
	public GameObject body;

	[PrefetchChildComponent(NameMask="HB Hit")]
	public GameObject proxyCollider;

	[PrefetchComponent]
	public ArmorModelRenderer armorRenderer;

	public UnityEngine.MonoBehaviour[] proxyDisableList;

	private bool[] initialDisableListValues;

	private bool isMine;

	private bool isFaking;

	private bool hasFaked;

	public bool treatAsProxy
	{
		get
		{
			return (!this.isMine ? true : this.isFaking);
		}
		set
		{
			if (this.isMine && this.isFaking != value)
			{
				if (!this.hasFaked)
				{
					this.initialDisableListValues = new bool[(int)this.proxyDisableList.Length];
					this.hasFaked = true;
				}
				this.isFaking = value;
				if (!value)
				{
					for (int i = 0; i < (int)this.initialDisableListValues.Length; i++)
					{
						if (this.initialDisableListValues[i] && this.proxyDisableList[i])
						{
							this.proxyDisableList[i].enabled = true;
						}
					}
					this.MineInit();
				}
				else
				{
					for (int j = 0; j < (int)this.initialDisableListValues.Length; j++)
					{
						this.initialDisableListValues[j] = (!this.proxyDisableList[j] ? false : this.proxyDisableList[j].enabled);
					}
					if (this.body)
					{
						this.body.SetActive(true);
					}
					if (this.armorRenderer)
					{
						this.armorRenderer.enabled = true;
					}
					this.ProxyInit();
				}
			}
		}
	}

	public PlayerProxyTest()
	{
	}

	private void MineInit()
	{
		if (this.body)
		{
			this.body.SetActive(false);
		}
		if (this.proxyCollider)
		{
			this.proxyCollider.SetActive(false);
		}
		if (this.armorRenderer)
		{
			this.armorRenderer.enabled = false;
		}
	}

	private void ProxyInit()
	{
		for (int i = 0; i < (int)this.proxyDisableList.Length; i++)
		{
			if (this.proxyDisableList[i])
			{
				this.proxyDisableList[i].enabled = false;
			}
		}
		if (this.proxyCollider)
		{
			this.proxyCollider.SetActive(true);
		}
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		if (info.networkView.isMine)
		{
			this.isMine = true;
			this.MineInit();
		}
	}
}