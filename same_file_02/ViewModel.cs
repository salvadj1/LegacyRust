using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ViewModel : IDRemote, Socket.Source, Socket.Mapped, Socket.Provider
{
	public const int kCap_PerspectiveNear = 1;

	public const int kCap_PerspectiveFar = 2;

	public const int kCap_PerspectiveFOV = 4;

	public const int kCap_PerspectiveAspect = 8;

	protected const int kIdleChannel_Idle = 0;

	protected const int kIdleChannel_IdleMovement = 1;

	protected const int kIdleChannel_Crouch = 2;

	protected const int kIdleChannel_CrouchMovement = 3;

	protected const int kIdleChannel_Bow = 4;

	protected const int kIdleChannel_BowMovement = 5;

	protected const int kIdleChannel_Fall = 6;

	protected const int kIdleChannel_Slip = 7;

	protected const int kIdleChannel_Zoom = 8;

	protected const int kIdleChannelCount = 9;

	protected const string kIdleChannel_Idle_Name = "idle";

	protected const string kIdleChannel_IdleMovement_Name = "move";

	protected const string kIdleChannel_Bow_Name = "bowi";

	protected const string kIdleChannel_BowMovement_Name = "bowm";

	protected const string kIdleChannel_Crouch_Name = "dcki";

	protected const string kIdleChannel_CrouchMovement_Name = "dckm";

	protected const string kIdleChannel_Fall_Name = "fall";

	protected const string kIdleChannel_Slip_Name = "slip";

	protected const string kIdleChannel_Zoom_Name = "zoom";

	[SerializeField]
	public Socket.CameraSpace pivot;

	[SerializeField]
	public Socket.CameraSpace pivot2;

	[SerializeField]
	public Socket.CameraSpace muzzle;

	[SerializeField]
	public Socket.CameraSpace sight;

	[SerializeField]
	public Socket.CameraSpace optics;

	[SerializeField]
	public Socket.CameraSpace bowPivot;

	protected readonly static string[] defaultSocketNames;

	[NonSerialized]
	protected IEnumerable<string> socketNames = ViewModel.defaultSocketNames;

	[NonSerialized]
	protected int socketVersion;

	[NonSerialized]
	private Socket.Map.Member _socketMap;

	private Vector3 originalRootOffset;

	private Quaternion originalRootRotation;

	private bool flipped;

	private Dictionary<Socket, Transform> proxies;

	private bool madeProxyDict;

	public int caps;

	public float perspectiveNearOverride = 0.1f;

	public float perspectiveFarOverride = 25f;

	public float perspectiveFOVOverride = 60f;

	public float perspectiveAspectOverride = 1f;

	public float lazyAngle = 5f;

	public float zoomFieldOfView = 40f;

	public AnimationCurve zoomCurve;

	public Vector3 zoomOffset;

	public Vector3 zoomRotate;

	public Vector3 offset;

	public Vector3 rotate;

	public Transform root;

	public Animation animation;

	public Texture crosshairTexture;

	public Texture dotTexture;

	public float zoomInDuration = 0.5f;

	public float zoomOutDuration = 0.4f;

	public bool showCrosshairZoom;

	public bool showCrosshairNotZoomed = true;

	public Color crosshairColor = Color.white;

	public Color crosshairOutline = Color.black;

	public LayerMask aimMask;

	public AnimationCurve headBobOffsetScale;

	public AnimationCurve headBobRotationScale;

	public bool barrelAiming = true;

	public bool barrelWhileZoom;

	public bool barrelWhileBowing;

	public Vector3 barrelPivot;

	public Vector2 barrelRotation;

	public float barrelLimit;

	public float noHitPlane = 20f;

	public float barrelAngleSmoothDamp = 0.01f;

	public float barrelAngleMaxSpeed = Single.PositiveInfinity;

	public float barrelLimitOffsetFactor = 1f;

	public float barrelLimitPivotFactor;

	public bool bowAllowed;

	public Vector3 bowOffsetPoint;

	public Vector3 bowOffsetAngles;

	public AnimationCurve bowCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	public bool bowCurveIs01Fraction;

	public float bowEnterDuration = 1f;

	public float bowExitDuration = 1f;

	private float bowTime;

	public AnimationCurve zoomPunch;

	public float punchScalar = 1f;

	private float punchTime = -2000f;

	private float zoomPunchValue;

	public string fireAnimName = "fire_1";

	public string deployAnimName = "deploy";

	public string reloadAnimName = "reload";

	public float fireAnimScaleSpeed = 1f;

	[SerializeField]
	protected AnimationBlender.ResidualField idleFrame;

	[SerializeField]
	protected AnimationBlender.ChannelField idleChannel;

	[SerializeField]
	protected AnimationBlender.ChannelField movementIdleChannel;

	[SerializeField]
	protected AnimationBlender.ChannelField bowChannel;

	[SerializeField]
	protected AnimationBlender.ChannelField bowMovementChannel;

	[SerializeField]
	protected AnimationBlender.ChannelField crouchChannel;

	[SerializeField]
	protected AnimationBlender.ChannelField crouchMovementChannel;

	[SerializeField]
	protected AnimationBlender.ChannelField fallChannel;

	[SerializeField]
	protected AnimationBlender.ChannelField slipChannel;

	[SerializeField]
	protected AnimationBlender.ChannelField zoomChannel;

	[NonSerialized]
	protected AnimationBlender.Mixer idleMixer;

	[NonSerialized]
	public ItemRepresentation itemRep;

	[NonSerialized]
	public IHeldItem item;

	[NonSerialized]
	private Angle2 lastLook;

	[SerializeField]
	private SkinnedMeshRenderer[] builtinRenderers;

	[NonSerialized]
	private ViewModel.MeshInstance.Holder meshInstances;

	[NonSerialized]
	private ViewModel.BarrelParameters bpHip;

	[NonSerialized]
	private ViewModel.BarrelParameters bpZoom;

	[NonSerialized]
	private ViewModel.BarrelParameters bpBow;

	private static bool force_legacy_fallback;

	private HeadBob _headBob;

	private LazyCam _lazyCam;

	private Quaternion _additiveRotation = Quaternion.identity;

	private float zoomTime;

	private float headBobLinearTime;

	private float headBobAngularTime;

	private float lastZoomFraction = Single.NaN;

	private float lastHeadBobLinearFraction;

	private float lastHeadBobAngular;

	private Vector3 lastLocalPositionOffset;

	private Vector3 lastLocalRotationOffset;

	private Vector3 lastSightRotation;

	private Transform eye;

	private Transform shelf;

	private List<GameObject> destroyOnUnbind;

	private static bool modifyAiming;

	public bool drawCrosshair
	{
		get
		{
			return (this.showCrosshairZoom ? true : this.showCrosshairNotZoomed);
		}
	}

	public HeadBob headBob
	{
		get
		{
			return this._headBob;
		}
		set
		{
			this._headBob = value;
		}
	}

	public new Character idMain
	{
		get
		{
			return (Character)base.idMain;
		}
	}

	public LazyCam lazyCam
	{
		get
		{
			return this._lazyCam;
		}
		set
		{
			this._lazyCam = value;
		}
	}

	public Quaternion lazyRotation
	{
		get
		{
			return this._additiveRotation;
		}
		set
		{
			if (this._additiveRotation != value)
			{
				this.pivot2.Rotate(this._additiveRotation);
				this.pivot.UnRotate(this._additiveRotation);
				this.pivot.Rotate(value);
				this.pivot2.UnRotate(value);
				this._additiveRotation = value;
			}
		}
	}

	public Vector3 muzzlePosition
	{
		get
		{
			return this.muzzle.position;
		}
	}

	public Quaternion muzzleRotation
	{
		get
		{
			return this.muzzle.rotation;
		}
	}

	IEnumerable<string> Socket.Source.SocketNames
	{
		get
		{
			return this.socketNames;
		}
	}

	int Socket.Source.SocketsVersion
	{
		get
		{
			return this.socketVersion;
		}
	}

	public Socket.Map socketMap
	{
		get
		{
			return this._socketMap.Get<ViewModel>(this);
		}
	}

	static ViewModel()
	{
		ViewModel.defaultSocketNames = new string[] { "muzzle", "sight", "optics", "pivot1", "pivot2", "bowPivot" };
	}

	public ViewModel()
	{
	}

	public void AddRenderers(SkinnedMeshRenderer[] renderers)
	{
		if (renderers != null)
		{
			SkinnedMeshRenderer[] skinnedMeshRendererArray = renderers;
			for (int i = 0; i < (int)skinnedMeshRendererArray.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = skinnedMeshRendererArray[i];
				this.meshInstances.Add(skinnedMeshRenderer);
			}
		}
	}

	protected new void Awake()
	{
		this.originalRootOffset = this.root.localPosition;
		this.originalRootRotation = this.root.localRotation;
		base.Awake();
		if (this.builtinRenderers != null)
		{
			SkinnedMeshRenderer[] skinnedMeshRendererArray = this.builtinRenderers;
			for (int i = 0; i < (int)skinnedMeshRendererArray.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = skinnedMeshRendererArray[i];
				if (skinnedMeshRenderer)
				{
					this.meshInstances.Add(skinnedMeshRenderer);
				}
			}
		}
		this.idleMixer = this.idleFrame.Alias(this.animation, (new AnimationBlender.ChannelConfig[9]).Define(0, "idle", this.idleChannel).Define(1, "move", this.movementIdleChannel).Define(4, "bowi", this.bowChannel).Define(5, "bowm", this.bowMovementChannel).Define(2, "dcki", this.crouchChannel).Define(3, "dckm", this.crouchMovementChannel).Define(6, "fall", this.fallChannel).Define(7, "slip", this.slipChannel).Define(8, "zoom", this.zoomChannel)).Create();
	}

	private ViewModel.BarrelTransform BarrelAim(Vector3 offset, ref ViewModel.BarrelParameters barrel)
	{
		float single;
		Vector3 point;
		RaycastHit2 raycastHit2;
		float single1;
		Vector3 vector3;
		Vector3 vector31;
		ViewModel.BarrelTransform barrelTransform = new ViewModel.BarrelTransform();
		Ray ray = this.idMain.eyesRay;
		if (!Physics2.Raycast2(ray, out raycastHit2, this.noHitPlane, this.aimMask.@value))
		{
			single = this.noHitPlane;
			point = ray.GetPoint(this.noHitPlane);
		}
		else
		{
			single = raycastHit2.distance;
			point = raycastHit2.point;
		}
		point = this.idMain.eyesTransformReadOnly.InverseTransformPoint(point);
		single = point.magnitude;
		Vector3 vector32 = Vector3.Scale(offset + this.barrelPivot, base.transform.localScale);
		Plane plane = new Plane(this.idMain.eyesTransformReadOnly.InverseTransformDirection(ray.direction), vector32);
		Ray ray1 = new Ray(point, -point);
		plane.Raycast(ray1, out single1);
		Vector3 point1 = ray1.GetPoint(single1);
		float single2 = Vector3.Distance(point1, vector32);
		float single3 = Vector3.Distance(point, vector32);
		if (Mathf.Approximately(0f, single3) && barrel.ir)
		{
			barrel.ir = false;
		}
		barrel.bc = single3;
		barrel.ca = single2;
		barrel.a = 90f;
		ViewModel.SolveTriangleSSA(barrel.a, barrel.bc, barrel.ca, out barrel.ab, out barrel.c, out barrel.b);
		barrel.ir = true;
		float single4 = -(90f - barrel.c);
		if (!barrel.once)
		{
			barrel.once = true;
			barrel.angle = single4;
		}
		else if (this.barrelAngleSmoothDamp <= 0f)
		{
			if (this.barrelAngleMaxSpeed <= 0f || this.barrelAngleMaxSpeed == Single.PositiveInfinity)
			{
				barrel.angle = single4;
			}
			else
			{
				barrel.angle = Mathf.MoveTowardsAngle(barrel.angle, single4, this.barrelAngleMaxSpeed * Time.deltaTime);
			}
		}
		else if (this.barrelAngleMaxSpeed > 0f)
		{
			barrel.angle = Mathf.SmoothDampAngle(barrel.angle, single4, ref barrel.angularVelocity, this.barrelAngleSmoothDamp, this.barrelAngleMaxSpeed);
		}
		else
		{
			barrel.angle = Mathf.SmoothDampAngle(barrel.angle, single4, ref barrel.angularVelocity, this.barrelAngleSmoothDamp);
		}
		Quaternion quaternion = Quaternion.Euler(-this.barrelRotation.x, this.barrelRotation.y, 0f);
		Quaternion quaternion1 = Quaternion.Inverse(quaternion);
		float single5 = barrel.angle;
		Plane plane1 = new Plane(point, vector32, Vector3.zero);
		Quaternion quaternion2 = quaternion1 * Quaternion.AngleAxis(single5, plane1.normal);
		if (barrel.bc >= this.barrelLimit)
		{
			vector3 = this.barrelPivot;
			vector31 = offset;
		}
		else
		{
			vector31 = (this.barrelLimitOffsetFactor == 0f ? offset : offset - (quaternion2 * Vector3.forward * ((this.barrelLimit - barrel.bc) * this.barrelLimitOffsetFactor)));
			vector3 = (this.barrelLimitPivotFactor == 0f ? this.barrelPivot : this.barrelPivot + (quaternion * Vector3.back * ((this.barrelLimit - barrel.bc) * this.barrelLimitPivotFactor)));
		}
		barrelTransform.origin = (quaternion2 * -vector3) + vector3;
		barrelTransform.angles = quaternion2.eulerAngles;
		barrelTransform.origin = barrelTransform.origin + vector31;
		barrelTransform.angles.x = Mathf.DeltaAngle(0f, barrelTransform.angles.x);
		barrelTransform.angles.y = Mathf.DeltaAngle(0f, barrelTransform.angles.y);
		barrelTransform.angles.z = Mathf.DeltaAngle(0f, barrelTransform.angles.z);
		return barrelTransform;
	}

	protected void BindCameraSpaceTransforms(Transform newShelf, Transform newEye)
	{
		Transform transforms = this.eye;
		Transform transforms1 = this.shelf;
		this.eye = newEye;
		this.shelf = newShelf;
		if (transforms != newEye || transforms1 != newShelf)
		{
			ViewModel viewModel = this;
			viewModel.socketVersion = viewModel.socketVersion + 1;
		}
	}

	public void BindTransforms(Transform shelf, Transform eye)
	{
		this.punchTime = Time.time - 20f;
		this.BindCameraSpaceTransforms(shelf, eye);
	}

	private void ClearProxies()
	{
		this.DeleteSocketMap();
		if (this.destroyOnUnbind != null)
		{
			foreach (GameObject gameObject in this.destroyOnUnbind)
			{
				if (!gameObject)
				{
					continue;
				}
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.destroyOnUnbind = null;
	}

	public void CrossFade(string name)
	{
		this.idleMixer.CrossFade(name);
	}

	public void CrossFade(string name, float fadeLength)
	{
		this.idleMixer.CrossFade(name, fadeLength);
	}

	public void CrossFade(string name, float fadeLength, PlayMode playMode)
	{
		this.idleMixer.CrossFade(name, fadeLength, playMode);
	}

	public void CrossFade(string name, float fadeLength, PlayMode playMode, float speed)
	{
		this.idleMixer.CrossFade(name, fadeLength, playMode, speed);
	}

	protected void DeleteSocketMap()
	{
		this._socketMap.DeleteBy<ViewModel>(this);
	}

	private void DrawShadowed(ref Rect r, Texture texture)
	{
		Color color = new Color();
		Color color1 = GUI.color;
		if (color1.a > 0.5f)
		{
			Rect rect = r;
			rect.x = rect.x + 1f;
			rect.y = rect.y - 1f;
			color.a = (color1.a - 0.5f) * 2f;
			color.a = this.crosshairOutline.a * (color.a * color.a);
			color.r = this.crosshairOutline.r;
			color.g = this.crosshairOutline.g;
			color.b = this.crosshairOutline.b;
			GUI.color = color;
			GUI.DrawTexture(rect, texture);
			rect.x = rect.x - 2f;
			GUI.DrawTexture(rect, texture);
			rect.y = rect.y + 2f;
			GUI.DrawTexture(rect, texture);
			rect.x = rect.x + 2f;
			GUI.DrawTexture(rect, texture);
			float single = 1f - color.a;
			color.r = this.crosshairColor.r * color.a + this.crosshairOutline.r * single;
			color.g = this.crosshairColor.g * color.a + this.crosshairOutline.g * single;
			color.b = this.crosshairColor.b * color.a + this.crosshairOutline.b * single;
			color.a = this.crosshairColor.a * color.a + this.crosshairOutline.a * single;
			GUI.color = color;
			GUI.DrawTexture(r, texture);
		}
		else if (color1.a > 0f)
		{
			float single1 = color1.a * 2f;
			float single2 = single1 + (single1 - single1 * single1);
			float single3 = 1f - single2;
			GUI.color = new Color(this.crosshairOutline.r * single2 + this.crosshairColor.r * single3, this.crosshairOutline.g * single2 + this.crosshairColor.g * single3, this.crosshairOutline.b * single2 + this.crosshairColor.b * single3, this.crosshairOutline.a * (single1 * single1));
			GUI.DrawTexture(r, texture);
		}
		GUI.color = color1;
	}

	public void Flip()
	{
		if (!this.flipped)
		{
			Vector3 vector3 = base.transform.localScale;
			vector3.z = -vector3.z;
			base.transform.localScale = vector3;
			this.flipped = true;
		}
	}

	protected void LateUpdate()
	{
		bool flag;
		bool flag1;
		bool flag2;
		bool flag3;
		bool flag4;
		bool flag5;
		bool flag6;
		Angle2 angle2;
		float single;
		float single1;
		float single2;
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		Vector3 vector32;
		Vector3 vector33;
		bool flag7;
		Character character = this.idMain;
		if (!character)
		{
			return;
		}
		float single3 = Time.deltaTime;
		if (!character)
		{
			flag6 = false;
			flag4 = false;
			flag5 = false;
			flag3 = false;
			flag2 = false;
			flag = false;
			flag1 = false;
			angle2 = this.lastLook;
		}
		else
		{
			flag6 = character.stateFlags.aim;
			flag4 = !character.stateFlags.grounded;
			flag5 = character.stateFlags.slipping;
			flag2 = character.stateFlags.movement;
			flag = character.stateFlags.aim;
			flag3 = character.stateFlags.crouch;
			angle2 = character.eyesAngles;
			if (!this.bowAllowed)
			{
				flag7 = false;
			}
			else
			{
				flag7 = (!character.stateFlags.sprint ? false : flag2);
			}
			flag1 = flag7;
		}
		if (angle2 != this.lastLook)
		{
			single = Angle2.AngleDistance(this.lastLook, angle2) / single3;
			this.lastLook = angle2;
		}
		else
		{
			single = 0f;
		}
		if (flag4)
		{
			this.idleMixer.SetSolo(6);
		}
		else if (flag5)
		{
			this.idleMixer.SetSolo(7);
		}
		else if (flag6)
		{
			this.idleMixer.SetSolo(8);
		}
		else if (flag1)
		{
			if (!flag2)
			{
				this.idleMixer.SetSolo(4);
			}
			else
			{
				this.idleMixer.SetSolo(5);
			}
		}
		else if (flag3)
		{
			if (!flag2)
			{
				this.idleMixer.SetSolo(2);
			}
			else
			{
				this.idleMixer.SetSolo(3);
			}
		}
		else if (!flag2)
		{
			this.idleMixer.SetSolo(0);
			if (single < -2f || single > 2f)
			{
				this.idleMixer.SetActive(0, false);
			}
		}
		else
		{
			this.idleMixer.SetSolo(1);
		}
		float single4 = Time.deltaTime / (!flag ? -this.zoomOutDuration : this.zoomInDuration);
		float single5 = this.zoomCurve.EvaluateClampedTime(ref this.zoomTime, single4);
		float single6 = Time.deltaTime / (!flag1 ? -this.bowExitDuration : this.bowEnterDuration);
		if (!float.IsInfinity(single6))
		{
			if (this.bowCurveIs01Fraction)
			{
				float item = this.bowCurve[0].time;
				Keyframe keyframe = this.bowCurve[this.bowCurve.length];
				single6 = single6 * (item - keyframe.time);
			}
			single1 = this.bowCurve.EvaluateClampedTime(ref this.bowTime, single6);
		}
		else
		{
			single1 = (!flag1 ? 0f : 1f);
		}
		if (flag1 != flag)
		{
			single2 = (flag || !this.bowAllowed ? single4 : single6);
		}
		else if (!this.bowAllowed)
		{
			single2 = single4;
		}
		else
		{
			single2 = (!flag1 ? Mathf.Min(single1, single4) : Mathf.Max(single1, single4));
		}
		this.root.localPosition = this.originalRootOffset;
		this.root.localRotation = this.originalRootRotation;
		Vector3 vector34 = this.sight.preEyePosition;
		Vector3 vector35 = this.bowPivot.preEyePosition;
		Vector3 vector36 = -this.root.InverseTransformPoint(vector34);
		Vector3 vector37 = -this.root.InverseTransformPoint(vector35);
		Quaternion quaternion = this.sight.preEyeRotation;
		Vector3 vector38 = this.root.InverseTransformDirection(quaternion * Vector3.forward);
		Vector3 vector39 = this.root.InverseTransformDirection(quaternion * Vector3.up);
		quaternion = Quaternion.Inverse(Quaternion.LookRotation(vector38, vector39));
		vector36 = quaternion * vector36;
		Vector3 vector310 = quaternion.eulerAngles;
		Quaternion quaternion1 = this.bowPivot.preEyeRotation;
		Vector3 vector311 = this.root.InverseTransformPoint(quaternion1 * Vector3.forward);
		Vector3 vector312 = this.root.InverseTransformDirection(quaternion1 * Vector3.up);
		quaternion1 = Quaternion.Inverse(Quaternion.LookRotation(vector311, vector312));
		vector37 = quaternion1 * vector37;
		Vector3 vector313 = quaternion1.eulerAngles;
		if (!this.barrelAiming)
		{
			vector32 = this.offset;
			vector33 = this.rotate;
		}
		else
		{
			ViewModel.BarrelTransform barrelTransform = this.BarrelAim(this.offset, ref this.bpHip);
			barrelTransform.Get(out vector32, out vector33);
		}
		if (this.barrelWhileZoom)
		{
			ViewModel.BarrelTransform barrelTransform1 = this.BarrelAim(vector36, ref this.bpZoom);
			barrelTransform1.Get(out vector36, out vector310);
		}
		if (this.barrelWhileBowing)
		{
			ViewModel.BarrelTransform barrelTransform2 = this.BarrelAim(vector37, ref this.bpBow);
			barrelTransform2.Get(out vector37, out vector313);
		}
		float single7 = 1f - single5;
		float single8 = this.zoomPunch.Evaluate(Time.time - this.punchTime) * this.punchScalar;
		float single9 = 1f - single1;
		vector3.x = (vector37.x + this.bowOffsetPoint.x) * single1 + ((vector36.x + this.zoomOffset.x) * single5 + vector32.x * single7) * single9;
		vector3.y = (vector37.y + this.bowOffsetPoint.y) * single1 + ((vector36.y + this.zoomOffset.y) * single5 + vector32.y * single7 * single9);
		vector3.z = (vector37.z + this.bowOffsetPoint.z) * single1 + ((vector36.z + (this.zoomOffset.z - single8)) * single5 + vector32.z * single7) * single9;
		vector31.x = Mathf.DeltaAngle(0f, ((Mathf.DeltaAngle(this.zoomRotate.x, vector310.x) + this.zoomRotate.x) * single5 + vector33.x * single7) * single9 + (Mathf.DeltaAngle(this.bowOffsetAngles.x, vector313.x) + this.bowOffsetAngles.x) * single1);
		vector31.y = Mathf.DeltaAngle(0f, ((Mathf.DeltaAngle(this.zoomRotate.y, vector310.y) + this.zoomRotate.y) * single5 + vector33.y * single7) * single9 + (Mathf.DeltaAngle(this.bowOffsetAngles.y, vector313.y) + this.bowOffsetAngles.y) * single1);
		vector31.z = Mathf.DeltaAngle(0f, ((Mathf.DeltaAngle(this.zoomRotate.z, vector310.z) + this.zoomRotate.z) * single5 + vector33.z * single7) * single9 + (Mathf.DeltaAngle(this.bowOffsetAngles.z, vector313.z) + this.bowOffsetAngles.z) * single1);
		this.lastLocalPositionOffset = vector3;
		this.lastLocalRotationOffset = vector31;
		Transform transforms = this.root;
		transforms.localEulerAngles = transforms.localEulerAngles + this.lastLocalRotationOffset;
		Transform transforms1 = this.root;
		transforms1.localPosition = transforms1.localPosition + this.lastLocalPositionOffset;
		this.lastZoomFraction = single5;
		if (this._headBob)
		{
			CameraFX cameraFX = CameraFX.mainCameraFX;
			if (!cameraFX)
			{
				Debug.Log("No CamFX");
			}
			else
			{
				cameraFX.SetFieldOfView(this.zoomFieldOfView, single5);
			}
		}
		this.pivot.Rotate(this._additiveRotation);
		this.pivot2.UnRotate(this._additiveRotation);
		if (this._lazyCam)
		{
			this._lazyCam.allow = (flag ? 1 : (int)flag1) == 0;
		}
		if (this._headBob)
		{
			this._headBob.viewModelPositionScalar = this.headBobOffsetScale.EvaluateClampedTime(ref this.headBobLinearTime, single4);
			this._headBob.viewModelRotationScalar = this.headBobRotationScale.EvaluateClampedTime(ref this.headBobAngularTime, single4);
		}
	}

	public void ModifyAiming(Ray ray, ref Vector3 p, ref Quaternion q)
	{
		float single;
		RaycastHit2 raycastHit2;
		if (ViewModel.modifyAiming)
		{
			single = (!Physics2.Raycast2(ray, out raycastHit2, this.noHitPlane) ? this.noHitPlane : raycastHit2.distance);
			Vector3 vector3 = this.shelf.InverseTransformPoint((this.pivot.position + this.pivot2.position) / 2f);
			Vector3 vector31 = this.shelf.InverseTransformPoint(ray.GetPoint(single));
			float single1 = Vector3.Angle(vector3, this.shelf.InverseTransformPoint(ray.origin));
			float single2 = single * Mathf.Cos(single1 * 0.0174532924f);
			float single3 = Mathf.Atan2(single2, vector31.magnitude);
			q = q * new Quaternion(0f, Mathf.Sin(single3), 0f, Mathf.Cos(single3));
		}
	}

	public void ModifyPerspective(ref PerspectiveMatrixBuilder perspective)
	{
		if ((this.caps & 1) == 1)
		{
			perspective.nearPlane = (double)this.perspectiveNearOverride;
		}
		if ((this.caps & 2) == 2)
		{
			perspective.farPlane = (double)this.perspectiveFarOverride;
		}
		if ((this.caps & 4) == 4)
		{
			perspective.fieldOfView = (double)this.perspectiveFOVOverride;
		}
		if ((this.caps & 8) == 8)
		{
			perspective.aspectRatio = (double)this.perspectiveAspectOverride;
		}
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		this.UnBindTransforms();
		this.meshInstances.Dispose();
	}

	private void OnDrawGizmosSelected()
	{
		if (!this.root)
		{
			return;
		}
		this.pivot.DrawGizmos("pivot1");
		this.pivot2.DrawGizmos("pivot2");
		this.muzzle.DrawGizmos("muzzle");
		this.sight.DrawGizmos("sights");
		this.bowPivot.DrawGizmos("bow");
		Gizmos.matrix = this.root.localToWorldMatrix;
		Vector3 vector3 = Angle2.Direction(this.barrelRotation.x, this.barrelRotation.y);
		Gizmos.DrawSphere(this.barrelPivot, 0.001f);
		Gizmos.DrawLine(this.barrelPivot, this.barrelPivot + vector3);
		Gizmos.matrix = Gizmos.matrix * Matrix4x4.TRS(this.barrelPivot, Quaternion.Euler(-this.barrelRotation.x, this.barrelRotation.y, 0f), Vector3.one);
		Gizmos.DrawWireCube(Vector3.forward * (this.barrelLimit * 0.5f), new Vector3(0.02f, 0.02f, this.barrelLimit));
		float single = this.bpHip.a;
		float single1 = this.bpHip.b;
		float single2 = this.bpHip.c;
		float single3 = this.bpHip.ab;
		float single4 = this.bpHip.bc;
		float single5 = this.bpHip.ca;
		Quaternion quaternion = Quaternion.Euler(0f, 0f, single);
		Quaternion quaternion1 = Quaternion.Euler(0f, 0f, single + single1);
		Quaternion quaternion2 = Quaternion.Euler(0f, 0f, single + single1 + single2);
		Vector3 vector31 = quaternion * Vector3.up * single3;
		Vector3 vector32 = vector31 + (quaternion1 * Vector3.up * single4);
		Vector3 vector33 = vector32 + (quaternion2 * Vector3.up * single5);
		Bounds bound = new Bounds();
		bound.Encapsulate(vector33);
		bound.Encapsulate(vector31);
		bound.Encapsulate(vector32);
		Gizmos.matrix = Matrix4x4.TRS(-vector32, Quaternion.identity, Vector3.one);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(vector33, 0.01f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(vector33, vector31);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(vector31, 0.01f);
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(vector31, vector32);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(vector32, 0.01f);
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(vector32, vector33);
		Gizmos.color = Color.black;
		float single6 = single5;
		float single7 = single6 * Mathf.Sin(single * 0.0174532924f);
		float single8 = Mathf.Sqrt(single7 * single7 + single6 * single6);
		Gizmos.color = Color.black;
		Vector3 vector34 = quaternion1 * Vector3.up * single7;
		Gizmos.DrawLine(vector34, vector32);
		Gizmos.color = Color.gray;
		vector34 = quaternion * Vector3.up * single8;
		Gizmos.DrawLine(vector34, vector32);
	}

	private void OnGUI()
	{
		Camera camera;
		Color color = new Color();
		float single;
		RaycastHit2 raycastHit2;
		if (Event.current.type != EventType.Repaint || RPOS.IsOpen || !this.drawCrosshair || !this.crosshairTexture || !this.dotTexture)
		{
			return;
		}
		if (!this._headBob)
		{
			if (!this._lazyCam)
			{
				return;
			}
			camera = this._lazyCam.camera;
		}
		else
		{
			camera = this._headBob.camera;
		}
		if (camera && (camera.enabled || MountedCamera.IsCameraBeingUsed(camera)))
		{
			color.r = 1f;
			color.g = 1f;
			color.b = 1f;
			if (this.showCrosshairNotZoomed)
			{
				if (!this.showCrosshairZoom)
				{
					color.a = Mathf.Clamp01(1f - this.lastZoomFraction);
				}
				else
				{
					color.a = 1f;
				}
			}
			else if (!this.showCrosshairZoom)
			{
				color.a = 1f;
			}
			else
			{
				color.a = this.lastZoomFraction;
			}
			if (color.a == 0f)
			{
				return;
			}
			GUI.color = color;
			Ray ray = camera.ViewportPointToRay(Vector3.one * 0.5f);
			Plane plane = new Plane(-camera.transform.forward, camera.transform.position + (camera.transform.forward * this.noHitPlane));
			plane.Raycast(ray, out single);
			Vector3? nullable = CameraFX.World2Screen(ray.GetPoint(single));
			if (nullable.HasValue)
			{
				Vector3 value = nullable.Value;
				value.y = (float)Screen.height - (value.y + 1f);
				Rect rect = new Rect(value.x - (float)this.crosshairTexture.width / 2f, value.y - (float)this.crosshairTexture.height / 2f, (float)this.crosshairTexture.width, (float)this.crosshairTexture.height);
				this.DrawShadowed(ref rect, this.crosshairTexture);
			}
			if (Physics2.Raycast2(ray, out raycastHit2))
			{
				nullable = CameraFX.World2Screen(raycastHit2.point);
				if (nullable.HasValue)
				{
					Vector3 vector3 = nullable.Value;
					vector3.y = (float)Screen.height - (vector3.y + 1f);
					Rect rect1 = new Rect(vector3.x - (float)this.dotTexture.width / 2f, vector3.y - (float)this.dotTexture.height / 2f, (float)this.dotTexture.width, (float)this.dotTexture.height);
					this.DrawShadowed(ref rect1, this.dotTexture);
				}
			}
		}
	}

	public bool Play(string name)
	{
		return this.idleMixer.Play(name);
	}

	public bool Play(string name, PlayMode playMode)
	{
		return this.idleMixer.Play(name, playMode);
	}

	public bool Play(string name, float speed)
	{
		return this.idleMixer.Play(name, speed);
	}

	public bool Play(string name, float speed, float time)
	{
		return this.idleMixer.Play(name, speed, time);
	}

	public bool Play(string name, PlayMode playMode, float speed)
	{
		return this.idleMixer.Play(name, playMode, speed);
	}

	public bool Play(string name, PlayMode playMode, float speed, float time)
	{
		return this.idleMixer.Play(name, playMode, speed, time);
	}

	public void PlayDeployAnimation()
	{
		this.Play(this.deployAnimName);
	}

	public void PlayFireAnimation(float speed)
	{
		this.Play(this.fireAnimName, speed);
		this.punchTime = Time.time;
	}

	public void PlayFireAnimation()
	{
		this.PlayFireAnimation(this.fireAnimScaleSpeed);
	}

	public bool PlayQueued(string name)
	{
		return this.idleMixer.PlayQueued(name);
	}

	public bool PlayQueued(string name, QueueMode queueMode)
	{
		return this.idleMixer.PlayQueued(name, queueMode);
	}

	public bool PlayQueued(string name, QueueMode queueMode, PlayMode playMode)
	{
		return this.idleMixer.PlayQueued(name, queueMode, playMode);
	}

	public void PlayReloadAnimation()
	{
		this.Play(this.reloadAnimName);
	}

	public void RemoveRenderers(SkinnedMeshRenderer[] renderers)
	{
		if (renderers != null)
		{
			SkinnedMeshRenderer[] skinnedMeshRendererArray = renderers;
			for (int i = 0; i < (int)skinnedMeshRendererArray.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = skinnedMeshRendererArray[i];
				this.meshInstances.Delete(skinnedMeshRenderer);
			}
		}
	}

	[ContextMenu("Set as current view model")]
	private void SetAsCurrentViewModel()
	{
		if (base.enabled)
		{
			CameraFX.ReplaceViewModel(this, this.itemRep, this.item, false);
		}
	}

	Socket.CameraConversion Socket.Source.CameraSpaceSetup()
	{
		return new Socket.CameraConversion(this.eye, this.shelf);
	}

	bool Socket.Source.GetSocket(string name, out Socket socket)
	{
		int num;
		string str = name;
		if (str != null)
		{
			if (ViewModel.<>f__switch$mapD == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(6)
				{
					{ "muzzle", 0 },
					{ "sight", 1 },
					{ "optics", 2 },
					{ "pivot1", 3 },
					{ "pivot2", 4 },
					{ "bowPivot", 5 }
				};
				ViewModel.<>f__switch$mapD = strs;
			}
			if (ViewModel.<>f__switch$mapD.TryGetValue(str, out num))
			{
				switch (num)
				{
					case 0:
					{
						socket = this.muzzle;
						return true;
					}
					case 1:
					{
						socket = this.sight;
						return true;
					}
					case 2:
					{
						socket = this.optics;
						return true;
					}
					case 3:
					{
						socket = this.pivot;
						return true;
					}
					case 4:
					{
						socket = this.pivot2;
						return true;
					}
					case 5:
					{
						socket = this.bowPivot;
						return true;
					}
				}
			}
		}
		socket = null;
		return false;
	}

	Type Socket.Source.ProxyScriptType(string name)
	{
		return typeof(SocketProxy);
	}

	bool Socket.Source.ReplaceSocket(string name, Socket socket)
	{
		int num;
		Socket.CameraSpace cameraSpace = (Socket.CameraSpace)socket;
		string str = name;
		if (str != null)
		{
			if (ViewModel.<>f__switch$mapC == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(6)
				{
					{ "muzzle", 0 },
					{ "sight", 1 },
					{ "optics", 2 },
					{ "pivot1", 3 },
					{ "pivot2", 4 },
					{ "bowPivot", 5 }
				};
				ViewModel.<>f__switch$mapC = strs;
			}
			if (ViewModel.<>f__switch$mapC.TryGetValue(str, out num))
			{
				switch (num)
				{
					case 0:
					{
						this.muzzle = cameraSpace;
						return true;
					}
					case 1:
					{
						this.sight = cameraSpace;
						return true;
					}
					case 2:
					{
						this.optics = cameraSpace;
						return true;
					}
					case 3:
					{
						this.pivot = cameraSpace;
						return true;
					}
					case 4:
					{
						this.pivot2 = cameraSpace;
						return true;
					}
					case 5:
					{
						this.bowPivot = cameraSpace;
						return true;
					}
				}
			}
		}
		return false;
	}

	private static void SolveTriangleSAS(float angleA, float lengthB, float lengthC, out float lengthA, out float angleB, out float angleC)
	{
		lengthA = Mathf.Sqrt(lengthB * lengthB + lengthC * lengthC - 2f * lengthB * lengthC * Mathf.Cos(angleA * 0.0174532924f));
		if (angleA >= 90f || lengthB < lengthC)
		{
			angleB = Mathf.Asin(Mathf.Sin(angleA * 0.0174532924f) * lengthB / lengthA) * 57.29578f;
			angleC = 180f - (angleA + angleB);
		}
		else
		{
			angleC = Mathf.Asin(Mathf.Sin(angleA * 0.0174532924f) * lengthC / lengthA) * 57.29578f;
			angleB = 180f - (angleA + angleC);
		}
	}

	private static void SolveTriangleSSA(float angleB, float lengthB, float lengthC, out float lengthA, out float angleA, out float angleC)
	{
		float single = Mathf.Sin(angleB * 0.0174532924f);
		angleC = Mathf.Asin(single * lengthC / lengthB) * 57.29578f;
		angleA = 180f - angleC - angleB;
		if (angleA < 0f || angleA > 180f)
		{
			angleA = angleA + 180f;
		}
		lengthA = Mathf.Sin(angleA * 0.0174532924f) * lengthB / single;
	}

	public void UnBindTransforms()
	{
		this.ClearProxies();
		if (CameraFX.mainViewModel == this)
		{
			CameraFX cameraFX = CameraFX.mainCameraFX;
			if (cameraFX)
			{
				cameraFX.SetFieldOfView(320432f, 0f);
			}
		}
	}

	protected void Update()
	{
		this.idleMixer.Update(1f, Time.deltaTime);
	}

	public void UpdateProxies()
	{
		Socket.Map map = this.socketMap;
		if (!object.ReferenceEquals(map, null))
		{
			map.SnapProxies();
		}
	}

	private struct BarrelParameters
	{
		public float a;

		public float b;

		public float c;

		public float bc;

		public float ca;

		public float ab;

		public bool once;

		public bool ir;

		public float angle;

		public float angularVelocity;
	}

	private struct BarrelTransform
	{
		public Vector3 origin;

		public Vector3 angles;

		public void Get(out Vector3 origin, out Vector3 angles)
		{
			origin = this.origin;
			angles = this.angles;
		}
	}

	private class MeshInstance
	{
		private const int kMaxDumpCount = 8;

		public ViewModel.MeshInstance next;

		public SkinnedMeshRenderer renderer;

		public bool legacy;

		public ViewModel.MeshInstance.ReplacementRenderer predraw;

		public ViewModel.MeshInstance.ReplacementRenderer postdraw;

		public bool disposed;

		public bool hasNext;

		private Material[] originalMaterials;

		private Material[] modifiedMaterials;

		private static ViewModel.MeshInstance dump;

		private static int dumpCount;

		private MeshInstance()
		{
		}

		private void Delete()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.predraw.Shutdown();
				this.postdraw.Shutdown();
				if (this.renderer)
				{
					this.renderer.sharedMaterials = this.originalMaterials;
				}
				this.renderer = null;
				if (ViewModel.MeshInstance.dumpCount >= 8)
				{
					this.next = null;
					this.hasNext = false;
				}
				else
				{
					this.next = ViewModel.MeshInstance.dump;
					ViewModel.MeshInstance.dump = this;
					int num = ViewModel.MeshInstance.dumpCount;
					ViewModel.MeshInstance.dumpCount = num + 1;
					this.hasNext = num > 0;
				}
			}
		}

		private static bool New(ViewModel.MeshInstance ptr, SkinnedMeshRenderer renderer, out ViewModel.MeshInstance newInstance)
		{
			if (!renderer)
			{
				newInstance = null;
				return false;
			}
			if (ViewModel.MeshInstance.dumpCount <= 0)
			{
				newInstance = new ViewModel.MeshInstance();
			}
			else
			{
				newInstance = ViewModel.MeshInstance.dump;
				int num = ViewModel.MeshInstance.dumpCount - 1;
				ViewModel.MeshInstance.dumpCount = num;
				if (num <= 0)
				{
					ViewModel.MeshInstance.dump = null;
				}
				else
				{
					ViewModel.MeshInstance.dump = newInstance.next;
				}
				newInstance.next = null;
				newInstance.hasNext = false;
				newInstance.disposed = false;
				newInstance.renderer = null;
			}
			if (ptr == null)
			{
				newInstance.hasNext = false;
				newInstance.next = null;
			}
			else
			{
				newInstance.hasNext = ptr.hasNext;
				newInstance.next = ptr.next;
				ptr.hasNext = true;
				ptr.next = newInstance;
			}
			newInstance.renderer = renderer;
			newInstance.originalMaterials = renderer.sharedMaterials;
			int num1 = renderer.sharedMesh.subMeshCount;
			if ((int)newInstance.originalMaterials.Length % num1 != 0)
			{
				Array.Resize<Material>(ref newInstance.originalMaterials, ((int)newInstance.originalMaterials.Length / num1 + 1) * num1);
			}
			newInstance.modifiedMaterials = newInstance.originalMaterials;
			return true;
		}

		public void SetPostdrawMaterial(Material mat)
		{
			this.SetReplacementRenderMaterial(ref this.postdraw, 2, mat);
		}

		public void SetPredrawMaterial(Material mat)
		{
			this.SetReplacementRenderMaterial(ref this.predraw, 1, mat);
		}

		private void SetReplacementRenderMaterial(ref ViewModel.MeshInstance.ReplacementRenderer rr, int itsa, Material mat)
		{
			if (!this.disposed)
			{
				if (rr.initialized)
				{
					rr.SetOverride(this.originalMaterials, mat, itsa);
				}
				else
				{
					this.legacy = (ViewModel.force_legacy_fallback ? true : this.renderer.sharedMesh.subMeshCount > 1);
					rr.Initialize(this.renderer, this.renderer, this.originalMaterials, mat, itsa, this.legacy);
				}
				Material[] materialArray = rr.UpdateMaterials(this.legacy);
				if (!this.legacy)
				{
					if (materialArray != null)
					{
						if (rr.offset == 0)
						{
							rr.offset = (int)this.modifiedMaterials.Length;
							Array.Resize<Material>(ref this.modifiedMaterials, (int)this.modifiedMaterials.Length + (int)this.originalMaterials.Length);
						}
						int num = rr.offset;
						for (int i = 0; i < (int)this.originalMaterials.Length; i++)
						{
							this.modifiedMaterials[num] = materialArray[i];
							num++;
						}
					}
					else if (rr.offset != 0)
					{
						int num1 = rr.offset;
						for (int j = rr.offset + (int)this.originalMaterials.Length; j < (int)this.modifiedMaterials.Length; j++)
						{
							this.modifiedMaterials[num1] = this.modifiedMaterials[j];
							num1++;
						}
						Array.Resize<Material>(ref this.modifiedMaterials, (int)this.modifiedMaterials.Length - (int)this.originalMaterials.Length);
						rr.offset = 0;
					}
					this.renderer.sharedMaterials = this.modifiedMaterials;
				}
			}
		}

		public struct Holder : IDisposable
		{
			public ViewModel.MeshInstance first;

			public int count;

			public bool disposed;

			public bool Add(SkinnedMeshRenderer renderer)
			{
				ViewModel.MeshInstance meshInstance;
				return (!renderer ? false : this.Add(renderer, out meshInstance));
			}

			public bool Add(SkinnedMeshRenderer renderer, out ViewModel.MeshInstance newOrExistingInstance)
			{
				if (this.disposed)
				{
					newOrExistingInstance = null;
					return false;
				}
				if (this.count == 0)
				{
					return this.AddShared(ViewModel.MeshInstance.New(null, renderer, out newOrExistingInstance), newOrExistingInstance);
				}
				if (object.ReferenceEquals(this.first.renderer, renderer))
				{
					newOrExistingInstance = this.first;
					return false;
				}
				int num = this.count - 1;
				ViewModel.MeshInstance meshInstance = this.first;
				for (int i = 0; i < num; i++)
				{
					if (object.ReferenceEquals(meshInstance.next.renderer, renderer))
					{
						newOrExistingInstance = meshInstance.next;
						return false;
					}
					meshInstance = meshInstance.next;
				}
				return this.AddShared(ViewModel.MeshInstance.New(meshInstance, renderer, out newOrExistingInstance), newOrExistingInstance);
			}

			private bool AddShared(bool didIt, ViewModel.MeshInstance meshInstance)
			{
				if (didIt)
				{
					ViewModel.MeshInstance.Holder holder = this;
					int num = holder.count;
					int num1 = num;
					holder.count = num + 1;
					if (num1 == 0)
					{
						this.first = meshInstance;
					}
				}
				CameraFX cameraFX = CameraFX.mainCameraFX;
				if (cameraFX)
				{
					Material material = cameraFX.predrawMaterial;
					Material material1 = material;
					if (material)
					{
						meshInstance.SetPredrawMaterial(material1);
					}
					Material material2 = cameraFX.postdrawMaterial;
					material1 = material2;
					if (material2)
					{
						meshInstance.SetPostdrawMaterial(material1);
					}
				}
				return didIt;
			}

			public void Clear()
			{
				while (this.count > 0)
				{
					this.FirstDelete();
				}
			}

			public bool Delete(ViewModel.MeshInstance instance)
			{
				if (this.disposed)
				{
					return false;
				}
				if (this.count > 0 && instance != null && !instance.disposed)
				{
					if (instance == this.first)
					{
						this.FirstDelete();
						return true;
					}
					int num = this.count - 1;
					ViewModel.MeshInstance meshInstance = this.first;
					for (int i = 0; i < num; i++)
					{
						if (meshInstance.next == instance)
						{
							this.IterDelete(meshInstance);
							return true;
						}
					}
				}
				return false;
			}

			public bool Delete(SkinnedMeshRenderer renderer)
			{
				if (this.disposed)
				{
					return false;
				}
				if (this.count > 0)
				{
					if (object.ReferenceEquals(renderer, this.first.renderer))
					{
						this.FirstDelete();
						return true;
					}
					int num = this.count - 1;
					ViewModel.MeshInstance meshInstance = this.first;
					for (int i = 0; i < num; i++)
					{
						if (object.ReferenceEquals(meshInstance.next.renderer, renderer))
						{
							this.IterDelete(meshInstance);
							return true;
						}
						meshInstance = meshInstance.next;
					}
				}
				return false;
			}

			public void Dispose()
			{
				if (!this.disposed)
				{
					try
					{
						this.Clear();
					}
					finally
					{
						this.disposed = true;
					}
				}
			}

			private void FirstDelete()
			{
				ViewModel.MeshInstance meshInstance = this.first;
				this.first = this.first.next;
				this.InstanceDeleteShared(meshInstance);
			}

			private void InstanceDeleteShared(ViewModel.MeshInstance instance)
			{
				ViewModel.MeshInstance.Holder holder = this;
				holder.count = holder.count - 1;
				instance.Delete();
			}

			private void IterDelete(ViewModel.MeshInstance iter)
			{
				ViewModel.MeshInstance meshInstance = iter.next;
				iter.hasNext = meshInstance.hasNext;
				iter.next = meshInstance.next;
				this.InstanceDeleteShared(meshInstance);
			}
		}

		public struct ReplacementRenderer
		{
			public const int kItsaPreDraw = 1;

			public const int kItsaPostDraw = 2;

			public Material[] materials;

			public SkinnedMeshRenderer renderer;

			public bool initialized;

			public int offset;

			public void Initialize(SkinnedMeshRenderer owner, SkinnedMeshRenderer source, Material[] originalMaterials, Material overrideMaterial, int itsa, bool legacy)
			{
				this.Shutdown();
				if (!legacy)
				{
					this.materials = (Material[])originalMaterials.Clone();
					this.initialized = true;
					if (!this.SetOverride(originalMaterials, overrideMaterial, itsa))
					{
						this.materials = null;
					}
				}
				else
				{
					Transform transforms = owner.transform;
					this.renderer = (SkinnedMeshRenderer)UnityEngine.Object.Instantiate(source);
					Transform transforms1 = this.renderer.transform;
					transforms1.parent = transforms.parent;
					transforms1.localPosition = transforms.localPosition;
					transforms1.localRotation = transforms.localRotation;
					transforms1.localScale = transforms.localScale;
					this.materials = (Material[])originalMaterials.Clone();
					this.initialized = true;
					this.SetOverride(originalMaterials, overrideMaterial, itsa);
					this.UpdateMaterials(true);
				}
			}

			public bool SetOverride(Material[] originals, Material material, int itsa)
			{
				bool flag = false;
				if (this.initialized)
				{
					int num = itsa;
					if (num == 1)
					{
						for (int i = 0; i < (int)originals.Length; i++)
						{
							if (originals[i])
							{
								if (originals[i].GetTag("SkipViewModelPredraw", false, "False") != "True")
								{
									this.materials[i] = material;
									flag = true;
								}
								else
								{
									this.materials[i] = null;
								}
							}
						}
					}
					else if (num == 2)
					{
						for (int j = 0; j < (int)originals.Length; j++)
						{
							if (originals[j])
							{
								if (originals[j].GetTag("SkipViewModelPostdraw", false, "False") != "True")
								{
									this.materials[j] = material;
									flag = true;
								}
								else
								{
									this.materials[j] = null;
								}
							}
						}
					}
					else
					{
						for (int k = 0; k < (int)originals.Length; k++)
						{
							if (originals[k])
							{
								this.materials[k] = material;
							}
							flag = true;
						}
					}
				}
				return flag;
			}

			public void Shutdown()
			{
				if (this.initialized)
				{
					if (this.renderer)
					{
						UnityEngine.Object.Destroy(this.renderer.gameObject);
					}
					this = new ViewModel.MeshInstance.ReplacementRenderer();
				}
			}

			public Material[] UpdateMaterials(bool legacy)
			{
				if (!this.initialized)
				{
					return null;
				}
				if (legacy && this.renderer)
				{
					this.renderer.sharedMaterials = this.materials;
				}
				return this.materials;
			}
		}
	}
}