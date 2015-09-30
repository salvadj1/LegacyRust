using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NGUI.Meshing
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Vertex
	{
		[FieldOffset(0)]
		public float x;

		[FieldOffset(4)]
		public float y;

		[FieldOffset(8)]
		public float z;

		[FieldOffset(12)]
		public float u;

		[FieldOffset(16)]
		public float v;

		[FieldOffset(20)]
		public float r;

		[FieldOffset(24)]
		public float g;

		[FieldOffset(28)]
		public float b;

		[FieldOffset(32)]
		public float a;

		public Color color
		{
			get
			{
				Color color = new Color();
				color.r = (float)this.r;
				color.g = (float)this.g;
				color.b = (float)this.b;
				color.a = (float)this.a;
				return color;
			}
			set
			{
				this.r = (float)value.r;
				this.g = (float)value.g;
				this.b = (float)value.b;
				this.a = (float)value.a;
			}
		}

		public Vector3 position
		{
			get
			{
				Vector3 vector3 = new Vector3();
				vector3.x = (float)this.x;
				vector3.y = (float)this.y;
				vector3.z = (float)this.z;
				return vector3;
			}
			set
			{
				this.x = (float)value.x;
				this.y = (float)value.y;
				this.z = (float)value.z;
			}
		}

		public Vector2 texcoord
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = (float)this.u;
				vector2.y = (float)this.v;
				return vector2;
			}
			set
			{
				this.u = (float)value.x;
				this.v = (float)value.y;
			}
		}
	}
}