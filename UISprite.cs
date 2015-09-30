using NGUI.Meshing;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Basic)")]
[ExecuteInEditMode]
public class UISprite : UIWidget
{
	private const UIWidget.WidgetFlags kRequiredFlags = UIWidget.WidgetFlags.CustomPivotOffset | UIWidget.WidgetFlags.CustomMaterialGet;

	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	protected UIAtlas.Sprite mSprite;

	protected Rect mOuter;

	protected Rect mOuterUV;

	private bool mSpriteSet;

	private string mLastName = string.Empty;

	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			UIMaterial uIMaterial;
			if (this.mAtlas != value)
			{
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				if (this.mAtlas == null)
				{
					uIMaterial = null;
				}
				else
				{
					uIMaterial = (UIMaterial)this.mAtlas.spriteMaterial;
				}
				this.material = uIMaterial;
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.sprite = this.mAtlas.spriteList[0];
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string str = this.mSpriteName;
					this.mSpriteName = string.Empty;
					this.spriteName = str;
					base.ChangedAuto();
					this.UpdateUVs(true);
				}
			}
		}
	}

	public Vector4 border
	{
		get
		{
			if ((byte)(this.widgetFlags & UIWidget.WidgetFlags.CustomBorder) != 16)
			{
				return Vector4.zero;
			}
			return this.customBorder;
		}
	}

	protected virtual Vector4 customBorder
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	protected override UIMaterial customMaterial
	{
		get
		{
			return this.material;
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
				if (this.mAtlas == null)
				{
					uIMaterial = null;
				}
				else
				{
					uIMaterial = (UIMaterial)this.mAtlas.spriteMaterial;
				}
				uIMaterial1 = uIMaterial;
				this.mSprite = null;
				base.baseMaterial = uIMaterial1;
				if (uIMaterial1 != null)
				{
					this.UpdateUVs(true);
				}
			}
			return uIMaterial1;
		}
		set
		{
			base.material = value;
		}
	}

	public Rect outerUV
	{
		get
		{
			this.UpdateUVs(false);
			return this.mOuterUV;
		}
	}

	public new Vector2 pivotOffset
	{
		get
		{
			Vector2 vector2 = Vector2.zero;
			if (this.sprite != null)
			{
				UIWidget.Pivot pivot = base.pivot;
				if (pivot == UIWidget.Pivot.Top || pivot == UIWidget.Pivot.Center || pivot == UIWidget.Pivot.Bottom)
				{
					vector2.x = (-1f - this.mSprite.paddingRight + this.mSprite.paddingLeft) * 0.5f;
				}
				else if (pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.BottomRight)
				{
					vector2.x = -1f - this.mSprite.paddingRight;
				}
				else
				{
					vector2.x = this.mSprite.paddingLeft;
				}
				if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.Center || pivot == UIWidget.Pivot.Right)
				{
					vector2.y = (1f + this.mSprite.paddingBottom - this.mSprite.paddingTop) * 0.5f;
				}
				else if (pivot == UIWidget.Pivot.BottomLeft || pivot == UIWidget.Pivot.Bottom || pivot == UIWidget.Pivot.BottomRight)
				{
					vector2.y = 1f + this.mSprite.paddingBottom;
				}
				else
				{
					vector2.y = -this.mSprite.paddingTop;
				}
			}
			return vector2;
		}
	}

	public UIAtlas.Sprite sprite
	{
		get
		{
			if (!this.mSpriteSet)
			{
				this.mSprite = null;
			}
			if (this.mSprite == null && this.mAtlas != null)
			{
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					this.sprite = this.mAtlas.GetSprite(this.mSpriteName);
				}
				if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
				{
					this.sprite = this.mAtlas.spriteList[0];
					this.mSpriteName = this.mSprite.name;
				}
				if (this.mSprite != null)
				{
					this.material = (UIMaterial)this.mAtlas.spriteMaterial;
				}
			}
			return this.mSprite;
		}
		set
		{
			UIMaterial uIMaterial;
			this.mSprite = value;
			this.mSpriteSet = true;
			if (this.mSprite == null || !(this.mAtlas != null))
			{
				uIMaterial = null;
			}
			else
			{
				uIMaterial = (UIMaterial)this.mAtlas.spriteMaterial;
			}
			this.material = uIMaterial;
		}
	}

	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (string.IsNullOrEmpty(this.mSpriteName))
				{
					return;
				}
				this.mSpriteName = string.Empty;
				this.mSprite = null;
				base.ChangedAuto();
			}
			else if (this.mSpriteName != value)
			{
				this.mSpriteName = value;
				this.mSprite = null;
				base.ChangedAuto();
				if (this.mSprite != null)
				{
					this.UpdateUVs(true);
				}
			}
		}
	}

	public UISprite() : base(UIWidget.WidgetFlags.CustomPivotOffset | UIWidget.WidgetFlags.CustomMaterialGet)
	{
	}

	protected UISprite(UIWidget.WidgetFlags additionalFlags) : base((UIWidget.WidgetFlags)((byte)(UIWidget.WidgetFlags.CustomPivotOffset | UIWidget.WidgetFlags.CustomMaterialGet | additionalFlags)))
	{
	}

	protected override void GetCustomVector2s(int start, int end, UIWidget.WidgetFlags[] flags, Vector2[] v)
	{
		for (int i = start; i < end; i++)
		{
			if (flags[i] != UIWidget.WidgetFlags.CustomPivotOffset)
			{
				base.GetCustomVector2s(i, i + 1, flags, v);
			}
			else
			{
				v[i] = this.pivotOffset;
			}
		}
	}

	public override void MakePixelPerfect()
	{
		if (this.sprite == null)
		{
			return;
		}
		Texture texture = base.mainTexture;
		Vector3 num = base.cachedTransform.localScale;
		if (texture != null)
		{
			Rect pixels = NGUIMath.ConvertToPixels(this.outerUV, texture.width, texture.height, true);
			float single = this.atlas.pixelSize;
			num.x = (float)Mathf.RoundToInt(pixels.width * single);
			num.y = (float)Mathf.RoundToInt(pixels.height * single);
			num.z = 1f;
			base.cachedTransform.localScale = num;
		}
		int num1 = Mathf.RoundToInt(num.x * (1f + this.mSprite.paddingLeft + this.mSprite.paddingRight));
		int num2 = Mathf.RoundToInt(num.y * (1f + this.mSprite.paddingTop + this.mSprite.paddingBottom));
		Vector3 vector3 = base.cachedTransform.localPosition;
		vector3.z = (float)Mathf.RoundToInt(vector3.z);
		if (num1 % 2 != 1 || base.pivot != UIWidget.Pivot.Top && base.pivot != UIWidget.Pivot.Center && base.pivot != UIWidget.Pivot.Bottom)
		{
			vector3.x = Mathf.Round(vector3.x);
		}
		else
		{
			vector3.x = Mathf.Floor(vector3.x) + 0.5f;
		}
		if (num2 % 2 != 1 || base.pivot != UIWidget.Pivot.Left && base.pivot != UIWidget.Pivot.Center && base.pivot != UIWidget.Pivot.Right)
		{
			vector3.y = Mathf.Round(vector3.y);
		}
		else
		{
			vector3.y = Mathf.Ceil(vector3.y) - 0.5f;
		}
		base.cachedTransform.localPosition = vector3;
	}

	public override void OnFill(MeshBuffer m)
	{
		m.FastQuad(this.mOuterUV, base.color);
	}

	protected override void OnStart()
	{
		if (this.mAtlas != null)
		{
			this.UpdateUVs(true);
		}
	}

	public override bool OnUpdate()
	{
		if (this.mLastName == this.mSpriteName)
		{
			this.UpdateUVs(false);
			return false;
		}
		this.mSprite = null;
		base.ChangedAuto();
		this.mLastName = this.mSpriteName;
		this.UpdateUVs(false);
		return true;
	}

	public virtual void UpdateUVs(bool force)
	{
		if (this.sprite != null && (force || this.mOuter != this.mSprite.outer))
		{
			Texture texture = base.mainTexture;
			if (texture != null)
			{
				this.mOuter = this.mSprite.outer;
				this.mOuterUV = this.mOuter;
				if (this.mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
				{
					this.mOuterUV = NGUIMath.ConvertToTexCoords(this.mOuterUV, texture.width, texture.height);
				}
				base.ChangedAuto();
			}
		}
	}
}