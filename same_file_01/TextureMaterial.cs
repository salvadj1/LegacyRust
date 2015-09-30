using System;
using System.Collections.Generic;
using UnityEngine;

public static class TextureMaterial
{
	private static Dictionary<Material, Dictionary<Texture, Material>> dict;

	static TextureMaterial()
	{
		TextureMaterial.dict = new Dictionary<Material, Dictionary<Texture, Material>>();
	}

	public static Material GetMaterial(Material skeleton, Texture mainTex)
	{
		Dictionary<Texture, Material> textures;
		Material material;
		if (!skeleton)
		{
			return null;
		}
		if (!TextureMaterial.dict.TryGetValue(skeleton, out textures))
		{
			Material material1 = new Material(skeleton)
			{
				mainTexture = mainTex
			};
			textures = new Dictionary<Texture, Material>()
			{
				{ mainTex, material1 }
			};
			TextureMaterial.dict.Add(skeleton, textures);
			return material1;
		}
		if (textures.TryGetValue(mainTex, out material))
		{
			return material;
		}
		Material material2 = new Material(skeleton)
		{
			mainTexture = mainTex
		};
		textures.Add(mainTex, material2);
		return material2;
	}
}