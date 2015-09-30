using AnimationOrTween;
using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox")]
public class UICheckbox : MonoBehaviour
{
	public static UICheckbox current;

	public UISprite checkSprite;

	public Animation checkAnimation;

	public GameObject eventReceiver;

	public string functionName = "OnActivate";

	public bool startsChecked = true;

	public Transform radioButtonRoot;

	public bool optionCanBeNone;

	[HideInInspector]
	[SerializeField]
	private bool option;

	private bool mChecked = true;

	private bool mStarted;

	private Transform mTrans;

	public bool isChecked
	{
		get
		{
			return this.mChecked;
		}
		set
		{
			if (this.radioButtonRoot == null || value || this.optionCanBeNone || !this.mStarted)
			{
				this.Set(value);
			}
		}
	}

	public UICheckbox()
	{
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		if (this.checkSprite != null)
		{
			this.checkSprite.alpha = (!this.startsChecked ? 0f : 1f);
		}
		if (this.option)
		{
			this.option = false;
			if (this.radioButtonRoot == null)
			{
				this.radioButtonRoot = this.mTrans.parent;
			}
		}
	}

	private void OnClick()
	{
		if (base.enabled)
		{
			this.isChecked = !this.isChecked;
		}
	}

	private void Set(bool state)
	{
		if (!this.mStarted)
		{
			this.mChecked = state;
			this.startsChecked = state;
			if (this.checkSprite != null)
			{
				this.checkSprite.alpha = (!state ? 0f : 1f);
			}
		}
		else if (this.mChecked != state)
		{
			if (this.radioButtonRoot != null && state)
			{
				UICheckbox[] componentsInChildren = this.radioButtonRoot.GetComponentsInChildren<UICheckbox>(true);
				int num = 0;
				int length = (int)componentsInChildren.Length;
				while (num < length)
				{
					UICheckbox uICheckbox = componentsInChildren[num];
					if (uICheckbox != this && uICheckbox.radioButtonRoot == this.radioButtonRoot)
					{
						uICheckbox.Set(false);
					}
					num++;
				}
			}
			this.mChecked = state;
			if (this.checkSprite != null)
			{
				Color color = this.checkSprite.color;
				color.a = (!this.mChecked ? 0f : 1f);
				TweenColor.Begin(this.checkSprite.gameObject, 0.2f, color);
			}
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				UICheckbox.current = this;
				this.eventReceiver.SendMessage(this.functionName, this.mChecked, SendMessageOptions.DontRequireReceiver);
			}
			if (this.checkAnimation != null)
			{
				ActiveAnimation.Play(this.checkAnimation, (!state ? Direction.Reverse : Direction.Forward));
			}
		}
	}

	private void Start()
	{
		if (this.eventReceiver == null)
		{
			this.eventReceiver = base.gameObject;
		}
		this.mChecked = !this.startsChecked;
		this.mStarted = true;
		this.Set(this.startsChecked);
	}
}