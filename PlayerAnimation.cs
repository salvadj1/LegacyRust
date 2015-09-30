using Facepunch.Movement;
using Facepunch.Precision;
using System;
using UnityEngine;

public class PlayerAnimation : IDLocalCharacter
{
	public const double MIN_ANIM_SPEED = 0.05;

	[PrefetchComponent]
	public Animation animation;

	[PrefetchComponent]
	public InventoryHolder itemHolder;

	private Transform characterTransform;

	private Vector3 localVelocity;

	private Vector3 lastPos;

	private Vector2 movementNormal;

	private Vector4 times;

	private Weights lastHeadingWeights;

	private Weights baseDecay;

	private Vector3G localVelocityPrecise;

	private Vector3G lastPosPrecise;

	private Vector2G movementNormalPrecise;

	private double speedPrecise;

	private double anglePrecise;

	private double lastAngleSpeedPrecise;

	private float speed;

	private float angle;

	private float positionTime;

	private float lastUnitScale;

	private float lastVelocityCalc;

	private Sampler movement;

	private bool wasAirborne;

	private bool decaying;

	private Configuration configuration;

	[NonSerialized]
	private string idealGroupName;

	[NonSerialized]
	private string usingGroupName;

	[NonSerialized]
	private int usingGroupIndex;

	[NonSerialized]
	private CharacterAnimationTrait animationTrait;

	private bool _madeItemAttachment;

	private int group_unarmed;

	private int group_armed = 1;

	[NonSerialized]
	private Socket.LocalSpace _itemAttachmentSocket;

	public Socket.LocalSpace itemAttachment
	{
		get
		{
			if (!this._madeItemAttachment && base.idMain)
			{
				Socket.ConfigBodyPart trait = base.GetTrait<CharacterItemAttachmentTrait>().socket;
				if (trait == null)
				{
					return null;
				}
				this._madeItemAttachment = trait.Extract(ref this._itemAttachmentSocket, base.idMain.hitBoxSystem);
			}
			return this._itemAttachmentSocket;
		}
	}

	public PlayerAnimation()
	{
	}

	private void Awake()
	{
		if (!this.animation)
		{
			Animation animations = base.animation;
			Animation animations1 = animations;
			this.animation = animations;
			if (!animations1)
			{
				Debug.LogError("There must be a animation component defined!", this);
			}
		}
		this.animationTrait = base.GetTrait<CharacterAnimationTrait>();
		if (!this.animationTrait.movementAnimationSetup.CreateSampler(this.animation, out this.movement))
		{
			Debug.LogError("Failed to make movement sampler", this);
		}
	}

	private void CalculateVelocity()
	{
		Vector3G vector3G = new Vector3G();
		double num = (double)Time.time - (double)this.lastVelocityCalc;
		Character character = base.idMain;
		Vector3 vector3 = (!character ? base.transform.position : character.origin);
		Vector3G vector3G1 = new Vector3G(ref vector3);
		double num1 = 1 / num;
		vector3G.x = num1 * (vector3G1.x - this.lastPosPrecise.x);
		vector3G.y = num1 * (vector3G1.y - this.lastPosPrecise.y);
		vector3G.z = num1 * (vector3G1.z - this.lastPosPrecise.z);
		Matrix4x4G matrix4x4G = new Matrix4x4G(base.transform.worldToLocalMatrix);
		Matrix4x4G.Mult3x3(ref vector3G, ref matrix4x4G, out this.localVelocityPrecise);
		this.lastVelocityCalc = Time.time;
		this.speedPrecise = Math.Sqrt(this.localVelocityPrecise.x * this.localVelocityPrecise.x + this.localVelocityPrecise.z * this.localVelocityPrecise.z);
		if (this.speedPrecise >= (double)this.movement.configuration.minMoveSpeed)
		{
			double num2 = 1 / this.speedPrecise;
			this.movementNormalPrecise.x = (double)this.localVelocity.x * num2;
			this.movementNormalPrecise.y = (double)this.localVelocity.z * num2;
			double num3 = this.anglePrecise;
			this.anglePrecise = Math.Atan2(this.movementNormalPrecise.x, this.movementNormalPrecise.y) / 3.14159265358979 * 180;
			float single = this.movement.configuration.maxTurnSpeed;
			if (single > 0f && this.anglePrecise != num3 && this.lastAngleSpeedPrecise >= 0.05)
			{
				double num4 = (double)Time.deltaTime * (double)single;
				if (Precise.MoveTowardsAngle(ref num3, ref this.anglePrecise, ref num4, out this.anglePrecise))
				{
					double num5 = this.anglePrecise / 180 * 3.14159265358979;
					this.movementNormalPrecise.x = Math.Sin(num5);
					this.movementNormalPrecise.y = Math.Cos(num5);
				}
			}
			this.lastAngleSpeedPrecise = this.speedPrecise;
		}
		else
		{
			this.speedPrecise = 0;
			this.movementNormalPrecise.x = 0;
			this.movementNormalPrecise.y = 0;
			if (this.lastAngleSpeedPrecise > 0)
			{
				float single1 = this.movement.configuration.maxTimeBetweenTurns;
				float single2 = single1;
				if (single1 > 0f)
				{
					PlayerAnimation playerAnimation = this;
					playerAnimation.lastAngleSpeedPrecise = playerAnimation.lastAngleSpeedPrecise - (double)(Time.deltaTime / single2);
				}
			}
		}
		this.lastPosPrecise = vector3G1;
		this.lastPos = vector3;
		this.movementNormal = this.movementNormalPrecise.f;
		this.speed = (float)this.speedPrecise;
		this.angle = (float)this.anglePrecise;
		this.localVelocity = this.localVelocityPrecise.f;
	}

	private void OnDestroy()
	{
		this.movement = null;
	}

	private void OnDrawGizmosSelected()
	{
		if (this._itemAttachmentSocket == null)
		{
			Socket.ConfigBodyPart trait = base.GetTrait<CharacterItemAttachmentTrait>().socket;
			if (trait != null)
			{
				try
				{
					if (trait.Extract(ref PlayerAnimation.EditorHelper.tempSocketForGizmos, base.GetComponentInChildren<HitBoxSystem>()))
					{
						PlayerAnimation.EditorHelper.tempSocketForGizmos.DrawGizmos("itemAttachment");
					}
				}
				finally
				{
					if (PlayerAnimation.EditorHelper.tempSocketForGizmos != null)
					{
						PlayerAnimation.EditorHelper.tempSocketForGizmos.parent = null;
					}
				}
			}
		}
		else
		{
			this.itemAttachment.DrawGizmos("itemAttachment");
		}
	}

	public bool PlayAnimation(Facepunch.Movement.GroupEvent GroupEvent, float animationSpeed, float animationTime)
	{
		AnimationState animationState;
		bool flag;
		if (this.movement == null)
		{
			Debug.Log("no Movement");
			return false;
		}
		try
		{
			if (this.movement.GetGroupEvent(GroupEvent, out animationState))
			{
				if (animationTime >= 0f)
				{
					animationState.normalizedTime = animationTime;
				}
				else
				{
					animationState.time = -animationTime;
				}
				if (!this.animation.Play(animationState.name, PlayMode.StopSameLayer))
				{
					return false;
				}
				if (animationState.speed != animationSpeed)
				{
					animationState.speed = animationSpeed;
				}
				return true;
			}
			else
			{
				flag = false;
			}
		}
		catch (NotImplementedException notImplementedException)
		{
			Debug.LogException(notImplementedException, this);
			flag = false;
		}
		return flag;
	}

	public bool PlayAnimation(Facepunch.Movement.GroupEvent GroupEvent, float animationSpeed)
	{
		return this.PlayAnimation(GroupEvent, animationSpeed, 0f);
	}

	public bool PlayAnimation(Facepunch.Movement.GroupEvent GroupEvent)
	{
		return this.PlayAnimation(GroupEvent, 1f, 0f);
	}

	[ContextMenu("Rebind Item Attachment")]
	private void RebindItemAttachment()
	{
		if (this._itemAttachmentSocket != null)
		{
			this._itemAttachmentSocket.eulerRotate = base.GetTrait<CharacterItemAttachmentTrait>().socket.eulerRotate;
			this._itemAttachmentSocket.offset = base.GetTrait<CharacterItemAttachmentTrait>().socket.offset;
		}
	}

	private void Start()
	{
		Character character = base.idMain;
		this.lastPos = (!character ? base.transform.position : character.origin);
		this.lastPosPrecise.f = this.lastPos;
	}

	private void Update()
	{
		CharacterStateFlags characterStateFlag;
		Weights weight = new Weights();
		State state;
		float single;
		double num;
		float single1;
		this.CalculateVelocity();
		Character character = base.idMain;
		bool flag = character;
		characterStateFlag = (!flag ? new CharacterStateFlags() : character.stateFlags);
		bool flag1 = !characterStateFlag.grounded;
		bool flag2 = characterStateFlag.focus;
		bool flag3 = characterStateFlag.crouch;
		weight.idle = 0f;
		if (this.movementNormal.x > 0f)
		{
			weight.east = this.movementNormal.x;
			weight.west = 0f;
		}
		else if (this.movementNormal.x >= 0f)
		{
			float single2 = 0f;
			single1 = single2;
			weight.west = single2;
			weight.east = single1;
		}
		else
		{
			weight.east = 0f;
			weight.west = -this.movementNormal.x;
		}
		if (this.movementNormal.y > 0f)
		{
			weight.north = this.movementNormal.y;
			weight.south = 0f;
		}
		else if (this.movementNormal.y >= 0f)
		{
			float single3 = 0f;
			single1 = single3;
			weight.south = single3;
			weight.north = single1;
		}
		else
		{
			weight.north = 0f;
			weight.south = -this.movementNormal.y;
		}
		if (this.movementNormal.y == 0f && this.movementNormal.x == 0f)
		{
			weight = this.lastHeadingWeights;
		}
		weight.idle = 0f;
		this.lastHeadingWeights = weight;
		if (flag1)
		{
			state = State.Walk;
		}
		else if (!flag3)
		{
			state = (!characterStateFlag.sprint || this.speedPrecise < (double)this.movement.configuration.runSpeed ? State.Walk : State.Run);
		}
		else
		{
			state = State.Crouch;
		}
		string str = this.itemHolder.animationGroupName;
		if (this.idealGroupName != str)
		{
			this.idealGroupName = str;
			str = str ?? this.animationTrait.defaultGroupName;
			int? nullable = this.movement.configuration.GroupIndex(str);
			if (nullable.HasValue)
			{
				this.usingGroupName = this.idealGroupName;
				this.usingGroupIndex = nullable.Value;
			}
			else
			{
				Debug.LogWarning(string.Concat("Could not find group name ", this.idealGroupName));
				this.usingGroupName = this.animationTrait.defaultGroupName;
				int? nullable1 = this.movement.configuration.GroupIndex(this.usingGroupName);
				this.usingGroupIndex = (!nullable1.HasValue ? 0 : nullable1.Value);
			}
		}
		int num1 = this.usingGroupIndex;
		if (characterStateFlag.slipping)
		{
			num = (double)(-Time.deltaTime);
			single = this.lastUnitScale;
		}
		else
		{
			num = (double)Time.deltaTime;
			this.movement.state = state;
			this.movement.@group = num1;
			single = this.movement.UpdateWeights(Time.deltaTime, flag1, (!flag ? true : characterStateFlag.movement));
		}
		this.wasAirborne = flag1;
		this.lastUnitScale = single;
		if (!double.IsNaN(this.speedPrecise) && !double.IsInfinity(this.speedPrecise))
		{
			float single4 = this.positionTime;
			this.positionTime = (float)(((double)this.positionTime + Math.Abs((double)single * this.speedPrecise * num)) % 1);
			if (this.positionTime < 0f)
			{
				PlayerAnimation playerAnimation = this;
				playerAnimation.positionTime = playerAnimation.positionTime + 1f;
			}
			else if (float.IsNaN(this.positionTime) || float.IsInfinity(this.positionTime))
			{
				this.positionTime = single4;
			}
			this.movement.configuration.OffsetTime(this.positionTime, out this.times);
		}
		float single5 = (!flag ? -base.transform.eulerAngles.x : character.eyesPitch);
		this.movement.SetWeights(this.animation, ref weight, ref this.times, single5);
	}

	private static class EditorHelper
	{
		public static Socket.LocalSpace tempSocketForGizmos;
	}
}