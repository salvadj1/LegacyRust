using NGUI.Meshing;
using NGUI.Structures;
using System;
using UnityEngine;

public class UIGeometricSprite : UISprite
{
	[HideInInspector]
	[SerializeField]
	private bool mFillCenter = true;

	protected Rect mInner;

	protected Rect mInnerUV;

	protected Vector3 mScale = Vector3.one;

	public bool fillCenter
	{
		get
		{
			return this.mFillCenter;
		}
		set
		{
			if (this.mFillCenter != value)
			{
				this.mFillCenter = value;
				this.MarkAsChanged();
			}
		}
	}

	public Rect innerUV
	{
		get
		{
			this.UpdateUVs(false);
			return this.mInnerUV;
		}
	}

	protected UIGeometricSprite(UIWidget.WidgetFlags additionalFlags) : base(additionalFlags)
	{
	}

	public override void MakePixelPerfect()
	{
		Vector3 num = base.cachedTransform.localPosition;
		num.x = (float)Mathf.RoundToInt(num.x);
		num.y = (float)Mathf.RoundToInt(num.y);
		num.z = (float)Mathf.RoundToInt(num.z);
		base.cachedTransform.localPosition = num;
		Vector3 vector3 = base.cachedTransform.localScale;
		vector3.x = (float)(Mathf.RoundToInt(vector3.x * 0.5f) << 1);
		vector3.y = (float)(Mathf.RoundToInt(vector3.y * 0.5f) << 1);
		vector3.z = 1f;
		base.cachedTransform.localScale = vector3;
	}

	public override void OnFill(MeshBuffer m)
	{
		NineRectangle nineRectangle;
		NineRectangle nineRectangle1;
		Vector4 vector4 = new Vector4();
		Vector4 vector41 = new Vector4();
		if (this.mOuterUV == this.mInnerUV)
		{
			base.OnFill(m);
			return;
		}
		float3 _float3 = new float3()
		{
			xyz = base.cachedTransform.localScale
		};
		vector4.x = this.mOuterUV.xMin;
		vector4.y = this.mInnerUV.xMin;
		vector4.z = this.mInnerUV.xMax;
		vector4.w = this.mOuterUV.xMax;
		vector41.x = this.mOuterUV.yMin;
		vector41.y = this.mInnerUV.yMin;
		vector41.z = this.mInnerUV.yMax;
		vector41.w = this.mOuterUV.yMax;
		NineRectangle.Calculate(base.pivot, base.atlas.pixelSize, base.mainTexture, ref vector4, ref vector41, ref _float3.xy, out nineRectangle, out nineRectangle1);
		Color color = base.color;
		if (!this.mFillCenter)
		{
			NineRectangle.Fill8(ref nineRectangle, ref nineRectangle1, ref color, m);
		}
		else
		{
			NineRectangle.Fill9(ref nineRectangle, ref nineRectangle1, ref color, m);
		}
	}

	public override void UpdateUVs(bool force)
	{
		if (base.cachedTransform.localScale != this.mScale)
		{
			this.mScale = base.cachedTransform.localScale;
			base.ChangedAuto();
		}
		if (base.sprite != null && (force || this.mInner != this.mSprite.inner || this.mOuter != this.mSprite.outer))
		{
			Texture texture = base.mainTexture;
			if (texture != null)
			{
				this.mInner = this.mSprite.inner;
				this.mOuter = this.mSprite.outer;
				this.mInnerUV = this.mInner;
				this.mOuterUV = this.mOuter;
				if (base.atlas.coordinates == UIAtlas.Coordinates.Pixels)
				{
					this.mOuterUV = NGUIMath.ConvertToTexCoords(this.mOuterUV, texture.width, texture.height);
					this.mInnerUV = NGUIMath.ConvertToTexCoords(this.mInnerUV, texture.width, texture.height);
				}
			}
		}
	}
}