using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraMount : MonoBehaviour
{
	private const string kTempMountGOName = "__-temp mount-__";

	[PrefetchComponent]
	public CameraFX cameraFX;

	[PrefetchComponent]
	public Camera camera;

	public KindOfCamera kindOfCamera;

	public SharedCameraMode cameraMode;

	private static CameraMount top;

	[SerializeField]
	private int importance;

	[SerializeField]
	private bool autoBind;

	[NonSerialized]
	private bool awoke;

	[NonSerialized]
	private bool bound;

	[NonSerialized]
	private bool destroyed;

	private static CameraMount temporaryCameraMount;

	private static CameraMount temporaryCameraMountSource;

	private static GameObject temporaryCameraMountGameObject;

	private static Transform temporaryCameraMountParent;

	private static bool temporaryCameraMountHasParent;

	private static bool creatingTemporaryCameraMount;

	private static bool createdTemporaryCameraMount;

	public static CameraMount current
	{
		get
		{
			return CameraMount.top;
		}
	}

	[Obsolete("use the open property instead!", true)]
	public new bool enabled
	{
		get
		{
			return this.open;
		}
		set
		{
			this.open = value;
		}
	}

	public bool isActiveMount
	{
		get
		{
			return CameraMount.top == this;
		}
	}

	public bool open
	{
		get
		{
			return (!this.awoke ? this.autoBind : this.bound);
		}
		set
		{
			if (!this.destroyed)
			{
				if (!this.awoke)
				{
					this.autoBind = value;
				}
				else if (this.bound != value)
				{
					if (!this.bound)
					{
						this.Bind();
					}
					else
					{
						this.UnBind();
					}
				}
			}
		}
	}

	private static Stack<CameraMount> queue
	{
		get
		{
			return CameraMount.QUEUE_LATE.queue;
		}
	}

	public CameraMount()
	{
	}

	private void Awake()
	{
		this.awoke = true;
		if (!this.camera)
		{
			this.camera = base.camera;
		}
		this.camera.enabled = false;
		if (CameraMount.creatingTemporaryCameraMount)
		{
			CameraMount.temporaryCameraMount = this;
			if (CameraMount.temporaryCameraMountHasParent)
			{
				Transform transforms = base.transform;
				transforms.parent = CameraMount.temporaryCameraMountParent;
				Transform transforms1 = CameraMount.temporaryCameraMountSource.transform;
				transforms.localPosition = transforms1.localPosition;
				transforms.localRotation = transforms1.localRotation;
				transforms.localScale = transforms1.localScale;
			}
			this.camera.CopyFrom(CameraMount.temporaryCameraMountSource.camera);
			this.cameraFX = CameraMount.temporaryCameraMountSource.cameraFX;
			if (CameraMount.temporaryCameraMountSource.open)
			{
				this.Bind();
			}
		}
		else if (this.autoBind)
		{
			this.Bind();
		}
	}

	private void Bind()
	{
		if (!CameraMount.top)
		{
			CameraMount.top = this;
			CameraMount.SetMountActive();
		}
		else if (CameraMount.top.importance < this.importance)
		{
			CameraMount.SetMountInactive();
			CameraMount.queue.Push(CameraMount.top);
			CameraMount.top = this;
			CameraMount.SetMountActive();
		}
		else if (CameraMount.queue.Count == 0 || CameraMount.queue.Peek().importance <= this.importance)
		{
			CameraMount.queue.Push(this);
		}
		else
		{
			CameraMount.SORT_QUEUE(this);
		}
		this.bound = true;
	}

	public static void ClearTemporaryCameraMount()
	{
		if (CameraMount.createdTemporaryCameraMount && CameraMount.temporaryCameraMount)
		{
			UnityEngine.Object.Destroy(CameraMount.temporaryCameraMount);
			UnityEngine.Object.Destroy(CameraMount.temporaryCameraMountGameObject);
			CameraMount.createdTemporaryCameraMount = false;
		}
	}

	public static CameraMount CreateTemporaryCameraMount(CameraMount copyFrom)
	{
		return CameraMount.CreateTemporaryCameraMount(copyFrom, null, false);
	}

	public static CameraMount CreateTemporaryCameraMount(CameraMount copyFrom, Transform parent)
	{
		return CameraMount.CreateTemporaryCameraMount(copyFrom, parent, parent);
	}

	private static CameraMount CreateTemporaryCameraMount(CameraMount copyFrom, Transform parent, bool hasParent)
	{
		if (CameraMount.creatingTemporaryCameraMount)
		{
			throw new InvalidOperationException("Invalid/unexpected call stack recursion");
		}
		CameraMount.ClearTemporaryCameraMount();
		try
		{
			CameraMount.creatingTemporaryCameraMount = true;
			CameraMount.temporaryCameraMountSource = copyFrom;
			CameraMount.temporaryCameraMountHasParent = hasParent;
			CameraMount.temporaryCameraMountParent = parent;
			GameObject gameObject = new GameObject("__-temp mount-__", new Type[] { typeof(CameraMount) })
			{
				hideFlags = HideFlags.DontSave
			};
			CameraMount.temporaryCameraMountGameObject = gameObject;
		}
		finally
		{
			CameraMount.creatingTemporaryCameraMount = false;
			CameraMount.temporaryCameraMountSource = null;
			CameraMount.temporaryCameraMountHasParent = false;
			CameraMount.temporaryCameraMountParent = null;
			CameraMount.createdTemporaryCameraMount = CameraMount.temporaryCameraMount;
		}
		return CameraMount.temporaryCameraMount;
	}

	public void EnableTransition(float duration, TransitionFunction function)
	{
		if (!this.open)
		{
			this.open = true;
			if (this.isActiveMount)
			{
				CameraFX.TransitionNow(duration, function);
			}
		}
	}

	public void EnableTransitionSpeed(float metersPerSecond, TransitionFunction function)
	{
		Vector3 vector3;
		if (!this.open)
		{
			this.open = true;
			if (this.isActiveMount && MountedCamera.GetPoint(out vector3))
			{
				Matrix4x4 matrix4x4 = this.camera.worldToCameraMatrix;
				float single = Vector3.Distance(matrix4x4.MultiplyPoint(Vector3.zero), vector3);
				if (single != 0f)
				{
					CameraFX.TransitionNow(single / metersPerSecond, function);
				}
			}
		}
	}

	private void OnDestroy()
	{
		this.destroyed = true;
		if (this.bound)
		{
			this.UnBind();
		}
	}

	private void OnNotActiveMount()
	{
	}

	public void OnPostMount(MountedCamera camera)
	{
	}

	public void OnPreMount(MountedCamera camera)
	{
	}

	private void OnSetActiveMount()
	{
	}

	private static void REMOVE_FROM_QUEUE(CameraMount rem)
	{
		try
		{
			int count = CameraMount.queue.Count;
			for (int i = 0; i < count; i++)
			{
				CameraMount cameraMount = CameraMount.queue.Pop();
				if (cameraMount != rem)
				{
					CameraMount.WORK_LATE.list.Add(cameraMount);
				}
			}
			CameraMount.WORK_LATE.list.Reverse();
			foreach (CameraMount cameraMount1 in CameraMount.WORK_LATE.list)
			{
				CameraMount.queue.Push(cameraMount1);
			}
		}
		finally
		{
			CameraMount.WORK_LATE.list.Clear();
		}
	}

	private static void SetMountActive()
	{
		try
		{
			CameraMount.top.OnSetActiveMount();
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, CameraMount.top);
		}
	}

	private static void SetMountInactive()
	{
		try
		{
			CameraMount.top.OnNotActiveMount();
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, CameraMount.top);
		}
	}

	private static void SORT_QUEUE(CameraMount addExtra)
	{
		CameraMount.WORK_LATE.list.Add(addExtra);
		CameraMount.SORT_QUEUE();
	}

	private static void SORT_QUEUE()
	{
		CameraMount.WORK_LATE.list.AddRange(CameraMount.queue);
		try
		{
			CameraMount.WORK_LATE.list.Sort((CameraMount a, CameraMount b) => a.importance.CompareTo(b.importance));
			CameraMount.queue.Clear();
			foreach (CameraMount cameraMount in CameraMount.WORK_LATE.list)
			{
				CameraMount.queue.Push(cameraMount);
			}
		}
		finally
		{
			CameraMount.WORK_LATE.list.Clear();
		}
	}

	private void UnBind()
	{
		if (CameraMount.top == this)
		{
			CameraMount.SetMountInactive();
			if (CameraMount.queue.Count <= 0)
			{
				CameraMount.top = null;
			}
			else
			{
				CameraMount.top = CameraMount.queue.Pop();
				CameraMount.SetMountActive();
			}
		}
		else if (CameraMount.queue.Count <= 1)
		{
			CameraMount.queue.Pop();
		}
		else if (CameraMount.queue.Peek() != this)
		{
			CameraMount.REMOVE_FROM_QUEUE(this);
		}
		else
		{
			CameraMount.queue.Pop();
		}
		this.bound = false;
	}

	private static class QUEUE_LATE
	{
		public readonly static Stack<CameraMount> queue;

		static QUEUE_LATE()
		{
			CameraMount.QUEUE_LATE.queue = new Stack<CameraMount>();
		}
	}

	private static class WORK_LATE
	{
		public readonly static List<CameraMount> list;

		static WORK_LATE()
		{
			CameraMount.WORK_LATE.list = new List<CameraMount>();
		}
	}
}