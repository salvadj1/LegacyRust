using System;
using UnityEngine;

[Serializable]
internal class TreeUnpackGroup
{
	public Mesh[] meshes;

	public string tag;

	public bool spherical;

	public int layer;

	public TreeUnpackGroup()
	{
	}
}