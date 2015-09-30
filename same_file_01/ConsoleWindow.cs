using Facepunch.Cursor;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class ConsoleWindow : MonoBehaviour
{
	public dfTextbox consoleInput;

	public dfLabel consoleOutput;

	public dfScrollbar consoleScroller;

	[NonSerialized]
	public UnlockCursorNode cursorManager = LockCursorManager.CreateCursorUnlockNode(false, "Console Window");

	public static ConsoleWindow singleton;

	[NonSerialized]
	protected bool shouldScrollDown = true;

	public ConsoleWindow()
	{
	}

	public void AddText(string str, bool bFromServer = false)
	{
		if (bFromServer)
		{
			str = string.Concat("[color #00ffff]", str, "[/color]\n");
		}
		dfLabel _dfLabel = this.consoleOutput;
		_dfLabel.Text = string.Concat(_dfLabel.Text, str, "\n");
		this.TrimBuffer();
		if (this.consoleScroller.Value >= this.consoleScroller.MaxValue - this.consoleScroller.ScrollSize - 50f)
		{
			this.shouldScrollDown = true;
		}
	}

	private void Awake()
	{
		ConsoleWindow.singleton = this;
	}

	private void CaptureLog(string log, string stacktrace, LogType type)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (log.StartsWith("This uLink evaluation license is temporary."))
		{
			return;
		}
		if (log.StartsWith("Failed to capture screen shot"))
		{
			return;
		}
		if (type != LogType.Log)
		{
			this.AddText(string.Concat("[color #ff0000]> ", log, "[/color]"), false);
		}
		else
		{
			this.AddText(string.Concat("[color #eeeeee]> ", log, "[/color]"), false);
		}
		if (log.StartsWith("Resynchronize Clock is still in progress"))
		{
			return;
		}
		if (type == LogType.Exception || type == LogType.Error)
		{
			if (stacktrace.Length <= 8)
			{
				string str = StackTraceUtility.ExtractStackTrace();
				if (str.Length <= 8)
				{
					ConsoleWindow.Client_Error(log, (new StackTrace()).ToString());
				}
				else
				{
					ConsoleWindow.Client_Error(log, str);
				}
			}
			else
			{
				ConsoleWindow.Client_Error(log, stacktrace);
			}
		}
	}

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern void Client_Error(string strLog, string strTrace);

	public static bool IsVisible()
	{
		return (!ConsoleWindow.singleton ? false : ConsoleWindow.singleton.GetComponent<dfPanel>().IsVisible);
	}

	private void OnDestroy()
	{
		ConsoleSystem.UnregisterLogCallback(new Application.LogCallback(this.CaptureLog));
	}

	public void OnInput()
	{
		string text = this.consoleInput.Text;
		this.consoleInput.Text = string.Empty;
		this.RunCommand(text);
	}

	public void RunCommand(string strInput)
	{
		this.AddText(string.Concat("[color #00ff00]> ", strInput, "[/color]"), false);
		string empty = string.Empty;
		if (!ConsoleSystem.RunCommand_Clientside(strInput, out empty, true))
		{
			ConsoleNetworker.SendCommandToServer(strInput);
		}
		else if (empty != string.Empty)
		{
			this.AddText(string.Concat("[color #ffff00]", empty, "[/color]"), false);
		}
	}

	private void Start()
	{
		ConsoleSystem.RegisterLogCallback(new Application.LogCallback(this.CaptureLog), false);
		ConsoleWindow.singleton.GetComponent<dfPanel>().Hide();
	}

	private void TrimBuffer()
	{
		if (this.consoleOutput.Text.Length < 5000)
		{
			return;
		}
		int num = this.consoleOutput.Text.IndexOf('\n');
		if (num == -1)
		{
			return;
		}
		this.consoleOutput.Text = this.consoleOutput.Text.Substring(num + 1);
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.F1))
		{
			if (this.shouldScrollDown && this.consoleScroller.Value != this.consoleScroller.MaxValue - this.consoleScroller.ScrollSize)
			{
				this.consoleScroller.Value = this.consoleScroller.MaxValue;
				this.shouldScrollDown = false;
			}
			return;
		}
		if (!ConsoleWindow.IsVisible())
		{
			ConsoleWindow.singleton.GetComponent<dfPanel>().Show();
			ConsoleWindow.singleton.GetComponent<dfPanel>().BringToFront();
			this.consoleInput.Focus();
			this.cursorManager.On = true;
		}
		else
		{
			this.consoleInput.Unfocus();
			ConsoleWindow.singleton.GetComponent<dfPanel>().Hide();
			this.cursorManager.On = false;
		}
		this.consoleInput.Text = string.Empty;
	}
}