using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SurveillanceMonitor : MonoBehaviour
{
	[NonSerialized]
	public Renderer renderer;

	[SerializeField]
	private int[] materialIds;

	public string textureName = "_MainTex";

	public float aspect = 1f;

	public float viewDistance = 30f;

	private Texture lastTexture;

	public SurveillanceCamera surveillanceCamera;

	private Material[] replacementMaterials;

	private Material[] originalSharedMaterials;

	private Material[] activeSharedMaterials;

	public SurveillanceMonitor()
	{
	}

	private void Awake()
	{
		Material material;
		this.renderer = base.renderer;
		this.originalSharedMaterials = this.renderer.sharedMaterials;
		if (this.materialIds == null || (int)this.materialIds.Length == 0)
		{
			Debug.LogWarning("Please, set the material ids for this SurveillanceMonitor. Assuming you meant to use id 0 only.", this);
			this.materialIds = new int[1];
		}
		HashSet<Material> materials = new HashSet<Material>();
		int num = 0;
		int[] numArray = new int[(int)this.materialIds.Length];
		for (int i = 0; i < (int)this.materialIds.Length; i++)
		{
			if (!materials.Add(this.originalSharedMaterials[this.materialIds[i]]))
			{
				for (int j = 0; j < i; j++)
				{
					if (this.originalSharedMaterials[this.materialIds[j]] == this.originalSharedMaterials[this.materialIds[i]])
					{
						numArray[i] = j;
					}
				}
			}
			else
			{
				numArray[i] = i;
				num++;
			}
		}
		this.replacementMaterials = new Material[num];
		this.activeSharedMaterials = (Material[])this.originalSharedMaterials.Clone();
		for (int k = 0; k < (int)this.materialIds.Length; k++)
		{
			if (numArray[k] != k)
			{
				material = this.replacementMaterials[this.materialIds[numArray[k]]];
			}
			else
			{
				Material[] materialArray = this.replacementMaterials;
				Material material1 = new Material(this.originalSharedMaterials[this.materialIds[k]]);
				material = material1;
				materialArray[k] = material1;
			}
			this.activeSharedMaterials[this.materialIds[k]] = material;
		}
	}

	private void BindTexture(Texture tex)
	{
		Material[] materialArray = this.replacementMaterials;
		for (int i = 0; i < (int)materialArray.Length; i++)
		{
			materialArray[i].SetTexture(this.textureName, tex);
		}
	}

	public void DropReference(RenderTexture texture)
	{
		if (this.lastTexture == texture)
		{
			this.lastTexture = null;
		}
	}

	private void OnWillRenderObject()
	{
		if (this.surveillanceCamera)
		{
			Camera camera = Camera.current;
			if (this.surveillanceCamera.camera == camera)
			{
				return;
			}
			Transform transforms = camera.transform;
			Vector3 vector3 = base.transform.position - transforms.position;
			if (vector3.sqrMagnitude <= this.viewDistance * this.viewDistance && Vector3.Dot(transforms.forward, vector3) > 0f)
			{
				RenderTexture renderTexture = this.surveillanceCamera.Render();
				Texture texture = renderTexture;
				if (!renderTexture)
				{
					goto Label1;
				}
				Material[] materialArray = this.replacementMaterials;
				for (int i = 0; i < (int)materialArray.Length; i++)
				{
					materialArray[i].SetTexture(this.textureName, texture);
				}
				this.renderer.sharedMaterials = this.activeSharedMaterials;
				return;
			}
		Label1:
			this.renderer.sharedMaterials = this.originalSharedMaterials;
		}
	}
}