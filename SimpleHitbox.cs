using System;
using UnityEngine;

public class SimpleHitbox : BaseHitBox
{
	private bool oldCrouch;

	private CapsuleCollider myCap;

	public bool fixedUpdate;

	private Transform parent;

	private Transform root;

	private Vector3 offset;

	public float crouchHeight = 1f;

	public float standingHeight = 1.85f;

	private Transform rootTransform;

	private Transform myTransform;

	public SimpleHitbox()
	{
	}

	private void FixedUpdate()
	{
		if (this.fixedUpdate)
		{
			this.Snap();
		}
	}

	private void Snap()
	{
		if (base.idMain.stateFlags.crouch != this.oldCrouch)
		{
			if (!base.idMain.stateFlags.crouch)
			{
				this.myCap.height = this.standingHeight;
			}
			else
			{
				this.myCap.height = this.crouchHeight;
			}
			this.oldCrouch = base.idMain.stateFlags.crouch;
		}
		Vector3 vector3 = this.parent.TransformPoint(this.offset);
		Vector3 vector31 = this.rootTransform.position;
		vector3.y = vector31.y + this.myCap.height * 0.5f;
		this.myTransform.position = vector3;
	}

	private void Start()
	{
		this.myCap = base.collider as CapsuleCollider;
		this.parent = base.transform.parent;
		this.root = this.parent.root;
		this.offset = Vector3.zero;
		base.transform.parent = null;
		this.rootTransform = this.root.transform;
		this.myTransform = base.transform;
	}

	private void Update()
	{
		if (!this.fixedUpdate)
		{
			this.Snap();
		}
	}
}