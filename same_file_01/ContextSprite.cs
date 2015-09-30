using Facepunch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[InterfaceDriverComponent(typeof(IContextRequestable), "_contextRequestable", "contextRequestable", AlwaysSaveDisabled=true, SearchRoute=InterfaceSearchRoute.GameObject | InterfaceSearchRoute.Parents, AdditionalProperties="renderer;meshFilter")]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ContextSprite : UnityEngine.MonoBehaviour
{
	private const double kFadeInRate = 8;

	private const double kFadeOutRate = 8;

	private const double kMinFade = 0;

	private const double kMaxFade = 1.2;

	private const double kGhostFade = 0.15;

	private const double kFadeDurationInFull = 0.15;

	private const double kFadeDurationOutFull = 0.15;

	private const float kRayDistance = 5f;

	private static bool gInit;

	private float timeRemoved;

	[HideInInspector]
	[SerializeField]
	private UnityEngine.MonoBehaviour _contextRequestable;

	private UnityEngine.MonoBehaviour contextRequestable;

	[PrefetchComponent]
	public MeshFilter meshFilter;

	[PrefetchComponent]
	public MeshRenderer renderer;

	private IContextRequestable requestable;

	private IContextRequestableVisibility requestableVisibility;

	private IContextRequestableStatus requestableStatus;

	private bool requestableIsVisibility;

	private bool requestableHasStatus;

	private bool selfVisible;

	private bool denied;

	private double fade;

	private MaterialPropertyBlock materialProperties;

	private float lastBoundFade = Single.NegativeInfinity;

	private readonly static ContextSprite.VisibleList visibleList;

	private static ContextSprite[] empty;

	public static ContextSprite.VisibleList AllVisible
	{
		get
		{
			return ContextSprite.visibleList;
		}
	}

	public static int layer
	{
		get
		{
			return ContextSprite.layerinfo.index;
		}
	}

	public static int layerMask
	{
		get
		{
			return ContextSprite.layerinfo.mask;
		}
	}

	static ContextSprite()
	{
		ContextSprite.visibleList = new ContextSprite.VisibleList();
		ContextSprite.empty = new ContextSprite[0];
	}

	public ContextSprite()
	{
	}

	public static IEnumerable<ContextSprite> AllVisibleForRequestable(IContextRequestableVisibility requestable)
	{
		if (ContextSprite.g.visible.Count != 0)
		{
			UnityEngine.MonoBehaviour monoBehaviour = requestable as UnityEngine.MonoBehaviour;
			UnityEngine.MonoBehaviour monoBehaviour1 = monoBehaviour;
			if (monoBehaviour)
			{
				return ContextSprite.AllVisibleForRequestable(monoBehaviour1);
			}
		}
		return ContextSprite.empty;
	}

	[DebuggerHidden]
	private static IEnumerable<ContextSprite> AllVisibleForRequestable(UnityEngine.MonoBehaviour requestable)
	{
		ContextSprite.<AllVisibleForRequestable>c__Iterator36 variable = null;
		return variable;
	}

	private void Awake()
	{
		this.contextRequestable = this._contextRequestable;
		if (this.contextRequestable)
		{
			this._contextRequestable = null;
		}
		else
		{
			if (!this.SearchForContextRequestable(out this.contextRequestable))
			{
				UnityEngine.Debug.LogError("Could not locate a IContextRequestable! -- destroying self.(component)", base.gameObject);
				UnityEngine.Object.Destroy(this);
				return;
			}
			UnityEngine.Debug.LogWarning("Please set the interface in inspector! had to search for it!", this.contextRequestable);
		}
		IContextRequestable contextRequestable = this.contextRequestable as IContextRequestable;
		IContextRequestable contextRequestable1 = contextRequestable;
		this.requestable = contextRequestable;
		if (contextRequestable1 == null)
		{
			UnityEngine.Debug.LogError("Context Requestable is not a IContextRequestable", base.gameObject);
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (!base.transform.IsChildOf(this.contextRequestable.transform))
		{
			UnityEngine.Debug.LogWarning(string.Format("Sprite for {0} is not a child of {0}.", this.contextRequestable), this);
		}
		this.requestableVisibility = this.contextRequestable as IContextRequestableVisibility;
		this.requestableIsVisibility = this.requestableVisibility != null;
		this.requestableStatus = this.contextRequestable as IContextRequestableStatus;
		this.requestableHasStatus = this.requestableStatus != null;
		MeshRenderer meshRenderer = this.renderer;
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		MaterialPropertyBlock materialPropertyBlock1 = materialPropertyBlock;
		this.materialProperties = materialPropertyBlock;
		meshRenderer.SetPropertyBlock(materialPropertyBlock1);
	}

	private static bool CalculateFadeDim(ref double fade, float elapsed)
	{
		if (fade < 0.15)
		{
			if (ContextSprite.CalculateFadeIn(ref fade, elapsed))
			{
				if (fade > 0.15)
				{
					fade = 0.15;
				}
				return true;
			}
		}
		else if (fade > 0.15 && ContextSprite.CalculateFadeOut(ref fade, elapsed))
		{
			if (fade < 0.15)
			{
				fade = 0.15;
			}
			return true;
		}
		return false;
	}

	private static bool CalculateFadeIn(ref double fade, float elapsed)
	{
		if ((double)elapsed <= 0)
		{
			return false;
		}
		if (fade > 1.2)
		{
			fade = 1.2;
			return true;
		}
		if (fade == 1.2)
		{
			return false;
		}
		double num = (double)elapsed / 0.15;
		if (1.2 - fade > num)
		{
			fade = fade + num;
		}
		else
		{
			fade = 1.2;
		}
		return true;
	}

	private static bool CalculateFadeOut(ref double fade, float elapsed)
	{
		if ((double)elapsed <= 0)
		{
			return false;
		}
		if (fade < 0)
		{
			fade = 0;
			return true;
		}
		if (fade == 0)
		{
			return false;
		}
		double num = (double)elapsed / 0.15;
		if (num < fade)
		{
			fade = fade - num;
		}
		else
		{
			fade = 0;
		}
		return true;
	}

	private static bool CheckRelation(Collider collider, Rigidbody rigidbody, Transform self)
	{
		if (collider.transform.IsChildOf(self) || self.IsChildOf(collider.transform))
		{
			return true;
		}
		Rigidbody rigidbody1 = rigidbody;
		if (rigidbody1 && collider.transform != rigidbody1.transform && (rigidbody1.transform.IsChildOf(self) || self.IsChildOf(rigidbody1.transform)))
		{
			return true;
		}
		return false;
	}

	public static bool FindSprite(Component component, out ContextSprite sprite)
	{
		if (component is ContextSprite)
		{
			sprite = (ContextSprite)component;
			return true;
		}
		if (!(component is IContextRequestable))
		{
			sprite = component.GetComponentInChildren<ContextSprite>();
			return sprite;
		}
		sprite = component.GetComponentInChildren<ContextSprite>();
		if (sprite)
		{
			if ((!sprite.contextRequestable ? sprite._contextRequestable : sprite.contextRequestable) == component)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsSeeThrough(ref RaycastHit hit)
	{
		Transform transforms;
		Transform transforms1 = base.transform;
		if (this.contextRequestable)
		{
			Transform transforms2 = this.contextRequestable.transform;
			if (transforms1 != transforms2)
			{
				if (transforms1.IsChildOf(transforms2))
				{
					transforms1 = transforms2;
				}
				else if (!transforms2.IsChildOf(transforms1))
				{
					transforms = hit.collider.transform;
					return (transforms == transforms2 || transforms == transforms1 || transforms.IsChildOf(transforms1) ? true : transforms.IsChildOf(transforms2));
				}
			}
		}
		transforms = hit.collider.transform;
		return (transforms == transforms1 ? true : transforms.IsChildOf(transforms1));
	}

	private void OnBecameInvisible()
	{
		if (this.selfVisible)
		{
			this.selfVisible = false;
			ContextSprite.g.Remove(this);
			if (this.requestableIsVisibility && this.contextRequestable)
			{
				this.requestableVisibility.OnContextVisibilityChanged(this, false);
			}
		}
		else if (this.denied)
		{
			this.denied = false;
		}
	}

	private void OnBecameVisible()
	{
		if (!this.selfVisible)
		{
			ContextSprite.g.Add(this);
			this.selfVisible = true;
			if (this.requestableIsVisibility && this.contextRequestable)
			{
				this.requestableVisibility.OnContextVisibilityChanged(this, true);
			}
		}
	}

	private void OnDestroy()
	{
		try
		{
			this.OnBecameInvisible();
		}
		finally
		{
			this.contextRequestable = null;
			this.requestable = null;
			this.requestableVisibility = null;
			this.requestableIsVisibility = false;
			this.requestableStatus = null;
			this.requestableHasStatus = false;
		}
	}

	public static bool Raycast(Ray ray, out ContextSprite sprite)
	{
		RaycastHit raycastHit;
		float single;
		bool flag = false;
		sprite = null;
		float single1 = Single.PositiveInfinity;
		foreach (ContextSprite contextSprite in ContextSprite.g.visible)
		{
			if (contextSprite.contextRequestable)
			{
				Collider collider = contextSprite.contextRequestable.collider;
				if (!collider)
				{
					collider = contextSprite.collider;
				}
				if (collider)
				{
					if (!collider.enabled)
					{
						continue;
					}
					else if (contextSprite.collider.Raycast(ray, out raycastHit, 5f))
					{
						float single2 = raycastHit.distance;
						single2 = single2 * single2;
						if (single2 < single1)
						{
							flag = true;
							single1 = single2;
							sprite = contextSprite;
						}
					}
				}
				if (!contextSprite.renderer.bounds.IntersectRay(ray, out single))
				{
					continue;
				}
				single = single * single;
				if (single >= single1)
				{
					continue;
				}
				flag = true;
				single1 = single;
				sprite = contextSprite;
			}
		}
		return flag;
	}

	private void Reset()
	{
		if (!this.renderer)
		{
			this.renderer = base.renderer as MeshRenderer;
		}
		if (!this.meshFilter)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
		if (!this._contextRequestable && !this.SearchForContextRequestable(out this._contextRequestable))
		{
			UnityEngine.Debug.LogWarning("Please add a script implementing IContextRequestable on this or a parent game object", this);
		}
	}

	[DebuggerHidden]
	private IEnumerator Retry()
	{
		ContextSprite.<Retry>c__Iterator35 variable = null;
		return variable;
	}

	private bool SearchForContextRequestable(out UnityEngine.MonoBehaviour impl)
	{
		Contextual contextual;
		if (Contextual.FindUp(base.transform, out contextual))
		{
			Facepunch.MonoBehaviour monoBehaviour = contextual.implementor;
			UnityEngine.MonoBehaviour monoBehaviour1 = monoBehaviour;
			impl = monoBehaviour;
			if (monoBehaviour1)
			{
				return true;
			}
		}
		impl = null;
		return false;
	}

	private void UpdateMaterialProperties()
	{
		float single = Mathf.Clamp01((float)this.fade);
		if (single != this.lastBoundFade)
		{
			this.materialProperties.Clear();
			this.materialProperties.AddFloat(ContextSprite.matHelper.fadeProp, single);
			this.lastBoundFade = single;
			this.renderer.SetPropertyBlock(this.materialProperties);
		}
	}

	public static void UpdateSpriteFading(Camera camera)
	{
		if (ContextSprite.gInit && camera)
		{
			ContextSprite.g.Step(camera);
		}
	}

	private static class g
	{
		private const int kMaxRecycleCount = 5;

		public static HashSet<ContextSprite> visible;

		public static Queue<HashSet<ContextSprite>> hashRecycle;

		public static Dictionary<UnityEngine.MonoBehaviour, HashSet<ContextSprite>> requestableVisibleSprites;

		private static int count;

		static g()
		{
			ContextSprite.g.visible = new HashSet<ContextSprite>();
			ContextSprite.g.hashRecycle = new Queue<HashSet<ContextSprite>>();
			ContextSprite.g.requestableVisibleSprites = new Dictionary<UnityEngine.MonoBehaviour, HashSet<ContextSprite>>();
			ContextSprite.gInit = true;
		}

		public static void Add(ContextSprite sprite)
		{
			HashSet<ContextSprite> contextSprites;
			ContextSprite.g.visible.Add(sprite);
			ContextSprite.g.count = ContextSprite.g.count + 1;
			if (!ContextSprite.g.requestableVisibleSprites.TryGetValue(sprite.contextRequestable, out contextSprites))
			{
				contextSprites = (ContextSprite.g.hashRecycle.Count <= 0 ? new HashSet<ContextSprite>() : ContextSprite.g.hashRecycle.Dequeue());
				ContextSprite.g.requestableVisibleSprites[sprite.contextRequestable] = contextSprites;
			}
			contextSprites.Add(sprite);
			if (ContextSprite.CalculateFadeOut(ref sprite.fade, Time.time - sprite.timeRemoved))
			{
				sprite.UpdateMaterialProperties();
			}
		}

		private static bool PhysRaycast(ref Ray ray, out RaycastHit hit, float distanceTo, int layerMask)
		{
			if (!Physics.Raycast(ray, out hit, distanceTo, layerMask))
			{
				return false;
			}
			UnityEngine.Debug.DrawLine(ray.origin, ray.GetPoint(hit.distance), Color.green);
			UnityEngine.Debug.DrawLine(ray.GetPoint(hit.distance), ray.GetPoint(distanceTo), Color.red);
			return true;
		}

		public static void Remove(ContextSprite sprite)
		{
			HashSet<ContextSprite> contextSprites;
			ContextSprite.g.visible.Remove(sprite);
			ContextSprite.g.count = ContextSprite.g.count - 1;
			if (ContextSprite.g.requestableVisibleSprites.TryGetValue(sprite.contextRequestable, out contextSprites))
			{
				if (contextSprites.Count != 1)
				{
					contextSprites.Remove(sprite);
				}
				else
				{
					contextSprites.Clear();
					if (ContextSprite.g.hashRecycle.Count < 5)
					{
						ContextSprite.g.hashRecycle.Enqueue(contextSprites);
					}
					ContextSprite.g.requestableVisibleSprites.Remove(sprite.contextRequestable);
				}
			}
			sprite.timeRemoved = Time.time;
		}

		public static void Step(Camera camera)
		{
			float single;
			RaycastHit raycastHit;
			bool flag;
			ContextSprite contextSprite = null;
			ContextStatusFlags contextStatusFlag;
			bool flag1;
			if (ContextSprite.g.count > 0)
			{
				single = Time.deltaTime;
				if (single <= 0f)
				{
					return;
				}
				int num = 525313;
				if (!RPOS.hideSprites)
				{
				Label2:
					foreach (ContextSprite contextSprite in ContextSprite.g.visible)
					{
						if (!contextSprite.requestableHasStatus)
						{
							flag = false;
						}
						else
						{
							contextStatusFlag = contextSprite.requestableStatus.ContextStatusPoll() & (ContextStatusFlags.SpriteFlag0 | ContextStatusFlags.SpriteFlag1);
							if ((int)contextStatusFlag != 0)
							{
								if (contextStatusFlag != ContextStatusFlags.SpriteFlag0)
								{
									goto Label1;
								}
								flag = true;
								goto Label0;
							}
						Label3:
							flag = false;
						Label0:
						}
						Vector3 vector3 = contextSprite.transform.position;
						Ray ray = camera.ScreenPointToRay(camera.WorldToScreenPoint(vector3));
						Vector3 vector31 = ray.direction;
						Vector3 vector32 = ray.origin;
						float single1 = vector3.x * vector31.x + vector3.y * vector31.y + vector3.z * vector31.z - (vector32.x * vector31.x + vector32.y * vector31.y + vector32.z * vector31.z);
						if (single1 <= 0f || ContextSprite.g.PhysRaycast(ref ray, out raycastHit, single1, num) && !contextSprite.IsSeeThrough(ref raycastHit))
						{
							flag1 = ContextSprite.CalculateFadeOut(ref contextSprite.fade, single);
						}
						else
						{
							flag1 = (!flag ? ContextSprite.CalculateFadeIn(ref contextSprite.fade, single) : ContextSprite.CalculateFadeDim(ref contextSprite.fade, single));
						}
						if (!flag1)
						{
							continue;
						}
						contextSprite.UpdateMaterialProperties();
					}
				}
				else
				{
					foreach (ContextSprite contextSprite1 in ContextSprite.g.visible)
					{
						if (!ContextSprite.CalculateFadeOut(ref contextSprite1.fade, single))
						{
							continue;
						}
						contextSprite1.UpdateMaterialProperties();
					}
				}
			}
			return;
			if (contextStatusFlag == ContextStatusFlags.SpriteFlag1)
			{
				if (ContextSprite.CalculateFadeOut(ref contextSprite.fade, single))
				{
					contextSprite.UpdateMaterialProperties();
				}
				goto Label2;
			}
			else if (contextStatusFlag == (ContextStatusFlags.SpriteFlag0 | ContextStatusFlags.SpriteFlag1))
			{
				if (ContextSprite.CalculateFadeIn(ref contextSprite.fade, single))
				{
					contextSprite.UpdateMaterialProperties();
				}
				goto Label2;
			}
			else
			{
				goto Label3;
			}
		}
	}

	private static class layerinfo
	{
		public readonly static int index;

		public readonly static int mask;

		static layerinfo()
		{
			ContextSprite.layerinfo.index = LayerMask.NameToLayer("Sprite");
			ContextSprite.layerinfo.mask = 1 << (ContextSprite.layerinfo.index & 31);
		}
	}

	private static class matHelper
	{
		public static int fadeProp;

		static matHelper()
		{
			ContextSprite.matHelper.fadeProp = Shader.PropertyToID("_Fade");
		}
	}

	private static class r
	{
		public static WaitForEndOfFrame wait;

		static r()
		{
			ContextSprite.r.wait = new WaitForEndOfFrame();
		}
	}

	public sealed class VisibleList : IEnumerable, IEnumerable<ContextSprite>
	{
		public int Count
		{
			get
			{
				return ContextSprite.g.visible.Count;
			}
		}

		internal VisibleList()
		{
		}

		public bool Contains(ContextSprite sprite)
		{
			return (!sprite || !sprite.selfVisible ? false : ContextSprite.g.visible.Contains(sprite));
		}

		public HashSet<ContextSprite>.Enumerator GetEnumerator()
		{
			return ContextSprite.g.visible.GetEnumerator();
		}

		IEnumerator<ContextSprite> System.Collections.Generic.IEnumerable<ContextSprite>.GetEnumerator()
		{
			return ((IEnumerable<ContextSprite>)ContextSprite.g.visible).GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)ContextSprite.g.visible).GetEnumerator();
		}
	}
}