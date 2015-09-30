using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class UIButtonMessage : MonoBehaviour
{
	public GameObject target;

	public string functionName;

	public UIButtonMessage.Trigger trigger;

	public bool includeChildren;

	private bool mStarted;

	private bool mHighlighted;

	public UIButtonMessage()
	{
	}

	private void OnClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnClick)
		{
			this.Send();
		}
	}

	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnDoubleClick)
		{
			this.Send();
		}
	}

	private void OnEnable()
	{
		if (this.mStarted && this.mHighlighted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver || !isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)
			{
				this.Send();
			}
			this.mHighlighted = isOver;
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && (isPressed && this.trigger == UIButtonMessage.Trigger.OnPress || !isPressed && this.trigger == UIButtonMessage.Trigger.OnRelease))
		{
			this.Send();
		}
	}

	private void Send()
	{
		if (string.IsNullOrEmpty(this.functionName))
		{
			return;
		}
		if (this.target == null)
		{
			this.target = base.gameObject;
		}
		if (!this.includeChildren)
		{
			this.target.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
			int num = 0;
			int length = (int)componentsInChildren.Length;
			while (num < length)
			{
				Transform transforms = componentsInChildren[num];
				transforms.gameObject.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
				num++;
			}
		}
	}

	private void Start()
	{
		this.mStarted = true;
	}

	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick
	}
}