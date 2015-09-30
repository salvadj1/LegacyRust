using System;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class CCDesc : MonoBehaviour
{
	[SerializeField]
	private float m_Height = 2f;

	[SerializeField]
	private float m_Radius = 0.4f;

	[SerializeField]
	private float m_SlopeLimit = 90f;

	[SerializeField]
	private float m_StepOffset = 0.5f;

	[SerializeField]
	private float m_SkinWidth = 0.05f;

	[SerializeField]
	private float m_MinMoveDistance;

	[SerializeField]
	private Vector3 m_Center = new Vector3()
	{
		y = 1f
	};

	[HideInInspector]
	[PrefetchComponent]
	[SerializeField]
	private CharacterController m_Collider;

	[NonSerialized]
	public object Tag;

	private static CCDesc s_CurrentMovingCCDesc;

	[NonSerialized]
	internal CCDesc.HitManager AssignedHitManager;

	public Rigidbody attachedRigidbody
	{
		get
		{
			return this.m_Collider.attachedRigidbody;
		}
	}

	public Vector3 bottom
	{
		get
		{
			Vector3 mCenter = new Vector3();
			mCenter.x = this.m_Center.x;
			mCenter.z = this.m_Center.z;
			float mHeight = this.m_Height * 0.5f;
			if (this.m_Radius > mHeight)
			{
				mHeight = this.m_Radius;
			}
			mCenter.y = this.m_Center.y - mHeight;
			return mCenter;
		}
	}

	public Bounds bounds
	{
		get
		{
			return this.m_Collider.bounds;
		}
	}

	public Vector3 center
	{
		get
		{
			return this.m_Center;
		}
	}

	public Vector3 centroidBottom
	{
		get
		{
			Vector3 mCenter = new Vector3();
			mCenter.x = this.m_Center.x;
			mCenter.z = this.m_Center.z;
			float mHeight = this.m_Height * 0.5f;
			mHeight = (this.m_Radius <= mHeight ? mHeight - this.m_Radius : 0f);
			mCenter.y = this.m_Center.y - mHeight;
			return mCenter;
		}
	}

	public Vector3 centroidTop
	{
		get
		{
			Vector3 mCenter = new Vector3();
			mCenter.x = this.m_Center.x;
			mCenter.z = this.m_Center.z;
			float mHeight = this.m_Height * 0.5f;
			mHeight = (this.m_Radius <= mHeight ? mHeight - this.m_Radius : 0f);
			mCenter.y = this.m_Center.y + mHeight;
			return mCenter;
		}
	}

	public new CharacterController collider
	{
		get
		{
			return this.m_Collider;
		}
	}

	public CollisionFlags collisionFlags
	{
		get
		{
			return this.m_Collider.collisionFlags;
		}
	}

	public bool detectCollisions
	{
		get
		{
			return this.m_Collider.detectCollisions;
		}
		set
		{
			this.m_Collider.detectCollisions = value;
		}
	}

	public float diameter
	{
		get
		{
			return this.m_Radius + this.m_Radius;
		}
	}

	public float effectiveHeight
	{
		get
		{
			float mRadius = this.m_Radius + this.m_Radius;
			return (mRadius <= this.m_Height ? this.m_Height : mRadius);
		}
	}

	public float effectiveSkinnedHeight
	{
		get
		{
			float mRadius = this.m_Radius + this.m_Radius;
			return (mRadius <= this.m_Height ? this.m_Height : mRadius) + (this.m_SkinWidth + this.m_SkinWidth);
		}
	}

	public new bool enabled
	{
		get
		{
			return this.m_Collider.enabled;
		}
		set
		{
			this.m_Collider.enabled = value;
		}
	}

	public Quaternion flatRotation
	{
		get
		{
			Vector3 vector3 = base.transform.forward;
			vector3.y = vector3.x * vector3.x + vector3.z * vector3.z;
			if (Mathf.Approximately(vector3.y, 0f))
			{
				Vector3 vector31 = base.transform.right;
				vector3.z = vector31.x;
				vector3.x = -vector31.z;
				vector3.y = vector31.x * vector31.x + vector31.z * vector31.z;
			}
			if (vector3.y != 1f)
			{
				vector3.y = 1f / Mathf.Sqrt(vector3.y);
			}
			vector3.x = vector3.x * vector3.y;
			vector3.z = vector3.z * vector3.z;
			vector3.y = 0f;
			return Quaternion.LookRotation(vector3, Vector3.up);
		}
	}

	public float height
	{
		get
		{
			return this.m_Height;
		}
	}

	public bool isGrounded
	{
		get
		{
			return this.m_Collider.isGrounded;
		}
	}

	public bool isTrigger
	{
		get
		{
			return this.m_Collider.isTrigger;
		}
		set
		{
			this.m_Collider.isTrigger = value;
		}
	}

	public PhysicMaterial material
	{
		get
		{
			return this.m_Collider.material;
		}
		set
		{
			this.m_Collider.material = value;
		}
	}

	public float minMoveDistance
	{
		get
		{
			return this.m_MinMoveDistance;
		}
	}

	public static CCDesc Moving
	{
		get
		{
			return CCDesc.s_CurrentMovingCCDesc;
		}
	}

	public float radius
	{
		get
		{
			return this.m_Radius;
		}
	}

	public PhysicMaterial sharedMaterial
	{
		get
		{
			return this.m_Collider.sharedMaterial;
		}
		set
		{
			this.m_Collider.sharedMaterial = value;
		}
	}

	public Vector3 skinnedBottom
	{
		get
		{
			Vector3 mCenter = new Vector3();
			mCenter.x = this.m_Center.x;
			mCenter.z = this.m_Center.z;
			float mHeight = this.m_Height * 0.5f;
			if (this.m_Radius > mHeight)
			{
				mHeight = this.m_Radius;
			}
			mCenter.y = this.m_Center.y - (mHeight + this.m_SkinWidth);
			return mCenter;
		}
	}

	public float skinnedDiameter
	{
		get
		{
			return this.m_Radius + this.m_Radius + this.m_SkinWidth + this.m_SkinWidth;
		}
	}

	public float skinnedHeight
	{
		get
		{
			return this.m_Height + this.m_SkinWidth + this.m_SkinWidth;
		}
	}

	public float skinnedRadius
	{
		get
		{
			return this.m_Radius + this.m_SkinWidth;
		}
	}

	public Vector3 skinnedTop
	{
		get
		{
			Vector3 mCenter = new Vector3();
			mCenter.x = this.m_Center.x;
			mCenter.z = this.m_Center.z;
			float mHeight = this.m_Height * 0.5f;
			if (this.m_Radius > mHeight)
			{
				mHeight = this.m_Radius;
			}
			mCenter.y = this.m_Center.y + mHeight + this.m_SkinWidth;
			return mCenter;
		}
	}

	public float skinWidth
	{
		get
		{
			return this.m_SkinWidth;
		}
	}

	public float slopeLimit
	{
		get
		{
			return this.m_SlopeLimit;
		}
	}

	public float stepOffset
	{
		get
		{
			return this.m_StepOffset;
		}
	}

	public Vector3 top
	{
		get
		{
			Vector3 mCenter = new Vector3();
			mCenter.x = this.m_Center.x;
			mCenter.z = this.m_Center.z;
			float mHeight = this.m_Height * 0.5f;
			if (this.m_Radius > mHeight)
			{
				mHeight = this.m_Radius;
			}
			mCenter.y = this.m_Center.y + mHeight;
			return mCenter;
		}
	}

	public Vector3 velocity
	{
		get
		{
			return this.m_Collider.velocity;
		}
	}

	public Vector3 worldBottom
	{
		get
		{
			return this.OffsetToWorld(this.bottom);
		}
	}

	public Vector3 worldCenter
	{
		get
		{
			return this.OffsetToWorld(this.m_Center);
		}
	}

	public Vector3 worldCentroidBottom
	{
		get
		{
			return this.OffsetToWorld(this.centroidBottom);
		}
	}

	public Vector3 worldCentroidTop
	{
		get
		{
			return this.OffsetToWorld(this.centroidTop);
		}
	}

	public Vector3 worldSkinnedBottom
	{
		get
		{
			return this.OffsetToWorld(this.skinnedBottom);
		}
	}

	public Vector3 worldSkinnedTop
	{
		get
		{
			return this.OffsetToWorld(this.skinnedTop);
		}
	}

	public Vector3 worldTop
	{
		get
		{
			return this.OffsetToWorld(this.top);
		}
	}

	public CCDesc()
	{
	}

	public Vector3 ClosestPointOnBounds(Vector3 position)
	{
		return this.m_Collider.ClosestPointOnBounds(position);
	}

	public CCDesc.HeightModification ModifyHeight(float newEffectiveSkinnedHeight, bool preview = false)
	{
		CCDesc.HeightModification mCenter = new CCDesc.HeightModification();
		float mRadius = this.m_Radius + this.m_Radius;
		float mSkinWidth = this.m_SkinWidth + this.m_SkinWidth + mRadius;
		float single = (mRadius <= this.m_Height ? this.m_Height + this.m_SkinWidth + this.m_SkinWidth : mSkinWidth);
		mCenter.original.effectiveSkinnedHeight = single;
		mCenter.original.center = this.m_Center;
		if (newEffectiveSkinnedHeight >= mSkinWidth)
		{
			mCenter.modified.effectiveSkinnedHeight = newEffectiveSkinnedHeight;
		}
		else
		{
			mCenter.modified.effectiveSkinnedHeight = mSkinWidth;
		}
		bool flag = mCenter.original.effectiveSkinnedHeight != mCenter.modified.effectiveSkinnedHeight;
		bool flag1 = flag;
		mCenter.differed = flag;
		if (!flag1)
		{
			mCenter.modified = mCenter.original;
			mCenter.delta = new CCDesc.HeightModification.State();
			mCenter.applied = false;
			mCenter.scale = 1f;
		}
		else
		{
			float single1 = single * 0.5f;
			float single2 = mCenter.original.center.y - single1;
			float single3 = mCenter.original.center.y + single1;
			mCenter.delta.effectiveSkinnedHeight = mCenter.modified.effectiveSkinnedHeight - mCenter.original.effectiveSkinnedHeight;
			mCenter.scale = mCenter.modified.effectiveSkinnedHeight / mCenter.original.effectiveSkinnedHeight;
			float single4 = single2 * mCenter.scale;
			float single5 = single3 * mCenter.scale;
			mCenter.modified.center.x = mCenter.original.center.x;
			mCenter.modified.center.z = mCenter.original.center.z;
			mCenter.modified.center.y = single4 + (single5 - single4) * 0.5f;
			mCenter.delta.center.x = 0f;
			mCenter.delta.center.z = 0f;
			mCenter.delta.center.y = mCenter.modified.center.y - mCenter.original.center.y;
			bool flag2 = !preview;
			flag1 = flag2;
			mCenter.applied = flag2;
			if (flag1)
			{
				this.m_Height = mCenter.modified.effectiveSkinnedHeight - (this.m_SkinWidth + this.m_SkinWidth);
				this.m_Center = mCenter.modified.center;
				if (mCenter.scale >= 1f)
				{
					this.m_Collider.height = this.m_Height;
					this.m_Collider.center = this.m_Center;
				}
				else
				{
					this.m_Collider.center = this.m_Center;
					this.m_Collider.height = this.m_Height;
				}
			}
		}
		return mCenter;
	}

	public CollisionFlags Move(Vector3 motion)
	{
		CollisionFlags collisionFlag;
		CCDesc cCDesc;
		CCDesc sCurrentMovingCCDesc = CCDesc.s_CurrentMovingCCDesc;
		try
		{
			CCDesc.s_CurrentMovingCCDesc = this;
			if (!object.ReferenceEquals(this.AssignedHitManager, null))
			{
				this.AssignedHitManager.Clear();
			}
			collisionFlag = this.m_Collider.Move(motion);
		}
		finally
		{
			if (!sCurrentMovingCCDesc)
			{
				cCDesc = null;
			}
			else
			{
				cCDesc = sCurrentMovingCCDesc;
			}
			CCDesc.s_CurrentMovingCCDesc = cCDesc;
		}
		return collisionFlag;
	}

	public Vector3 OffsetToWorld(Vector3 offset)
	{
		if (offset.x != 0f || offset.z != 0f)
		{
			offset = this.flatRotation * offset;
		}
		Vector3 vector3 = base.transform.lossyScale;
		offset.x = offset.x * vector3.x;
		offset.y = offset.y * vector3.y;
		offset.z = offset.z * vector3.z;
		Vector3 vector31 = base.transform.position;
		offset.x = offset.x + vector31.x;
		offset.y = offset.y + vector31.y;
		offset.z = offset.z + vector31.z;
		return offset;
	}

	public bool Raycast(Ray ray, out RaycastHit hitInfo, float distance)
	{
		return this.m_Collider.Raycast(ray, out hitInfo, distance);
	}

	public bool SimpleMove(Vector3 speed)
	{
		bool flag;
		CCDesc cCDesc;
		CCDesc sCurrentMovingCCDesc = CCDesc.s_CurrentMovingCCDesc;
		try
		{
			CCDesc.s_CurrentMovingCCDesc = this;
			flag = this.m_Collider.SimpleMove(speed);
		}
		finally
		{
			if (!sCurrentMovingCCDesc)
			{
				cCDesc = null;
			}
			else
			{
				cCDesc = sCurrentMovingCCDesc;
			}
			CCDesc.s_CurrentMovingCCDesc = cCDesc;
		}
		return flag;
	}

	public struct HeightModification
	{
		public CCDesc.HeightModification.State original;

		public CCDesc.HeightModification.State modified;

		public CCDesc.HeightModification.State delta;

		public float scale;

		public bool differed;

		public bool applied;

		public float bottomDeltaHeight
		{
			get
			{
				return this.modified.skinnedBottomY - this.original.skinnedBottomY;
			}
		}

		public float topDeltaHeight
		{
			get
			{
				return this.modified.skinnedTopY - this.original.skinnedTopY;
			}
		}

		public struct State
		{
			public float effectiveSkinnedHeight;

			public Vector3 center;

			public float skinnedBottomY
			{
				get
				{
					return this.center.y - this.effectiveSkinnedHeight * 0.5f;
				}
			}

			public float skinnedTopY
			{
				get
				{
					return this.center.y + this.effectiveSkinnedHeight * 0.5f;
				}
			}
		}
	}

	public struct Hit
	{
		public readonly CharacterController CharacterController;

		public readonly CCDesc CCDesc;

		public readonly Collider Collider;

		public readonly Vector3 Point;

		public readonly Vector3 Normal;

		public readonly Vector3 MoveDirection;

		public readonly float MoveLength;

		public GameObject GameObject
		{
			get
			{
				GameObject collider;
				if (!this.Collider)
				{
					collider = null;
				}
				else
				{
					collider = this.Collider.transform.gameObject;
				}
				return collider;
			}
		}

		public Rigidbody Rigidbody
		{
			get
			{
				Rigidbody collider;
				if (!this.Collider)
				{
					collider = null;
				}
				else
				{
					collider = this.Collider.attachedRigidbody;
				}
				return collider;
			}
		}

		public Transform Transform
		{
			get
			{
				Transform collider;
				if (!this.Collider)
				{
					collider = null;
				}
				else
				{
					collider = this.Collider.transform;
				}
				return collider;
			}
		}

		public Hit(UnityEngine.ControllerColliderHit ControllerColliderHit)
		{
			this.CharacterController = ControllerColliderHit.controller;
			CCDesc sCurrentMovingCCDesc = CCDesc.s_CurrentMovingCCDesc;
			if (!sCurrentMovingCCDesc || sCurrentMovingCCDesc.collider != this.CharacterController)
			{
				this.CCDesc = this.CharacterController.GetComponent<CCDesc>();
			}
			else
			{
				this.CCDesc = sCurrentMovingCCDesc;
			}
			this.Collider = ControllerColliderHit.collider;
			this.Point = ControllerColliderHit.point;
			this.Normal = ControllerColliderHit.normal;
			this.MoveDirection = ControllerColliderHit.moveDirection;
			this.MoveLength = ControllerColliderHit.moveLength;
		}
	}

	public delegate bool HitFilter(CCDesc.HitManager hitManager, ref CCDesc.Hit hit);

	public class HitManager : IDisposable
	{
		private CCDesc.Hit[] buffer;

		private int bufferSize;

		private int filledCount;

		private bool issuingEvent;

		public object Tag;

		private CCDesc.HitFilter OnHit;

		public int Count
		{
			get
			{
				return this.filledCount;
			}
		}

		public CCDesc.Hit this[int i]
		{
			get
			{
				if (i < 0 || i >= this.filledCount)
				{
					throw new ArgumentOutOfRangeException("i");
				}
				return this.buffer[i];
			}
		}

		public HitManager(int bufferSize)
		{
			this.bufferSize = bufferSize;
			this.buffer = new CCDesc.Hit[bufferSize];
			this.filledCount = 0;
		}

		public HitManager() : this(8)
		{
		}

		public void Clear()
		{
			while (this.filledCount > 0)
			{
				CCDesc.HitManager hitManager = this;
				int num = hitManager.filledCount - 1;
				int num1 = num;
				hitManager.filledCount = num;
				CCDesc.Hit hit = new CCDesc.Hit();
				this.buffer[num1] = hit;
			}
		}

		public void CopyTo(CCDesc.Hit[] array, int startIndex = 0)
		{
			for (int i = 0; i < this.filledCount; i++)
			{
				int num = startIndex;
				startIndex = num + 1;
				array[num] = this.buffer[i];
			}
		}

		public void Dispose()
		{
			this.buffer = null;
			this.OnHit = null;
		}

		public bool Push(ControllerColliderHit cchit)
		{
			if (this.issuingEvent)
			{
				Debug.LogError("Push during event call back");
				return false;
			}
			if (object.ReferenceEquals(cchit, null))
			{
				return false;
			}
			CCDesc.Hit hit = new CCDesc.Hit(cchit);
			return this.Push(ref hit);
		}

		public bool Push(ref CCDesc.Hit evnt)
		{
			if (this.issuingEvent)
			{
				Debug.LogError("Push during event call back");
				return false;
			}
			CCDesc.HitFilter onHit = this.OnHit;
			if (onHit != null)
			{
				bool flag = false;
				try
				{
					try
					{
						this.issuingEvent = true;
						flag = !onHit(this, ref evnt);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
				finally
				{
					this.issuingEvent = false;
				}
				if (flag)
				{
					return false;
				}
			}
			CCDesc.HitManager hitManager = this;
			int num = hitManager.filledCount;
			int num1 = num;
			hitManager.filledCount = num + 1;
			int num2 = num1;
			if (this.filledCount > this.bufferSize)
			{
				do
				{
					CCDesc.HitManager hitManager1 = this;
					hitManager1.bufferSize = hitManager1.bufferSize + 8;
				}
				while (this.filledCount > this.bufferSize);
				if (this.filledCount <= 1)
				{
					this.buffer = new CCDesc.Hit[this.bufferSize];
				}
				else
				{
					CCDesc.Hit[] hitArray = this.buffer;
					this.buffer = new CCDesc.Hit[this.bufferSize];
					Array.Copy(hitArray, this.buffer, this.filledCount - 1);
				}
			}
			this.buffer[num2] = evnt;
			return true;
		}

		public CCDesc.Hit[] ToArray()
		{
			CCDesc.Hit[] hitArray = new CCDesc.Hit[this.filledCount];
			this.CopyTo(hitArray, 0);
			return hitArray;
		}

		public event CCDesc.HitFilter OnHit
		{
			add
			{
				this.OnHit += value;
			}
			remove
			{
				this.OnHit -= value;
			}
		}
	}
}