using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Sliced)")]
[ExecuteInEditMode]
public class UISlicedSprite : UIGeometricSprite
{
	public new Vector4 border
	{
		get
		{
			UIAtlas.Sprite sprite = base.sprite;
			if (sprite == null)
			{
				return Vector4.zero;
			}
			Rect pixels = sprite.outer;
			Rect rect = sprite.inner;
			Texture texture = base.mainTexture;
			if (base.atlas.coordinates == UIAtlas.Coordinates.TexCoords && texture != null)
			{
				pixels = NGUIMath.ConvertToPixels(pixels, texture.width, texture.height, true);
				rect = NGUIMath.ConvertToPixels(rect, texture.width, texture.height, true);
			}
			return new Vector4(rect.xMin - pixels.xMin, rect.yMin - pixels.yMin, pixels.xMax - rect.xMax, pixels.yMax - rect.yMax) * base.atlas.pixelSize;
		}
	}

	protected override Vector4 customBorder
	{
		get
		{
			return this.border;
		}
	}

	public UISlicedSprite() : this(UIWidget.WidgetFlags.CustomBorder)
	{
	}

	protected UISlicedSprite(UIWidget.WidgetFlags additionalFlags) : base(additionalFlags)
	{
	}
}