using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Textbox")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfTextbox : dfInteractiveBase, IDFMultiRender
{
	[SerializeField]
	protected dfFontBase font;

	[SerializeField]
	protected bool acceptsTab;

	[SerializeField]
	protected bool displayAsPassword;

	[SerializeField]
	protected string passwordChar = "*";

	[SerializeField]
	protected bool readOnly;

	[SerializeField]
	protected string text = string.Empty;

	[SerializeField]
	protected Color32 textColor = UnityEngine.Color.white;

	[SerializeField]
	protected Color32 selectionBackground = new Color32(0, 105, 210, 255);

	[SerializeField]
	protected string selectionSprite = string.Empty;

	[SerializeField]
	protected float textScale = 1f;

	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	[SerializeField]
	protected RectOffset padding = new RectOffset();

	[SerializeField]
	protected float cursorBlinkTime = 0.45f;

	[SerializeField]
	protected int cursorWidth = 1;

	[SerializeField]
	protected int maxLength = 1024;

	[SerializeField]
	protected bool selectOnFocus;

	[SerializeField]
	protected bool shadow;

	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	[SerializeField]
	protected bool useMobileKeyboard;

	[SerializeField]
	protected int mobileKeyboardType;

	[SerializeField]
	protected bool mobileAutoCorrect;

	[SerializeField]
	protected bool mobileHideInputField;

	[SerializeField]
	protected dfMobileKeyboardTrigger mobileKeyboardTrigger;

	[SerializeField]
	protected UnityEngine.TextAlignment textAlign;

	private Vector2 startSize = Vector2.zero;

	private int selectionStart;

	private int selectionEnd;

	private int mouseSelectionAnchor;

	private int scrollIndex;

	private int cursorIndex;

	private float leftOffset;

	private bool cursorShown;

	private float[] charWidths;

	private float whenGotFocus;

	private string undoText = string.Empty;

	private dfRenderData textRenderData;

	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();

	private PropertyChangedEventHandler<bool> ReadOnlyChanged;

	private PropertyChangedEventHandler<string> PasswordCharacterChanged;

	private PropertyChangedEventHandler<string> TextChanged;

	private PropertyChangedEventHandler<string> TextSubmitted;

	private PropertyChangedEventHandler<string> TextCancelled;

	public float CursorBlinkTime
	{
		get
		{
			return this.cursorBlinkTime;
		}
		set
		{
			this.cursorBlinkTime = value;
		}
	}

	public int CursorIndex
	{
		get
		{
			return this.cursorIndex;
		}
		set
		{
			value = Mathf.Max(0, value);
			value = Mathf.Min(0, this.text.Length - 1);
			this.cursorIndex = value;
		}
	}

	public int CursorWidth
	{
		get
		{
			return this.cursorWidth;
		}
		set
		{
			this.cursorWidth = value;
		}
	}

	public dfFontBase Font
	{
		get
		{
			if (this.font == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					this.font = manager.DefaultFont;
				}
			}
			return this.font;
		}
		set
		{
			if (value != this.font)
			{
				this.font = value;
				this.Invalidate();
			}
		}
	}

	public bool HideMobileInputField
	{
		get
		{
			return this.mobileHideInputField;
		}
		set
		{
			this.mobileHideInputField = value;
		}
	}

	public bool IsPasswordField
	{
		get
		{
			return this.displayAsPassword;
		}
		set
		{
			if (value != this.displayAsPassword)
			{
				this.displayAsPassword = value;
				this.Invalidate();
			}
		}
	}

	public int MaxLength
	{
		get
		{
			return this.maxLength;
		}
		set
		{
			if (value != this.maxLength)
			{
				this.maxLength = Mathf.Max(0, value);
				if (this.maxLength < this.text.Length)
				{
					this.Text = this.text.Substring(0, this.maxLength);
				}
				this.Invalidate();
			}
		}
	}

	public bool MobileAutoCorrect
	{
		get
		{
			return this.mobileAutoCorrect;
		}
		set
		{
			this.mobileAutoCorrect = value;
		}
	}

	public dfMobileKeyboardTrigger MobileKeyboardTrigger
	{
		get
		{
			return this.mobileKeyboardTrigger;
		}
		set
		{
			this.mobileKeyboardTrigger = value;
		}
	}

	public RectOffset Padding
	{
		get
		{
			if (this.padding == null)
			{
				this.padding = new RectOffset();
			}
			return this.padding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.padding))
			{
				this.padding = value;
				this.Invalidate();
			}
		}
	}

	public string PasswordCharacter
	{
		get
		{
			return this.passwordChar;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				this.passwordChar = value;
			}
			else
			{
				this.passwordChar = value[0].ToString();
			}
			this.OnPasswordCharacterChanged();
			this.Invalidate();
		}
	}

	public bool ReadOnly
	{
		get
		{
			return this.readOnly;
		}
		set
		{
			if (value != this.readOnly)
			{
				this.readOnly = value;
				this.OnReadOnlyChanged();
				this.Invalidate();
			}
		}
	}

	public string SelectedText
	{
		get
		{
			if (this.selectionEnd == this.selectionStart)
			{
				return string.Empty;
			}
			return this.text.Substring(this.selectionStart, this.selectionEnd - this.selectionStart);
		}
	}

	public Color32 SelectionBackgroundColor
	{
		get
		{
			return this.selectionBackground;
		}
		set
		{
			this.selectionBackground = value;
			this.Invalidate();
		}
	}

	public int SelectionEnd
	{
		get
		{
			return this.selectionEnd;
		}
		set
		{
			if (value != this.selectionEnd)
			{
				this.selectionEnd = Mathf.Max(0, Mathf.Min(value, this.text.Length));
				this.selectionStart = Mathf.Max(this.selectionStart, this.selectionEnd);
				this.Invalidate();
			}
		}
	}

	public int SelectionLength
	{
		get
		{
			return this.selectionEnd - this.selectionStart;
		}
	}

	public string SelectionSprite
	{
		get
		{
			return this.selectionSprite;
		}
		set
		{
			if (value != this.selectionSprite)
			{
				this.selectionSprite = value;
				this.Invalidate();
			}
		}
	}

	public int SelectionStart
	{
		get
		{
			return this.selectionStart;
		}
		set
		{
			if (value != this.selectionStart)
			{
				this.selectionStart = Mathf.Max(0, Mathf.Min(value, this.text.Length));
				this.selectionEnd = Mathf.Max(this.selectionEnd, this.selectionStart);
				this.Invalidate();
			}
		}
	}

	public bool SelectOnFocus
	{
		get
		{
			return this.selectOnFocus;
		}
		set
		{
			this.selectOnFocus = value;
		}
	}

	public bool Shadow
	{
		get
		{
			return this.shadow;
		}
		set
		{
			if (value != this.shadow)
			{
				this.shadow = value;
				this.Invalidate();
			}
		}
	}

	public Color32 ShadowColor
	{
		get
		{
			return this.shadowColor;
		}
		set
		{
			if (!value.Equals(this.shadowColor))
			{
				this.shadowColor = value;
				this.Invalidate();
			}
		}
	}

	public Vector2 ShadowOffset
	{
		get
		{
			return this.shadowOffset;
		}
		set
		{
			if (value != this.shadowOffset)
			{
				this.shadowOffset = value;
				this.Invalidate();
			}
		}
	}

	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (value.Length > this.MaxLength)
			{
				value = value.Substring(0, this.MaxLength);
			}
			value = value.Replace("\t", " ");
			if (value != this.text)
			{
				this.text = value;
				int num = 0;
				int num1 = num;
				this.cursorIndex = num;
				this.scrollIndex = num1;
				this.OnTextChanged();
				this.Invalidate();
			}
		}
	}

	public UnityEngine.TextAlignment TextAlignment
	{
		get
		{
			return this.textAlign;
		}
		set
		{
			if (value != this.textAlign)
			{
				this.textAlign = value;
				this.Invalidate();
			}
		}
	}

	public Color32 TextColor
	{
		get
		{
			return this.textColor;
		}
		set
		{
			this.textColor = value;
			this.Invalidate();
		}
	}

	public float TextScale
	{
		get
		{
			return this.textScale;
		}
		set
		{
			value = Mathf.Max(0.1f, value);
			if (!Mathf.Approximately(this.textScale, value))
			{
				this.textScale = value;
				this.Invalidate();
			}
		}
	}

	public dfTextScaleMode TextScaleMode
	{
		get
		{
			return this.textScaleMode;
		}
		set
		{
			this.textScaleMode = value;
			this.Invalidate();
		}
	}

	public bool UseMobileKeyboard
	{
		get
		{
			return this.useMobileKeyboard;
		}
		set
		{
			this.useMobileKeyboard = value;
		}
	}

	public dfTextbox()
	{
	}

	private void addQuadIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] numArray = new int[] { 0, 1, 3, 3, 1, 2 };
		for (int i = 0; i < (int)numArray.Length; i++)
		{
			triangles.Add(count + numArray[i]);
		}
	}

	public override void Awake()
	{
		base.Awake();
		this.startSize = base.Size;
	}

	public void ClearSelection()
	{
		this.selectionStart = 0;
		this.selectionEnd = 0;
		this.mouseSelectionAnchor = 0;
	}

	private void copySelectionToClipboard()
	{
		if (this.selectionStart == this.selectionEnd)
		{
			return;
		}
		dfClipboardHelper.clipBoard = this.text.Substring(this.selectionStart, this.selectionEnd - this.selectionStart);
	}

	private void cutSelectionToClipboard()
	{
		this.copySelectionToClipboard();
		this.deleteSelection();
	}

	private void deleteNextChar()
	{
		this.ClearSelection();
		if (this.cursorIndex >= this.text.Length)
		{
			return;
		}
		this.text = this.text.Remove(this.cursorIndex, 1);
		this.cursorShown = true;
		this.OnTextChanged();
		this.Invalidate();
	}

	private void deleteNextWord()
	{
		this.ClearSelection();
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		int length = this.findNextWord(this.cursorIndex);
		if (length == this.cursorIndex)
		{
			length = this.text.Length;
		}
		this.text = this.text.Remove(this.cursorIndex, length - this.cursorIndex);
		this.OnTextChanged();
		this.Invalidate();
	}

	private void deletePreviousChar()
	{
		if (this.selectionStart != this.selectionEnd)
		{
			int num = this.selectionStart;
			this.deleteSelection();
			this.setCursorPos(num);
			return;
		}
		this.ClearSelection();
		if (this.cursorIndex == 0)
		{
			return;
		}
		this.text = this.text.Remove(this.cursorIndex - 1, 1);
		dfTextbox _dfTextbox = this;
		_dfTextbox.cursorIndex = _dfTextbox.cursorIndex - 1;
		this.cursorShown = true;
		this.OnTextChanged();
		this.Invalidate();
	}

	private void deletePreviousWord()
	{
		this.ClearSelection();
		if (this.cursorIndex == 0)
		{
			return;
		}
		int num = this.findPreviousWord(this.cursorIndex);
		if (num == this.cursorIndex)
		{
			num = 0;
		}
		this.text = this.text.Remove(num, this.cursorIndex - num);
		this.setCursorPos(num);
		this.OnTextChanged();
		this.Invalidate();
	}

	private void deleteSelection()
	{
		if (this.selectionStart == this.selectionEnd)
		{
			return;
		}
		this.text = this.text.Remove(this.selectionStart, this.selectionEnd - this.selectionStart);
		this.setCursorPos(this.selectionStart);
		this.ClearSelection();
		this.OnTextChanged();
		this.Invalidate();
	}

	[DebuggerHidden]
	private IEnumerator doCursorBlink()
	{
		dfTextbox.<doCursorBlink>c__Iterator42 variable = null;
		return variable;
	}

	private int findNextWord(int startIndex)
	{
		int length = this.text.Length;
		int num = startIndex;
		int num1 = num;
		while (num1 < length)
		{
			char chr = this.text[num1];
			if (char.IsWhiteSpace(chr) || char.IsSeparator(chr) || char.IsPunctuation(chr))
			{
				num = num1;
				break;
			}
			else
			{
				num1++;
			}
		}
		while (num < length)
		{
			char chr1 = this.text[num];
			if (char.IsWhiteSpace(chr1) || char.IsSeparator(chr1) || char.IsPunctuation(chr1))
			{
				num++;
			}
			else
			{
				break;
			}
		}
		return num;
	}

	private int findPreviousWord(int startIndex)
	{
		int num = startIndex;
		while (num > 0)
		{
			char chr = this.text[num - 1];
			if (char.IsWhiteSpace(chr) || char.IsSeparator(chr) || char.IsPunctuation(chr))
			{
				num--;
			}
			else
			{
				break;
			}
		}
		int num1 = num;
		while (num1 >= 0)
		{
			if (num1 != 0)
			{
				char chr1 = this.text[num1 - 1];
				if (char.IsWhiteSpace(chr1) || char.IsSeparator(chr1) || char.IsPunctuation(chr1))
				{
					num = num1;
					break;
				}
				else
				{
					num1--;
				}
			}
			else
			{
				num = 0;
				break;
			}
		}
		return num;
	}

	private int getCharIndexOfMouse(dfMouseEventArgs args)
	{
		Vector2 hitPosition = base.GetHitPosition(args);
		float units = base.PixelsToUnits();
		int num = this.scrollIndex;
		float single = this.leftOffset / units;
		for (int i = this.scrollIndex; i < (int)this.charWidths.Length; i++)
		{
			single = single + this.charWidths[i] / units;
			if (single < hitPosition.x)
			{
				num++;
			}
		}
		return num;
	}

	private float getTextScaleMultiplier()
	{
		if (this.textScaleMode == dfTextScaleMode.None || !Application.isPlaying)
		{
			return 1f;
		}
		if (this.textScaleMode == dfTextScaleMode.ScreenResolution)
		{
			return (float)Screen.height / (float)this.manager.FixedHeight;
		}
		return base.Size.y / this.startSize.y;
	}

	private void moveSelectionPointLeft()
	{
		if (this.cursorIndex == 0)
		{
			return;
		}
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionEnd = this.cursorIndex;
			this.selectionStart = this.cursorIndex - 1;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			dfTextbox _dfTextbox = this;
			_dfTextbox.selectionEnd = _dfTextbox.selectionEnd - 1;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			dfTextbox _dfTextbox1 = this;
			_dfTextbox1.selectionStart = _dfTextbox1.selectionStart - 1;
		}
		this.setCursorPos(this.cursorIndex - 1);
	}

	private void moveSelectionPointLeftWord()
	{
		if (this.cursorIndex == 0)
		{
			return;
		}
		int num = this.findPreviousWord(this.cursorIndex);
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionEnd = this.cursorIndex;
			this.selectionStart = num;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			this.selectionEnd = num;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			this.selectionStart = num;
		}
		this.setCursorPos(num);
	}

	private void moveSelectionPointRight()
	{
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionEnd = this.cursorIndex + 1;
			this.selectionStart = this.cursorIndex;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			dfTextbox _dfTextbox = this;
			_dfTextbox.selectionEnd = _dfTextbox.selectionEnd + 1;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			dfTextbox _dfTextbox1 = this;
			_dfTextbox1.selectionStart = _dfTextbox1.selectionStart + 1;
		}
		this.setCursorPos(this.cursorIndex + 1);
	}

	private void moveSelectionPointRightWord()
	{
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		int num = this.findNextWord(this.cursorIndex);
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionStart = this.cursorIndex;
			this.selectionEnd = num;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			this.selectionEnd = num;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			this.selectionStart = num;
		}
		this.setCursorPos(num);
	}

	private void moveToEnd()
	{
		this.ClearSelection();
		this.setCursorPos(this.text.Length);
	}

	private void moveToNextChar()
	{
		this.ClearSelection();
		this.setCursorPos(this.cursorIndex + 1);
	}

	private void moveToNextWord()
	{
		this.ClearSelection();
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		this.setCursorPos(this.findNextWord(this.cursorIndex));
	}

	private void moveToPreviousChar()
	{
		this.ClearSelection();
		this.setCursorPos(this.cursorIndex - 1);
	}

	private void moveToPreviousWord()
	{
		this.ClearSelection();
		if (this.cursorIndex == 0)
		{
			return;
		}
		this.setCursorPos(this.findPreviousWord(this.cursorIndex));
	}

	private void moveToStart()
	{
		this.ClearSelection();
		this.setCursorPos(0);
	}

	protected internal virtual void OnCancel()
	{
		this.text = this.undoText;
		base.SignalHierarchy("OnTextCancelled", new object[] { this, this.text });
		if (this.TextCancelled != null)
		{
			this.TextCancelled(this, this.text);
		}
	}

	protected internal override void OnDoubleClick(dfMouseEventArgs args)
	{
		if (args.Source != this)
		{
			base.OnDoubleClick(args);
			return;
		}
		if (!this.ReadOnly && this.HasFocus && args.Buttons.IsSet(dfMouseButtons.Left) && Time.realtimeSinceStartup - this.whenGotFocus > 0.5f)
		{
			this.selectWordAtIndex(this.getCharIndexOfMouse(args));
		}
		base.OnDoubleClick(args);
	}

	public override void OnEnable()
	{
		bool flag;
		if (this.padding == null)
		{
			this.padding = new RectOffset();
		}
		base.OnEnable();
		if (this.size.magnitude == 0f)
		{
			base.Size = new Vector2(100f, 20f);
		}
		this.cursorShown = false;
		int num = 0;
		int num1 = num;
		this.scrollIndex = num;
		this.cursorIndex = num1;
		flag = (this.Font == null ? false : this.Font.IsValid);
		if (Application.isPlaying && !flag)
		{
			this.Font = base.GetManager().DefaultFont;
		}
	}

	protected internal override void OnEnterFocus(dfFocusEventArgs args)
	{
		base.OnEnterFocus(args);
		this.undoText = this.Text;
		if (!this.ReadOnly)
		{
			this.whenGotFocus = Time.realtimeSinceStartup;
			base.StartCoroutine(this.doCursorBlink());
			if (!this.selectOnFocus)
			{
				int num = 0;
				int num1 = num;
				this.selectionEnd = num;
				this.selectionStart = num1;
			}
			else
			{
				this.selectionStart = 0;
				this.selectionEnd = this.text.Length;
			}
		}
		this.Invalidate();
	}

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (this.ReadOnly)
		{
			return;
		}
		base.OnKeyDown(args);
		if (args.Used)
		{
			return;
		}
		KeyCode keyCode = args.KeyCode;
		switch (keyCode)
		{
			case KeyCode.RightArrow:
			{
				if (args.Control)
				{
					if (!args.Shift)
					{
						this.moveToNextWord();
					}
					else
					{
						this.moveSelectionPointRightWord();
					}
				}
				else if (!args.Shift)
				{
					this.moveToNextChar();
				}
				else
				{
					this.moveSelectionPointRight();
				}
				break;
			}
			case KeyCode.LeftArrow:
			{
				if (args.Control)
				{
					if (!args.Shift)
					{
						this.moveToPreviousWord();
					}
					else
					{
						this.moveSelectionPointLeftWord();
					}
				}
				else if (!args.Shift)
				{
					this.moveToPreviousChar();
				}
				else
				{
					this.moveSelectionPointLeft();
				}
				break;
			}
			case KeyCode.Insert:
			{
				if (args.Shift)
				{
					string str = dfClipboardHelper.clipBoard;
					if (!string.IsNullOrEmpty(str))
					{
						this.pasteAtCursor(str);
					}
				}
				break;
			}
			case KeyCode.Home:
			{
				if (!args.Shift)
				{
					this.moveToStart();
				}
				else
				{
					this.selectToStart();
				}
				break;
			}
			case KeyCode.End:
			{
				if (!args.Shift)
				{
					this.moveToEnd();
				}
				else
				{
					this.selectToEnd();
				}
				break;
			}
			default:
			{
				switch (keyCode)
				{
					case KeyCode.A:
					{
						if (args.Control)
						{
							this.selectAll();
						}
						break;
					}
					case KeyCode.C:
					{
						if (args.Control)
						{
							this.copySelectionToClipboard();
						}
						break;
					}
					default:
					{
						switch (keyCode)
						{
							case KeyCode.V:
							{
								if (args.Control)
								{
									string str1 = dfClipboardHelper.clipBoard;
									if (!string.IsNullOrEmpty(str1))
									{
										this.pasteAtCursor(str1);
									}
								}
								break;
							}
							case KeyCode.X:
							{
								if (args.Control)
								{
									this.cutSelectionToClipboard();
								}
								break;
							}
							default:
							{
								if (keyCode == KeyCode.Backspace)
								{
									if (!args.Control)
									{
										this.deletePreviousChar();
									}
									else
									{
										this.deletePreviousWord();
									}
								}
								else if (keyCode == KeyCode.Return)
								{
									this.OnSubmit();
								}
								else if (keyCode == KeyCode.Escape)
								{
									this.ClearSelection();
									int num = 0;
									int num1 = num;
									this.scrollIndex = num;
									this.cursorIndex = num1;
									this.Invalidate();
									this.OnCancel();
								}
								else
								{
									if (keyCode != KeyCode.Delete)
									{
										base.OnKeyDown(args);
										return;
									}
									if (this.selectionStart != this.selectionEnd)
									{
										this.deleteSelection();
									}
									else if (!args.Control)
									{
										this.deleteNextChar();
									}
									else
									{
										this.deleteNextWord();
									}
								}
								break;
							}
						}
						break;
					}
				}
				break;
			}
		}
		args.Use();
	}

	protected internal override void OnKeyPress(dfKeyEventArgs args)
	{
		if (this.ReadOnly || char.IsControl(args.Character))
		{
			base.OnKeyPress(args);
			return;
		}
		base.OnKeyPress(args);
		if (args.Used)
		{
			return;
		}
		this.processKeyPress(args);
	}

	protected internal override void OnLeaveFocus(dfFocusEventArgs args)
	{
		base.OnLeaveFocus(args);
		this.cursorShown = false;
		this.ClearSelection();
		this.Invalidate();
		this.whenGotFocus = 0f;
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		bool flag;
		if (args.Source != this)
		{
			base.OnMouseDown(args);
			return;
		}
		if (this.ReadOnly || !args.Buttons.IsSet(dfMouseButtons.Left))
		{
			flag = false;
		}
		else
		{
			flag = (this.HasFocus || this.SelectOnFocus ? Time.realtimeSinceStartup - this.whenGotFocus > 0.25f : true);
		}
		if (flag)
		{
			int charIndexOfMouse = this.getCharIndexOfMouse(args);
			if (charIndexOfMouse != this.cursorIndex)
			{
				this.cursorIndex = charIndexOfMouse;
				this.cursorShown = true;
				this.Invalidate();
				args.Use();
			}
			this.mouseSelectionAnchor = this.cursorIndex;
			int num = this.cursorIndex;
			int num1 = num;
			this.selectionEnd = num;
			this.selectionStart = num1;
		}
		base.OnMouseDown(args);
	}

	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if (args.Source != this)
		{
			base.OnMouseMove(args);
			return;
		}
		if (!this.ReadOnly && this.HasFocus && args.Buttons.IsSet(dfMouseButtons.Left))
		{
			int charIndexOfMouse = this.getCharIndexOfMouse(args);
			if (charIndexOfMouse != this.cursorIndex)
			{
				this.cursorIndex = charIndexOfMouse;
				this.cursorShown = true;
				this.Invalidate();
				args.Use();
				this.selectionStart = Mathf.Min(this.mouseSelectionAnchor, charIndexOfMouse);
				this.selectionEnd = Mathf.Max(this.mouseSelectionAnchor, charIndexOfMouse);
				return;
			}
		}
		base.OnMouseMove(args);
	}

	protected internal virtual void OnPasswordCharacterChanged()
	{
		if (this.PasswordCharacterChanged != null)
		{
			this.PasswordCharacterChanged(this, this.passwordChar);
		}
	}

	protected internal virtual void OnReadOnlyChanged()
	{
		if (this.ReadOnlyChanged != null)
		{
			this.ReadOnlyChanged(this, this.readOnly);
		}
	}

	protected internal virtual void OnSubmit()
	{
		base.SignalHierarchy("OnTextSubmitted", new object[] { this, this.text });
		if (this.TextSubmitted != null)
		{
			this.TextSubmitted(this, this.text);
		}
	}

	protected override void OnTabKeyPressed(dfKeyEventArgs args)
	{
		if (!this.acceptsTab)
		{
			base.OnTabKeyPressed(args);
		}
		else
		{
			base.OnKeyPress(args);
			if (args.Used)
			{
				return;
			}
			args.Character = '\t';
			this.processKeyPress(args);
		}
	}

	protected internal virtual void OnTextChanged()
	{
		base.SignalHierarchy("OnTextChanged", new object[] { this.text });
		if (this.TextChanged != null)
		{
			this.TextChanged(this, this.text);
		}
	}

	private string passwordDisplayText()
	{
		return new string(this.passwordChar[0], this.text.Length);
	}

	private void pasteAtCursor(string clipData)
	{
		this.deleteSelection();
		StringBuilder stringBuilder = new StringBuilder(this.text.Length + clipData.Length);
		stringBuilder.Append(this.text);
		for (int i = 0; i < clipData.Length; i++)
		{
			char chr = clipData[i];
			if (chr >= ' ')
			{
				dfTextbox _dfTextbox = this;
				int num = _dfTextbox.cursorIndex;
				int num1 = num;
				_dfTextbox.cursorIndex = num + 1;
				stringBuilder.Insert(num1, chr);
			}
		}
		stringBuilder.Length = Mathf.Min(stringBuilder.Length, this.maxLength);
		this.text = stringBuilder.ToString();
		this.setCursorPos(this.cursorIndex);
		this.OnTextChanged();
		this.Invalidate();
	}

	private void processKeyPress(dfKeyEventArgs args)
	{
		this.deleteSelection();
		if (this.text.Length < this.MaxLength)
		{
			if (this.cursorIndex != this.text.Length)
			{
				string str = this.text;
				int num = this.cursorIndex;
				char character = args.Character;
				this.text = str.Insert(num, character.ToString());
			}
			else
			{
				dfTextbox _dfTextbox = this;
				_dfTextbox.text = string.Concat(_dfTextbox.text, args.Character);
			}
			dfTextbox _dfTextbox1 = this;
			_dfTextbox1.cursorIndex = _dfTextbox1.cursorIndex + 1;
			this.OnTextChanged();
			this.Invalidate();
		}
		args.Use();
	}

	private void renderCursor(int startIndex, int cursorIndex, float[] charWidths, float leftOffset)
	{
		if (string.IsNullOrEmpty(this.SelectionSprite) || base.Atlas == null)
		{
			return;
		}
		float single = 0f;
		for (int i = startIndex; i < cursorIndex; i++)
		{
			single = single + charWidths[i];
		}
		float units = base.PixelsToUnits();
		float single1 = (single + leftOffset + (float)this.padding.left * units).Quantize(units);
		float single2 = (float)(-this.padding.top) * units;
		float single3 = units * (float)this.cursorWidth;
		float single4 = (this.size.y - (float)this.padding.vertical) * units;
		Vector3 vector3 = new Vector3(single1, single2);
		Vector3 vector31 = new Vector3(single1 + single3, single2);
		Vector3 vector32 = new Vector3(single1 + single3, single2 - single4);
		Vector3 vector33 = new Vector3(single1, single2 - single4);
		dfList<Vector3> vertices = this.renderData.Vertices;
		dfList<int> triangles = this.renderData.Triangles;
		dfList<Vector2> uV = this.renderData.UV;
		dfList<Color32> colors = this.renderData.Colors;
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(this.size) * units;
		this.addQuadIndices(vertices, triangles);
		vertices.Add(vector3 + upperLeft);
		vertices.Add(vector31 + upperLeft);
		vertices.Add(vector32 + upperLeft);
		vertices.Add(vector33 + upperLeft);
		Color32 color32 = base.ApplyOpacity(this.TextColor);
		colors.Add(color32);
		colors.Add(color32);
		colors.Add(color32);
		colors.Add(color32);
		Rect item = base.Atlas[this.SelectionSprite].region;
		uV.Add(new Vector2(item.x, item.yMax));
		uV.Add(new Vector2(item.xMax, item.yMax));
		uV.Add(new Vector2(item.xMax, item.y));
		uV.Add(new Vector2(item.x, item.y));
	}

	public dfList<dfRenderData> RenderMultiple()
	{
		if (base.Atlas == null || this.Font == null)
		{
			return null;
		}
		if (!this.isVisible)
		{
			return null;
		}
		if (this.renderData == null)
		{
			this.renderData = dfRenderData.Obtain();
			this.textRenderData = dfRenderData.Obtain();
			this.isControlInvalidated = true;
		}
		if (!this.isControlInvalidated)
		{
			for (int i = 0; i < this.buffers.Count; i++)
			{
				this.buffers[i].Transform = base.transform.localToWorldMatrix;
			}
			return this.buffers;
		}
		this.buffers.Clear();
		this.renderData.Clear();
		this.renderData.Material = base.Atlas.Material;
		this.renderData.Transform = base.transform.localToWorldMatrix;
		this.buffers.Add(this.renderData);
		this.textRenderData.Clear();
		this.textRenderData.Material = base.Atlas.Material;
		this.textRenderData.Transform = base.transform.localToWorldMatrix;
		this.buffers.Add(this.textRenderData);
		this.renderBackground();
		this.renderText(this.textRenderData);
		this.isControlInvalidated = false;
		this.updateCollider();
		return this.buffers;
	}

	private void renderSelection(int scrollIndex, float[] charWidths, float leftOffset)
	{
		if (string.IsNullOrEmpty(this.SelectionSprite) || base.Atlas == null)
		{
			return;
		}
		float units = base.PixelsToUnits();
		float single = (this.size.x - (float)this.padding.horizontal) * units;
		int num = scrollIndex;
		float single1 = 0f;
		int num1 = scrollIndex;
		while (num1 < this.text.Length)
		{
			num++;
			single1 = single1 + charWidths[num1];
			if (single1 <= single)
			{
				num1++;
			}
			else
			{
				break;
			}
		}
		if (this.selectionStart > num || this.selectionEnd < scrollIndex)
		{
			return;
		}
		int num2 = Mathf.Max(scrollIndex, this.selectionStart);
		if (num2 > num)
		{
			return;
		}
		int num3 = Mathf.Min(this.selectionEnd, num);
		if (num3 <= scrollIndex)
		{
			return;
		}
		float single2 = 0f;
		float single3 = 0f;
		single1 = 0f;
		int num4 = scrollIndex;
		while (num4 <= num)
		{
			if (num4 == num2)
			{
				single2 = single1;
			}
			if (num4 != num3)
			{
				single1 = single1 + charWidths[num4];
				num4++;
			}
			else
			{
				single3 = single1;
				break;
			}
		}
		float size = base.Size.y * units;
		this.addQuadIndices(this.renderData.Vertices, this.renderData.Triangles);
		float single4 = single2 + leftOffset + (float)this.padding.left * units;
		float single5 = single4 + Mathf.Min(single3 - single2, single);
		float single6 = (float)(-(this.padding.top + 1)) * units;
		float single7 = single6 - size + (float)(this.padding.vertical + 2) * units;
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size) * units;
		Vector3 vector3 = new Vector3(single4, single6) + upperLeft;
		Vector3 vector31 = new Vector3(single5, single6) + upperLeft;
		Vector3 vector32 = new Vector3(single4, single7) + upperLeft;
		Vector3 vector33 = new Vector3(single5, single7) + upperLeft;
		this.renderData.Vertices.Add(vector3);
		this.renderData.Vertices.Add(vector31);
		this.renderData.Vertices.Add(vector33);
		this.renderData.Vertices.Add(vector32);
		Color32 color32 = base.ApplyOpacity(this.SelectionBackgroundColor);
		this.renderData.Colors.Add(color32);
		this.renderData.Colors.Add(color32);
		this.renderData.Colors.Add(color32);
		this.renderData.Colors.Add(color32);
		dfAtlas.ItemInfo item = base.Atlas[this.SelectionSprite];
		Rect rect = item.region;
		float single8 = rect.width / item.sizeInPixels.x;
		float single9 = rect.height / item.sizeInPixels.y;
		this.renderData.UV.Add(new Vector2(rect.x + single8, rect.yMax - single9));
		this.renderData.UV.Add(new Vector2(rect.xMax - single8, rect.yMax - single9));
		this.renderData.UV.Add(new Vector2(rect.xMax - single8, rect.y + single9));
		this.renderData.UV.Add(new Vector2(rect.x + single8, rect.y + single9));
	}

	private void renderText(dfRenderData textBuffer)
	{
		int num;
		float units = base.PixelsToUnits();
		Vector2 vector2 = new Vector2(this.size.x - (float)this.padding.horizontal, this.size.y - (float)this.padding.vertical);
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(upperLeft.x + (float)this.padding.left, upperLeft.y - (float)this.padding.top, 0f) * units;
		string str = (!this.IsPasswordField || string.IsNullOrEmpty(this.passwordChar) ? this.text : this.passwordDisplayText());
		Color32 color32 = (!base.IsEnabled ? base.DisabledColor : this.TextColor);
		float textScaleMultiplier = this.getTextScaleMultiplier();
		using (dfFontRendererBase textScale = this.font.ObtainRenderer())
		{
			textScale.WordWrap = false;
			textScale.MaxSize = vector2;
			textScale.PixelRatio = units;
			textScale.TextScale = this.TextScale * textScaleMultiplier;
			textScale.VectorOffset = vector3;
			textScale.MultiLine = false;
			textScale.TextAlign = UnityEngine.TextAlignment.Left;
			textScale.ProcessMarkup = false;
			textScale.DefaultColor = color32;
			textScale.BottomColor = new Color32?(color32);
			textScale.OverrideMarkupColors = false;
			textScale.Opacity = base.CalculateOpacity();
			textScale.Shadow = this.Shadow;
			textScale.ShadowColor = this.ShadowColor;
			textScale.ShadowOffset = this.ShadowOffset;
			this.cursorIndex = Mathf.Min(this.cursorIndex, str.Length);
			this.scrollIndex = Mathf.Min(Mathf.Min(this.scrollIndex, this.cursorIndex), str.Length);
			this.charWidths = textScale.GetCharacterWidths(str);
			Vector2 vector21 = vector2 * units;
			this.leftOffset = 0f;
			if (this.textAlign != UnityEngine.TextAlignment.Left)
			{
				this.scrollIndex = Mathf.Max(0, Mathf.Min(this.cursorIndex, str.Length - 1));
				float single = 0f;
				float fontSize = (float)this.font.FontSize * 1.25f * units;
				while (this.scrollIndex > 0 && single < vector21.x - fontSize)
				{
					float[] singleArray = this.charWidths;
					dfTextbox _dfTextbox = this;
					int num1 = _dfTextbox.scrollIndex;
					num = num1;
					_dfTextbox.scrollIndex = num1 - 1;
					single = single + singleArray[num];
				}
				float single1 = (str.Length <= 0 ? 0f : textScale.GetCharacterWidths(str.Substring(this.scrollIndex)).Sum());
				UnityEngine.TextAlignment textAlignment = this.textAlign;
				if (textAlignment == UnityEngine.TextAlignment.Center)
				{
					this.leftOffset = Mathf.Max(0f, (vector21.x - single1) * 0.5f);
				}
				else if (textAlignment == UnityEngine.TextAlignment.Right)
				{
					this.leftOffset = Mathf.Max(0f, vector21.x - single1);
				}
				vector3.x = vector3.x + this.leftOffset;
				textScale.VectorOffset = vector3;
			}
			else
			{
				float single2 = 0f;
				for (int i = this.scrollIndex; i < this.cursorIndex; i++)
				{
					single2 = single2 + this.charWidths[i];
				}
				while (single2 >= vector21.x && this.scrollIndex < this.cursorIndex)
				{
					float[] singleArray1 = this.charWidths;
					dfTextbox _dfTextbox1 = this;
					int num2 = _dfTextbox1.scrollIndex;
					num = num2;
					_dfTextbox1.scrollIndex = num2 + 1;
					single2 = single2 - singleArray1[num];
				}
			}
			if (this.selectionEnd != this.selectionStart)
			{
				this.renderSelection(this.scrollIndex, this.charWidths, this.leftOffset);
			}
			else if (this.cursorShown)
			{
				this.renderCursor(this.scrollIndex, this.cursorIndex, this.charWidths, this.leftOffset);
			}
			textScale.Render(str.Substring(this.scrollIndex), textBuffer);
		}
	}

	private void selectAll()
	{
		this.selectionStart = 0;
		this.selectionEnd = this.text.Length;
		this.scrollIndex = 0;
		this.setCursorPos(0);
	}

	private void selectToEnd()
	{
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionStart = this.cursorIndex;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			this.selectionStart = this.selectionEnd;
		}
		this.selectionEnd = this.text.Length;
		this.setCursorPos(this.text.Length);
	}

	private void selectToStart()
	{
		if (this.cursorIndex == 0)
		{
			return;
		}
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionEnd = this.cursorIndex;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			this.selectionEnd = this.selectionStart;
		}
		this.selectionStart = 0;
		this.setCursorPos(0);
	}

	private void selectWordAtIndex(int index)
	{
		if (string.IsNullOrEmpty(this.text))
		{
			return;
		}
		index = Mathf.Max(Mathf.Min(this.text.Length - 1, index), 0);
		if (char.IsLetterOrDigit(this.text[index]))
		{
			this.selectionStart = index;
			int num = index;
			while (num > 0)
			{
				if (!char.IsLetterOrDigit(this.text[num - 1]))
				{
					break;
				}
				else
				{
					dfTextbox _dfTextbox = this;
					_dfTextbox.selectionStart = _dfTextbox.selectionStart - 1;
					num--;
				}
			}
			this.selectionEnd = index;
			int num1 = index;
			while (num1 < this.text.Length)
			{
				if (!char.IsLetterOrDigit(this.text[num1]))
				{
					break;
				}
				else
				{
					this.selectionEnd = num1 + 1;
					num1++;
				}
			}
		}
		else
		{
			this.selectionStart = index;
			this.selectionEnd = index + 1;
			this.mouseSelectionAnchor = 0;
		}
		this.cursorIndex = this.selectionStart;
		this.Invalidate();
	}

	private void setCursorPos(int index)
	{
		index = Mathf.Max(0, Mathf.Min(this.text.Length, index));
		if (index == this.cursorIndex)
		{
			return;
		}
		this.cursorIndex = index;
		this.cursorShown = this.HasFocus;
		this.scrollIndex = Mathf.Min(this.scrollIndex, this.cursorIndex);
		this.Invalidate();
	}

	public event PropertyChangedEventHandler<string> PasswordCharacterChanged
	{
		add
		{
			this.PasswordCharacterChanged += value;
		}
		remove
		{
			this.PasswordCharacterChanged -= value;
		}
	}

	public event PropertyChangedEventHandler<bool> ReadOnlyChanged
	{
		add
		{
			this.ReadOnlyChanged += value;
		}
		remove
		{
			this.ReadOnlyChanged -= value;
		}
	}

	public event PropertyChangedEventHandler<string> TextCancelled
	{
		add
		{
			this.TextCancelled += value;
		}
		remove
		{
			this.TextCancelled -= value;
		}
	}

	public event PropertyChangedEventHandler<string> TextChanged
	{
		add
		{
			this.TextChanged += value;
		}
		remove
		{
			this.TextChanged -= value;
		}
	}

	public event PropertyChangedEventHandler<string> TextSubmitted
	{
		add
		{
			this.TextSubmitted += value;
		}
		remove
		{
			this.TextSubmitted -= value;
		}
	}
}