using Facepunch;
using Facepunch.Build;
using Facepunch.Hash;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

[UniqueBundleScriptableObject]
public class NGCConfiguration : ScriptableObject
{
	private const string bundledPath = "content/network/NGCConf";

	[SerializeField]
	private NGCConfiguration.PrefabEntry[] entries;

	public NGCConfiguration()
	{
	}

	public void Install()
	{
		NGCConfiguration.PrefabEntry[] prefabEntryArray = this.entries;
		for (int i = 0; i < (int)prefabEntryArray.Length; i++)
		{
			NGCConfiguration.PrefabEntry prefabEntry = prefabEntryArray[i];
			if (prefabEntry != null && prefabEntry.ReadyToRegister)
			{
				NGC.Prefab.Register.Add(prefabEntry.Path, prefabEntry.HashCode, string.Concat(";", prefabEntry.Name));
			}
		}
	}

	public static NGCConfiguration Load()
	{
		return Bundling.Load<NGCConfiguration>("content/network/NGCConf");
	}

	protected void OnEnable()
	{
		if (this.entries != null)
		{
			HashSet<string> strs = new HashSet<string>();
			int num = 0;
			for (int i = 0; i < (int)this.entries.Length; i++)
			{
				if (this.entries[i] != null)
				{
					if (strs.Add(this.entries[i].Name))
					{
						if (string.IsNullOrEmpty(this.entries[i].Path))
						{
							UnityEngine.Debug.LogWarning(string.Format("ngc prefab {0} has no path!", this.entries[i].Name), this);
						}
						int num1 = num;
						num = num1 + 1;
						this.entries[num1] = this.entries[i];
					}
					else
					{
						UnityEngine.Debug.LogWarning(string.Format("Removing duplicate ngc prefab named '{0}' (path:{1})", this.entries[i].Name, this.entries[i].Path));
					}
				}
			}
			if (num < (int)this.entries.Length)
			{
				Array.Resize<NGCConfiguration.PrefabEntry>(ref this.entries, num);
				UnityEngine.Debug.LogWarning("The entries of the ngcconfiguration were altered!", this);
			}
		}
		else
		{
			this.entries = new NGCConfiguration.PrefabEntry[0];
		}
	}

	[Serializable]
	public sealed class PrefabEntry
	{
		private const uint peSeed = 2260766486;

		[SerializeField]
		private string name;

		[SerializeField]
		private string path;

		[SerializeField]
		private string guidText;

		[NonSerialized]
		private bool calculatedHashCode;

		[NonSerialized]
		private int _hashCode;

		public int HashCode
		{
			get
			{
				int num;
				if (!this.calculatedHashCode)
				{
					int num1 = NGCConfiguration.PrefabEntry.hash(this.guidText);
					int num2 = num1;
					this._hashCode = num1;
					num = num2;
				}
				else
				{
					num = this._hashCode;
				}
				return num;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string Path
		{
			get
			{
				return this.path;
			}
		}

		public bool ReadyToRegister
		{
			get
			{
				return (string.IsNullOrEmpty(this.Name) || string.IsNullOrEmpty(this.Path) ? false : this.HashCode != 0);
			}
		}

		public PrefabEntry()
		{
		}

		public override int GetHashCode()
		{
			int num;
			if (!this.calculatedHashCode)
			{
				int num1 = NGCConfiguration.PrefabEntry.hash(this.guidText);
				int num2 = num1;
				this._hashCode = num1;
				num = num2;
			}
			else
			{
				num = this._hashCode;
			}
			return num;
		}

		private static int hash(string guid)
		{
			if (string.IsNullOrEmpty(guid))
			{
				return 0;
			}
			int current = 0;
			int num = 0;
			int current1 = 0;
			int num1 = 0;
			int current2 = 0;
			int num2 = 0;
			using (IEnumerator<int> enumerator = NGCConfiguration.PrefabEntry.ParseInts(guid))
			{
				if (enumerator.MoveNext())
				{
					current = enumerator.Current;
					if (enumerator.MoveNext())
					{
						num = enumerator.Current;
						if (enumerator.MoveNext())
						{
							current1 = enumerator.Current;
							if (enumerator.MoveNext())
							{
								num1 = enumerator.Current;
								if (enumerator.MoveNext())
								{
									current2 = enumerator.Current;
									if (enumerator.MoveNext())
									{
										num2 = enumerator.Current;
									}
								}
							}
						}
					}
				}
			}
			NGCConfiguration.PrefabEntry.hashwork.guid[0] = current;
			NGCConfiguration.PrefabEntry.hashwork.guid[1] = num2;
			NGCConfiguration.PrefabEntry.hashwork.guid[2] = current2;
			NGCConfiguration.PrefabEntry.hashwork.guid[3] = current1;
			NGCConfiguration.PrefabEntry.hashwork.guid[4] = num1;
			NGCConfiguration.PrefabEntry.hashwork.guid[5] = num;
			return MurmurHash2.SINT(NGCConfiguration.PrefabEntry.hashwork.guid, (int)NGCConfiguration.PrefabEntry.hashwork.guid.Length, -2034200810);
		}

		[DebuggerHidden]
		private static IEnumerator<int> ParseInts(string hex)
		{
			NGCConfiguration.PrefabEntry.<ParseInts>c__Iterator2B variable = null;
			return variable;
		}

		public override string ToString()
		{
			return string.Format("[PrefabEntry: Name=\"{1}\", HashCode={0:X}, Path=\"{2}\"]", this.HashCode, this.Name, this.Path);
		}

		private static class hashwork
		{
			public readonly static int[] guid;

			static hashwork()
			{
				NGCConfiguration.PrefabEntry.hashwork.guid = new int[6];
			}
		}
	}
}