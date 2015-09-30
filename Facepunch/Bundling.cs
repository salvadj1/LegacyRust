using Facepunch.Load;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Facepunch
{
	public static class Bundling
	{
		private const bool kBundleUnloadClearsEverything = true;

		private const string kUnloadedBundlesMessage = "Bundles were not loaded";

		private static Bundling.LoadedBundleMap Map;

		private static bool HasLoadedBundleMap;

		private static Bundling.OnLoadedEventHandler nextLoadEvents;

		public static bool Loaded
		{
			get
			{
				return Bundling.HasLoadedBundleMap;
			}
		}

		public static void BindToLoader(Loader loader)
		{
			Bundling.BundleBridger bundleBridger = new Bundling.BundleBridger();
			loader.OnGroupedAssetBundlesLoaded += new MultipleAssetBundlesLoadedEventHandler(bundleBridger.AddArrays);
			loader.OnAllAssetBundlesLoaded += new MultipleAssetBundlesLoadedEventHandler(bundleBridger.FinalizeAndInstall);
		}

		public static bool Load(string path, Type type, out UnityEngine.Object asset)
		{
			if (!Bundling.HasLoadedBundleMap)
			{
				throw new InvalidOperationException("Bundles were not loaded");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				asset = null;
				return false;
			}
			return Bundling.Map.Assets.Load(path, type, out asset);
		}

		public static UnityEngine.Object Load(string path, Type type)
		{
			UnityEngine.Object obj;
			Bundling.Load(path, type, out obj);
			return obj;
		}

		public static bool Load<T>(string path, out T asset)
		where T : UnityEngine.Object
		{
			UnityEngine.Object obj;
			if (Bundling.Load(path, typeof(T), out obj))
			{
				asset = (T)obj;
				return true;
			}
			asset = (T)null;
			return false;
		}

		public static T Load<T>(string path)
		where T : UnityEngine.Object
		{
			T t;
			Bundling.Load<T>(path, out t);
			return t;
		}

		public static bool Load<T>(string path, Type type, out T asset)
		where T : UnityEngine.Object
		{
			UnityEngine.Object obj;
			if (!typeof(T).IsAssignableFrom(type))
			{
				throw new ArgumentException(string.Format("The given type ({1}) cannot cast to {0}", typeof(T), type), "type");
			}
			if (Bundling.Load(path, type, out obj))
			{
				asset = (T)obj;
				return true;
			}
			asset = (T)null;
			return false;
		}

		public static T Load<T>(string path, Type type)
		where T : UnityEngine.Object
		{
			T t;
			Bundling.Load<T>(path, type, out t);
			return t;
		}

		public static UnityEngine.Object[] LoadAll()
		{
			if (!Bundling.HasLoadedBundleMap)
			{
				throw new InvalidOperationException("Bundles were not loaded");
			}
			return (new List<UnityEngine.Object>(Bundling.Map.Assets.LoadAll())).ToArray();
		}

		public static UnityEngine.Object[] LoadAll(Type type)
		{
			if (type == typeof(UnityEngine.Object))
			{
				return Bundling.LoadAll();
			}
			if (!Bundling.HasLoadedBundleMap)
			{
				throw new InvalidOperationException("Bundles were not loaded");
			}
			return (new List<UnityEngine.Object>(Bundling.Map.Assets.LoadAll(type))).ToArray();
		}

		public static T[] LoadAll<T>()
		where T : UnityEngine.Object
		{
			if (!Bundling.HasLoadedBundleMap)
			{
				throw new InvalidOperationException("Bundles were not loaded");
			}
			List<T> ts = new List<T>();
			IEnumerator<UnityEngine.Object> enumerator = Bundling.Map.Assets.LoadAll(typeof(T)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ts.Add((T)enumerator.Current);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			return ts.ToArray();
		}

		public static T[] LoadAll<T>(Type type)
		where T : UnityEngine.Object
		{
			if (!typeof(T).IsAssignableFrom(type))
			{
				throw new ArgumentException(string.Format("The given type ({1}) cannot cast to {0}", typeof(T), type), "type");
			}
			if (!Bundling.HasLoadedBundleMap)
			{
				throw new InvalidOperationException("Bundles were not loaded");
			}
			List<T> ts = new List<T>();
			IEnumerator<UnityEngine.Object> enumerator = Bundling.Map.Assets.LoadAll(type).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ts.Add((T)enumerator.Current);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			return ts.ToArray();
		}

		[Obsolete("This only works outside of editor for now, avoid it")]
		public static bool LoadAsync(string path, Type type, out AssetBundleRequest request)
		{
			if (!Bundling.HasLoadedBundleMap)
			{
				throw new InvalidOperationException("Bundles were not loaded");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				request = null;
				return false;
			}
			return Bundling.Map.Assets.LoadAsync(path, type, out request);
		}

		[Obsolete("This only works outside of editor for now, avoid it")]
		public static AssetBundleRequest LoadAsync(string path, Type type)
		{
			AssetBundleRequest assetBundleRequest;
			Bundling.LoadAsync(path, type, out assetBundleRequest);
			return assetBundleRequest;
		}

		[Obsolete("This only works outside of editor for now, avoid it")]
		public static bool LoadAsync<T>(string path, out AssetBundleRequest request)
		where T : UnityEngine.Object
		{
			return Bundling.LoadAsync(path, typeof(T), out request);
		}

		[Obsolete("This only works outside of editor for now, avoid it")]
		public static AssetBundleRequest LoadAsync<T>(string path)
		{
			return Bundling.LoadAsync(path, typeof(T));
		}

		public static void Unload()
		{
			if (Bundling.HasLoadedBundleMap)
			{
				Bundling.Map.Dispose();
			}
		}

		public static event Bundling.OnLoadedEventHandler OnceLoaded
		{
			add
			{
				if (!Bundling.Loaded)
				{
					Bundling.nextLoadEvents += value;
				}
				else
				{
					value();
				}
			}
			remove
			{
				Bundling.nextLoadEvents -= value;
			}
		}

		private class BundleBridger
		{
			private readonly List<Bundling.LoadedBundle> scenes;

			private readonly Dictionary<Type, List<Bundling.LoadedBundle>> assetsMap;

			private Type lastAssetMapSearchKey;

			private List<Bundling.LoadedBundle> lastAssetMapSearchValue;

			public BundleBridger()
			{
			}

			public void Add(AssetBundle bundle, Item item)
			{
				if (item.ContentType != ContentType.Assets)
				{
					this.scenes.Add(new Bundling.LoadedBundle(bundle, item));
				}
				else
				{
					this.AssetListOfType(item.TypeOfAssets).Add(new Bundling.LoadedBundle(bundle, item));
				}
			}

			public void AddArrays(AssetBundle[] bundles, Item[] items)
			{
				for (int i = 0; i < (int)bundles.Length; i++)
				{
					this.Add(bundles[i], items[i]);
				}
			}

			private List<Bundling.LoadedBundle> AssetListOfType(Type type)
			{
				if (type != this.lastAssetMapSearchKey)
				{
					Type type1 = type;
					Type type2 = type1;
					this.lastAssetMapSearchKey = type1;
					if (!this.assetsMap.TryGetValue(type2, out this.lastAssetMapSearchValue))
					{
						Dictionary<Type, List<Bundling.LoadedBundle>> types = this.assetsMap;
						Type type3 = this.lastAssetMapSearchKey;
						List<Bundling.LoadedBundle> loadedBundles = new List<Bundling.LoadedBundle>();
						List<Bundling.LoadedBundle> loadedBundles1 = loadedBundles;
						this.lastAssetMapSearchValue = loadedBundles;
						types[type3] = loadedBundles1;
					}
				}
				return this.lastAssetMapSearchValue;
			}

			public void FinalizeAndInstall(AssetBundle[] bundles, Item[] items)
			{
				if (new Bundling.LoadedBundleMap(this.assetsMap, this.scenes) == Bundling.Map && Bundling.nextLoadEvents != null)
				{
					Bundling.OnLoadedEventHandler onLoadedEventHandler = Bundling.nextLoadEvents;
					Bundling.nextLoadEvents = null;
					try
					{
						onLoadedEventHandler();
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogException(exception);
					}
				}
			}

			private bool Remove(Type type)
			{
				if (!this.assetsMap.Remove(type))
				{
					return false;
				}
				if (type == this.lastAssetMapSearchKey)
				{
					this.lastAssetMapSearchValue = null;
					this.lastAssetMapSearchKey = null;
				}
				return true;
			}
		}

		private class LoadedBundle
		{
			public readonly Item Item;

			private AssetBundle Bundle;

			public LoadedBundle(AssetBundle bundle, Item item)
			{
				this.Bundle = bundle;
				this.Item = item;
			}

			public bool Contains(string path)
			{
				return this.Bundle.Contains(path);
			}

			public UnityEngine.Object Load(string path)
			{
				return this.Bundle.Load(path);
			}

			public UnityEngine.Object Load(string path, Type type)
			{
				return this.Bundle.Load(path, type);
			}

			public UnityEngine.Object[] LoadAll()
			{
				return this.Bundle.LoadAll();
			}

			public UnityEngine.Object[] LoadAll(Type type)
			{
				return this.Bundle.LoadAll(type);
			}

			public AssetBundleRequest LoadAsync(string path, Type type)
			{
				return this.Bundle.LoadAsync(path, type);
			}

			internal void Unload()
			{
				if (this.Bundle)
				{
					this.Bundle.Unload(true);
				}
				this.Bundle = null;
			}
		}

		private class LoadedBundleAssetMap
		{
			public readonly Bundling.LoadedBundleListOfAssets[] AllLoadedBundleAssetLists;

			private readonly Dictionary<Type, short[]> typeMap;

			private readonly short[] tempBuffer;

			internal LoadedBundleAssetMap(IEnumerable<KeyValuePair<Type, List<Bundling.LoadedBundle>>> assets)
			{
				List<KeyValuePair<Type, List<Bundling.LoadedBundle>>> keyValuePairs = new List<KeyValuePair<Type, List<Bundling.LoadedBundle>>>(assets);
				keyValuePairs.Sort((KeyValuePair<Type, List<Bundling.LoadedBundle>> x, KeyValuePair<Type, List<Bundling.LoadedBundle>> y) => ((!typeof(GameObject).IsAssignableFrom(x.Key) ? (!typeof(ScriptableObject).IsAssignableFrom(x.Key) ? 2 : 1) : 0)).CompareTo((!typeof(GameObject).IsAssignableFrom(y.Key) ? (!typeof(ScriptableObject).IsAssignableFrom(y.Key) ? 2 : 1) : 0)));
				this.AllLoadedBundleAssetLists = new Bundling.LoadedBundleListOfAssets[keyValuePairs.Count];
				for (int i = 0; i < (int)this.AllLoadedBundleAssetLists.Length; i++)
				{
					KeyValuePair<Type, List<Bundling.LoadedBundle>> item = keyValuePairs[i];
					this.AllLoadedBundleAssetLists[i] = new Bundling.LoadedBundleListOfAssets(item.Key, item.Value);
				}
				this.tempBuffer = new short[(int)this.AllLoadedBundleAssetLists.Length];
			}

			public bool Load(string path, Type type, out UnityEngine.Object asset)
			{
				short[] numArray;
				if (!this.TypeIndices(type, out numArray))
				{
					UnityEngine.Debug.Log(string.Concat("no type index for ", type));
					asset = null;
					return false;
				}
				int num = 0;
				while (numArray[num] >= 0)
				{
					if (this.AllLoadedBundleAssetLists[numArray[num]].Load(path, out asset))
					{
						return true;
					}
					int num1 = num + 1;
					num = num1;
					if (num1 < (int)numArray.Length)
					{
						continue;
					}
					asset = null;
					return false;
				}
				while (num < (int)numArray.Length)
				{
					if (this.AllLoadedBundleAssetLists[-(numArray[num] + 1)].Load(path, type, out asset))
					{
						return true;
					}
					num++;
				}
				asset = null;
				return false;
			}

			[DebuggerHidden]
			public IEnumerable<UnityEngine.Object> LoadAll()
			{
				Bundling.LoadedBundleAssetMap.<LoadAll>c__Iterator15 variable = null;
				return variable;
			}

			[DebuggerHidden]
			public IEnumerable<UnityEngine.Object> LoadAll(Type type)
			{
				Bundling.LoadedBundleAssetMap.<LoadAll>c__Iterator16 variable = null;
				return variable;
			}

			public bool LoadAsync(string path, Type type, out AssetBundleRequest request)
			{
				short[] numArray;
				if (!this.TypeIndices(type, out numArray))
				{
					request = null;
					return false;
				}
				int num = 0;
				while (numArray[num] >= 0)
				{
					if (this.AllLoadedBundleAssetLists[numArray[num]].LoadAsync(path, out request))
					{
						return true;
					}
					int num1 = num + 1;
					num = num1;
					if (num1 < (int)numArray.Length)
					{
						continue;
					}
					request = null;
					return false;
				}
				while (num < (int)numArray.Length)
				{
					if (this.AllLoadedBundleAssetLists[-(numArray[num] + 1)].LoadAsync(path, type, out request))
					{
						return true;
					}
					num++;
				}
				request = null;
				return false;
			}

			private bool TypeIndices(Type key, out short[] value)
			{
				if (key == null)
				{
					throw new ArgumentNullException("type");
				}
				if (this.typeMap.TryGetValue(key, out value))
				{
					return value != null;
				}
				if (!typeof(UnityEngine.Object).IsAssignableFrom(key))
				{
					throw new ArgumentOutOfRangeException("type", string.Format("type {0} is not assignable to UnityEngine.Object", key));
				}
				if (typeof(Component).IsAssignableFrom(key))
				{
					if (typeof(Component) != key)
					{
						bool flag = this.TypeIndices(typeof(Component), out value);
						this.typeMap[key] = value;
						return flag;
					}
					bool flag1 = this.TypeIndices(typeof(GameObject), out value);
					value = (short[])value.Clone();
					for (int i = 0; i < (int)value.Length; i++)
					{
						if (value[i] >= 0)
						{
							value[i] = (short)(-(value[i] + 1));
						}
					}
					this.typeMap[key] = value;
					return flag1;
				}
				int num = 0;
				for (int j = 0; j < (int)this.AllLoadedBundleAssetLists.Length; j++)
				{
					if (key.IsAssignableFrom(this.AllLoadedBundleAssetLists[j].TypeOfAssets))
					{
						int num1 = num;
						num = num1 + 1;
						this.tempBuffer[num1] = (short)j;
					}
				}
				int num2 = 0;
				int num3 = num;
				for (int k = 0; k < (int)this.AllLoadedBundleAssetLists.Length; k++)
				{
					if (num2 < num3 && k == this.tempBuffer[num2])
					{
						num2++;
					}
					else if (this.AllLoadedBundleAssetLists[k].TypeOfAssets.IsAssignableFrom(key))
					{
						int num4 = num;
						num = num4 + 1;
						this.tempBuffer[num4] = (short)(-(k + 1));
					}
				}
				if (num == 0)
				{
					object obj = null;
					short[] numArray = (short[])obj;
					value = (short[])obj;
					this.typeMap[key] = numArray;
					return false;
				}
				value = new short[num];
				while (true)
				{
					int num5 = num - 1;
					num = num5;
					if (num5 < 0)
					{
						break;
					}
					value[num] = this.tempBuffer[num];
				}
				this.typeMap[key] = value;
				return true;
			}

			internal void Unload()
			{
				Bundling.LoadedBundleListOfAssets[] allLoadedBundleAssetLists = this.AllLoadedBundleAssetLists;
				for (int i = 0; i < (int)allLoadedBundleAssetLists.Length; i++)
				{
					allLoadedBundleAssetLists[i].Unload();
				}
			}
		}

		private class LoadedBundleListOfAssets
		{
			public readonly Type TypeOfAssets;

			public readonly Bundling.LoadedBundle[] Bundles;

			private readonly Dictionary<string, short> pathsToFoundBundles;

			public LoadedBundleListOfAssets(Type typeOfAssets, List<Bundling.LoadedBundle> bundles)
			{
				this.TypeOfAssets = typeOfAssets;
				this.Bundles = bundles.ToArray();
				this.pathsToFoundBundles = new Dictionary<string, short>(StringComparer.InvariantCultureIgnoreCase);
			}

			public bool Load(string path, Type type, out UnityEngine.Object asset)
			{
				short num;
				if (!this.PathIndex(path, out num))
				{
					asset = null;
					return false;
				}
				UnityEngine.Object obj = this.Bundles[num].Load(path, type);
				UnityEngine.Object obj1 = obj;
				asset = obj;
				return obj1;
			}

			public bool Load(string path, out UnityEngine.Object asset)
			{
				short num;
				if (!this.PathIndex(path, out num))
				{
					asset = null;
					return false;
				}
				UnityEngine.Object obj = this.Bundles[num].Load(path);
				UnityEngine.Object obj1 = obj;
				asset = obj;
				return obj1;
			}

			[DebuggerHidden]
			public IEnumerable<UnityEngine.Object> LoadAll()
			{
				Bundling.LoadedBundleListOfAssets.<LoadAll>c__Iterator17 variable = null;
				return variable;
			}

			[DebuggerHidden]
			public IEnumerable<UnityEngine.Object> LoadAll(Type type)
			{
				Bundling.LoadedBundleListOfAssets.<LoadAll>c__Iterator18 variable = null;
				return variable;
			}

			public bool LoadAsync(string path, Type type, out AssetBundleRequest request)
			{
				short num;
				if (!this.PathIndex(path, out num))
				{
					request = null;
					return false;
				}
				AssetBundleRequest assetBundleRequest = this.Bundles[num].LoadAsync(path, type);
				AssetBundleRequest assetBundleRequest1 = assetBundleRequest;
				request = assetBundleRequest;
				return assetBundleRequest1 != null;
			}

			public bool LoadAsync(string path, out AssetBundleRequest request)
			{
				return this.LoadAsync(path, this.TypeOfAssets, out request);
			}

			private bool PathIndex(string path, out short index)
			{
				if (this.pathsToFoundBundles.TryGetValue(path, out index))
				{
					if (index == -1)
					{
						return false;
					}
					return true;
				}
				for (int i = 0; i < (int)this.Bundles.Length; i++)
				{
					if (this.Bundles[i].Contains(path))
					{
						Dictionary<string, short> strs = this.pathsToFoundBundles;
						short num = (short)i;
						short num1 = num;
						index = num;
						strs[path] = num1;
						return true;
					}
				}
				this.pathsToFoundBundles[path] = -1;
				return false;
			}

			internal void Unload()
			{
				Bundling.LoadedBundle[] bundles = this.Bundles;
				for (int i = 0; i < (int)bundles.Length; i++)
				{
					bundles[i].Unload();
				}
			}
		}

		private class LoadedBundleListOfScenes
		{
			public readonly Bundling.LoadedBundle[] Bundles;

			public LoadedBundleListOfScenes(IEnumerable<Bundling.LoadedBundle> bundles)
			{
				if (!(bundles is List<Bundling.LoadedBundle>))
				{
					this.Bundles = (new List<Bundling.LoadedBundle>(bundles)).ToArray();
				}
				else
				{
					this.Bundles = ((List<Bundling.LoadedBundle>)bundles).ToArray();
				}
			}

			internal void Unload()
			{
				Bundling.LoadedBundle[] bundles = this.Bundles;
				for (int i = 0; i < (int)bundles.Length; i++)
				{
					bundles[i].Unload();
				}
			}
		}

		private class LoadedBundleMap : IDisposable
		{
			public readonly Bundling.LoadedBundleListOfScenes Scenes;

			public readonly Bundling.LoadedBundleAssetMap Assets;

			private bool disposed;

			public LoadedBundleMap(IEnumerable<KeyValuePair<Type, List<Bundling.LoadedBundle>>> assets, IEnumerable<Bundling.LoadedBundle> scenes)
			{
				this.Assets = new Bundling.LoadedBundleAssetMap(assets);
				this.Scenes = new Bundling.LoadedBundleListOfScenes(scenes);
				Bundling.Map = this;
				Bundling.HasLoadedBundleMap = true;
			}

			public void Dispose()
			{
				if (!this.disposed)
				{
					if (Bundling.Map == this)
					{
						Bundling.Map = null;
						Bundling.HasLoadedBundleMap = false;
					}
					this.disposed = true;
					this.Assets.Unload();
					this.Scenes.Unload();
				}
			}
		}

		public delegate void OnLoadedEventHandler();
	}
}