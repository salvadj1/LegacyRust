using Facepunch.Cursor;
using System;
using System.Diagnostics;
using UnityEngine;

internal class ContextUI : MonoBehaviour
{
	[SerializeField]
	private GUISkin skin;

	[NonSerialized]
	internal UnlockCursorNode clientUnlock;

	[NonSerialized]
	internal ContextClientStage clientContext;

	[NonSerialized]
	internal MonoBehaviour clientQuery;

	[NonSerialized]
	internal ContextClientState _clientState;

	[NonSerialized]
	internal ulong clientQueryTime;

	[NonSerialized]
	internal string validatingString;

	[NonSerialized]
	internal int clientSelection;

	[NonSerialized]
	internal static ContextClientWorkingCallback clientWorkingCallbacks;

	private static GUIContent temp;

	internal ContextClientState clientState
	{
		get
		{
			return this._clientState;
		}
	}

	static ContextUI()
	{
		ContextUI.temp = new GUIContent();
	}

	public ContextUI()
	{
	}

	private void Awake()
	{
		base.useGUILayout = false;
		this.clientUnlock = LockCursorManager.CreateCursorUnlockNode(false, UnlockCursorFlags.DoNotResetInput, "Context Popup");
	}

	private static Rect BoxRect(Vector2 contentSize, GUIStyle box, out int xOffset, out int yOffset)
	{
		Rect rect = box.padding.Add(new Rect(0f, 0f, contentSize.x, contentSize.y));
		int num = Mathf.FloorToInt(((float)Screen.width - rect.width) * 0.5f);
		int num1 = Mathf.FloorToInt(((float)Screen.height - rect.height) * 0.5f);
		rect.x = rect.x + (float)num;
		rect.y = rect.y + (float)num1;
		Rect rect1 = box.padding.Remove(rect);
		xOffset = Mathf.FloorToInt(rect1.x);
		yOffset = Mathf.FloorToInt(rect1.y);
		return rect;
	}

	private int GUIOptions(GUIStyle box, GUIStyle button)
	{
		int num;
		int num1;
		Rect rect;
		int? nullable;
		int? nullable1;
		Rect[] rectArray = new Rect[this.clientContext.length];
		if (button.fixedWidth != 0f)
		{
			nullable = new int?((int)button.fixedWidth);
		}
		else
		{
			nullable = null;
		}
		int? nullable2 = nullable;
		if (button.fixedHeight != 0f)
		{
			nullable1 = new int?((int)button.fixedHeight);
		}
		else
		{
			nullable1 = null;
		}
		int? nullable3 = nullable1;
		float single = Single.NegativeInfinity;
		float single1 = 0f;
		for (int i = 0; i < this.clientContext.length; i++)
		{
			ContextUI.temp.text = this.clientContext.option[i].text;
			Vector2 vector2 = button.CalcSize(ContextUI.temp);
			RectOffset rectOffset = button.margin;
			Rect rect1 = button.padding.Add(new Rect(0f, 0f, (float)((!nullable2.HasValue ? Mathf.CeilToInt(vector2.x) : nullable2.Value)), (float)((!nullable3.HasValue ? Mathf.CeilToInt(vector2.y) : nullable3.Value))));
			rect = rect1;
			rectArray[i] = rect1;
			Rect rect2 = rectOffset.Add(rect);
			if (rect2.width > single)
			{
				single = rect2.width;
			}
			single1 = single1 + rect2.height;
		}
		GUI.Box(ContextUI.BoxRect(new Vector2(single, single1), box, out num, out num1), GUIContent.none, box);
		int num2 = -1;
		for (int j = 0; j < this.clientContext.length; j++)
		{
			Rect rect3 = button.margin.Add(rectArray[j]);
			rect3.width = single;
			rect3.x = (float)num;
			rect3.y = (float)num1;
			rect = button.margin.Add(rect3);
			num1 = Mathf.FloorToInt(rect.yMax);
			if (GUI.Button(button.margin.Remove(rect3), this.clientContext.option[j].text, button))
			{
				num2 = j;
			}
		}
		return num2;
	}

	private static void GUIString(string text, GUIStyle box)
	{
		int num;
		int num1;
		GUI.Box(ContextUI.BoxRect(box.CalcSize(ContextUI.temp), box, out num, out num1), ContextUI.temp, box);
	}

	[Conditional("CLIENT_POPUP_LOG")]
	private static void LOG(string shorthand, UnityEngine.Object contextual)
	{
	}

	private void OnClientHideMenu()
	{
		base.CancelInvoke("OnClientShowMenu");
		if ((int)this.clientUnlock.TryLock() == 0)
		{
			Context.UICommands.IsButtonHeld(true);
			Input.ResetInputAxes();
		}
		base.enabled = false;
	}

	private void OnClientOptionsCleared()
	{
		if (this.clientSelection != -1)
		{
			this.clientSelection = -1;
		}
		this.clientContext.length = 0;
	}

	private void OnClientOptionsMade()
	{
		if (this._clientState == ContextClientState.Validating)
		{
			return;
		}
		ulong num = NetCull.localTimeInMillis - this.clientQueryTime;
		if (num <= (long)300)
		{
			base.Invoke("OnClientShowMenu", (float)((double)((float)(num + (long)50)) / 1000));
		}
		else
		{
			this.OnClientShowMenu();
		}
		this.SetContextClientState(ContextClientState.Options);
	}

	private void OnClientPromptBegin(NetEntityID? useID)
	{
		NetEntityID value;
		if (!useID.HasValue)
		{
			NetEntityID.Of(this.clientQuery, out value);
		}
		else
		{
			value = useID.Value;
		}
		this.clientQueryTime = NetCull.localTimeInMillis;
		Context.UICommands.Issue_Request(value);
		this.SetContextClientState(ContextClientState.Polling);
	}

	private void OnClientPromptEnd()
	{
		this.OnClientHideMenu();
		this.SetContextClientState(ContextClientState.Off);
	}

	private void OnClientSelection(int i)
	{
		this.clientSelection = i;
		Context.UICommands.Issue_Selection(this.clientContext.option[i].name);
		this.validatingString = string.Concat(this.clientContext.option[i].text, "..");
		this.SetContextClientState(ContextClientState.Validating);
	}

	private void OnClientShowMenu()
	{
		this.clientSelection = -1;
		this.clientUnlock.On = true;
		base.enabled = true;
	}

	private void OnClientValidated()
	{
		this.SetContextClientState(ContextClientState.Off);
	}

	private void OnDestroy()
	{
		this.clientUnlock.Dispose();
		this.clientUnlock = null;
	}

	private void OnDisable()
	{
		LockCursorManager.DisableGUICheckOnDisable(this);
	}

	private void OnEnable()
	{
		LockCursorManager.DisableGUICheckOnEnable(this);
	}

	private void OnGUI()
	{
		GUI.depth = 1;
		GUI.skin = this.skin;
		GUIStyle gUIStyle = "ctxbox";
		GUIStyle gUIStyle1 = "ctxbutton";
		int num = -1;
		ContextClientState contextClientState = this.clientState;
		if (contextClientState == ContextClientState.Options)
		{
			num = this.GUIOptions(gUIStyle, gUIStyle1);
			if (num == -1 && NetCull.localTimeInMillis - this.clientQueryTime > (long)300 && !Context.UICommands.IsButtonHeld(false))
			{
				Context.EndQuery();
			}
		}
		else
		{
			if (contextClientState != ContextClientState.Validating)
			{
				return;
			}
			ContextUI.GUIString(this.validatingString, gUIStyle);
		}
		if (num != -1)
		{
			this.OnClientSelection(num);
		}
	}

	internal void OnServerCancel()
	{
		this.OnClientPromptEnd();
	}

	internal void OnServerCancelSent()
	{
		Context.UICommands.Issue_Cancel();
		if (this._clientState == ContextClientState.Options)
		{
			this.OnClientHideMenu();
		}
		this.SetContextClientState(ContextClientState.Validating);
	}

	internal void OnServerImmediate(bool success)
	{
		if (!success)
		{
			this.OnClientPromptEnd();
		}
		else
		{
			this.OnClientValidated();
			this.OnClientOptionsCleared();
			this.OnClientPromptEnd();
		}
	}

	internal void OnServerMenu(ContextMenuData menu)
	{
		this.clientContext.Set(menu);
		this.OnClientOptionsMade();
	}

	internal void OnServerNoOp()
	{
		this.clientContext.length = 0;
		this.OnClientPromptEnd();
	}

	internal void OnServerQuerySent(MonoBehaviour script, NetEntityID entID)
	{
		this.clientQuery = script;
		this.OnClientPromptBegin(new NetEntityID?(entID));
	}

	internal void OnServerQuickTapSent()
	{
		Context.UICommands.Issue_QuickTap();
		if (this._clientState == ContextClientState.Options)
		{
			this.OnClientHideMenu();
		}
		this.SetContextClientState(ContextClientState.Validating);
	}

	internal void OnServerRestartPolling()
	{
		this.OnClientOptionsCleared();
		this.SetContextClientState(ContextClientState.Polling);
		this.OnClientOptionsMade();
	}

	internal void OnServerSelection(bool success)
	{
		if (!success)
		{
			this.OnClientPromptEnd();
		}
		else
		{
			this.OnClientValidated();
			this.OnClientOptionsCleared();
			this.OnClientPromptEnd();
		}
	}

	internal void OnServerSelectionStale()
	{
		this.OnClientPromptEnd();
	}

	private void SetContextClientState(ContextClientState state)
	{
		if (this._clientState != state)
		{
			if (this._clientState == ContextClientState.Off)
			{
				this._clientState = state;
				if (ContextUI.clientWorkingCallbacks != null)
				{
					ContextUI.clientWorkingCallbacks(true);
				}
			}
			else if (state != ContextClientState.Off)
			{
				this._clientState = state;
			}
			else
			{
				this._clientState = state;
				if (ContextUI.clientWorkingCallbacks != null)
				{
					ContextUI.clientWorkingCallbacks(false);
				}
			}
		}
	}
}