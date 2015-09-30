using System;
using UnityEngine;

public class DisplayRenderBounds : MonoBehaviour
{
	public DisplayRenderBounds()
	{
	}

	private void OnDrawGizmos()
	{
		Renderer renderer = base.renderer;
		if (renderer)
		{
			Bounds bound = renderer.bounds;
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(bound.center, bound.size);
			if (!(renderer is SkinnedMeshRenderer))
			{
				MeshFilter component = base.GetComponent<MeshFilter>();
				if (component)
				{
					Mesh mesh = component.sharedMesh;
					if (mesh)
					{
						Gizmos.color = Color.magenta;
						Gizmos.matrix = base.transform.localToWorldMatrix;
						bound = mesh.bounds;
						Gizmos.DrawWireCube(bound.center, bound.size);
					}
				}
			}
			else
			{
				SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
				Gizmos.color = Color.yellow;
				Gizmos.matrix = skinnedMeshRenderer.localToWorldMatrix;
				bound = skinnedMeshRenderer.localBounds;
				Gizmos.DrawWireCube(bound.center, bound.size);
			}
		}
	}
}