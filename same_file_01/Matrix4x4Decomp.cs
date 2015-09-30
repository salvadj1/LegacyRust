using System;
using UnityEngine;

public struct Matrix4x4Decomp
{
	public Vector3 r;

	public Vector3 u;

	public Vector3 f;

	public Vector3 t;

	public Vector4 s;

	public Matrix4x4 m
	{
		get
		{
			Matrix4x4 matrix4x4 = new Matrix4x4();
			matrix4x4.m00 = this.r.x;
			matrix4x4.m01 = this.r.y;
			matrix4x4.m02 = this.r.z;
			matrix4x4.m03 = this.s.x;
			matrix4x4.m10 = this.u.x;
			matrix4x4.m11 = this.u.y;
			matrix4x4.m12 = this.u.z;
			matrix4x4.m13 = this.s.y;
			matrix4x4.m20 = this.f.x;
			matrix4x4.m21 = this.f.y;
			matrix4x4.m22 = this.f.z;
			matrix4x4.m23 = this.s.z;
			matrix4x4.m30 = this.t.x;
			matrix4x4.m31 = this.t.y;
			matrix4x4.m32 = this.t.z;
			matrix4x4.m33 = this.s.w;
			return matrix4x4;
		}
		set
		{
			this.r.x = value.m00;
			this.r.y = value.m01;
			this.r.z = value.m02;
			this.s.x = value.m03;
			this.u.x = value.m10;
			this.u.y = value.m11;
			this.u.z = value.m12;
			this.s.y = value.m13;
			this.f.x = value.m20;
			this.f.y = value.m21;
			this.f.z = value.m22;
			this.s.z = value.m23;
			this.t.x = value.m30;
			this.t.y = value.m31;
			this.t.z = value.m32;
			this.s.w = value.m33;
		}
	}

	public Quaternion q
	{
		get
		{
			return Quaternion.LookRotation(this.f, this.u);
		}
		set
		{
			Quaternion quaternion = value * Quaternion.Inverse(this.q);
			this.r = quaternion * this.r;
			this.u = quaternion * this.u;
			this.f = quaternion * this.f;
		}
	}

	public Vector3 S
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = this.s.x;
			vector3.y = this.s.y;
			vector3.z = this.s.z;
			return vector3;
		}
		set
		{
			this.s.x = value.x;
			this.s.y = value.y;
			this.s.z = value.z;
		}
	}

	public float w
	{
		get
		{
			return this.s.w;
		}
		set
		{
			this.s.w = value;
		}
	}

	public Matrix4x4Decomp(Matrix4x4 v)
	{
		this.r.x = v.m00;
		this.r.y = v.m01;
		this.r.z = v.m02;
		this.s.x = v.m03;
		this.u.x = v.m10;
		this.u.y = v.m11;
		this.u.z = v.m12;
		this.s.y = v.m13;
		this.f.x = v.m20;
		this.f.y = v.m21;
		this.f.z = v.m22;
		this.s.z = v.m23;
		this.t.x = v.m30;
		this.t.y = v.m31;
		this.t.z = v.m32;
		this.s.w = v.m33;
	}
}