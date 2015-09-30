using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIPanelMaterialPropertyBlock
{
	private UIPanelMaterialPropertyBlock.Node first;

	private UIPanelMaterialPropertyBlock.Node last;

	private int count;

	private static UIPanelMaterialPropertyBlock.Node dump;

	private static int dumpCount;

	public UIPanelMaterialPropertyBlock()
	{
	}

	public void AddToMaterialPropertyBlock(MaterialPropertyBlock block)
	{
		UIPanelMaterialPropertyBlock.Node node = this.first;
		int num = this.count;
		while (true)
		{
			int num1 = num;
			num = num1 - 1;
			if (num1 <= 0)
			{
				break;
			}
			switch (node.type)
			{
				case UIPanelMaterialPropertyBlock.PropType.Float:
				{
					block.AddFloat(node.property, node.@value.FLOAT);
					break;
				}
				case UIPanelMaterialPropertyBlock.PropType.Vector:
				{
					block.AddVector(node.property, node.@value.VECTOR);
					break;
				}
				case UIPanelMaterialPropertyBlock.PropType.Color:
				{
					block.AddColor(node.property, node.@value.COLOR);
					break;
				}
				case UIPanelMaterialPropertyBlock.PropType.Matrix:
				{
					block.AddMatrix(node.property, node.@value.MATRIX);
					break;
				}
			}
			node = node.next;
		}
	}

	public void Clear()
	{
		if (this.count > 0)
		{
			this.first.prev = UIPanelMaterialPropertyBlock.dump;
			UIPanelMaterialPropertyBlock.dump = this.last;
			if (UIPanelMaterialPropertyBlock.dumpCount > 0)
			{
				this.first.prev.next = this.first;
				this.first.prev.hasNext = true;
				this.first.hasPrev = true;
			}
			object obj = null;
			UIPanelMaterialPropertyBlock.Node node = (UIPanelMaterialPropertyBlock.Node)obj;
			this.last = (UIPanelMaterialPropertyBlock.Node)obj;
			this.first = node;
			UIPanelMaterialPropertyBlock.dumpCount = UIPanelMaterialPropertyBlock.dumpCount + this.count;
			this.count = 0;
		}
	}

	private static UIPanelMaterialPropertyBlock.Node NewNode(UIPanelMaterialPropertyBlock block, int prop, UIPanelMaterialPropertyBlock.PropType type)
	{
		UIPanelMaterialPropertyBlock.Node node;
		if (UIPanelMaterialPropertyBlock.dumpCount <= 0)
		{
			node = new UIPanelMaterialPropertyBlock.Node();
		}
		else
		{
			node = UIPanelMaterialPropertyBlock.dump;
			UIPanelMaterialPropertyBlock.dump = node.prev;
			UIPanelMaterialPropertyBlock.dumpCount = UIPanelMaterialPropertyBlock.dumpCount - 1;
			node.disposed = false;
		}
		node.property = prop;
		node.type = type;
		UIPanelMaterialPropertyBlock uIPanelMaterialPropertyBlock = block;
		int num = uIPanelMaterialPropertyBlock.count;
		int num1 = num;
		uIPanelMaterialPropertyBlock.count = num + 1;
		if (num1 != 0)
		{
			node.prev = block.last;
			node.hasPrev = true;
			node.next = null;
			node.hasNext = false;
			block.last = node;
			node.prev.next = node;
			node.prev.hasNext = true;
		}
		else
		{
			UIPanelMaterialPropertyBlock.Node node1 = node;
			UIPanelMaterialPropertyBlock.Node node2 = node1;
			block.last = node1;
			block.first = node2;
			int num2 = 0;
			bool flag = (bool)num2;
			node.hasPrev = (bool)num2;
			node.hasNext = flag;
			object obj = null;
			node2 = (UIPanelMaterialPropertyBlock.Node)obj;
			node.prev = (UIPanelMaterialPropertyBlock.Node)obj;
			node.next = node2;
		}
		return node;
	}

	private static UIPanelMaterialPropertyBlock.Node NewNode(UIPanelMaterialPropertyBlock block, int prop, ref Vector4 value)
	{
		UIPanelMaterialPropertyBlock.Node node = UIPanelMaterialPropertyBlock.NewNode(block, prop, UIPanelMaterialPropertyBlock.PropType.Vector);
		node.@value.VECTOR.x = value.x;
		node.@value.VECTOR.y = value.y;
		node.@value.VECTOR.z = value.z;
		node.@value.VECTOR.w = value.w;
		return node;
	}

	private static UIPanelMaterialPropertyBlock.Node NewNode(UIPanelMaterialPropertyBlock block, int prop, ref Color value)
	{
		UIPanelMaterialPropertyBlock.Node node = UIPanelMaterialPropertyBlock.NewNode(block, prop, UIPanelMaterialPropertyBlock.PropType.Color);
		node.@value.COLOR.r = value.r;
		node.@value.COLOR.g = value.g;
		node.@value.COLOR.b = value.b;
		node.@value.COLOR.a = value.a;
		return node;
	}

	private static UIPanelMaterialPropertyBlock.Node NewNode(UIPanelMaterialPropertyBlock block, int prop, ref float value)
	{
		UIPanelMaterialPropertyBlock.Node node = UIPanelMaterialPropertyBlock.NewNode(block, prop, UIPanelMaterialPropertyBlock.PropType.Float);
		node.@value.FLOAT = value;
		return node;
	}

	private static UIPanelMaterialPropertyBlock.Node NewNode(UIPanelMaterialPropertyBlock block, int prop, ref Matrix4x4 value)
	{
		UIPanelMaterialPropertyBlock.Node node = UIPanelMaterialPropertyBlock.NewNode(block, prop, UIPanelMaterialPropertyBlock.PropType.Matrix);
		node.@value.MATRIX.m00 = value.m00;
		node.@value.MATRIX.m10 = value.m10;
		node.@value.MATRIX.m20 = value.m20;
		node.@value.MATRIX.m30 = value.m30;
		node.@value.MATRIX.m01 = value.m01;
		node.@value.MATRIX.m11 = value.m11;
		node.@value.MATRIX.m21 = value.m21;
		node.@value.MATRIX.m31 = value.m31;
		node.@value.MATRIX.m02 = value.m02;
		node.@value.MATRIX.m12 = value.m12;
		node.@value.MATRIX.m22 = value.m22;
		node.@value.MATRIX.m32 = value.m32;
		node.@value.MATRIX.m03 = value.m03;
		node.@value.MATRIX.m13 = value.m13;
		node.@value.MATRIX.m23 = value.m23;
		node.@value.MATRIX.m33 = value.m33;
		return node;
	}

	public void Set(string property, Color value)
	{
		this.Set(Shader.PropertyToID(property), value);
	}

	public void Set(string property, Vector4 value)
	{
		this.Set(Shader.PropertyToID(property), value);
	}

	public void Set(string property, float value)
	{
		this.Set(Shader.PropertyToID(property), value);
	}

	public void Set(string property, Matrix4x4 value)
	{
		this.Set(Shader.PropertyToID(property), value);
	}

	public void Set(int property, Color value)
	{
		UIPanelMaterialPropertyBlock.NewNode(this, property, ref value);
	}

	public void Set(int property, Vector4 value)
	{
		UIPanelMaterialPropertyBlock.NewNode(this, property, ref value);
	}

	public void Set(int property, float value)
	{
		UIPanelMaterialPropertyBlock.NewNode(this, property, ref value);
	}

	public void Set(int property, Matrix4x4 value)
	{
		UIPanelMaterialPropertyBlock.NewNode(this, property, ref value);
	}

	private class Node
	{
		public UIPanelMaterialPropertyBlock.Node prev;

		public UIPanelMaterialPropertyBlock.Node next;

		public int property;

		public UIPanelMaterialPropertyBlock.PropValue @value;

		public UIPanelMaterialPropertyBlock.PropType type;

		public bool hasNext;

		public bool hasPrev;

		public bool disposed;

		public Node()
		{
		}
	}

	private enum PropType : byte
	{
		Float,
		Vector,
		Color,
		Matrix
	}

	[StructLayout(LayoutKind.Explicit)]
	private struct PropValue
	{
		[FieldOffset(0)]
		public Color COLOR;

		[FieldOffset(0)]
		public float FLOAT;

		[FieldOffset(0)]
		public Vector4 VECTOR;

		[FieldOffset(0)]
		public Matrix4x4 MATRIX;
	}
}