using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NGUI.Structures
{
	[StructLayout(LayoutKind.Explicit)]
	public struct float3
	{
		[FieldOffset(0)]
		public Vector2 xy;

		[FieldOffset(4)]
		public Vector2 yz;

		[FieldOffset(0)]
		public Vector3 xyz;

		[FieldOffset(0)]
		public float x;

		[FieldOffset(4)]
		public float y;

		[FieldOffset(8)]
		public float z;
	}
}