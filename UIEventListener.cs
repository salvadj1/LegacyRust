using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	public object parameter;

	public UIEventListener.VoidDelegate onSubmit;

	public UIEventListener.VoidDelegate onClick;

	public UIEventListener.VoidDelegate onDoubleClick;

	public UIEventListener.BoolDelegate onHover;

	public UIEventListener.BoolDelegate onPress;

	public UIEventListener.BoolDelegate onSelect;

	public UIEventListener.FloatDelegate onScroll;

	public UIEventListener.VectorDelegate onDrag;

	public UIEventListener.ObjectDelegate onDrop;

	public UIEventListener.StringDelegate onInput;

	public UIEventListener()
	{
	}

	[Obsolete("Please use UIEventListener.Get instead of UIEventListener.Add")]
	public static UIEventListener Add(GameObject go)
	{
		return UIEventListener.Get(go);
	}

	public static UIEventListener Get(GameObject go)
	{
		UIEventListener component = go.GetComponent<UIEventListener>();
		if (component == null)
		{
			component = go.AddComponent<UIEventListener>();
		}
		return component;
	}

	private void OnClick()
	{
		if (this.onClick != null)
		{
			this.onClick(base.gameObject);
		}
	}

	private void OnDoubleClick()
	{
		if (this.onDoubleClick != null)
		{
			this.onDoubleClick(base.gameObject);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag != null)
		{
			this.onDrag(base.gameObject, delta);
		}
	}

	private void OnDrop(GameObject go)
	{
		if (this.onDrop != null)
		{
			this.onDrop(base.gameObject, go);
		}
	}

	private void OnHover(bool isOver)
	{
		if (this.onHover != null)
		{
			this.onHover(base.gameObject, isOver);
		}
	}

	private void OnInput(string text)
	{
		if (this.onInput != null)
		{
			this.onInput(base.gameObject, text);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (this.onPress != null)
		{
			this.onPress(base.gameObject, isPressed);
		}
	}

	private void OnScroll(float delta)
	{
		if (this.onScroll != null)
		{
			this.onScroll(base.gameObject, delta);
		}
	}

	private void OnSelect(bool selected)
	{
		if (this.onSelect != null)
		{
			this.onSelect(base.gameObject, selected);
		}
	}

	private void OnSubmit()
	{
		if (this.onSubmit != null)
		{
			this.onSubmit(base.gameObject);
		}
	}

	public delegate void BoolDelegate(GameObject go, bool state);

	public delegate void FloatDelegate(GameObject go, float delta);

	public delegate void ObjectDelegate(GameObject go, GameObject draggedObject);

	public delegate void StringDelegate(GameObject go, string text);

	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	public delegate void VoidDelegate(GameObject go);
}