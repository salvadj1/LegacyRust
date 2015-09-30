using System;
using UnityEngine;

public class DisableOnConnectedState : MonoBehaviour
{
	protected static bool connectedStatus;

	public bool disableWhenConnected;

	static DisableOnConnectedState()
	{
	}

	public DisableOnConnectedState()
	{
	}

	protected void DoOnConnected()
	{
		base.gameObject.SetActive(!this.disableWhenConnected);
		dfControl component = base.gameObject.GetComponent<dfControl>();
		if (component)
		{
			if (!this.disableWhenConnected)
			{
				component.Show();
			}
			else
			{
				component.Hide();
			}
		}
	}

	protected void DoOnDisconnected()
	{
		base.gameObject.SetActive(this.disableWhenConnected);
		dfControl component = base.gameObject.GetComponent<dfControl>();
		if (component)
		{
			if (this.disableWhenConnected)
			{
				component.Show();
			}
			else
			{
				component.Hide();
			}
		}
	}

	public static void OnConnected()
	{
		DisableOnConnectedState.connectedStatus = true;
		UnityEngine.Object[] objArray = Resources.FindObjectsOfTypeAll(typeof(DisableOnConnectedState));
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			DisableOnConnectedState disableOnConnectedState = (DisableOnConnectedState)objArray[i];
			if (disableOnConnectedState.gameObject == null)
			{
				return;
			}
			disableOnConnectedState.DoOnConnected();
		}
	}

	public static void OnDisconnected()
	{
		DisableOnConnectedState.connectedStatus = false;
		UnityEngine.Object[] objArray = Resources.FindObjectsOfTypeAll(typeof(DisableOnConnectedState));
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			DisableOnConnectedState disableOnConnectedState = (DisableOnConnectedState)objArray[i];
			if (disableOnConnectedState.gameObject == null)
			{
				return;
			}
			disableOnConnectedState.DoOnDisconnected();
		}
	}

	private void Start()
	{
		if (!DisableOnConnectedState.connectedStatus)
		{
			this.DoOnDisconnected();
		}
		else
		{
			this.DoOnConnected();
		}
	}
}