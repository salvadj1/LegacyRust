using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("Mesh/Combine Children")]
public class CombineChildren : MonoBehaviour
{
	public bool generateTriangleStrips = true;

	public bool combineOnStart = true;

	public CombineChildren()
	{
	}

	public void DoCombine()
	{
		Component[] componentsInChildren = base.GetComponentsInChildren(typeof(MeshFilter));
		Matrix4x4 matrix4x4 = base.transform.worldToLocalMatrix;
		Hashtable hashtables = new Hashtable();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			MeshFilter meshFilter = (MeshFilter)componentsInChildren[i];
			Renderer renderer = componentsInChildren[i].renderer;
			MeshCombineUtility.MeshInstance meshInstance = new MeshCombineUtility.MeshInstance()
			{
				mesh = meshFilter.sharedMesh
			};
			if (renderer != null && renderer.enabled && meshInstance.mesh != null)
			{
				meshInstance.transform = matrix4x4 * meshFilter.transform.localToWorldMatrix;
				Material[] materialArray = renderer.sharedMaterials;
				for (int j = 0; j < (int)materialArray.Length; j++)
				{
					meshInstance.subMeshIndex = Math.Min(j, meshInstance.mesh.subMeshCount - 1);
					ArrayList item = (ArrayList)hashtables[materialArray[j]];
					if (item == null)
					{
						item = new ArrayList();
						item.Add(meshInstance);
						hashtables.Add(materialArray[j], item);
					}
					else
					{
						item.Add(meshInstance);
					}
				}
				renderer.enabled = false;
			}
		}
		IDictionaryEnumerator enumerator = hashtables.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				DictionaryEntry current = (DictionaryEntry)enumerator.Current;
				MeshCombineUtility.MeshInstance[] array = (MeshCombineUtility.MeshInstance[])((ArrayList)current.Value).ToArray(typeof(MeshCombineUtility.MeshInstance));
				if (hashtables.Count != 1)
				{
					GameObject gameObject = new GameObject("Combined mesh");
					gameObject.transform.parent = base.transform;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.AddComponent(typeof(MeshFilter));
					gameObject.AddComponent("MeshRenderer");
					gameObject.renderer.material = (Material)current.Key;
					MeshFilter component = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
					component.mesh = MeshCombineUtility.Combine(array, this.generateTriangleStrips);
				}
				else
				{
					if (base.GetComponent(typeof(MeshFilter)) == null)
					{
						base.gameObject.AddComponent(typeof(MeshFilter));
					}
					if (!base.GetComponent("MeshRenderer"))
					{
						base.gameObject.AddComponent("MeshRenderer");
					}
					MeshFilter component1 = (MeshFilter)base.GetComponent(typeof(MeshFilter));
					component1.mesh = MeshCombineUtility.Combine(array, this.generateTriangleStrips);
					base.renderer.material = (Material)current.Key;
					base.renderer.enabled = true;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	private void Start()
	{
		if (this.combineOnStart)
		{
			this.DoCombine();
		}
	}
}