using System;
using UnityEngine;

public sealed class CCMotorSettings : ScriptableObject
{
	private readonly static CCMotor.Movement Movement_init;

	public float maxForwardSpeed = CCMotorSettings.Movement_init.maxForwardSpeed;

	public float maxSidewaysSpeed = CCMotorSettings.Movement_init.maxSidewaysSpeed;

	public float maxBackwardsSpeed = CCMotorSettings.Movement_init.maxBackwardsSpeed;

	public float maxGroundAcceleration = CCMotorSettings.Movement_init.maxGroundAcceleration;

	public float maxAirAcceleration = CCMotorSettings.Movement_init.maxAirAcceleration;

	public float inputAirVelocityRatio = CCMotorSettings.Movement_init.inputAirVelocityRatio;

	public float gravity = CCMotorSettings.Movement_init.gravity;

	public float maxFallSpeed = CCMotorSettings.Movement_init.maxFallSpeed;

	public float maxAirHorizontalSpeed = CCMotorSettings.Movement_init.maxAirHorizontalSpeed;

	public float maxUnblockingHeightDifference = CCMotorSettings.Movement_init.maxUnblockingHeightDifference;

	public AnimationCurve slopeSpeedMultiplier = CCMotor.Movement.init.slopeSpeedMultiplier;

	public bool jumpEnable = CCMotor.Jumping.init.enable;

	public float jumpBaseHeight = CCMotor.Jumping.init.baseHeight;

	public float jumpExtraHeight = CCMotor.Jumping.init.extraHeight;

	public float jumpPerpAmount = CCMotor.Jumping.init.perpAmount;

	public float jumpSteepPerpAmount = CCMotor.Jumping.init.steepPerpAmount;

	public bool slidingEnable = CCMotor.Sliding.init.enable;

	public float slidingSpeed = CCMotor.Sliding.init.slidingSpeed;

	public float slidingSidewaysControl = CCMotor.Sliding.init.sidewaysControl;

	public float slidingSpeedControl = CCMotor.Sliding.init.speedControl;

	public bool platformMovementEnable = CCMotor.MovingPlatform.init.enable;

	public CCMotor.JumpMovementTransfer platformMovementTransfer = CCMotor.MovingPlatform.init.movementTransfer;

	public CCMotor.Jumping jumping
	{
		get
		{
			CCMotor.Jumping jumping = new CCMotor.Jumping();
			jumping.enable = this.jumpEnable;
			jumping.baseHeight = this.jumpBaseHeight;
			jumping.extraHeight = this.jumpExtraHeight;
			jumping.perpAmount = this.jumpPerpAmount;
			jumping.steepPerpAmount = this.jumpSteepPerpAmount;
			return jumping;
		}
		set
		{
			this.jumpEnable = value.enable;
			this.jumpBaseHeight = value.baseHeight;
			this.jumpExtraHeight = value.extraHeight;
			this.jumpPerpAmount = value.perpAmount;
			this.jumpSteepPerpAmount = value.steepPerpAmount;
		}
	}

	public CCMotor.Movement movement
	{
		get
		{
			CCMotor.Movement animationCurve = new CCMotor.Movement();
			animationCurve.maxForwardSpeed = this.maxForwardSpeed;
			animationCurve.maxSidewaysSpeed = this.maxSidewaysSpeed;
			animationCurve.maxBackwardsSpeed = this.maxBackwardsSpeed;
			animationCurve.maxGroundAcceleration = this.maxGroundAcceleration;
			animationCurve.maxAirAcceleration = this.maxAirAcceleration;
			animationCurve.inputAirVelocityRatio = this.inputAirVelocityRatio;
			animationCurve.gravity = this.gravity;
			animationCurve.maxFallSpeed = this.maxFallSpeed;
			animationCurve.maxAirHorizontalSpeed = this.maxAirHorizontalSpeed;
			animationCurve.maxUnblockingHeightDifference = this.maxUnblockingHeightDifference;
			animationCurve.slopeSpeedMultiplier = new AnimationCurve(this.slopeSpeedMultiplier.keys)
			{
				postWrapMode = this.slopeSpeedMultiplier.postWrapMode,
				preWrapMode = this.slopeSpeedMultiplier.preWrapMode
			};
			return animationCurve;
		}
		set
		{
			this.maxForwardSpeed = value.maxForwardSpeed;
			this.maxSidewaysSpeed = value.maxSidewaysSpeed;
			this.maxBackwardsSpeed = value.maxBackwardsSpeed;
			this.maxGroundAcceleration = value.maxGroundAcceleration;
			this.maxAirAcceleration = value.maxAirAcceleration;
			this.inputAirVelocityRatio = value.inputAirVelocityRatio;
			this.gravity = value.gravity;
			this.maxFallSpeed = value.maxFallSpeed;
			this.maxUnblockingHeightDifference = value.maxUnblockingHeightDifference;
			this.slopeSpeedMultiplier.keys = value.slopeSpeedMultiplier.keys;
			this.slopeSpeedMultiplier.postWrapMode = value.slopeSpeedMultiplier.postWrapMode;
			this.slopeSpeedMultiplier.preWrapMode = value.slopeSpeedMultiplier.preWrapMode;
		}
	}

	public CCMotor.MovingPlatform movingPlatform
	{
		get
		{
			CCMotor.MovingPlatform movingPlatform = new CCMotor.MovingPlatform();
			movingPlatform.enable = this.platformMovementEnable;
			movingPlatform.movementTransfer = this.platformMovementTransfer;
			return movingPlatform;
		}
		set
		{
			this.platformMovementEnable = value.enable;
			this.platformMovementTransfer = value.movementTransfer;
		}
	}

	public CCMotor.Sliding sliding
	{
		get
		{
			CCMotor.Sliding sliding = new CCMotor.Sliding();
			sliding.enable = this.slidingEnable;
			sliding.slidingSpeed = this.slidingSpeed;
			sliding.sidewaysControl = this.slidingSidewaysControl;
			sliding.speedControl = this.slidingSpeedControl;
			return sliding;
		}
		set
		{
			this.slidingEnable = value.enable;
			this.slidingSpeed = value.slidingSpeed;
			this.slidingSidewaysControl = value.sidewaysControl;
			this.slidingSpeedControl = value.speedControl;
		}
	}

	static CCMotorSettings()
	{
		CCMotorSettings.Movement_init = CCMotor.Movement.init;
	}

	public CCMotorSettings()
	{
	}

	public void BindSettingsTo(CCMotor motor)
	{
		motor.jumping.setup = this.jumping;
		motor.movement.setup = this.movement;
		motor.movingPlatform.setup = this.movingPlatform;
		motor.sliding = this.sliding;
		motor.OnBindCCMotorSettings();
	}

	public void CopySettingsFrom(CCMotor motor)
	{
		this.jumping = motor.jumping.setup;
		this.movement = motor.movement.setup;
		this.movingPlatform = motor.movingPlatform.setup;
		this.sliding = motor.sliding;
	}

	public override string ToString()
	{
		return string.Format("[CCMotorSettings: movement={0}, jumping={1}, sliding={2}, movingPlatform={3}]", new object[] { this.movement, this.jumping, this.sliding, this.movingPlatform });
	}
}