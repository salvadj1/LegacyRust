using Facepunch;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public class BasicWildLifeAI : NetBehaviour, IInterpTimedEventReceiver
{
	private const string RPCName_GetNetworkUpdate = "GetNetworkUpdate";

	private const string RPCName_Snd = "Snd";

	private const string RPCName_ClientHealthChange = "ClientHealthChange";

	private const string RPCName_ClientDeath = "ClientDeath";

	public bool afraidOfFootsteps = true;

	public bool afraidOfDanger = true;

	[SerializeField]
	protected AudioClipArray idleSounds;

	[SerializeField]
	protected AudioClipArray fleeStartSounds;

	[SerializeField]
	protected AudioClipArray deathSounds;

	[SerializeField]
	protected float walkSpeed = 1f;

	[SerializeField]
	protected float runSpeed = 3f;

	[SerializeField]
	protected float walkAnimScalar = 1f;

	[SerializeField]
	protected float runAnimScalar = 1f;

	protected Transform _myTransform;

	protected TakeDamage _takeDamage;

	protected BaseAIMovement _wildMove;

	protected TransformInterpolator _interp;

	public new Transform transform
	{
		get
		{
			return this._myTransform;
		}
	}

	public BasicWildLifeAI()
	{
	}

	protected void Awake()
	{
		this._myTransform = base.transform;
		this._takeDamage = base.GetComponent<TakeDamage>();
		this._wildMove = base.GetComponent<BaseAIMovement>();
		this._interp = base.GetComponent<TransformInterpolator>();
		UnityEngine.Object.Destroy(base.GetComponent<BasicWildLifeMovement>());
		UnityEngine.Object.Destroy(base.GetComponent<VisNode>());
		this._takeDamage.enabled = false;
	}

	[RPC]
	protected void ClientDeath(Vector3 deadPos, uLink.NetworkViewID attackerID, uLink.NetworkMessageInfo info)
	{
		this.OnClientDeath(ref deadPos, attackerID, ref info);
	}

	[RPC]
	protected void ClientHealthChange(float newHealth)
	{
		this._takeDamage.health <= newHealth;
		this._takeDamage.health = newHealth;
	}

	protected void DoClientDeath()
	{
		base.animation[this.GetDeathAnim()].wrapMode = WrapMode.ClampForever;
		base.animation.CrossFade(this.GetDeathAnim(), 0.2f);
		this._takeDamage.health = 0f;
	}

	public virtual string GetDeathAnim()
	{
		return "death";
	}

	protected float GetMoveSpeedForAnim()
	{
		Vector3 vector3;
		this._interp.SampleWorldVelocity(out vector3);
		return vector3.magnitude;
	}

	[RPC]
	protected void GetNetworkUpdate(Vector3 pos, Angle2 rot, uLink.NetworkMessageInfo info)
	{
		Quaternion quaternion = (Quaternion)rot;
		this.OnNetworkUpdate(ref pos, ref quaternion, ref info);
	}

	protected float GetRunAnimScalar()
	{
		return this.runAnimScalar;
	}

	protected float GetWalkAnimScalar()
	{
		return this.walkAnimScalar;
	}

	void IInterpTimedEventReceiver.OnInterpTimedEvent()
	{
		if (!this.OnInterpTimedEvent())
		{
			InterpTimedEvent.MarkUnhandled();
		}
	}

	protected void OnClientDeath(ref Vector3 deathPosition, uLink.NetworkViewID attackerNetViewID, ref uLink.NetworkMessageInfo info)
	{
		Vector3 vector3;
		Vector3 vector31;
		TransformHelpers.GetGroundInfo(deathPosition + new Vector3(0f, 0.25f, 0f), 10f, out vector3, out vector31);
		deathPosition = vector3;
		Quaternion quaternion = TransformHelpers.LookRotationForcedUp(this._myTransform.rotation * Vector3.forward, vector31);
		this._interp.SetGoals(deathPosition, quaternion, info.timestamp);
		if (!attackerNetViewID.isMine)
		{
			InterpTimedEvent.Queue(this, "DEATH", ref info);
		}
		else
		{
			this.DoClientDeath();
		}
	}

	protected void OnDestroy()
	{
		InterpTimedEvent.Remove(this);
	}

	protected virtual bool OnInterpTimedEvent()
	{
		int num;
		string tag = InterpTimedEvent.Tag;
		if (tag != null)
		{
			if (BasicWildLifeAI.<>f__switch$map7 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(2)
				{
					{ "DEATH", 0 },
					{ "SOUND", 1 }
				};
				BasicWildLifeAI.<>f__switch$map7 = strs;
			}
			if (BasicWildLifeAI.<>f__switch$map7.TryGetValue(tag, out num))
			{
				if (num == 0)
				{
					this.DoClientDeath();
					return true;
				}
				if (num == 1)
				{
					this.PlaySnd(InterpTimedEvent.Argument<int>(0));
					return true;
				}
			}
		}
		return false;
	}

	protected void OnNetworkUpdate(ref Vector3 origin, ref Quaternion rotation, ref uLink.NetworkMessageInfo info)
	{
		this._wildMove.ProcessNetworkUpdate(ref origin, ref rotation);
		this._interp.SetGoals(origin, rotation, info.timestamp);
	}

	protected virtual bool PlaySnd(int type)
	{
		AudioClip item = null;
		float single = 1f;
		float single1 = 5f;
		float single2 = 20f;
		if (type == 0)
		{
			if (this.idleSounds != null)
			{
				item = this.idleSounds[UnityEngine.Random.Range(0, this.idleSounds.Length)];
			}
			single = 0.4f;
			single1 = 0.25f;
			single2 = 8f;
		}
		else if (type != 3)
		{
			if (type != 4)
			{
				return false;
			}
			if (this.deathSounds != null)
			{
				item = this.deathSounds[UnityEngine.Random.Range(0, this.deathSounds.Length)];
			}
			single = 1f;
			single1 = 2.25f;
			single2 = 20f;
		}
		else
		{
			if (this.fleeStartSounds != null)
			{
				item = this.fleeStartSounds[UnityEngine.Random.Range(0, this.fleeStartSounds.Length)];
			}
			single = 0.9f;
			single1 = 1.25f;
			single2 = 10f;
		}
		if (item)
		{
			item.PlayLocal(this.transform, Vector3.zero, single, single1, single2);
		}
		return true;
	}

	[RPC]
	protected void Snd(byte type, uLink.NetworkMessageInfo info)
	{
		try
		{
			InterpTimedEvent.Queue(this, "SOUND", ref info, new object[] { (int)type });
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogWarning("Running emergency dump because of previous exception in Snd", this);
			InterpTimedEvent.EMERGENCY_DUMP(true);
		}
	}

	protected void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		this._interp.running = true;
	}

	public enum AISound : byte
	{
		Idle,
		Warn,
		Attack,
		Afraid,
		Death,
		Chase,
		ChaseClose
	}
}