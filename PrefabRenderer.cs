using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public sealed class PrefabRenderer : IDisposable
{
	private Material[] originalMaterials;

	private Mesh[] originalMeshes;

	private PrefabRenderer.MeshRender[] meshes;

	private int[] skipBits;

	private GameObject prefab;

	private bool disposed;

	private readonly int prefabId;

	public int materialCount
	{
		get
		{
			return (int)this.originalMaterials.Length;
		}
	}

	private PrefabRenderer(int prefabId)
	{
		this.prefabId = prefabId;
		PrefabRenderer.Runtime.Register[this.prefabId] = new WeakReference(this);
	}

	public void Dispose()
	{
		if (!this.disposed)
		{
			this.disposed = true;
			GC.SuppressFinalize(this);
			object @lock = PrefabRenderer.Runtime.Lock;
			Monitor.Enter(@lock);
			try
			{
				PrefabRenderer.Runtime.Register.Remove(this.prefabId);
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
	}

	private static void DoNotCareResize<T>(ref T[] array, int size)
	{
		if (array == null || (int)array.Length != size)
		{
			array = new T[size];
		}
	}

	~PrefabRenderer()
	{
		if (!this.disposed)
		{
			lock (true)
			{
				PrefabRenderer.Runtime.Register.Remove(this.prefabId);
			}
		}
	}

	public Material GetMaterial(int index)
	{
		return this.originalMaterials[index];
	}

	public Material[] GetMaterialArrayCopy()
	{
		return (Material[])this.originalMaterials.Clone();
	}

	public static PrefabRenderer GetOrCreateRender(GameObject prefab)
	{
		PrefabRenderer target;
		bool flag;
		WeakReference weakReference;
		if (!prefab)
		{
			return null;
		}
		while (prefab.transform.parent)
		{
			prefab = prefab.transform.parent.gameObject;
		}
		int instanceID = prefab.GetInstanceID();
		object @lock = PrefabRenderer.Runtime.Lock;
		Monitor.Enter(@lock);
		try
		{
			if (!PrefabRenderer.Runtime.Register.TryGetValue(instanceID, out weakReference))
			{
				target = null;
			}
			else
			{
				target = (PrefabRenderer)weakReference.Target;
			}
			flag = target != null;
			if (!flag)
			{
				target = new PrefabRenderer(instanceID);
			}
		}
		finally
		{
			Monitor.Exit(@lock);
		}
		if (!flag)
		{
			target.prefab = prefab;
			target.Refresh();
		}
		return target;
	}

	public void Refresh()
	{
		Renderer renderer;
		PrefabRenderer prefabRenderer = this;
		Transform transforms = this.prefab.transform;
		HashSet<Material> materials = new HashSet<Material>();
		HashSet<Mesh> meshes = new HashSet<Mesh>();
		List<Material[]> materialArrays = new List<Material[]>();
		List<Mesh> meshes1 = new List<Mesh>();
		int num = 0;
		Renderer[] componentsInChildren = this.prefab.GetComponentsInChildren<Renderer>(true);
		int num1 = 0;
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			renderer = componentsInChildren[i];
			if (renderer && renderer.enabled && !renderer.name.EndsWith("-lod", StringComparison.InvariantCultureIgnoreCase) && !renderer.name.EndsWith("_LOD_LOWEST", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!(renderer is MeshRenderer))
				{
					if (renderer is SkinnedMeshRenderer)
					{
						goto Label1;
					}
					goto Label0;
				}
				else
				{
					int num2 = num1;
					num1 = num2 + 1;
					componentsInChildren[num2] = renderer;
					Mesh component = renderer.GetComponent<MeshFilter>().sharedMesh;
					if (component && meshes.Add(component))
					{
						num++;
					}
					meshes1.Add(component);
				}
			Label2:
				Material[] materialArray = renderer.sharedMaterials;
				materialArrays.Add(materialArray);
				materials.UnionWith(materialArray);
			}
		Label0:
		}
		for (int j = num1; j < (int)componentsInChildren.Length; j++)
		{
			componentsInChildren[j] = null;
		}
		int count = materials.Count;
		int num3 = (count % 32 <= 0 ? count / 32 : count / 32 + 1);
		PrefabRenderer.DoNotCareResize<int>(ref prefabRenderer.skipBits, num3);
		for (int k = 0; k < num3; k++)
		{
			this.skipBits[k] = 0;
		}
		Dictionary<Material, int> materials1 = new Dictionary<Material, int>(count);
		Dictionary<Mesh, int> meshes2 = new Dictionary<Mesh, int>(num);
		PrefabRenderer.DoNotCareResize<Material>(ref prefabRenderer.originalMaterials, count);
		int num4 = 0;
		foreach (Material material in materials)
		{
			if (material.GetTag("IgnorePrefabRenderer", false, "False") == "True")
			{
				this.skipBits[num4 / 32] = this.skipBits[num4 / 32] | 1 << (num4 % 32 & 31 & 31);
			}
			prefabRenderer.originalMaterials[num4] = material;
			int num5 = num4;
			num4 = num5 + 1;
			materials1[material] = num5;
		}
		PrefabRenderer.DoNotCareResize<Mesh>(ref prefabRenderer.originalMeshes, num);
		int num6 = 0;
		foreach (Mesh mesh in meshes)
		{
			prefabRenderer.originalMeshes[num6] = mesh;
			int num7 = num6;
			num6 = num7 + 1;
			meshes2[mesh] = num7;
		}
		PrefabRenderer.DoNotCareResize<PrefabRenderer.MeshRender>(ref prefabRenderer.meshes, num1);
		for (int l = 0; l < num1; l++)
		{
			Renderer renderer1 = componentsInChildren[l];
			Material[] item = materialArrays[l];
			int[] numArray = new int[(int)item.Length];
			for (int m = 0; m < (int)item.Length; m++)
			{
				numArray[m] = materials1[item[m]];
			}
			prefabRenderer.meshes[l].Set(meshes2[meshes1[l]], numArray, renderer1.transform.localToWorldMatrix * transforms.worldToLocalMatrix, renderer1.gameObject.layer, renderer1.castShadows, renderer1.receiveShadows);
		}
		return;
	Label1:
		int num8 = num1;
		num1 = num8 + 1;
		componentsInChildren[num8] = renderer;
		Mesh mesh1 = ((SkinnedMeshRenderer)renderer).sharedMesh;
		if (mesh1 && meshes.Add(mesh1))
		{
			num++;
		}
		meshes1.Add(mesh1);
		goto Label2;
	}

	public void Render(Camera camera, Matrix4x4 world, MaterialPropertyBlock props, Material[] overrideMaterials)
	{
		Material[] materialArray = overrideMaterials ?? this.originalMaterials;
		PrefabRenderer.MeshRender[] meshRenderArray = this.meshes;
		for (int i = 0; i < (int)meshRenderArray.Length; i++)
		{
			PrefabRenderer.MeshRender meshRender = meshRenderArray[i];
			Matrix4x4 matrix4x4 = world;
			Mesh mesh = this.originalMeshes[meshRender.mesh];
			int num = 0;
			int[] numArray = meshRender.materials;
			for (int j = 0; j < (int)numArray.Length; j++)
			{
				int num1 = numArray[j];
				if ((this.skipBits[num1 / 32] & 1 << (num1 % 32 & 31)) == 0)
				{
					Material material = materialArray[num1];
					int num2 = num;
					num = num2 + 1;
					Graphics.DrawMesh(mesh, matrix4x4, material, meshRender.layer, camera, num2, props, meshRender.castShadows, meshRender.receiveShadows);
				}
			}
		}
	}

	public void RenderOneMaterial(Camera camera, Matrix4x4 world, MaterialPropertyBlock props, Material overrideMaterial)
	{
		if (!overrideMaterial)
		{
			return;
		}
		PrefabRenderer.MeshRender[] meshRenderArray = this.meshes;
		for (int i = 0; i < (int)meshRenderArray.Length; i++)
		{
			PrefabRenderer.MeshRender meshRender = meshRenderArray[i];
			Matrix4x4 matrix4x4 = world;
			Mesh mesh = this.originalMeshes[meshRender.mesh];
			int num = 0;
			for (int j = 0; j < (int)meshRender.materials.Length; j++)
			{
				int num1 = meshRender.materials[j];
				if ((this.skipBits[num1 / 32] & 1 << (num1 % 32 & 31)) == 0)
				{
					int num2 = num;
					num = num2 + 1;
					Graphics.DrawMesh(mesh, matrix4x4, overrideMaterial, meshRender.layer, camera, num2, props, meshRender.castShadows, meshRender.receiveShadows);
				}
			}
		}
	}

	private struct MeshRender
	{
		public int mesh;

		public Matrix4x4 transform;

		public int[] materials;

		public int layer;

		public bool castShadows;

		public bool receiveShadows;

		public void Set(int mesh, int[] materials, Matrix4x4 transform, int layer, bool castShadows, bool receiveShadows)
		{
			this.mesh = mesh;
			this.materials = materials;
			this.transform = transform;
			this.layer = layer;
			this.castShadows = castShadows;
			this.receiveShadows = receiveShadows;
		}
	}

	private static class Runtime
	{
		public static object Lock;

		public static Dictionary<int, WeakReference> Register;

		static Runtime()
		{
			PrefabRenderer.Runtime.Lock = new object();
			PrefabRenderer.Runtime.Register = new Dictionary<int, WeakReference>();
		}
	}
}