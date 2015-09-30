using NGUI.Meshing;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Label")]
[ExecuteInEditMode]
public class UILabel : UIWidget
{
	[HideInInspector]
	[SerializeField]
	private UIFont mFont;

	[HideInInspector]
	[SerializeField]
	private string mText = string.Empty;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineWidth;

	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineCount;

	[HideInInspector]
	[SerializeField]
	private bool mPassword;

	[HideInInspector]
	[SerializeField]
	private bool mShowLastChar;

	[HideInInspector]
	[SerializeField]
	private bool mOverflowRight;

	[HideInInspector]
	[SerializeField]
	private UILabel.Effect mEffectStyle;

	[HideInInspector]
	[SerializeField]
	private Color mEffectColor = Color.black;

	[HideInInspector]
	[SerializeField]
	private UIFont.SymbolStyle mSymbols = UIFont.SymbolStyle.Uncolored;

	[HideInInspector]
	[SerializeField]
	private char mCarratChar = '|';

	[HideInInspector]
	[SerializeField]
	private Color mHighlightTextColor = Color.cyan;

	[HideInInspector]
	[SerializeField]
	private Color mHighlightColor = Color.black;

	[HideInInspector]
	[SerializeField]
	private char mHighlightChar = '|';

	[HideInInspector]
	[SerializeField]
	private float mHighlightCharSplit = 0.5f;

	[HideInInspector]
	[SerializeField]
	private float mLineWidth;

	[HideInInspector]
	[SerializeField]
	private bool mMultiline = true;

	private Vector3? lastQueryPos;

	private bool mShouldBeProcessed = true;

	private string mProcessedText;

	private UITextSelection mSelection;

	private Vector3 mLastScale = Vector3.one;

	private string mLastText = string.Empty;

	private int mLastWidth;

	private bool mLastEncoding = true;

	private int mLastCount;

	private bool mLastPass;

	private bool mLastShow;

	private bool mInvisibleHack;

	private bool mLastInvisibleHack;

	private UILabel.Effect mLastEffect;

	private Color mLastColor = Color.black;

	private Vector3 mSize = Vector3.zero;

	private List<UITextMarkup> _markups;

	public char carratChar
	{
		get
		{
			return this.mCarratChar;
		}
		set
		{
			if (this.mCarratChar != value)
			{
				bool flag = this.shouldShowCarrat;
				this.mCarratChar = value;
				if (flag)
				{
					this.ForceChanges();
				}
			}
		}
	}

	public UITextPosition carratPosition
	{
		get
		{
			return this.mSelection.carratPos;
		}
	}

	private bool carratWouldBeVisibleIfOn
	{
		get
		{
			return this.mCarratChar != '\0';
		}
	}

	protected override UIMaterial customMaterial
	{
		get
		{
			return this.material;
		}
	}

	public bool drawingCarrat
	{
		get
		{
			return (this.mCarratChar == 0 ? false : this.mSelection.showCarrat);
		}
	}

	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (this.mEffectColor != value)
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.ForceChanges();
				}
			}
		}
	}

	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.ForceChanges();
			}
		}
	}

	public UIFont font
	{
		get
		{
			return this.mFont;
		}
		set
		{
			UIMaterial uIMaterial;
			if (this.mFont != value)
			{
				this.mFont = value;
				if (this.mFont == null)
				{
					uIMaterial = null;
				}
				else
				{
					uIMaterial = (UIMaterial)this.mFont.material;
				}
				base.baseMaterial = uIMaterial;
				base.ChangedAuto();
				this.ForceChanges();
				this.MarkAsChanged();
			}
		}
	}

	public bool hasSelection
	{
		get
		{
			return this.mSelection.hasSelection;
		}
	}

	public char highlightChar
	{
		get
		{
			return this.mHighlightChar;
		}
		set
		{
			if (this.mHighlightChar != value)
			{
				bool flag = (!this.hasSelection ? false : this.mHighlightColor.a > 0f);
				this.mHighlightChar = value;
				if (flag)
				{
					this.ForceChanges();
				}
			}
		}
	}

	public float highlightCharSplit
	{
		get
		{
			return this.mHighlightCharSplit;
		}
		set
		{
			if (value > 1f)
			{
				value = 1f;
			}
			else if (value < 0f)
			{
				value = 0f;
			}
			if (this.mHighlightCharSplit != value)
			{
				bool flag = (!this.hasSelection || this.mHighlightColor.a <= 0f ? false : this.mHighlightChar != '\0');
				this.mHighlightCharSplit = value;
				if (flag)
				{
					this.ForceChanges();
				}
			}
		}
	}

	public Color highlightColor
	{
		get
		{
			return this.mHighlightColor;
		}
		set
		{
			if (this.mHighlightColor != value)
			{
				bool flag = (!this.hasSelection || this.mHighlightChar == 0 || this.mHighlightColor.a <= 0f ? value.a > 0f : true);
				this.mHighlightColor = value;
				if (flag)
				{
					this.ForceChanges();
				}
			}
		}
	}

	public Color highlightTextColor
	{
		get
		{
			return this.mHighlightTextColor;
		}
		set
		{
			if (this.mHighlightTextColor != value)
			{
				bool flag = (!this.hasSelection || this.mHighlightColor.a <= 0f ? value.a > 0f : true);
				this.mHighlightTextColor = value;
				if (flag)
				{
					this.ForceChanges();
				}
			}
		}
	}

	private bool highlightWouldBeVisibleIfOn
	{
		get
		{
			return (this.mHighlightChar == 0 || this.mHighlightColor.a <= 0f ? this.mHighlightTextColor != base.color : true);
		}
	}

	public bool invisibleHack
	{
		get
		{
			return this.mInvisibleHack;
		}
		set
		{
			if (this.mInvisibleHack != value)
			{
				this.mInvisibleHack = value;
				this.ForceChanges();
			}
		}
	}

	public int lineWidth
	{
		get
		{
			return this.mMaxLineWidth;
		}
		set
		{
			if (this.mMaxLineWidth != value)
			{
				this.mMaxLineWidth = value;
				this.ForceChanges();
			}
		}
	}

	private List<UITextMarkup> markups
	{
		get
		{
			List<UITextMarkup> uITextMarkups = this._markups;
			if (uITextMarkups == null)
			{
				List<UITextMarkup> uITextMarkups1 = new List<UITextMarkup>();
				List<UITextMarkup> uITextMarkups2 = uITextMarkups1;
				this._markups = uITextMarkups1;
				uITextMarkups = uITextMarkups2;
			}
			return uITextMarkups;
		}
	}

	public new UIMaterial material
	{
		get
		{
			UIMaterial uIMaterial;
			UIMaterial uIMaterial1 = base.baseMaterial;
			if (uIMaterial1 == null)
			{
				if (this.mFont == null)
				{
					uIMaterial = null;
				}
				else
				{
					uIMaterial = (UIMaterial)this.mFont.material;
				}
				uIMaterial1 = uIMaterial;
				this.material = uIMaterial1;
			}
			return uIMaterial1;
		}
		set
		{
			base.material = value;
		}
	}

	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				this.ForceChanges();
				if (value == 1)
				{
					this.mPassword = false;
				}
			}
		}
	}

	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = (!value ? 1 : 0);
				this.ForceChanges();
				if (value)
				{
					this.mPassword = false;
				}
			}
		}
	}

	public bool overflowRight
	{
		get
		{
			return this.mOverflowRight;
		}
		set
		{
			if (this.mOverflowRight != value)
			{
				this.mOverflowRight = value;
				UIWidget.Pivot pivot = base.pivot;
				switch (pivot)
				{
					case UIWidget.Pivot.TopLeft:
					case UIWidget.Pivot.Left:
					{
						this.ForceChanges();
						break;
					}
					default:
					{
						if (pivot == UIWidget.Pivot.BottomLeft)
						{
							goto case UIWidget.Pivot.Left;
						}
						break;
					}
				}
			}
		}
	}

	public bool password
	{
		get
		{
			return this.mPassword;
		}
		set
		{
			if (this.mPassword != value)
			{
				this.mPassword = value;
				this.mMaxLineCount = 1;
				this.mEncoding = false;
				this.ForceChanges();
			}
		}
	}

	public string processedText
	{
		get
		{
			if (this.mLastScale != base.cachedTransform.localScale)
			{
				this.mLastScale = base.cachedTransform.localScale;
				this.mShouldBeProcessed = true;
			}
			if (this.PendingChanges())
			{
				this.ProcessText();
			}
			return this.mProcessedText;
		}
	}

	public new Vector2 relativeSize
	{
		get
		{
			if (this.mFont == null)
			{
				return Vector3.one;
			}
			if (this.PendingChanges())
			{
				this.ProcessText();
			}
			return this.mSize;
		}
	}

	public UITextSelection selection
	{
		get
		{
			return this.mSelection;
		}
		set
		{
			UITextSelection.Change changesTo = this.mSelection.GetChangesTo(ref value);
			this.mSelection = value;
			switch (changesTo)
			{
				case UITextSelection.Change.NoneToCarrat:
				case UITextSelection.Change.CarratMove:
				case UITextSelection.Change.CarratToNone:
				{
					if (this.carratWouldBeVisibleIfOn)
					{
						this.ForceChanges();
					}
					break;
				}
				case UITextSelection.Change.CarratToSelection:
				case UITextSelection.Change.SelectionToCarrat:
				{
					if (this.carratWouldBeVisibleIfOn || this.highlightWouldBeVisibleIfOn)
					{
						this.ForceChanges();
					}
					break;
				}
				case UITextSelection.Change.SelectionAdjusted:
				case UITextSelection.Change.NoneToSelection:
				case UITextSelection.Change.SelectionToNone:
				{
					if (this.highlightWouldBeVisibleIfOn)
					{
						this.ForceChanges();
					}
					break;
				}
			}
		}
	}

	public UITextPosition selectPosition
	{
		get
		{
			return this.mSelection.selectPos;
		}
	}

	public bool shouldShowCarrat
	{
		get
		{
			return this.mSelection.showCarrat;
		}
	}

	public bool showLastPasswordChar
	{
		get
		{
			return this.mShowLastChar;
		}
		set
		{
			if (this.mShowLastChar != value)
			{
				this.mShowLastChar = value;
				this.ForceChanges();
			}
		}
	}

	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.ForceChanges();
				if (value)
				{
					this.mPassword = false;
				}
			}
		}
	}

	public UIFont.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.ForceChanges();
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
			if (value != null && this.mText != value)
			{
				this.mText = value;
				this.ForceChanges();
			}
		}
	}

	public UILabel() : base(UIWidget.WidgetFlags.CustomRelativeSize | UIWidget.WidgetFlags.CustomMaterialGet)
	{
	}

	private void ApplyChanges()
	{
		this.mShouldBeProcessed = false;
		this.mLastText = this.text;
		this.mLastInvisibleHack = this.mInvisibleHack;
		this.mLastWidth = this.mMaxLineWidth;
		this.mLastEncoding = this.mEncoding;
		this.mLastCount = this.mMaxLineCount;
		this.mLastPass = this.mPassword;
		this.mLastShow = this.mShowLastChar;
		this.mLastEffect = this.mEffectStyle;
		this.mLastColor = this.mEffectColor;
	}

	public int CalculateTextPosition(Space space, Vector3[] points, UITextPosition[] positions)
	{
		int num;
		if (!this.mFont)
		{
			return -1;
		}
		string str = this.processedText;
		num = (space != Space.Self ? this.mFont.CalculatePlacement(points, positions, str, base.cachedTransform.worldToLocalMatrix) : this.mFont.CalculatePlacement(points, positions, str));
		int num1 = -1;
		for (int i = 0; i < num; i++)
		{
			this.ConvertProcessedTextPosition(ref positions[i], ref num1);
		}
		return num;
	}

	private void ConvertProcessedTextPosition(ref UITextPosition p, ref int markupCount)
	{
		if (markupCount == -1)
		{
			markupCount = this.markups.Count;
		}
		if (markupCount == 0)
		{
			return;
		}
		UITextMarkup item = this.markups[0];
		int num = 0;
		while (true)
		{
			if (p.position > item.index)
			{
				return;
			}
			switch (item.mod)
			{
				case UITextMod.End:
				{
					p.deformed = (short)(this.mText.Length - item.index);
					break;
				}
				case UITextMod.Removed:
				{
					p.deformed = (short)(p.deformed + 1);
					int num1 = num + 1;
					num = num1;
					if (num1 < markupCount)
					{
						item = this.markups[num];
						continue;
					}
					break;
				}
				case UITextMod.Replaced:
				{
					int num2 = num + 1;
					num = num2;
					if (num2 < markupCount)
					{
						item = this.markups[num];
						continue;
					}
					break;
				}
				case UITextMod.Added:
				{
					p.deformed = (short)(p.deformed - 1);
					int num3 = num + 1;
					num = num3;
					if (num3 < markupCount)
					{
						item = this.markups[num];
						continue;
					}
					break;
				}
				default:
				{
					int num4 = num + 1;
					num = num4;
					if (num4 < markupCount)
					{
						item = this.markups[num];
						continue;
					}
					break;
				}
			}
		}
	}

	public UITextPosition ConvertUnprocessedPosition(int position)
	{
		UITextPosition uITextPosition = new UITextPosition();
		string str = this.processedText;
		int count = this.markups.Count;
		int num = position;
		if (count > 0)
		{
			int num1 = 0;
			UITextMarkup item = this.markups[num1];
			while (item.index <= position)
			{
				switch (item.mod)
				{
					case UITextMod.End:
					{
						position = position - (num - item.index);
						num1 = count;
						goto case UITextMod.Replaced;
					}
					case UITextMod.Removed:
					{
						position--;
						goto case UITextMod.Replaced;
					}
					case UITextMod.Replaced:
					{
						int num2 = num1 + 1;
						num1 = num2;
						if (num2 < count)
						{
							item = this.markups[num1];
							continue;
						}
						break;
					}
					case UITextMod.Added:
					{
						position++;
						goto case UITextMod.Replaced;
					}
					default:
					{
						goto case UITextMod.Replaced;
					}
				}
			}
		}
		UILabel.CountLinesGetColumn(str, position, out uITextPosition.position, out uITextPosition.line, out uITextPosition.column, out uITextPosition.region);
		uITextPosition.uniformRegion = uITextPosition.region;
		uITextPosition.deformed = (short)(num - uITextPosition.position);
		return uITextPosition;
	}

	public UITextSelection ConvertUnprocessedSelection(int carratPos, int selectPos)
	{
		UITextSelection uITextSelection = new UITextSelection();
		uITextSelection.carratPos = this.ConvertUnprocessedPosition(carratPos);
		if (carratPos != selectPos)
		{
			uITextSelection.selectPos = this.ConvertUnprocessedPosition(selectPos);
		}
		else
		{
			uITextSelection.selectPos = uITextSelection.carratPos;
		}
		return uITextSelection;
	}

	private static void CountLinesGetColumn(string text, int inPos, out int pos, out int lines, out int column, out UITextRegion region)
	{
		if (inPos < 0)
		{
			region = UITextRegion.Before;
			pos = 0;
			lines = 0;
			column = 0;
		}
		else if (inPos != 0)
		{
			if (inPos > text.Length)
			{
				region = UITextRegion.End;
				pos = text.Length;
			}
			else if (inPos != text.Length)
			{
				region = UITextRegion.Inside;
				pos = inPos;
			}
			else
			{
				region = UITextRegion.Past;
				pos = inPos;
			}
			int num = text.IndexOf('\n', 0, pos);
			if (num != -1)
			{
				int num1 = num;
				lines = 1;
				while (true)
				{
					int num2 = num + 1;
					num = num2;
					if (num2 >= pos)
					{
						break;
					}
					num = text.IndexOf('\n', num, pos - num);
					if (num != -1)
					{
						lines = lines + 1;
						num1 = num;
					}
					else
					{
						break;
					}
				}
				column = pos - (num1 + 1);
			}
			else
			{
				lines = 0;
				column = pos;
			}
		}
		else
		{
			pos = 0;
			lines = 0;
			column = 0;
			region = UITextRegion.Pre;
		}
	}

	private void ForceChanges()
	{
		base.ChangedAuto();
		this.mShouldBeProcessed = true;
	}

	protected override void GetCustomVector2s(int start, int end, UIWidget.WidgetFlags[] flags, Vector2[] v)
	{
		for (int i = 0; i < end; i++)
		{
			if (flags[i] != UIWidget.WidgetFlags.CustomRelativeSize)
			{
				base.GetCustomVector2s(i, i + 1, flags, v);
			}
			else
			{
				v[i] = this.relativeSize;
			}
		}
	}

	public void GetProcessedIndices(ref int start, ref int end)
	{
		int num;
		int count = this.markups.Count;
		if (count == 0 || this.markups[0].index > end)
		{
			return;
		}
		int num1 = start;
		int length = end;
		int num2 = 0;
		while (this.markups[num2].index <= start)
		{
			switch (this.markups[num2].mod)
			{
				case UITextMod.End:
				{
					length = this.mProcessedText.Length - 1;
					return;
				}
				case UITextMod.Removed:
				{
					num1--;
					length--;
					num = num2 + 1;
					num2 = num;
					if (num < count)
					{
						continue;
					}
					start = num1;
					end = length;
					return;
				}
				case UITextMod.Replaced:
				{
					num = num2 + 1;
					num2 = num;
					if (num < count)
					{
						continue;
					}
					start = num1;
					end = length;
					return;
				}
				case UITextMod.Added:
				{
					num1++;
					length++;
					num = num2 + 1;
					num2 = num;
					if (num < count)
					{
						continue;
					}
					start = num1;
					end = length;
					return;
				}
				default:
				{
					num = num2 + 1;
					num2 = num;
					if (num < count)
					{
						continue;
					}
					start = num1;
					end = length;
					return;
				}
			}
		}
		while (this.markups[num2].index <= end)
		{
			switch (this.markups[num2].mod)
			{
				case UITextMod.End:
				{
					length = this.mProcessedText.Length - 1;
					return;
				}
				case UITextMod.Removed:
				{
					length--;
					goto case UITextMod.Replaced;
				}
				case UITextMod.Replaced:
				{
					int num3 = num2 + 1;
					num2 = num3;
					if (num3 < count)
					{
						continue;
					}
					break;
				}
				case UITextMod.Added:
				{
					length++;
					goto case UITextMod.Replaced;
				}
				default:
				{
					goto case UITextMod.Replaced;
				}
			}
		}
		start = num1;
		end = length;
	}

	public override void MakePixelPerfect()
	{
		if (this.mFont == null)
		{
			base.MakePixelPerfect();
		}
		else
		{
			float single = (this.font.atlas == null ? 1f : this.font.atlas.pixelSize);
			Vector3 vector3 = base.cachedTransform.localScale;
			vector3.x = (float)this.mFont.size * single;
			vector3.y = vector3.x;
			vector3.z = 1f;
			Vector2 vector2 = this.relativeSize * vector3.x;
			int num = Mathf.RoundToInt(vector2.x / single);
			int num1 = Mathf.RoundToInt(vector2.y / single);
			Vector3 vector31 = base.cachedTransform.localPosition;
			vector31.x = (float)Mathf.FloorToInt(vector31.x / single);
			vector31.y = (float)Mathf.CeilToInt(vector31.y / single);
			vector31.z = (float)Mathf.RoundToInt(vector31.z);
			if (base.cachedTransform.localRotation == Quaternion.identity)
			{
				if (num % 2 == 1 && (base.pivot == UIWidget.Pivot.Top || base.pivot == UIWidget.Pivot.Center || base.pivot == UIWidget.Pivot.Bottom))
				{
					vector31.x = vector31.x + 0.5f;
				}
				if (num1 % 2 == 1 && (base.pivot == UIWidget.Pivot.Left || base.pivot == UIWidget.Pivot.Center || base.pivot == UIWidget.Pivot.Right))
				{
					vector31.y = vector31.y - 0.5f;
				}
			}
			vector31.x = vector31.x * single;
			vector31.y = vector31.y * single;
			base.cachedTransform.localPosition = vector31;
			base.cachedTransform.localScale = vector3;
		}
	}

	public void MakePositionPerfect()
	{
		float single = (this.font.atlas == null ? 1f : this.font.atlas.pixelSize);
		Vector3 vector3 = base.cachedTransform.localScale;
		if (this.mFont.size == Mathf.RoundToInt(vector3.x / single) && this.mFont.size == Mathf.RoundToInt(vector3.y / single) && base.cachedTransform.localRotation == Quaternion.identity)
		{
			Vector2 vector2 = this.relativeSize * vector3.x;
			int num = Mathf.RoundToInt(vector2.x / single);
			int num1 = Mathf.RoundToInt(vector2.y / single);
			Vector3 vector31 = base.cachedTransform.localPosition;
			vector31.x = (float)Mathf.FloorToInt(vector31.x / single);
			vector31.y = (float)Mathf.CeilToInt(vector31.y / single);
			vector31.z = (float)Mathf.RoundToInt(vector31.z);
			if (num % 2 == 1 && (base.pivot == UIWidget.Pivot.Top || base.pivot == UIWidget.Pivot.Center || base.pivot == UIWidget.Pivot.Bottom))
			{
				vector31.x = vector31.x + 0.5f;
			}
			if (num1 % 2 == 1 && (base.pivot == UIWidget.Pivot.Left || base.pivot == UIWidget.Pivot.Center || base.pivot == UIWidget.Pivot.Right))
			{
				vector31.y = vector31.y - 0.5f;
			}
			vector31.x = vector31.x * single;
			vector31.y = vector31.y * single;
			if (base.cachedTransform.localPosition != vector31)
			{
				base.cachedTransform.localPosition = vector31;
			}
		}
	}

	public override void MarkAsChanged()
	{
		this.ForceChanges();
		base.MarkAsChanged();
	}

	public override void OnFill(MeshBuffer m)
	{
		if (this.mFont == null)
		{
			return;
		}
		Color color = (!this.mInvisibleHack ? base.color : Color.clear);
		this.MakePositionPerfect();
		UIWidget.Pivot pivot = base.pivot;
		int num = m.vSize;
		if (pivot != UIWidget.Pivot.Left && pivot != UIWidget.Pivot.TopLeft && pivot != UIWidget.Pivot.BottomLeft)
		{
			if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
			{
				UIFont uIFont = this.mFont;
				string str = this.processedText;
				bool flag = this.mEncoding;
				UIFont.SymbolStyle symbolStyle = this.mSymbols;
				Vector2 vector2 = this.relativeSize;
				uIFont.Print(str, color, m, flag, symbolStyle, UIFont.Alignment.Right, Mathf.RoundToInt(vector2.x * (float)this.mFont.size), ref this.mSelection, this.mCarratChar, this.mHighlightTextColor, this.mHighlightColor, this.mHighlightChar, this.mHighlightCharSplit);
			}
			else
			{
				UIFont uIFont1 = this.mFont;
				string str1 = this.processedText;
				bool flag1 = this.mEncoding;
				UIFont.SymbolStyle symbolStyle1 = this.mSymbols;
				Vector2 vector21 = this.relativeSize;
				uIFont1.Print(str1, color, m, flag1, symbolStyle1, UIFont.Alignment.Center, Mathf.RoundToInt(vector21.x * (float)this.mFont.size), ref this.mSelection, this.mCarratChar, this.mHighlightTextColor, this.mHighlightColor, this.mHighlightChar, this.mHighlightCharSplit);
			}
		}
		else if (!this.mOverflowRight)
		{
			this.mFont.Print(this.processedText, color, m, this.mEncoding, this.mSymbols, UIFont.Alignment.Left, 0, ref this.mSelection, this.mCarratChar, this.mHighlightTextColor, this.mHighlightColor, this.mHighlightChar, this.mHighlightCharSplit);
		}
		else
		{
			UIFont uIFont2 = this.mFont;
			string str2 = this.processedText;
			bool flag2 = this.mEncoding;
			UIFont.SymbolStyle symbolStyle2 = this.mSymbols;
			Vector2 vector22 = this.relativeSize;
			uIFont2.Print(str2, color, m, flag2, symbolStyle2, UIFont.Alignment.LeftOverflowRight, Mathf.RoundToInt(vector22.x * (float)this.mFont.size), ref this.mSelection, this.mCarratChar, this.mHighlightTextColor, this.mHighlightColor, this.mHighlightChar, this.mHighlightCharSplit);
		}
		m.ApplyEffect(base.cachedTransform, num, this.effectStyle, this.effectColor, (float)this.mFont.size);
	}

	protected override void OnStart()
	{
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
	}

	private bool PendingChanges()
	{
		return (this.mShouldBeProcessed || this.mLastText != this.text || this.mInvisibleHack != this.mLastInvisibleHack || this.mLastWidth != this.mMaxLineWidth || this.mLastEncoding != this.mEncoding || this.mLastCount != this.mMaxLineCount || this.mLastPass != this.mPassword || this.mLastShow != this.mShowLastChar || this.mLastEffect != this.mEffectStyle ? true : this.mLastColor != this.mEffectColor);
	}

	private void ProcessText()
	{
		float single;
		base.ChangedAuto();
		this.mLastText = this.mText;
		this.markups.Clear();
		string str = this.mProcessedText;
		this.mProcessedText = this.mText;
		if (this.mPassword)
		{
			this.mProcessedText = this.mFont.WrapText(this.markups, this.mProcessedText, 100000f, 1, false, UIFont.SymbolStyle.None);
			string empty = string.Empty;
			if (!this.mShowLastChar)
			{
				int num = 0;
				int length = this.mProcessedText.Length;
				while (num < length)
				{
					empty = string.Concat(empty, "*");
					num++;
				}
			}
			else
			{
				int num1 = 1;
				int length1 = this.mProcessedText.Length;
				while (num1 < length1)
				{
					empty = string.Concat(empty, "*");
					num1++;
				}
				if (this.mProcessedText.Length > 0)
				{
					empty = string.Concat(empty, this.mProcessedText[this.mProcessedText.Length - 1]);
				}
			}
			this.mProcessedText = empty;
		}
		else if (this.mMaxLineWidth > 0)
		{
			UIFont uIFont = this.mFont;
			List<UITextMarkup> uITextMarkups = this.markups;
			string str1 = this.mProcessedText;
			float single1 = (float)this.mMaxLineWidth;
			Vector3 vector3 = base.cachedTransform.localScale;
			this.mProcessedText = uIFont.WrapText(uITextMarkups, str1, single1 / vector3.x, this.mMaxLineCount, this.mEncoding, this.mSymbols);
		}
		else if (this.mMaxLineCount > 0)
		{
			this.mProcessedText = this.mFont.WrapText(this.markups, this.mProcessedText, 100000f, this.mMaxLineCount, this.mEncoding, this.mSymbols);
		}
		this.mSize = (string.IsNullOrEmpty(this.mProcessedText) ? Vector2.one : this.mFont.CalculatePrintedSize(this.mProcessedText, this.mEncoding, this.mSymbols));
		float single2 = base.cachedTransform.localScale.x;
		float single3 = this.mSize.x;
		single = (!(this.mFont != null) || single2 <= 1f ? 1f : (float)this.lineWidth / single2);
		this.mSize.x = Mathf.Max(single3, single);
		this.mSize.y = Mathf.Max(this.mSize.y, 1f);
		if (str != this.mProcessedText)
		{
			this.mSelection = new UITextSelection();
		}
		this.ApplyChanges();
	}

	public void UnionProcessedChanges(string newProcessedText)
	{
		this.text = newProcessedText;
	}

	public enum Effect
	{
		None,
		Shadow,
		Outline
	}
}