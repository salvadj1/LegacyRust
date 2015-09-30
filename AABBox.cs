using System;
using System.Reflection;
using UnityEngine;

public struct AABBox : IEquatable<AABBox>
{
	public const int kX = 2;

	public const int kY = 4;

	public const int kZ = 1;

	public const int kA = 0;

	public const int kB = 1;

	public const int kC = 2;

	public const int kD = 3;

	public const int kE = 4;

	public const int kF = 5;

	public const int kG = 6;

	public const int kH = 7;

	public Vector3 m;

	public Vector3 M;

	public Vector3 a
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = this.m.x;
			vector3.y = this.m.y;
			vector3.z = this.m.z;
			return vector3;
		}
	}

	public Vector3 b
	{
		get
		{
			Vector3 m = new Vector3();
			m.x = this.m.x;
			m.y = this.m.y;
			m.z = this.M.z;
			return m;
		}
	}

	public Vector3 c
	{
		get
		{
			Vector3 m = new Vector3();
			m.x = this.M.x;
			m.y = this.m.y;
			m.z = this.m.z;
			return m;
		}
	}

	public Vector3 center
	{
		get
		{
			Vector3 m = new Vector3();
			m.x = this.m.x + (this.M.x - this.m.x) * 0.5f;
			m.y = this.m.y + (this.M.y - this.m.y) * 0.5f;
			m.z = this.m.z + (this.M.z - this.m.z) * 0.5f;
			return m;
		}
		set
		{
			float single = value.x - (this.m.x + (this.M.x - this.m.x) * 0.5f);
			this.m.x = this.m.x + single;
			this.M.x = this.M.x + single;
			single = value.y - (this.m.y + (this.M.y - this.m.y) * 0.5f);
			this.m.y = this.m.y + single;
			this.M.y = this.M.y + single;
			single = value.z - (this.m.z + (this.M.z - this.m.z) * 0.5f);
			this.m.z = this.m.z + single;
			this.M.z = this.M.z + single;
		}
	}

	public Vector3 d
	{
		get
		{
			Vector3 m = new Vector3();
			m.x = this.M.x;
			m.y = this.m.y;
			m.z = this.M.z;
			return m;
		}
	}

	public Vector3 e
	{
		get
		{
			Vector3 m = new Vector3();
			m.x = this.m.x;
			m.y = this.M.y;
			m.z = this.m.z;
			return m;
		}
	}

	public bool empty
	{
		get
		{
			return (this.m.x != this.M.x || this.m.y != this.M.y ? false : this.m.z == this.M.z);
		}
	}

	public Vector3 f
	{
		get
		{
			Vector3 m = new Vector3();
			m.x = this.m.x;
			m.y = this.M.y;
			m.z = this.M.z;
			return m;
		}
	}

	public Vector3 g
	{
		get
		{
			Vector3 m = new Vector3();
			m.x = this.M.x;
			m.y = this.M.y;
			m.z = this.m.z;
			return m;
		}
	}

	public Vector3 h
	{
		get
		{
			Vector3 m = new Vector3();
			m.x = this.M.x;
			m.y = this.M.y;
			m.z = this.M.z;
			return m;
		}
	}

	public Vector3 this[int corner]
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = ((corner & 2) != 2 ? this.m.x : this.M.x);
			vector3.y = ((corner & 4) != 4 ? this.m.y : this.M.y);
			vector3.z = ((corner & 1) != 1 ? this.m.z : this.M.z);
			return vector3;
		}
	}

	public float this[int corner, int axis]
	{
		get
		{
			switch (axis)
			{
				case 0:
				{
					return ((corner & 2) != 2 ? this.m.x : this.M.x);
				}
				case 1:
				{
					return ((corner & 4) != 4 ? this.m.y : this.M.y);
				}
				case 2:
				{
					return ((corner & 1) != 1 ? this.m.z : this.M.z);
				}
			}
			throw new ArgumentOutOfRangeException("axis", (object)axis, "axis<0||axis>2");
		}
	}

	public Vector3 line00
	{
		get
		{
			return this.a;
		}
	}

	public Vector3 line01
	{
		get
		{
			return this.b;
		}
	}

	public Vector3 line10
	{
		get
		{
			return this.a;
		}
	}

	public Vector3 line11
	{
		get
		{
			return this.c;
		}
	}

	public Vector3 line20
	{
		get
		{
			return this.a;
		}
	}

	public Vector3 line21
	{
		get
		{
			return this.e;
		}
	}

	public Vector3 line30
	{
		get
		{
			return this.b;
		}
	}

	public Vector3 line31
	{
		get
		{
			return this.d;
		}
	}

	public Vector3 line40
	{
		get
		{
			return this.b;
		}
	}

	public Vector3 line41
	{
		get
		{
			return this.f;
		}
	}

	public Vector3 line50
	{
		get
		{
			return this.c;
		}
	}

	public Vector3 line51
	{
		get
		{
			return this.d;
		}
	}

	public Vector3 line60
	{
		get
		{
			return this.c;
		}
	}

	public Vector3 line61
	{
		get
		{
			return this.g;
		}
	}

	public Vector3 line70
	{
		get
		{
			return this.d;
		}
	}

	public Vector3 line71
	{
		get
		{
			return this.h;
		}
	}

	public Vector3 line80
	{
		get
		{
			return this.e;
		}
	}

	public Vector3 line81
	{
		get
		{
			return this.f;
		}
	}

	public Vector3 line90
	{
		get
		{
			return this.e;
		}
	}

	public Vector3 line91
	{
		get
		{
			return this.g;
		}
	}

	public Vector3 lineA0
	{
		get
		{
			return this.f;
		}
	}

	public Vector3 lineA1
	{
		get
		{
			return this.h;
		}
	}

	public Vector3 lineB0
	{
		get
		{
			return this.g;
		}
	}

	public Vector3 lineB1
	{
		get
		{
			return this.h;
		}
	}

	public Vector3 max
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = (this.m.x <= this.M.x ? this.M.x : this.m.x);
			vector3.y = (this.m.y <= this.M.y ? this.M.y : this.m.y);
			vector3.z = (this.m.z <= this.M.z ? this.M.z : this.m.z);
			return vector3;
		}
	}

	public Vector3 min
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = (this.M.x >= this.m.x ? this.m.x : this.M.x);
			vector3.y = (this.M.y >= this.m.y ? this.m.y : this.M.y);
			vector3.z = (this.M.z >= this.m.z ? this.m.z : this.M.z);
			return vector3;
		}
	}

	public Vector3 size
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = (this.M.x >= this.m.x ? this.M.x - this.m.x : this.m.x - this.M.x);
			vector3.y = (this.M.y >= this.m.y ? this.M.y - this.m.y : this.m.y - this.M.y);
			vector3.z = (this.M.z >= this.m.z ? this.M.z - this.m.z : this.m.z - this.M.z);
			return vector3;
		}
		set
		{
			Vector3 m = new Vector3();
			m.x = this.m.x + (this.M.x - this.m.x) * 0.5f;
			m.y = this.m.y + (this.M.y - this.m.y) * 0.5f;
			m.z = this.m.z + (this.M.z - this.m.z) * 0.5f;
			if (value.x >= 0f)
			{
				value.x = value.x * 0.5f;
			}
			else
			{
				value.x = value.x * -0.5f;
			}
			this.m.x = m.x - value.x;
			this.M.x = m.x + value.x;
			if (value.y >= 0f)
			{
				value.y = value.y * 0.5f;
			}
			else
			{
				value.y = value.y * -0.5f;
			}
			this.m.y = m.y - value.y;
			this.M.y = m.y + value.y;
			if (value.z >= 0f)
			{
				value.z = value.z * 0.5f;
			}
			else
			{
				value.z = value.z * -0.5f;
			}
			this.m.z = m.z - value.z;
			this.M.z = m.z + value.z;
		}
	}

	public float surfaceArea
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = (this.M.x >= this.m.x ? this.M.x - this.m.x : this.m.x - this.M.x);
			vector3.y = (this.M.y >= this.m.y ? this.M.y - this.m.y : this.m.y - this.M.y);
			vector3.z = (this.M.z >= this.m.z ? this.M.z - this.m.z : this.m.z - this.M.z);
			return 2f * vector3.x * vector3.y + 2f * vector3.y * vector3.z + 2f * vector3.x * vector3.z;
		}
	}

	public float volume
	{
		get
		{
			if (this.M.x == this.m.x || this.M.y == this.m.y || this.M.z == this.m.z)
			{
				return 0f;
			}
			if (this.M.x < this.m.x)
			{
				if (this.M.y < this.m.y)
				{
					if (this.M.z < this.m.z)
					{
						return (this.m.x - this.M.x) * (this.m.y - this.M.y) * (this.m.z - this.M.z);
					}
					return (this.m.x - this.M.x) * (this.m.y - this.M.y) * (this.M.z - this.m.z);
				}
				if (this.M.z < this.m.z)
				{
					return (this.m.x - this.M.x) * (this.M.y - this.m.y) * (this.m.z - this.M.z);
				}
				return (this.m.x - this.M.x) * (this.M.y - this.m.y) * (this.M.z - this.m.z);
			}
			if (this.M.y < this.m.y)
			{
				if (this.M.z < this.m.z)
				{
					return (this.M.x - this.m.x) * (this.m.y - this.M.y) * (this.m.z - this.M.z);
				}
				return (this.M.x - this.m.x) * (this.m.y - this.M.y) * (this.M.z - this.m.z);
			}
			if (this.M.z < this.m.z)
			{
				return (this.M.x - this.m.x) * (this.M.y - this.m.y) * (this.m.z - this.M.z);
			}
			return (this.M.x - this.m.x) * (this.M.y - this.m.y) * (this.M.z - this.m.z);
		}
	}

	public AABBox(Vector3 min, Vector3 max)
	{
		if (min.x <= max.x)
		{
			this.m.x = min.x;
			this.M.x = max.x;
		}
		else
		{
			this.m.x = max.x;
			this.M.x = min.x;
		}
		if (min.y <= max.y)
		{
			this.m.y = min.y;
			this.M.y = max.y;
		}
		else
		{
			this.m.y = max.y;
			this.M.y = min.y;
		}
		if (min.z <= max.z)
		{
			this.m.z = min.z;
			this.M.z = max.z;
		}
		else
		{
			this.m.z = max.z;
			this.M.z = min.z;
		}
	}

	public AABBox(ref Vector3 min, ref Vector3 max)
	{
		if (min.x <= max.x)
		{
			this.m.x = min.x;
			this.M.x = max.x;
		}
		else
		{
			this.m.x = max.x;
			this.M.x = min.x;
		}
		if (min.y <= max.y)
		{
			this.m.y = min.y;
			this.M.y = max.y;
		}
		else
		{
			this.m.y = max.y;
			this.M.y = min.y;
		}
		if (min.z <= max.z)
		{
			this.m.z = min.z;
			this.M.z = max.z;
		}
		else
		{
			this.m.z = max.z;
			this.M.z = min.z;
		}
	}

	public AABBox(ref Vector3 center)
	{
		float single = center.x;
		float single1 = single;
		this.M.x = single;
		this.m.x = single1;
		float single2 = center.y;
		single1 = single2;
		this.M.y = single2;
		this.m.y = single1;
		float single3 = center.z;
		single1 = single3;
		this.M.z = single3;
		this.m.z = single1;
	}

	public AABBox(Vector3 center)
	{
		float single = center.x;
		float single1 = single;
		this.M.x = single;
		this.m.x = single1;
		float single2 = center.y;
		single1 = single2;
		this.M.y = single2;
		this.m.y = single1;
		float single3 = center.z;
		single1 = single3;
		this.M.z = single3;
		this.m.z = single1;
	}

	public AABBox(Bounds bounds) : this(bounds.min, bounds.max)
	{
	}

	public AABBox(ref Bounds bounds) : this(bounds.min, bounds.max)
	{
	}

	public static AABBox CenterAndSize(Vector3 center, Vector3 size)
	{
		center.x = center.x - size.x * 0.5f;
		center.y = center.y - size.y * 0.5f;
		center.z = center.z - size.z * 0.5f;
		size.x = center.x + size.x;
		size.y = center.y + size.y;
		size.z = center.z + size.z;
		return new AABBox(ref center, ref size);
	}

	public bool Contains(ref Vector3 v)
	{
		return (this.m.x > this.M.x || this.m.y > this.M.y || this.m.z > this.M.z || v.x < this.m.x || v.y < this.m.y || v.z < this.m.z || v.x > this.M.x || v.y > this.M.y ? false : v.z <= this.M.z);
	}

	public void Encapsulate(ref Vector3 v)
	{
		if (v.x < this.m.x)
		{
			this.m.x = v.x;
		}
		if (v.x > this.M.x)
		{
			this.M.x = v.x;
		}
		if (v.y < this.m.y)
		{
			this.m.y = v.y;
		}
		if (v.y > this.M.y)
		{
			this.M.y = v.y;
		}
		if (v.z < this.m.z)
		{
			this.m.z = v.z;
		}
		if (v.z > this.M.z)
		{
			this.M.z = v.z;
		}
	}

	public void Encapsulate(Vector3 v)
	{
		if (v.x < this.m.x)
		{
			this.m.x = v.x;
		}
		if (v.x > this.M.x)
		{
			this.M.x = v.x;
		}
		if (v.y < this.m.y)
		{
			this.m.y = v.y;
		}
		if (v.y > this.M.y)
		{
			this.M.y = v.y;
		}
		if (v.z < this.m.z)
		{
			this.m.z = v.z;
		}
		if (v.z > this.M.z)
		{
			this.M.z = v.z;
		}
	}

	public void Encapsulate(ref AABBox v)
	{
		if (v.M.x >= v.m.x)
		{
			if (v.m.x < this.m.x)
			{
				this.m.x = v.m.x;
			}
			if (v.M.x > this.M.x)
			{
				this.M.x = v.M.x;
			}
		}
		else
		{
			if (v.M.x < this.m.x)
			{
				this.m.x = v.M.x;
			}
			if (v.m.x > this.M.x)
			{
				this.M.x = v.m.x;
			}
		}
		if (v.M.y >= v.m.y)
		{
			if (v.m.y < this.m.y)
			{
				this.m.y = v.m.y;
			}
			if (v.M.y > this.M.y)
			{
				this.M.y = v.M.y;
			}
		}
		else
		{
			if (v.M.y < this.m.y)
			{
				this.m.y = v.M.y;
			}
			if (v.m.y > this.M.y)
			{
				this.M.y = v.m.y;
			}
		}
		if (v.M.z >= v.m.z)
		{
			if (v.m.z < this.m.z)
			{
				this.m.z = v.m.z;
			}
			if (v.M.z > this.M.z)
			{
				this.M.z = v.M.z;
			}
		}
		else
		{
			if (v.M.z < this.m.z)
			{
				this.m.z = v.M.z;
			}
			if (v.m.z > this.M.z)
			{
				this.M.z = v.m.z;
			}
		}
	}

	public void Encapsulate(AABBox v)
	{
		if (v.M.x >= v.m.x)
		{
			if (v.m.x < this.m.x)
			{
				this.m.x = v.m.x;
			}
			if (v.M.x > this.M.x)
			{
				this.M.x = v.M.x;
			}
		}
		else
		{
			if (v.M.x < this.m.x)
			{
				this.m.x = v.M.x;
			}
			if (v.m.x > this.M.x)
			{
				this.M.x = v.m.x;
			}
		}
		if (v.M.y >= v.m.y)
		{
			if (v.m.y < this.m.y)
			{
				this.m.y = v.m.y;
			}
			if (v.M.y > this.M.y)
			{
				this.M.y = v.M.y;
			}
		}
		else
		{
			if (v.M.y < this.m.y)
			{
				this.m.y = v.M.y;
			}
			if (v.m.y > this.M.y)
			{
				this.M.y = v.m.y;
			}
		}
		if (v.M.z >= v.m.z)
		{
			if (v.m.z < this.m.z)
			{
				this.m.z = v.m.z;
			}
			if (v.M.z > this.M.z)
			{
				this.M.z = v.M.z;
			}
		}
		else
		{
			if (v.M.z < this.m.z)
			{
				this.m.z = v.M.z;
			}
			if (v.m.z > this.M.z)
			{
				this.M.z = v.m.z;
			}
		}
	}

	public void Encapsulate(ref Vector3 min, ref Vector3 max)
	{
		if (max.x >= min.x)
		{
			if (min.x < this.m.x)
			{
				this.m.x = min.x;
			}
			if (max.x > this.M.x)
			{
				this.M.x = max.x;
			}
		}
		else
		{
			if (max.x < this.m.x)
			{
				this.m.x = max.x;
			}
			if (min.x > this.M.x)
			{
				this.M.x = min.x;
			}
		}
		if (max.y >= min.y)
		{
			if (min.y < this.m.y)
			{
				this.m.y = min.y;
			}
			if (max.y > this.M.y)
			{
				this.M.y = max.y;
			}
		}
		else
		{
			if (max.y < this.m.y)
			{
				this.m.y = max.y;
			}
			if (min.y > this.M.y)
			{
				this.M.y = min.y;
			}
		}
		if (max.z >= min.z)
		{
			if (min.z < this.m.z)
			{
				this.m.z = min.z;
			}
			if (max.z > this.M.z)
			{
				this.M.z = max.z;
			}
		}
		else
		{
			if (max.z < this.m.z)
			{
				this.m.z = max.z;
			}
			if (min.z > this.M.z)
			{
				this.M.z = min.z;
			}
		}
	}

	public void Encapsulate(Vector3 min, ref Vector3 max)
	{
		if (max.x >= min.x)
		{
			if (min.x < this.m.x)
			{
				this.m.x = min.x;
			}
			if (max.x > this.M.x)
			{
				this.M.x = max.x;
			}
		}
		else
		{
			if (max.x < this.m.x)
			{
				this.m.x = max.x;
			}
			if (min.x > this.M.x)
			{
				this.M.x = min.x;
			}
		}
		if (max.y >= min.y)
		{
			if (min.y < this.m.y)
			{
				this.m.y = min.y;
			}
			if (max.y > this.M.y)
			{
				this.M.y = max.y;
			}
		}
		else
		{
			if (max.y < this.m.y)
			{
				this.m.y = max.y;
			}
			if (min.y > this.M.y)
			{
				this.M.y = min.y;
			}
		}
		if (max.z >= min.z)
		{
			if (min.z < this.m.z)
			{
				this.m.z = min.z;
			}
			if (max.z > this.M.z)
			{
				this.M.z = max.z;
			}
		}
		else
		{
			if (max.z < this.m.z)
			{
				this.m.z = max.z;
			}
			if (min.z > this.M.z)
			{
				this.M.z = min.z;
			}
		}
	}

	public void Encapsulate(ref Vector3 min, Vector3 max)
	{
		if (max.x >= min.x)
		{
			if (min.x < this.m.x)
			{
				this.m.x = min.x;
			}
			if (max.x > this.M.x)
			{
				this.M.x = max.x;
			}
		}
		else
		{
			if (max.x < this.m.x)
			{
				this.m.x = max.x;
			}
			if (min.x > this.M.x)
			{
				this.M.x = min.x;
			}
		}
		if (max.y >= min.y)
		{
			if (min.y < this.m.y)
			{
				this.m.y = min.y;
			}
			if (max.y > this.M.y)
			{
				this.M.y = max.y;
			}
		}
		else
		{
			if (max.y < this.m.y)
			{
				this.m.y = max.y;
			}
			if (min.y > this.M.y)
			{
				this.M.y = min.y;
			}
		}
		if (max.z >= min.z)
		{
			if (min.z < this.m.z)
			{
				this.m.z = min.z;
			}
			if (max.z > this.M.z)
			{
				this.M.z = max.z;
			}
		}
		else
		{
			if (max.z < this.m.z)
			{
				this.m.z = max.z;
			}
			if (min.z > this.M.z)
			{
				this.M.z = min.z;
			}
		}
	}

	public void Encapsulate(Vector3 min, Vector3 max)
	{
		if (max.x >= min.x)
		{
			if (min.x < this.m.x)
			{
				this.m.x = min.x;
			}
			if (max.x > this.M.x)
			{
				this.M.x = max.x;
			}
		}
		else
		{
			if (max.x < this.m.x)
			{
				this.m.x = max.x;
			}
			if (min.x > this.M.x)
			{
				this.M.x = min.x;
			}
		}
		if (max.y >= min.y)
		{
			if (min.y < this.m.y)
			{
				this.m.y = min.y;
			}
			if (max.y > this.M.y)
			{
				this.M.y = max.y;
			}
		}
		else
		{
			if (max.y < this.m.y)
			{
				this.m.y = max.y;
			}
			if (min.y > this.M.y)
			{
				this.M.y = min.y;
			}
		}
		if (max.z >= min.z)
		{
			if (min.z < this.m.z)
			{
				this.m.z = min.z;
			}
			if (max.z > this.M.z)
			{
				this.M.z = max.z;
			}
		}
		else
		{
			if (max.z < this.m.z)
			{
				this.m.z = max.z;
			}
			if (min.z > this.M.z)
			{
				this.M.z = min.z;
			}
		}
	}

	public void Encapsulate(ref Bounds bounds)
	{
		Vector3 vector3 = bounds.min;
		Vector3 vector31 = bounds.max;
		if (vector31.x >= vector3.x)
		{
			if (vector3.x < this.m.x)
			{
				this.m.x = vector3.x;
			}
			if (vector31.x > this.M.x)
			{
				this.M.x = vector31.x;
			}
		}
		else
		{
			if (vector31.x < this.m.x)
			{
				this.m.x = vector31.x;
			}
			if (vector3.x > this.M.x)
			{
				this.M.x = vector3.x;
			}
		}
		if (vector31.y >= vector3.y)
		{
			if (vector3.y < this.m.y)
			{
				this.m.y = vector3.y;
			}
			if (vector31.y > this.M.y)
			{
				this.M.y = vector31.y;
			}
		}
		else
		{
			if (vector31.y < this.m.y)
			{
				this.m.y = vector31.y;
			}
			if (vector3.y > this.M.y)
			{
				this.M.y = vector3.y;
			}
		}
		if (vector31.z >= vector3.z)
		{
			if (vector3.z < this.m.z)
			{
				this.m.z = vector3.z;
			}
			if (vector31.z > this.M.z)
			{
				this.M.z = vector31.z;
			}
		}
		else
		{
			if (vector31.z < this.m.z)
			{
				this.m.z = vector31.z;
			}
			if (vector3.z > this.M.z)
			{
				this.M.z = vector3.z;
			}
		}
	}

	public void Encapsulate(Bounds bounds)
	{
		Vector3 vector3 = bounds.min;
		Vector3 vector31 = bounds.max;
		if (vector31.x >= vector3.x)
		{
			if (vector3.x < this.m.x)
			{
				this.m.x = vector3.x;
			}
			if (vector31.x > this.M.x)
			{
				this.M.x = vector31.x;
			}
		}
		else
		{
			if (vector31.x < this.m.x)
			{
				this.m.x = vector31.x;
			}
			if (vector3.x > this.M.x)
			{
				this.M.x = vector3.x;
			}
		}
		if (vector31.y >= vector3.y)
		{
			if (vector3.y < this.m.y)
			{
				this.m.y = vector3.y;
			}
			if (vector31.y > this.M.y)
			{
				this.M.y = vector31.y;
			}
		}
		else
		{
			if (vector31.y < this.m.y)
			{
				this.m.y = vector31.y;
			}
			if (vector3.y > this.M.y)
			{
				this.M.y = vector3.y;
			}
		}
		if (vector31.z >= vector3.z)
		{
			if (vector3.z < this.m.z)
			{
				this.m.z = vector3.z;
			}
			if (vector31.z > this.M.z)
			{
				this.M.z = vector31.z;
			}
		}
		else
		{
			if (vector31.z < this.m.z)
			{
				this.m.z = vector31.z;
			}
			if (vector3.z > this.M.z)
			{
				this.M.z = vector3.z;
			}
		}
	}

	public void EnsureMinMax()
	{
		if (this.m.x > this.M.x)
		{
			float single = this.m.x;
			this.m.x = this.M.x;
			this.M.x = single;
		}
		if (this.m.y > this.M.y)
		{
			float single1 = this.m.y;
			this.m.y = this.M.y;
			this.M.y = single1;
		}
		if (this.m.z > this.M.z)
		{
			float single2 = this.m.z;
			this.m.z = this.M.z;
			this.M.z = single2;
		}
	}

	public override bool Equals(object obj)
	{
		return (!(obj is AABBox) ? false : this.Equals((AABBox)obj));
	}

	public bool Equals(AABBox other)
	{
		return (!this.m.x.Equals(other.m.x) || !this.m.y.Equals(other.m.y) || !this.m.z.Equals(other.m.z) || !this.M.x.Equals(other.M.x) || !this.M.y.Equals(other.M.y) ? false : this.M.z.Equals(other.M.z));
	}

	public bool Equals(ref AABBox other)
	{
		return (!this.m.x.Equals(other.m.x) || !this.m.y.Equals(other.m.y) || !this.m.z.Equals(other.m.z) || !this.M.x.Equals(other.M.x) || !this.M.y.Equals(other.M.y) ? false : this.M.z.Equals(other.M.z));
	}

	public override int GetHashCode()
	{
		float m = (this.m.x + this.M.x) * 0.5f;
		float single = (this.m.y + this.M.y) * 0.5f;
		int hashCode = m.GetHashCode() ^ single.GetHashCode();
		float m1 = this.m.x + this.M.x - (this.m.y + this.M.y);
		int num = (m1.GetHashCode() & 2147483647) % 32;
		return hashCode << (num & 31) ^ hashCode >> (num & 31);
	}

	public static explicit operator Bounds(AABBox mM)
	{
		Vector3 m = new Vector3();
		Vector3 vector3 = new Vector3();
		m.x = mM.M.x - mM.m.x;
		vector3.x = mM.m.x + m.x * 0.5f;
		if (m.x < 0f)
		{
			m.x = -m.x;
		}
		m.y = mM.M.y - mM.m.y;
		vector3.y = mM.m.y + m.y * 0.5f;
		if (m.y < 0f)
		{
			m.y = -m.y;
		}
		m.z = mM.M.z - mM.m.z;
		vector3.z = mM.m.z + m.z * 0.5f;
		if (m.z < 0f)
		{
			m.z = -m.z;
		}
		return new Bounds(vector3, m);
	}

	public static explicit operator AABBox(Bounds bounds)
	{
		AABBox aABBox = new AABBox();
		Vector3 vector3 = bounds.min;
		Vector3 vector31 = bounds.max;
		if (vector3.x <= vector31.x)
		{
			aABBox.M.x = vector31.x;
			aABBox.m.x = vector3.x;
		}
		else
		{
			aABBox.M.x = vector3.x;
			aABBox.m.x = vector31.x;
		}
		if (vector3.y <= vector31.y)
		{
			aABBox.M.y = vector31.y;
			aABBox.m.y = vector3.y;
		}
		else
		{
			aABBox.M.y = vector3.y;
			aABBox.m.y = vector31.y;
		}
		if (vector3.z <= vector31.z)
		{
			aABBox.M.z = vector31.z;
			aABBox.m.z = vector3.z;
		}
		else
		{
			aABBox.M.z = vector3.z;
			aABBox.m.z = vector31.z;
		}
		return aABBox;
	}

	public static explicit operator BBox(AABBox mM)
	{
		BBox m = new BBox();
		m.a.x = mM.m.x;
		m.a.y = mM.m.y;
		m.a.z = mM.m.z;
		m.b.x = mM.m.x;
		m.b.y = mM.m.y;
		m.b.z = mM.M.z;
		m.c.x = mM.M.x;
		m.c.y = mM.m.y;
		m.c.z = mM.m.z;
		m.d.x = mM.M.x;
		m.d.y = mM.m.y;
		m.d.z = mM.M.z;
		m.e.x = mM.m.x;
		m.e.y = mM.M.y;
		m.e.z = mM.m.z;
		m.f.x = mM.m.x;
		m.f.y = mM.M.y;
		m.f.z = mM.M.z;
		m.g.x = mM.M.x;
		m.g.y = mM.M.y;
		m.g.z = mM.m.z;
		m.h.x = mM.M.x;
		m.h.y = mM.M.y;
		m.h.z = mM.M.z;
		return m;
	}

	public static explicit operator AABBox(BBox box)
	{
		AABBox aABBox = new AABBox();
		float single = box.a.x;
		float single1 = single;
		aABBox.M.x = single;
		aABBox.m.x = single1;
		float single2 = box.a.y;
		single1 = single2;
		aABBox.M.y = single2;
		aABBox.m.y = single1;
		float single3 = box.a.z;
		single1 = single3;
		aABBox.M.z = single3;
		aABBox.m.z = single1;
		if (box.b.x < aABBox.m.x)
		{
			aABBox.m.x = box.b.x;
		}
		if (box.b.x > aABBox.M.x)
		{
			aABBox.M.x = box.b.x;
		}
		if (box.b.y < aABBox.m.y)
		{
			aABBox.m.y = box.b.y;
		}
		if (box.b.y > aABBox.M.y)
		{
			aABBox.M.y = box.b.y;
		}
		if (box.b.z < aABBox.m.z)
		{
			aABBox.m.z = box.b.z;
		}
		if (box.b.z > aABBox.M.z)
		{
			aABBox.M.z = box.b.z;
		}
		if (box.c.x < aABBox.m.x)
		{
			aABBox.m.x = box.c.x;
		}
		if (box.c.x > aABBox.M.x)
		{
			aABBox.M.x = box.c.x;
		}
		if (box.c.y < aABBox.m.y)
		{
			aABBox.m.y = box.c.y;
		}
		if (box.c.y > aABBox.M.y)
		{
			aABBox.M.y = box.c.y;
		}
		if (box.c.z < aABBox.m.z)
		{
			aABBox.m.z = box.c.z;
		}
		if (box.c.z > aABBox.M.z)
		{
			aABBox.M.z = box.c.z;
		}
		if (box.d.x < aABBox.m.x)
		{
			aABBox.m.x = box.d.x;
		}
		if (box.d.x > aABBox.M.x)
		{
			aABBox.M.x = box.d.x;
		}
		if (box.d.y < aABBox.m.y)
		{
			aABBox.m.y = box.d.y;
		}
		if (box.d.y > aABBox.M.y)
		{
			aABBox.M.y = box.d.y;
		}
		if (box.d.z < aABBox.m.z)
		{
			aABBox.m.z = box.d.z;
		}
		if (box.d.z > aABBox.M.z)
		{
			aABBox.M.z = box.d.z;
		}
		if (box.e.x < aABBox.m.x)
		{
			aABBox.m.x = box.e.x;
		}
		if (box.e.x > aABBox.M.x)
		{
			aABBox.M.x = box.e.x;
		}
		if (box.e.y < aABBox.m.y)
		{
			aABBox.m.y = box.e.y;
		}
		if (box.e.y > aABBox.M.y)
		{
			aABBox.M.y = box.e.y;
		}
		if (box.e.z < aABBox.m.z)
		{
			aABBox.m.z = box.e.z;
		}
		if (box.e.z > aABBox.M.z)
		{
			aABBox.M.z = box.e.z;
		}
		if (box.f.x < aABBox.m.x)
		{
			aABBox.m.x = box.f.x;
		}
		if (box.f.x > aABBox.M.x)
		{
			aABBox.M.x = box.f.x;
		}
		if (box.f.y < aABBox.m.y)
		{
			aABBox.m.y = box.f.y;
		}
		if (box.f.y > aABBox.M.y)
		{
			aABBox.M.y = box.f.y;
		}
		if (box.f.z < aABBox.m.z)
		{
			aABBox.m.z = box.f.z;
		}
		if (box.f.z > aABBox.M.z)
		{
			aABBox.M.z = box.f.z;
		}
		if (box.g.x < aABBox.m.x)
		{
			aABBox.m.x = box.g.x;
		}
		if (box.g.x > aABBox.M.x)
		{
			aABBox.M.x = box.g.x;
		}
		if (box.g.y < aABBox.m.y)
		{
			aABBox.m.y = box.g.y;
		}
		if (box.g.y > aABBox.M.y)
		{
			aABBox.M.y = box.g.y;
		}
		if (box.g.z < aABBox.m.z)
		{
			aABBox.m.z = box.g.z;
		}
		if (box.g.z > aABBox.M.z)
		{
			aABBox.M.z = box.g.z;
		}
		if (box.h.x < aABBox.m.x)
		{
			aABBox.m.x = box.h.x;
		}
		if (box.h.x > aABBox.M.x)
		{
			aABBox.M.x = box.h.x;
		}
		if (box.h.y < aABBox.m.y)
		{
			aABBox.m.y = box.h.y;
		}
		if (box.h.y > aABBox.M.y)
		{
			aABBox.M.y = box.h.y;
		}
		if (box.h.z < aABBox.m.z)
		{
			aABBox.m.z = box.h.z;
		}
		if (box.h.z > aABBox.M.z)
		{
			aABBox.M.z = box.h.z;
		}
		return aABBox;
	}

	public void SetMinMax(ref Vector3 min, ref Vector3 max)
	{
		if (min.x <= max.x)
		{
			this.m.x = min.x;
			this.M.x = max.x;
		}
		else
		{
			this.m.x = max.x;
			this.M.x = min.x;
		}
		if (min.y <= max.y)
		{
			this.m.y = min.y;
			this.M.y = max.y;
		}
		else
		{
			this.m.y = max.y;
			this.M.y = min.y;
		}
		if (min.z <= max.z)
		{
			this.m.z = min.z;
			this.M.z = max.z;
		}
		else
		{
			this.m.z = max.z;
			this.M.z = min.z;
		}
	}

	public void SetMinMax(ref Vector3 min, Vector3 max)
	{
		if (min.x <= max.x)
		{
			this.m.x = min.x;
			this.M.x = max.x;
		}
		else
		{
			this.m.x = max.x;
			this.M.x = min.x;
		}
		if (min.y <= max.y)
		{
			this.m.y = min.y;
			this.M.y = max.y;
		}
		else
		{
			this.m.y = max.y;
			this.M.y = min.y;
		}
		if (min.z <= max.z)
		{
			this.m.z = min.z;
			this.M.z = max.z;
		}
		else
		{
			this.m.z = max.z;
			this.M.z = min.z;
		}
	}

	public void SetMinMax(Vector3 min, ref Vector3 max)
	{
		if (min.x <= max.x)
		{
			this.m.x = min.x;
			this.M.x = max.x;
		}
		else
		{
			this.m.x = max.x;
			this.M.x = min.x;
		}
		if (min.y <= max.y)
		{
			this.m.y = min.y;
			this.M.y = max.y;
		}
		else
		{
			this.m.y = max.y;
			this.M.y = min.y;
		}
		if (min.z <= max.z)
		{
			this.m.z = min.z;
			this.M.z = max.z;
		}
		else
		{
			this.m.z = max.z;
			this.M.z = min.z;
		}
	}

	public void SetMinMax(Vector3 min, Vector3 max)
	{
		if (min.x <= max.x)
		{
			this.m.x = min.x;
			this.M.x = max.x;
		}
		else
		{
			this.m.x = max.x;
			this.M.x = min.x;
		}
		if (min.y <= max.y)
		{
			this.m.y = min.y;
			this.M.y = max.y;
		}
		else
		{
			this.m.y = max.y;
			this.M.y = min.y;
		}
		if (min.z <= max.z)
		{
			this.m.z = min.z;
			this.M.z = max.z;
		}
		else
		{
			this.m.z = max.z;
			this.M.z = min.z;
		}
	}

	public void SetMinMax(Bounds bounds)
	{
		Vector3 vector3 = bounds.min;
		Vector3 vector31 = bounds.max;
		if (vector3.x <= vector31.x)
		{
			this.m.x = vector3.x;
			this.M.x = vector31.x;
		}
		else
		{
			this.m.x = vector31.x;
			this.M.x = vector3.x;
		}
		if (vector3.y <= vector31.y)
		{
			this.m.y = vector3.y;
			this.M.y = vector31.y;
		}
		else
		{
			this.m.y = vector31.y;
			this.M.y = vector3.y;
		}
		if (vector3.z <= vector31.z)
		{
			this.m.z = vector3.z;
			this.M.z = vector31.z;
		}
		else
		{
			this.m.z = vector31.z;
			this.M.z = vector3.z;
		}
	}

	public void SetMinMax(ref Bounds bounds)
	{
		Vector3 vector3 = bounds.min;
		Vector3 vector31 = bounds.max;
		if (vector3.x <= vector31.x)
		{
			this.m.x = vector3.x;
			this.M.x = vector31.x;
		}
		else
		{
			this.m.x = vector31.x;
			this.M.x = vector3.x;
		}
		if (vector3.y <= vector31.y)
		{
			this.m.y = vector3.y;
			this.M.y = vector31.y;
		}
		else
		{
			this.m.y = vector31.y;
			this.M.y = vector3.y;
		}
		if (vector3.z <= vector31.z)
		{
			this.m.z = vector3.z;
			this.M.z = vector31.z;
		}
		else
		{
			this.m.z = vector31.z;
			this.M.z = vector3.z;
		}
	}

	public BBox ToBBox()
	{
		BBox m = new BBox();
		m.a.x = this.m.x;
		m.a.y = this.m.y;
		m.a.z = this.m.z;
		m.b.x = this.m.x;
		m.b.y = this.m.y;
		m.b.z = this.M.z;
		m.c.x = this.M.x;
		m.c.y = this.m.y;
		m.c.z = this.m.z;
		m.d.x = this.M.x;
		m.d.y = this.m.y;
		m.d.z = this.M.z;
		m.e.x = this.m.x;
		m.e.y = this.M.y;
		m.e.z = this.m.z;
		m.f.x = this.m.x;
		m.f.y = this.M.y;
		m.f.z = this.M.z;
		m.g.x = this.M.x;
		m.g.y = this.M.y;
		m.g.z = this.m.z;
		m.h.x = this.M.x;
		m.h.y = this.M.y;
		m.h.z = this.M.z;
		return m;
	}

	public void ToBBox(out BBox box)
	{
		box = new BBox();
		box.a.x = this.m.x;
		box.a.y = this.m.y;
		box.a.z = this.m.z;
		box.b.x = this.m.x;
		box.b.y = this.m.y;
		box.b.z = this.M.z;
		box.c.x = this.M.x;
		box.c.y = this.m.y;
		box.c.z = this.m.z;
		box.d.x = this.M.x;
		box.d.y = this.m.y;
		box.d.z = this.M.z;
		box.e.x = this.m.x;
		box.e.y = this.M.y;
		box.e.z = this.m.z;
		box.f.x = this.m.x;
		box.f.y = this.M.y;
		box.f.z = this.M.z;
		box.g.x = this.M.x;
		box.g.y = this.M.y;
		box.g.z = this.m.z;
		box.h.x = this.M.x;
		box.h.y = this.M.y;
		box.h.z = this.M.z;
	}

	public void ToBoxCorners3x4(ref Matrix4x4 t, out BBox box, out AABBox mM)
	{
		box = new BBox();
		mM = new AABBox();
		Vector3 m = new Vector3();
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.m.z;
		box.a.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.a.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.a.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		mM.m = box.a;
		mM.M = box.a;
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.M.z;
		box.b.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.b.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.b.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (box.b.x < mM.m.x)
		{
			mM.m.x = box.b.x;
		}
		if (box.b.x > mM.M.x)
		{
			mM.M.x = box.b.x;
		}
		if (box.b.y < mM.m.y)
		{
			mM.m.y = box.b.y;
		}
		if (box.b.y > mM.M.y)
		{
			mM.M.y = box.b.y;
		}
		if (box.b.z < mM.m.z)
		{
			mM.m.z = box.b.z;
		}
		if (box.b.z > mM.M.z)
		{
			mM.M.z = box.b.z;
		}
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.m.z;
		box.c.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.c.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.c.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (box.c.x < mM.m.x)
		{
			mM.m.x = box.c.x;
		}
		if (box.c.x > mM.M.x)
		{
			mM.M.x = box.c.x;
		}
		if (box.c.y < mM.m.y)
		{
			mM.m.y = box.c.y;
		}
		if (box.c.y > mM.M.y)
		{
			mM.M.y = box.c.y;
		}
		if (box.c.z < mM.m.z)
		{
			mM.m.z = box.c.z;
		}
		if (box.c.z > mM.M.z)
		{
			mM.M.z = box.c.z;
		}
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.M.z;
		box.d.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.d.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.d.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (box.d.x < mM.m.x)
		{
			mM.m.x = box.d.x;
		}
		if (box.d.x > mM.M.x)
		{
			mM.M.x = box.d.x;
		}
		if (box.d.y < mM.m.y)
		{
			mM.m.y = box.d.y;
		}
		if (box.d.y > mM.M.y)
		{
			mM.M.y = box.d.y;
		}
		if (box.d.z < mM.m.z)
		{
			mM.m.z = box.d.z;
		}
		if (box.d.z > mM.M.z)
		{
			mM.M.z = box.d.z;
		}
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.m.z;
		box.e.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.e.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.e.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (box.e.x < mM.m.x)
		{
			mM.m.x = box.e.x;
		}
		if (box.e.x > mM.M.x)
		{
			mM.M.x = box.e.x;
		}
		if (box.e.y < mM.m.y)
		{
			mM.m.y = box.e.y;
		}
		if (box.e.y > mM.M.y)
		{
			mM.M.y = box.e.y;
		}
		if (box.e.z < mM.m.z)
		{
			mM.m.z = box.e.z;
		}
		if (box.e.z > mM.M.z)
		{
			mM.M.z = box.e.z;
		}
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.M.z;
		box.f.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.f.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.f.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (box.f.x < mM.m.x)
		{
			mM.m.x = box.f.x;
		}
		if (box.f.x > mM.M.x)
		{
			mM.M.x = box.f.x;
		}
		if (box.f.y < mM.m.y)
		{
			mM.m.y = box.f.y;
		}
		if (box.f.y > mM.M.y)
		{
			mM.M.y = box.f.y;
		}
		if (box.f.z < mM.m.z)
		{
			mM.m.z = box.f.z;
		}
		if (box.f.z > mM.M.z)
		{
			mM.M.z = box.f.z;
		}
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.m.z;
		box.g.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.g.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.g.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (box.g.x < mM.m.x)
		{
			mM.m.x = box.g.x;
		}
		if (box.g.x > mM.M.x)
		{
			mM.M.x = box.g.x;
		}
		if (box.g.y < mM.m.y)
		{
			mM.m.y = box.g.y;
		}
		if (box.g.y > mM.M.y)
		{
			mM.M.y = box.g.y;
		}
		if (box.g.z < mM.m.z)
		{
			mM.m.z = box.g.z;
		}
		if (box.g.z > mM.M.z)
		{
			mM.M.z = box.g.z;
		}
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.M.z;
		box.h.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.h.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.h.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (box.h.x < mM.m.x)
		{
			mM.m.x = box.h.x;
		}
		if (box.h.x > mM.M.x)
		{
			mM.M.x = box.h.x;
		}
		if (box.h.y < mM.m.y)
		{
			mM.m.y = box.h.y;
		}
		if (box.h.y > mM.M.y)
		{
			mM.M.y = box.h.y;
		}
		if (box.h.z < mM.m.z)
		{
			mM.m.z = box.h.z;
		}
		if (box.h.z > mM.M.z)
		{
			mM.M.z = box.h.z;
		}
	}

	public void ToBoxCorners3x4(ref Matrix4x4 t, out BBox box)
	{
		box = new BBox();
		Vector3 m = new Vector3();
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.m.z;
		box.a.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.a.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.a.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.M.z;
		box.b.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.b.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.b.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.m.z;
		box.c.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.c.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.c.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.M.z;
		box.d.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.d.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.d.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.m.z;
		box.e.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.e.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.e.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.M.z;
		box.f.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.f.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.f.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.m.z;
		box.g.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.g.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.g.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.M.z;
		box.h.x = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		box.h.y = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		box.h.z = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
	}

	public void ToBoxCorners4x4(ref Matrix4x4 t, out BBox box, out AABBox mM)
	{
		box = new BBox();
		mM = new AABBox();
		Vector4 m = new Vector4();
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.a.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.a.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.a.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		mM.m = box.a;
		mM.M = box.a;
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.b.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.b.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.b.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (box.b.x < mM.m.x)
		{
			mM.m.x = box.b.x;
		}
		if (box.b.x > mM.M.x)
		{
			mM.M.x = box.b.x;
		}
		if (box.b.y < mM.m.y)
		{
			mM.m.y = box.b.y;
		}
		if (box.b.y > mM.M.y)
		{
			mM.M.y = box.b.y;
		}
		if (box.b.z < mM.m.z)
		{
			mM.m.z = box.b.z;
		}
		if (box.b.z > mM.M.z)
		{
			mM.M.z = box.b.z;
		}
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.c.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.c.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.c.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (box.c.x < mM.m.x)
		{
			mM.m.x = box.c.x;
		}
		if (box.c.x > mM.M.x)
		{
			mM.M.x = box.c.x;
		}
		if (box.c.y < mM.m.y)
		{
			mM.m.y = box.c.y;
		}
		if (box.c.y > mM.M.y)
		{
			mM.M.y = box.c.y;
		}
		if (box.c.z < mM.m.z)
		{
			mM.m.z = box.c.z;
		}
		if (box.c.z > mM.M.z)
		{
			mM.M.z = box.c.z;
		}
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.d.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.d.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.d.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (box.d.x < mM.m.x)
		{
			mM.m.x = box.d.x;
		}
		if (box.d.x > mM.M.x)
		{
			mM.M.x = box.d.x;
		}
		if (box.d.y < mM.m.y)
		{
			mM.m.y = box.d.y;
		}
		if (box.d.y > mM.M.y)
		{
			mM.M.y = box.d.y;
		}
		if (box.d.z < mM.m.z)
		{
			mM.m.z = box.d.z;
		}
		if (box.d.z > mM.M.z)
		{
			mM.M.z = box.d.z;
		}
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.e.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.e.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.e.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (box.e.x < mM.m.x)
		{
			mM.m.x = box.e.x;
		}
		if (box.e.x > mM.M.x)
		{
			mM.M.x = box.e.x;
		}
		if (box.e.y < mM.m.y)
		{
			mM.m.y = box.e.y;
		}
		if (box.e.y > mM.M.y)
		{
			mM.M.y = box.e.y;
		}
		if (box.e.z < mM.m.z)
		{
			mM.m.z = box.e.z;
		}
		if (box.e.z > mM.M.z)
		{
			mM.M.z = box.e.z;
		}
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.f.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.f.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.f.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (box.f.x < mM.m.x)
		{
			mM.m.x = box.f.x;
		}
		if (box.f.x > mM.M.x)
		{
			mM.M.x = box.f.x;
		}
		if (box.f.y < mM.m.y)
		{
			mM.m.y = box.f.y;
		}
		if (box.f.y > mM.M.y)
		{
			mM.M.y = box.f.y;
		}
		if (box.f.z < mM.m.z)
		{
			mM.m.z = box.f.z;
		}
		if (box.f.z > mM.M.z)
		{
			mM.M.z = box.f.z;
		}
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.g.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.g.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.g.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (box.g.x < mM.m.x)
		{
			mM.m.x = box.g.x;
		}
		if (box.g.x > mM.M.x)
		{
			mM.M.x = box.g.x;
		}
		if (box.g.y < mM.m.y)
		{
			mM.m.y = box.g.y;
		}
		if (box.g.y > mM.M.y)
		{
			mM.M.y = box.g.y;
		}
		if (box.g.z < mM.m.z)
		{
			mM.m.z = box.g.z;
		}
		if (box.g.z > mM.M.z)
		{
			mM.M.z = box.g.z;
		}
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.h.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.h.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.h.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (box.h.x < mM.m.x)
		{
			mM.m.x = box.h.x;
		}
		if (box.h.x > mM.M.x)
		{
			mM.M.x = box.h.x;
		}
		if (box.h.y < mM.m.y)
		{
			mM.m.y = box.h.y;
		}
		if (box.h.y > mM.M.y)
		{
			mM.M.y = box.h.y;
		}
		if (box.h.z < mM.m.z)
		{
			mM.m.z = box.h.z;
		}
		if (box.h.z > mM.M.z)
		{
			mM.M.z = box.h.z;
		}
	}

	public void ToBoxCorners4x4(ref Matrix4x4 t, out BBox box)
	{
		box = new BBox();
		Vector4 m = new Vector4();
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.a.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.a.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.a.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.b.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.b.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.b.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.c.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.c.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.c.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.d.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.d.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.d.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.e.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.e.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.e.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.f.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.f.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.f.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.g.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.g.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.g.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		box.h.x = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		box.h.y = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		box.h.z = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
	}

	public static void Transform3x4(ref AABBox src, ref Matrix4x4 transform, out AABBox dst)
	{
		src.TransformedAABB3x4(ref transform, out dst);
	}

	public static void Transform3x4(ref AABBox src, ref Matrix4x4 transform, out Bounds dst)
	{
		src.TransformedAABB3x4(ref transform, out dst);
	}

	public static void Transform3x4(ref Bounds boundsSrc, ref Matrix4x4 transform, out AABBox dst)
	{
		AABBox aABBox = new AABBox(boundsSrc.min, boundsSrc.max);
		AABBox.Transform3x4(ref aABBox, ref transform, out dst);
	}

	public static void Transform3x4(ref Bounds boundsSrc, ref Matrix4x4 transform, out Bounds dst)
	{
		AABBox aABBox = new AABBox(boundsSrc.min, boundsSrc.max);
		AABBox.Transform3x4(ref aABBox, ref transform, out dst);
	}

	public static void Transform3x4(AABBox src, ref Matrix4x4 transform, out AABBox dst)
	{
		src.TransformedAABB3x4(ref transform, out dst);
	}

	public static void Transform3x4(AABBox src, ref Matrix4x4 transform, out Bounds dst)
	{
		src.TransformedAABB3x4(ref transform, out dst);
	}

	public static void Transform3x4(Bounds boundsSrc, ref Matrix4x4 transform, out AABBox dst)
	{
		AABBox aABBox = new AABBox(boundsSrc.min, boundsSrc.max);
		AABBox.Transform3x4(ref aABBox, ref transform, out dst);
	}

	public static void Transform3x4(Bounds boundsSrc, ref Matrix4x4 transform, out Bounds dst)
	{
		AABBox aABBox = new AABBox(boundsSrc.min, boundsSrc.max);
		AABBox.Transform3x4(ref aABBox, ref transform, out dst);
	}

	public static void Transform4x4(ref AABBox src, ref Matrix4x4 transform, out AABBox dst)
	{
		src.TransformedAABB4x4(ref transform, out dst);
	}

	public static void Transform4x4(ref AABBox src, ref Matrix4x4 transform, out Bounds dst)
	{
		src.TransformedAABB4x4(ref transform, out dst);
	}

	public static void Transform4x4(ref Bounds boundsSrc, ref Matrix4x4 transform, out AABBox dst)
	{
		AABBox aABBox = new AABBox(boundsSrc.min, boundsSrc.max);
		AABBox.Transform4x4(ref aABBox, ref transform, out dst);
	}

	public static void Transform4x4(ref Bounds boundsSrc, ref Matrix4x4 transform, out Bounds dst)
	{
		AABBox aABBox = new AABBox(boundsSrc.min, boundsSrc.max);
		AABBox.Transform4x4(ref aABBox, ref transform, out dst);
	}

	public static void Transform4x4(AABBox src, ref Matrix4x4 transform, out AABBox dst)
	{
		src.TransformedAABB4x4(ref transform, out dst);
	}

	public static void Transform4x4(AABBox src, ref Matrix4x4 transform, out Bounds dst)
	{
		src.TransformedAABB4x4(ref transform, out dst);
	}

	public static void Transform4x4(Bounds boundsSrc, ref Matrix4x4 transform, out AABBox dst)
	{
		AABBox aABBox = new AABBox(boundsSrc.min, boundsSrc.max);
		AABBox.Transform4x4(ref aABBox, ref transform, out dst);
	}

	public static void Transform4x4(Bounds boundsSrc, ref Matrix4x4 transform, out Bounds dst)
	{
		AABBox aABBox = new AABBox(boundsSrc.min, boundsSrc.max);
		AABBox.Transform4x4(ref aABBox, ref transform, out dst);
	}

	public void TransformedAABB3x4(ref Matrix4x4 t, out AABBox mM)
	{
		mM = new AABBox();
		Vector3 m = new Vector3();
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.m.z;
		float single = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		float single1 = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		float single2 = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		float single3 = single;
		float single4 = single3;
		mM.M.x = single3;
		mM.m.x = single4;
		float single5 = single1;
		single4 = single5;
		mM.M.y = single5;
		mM.m.y = single4;
		float single6 = single2;
		single4 = single6;
		mM.M.z = single6;
		mM.m.z = single4;
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.M.z;
		single = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		single1 = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		single2 = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.m.z;
		single = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		single1 = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		single2 = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.M.z;
		single = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		single1 = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		single2 = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.m.z;
		single = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		single1 = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		single2 = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.M.z;
		single = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		single1 = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		single2 = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.m.z;
		single = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		single1 = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		single2 = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.M.z;
		single = t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03;
		single1 = t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13;
		single2 = t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
	}

	public void TransformedAABB3x4(ref Matrix4x4 t, out Bounds bounds)
	{
		AABBox aABBox;
		Vector3 m = new Vector3();
		Vector3 vector3 = new Vector3();
		this.TransformedAABB3x4(ref t, out aABBox);
		m.x = aABBox.M.x - aABBox.m.x;
		vector3.x = aABBox.m.x + m.x * 0.5f;
		m.y = aABBox.M.y - aABBox.m.y;
		vector3.y = aABBox.m.y + m.y * 0.5f;
		m.z = aABBox.M.z - aABBox.m.z;
		vector3.z = aABBox.m.z + m.z * 0.5f;
		bounds = new Bounds(vector3, m);
	}

	public void TransformedAABB4x4(ref Matrix4x4 t, out AABBox mM)
	{
		mM = new AABBox();
		Vector4 m = new Vector4();
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		float single = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		float single1 = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		float single2 = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		float single3 = single;
		float single4 = single3;
		mM.M.x = single3;
		mM.m.x = single4;
		float single5 = single1;
		single4 = single5;
		mM.M.y = single5;
		mM.m.y = single4;
		float single6 = single2;
		single4 = single6;
		mM.M.z = single6;
		mM.m.z = single4;
		m.x = this.m.x;
		m.y = this.m.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		single = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		single1 = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		single2 = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		single = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		single1 = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		single2 = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.M.x;
		m.y = this.m.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		single = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		single1 = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		single2 = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		single = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		single1 = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		single2 = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.m.x;
		m.y = this.M.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		single = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		single1 = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		single2 = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.m.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		single = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		single1 = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		single2 = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
		m.x = this.M.x;
		m.y = this.M.y;
		m.z = this.M.z;
		m.w = 1f / (t.m30 * m.x + t.m31 * m.y + t.m32 * m.z + t.m33);
		single = (t.m00 * m.x + t.m01 * m.y + t.m02 * m.z + t.m03) * m.w;
		single1 = (t.m10 * m.x + t.m11 * m.y + t.m12 * m.z + t.m13) * m.w;
		single2 = (t.m20 * m.x + t.m21 * m.y + t.m22 * m.z + t.m23) * m.w;
		if (single < mM.m.x)
		{
			mM.m.x = single;
		}
		if (single > mM.M.x)
		{
			mM.M.x = single;
		}
		if (single1 < mM.m.y)
		{
			mM.m.y = single1;
		}
		if (single1 > mM.M.y)
		{
			mM.M.y = single1;
		}
		if (single2 < mM.m.z)
		{
			mM.m.z = single2;
		}
		if (single2 > mM.M.z)
		{
			mM.M.z = single2;
		}
	}

	public void TransformedAABB4x4(ref Matrix4x4 t, out Bounds bounds)
	{
		AABBox aABBox;
		Vector3 m = new Vector3();
		Vector3 vector3 = new Vector3();
		this.TransformedAABB4x4(ref t, out aABBox);
		m.x = aABBox.M.x - aABBox.m.x;
		vector3.x = aABBox.m.x + m.x * 0.5f;
		m.y = aABBox.M.y - aABBox.m.y;
		vector3.y = aABBox.m.y + m.y * 0.5f;
		m.z = aABBox.M.z - aABBox.m.z;
		vector3.z = aABBox.m.z + m.z * 0.5f;
		bounds = new Bounds(vector3, m);
	}
}