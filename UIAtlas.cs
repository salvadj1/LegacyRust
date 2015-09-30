using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private Material material;

	[HideInInspector]
	[SerializeField]
	private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

	[HideInInspector]
	[SerializeField]
	private UIAtlas.Coordinates mCoordinates;

	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	[HideInInspector]
	[SerializeField]
	private UIAtlas mReplacement;

	public UIAtlas.Coordinates coordinates
	{
		get
		{
			return (this.mReplacement == null ? this.mCoordinates : this.mReplacement.coordinates);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.coordinates = value;
			}
			else if (this.mCoordinates != value)
			{
				if (this.material == null || this.material.mainTexture == null)
				{
					Debug.LogError("Can't switch coordinates until the atlas material has a valid texture");
					return;
				}
				this.mCoordinates = value;
				Texture texture = this.material.mainTexture;
				int num = 0;
				int count = this.sprites.Count;
				while (num < count)
				{
					UIAtlas.Sprite item = this.sprites[num];
					if (this.mCoordinates != UIAtlas.Coordinates.TexCoords)
					{
						item.outer = NGUIMath.ConvertToPixels(item.outer, texture.width, texture.height, true);
						item.inner = NGUIMath.ConvertToPixels(item.inner, texture.width, texture.height, true);
					}
					else
					{
						item.outer = NGUIMath.ConvertToTexCoords(item.outer, texture.width, texture.height);
						item.inner = NGUIMath.ConvertToTexCoords(item.inner, texture.width, texture.height);
					}
					num++;
				}
			}
		}
	}

	public float pixelSize
	{
		get
		{
			return (this.mReplacement == null ? this.mPixelSize : this.mReplacement.pixelSize);
		}
		set
		{
			if (this.mReplacement == null)
			{
				float single = Mathf.Clamp(value, 0.25f, 4f);
				if (this.mPixelSize != single)
				{
					this.mPixelSize = single;
					this.MarkAsDirty();
				}
			}
			else
			{
				this.mReplacement.pixelSize = value;
			}
		}
	}

	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uIAtla = value;
			if (uIAtla == this)
			{
				uIAtla = null;
			}
			if (this.mReplacement != uIAtla)
			{
				if (uIAtla != null && uIAtla.replacement == this)
				{
					uIAtla.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsDirty();
				}
				this.mReplacement = uIAtla;
				this.MarkAsDirty();
			}
		}
	}

	public List<UIAtlas.Sprite> spriteList
	{
		get
		{
			return (this.mReplacement == null ? this.sprites : this.mReplacement.spriteList);
		}
		set
		{
			if (this.mReplacement == null)
			{
				this.sprites = value;
			}
			else
			{
				this.mReplacement.spriteList = value;
			}
		}
	}

	public Material spriteMaterial
	{
		get
		{
			return (this.mReplacement == null ? this.material : this.mReplacement.spriteMaterial);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
			}
			else if (this.material != null)
			{
				this.MarkAsDirty();
				this.material = value;
				this.MarkAsDirty();
			}
			else
			{
				this.material = value;
			}
		}
	}

	public Texture texture
	{
		get
		{
			Texture texture;
			if (this.mReplacement != null)
			{
				texture = this.mReplacement.texture;
			}
			else if (this.material == null)
			{
				texture = null;
			}
			else
			{
				texture = this.material.mainTexture;
			}
			return texture;
		}
	}

	public UIAtlas()
	{
	}

	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		if (a == null || b == null)
		{
			return false;
		}
		return (a == b || a.References(b) ? true : b.References(a));
	}

	public List<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		List<string> strs = new List<string>();
		int num = 0;
		int count = this.sprites.Count;
		while (num < count)
		{
			UIAtlas.Sprite item = this.sprites[num];
			if (item != null && !string.IsNullOrEmpty(item.name))
			{
				strs.Add(item.name);
			}
			num++;
		}
		strs.Sort();
		return strs;
	}

	public UIAtlas.Sprite GetSprite(string name)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (string.IsNullOrEmpty(name))
		{
			Debug.LogWarning("Expected a valid name, found nothing");
		}
		else
		{
			int num = 0;
			int count = this.sprites.Count;
			while (num < count)
			{
				UIAtlas.Sprite item = this.sprites[num];
				if (!string.IsNullOrEmpty(item.name) && name == item.name)
				{
					return item;
				}
				num++;
			}
		}
		return null;
	}

	public void MarkAsDirty()
	{
		UISprite[] uISpriteArray = NGUITools.FindActive<UISprite>();
		int num = 0;
		int length = (int)uISpriteArray.Length;
		while (num < length)
		{
			UISprite uISprite = uISpriteArray[num];
			if (UIAtlas.CheckIfRelated(this, uISprite.atlas))
			{
				UIAtlas uIAtla = uISprite.atlas;
				uISprite.atlas = null;
				uISprite.atlas = uIAtla;
			}
			num++;
		}
		UIFont[] uIFontArray = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int num1 = 0;
		int length1 = (int)uIFontArray.Length;
		while (num1 < length1)
		{
			UIFont uIFont = uIFontArray[num1];
			if (UIAtlas.CheckIfRelated(this, uIFont.atlas))
			{
				UIAtlas uIAtla1 = uIFont.atlas;
				uIFont.atlas = null;
				uIFont.atlas = uIAtla1;
			}
			num1++;
		}
		UILabel[] uILabelArray = NGUITools.FindActive<UILabel>();
		int num2 = 0;
		int length2 = (int)uILabelArray.Length;
		while (num2 < length2)
		{
			UILabel uILabel = uILabelArray[num2];
			if (uILabel.font != null && UIAtlas.CheckIfRelated(this, uILabel.font.atlas))
			{
				UIFont uIFont1 = uILabel.font;
				uILabel.font = null;
				uILabel.font = uIFont1;
			}
			num2++;
		}
	}

	private bool References(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (atlas == this)
		{
			return true;
		}
		return (this.mReplacement == null ? false : this.mReplacement.References(atlas));
	}

	public enum Coordinates
	{
		Pixels,
		TexCoords
	}

	[Serializable]
	public class Sprite
	{
		public string name;

		public Rect outer;

		public Rect inner;

		public float paddingLeft;

		public float paddingRight;

		public float paddingTop;

		public float paddingBottom;

		public bool hasPadding
		{
			get
			{
				return (this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f ? true : this.paddingBottom != 0f);
			}
		}

		public Sprite()
		{
		}
	}
}