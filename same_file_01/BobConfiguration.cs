using System;
using UnityEngine;

public class BobConfiguration : ScriptableObject
{
	public Vector3 springConstant = Vector3.one * 5f;

	public Vector3 springDampen = Vector3.one * 0.1f;

	public float weightMass = 5f;

	public float timeScale = 1f;

	public Vector3 forceSpeedMultiplier = Vector3.one;

	public Vector3 inputForceMultiplier = Vector3.one;

	public Vector3 elipsoidRadii = Vector3.one;

	public Vector3 maxVelocity = Vector3.one * 20f;

	public Vector3 positionDeadzone = new Vector3(0.0001f, 0.0001f, 0.0001f);

	public Vector3 rotationDeadzone = new Vector3(0.0001f, 0.0001f, 0.0001f);

	public Vector3 angularSpringConstant = Vector3.one * 5f;

	public Vector3 angularSpringDampen = Vector3.one * 0.1f;

	public float angularWeightMass = 5f;

	[SerializeField]
	public BobForceCurve[] additionalCurves;

	public AnimationCurve allowCurve;

	public AnimationCurve forbidCurve;

	public float solveRate = 100f;

	public Vector3 impulseForceScale = Vector3.one;

	public float impulseForceSmooth = 0.02f;

	public float impulseForceMaxChangeAcceleration = Single.PositiveInfinity;

	public Vector3 angularImpulseForceScale = Vector3.one;

	public float angleImpulseForceSmooth = 0.02f;

	public float angleImpulseForceMaxChangeAcceleration = Single.PositiveInfinity;

	public float intermitRate = 20f;

	public BobAntiOutput[] antiOutputs;

	public BobConfiguration()
	{
	}
}