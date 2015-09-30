using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Update Manager")]
[ExecuteInEditMode]
public class UpdateManager : MonoBehaviour
{
	private static UpdateManager mInst;

	private List<UpdateManager.UpdateEntry> mOnUpdate = new List<UpdateManager.UpdateEntry>();

	private List<UpdateManager.UpdateEntry> mOnLate = new List<UpdateManager.UpdateEntry>();

	private List<UpdateManager.UpdateEntry> mOnCoro = new List<UpdateManager.UpdateEntry>();

	private List<UpdateManager.DestroyEntry> mDest = new List<UpdateManager.DestroyEntry>();

	private float mTime;

	public UpdateManager()
	{
	}

	private void Add(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func, List<UpdateManager.UpdateEntry> list)
	{
		int num = 0;
		int count = list.Count;
		while (num < count)
		{
			if (list[num].func == func)
			{
				return;
			}
			num++;
		}
		UpdateManager.UpdateEntry updateEntry = new UpdateManager.UpdateEntry()
		{
			index = updateOrder,
			func = func,
			mb = mb,
			isMonoBehaviour = mb != null
		};
		list.Add(updateEntry);
		if (updateOrder != 0)
		{
			list.Sort(new Comparison<UpdateManager.UpdateEntry>(UpdateManager.Compare));
		}
	}

	public static void AddCoroutine(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
	{
		UpdateManager.CreateInstance();
		UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnCoro);
	}

	public static void AddDestroy(UnityEngine.Object obj, float delay)
	{
		if (obj == null)
		{
			return;
		}
		if (!Application.isPlaying)
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		else if (delay <= 0f)
		{
			UnityEngine.Object.Destroy(obj);
		}
		else
		{
			UpdateManager.CreateInstance();
			UpdateManager.DestroyEntry destroyEntry = new UpdateManager.DestroyEntry()
			{
				obj = obj,
				time = Time.realtimeSinceStartup + delay
			};
			UpdateManager.mInst.mDest.Add(destroyEntry);
		}
	}

	public static void AddLateUpdate(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
	{
		UpdateManager.CreateInstance();
		UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnLate);
	}

	public static void AddUpdate(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
	{
		UpdateManager.CreateInstance();
		UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnUpdate);
	}

	private static int Compare(UpdateManager.UpdateEntry a, UpdateManager.UpdateEntry b)
	{
		if (a.index < b.index)
		{
			return 1;
		}
		if (a.index > b.index)
		{
			return 1;
		}
		return 0;
	}

	[DebuggerHidden]
	private IEnumerator CoroutineFunction()
	{
		UpdateManager.<CoroutineFunction>c__Iterator48 variable = null;
		return variable;
	}

	private bool CoroutineUpdate()
	{
		float single = Time.realtimeSinceStartup;
		float single1 = single - this.mTime;
		if (single1 < 0.001f)
		{
			return true;
		}
		this.mTime = single;
		this.UpdateList(this.mOnCoro, single1);
		bool flag = Application.isPlaying;
		int count = this.mDest.Count;
		while (count > 0)
		{
			int num = count - 1;
			count = num;
			UpdateManager.DestroyEntry item = this.mDest[num];
			if (flag && item.time >= this.mTime)
			{
				continue;
			}
			if (item.obj != null)
			{
				NGUITools.Destroy(item.obj);
				item.obj = null;
			}
			this.mDest.RemoveAt(count);
		}
		if (this.mOnUpdate.Count != 0 || this.mOnLate.Count != 0 || this.mOnCoro.Count != 0 || this.mDest.Count != 0)
		{
			return true;
		}
		NGUITools.Destroy(base.gameObject);
		return false;
	}

	private static void CreateInstance()
	{
		if (UpdateManager.mInst == null)
		{
			UpdateManager.mInst = UnityEngine.Object.FindObjectOfType(typeof(UpdateManager)) as UpdateManager;
			if (UpdateManager.mInst == null && Application.isPlaying)
			{
				GameObject gameObject = new GameObject("_UpdateManager");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				UpdateManager.mInst = gameObject.AddComponent<UpdateManager>();
			}
		}
	}

	private void LateUpdate()
	{
		this.UpdateList(this.mOnLate, Time.deltaTime);
		if (!Application.isPlaying)
		{
			this.CoroutineUpdate();
		}
	}

	private void OnApplicationQuit()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			this.mTime = Time.realtimeSinceStartup;
			base.StartCoroutine(this.CoroutineFunction());
		}
	}

	private void Update()
	{
		if (UpdateManager.mInst == this)
		{
			this.UpdateList(this.mOnUpdate, Time.deltaTime);
		}
		else
		{
			NGUITools.Destroy(base.gameObject);
		}
	}

	private void UpdateList(List<UpdateManager.UpdateEntry> list, float delta)
	{
		int count = list.Count;
		while (count > 0)
		{
			int num = count - 1;
			count = num;
			UpdateManager.UpdateEntry item = list[num];
			if (item.isMonoBehaviour)
			{
				if (item.mb == null)
				{
					list.RemoveAt(count);
					continue;
				}
				else if (!item.mb.enabled || !item.mb.gameObject.activeInHierarchy)
				{
					continue;
				}
			}
			item.func(delta);
		}
	}

	public class DestroyEntry
	{
		public UnityEngine.Object obj;

		public float time;

		public DestroyEntry()
		{
		}
	}

	public delegate void OnUpdate(float delta);

	public class UpdateEntry
	{
		public int index;

		public UpdateManager.OnUpdate func;

		public MonoBehaviour mb;

		public bool isMonoBehaviour;

		public UpdateEntry()
		{
		}
	}
}