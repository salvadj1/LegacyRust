using System;
using UnityEngine;

public class HumanControlConfiguration : ControlConfiguration
{
	[SerializeField]
	private AnimationCurve sprintAddSpeedByTime = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.4f, 1f, 0f, 0f) });

	[SerializeField]
	private AnimationCurve crouchMulSpeedByTime = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.4f, 0.55f, 0f, 0f) });

	[SerializeField]
	private AnimationCurve landingSpeedPenalty = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.25f, 0.5f, -2f, -2f), new Keyframe(0.75f, 1f, 0f, 0f) });

	[SerializeField]
	private Vector2 sprintScalars = new Vector2(0.2f, 1f);

	public AnimationCurve curveCrouchMulSpeedByTime
	{
		get
		{
			return this.crouchMulSpeedByTime;
		}
	}

	public AnimationCurve curveLandingSpeedPenalty
	{
		get
		{
			return this.landingSpeedPenalty;
		}
	}

	public AnimationCurve curveSprintAddSpeedByTime
	{
		get
		{
			return this.sprintAddSpeedByTime;
		}
	}

	public Vector2 sprintScale
	{
		get
		{
			return this.sprintScalars;
		}
	}

	public float sprintScaleX
	{
		get
		{
			return this.sprintScalars.x;
		}
	}

	public float sprintScaleY
	{
		get
		{
			return this.sprintScalars.y;
		}
	}

	public HumanControlConfiguration()
	{
	}
}