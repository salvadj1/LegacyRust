using Facepunch.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[AddComponentMenu("ID/Local/CCMotor")]
public sealed class CCMotor : IDRemote
{
	private const float kYEpsilon = 0.001f;

	private const float kYMaxNotGrounded = 0.01f;

	private const float kResetButtonDownTime = -100f;

	private const float kJumpButtonDelaySeconds = 0.2f;

	private const float kHitEpsilon = 0.001f;

	private CCTotemPole cc;

	internal Transform tr;

	public CCMotor.StepMode stepMode;

	internal bool canControl;

	internal bool sendFallMessage;

	internal bool sendLandMessage;

	internal bool sendJumpMessage;

	internal bool sendExternalVelocityMessage;

	internal bool sendJumpFailureMessage;

	private bool _grounded;

	private bool _installed;

	[NonSerialized]
	public CCTotem.PositionPlacement? LastPositionPlacement;

	private CCMotor.YawAngle currentYaw;

	private CCMotor.YawAngle previousYaw;

	public float minTimeBetweenJumps;

	private Vector3 _groundNormal;

	private Vector3 _lastGroundNormal;

	[SerializeField]
	private CCMotorSettings _settings;

	public CCMotor.InputFrame input;

	public CCMotor.MovementContext movement = new CCMotor.MovementContext(CCMotor.Movement.init);

	private CCMotor.JumpBaseVerticalSpeedArgs jumpVerticalSpeedCalculator;

	public CCMotor.JumpingContext jumping = new CCMotor.JumpingContext(CCMotor.Jumping.init);

	public CCMotor.MovingPlatformContext movingPlatform = new CCMotor.MovingPlatformContext(CCMotor.MovingPlatform.init);

	public CCMotor.Sliding sliding;

	private StringBuilder stringBuilder;

	private static bool ccmotor_debug;

	private float baseHeightVerticalSpeed
	{
		get
		{
			return this.jumpVerticalSpeedCalculator.CalculateVerticalSpeed(ref this.jumping.setup, ref this.movement.setup);
		}
	}

	public CCMotor ccmotor
	{
		get
		{
			return this;
		}
	}

	public CCTotemPole ccTotemPole
	{
		get
		{
			return this.cc;
		}
	}

	private CCMotor.YawAngle characterYawAngle
	{
		get
		{
			Character character = (Character)base.idMain;
			return character.eyesYaw + Mathf.DeltaAngle(this.previousYaw.Degrees, this.currentYaw.Degrees);
		}
	}

	public Vector3 currentGroundNormal
	{
		get
		{
			return this._groundNormal;
		}
	}

	public Vector3 currentHitPoint
	{
		get
		{
			return this.movement.hitPoint;
		}
	}

	public Vector3 differentVelocity
	{
		get
		{
			return this.movement.velocity;
		}
		set
		{
			if (this.movement.velocity.x != value.x || this.movement.velocity.y != value.y || this.movement.velocity.z != value.z)
			{
				this.velocity = value;
			}
		}
	}

	public Vector3 direction
	{
		get
		{
			return this.input.moveDirection;
		}
	}

	public bool driveable
	{
		get
		{
			return this.canControl;
		}
		set
		{
			this.canControl = value;
		}
	}

	public Vector3? fallbackCurrentGroundNormal
	{
		get
		{
			if (this._grounded)
			{
				return new Vector3?(this._groundNormal);
			}
			return null;
		}
	}

	public Vector3? fallbackPreviousGroundNormal
	{
		get
		{
			if (this._lastGroundNormal.x == 0f && this._lastGroundNormal.y == 0f && this._lastGroundNormal.z == 0f)
			{
				return null;
			}
			return new Vector3?(this._lastGroundNormal);
		}
	}

	public bool isCrouchBlocked
	{
		get
		{
			return this.movement.crouchBlocked;
		}
	}

	public bool isGrounded
	{
		get
		{
			return this._grounded;
		}
	}

	public bool isJumping
	{
		get
		{
			return this.jumping.jumping;
		}
	}

	public bool isSliding
	{
		get
		{
			return (!this._grounded || !this.sliding.enable ? false : this.tooSteep);
		}
	}

	public bool isTouchingCeiling
	{
		get
		{
			return (this.movement.collisionFlags & CollisionFlags.Above) == CollisionFlags.Above;
		}
	}

	public bool movingWithPlatform
	{
		get
		{
			return (!this.movingPlatform.setup.enable || !this._grounded && this.movingPlatform.setup.movementTransfer != CCMotor.JumpMovementTransfer.PermaLocked ? false : this.movingPlatform.activePlatform != null);
		}
	}

	public Vector3 previousGroundNormal
	{
		get
		{
			return this._lastGroundNormal;
		}
	}

	public Vector3 previousHitPoint
	{
		get
		{
			return this.movement.lastHitPoint;
		}
	}

	public CCMotorSettings settings
	{
		get
		{
			return this._settings;
		}
		set
		{
			if (value != this._settings)
			{
				this._settings = value;
				if (Application.isPlaying)
				{
					value.BindSettingsTo(this);
				}
			}
		}
	}

	public string setupString
	{
		get
		{
			return string.Format("movement={0}, jumping={1}, sliding={2}, movingPlatform={3}", new object[] { this.movement.setup, this.jumping.setup, this.sliding, this.movingPlatform.setup });
		}
	}

	public bool tooSteep
	{
		get
		{
			return this._groundNormal.y <= Mathf.Cos(this.cc.slopeLimit * 0.0174532924f);
		}
	}

	[Obsolete("Do not query this", true)]
	public new Transform transform
	{
		get
		{
			return this.tr;
		}
	}

	public Vector3 velocity
	{
		get
		{
			return this.movement.velocity;
		}
		set
		{
			this._grounded = false;
			this.movement.velocity = value;
			Vector3 vector3 = new Vector3();
			this.movement.frameVelocity = vector3;
			if (this.sendExternalVelocityMessage)
			{
				this.RouteMessage("OnExternalVelocity");
			}
		}
	}

	static CCMotor()
	{
	}

	public CCMotor()
	{
	}

	private void ApplyGravityAndJumping(float deltaTime, ref Vector3 velocity, ref Vector3 acceleration, out bool simulate)
	{
		Vector3 vector3 = new Vector3();
		float single;
		float single1 = Time.time;
		if (!this.input.jump || !this.canControl)
		{
			this.jumping.holdingJumpButton = false;
			this.jumping.lastButtonDownTime = -100f;
		}
		if (this.input.jump && this.jumping.lastButtonDownTime < 0f && this.canControl)
		{
			this.jumping.lastButtonDownTime = single1;
		}
		if (!this._grounded)
		{
			acceleration.y = -this.movement.setup.gravity;
			acceleration.z = 0f;
			acceleration.x = 0f;
			if (this.jumping.jumping && this.jumping.holdingJumpButton && single1 < this.jumping.lastStartTime + this.jumping.setup.extraHeight / this.jumpVerticalSpeedCalculator.CalculateVerticalSpeed(ref this.jumping.setup, ref this.movement.setup))
			{
				acceleration.x = acceleration.x + this.jumping.jumpDir.x * this.movement.setup.gravity;
				acceleration.y = acceleration.y + this.jumping.jumpDir.y * this.movement.setup.gravity;
				acceleration.z = acceleration.z + this.jumping.jumpDir.z * this.movement.setup.gravity;
			}
			vector3.x = acceleration.x * deltaTime;
			vector3.y = acceleration.y * deltaTime;
			vector3.z = acceleration.z * deltaTime;
			velocity.y = this.movement.velocity.y + vector3.y;
			if (this.movement.setup.inputAirVelocityRatio == 1f)
			{
				velocity.x = velocity.x + vector3.x;
				velocity.z = velocity.z + vector3.z;
			}
			else if (this.movement.setup.inputAirVelocityRatio != 0f)
			{
				float single2 = 1f - this.movement.setup.inputAirVelocityRatio;
				velocity.x = velocity.x * this.movement.setup.inputAirVelocityRatio + this.movement.velocity.x * single2 + vector3.x;
				velocity.z = velocity.z * this.movement.setup.inputAirVelocityRatio + this.movement.velocity.z * single2 + vector3.z;
			}
			else
			{
				velocity.x = this.movement.velocity.x + vector3.x;
				velocity.z = this.movement.velocity.z + vector3.z;
			}
			if (-velocity.y > this.movement.setup.maxFallSpeed)
			{
				velocity.y = -this.movement.setup.maxFallSpeed;
			}
			if (this.movement.setup.maxAirHorizontalSpeed > 0f)
			{
				float single3 = velocity.x * velocity.x + velocity.z * velocity.z;
				if (single3 > this.movement.setup.maxAirHorizontalSpeed * this.movement.setup.maxAirHorizontalSpeed)
				{
					float single4 = this.movement.setup.maxAirHorizontalSpeed / Mathf.Sqrt(single3);
					velocity.x = velocity.x * single4;
					velocity.z = velocity.z * single4;
				}
			}
			simulate = true;
		}
		else
		{
			if (velocity.y < 0f)
			{
				velocity.y = velocity.y - this.movement.setup.gravity * deltaTime;
			}
			else
			{
				velocity.y = -this.movement.setup.gravity * deltaTime;
			}
			if (!this.jumping.setup.enable || !this.canControl || single1 - this.jumping.lastButtonDownTime >= 0.2f || this.minTimeBetweenJumps > 0f && single1 - this.jumping.lastLandTime < this.minTimeBetweenJumps)
			{
				this.jumping.holdingJumpButton = false;
			}
			else if (this.minTimeBetweenJumps <= 0f || single1 - this.jumping.lastLandTime >= this.minTimeBetweenJumps)
			{
				this._grounded = false;
				this.jumping.jumping = true;
				this.jumping.lastStartTime = single1;
				this.jumping.lastButtonDownTime = -100f;
				this.jumping.holdingJumpButton = true;
				Vector3 vector31 = Vector3.up;
				Vector3 vector32 = this._groundNormal;
				single = (!this.tooSteep ? this.jumping.setup.perpAmount : this.jumping.setup.steepPerpAmount);
				this.jumping.jumpDir = Vector3.Slerp(vector31, vector32, single);
				float single5 = this.jumpVerticalSpeedCalculator.CalculateVerticalSpeed(ref this.jumping.setup, ref this.movement.setup);
				velocity.x = velocity.x + this.jumping.jumpDir.x * single5;
				velocity.y = this.jumping.jumpDir.y * single5;
				velocity.z = velocity.z + this.jumping.jumpDir.z * single5;
				if (this.movingPlatform.setup.enable && (this.movingPlatform.setup.movementTransfer == CCMotor.JumpMovementTransfer.InitTransfer || this.movingPlatform.setup.movementTransfer == CCMotor.JumpMovementTransfer.PermaTransfer))
				{
					this.movement.frameVelocity = this.movingPlatform.platformVelocity;
					velocity.x = velocity.x + this.movingPlatform.platformVelocity.x;
					velocity.y = velocity.y + this.movingPlatform.platformVelocity.y;
					velocity.z = velocity.z + this.movingPlatform.platformVelocity.z;
				}
				this.jumping.startedJumping = true;
				if (this.sendJumpMessage)
				{
					this.RouteMessage("OnJump", SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (this.sendJumpFailureMessage)
			{
				this.RouteMessage("OnJumpFailed", SendMessageOptions.DontRequireReceiver);
			}
			simulate = false;
		}
	}

	private void ApplyHorizontalPushVelocity(ref Vector3 velocity)
	{
		Capsule capsule;
		CCTotemPole cCTotemPole = this.cc;
		if (cCTotemPole && cCTotemPole.Exists && this.cc.totemicObject.CCDesc.collider.GetGeometricShapeWorld(out capsule))
		{
			Sphere sphere = (Sphere)capsule;
			Vector world = new Vector();
			bool flag = false;
			Collider[] colliderArray = Physics.OverlapSphere(this.cc.totemicObject.CCDesc.worldCenter, this.cc.totemicObject.CCDesc.effectiveSkinnedHeight, 1310720);
			for (int i = 0; i < (int)colliderArray.Length; i++)
			{
				Collider collider = colliderArray[i];
				CCPusher component = collider.GetComponent<CCPusher>();
				if (component)
				{
					Vector3 vector3 = new Vector3();
					if (component.Push(sphere.Transform(collider.WorldToCollider()), ref vector3))
					{
						flag = true;
						world = world + (collider.ColliderToWorld() * vector3);
					}
				}
			}
			if (flag)
			{
				world.y = 0f;
				velocity.x = velocity.x + world.x;
				velocity.z = velocity.z + world.z;
			}
		}
	}

	private void ApplyInputVelocityChange(float deltaTime, ref Vector3 velocity, ref Vector3 acceleration)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		Vector3 vector32 = (!this.canControl ? new Vector3() : this.input.moveDirection);
		if (!this._grounded || !this.tooSteep)
		{
			this.DesiredHorizontalVelocity(ref vector32, out vector3);
		}
		else
		{
			vector3.y = 0f;
			float single = this._groundNormal.x * this._groundNormal.x + this._groundNormal.z * this._groundNormal.z;
			if (single != 1f)
			{
				float single1 = Mathf.Sqrt(single);
				vector3.x = this._groundNormal.x / single1;
				vector3.z = this._groundNormal.z / single1;
			}
			else
			{
				vector3.x = this._groundNormal.x;
				vector3.z = this._groundNormal.z;
			}
			Vector3 vector33 = Vector3.Project(vector32, vector3);
			vector3.x = vector3.x + (vector33.x * this.sliding.speedControl + (vector32.x - vector33.x) * this.sliding.sidewaysControl);
			vector3.z = vector3.z + (vector33.z * this.sliding.speedControl + (vector32.z - vector33.z) * this.sliding.sidewaysControl);
			vector3.y = vector33.y * this.sliding.speedControl + (vector32.y - vector33.y) * this.sliding.sidewaysControl;
			vector3.x = vector3.x * this.sliding.slidingSpeed;
			vector3.y = vector3.y * this.sliding.slidingSpeed;
			vector3.z = vector3.z * this.sliding.slidingSpeed;
		}
		if (this.movingPlatform.setup.enable && this.movingPlatform.setup.movementTransfer == CCMotor.JumpMovementTransfer.PermaTransfer)
		{
			vector3.x = vector3.x + this.movement.frameVelocity.x;
			vector3.z = vector3.z + this.movement.frameVelocity.z;
			vector3.y = 0f;
		}
		if (!this._grounded)
		{
			acceleration.x = 0f;
			acceleration.y = 0f;
			acceleration.z = 0f;
			velocity.y = 0f;
		}
		else
		{
			acceleration.x = 0f;
			acceleration.y = 0f;
			acceleration.z = 0f;
			float single2 = vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z;
			if (single2 != 0f)
			{
				vector3 = Vector3.Cross(Vector3.Cross(Vector3.up, vector3), this._groundNormal);
				float single3 = vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z;
				if (single3 != single2)
				{
					float single4 = Mathf.Sqrt(single2);
					if (single3 != 1f)
					{
						float single5 = single4 / Mathf.Sqrt(single3);
						vector3.x = vector3.x * single5;
						vector3.y = vector3.y * single5;
						vector3.z = vector3.z * single5;
					}
					else
					{
						vector3.x = vector3.x * single4;
						vector3.y = vector3.y * single4;
						vector3.z = vector3.z * single4;
					}
				}
			}
		}
		if (this._grounded || this.canControl)
		{
			float single6 = (!this._grounded ? this.movement.setup.maxAirAcceleration : this.movement.setup.maxGroundAcceleration) * deltaTime;
			vector31.x = vector3.x - velocity.x;
			vector31.y = vector3.y - velocity.y;
			vector31.z = vector3.z - velocity.z;
			float single7 = vector31.x * vector31.x + vector31.y * vector31.y + vector31.z * vector31.z;
			if (single7 > single6 * single6)
			{
				float single8 = single6 / Mathf.Sqrt(single7);
				vector31.x = vector31.x * single8;
				vector31.y = vector31.y * single8;
				vector31.z = vector31.z * single8;
			}
			velocity = velocity + vector31;
		}
		if (this._grounded && velocity.y > 0f)
		{
			velocity.y = 0f;
		}
	}

	private CCTotem.MoveInfo ApplyMovementDelta(ref Vector3 moveDistance, float crouchDelta)
	{
		float height = this.cc.Height + crouchDelta;
		return this.cc.Move(moveDistance, height);
	}

	private void ApplyYawDelta(float yRotation)
	{
		if (yRotation != 0f)
		{
			this.currentYaw = Mathf.DeltaAngle(0f, this.currentYaw.Degrees + yRotation);
		}
	}

	private new void Awake()
	{
		if (this._settings)
		{
			this._settings.BindSettingsTo(this);
		}
	}

	private void BindCharacter()
	{
		Character character = (Character)base.idMain;
		character.origin = this.tr.position;
		float single = Mathf.DeltaAngle(this.previousYaw.Degrees, this.currentYaw.Degrees);
		if (single != 0f)
		{
			this.previousYaw = this.currentYaw;
			Character character1 = character;
			character1.eyesYaw = character1.eyesYaw + single;
		}
	}

	private void BindPosition(ref CCTotem.PositionPlacement placement)
	{
		this.tr.position = placement.bottom;
		this.LastPositionPlacement = new CCTotem.PositionPlacement?(placement);
	}

	private float CalculateJumpVerticalSpeed(float targetJumpHeight)
	{
		return Mathf.Sqrt(2f * targetJumpHeight * this.movement.setup.gravity);
	}

	private void DesiredHorizontalVelocity(ref Vector3 inputMoveDirection, out Vector3 desiredVelocity)
	{
		Vector3 vector3 = this.InverseTransformDirection(inputMoveDirection);
		float single = this.MaxSpeedInDirection(ref vector3);
		if (this._grounded)
		{
			AnimationCurve animationCurve = this.movement.setup.slopeSpeedMultiplier;
			Vector3 vector31 = this.movement.velocity.normalized;
			single = single * animationCurve.Evaluate(Mathf.Asin(vector31.y) * 57.29578f);
		}
		desiredVelocity = this.TransformDirection(vector3 * single);
		if (this._grounded)
		{
			this.ApplyHorizontalPushVelocity(ref desiredVelocity);
		}
	}

	private void DoPush(Rigidbody pusher, Collider pusherCollider, Collision collisionFromPusher)
	{
	}

	private void FixedUpdate()
	{
		float single = Time.deltaTime;
		if (single == 0f)
		{
			return;
		}
		if (this.movingPlatform.setup.enable)
		{
			if (this.movingPlatform.activePlatform == null)
			{
				Vector3 vector3 = new Vector3();
				this.movingPlatform.platformVelocity = vector3;
			}
			else
			{
				Matrix4x4 matrix4x4 = this.movingPlatform.activePlatform.localToWorldMatrix;
				if (this.movingPlatform.newPlatform)
				{
					this.movingPlatform.newPlatform = false;
				}
				else
				{
					Vector3 vector31 = matrix4x4.MultiplyPoint3x4(this.movingPlatform.activeLocal.point);
					Vector3 vector32 = this.movingPlatform.lastMatrix.MultiplyPoint3x4(this.movingPlatform.activeLocal.point);
					this.movingPlatform.platformVelocity.x = (vector31.x - vector32.x) / single;
					this.movingPlatform.platformVelocity.y = (vector31.y - vector32.y) / single;
					this.movingPlatform.platformVelocity.z = (vector31.z - vector32.z) / single;
				}
				this.movingPlatform.lastMatrix = matrix4x4;
			}
		}
		if (this.stepMode == CCMotor.StepMode.ViaFixedUpdate)
		{
			this.StepPhysics(single);
		}
	}

	public void InitializeSetup(Character character, CCTotemPole cc, CharacterCCMotorTrait trait)
	{
		this.tr = base.transform;
		this.cc = cc;
		base.idMain = character;
		CCMotor.YawAngle yawAngle = 0f;
		CCMotor.YawAngle yawAngle1 = yawAngle;
		this.previousYaw = yawAngle;
		this.currentYaw = yawAngle1;
		if (trait)
		{
			if (trait.settings)
			{
				this.settings = trait.settings;
			}
			this.canControl = trait.canControl;
			this.sendLandMessage = trait.sendLandMessage;
			this.sendJumpMessage = trait.sendJumpMessage;
			this.sendJumpFailureMessage = trait.sendJumpFailureMessage;
			this.sendFallMessage = trait.sendFallMessage;
			this.sendExternalVelocityMessage = trait.sendExternalVelocityMessage;
			this.stepMode = trait.stepMode;
			this.minTimeBetweenJumps = trait.minTimeBetweenJumps;
		}
		if (!this._installed && cc)
		{
			this._installed = true;
			CCMotor.Callbacks.InstallCallbacks(this, cc);
		}
	}

	private Vector3 InverseTransformDirection(Vector3 direction)
	{
		return this.characterYawAngle.Unrotate(direction);
	}

	private Vector3 InverseTransformPoint(Vector3 point)
	{
		return this.InverseTransformDirection(this.tr.InverseTransformPoint(point));
	}

	public float MaxSpeedInDirection(ref Vector3 desiredMovementDirection)
	{
		Vector3 vector3 = new Vector3();
		if (desiredMovementDirection.x == 0f && desiredMovementDirection.y == 0f && desiredMovementDirection.z == 0f)
		{
			return 0f;
		}
		if (this.movement.setup.maxSidewaysSpeed == 0f)
		{
			return 0f;
		}
		float single = (desiredMovementDirection.z <= 0f ? this.movement.setup.maxBackwardsSpeed : this.movement.setup.maxForwardSpeed) / this.movement.setup.maxSidewaysSpeed;
		vector3.x = desiredMovementDirection.x;
		vector3.y = 0f;
		vector3.z = desiredMovementDirection.z / single;
		float single1 = vector3.x * vector3.x + vector3.z * vector3.z;
		if (single1 != 1f)
		{
			float single2 = Mathf.Sqrt(single1);
			vector3.x = vector3.x / single2;
			vector3.z = vector3.z / single2;
		}
		vector3.z = vector3.z * single;
		return Mathf.Sqrt(vector3.x * vector3.x + vector3.z * vector3.z) * this.movement.setup.maxSidewaysSpeed;
	}

	private void MoveFromCollision(Collision collision)
	{
		PlayerPusher component = collision.gameObject.GetComponent<PlayerPusher>();
		if (component)
		{
			ContactPoint[] contactPointArray = collision.contacts;
			Vector3 vector3 = Vector3.zero;
			Vector3 vector31 = Vector3.zero;
			Vector3 length = Vector3.zero;
			for (int i = 0; i < (int)contactPointArray.Length; i++)
			{
				length = length + contactPointArray[i].point;
				vector31 = vector31 + contactPointArray[i].normal;
			}
			vector31.Normalize();
			length = length / (float)((int)contactPointArray.Length);
			Vector3 vector32 = this.tr.position;
			vector32.y = length.y;
			Vector3 pointVelocity = component.rigidbody.GetPointVelocity(vector32);
			vector3 = vector31 * (pointVelocity.magnitude * Time.deltaTime);
			Vector3 vector33 = this.tr.position;
			UnityEngine.Debug.DrawLine(vector33, vector33 + vector3, Color.yellow, 60f);
			this.ApplyMovementDelta(ref vector3, 0f);
			UnityEngine.Debug.DrawLine(vector33, this.tr.position, Color.green, 60f);
			this.BindCharacter();
		}
	}

	internal void OnBindCCMotorSettings()
	{
	}

	public void OnCollisionEnter(Collision collision)
	{
		this.MoveFromCollision(collision);
	}

	public void OnCollisionStay(Collision collision)
	{
		this.MoveFromCollision(collision);
	}

	private new void OnDestroy()
	{
		try
		{
			base.OnDestroy();
		}
		finally
		{
			if (this._installed)
			{
				CCMotor.Callbacks.UninstallCallbacks(this, this.cc);
			}
			this.cc = null;
		}
	}

	private void OnHit(ref CCDesc.Hit hit)
	{
		Vector3 vector3 = new Vector3();
		Vector3 normal = hit.Normal;
		Vector3 moveDirection = hit.MoveDirection;
		if (normal.y > 0f && normal.y > this._groundNormal.y && moveDirection.y < 0f)
		{
			Vector3 point = hit.Point;
			vector3.x = point.x - this.movement.lastHitPoint.x;
			vector3.y = point.y - this.movement.lastHitPoint.y;
			vector3.z = point.z - this.movement.lastHitPoint.z;
			if ((this._lastGroundNormal.x != 0f || this._lastGroundNormal.y != 0f || this._lastGroundNormal.z != 0f) && vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z <= 0.001f)
			{
				this._groundNormal = this._lastGroundNormal;
			}
			else
			{
				this._groundNormal = normal;
			}
			this.movingPlatform.hitPlatform = hit.Collider.transform;
			this.movement.hitPoint = point;
			Vector3 vector31 = new Vector3();
			this.movement.frameVelocity = vector31;
		}
	}

	public void OnPushEnter(Rigidbody pusher, Collider pusherCollider, Collision collisionFromPusher)
	{
		this.DoPush(pusher, pusherCollider, collisionFromPusher);
	}

	public void OnPushExit(Rigidbody pusher, Collider pusherCollider, Collision collisionFromPusher)
	{
		this.DoPush(pusher, pusherCollider, collisionFromPusher);
	}

	public void OnPushStay(Rigidbody pusher, Collider pusherCollider, Collision collisionFromPusher)
	{
		this.DoPush(pusher, pusherCollider, collisionFromPusher);
	}

	private void RouteMessage(string messageName)
	{
		base.idMain.SendMessage(messageName, SendMessageOptions.DontRequireReceiver);
	}

	private void RouteMessage(string messageName, SendMessageOptions sendOptions)
	{
		base.idMain.SendMessage(messageName, sendOptions);
	}

	public void Step()
	{
		this.Step(Time.deltaTime);
	}

	public void Step(float deltaTime)
	{
		if (deltaTime <= 0f || !base.enabled)
		{
			return;
		}
		this.StepPhysics(deltaTime);
	}

	private void StepPhysics(float deltaTime)
	{
		bool flag;
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		Vector3 vector32 = new Vector3();
		Vector3 vector33 = new Vector3();
		float single;
		Vector3 vector34;
		Vector3 vector35 = new Vector3();
		Vector3 vector36 = new Vector3();
		bool flag1;
		Vector3 vector37 = this.movement.velocity;
		Vector3 vector38 = this.movement.acceleration;
		this.ApplyInputVelocityChange(deltaTime, ref vector37, ref vector38);
		this.ApplyGravityAndJumping(deltaTime, ref vector37, ref vector38, out flag);
		if (this.movingWithPlatform)
		{
			Vector3 vector39 = this.movingPlatform.activePlatform.TransformPoint(this.movingPlatform.activeLocal.point);
			vector3.x = vector39.x - this.movingPlatform.activeGlobal.point.x;
			vector3.y = vector39.y - this.movingPlatform.activeGlobal.point.y;
			vector3.z = vector39.z - this.movingPlatform.activeGlobal.point.z;
			if (vector3.x != 0f || vector3.y != 0f || vector3.z != 0f)
			{
				this.ApplyMovementDelta(ref vector3, 0f);
			}
			Quaternion quaternion = this.movingPlatform.activePlatform.rotation * this.movingPlatform.activeLocal.rotation;
			Quaternion quaternion1 = quaternion * Quaternion.Inverse(this.movingPlatform.activeGlobal.rotation);
			float single1 = quaternion1.eulerAngles.y;
			if (single1 != 0f)
			{
				this.ApplyYawDelta(single1);
			}
		}
		vector31.x = vector38.x * deltaTime;
		vector31.y = vector38.y * deltaTime;
		vector31.z = vector38.z * deltaTime;
		Vector3 vector310 = this.tr.position;
		if (!flag)
		{
			vector32.x = vector37.x * deltaTime;
			vector32.y = vector37.y * deltaTime;
			vector32.z = vector37.z * deltaTime;
			vector33.x = vector310.x + vector32.x;
			vector33.y = vector310.y + vector32.y;
			vector33.z = vector310.z + vector32.z;
		}
		else
		{
			vector33.x = vector310.x + deltaTime * (this.movement.velocity.x + vector31.x / 2f);
			vector33.y = vector310.y + deltaTime * (this.movement.velocity.y + vector31.y / 2f);
			vector33.z = vector310.z + deltaTime * (this.movement.velocity.z + vector31.z / 2f);
			vector32.x = vector33.x - vector310.x;
			vector32.y = vector33.y - vector310.y;
			vector32.z = vector33.z - vector310.z;
		}
		float single2 = this.cc.stepOffset;
		float single3 = single2 * single2;
		float single4 = vector32.x * vector32.x + vector32.z * vector32.z;
		single = (single4 <= single3 ? single2 : Mathf.Sqrt(single4));
		if (this._grounded)
		{
			vector32.y = vector32.y - single;
		}
		this.movingPlatform.hitPlatform = null;
		this._groundNormal = new Vector3();
		float single5 = this.input.crouchSpeed * deltaTime;
		CCTotem.MoveInfo moveInfo = this.ApplyMovementDelta(ref vector32, single5);
		this.movement.collisionFlags = moveInfo.CollisionFlags;
		float wantedHeight = moveInfo.WantedHeight - moveInfo.PositionPlacement.height;
		CollisionFlags collisionFlags = moveInfo.CollisionFlags | moveInfo.WorkingCollisionFlags;
		flag1 = (this.input.crouchSpeed <= 0f || (collisionFlags & CollisionFlags.Above) != CollisionFlags.Above ? false : wantedHeight > this.movement.setup.maxUnblockingHeightDifference);
		this.movement.crouchBlocked = flag1;
		this.movement.lastHitPoint = this.movement.hitPoint;
		this._lastGroundNormal = this._groundNormal;
		if (this.movingPlatform.setup.enable && this.movingPlatform.activePlatform != this.movingPlatform.hitPlatform && this.movingPlatform.hitPlatform != null)
		{
			this.movingPlatform.activePlatform = this.movingPlatform.hitPlatform;
			this.movingPlatform.lastMatrix = this.movingPlatform.hitPlatform.localToWorldMatrix;
			this.movingPlatform.newPlatform = true;
		}
		if (this.movement.collisionFlags == CollisionFlags.None)
		{
			vector34 = vector33;
			this.movement.velocity = vector37;
			this.movement.acceleration = vector38;
		}
		else
		{
			this.movement.acceleration.x = 0f;
			this.movement.acceleration.y = 0f;
			this.movement.acceleration.z = 0f;
			vector35.x = vector37.x;
			vector35.y = 0f;
			vector35.z = vector37.z;
			vector34 = this.tr.position;
			this.movement.velocity.x = (vector34.x - vector310.x) / deltaTime;
			this.movement.velocity.y = (vector34.y - vector310.y) / deltaTime;
			this.movement.velocity.z = (vector34.z - vector310.z) / deltaTime;
			vector36.x = this.movement.velocity.x;
			vector36.y = 0f;
			vector36.z = this.movement.velocity.z;
			if (vector35.x != 0f || vector35.z != 0f)
			{
				float single6 = (vector36.x * vector35.x + vector36.z * vector35.z) / (vector35.x * vector35.x + vector35.z * vector35.z);
				if (single6 <= 0f)
				{
					this.movement.velocity.x = 0f;
					this.movement.velocity.z = 0f;
				}
				else if (single6 < 1f)
				{
					this.movement.velocity.x = vector35.x * single6;
					this.movement.velocity.z = vector35.z * single6;
				}
				else
				{
					this.movement.velocity.x = vector35.x;
					this.movement.velocity.z = vector35.z;
				}
			}
			else
			{
				this.movement.velocity.x = 0f;
				this.movement.velocity.z = 0f;
			}
			if (this.movement.velocity.y < vector37.y - 0.001f)
			{
				if (this.movement.velocity.y >= 0f)
				{
					this.jumping.holdingJumpButton = false;
				}
				else
				{
					this.movement.velocity.y = vector37.y;
				}
			}
		}
		if (this._grounded != this._groundNormal.y > 0.01f)
		{
			if (!this._grounded)
			{
				this._grounded = true;
				this.jumping.jumping = false;
				if (this.jumping.startedJumping)
				{
					this.jumping.startedJumping = false;
					this.jumping.lastLandTime = Time.time;
				}
				this.SubtractNewPlatformVelocity();
				if (this.sendLandMessage)
				{
					this.RouteMessage("OnLand", SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._grounded = false;
				if (this.movingPlatform.setup.enable && (this.movingPlatform.setup.movementTransfer == CCMotor.JumpMovementTransfer.InitTransfer || this.movingPlatform.setup.movementTransfer == CCMotor.JumpMovementTransfer.PermaTransfer))
				{
					this.movement.frameVelocity = this.movingPlatform.platformVelocity;
					this.movement.velocity = this.movement.velocity + this.movingPlatform.platformVelocity;
				}
				if (this.sendFallMessage)
				{
					this.RouteMessage("OnFall", SendMessageOptions.DontRequireReceiver);
				}
				vector34.y = vector34.y + single;
			}
		}
		if (this.movingWithPlatform)
		{
			this.movingPlatform.activeGlobal.point.x = vector34.x;
			float single7 = vector34.y;
			Vector3 vector311 = this.cc.center;
			this.movingPlatform.activeGlobal.point.y = single7 + (vector311.y - this.cc.height * 0.5f + this.cc.radius);
			this.movingPlatform.activeGlobal.point.z = vector34.z;
			this.movingPlatform.activeLocal.point = this.movingPlatform.activePlatform.InverseTransformPoint(this.movingPlatform.activeGlobal.point);
			this.movingPlatform.activeGlobal.rotation = this.tr.rotation;
			this.movingPlatform.activeLocal.rotation = Quaternion.Inverse(this.movingPlatform.activePlatform.rotation) * this.movingPlatform.activeGlobal.rotation;
		}
		this.BindCharacter();
	}

	private void SubtractNewPlatformVelocity()
	{
		if (this.movingPlatform.setup.enable && (this.movingPlatform.setup.movementTransfer == CCMotor.JumpMovementTransfer.InitTransfer || this.movingPlatform.setup.movementTransfer == CCMotor.JumpMovementTransfer.PermaTransfer))
		{
			if (!this.movingPlatform.newPlatform)
			{
				this.movement.velocity = this.movement.velocity - this.movingPlatform.platformVelocity;
			}
			else
			{
				base.StartCoroutine(this.SubtractNewPlatformVelocityLateRoutine(this.movingPlatform.activePlatform));
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator SubtractNewPlatformVelocityLateRoutine(Transform platform)
	{
		CCMotor.<SubtractNewPlatformVelocityLateRoutine>c__Iterator26 variable = null;
		return variable;
	}

	public void Teleport(Vector3 origin)
	{
		if (!this.cc)
		{
			this.tr.position = origin;
		}
		else
		{
			this.cc.Teleport(origin);
		}
	}

	private Vector3 TransformDirection(Vector3 direction)
	{
		return this.characterYawAngle.Rotate(direction);
	}

	private Vector3 TransformPoint(Vector3 point)
	{
		return this.tr.TransformPoint(this.TransformDirection(point));
	}

	private void Update()
	{
		if (this.stepMode == CCMotor.StepMode.ViaUpdate)
		{
			float single = Time.deltaTime;
			float single1 = single;
			if (single != 0f)
			{
				this.StepPhysics(single1);
				return;
			}
		}
	}

	private static class Callbacks
	{
		public readonly static CCDesc.HitFilter HitFilter;

		public readonly static CCTotem.PositionBinder PositionBinder;

		public readonly static CCTotem.ConfigurationBinder ConfigurationBinder;

		static Callbacks()
		{
			CCMotor.Callbacks.HitFilter = new CCDesc.HitFilter(CCMotor.Callbacks.OnHit);
			CCMotor.Callbacks.PositionBinder = new CCTotem.PositionBinder(CCMotor.Callbacks.OnBindPosition);
			CCMotor.Callbacks.ConfigurationBinder = new CCTotem.ConfigurationBinder(CCMotor.Callbacks.OnConfigurationBinding);
		}

		public static void InstallCallbacks(CCMotor CCMotor, CCTotemPole CCTotemPole)
		{
			CCTotemPole.Tag = CCMotor;
			CCTotemPole.OnBindPosition += CCMotor.Callbacks.PositionBinder;
			CCTotemPole.OnConfigurationBinding += CCMotor.Callbacks.ConfigurationBinder;
		}

		private static void OnBindPosition(ref CCTotem.PositionPlacement PositionPlacement, object Tag)
		{
			CCMotor tag = (CCMotor)Tag;
			if (tag)
			{
				tag.BindPosition(ref PositionPlacement);
			}
		}

		private static void OnConfigurationBinding(bool Bind, CCDesc CCDesc, object Tag)
		{
			CCHitDispatch hitDispatch = CCHitDispatch.GetHitDispatch(CCDesc);
			if (hitDispatch)
			{
				CCDesc.HitManager hits = hitDispatch.Hits;
				if (!object.ReferenceEquals(hits, null))
				{
					if (Bind)
					{
						hits.Tag = Tag;
						hits.OnHit += CCMotor.Callbacks.HitFilter;
					}
					else if (object.ReferenceEquals(hits.Tag, Tag))
					{
						hits.Tag = null;
						hits.OnHit -= CCMotor.Callbacks.HitFilter;
					}
				}
			}
			if (Bind)
			{
				CCDesc.Tag = Tag;
				if (!CCDesc.GetComponent<CCTotemicFigure>())
				{
					IDRemote component = CCDesc.GetComponent<IDRemote>();
					if (!component)
					{
						component = CCDesc.gameObject.AddComponent<IDRemoteDefault>();
					}
					component.idMain = ((CCMotor)Tag).idMain;
					CCDesc.detectCollisions = true;
				}
			}
			else if (object.ReferenceEquals(CCDesc.Tag, Tag))
			{
				CCDesc.Tag = null;
			}
		}

		private static bool OnHit(CCDesc.HitManager HitManager, ref CCDesc.Hit hit)
		{
			CCMotor tag = (CCMotor)HitManager.Tag;
			if (CCMotor.ccmotor_debug && !(hit.Collider is TerrainCollider))
			{
				UnityEngine.Debug.Log(string.Format("{{\"ccmotor\":{{\"hit\":{{\"point\":[{0},{1},{2}],\"normal\":[{3},{4},{5}]}},\"dir\":[{6},{7},{8}],\"move\":{9},\"obj\":{10}}}}}", new object[] { hit.Point.x, hit.Point.y, hit.Point.z, hit.Normal.x, hit.Normal.y, hit.Normal.z, hit.MoveDirection.x, hit.MoveDirection.y, hit.MoveDirection.z, hit.MoveLength, hit.Collider }), hit.GameObject);
			}
			tag.OnHit(ref hit);
			return true;
		}

		public static void UninstallCallbacks(CCMotor CCMotor, CCTotemPole CCTotemPole)
		{
			if (CCTotemPole && object.ReferenceEquals(CCTotemPole.Tag, CCMotor))
			{
				CCTotemPole.OnConfigurationBinding -= CCMotor.Callbacks.ConfigurationBinder;
				CCTotemPole.OnBindPosition -= CCMotor.Callbacks.PositionBinder;
				CCTotemPole.Tag = null;
			}
		}
	}

	public struct InputFrame
	{
		public Vector3 moveDirection;

		public bool jump;

		public float crouchSpeed;
	}

	private struct JumpBaseVerticalSpeedArgs
	{
		private float _baseHeight;

		private float _gravity;

		private float _verticalSpeed;

		private bool dirty;

		public float baseHeight
		{
			get
			{
				return this._baseHeight;
			}
			set
			{
				if (this._baseHeight != value)
				{
					this.dirty = true;
					this._baseHeight = value;
				}
			}
		}

		public float gravity
		{
			get
			{
				return this._gravity;
			}
			set
			{
				if (this._gravity != value)
				{
					this.dirty = true;
					this._gravity = value;
				}
			}
		}

		public float CalculateVerticalSpeed(ref CCMotor.Jumping jumping, ref CCMotor.Movement movement)
		{
			if (this.dirty || this._baseHeight != jumping.baseHeight || this._gravity != movement.gravity)
			{
				this._baseHeight = jumping.baseHeight;
				this._gravity = movement.gravity;
				this._verticalSpeed = Mathf.Sqrt(2f * this._baseHeight * this._gravity);
				this.dirty = false;
			}
			return this._verticalSpeed;
		}
	}

	public struct Jumping
	{
		public bool enable;

		public float baseHeight;

		public float extraHeight;

		public float perpAmount;

		public float steepPerpAmount;

		public readonly static CCMotor.Jumping init;

		static Jumping()
		{
			CCMotor.Jumping jumping = new CCMotor.Jumping()
			{
				enable = true,
				baseHeight = 1f,
				extraHeight = 4.1f,
				steepPerpAmount = 0.5f
			};
			CCMotor.Jumping.init = jumping;
		}

		public override string ToString()
		{
			return string.Format("[Jumping: enable={0}, baseHeight={1}, extraHeight={2}, perpAmount={3}, steepPerpAmount={4}]", new object[] { this.enable, this.baseHeight, this.extraHeight, this.perpAmount, this.steepPerpAmount });
		}
	}

	public struct JumpingContext
	{
		public CCMotor.Jumping setup;

		public bool jumping;

		public bool holdingJumpButton;

		public bool startedJumping;

		public float lastStartTime;

		public float lastButtonDownTime;

		public float lastLandTime;

		public Vector3 jumpDir;

		public JumpingContext(ref CCMotor.Jumping setup)
		{
			this.setup = setup;
			this.jumping = false;
			this.holdingJumpButton = false;
			this.startedJumping = false;
			this.lastStartTime = 0f;
			this.lastButtonDownTime = -100f;
			this.jumpDir.x = 0f;
			this.jumpDir.y = 1f;
			this.jumpDir.z = 0f;
			this.lastLandTime = Single.MinValue;
		}

		public JumpingContext(CCMotor.Jumping setup) : this(ref setup)
		{
		}

		public static implicit operator Jumping(CCMotor.JumpingContext c)
		{
			return c.setup;
		}
	}

	public enum JumpMovementTransfer
	{
		None,
		InitTransfer,
		PermaTransfer,
		PermaLocked
	}

	public struct Movement
	{
		public float maxForwardSpeed;

		public float maxSidewaysSpeed;

		public float maxBackwardsSpeed;

		public float maxGroundAcceleration;

		public float maxAirAcceleration;

		public float inputAirVelocityRatio;

		public float gravity;

		public float maxFallSpeed;

		public float maxAirHorizontalSpeed;

		public float maxUnblockingHeightDifference;

		public AnimationCurve slopeSpeedMultiplier;

		public static CCMotor.Movement init
		{
			get
			{
				CCMotor.Movement animationCurve = new CCMotor.Movement();
				animationCurve.maxForwardSpeed = 3f;
				animationCurve.maxSidewaysSpeed = 3f;
				animationCurve.maxBackwardsSpeed = 3f;
				animationCurve.maxGroundAcceleration = 30f;
				animationCurve.maxAirAcceleration = 20f;
				animationCurve.gravity = 10f;
				animationCurve.maxFallSpeed = 20f;
				animationCurve.inputAirVelocityRatio = 0.8f;
				animationCurve.maxAirHorizontalSpeed = 750f;
				animationCurve.maxUnblockingHeightDifference = 0f;
				animationCurve.slopeSpeedMultiplier = new AnimationCurve(new Keyframe[] { new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 0f) });
				return animationCurve;
			}
		}

		public override string ToString()
		{
			return string.Format("[Movement: maxForwardSpeed={0}, maxSidewaysSpeed={1}, maxBackwardsSpeed={2}, maxGroundAcceleration={3}, maxAirAcceleration={4}, inputAirVelocityRatio={5}, gravity={6}, maxFallSpeed={7}, slopeSpeedMultiplier={8}, maxAirHorizontalSpeed={9}]", new object[] { this.maxForwardSpeed, this.maxSidewaysSpeed, this.maxBackwardsSpeed, this.maxGroundAcceleration, this.maxAirAcceleration, this.inputAirVelocityRatio, this.gravity, this.maxFallSpeed, this.slopeSpeedMultiplier, this.maxAirHorizontalSpeed });
		}
	}

	public struct MovementContext
	{
		public CCMotor.Movement setup;

		public CollisionFlags collisionFlags;

		public bool crouchBlocked;

		public Vector3 acceleration;

		public Vector3 velocity;

		public Vector3 frameVelocity;

		public Vector3 hitPoint;

		public Vector3 lastHitPoint;

		public MovementContext(ref CCMotor.Movement setup)
		{
			this.setup = setup;
			this.collisionFlags = CollisionFlags.None;
			this.crouchBlocked = false;
			this.acceleration = new Vector3();
			this.velocity = new Vector3();
			this.frameVelocity = new Vector3();
			this.hitPoint = new Vector3();
			this.lastHitPoint.x = Single.PositiveInfinity;
			this.lastHitPoint.y = 0f;
			this.lastHitPoint.z = 0f;
		}

		public MovementContext(CCMotor.Movement setup) : this(ref setup)
		{
		}

		public static implicit operator Movement(CCMotor.MovementContext c)
		{
			return c.setup;
		}
	}

	public struct MovingPlatform
	{
		public bool enable;

		public CCMotor.JumpMovementTransfer movementTransfer;

		public readonly static CCMotor.MovingPlatform init;

		static MovingPlatform()
		{
			CCMotor.MovingPlatform movingPlatform = new CCMotor.MovingPlatform()
			{
				enable = true,
				movementTransfer = CCMotor.JumpMovementTransfer.PermaTransfer
			};
			CCMotor.MovingPlatform.init = movingPlatform;
		}

		public override string ToString()
		{
			return string.Format("[MovingPlatform: enable={0}, movementTransfer={1}]", this.enable, this.movementTransfer);
		}
	}

	public struct MovingPlatformContext
	{
		public CCMotor.MovingPlatform setup;

		public Transform hitPlatform;

		public Transform activePlatform;

		public CCMotor.MovingPlatformContext.PointAndRotation activeLocal;

		public CCMotor.MovingPlatformContext.PointAndRotation activeGlobal;

		public Matrix4x4 lastMatrix;

		public Vector3 platformVelocity;

		public bool newPlatform;

		public MovingPlatformContext(ref CCMotor.MovingPlatform setup)
		{
			this.setup = setup;
			this.hitPlatform = null;
			this.activePlatform = null;
			this.activeLocal = new CCMotor.MovingPlatformContext.PointAndRotation();
			this.activeGlobal = new CCMotor.MovingPlatformContext.PointAndRotation();
			this.lastMatrix = new Matrix4x4();
			this.platformVelocity = new Vector3();
			this.newPlatform = false;
		}

		public MovingPlatformContext(CCMotor.MovingPlatform setup) : this(ref setup)
		{
		}

		public static implicit operator MovingPlatform(CCMotor.MovingPlatformContext c)
		{
			return c.setup;
		}

		public struct PointAndRotation
		{
			public Vector3 point;

			public Quaternion rotation;
		}
	}

	public struct Sliding
	{
		public bool enable;

		public float slidingSpeed;

		public float sidewaysControl;

		public float speedControl;

		public readonly static CCMotor.Sliding init;

		static Sliding()
		{
			CCMotor.Sliding sliding = new CCMotor.Sliding()
			{
				enable = true,
				slidingSpeed = 15f,
				sidewaysControl = 1f,
				speedControl = 0.4f
			};
			CCMotor.Sliding.init = sliding;
		}

		public override string ToString()
		{
			return string.Format("[Sliding enable={0}, slidingSpeed={1}, sidewaysControl={2}, speedControl={3}]", new object[] { this.enable, this.slidingSpeed, this.sidewaysControl, this.speedControl });
		}
	}

	public enum StepMode
	{
		ViaUpdate,
		ViaFixedUpdate,
		Elsewhere
	}

	private struct YawAngle
	{
		public readonly float Degrees;

		private YawAngle(float Degrees)
		{
			this.Degrees = Degrees;
		}

		public static implicit operator YawAngle(float Degrees)
		{
			return new CCMotor.YawAngle(Degrees);
		}

		public Vector3 Rotate(Vector3 direction)
		{
			return Quaternion.AngleAxis(this.Degrees, Vector3.up) * direction;
		}

		public Vector3 Unrotate(Vector3 direction)
		{
			return Quaternion.AngleAxis(this.Degrees, Vector3.down) * direction;
		}
	}
}