using System;
using UnityEngine;

public class ModelPrefabSkin : ScriptableObject
{
	public string prefab;

	public ModelPrefabSkin.Part[] parts;

	public bool once;

	[NonSerialized]
	public object editorData;

	public ModelPrefabSkin()
	{
	}

	[Serializable]
	public class Part
	{
		public string path;

		public string mesh;

		public string[] materials;

		public Part()
		{
			this.path = string.Empty;
			this.mesh = string.Empty;
		}
	}
}