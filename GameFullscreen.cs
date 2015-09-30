using System;
using UnityEngine;

public sealed class GameFullscreen : PostEffectsBase
{
	private const ScaleMode kDefaultScaleMode = ScaleMode.StretchToFill;

	private const int kDefaultOverlayPass = 1;

	private const float sqrtOf3 = 1.73205078f;

	public Color tintColor = Color.white;

	public Color fadeColor = new Color(0f, 0f, 0f, 1f);

	public readonly GameFullscreen.Overlay[] overlays;

	public Shader shader;

	private Material material;

	public Color autoFadeColor
	{
		get
		{
			return this.fadeColor;
		}
		set
		{
			float single;
			this.fadeColor.r = value.r;
			this.fadeColor.g = value.g;
			this.fadeColor.b = value.b;
			if (value.r != value.g || value.r != value.b)
			{
				float single1 = Mathf.Atan2(1.73205078f * (value.g - value.b), 2f * value.r - value.g - value.b) * 57.29578f;
				if (float.IsNaN(single1) || float.IsInfinity(single1))
				{
					float single2 = 1f;
					single = single2;
					this.tintColor.b = single2;
					float single3 = single;
					single = single3;
					this.tintColor.g = single3;
					this.tintColor.r = single;
				}
				else
				{
					float single4 = (single1 >= 0f ? single1 : single1 + 360f) / 60f;
					float single5 = 1f * (1f - Mathf.Abs(single4 % 2f - 1f));
					switch (Mathf.FloorToInt(single4) % 6)
					{
						case 0:
						{
							this.tintColor.r = 1f;
							this.tintColor.g = single5;
							this.tintColor.b = 0f;
							break;
						}
						case 1:
						{
							this.tintColor.r = single5;
							this.tintColor.g = 1f;
							this.tintColor.b = 0f;
							break;
						}
						case 2:
						{
							this.tintColor.r = 0f;
							this.tintColor.g = 1f;
							this.tintColor.b = single5;
							break;
						}
						case 3:
						{
							this.tintColor.r = 0f;
							this.tintColor.g = single5;
							this.tintColor.b = 1f;
							break;
						}
						case 4:
						{
							this.tintColor.r = single5;
							this.tintColor.g = 0f;
							this.tintColor.b = 1f;
							break;
						}
						case 5:
						{
							this.tintColor.r = 1f;
							this.tintColor.g = 0f;
							this.tintColor.b = single5;
							break;
						}
						default:
						{
							goto case 0;
						}
					}
				}
			}
			else
			{
				float single6 = 1f;
				single = single6;
				this.tintColor.b = single6;
				float single7 = single;
				single = single7;
				this.tintColor.g = single7;
				this.tintColor.r = single;
			}
			this.tintColor.a = Mathf.Clamp01(Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0f, 0.5f, value.a)));
			this.fadeColor.a = Mathf.Clamp01(Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0f, 1f, value.a)));
		}
	}

	private bool run
	{
		get
		{
			if (this.fadeColor.a > 0f || this.tintColor.a > 0f)
			{
				return true;
			}
			for (int i = 0; i < (int)this.overlays.Length; i++)
			{
				if (this.overlays[i].willRender)
				{
					return true;
				}
			}
			return false;
		}
	}

	public GameFullscreen()
	{
		this.tintColor = Color.white;
		this.fadeColor = new Color(0f, 0f, 0f, 1f);
		GameFullscreen.Overlay[] overlayArray = new GameFullscreen.Overlay[4];
		GameFullscreen.Overlay overlay = new GameFullscreen.Overlay()
		{
			pass = 1
		};
		overlayArray[0] = overlay;
		GameFullscreen.Overlay overlay1 = new GameFullscreen.Overlay()
		{
			pass = 1
		};
		overlayArray[1] = overlay1;
		GameFullscreen.Overlay overlay2 = new GameFullscreen.Overlay()
		{
			pass = 1
		};
		overlayArray[2] = overlay2;
		GameFullscreen.Overlay overlay3 = new GameFullscreen.Overlay()
		{
			pass = 1
		};
		overlayArray[3] = overlay3;
		this.overlays = overlayArray;
		base();
	}

	public override bool CheckResources()
	{
		this.CheckSupport(false);
		this.material = this.CheckShaderAndCreateMaterial(this.shader, this.material);
		if (!this.isSupported)
		{
			this.ReportAutoDisable();
		}
		return this.isSupported;
	}

	protected void OnDisable()
	{
		if (this.material)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
		}
	}

	protected void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		int j;
		int num;
		if (!this.CheckResources() || !this.run)
		{
			Graphics.Blit(src, dst);
			return;
		}
		if (this.tintColor.a > 0f || this.fadeColor.a > 0f)
		{
			this.material.SetColor("_FadeColor", this.tintColor);
			this.material.SetColor("_SolidColor", this.fadeColor);
			for (int i = 0; i < (int)this.overlays.Length; i++)
			{
				if (this.overlays[i].willRender)
				{
					this.material.SetFloat("_Blend", this.overlays[i].alpha);
					this.material.SetTexture("_OverlayTex", this.overlays[i].texture);
					RenderTexture temporary = RenderTexture.GetTemporary(src.width, src.height, 0);
					RenderTexture renderTexture = RenderTexture.GetTemporary(src.width, src.height, 0);
					try
					{
						Graphics.Blit(src, temporary, this.material, this.overlays[i].pass);
						while (true)
						{
							int num1 = i + 1;
							i = num1;
							if (num1 >= (int)this.overlays.Length)
							{
								break;
							}
							if (this.overlays[i].willRender)
							{
								this.material.SetFloat("_Blend", this.overlays[i].alpha);
								this.material.SetTexture("_OverlayTex", this.overlays[i].texture);
								Graphics.Blit(temporary, renderTexture, this.material, this.overlays[i].pass);
								RenderTexture renderTexture1 = temporary;
								temporary = renderTexture;
								renderTexture = renderTexture1;
							}
						}
						Graphics.Blit(temporary, dst, this.material, 0);
					}
					finally
					{
						RenderTexture.ReleaseTemporary(temporary);
						RenderTexture.ReleaseTemporary(renderTexture);
					}
					return;
				}
			}
			Graphics.Blit(src, dst, this.material, 0);
			return;
		}
		for (j = 0; j < (int)this.overlays.Length; j++)
		{
			if (this.overlays[j].willRender)
			{
				this.material.SetFloat("_Blend", this.overlays[j].alpha);
				this.material.SetTexture("_OverlayTex", this.overlays[j].texture);
				num = this.overlays[j].pass;
				while (true)
				{
					int num2 = j + 1;
					j = num2;
					if (num2 >= (int)this.overlays.Length)
					{
						break;
					}
					if (this.overlays[j].willRender)
					{
						goto Label0;
					}
				}
				Graphics.Blit(src, dst, this.material, num);
				return;
			}
		}
		return;
	Label0:
		RenderTexture temporary1 = RenderTexture.GetTemporary(src.width, src.height, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(src.width, src.height, 0);
		try
		{
			Graphics.Blit(src, temporary1, this.material, num);
			this.material.SetFloat("_Blend", this.overlays[j].alpha);
			this.material.SetTexture("_OverlayTex", this.overlays[j].texture);
			num = this.overlays[j].pass;
			while (true)
			{
				int num3 = j + 1;
				j = num3;
				if (num3 >= (int)this.overlays.Length)
				{
					break;
				}
				if (this.overlays[j].willRender)
				{
					Graphics.Blit(temporary1, temporary2, this.material, num);
					RenderTexture renderTexture2 = temporary1;
					temporary1 = temporary2;
					temporary2 = renderTexture2;
					this.material.SetFloat("_Blend", this.overlays[j].alpha);
					this.material.SetTexture("_OverlayTex", this.overlays[j].texture);
					num = this.overlays[j].pass;
				}
			}
			Graphics.Blit(temporary1, dst, this.material, num);
		}
		finally
		{
			RenderTexture.ReleaseTemporary(temporary1);
			RenderTexture.ReleaseTemporary(temporary2);
		}
	}

	public struct Overlay
	{
		public ScaleMode scaleMode;

		public int pass;

		private Texture2D _texture;

		private float _alpha;

		private bool hasTex;

		private bool hasAlpha;

		private bool shouldDraw;

		public float alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				this._alpha = value;
				bool flag = this.hasAlpha;
				this.hasAlpha = value > 0f;
				if (flag != this.hasAlpha)
				{
					this.shouldDraw = (!this.hasAlpha ? false : this.hasTex);
				}
			}
		}

		public Texture2D texture
		{
			get
			{
				return this._texture;
			}
			set
			{
				this._texture = value;
				bool flag = this.hasTex;
				this.hasTex = this._texture;
				if (flag != this.hasTex)
				{
					this.shouldDraw = (!this.hasTex ? false : this.hasAlpha);
				}
			}
		}

		public bool willRender
		{
			get
			{
				if (this.shouldDraw && !this._texture)
				{
					this.hasTex = false;
					this._texture = null;
					this.shouldDraw = false;
				}
				return this.shouldDraw;
			}
		}
	}
}