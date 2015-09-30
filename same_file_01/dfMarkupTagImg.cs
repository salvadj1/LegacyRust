using System;
using UnityEngine;

[dfMarkupTagInfo("img")]
public class dfMarkupTagImg : dfMarkupTag
{
	public dfMarkupTagImg() : base("img")
	{
		this.IsClosedTag = true;
	}

	public dfMarkupTagImg(dfMarkupTag original) : base(original)
	{
		this.IsClosedTag = true;
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (base.Owner == null)
		{
			Debug.LogError(string.Concat("Tag has no parent: ", this));
			return;
		}
		style = this.applyStyleAttributes(style);
		dfMarkupAttribute _dfMarkupAttribute = base.findAttribute(new string[] { "src" });
		if (_dfMarkupAttribute == null)
		{
			return;
		}
		string value = _dfMarkupAttribute.Value;
		dfMarkupBox _dfMarkupBox = this.createImageBox(base.Owner.Atlas, value, style);
		if (_dfMarkupBox == null)
		{
			return;
		}
		Vector2 size = Vector2.zero;
		dfMarkupAttribute _dfMarkupAttribute1 = base.findAttribute(new string[] { "height" });
		if (_dfMarkupAttribute1 != null)
		{
			size.y = (float)dfMarkupStyle.ParseSize(_dfMarkupAttribute1.Value, (int)_dfMarkupBox.Size.y);
		}
		dfMarkupAttribute _dfMarkupAttribute2 = base.findAttribute(new string[] { "width" });
		if (_dfMarkupAttribute2 != null)
		{
			size.x = (float)dfMarkupStyle.ParseSize(_dfMarkupAttribute2.Value, (int)_dfMarkupBox.Size.x);
		}
		if (size.sqrMagnitude <= 1.401298E-45f)
		{
			size = _dfMarkupBox.Size;
		}
		else if (size.x <= 1.401298E-45f)
		{
			size.x = size.y * (_dfMarkupBox.Size.x / _dfMarkupBox.Size.y);
		}
		else if (size.y <= 1.401298E-45f)
		{
			size.y = size.x * (_dfMarkupBox.Size.y / _dfMarkupBox.Size.x);
		}
		_dfMarkupBox.Size = size;
		_dfMarkupBox.Baseline = (int)size.y;
		container.AddChild(_dfMarkupBox);
	}

	private dfMarkupStyle applyStyleAttributes(dfMarkupStyle style)
	{
		dfMarkupAttribute _dfMarkupAttribute = base.findAttribute(new string[] { "valign" });
		if (_dfMarkupAttribute != null)
		{
			style.VerticalAlign = dfMarkupStyle.ParseVerticalAlignment(_dfMarkupAttribute.Value);
		}
		dfMarkupAttribute _dfMarkupAttribute1 = base.findAttribute(new string[] { "color" });
		if (_dfMarkupAttribute1 != null)
		{
			Color opacity = dfMarkupStyle.ParseColor(_dfMarkupAttribute1.Value, base.Owner.Color);
			opacity.a = style.Opacity;
			style.Color = opacity;
		}
		return style;
	}

	private dfMarkupBox createImageBox(dfAtlas atlas, string source, dfMarkupStyle style)
	{
		if (source.ToLowerInvariant().StartsWith("http://"))
		{
			return null;
		}
		if (atlas != null && atlas[source] != null)
		{
			dfMarkupBoxSprite _dfMarkupBoxSprite = new dfMarkupBoxSprite(this, dfMarkupDisplayType.inline, style);
			_dfMarkupBoxSprite.LoadImage(atlas, source);
			return _dfMarkupBoxSprite;
		}
		Texture texture = dfMarkupImageCache.Load(source);
		if (texture == null)
		{
			return null;
		}
		dfMarkupBoxTexture _dfMarkupBoxTexture = new dfMarkupBoxTexture(this, dfMarkupDisplayType.inline, style);
		_dfMarkupBoxTexture.LoadTexture(texture);
		return _dfMarkupBoxTexture;
	}
}