using System;
using UnityEngine;

public class BulletWeaponImpact : WeaponImpact
{
	public readonly Transform hitTransform;

	private Vector3 hitPoint;

	private Vector3 hitDirection;

	public BulletWeaponDataBlock dataBlock
	{
		get
		{
			return (BulletWeaponDataBlock)this.dataBlock;
		}
	}

	public IBulletWeaponItem item
	{
		get
		{
			return this.item as IBulletWeaponItem;
		}
	}

	public Vector3 localDirection
	{
		get
		{
			return (!this.hitTransform ? Vector3.forward : this.hitDirection);
		}
	}

	public Vector3 localPoint
	{
		get
		{
			return (!this.hitTransform ? new Vector3() : this.hitPoint);
		}
	}

	public Vector3 worldDirection
	{
		get
		{
			return (!this.hitTransform ? this.hitDirection : this.hitTransform.TransformDirection(this.hitDirection));
		}
	}

	public Vector3 worldPoint
	{
		get
		{
			return (!this.hitTransform ? this.hitPoint : this.hitTransform.TransformPoint(this.hitPoint));
		}
	}

	public BulletWeaponImpact(BulletWeaponDataBlock dataBlock, IBulletWeaponItem item, ItemRepresentation itemRep, Transform hitTransform, Vector3 localHitPoint, Vector3 localHitDirection) : base(dataBlock, item, itemRep)
	{
		this.hitTransform = hitTransform;
		this.hitPoint = localHitPoint;
		this.hitDirection = localHitDirection;
	}

	public BulletWeaponImpact(BulletWeaponDataBlock dataBlock, IBulletWeaponItem item, ItemRepresentation itemRep, Vector3 worldHitPoint, Vector3 worldHitDirection) : this(dataBlock, item, itemRep, null, worldHitPoint, worldHitDirection)
	{
	}
}