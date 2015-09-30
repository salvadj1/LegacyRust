using System;
using UnityEngine;

[ExecuteInEditMode]
public class FPGrassProbabilities : ScriptableObject, IFPGrassAsset
{
	private const int kBillnearPixelSize = 4;

	private const int kWidth = 16;

	private const int kHeight = 2;

	private const TextureFormat kDetailProbabilityFormat = TextureFormat.Alpha8;

	private const sbyte kTOpt_Default = 0;

	private const sbyte kTOpt_NoSetPixelsOnCreate = 1;

	private const sbyte kTOpt_NoApplyPixelsOnCreate = 3;

	private const sbyte kTOpt_ReCreate = 4;

	[Obsolete]
	[SerializeField]
	private Texture2D probabilityTexture;

	[HideInInspector]
	[SerializeField]
	private Color[] pixels;

	[NonSerialized]
	private Texture2D texture;

	[NonSerialized]
	private bool linear;

	[NonSerialized]
	private int applyLock;

	[NonSerialized]
	private bool updateQueued;

	[NonSerialized]
	private bool applyQueued;

	[NonSerialized]
	private bool enabled;

	public FPGrassProbabilities()
	{
	}

	public int GetDetailID(int splatChannel, int detailIndex)
	{
		return (int)(this.GetPixels()[4 * splatChannel + detailIndex].a * 256f);
	}

	public float GetDetailProbability(int splatChannel, int detailIndex)
	{
		return this.GetPixels()[4 * splatChannel + detailIndex + 16].a;
	}

	private Color[] GetPixels()
	{
		if (this.probabilityTexture)
		{
			Debug.LogWarning("ProbabilityTexture is now created at runtime. Saved the pixels off the texture and now dereferencing it", this.probabilityTexture);
			this.pixels = this.probabilityTexture.GetPixels(0, 0, 16, 2, 0);
			this.probabilityTexture = null;
		}
		else if (object.ReferenceEquals(this.pixels, null) || (int)this.pixels.Length != 32)
		{
			this.pixels = new Color[32];
			try
			{
				this.StartEditing();
				for (int i = 0; i < 4; i++)
				{
					this.SetDetailProperty(i, 0, 0, 1f);
				}
			}
			finally
			{
				this.StopEditing();
			}
		}
		return this.pixels;
	}

	private Texture2D GetTexture(sbyte TOpt)
	{
		Texture2D texture2D;
		if (this.texture)
		{
			if (((int)TOpt & 4) == 0)
			{
				return this.texture;
			}
			UnityEngine.Object.DestroyImmediate(this.texture, false);
			this.texture = null;
		}
		if (FPGrass.Support.DetailProbabilityFilterMode != FilterMode.Point)
		{
			texture2D = new Texture2D(64, 8, TextureFormat.Alpha8, false, false)
			{
				hideFlags = HideFlags.DontSave,
				name = "FPGrass Detail Probability (Linear)",
				anisoLevel = 0,
				filterMode = FPGrass.Support.DetailProbabilityFilterMode,
				wrapMode = TextureWrapMode.Clamp
			};
			this.texture = texture2D;
			this.linear = true;
		}
		else
		{
			texture2D = new Texture2D(16, 2, TextureFormat.Alpha8, false, false)
			{
				hideFlags = HideFlags.DontSave,
				name = "FPGrass Detail Probability (Point)",
				anisoLevel = 0,
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Clamp
			};
			this.texture = texture2D;
			this.linear = false;
		}
		if (((int)TOpt & 1) == 0)
		{
			this.UpdatePixels(((int)TOpt & 3) == 0);
		}
		return this.texture;
	}

	public Texture2D GetTexture()
	{
		return this.GetTexture(0);
	}

	public void Initialize()
	{
		if (this.probabilityTexture)
		{
			this.pixels = null;
			this.GetPixels();
		}
	}

	private void OnDisable()
	{
		if (this.enabled)
		{
			this.enabled = false;
			if (this.texture)
			{
				UnityEngine.Object.DestroyImmediate(this.texture, false);
				this.texture = null;
			}
		}
	}

	private void OnEnable()
	{
		if (!this.enabled)
		{
			this.enabled = true;
			this.Initialize();
		}
	}

	public void SetDetailProperty(int splatChannel, int detailIndex, int detailID, float probability)
	{
		object obj;
		float single;
		Color[] pixels = this.GetPixels();
		int num = 4 * splatChannel + detailIndex;
		int num1 = num + 16;
		if (detailID >= 0)
		{
			obj = (detailID <= 256 ? detailID : 256);
		}
		else
		{
			obj = null;
		}
		float single1 = (float)obj / 256f;
		if (probability >= 0f)
		{
			single = (probability <= 1f ? probability : 1f);
		}
		else
		{
			single = 0f;
		}
		float single2 = single;
		bool flag = false;
		if (FPGrassProbabilities.SetDif(ref pixels[num].a, single1))
		{
			flag = true;
		}
		if (FPGrassProbabilities.SetDif(ref pixels[num1].a, single2))
		{
			flag = true;
		}
		if (flag)
		{
			this.UpdatePixels(true);
		}
	}

	private static bool SetDif(ref float current, float value)
	{
		if (current == value)
		{
			return false;
		}
		current = value;
		return true;
	}

	public void StartEditing()
	{
		FPGrassProbabilities fPGrassProbability = this;
		fPGrassProbability.applyLock = fPGrassProbability.applyLock + 1;
	}

	public void StopEditing()
	{
		FPGrassProbabilities fPGrassProbability = this;
		int num = fPGrassProbability.applyLock - 1;
		int num1 = num;
		fPGrassProbability.applyLock = num;
		if (num1 <= 0)
		{
			this.applyLock = 0;
			if (this.updateQueued)
			{
				this.updateQueued = false;
				bool flag = this.applyQueued;
				this.applyQueued = false;
				this.UpdatePixels(flag);
			}
		}
	}

	private void UpdatePixels(bool apply = false)
	{
		if (this.applyLock != 0)
		{
			this.updateQueued = true;
			FPGrassProbabilities fPGrassProbability = this;
			fPGrassProbability.applyQueued = fPGrassProbability.applyQueued | apply;
		}
		else
		{
			Color[] pixels = this.GetPixels();
			Texture2D texture = this.GetTexture(1);
			if (!this.linear)
			{
				texture.SetPixels(0, 0, 16, 2, pixels);
			}
			else
			{
				FPGrassProbabilities.Linear.SetLinearPixels(pixels, texture);
			}
			if (apply)
			{
				texture.Apply();
			}
		}
	}

	private static class Linear
	{
		private const int kB = 4;

		private const int kW = 16;

		private const int kH = 2;

		private const int kP_Stride = 16;

		private const int kB_Stride = 64;

		private readonly static Color[] B;

		static Linear()
		{
			FPGrassProbabilities.Linear.B = new Color[512];
		}

		public static void SetLinearPixels(Color[] P, Texture2D Dest)
		{
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					int num = i * 4;
					int num1 = 0;
					while (num1 < 4)
					{
						int num2 = j * 4;
						int num3 = 0;
						while (num3 < 4)
						{
							int num4 = j * 16 + i;
							int num5 = num2 * 64 + num;
							FPGrassProbabilities.Linear.B[num5].r = P[num4].r;
							FPGrassProbabilities.Linear.B[num5].g = P[num4].g;
							FPGrassProbabilities.Linear.B[num5].b = P[num4].b;
							FPGrassProbabilities.Linear.B[num5].a = P[num4].a;
							num3++;
							num2++;
						}
						num1++;
						num++;
					}
				}
			}
			Dest.SetPixels(0, 0, 64, 8, FPGrassProbabilities.Linear.B, 0);
		}
	}
}