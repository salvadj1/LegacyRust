using System;
using UnityEngine;

public struct HealthDimmer
{
	[NonSerialized]
	private float averageRGB;

	[NonSerialized]
	private float? percent;

	[NonSerialized]
	private bool initialized;

	[NonSerialized]
	private bool valid;

	[NonSerialized]
	private bool structureStyle;

	[NonSerialized]
	private MeshRenderer[] renderers;

	[NonSerialized]
	private MaterialPropertyBlock propBlock;

	[NonSerialized]
	private TakeDamage takeDamage;

	[NonSerialized]
	public bool disabled;

	private static bool GetFirstMaterial<TRenderer>(TRenderer[] renderers, out Material material)
	where TRenderer : Renderer
	{
		if (renderers != null)
		{
			int length = (int)renderers.Length;
			int num = length;
			if (length > 0)
			{
				for (int i = 0; i < num; i++)
				{
					TRenderer tRenderer = renderers[i];
					TRenderer tRenderer1 = tRenderer;
					if (tRenderer)
					{
						Material material1 = tRenderer1.sharedMaterial;
						Material material2 = material1;
						if (material1 && material2.HasProperty(HealthDimmer.PropOnce._Color))
						{
							material = material2;
							return true;
						}
					}
				}
			}
		}
		material = null;
		return false;
	}

	private void Initialize(IDBase self)
	{
		Material material;
		if (self)
		{
			TakeDamage local = self.GetLocal<TakeDamage>();
			TakeDamage takeDamage = local;
			if (local)
			{
				MeshRenderer[] componentsInChildren = self.GetComponentsInChildren<MeshRenderer>(true);
				MeshRenderer[] meshRendererArray = componentsInChildren;
				if (!HealthDimmer.GetFirstMaterial<MeshRenderer>(componentsInChildren, out material))
				{
					this.renderers = null;
					this.valid = false;
					this.takeDamage = null;
					return;
				}
				this.renderers = meshRendererArray;
				this.takeDamage = takeDamage;
				this.valid = true;
				this.structureStyle = self.idMain is StructureComponent;
				Color color = material.GetColor(HealthDimmer.PropOnce._Color);
				this.averageRGB = (color.r + color.g + color.b) * 0.333333343f;
				this.propBlock = new MaterialPropertyBlock();
				this.percent = null;
				return;
			}
		}
		this.renderers = null;
		this.valid = false;
		this.takeDamage = null;
	}

	private void MakeColor(float percent, out Color color)
	{
		color = new Color();
		float single;
		if (!this.structureStyle)
		{
			float single1 = this.averageRGB * 0.33f;
			single = single1 + (this.averageRGB - single1) * percent;
		}
		else
		{
			single = 0.35f + (this.averageRGB - 0.35f) * percent;
		}
		float single2 = single;
		float single3 = single2;
		color.b = single2;
		float single4 = single3;
		single3 = single4;
		color.g = single4;
		color.r = single3;
		color.a = 1f;
	}

	public void Reset()
	{
		this.percent = null;
		if (!this.initialized)
		{
			return;
		}
		if (this.propBlock != null)
		{
			this.propBlock.Clear();
		}
		if (this.valid)
		{
			MeshRenderer[] meshRendererArray = this.renderers;
			for (int i = 0; i < (int)meshRendererArray.Length; i++)
			{
				MeshRenderer meshRenderer = meshRendererArray[i];
				if (meshRenderer)
				{
					meshRenderer.SetPropertyBlock(null);
				}
			}
		}
	}

	public void UpdateHealthAmount(IDBase self, float newHealth, bool force = false)
	{
		Color color;
		if (!this.initialized)
		{
			this.initialized = true;
			this.Initialize(self);
		}
		if (!this.takeDamage)
		{
			return;
		}
		this.takeDamage.health = newHealth;
		if (this.disabled || !this.valid)
		{
			return;
		}
		float single = this.takeDamage.health / this.takeDamage.maxHealth;
		if (!force && this.percent.HasValue && this.percent.Value == single)
		{
			return;
		}
		this.percent = new float?(single);
		this.MakeColor(single, out color);
		this.propBlock.Clear();
		this.propBlock.AddColor(HealthDimmer.PropOnce._Color, color);
		MeshRenderer[] meshRendererArray = this.renderers;
		for (int i = 0; i < (int)meshRendererArray.Length; i++)
		{
			MeshRenderer meshRenderer = meshRendererArray[i];
			if (meshRenderer)
			{
				meshRenderer.SetPropertyBlock(this.propBlock);
			}
		}
	}

	private static class PropOnce
	{
		public readonly static int _Color;

		static PropOnce()
		{
			HealthDimmer.PropOnce._Color = Shader.PropertyToID("_Color");
		}
	}
}