using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Sprite/Texture")]
[ExecuteInEditMode]
[Serializable]
public class dfTextureSprite : dfControl
{
	private static int[] TRIANGLE_INDICES;

	[SerializeField]
	protected Texture2D texture;

	[SerializeField]
	protected UnityEngine.Material material;

	[SerializeField]
	protected dfSpriteFlip flip;

	[SerializeField]
	protected dfFillDirection fillDirection;

	[SerializeField]
	protected float fillAmount = 1f;

	[SerializeField]
	protected bool invertFill;

	private bool createdRuntimeMaterial;

	private UnityEngine.Material renderMaterial;

	private PropertyChangedEventHandler<Texture2D> TextureChanged;

	public float FillAmount
	{
		get
		{
			return this.fillAmount;
		}
		set
		{
			if (!Mathf.Approximately(value, this.fillAmount))
			{
				this.fillAmount = Mathf.Max(0f, Mathf.Min(1f, value));
				this.Invalidate();
			}
		}
	}

	public dfFillDirection FillDirection
	{
		get
		{
			return this.fillDirection;
		}
		set
		{
			if (value != this.fillDirection)
			{
				this.fillDirection = value;
				this.Invalidate();
			}
		}
	}

	public dfSpriteFlip Flip
	{
		get
		{
			return this.flip;
		}
		set
		{
			if (value != this.flip)
			{
				this.flip = value;
				this.Invalidate();
			}
		}
	}

	public bool InvertFill
	{
		get
		{
			return this.invertFill;
		}
		set
		{
			if (value != this.invertFill)
			{
				this.invertFill = value;
				this.Invalidate();
			}
		}
	}

	public UnityEngine.Material Material
	{
		get
		{
			return this.material;
		}
		set
		{
			if (value != this.material)
			{
				this.disposeCreatedMaterial();
				this.renderMaterial = null;
				this.material = value;
				this.Invalidate();
			}
		}
	}

	public UnityEngine.Material RenderMaterial
	{
		get
		{
			return this.renderMaterial;
		}
	}

	public Texture2D Texture
	{
		get
		{
			return this.texture;
		}
		set
		{
			if (value != this.texture)
			{
				this.texture = value;
				this.Invalidate();
				if (value != null && this.size.sqrMagnitude <= 1.401298E-45f)
				{
					this.size = new Vector2((float)value.width, (float)value.height);
				}
				this.OnTextureChanged(value);
			}
		}
	}

	static dfTextureSprite()
	{
		dfTextureSprite.TRIANGLE_INDICES = new int[] { 0, 1, 3, 3, 1, 2 };
	}

	public dfTextureSprite()
	{
	}

	private void disposeCreatedMaterial()
	{
		if (this.createdRuntimeMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
			this.material = null;
			this.createdRuntimeMaterial = false;
		}
	}

	private void doFill(dfRenderData renderData)
	{
		dfList<Vector3> vertices = renderData.Vertices;
		dfList<Vector2> uV = renderData.UV;
		int num = 0;
		int num1 = 1;
		int num2 = 3;
		int num3 = 2;
		if (this.invertFill)
		{
			if (this.fillDirection != dfFillDirection.Horizontal)
			{
				num = 3;
				num1 = 2;
				num2 = 0;
				num3 = 1;
			}
			else
			{
				num = 1;
				num1 = 0;
				num2 = 2;
				num3 = 3;
			}
		}
		if (this.fillDirection != dfFillDirection.Horizontal)
		{
			vertices[num2] = Vector3.Lerp(vertices[num2], vertices[num], 1f - this.fillAmount);
			vertices[num3] = Vector3.Lerp(vertices[num3], vertices[num1], 1f - this.fillAmount);
			uV[num2] = Vector2.Lerp(uV[num2], uV[num], 1f - this.fillAmount);
			uV[num3] = Vector2.Lerp(uV[num3], uV[num1], 1f - this.fillAmount);
		}
		else
		{
			vertices[num1] = Vector3.Lerp(vertices[num1], vertices[num], 1f - this.fillAmount);
			vertices[num3] = Vector3.Lerp(vertices[num3], vertices[num2], 1f - this.fillAmount);
			uV[num1] = Vector2.Lerp(uV[num1], uV[num], 1f - this.fillAmount);
			uV[num3] = Vector2.Lerp(uV[num3], uV[num2], 1f - this.fillAmount);
		}
	}

	private void ensureMaterial()
	{
		if (this.material != null || this.texture == null)
		{
			return;
		}
		Shader shader = Shader.Find("Daikon Forge/Default UI Shader");
		if (shader == null)
		{
			Debug.LogError("Failed to find default shader");
			return;
		}
		UnityEngine.Material material = new UnityEngine.Material(shader)
		{
			name = "Default Texture Shader",
			hideFlags = HideFlags.DontSave,
			mainTexture = this.texture
		};
		this.material = material;
		this.createdRuntimeMaterial = true;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.renderMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(this.renderMaterial);
			this.renderMaterial = null;
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.disposeCreatedMaterial();
		if (Application.isPlaying && this.renderMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(this.renderMaterial);
			this.renderMaterial = null;
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		this.renderMaterial = null;
	}

	protected override void OnRebuildRenderData()
	{
		base.OnRebuildRenderData();
		if (this.texture == null)
		{
			return;
		}
		this.ensureMaterial();
		if (this.material == null)
		{
			return;
		}
		if (this.renderMaterial == null)
		{
			UnityEngine.Material material = new UnityEngine.Material(this.material)
			{
				hideFlags = HideFlags.DontSave,
				name = string.Concat(this.material.name, " (copy)")
			};
			this.renderMaterial = material;
		}
		this.renderMaterial.mainTexture = this.texture;
		this.renderData.Material = this.renderMaterial;
		float units = base.PixelsToUnits();
		float single = 0f;
		float single1 = 0f;
		float single2 = this.size.x * units;
		float single3 = -this.size.y * units;
		Vector3 num = this.pivot.TransformToUpperLeft(this.size).RoundToInt() * units;
		this.renderData.Vertices.Add(new Vector3(single, single1, 0f) + num);
		this.renderData.Vertices.Add(new Vector3(single2, single1, 0f) + num);
		this.renderData.Vertices.Add(new Vector3(single2, single3, 0f) + num);
		this.renderData.Vertices.Add(new Vector3(single, single3, 0f) + num);
		this.renderData.Triangles.AddRange(dfTextureSprite.TRIANGLE_INDICES);
		this.rebuildUV(this.renderData);
		Color32 color32 = base.ApplyOpacity(this.color);
		this.renderData.Colors.Add(color32);
		this.renderData.Colors.Add(color32);
		this.renderData.Colors.Add(color32);
		this.renderData.Colors.Add(color32);
		if (this.fillAmount < 1f)
		{
			this.doFill(this.renderData);
		}
	}

	protected internal virtual void OnTextureChanged(Texture2D value)
	{
		base.SignalHierarchy("OnTextureChanged", new object[] { value });
		if (this.TextureChanged != null)
		{
			this.TextureChanged(this, value);
		}
	}

	private void rebuildUV(dfRenderData renderData)
	{
		dfList<Vector2> uV = renderData.UV;
		uV.Add(new Vector2(0f, 1f));
		uV.Add(new Vector2(1f, 1f));
		uV.Add(new Vector2(1f, 0f));
		uV.Add(new Vector2(0f, 0f));
		Vector2 item = Vector2.zero;
		if (this.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			item = uV[1];
			uV[1] = uV[0];
			uV[0] = item;
			item = uV[3];
			uV[3] = uV[2];
			uV[2] = item;
		}
		if (this.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			item = uV[0];
			uV[0] = uV[3];
			uV[3] = item;
			item = uV[1];
			uV[1] = uV[2];
			uV[2] = item;
		}
	}

	public event PropertyChangedEventHandler<Texture2D> TextureChanged
	{
		add
		{
			this.TextureChanged += value;
		}
		remove
		{
			this.TextureChanged -= value;
		}
	}
}