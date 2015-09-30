using Facepunch.Cursor;
using Facepunch.Utility;
using System;
using UnityEngine;

public class ChatUI : MonoBehaviour
{
	public dfTextbox textInput;

	public dfPanel chatContainer;

	public UnityEngine.Object chatLine;

	public static ChatUI singleton;

	[NonSerialized]
	private UnlockCursorNode unlockNode;

	public ChatUI()
	{
	}

	public static void AddLine(string name, string text)
	{
		if (ChatUI.singleton == null)
		{
			return;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(ChatUI.singleton.chatLine);
		if (gameObject == null)
		{
			return;
		}
		ChatLine component = gameObject.GetComponent<ChatLine>();
		component.Setup(string.Concat(name, ":"), text);
		ChatUI.singleton.chatContainer.AddControl(component.GetComponent<dfPanel>());
	}

	private void Awake()
	{
		this.unlockNode = LockCursorManager.CreateCursorUnlockNode(false, "ChatUI");
	}

	public void CancelChatting()
	{
		this.textInput.Text = string.Empty;
		ChatUI.singleton.Invoke("CancelChatting_Delayed", 0.2f);
	}

	public void CancelChatting_Delayed()
	{
		this.unlockNode.TryLock();
		this.textInput.Text = string.Empty;
		this.textInput.Unfocus();
		this.textInput.Hide();
	}

	public void ClearText()
	{
		this.textInput.Text = string.Empty;
	}

	public static void Close()
	{
		if (ChatUI.singleton == null)
		{
			return;
		}
		ChatUI.singleton.CancelChatting();
	}

	public static bool IsVisible()
	{
		if (ChatUI.singleton == null)
		{
			return false;
		}
		return ChatUI.singleton.textInput.IsVisible;
	}

	private void OnDestroy()
	{
		if (this.unlockNode != null)
		{
			this.unlockNode.Dispose();
			this.unlockNode = null;
		}
	}

	private void OnLoseFocus()
	{
		this.CancelChatting();
	}

	public static void Open()
	{
		if (ChatUI.singleton == null)
		{
			return;
		}
		ChatUI.singleton.textInput.Text = string.Empty;
		ChatUI.singleton.textInput.Show();
		ChatUI.singleton.textInput.Focus();
		ChatUI.singleton.Invoke("ClearText", 0.05f);
		if (ChatUI.singleton.unlockNode != null)
		{
			ChatUI.singleton.unlockNode.On = true;
		}
	}

	public void ReLayout()
	{
		this.chatContainer.RelativePosition = new Vector2(0f, 0f);
		dfPanel[] componentsInChildren = this.chatContainer.GetComponentsInChildren<dfPanel>();
		float height = 0f;
		dfPanel[] dfPanelArray = componentsInChildren;
		for (int i = 0; i < (int)dfPanelArray.Length; i++)
		{
			dfPanel _dfPanel = dfPanelArray[i];
			if (_dfPanel.gameObject != this.chatContainer.gameObject)
			{
				height = height + _dfPanel.Height;
			}
		}
		Vector2 vector2 = new Vector2(0f, this.chatContainer.Height - height);
		dfPanel[] dfPanelArray1 = componentsInChildren;
		for (int j = 0; j < (int)dfPanelArray1.Length; j++)
		{
			dfPanel _dfPanel1 = dfPanelArray1[j];
			if (_dfPanel1.gameObject != this.chatContainer.gameObject)
			{
				_dfPanel1.RelativePosition = vector2;
				vector2.y = vector2.y + _dfPanel1.Height;
			}
		}
	}

	public void SendChat()
	{
		if (this.textInput.Text != string.Empty)
		{
			ConsoleNetworker.SendCommandToServer(string.Concat("chat.say ", Facepunch.Utility.String.QuoteSafe(this.textInput.Text)));
		}
		this.CancelChatting();
	}

	private void Start()
	{
		ChatUI.singleton = this;
		this.textInput.Hide();
	}
}