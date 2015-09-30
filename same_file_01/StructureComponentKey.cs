using System;
using UnityEngine;

public struct StructureComponentKey : IEquatable<StructureComponentKey>
{
	private const float kStepX = 2.5f;

	private const float kStepY = 4f;

	private const float kStepZ = 2.5f;

	private const float kInverseStepX = 0.4f;

	private const float kInverseStepY = 0.25f;

	private const float kInverseStepZ = 0.4f;

	public readonly int iX;

	public readonly int iY;

	public readonly int iZ;

	public readonly int hashCode;

	public Vector3 vector
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = (float)this.iX * 2.5f;
			vector3.y = (float)this.iY * 4f;
			vector3.z = (float)this.iZ * 2.5f;
			return vector3;
		}
	}

	public float x
	{
		get
		{
			return (float)this.iX * 2.5f;
		}
	}

	public float y
	{
		get
		{
			return (float)this.iY * 4f;
		}
	}

	public float z
	{
		get
		{
			return (float)this.iZ * 2.5f;
		}
	}

	private StructureComponentKey(int iX, int iY, int iZ)
	{
		this.hashCode = (iX << 8 | iX >> 8 & 16777215) ^ (iY << 16 | iY >> 16 & 65535) ^ (iZ << 24 | iZ >> 24 & 255) ^ iX * iY * iZ;
		this.iX = iX;
		this.iY = iY;
		this.iZ = iZ;
	}

	public StructureComponentKey(float x, float y, float z) : this(StructureComponentKey.ROUND(x, 0.4f), StructureComponentKey.ROUND(y, 0.25f), StructureComponentKey.ROUND(z, 0.4f))
	{
	}

	public StructureComponentKey(Vector3 v) : this(v.x, v.y, v.z)
	{
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StructureComponentKey))
		{
			return false;
		}
		StructureComponentKey structureComponentKey = (StructureComponentKey)obj;
		return (structureComponentKey.iX != this.iX || structureComponentKey.iZ != this.iZ ? false : structureComponentKey.iY == this.iY);
	}

	public bool Equals(StructureComponentKey other)
	{
		return (this.iX != other.iX || other.iZ != this.iZ ? false : other.iY == this.iY);
	}

	public override int GetHashCode()
	{
		return this.hashCode;
	}

	public static bool operator ==(StructureComponentKey l, StructureComponentKey r)
	{
		return (l.hashCode != r.hashCode || l.iX != r.iX || l.iY != r.iY ? false : l.iZ == r.iZ);
	}

	public static explicit operator StructureComponentKey(Vector3 v)
	{
		return new StructureComponentKey(StructureComponentKey.ROUND(v.x, 0.4f), StructureComponentKey.ROUND(v.y, 0.25f), StructureComponentKey.ROUND(v.z, 0.4f));
	}

	public static implicit operator Vector3(StructureComponentKey key)
	{
		Vector3 vector3 = new Vector3();
		vector3.x = (float)key.iX * 2.5f;
		vector3.y = (float)key.iY * 4f;
		vector3.z = (float)key.iZ * 2.5f;
		return vector3;
	}

	public static bool operator !=(StructureComponentKey l, StructureComponentKey r)
	{
		return (l.hashCode != r.hashCode || l.iX != r.iX || l.iY != r.iY ? true : l.iZ != r.iZ);
	}

	public static int ROUND(float v, float inverseStepSize)
	{
		if (v < 0f)
		{
			return -Mathf.RoundToInt(v * -inverseStepSize);
		}
		if (v <= 0f)
		{
			return 0;
		}
		return Mathf.RoundToInt(v * inverseStepSize);
	}

	public override string ToString()
	{
		return string.Format("[{0},{1},{2}]", this.iX, this.iY, this.iZ);
	}
}