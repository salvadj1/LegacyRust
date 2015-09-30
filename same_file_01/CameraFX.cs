using Facepunch.Precision;
using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFX : IDRemote
{
	[NonSerialized]
	public Camera camera;

	[SerializeField]
	private MonoBehaviour[] _effects;

	[SerializeField]
	private Material viewModelPredrawMaterial;

	private Material viewModelPostdrawMaterial;

	private AdaptiveNearPlane adaptiveNearPlane;

	private float fieldOfViewAdjustment;

	private float fieldOfViewFraction;

	[SerializeField]
	private bool recalcViewMatrix = true;

	private ICameraFX[] effects;

	private Quaternion preRotation;

	private Vector3 preLocalPosition;

	private static Transform viewModelRootTransform;

	private MatrixHelper.ProjectHelperG projectViewport;

	private MatrixHelper.ProjectHelperG projectScreen;

	private Matrix4x4G localToWorldMatrix;

	private Matrix4x4G worldToLocalMatrix;

	private Matrix4x4G cameraToWorldMatrixUnAltered;

	private Matrix4x4G worldToCameraMatrixUnAltered;

	private Matrix4x4G projectionMatrixUnAltered;

	private Matrix4x4G cameraToWorldMatrix;

	private Matrix4x4G worldToCameraMatrix;

	private Matrix4x4G projectionMatrix;

	private Matrix4x4 preProjectionMatrix;

	private bool awoke;

	private static CameraFX _mainCameraFX;

	private static Camera _mainCamera;

	private static MountedCamera _mainMountedCamera;

	private static bool _hasMainCameraFX;

	private static bool _hasMainCamera;

	private static bool _mainIsMount;

	private static bool vm_projuse;

	private static bool vm_flip;

	private static CameraFX.CameraTransitionData g_trans;

	private ViewModel viewModel;

	private ItemRepresentation rep;

	private IHeldItem item;

	public new Character idMain
	{
		get
		{
			return (Character)base.idMain;
		}
	}

	public static CameraFX mainCameraFX
	{
		get
		{
			CameraFX cameraFX;
			Camera camera = Camera.main;
			if (CameraFX._mainCamera != camera)
			{
				CameraFX._mainCamera = camera;
				if (!camera)
				{
					CameraFX._hasMainCamera = false;
					CameraFX._mainIsMount = false;
					CameraFX._hasMainCameraFX = false;
					CameraFX._mainCameraFX = null;
				}
				else
				{
					CameraFX._hasMainCamera = true;
					CameraFX._mainIsMount = MountedCamera.IsMountedCamera(camera);
					if (!CameraFX._mainIsMount)
					{
						CameraFX._mainMountedCamera = null;
						CameraFX component = camera.GetComponent<CameraFX>();
						CameraFX._mainCameraFX = component;
						CameraFX._hasMainCameraFX = component;
					}
					else
					{
						CameraFX._mainMountedCamera = MountedCamera.main;
						CameraFX cameraFX1 = CameraFX._mainMountedCamera.cameraFX;
						CameraFX._mainCameraFX = cameraFX1;
						CameraFX._hasMainCameraFX = cameraFX1;
					}
				}
			}
			else if (CameraFX._hasMainCamera && !camera)
			{
				CameraFX._hasMainCamera = false;
				CameraFX._mainIsMount = false;
				CameraFX._hasMainCameraFX = false;
				CameraFX._mainCameraFX = null;
			}
			else if (CameraFX._mainIsMount && CameraFX._mainCameraFX != CameraFX._mainMountedCamera.cameraFX)
			{
				CameraFX._mainCameraFX = CameraFX._mainMountedCamera.cameraFX;
				CameraFX._hasMainCameraFX = CameraFX._mainCameraFX;
			}
			if (!CameraFX._hasMainCamera)
			{
				cameraFX = null;
			}
			else
			{
				cameraFX = (!CameraFX._mainIsMount ? CameraFX._mainCameraFX : MountedCamera.main.cameraFX);
			}
			return cameraFX;
		}
	}

	public static ViewModel mainViewModel
	{
		get
		{
			ViewModel viewModel;
			CameraFX cameraFX = CameraFX.mainCameraFX;
			if (!cameraFX)
			{
				viewModel = null;
			}
			else
			{
				viewModel = cameraFX.viewModel;
			}
			return viewModel;
		}
	}

	public Material postdrawMaterial
	{
		get
		{
			return this.viewModelPostdrawMaterial;
		}
	}

	public Material predrawMaterial
	{
		get
		{
			return this.viewModelPredrawMaterial;
		}
	}

	static CameraFX()
	{
		CameraFX.vm_projuse = false;
		CameraFX.vm_flip = false;
		CameraFX.g_trans = CameraFX.CameraTransitionData.identity;
	}

	public CameraFX()
	{
	}

	private void ApplyFieldOfView()
	{
		float single = Mathf.Lerp((float)render.fov, this.fieldOfViewAdjustment, this.fieldOfViewFraction);
		if (this.camera.fieldOfView != single)
		{
			this.camera.fieldOfView = single;
		}
	}

	internal static void ApplyTransitionAlterations(Camera camera, CameraFX fx, bool useFX)
	{
		Matrix4x4G matrix4x4G;
		Matrix4x4G matrix4x4G1;
		if (!useFX)
		{
			camera.ExtractCameraMatrixWorldToCamera(out matrix4x4G);
			camera.ExtractCameraMatrixProjection(out matrix4x4G1);
			int num = CameraFX.g_trans.Update(ref matrix4x4G, ref matrix4x4G1);
			if ((num & 1) == 1)
			{
				camera.ResetWorldToCameraMatrix();
				camera.worldToCameraMatrix = matrix4x4G.f;
			}
			if ((num & 2) == 2)
			{
				camera.ResetProjectionMatrix();
				camera.projectionMatrix = matrix4x4G1.f;
			}
		}
		else
		{
			int num1 = CameraFX.g_trans.Update(ref fx.worldToCameraMatrix, ref fx.projectionMatrix);
			if ((num1 & 1) == 1)
			{
				camera.worldToCameraMatrix = fx.worldToCameraMatrix.f;
				Matrix4x4G.Inverse(ref fx.worldToCameraMatrix, out fx.cameraToWorldMatrix);
			}
			if ((num1 & 2) == 2)
			{
				camera.projectionMatrix = fx.projectionMatrix.f;
			}
		}
	}

	protected new void Awake()
	{
		this.camera = base.camera;
		this.adaptiveNearPlane = base.GetComponent<AdaptiveNearPlane>();
		int num = 0;
		if (this._effects != null && (int)this._effects.Length != 0)
		{
			for (int i = 0; i < (int)this._effects.Length; i++)
			{
				if (!this._effects[i] || !(this._effects[i] is ICameraFX))
				{
					Debug.LogWarning(string.Concat("effect at index ", i, " is missing, null or not a ICameraFX"), this);
				}
				else
				{
					int num1 = num;
					num = num1 + 1;
					this._effects[num1] = this._effects[i];
				}
			}
		}
		Array.Resize<MonoBehaviour>(ref this._effects, num);
		Array.Resize<ICameraFX>(ref this.effects, num);
		if (num != 0)
		{
			for (int j = 0; j < num; j++)
			{
				this.effects[j] = (ICameraFX)this._effects[j];
			}
		}
		else
		{
			Debug.LogWarning("There are no effects", this);
		}
		this.awoke = true;
		if (this.viewModel)
		{
			ViewModel viewModel = this.viewModel;
			this.viewModel = null;
			ItemRepresentation itemRepresentation = this.rep;
			this.rep = null;
			IHeldItem heldItem = this.item;
			this.item = null;
			this.SetViewModel(viewModel, itemRepresentation, heldItem);
		}
		base.Awake();
	}

	private static bool Bind()
	{
		return CameraFX.mainCameraFX;
	}

	public Vector3 InverseTransformDirection(Vector3 v)
	{
		return this.worldToLocalMatrix.f.MultiplyVector(v);
	}

	public Vector3 InverseTransformPoint(Vector3 v)
	{
		return this.worldToLocalMatrix.f.MultiplyPoint3x4(v);
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		if (CameraFX._mainCameraFX == this)
		{
			CameraFX._mainCamera = null;
			CameraFX._mainCameraFX = null;
			CameraFX._hasMainCameraFX = false;
		}
	}

	public void PostPostRender()
	{
		base.transform.localPosition = this.preLocalPosition;
		base.transform.rotation = this.preRotation;
		this.camera.projectionMatrix = this.preProjectionMatrix;
	}

	public void PostPreCull()
	{
		Vector3G vector3G;
		Matrix4x4G matrix4x4G;
		Matrix4x4G matrix4x4G1;
		Vector3G vector3G1 = new Vector3G();
		Vector3G vector3G2;
		Vector3G vector3G3;
		Vector3G vector3G4;
		Vector3G vector3G5;
		Vector3G vector3G6 = new Vector3G();
		RaycastHit raycastHit;
		Matrix4x4G matrix4x4G2;
		PerspectiveMatrixBuilder perspectiveMatrixBuilder = new PerspectiveMatrixBuilder();
		Vector4 vector4 = new Vector4();
		PerspectiveMatrixBuilder perspectiveMatrixBuilder1 = new PerspectiveMatrixBuilder();
		Matrix4x4G matrix4x4G3;
		Vector3G vector3G7;
		QuaternionG quaternionG;
		Vector3G vector3G8;
		Matrix4x4G matrix4x4G4;
		Matrix4x4G matrix4x4G5;
		Matrix4x4G matrix4x4G6;
		Matrix4x4G matrix4x4G7;
		Matrix4x4G matrix4x4G8;
		Matrix4x4G matrix4x4G9;
		Matrix4x4G matrix4x4G10;
		Matrix4x4G matrix4x4G11;
		if (CameraFX.viewModelRootTransform)
		{
			Quaternion quaternion = base.transform.localRotation;
			Vector3 vector3 = base.transform.localPosition;
			if (this.viewModel)
			{
				this.viewModel.ModifyAiming(new Ray(base.transform.parent.position, base.transform.parent.forward), ref vector3, ref quaternion);
			}
			CameraFX.viewModelRootTransform.localRotation = Quaternion.Inverse(quaternion);
			CameraFX.viewModelRootTransform.localPosition = -vector3;
		}
		this.camera.transform.ExtractLocalToWorldToLocal(out this.localToWorldMatrix, out this.worldToLocalMatrix);
		if (this.adaptiveNearPlane)
		{
			int num = this.camera.cullingMask & ~this.adaptiveNearPlane.ignoreLayers.@value | this.adaptiveNearPlane.forceLayers.@value;
			Vector3G vector3G9 = new Vector3G();
			this.localToWorldMatrix.MultiplyPoint(ref vector3G9, out vector3G);
			Collider[] colliderArray = Physics.OverlapSphere(vector3G.f, this.adaptiveNearPlane.minNear + this.adaptiveNearPlane.maxNear, num);
			int num1 = -1;
			float single = Single.PositiveInfinity;
			double num2 = (double)this.camera.fieldOfView;
			double num3 = (double)this.camera.aspect;
			double num4 = (double)this.adaptiveNearPlane.minNear;
			double num5 = (double)(this.adaptiveNearPlane.maxNear + this.adaptiveNearPlane.threshold);
			float single1 = this.adaptiveNearPlane.minNear;
			float single2 = this.adaptiveNearPlane.maxNear + this.adaptiveNearPlane.threshold - single1;
			Matrix4x4G.Perspective(ref num2, ref num3, ref num4, ref num5, out matrix4x4G);
			Matrix4x4G.Inverse(ref matrix4x4G, out matrix4x4G1);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					vector3G1.x = ((double)i - 3.5) / 3.5;
					vector3G1.y = ((double)j - 3.5) / 3.5;
					vector3G1.z = 0;
					matrix4x4G1.MultiplyPoint(ref vector3G1, out vector3G2);
					vector3G1.z = 1;
					matrix4x4G1.MultiplyPoint(ref vector3G1, out vector3G3);
					vector3G2.x = -vector3G2.x;
					vector3G2.y = -vector3G2.y;
					vector3G2.z = -vector3G2.z;
					vector3G3.x = -vector3G3.x;
					vector3G3.y = -vector3G3.y;
					vector3G3.z = -vector3G3.z;
					this.localToWorldMatrix.MultiplyPoint(ref vector3G2, out vector3G4);
					this.localToWorldMatrix.MultiplyPoint(ref vector3G3, out vector3G5);
					vector3G6.x = vector3G5.x - vector3G4.x;
					vector3G6.y = vector3G5.y - vector3G4.y;
					vector3G6.z = vector3G5.z - vector3G4.z;
					float single3 = (float)Math.Sqrt(vector3G6.x * vector3G6.x + vector3G6.y * vector3G6.y + vector3G6.z * vector3G6.z);
					float single4 = single3;
					Ray ray = new Ray(vector3G4.f, vector3G6.f);
					for (int k = 0; k < (int)colliderArray.Length; k++)
					{
						if (colliderArray[k].Raycast(ray, out raycastHit, single4))
						{
							float single5 = raycastHit.distance;
							if (single5 < single4)
							{
								single4 = single5;
								float single6 = single1 + single5 / single3 * single2;
								if (single > single6)
								{
									single = single6;
									num1 = k;
								}
							}
						}
					}
				}
			}
			if (!float.IsInfinity(single))
			{
				single = single - this.adaptiveNearPlane.threshold;
				if (single >= this.adaptiveNearPlane.maxNear)
				{
					this.camera.nearClipPlane = this.adaptiveNearPlane.maxNear;
				}
				else if (single > this.adaptiveNearPlane.minNear)
				{
					this.camera.nearClipPlane = single;
				}
				else
				{
					this.camera.nearClipPlane = this.adaptiveNearPlane.minNear;
				}
			}
			else
			{
				this.camera.nearClipPlane = this.adaptiveNearPlane.maxNear;
			}
		}
		perspectiveMatrixBuilder.fieldOfView = (double)this.camera.fieldOfView;
		perspectiveMatrixBuilder.aspectRatio = (double)this.camera.aspect;
		perspectiveMatrixBuilder.nearPlane = (double)this.camera.nearClipPlane;
		perspectiveMatrixBuilder.farPlane = (double)this.camera.farClipPlane;
		PerspectiveMatrixBuilder perspectiveMatrixBuilder2 = perspectiveMatrixBuilder;
		if (!this.camera.isOrthoGraphic)
		{
			if (this.viewModel)
			{
				this.viewModel.ModifyPerspective(ref perspectiveMatrixBuilder2);
			}
			if (!CameraFX.vm_projuse)
			{
				perspectiveMatrixBuilder.ToProjectionMatrix(out this.projectionMatrix);
			}
			else
			{
				perspectiveMatrixBuilder2.ToProjectionMatrix(out this.projectionMatrix);
			}
			this.camera.projectionMatrix = this.projectionMatrix.f;
			perspectiveMatrixBuilder2.ToProjectionMatrix(out matrix4x4G2);
		}
		else
		{
			this.projectionMatrix.f = this.camera.projectionMatrix;
			matrix4x4G2 = this.projectionMatrix;
		}
		vector4.y = (float)perspectiveMatrixBuilder2.nearPlane;
		vector4.z = (float)perspectiveMatrixBuilder2.farPlane;
		vector4.w = (float)(1 / perspectiveMatrixBuilder2.farPlane);
		if (CameraFX.vm_flip != CameraFX.PLATFORM_POLL.flipRequired)
		{
			vector4.x = -1f;
			perspectiveMatrixBuilder1.nearPlane = perspectiveMatrixBuilder2.nearPlane;
			perspectiveMatrixBuilder1.farPlane = perspectiveMatrixBuilder2.farPlane;
			perspectiveMatrixBuilder1.fieldOfView = -perspectiveMatrixBuilder2.fieldOfView;
			perspectiveMatrixBuilder1.aspectRatio = -perspectiveMatrixBuilder2.aspectRatio;
			perspectiveMatrixBuilder1.ToProjectionMatrix(out matrix4x4G3);
			Shader.SetGlobalMatrix("V_MUNITY_MATRIX_P", matrix4x4G3.f);
		}
		else
		{
			vector4.x = 1f;
			Shader.SetGlobalMatrix("V_MUNITY_MATRIX_P", matrix4x4G2.f);
		}
		Shader.SetGlobalVector("V_M_ProjectionParams", vector4);
		if (!this.recalcViewMatrix)
		{
			this.cameraToWorldMatrix.f = this.camera.cameraToWorldMatrix;
			this.worldToCameraMatrix.f = this.camera.worldToCameraMatrix;
		}
		else
		{
			this.camera.transform.ExtractWorldCoordinates(out vector3G7, out quaternionG, out vector3G8);
			vector3G8.x = 1;
			vector3G8.y = 1;
			vector3G8.z = -1;
			Matrix4x4G.TRS(ref vector3G7, ref quaternionG, ref vector3G8, out this.cameraToWorldMatrix);
			if (Matrix4x4G.Inverse(ref this.cameraToWorldMatrix, out this.worldToCameraMatrix))
			{
				this.camera.worldToCameraMatrix = this.worldToCameraMatrix.f;
			}
		}
		this.worldToCameraMatrixUnAltered = this.worldToCameraMatrix;
		this.cameraToWorldMatrixUnAltered = this.cameraToWorldMatrix;
		this.projectionMatrixUnAltered = this.projectionMatrix;
		CameraFX.ApplyTransitionAlterations(this.camera, this, true);
		Matrix4x4G matrix4x4G12 = this.worldToCameraMatrix;
		Matrix4x4G matrix4x4G13 = matrix4x4G12;
		this.projectViewport.modelview = matrix4x4G12;
		this.projectScreen.modelview = matrix4x4G13;
		Matrix4x4G matrix4x4G14 = this.projectionMatrix;
		matrix4x4G13 = matrix4x4G14;
		this.projectViewport.projection = matrix4x4G14;
		this.projectScreen.projection = matrix4x4G13;
		Rect rect = this.camera.pixelRect;
		this.projectScreen.offset.x = (double)rect.x;
		this.projectScreen.offset.y = (double)rect.y;
		this.projectScreen.size.x = (double)rect.width;
		this.projectScreen.size.y = (double)rect.height;
		rect = this.camera.rect;
		this.projectViewport.offset.x = (double)rect.x;
		this.projectViewport.offset.y = (double)rect.y;
		this.projectViewport.size.x = (double)rect.width;
		this.projectViewport.size.y = (double)rect.height;
		Matrix4x4G.Mult(ref this.localToWorldMatrix, ref this.worldToCameraMatrix, out matrix4x4G7);
		Matrix4x4G.Mult(ref matrix4x4G7, ref this.projectionMatrix, out matrix4x4G4);
		Matrix4x4G.Inverse(ref matrix4x4G7, out matrix4x4G8);
		Matrix4x4G.Inverse(ref matrix4x4G4, out matrix4x4G5);
		Matrix4x4G.Inverse(ref this.localToWorldMatrix, out matrix4x4G10);
		Matrix4x4G.Transpose(ref matrix4x4G5, out matrix4x4G6);
		Matrix4x4G.Transpose(ref matrix4x4G8, out matrix4x4G9);
		Matrix4x4G.Transpose(ref matrix4x4G10, out matrix4x4G11);
		if (this.viewModel)
		{
			this.viewModel.UpdateProxies();
		}
		BoundHack.Achieve(base.transform.position);
		ContextSprite.UpdateSpriteFading(this.camera);
		PlayerClient playerClient = PlayerClient.localPlayerClient;
		if (playerClient)
		{
			playerClient.ProcessLocalPlayerPreRender();
		}
		RPOS.BeforeSceneRender_Internal(this.camera);
	}

	public void PrePostRender()
	{
		this.camera.ResetWorldToCameraMatrix();
		for (int i = (int)this._effects.Length - 1; i >= 0; i--)
		{
			if (this._effects[i] && this._effects[i].enabled)
			{
				this.effects[i].PostRender();
			}
		}
	}

	public void PrePreCull()
	{
		this.ApplyFieldOfView();
		this.camera.ResetProjectionMatrix();
		this.preProjectionMatrix = this.camera.projectionMatrix;
		this.preLocalPosition = base.transform.localPosition;
		this.preRotation = base.transform.rotation;
		for (int i = 0; i < (int)this._effects.Length; i++)
		{
			if (this._effects[i] && this._effects[i].enabled)
			{
				this.effects[i].PreCull();
			}
		}
	}

	public static void RemoveViewModel()
	{
		if (CameraFX.mainViewModel)
		{
			CameraFX.ReplaceViewModel(null, false);
		}
	}

	public static void RemoveViewModel(ref ViewModel vm, bool deleteEvenIfNotCurrent, bool removeCurrentIfNotVM)
	{
		if (!vm)
		{
			if (removeCurrentIfNotVM)
			{
				CameraFX.RemoveViewModel();
			}
			return;
		}
		if (CameraFX.mainViewModel != vm)
		{
			if (deleteEvenIfNotCurrent)
			{
				UnityEngine.Object.Destroy(vm.gameObject);
				vm = null;
			}
			if (removeCurrentIfNotVM)
			{
				CameraFX.ReplaceViewModel(null, false);
			}
		}
		else
		{
			CameraFX.ReplaceViewModel(null, false);
			vm = null;
		}
	}

	public static void ReplaceViewModel(ViewModel vm, bool butDontDestroyOld)
	{
		CameraFX.ReplaceViewModel(vm, null, null, butDontDestroyOld);
	}

	public static void ReplaceViewModel(ViewModel vm, ItemRepresentation rep, IHeldItem item, bool butDontDestroyOld)
	{
		CameraFX cameraFX = CameraFX.mainCameraFX;
		if (cameraFX && cameraFX.viewModel != vm)
		{
			ViewModel viewModel = cameraFX.viewModel;
			cameraFX.SetViewModel(vm, rep, item);
			if (!butDontDestroyOld && viewModel)
			{
				UnityEngine.Object.Destroy(viewModel.gameObject);
			}
		}
	}

	public static Ray? Screen2Ray(Vector3 point)
	{
		if (CameraFX.Bind())
		{
			return new Ray?(CameraFX._mainCameraFX.ScreenPointToRay(point));
		}
		if (!CameraFX._hasMainCamera)
		{
			return null;
		}
		return new Ray?(CameraFX._mainCamera.ScreenPointToRay(point));
	}

	public static Vector3? Screen2Viewport(Vector3 point)
	{
		if (CameraFX.Bind())
		{
			return new Vector3?(CameraFX._mainCameraFX.ViewportToScreenPoint(point));
		}
		if (!CameraFX._hasMainCamera)
		{
			return null;
		}
		return new Vector3?(CameraFX._mainCamera.ViewportToScreenPoint(point));
	}

	public static Vector3? Screen2World(Vector3 point)
	{
		if (CameraFX.Bind())
		{
			return new Vector3?(CameraFX._mainCameraFX.ScreenToWorldPoint(point));
		}
		if (!CameraFX._hasMainCamera)
		{
			return null;
		}
		return new Vector3?(CameraFX._mainCamera.ScreenToWorldPoint(point));
	}

	public Ray ScreenPointToRay(Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1;
		Vector3G vector3G2 = new Vector3G(v);
		this.projectScreen.UnProject(ref vector3G2, out vector3G);
		vector3G2.z = vector3G2.z + 1;
		this.projectScreen.UnProject(ref vector3G2, out vector3G1);
		return new Ray(vector3G.f, new Vector3((float)(vector3G1.x - vector3G.x), (float)(vector3G1.y - vector3G.y), (float)(vector3G1.z - vector3G.z)));
	}

	public Vector3 ScreenToViewportPoint(Vector3 v)
	{
		return this.camera.ScreenToViewportPoint(v);
	}

	public Vector3 ScreenToWorldPoint(Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		this.projectScreen.UnProject(ref vector3G1, out vector3G);
		return vector3G.f;
	}

	public bool ScreenToWorldPoint(ref Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		bool flag = this.projectScreen.UnProject(ref vector3G1, out vector3G);
		v = vector3G.f;
		return flag;
	}

	public bool ScreenToWorldPoint(ref Vector3 v, out Vector3 p)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		bool flag = this.projectScreen.UnProject(ref vector3G1, out vector3G);
		p = vector3G.f;
		return flag;
	}

	public void SetFieldOfView(float fieldOfView, float fraction)
	{
		this.fieldOfViewAdjustment = fieldOfView;
		this.fieldOfViewFraction = fraction;
		this.ApplyFieldOfView();
	}

	private void SetViewModel(ViewModel vm)
	{
		this.SetViewModel(vm, null, null);
	}

	private void SetViewModel(ViewModel vm, ItemRepresentation rep, IHeldItem item)
	{
		if (!this.awoke)
		{
			this.viewModel = vm;
			this.rep = rep;
			this.item = item;
			return;
		}
		if (this.viewModel != vm)
		{
			if (this.viewModel)
			{
				if (this.viewModel.itemRep)
				{
					try
					{
						this.viewModel.itemRep.UnBindViewModel(this.viewModel, this.viewModel.item);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception, this.viewModel.itemRep);
					}
				}
				this.viewModel.UnBindTransforms();
				this.viewModel.idMain = null;
			}
			this.viewModel = vm;
			if (vm)
			{
				if (!CameraFX.viewModelRootTransform)
				{
					Transform gameObject = (new GameObject("__View Model Root")).transform;
					CameraFX.viewModelRootTransform = (new GameObject("Eye Camera Difference")).transform;
					CameraFX.viewModelRootTransform.parent = gameObject;
				}
				vm.idMain = this.idMain;
				vm.transform.parent = CameraFX.viewModelRootTransform;
				if (rep)
				{
					rep.PrepareViewModel(vm, item);
				}
				vm.BindTransforms(CameraFX.viewModelRootTransform, base.transform.parent);
				if (rep)
				{
					rep.BindViewModel(vm, item);
					vm.itemRep = rep;
					vm.item = item;
				}
			}
			for (int i = (int)this._effects.Length - 1; i >= 0; i--)
			{
				if (this._effects[i])
				{
					this.effects[i].OnViewModelChange(vm);
				}
			}
		}
	}

	public Vector3 TransformDirection(Vector3 v)
	{
		return this.localToWorldMatrix.f.MultiplyVector(v);
	}

	public Vector3 TransformPoint(Vector3 v)
	{
		return this.localToWorldMatrix.f.MultiplyPoint3x4(v);
	}

	public static void TransitionNow(float duration, TransitionFunction function)
	{
		if (duration > 0f)
		{
			CameraFX.g_trans.Set(duration, function);
		}
		else
		{
			float single = Single.NegativeInfinity;
			float single1 = single;
			CameraFX.g_trans.start = single;
			CameraFX.g_trans.end = single1;
		}
	}

	public static Ray? Viewport2Ray(Vector3 point)
	{
		if (CameraFX.Bind())
		{
			return new Ray?(CameraFX._mainCameraFX.ScreenPointToRay(point));
		}
		if (!CameraFX._hasMainCamera)
		{
			return null;
		}
		return new Ray?(CameraFX._mainCamera.ScreenPointToRay(point));
	}

	public static Vector3? Viewport2Screen(Vector3 point)
	{
		if (CameraFX.Bind())
		{
			return new Vector3?(CameraFX._mainCameraFX.ViewportToScreenPoint(point));
		}
		if (!CameraFX._hasMainCamera)
		{
			return null;
		}
		return new Vector3?(CameraFX._mainCamera.ViewportToScreenPoint(point));
	}

	public static Vector3? Viewport2World(Vector3 point)
	{
		if (CameraFX.Bind())
		{
			return new Vector3?(CameraFX._mainCameraFX.ScreenToWorldPoint(point));
		}
		if (!CameraFX._hasMainCamera)
		{
			return null;
		}
		return new Vector3?(CameraFX._mainCamera.ScreenToWorldPoint(point));
	}

	public Ray ViewportPointToRay(Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1;
		Vector3G vector3G2 = new Vector3G(v);
		this.projectViewport.UnProject(ref vector3G2, out vector3G);
		vector3G2.z = vector3G2.z + 1;
		this.projectViewport.UnProject(ref vector3G2, out vector3G1);
		return new Ray(vector3G.f, new Vector3((float)(vector3G1.x - vector3G.x), (float)(vector3G1.y - vector3G.y), (float)(vector3G1.z - vector3G.z)));
	}

	public Vector3 ViewportToScreenPoint(Vector3 v)
	{
		return this.camera.ViewportToScreenPoint(v);
	}

	public Vector3 ViewportToWorldPoint(Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		this.projectViewport.UnProject(ref vector3G1, out vector3G);
		return vector3G.f;
	}

	public bool ViewportToWorldPoint(ref Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		bool flag = this.projectViewport.UnProject(ref vector3G1, out vector3G);
		v = vector3G.f;
		return flag;
	}

	public bool ViewportToWorldPoint(ref Vector3 v, out Vector3 p)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		bool flag = this.projectViewport.UnProject(ref vector3G1, out vector3G);
		p = vector3G.f;
		return flag;
	}

	public static Vector3? World2Screen(Vector3 point)
	{
		if (CameraFX.Bind())
		{
			return new Vector3?(CameraFX._mainCameraFX.WorldToScreenPoint(point));
		}
		if (!CameraFX._hasMainCamera)
		{
			return null;
		}
		return new Vector3?(CameraFX._mainCamera.WorldToScreenPoint(point));
	}

	public static Vector3? World2Viewport(Vector3 point)
	{
		if (CameraFX.Bind())
		{
			return new Vector3?(CameraFX._mainCameraFX.WorldToViewportPoint(point));
		}
		if (!CameraFX._hasMainCamera)
		{
			return null;
		}
		return new Vector3?(CameraFX._mainCamera.WorldToViewportPoint(point));
	}

	public Vector3 WorldToScreenPoint(Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		this.projectScreen.Project(ref vector3G1, out vector3G);
		return vector3G.f;
	}

	public bool WorldToScreenPoint(ref Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		bool flag = this.projectScreen.Project(ref vector3G1, out vector3G);
		v = vector3G.f;
		return flag;
	}

	public bool WorldToScreenPoint(ref Vector3 v, out Vector3 p)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		bool flag = this.projectScreen.Project(ref vector3G1, out vector3G);
		p = vector3G.f;
		return flag;
	}

	public Vector3 WorldToViewportPoint(Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		this.projectViewport.Project(ref vector3G1, out vector3G);
		return vector3G.f;
	}

	public bool WorldToViewportPoint(ref Vector3 v)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		bool flag = this.projectViewport.Project(ref vector3G1, out vector3G);
		v = vector3G.f;
		return flag;
	}

	public bool WorldToViewportPoint(ref Vector3 v, out Vector3 p)
	{
		Vector3G vector3G;
		Vector3G vector3G1 = new Vector3G(v);
		bool flag = this.projectViewport.Project(ref vector3G1, out vector3G);
		p = vector3G.f;
		return flag;
	}

	public struct CameraTransitionData
	{
		public TransitionFunction func;

		public Matrix4x4G view;

		public Matrix4x4G proj;

		private Matrix4x4G lastView;

		private Matrix4x4G lastProj;

		public float start;

		public float end;

		public float lastTime;

		public static CameraFX.CameraTransitionData identity
		{
			get
			{
				CameraFX.CameraTransitionData cameraTransitionDatum = new CameraFX.CameraTransitionData();
				Matrix4x4G matrix4x4G = Matrix4x4G.identity;
				Matrix4x4G matrix4x4G1 = matrix4x4G;
				cameraTransitionDatum.lastProj = matrix4x4G;
				Matrix4x4G matrix4x4G2 = matrix4x4G1;
				matrix4x4G1 = matrix4x4G2;
				cameraTransitionDatum.lastView = matrix4x4G2;
				Matrix4x4G matrix4x4G3 = matrix4x4G1;
				matrix4x4G1 = matrix4x4G3;
				cameraTransitionDatum.proj = matrix4x4G3;
				cameraTransitionDatum.view = matrix4x4G1;
				float single = Single.NegativeInfinity;
				float single1 = single;
				cameraTransitionDatum.lastTime = single;
				float single2 = single1;
				single1 = single2;
				cameraTransitionDatum.start = single2;
				cameraTransitionDatum.end = single1;
				cameraTransitionDatum.func = TransitionFunction.Linear;
				return cameraTransitionDatum;
			}
		}

		public static float timeSource
		{
			get
			{
				return Time.time;
			}
		}

		public void Set(float duration, TransitionFunction func)
		{
			this.start = CameraFX.CameraTransitionData.timeSource;
			this.lastTime = this.start;
			this.end = this.start + duration;
			this.view = this.lastView;
			this.proj = this.lastProj;
			this.func = func;
		}

		public int Update(ref Matrix4x4G currentView, ref Matrix4x4G currentProj)
		{
			int num;
			try
			{
				float single = CameraFX.CameraTransitionData.timeSource;
				if (this.end > single)
				{
					float single1 = Mathf.InverseLerp(this.start, this.end, single);
					if (single1 < 1f)
					{
						single1 = this.func.Evaluate(single1);
						Matrix4x4G camera = TransitionFunctions.SlerpWorldToCamera((double)single1, this.view, currentView);
						Matrix4x4G matrix4x4G = TransitionFunctions.Linear((double)single1, this.proj, currentProj);
						this.lastTime = single;
						if (!Matrix4x4G.Equals(ref camera, ref currentView))
						{
							currentView = camera;
							if (Matrix4x4G.Equals(ref matrix4x4G, ref currentProj))
							{
								num = 1;
								return num;
							}
							else
							{
								currentProj = matrix4x4G;
								num = 3;
								return num;
							}
						}
						else if (!Matrix4x4G.Equals(ref matrix4x4G, ref currentProj))
						{
							currentProj = matrix4x4G;
							num = 2;
							return num;
						}
					}
				}
				num = 0;
			}
			finally
			{
				this.lastView = currentView;
				this.lastProj = currentProj;
			}
			return num;
		}
	}

	private static class PLATFORM_POLL
	{
		public readonly static bool flipRequired;

		static PLATFORM_POLL()
		{
			string str = SystemInfo.graphicsDeviceVersion;
			if (str != null)
			{
				if (!str.StartsWith("OpenGL", StringComparison.InvariantCultureIgnoreCase))
				{
					if (!str.StartsWith("Direct3D", StringComparison.InvariantCultureIgnoreCase))
					{
						goto Label1;
					}
					CameraFX.PLATFORM_POLL.flipRequired = true;
					return;
				}
				else
				{
					CameraFX.PLATFORM_POLL.flipRequired = false;
					return;
				}
			}
		Label1:
			RuntimePlatform runtimePlatform = Application.platform;
			switch (runtimePlatform)
			{
				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsWebPlayer:
				case RuntimePlatform.WindowsEditor:
				{
				Label2:
					CameraFX.PLATFORM_POLL.flipRequired = true;
					break;
				}
				default:
				{
					switch (runtimePlatform)
					{
						case RuntimePlatform.MetroPlayerX86:
						case RuntimePlatform.MetroPlayerX64:
						case RuntimePlatform.MetroPlayerARM:
						{
							goto Label2;
						}
						default:
						{
							if (runtimePlatform == RuntimePlatform.XBOX360)
							{
								goto Label2;
							}
							CameraFX.PLATFORM_POLL.flipRequired = false;
							break;
						}
					}
					break;
				}
			}
		}
	}
}