using System;
using UnityEngine;

public class SceneChildMeshes : MonoBehaviour
{
	[SerializeField]
	private Mesh[] sceneMeshes;

	[SerializeField]
	private Mesh[] treeMeshes;

	private static SceneChildMeshes lastFound;

	public SceneChildMeshes()
	{
	}

	private static SceneChildMeshes GetMapSingleton(bool canCreate)
	{
		if (!SceneChildMeshes.lastFound)
		{
			UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(SceneChildMeshes));
			if ((int)objArray.Length != 0)
			{
				SceneChildMeshes.lastFound = (SceneChildMeshes)objArray[0];
			}
			else if (canCreate)
			{
				GameObject gameObject = new GameObject("__Scene Child Meshes", new Type[] { typeof(SceneChildMeshes) })
				{
					hideFlags = HideFlags.HideInHierarchy
				};
				SceneChildMeshes.lastFound = gameObject.GetComponent<SceneChildMeshes>();
			}
		}
		return SceneChildMeshes.lastFound;
	}
}