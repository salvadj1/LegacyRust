using System;
using UnityEngine;

public class OptionsWindow : MonoBehaviour
{
	public OptionsWindow()
	{
	}

	public void DoApply()
	{
		base.BroadcastMessage("UpdateConVars");
		ConsoleSystem.Run("config.save", false);
	}

	public void DoOK()
	{
		this.DoApply();
	}

	public void OnWindowVisibleChanged()
	{
		if (base.GetComponent<dfPanel>().IsVisible)
		{
			base.BroadcastMessage("UpdateFromConVar");
		}
	}

	private void Start()
	{
	}
}