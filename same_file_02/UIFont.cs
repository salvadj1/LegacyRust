using NGUI.Meshing;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Font")]
[ExecuteInEditMode]
public class UIFont : MonoBehaviour
{
	private const int mangleStartSize = 8;

	[HideInInspector]
	[SerializeField]
	private Material mMat;

	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	[HideInInspector]
	[SerializeField]
	private BMFont mFont = new BMFont();

	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	[HideInInspector]
	[SerializeField]
	private int mSpacingY;

	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	[HideInInspector]
	[SerializeField]
	private UIFont mReplacement;

	private UIAtlas.Sprite mSprite;

	private bool mSpriteSet;

	private List<Color> mColors = new List<Color>();

	private static List<UITextMarkup> _tempMarkup;

	private static Vector3[] manglePoints;

	private static int[] mangleIndices;

	private static UITextPosition[] manglePositions;

	private readonly static UIFont.MangleSorter mangleSort;

	private readonly static UITextPosition[] empty;

	public UIAtlas atlas
	{
		get
		{
			return (this.mReplacement == null ? this.mAtlas : this.mReplacement.atlas);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.atlas = value;
			}
			else if (this.mAtlas != value)
			{
				if (value == null)
				{
					if (this.mAtlas != null)
					{
						this.mMat = this.mAtlas.spriteMaterial;
					}
					if (this.sprite != null)
					{
						this.mUVRect = this.uvRect;
					}
				}
				this.mAtlas = value;
				this.MarkAsDirty();
			}
		}
	}

	public BMFont bmFont
	{
		get
		{
			return (this.mReplacement == null ? this.mFont : this.mReplacement.bmFont);
		}
	}

	public int horizontalSpacing
	{
		get
		{
			return (this.mReplacement == null ? this.mSpacingX : this.mReplacement.horizontalSpacing);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.horizontalSpacing = value;
			}
			else if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsDirty();
			}
		}
	}

	public Material material
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.material;
			}
			return (this.mAtlas == null ? this.mMat : this.mAtlas.spriteMaterial);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.material = value;
			}
			else if (this.mAtlas == null && this.mMat != value)
			{
				this.mMat = value;
				this.MarkAsDirty();
			}
		}
	}

	public UIFont replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIFont uIFont = value;
			if (uIFont == this)
			{
				uIFont = null;
			}
			if (this.mReplacement != uIFont)
			{
				if (uIFont != null && uIFont.replacement == this)
				{
					uIFont.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsDirty();
				}
				this.mReplacement = uIFont;
				this.MarkAsDirty();
			}
		}
	}

	public int size
	{
		get
		{
			return (this.mReplacement == null ? this.mFont.charSize : this.mReplacement.size);
		}
	}

	public UIAtlas.Sprite sprite
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.sprite;
			}
			if (!this.mSpriteSet)
			{
				this.mSprite = null;
			}
			if (this.mSprite == null && this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
			{
				this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
				if (this.mSprite == null)
				{
					this.mSprite = this.mAtlas.GetSprite(base.name);
				}
				this.mSpriteSet = true;
				if (this.mSprite == null)
				{
					Debug.LogError(string.Concat("Can't find the sprite '", this.mFont.spriteName, "' in UIAtlas on ", NGUITools.GetHierarchy(this.mAtlas.gameObject)));
					this.mFont.spriteName = null;
				}
			}
			return this.mSprite;
		}
	}

	public string spriteName
	{
		get
		{
			return (this.mReplacement == null ? this.mFont.spriteName : this.mReplacement.spriteName);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteName = value;
			}
			else if (this.mFont.spriteName != value)
			{
				this.mFont.spriteName = value;
				this.MarkAsDirty();
			}
		}
	}

	public static List<UITextMarkup> tempMarkup
	{
		get
		{
			List<UITextMarkup> uITextMarkups = UIFont._tempMarkup;
			if (uITextMarkups == null)
			{
				uITextMarkups = new List<UITextMarkup>();
				UIFont._tempMarkup = uITextMarkups;
			}
			return uITextMarkups;
		}
	}

	public int texHeight
	{
		get
		{
			int num;
			if (this.mReplacement == null)
			{
				num = (this.mFont == null ? 1 : this.mFont.texHeight);
			}
			else
			{
				num = this.mReplacement.texHeight;
			}
			return num;
		}
	}

	public Texture2D texture
	{
		get
		{
			Texture2D texture2D;
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			Material material = this.material;
			if (material == null)
			{
				texture2D = null;
			}
			else
			{
				texture2D = material.mainTexture as Texture2D;
			}
			return texture2D;
		}
	}

	public int texWidth
	{
		get
		{
			int num;
			if (this.mReplacement == null)
			{
				num = (this.mFont == null ? 1 : this.mFont.texWidth);
			}
			else
			{
				num = this.mReplacement.texWidth;
			}
			return num;
		}
	}

	public Rect uvRect
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.uvRect;
			}
			if (this.mAtlas != null && this.mSprite == null && this.sprite != null)
			{
				Texture texture = this.mAtlas.texture;
				if (texture != null)
				{
					this.mUVRect = this.mSprite.outer;
					if (this.mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
					{
						this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
					}
					if (this.mSprite.hasPadding)
					{
						Rect rect = this.mUVRect;
						this.mUVRect.xMin = rect.xMin - this.mSprite.paddingLeft * rect.width;
						this.mUVRect.yMin = rect.yMin - this.mSprite.paddingBottom * rect.height;
						this.mUVRect.xMax = rect.xMax + this.mSprite.paddingRight * rect.width;
						this.mUVRect.yMax = rect.yMax + this.mSprite.paddingTop * rect.height;
					}
					if (this.mSprite.hasPadding)
					{
						this.Trim();
					}
				}
			}
			return this.mUVRect;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.uvRect = value;
			}
			else if (this.sprite == null && this.mUVRect != value)
			{
				this.mUVRect = value;
				this.MarkAsDirty();
			}
		}
	}

	public int verticalSpacing
	{
		get
		{
			return (this.mReplacement == null ? this.mSpacingY : this.mReplacement.verticalSpacing);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.verticalSpacing = value;
			}
			else if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsDirty();
			}
		}
	}

	static UIFont()
	{
		UIFont.manglePoints = new Vector3[8];
		UIFont.mangleIndices = new int[8];
		UIFont.manglePositions = new UITextPosition[8];
		UIFont.mangleSort = new UIFont.MangleSorter();
		UIFont.empty = new UITextPosition[0];
	}

	public UIFont()
	{
	}

	private void Align(ref UIFont.PrintContext ctx)
	{
		int num;
		if (this.mFont.charSize > 0)
		{
			switch (ctx.alignment)
			{
				case UIFont.Alignment.Left:
				{
					num = 0;
					break;
				}
				case UIFont.Alignment.Center:
				{
					num = Mathf.Max(0, Mathf.RoundToInt((float)(ctx.lineWidth - ctx.x) * 0.5f));
					break;
				}
				case UIFont.Alignment.Right:
				{
					num = Mathf.Max(0, Mathf.RoundToInt((float)(ctx.lineWidth - ctx.x)));
					break;
				}
				case UIFont.Alignment.LeftOverflowRight:
				{
					num = Mathf.Max(0, Mathf.RoundToInt((float)(ctx.x - ctx.lineWidth)));
					break;
				}
				default:
				{
					throw new NotImplementedException();
				}
			}
			if (num == 0)
			{
				return;
			}
			float single = (float)((double)num / (double)this.mFont.charSize);
			for (int i = ctx.indexOffset; i < ctx.m.vSize; i++)
			{
				ctx.m.v[i].x = ctx.m.v[i].x + single;
			}
		}
	}

	[Obsolete("You must specify some point", true)]
	public UITextPosition[] CalculatePlacement(string text)
	{
		return UIFont.empty;
	}

	private int CalculatePlacement(Vector2[] points, ref UITextPosition[] positions, string text)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.CalculatePlacement(points, positions, text);
		}
		int num = this.FillMangle(points, 0, positions, 0, Mathf.Min((int)points.Length, (int)positions.Length));
		if (num <= 0)
		{
			return num;
		}
		return this.ProcessShared(num, ref positions, text);
	}

	public int CalculatePlacement(Vector2[] points, UITextPosition[] positions, string text)
	{
		if (positions == null)
		{
			throw new ArgumentNullException("positions");
		}
		return this.CalculatePlacement(points, ref positions, text, base.transform);
	}

	public UITextPosition CalculatePlacement(string text, Vector2 point)
	{
		Vector2[] vector2Array = new Vector2[] { point };
		UITextPosition[] uITextPositionArray = new UITextPosition[1];
		UITextPosition uITextPosition = new UITextPosition();
		uITextPositionArray[0] = uITextPosition;
		UITextPosition[] uITextPositionArray1 = uITextPositionArray;
		this.CalculatePlacement(vector2Array, uITextPositionArray1, text);
		return uITextPositionArray1[0];
	}

	public UITextPosition[] CalculatePlacement(string text, params Vector2[] points)
	{
		UITextPosition[] uITextPositionArray = null;
		return (this.CalculatePlacement(points, ref uITextPositionArray, text) <= 0 ? UIFont.empty : uITextPositionArray);
	}

	private int CalculatePlacement(Vector3[] points, ref UITextPosition[] positions, string text)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.CalculatePlacement(points, positions, text);
		}
		int num = this.FillMangle(points, 0, positions, 0, Mathf.Min((int)points.Length, (int)positions.Length));
		if (num <= 0)
		{
			return num;
		}
		return this.ProcessShared(num, ref positions, text);
	}

	public int CalculatePlacement(Vector3[] points, UITextPosition[] positions, string text)
	{
		if (positions == null)
		{
			throw new ArgumentNullException("positions");
		}
		return this.CalculatePlacement(points, ref positions, text, base.transform);
	}

	public UITextPosition[] CalculatePlacement(string text, params Vector3[] points)
	{
		UITextPosition[] uITextPositionArray = null;
		return (this.CalculatePlacement(points, ref uITextPositionArray, text) <= 0 ? UIFont.empty : uITextPositionArray);
	}

	private int CalculatePlacement(Vector3[] points, ref UITextPosition[] positions, string text, Matrix4x4 transform)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.CalculatePlacement(points, positions, text);
		}
		int num = this.FillMangle(points, 0, positions, 0, (int)points.Length);
		if (num <= 0)
		{
			return num;
		}
		for (int i = 0; i < num; i++)
		{
			UIFont.manglePoints[i] = transform.MultiplyPoint(UIFont.manglePoints[i]);
		}
		return this.ProcessShared(num, ref positions, text);
	}

	public int CalculatePlacement(Vector3[] points, UITextPosition[] positions, string text, Matrix4x4 transform)
	{
		if (positions == null)
		{
			throw new ArgumentNullException("positions");
		}
		return this.CalculatePlacement(points, ref positions, text, transform);
	}

	public UITextPosition CalculatePlacement(string text, Matrix4x4 transform, Vector3 point)
	{
		Vector3[] vector3Array = new Vector3[] { point };
		UITextPosition[] uITextPositionArray = new UITextPosition[1];
		UITextPosition uITextPosition = new UITextPosition();
		uITextPositionArray[0] = uITextPosition;
		UITextPosition[] uITextPositionArray1 = uITextPositionArray;
		this.CalculatePlacement(vector3Array, uITextPositionArray1, text, transform);
		return uITextPositionArray1[0];
	}

	public UITextPosition[] CalculatePlacement(string text, Matrix4x4 transform, params Vector3[] points)
	{
		UITextPosition[] uITextPositionArray = null;
		if (points == null)
		{
			return null;
		}
		if ((int)points.Length == 0)
		{
			return UIFont.empty;
		}
		return (this.CalculatePlacement(points, ref uITextPositionArray, text, transform) <= 0 ? UIFont.empty : uITextPositionArray);
	}

	private int CalculatePlacement(Vector2[] points, ref UITextPosition[] positions, string text, Matrix4x4 transform)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.CalculatePlacement(points, positions, text);
		}
		int num = this.FillMangle(points, 0, positions, 0, Mathf.Min((int)points.Length, (int)positions.Length));
		if (num <= 0)
		{
			return num;
		}
		for (int i = 0; i < num; i++)
		{
			UIFont.manglePoints[i] = transform.MultiplyPoint(UIFont.manglePoints[i]);
		}
		return this.ProcessShared(num, ref positions, text);
	}

	public int CalculatePlacement(Vector2[] points, UITextPosition[] positions, string text, Matrix4x4 transform)
	{
		if (positions == null)
		{
			throw new ArgumentNullException("positions");
		}
		return this.CalculatePlacement(points, ref positions, text, transform);
	}

	public UITextPosition[] CalculatePlacement(string text, Matrix4x4 transform, params Vector2[] points)
	{
		UITextPosition[] uITextPositionArray = null;
		if (points == null)
		{
			return null;
		}
		if ((int)points.Length == 0)
		{
			return UIFont.empty;
		}
		return (this.CalculatePlacement(points, ref uITextPositionArray, text, transform) <= 0 ? UIFont.empty : uITextPositionArray);
	}

	private int CalculatePlacement(Vector3[] points, ref UITextPosition[] positions, string text, Transform self)
	{
		if (!self)
		{
			return this.CalculatePlacement(points, positions, text);
		}
		return this.CalculatePlacement(points, positions, text, self.worldToLocalMatrix);
	}

	public int CalculatePlacement(Vector3[] points, UITextPosition[] positions, string text, Transform self)
	{
		if (positions == null)
		{
			throw new ArgumentNullException("positions");
		}
		return this.CalculatePlacement(points, ref positions, text, base.transform);
	}

	private int CalculatePlacement(Vector2[] points, ref UITextPosition[] positions, string text, Transform self)
	{
		if (!self)
		{
			return this.CalculatePlacement(points, positions, text);
		}
		return this.CalculatePlacement(points, positions, text, self.worldToLocalMatrix);
	}

	public int CalculatePlacement(Vector2[] points, UITextPosition[] positions, string text, Transform self)
	{
		if (positions == null)
		{
			throw new ArgumentNullException("positions");
		}
		return this.CalculatePlacement(points, ref positions, text, base.transform);
	}

	public UITextPosition CalculatePlacement(string text, Transform self, Vector2 point)
	{
		Vector2[] vector2Array = new Vector2[] { point };
		UITextPosition[] uITextPositionArray = new UITextPosition[1];
		UITextPosition uITextPosition = new UITextPosition();
		uITextPositionArray[0] = uITextPosition;
		UITextPosition[] uITextPositionArray1 = uITextPositionArray;
		this.CalculatePlacement(vector2Array, uITextPositionArray1, text, self);
		return uITextPositionArray1[0];
	}

	public UITextPosition[] CalculatePlacement(string text, Transform self, params Vector2[] points)
	{
		UITextPosition[] uITextPositionArray = null;
		if (points == null)
		{
			return null;
		}
		if ((int)points.Length == 0)
		{
			return UIFont.empty;
		}
		return (this.CalculatePlacement(points, ref uITextPositionArray, text, self) <= 0 ? UIFont.empty : uITextPositionArray);
	}

	public UITextPosition CalculatePlacement(string text, Vector3 point)
	{
		Vector3[] vector3Array = new Vector3[] { point };
		UITextPosition[] uITextPositionArray = new UITextPosition[1];
		UITextPosition uITextPosition = new UITextPosition();
		uITextPositionArray[0] = uITextPosition;
		UITextPosition[] uITextPositionArray1 = uITextPositionArray;
		this.CalculatePlacement(vector3Array, uITextPositionArray1, text);
		return uITextPositionArray1[0];
	}

	public UITextPosition CalculatePlacement(string text, Matrix4x4 transform, Vector2 point)
	{
		Vector2[] vector2Array = new Vector2[] { point };
		UITextPosition[] uITextPositionArray = new UITextPosition[1];
		UITextPosition uITextPosition = new UITextPosition();
		uITextPositionArray[0] = uITextPosition;
		UITextPosition[] uITextPositionArray1 = uITextPositionArray;
		this.CalculatePlacement(vector2Array, uITextPositionArray1, text, transform);
		return uITextPositionArray1[0];
	}

	public UITextPosition CalculatePlacement(string text, Transform self, Vector3 point)
	{
		Vector3[] vector3Array = new Vector3[] { point };
		UITextPosition[] uITextPositionArray = new UITextPosition[1];
		UITextPosition uITextPosition = new UITextPosition();
		uITextPositionArray[0] = uITextPosition;
		UITextPosition[] uITextPositionArray1 = uITextPositionArray;
		this.CalculatePlacement(vector3Array, uITextPositionArray1, text, self);
		return uITextPositionArray1[0];
	}

	public UITextPosition[] CalculatePlacement(string text, Transform self, params Vector3[] points)
	{
		UITextPosition[] uITextPositionArray = null;
		if (points == null)
		{
			return null;
		}
		if ((int)points.Length == 0)
		{
			return UIFont.empty;
		}
		return (this.CalculatePlacement(points, ref uITextPositionArray, text, self) <= 0 ? UIFont.empty : uITextPositionArray);
	}

	public Vector2 CalculatePrintedSize(string text, bool encoding, UIFont.SymbolStyle symbolStyle)
	{
		BMSymbol bMSymbol;
		BMGlyph bMGlyph;
		if (this.mReplacement != null)
		{
			return this.mReplacement.CalculatePrintedSize(text, encoding, symbolStyle);
		}
		Vector2 vector2 = Vector2.zero;
		if (this.mFont != null && this.mFont.isValid && !string.IsNullOrEmpty(text))
		{
			if (encoding)
			{
				text = NGUITools.StripSymbols(text);
			}
			int length = text.Length;
			int num = 0;
			int num1 = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = this.mFont.charSize + this.mSpacingY;
			for (int i = 0; i < length; i++)
			{
				char chr = text[i];
				if (chr == '\n')
				{
					if (num1 > num)
					{
						num = num1;
					}
					num1 = 0;
					num2 = num2 + num4;
					num3 = 0;
				}
				else if (chr < ' ')
				{
					num3 = 0;
				}
				else if (this.mFont.MatchSymbol(text, i, length, out bMSymbol))
				{
					num1 = num1 + this.mSpacingX + bMSymbol.width;
					i = i + (bMSymbol.sequence.Length - 1);
					num3 = 0;
				}
				else if (this.mFont.GetGlyph(chr, out bMGlyph))
				{
					num1 = num1 + this.mSpacingX + (num3 == 0 ? bMGlyph.advance : bMGlyph.advance + bMGlyph.GetKerning(num3));
					num3 = chr;
				}
			}
			float single = (this.mFont.charSize <= 0 ? 1f : 1f / (float)this.mFont.charSize);
			vector2.x = single * (float)((num1 <= num ? num : num1));
			vector2.y = single * (float)(num2 + num4);
		}
		return vector2;
	}

	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		if (a == null || b == null)
		{
			return false;
		}
		return (a == b || a.References(b) ? true : b.References(a));
	}

	private void DrawCarat(ref UIFont.PrintContext ctx)
	{
		Vector2 vector2 = new Vector2();
		Vector2 vector21 = new Vector2();
		Vector2 vector22 = new Vector2();
		Vector2 vector23 = new Vector2();
		vector2.x = ctx.scale.x * (float)(ctx.previousX + ctx.carratGlyph.offsetX);
		vector2.y = -ctx.scale.y * (float)(ctx.y + ctx.carratGlyph.offsetY);
		vector21.x = vector2.x + ctx.scale.x * (float)ctx.carratGlyph.width;
		vector21.y = vector2.y - ctx.scale.y * (float)ctx.carratGlyph.height;
		vector22.x = this.mUVRect.xMin + ctx.invX * (float)ctx.carratGlyph.x;
		vector22.y = this.mUVRect.yMax - ctx.invY * (float)ctx.carratGlyph.y;
		vector23.x = vector22.x + ctx.invX * (float)ctx.carratGlyph.width;
		vector23.y = vector22.y - ctx.invY * (float)ctx.carratGlyph.height;
		ctx.m.FastQuad(vector2, vector21, vector22, vector23, ctx.normalColor);
	}

	private static UITextMod EndLine(ref StringBuilder s)
	{
		int length = s.Length - 1;
		if (length > 0 && s[length] == ' ')
		{
			s[length] = '\n';
			return UITextMod.Replaced;
		}
		s.Append('\n');
		return UITextMod.Added;
	}

	private int FillMangle(Vector2[] points, int pointsOffset, UITextPosition[] positions, int positionsOffset, int len)
	{
		if (positions == null || points == null)
		{
			return 0;
		}
		if ((int)points.Length - pointsOffset < len || (int)positions.Length - positionsOffset < len)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (len > (int)UIFont.mangleIndices.Length)
		{
			Array.Resize<Vector3>(ref UIFont.manglePoints, len);
			Array.Resize<int>(ref UIFont.mangleIndices, len);
			Array.Resize<UITextPosition>(ref UIFont.manglePositions, len);
		}
		for (int i = 0; i < len; i++)
		{
			UIFont.manglePoints[i].x = points[i + pointsOffset].x;
			UIFont.manglePoints[i].y = points[i + pointsOffset].y;
			UIFont.manglePoints[i].z = 0f;
			UIFont.mangleIndices[i] = i;
		}
		return len;
	}

	private int FillMangle(Vector3[] points, int pointsOffset, UITextPosition[] positions, int positionsOffset, int len)
	{
		if (points == null)
		{
			throw new ArgumentNullException("null array", "points");
		}
		if ((int)points.Length - pointsOffset < len)
		{
			throw new ArgumentException("not large enough", "points");
		}
		if (positions != null && (int)positions.Length - positionsOffset < len)
		{
			throw new ArgumentException("not large enough", "positions");
		}
		if (len > (int)UIFont.mangleIndices.Length)
		{
			Array.Resize<Vector3>(ref UIFont.manglePoints, len);
			Array.Resize<int>(ref UIFont.mangleIndices, len);
			Array.Resize<UITextPosition>(ref UIFont.manglePositions, len);
		}
		for (int i = 0; i < len; i++)
		{
			UIFont.manglePoints[i] = points[i + pointsOffset];
			UIFont.mangleIndices[i] = i;
		}
		return len;
	}

	private void MangleSort(int len)
	{
		UIFont.mangleSort.SetLineSizing((double)this.bmFont.charSize, (double)this.verticalSpacing);
		Array.Sort<Vector3, int>(UIFont.manglePoints, UIFont.mangleIndices, 0, len, UIFont.mangleSort);
	}

	public void MarkAsDirty()
	{
		this.mSprite = null;
		UILabel[] uILabelArray = NGUITools.FindActive<UILabel>();
		int num = 0;
		int length = (int)uILabelArray.Length;
		while (num < length)
		{
			UILabel uILabel = uILabelArray[num];
			if (uILabel.enabled && uILabel.gameObject.activeInHierarchy && UIFont.CheckIfRelated(this, uILabel.font))
			{
				UIFont uIFont = uILabel.font;
				uILabel.font = null;
				uILabel.font = uIFont;
			}
			num++;
		}
	}

	public void Print(string text, Color color, MeshBuffer m, bool encoding, UIFont.SymbolStyle symbolStyle, UIFont.Alignment alignment, int lineWidth)
	{
		UITextSelection uITextSelection = new UITextSelection();
		this.Print(text, color, m, encoding, symbolStyle, alignment, lineWidth, ref uITextSelection, '\0', color, Color.clear, '\0', -1f);
	}

	public void Print(string text, Color normalColor, MeshBuffer m, bool encoding, UIFont.SymbolStyle symbolStyle, UIFont.Alignment alignment, int lineWidth, ref UITextSelection selection, char carratChar, Color highlightTextColor, Color highlightBarColor, char highlightChar, float highlightSplit)
	{
		UIFont.PrintContext length = new UIFont.PrintContext();
		bool flag;
		float single;
		bool glyph;
		if (this.mReplacement != null)
		{
			this.mReplacement.Print(text, normalColor, m, encoding, symbolStyle, alignment, lineWidth, ref selection, carratChar, highlightTextColor, highlightBarColor, highlightChar, highlightSplit);
		}
		else if (this.mFont != null && text != null)
		{
			if (!this.mFont.isValid)
			{
				Debug.LogError("Attempting to print using an invalid font!");
				return;
			}
			int num = 0;
			this.mColors.Clear();
			this.mColors.Add(normalColor);
			length.m = m;
			length.lineWidth = lineWidth;
			length.alignment = alignment;
			single = (this.mFont.charSize <= 0 ? 1f : 1f / (float)this.mFont.charSize);
			length.scale.x = single;
			length.scale.y = length.scale.x;
			length.normalColor = normalColor;
			length.indexOffset = length.m.vSize;
			length.maxX = 0;
			length.x = 0;
			length.y = 0;
			length.prev = 0;
			length.lineHeight = this.mFont.charSize + this.mSpacingY;
			length.v0 = new Vector3();
			length.v1 = new Vector3();
			length.u0 = new Vector2();
			length.u1 = new Vector2();
			Rect rect = this.uvRect;
			length.invX = rect.width / (float)this.mFont.texWidth;
			Rect rect1 = this.uvRect;
			length.invY = rect1.height / (float)this.mFont.texHeight;
			length.textLength = text.Length;
			length.nonHighlightColor = normalColor;
			length.carratChar = carratChar;
			if (length.carratChar != 0)
			{
				int num1 = selection.carratIndex;
				int num2 = num1;
				length.carratIndex = num1;
				if (num2 == -1)
				{
					length.carratGlyph = null;
					length.carratChar = '\0';
				}
				else if (!this.mFont.GetGlyph(carratChar, out length.carratGlyph))
				{
					length.carratIndex = -1;
				}
			}
			else
			{
				length.carratIndex = -1;
				length.carratGlyph = null;
			}
			length.highlightChar = highlightChar;
			length.highlightBarColor = highlightBarColor;
			length.highlightTextColor = highlightTextColor;
			length.highlightSplit = highlightSplit;
			length.highlightBarDraw = (length.highlightChar == 0 || length.highlightSplit < 0f || length.highlightSplit > 1f ? 1 : (int)(highlightBarColor.a <= 0f)) == 0;
			if (!length.highlightBarDraw && length.highlightTextColor == length.normalColor)
			{
				length.highlight = UIHighlight.invalid;
				length.highlightGlyph = null;
			}
			else if (selection.GetHighlight(out length.highlight))
			{
				if (length.highlightChar != length.carratChar)
				{
					glyph = !this.mFont.GetGlyph(length.highlightChar, out length.highlightGlyph);
				}
				else
				{
					BMGlyph bMGlyph = length.carratGlyph;
					BMGlyph bMGlyph1 = bMGlyph;
					length.highlightGlyph = bMGlyph;
					glyph = bMGlyph1 == null;
				}
				if (glyph)
				{
					length.highlight = UIHighlight.invalid;
				}
			}
			else
			{
				length.highlightGlyph = null;
				length.highlightBarDraw = false;
			}
			length.j = 0;
			length.previousX = 0;
			length.isLineEnd = false;
			length.highlightVertex = -1;
			length.glyph = null;
			length.c = '\0';
			length.skipSymbols = (!encoding ? true : symbolStyle == UIFont.SymbolStyle.None);
			length.printChar = false;
			length.printColor = normalColor;
			length.symbol = null;
			length.text = text;
			length.i = 0;
			while (length.i < length.textLength)
			{
				length.c = length.text[length.i];
				if (length.c == '\n')
				{
					length.isLineEnd = true;
				}
				else if (length.c >= ' ')
				{
					if (encoding && length.c == '[')
					{
						int num3 = NGUITools.ParseSymbol(text, length.i, this.mColors, ref num);
						if (num3 <= 0)
						{
							goto Label1;
						}
						length.nonHighlightColor = this.mColors[this.mColors.Count - 1];
						length.i = length.i + (num3 - 1);
						goto Label0;
					}
					if (!length.skipSymbols && this.mFont.MatchSymbol(length.text, length.i, length.textLength, out length.symbol))
					{
						length.v0.x = length.scale.x * (float)length.x;
						length.v0.y = -length.scale.y * (float)length.y;
						length.v1.x = length.v0.x + length.scale.x * (float)length.symbol.width;
						length.v1.y = length.v0.y - length.scale.y * (float)length.symbol.height;
						length.u0.x = this.mUVRect.xMin + length.invX * (float)length.symbol.x;
						length.u0.y = this.mUVRect.yMax - length.invY * (float)length.symbol.y;
						length.u1.x = length.u0.x + length.invX * (float)length.symbol.width;
						length.u1.y = length.u0.y - length.invY * (float)length.symbol.height;
						length.previousX = length.x;
						length.x = length.x + this.mSpacingX + length.symbol.width;
						length.i = length.i + (length.symbol.sequence.Length - 1);
						length.prev = 0;
						if (symbolStyle != UIFont.SymbolStyle.Colored)
						{
							length.printColor = (length.highlight.b.i <= length.j || length.highlight.a.i > length.j ? new Color(1f, 1f, 1f, length.nonHighlightColor.a) : length.highlightTextColor);
						}
						else
						{
							length.printColor = (length.highlight.b.i <= length.j || length.highlight.a.i > length.j ? length.nonHighlightColor : length.highlightTextColor);
						}
					}
					else if (this.mFont.GetGlyph(length.c, out length.glyph))
					{
						flag = length.prev != 0;
						if (flag)
						{
							length.previousX = length.x;
							length.x = length.x + length.glyph.GetKerning(length.prev);
						}
						if (length.c == ' ')
						{
							goto Label3;
						}
						length.v0.x = length.scale.x * (float)(length.x + length.glyph.offsetX);
						length.v0.y = -length.scale.y * (float)(length.y + length.glyph.offsetY);
						length.v1.x = length.v0.x + length.scale.x * (float)length.glyph.width;
						length.v1.y = length.v0.y - length.scale.y * (float)length.glyph.height;
						length.u0.x = this.mUVRect.xMin + length.invX * (float)length.glyph.x;
						length.u0.y = this.mUVRect.yMax - length.invY * (float)length.glyph.y;
						length.u1.x = length.u0.x + length.invX * (float)length.glyph.width;
						length.u1.y = length.u0.y - length.invY * (float)length.glyph.height;
						if (!flag)
						{
							length.previousX = length.x;
						}
						length.x = length.x + this.mSpacingX + length.glyph.advance;
						length.prev = length.c;
						if (length.glyph.channel == 0 || length.glyph.channel == 15)
						{
							length.printColor = (length.highlight.b.i <= length.j || length.highlight.a.i > length.j ? length.nonHighlightColor : length.highlightTextColor);
						}
						else
						{
							Color color = (length.highlight.b.i <= length.j || length.highlight.a.i > length.j ? length.nonHighlightColor : length.highlightTextColor);
							color = color * 0.49f;
							switch (length.glyph.channel)
							{
								case 1:
								{
									color.b = color.b + 0.51f;
									goto case 7;
								}
								case 2:
								{
									color.g = color.g + 0.51f;
									goto case 7;
								}
								case 3:
								case 5:
								case 6:
								case 7:
								{
									length.printColor = color;
									break;
								}
								case 4:
								{
									color.r = color.r + 0.51f;
									goto case 7;
								}
								case 8:
								{
									color.a = color.a + 0.51f;
									goto case 7;
								}
								default:
								{
									goto case 7;
								}
							}
						}
					}
					else
					{
						goto Label2;
					}
					length.printChar = true;
				}
				else
				{
					length.prev = 0;
					goto Label0;
				}
				if (length.highlight.b.i == length.j)
				{
					if (length.printChar)
					{
						length.m.FastQuad(length.v0, length.v1, length.u0, length.u1, length.printColor);
						length.printChar = false;
					}
					if (length.highlightBarDraw)
					{
						this.PutHighlightEnd(ref length);
					}
				}
				else if (length.highlight.a.i == length.j)
				{
					if (length.highlightBarDraw)
					{
						this.PutHighlightStart(ref length);
					}
					if (length.printChar)
					{
						length.m.FastQuad(length.v0, length.v1, length.u0, length.u1, length.printColor);
						length.printChar = false;
					}
				}
				else if (length.carratIndex == length.j)
				{
					if (length.printChar)
					{
						length.m.FastQuad(length.v0, length.v1, length.u0, length.u1, length.printColor);
						length.printChar = false;
					}
					this.DrawCarat(ref length);
				}
				else if (length.printChar)
				{
					length.m.FastQuad(length.v0, length.v1, length.u0, length.u1, length.printColor);
					length.printChar = false;
				}
				length.j = length.j + 1;
				if (length.isLineEnd)
				{
					length.isLineEnd = false;
					if (length.x > length.maxX)
					{
						length.maxX = length.x;
					}
					bool flag1 = (!length.highlightBarDraw ? false : length.highlightVertex != -1);
					if (flag1)
					{
						this.PutHighlightEnd(ref length);
					}
					if (length.indexOffset < length.m.vSize)
					{
						this.Align(ref length);
						length.indexOffset = length.m.vSize;
					}
					length.previousX = length.x;
					length.x = 0;
					length.y = length.y + length.lineHeight;
					length.prev = 0;
					if (flag1)
					{
						this.PutHighlightStart(ref length);
					}
				}
			Label0:
				length.i = length.i + 1;
			}
			length.previousX = length.x;
			if (length.highlightVertex != -1)
			{
				this.PutHighlightEnd(ref length);
			}
			else if (length.j == length.carratIndex)
			{
				this.DrawCarat(ref length);
			}
			if (length.indexOffset < length.m.vSize)
			{
				this.Align(ref length);
				length.indexOffset = length.m.vSize;
			}
		}
		return;
	Label3:
		if (!flag)
		{
			length.previousX = length.x;
		}
		length.x = length.x + this.mSpacingX + length.glyph.advance;
		length.prev = length.c;
		goto Label2;
	}

	private int ProcessPlacement(int count, string text)
	{
		BMGlyph bMGlyph;
		int num;
		if (this.mReplacement != null)
		{
			return this.mReplacement.ProcessPlacement(count, text);
		}
		int num1 = 0;
		if (UIFont.manglePoints[UIFont.mangleIndices[num1]].y < 0f)
		{
			do
			{
				UIFont.manglePositions[num1] = new UITextPosition(UITextRegion.Before);
				num = num1 + 1;
				num1 = num;
			}
			while (num < count && UIFont.manglePoints[UIFont.mangleIndices[num1]].y < 0f);
			if (num1 >= count)
			{
				return count;
			}
		}
		int length = text.Length;
		int num2 = this.verticalSpacing + this.bmFont.charSize;
		if (length == 0)
		{
			while (UIFont.manglePoints[UIFont.mangleIndices[num1]].y <= (float)num2)
			{
				if (UIFont.manglePoints[UIFont.mangleIndices[num1]].x >= 0f)
				{
					UIFont.manglePositions[num1] = new UITextPosition(UITextRegion.Past);
				}
				else
				{
					UIFont.manglePositions[num1] = new UITextPosition(UITextRegion.Pre);
				}
				int num3 = num1 + 1;
				num1 = num3;
				if (num3 < count)
				{
					continue;
				}
				return count;
			}
			while (num1 < count)
			{
				int num4 = num1;
				num1 = num4 + 1;
				UIFont.manglePositions[num4] = new UITextPosition(UITextRegion.End);
			}
			return count;
		}
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = -1;
		int kerning = 0;
		int num10 = 0;
		bool flag = false;
		bool flag1 = false;
	Label0:
		while (num1 < count)
		{
			Vector3 vector3 = UIFont.manglePoints[UIFont.mangleIndices[num1]];
			int num11 = Mathf.FloorToInt(vector3.y) / num2;
		Label1:
			while (!flag1)
			{
				if (num11 > num5)
				{
					flag = false;
					do
					{
						while (text[num7] != '\n')
						{
							int num12 = num7 + 1;
							num7 = num12;
							if (num12 < length)
							{
								num6++;
							}
							else
							{
								flag1 = true;
								goto Label1;
							}
						}
						num5++;
						num6 = 0;
						num10 = 0;
						num9 = num7;
						int num13 = num7 + 1;
						num7 = num13;
						num8 = num13;
						kerning = 0;
					}
					while (num11 > num5);
					goto Label0;
				}
				else if (vector3.x < 0f)
				{
					int num14 = num1;
					num1 = num14 + 1;
					UIFont.manglePositions[num14] = new UITextPosition(num5, 0, num8, UITextRegion.Pre);
					goto Label0;
				}
				else if (!flag)
				{
					while ((float)kerning < vector3.x)
					{
						if (num7 < length)
						{
							int num15 = text[num7];
							if (num15 != 10)
							{
								if (this.mFont.GetGlyph(num15, out bMGlyph))
								{
									if (num9 >= num8)
									{
										kerning = kerning + bMGlyph.GetKerning(text[num9]);
									}
									kerning = kerning + this.mSpacingX + bMGlyph.advance;
								}
								int num16 = num7;
								num7 = num16 + 1;
								num9 = num16;
								int num17 = num6;
								num6 = num17 + 1;
								num10 = num17;
							}
							else
							{
								num9 = num7;
								int num18 = num7 + 1;
								num7 = num18;
								num8 = num18;
								num10 = num6;
								num6 = 0;
								flag = true;
								goto Label1;
							}
						}
						else
						{
							flag1 = true;
							goto Label1;
						}
					}
					int num19 = num1;
					num1 = num19 + 1;
					UIFont.manglePositions[num19] = new UITextPosition(num5, num10, num9, UITextRegion.Inside);
					goto Label0;
				}
				else
				{
					int num20 = num1;
					num1 = num20 + 1;
					UIFont.manglePositions[num20] = new UITextPosition(num5, num10, num9, UITextRegion.Past);
					goto Label0;
				}
			}
			if (num11 != num5)
			{
				while (num1 < count)
				{
					int num21 = num1;
					num1 = num21 + 1;
					UIFont.manglePositions[num21] = new UITextPosition(num5, num6, num7, UITextRegion.End);
				}
			}
			else
			{
				int num22 = num1;
				num1 = num22 + 1;
				UIFont.manglePositions[num22] = new UITextPosition(num5, num6, num7, UITextRegion.Past);
			}
		}
		if (num1 < count)
		{
			Debug.LogError(string.Concat(" skipped ", count - num1));
		}
		return count;
	}

	private int ProcessShared(int len, ref UITextPosition[] positions, string text)
	{
		if (this.mFont.charSize > 0)
		{
			for (int i = 0; i < len; i++)
			{
				UIFont.manglePoints[i].x = UIFont.manglePoints[i].x * (float)this.mFont.charSize;
				UIFont.manglePoints[i].y = UIFont.manglePoints[i].y * (float)this.mFont.charSize;
			}
		}
		this.MangleSort(len);
		len = this.ProcessPlacement(len, text);
		if (len > 0)
		{
			if (positions == null)
			{
				positions = new UITextPosition[len];
			}
			for (int j = 0; j < len; j++)
			{
				positions[UIFont.mangleIndices[j]] = UIFont.manglePositions[j];
			}
		}
		return len;
	}

	private void PutHighlightEnd(ref UIFont.PrintContext ctx)
	{
		if (ctx.highlightVertex == -1)
		{
			return;
		}
		float single = ctx.scale.x * (float)(ctx.previousX + ctx.highlightGlyph.offsetX) - ctx.m.v[ctx.highlightVertex].x;
		if (single > 0f)
		{
			ctx.m.v[ctx.highlightVertex].x = ctx.m.v[ctx.highlightVertex].x + single;
			ctx.m.v[ctx.highlightVertex + 1].x = ctx.m.v[ctx.highlightVertex + 1].x + single;
			ctx.m.v[ctx.highlightVertex + 4].x = ctx.m.v[ctx.highlightVertex + 4].x + single;
			ctx.m.v[ctx.highlightVertex + 4 + 1].x = ctx.m.v[ctx.highlightVertex + 4 + 1].x + single;
			ctx.m.v[ctx.highlightVertex + 4 + 2].x = ctx.m.v[ctx.highlightVertex + 4 + 2].x + single;
			ctx.m.v[ctx.highlightVertex + 4 + 3].x = ctx.m.v[ctx.highlightVertex + 4 + 3].x + single;
		}
		ctx.highlightVertex = -1;
	}

	private void PutHighlightStart(ref UIFont.PrintContext ctx)
	{
		Vector2 vector2 = new Vector2();
		Vector2 vector21 = new Vector2();
		Vector2 vector22 = new Vector2();
		Vector2 vector23 = new Vector2();
		if (ctx.highlightVertex != -1)
		{
			this.PutHighlightEnd(ref ctx);
		}
		float single = ctx.scale.x * ((float)ctx.highlightGlyph.width * ctx.highlightSplit);
		vector2.x = ctx.scale.x * (float)(ctx.previousX + ctx.highlightGlyph.offsetX) - single;
		vector2.y = -ctx.scale.y * (float)(ctx.y + ctx.highlightGlyph.offsetY);
		vector21.x = vector2.x + single;
		float single1 = vector21.x - vector2.x;
		vector2.x = vector2.x + single1;
		vector21.x = vector21.x + single1;
		vector21.y = vector2.y - ctx.scale.y * (float)ctx.highlightGlyph.height;
		vector22.x = this.mUVRect.xMin + ctx.invX * (float)ctx.highlightGlyph.x;
		vector22.y = this.mUVRect.yMax - ctx.invY * (float)ctx.highlightGlyph.y;
		vector23.x = vector22.x + ctx.invX * (float)ctx.highlightGlyph.width * ctx.highlightSplit;
		vector23.y = vector22.y - ctx.invY * (float)ctx.highlightGlyph.height;
		ctx.m.FastQuad(vector2, vector21, vector22, vector23, ctx.highlightBarColor);
		ctx.highlightVertex = ctx.m.FastQuad(new Vector2(vector21.x, vector2.y), vector21, new Vector2(vector23.x, vector22.y), vector23, ctx.highlightBarColor);
		float single2 = vector21.x;
		vector21.x = vector2.x + ctx.scale.x * (float)ctx.highlightGlyph.width;
		vector2.x = single2;
		single2 = vector23.x;
		vector23.x = vector22.x + ctx.invX * (float)ctx.highlightGlyph.width;
		vector22.x = single2;
		ctx.m.FastQuad(vector2, vector21, vector22, vector23, ctx.highlightBarColor);
	}

	private bool References(UIFont font)
	{
		if (font == null)
		{
			return false;
		}
		if (font == this)
		{
			return true;
		}
		return (this.mReplacement == null ? false : this.mReplacement.References(font));
	}

	private void Trim()
	{
		Texture texture = this.mAtlas.texture;
		if (texture != null && this.mSprite != null)
		{
			Rect pixels = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
			Rect rect = (this.mAtlas.coordinates != UIAtlas.Coordinates.TexCoords ? this.mSprite.outer : NGUIMath.ConvertToPixels(this.mSprite.outer, texture.width, texture.height, true));
			int num = Mathf.RoundToInt(rect.xMin - pixels.xMin);
			int num1 = Mathf.RoundToInt(rect.yMin - pixels.yMin);
			int num2 = Mathf.RoundToInt(rect.xMax - pixels.xMin);
			int num3 = Mathf.RoundToInt(rect.yMax - pixels.yMin);
			this.mFont.Trim(num, num1, num2, num3);
		}
	}

	public string WrapText(List<UITextMarkup> markups, string text, float maxWidth, int maxLineCount, bool encoding, UIFont.SymbolStyle symbolStyle)
	{
		BMGlyph bMGlyph;
		int num;
		UITextMod uITextMod;
		bool flag;
		StringBuilder stringBuilder;
		if (this.mReplacement != null)
		{
			return this.mReplacement.WrapText(markups, text, maxWidth, maxLineCount, encoding, symbolStyle);
		}
		markups = markups ?? UIFont.tempMarkup;
		markups.Clear();
		int num1 = Mathf.RoundToInt(maxWidth * (float)this.size);
		if (num1 < 1)
		{
			return text;
		}
		StringBuilder stringBuilder1 = new StringBuilder();
		int length = text.Length;
		int num2 = num1;
		int num3 = 0;
		int num4 = 0;
		int length1 = 0;
		bool flag1 = true;
		bool flag2 = maxLineCount != 1;
		int num5 = 1;
		BMSymbol bMSymbol = null;
		int num6 = 0;
		while (length1 < length)
		{
			char chr = text[length1];
			if (num3 == 92 && chr == 'n')
			{
				if (num4 >= length1 - 1)
				{
					stringBuilder1.Append(chr);
				}
				else
				{
					stringBuilder1.Append(text.Substring(num4, length1 - (num4 + 2)));
				}
				stringBuilder1.Remove(stringBuilder1.Length - 1, 1);
				UITextMarkup uITextMarkup = new UITextMarkup()
				{
					index = length1 - 1,
					mod = UITextMod.Removed
				};
				markups.Add(uITextMarkup);
				UITextMarkup uITextMarkup1 = new UITextMarkup()
				{
					index = length1,
					mod = UITextMod.Replaced,
					@value = '\n'
				};
				markups.Add(uITextMarkup1);
				num4 = length1 + 1;
				chr = '\n';
			}
			if (chr != '\n')
			{
				if (chr == ' ' && num3 != 32 && num4 < length1)
				{
					stringBuilder1.Append(text.Substring(num4, length1 - num4 + 1));
					flag1 = false;
					num4 = length1 + 1;
					num3 = chr;
				}
				if (encoding && chr == '[' && length1 + 2 < length)
				{
					if (text[length1 + 2] != ']')
					{
						if (length1 + 7 >= length || text[length1 + 7] != ']' || num6 != 0)
						{
							goto Label1;
						}
						UITextMarkup uITextMarkup2 = new UITextMarkup()
						{
							index = length1,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup2);
						UITextMarkup uITextMarkup3 = new UITextMarkup()
						{
							index = length1 + 1,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup3);
						UITextMarkup uITextMarkup4 = new UITextMarkup()
						{
							index = length1 + 2,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup4);
						UITextMarkup uITextMarkup5 = new UITextMarkup()
						{
							index = length1 + 3,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup5);
						UITextMarkup uITextMarkup6 = new UITextMarkup()
						{
							index = length1 + 4,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup6);
						UITextMarkup uITextMarkup7 = new UITextMarkup()
						{
							index = length1 + 5,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup7);
						UITextMarkup uITextMarkup8 = new UITextMarkup()
						{
							index = length1 + 6,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup8);
						UITextMarkup uITextMarkup9 = new UITextMarkup()
						{
							index = length1 + 7,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup9);
						length1 = length1 + 7;
						goto Label0;
					}
					else
					{
						if (text[length1 + 1] == '-')
						{
							if (num6 == 0)
							{
								goto Label4;
							}
						}
						else if (text[length1 + 1] == '»')
						{
							int num7 = num6;
							num6 = num7 + 1;
							if (num7 == 0)
							{
								goto Label3;
							}
						}
						else if (text[length1 + 1] == '«')
						{
							int num8 = num6 - 1;
							num6 = num8;
							if (num8 != 0)
							{
								goto Label2;
							}
							UITextMarkup uITextMarkup10 = new UITextMarkup()
							{
								index = length1,
								mod = UITextMod.Removed
							};
							markups.Add(uITextMarkup10);
							UITextMarkup uITextMarkup11 = new UITextMarkup()
							{
								index = length1 + 1,
								mod = UITextMod.Removed
							};
							markups.Add(uITextMarkup11);
							UITextMarkup uITextMarkup12 = new UITextMarkup()
							{
								index = length1 + 2,
								mod = UITextMod.Removed
							};
							markups.Add(uITextMarkup12);
							length1 = length1 + 2;
							goto Label0;
						}
					Label2:
					}
				}
			Label1:
				flag = (!encoding || symbolStyle == UIFont.SymbolStyle.None ? false : this.mFont.MatchSymbol(text, length1, length, out bMSymbol));
				bool flag3 = flag;
				if (!flag3)
				{
					if (this.mFont.GetGlyph(chr, out bMGlyph))
					{
						goto Label5;
					}
					UITextMarkup uITextMarkup13 = new UITextMarkup()
					{
						index = length1,
						mod = UITextMod.Removed
					};
					markups.Add(uITextMarkup13);
					goto Label0;
				}
				else
				{
					num = this.mSpacingX + bMSymbol.width;
				}
			Label7:
				num2 = num2 - num;
				if (num2 >= 0)
				{
					num3 = chr;
				}
				else
				{
					if (!flag1 && flag2 && num5 != maxLineCount)
					{
						goto Label6;
					}
					stringBuilder1.Append(text.Substring(num4, Mathf.Max(0, length1 - num4)));
					if (!flag2 || num5 == maxLineCount)
					{
						num4 = length1;
						markups.Add(new UITextMarkup()
						{
							index = length1
						});
						break;
					}
					else
					{
						uITextMod = UIFont.EndLine(ref stringBuilder1);
						if (uITextMod == UITextMod.Replaced)
						{
							UITextMarkup uITextMarkup14 = new UITextMarkup()
							{
								index = length1,
								mod = UITextMod.Replaced,
								@value = '\n'
							};
							markups.Add(uITextMarkup14);
						}
						else if (uITextMod == UITextMod.Added)
						{
							UITextMarkup uITextMarkup15 = new UITextMarkup()
							{
								index = length1,
								mod = UITextMod.Added
							};
							markups.Add(uITextMarkup15);
						}
						flag1 = true;
						num5++;
						if (chr != ' ')
						{
							num4 = length1;
							num2 = num1 - num;
						}
						else
						{
							num4 = length1 + 1;
							num2 = num1;
							UITextMarkup uITextMarkup16 = new UITextMarkup()
							{
								index = length1,
								mod = UITextMod.Removed
							};
							markups.Add(uITextMarkup16);
						}
						num3 = 0;
					}
				}
				if (flag3)
				{
					for (int i = 0; i < bMSymbol.sequence.Length; i++)
					{
						UITextMarkup uITextMarkup17 = new UITextMarkup()
						{
							index = length1 + i,
							mod = UITextMod.Removed
						};
						markups.Add(uITextMarkup17);
					}
					length1 = length1 + (bMSymbol.sequence.Length - 1);
					num3 = 0;
				}
			}
			else if (!flag2 || num5 == maxLineCount)
			{
				markups.Add(new UITextMarkup()
				{
					index = length1
				});
				break;
			}
			else
			{
				num2 = num1;
				if (num4 >= length1)
				{
					stringBuilder1.Append(chr);
				}
				else
				{
					stringBuilder1.Append(text.Substring(num4, length1 - num4 + 1));
				}
				flag1 = true;
				num5++;
				num4 = length1 + 1;
				num3 = 0;
			}
		Label0:
			length1++;
		}
		if (num4 < length1)
		{
			stringBuilder = stringBuilder1.Append(text.Substring(num4, length1 - num4));
		}
		return stringBuilder1.ToString();
	Label3:
		UITextMarkup uITextMarkup18 = new UITextMarkup()
		{
			index = length1,
			mod = UITextMod.Removed
		};
		markups.Add(uITextMarkup18);
		UITextMarkup uITextMarkup19 = new UITextMarkup()
		{
			index = length1 + 1,
			mod = UITextMod.Removed
		};
		markups.Add(uITextMarkup19);
		UITextMarkup uITextMarkup20 = new UITextMarkup()
		{
			index = length1 + 2,
			mod = UITextMod.Removed
		};
		markups.Add(uITextMarkup20);
		length1 = length1 + 2;
		goto Label0;
	Label4:
		UITextMarkup uITextMarkup21 = new UITextMarkup()
		{
			index = length1,
			mod = UITextMod.Removed
		};
		markups.Add(uITextMarkup21);
		UITextMarkup uITextMarkup22 = new UITextMarkup()
		{
			index = length1 + 1,
			mod = UITextMod.Removed
		};
		markups.Add(uITextMarkup22);
		UITextMarkup uITextMarkup23 = new UITextMarkup()
		{
			index = length1 + 2,
			mod = UITextMod.Removed
		};
		markups.Add(uITextMarkup23);
		length1 = length1 + 2;
		goto Label0;
	Label5:
		int num9 = this.mSpacingX;
		num = num9 + (num3 == 0 ? bMGlyph.advance : bMGlyph.advance + bMGlyph.GetKerning(num3));
		goto Label7;
	Label6:
		while (num4 < length && text[num4] == ' ')
		{
			UITextMarkup uITextMarkup24 = new UITextMarkup()
			{
				index = num4,
				mod = UITextMod.Removed
			};
			markups.Add(uITextMarkup24);
			num4++;
		}
		flag1 = true;
		num2 = num1;
		length1 = num4 - 1;
		int count = markups.Count - 1;
		while (count >= 0)
		{
			if (markups[count].index < length1)
			{
				break;
			}
			else
			{
				markups.RemoveAt(count);
				count--;
			}
		}
		num3 = 0;
		if (!flag2 || num5 == maxLineCount)
		{
			markups.Add(new UITextMarkup()
			{
				index = length1
			});
			if (num4 < length1)
			{
				stringBuilder = stringBuilder1.Append(text.Substring(num4, length1 - num4));
			}
			return stringBuilder1.ToString();
		}
		else
		{
			num5++;
			uITextMod = UIFont.EndLine(ref stringBuilder1);
			if (uITextMod == UITextMod.Replaced)
			{
				UITextMarkup uITextMarkup25 = new UITextMarkup()
				{
					index = length1,
					mod = UITextMod.Replaced,
					@value = '\n'
				};
				markups.Add(uITextMarkup25);
			}
			else if (uITextMod == UITextMod.Added)
			{
				UITextMarkup uITextMarkup26 = new UITextMarkup()
				{
					index = length1,
					mod = UITextMod.Added
				};
				markups.Add(uITextMarkup26);
			}
			goto Label0;
		}
	}

	public string WrapText(List<UITextMarkup> markups, string text, float maxWidth, int maxLineCount, bool encoding)
	{
		return this.WrapText(markups, text, maxWidth, maxLineCount, encoding, UIFont.SymbolStyle.None);
	}

	public string WrapText(List<UITextMarkup> markups, string text, float maxWidth, int maxLineCount)
	{
		return this.WrapText(markups, text, maxWidth, maxLineCount, false, UIFont.SymbolStyle.None);
	}

	public enum Alignment
	{
		Left,
		Center,
		Right,
		LeftOverflowRight
	}

	private class MangleSorter : Comparer<Vector3>
	{
		public double lineHeight;

		public double vSpacing;

		private bool noLineSize;

		private bool noVSpacing;

		public MangleSorter()
		{
		}

		public override int Compare(Vector3 x, Vector3 y)
		{
			int num;
			if (this.noLineSize)
			{
				num = x.y.CompareTo(y.y);
			}
			else
			{
				double num1 = (double)x.y / this.lineHeight;
				double num2 = (double)y.y / this.lineHeight;
				if (!this.noVSpacing)
				{
					if (num1 >= 1 || num1 <= -1)
					{
						num1 = ((double)x.y - this.lineHeight) / (this.lineHeight + this.vSpacing);
					}
					if (num2 >= 1 || num2 <= -1)
					{
						num2 = ((double)y.y - this.lineHeight) / (this.lineHeight + this.vSpacing);
					}
				}
				num1 = (num1 >= 0 ? Math.Floor(num1) : -Math.Ceiling(-num1));
				num2 = (num2 >= 0 ? Math.Floor(num2) : -Math.Ceiling(-num2));
				num = num1.CompareTo(num2);
			}
			if (num == 0)
			{
				num = x.x.CompareTo(y.x);
				if (num == 0)
				{
					num = x.z.CompareTo(y.z);
				}
			}
			return num;
		}

		public void SetLineSizing(double height, double spacing)
		{
			if (height != 0)
			{
				this.lineHeight = height;
				if (spacing == 0)
				{
					this.noVSpacing = true;
					this.noLineSize = false;
				}
				else if (spacing != -height)
				{
					this.vSpacing = spacing;
					this.noLineSize = false;
					this.noVSpacing = false;
				}
				else
				{
					this.noLineSize = true;
					this.noVSpacing = true;
				}
			}
			else if (spacing != 0)
			{
				this.lineHeight = spacing;
				this.noVSpacing = true;
				this.noLineSize = false;
			}
			else
			{
				this.noLineSize = true;
			}
		}
	}

	private struct PrintContext
	{
		public MeshBuffer m;

		public BMGlyph glyph;

		public BMGlyph highlightGlyph;

		public BMGlyph carratGlyph;

		public BMSymbol symbol;

		public string text;

		public UIHighlight highlight;

		public Color printColor;

		public Color nonHighlightColor;

		public Color normalColor;

		public Color highlightTextColor;

		public Color highlightBarColor;

		public Vector3 v0;

		public Vector3 v1;

		public Vector2 u0;

		public Vector2 u1;

		public Vector2 scale;

		public float invX;

		public float invY;

		public float highlightSplit;

		public int x;

		public int maxX;

		public int previousX;

		public int y;

		public int highlightVertex;

		public int prev;

		public int lineHeight;

		public int lineWidth;

		public int indexOffset;

		public int textLength;

		public int i;

		public int carratIndex;

		public int j;

		public UIFont.Alignment alignment;

		public char carratChar;

		public char highlightChar;

		public char c;

		public bool highlightBarDraw;

		public bool isLineEnd;

		public bool skipSymbols;

		public bool printChar;
	}

	public enum SymbolStyle
	{
		None,
		Uncolored,
		Colored
	}
}