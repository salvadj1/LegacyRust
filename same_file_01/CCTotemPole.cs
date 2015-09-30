using System;
using UnityEngine;

public sealed class CCTotemPole : CCTotem<CCTotem.TotemPole, CCTotemPole>
{
	[SerializeField]
	private CCDesc prefab;

	[SerializeField]
	private float minimumHeight = 0.6f;

	[SerializeField]
	private float maximumHeight = 2.08f;

	[SerializeField]
	private float initialHeightFraction = 1f;

	[SerializeField]
	private float bottomBufferUnits = 0.1f;

	[NonSerialized]
	private bool HasLastGoodConfiguration;

	[NonSerialized]
	private CCTotem.Configuration LastGoodConfiguration;

	[NonSerialized]
	private CCTotem.ConfigurationBinder ConfigurationBinder;

	[NonSerialized]
	public ArgumentException LastException;

	[NonSerialized]
	public object Tag;

	private CCTotem.PositionBinder OnBindPosition;

	public Vector3 center
	{
		get
		{
			return (!this.Exists ? this.prefab.center : this.totemicObject.center);
		}
	}

	public CollisionFlags collisionFlags
	{
		get
		{
			return (!this.Exists ? CollisionFlags.None : this.totemicObject.collisionFlags);
		}
	}

	public bool Exists
	{
		get
		{
			return !object.ReferenceEquals(this.totemicObject, null);
		}
	}

	[Obsolete("this is the height of the character controller. prefer this.Height")]
	public float height
	{
		get
		{
			return (!this.Exists ? this.prefab.height : this.totemicObject.height);
		}
	}

	public float Height
	{
		get
		{
			return (!this.Exists ? this.minimumHeight + this.initialHeightFraction * (this.maximumHeight - this.minimumHeight) : this.totemicObject.Expansion.Value);
		}
	}

	public bool isGrounded
	{
		get
		{
			return (!this.Exists ? false : this.totemicObject.isGrounded);
		}
	}

	public float MaximumHeight
	{
		get
		{
			return this.maximumHeight;
		}
	}

	private CCTotem.Initialization Members
	{
		get
		{
			return new CCTotem.Initialization(this, this.prefab, this.minimumHeight, this.maximumHeight, this.minimumHeight + (this.maximumHeight - this.minimumHeight) * this.initialHeightFraction, this.bottomBufferUnits);
		}
	}

	public float MinimumHeight
	{
		get
		{
			return this.minimumHeight;
		}
	}

	public float radius
	{
		get
		{
			return (!this.Exists ? this.prefab.radius : this.totemicObject.radius);
		}
	}

	public float slopeLimit
	{
		get
		{
			return (!this.Exists ? this.prefab.slopeLimit : this.totemicObject.slopeLimit);
		}
	}

	public float stepOffset
	{
		get
		{
			return (!this.Exists ? this.prefab.stepOffset : this.totemicObject.stepOffset);
		}
	}

	public Vector3 velocity
	{
		get
		{
			return (!this.Exists ? Vector3.zero : this.totemicObject.velocity);
		}
	}

	public CCTotemPole()
	{
	}

	private void Awake()
	{
		if (this.UpdateConfiguration())
		{
			this.CreatePhysics();
			return;
		}
		Debug.LogException(this.LastException, this);
	}

	private void BindPositions(CCTotem.PositionPlacement PositionPlacement)
	{
		if (this.OnBindPosition != null)
		{
			try
			{
				this.OnBindPosition(ref PositionPlacement, this.Tag);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
	}

	private void CreatePhysics()
	{
		if (!this.HasLastGoodConfiguration && !this.UpdateConfiguration())
		{
			Debug.LogException(this.LastException, this);
			return;
		}
		base.AssignTotemicObject(new CCTotem.TotemPole(ref this.LastGoodConfiguration));
		this.totemicObject.Create();
	}

	internal void DestroyCCDesc(ref CCDesc CCDesc)
	{
		if (CCDesc)
		{
			CCDesc cCDesc = CCDesc;
			CCDesc = null;
			this.ExecuteBinding(cCDesc, false);
			UnityEngine.Object.Destroy(cCDesc.gameObject);
		}
	}

	internal void ExecuteAllBindings(bool Bind)
	{
		if (this.Exists)
		{
			this.ExecuteBinding(this.totemicObject.CCDesc, Bind);
			for (int i = 0; i < this.totemicObject.Configuration.numRequiredTotemicFigures; i++)
			{
				this.ExecuteBinding(this.totemicObject.TotemicFigures[i].CCDesc, Bind);
			}
		}
	}

	internal void ExecuteBinding(CCDesc CCDesc, bool Bind)
	{
		if (CCDesc && !object.ReferenceEquals(this.ConfigurationBinder, null))
		{
			try
			{
				this.ConfigurationBinder(Bind, CCDesc, this.Tag);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
	}

	public CCTotem.MoveInfo Move(Vector3 motion)
	{
		return this.Move(motion, this.Height);
	}

	public CCTotem.MoveInfo Move(Vector3 motion, float height)
	{
		CCTotem.TotemPole totemPole = this.totemicObject;
		if (object.ReferenceEquals(totemPole, null))
		{
			throw new InvalidOperationException("Exists == false");
		}
		CCTotem.MoveInfo moveInfo = totemPole.Move(motion, height);
		this.BindPositions(moveInfo.PositionPlacement);
		return moveInfo;
	}

	private new void OnDestroy()
	{
		try
		{
			base.OnDestroy();
		}
		finally
		{
			this.OnBindPosition = null;
			this.ConfigurationBinder = null;
			this.Tag = null;
		}
	}

	public bool SmudgeTo(Vector3 worldSkinnedBottom)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		if (!this.Exists)
		{
			return false;
		}
		Vector3 vector32 = base.transform.position;
		if (vector32 == worldSkinnedBottom)
		{
			return true;
		}
		Vector3 vector33 = worldSkinnedBottom - vector32;
		CCDesc cCDesc = this.totemicObject.CCDesc;
		if (!cCDesc)
		{
			return false;
		}
		float single = 0f;
		float single1 = single;
		vector3.z = single;
		vector3.x = single1;
		vector3.y = cCDesc.effectiveHeight * 0.5f - cCDesc.radius;
		Vector3 vector34 = cCDesc.center;
		Vector3 world = cCDesc.OffsetToWorld(vector34 - vector3);
		Vector3 world1 = cCDesc.OffsetToWorld(vector34 + vector3);
		Vector3 world2 = cCDesc.OffsetToWorld(vector34 + new Vector3(cCDesc.skinnedRadius, 0f, 0f)) - cCDesc.worldCenter;
		float single2 = world2.magnitude;
		float single3 = vector33.magnitude;
		float single4 = 1f / single3;
		vector31.x = vector33.x * single4;
		vector31.y = vector33.y * single4;
		vector31.z = vector33.z * single4;
		int num = 0;
		int num1 = base.gameObject.layer;
		for (int i = 0; i < 32; i++)
		{
			if (!Physics.GetIgnoreLayerCollision(num1, i))
			{
				num = num | 1 << (i & 31 & 31);
			}
		}
		if (Physics.CapsuleCast(world, world1, single2, vector31, single3, num))
		{
			return false;
		}
		Transform transforms = this.totemicObject.CCDesc.transform;
		transforms.position = transforms.position + vector33;
		for (int j = 0; j < this.totemicObject.Configuration.numRequiredTotemicFigures; j++)
		{
			Transform cCDesc1 = this.totemicObject.TotemicFigures[j].CCDesc.transform;
			cCDesc1.position = cCDesc1.position + vector33;
		}
		Vector3 cCDesc2 = this.totemicObject.CCDesc.worldSkinnedBottom;
		Vector3 cCDesc3 = this.totemicObject.CCDesc.worldSkinnedTop;
		Vector3 cCDesc4 = this.totemicObject.CCDesc.transform.position;
		CCTotem.Configuration configuration = this.totemicObject.Configuration;
		this.BindPositions(new CCTotem.PositionPlacement(cCDesc2, cCDesc3, cCDesc4, configuration.poleExpandedHeight));
		return true;
	}

	public void Teleport(Vector3 origin)
	{
		if (this.Exists)
		{
			base.ClearTotemicObject();
		}
		base.transform.position = origin;
		this.CreatePhysics();
	}

	public bool UpdateConfiguration()
	{
		bool flag;
		this.LastException = null;
		CCTotem.Initialization members = this.Members;
		try
		{
			this.LastGoodConfiguration = new CCTotem.Configuration(ref members);
			this.HasLastGoodConfiguration = true;
			flag = true;
		}
		catch (ArgumentException argumentException)
		{
			this.LastException = argumentException;
			flag = false;
		}
		return flag;
	}

	public event CCTotem.PositionBinder OnBindPosition
	{
		add
		{
			this.OnBindPosition += value;
		}
		remove
		{
			this.OnBindPosition -= value;
		}
	}

	public event CCTotem.ConfigurationBinder OnConfigurationBinding
	{
		add
		{
			if (!object.ReferenceEquals(this.ConfigurationBinder, value))
			{
				if (!object.ReferenceEquals(this.ConfigurationBinder, null))
				{
					this.ExecuteAllBindings(false);
					this.ConfigurationBinder = null;
				}
				this.ConfigurationBinder = value;
				this.ExecuteAllBindings(true);
			}
		}
		remove
		{
			if (object.ReferenceEquals(this.ConfigurationBinder, value))
			{
				this.ExecuteAllBindings(false);
				if (object.ReferenceEquals(this.ConfigurationBinder, value))
				{
					this.ConfigurationBinder = null;
				}
			}
		}
	}
}