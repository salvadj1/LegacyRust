using Facepunch.Progress;
using System;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
	public dfRichTextLabel infoText;

	public dfPanel progressBar;

	public dfSprite progressIndicator;

	public readonly static ProgressBar Operations;

	private static string labelString;

	private static bool showing;

	private static LoadingScreen singleton;

	static LoadingScreen()
	{
		LoadingScreen.Operations = new ProgressBar();
	}

	public LoadingScreen()
	{
	}

	private void Awake()
	{
		LoadingScreen.singleton = this;
		if (!LoadingScreen.showing)
		{
			LoadingScreen.Hide();
		}
		else
		{
			LoadingScreen.Show();
		}
	}

	public static void Hide()
	{
		LoadingScreen.showing = false;
		if (LoadingScreen.singleton)
		{
			LoadingScreen.singleton.GetComponent<dfPanel>().Hide();
		}
	}

	public void LateUpdate()
	{
		float single;
		if (LoadingScreen.Operations.Update(out single))
		{
			if (!this.progressBar.IsVisible)
			{
				this.progressBar.Show();
			}
			this.progressIndicator.FillAmount = single;
		}
		else if (this.progressBar.IsVisible)
		{
			this.progressBar.Hide();
		}
	}

	public static void Show()
	{
		LoadingScreen.showing = true;
		if (LoadingScreen.singleton)
		{
			LoadingScreen.singleton.GetComponent<dfPanel>().Show();
		}
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(LoadingScreen.labelString) && this.infoText)
		{
			this.infoText.Text = LoadingScreen.labelString;
		}
	}

	public static void Update(string strText)
	{
		LoadingScreen.Operations.Clean();
		Debug.Log(string.Concat("LoadingScreen: ", strText));
		LoadingScreen.labelString = strText;
		if (LoadingScreen.singleton)
		{
			LoadingScreen.singleton.infoText.Text = strText;
		}
	}
}