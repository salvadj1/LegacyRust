using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LazyCam : MonoBehaviour, ICameraFX
{
	private Quaternion aim;

	private Quaternion view;

	private Quaternion sub;

	private Quaternion @add;

	public float maxAngle = 10f;

	public float damp = 0.01f;

	public float targetAngle = 10f;

	public float enableSeconds = 0.1f;

	public float disableSeconds = 0.1f;

	private float enableFraction;

	[NonSerialized]
	private bool isActivelyLazy;

	[NonSerialized]
	private bool wasActivelyLazy;

	[NonSerialized]
	private Matrix4x4 _world2cam;

	[NonSerialized]
	private Matrix4x4 _cam2world;

	[NonSerialized]
	private float vel;

	[NonSerialized]
	private Camera camera;

	[NonSerialized]
	private Transform transform;

	private bool _allow;

	private ViewModel viewModel;

	private bool hasViewModel;

	public bool allow
	{
		get
		{
			return this._allow;
		}
		set
		{
			if (!value)
			{
				this._allow = false;
			}
			else
			{
				base.enabled = true;
				this._allow = true;
			}
		}
	}

	public Matrix4x4 cameraToWorldMatrix
	{
		get
		{
			return (!this.wasActivelyLazy ? this.camera.cameraToWorldMatrix : this._cam2world);
		}
	}

	public Matrix4x4 worldToCameraMatrix
	{
		get
		{
			return (!this.wasActivelyLazy ? this.camera.worldToCameraMatrix : this._world2cam);
		}
	}

	public LazyCam()
	{
	}

	private void Awake()
	{
		this.transform = base.transform;
		this.camera = base.camera;
		if (!this.camera)
		{
			Debug.LogError("No camera detected");
		}
	}

	void ICameraFX.OnViewModelChange(ViewModel viewModel)
	{
		if (this.hasViewModel && this.viewModel)
		{
			this.viewModel.lazyCam = null;
		}
		this.viewModel = viewModel;
		this.hasViewModel = this.viewModel;
		if (!this.hasViewModel)
		{
			this.allow = false;
		}
		else
		{
			this.viewModel.lazyCam = this;
			this.targetAngle = this.viewModel.lazyAngle;
			this.allow = true;
		}
	}

	void ICameraFX.PostRender()
	{
		bool flag = this.isActivelyLazy;
		bool flag1 = flag;
		this.wasActivelyLazy = flag;
		if (flag1)
		{
			this.isActivelyLazy = false;
			Transform transforms = this.transform;
			transforms.rotation = transforms.rotation * this.sub;
		}
	}

	void ICameraFX.PreCull()
	{
		this.aim = this.transform.rotation;
		Quaternion quaternion = Quaternion.identity;
		Quaternion quaternion1 = quaternion;
		this.sub = quaternion;
		this.@add = quaternion1;
		if (!this._allow)
		{
			LazyCam lazyCam = this;
			lazyCam.enableFraction = lazyCam.enableFraction - Time.deltaTime / this.disableSeconds;
			if (this.enableFraction <= 0f)
			{
				this.enableFraction = 0f;
			}
		}
		else
		{
			LazyCam lazyCam1 = this;
			lazyCam1.enableFraction = lazyCam1.enableFraction + Time.deltaTime / this.enableSeconds;
			if (this.enableFraction >= 1f)
			{
				this.enableFraction = 1f;
			}
		}
		this.maxAngle = Mathf.SmoothDampAngle(this.maxAngle, this.targetAngle * this.enableFraction, ref this.vel, this.damp);
		if (!Mathf.Approximately(this.maxAngle, 0f))
		{
			this.isActivelyLazy = true;
			float single = Quaternion.Angle(this.aim, this.view);
			if (single >= this.maxAngle)
			{
				float single1 = 1f - this.maxAngle / single;
				this.view = Quaternion.Slerp(this.view, this.aim, single1);
			}
			Quaternion quaternion2 = Quaternion.Inverse(this.aim) * this.view;
			quaternion1 = quaternion2;
			this.@add = quaternion2;
			this.sub = Quaternion.Inverse(quaternion1);
			this.transform.rotation = this.view;
			this._world2cam = this.camera.worldToCameraMatrix;
			this._cam2world = this.camera.cameraToWorldMatrix;
			if (this.hasViewModel)
			{
				this.viewModel.lazyRotation = this.sub;
			}
		}
		else
		{
			this.view = this.aim;
			if (!this._allow)
			{
				base.enabled = false;
			}
			if (this.hasViewModel)
			{
				this.viewModel.lazyRotation = Quaternion.identity;
			}
		}
	}

	private void Start()
	{
		this.view = this.transform.rotation;
	}
}