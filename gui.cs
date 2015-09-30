using System;
using UnityEngine;

public class gui : ConsoleSystem
{
	public gui()
	{
	}

	[Client]
	[Help("Hides all GUI (useful for taking screenshots)", "")]
	public static void hide(ref ConsoleSystem.Arg args)
	{
		GUIHide.SetVisible(false);
	}

	[Client]
	[Help("Hides the alpha/branding on the top right", "")]
	public static void hide_branding(ref ConsoleSystem.Arg args)
	{
		GameObject gameObject = GameObject.Find("BrandingPanel");
		if (gameObject == null)
		{
			return;
		}
		gameObject.GetComponent<dfPanel>().Hide();
	}

	[Client]
	[Help("The opposite of gui.hide", "")]
	public static void show(ref ConsoleSystem.Arg args)
	{
		GUIHide.SetVisible(true);
	}

	[Client]
	[Help("The opposite of gui.hide_branding", "")]
	public static void show_branding(ref ConsoleSystem.Arg args)
	{
		GameObject gameObject = GameObject.Find("BrandingPanel");
		if (gameObject == null)
		{
			return;
		}
		gameObject.GetComponent<dfPanel>().Show();
	}
}