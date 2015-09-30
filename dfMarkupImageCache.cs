using System;
using System.Collections.Generic;
using UnityEngine;

public class dfMarkupImageCache
{
	private static Dictionary<string, Texture> cache;

	static dfMarkupImageCache()
	{
		dfMarkupImageCache.cache = new Dictionary<string, Texture>();
	}

	public dfMarkupImageCache()
	{
	}

	public static void Clear()
	{
		dfMarkupImageCache.cache.Clear();
	}

	public static void Load(string name, Texture image)
	{
		dfMarkupImageCache.cache[name.ToLowerInvariant()] = image;
	}

	public static Texture Load(string path)
	{
		path = path.ToLowerInvariant();
		if (dfMarkupImageCache.cache.ContainsKey(path))
		{
			return dfMarkupImageCache.cache[path];
		}
		Texture texture = Resources.Load(path) as Texture;
		if (texture != null)
		{
			dfMarkupImageCache.cache[path] = texture;
		}
		return texture;
	}

	public static void Unload(string name)
	{
		dfMarkupImageCache.cache.Remove(name.ToLowerInvariant());
	}
}