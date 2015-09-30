using Facepunch.Precision;
using System;
using UnityEngine;

public sealed class HeadBob : MonoBehaviour, ICameraFX
{
	public BobConfiguration cfg;

	[SerializeField]
	private CCMotor _motor;

	[SerializeField]
	private CameraMount _mount;

	[SerializeField]
	private float _globalScalar = 1f;

	[SerializeField]
	private float _globalPositionScalar = 1f;

	[SerializeField]
	private float _globalRotationScalar = 1f;

	private static double bob_scale;

	private static double bob_scale_linear;

	private static double bob_scale_angular;

	private float _viewModelPositionScalar = 1f;

	private float _viewModelRotationScalar = 1f;

	private float _aimPositionScalar = 1f;

	private float _aimRotationScalar = 1f;

	public bool simStep = true;

	public bool allowOnEnable = true;

	public bool forceForbidOnDisable;

	public bool allowAntiOutputs;

	private Transform otherParent;

	private ViewModel viewModel;

	private Matrix4x4G worldToLocal;

	private Matrix4x4G localToWorld;

	private Vector3G localVelocity;

	private Vector3G worldVelocity;

	private Vector3G groundLocalVelocity;

	private Vector3G groundWorldVelocity;

	private Vector3G localAngularVelocity;

	private Vector3G groundLocalAngularVelocity;

	private double localVelocityMag;

	private double worldVelocityMag;

	private double groundLocalVelocityMag;

	private double groundWorldVelocityMag;

	private double localAngularVelocityMag;

	private double groundLocalAngularVelocityMag;

	private Vector3G inputForce;

	private Vector3G raw_pos;

	private Vector3G raw_rot;

	private double timeSolve;

	private double timeIntermit;

	private int additionalCurveCount;

	private HeadBob.Weight working;

	private HeadBob.Weight predicted;

	private HeadBob.Weight intermitStart;

	private HeadBob.Weight intermitNext;

	private double intermitFraction;

	private HeadBob.VectorAccelSampler impulseForce;

	private HeadBob.VectorAccelSampler impulseTorque;

	private HeadBob.VectorStamp lastPosition;

	private HeadBob.VectorStamp lastRotation;

	private float allowFractionNormalized;

	private float allowValue;

	private Vector3 preCullLP;

	private Vector3 preCullLR;

	private bool anyAdditionalCurves;

	private bool _allow;

	private bool awake;

	private bool added;

	private bool hadMotor;

	private bool _wasForbidden;

	public float aimPositionScalar
	{
		get
		{
			return this._aimPositionScalar;
		}
		set
		{
			this._aimPositionScalar = value;
		}
	}

	public float aimRotationScalar
	{
		get
		{
			return this._aimRotationScalar;
		}
		set
		{
			this._aimRotationScalar = value;
		}
	}

	public bool allow
	{
		get
		{
			return (!this._allow ? false : base.enabled);
		}
		set
		{
			this._allow = value;
			if (value)
			{
				this._wasForbidden = false;
				base.enabled = true;
			}
		}
	}

	public float globalPositionScalar
	{
		get
		{
			return this._globalPositionScalar;
		}
	}

	public float globalRotationScalar
	{
		get
		{
			return this._globalRotationScalar;
		}
	}

	public float globalScalar
	{
		get
		{
			return this._globalScalar;
		}
	}

	private Vector3 offset
	{
		get
		{
			Vector3 rawPos = new Vector3();
			double num = (double)this.allowValue * this.positionScalar;
			rawPos.x = (float)(this.raw_pos.x * num);
			rawPos.y = (float)(this.raw_pos.y * num);
			rawPos.z = (float)(this.raw_pos.z * num);
			return rawPos;
		}
	}

	public double positionScalar
	{
		get
		{
			return HeadBob.bob_scale * HeadBob.bob_scale_linear * (double)this._globalScalar * (double)this._globalPositionScalar * (double)this._viewModelPositionScalar * (double)this._aimPositionScalar;
		}
	}

	private Vector3 rotationOffset
	{
		get
		{
			Vector3 rawRot = new Vector3();
			double num = (double)this.allowValue * this.rotationScalar;
			rawRot.x = (float)(this.raw_rot.x * num);
			rawRot.y = (float)(this.raw_rot.y * num);
			rawRot.z = (float)(this.raw_rot.z * num);
			return rawRot;
		}
	}

	public double rotationScalar
	{
		get
		{
			return HeadBob.bob_scale * HeadBob.bob_scale_angular * (double)this._globalScalar * (double)this._globalRotationScalar * (double)this._viewModelRotationScalar * (double)this._aimRotationScalar;
		}
	}

	public float viewModelPositionScalar
	{
		get
		{
			return this._viewModelPositionScalar;
		}
		set
		{
			this._viewModelPositionScalar = value;
		}
	}

	public float viewModelRotationScalar
	{
		get
		{
			return this._viewModelRotationScalar;
		}
		set
		{
			this._viewModelRotationScalar = value;
		}
	}

	static HeadBob()
	{
		HeadBob.bob_scale = 1;
		HeadBob.bob_scale_linear = 1;
		HeadBob.bob_scale_angular = 1;
	}

	public HeadBob()
	{
	}

	public bool AddEffect(BobEffect effect)
	{
		return this.working.stack.CreateInstance(effect);
	}

	private bool Advance(float dt)
	{
		bool flag = false;
		if (!this._motor)
		{
			flag = this.CheckChanges(false, base.transform.parent ?? base.transform);
			this.PushPosition();
		}
		else
		{
			flag = this.CheckChanges(true, this._motor.idMain.transform);
			this.PushPosition();
			this.GatherInfo(this._motor);
		}
		if (this.cfg.additionalCurves == null)
		{
			this.additionalCurveCount = 0;
			this.anyAdditionalCurves = false;
		}
		else
		{
			int length = (int)this.cfg.additionalCurves.Length;
			int num = length;
			this.additionalCurveCount = length;
			this.anyAdditionalCurves = num > 0;
		}
		if (this.anyAdditionalCurves)
		{
			Array.Resize<Vector3G>(ref this.working.additionalPositions, this.additionalCurveCount);
			Array.Resize<Vector3G>(ref this.predicted.additionalPositions, this.additionalCurveCount);
		}
		if (!this._allow)
		{
			if (this.allowFractionNormalized > 0f)
			{
				int num1 = this.cfg.forbidCurve.length;
				if ((float)num1 != 0f)
				{
					HeadBob headBob = this;
					headBob.allowFractionNormalized = headBob.allowFractionNormalized - dt / (float)num1;
					if (this.allowFractionNormalized > 0f)
					{
						this.allowValue = 1f - this.cfg.forbidCurve.Evaluate((1f - this.allowFractionNormalized) * (float)num1);
					}
					else
					{
						this.allowFractionNormalized = 0f;
						this.allowValue = 0f;
					}
				}
				else
				{
					this.allowFractionNormalized = 0f;
					this.allowValue = 0f;
				}
				flag = true;
			}
			if (this._wasForbidden && this.allowFractionNormalized == 0f)
			{
				base.enabled = false;
			}
		}
		else if (this.allowFractionNormalized < 1f)
		{
			int num2 = this.cfg.allowCurve.length;
			if ((float)num2 != 0f)
			{
				HeadBob headBob1 = this;
				headBob1.allowFractionNormalized = headBob1.allowFractionNormalized + dt / (float)num2;
				if (this.allowFractionNormalized < 1f)
				{
					this.allowValue = this.cfg.allowCurve.Evaluate(this.allowFractionNormalized * (float)num2);
				}
				else
				{
					this.allowFractionNormalized = 1f;
					this.allowValue = 1f;
				}
			}
			else
			{
				this.allowFractionNormalized = 1f;
				this.allowValue = 1f;
			}
			flag = true;
		}
		if (this.Step(dt) == 0 && !flag)
		{
			return false;
		}
		return true;
	}

	private void Awake()
	{
		this.awake = true;
		this.working.stack = new BobEffectStack();
		this.predicted.stack = this.working.stack.Fork();
	}

	private bool CheckChanges(bool hasMotor, Transform parent)
	{
		if (this.hadMotor == hasMotor && !(this.otherParent != parent))
		{
			return false;
		}
		this.hadMotor = hasMotor;
		this.groundLocalVelocity = new Vector3G();
		this.groundWorldVelocity = new Vector3G();
		this.localVelocity = new Vector3G();
		this.worldVelocity = new Vector3G();
		this.impulseForce = new HeadBob.VectorAccelSampler();
		this.impulseTorque = new HeadBob.VectorAccelSampler();
		this.lastPosition = new HeadBob.VectorStamp();
		this.otherParent = parent;
		this.raw_pos = new Vector3G();
		this.raw_rot = new Vector3G();
		BobEffectStack bobEffectStack = this.predicted.stack;
		this.predicted = new HeadBob.Weight()
		{
			stack = bobEffectStack
		};
		bobEffectStack = this.working.stack;
		this.working = new HeadBob.Weight()
		{
			stack = bobEffectStack
		};
		return true;
	}

	private void CheckDeadZone()
	{
		if (this.raw_pos.x >= (double)(-this.cfg.positionDeadzone.x) && this.raw_pos.x < (double)this.cfg.positionDeadzone.x)
		{
			this.raw_pos.x = 0;
		}
		if (this.raw_pos.y >= (double)(-this.cfg.positionDeadzone.y) && this.raw_pos.y < (double)this.cfg.positionDeadzone.y)
		{
			this.raw_pos.y = 0;
		}
		if (this.raw_pos.z >= (double)(-this.cfg.positionDeadzone.z) && this.raw_pos.z < (double)this.cfg.positionDeadzone.z)
		{
			this.raw_pos.z = 0;
		}
		if (this.raw_rot.x >= (double)(-this.cfg.rotationDeadzone.x) && this.raw_rot.x < (double)this.cfg.rotationDeadzone.x)
		{
			this.raw_rot.x = 0;
		}
		if (this.raw_rot.y >= (double)(-this.cfg.rotationDeadzone.y) && this.raw_rot.y < (double)this.cfg.rotationDeadzone.y)
		{
			this.raw_rot.y = 0;
		}
		if (this.raw_rot.z >= (double)(-this.cfg.rotationDeadzone.z) && this.raw_rot.z < (double)this.cfg.rotationDeadzone.z)
		{
			this.raw_rot.z = 0;
		}
	}

	private static void DrawForceAxes(Vector3 force, Vector3 radii, Vector3 k, float boxDim)
	{
		Color color = Gizmos.color;
		Gizmos.color = color * Color.red;
		HeadBob.DrawForceLine(Vector3.right, force, radii, k, boxDim);
		Gizmos.color = color * Color.green;
		HeadBob.DrawForceLine(Vector3.up, force, radii, k, boxDim);
		Gizmos.color = color * Color.blue;
		HeadBob.DrawForceLine(Vector3.forward, force, radii, k, boxDim);
		Gizmos.color = color;
	}

	private static void DrawForceLine(Vector3 posdir, Vector3 force, Vector3 radii, Vector3 k, float boxDim)
	{
		Vector3 vector3 = Vector3.Scale(radii, posdir);
		Vector3 vector31 = vector3 * 2f;
		float single = Vector3.Dot(force, posdir) / (Vector3.Dot(posdir, radii) * Vector3.Dot(posdir, k));
		if (single < 0f)
		{
			single = -single;
			vector3 = -vector3;
			vector31 = -vector31;
			posdir = -posdir;
		}
		Vector3 vector32 = vector3 + ((vector31 - vector3) * single);
		Color color = Gizmos.color;
		Gizmos.color = color * new Color(1f, 1f, 1f, 0.5f);
		Matrix4x4 matrix4x4 = Gizmos.matrix;
		Gizmos.matrix = Gizmos.matrix * Matrix4x4.TRS(Vector3.zero, Quaternion.LookRotation(posdir), new Vector3(1f, 1f, 1f));
		float single1 = Vector3.Dot(posdir, vector3);
		float single2 = Vector3.Dot(posdir, vector31) - single1;
		float single3 = Vector3.Dot(posdir, vector32) - single1;
		Gizmos.DrawWireCube(new Vector3(0f, 0f, single1 + single2 / 2f), new Vector3(boxDim, boxDim, single2));
		Gizmos.DrawWireCube(new Vector3(0f, 0f, -(single1 + single2 / 2f)), new Vector3(boxDim, boxDim, single2));
		Gizmos.color = color;
		Gizmos.DrawCube(new Vector3(0f, 0f, single1 + single3 / 2f), new Vector3(boxDim, boxDim, single3));
		Gizmos.matrix = matrix4x4;
	}

	private void GatherInfo(CCMotor motor)
	{
		if (!motor.isGrounded || motor.isSliding)
		{
			this.groundLocalVelocity = new Vector3G();
			this.groundWorldVelocity = new Vector3G();
			this.groundLocalAngularVelocity = new Vector3G();
			this.groundLocalVelocityMag = 0;
			this.groundWorldVelocityMag = 0;
			this.groundLocalAngularVelocityMag = 0;
		}
		else
		{
			this.groundLocalVelocity = this.localVelocity;
			this.groundWorldVelocity = this.worldVelocity;
			this.groundLocalAngularVelocity = this.localAngularVelocity;
			this.groundLocalVelocityMag = this.localVelocityMag;
			this.groundWorldVelocityMag = this.worldVelocityMag;
			this.groundLocalAngularVelocityMag = this.localAngularVelocityMag;
		}
		this.inputForce.x = (double)motor.input.moveDirection.x;
		this.inputForce.y = (double)motor.input.moveDirection.y;
		this.inputForce.z = (double)motor.input.moveDirection.z;
		Matrix4x4G.Mult3x3(ref this.inputForce, ref this.worldToLocal, out this.inputForce);
		this.inputForce.x = this.inputForce.x * (double)this.cfg.inputForceMultiplier.x;
		this.inputForce.y = this.inputForce.y * (double)this.cfg.inputForceMultiplier.y;
		this.inputForce.z = this.inputForce.z * (double)this.cfg.inputForceMultiplier.z;
	}

	void ICameraFX.OnViewModelChange(ViewModel viewModel)
	{
		Transform transforms;
		Transform transforms1;
		if (this.viewModel != viewModel)
		{
			this._viewModelPositionScalar = 1f;
			this._viewModelRotationScalar = 1f;
			if (!this.viewModel)
			{
				transforms = base.transform;
			}
			else
			{
				transforms = this.viewModel.transform;
				this.viewModel.headBob = null;
			}
			if (!viewModel)
			{
				transforms1 = base.transform;
			}
			else
			{
				viewModel.headBob = this;
				transforms1 = viewModel.transform;
			}
			this.viewModel = viewModel;
			if (!transforms)
			{
				transforms = null;
			}
			if (!transforms1)
			{
				transforms1 = null;
			}
		}
	}

	void ICameraFX.PostRender()
	{
		Transform transforms = base.transform;
		transforms.localPosition = transforms.localPosition - this.preCullLP;
		Transform transforms1 = base.transform;
		transforms1.localEulerAngles = transforms1.localEulerAngles - this.preCullLR;
		if (this.added)
		{
			bool flag = this.viewModel;
			int num = (this.cfg.antiOutputs == null || !this.allowAntiOutputs ? 0 : (int)this.cfg.antiOutputs.Length);
			if (flag)
			{
				Transform transforms2 = this.viewModel.transform;
				for (int i = num - 1; i >= 0; i--)
				{
					this.cfg.antiOutputs[i].Subtract(transforms2);
				}
			}
			this.added = false;
		}
	}

	void ICameraFX.PreCull()
	{
		Transform transforms;
		int num = (this.cfg.antiOutputs == null || !this.allowAntiOutputs ? 0 : (int)this.cfg.antiOutputs.Length);
		bool flag = this.viewModel;
		if (!flag)
		{
			transforms = null;
		}
		else
		{
			transforms = this.viewModel.transform;
		}
		Transform transforms1 = transforms;
		if (flag && this.added)
		{
			for (int i = num - 1; i >= 0; i--)
			{
				this.cfg.antiOutputs[i].Subtract(transforms1);
			}
		}
		this.Advance(Time.deltaTime);
		this.preCullLP = this.offset;
		this.preCullLR = this.rotationOffset;
		Transform transforms2 = base.transform;
		transforms2.localPosition = transforms2.localPosition + this.preCullLP;
		Transform transforms3 = base.transform;
		transforms3.localEulerAngles = transforms3.localEulerAngles + this.preCullLR;
		num = (this.cfg.antiOutputs == null || !this.allowAntiOutputs ? 0 : (int)this.cfg.antiOutputs.Length);
		if (flag)
		{
			this.added = true;
			for (int j = num - 1; j >= 0; j--)
			{
				this.cfg.antiOutputs[j].Add(transforms1, ref this.preCullLP, ref this.preCullLR);
			}
		}
	}

	private void LateUpdate()
	{
		if (!base.camera)
		{
			if (this.Advance(Time.deltaTime))
			{
				base.transform.localPosition = this.offset;
				base.transform.localEulerAngles = this.rotationOffset;
			}
		}
		else if (!this._allow && this._mount && !this._mount.isActiveMount)
		{
			base.enabled = false;
		}
	}

	private void OnDestroy()
	{
		this.forceForbidOnDisable = false;
	}

	private void OnDisable()
	{
		if (this.awake)
		{
			if (this.forceForbidOnDisable && this._allow)
			{
				base.enabled = true;
				this._wasForbidden = base.enabled;
				if (this._wasForbidden)
				{
					this._allow = false;
					return;
				}
			}
			this.allowFractionNormalized = 0f;
			this.allowValue = 0f;
			base.transform.localPosition = Vector3.zero;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = ((!base.transform.parent ? base.transform : base.transform.parent)).localToWorldMatrix;
		Gizmos.DrawLine(Vector3.zero, this.offset);
		Matrix4x4 matrix4x4 = Gizmos.matrix;
		Gizmos.matrix = Gizmos.matrix * Matrix4x4.Scale(this.cfg.elipsoidRadii);
		Gizmos.DrawWireSphere(Vector3.zero, 1f);
		Gizmos.matrix = matrix4x4;
		Gizmos.color = new Color(1f, 1f, 1f, 0.8f);
		HeadBob.DrawForceAxes(this.working.position.acceleration.f, this.cfg.elipsoidRadii, this.cfg.springConstant, 0.2f);
		Gizmos.color = Color.white;
		HeadBob.DrawForceAxes(this.working.position.acceleration.f, this.cfg.elipsoidRadii, this.cfg.maxVelocity, 0.1f);
	}

	private void OnEnable()
	{
		if (this.allowOnEnable)
		{
			this._allow = true;
		}
		this._wasForbidden = false;
	}

	private void OnLocallyAppended(IDMain main)
	{
		if (!this._motor)
		{
			this._motor = main.GetRemote<CCMotor>();
		}
	}

	private void PushPosition()
	{
		HeadBob.VectorStamp vectorStamp = new HeadBob.VectorStamp();
		HeadBob.VectorStamp vectorStamp1 = new HeadBob.VectorStamp();
		Vector3 vector3;
		Vector3 vector31;
		this.worldToLocal.f = this.otherParent.worldToLocalMatrix;
		this.localToWorld.f = this.otherParent.localToWorldMatrix;
		vectorStamp.timeStamp = Time.time;
		vectorStamp.valid = true;
		if (this._motor)
		{
			Character character = this._motor.idMain as Character;
			Character character1 = character;
			if (!character)
			{
				goto Label1;
			}
			vector31 = character1.eyesAngles.eulerAngles;
			vector3 = character1.eyesOrigin;
			goto Label0;
		}
	Label1:
		vector31 = this.otherParent.eulerAngles;
		vector3 = this.otherParent.position;
	Label0:
		vectorStamp.vector.x = (double)vector3.x;
		vectorStamp.vector.y = (double)vector3.y;
		vectorStamp.vector.z = (double)vector3.z;
		vectorStamp1.vector.x = (double)vector31.x;
		vectorStamp1.vector.y = (double)vector31.y;
		vectorStamp1.vector.z = (double)vector31.z;
		vectorStamp1.timeStamp = Time.time;
		vectorStamp1.valid = true;
		if (this.lastPosition.valid && this.lastPosition.timeStamp != vectorStamp.timeStamp)
		{
			double num = 1 / (double)(vectorStamp.timeStamp - this.lastPosition.timeStamp);
			this.worldVelocity.x = (vectorStamp.vector.x - this.lastPosition.vector.x) * num;
			this.worldVelocity.y = (vectorStamp.vector.y - this.lastPosition.vector.y) * num;
			this.worldVelocity.z = (vectorStamp.vector.z - this.lastPosition.vector.z) * num;
			Matrix4x4G.Mult3x3(ref this.worldVelocity, ref this.worldToLocal, out this.localVelocity);
		}
		this.impulseForce.Sample(ref this.localVelocity, vectorStamp.timeStamp);
		this.lastPosition = vectorStamp;
		if (this.lastRotation.valid && this.lastRotation.timeStamp != vectorStamp1.timeStamp)
		{
			double num1 = 1 / (double)(vectorStamp1.timeStamp - this.lastRotation.timeStamp);
			Precise.DeltaAngle(ref this.lastRotation.vector.x, ref vectorStamp1.vector.x, out this.localAngularVelocity.x);
			Precise.DeltaAngle(ref this.lastRotation.vector.y, ref vectorStamp1.vector.y, out this.localAngularVelocity.y);
			Precise.DeltaAngle(ref this.lastRotation.vector.z, ref vectorStamp1.vector.z, out this.localAngularVelocity.z);
			this.localAngularVelocity.x = this.localAngularVelocity.x * num1;
			this.localAngularVelocity.y = this.localAngularVelocity.y * num1;
			this.localAngularVelocity.z = this.localAngularVelocity.z * num1;
		}
		this.impulseTorque.Sample(ref this.localAngularVelocity, vectorStamp1.timeStamp);
		this.lastRotation = vectorStamp1;
		this.localVelocityMag = Math.Sqrt(this.localVelocity.x * this.localVelocity.x + this.localVelocity.y * this.localVelocity.y + this.localVelocity.z * this.localVelocity.z);
		this.worldVelocityMag = Math.Sqrt(this.worldVelocity.x * this.worldVelocity.x + this.worldVelocity.y * this.worldVelocity.y + this.worldVelocity.z * this.worldVelocity.z);
		this.localAngularVelocityMag = Math.Sqrt(this.localAngularVelocity.x * this.localAngularVelocity.x + this.localAngularVelocity.y * this.localAngularVelocity.y + this.localAngularVelocity.z * this.localAngularVelocity.z);
	}

	private void Solve(ref HeadBob.Weight weight, ref double dt)
	{
		Vector3G vector3G = new Vector3G();
		double num;
		vector3G.x = dt * this.groundLocalVelocity.x * (double)this.cfg.forceSpeedMultiplier.x;
		vector3G.y = dt * this.groundLocalVelocity.y * (double)this.cfg.forceSpeedMultiplier.y;
		vector3G.z = dt * this.groundLocalVelocity.z * (double)this.cfg.forceSpeedMultiplier.z;
		Vector3G vector3G1 = weight.position.fE;
		Vector3G vector3G2 = weight.rotation.fE;
		Vector3G vector3G3 = new Vector3G();
		weight.position.fE = vector3G3;
		Vector3G vector3G4 = new Vector3G();
		weight.rotation.fE = vector3G4;
		if (this.anyAdditionalCurves)
		{
			for (int i = 0; i < this.additionalCurveCount; i++)
			{
				BobForceCurve bobForceCurve = this.cfg.additionalCurves[i];
				switch (bobForceCurve.source)
				{
					case BobForceCurveSource.LocalMovementMagnitude:
					{
						num = this.groundLocalVelocityMag;
						break;
					}
					case BobForceCurveSource.LocalMovementX:
					{
						num = this.groundLocalVelocity.x;
						break;
					}
					case BobForceCurveSource.LocalMovementY:
					{
						num = this.groundLocalVelocity.y;
						break;
					}
					case BobForceCurveSource.LocalMovementZ:
					{
						num = this.groundLocalVelocity.z;
						break;
					}
					case BobForceCurveSource.WorldMovementMagnitude:
					{
						num = this.groundWorldVelocityMag;
						break;
					}
					case BobForceCurveSource.WorldMovementX:
					{
						num = this.groundWorldVelocity.x;
						break;
					}
					case BobForceCurveSource.WorldMovementY:
					{
						num = this.groundWorldVelocity.y;
						break;
					}
					case BobForceCurveSource.WorldMovementZ:
					{
						num = this.groundWorldVelocity.z;
						break;
					}
					case BobForceCurveSource.LocalVelocityMagnitude:
					{
						num = this.localVelocityMag;
						break;
					}
					case BobForceCurveSource.LocalVelocityX:
					{
						num = this.localVelocity.x;
						break;
					}
					case BobForceCurveSource.LocalVelocityY:
					{
						num = this.localVelocity.y;
						break;
					}
					case BobForceCurveSource.LocalVelocityZ:
					{
						num = this.localVelocity.z;
						break;
					}
					case BobForceCurveSource.WorldVelocityMagnitude:
					{
						num = this.worldVelocityMag;
						break;
					}
					case BobForceCurveSource.WorldVelocityX:
					{
						num = this.worldVelocity.x;
						break;
					}
					case BobForceCurveSource.WorldVelocityY:
					{
						num = this.worldVelocity.y;
						break;
					}
					case BobForceCurveSource.WorldVelocityZ:
					{
						num = this.worldVelocity.z;
						break;
					}
					case BobForceCurveSource.RotationMagnitude:
					{
						num = this.localAngularVelocityMag;
						break;
					}
					case BobForceCurveSource.RotationPitch:
					{
						num = this.localAngularVelocity.x;
						break;
					}
					case BobForceCurveSource.RotationYaw:
					{
						num = this.localAngularVelocity.y;
						break;
					}
					case BobForceCurveSource.RotationRoll:
					{
						num = this.localAngularVelocity.z;
						break;
					}
					case BobForceCurveSource.TurnMagnitude:
					{
						num = this.groundLocalAngularVelocityMag;
						break;
					}
					case BobForceCurveSource.TurnPitch:
					{
						num = this.groundLocalAngularVelocity.x;
						break;
					}
					case BobForceCurveSource.TurnYaw:
					{
						num = this.groundLocalAngularVelocity.y;
						break;
					}
					case BobForceCurveSource.TurnRoll:
					{
						num = this.groundLocalAngularVelocity.z;
						break;
					}
					default:
					{
						goto case BobForceCurveSource.LocalVelocityZ;
					}
				}
				BobForceCurveTarget bobForceCurveTarget = bobForceCurve.target;
				if (bobForceCurveTarget != BobForceCurveTarget.Position)
				{
					if (bobForceCurveTarget != BobForceCurveTarget.Rotation)
					{
						goto Label3;
					}
					bobForceCurve.Calculate(ref weight.additionalPositions[i], ref num, ref dt, ref weight.rotation.fE);
					goto Label1;
				}
			Label3:
				bobForceCurve.Calculate(ref weight.additionalPositions[i], ref num, ref dt, ref weight.position.fE);
			Label1:
			}
		}
		if (this.cfg.impulseForceSmooth <= 0f)
		{
			weight.position.fI = this.impulseForce.accel;
		}
		else
		{
			Vector3G.SmoothDamp(ref weight.position.fI, ref this.impulseForce.accel, ref weight.position.fIV, this.cfg.impulseForceSmooth, this.cfg.impulseForceMaxChangeAcceleration, ref dt);
		}
		if (this.cfg.angleImpulseForceSmooth <= 0f)
		{
			weight.rotation.fI = this.impulseTorque.accel;
		}
		else
		{
			Vector3G.SmoothDamp(ref weight.rotation.fI, ref this.impulseTorque.accel, ref weight.rotation.fIV, this.cfg.angleImpulseForceSmooth, this.cfg.angleImpulseForceMaxChangeAcceleration, ref dt);
		}
		weight.position.fE.x = weight.position.fE.x + (this.inputForce.x + weight.position.fI.x * (double)this.cfg.impulseForceScale.x);
		weight.position.fE.y = weight.position.fE.y + (this.inputForce.y + weight.position.fI.y * (double)this.cfg.impulseForceScale.y);
		weight.position.fE.z = weight.position.fE.z + (this.inputForce.z + weight.position.fI.z * (double)this.cfg.impulseForceScale.z);
		weight.rotation.fE.x = weight.rotation.fE.x + weight.rotation.fI.x * (double)this.cfg.angularImpulseForceScale.x;
		weight.rotation.fE.y = weight.rotation.fE.y + weight.rotation.fI.y * (double)this.cfg.angularImpulseForceScale.y;
		weight.rotation.fE.z = weight.rotation.fE.z + weight.rotation.fI.z * (double)this.cfg.angularImpulseForceScale.z;
		Vector3G vector3G5 = weight.position.@value;
		vector3G5.x = vector3G5.x / (double)this.cfg.elipsoidRadii.x;
		vector3G5.y = vector3G5.y / (double)this.cfg.elipsoidRadii.y;
		vector3G5.z = vector3G5.z / (double)this.cfg.elipsoidRadii.z;
		double num1 = vector3G5.x * vector3G5.x + vector3G5.y * vector3G5.y + vector3G5.z * vector3G5.z;
		if (num1 > 1)
		{
			num1 = 1 / Math.Sqrt(num1);
			vector3G5.x = vector3G5.x * num1;
			vector3G5.y = vector3G5.y * num1;
			vector3G5.z = vector3G5.z * num1;
		}
		vector3G5.x = vector3G5.x * (double)this.cfg.elipsoidRadii.x;
		vector3G5.y = vector3G5.y * (double)this.cfg.elipsoidRadii.y;
		vector3G5.z = vector3G5.z * (double)this.cfg.elipsoidRadii.z;
		weight.stack.Simulate(ref dt, ref weight.position.fE, ref weight.rotation.fE);
		weight.position.acceleration.x = weight.position.fE.x - vector3G1.x + (vector3G5.x * (double)(-this.cfg.springConstant.x) - weight.position.velocity.x * (double)this.cfg.springDampen.x) * (double)this.cfg.weightMass;
		weight.position.acceleration.y = weight.position.fE.y - vector3G1.y + (vector3G5.y * (double)(-this.cfg.springConstant.y) - weight.position.velocity.y * (double)this.cfg.springDampen.y) * (double)this.cfg.weightMass;
		weight.position.acceleration.z = weight.position.fE.z - vector3G1.z + (vector3G5.z * (double)(-this.cfg.springConstant.z) - weight.position.velocity.z * (double)this.cfg.springDampen.z) * (double)this.cfg.weightMass;
		weight.position.velocity.x = weight.position.velocity.x + weight.position.acceleration.x * dt;
		weight.position.velocity.y = weight.position.velocity.y + weight.position.acceleration.y * dt;
		weight.position.velocity.z = weight.position.velocity.z + weight.position.acceleration.z * dt;
		if (float.IsInfinity(this.cfg.maxVelocity.x))
		{
			weight.position.@value.x = weight.position.@value.x + weight.position.velocity.x * dt;
		}
		else if (weight.position.velocity.x < (double)(-this.cfg.maxVelocity.x))
		{
			weight.position.@value.x = weight.position.@value.x - (double)this.cfg.maxVelocity.x * dt;
		}
		else if (weight.position.velocity.x <= (double)this.cfg.maxVelocity.x)
		{
			weight.position.@value.x = weight.position.@value.x + weight.position.velocity.x * dt;
		}
		else
		{
			weight.position.@value.x = weight.position.@value.x + (double)this.cfg.maxVelocity.x * dt;
		}
		if (float.IsInfinity(this.cfg.maxVelocity.y))
		{
			weight.position.@value.y = weight.position.@value.y + weight.position.velocity.y * dt;
		}
		else if (weight.position.velocity.y < (double)(-this.cfg.maxVelocity.y))
		{
			weight.position.@value.y = weight.position.@value.y - (double)this.cfg.maxVelocity.y * dt;
		}
		else if (weight.position.velocity.y <= (double)this.cfg.maxVelocity.y)
		{
			weight.position.@value.y = weight.position.@value.y + weight.position.velocity.y * dt;
		}
		else
		{
			weight.position.@value.y = weight.position.@value.y + (double)this.cfg.maxVelocity.y * dt;
		}
		if (float.IsInfinity(this.cfg.maxVelocity.z))
		{
			weight.position.@value.z = weight.position.@value.z + weight.position.velocity.z * dt;
		}
		else if (weight.position.velocity.z < (double)(-this.cfg.maxVelocity.z))
		{
			weight.position.@value.z = weight.position.@value.z - (double)this.cfg.maxVelocity.z * dt;
		}
		else if (weight.position.velocity.z <= (double)this.cfg.maxVelocity.z)
		{
			weight.position.@value.z = weight.position.@value.z + weight.position.velocity.z * dt;
		}
		else
		{
			weight.position.@value.z = weight.position.@value.z + (double)this.cfg.maxVelocity.z * dt;
		}
		weight.rotation.acceleration.x = weight.rotation.fE.x - vector3G2.x + (weight.rotation.@value.x * (double)(-this.cfg.angularSpringConstant.x) - weight.rotation.velocity.x * (double)this.cfg.angularSpringDampen.x) * (double)this.cfg.angularWeightMass;
		weight.rotation.acceleration.y = weight.rotation.fE.y - vector3G2.y + (weight.rotation.@value.y * (double)(-this.cfg.angularSpringConstant.y) - weight.rotation.velocity.y * (double)this.cfg.angularSpringDampen.y) * (double)this.cfg.angularWeightMass;
		weight.rotation.acceleration.z = weight.rotation.fE.z - vector3G2.z + (weight.rotation.@value.z * (double)(-this.cfg.angularSpringConstant.z) - weight.rotation.velocity.z * (double)this.cfg.angularSpringDampen.z) * (double)this.cfg.angularWeightMass;
		weight.rotation.velocity.x = weight.rotation.velocity.x + weight.rotation.acceleration.x * dt;
		weight.rotation.velocity.y = weight.rotation.velocity.y + weight.rotation.acceleration.y * dt;
		weight.rotation.velocity.z = weight.rotation.velocity.z + weight.rotation.acceleration.z * dt;
		weight.rotation.@value.x = weight.rotation.@value.x + weight.rotation.velocity.x * dt;
		weight.rotation.@value.y = weight.rotation.@value.y + weight.rotation.velocity.y * dt;
		weight.rotation.@value.z = weight.rotation.@value.z + weight.rotation.velocity.z * dt;
	}

	private int Step(float dt)
	{
		double num;
		int num1 = 0;
		int num2 = 0;
		HeadBob headBob = this;
		headBob.timeSolve = headBob.timeSolve + (double)dt;
		double num3 = ((double)this.cfg.solveRate >= 0 ? 1 / (double)this.cfg.solveRate : 1 / -(double)this.cfg.solveRate);
		if ((double)this.cfg.intermitRate != 0)
		{
			num = ((double)this.cfg.intermitRate >= 0 ? 1 / (double)this.cfg.intermitRate : 1 / -(double)this.cfg.intermitRate);
		}
		else
		{
			num = 0;
		}
		double num4 = num;
		if (double.IsInfinity(num3) || num3 == 0)
		{
			num3 = this.timeSolve;
		}
		bool flag = num4 > num3;
		double num5 = num3 * (double)this.cfg.timeScale;
		if (this.timeSolve >= num3)
		{
			do
			{
				HeadBob headBob1 = this;
				headBob1.timeSolve = headBob1.timeSolve - num3;
				if (flag)
				{
					HeadBob headBob2 = this;
					headBob2.timeIntermit = headBob2.timeIntermit - num3;
					if (this.timeIntermit < 0)
					{
						this.intermitStart = this.working;
					}
				}
				this.Solve(ref this.working, ref num5);
				if (flag && this.timeIntermit < 0)
				{
					this.intermitNext = this.working;
					this.intermitFraction = (this.timeIntermit + num3) / num3;
					HeadBob headBob3 = this;
					headBob3.timeIntermit = headBob3.timeIntermit + num4;
					num2++;
				}
				num1++;
			}
			while (this.timeSolve >= num3);
		}
		if (flag)
		{
			if (num2 > 0)
			{
				if (!this.simStep)
				{
					this.raw_pos = this.intermitNext.position.@value;
					this.raw_rot = this.intermitNext.rotation.@value;
					this.CheckDeadZone();
				}
				else
				{
					Vector3G.Lerp(ref this.intermitStart.position.@value, ref this.intermitNext.position.@value, ref this.intermitFraction, out this.raw_pos);
					Vector3G.Lerp(ref this.intermitStart.rotation.@value, ref this.intermitNext.rotation.@value, ref this.intermitFraction, out this.raw_rot);
					this.CheckDeadZone();
				}
			}
			return num2;
		}
		if (!this.simStep)
		{
			this.raw_pos = this.working.position.@value;
			this.raw_rot = this.working.rotation.@value;
			this.CheckDeadZone();
		}
		else
		{
			this.working.CopyTo(ref this.predicted);
			this.Solve(ref this.predicted, ref num5);
			num1 = -(num1 + 1);
			double num6 = this.timeSolve / num3;
			Vector3G.Lerp(ref this.working.position.@value, ref this.predicted.position.@value, ref num6, out this.raw_pos);
			Vector3G.Lerp(ref this.working.rotation.@value, ref this.predicted.rotation.@value, ref num6, out this.raw_rot);
			this.CheckDeadZone();
		}
		return num1;
	}

	private struct VectorAccelSampler
	{
		public HeadBob.VectorStamp sample0;

		public HeadBob.VectorStamp sample1;

		public HeadBob.VectorStamp sample2;

		public Vector3G accel;

		public void Sample(ref Vector3G v, float timeStamp)
		{
			if (this.sample1.timeStamp < timeStamp)
			{
				this.sample2 = this.sample1;
			}
			if (this.sample0.timeStamp < timeStamp)
			{
				this.sample1 = this.sample0;
			}
			this.sample0.vector = v;
			this.sample0.timeStamp = timeStamp;
			this.sample0.valid = true;
			Vector3G vector3G = new Vector3G();
			double num = this.sample0.AddDifference(ref this.sample1, ref vector3G) + this.sample0.AddDifference(ref this.sample2, ref vector3G);
			if (num != 0)
			{
				num = 1 / num;
				this.accel.x = vector3G.x * num;
				this.accel.y = vector3G.y * num;
				this.accel.z = vector3G.z * num;
			}
		}
	}

	private struct VectorStamp
	{
		public Vector3G vector;

		public float timeStamp;

		public bool valid;

		public double AddDifference(ref HeadBob.VectorStamp previous, ref Vector3G difference)
		{
			if (!previous.valid || previous.timeStamp == this.timeStamp)
			{
				return 0;
			}
			double num = 1 / (double)(this.timeStamp - previous.timeStamp);
			difference.x = difference.x + num * (this.vector.x - previous.vector.x);
			difference.y = difference.y + num * (this.vector.y - previous.vector.y);
			difference.z = difference.z + num * (this.vector.z - previous.vector.z);
			return 1;
		}
	}

	private struct Weight
	{
		public HeadBob.Weight.Element position;

		public HeadBob.Weight.Element rotation;

		public Vector3G[] additionalPositions;

		public BobEffectStack stack;

		public void CopyTo(ref HeadBob.Weight other)
		{
			if (other.additionalPositions != this.additionalPositions && this.additionalPositions != null)
			{
				Array.Copy(this.additionalPositions, other.additionalPositions, (int)this.additionalPositions.Length);
			}
			other.rotation = this.rotation;
			other.position = this.position;
			if (other.stack != null && other.stack.IsForkOf(this.stack))
			{
				other.stack.Join();
			}
		}

		public struct Element
		{
			public Vector3G @value;

			public Vector3G velocity;

			public Vector3G acceleration;

			public Vector3G fI;

			public Vector3G fE;

			public Vector3G fIV;
		}
	}
}