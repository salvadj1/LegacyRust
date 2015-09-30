using Facepunch.Cursor;
using System;
using UnityEngine;

public class LockEntry : MonoBehaviour
{
	private static LockEntry singleton;

	private UnlockCursorNode cursorLocker;

	public dfTextbox passwordInput;

	public dfRichTextLabel entryLabel;

	private bool changingEntry;

	static LockEntry()
	{
	}

	public LockEntry()
	{
	}

	public void CancelPressed()
	{
		LockEntry.Hide();
	}

	public static void Hide()
	{
		LockEntry.singleton.SetVisible(false);
	}

	public static bool IsVisible()
	{
		if (LockEntry.singleton == null)
		{
			return false;
		}
		return LockEntry.singleton.passwordInput.IsVisible;
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
			this.cursorLocker = LockCursorManager.CreateCursorUnlockNode(false, "Lock Entry");
		}
		this.cursorLocker.On = true;
	}

	public void PasswordEntered()
	{
		string text = this.passwordInput.Text;
		if (text.Length != 4)
		{
			ConsoleSystem.Run("notice.popup 5  \"Password must be 4 digits.\"", false);
			return;
		}
		string str = text;
		for (int i = 0; i < str.Length; i++)
		{
			if (!char.IsDigit(str[i]))
			{
				ConsoleSystem.Run("notice.popup 5  \"Must be digits only.\"", false);
				return;
			}
		}
		ConsoleNetworker.SendCommandToServer(string.Concat("lockentry.passwordentry ", text, " ", (!this.changingEntry ? "false" : "true")));
		LockEntry.Hide();
	}

	public void SetVisible(bool visible)
	{
		this.entryLabel.Text = (!this.changingEntry ? "Password:" : "New Password:");
		dfPanel component = base.GetComponent<dfPanel>();
		if (!visible)
		{
			component.Hide();
			this.passwordInput.Unfocus();
		}
		else
		{
			component.Show();
			component.BringToFront();
			this.passwordInput.Text = string.Empty;
			this.passwordInput.Focus();
		}
		base.gameObject.SetActive(visible);
	}

	public static void Show(bool changing)
	{
		LockEntry.singleton.changingEntry = changing;
		LockEntry.singleton.SetVisible(true);
	}

	public void Start()
	{
		LockEntry.singleton = this;
		LockEntry.Hide();
	}
}