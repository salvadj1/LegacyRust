using NGUIHack;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Input (Basic)")]
public class UIInput : MonoBehaviour
{
	public static UIInput current;

	public UILabel label;

	public int maxChars;

	public bool inputMultiline;

	public UIInput.Validator validator;

	public UIInput.KeyboardType type;

	public bool isPassword;

	public Color activeColor = Color.white;

	public GameObject eventReceiver;

	public string functionName = "OnSubmit";

	public bool trippleClickSelect = true;

	private List<UITextMarkup> markups;

	private string mText = string.Empty;

	private string mDefaultText = string.Empty;

	private Color mDefaultColor = Color.white;

	private string mLastIME = string.Empty;

	public string inputText
	{
		get
		{
			return this.mText;
		}
	}

	public bool selected
	{
		get
		{
			return UICamera.selectedObject == base.gameObject;
		}
		set
		{
			if (!value && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			else if (value)
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			value = value ?? string.Empty;
			bool empty = (this.mText ?? string.Empty) != value;
			this.mText = value;
			if (this.label != null)
			{
				if (value != string.Empty)
				{
					value = this.mDefaultText;
				}
				this.label.supportEncoding = false;
				this.label.showLastPasswordChar = this.selected;
				this.label.color = (this.selected || value != this.mDefaultText ? this.activeColor : this.mDefaultColor);
				if (empty)
				{
					this.UpdateLabel();
				}
			}
		}
	}

	public UIInput()
	{
	}

	private void Awake()
	{
		this.markups = new List<UITextMarkup>();
		this.Init();
	}

	internal string CheckChanges(string newText)
	{
		if (this.mText == newText)
		{
			return this.mText;
		}
		this.mText = newText;
		this.UpdateLabel();
		return this.mText;
	}

	internal void CheckPositioning(int carratPos, int selectPos)
	{
		this.label.selection = this.label.ConvertUnprocessedSelection(carratPos, selectPos);
	}

	internal void GainFocus()
	{
		UIUnityEvents.TextGainFocus(this);
	}

	protected void Init()
	{
		if (this.label == null)
		{
			this.label = base.GetComponentInChildren<UILabel>();
		}
		if (this.label != null)
		{
			this.mDefaultText = this.label.text;
			this.mDefaultColor = this.label.color;
			this.label.supportEncoding = false;
		}
	}

	internal void LoseFocus()
	{
		UIUnityEvents.TextLostFocus(this);
	}

	private void OnDisable()
	{
		if (UICamera.IsHighlighted(base.gameObject))
		{
			this.OnSelect(false);
		}
	}

	private void OnEnable()
	{
		if (UICamera.IsHighlighted(base.gameObject))
		{
			this.OnSelect(true);
		}
	}

	internal void OnEvent(UICamera camera, NGUIHack.Event @event, EventType type)
	{
		switch (type)
		{
			case EventType.MouseDown:
			{
				if (@event.button == 0)
				{
					UIUnityEvents.TextClickDown(camera, this, @event, this.label);
				}
				return;
			}
			case EventType.MouseUp:
			{
				if (@event.button != 0)
				{
					Debug.Log("Wee");
				}
				else
				{
					UIUnityEvents.TextClickUp(camera, this, @event, this.label);
				}
				return;
			}
			case EventType.MouseMove:
			{
				return;
			}
			case EventType.MouseDrag:
			{
				if (@event.button == 0)
				{
					UIUnityEvents.TextDrag(camera, this, @event, this.label);
				}
				return;
			}
			case EventType.KeyDown:
			{
				UIUnityEvents.TextKeyDown(camera, this, @event, this.label);
				return;
			}
			case EventType.KeyUp:
			{
				UIUnityEvents.TextKeyUp(camera, this, @event, this.label);
				return;
			}
			default:
			{
				return;
			}
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (this.label != null && base.enabled && base.gameObject.activeInHierarchy)
		{
			if (!isSelected)
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.label.text = this.mText;
				}
				else
				{
					this.label.text = this.mDefaultText;
					this.label.color = this.mDefaultColor;
					if (this.isPassword)
					{
						this.label.password = false;
					}
				}
				this.label.showLastPasswordChar = false;
			}
			else
			{
				this.mText = (this.label.text != this.mDefaultText ? this.label.text : string.Empty);
				this.label.color = this.activeColor;
				if (this.isPassword)
				{
					this.label.password = true;
				}
				Transform transforms = this.label.cachedTransform;
				Vector3 vector3 = this.label.pivotOffset;
				vector3.y = vector3.y + this.label.relativeSize.y;
				vector3 = transforms.TransformPoint(vector3);
				this.UpdateLabel();
			}
		}
	}

	public bool RequestKeyboardFocus()
	{
		return UIUnityEvents.RequestKeyboardFocus(this);
	}

	public bool SendSubmitMessage()
	{
		if (this.eventReceiver == null)
		{
			this.eventReceiver = base.gameObject;
		}
		string str = this.mText;
		this.eventReceiver.SendMessage(this.functionName, SendMessageOptions.DontRequireReceiver);
		return str != this.mText;
	}

	private void Update()
	{
	}

	internal void UpdateLabel()
	{
		string str;
		if (this.maxChars > 0 && this.mText.Length > this.maxChars)
		{
			this.mText = this.mText.Substring(0, this.maxChars);
		}
		if (this.label.font != null)
		{
			str = (!this.selected ? this.mText : string.Concat(this.mText, this.mLastIME));
			this.label.supportEncoding = false;
			UIFont uIFont = this.label.font;
			List<UITextMarkup> uITextMarkups = this.markups;
			float single = (float)this.label.lineWidth;
			Vector3 vector3 = this.label.cachedTransform.localScale;
			str = uIFont.WrapText(uITextMarkups, str, single / vector3.x, 0, false, UIFont.SymbolStyle.None);
			this.markups.SortMarkup();
			this.label.text = str;
			this.label.showLastPasswordChar = this.selected;
		}
	}

	public enum KeyboardType
	{
		Default,
		ASCIICapable,
		NumbersAndPunctuation,
		URL,
		NumberPad,
		PhonePad,
		NamePhonePad,
		EmailAddress
	}

	public delegate char Validator(string currentText, char nextChar);
}