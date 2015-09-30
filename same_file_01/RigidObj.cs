using Facepunch;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public abstract class RigidObj : IDMain, IInterpTimedEventReceiver
{
	private const string kDoNetworkMethodName = "__invoke_do_network";

	[NonSerialized]
	public Rigidbody rigidbody;

	[NonSerialized]
	protected readonly RigidObj.FeatureFlags featureFlags;

	[SerializeField]
	private float updateRate = 2f;

	private double updateInterval;

	private double serverLastUpdateTimestamp;

	protected Facepunch.NetworkView view;

	private RigidbodyInterpolator _interp;

	private RigidObjServerCollision _serverCollision;

	private bool hasInterp;

	private bool __hiding;

	private bool __done;

	private bool __calling_from_do_network;

	protected Vector3 initialVelocity;

	protected double spawnTime;

	protected uLink.NetworkViewID ownerViewID;

	private Facepunch.NetworkView __ownerView;

	public bool expectsInitialVelocity
	{
		get
		{
			return (byte)(this.featureFlags & RigidObj.FeatureFlags.StreamInitialVelocity) == 1;
		}
	}

	public bool expectsOwner
	{
		get
		{
			return (byte)(this.featureFlags & RigidObj.FeatureFlags.StreamOwnerViewID) == 2;
		}
	}

	public Facepunch.NetworkView ownerView
	{
		get
		{
			Facepunch.NetworkView _OwnerView;
			if (!this.__ownerView)
			{
				Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(this.ownerViewID);
				Facepunch.NetworkView networkView1 = networkView;
				this.__ownerView = networkView;
				_OwnerView = networkView1;
			}
			else
			{
				_OwnerView = this.__ownerView;
			}
			return _OwnerView;
		}
	}

	public bool serverSideCollisions
	{
		get
		{
			return (byte)(this.featureFlags & RigidObj.FeatureFlags.ServerCollisions) == 128;
		}
	}

	public bool showing
	{
		get
		{
			return !this.__hiding;
		}
		protected set
		{
			if (this.__hiding == value)
			{
				this.__hiding = !value;
				if (!this.__hiding)
				{
					this.OnShow();
				}
				else
				{
					this.OnHide();
				}
			}
		}
	}

	protected RigidObj(RigidObj.FeatureFlags classFeatures) : base(IDFlags.Item)
	{
		this.featureFlags = classFeatures;
	}

	private void __invoke_do_network()
	{
		if (this.__calling_from_do_network)
		{
			return;
		}
		try
		{
			this.__calling_from_do_network = true;
			this.DoNetwork();
		}
		finally
		{
			this.__calling_from_do_network = false;
		}
	}

	protected void Awake()
	{
		this.rigidbody = base.rigidbody;
		this._interp = base.GetComponent<RigidbodyInterpolator>();
	}

	protected virtual void DoNetwork()
	{
		base.networkView.RPC("RecieveNetwork", uLink.RPCMode.AllExceptOwner, new object[] { this.rigidbody.position, this.rigidbody.rotation });
		this.serverLastUpdateTimestamp = NetCull.time;
	}

	void IInterpTimedEventReceiver.OnInterpTimedEvent()
	{
		if (!this.OnInterpTimedEvent())
		{
			InterpTimedEvent.MarkUnhandled();
		}
	}

	protected abstract void OnDone();

	protected abstract void OnHide();

	protected virtual bool OnInterpTimedEvent()
	{
		int num;
		string tag = InterpTimedEvent.Tag;
		if (tag != null)
		{
			if (RigidObj.<>f__switch$map5 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(2)
				{
					{ "_init", 0 },
					{ "_done", 1 }
				};
				RigidObj.<>f__switch$map5 = strs;
			}
			if (RigidObj.<>f__switch$map5.TryGetValue(tag, out num))
			{
				if (num == 0)
				{
					this.showing = true;
					if (this.expectsInitialVelocity)
					{
						this.rigidbody.isKinematic = false;
						this.rigidbody.velocity = this.initialVelocity;
					}
					return true;
				}
				if (num == 1)
				{
					try
					{
						this.OnDone();
					}
					finally
					{
						try
						{
							this.showing = false;
						}
						finally
						{
							UnityEngine.Object.Destroy(base.gameObject);
						}
					}
					return true;
				}
			}
		}
		return false;
	}

	internal void OnServerCollision(byte kind, Collision collision)
	{
		switch (kind)
		{
			case 0:
			{
				this.OnServerCollisionEnter(collision);
				break;
			}
			case 1:
			{
				this.OnServerCollisionExit(collision);
				break;
			}
			case 2:
			{
				this.OnServerCollisionStay(collision);
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
		}
	}

	protected virtual void OnServerCollisionEnter(Collision collision)
	{
	}

	protected virtual void OnServerCollisionExit(Collision collision)
	{
	}

	protected virtual void OnServerCollisionStay(Collision collision)
	{
	}

	protected abstract void OnShow();

	[RPC]
	protected void RecieveNetwork(Vector3 pos, Quaternion rot, uLink.NetworkMessageInfo info)
	{
		PosRot posRot = new PosRot();
		if (this.hasInterp && this._interp)
		{
			posRot.position = pos;
			posRot.rotation = rot;
			this.rigidbody.isKinematic = true;
			this._interp.SetGoals(posRot, info.timestamp);
			this._interp.running = true;
		}
	}

	[Obsolete("Do not call manually")]
	[RPC]
	protected void RODone(uLink.NetworkMessageInfo info)
	{
		if (!this.__done)
		{
			NetCull.DontDestroyWithNetwork(this);
			InterpTimedEvent.Queue(this, "_done", ref info);
		}
	}

	protected void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		PosRot posRot = new PosRot();
		this.view = (Facepunch.NetworkView)info.networkView;
		uLink.BitStream bitStream = this.view.initialData;
		if (this.expectsInitialVelocity)
		{
			this.initialVelocity = bitStream.ReadVector3();
		}
		if (this.expectsOwner)
		{
			this.ownerViewID = bitStream.ReadNetworkViewID();
		}
		this.spawnTime = info.timestamp;
		this.updateInterval = 1 / ((double)NetCull.sendRate * (double)Mathf.Max(1f, this.updateRate));
		this.hasInterp = this._interp;
		if (this.hasInterp)
		{
			this._interp.running = false;
		}
		this.rigidbody.isKinematic = true;
		this.__hiding = this.spawnTime > Interpolation.time;
		if (!this.__hiding)
		{
			this.OnShow();
		}
		else
		{
			this.OnHide();
			if (this.hasInterp)
			{
				posRot.position = this.view.position;
				posRot.rotation = this.view.rotation;
				this._interp.SetGoals(posRot, this.spawnTime);
			}
			InterpTimedEvent.Queue(this, "_init", ref info);
		}
	}

	[Flags]
	protected enum FeatureFlags : byte
	{
		StreamInitialVelocity = 1,
		StreamOwnerViewID = 2,
		ServerCollisions = 128
	}
}