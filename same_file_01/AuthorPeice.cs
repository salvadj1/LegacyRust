using System;
using UnityEngine;

public class AuthorPeice : AuthorShared
{
	[SerializeField]
	private AuthorCreation _creation;

	[SerializeField]
	private string _peiceID;

	private bool destroyed;

	public AuthorCreation creation
	{
		get
		{
			return this._creation;
		}
	}

	public string peiceID
	{
		get
		{
			return this._peiceID;
		}
		set
		{
			this._peiceID = value ?? string.Empty;
		}
	}

	public UnityEngine.Object selectReference
	{
		get
		{
			return base.gameObject;
		}
	}

	public AuthorPeice()
	{
	}

	protected static bool ActionButton(AuthorShared.Content content, ref AuthorShared.PeiceAction act, bool isSelected, AuthorShared.PeiceAction onAction, AuthorShared.PeiceAction offAction, GUIStyle style, params GUILayoutOption[] options)
	{
		if (AuthorShared.Toggle(content, isSelected, style, options) == isSelected)
		{
			return false;
		}
		act = (!isSelected ? onAction : offAction);
		return true;
	}

	protected static bool ActionButton(AuthorShared.Content content, ref AuthorShared.PeiceAction act, bool isSelected, AuthorShared.PeiceAction action, GUIStyle style, params GUILayoutOption[] options)
	{
		if (AuthorShared.Toggle(content, isSelected, style, options) == isSelected)
		{
			return false;
		}
		act = action;
		return true;
	}

	public void Delete()
	{
		if (!this.destroyed)
		{
			try
			{
				this.OnPeiceDestroy();
			}
			finally
			{
				this.destroyed = true;
				UnityEngine.Object.DestroyImmediate(this);
			}
		}
	}

	protected string FromRootBonePath(Transform transform)
	{
		if (!this.creation)
		{
			return string.Empty;
		}
		return this.creation.RootBonePath(this, transform);
	}

	protected virtual void OnDidUnRegister()
	{
	}

	public virtual void OnListClicked()
	{
		if (AuthorShared.SelectionContains(this.selectReference) || AuthorShared.SelectionContains(this))
		{
		}
	}

	protected virtual void OnPeiceDestroy()
	{
		if (this._creation)
		{
			this.OnWillUnRegister();
			this._creation.UnregisterPeice(this);
			this.OnDidUnRegister();
		}
	}

	protected virtual void OnRegistered()
	{
	}

	public virtual bool OnSceneView()
	{
		return false;
	}

	protected virtual void OnWillUnRegister()
	{
	}

	public virtual bool PeiceInspectorGUI()
	{
		AuthorShared.BeginHorizontal(AuthorShared.Styles.gradientInlineFill, new GUILayoutOption[0]);
		GUILayout.Space(48f);
		AuthorShared.Content content = AuthorShared.ObjectContent<Transform>(base.transform, typeof(Transform));
		if (GUILayout.Button(content.image, new GUILayoutOption[] { GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false) }))
		{
			AuthorShared.PingObject(this);
		}
		GUILayout.Space(10f);
		GUILayout.Label(this.peiceID, AuthorShared.Styles.boldLabel, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		AuthorShared.EndHorizontal();
		return false;
	}

	public virtual AuthorShared.PeiceAction PeiceListGUI()
	{
		bool flag = (AuthorShared.SelectionContains(this.selectReference) ? true : AuthorShared.SelectionContains(this));
		AuthorShared.PeiceAction peiceAction = AuthorShared.PeiceAction.None;
		AuthorShared.BeginHorizontal(new GUILayoutOption[0]);
		AuthorPeice.ActionButton(this.peiceID, ref peiceAction, flag, AuthorShared.PeiceAction.AddToSelection, AuthorShared.PeiceAction.RemoveFromSelection, AuthorShared.Styles.peiceButtonLeft, new GUILayoutOption[0]);
		AuthorPeice.ActionButton(AuthorShared.Icon.solo, ref peiceAction, flag, AuthorShared.PeiceAction.SelectSolo, AuthorShared.Styles.peiceButtonMid, new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		Color color = GUI.contentColor;
		GUI.contentColor = Color.red;
		AuthorPeice.ActionButton(AuthorShared.Icon.delete, ref peiceAction, flag, AuthorShared.PeiceAction.Delete, AuthorShared.Styles.peiceButtonRight, new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUI.contentColor = color;
		AuthorShared.EndHorizontal();
		return peiceAction;
	}

	public void Registered(AuthorCreation creation)
	{
		this._creation = creation;
		this._peiceID = this._peiceID ?? string.Empty;
		this.OnRegistered();
	}

	public virtual void SaveJsonProperties(JSONStream stream)
	{
	}
}