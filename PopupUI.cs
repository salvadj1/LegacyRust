using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
	public static PopupUI singleton;

	public UnityEngine.Object prefabNotice;

	public UnityEngine.Object prefabInventory;

	protected dfPanel panelLocal;

	public PopupUI()
	{
	}

	public void CreateInventory(string strText)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.prefabInventory);
		this.panelLocal.AddControl(gameObject.GetComponent<dfPanel>());
		gameObject.GetComponent<PopupInventory>().Setup(1.5f, strText);
	}

	public void CreateNotice(float fSeconds, string strIcon, string strText)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.prefabNotice);
		this.panelLocal.AddControl(gameObject.GetComponent<dfPanel>());
		gameObject.GetComponent<PopupNotice>().Setup(fSeconds, strIcon, strText);
	}

	[DebuggerHidden]
	public IEnumerator DoTests()
	{
		PopupUI.<DoTests>c__Iterator32 variable = null;
		return variable;
	}

	private void Start()
	{
		PopupUI.singleton = this;
		this.panelLocal = base.GetComponent<dfPanel>();
	}
}