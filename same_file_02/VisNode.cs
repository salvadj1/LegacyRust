using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("Vis/Node")]
public class VisNode : IDLocal
{
	private const int defaultUnobstructedLayers = 1;

	[PrefetchComponent]
	[SerializeField]
	private VisReactor reactor;

	[SerializeField]
	private float dotArc = 0.75f;

	[SerializeField]
	private float distance = 10f;

	[SerializeField]
	private float dotArcBegin;

	[HideInInspector]
	[SerializeField]
	private int _sightMask = -1;

	[HideInInspector]
	[SerializeField]
	private int _spectMask = -1;

	[HideInInspector]
	[SerializeField]
	private int _traitMask = 16777217;

	[NonSerialized]
	private int _sightCurrentMask;

	[NonSerialized]
	private int _seeMask;

	[NonSerialized]
	private bool anySeenTraitChanges;

	[NonSerialized]
	private bool hasStatusHandler;

	[NonSerialized]
	private bool __skipOnce_;

	[NonSerialized]
	private bool awake;

	[NonSerialized]
	private bool active;

	[NonSerialized]
	private bool dataConstructed;

	public bool blind;

	public bool deaf;

	public bool mute;

	[SerializeField]
	private VisClass _class;

	[NonSerialized]
	private VisClass.Handle _handle;

	private long queriesBitMask;

	private IVisHandler statusHandler;

	[NonSerialized]
	private VisNode.TraitHistory histSight;

	[NonSerialized]
	private VisNode.TraitHistory histSpect;

	[NonSerialized]
	private VisNode.TraitHistory histTrait;

	[NonSerialized]
	private VisNode.TraitHistory histSeen;

	private VisNode.VisMem spect;

	private VisNode.VisMem sight;

	private ODBSet<VisNode> enter;

	private ODBSet<VisNode> exit;

	internal ODBItem<VisNode> item;

	private List<VisNode> cleanList;

	[HideInInspector]
	[NonSerialized]
	private Transform _transform;

	[NonSerialized]
	private Vis.Stamp _stamp;

	private static ObjectDB<VisNode> db;

	private static VisManager manager;

	private static ODBSet<VisNode> recentlyDisabled;

	private static ODBSet<VisNode> disabledLastStep;

	private static VisNode operandA;

	private static VisNode operandB;

	private static float pX;

	private static float pY;

	private static float pZ;

	private static float bX;

	private static float bY;

	private static float bZ;

	private static float fX;

	private static float fY;

	private static float fZ;

	private static float fW;

	private static float dX;

	private static float dY;

	private static float dZ;

	private static float nX;

	private static float nY;

	private static float nZ;

	private static float dV;

	private static float dV2;

	private static float dot;

	private static float planeDot;

	private static float SIGHT;

	private static float PLANEDOTSIGHT;

	private static float SIGHT2;

	private static float DOT;

	private static bool FALLBACK_TOO_CLOSE;

	private static int temp_bTraits;

	internal VisReactor __reactor
	{
		set
		{
			this.reactor = value;
		}
	}

	public bool anySight
	{
		get
		{
			return this.sight.any;
		}
	}

	public bool anySightHad
	{
		get
		{
			return this.sight.had;
		}
	}

	public bool anySightLost
	{
		get
		{
			return this.sight.rem;
		}
	}

	public bool anySightNew
	{
		get
		{
			return this.sight.@add;
		}
	}

	public bool anySpectators
	{
		get
		{
			return this.spect.any;
		}
	}

	public bool anySpectatorsHad
	{
		get
		{
			return this.spect.had;
		}
	}

	public bool anySpectatorsLost
	{
		get
		{
			return this.spect.rem;
		}
	}

	public bool anySpectatorsNew
	{
		get
		{
			return this.spect.@add;
		}
	}

	public float arc
	{
		get
		{
			return this.dotArc;
		}
		set
		{
			this.dotArc = Mathf.Clamp01(value);
		}
	}

	public Vector3 forward
	{
		get
		{
			return this._stamp.forward;
		}
	}

	public Transform head
	{
		get
		{
			return this._transform;
		}
		set
		{
			if (!value)
			{
				this._transform = base.transform;
			}
			else
			{
				this._transform = value;
			}
		}
	}

	public int numSight
	{
		get
		{
			return this.sight.count;
		}
	}

	public int numSpectators
	{
		get
		{
			return this.spect.count;
		}
	}

	public Plane plane
	{
		get
		{
			Vector4 vector4 = this._stamp.forward;
			return new Plane(new Vector3(vector4.x, vector4.y, vector4.z), vector4.w);
		}
	}

	public Vector3 position
	{
		get
		{
			return this._stamp.position;
		}
	}

	public float radius
	{
		get
		{
			return this.distance;
		}
		set
		{
			this.distance = value;
		}
	}

	public Quaternion rotation
	{
		get
		{
			return this._stamp.rotation;
		}
	}

	public Vis.Mask seenMask
	{
		get
		{
			return new Vis.Mask()
			{
				data = this._seeMask
			};
		}
	}

	public Vis.Mask spectMask
	{
		get
		{
			return new Vis.Mask()
			{
				data = this._spectMask
			};
		}
		set
		{
			this._spectMask = value.data;
		}
	}

	public Vis.Stamp stamp
	{
		get
		{
			return this._stamp;
		}
	}

	public Vis.Mask traitMask
	{
		get
		{
			return new Vis.Mask()
			{
				data = this._traitMask
			};
		}
		set
		{
			this._traitMask = value.data;
		}
	}

	public Vis.Mask viewMask
	{
		get
		{
			return new Vis.Mask()
			{
				data = this._sightMask
			};
		}
		set
		{
			this._sightMask = value.data;
		}
	}

	static VisNode()
	{
		VisNode.db = new ObjectDB<VisNode>();
		VisNode.recentlyDisabled = new ODBSet<VisNode>();
		VisNode.disabledLastStep = new ODBSet<VisNode>();
		VisNode.FALLBACK_TOO_CLOSE = false;
	}

	public VisNode()
	{
	}

	private bool _CanSee(VisNode other)
	{
		return (other.spect.count >= this.sight.count ? this.sight.list.Contains(other) : other.spect.list.Contains(this));
	}

	protected void _CB_OnHidden_()
	{
		if (this.reactor)
		{
			this.reactor.SPECTATED_EXIT();
		}
	}

	protected void _CB_OnHiddenFrom_(VisNode spectator)
	{
		if (this.reactor)
		{
			this.reactor.SPECTATOR_REMOVE(spectator);
		}
	}

	protected void _CB_OnSeen_()
	{
		if (this.reactor)
		{
			this.reactor.SPECTATED_ENTER();
		}
	}

	protected void _CB_OnSeenBy_(VisNode spectator)
	{
		if (this.reactor)
		{
			this.reactor.SPECTATOR_ADD(spectator);
		}
	}

	private bool _IsSeenBy(VisNode other)
	{
		return (other.sight.count >= this.spect.count ? this.spect.list.Contains(other) : other.sight.list.Contains(this));
	}

	private void _REACTOR_SEE_ADD(ODBSibling<VisNode> sib)
	{
		while (sib.has)
		{
			VisNode visNode = sib.item.self;
			sib = sib.item.n;
			this.reactor.SEE_ADD(visNode);
		}
	}

	private void _REACTOR_SEE_REMOVE(ODBSibling<VisNode> sib)
	{
		while (sib.has)
		{
			VisNode visNode = sib.item.self;
			sib = sib.item.n;
			this.reactor.SEE_REMOVE(visNode);
		}
	}

	[Conditional("UNITY_EDITOR")]
	private static void _VALIDATE(VisNode vis)
	{
		if (vis.sight.count > 0 != vis.sight.any)
		{
			UnityEngine.Debug.LogError(string.Format("buzz {0} {1}", vis.sight.count, vis.sight.any), vis);
		}
		if (vis.sight.list.count != vis.sight.count)
		{
			UnityEngine.Debug.LogError(string.Format("buzz {0} {1}", vis.sight.list.count, vis.sight.count), vis);
		}
		if (vis.spect.count > 0 != vis.spect.any)
		{
			UnityEngine.Debug.LogError(string.Format("buzz {0} {1}", vis.spect.count, vis.spect.any), vis);
		}
		if (vis.spect.list.count != vis.spect.count)
		{
			UnityEngine.Debug.LogError(string.Format("buzz {0} {1}", vis.spect.list.count, vis.spect.count), vis);
		}
	}

	public static bool AreAware(VisNode instigator, VisNode target)
	{
		return (!VisNode.CanSee(instigator, target) ? false : instigator._IsSeenBy(target));
	}

	public static bool AreOblivious(VisNode instigator, VisNode target)
	{
		return (VisNode.CanSee(instigator, target) ? false : !instigator._IsSeenBy(target));
	}

	public static bool AttentionMessage(VisNode instigator, string message, object arg)
	{
		return false;
	}

	public static bool AttentionMessage(VisNode instigator, string message)
	{
		return VisNode.AttentionMessage(instigator, message, null);
	}

	public bool AttentionMessage(string message, object arg)
	{
		return false;
	}

	public bool AttentionMessage(string message)
	{
		return false;
	}

	public static bool AudibleMessage(VisNode instigator, Vector3 position, float radius, string message, object arg)
	{
		if (!instigator || instigator.mute || radius <= 0f || !instigator.enabled)
		{
			return false;
		}
		VisNode.DoAudibleMessage(instigator, position, radius, message, arg);
		return true;
	}

	public static bool AudibleMessage(VisNode instigator, float radius, string message, object arg)
	{
		if (!instigator || instigator.mute || radius <= 0f || !instigator.enabled)
		{
			return false;
		}
		VisNode.DoAudibleMessage(instigator, instigator._stamp.position, radius, message, arg);
		return true;
	}

	public static bool AudibleMessage(VisNode instigator, Vector3 position, float radius, string message)
	{
		if (!instigator || instigator.mute || radius <= 0f || !instigator.enabled)
		{
			return false;
		}
		VisNode.DoAudibleMessage(instigator, position, radius, message, null);
		return true;
	}

	public static bool AudibleMessage(VisNode instigator, float radius, string message)
	{
		if (!instigator || instigator.mute || radius <= 0f || !instigator.enabled)
		{
			return false;
		}
		VisNode.DoAudibleMessage(instigator, instigator._stamp.position, radius, message, null);
		return true;
	}

	public bool AudibleMessage(float radius, string message, object arg)
	{
		if (this.mute || !base.enabled || radius <= 0f)
		{
			return false;
		}
		VisNode.DoAudibleMessage(this, this._stamp.position, radius, message, arg);
		return true;
	}

	public bool AudibleMessage(float radius, string message)
	{
		if (this.mute || !base.enabled || radius <= 0f)
		{
			return false;
		}
		VisNode.DoAudibleMessage(this, this._stamp.position, radius, message, null);
		return true;
	}

	public bool AudibleMessage(Vector3 point, float radius, string message, object arg)
	{
		if (this.mute || !base.enabled || radius <= 0f)
		{
			return false;
		}
		VisNode.DoAudibleMessage(this, point, radius, message, arg);
		return true;
	}

	public bool AudibleMessage(Vector3 point, float radius, string message)
	{
		if (this.mute || !base.enabled || radius <= 0f)
		{
			return false;
		}
		VisNode.DoAudibleMessage(this, point, radius, message, null);
		return true;
	}

	private void Awake()
	{
		this.awake = true;
		if (!this._transform)
		{
			this._transform = base.transform;
		}
		if (base.enabled)
		{
			UnityEngine.Debug.LogWarning("VisNode was enabled prior to awake. VisNode's enabled button should always be off when the game is not running");
			this.Register();
		}
		this.histSight.last = 0;
		this.histSpect.last = this._spectMask;
		this.histTrait.last = this._traitMask;
		this.statusHandler = this.idMain as IVisHandler;
		this.hasStatusHandler = this.statusHandler != null;
		if (this._class)
		{
			this._handle = this._class.handle;
		}
	}

	public bool CanSee(Vis.Trait trait)
	{
		return (this._seeMask & (int)Vis.Trait.Unconcious << (int)(trait & Vis.Trait.Animal)) != (int)Vis.Trait.Alive;
	}

	public bool CanSee(Vis.Life life)
	{
		return (this._seeMask & (int)life) == (int)life;
	}

	public bool CanSee(Vis.Status status)
	{
		return (this._seeMask >> 8 & (int)status) == (int)status;
	}

	public bool CanSee(Vis.Role role)
	{
		return (this._seeMask >> 24 & (int)role) == (int)role;
	}

	public bool CanSee(Vis.Mask mask)
	{
		return (this._seeMask & mask.data) == mask.data;
	}

	public bool CanSee(VisNode other)
	{
		return VisNode.CanSee(this, other);
	}

	public static bool CanSee(VisNode instigator, VisNode target)
	{
		return (instigator == target ? true : instigator._CanSee(target));
	}

	public bool CanSeeAny(Vis.Life life)
	{
		return (this._seeMask & (int)life) != 0;
	}

	public bool CanSeeAny(Vis.Status status)
	{
		return (this._seeMask & (int)status << (int)Vis.Status.Alert) != 0;
	}

	public bool CanSeeAny(Vis.Role role)
	{
		return (this._seeMask & (int)role << (int)(Vis.Role.Target | Vis.Role.Entourage)) != 0;
	}

	public bool CanSeeAny(Vis.Mask mask)
	{
		return (this._seeMask & mask.data) != 0;
	}

	public bool CanSeeOnly(Vis.Life life)
	{
		return (this._seeMask & 7) == (int)life;
	}

	public bool CanSeeOnly(Vis.Status status)
	{
		return (this._seeMask & 32512) == (int)status << (int)Vis.Status.Alert;
	}

	public bool CanSeeOnly(Vis.Role role)
	{
		return (this._seeMask & -16777216) == (int)role << (int)(Vis.Role.Target | Vis.Role.Entourage);
	}

	public bool CanSeeOnly(Vis.Mask mask)
	{
		return this._seeMask == mask.data;
	}

	public bool CanSeeOnly(Vis.Trait trait)
	{
		return this._seeMask == (int)Vis.Trait.Unconcious << (int)(trait & Vis.Trait.Animal);
	}

	public bool CanSeeUnobstructed(VisNode other)
	{
		return (!this.CanSee(other) ? false : this.Unobstructed(other));
	}

	private void CheckQueries()
	{
		this.histSeen.Upd(this._seeMask);
		if (this._handle.valid)
		{
			if (this.sight.rem)
			{
				this.DoQueryRem(this.exit.first);
			}
			if (this.anySeenTraitChanges || this.histTrait.changed)
			{
				this.DoQueryRemAdd(this.sight.list.first);
			}
			else if (this.sight.@add)
			{
				this.DoQueryRemAdd(this.enter.first);
			}
		}
	}

	private void CheckReactions()
	{
		if (this.sight.rem)
		{
			this._REACTOR_SEE_REMOVE(this.exit.first);
			if (!this.sight.@add && !this.sight.any)
			{
				this.reactor.AWARE_EXIT();
			}
		}
		if (this.sight.@add)
		{
			if (!this.sight.had)
			{
				this.reactor.AWARE_ENTER();
			}
			this._REACTOR_SEE_ADD(this.enter.first);
		}
	}

	private static void ClearVis(ODBSibling<VisNode> iter)
	{
		do
		{
			VisNode.operandA = iter.item.self;
			iter = iter.item.n;
			if (!VisNode.operandA.sight.any)
			{
				continue;
			}
			ODBSibling<VisNode> oDBSibling = VisNode.operandA.sight.last.first;
			do
			{
				VisNode.operandB = oDBSibling.item.self;
				oDBSibling = oDBSibling.item.n;
				VisNode.ResolveHide();
			}
			while (oDBSibling.has);
			VisNode.operandB = null;
		}
		while (iter.has);
		VisNode.operandA = null;
	}

	public static Vis.Comparison Compare(VisNode self, VisNode target)
	{
		if (self == target)
		{
			return Vis.Comparison.IsSelf;
		}
		if (self._CanSee(target))
		{
			if (self._IsSeenBy(target))
			{
				return Vis.Comparison.Contact;
			}
			return Vis.Comparison.Stealthy;
		}
		if (self._IsSeenBy(target))
		{
			return Vis.Comparison.Prey;
		}
		return Vis.Comparison.Oblivious;
	}

	public static bool ComparisonMessage(VisNode instigator, Vis.Comparison comparison, string message, object arg)
	{
		Vis.Comparison comparison1 = comparison;
		switch (comparison1)
		{
			case Vis.Comparison.Prey:
			{
				return VisNode.PreyMessage(instigator, message, arg);
			}
			case Vis.Comparison.IsSelf:
			{
				if (!instigator || !instigator.enabled)
				{
					return false;
				}
				instigator.SendMessage(message, arg, SendMessageOptions.DontRequireReceiver);
				return true;
			}
			case Vis.Comparison.Contact:
			{
				return VisNode.ContactMessage(instigator, message, arg);
			}
			default:
			{
				if (comparison1 == Vis.Comparison.Oblivious)
				{
					break;
				}
				else
				{
					if (comparison1 != Vis.Comparison.Stealthy)
					{
						throw new ArgumentException(string.Concat(" do not know what to do with ", comparison), "comparison");
					}
					return VisNode.StealthMessage(instigator, message, arg);
				}
			}
		}
		return VisNode.ObliviousMessage(instigator, message, arg);
	}

	public static bool ComparisonMessage(VisNode instigator, Vis.Comparison comparison, string message)
	{
		return VisNode.ComparisonMessage(instigator, comparison, message, null);
	}

	public bool ComparisonMessage(Vis.Comparison comparison, string message, object arg)
	{
		return VisNode.ComparisonMessage(this, comparison, message, arg);
	}

	public bool ComparisonMessage(Vis.Comparison comparison, string message)
	{
		return VisNode.ComparisonMessage(this, comparison, message, null);
	}

	public static bool ContactMessage(VisNode instigator, string message, object arg)
	{
		return false;
	}

	public static bool ContactMessage(VisNode instigator, string message)
	{
		return VisNode.AttentionMessage(instigator, message, null);
	}

	public bool ContactMessage(string message, object arg)
	{
		return false;
	}

	public bool ContactMessage(string message)
	{
		return false;
	}

	private static void Copy(ODBSet<VisNode> src, ODBSet<VisNode> dst)
	{
		dst.Clear();
		dst.UnionWith(src);
	}

	private static void DoAttentionMessage(VisNode instigator, string message, object arg)
	{
		VisNode.RouteMessageHSet(instigator.sight.list, message, arg);
	}

	private static void DoAudibleMessage(VisNode instigator, Vector3 position, float radius, string message, object arg)
	{
		VisNode.Search.Radial.Enumerator nodesInRadius = Vis.GetNodesInRadius(position, radius);
		if (!instigator.deaf)
		{
			while (nodesInRadius.MoveNext())
			{
				if (!object.ReferenceEquals(nodesInRadius.Current, instigator))
				{
					nodesInRadius.Current.SendMessage(message, arg, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					break;
				}
			}
		}
		while (nodesInRadius.MoveNext())
		{
			nodesInRadius.Current.SendMessage(message, arg, SendMessageOptions.DontRequireReceiver);
		}
		nodesInRadius.Dispose();
	}

	private static void DoContactMessage(VisNode instigator, string message, object arg)
	{
		if (instigator.spect.count >= instigator.sight.count)
		{
			VisNode.RouteMessageOp(HSetOper.Intersect, instigator.sight.list, instigator.spect.list, message, arg);
		}
		else
		{
			VisNode.RouteMessageOp(HSetOper.Intersect, instigator.spect.list, instigator.sight.list, message, arg);
		}
	}

	private static void DoGestureMessage(VisNode instigator, string message, object arg)
	{
		VisNode.RouteMessageHSet(instigator.spect.list, message, arg);
	}

	private static void DoObliviousMessage(VisNode instigator, string message, object arg)
	{
		if (instigator.spect.count >= instigator.sight.count)
		{
			VisNode.RouteMessageOpUnionFirst(HSetOper.SymmetricExcept, instigator.sight.list, instigator.spect.list, VisNode.db, message, arg);
		}
		else
		{
			VisNode.RouteMessageOpUnionFirst(HSetOper.SymmetricExcept, instigator.spect.list, instigator.sight.list, VisNode.db, message, arg);
		}
	}

	private static void DoPreyMessage(VisNode instigator, string message, object arg)
	{
		VisNode.RouteMessageOp(HSetOper.Except, instigator.spect.list, instigator.sight.list, message, arg);
	}

	private void DoQueryRecurse(int i, VisNode other)
	{
		if (i >= this._handle.Length)
		{
			return;
		}
		VisQuery.Instance item = this._handle[i];
		switch (item.TryAdd(this, other))
		{
			case VisQuery.TryResult.Enter:
			{
				this.DoQueryRecurse(i + 1, other);
				item.ExecuteEnter(this, other);
				break;
			}
			case VisQuery.TryResult.Stay:
			{
				this.DoQueryRecurse(i + 1, other);
				break;
			}
			case VisQuery.TryResult.Exit:
			{
				item.ExecuteExit(this, other);
				this.DoQueryRecurse(i + 1, other);
				break;
			}
			default:
			{
				goto case VisQuery.TryResult.Stay;
			}
		}
	}

	private void DoQueryRem(ODBSibling<VisNode> sib)
	{
		if (this._handle.valid)
		{
			int length = this._handle.Length;
			int num = length;
			if (length > 0)
			{
				while (sib.has)
				{
					VisNode visNode = sib.item.self;
					sib = sib.item.n;
					for (int i = 0; i < num; i++)
					{
						VisQuery.Instance item = this._handle[i];
						if (item.TryRemove(this, visNode) == VisQuery.TryResult.Exit)
						{
							item.ExecuteExit(this, visNode);
						}
					}
				}
			}
		}
	}

	private void DoQueryRemAdd(ODBSibling<VisNode> sib)
	{
		if (this._handle.valid && this._handle.Length > 0)
		{
			while (sib.has)
			{
				VisNode visNode = sib.item.self;
				sib = sib.item.n;
				this.DoQueryRecurse(0, visNode);
			}
		}
	}

	private static void DoStealthMessage(VisNode instigator, string message, object arg)
	{
		VisNode.RouteMessageOp(HSetOper.Except, instigator.sight.list, instigator.spect.list, message, arg);
	}

	private void DrawConnections(ODBSet<VisNode> list)
	{
		if (list != null)
		{
			ODBForwardEnumerator<VisNode> enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Vector3 current = enumerator.Current._stamp.position;
				Gizmos.DrawLine(this._stamp.position, current);
				Gizmos.DrawWireSphere(current, 0.5f);
			}
			enumerator.Dispose();
		}
	}

	private static void Finally()
	{
		if (VisNode.disabledLastStep.any)
		{
			VisNode.RunStamp(VisNode.disabledLastStep.first);
			VisNode.disabledLastStep.Clear();
		}
	}

	public static bool GestureMessage(VisNode instigator, string message, object arg)
	{
		if (!instigator || !instigator.enabled)
		{
			return false;
		}
		VisNode.DoGestureMessage(instigator, message, arg);
		return true;
	}

	public static bool GestureMessage(VisNode instigator, string message)
	{
		return VisNode.GestureMessage(instigator, message, null);
	}

	public bool GestureMessage(string message, object arg)
	{
		if (!base.enabled)
		{
			return false;
		}
		VisNode.DoGestureMessage(this, message, arg);
		return true;
	}

	public bool GestureMessage(string message)
	{
		if (!base.enabled)
		{
			return false;
		}
		VisNode.DoGestureMessage(this, message, null);
		return true;
	}

	public static void GlobalMessage(string message, object arg)
	{
		ODBForwardEnumerator<VisNode> enumerator = VisNode.db.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				enumerator.Current.SendMessage(message, arg, SendMessageOptions.DontRequireReceiver);
			}
		}
		finally
		{
			enumerator.Dispose();
		}
	}

	public bool IsSeenBy(VisNode other)
	{
		return VisNode.IsSeenBy(this, other);
	}

	public static bool IsSeenBy(VisNode instigator, VisNode target)
	{
		return (instigator == target ? true : instigator._IsSeenBy(target));
	}

	public static bool IsStealthly(VisNode instigator, VisNode target)
	{
		return (!VisNode.CanSee(instigator, target) ? false : !instigator._IsSeenBy(target));
	}

	private static bool LogicSight()
	{
		if (!VisNode.operandB.active)
		{
			return false;
		}
		VisNode.bX = VisNode.operandB._stamp.position.x;
		VisNode.bY = VisNode.operandB._stamp.position.y;
		VisNode.bZ = VisNode.operandB._stamp.position.z;
		VisNode.planeDot = VisNode.bX * VisNode.fX + VisNode.bY * VisNode.fY + VisNode.bZ * VisNode.fZ;
		if (VisNode.planeDot < VisNode.fW || VisNode.planeDot > VisNode.PLANEDOTSIGHT)
		{
			return false;
		}
		VisNode.dX = VisNode.bX - VisNode.pX;
		VisNode.dY = VisNode.bY - VisNode.pY;
		VisNode.dZ = VisNode.bZ - VisNode.pZ;
		VisNode.dV2 = VisNode.dX * VisNode.dX + VisNode.dY * VisNode.dY + VisNode.dZ * VisNode.dZ;
		if (VisNode.dV2 > VisNode.SIGHT2)
		{
			return false;
		}
		if (VisNode.dV2 < 4.203895E-45f)
		{
			return VisNode.FALLBACK_TOO_CLOSE;
		}
		VisNode.dV = Mathf.Sqrt(VisNode.dV2);
		VisNode.nX = VisNode.dX / VisNode.dV;
		VisNode.nY = VisNode.dY / VisNode.dV;
		VisNode.nZ = VisNode.dZ / VisNode.dV;
		VisNode.dot = VisNode.fX * VisNode.nX + VisNode.fY * VisNode.nY + VisNode.fZ * VisNode.nZ;
		return VisNode.DOT < VisNode.dot;
	}

	public static bool ObliviousMessage(VisNode instigator, string message, object arg)
	{
		if (!instigator || !instigator.enabled)
		{
			return false;
		}
		VisNode.DoObliviousMessage(instigator, message, arg);
		return true;
	}

	public static bool ObliviousMessage(VisNode instigator, string message)
	{
		return VisNode.StealthMessage(instigator, message, null);
	}

	public bool ObliviousMessage(string message, object arg)
	{
		if (!base.enabled)
		{
			return false;
		}
		VisNode.ContactMessage(this, message, arg);
		return true;
	}

	public bool ObliviousMessage(string message)
	{
		if (!base.enabled)
		{
			return false;
		}
		VisNode.ContactMessage(this, message, null);
		return true;
	}

	private void OnDestroy()
	{
		if (VisManager.guardedUpdate)
		{
			UnityEngine.Debug.LogError(string.Concat("DESTROYING IN GUARDED UPDATE! ", base.name), this);
		}
		this.Unregister();
		VisNode.RemoveNow(this);
	}

	private void OnDisable()
	{
		if (this.awake)
		{
			bool flag = this.active;
			this.Unregister();
			if (flag && !this.active)
			{
				VisNode.recentlyDisabled.Add(this);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		VisGizmosUtility.ResetMatrixStack();
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		this.DrawConnections(this.sight.list);
		Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
		this.DrawConnections(this.spect.list);
		Transform transforms = (!this._transform ? base.transform : this._transform);
		Gizmos.color = new Color(1f, 1f, 1f, 0.9f);
		Vector3 vector3 = transforms.forward.normalized;
		Vector3 vector31 = transforms.position;
		Vector3 vector32 = vector31 + (vector3 * this.distance);
		Gizmos.DrawLine(vector31, vector32);
		VisGizmosUtility.DrawDotArc(vector31, transforms, this.distance, this.dotArc, this.dotArcBegin);
	}

	private void OnEnable()
	{
		if (this.awake)
		{
			this.Register();
		}
	}

	public static bool PreyMessage(VisNode instigator, string message, object arg)
	{
		return false;
	}

	public static bool PreyMessage(VisNode instigator, string message)
	{
		return VisNode.GestureMessage(instigator, message, null);
	}

	public bool PreyMessage(string message)
	{
		return false;
	}

	public static void Process()
	{
		if (VisNode.db.any)
		{
			if (!VisNode.recentlyDisabled.any)
			{
				VisNode.RunStamp(VisNode.db.first);
				VisNode.UpdateVis(VisNode.db.first);
				VisNode.RunStat(VisNode.db.first);
				VisNode.RunHiddenCalls(VisNode.db.first);
				VisNode.RunVoidSeenHiddenCalls(VisNode.db.last);
				VisNode.RunSeenCalls(VisNode.db.first);
				VisNode.RunQueries(VisNode.db.last);
				VisNode.Finally();
			}
			else
			{
				VisNode.RunStamp(VisNode.db.first);
				VisNode.RunStamp(VisNode.recentlyDisabled.first);
				VisNode.ClearVis(VisNode.recentlyDisabled.first);
				VisNode.UpdateVis(VisNode.db.first);
				VisNode.RunStat(VisNode.recentlyDisabled.first);
				VisNode.RunStat(VisNode.db.first);
				VisNode.RunHiddenCalls(VisNode.recentlyDisabled.first);
				VisNode.RunHiddenCalls(VisNode.db.first);
				VisNode.RunVoidSeenHiddenCalls(VisNode.recentlyDisabled.last);
				VisNode.RunVoidSeenHiddenCalls(VisNode.db.last);
				VisNode.RunSeenCalls(VisNode.recentlyDisabled.first);
				VisNode.RunSeenCalls(VisNode.db.first);
				VisNode.RunQueries(VisNode.recentlyDisabled.last);
				VisNode.RunQueries(VisNode.db.last);
				VisNode.Finally();
				VisNode.SwapDisabled();
			}
		}
		else if (VisNode.recentlyDisabled.any)
		{
			VisNode.RunStamp(VisNode.recentlyDisabled.first);
			VisNode.ClearVis(VisNode.recentlyDisabled.first);
			VisNode.RunStat(VisNode.recentlyDisabled.first);
			VisNode.RunHiddenCalls(VisNode.recentlyDisabled.first);
			VisNode.RunVoidSeenHiddenCalls(VisNode.recentlyDisabled.last);
			VisNode.RunSeenCalls(VisNode.recentlyDisabled.first);
			VisNode.RunQueries(VisNode.recentlyDisabled.last);
			VisNode.Finally();
			VisNode.SwapDisabled();
		}
	}

	private void Register()
	{
		if (!this.awake || this.active)
		{
			return;
		}
		if (VisManager.guardedUpdate)
		{
			throw new InvalidOperationException("DO NOT INSTANTIATE WHILE VisibilityManager.isUpdatingVisibility!!");
		}
		if (!VisNode.manager)
		{
			VisNode.manager = (new GameObject("__Vis", new Type[] { typeof(VisManager) })).GetComponent<VisManager>();
		}
		if (!this.dataConstructed)
		{
			this.sight.list = new ODBSet<VisNode>();
			this.sight.last = new ODBSet<VisNode>();
			this.spect.list = new ODBSet<VisNode>();
			this.spect.last = new ODBSet<VisNode>();
			this.enter = new ODBSet<VisNode>();
			this.exit = new ODBSet<VisNode>();
			this.cleanList = new List<VisNode>();
			this.dataConstructed = true;
		}
		else if (!VisNode.recentlyDisabled.Remove(this))
		{
			VisNode.disabledLastStep.Remove(this);
		}
		this.item = VisNode.db.Register(this);
		this.active = this.item == this;
	}

	private static void RemoveLinkNow(VisNode node, VisNode didSee)
	{
		if (node.sight.list.Remove(node))
		{
			node.sight.rem = true;
			didSee.spect.rem = didSee.spect.rem | didSee.spect.list.Remove(node);
		}
		if (node.sight.last.Remove(didSee))
		{
			didSee.spect.last.Remove(node);
		}
		else
		{
			node.enter.Remove(didSee);
		}
		int num = node.sight.count - 1;
		int num1 = num;
		node.sight.count = num;
		if (num1 == 0)
		{
			node.sight.any = false;
		}
		int num2 = didSee.spect.count - 1;
		num1 = num2;
		didSee.spect.count = num2;
		if (num1 == 0)
		{
			didSee.spect.any = false;
		}
	}

	internal static void RemoveNow(VisNode node)
	{
		int i;
		if (!node.dataConstructed)
		{
			return;
		}
		if (!VisNode.recentlyDisabled.Remove(node))
		{
			VisNode.disabledLastStep.Remove(node);
		}
		for (i = 0; i < node.cleanList.Count; i++)
		{
			node.cleanList[i].exit.Remove(node);
		}
		ODBForwardEnumerator<VisNode> enumerator = node.exit.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.cleanList.Remove(node);
		}
		enumerator.Dispose();
		node.cleanList.Clear();
		node.cleanList.AddRange(node.sight.list);
		for (i = 0; i < node.cleanList.Count; i++)
		{
			VisNode.RemoveLinkNow(node, node.cleanList[i]);
		}
		node.cleanList.Clear();
		node.cleanList.AddRange(node.spect.list);
		for (i = 0; i < node.cleanList.Count; i++)
		{
			VisNode.RemoveLinkNow(node.cleanList[i], node);
		}
		node.cleanList.Clear();
	}

	protected new void Reset()
	{
		base.Reset();
		VisReactor component = base.GetComponent<VisReactor>();
		if (component)
		{
			this.reactor = component;
			this.reactor.__visNode = this;
		}
	}

	private static void ResolveHide()
	{
		if (VisNode.operandA.sight.list.Remove(VisNode.operandB))
		{
			VisNode.operandB.spect.rem = VisNode.operandB.spect.rem | VisNode.operandB.spect.list.Remove(VisNode.operandA);
			VisNode.operandA.exit.Add(VisNode.operandB);
			VisNode.operandB.cleanList.Add(VisNode.operandA);
		}
	}

	private static void ResolveSee()
	{
		if (VisNode.operandA.sight.list.Add(VisNode.operandB))
		{
			VisNode.operandB.spect.@add = VisNode.operandB.spect.@add | VisNode.operandB.spect.list.Add(VisNode.operandA);
			VisNode.operandA.sight.@add = true;
			VisNode.operandA.enter.Add(VisNode.operandB);
		}
	}

	private static void RouteMessageHSet(ODBSet<VisNode> list, string msg, object arg)
	{
		if (list.any)
		{
			ODBSibling<VisNode> oDBSibling = list.first;
			do
			{
				VisNode visNode = oDBSibling.item.self;
				oDBSibling = oDBSibling.item.n;
				try
				{
					visNode.SendMessage(msg, arg, SendMessageOptions.DontRequireReceiver);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(exception, visNode);
				}
			}
			while (oDBSibling.has);
		}
	}

	private static void RouteMessageList(RecycleList<VisNode> list, string msg)
	{
		VisNode.RouteMessageList(list, msg, null);
	}

	private static void RouteMessageList(RecycleList<VisNode> list, string msg, object arg)
	{
		RecycleListIter<VisNode> recycleListIter = list.MakeIter();
		try
		{
			while (recycleListIter.MoveNext())
			{
				try
				{
					recycleListIter.Current.SendMessage(msg, arg, SendMessageOptions.DontRequireReceiver);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(exception, recycleListIter.Current);
				}
			}
		}
		finally
		{
			recycleListIter.Dispose();
		}
	}

	private static void RouteMessageOp(HSetOper op, ODBSet<VisNode> a, IEnumerable<VisNode> b, string msg, object arg)
	{
		RecycleList<VisNode> recycleList = a.OperList(op, b);
		VisNode.RouteMessageList(recycleList, msg, arg);
		recycleList.Dispose();
	}

	private static void RouteMessageOp(HSetOper op, ODBSet<VisNode> a, IEnumerable<VisNode> b, string msg)
	{
		VisNode.RouteMessageOp(op, a, b, msg, null);
	}

	private static void RouteMessageOpUnionFirst(HSetOper op, ODBSet<VisNode> a, ODBSet<VisNode> aa, IEnumerable<VisNode> b, string msg, object arg)
	{
		ODBSet<VisNode> oDBSet = new ODBSet<VisNode>(a);
		oDBSet.UnionWith(aa);
		VisNode.RouteMessageOp(op, oDBSet, b, msg, arg);
	}

	private static void RouteMessageOpUnionFirst(HSetOper op, ODBSet<VisNode> a, ODBSet<VisNode> aa, IEnumerable<VisNode> b, string msg)
	{
		VisNode.RouteMessageOpUnionFirst(op, a, aa, b, msg, null);
	}

	private static void RunHiddenCalls(ODBSibling<VisNode> sib)
	{
		do
		{
			VisNode.operandA = sib.item.self;
			sib = sib.item.n;
			if (!VisNode.operandA.sight.rem)
			{
				continue;
			}
			ODBSibling<VisNode> oDBSibling = VisNode.operandA.exit.first;
			do
			{
				VisNode.operandB = oDBSibling.item.self;
				oDBSibling = oDBSibling.item.n;
				VisNode.operandB._CB_OnHiddenFrom_(VisNode.operandA);
			}
			while (oDBSibling.has);
			VisNode.operandB = null;
		}
		while (sib.has);
		VisNode.operandA = null;
	}

	private static void RunQueries(ODBSibling<VisNode> sib)
	{
		do
		{
			VisNode.operandA = sib.item.self;
			sib = sib.item.p;
			if (VisNode.operandA.reactor)
			{
				VisNode.operandA.CheckReactions();
			}
			VisNode.operandA.CheckQueries();
		}
		while (sib.has);
		VisNode.operandA = null;
	}

	private static void RunSeenCalls(ODBSibling<VisNode> sib)
	{
		do
		{
			VisNode.operandA = sib.item.self;
			sib = sib.item.n;
			if (!VisNode.operandA.sight.@add)
			{
				continue;
			}
			ODBSibling<VisNode> oDBSibling = VisNode.operandA.enter.last;
			do
			{
				VisNode.operandB = oDBSibling.item.self;
				oDBSibling = oDBSibling.item.p;
				VisNode.operandB._CB_OnSeenBy_(VisNode.operandA);
			}
			while (oDBSibling.has);
			VisNode.operandB = null;
		}
		while (sib.has);
		VisNode.operandA = null;
	}

	private static void RunStamp(ODBSibling<VisNode> sib)
	{
		do
		{
			VisNode.operandA = sib.item.self;
			sib = sib.item.n;
			VisNode.operandA.Stamp();
		}
		while (sib.has);
		VisNode.operandA = null;
	}

	private static void RunStat(ODBSibling<VisNode> sib)
	{
		do
		{
			VisNode.operandA = sib.item.self;
			sib = sib.item.n;
			VisNode.operandA.StatUpdate();
		}
		while (sib.has);
		VisNode.operandA = null;
	}

	private static void RunVoidSeenHiddenCalls(ODBSibling<VisNode> sib)
	{
		do
		{
			VisNode.operandA = sib.item.self;
			sib = sib.item.p;
			if (VisNode.operandA.spect.had)
			{
				if (!VisNode.operandA.spect.any)
				{
					VisNode.operandA._CB_OnHidden_();
					VisNode.operandA.spect.had = false;
				}
			}
			else if (VisNode.operandA.spect.any)
			{
				VisNode.operandA._CB_OnSeen_();
				VisNode.operandA.spect.had = true;
			}
			VisNode.operandA.sight.had = VisNode.operandA.sight.any;
		}
		while (sib.has);
		VisNode.operandA = null;
	}

	private void SeenHideFire()
	{
		if (this.spect.had != this.spect.any)
		{
			if (!this.spect.any)
			{
				this._CB_OnHidden_();
			}
			else
			{
				this._CB_OnSeen_();
			}
			this.spect.had = this.spect.any;
		}
		this.sight.had = this.sight.any;
	}

	internal static void Stage1(VisNode self)
	{
		self.Stamp();
	}

	private void Stamp()
	{
		this._stamp.Collect(this._transform);
		VisNode.Transfer(this.sight.list, this.sight.last, this.sight.@add, this.sight.rem);
		VisNode.Transfer(this.spect.list, this.spect.last, this.spect.@add, this.spect.rem);
		if (this.sight.@add)
		{
			this.enter.Clear();
			this.sight.@add = false;
		}
		if (this.sight.rem)
		{
			this.exit.Clear();
			this.sight.rem = false;
		}
		this.spect.@add = false;
		if (this.spect.rem)
		{
			this.spect.rem = false;
			this.cleanList.Clear();
		}
		if (this.hasStatusHandler)
		{
			Vis.Mask mask = this.statusHandler.VisPoll(this.traitMask);
			this._traitMask = mask.data;
		}
		this.histTrait.Upd(this._traitMask);
		this._sightCurrentMask = 0;
		this.histSight.Upd(this._sightCurrentMask);
		this.histSpect.Upd(this._spectMask);
		this._seeMask = 0;
		this.anySeenTraitChanges = false;
	}

	private void StatUpdate()
	{
		this.sight.count = this.sight.list.count;
		this.sight.any = this.sight.count > 0;
		this.spect.count = this.spect.list.count;
		this.spect.any = this.spect.count > 0;
	}

	public static bool StealthMessage(VisNode instigator, string message, object arg)
	{
		return false;
	}

	public static bool StealthMessage(VisNode instigator, string message)
	{
		return VisNode.StealthMessage(instigator, message, null);
	}

	public bool StealthMessage(string message, object arg)
	{
		return false;
	}

	private static void SwapDisabled()
	{
		ODBSet<VisNode> oDBSet = VisNode.disabledLastStep;
		VisNode.disabledLastStep = VisNode.recentlyDisabled;
		VisNode.recentlyDisabled = oDBSet;
	}

	private static void Transfer(ODBSet<VisNode> src, ODBSet<VisNode> dst, bool addAny, bool remAny)
	{
		if (addAny)
		{
			if (!remAny)
			{
				dst.UnionWith(src);
			}
			else
			{
				VisNode.Copy(src, dst);
			}
		}
		else if (remAny)
		{
			dst.ExceptWith(src);
		}
	}

	public bool Unobstructed(VisNode other)
	{
		return Physics.Linecast(this._stamp.position, other._stamp.position, 1);
	}

	private void Unregister()
	{
		if (this.active)
		{
			if (VisManager.guardedUpdate)
			{
				throw new InvalidOperationException("DO NOT OR DISABLE DESTROY WHILE VisibilityManager.isUpdatingVisibility!!");
			}
			VisNode.db.Unregister(ref this.item);
			this.active = this.item == this;
		}
	}

	private static void UpdateVis(ODBSibling<VisNode> first_sib)
	{
		VisNode.FALLBACK_TOO_CLOSE = false;
		ODBSibling<VisNode> firstSib = first_sib;
		do
		{
			VisNode.operandA = firstSib.item.self;
			firstSib = firstSib.item.n;
			if (VisNode.operandA._sightCurrentMask != 0)
			{
				VisNode.pX = VisNode.operandA._stamp.position.x;
				VisNode.pY = VisNode.operandA._stamp.position.y;
				VisNode.pZ = VisNode.operandA._stamp.position.z;
				VisNode.fX = VisNode.operandA._stamp.plane.x;
				VisNode.fY = VisNode.operandA._stamp.plane.y;
				VisNode.fZ = VisNode.operandA._stamp.plane.z;
				VisNode.fW = VisNode.operandA._stamp.plane.w;
				VisNode.DOT = VisNode.operandA.dotArc;
				VisNode.SIGHT = VisNode.operandA.distance;
				VisNode.SIGHT2 = VisNode.SIGHT * VisNode.SIGHT;
				VisNode.PLANEDOTSIGHT = VisNode.fW + VisNode.SIGHT;
				if (VisNode.operandA.sight.any)
				{
					VisNode.FALLBACK_TOO_CLOSE = true;
					ODBSibling<VisNode> oDBSibling = VisNode.operandA.sight.last.first;
					if (!VisNode.operandA.histSight.changed)
					{
						VisNode.operandB = oDBSibling.item.self;
						do
						{
							VisNode.operandB = oDBSibling.item.self;
							oDBSibling = oDBSibling.item.n;
							if (VisNode.operandB.active)
							{
								VisNode.operandB.__skipOnce_ = true;
								VisNode.temp_bTraits = VisNode.operandB._traitMask;
								if (VisNode.operandB.histTrait.changed)
								{
									if ((VisNode.temp_bTraits & VisNode.operandA._sightCurrentMask) == 0 || !VisNode.LogicSight())
									{
										VisNode.ResolveHide();
										continue;
									}
									else
									{
										VisNode.operandA.anySeenTraitChanges = true;
									}
								}
								else if (!VisNode.LogicSight())
								{
									VisNode.ResolveHide();
									continue;
								}
								VisNode tempBTraits = VisNode.operandA;
								tempBTraits._seeMask = tempBTraits._seeMask | VisNode.temp_bTraits;
							}
							else
							{
								VisNode.ResolveHide();
							}
						}
						while (oDBSibling.has);
					}
					else
					{
						do
						{
							VisNode.operandB = oDBSibling.item.self;
							oDBSibling = oDBSibling.item.n;
							if (VisNode.operandB.active)
							{
								VisNode.operandB.__skipOnce_ = true;
								VisNode.temp_bTraits = VisNode.operandB._traitMask;
								if ((VisNode.temp_bTraits & VisNode.operandA._sightCurrentMask) == 0 || !VisNode.LogicSight())
								{
									VisNode.ResolveHide();
								}
								else
								{
									VisNode visNode = VisNode.operandA;
									visNode._seeMask = visNode._seeMask | VisNode.temp_bTraits;
								}
							}
							else
							{
								VisNode.ResolveHide();
							}
						}
						while (oDBSibling.has);
					}
					VisNode.FALLBACK_TOO_CLOSE = false;
				}
				VisNode.operandA.__skipOnce_ = true;
				ODBSibling<VisNode> firstSib1 = first_sib;
				do
				{
					VisNode.operandB = firstSib1.item.self;
					firstSib1 = firstSib1.item.n;
					if (!VisNode.operandB.__skipOnce_)
					{
						VisNode.temp_bTraits = VisNode.operandB._traitMask;
						if ((VisNode.temp_bTraits & VisNode.operandA._sightCurrentMask) == 0 || !VisNode.LogicSight())
						{
							continue;
						}
						VisNode.ResolveSee();
						VisNode tempBTraits1 = VisNode.operandA;
						tempBTraits1._seeMask = tempBTraits1._seeMask | VisNode.temp_bTraits;
					}
					else
					{
						VisNode.operandB.__skipOnce_ = false;
					}
				}
				while (firstSib1.has);
				VisNode.operandB = null;
			}
			else if (VisNode.operandA.sight.any)
			{
				ODBSibling<VisNode> oDBSibling1 = VisNode.operandA.sight.last.first;
				do
				{
					VisNode.operandB = oDBSibling1.item.self;
					oDBSibling1 = oDBSibling1.item.n;
					VisNode.ResolveHide();
				}
				while (oDBSibling1.has);
				VisNode.operandB = null;
			}
		}
		while (firstSib.has);
		VisNode.operandA = null;
	}

	public static class Search
	{
		public interface ISearch : IEnumerable, IEnumerable<VisNode>
		{

		}

		public interface ISearch<TEnumerator> : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>
		where TEnumerator : struct, IEnumerator<VisNode>
		{
			TEnumerator GetEnumerator();
		}

		public struct MaskCompareData
		{
			public Vis.Op op;

			public int mask;

			public MaskCompareData(Vis.Op op, Vis.Mask mask)
			{
				this.op = op;
				this.mask = mask.data;
			}

			public bool Pass(int mask)
			{
				return Vis.Evaluate(this.op, this.mask, mask);
			}
		}

		public struct Point : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Point.Enumerator>
		{
			public Vector3 point;

			public Point(Vector3 point)
			{
				this.point = point;
			}

			public VisNode.Search.Point.Enumerator GetEnumerator()
			{
				return new VisNode.Search.Point.Enumerator(new VisNode.Search.PointVisibilityData(this.point));
			}

			IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
			{
				public ODBForwardEnumerator<VisNode> e;

				public VisNode Current;

				private bool d;

				public VisNode.Search.PointVisibilityData data;

				VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
				{
					get
					{
						return this.Current;
					}
				}

				object System.Collections.IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				public Enumerator(VisNode.Search.PointVisibilityData pv)
				{
					this.Current = null;
					this.d = false;
					this.e = VisNode.db.GetEnumerator();
					this.data = pv;
				}

				public void Dispose()
				{
					if (!this.d)
					{
						this.e.Dispose();
						this.d = true;
					}
				}

				public bool MoveNext()
				{
					while (this.e.MoveNext())
					{
						if (!this.Pass(this.e.Current))
						{
							continue;
						}
						return true;
					}
					this.Current = null;
					return false;
				}

				private bool Pass(VisNode cur)
				{
					if (!this.data.Pass(cur))
					{
						return false;
					}
					this.Current = cur;
					return true;
				}

				public void Reset()
				{
					this.Dispose();
					this.d = false;
					this.e = VisNode.db.GetEnumerator();
				}
			}

			public struct SightMasked : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Point.SightMasked.Enumerator>
			{
				public Vector3 point;

				public VisNode.Search.MaskCompareData maskComp;

				public SightMasked(Vector3 point, Vis.Mask mask, Vis.Op op)
				{
					this.point = point;
					this.maskComp = new VisNode.Search.MaskCompareData(op, mask);
				}

				public VisNode.Search.Point.SightMasked.Enumerator GetEnumerator()
				{
					return new VisNode.Search.Point.SightMasked.Enumerator(new VisNode.Search.PointVisibilityData(this.point), this.maskComp);
				}

				IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
				{
					public ODBForwardEnumerator<VisNode> e;

					public VisNode Current;

					private bool d;

					public VisNode.Search.PointVisibilityData data;

					public VisNode.Search.MaskCompareData viewComp;

					VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
					{
						get
						{
							return this.Current;
						}
					}

					object System.Collections.IEnumerator.Current
					{
						get
						{
							return this.Current;
						}
					}

					public Enumerator(VisNode.Search.PointVisibilityData pv, VisNode.Search.MaskCompareData mc)
					{
						this.Current = null;
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
						this.data = pv;
						this.viewComp = mc;
					}

					public void Dispose()
					{
						if (!this.d)
						{
							this.e.Dispose();
							this.d = true;
						}
					}

					public bool MoveNext()
					{
						while (this.e.MoveNext())
						{
							if (!this.Pass(this.e.Current))
							{
								continue;
							}
							return true;
						}
						this.Current = null;
						return false;
					}

					private bool Pass(VisNode cur)
					{
						if (!this.viewComp.Pass(cur._sightMask) || !this.data.Pass(cur))
						{
							return false;
						}
						this.Current = cur;
						return true;
					}

					public void Reset()
					{
						this.Dispose();
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
					}
				}
			}

			public struct TraitMasked : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Point.TraitMasked.Enumerator>
			{
				public Vector3 point;

				public VisNode.Search.MaskCompareData maskComp;

				public TraitMasked(Vector3 point, Vis.Mask mask, Vis.Op op)
				{
					this.point = point;
					this.maskComp = new VisNode.Search.MaskCompareData(op, mask);
				}

				public VisNode.Search.Point.TraitMasked.Enumerator GetEnumerator()
				{
					return new VisNode.Search.Point.TraitMasked.Enumerator(new VisNode.Search.PointVisibilityData(this.point), this.maskComp);
				}

				IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
				{
					public ODBForwardEnumerator<VisNode> e;

					public VisNode Current;

					private bool d;

					public VisNode.Search.PointVisibilityData data;

					public VisNode.Search.MaskCompareData traitComp;

					VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
					{
						get
						{
							return this.Current;
						}
					}

					object System.Collections.IEnumerator.Current
					{
						get
						{
							return this.Current;
						}
					}

					public Enumerator(VisNode.Search.PointVisibilityData pv, VisNode.Search.MaskCompareData mc)
					{
						this.Current = null;
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
						this.data = pv;
						this.traitComp = mc;
					}

					public void Dispose()
					{
						if (!this.d)
						{
							this.e.Dispose();
							this.d = true;
						}
					}

					public bool MoveNext()
					{
						while (this.e.MoveNext())
						{
							if (!this.Pass(this.e.Current))
							{
								continue;
							}
							return true;
						}
						this.Current = null;
						return false;
					}

					private bool Pass(VisNode cur)
					{
						if (!this.traitComp.Pass(cur._traitMask) || !this.data.Pass(cur))
						{
							return false;
						}
						this.Current = cur;
						return true;
					}

					public void Reset()
					{
						this.Dispose();
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
					}
				}
			}

			public struct Visual : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Point.Visual.Enumerator>
			{
				public Vector3 point;

				public Visual(Vector3 point)
				{
					this.point = point;
				}

				public VisNode.Search.Point.Visual.Enumerator GetEnumerator()
				{
					return new VisNode.Search.Point.Visual.Enumerator(new VisNode.Search.PointVisibilityData(this.point));
				}

				IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
				{
					public ODBForwardEnumerator<VisNode> e;

					public VisNode Current;

					private bool d;

					public VisNode.Search.PointVisibilityData data;

					VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
					{
						get
						{
							return this.Current;
						}
					}

					object System.Collections.IEnumerator.Current
					{
						get
						{
							return this.Current;
						}
					}

					public Enumerator(VisNode.Search.PointVisibilityData pv)
					{
						this.Current = null;
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
						this.data = pv;
					}

					public void Dispose()
					{
						if (!this.d)
						{
							this.e.Dispose();
							this.d = true;
						}
					}

					public bool MoveNext()
					{
						while (this.e.MoveNext())
						{
							if (!this.Pass(this.e.Current))
							{
								continue;
							}
							return true;
						}
						this.Current = null;
						return false;
					}

					private bool Pass(VisNode cur)
					{
						return false;
					}

					public void Reset()
					{
						this.Dispose();
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
					}
				}

				public struct SightMasked : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Point.Visual.SightMasked.Enumerator>
				{
					public Vector3 point;

					public VisNode.Search.MaskCompareData maskComp;

					public SightMasked(Vector3 point, Vis.Mask mask, Vis.Op op)
					{
						this.point = point;
						this.maskComp = new VisNode.Search.MaskCompareData(op, mask);
					}

					public VisNode.Search.Point.Visual.SightMasked.Enumerator GetEnumerator()
					{
						return new VisNode.Search.Point.Visual.SightMasked.Enumerator(new VisNode.Search.PointVisibilityData(this.point), this.maskComp);
					}

					IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
					{
						return this.GetEnumerator();
					}

					IEnumerator System.Collections.IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}

					public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
					{
						public ODBForwardEnumerator<VisNode> e;

						public VisNode Current;

						private bool d;

						public VisNode.Search.PointVisibilityData data;

						public VisNode.Search.MaskCompareData viewComp;

						VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
						{
							get
							{
								return this.Current;
							}
						}

						object System.Collections.IEnumerator.Current
						{
							get
							{
								return this.Current;
							}
						}

						public Enumerator(VisNode.Search.PointVisibilityData pv, VisNode.Search.MaskCompareData mc)
						{
							this.Current = null;
							this.d = false;
							this.e = VisNode.db.GetEnumerator();
							this.data = pv;
							this.viewComp = mc;
						}

						public void Dispose()
						{
							if (!this.d)
							{
								this.e.Dispose();
								this.d = true;
							}
						}

						public bool MoveNext()
						{
							while (this.e.MoveNext())
							{
								if (!this.Pass(this.e.Current))
								{
									continue;
								}
								return true;
							}
							this.Current = null;
							return false;
						}

						private bool Pass(VisNode cur)
						{
							return false;
						}

						public void Reset()
						{
							this.Dispose();
							this.d = false;
							this.e = VisNode.db.GetEnumerator();
						}
					}
				}

				public struct TraitMasked : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Point.Visual.TraitMasked.Enumerator>
				{
					public Vector3 point;

					public VisNode.Search.MaskCompareData maskComp;

					public TraitMasked(Vector3 point, Vis.Mask mask, Vis.Op op)
					{
						this.point = point;
						this.maskComp = new VisNode.Search.MaskCompareData(op, mask);
					}

					public VisNode.Search.Point.Visual.TraitMasked.Enumerator GetEnumerator()
					{
						return new VisNode.Search.Point.Visual.TraitMasked.Enumerator(new VisNode.Search.PointVisibilityData(this.point), this.maskComp);
					}

					IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
					{
						return this.GetEnumerator();
					}

					IEnumerator System.Collections.IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}

					public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
					{
						public ODBForwardEnumerator<VisNode> e;

						public VisNode Current;

						private bool d;

						public VisNode.Search.PointVisibilityData data;

						public VisNode.Search.MaskCompareData traitComp;

						VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
						{
							get
							{
								return this.Current;
							}
						}

						object System.Collections.IEnumerator.Current
						{
							get
							{
								return this.Current;
							}
						}

						public Enumerator(VisNode.Search.PointVisibilityData pv, VisNode.Search.MaskCompareData mc)
						{
							this.Current = null;
							this.d = false;
							this.e = VisNode.db.GetEnumerator();
							this.data = pv;
							this.traitComp = mc;
						}

						public void Dispose()
						{
							if (!this.d)
							{
								this.e.Dispose();
								this.d = true;
							}
						}

						public bool MoveNext()
						{
							while (this.e.MoveNext())
							{
								if (!this.Pass(this.e.Current))
								{
									continue;
								}
								return true;
							}
							this.Current = null;
							return false;
						}

						private bool Pass(VisNode cur)
						{
							return false;
						}

						public void Reset()
						{
							this.Dispose();
							this.d = false;
							this.e = VisNode.db.GetEnumerator();
						}
					}
				}
			}
		}

		public struct PointRadiusData
		{
			public float radiusSquare;

			public float x;

			public float y;

			public float z;

			public float dX;

			public float dY;

			public float dZ;

			public float d2;

			public PointRadiusData(Vector3 pos, float radius)
			{
				this.x = pos.x;
				this.y = pos.y;
				this.z = pos.z;
				this.radiusSquare = radius * radius;
				this.dX = 0f;
				this.dY = 0f;
				this.dZ = 0f;
				this.d2 = 0f;
			}

			public bool Pass(VisNode current)
			{
				this.dX = this.x - current._stamp.position.x;
				this.dY = this.y - current._stamp.position.y;
				this.dZ = this.z - current._stamp.position.z;
				this.d2 = this.dX * this.dX + this.dY * this.dY + this.dZ * this.dZ;
				if (this.d2 <= this.radiusSquare)
				{
					return true;
				}
				return false;
			}
		}

		public struct PointRadiusMaskData
		{
			public VisNode.Search.PointRadiusData pr;

			public VisNode.Search.MaskCompareData mc;

			public PointRadiusMaskData(Vector3 pos, float radius, Vis.Op op, Vis.Mask mask) : this(new VisNode.Search.PointRadiusData(pos, radius), new VisNode.Search.MaskCompareData(op, mask))
			{
			}

			public PointRadiusMaskData(VisNode.Search.PointRadiusData pr, VisNode.Search.MaskCompareData mc)
			{
				this.pr = pr;
				this.mc = mc;
			}

			public bool Pass(VisNode current, int mask)
			{
				return (!this.mc.Pass(mask) ? false : this.pr.Pass(current));
			}
		}

		public struct PointVisibilityData
		{
			public float x;

			public float y;

			public float z;

			public float dX;

			public float dY;

			public float dZ;

			public float d2;

			public float d;

			public float nX;

			public float nY;

			public float nZ;

			public float radius;

			public float radiusSquare;

			public PointVisibilityData(Vector3 point)
			{
				this.x = point.x;
				this.y = point.y;
				this.z = point.z;
				this.dX = 0f;
				this.dY = 0f;
				this.dZ = 0f;
				this.d2 = 0f;
				this.d = 0f;
				this.nX = 0f;
				this.nY = 0f;
				this.nZ = 0f;
				this.radius = 0f;
				this.radiusSquare = 0f;
			}

			public bool Pass(VisNode Current)
			{
				this.radius = Current.distance;
				this.radiusSquare = this.radiusSquare * this.radiusSquare;
				this.dX = this.x - Current._stamp.position.x;
				this.dY = this.y - Current._stamp.position.y;
				this.dZ = this.z - Current._stamp.position.z;
				this.d2 = this.dX * this.dX + this.dY * this.dY + this.dZ * this.dZ;
				if (this.d2 < 4.203895E-45f)
				{
					return true;
				}
				this.d = Mathf.Sqrt(this.d2);
				this.nX = this.dX / this.d;
				this.nY = this.dY / this.d;
				this.nZ = this.dZ / this.d;
				VisNode.dot = Current._stamp.plane.x * this.nX + Current._stamp.plane.y * this.nY + Current._stamp.plane.z * this.nZ;
				if (VisNode.dot >= Current.dotArc)
				{
					return true;
				}
				return false;
			}
		}

		public struct Radial : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Radial.Enumerator>
		{
			public Vector3 point;

			public float radius;

			public Radial(Vector3 point, float radius)
			{
				this.point = point;
				this.radius = radius;
			}

			public VisNode.Search.Radial.Enumerator GetEnumerator()
			{
				return new VisNode.Search.Radial.Enumerator(new VisNode.Search.PointRadiusData(this.point, this.radius));
			}

			IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public struct Audible : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Radial.Audible.Enumerator>
			{
				public Vector3 point;

				public float radius;

				public Audible(Vector3 point, float radius)
				{
					this.point = point;
					this.radius = radius;
				}

				public VisNode.Search.Radial.Audible.Enumerator GetEnumerator()
				{
					return new VisNode.Search.Radial.Audible.Enumerator(new VisNode.Search.PointRadiusData(this.point, this.radius));
				}

				IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
				{
					public ODBForwardEnumerator<VisNode> e;

					public VisNode Current;

					private bool d;

					public VisNode.Search.PointRadiusData data;

					VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
					{
						get
						{
							return this.Current;
						}
					}

					object System.Collections.IEnumerator.Current
					{
						get
						{
							return this.Current;
						}
					}

					public Enumerator(VisNode.Search.PointRadiusData pr)
					{
						this.Current = null;
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
						this.data = pr;
					}

					public void Dispose()
					{
						if (!this.d)
						{
							this.e.Dispose();
							this.d = true;
						}
					}

					public bool MoveNext()
					{
						while (this.e.MoveNext())
						{
							if (!this.Pass(this.e.Current))
							{
								continue;
							}
							return true;
						}
						this.Current = null;
						return false;
					}

					private bool Pass(VisNode cur)
					{
						if (cur.deaf || !this.data.Pass(cur))
						{
							return false;
						}
						this.Current = cur;
						return true;
					}

					public void Reset()
					{
						this.Dispose();
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
					}
				}

				public struct SightMasked : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Radial.Audible.SightMasked.Enumerator>
				{
					public Vector3 point;

					public float radius;

					public VisNode.Search.MaskCompareData maskComp;

					public SightMasked(Vector3 point, float radius, Vis.Mask mask, Vis.Op op)
					{
						this.point = point;
						this.radius = radius;
						this.maskComp = new VisNode.Search.MaskCompareData(op, mask);
					}

					public VisNode.Search.Radial.Audible.SightMasked.Enumerator GetEnumerator()
					{
						return new VisNode.Search.Radial.Audible.SightMasked.Enumerator(new VisNode.Search.PointRadiusData(this.point, this.radius), this.maskComp);
					}

					IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
					{
						return this.GetEnumerator();
					}

					IEnumerator System.Collections.IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}

					public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
					{
						public ODBForwardEnumerator<VisNode> e;

						public VisNode Current;

						private bool d;

						public VisNode.Search.PointRadiusData data;

						public VisNode.Search.MaskCompareData viewComp;

						VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
						{
							get
							{
								return this.Current;
							}
						}

						object System.Collections.IEnumerator.Current
						{
							get
							{
								return this.Current;
							}
						}

						public Enumerator(VisNode.Search.PointRadiusData pr, VisNode.Search.MaskCompareData mc)
						{
							this.Current = null;
							this.d = false;
							this.e = VisNode.db.GetEnumerator();
							this.data = pr;
							this.viewComp = mc;
						}

						public void Dispose()
						{
							if (!this.d)
							{
								this.e.Dispose();
								this.d = true;
							}
						}

						public bool MoveNext()
						{
							while (this.e.MoveNext())
							{
								if (!this.Pass(this.e.Current))
								{
									continue;
								}
								return true;
							}
							this.Current = null;
							return false;
						}

						private bool Pass(VisNode cur)
						{
							if (cur.deaf || !this.viewComp.Pass(cur._sightMask) || !this.data.Pass(cur))
							{
								return false;
							}
							this.Current = cur;
							return true;
						}

						public void Reset()
						{
							this.Dispose();
							this.d = false;
							this.e = VisNode.db.GetEnumerator();
						}
					}
				}

				public struct TraitMasked : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Radial.Audible.TraitMasked.Enumerator>
				{
					public Vector3 point;

					public float radius;

					public VisNode.Search.MaskCompareData maskComp;

					public TraitMasked(Vector3 point, float radius, Vis.Mask mask, Vis.Op op)
					{
						this.point = point;
						this.radius = radius;
						this.maskComp = new VisNode.Search.MaskCompareData(op, mask);
					}

					public VisNode.Search.Radial.Audible.TraitMasked.Enumerator GetEnumerator()
					{
						return new VisNode.Search.Radial.Audible.TraitMasked.Enumerator(new VisNode.Search.PointRadiusData(this.point, this.radius), this.maskComp);
					}

					IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
					{
						return this.GetEnumerator();
					}

					IEnumerator System.Collections.IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}

					public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
					{
						public ODBForwardEnumerator<VisNode> e;

						public VisNode Current;

						private bool d;

						public VisNode.Search.PointRadiusData data;

						public VisNode.Search.MaskCompareData traitComp;

						VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
						{
							get
							{
								return this.Current;
							}
						}

						object System.Collections.IEnumerator.Current
						{
							get
							{
								return this.Current;
							}
						}

						public Enumerator(VisNode.Search.PointRadiusData pr, VisNode.Search.MaskCompareData mc)
						{
							this.Current = null;
							this.d = false;
							this.e = VisNode.db.GetEnumerator();
							this.data = pr;
							this.traitComp = mc;
						}

						public void Dispose()
						{
							if (!this.d)
							{
								this.e.Dispose();
								this.d = true;
							}
						}

						public bool MoveNext()
						{
							while (this.e.MoveNext())
							{
								if (!this.Pass(this.e.Current))
								{
									continue;
								}
								return true;
							}
							this.Current = null;
							return false;
						}

						private bool Pass(VisNode cur)
						{
							if (cur.deaf || !this.traitComp.Pass(cur._traitMask) || !this.data.Pass(cur))
							{
								return false;
							}
							this.Current = cur;
							return true;
						}

						public void Reset()
						{
							this.Dispose();
							this.d = false;
							this.e = VisNode.db.GetEnumerator();
						}
					}
				}
			}

			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
			{
				public ODBForwardEnumerator<VisNode> e;

				public VisNode Current;

				private bool d;

				public VisNode.Search.PointRadiusData data;

				VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
				{
					get
					{
						return this.Current;
					}
				}

				object System.Collections.IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				public Enumerator(VisNode.Search.PointRadiusData pr)
				{
					this.Current = null;
					this.d = false;
					this.e = VisNode.db.GetEnumerator();
					this.data = pr;
				}

				public void Dispose()
				{
					if (!this.d)
					{
						this.e.Dispose();
						this.d = true;
					}
				}

				public bool MoveNext()
				{
					while (this.e.MoveNext())
					{
						if (!this.Pass(this.e.Current))
						{
							continue;
						}
						return true;
					}
					this.Current = null;
					return false;
				}

				private bool Pass(VisNode cur)
				{
					if (!this.data.Pass(cur))
					{
						return false;
					}
					this.Current = cur;
					return true;
				}

				public void Reset()
				{
					this.Dispose();
					this.d = false;
					this.e = VisNode.db.GetEnumerator();
				}
			}

			public struct SightMasked : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Radial.SightMasked.Enumerator>
			{
				public Vector3 point;

				public float radius;

				public VisNode.Search.MaskCompareData maskComp;

				public SightMasked(Vector3 point, float radius, Vis.Mask mask, Vis.Op op)
				{
					this.point = point;
					this.radius = radius;
					this.maskComp = new VisNode.Search.MaskCompareData(op, mask);
				}

				public VisNode.Search.Radial.SightMasked.Enumerator GetEnumerator()
				{
					return new VisNode.Search.Radial.SightMasked.Enumerator(new VisNode.Search.PointRadiusData(this.point, this.radius), this.maskComp);
				}

				IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
				{
					public ODBForwardEnumerator<VisNode> e;

					public VisNode Current;

					private bool d;

					public VisNode.Search.PointRadiusData data;

					public VisNode.Search.MaskCompareData viewComp;

					VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
					{
						get
						{
							return this.Current;
						}
					}

					object System.Collections.IEnumerator.Current
					{
						get
						{
							return this.Current;
						}
					}

					public Enumerator(VisNode.Search.PointRadiusData pr, VisNode.Search.MaskCompareData mc)
					{
						this.Current = null;
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
						this.data = pr;
						this.viewComp = mc;
					}

					public void Dispose()
					{
						if (!this.d)
						{
							this.e.Dispose();
							this.d = true;
						}
					}

					public bool MoveNext()
					{
						while (this.e.MoveNext())
						{
							if (!this.Pass(this.e.Current))
							{
								continue;
							}
							return true;
						}
						this.Current = null;
						return false;
					}

					private bool Pass(VisNode cur)
					{
						if (!this.viewComp.Pass(cur._sightMask) || !this.data.Pass(cur))
						{
							return false;
						}
						this.Current = cur;
						return true;
					}

					public void Reset()
					{
						this.Dispose();
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
					}
				}
			}

			public struct TraitMasked : IEnumerable, VisNode.Search.ISearch, IEnumerable<VisNode>, VisNode.Search.ISearch<VisNode.Search.Radial.TraitMasked.Enumerator>
			{
				public Vector3 point;

				public float radius;

				public VisNode.Search.MaskCompareData maskComp;

				public TraitMasked(Vector3 point, float radius, Vis.Mask mask, Vis.Op op)
				{
					this.point = point;
					this.radius = radius;
					this.maskComp = new VisNode.Search.MaskCompareData(op, mask);
				}

				public VisNode.Search.Radial.TraitMasked.Enumerator GetEnumerator()
				{
					return new VisNode.Search.Radial.TraitMasked.Enumerator(new VisNode.Search.PointRadiusData(this.point, this.radius), this.maskComp);
				}

				IEnumerator<VisNode> System.Collections.Generic.IEnumerable<VisNode>.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				public struct Enumerator : IDisposable, IEnumerator, IEnumerator<VisNode>
				{
					public ODBForwardEnumerator<VisNode> e;

					public VisNode Current;

					private bool d;

					public VisNode.Search.PointRadiusData data;

					public VisNode.Search.MaskCompareData traitComp;

					VisNode System.Collections.Generic.IEnumerator<VisNode>.Current
					{
						get
						{
							return this.Current;
						}
					}

					object System.Collections.IEnumerator.Current
					{
						get
						{
							return this.Current;
						}
					}

					public Enumerator(VisNode.Search.PointRadiusData pr, VisNode.Search.MaskCompareData mc)
					{
						this.Current = null;
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
						this.data = pr;
						this.traitComp = mc;
					}

					public void Dispose()
					{
						if (!this.d)
						{
							this.e.Dispose();
							this.d = true;
						}
					}

					public bool MoveNext()
					{
						while (this.e.MoveNext())
						{
							if (!this.Pass(this.e.Current))
							{
								continue;
							}
							return true;
						}
						this.Current = null;
						return false;
					}

					private bool Pass(VisNode cur)
					{
						if (!this.traitComp.Pass(cur._traitMask) || !this.data.Pass(cur))
						{
							return false;
						}
						this.Current = cur;
						return true;
					}

					public void Reset()
					{
						this.Dispose();
						this.d = false;
						this.e = VisNode.db.GetEnumerator();
					}
				}
			}
		}
	}

	private struct TraitHistory
	{
		public int last;

		public bool changed;

		public int Upd(int newTraits)
		{
			int num = newTraits ^ this.last;
			this.changed = num != 0;
			this.last = newTraits;
			return num;
		}
	}

	private struct VisMem
	{
		public ODBSet<VisNode> list;

		public ODBSet<VisNode> last;

		public int count;

		public bool @add;

		public bool rem;

		public bool any;

		public bool had;
	}
}