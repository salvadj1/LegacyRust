using System;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabRendererTest : MonoBehaviour
{
	public GameObject prefab;

	public Material[] materialKeys;

	public Material[] materialValues;

	[NonSerialized]
	private PrefabRenderer renderer;

	[NonSerialized]
	private GameObject prefabRendering;

	[NonSerialized]
	private Material[] oldMaterialKeys;

	[NonSerialized]
	private Material[] oldMaterialValues;

	[NonSerialized]
	private bool oi;

	[NonSerialized]
	private Material[] overrideMaterials;

	public PrefabRendererTest()
	{
	}

	[ContextMenu("Refresh material overrides")]
	private void ApplyOverrides()
	{
		if (this.renderer == null)
		{
			return;
		}
		this.overrideMaterials = this.renderer.GetMaterialArrayCopy();
		if ((int)this.overrideMaterials.Length == 0 || this.materialKeys == null || this.materialValues == null)
		{
			return;
		}
		int num = Mathf.Min((int)this.overrideMaterials.Length, Mathf.Min((int)this.materialKeys.Length, (int)this.materialValues.Length));
		for (int i = 0; i < num; i++)
		{
			int num1 = Array.IndexOf<Material>(this.materialKeys, this.overrideMaterials[i]);
			if (num1 != -1 && num1 < (int)this.materialValues.Length)
			{
				this.overrideMaterials[i] = this.materialValues[num1];
			}
		}
	}

	[ContextMenu("List Materials")]
	private void ListMaterials()
	{
		if (this.renderer == null)
		{
			return;
		}
		int num = this.renderer.materialCount;
		for (int i = 0; i < num; i++)
		{
			Debug.Log(this.renderer.GetMaterial(i), this.renderer.GetMaterial(i));
		}
	}

	[ContextMenu("Print info")]
	private void PrintINfo()
	{
		if (this.renderer != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Material[] materialArrayCopy = this.renderer.GetMaterialArrayCopy();
			for (int i = 0; i < (int)materialArrayCopy.Length; i++)
			{
				stringBuilder.AppendLine(materialArrayCopy[i].ToString());
			}
			Debug.Log(stringBuilder, this);
		}
		else
		{
			Debug.Log("No Renderer", this);
		}
	}

	[ContextMenu("Refresh")]
	private void RefreshRenderer()
	{
		if (this.renderer != null)
		{
			this.renderer.Refresh();
		}
	}

	private void Update()
	{
		if (this.prefabRendering != this.prefab || !this.oi)
		{
			if (this.prefabRendering)
			{
				this.renderer = null;
			}
			if (this.prefab)
			{
				this.renderer = PrefabRenderer.GetOrCreateRender(this.prefab);
			}
			this.prefabRendering = this.prefab;
			this.oi = true;
			this.ApplyOverrides();
		}
		if (this.renderer == null)
		{
			Debug.Log("None", this);
			return;
		}
		this.renderer.Render(null, base.transform.localToWorldMatrix, null, this.overrideMaterials);
	}
}