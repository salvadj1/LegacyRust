using Facepunch.Progress;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class SlowSpawn : ThrottledTask, IProgress
{
	[SerializeField]
	private string findSequence = "_decor_";

	[SerializeField]
	private Mesh[] meshes;

	[SerializeField]
	private Material sharedMaterial;

	[SerializeField]
	private SlowSpawn.SpawnFlags runtimeLoad = SlowSpawn.SpawnFlags.Collider;

	[HideInInspector]
	[SerializeField]
	private int[] meshIndex;

	[HideInInspector]
	[SerializeField]
	private Vector4[] ps;

	[HideInInspector]
	[SerializeField]
	private Quaternion[] r;

	[NonSerialized]
	private int iter = -1;

	[NonSerialized]
	private int iter_end;

	public int Count
	{
		get
		{
			return (this.ps != null ? (int)this.ps.Length : 0);
		}
	}

	public int CountSpawned
	{
		get
		{
			int num;
			if (!base.Working)
			{
				num = (this.iter != -1 ? this.Count : 0);
			}
			else
			{
				num = this.iter;
			}
			return num;
		}
	}

	public SlowSpawn.InstanceParameters this[int i]
	{
		get
		{
			return new SlowSpawn.InstanceParameters(this, i);
		}
	}

	public float progress
	{
		get
		{
			float count;
			if (!base.Working)
			{
				count = (this.iter != -1 || !base.enabled ? 1f : 0f);
			}
			else
			{
				count = (float)((double)this.iter / (double)this.Count);
			}
			return count;
		}
	}

	public SlowSpawn()
	{
	}

	[DebuggerHidden]
	public IEnumerable<GameObject> SpawnAll(SlowSpawn.SpawnFlags SpawnFlags = 7, UnityEngine.HideFlags HideFlags = 9)
	{
		SlowSpawn.<SpawnAll>c__Iterator3A variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		SlowSpawn.<Start>c__Iterator39 variable = null;
		return variable;
	}

	public struct InstanceParameters
	{
		public const SlowSpawn.SpawnFlags DefaultSpawnFlags = SlowSpawn.SpawnFlags.All;

		public const HideFlags DefaultHideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;

		public readonly Vector3 Position;

		public readonly Vector3 Scale;

		public readonly Quaternion Rotation;

		public readonly Mesh Mesh;

		public readonly Material SharedMaterial;

		public readonly int Layer;

		public readonly int Index;

		public InstanceParameters(SlowSpawn SlowSpawn, int Index)
		{
			this.Index = Index;
			this.Layer = SlowSpawn.gameObject.layer;
			Vector4 slowSpawn = SlowSpawn.ps[Index];
			this.Position.x = slowSpawn.x;
			this.Position.y = slowSpawn.y;
			this.Position.z = slowSpawn.z;
			float single = slowSpawn.w;
			float single1 = single;
			this.Scale.z = single;
			float single2 = single1;
			single1 = single2;
			this.Scale.y = single2;
			this.Scale.x = single1;
			this.Rotation = SlowSpawn.r[Index];
			this.Mesh = SlowSpawn.meshes[SlowSpawn.meshIndex[Index]];
			this.SharedMaterial = SlowSpawn.sharedMaterial;
		}

		public MeshCollider AddCollider(GameObject go)
		{
			MeshCollider mesh = go.AddComponent<MeshCollider>();
			mesh.sharedMesh = this.Mesh;
			return mesh;
		}

		public MeshFilter AddMeshFilter(GameObject go)
		{
			MeshFilter mesh = go.AddComponent<MeshFilter>();
			mesh.sharedMesh = this.Mesh;
			return mesh;
		}

		public MeshRenderer AddRenderer(GameObject go)
		{
			MeshRenderer sharedMaterial = go.AddComponent<MeshRenderer>();
			sharedMaterial.sharedMaterial = this.SharedMaterial;
			return sharedMaterial;
		}

		private SlowSpawn.SpawnFlags AddTo(GameObject go, SlowSpawn.SpawnFlags spawnFlags, bool safe)
		{
			SlowSpawn.SpawnFlags spawnFlag = (SlowSpawn.SpawnFlags)0;
			if ((spawnFlags & SlowSpawn.SpawnFlags.MeshFilter) == SlowSpawn.SpawnFlags.MeshFilter && (safe || !go.GetComponent<MeshFilter>()))
			{
				spawnFlag = spawnFlag | SlowSpawn.SpawnFlags.MeshFilter;
				this.AddMeshFilter(go);
			}
			if ((spawnFlags & SlowSpawn.SpawnFlags.Renderer) == SlowSpawn.SpawnFlags.Renderer && (safe || !go.renderer))
			{
				spawnFlag = spawnFlag | SlowSpawn.SpawnFlags.Renderer;
				this.AddRenderer(go);
			}
			if ((spawnFlags & SlowSpawn.SpawnFlags.Collider) == SlowSpawn.SpawnFlags.Collider && (safe || !go.collider))
			{
				spawnFlag = spawnFlag | SlowSpawn.SpawnFlags.Collider;
				this.AddCollider(go);
			}
			return spawnFlag;
		}

		public SlowSpawn.SpawnFlags AddTo(GameObject go, SlowSpawn.SpawnFlags spawnFlags = 7)
		{
			return this.AddTo(go, spawnFlags, false);
		}

		public GameObject Spawn(SlowSpawn.SpawnFlags spawnFlags = 7, UnityEngine.HideFlags HideFlags = 9)
		{
			GameObject gameObject = new GameObject(string.Empty)
			{
				hideFlags = HideFlags,
				layer = this.Layer
			};
			gameObject.transform.position = this.Position;
			gameObject.transform.rotation = this.Rotation;
			GameObject gameObject1 = gameObject;
			this.AddTo(gameObject1, spawnFlags, true);
			return gameObject1;
		}
	}

	[Flags]
	public enum SpawnFlags
	{
		Collider = 1,
		Renderer = 2,
		MeshFilter = 4,
		Graphics = 6,
		All = 7
	}
}