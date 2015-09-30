using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Sprite/Web")]
[ExecuteInEditMode]
[Serializable]
public class dfWebSprite : dfTextureSprite
{
	[SerializeField]
	protected string url = string.Empty;

	[SerializeField]
	protected Texture2D loadingImage;

	[SerializeField]
	protected Texture2D errorImage;

	public Texture2D ErrorImage
	{
		get
		{
			return this.errorImage;
		}
		set
		{
			this.errorImage = value;
		}
	}

	public Texture2D LoadingImage
	{
		get
		{
			return this.loadingImage;
		}
		set
		{
			this.loadingImage = value;
		}
	}

	public string URL
	{
		get
		{
			return this.url;
		}
		set
		{
			if (value != this.url)
			{
				this.url = value;
				if (Application.isPlaying)
				{
					base.StopAllCoroutines();
					base.StartCoroutine(this.downloadTexture());
				}
			}
		}
	}

	public dfWebSprite()
	{
	}

	[DebuggerHidden]
	private IEnumerator downloadTexture()
	{
		dfWebSprite.<downloadTexture>c__Iterator43 variable = null;
		return variable;
	}

	public override void Start()
	{
		base.Start();
		if (base.Texture == null)
		{
			base.Texture = this.LoadingImage;
		}
		if (Application.isPlaying)
		{
			base.StartCoroutine(this.downloadTexture());
		}
	}
}