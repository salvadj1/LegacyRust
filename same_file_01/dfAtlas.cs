using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Texture Atlas")]
[ExecuteInEditMode]
[Serializable]
public class dfAtlas : MonoBehaviour
{
	[SerializeField]
	protected UnityEngine.Material material;

	[SerializeField]
	protected List<dfAtlas.ItemInfo> items = new List<dfAtlas.ItemInfo>();

	private Dictionary<string, dfAtlas.ItemInfo> map = new Dictionary<string, dfAtlas.ItemInfo>();

	private dfAtlas replacementAtlas;

	public int Count
	{
		get
		{
			return (this.replacementAtlas == null ? this.items.Count : this.replacementAtlas.Count);
		}
	}

	public dfAtlas.ItemInfo this[string key]
	{
		get
		{
			if (this.replacementAtlas != null)
			{
				return this.replacementAtlas[key];
			}
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			if (this.map.Count == 0)
			{
				this.RebuildIndexes();
			}
			dfAtlas.ItemInfo itemInfo = null;
			if (this.map.TryGetValue(key, out itemInfo))
			{
				return itemInfo;
			}
			return null;
		}
	}

	public List<dfAtlas.ItemInfo> Items
	{
		get
		{
			return (this.replacementAtlas == null ? this.items : this.replacementAtlas.Items);
		}
	}

	public UnityEngine.Material Material
	{
		get
		{
			return (this.replacementAtlas == null ? this.material : this.replacementAtlas.Material);
		}
		set
		{
			if (this.replacementAtlas == null)
			{
				this.material = value;
			}
			else
			{
				this.replacementAtlas.Material = value;
			}
		}
	}

	public dfAtlas Replacement
	{
		get
		{
			return this.replacementAtlas;
		}
		set
		{
			this.replacementAtlas = value;
		}
	}

	public Texture2D Texture
	{
		get
		{
			return (this.replacementAtlas == null ? this.material.mainTexture as Texture2D : this.replacementAtlas.Texture);
		}
	}

	public dfAtlas()
	{
	}

	public void AddItem(dfAtlas.ItemInfo item)
	{
		this.items.Add(item);
		this.RebuildIndexes();
	}

	public void AddItems(IEnumerable<dfAtlas.ItemInfo> items)
	{
		this.items.AddRange(items);
		this.RebuildIndexes();
	}

	internal static bool Equals(dfAtlas lhs, dfAtlas rhs)
	{
		if (object.ReferenceEquals(lhs, rhs))
		{
			return true;
		}
		if (lhs == null || rhs == null)
		{
			return false;
		}
		return lhs.material == rhs.material;
	}

	public void RebuildIndexes()
	{
		if (this.map != null)
		{
			this.map.Clear();
		}
		else
		{
			this.map = new Dictionary<string, dfAtlas.ItemInfo>();
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			dfAtlas.ItemInfo item = this.items[i];
			this.map[item.name] = item;
		}
	}

	public void Remove(string name)
	{
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (this.items[i].name == name)
			{
				this.items.RemoveAt(i);
			}
		}
		this.RebuildIndexes();
	}

	[Serializable]
	public class ItemInfo : IComparable<dfAtlas.ItemInfo>, IEquatable<dfAtlas.ItemInfo>
	{
		public string name;

		public Rect region;

		public RectOffset border;

		public bool rotated;

		public Vector2 sizeInPixels;

		[SerializeField]
		public string textureGUID;

		public bool deleted;

		[SerializeField]
		public Texture2D texture;

		public ItemInfo()
		{
		}

		public int CompareTo(dfAtlas.ItemInfo other)
		{
			return this.name.CompareTo(other.name);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is dfAtlas.ItemInfo))
			{
				return false;
			}
			return this.name.Equals(((dfAtlas.ItemInfo)obj).name);
		}

		public bool Equals(dfAtlas.ItemInfo other)
		{
			return this.name.Equals(other.name);
		}

		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		public static bool operator ==(dfAtlas.ItemInfo lhs, dfAtlas.ItemInfo rhs)
		{
			if (object.ReferenceEquals(lhs, rhs))
			{
				return true;
			}
			if (lhs == null || rhs == null)
			{
				return false;
			}
			return lhs.name.Equals(rhs.name);
		}

		public static bool operator !=(dfAtlas.ItemInfo lhs, dfAtlas.ItemInfo rhs)
		{
			return !(lhs == rhs);
		}
	}
}