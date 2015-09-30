using Facepunch;
using Facepunch.Cursor;
using System;
using uLink;
using UnityEngine;

public class DeathScreen : UnityEngine.MonoBehaviour
{
	public dfLabel lblDescription;

	private static DeathScreen singleton;

	private UnlockCursorNode cursorLocker;

	static DeathScreen()
	{
	}

	public DeathScreen()
	{
	}

	public void Hide()
	{
		base.gameObject.GetComponent<dfPanel>().Hide();
		base.gameObject.SetActive(false);
	}

	public void IntroAnimations()
	{
		dfTweenComponentBase[] componentsInChildren = base.gameObject.GetComponentsInChildren<dfTweenComponentBase>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			dfTweenComponentBase _dfTweenComponentBase = componentsInChildren[i];
			if (_dfTweenComponentBase.TweenName == "FadeIn")
			{
				_dfTweenComponentBase.Play();
			}
		}
	}

	private void OnDestroy()
	{
		this.cursorLocker.Dispose();
		this.cursorLocker = null;
	}

	public void OnDisable()
	{
		if (this.cursorLocker)
		{
			this.cursorLocker.On = false;
		}
	}

	public void OnEnable()
	{
		if (!this.cursorLocker)
		{
			this.cursorLocker = LockCursorManager.CreateCursorUnlockNode(false, "Death Screen");
		}
		this.cursorLocker.On = true;
	}

	public void OutroAnimations()
	{
		dfTweenComponentBase[] componentsInChildren = base.gameObject.GetComponentsInChildren<dfTweenComponentBase>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			dfTweenComponentBase _dfTweenComponentBase = componentsInChildren[i];
			if (_dfTweenComponentBase.TweenName == "FadeOut")
			{
				_dfTweenComponentBase.Play();
			}
		}
		this.cursorLocker.On = false;
		base.Invoke("Hide", 5f);
	}

	public void RequestRespawn()
	{
		this.OutroAnimations();
		ServerManagement.Get().networkView.RPC<bool>("RequestRespawn", uLink.RPCMode.Server, false);
	}

	public void RequestRespawn_InCamp()
	{
		this.OutroAnimations();
		ServerManagement.Get().networkView.RPC<bool>("RequestRespawn", uLink.RPCMode.Server, true);
	}

	public static void Show()
	{
		if (DeathScreen.singleton == null)
		{
			return;
		}
		DeathScreen.singleton.CancelInvoke("Hide");
		DeathScreen.singleton.Hide();
		DeathScreen.singleton.gameObject.GetComponent<dfPanel>().Show();
		DeathScreen.singleton.lblDescription.Text = deathscreen.reason;
		DeathScreen.singleton.gameObject.SetActive(true);
		DeathScreen.singleton.IntroAnimations();
		deathscreen.reason = string.Empty;
	}

	private void Start()
	{
		DeathScreen.singleton = this;
		this.Hide();
	}
}