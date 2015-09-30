using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite Animation")]
[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
public class UISpriteAnimation : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private int mFPS = 30;

	[HideInInspector]
	[SerializeField]
	private string mPrefix = string.Empty;

	private UISprite mSprite;

	private float mDelta;

	private int mIndex;

	private List<string> mSpriteNames = new List<string>();

	public int framesPerSecond
	{
		get
		{
			return this.mFPS;
		}
		set
		{
			this.mFPS = value;
		}
	}

	public string namePrefix
	{
		get
		{
			return this.mPrefix;
		}
		set
		{
			if (this.mPrefix != value)
			{
				this.mPrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	public UISpriteAnimation()
	{
	}

	private void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UIAtlas.Sprite> sprites = this.mSprite.atlas.spriteList;
			int num = 0;
			int count = sprites.Count;
			while (num < count)
			{
				UIAtlas.Sprite item = sprites[num];
				if (string.IsNullOrEmpty(this.mPrefix) || item.name.StartsWith(this.mPrefix))
				{
					this.mSpriteNames.Add(item.name);
				}
				num++;
			}
			this.mSpriteNames.Sort();
		}
	}

	private void Start()
	{
		this.RebuildSpriteList();
	}

	private void Update()
	{
		if (this.mSpriteNames.Count > 1 && Application.isPlaying)
		{
			UISpriteAnimation uISpriteAnimation = this;
			uISpriteAnimation.mDelta = uISpriteAnimation.mDelta + Time.deltaTime;
			float single = ((float)this.mFPS <= 0f ? 0f : 1f / (float)this.mFPS);
			if (single < this.mDelta)
			{
				this.mDelta = (single <= 0f ? 0f : this.mDelta - single);
				UISpriteAnimation uISpriteAnimation1 = this;
				int num = uISpriteAnimation1.mIndex + 1;
				int num1 = num;
				uISpriteAnimation1.mIndex = num;
				if (num1 >= this.mSpriteNames.Count)
				{
					this.mIndex = 0;
				}
				this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
				this.mSprite.MakePixelPerfect();
			}
		}
	}
}