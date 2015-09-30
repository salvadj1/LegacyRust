using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class MountedCamera : MonoBehaviour
{
	public Camera camera;

	private static MountedCamera singleton;

	private CameraFX _cameraFX;

	private Matrix4x4 transitionView;

	private Matrix4x4 transitionProj;

	private float transitionStart;

	private float transitionEnd;

	private TransitionFunction transitionFunc;

	private bool once;

	private Matrix4x4 lastView;

	private Matrix4x4 lastProj;

	private CameraMount mount;

	private readonly static Matrix4x4 negateZMatrix;

	public CameraFX cameraFX
	{
		get
		{
			return this._cameraFX;
		}
	}

	public static MountedCamera main
	{
		get
		{
			return MountedCamera.singleton;
		}
	}

	static MountedCamera()
	{
		MountedCamera.negateZMatrix = Matrix4x4.Scale(new Vector3(1f, 1f, -1f));
	}

	public MountedCamera()
	{
	}

	private void Awake()
	{
		this.camera = base.camera;
		MountedCamera.singleton = this;
		CameraFXPre.mountedCamera = this;
		CameraFXPost.mountedCamera = this;
	}

	public static bool GetPoint(out Vector3 point)
	{
		if (!MountedCamera.singleton || !MountedCamera.singleton.camera || !MountedCamera.singleton.camera.enabled)
		{
			point = new Vector3();
			return false;
		}
		Matrix4x4 matrix4x4 = MountedCamera.singleton.camera.worldToCameraMatrix;
		point = matrix4x4.MultiplyPoint(Vector3.zero);
		return true;
	}

	public static bool IsCameraBeingUsed(Camera camera)
	{
		bool flag;
		if (!camera || !MountedCamera.singleton)
		{
			flag = false;
		}
		else if (!MountedCamera.singleton.camera || !MountedCamera.singleton.camera.enabled)
		{
			flag = false;
		}
		else if (camera == MountedCamera.singleton.camera)
		{
			flag = true;
		}
		else
		{
			flag = (!MountedCamera.singleton.mount ? false : MountedCamera.singleton.mount.camera == camera);
		}
		return flag;
	}

	public static bool IsMountedCamera(Camera camera)
	{
		bool flag;
		if (!MountedCamera.singleton)
		{
			flag = false;
		}
		else if (MountedCamera.singleton.camera == camera)
		{
			flag = true;
		}
		else
		{
			flag = (!MountedCamera.singleton.mount ? false : MountedCamera.singleton.mount.camera == camera);
		}
		return flag;
	}

	private void OnDestroy()
	{
		if (MountedCamera.singleton == this)
		{
			MountedCamera.singleton = null;
		}
	}

	public void PreCullBegin()
	{
		CameraMount cameraMount = CameraMount.current;
		if (cameraMount != this.mount)
		{
			if (!cameraMount)
			{
				this._cameraFX = null;
			}
			else
			{
				this._cameraFX = cameraMount.cameraFX;
			}
			CameraFXPre.cameraFX = this._cameraFX;
			CameraFXPost.cameraFX = this._cameraFX;
			this.mount = cameraMount;
		}
		if (this.mount)
		{
			Camera camera = this.mount.camera;
			camera.ResetAspect();
			camera.ResetProjectionMatrix();
			camera.ResetWorldToCameraMatrix();
			this.mount.OnPreMount(this);
		}
	}

	public void PreCullEnd(bool postCamFX)
	{
		if (!this.mount)
		{
			if (!this.once)
			{
				this.lastView = this.camera.worldToCameraMatrix;
				this.lastProj = this.camera.projectionMatrix;
				this.once = true;
			}
			this.camera.ResetProjectionMatrix();
			this.camera.ResetWorldToCameraMatrix();
			this.camera.worldToCameraMatrix = this.lastView;
			this.camera.projectionMatrix = this.lastProj;
			if (!postCamFX)
			{
				CameraFX.ApplyTransitionAlterations(this.camera, null, false);
			}
		}
		else
		{
			Transform transforms = this.mount.transform;
			base.transform.position = transforms.position;
			base.transform.rotation = transforms.rotation;
			CameraClearFlags cameraClearFlag = this.camera.clearFlags;
			int num = this.camera.cullingMask;
			DepthTextureMode depthTextureMode = this.camera.depthTextureMode;
			this.camera.ResetProjectionMatrix();
			this.camera.ResetWorldToCameraMatrix();
			this.mount.camera.depthTextureMode = depthTextureMode;
			this.camera.CopyFrom(this.mount.camera);
			if (!postCamFX)
			{
				CameraFX.ApplyTransitionAlterations(this.camera, null, false);
			}
			this.camera.clearFlags = cameraClearFlag;
			this.camera.cullingMask = num;
			if (this.camera.depthTextureMode != depthTextureMode)
			{
				Debug.Log("Yea this is changing depth texture mode!", this.mount);
				this.camera.depthTextureMode = depthTextureMode;
			}
			this.mount.OnPostMount(this);
			this.lastView = this.camera.worldToCameraMatrix;
			this.lastProj = this.camera.projectionMatrix;
			this.once = true;
		}
		Matrix4x4 matrix4x4 = this.camera.cameraToWorldMatrix;
		base.transform.position = matrix4x4.MultiplyPoint(Vector3.zero);
		base.transform.rotation = Quaternion.LookRotation(matrix4x4.MultiplyVector(-Vector3.forward), matrix4x4.MultiplyVector(Vector3.up));
		Shader.SetGlobalMatrix("_RUST_MATRIX_CAMERA_TO_WORLD", matrix4x4 * MountedCamera.negateZMatrix);
		Shader.SetGlobalMatrix("_RUST_MATRIX_WORLD_TO_CAMERA", this.camera.worldToCameraMatrix * MountedCamera.negateZMatrix);
	}
}