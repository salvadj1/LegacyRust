using System;
using UnityEngine;

namespace NGUI.Meshing
{
	public class MeshBuffer
	{
		public Vertex[] v;

		public int vSize;

		public int iCount;

		private Primitive[] primitives;

		private int vertCapacity;

		private int primSize;

		private int primCapacity;

		private PrimitiveKind lastPrimitiveKind = PrimitiveKind.Invalid;

		public MeshBuffer()
		{
		}

		public int Alloc(PrimitiveKind kind, out int end)
		{
			int num = Primitive.VertexCount(kind);
			if (this.lastPrimitiveKind != kind)
			{
				int num1 = MeshBuffer.Gen_Alloc<Primitive>(1, ref this.primSize, ref this.primCapacity, ref this.primitives, 4, 32, 32);
				if (!Primitive.JoinsInList(kind))
				{
					this.lastPrimitiveKind = PrimitiveKind.Invalid;
				}
				else
				{
					this.lastPrimitiveKind = kind;
				}
				this.primitives[num1] = new Primitive(kind, (ushort)this.vSize);
			}
			MeshBuffer meshBuffer = this;
			meshBuffer.iCount = meshBuffer.iCount + Primitive.IndexCount(kind);
			int num2 = MeshBuffer.Gen_Alloc<Vertex>(num, ref this.vSize, ref this.vertCapacity, ref this.v, 32, 512, 512);
			end = num2 + num;
			return num2;
		}

		public int Alloc(PrimitiveKind primitive, Color color, out int end)
		{
			int num = this.Alloc(primitive, out end);
			float single = (float)color.r;
			float single1 = (float)color.g;
			float single2 = (float)color.b;
			float single3 = (float)color.a;
			for (int i = num; i < end; i++)
			{
				this.v[i].r = single;
				this.v[i].g = single1;
				this.v[i].b = single2;
				this.v[i].a = single3;
			}
			return num;
		}

		public int Alloc(PrimitiveKind primitive, float z, Color color, out int end)
		{
			int num = this.Alloc(primitive, out end);
			float single = (float)color.r;
			float single1 = (float)color.g;
			float single2 = (float)color.b;
			float single3 = (float)color.a;
			for (int i = num; i < end; i++)
			{
				this.v[i].r = single;
				this.v[i].g = single1;
				this.v[i].b = single2;
				this.v[i].a = single3;
				this.v[i].z = z;
			}
			return num;
		}

		public int Alloc(PrimitiveKind primitive, float z, ref Color color, out int end)
		{
			int num = this.Alloc(primitive, out end);
			float single = (float)color.r;
			float single1 = (float)color.g;
			float single2 = (float)color.b;
			float single3 = (float)color.a;
			for (int i = num; i < end; i++)
			{
				this.v[i].r = single;
				this.v[i].g = single1;
				this.v[i].b = single2;
				this.v[i].a = single3;
				this.v[i].z = z;
			}
			return num;
		}

		public int Alloc(PrimitiveKind primitive, float z, out int end)
		{
			int num = this.Alloc(primitive, out end);
			for (int i = num; i < end; i++)
			{
				this.v[i].z = z;
				float single = 1f;
				float single1 = single;
				this.v[i].a = single;
				float single2 = single1;
				single1 = single2;
				this.v[i].b = single2;
				float single3 = single1;
				single1 = single3;
				this.v[i].g = single3;
				this.v[i].r = single1;
			}
			return num;
		}

		public int Alloc(PrimitiveKind primitive, Vertex V, out int end)
		{
			int num = this.Alloc(primitive, out end);
			for (int i = num; i < end; i++)
			{
				this.v[i].x = V.x;
				this.v[i].y = V.y;
				this.v[i].r = V.r;
				this.v[i].u = V.u;
				this.v[i].v = V.v;
				this.v[i].g = V.g;
				this.v[i].b = V.b;
				this.v[i].a = V.a;
			}
			return num;
		}

		public int Alloc(PrimitiveKind kind)
		{
			int num;
			return this.Alloc(kind, out num);
		}

		public int Alloc(PrimitiveKind kind, Color color)
		{
			int num;
			return this.Alloc(kind, color, out num);
		}

		public int Alloc(PrimitiveKind kind, float z)
		{
			int num;
			return this.Alloc(kind, z, out num);
		}

		public int Alloc(PrimitiveKind kind, float z, Color color)
		{
			int num;
			return this.Alloc(kind, z, color, out num);
		}

		public int Alloc(PrimitiveKind kind, float z, ref Color color)
		{
			int num;
			return this.Alloc(kind, z, ref color, out num);
		}

		public int Alloc(PrimitiveKind kind, Vertex v)
		{
			int num;
			return this.Alloc(kind, v, out num);
		}

		public void ApplyEffect(Transform transform, int vertexStart, UILabel.Effect effect, Color effectColor, float size)
		{
			this.ApplyEffect(transform, vertexStart, this.vSize, effect, effectColor, size);
		}

		public void ApplyEffect(Transform transform, int vertexStart, int vertexEnd, UILabel.Effect effect, Color effectColor, float size)
		{
			int num;
			if (effect != UILabel.Effect.None && vertexStart != vertexEnd && !NGUITools.ZeroAlpha(effectColor.a) && size != 0f && !MeshBuffer.ZeroedXYScale(transform) && this.SeekPrimitiveIndex(vertexStart, out num))
			{
				float single = 1f / size;
				UILabel.Effect effect1 = effect;
				if (effect1 == UILabel.Effect.Shadow)
				{
					this.ApplyShadow(vertexStart, vertexEnd, num, single, effectColor.r, effectColor.g, effectColor.b, effectColor.a);
				}
				else if (effect1 == UILabel.Effect.Outline)
				{
					this.ApplyOutline(vertexStart, vertexEnd, num, single, effectColor.r, effectColor.g, effectColor.b, effectColor.a);
				}
			}
		}

		private void ApplyOutline(int start, int end, int primitiveIndex, float pixel, float r, float g, float b, float a)
		{
			int num;
			int num1;
			int num2;
			int num3;
			while (start < end)
			{
				if (primitiveIndex != this.primSize - 1 && this.primitives[primitiveIndex + 1].start <= start)
				{
					primitiveIndex++;
				}
				int num4 = this.Alloc(this.primitives[primitiveIndex].kind, out num);
				int num5 = this.Alloc(this.primitives[primitiveIndex].kind, out num1);
				int num6 = this.Alloc(this.primitives[primitiveIndex].kind, out num2);
				int num7 = this.Alloc(this.primitives[primitiveIndex].kind, out num3);
				if (num7 == num3)
				{
					throw new InvalidOperationException();
				}
				while (num7 < num3)
				{
					this.v[num7] = this.v[start];
					this.v[start].r = r;
					this.v[start].g = g;
					this.v[start].b = b;
					this.v[start].a = this.v[start].a * a;
					Vertex vertex = this.v[start];
					Vertex vertex1 = vertex;
					this.v[num6] = vertex;
					Vertex vertex2 = vertex1;
					vertex1 = vertex2;
					this.v[num5] = vertex2;
					this.v[num4] = vertex1;
					this.v[start].x = this.v[start].x + pixel;
					this.v[start].y = this.v[start].y - pixel;
					this.v[num4].x = this.v[num4].x - pixel;
					this.v[num4].y = this.v[num4].y + pixel;
					this.v[num5].x = this.v[num5].x + pixel;
					this.v[num5].y = this.v[num5].y + pixel;
					this.v[num6].x = this.v[num6].x - pixel;
					this.v[num6].y = this.v[num6].y - pixel;
					num4++;
					num5++;
					num6++;
					num7++;
					start++;
				}
			}
		}

		private void ApplyShadow(int start, int end, int primitiveIndex, float pixel, float r, float g, float b, float a)
		{
			int num;
			while (start < end)
			{
				if (primitiveIndex != this.primSize - 1 && this.primitives[primitiveIndex + 1].start <= start)
				{
					primitiveIndex++;
				}
				int num1 = this.Alloc(this.primitives[primitiveIndex].kind, out num);
				if (num1 == num)
				{
					throw new InvalidOperationException();
				}
				while (num1 < num)
				{
					int num2 = num1;
					num1 = num2 + 1;
					this.v[num2] = this.v[start];
					this.v[start].r = r;
					this.v[start].g = g;
					this.v[start].b = b;
					this.v[start].a = this.v[start].a * a;
					this.v[start].x = this.v[start].x + pixel;
					this.v[start].y = this.v[start].y - pixel;
					start++;
				}
			}
		}

		public void BuildTransformedVertices4x4(ref Vector3[] tV, float m00, float m10, float m20, float m30, float m01, float m11, float m21, float m31, float m02, float m12, float m22, float m32, float m03, float m13, float m23, float m33)
		{
			Array.Resize<Vector3>(ref tV, this.vSize);
			for (int i = 0; i < this.vSize; i++)
			{
				float single = 1f / (m30 * this.v[i].x + m31 * this.v[i].y + m32 * this.v[i].z + m33);
				tV[i].x = (m00 * this.v[i].x + m01 * this.v[i].y + m02 * this.v[i].z + m03) * single;
				tV[i].y = (m10 * this.v[i].x + m11 * this.v[i].y + m12 * this.v[i].z + m13) * single;
				tV[i].z = (m20 * this.v[i].x + m21 * this.v[i].y + m22 * this.v[i].z + m23) * single;
			}
		}

		public void Clear()
		{
			this.vSize = 0;
			this.iCount = 0;
			this.primSize = 0;
			this.lastPrimitiveKind = PrimitiveKind.Invalid;
		}

		private void Extract(MeshBuffer.FillBuffer<Vector3> vertices, MeshBuffer.FillBuffer<Vector2> uvs, MeshBuffer.FillBuffer<Color> colors, MeshBuffer.FillBuffer<int> triangles)
		{
			Vector3[] vector3Array = vertices.buf;
			Vector2[] vector2Array = uvs.buf;
			Color[] colorArray = colors.buf;
			int[] numArray = triangles.buf;
			int num = vertices.offset;
			int num1 = uvs.offset;
			int num2 = colors.offset;
			for (int i = 0; i < this.vSize; i++)
			{
				vector3Array[num].x = (float)this.v[i].x;
				vector3Array[num].y = (float)this.v[i].y;
				vector3Array[num].z = (float)this.v[i].z;
				vector2Array[num1].x = (float)this.v[i].u;
				vector2Array[num1].y = (float)this.v[i].v;
				colorArray[num2].r = (float)this.v[i].r;
				colorArray[num2].g = (float)this.v[i].g;
				colorArray[num2].b = (float)this.v[i].b;
				colorArray[num2].a = (float)this.v[i].a;
				num++;
				num1++;
				num2++;
			}
			int num3 = triangles.offset;
			int num4 = vertices.offset;
			if (this.primSize > 0)
			{
				for (int j = 0; j < this.primSize - 1; j++)
				{
					this.primitives[j].Put(numArray, ref num4, ref num3, (int)this.primitives[j + 1].start);
				}
				this.primitives[this.primSize - 1].Put(numArray, ref num4, ref num3, this.vSize);
			}
		}

		public bool ExtractMeshBuffers(ref Vector3[] vertices, ref Vector2[] uvs, ref Color[] colors, ref int[] triangles)
		{
			bool flag = MeshBuffer.ResizeChecked<Vector3>(ref vertices, this.vSize) | MeshBuffer.ResizeChecked<Vector2>(ref uvs, this.vSize) | MeshBuffer.ResizeChecked<Color>(ref colors, this.vSize) | MeshBuffer.ResizeChecked<int>(ref triangles, this.iCount);
			MeshBuffer.FillBuffer<Vector3> fillBuffer = new MeshBuffer.FillBuffer<Vector3>()
			{
				buf = vertices
			};
			MeshBuffer.FillBuffer<Vector2> fillBuffer1 = new MeshBuffer.FillBuffer<Vector2>()
			{
				buf = uvs
			};
			MeshBuffer.FillBuffer<Color> fillBuffer2 = new MeshBuffer.FillBuffer<Color>()
			{
				buf = colors
			};
			MeshBuffer.FillBuffer<int> fillBuffer3 = new MeshBuffer.FillBuffer<int>()
			{
				buf = triangles
			};
			this.Extract(fillBuffer, fillBuffer1, fillBuffer2, fillBuffer3);
			return flag;
		}

		public void FastCell(Vector2 xy0, Vector2 xy1, Vector2 uv0, Vector2 uv1, ref Color color)
		{
			Vertex vertex = new Vertex();
			Vertex vertex1 = new Vertex();
			Vertex vertex2 = new Vertex();
			Vertex vertex3 = new Vertex();
			float single = xy1.x;
			float single1 = single;
			vertex1.x = single;
			vertex.x = single1;
			float single2 = xy1.y;
			single1 = single2;
			vertex3.y = single2;
			vertex.y = single1;
			float single3 = xy0.x;
			single1 = single3;
			vertex3.x = single3;
			vertex2.x = single1;
			float single4 = xy0.y;
			single1 = single4;
			vertex2.y = single4;
			vertex1.y = single1;
			float single5 = 0f;
			single1 = single5;
			vertex3.z = single5;
			float single6 = single1;
			single1 = single6;
			vertex2.z = single6;
			float single7 = single1;
			single1 = single7;
			vertex1.z = single7;
			vertex.z = single1;
			float single8 = uv1.x;
			single1 = single8;
			vertex1.u = single8;
			vertex.u = single1;
			float single9 = uv1.y;
			single1 = single9;
			vertex3.v = single9;
			vertex.v = single1;
			float single10 = uv0.x;
			single1 = single10;
			vertex3.u = single10;
			vertex2.u = single1;
			float single11 = uv0.y;
			single1 = single11;
			vertex2.v = single11;
			vertex1.v = single1;
			float single12 = color.r;
			single1 = single12;
			vertex3.r = single12;
			float single13 = single1;
			single1 = single13;
			vertex2.r = single13;
			float single14 = single1;
			single1 = single14;
			vertex1.r = single14;
			vertex.r = single1;
			float single15 = color.g;
			single1 = single15;
			vertex3.g = single15;
			float single16 = single1;
			single1 = single16;
			vertex2.g = single16;
			float single17 = single1;
			single1 = single17;
			vertex1.g = single17;
			vertex.g = single1;
			float single18 = color.b;
			single1 = single18;
			vertex3.b = single18;
			float single19 = single1;
			single1 = single19;
			vertex2.b = single19;
			float single20 = single1;
			single1 = single20;
			vertex1.b = single20;
			vertex.b = single1;
			float single21 = color.a;
			single1 = single21;
			vertex3.a = single21;
			float single22 = single1;
			single1 = single22;
			vertex2.a = single22;
			float single23 = single1;
			single1 = single23;
			vertex1.a = single23;
			vertex.a = single1;
			this.Quad(vertex, vertex1, vertex2, vertex3);
		}

		public int FastQuad(Vector2 uv0, Vector2 uv1, Color color)
		{
			Vertex vertex = new Vertex();
			Vertex vertex1 = new Vertex();
			Vertex vertex2 = new Vertex();
			Vertex vertex3 = new Vertex();
			float single = 1f;
			float single1 = single;
			vertex1.x = single;
			vertex.x = single1;
			float single2 = -1f;
			single1 = single2;
			vertex2.y = single2;
			vertex1.y = single1;
			float single3 = 0f;
			single1 = single3;
			vertex3.z = single3;
			float single4 = single1;
			single1 = single4;
			vertex3.y = single4;
			float single5 = single1;
			single1 = single5;
			vertex3.x = single5;
			float single6 = single1;
			single1 = single6;
			vertex2.z = single6;
			float single7 = single1;
			single1 = single7;
			vertex2.x = single7;
			float single8 = single1;
			single1 = single8;
			vertex1.z = single8;
			float single9 = single1;
			single1 = single9;
			vertex.z = single9;
			vertex.y = single1;
			float single10 = uv1.x;
			single1 = single10;
			vertex1.u = single10;
			vertex.u = single1;
			float single11 = uv1.y;
			single1 = single11;
			vertex3.v = single11;
			vertex.v = single1;
			float single12 = uv0.x;
			single1 = single12;
			vertex3.u = single12;
			vertex2.u = single1;
			float single13 = uv0.y;
			single1 = single13;
			vertex2.v = single13;
			vertex1.v = single1;
			float single14 = color.r;
			single1 = single14;
			vertex3.r = single14;
			float single15 = single1;
			single1 = single15;
			vertex2.r = single15;
			float single16 = single1;
			single1 = single16;
			vertex1.r = single16;
			vertex.r = single1;
			float single17 = color.g;
			single1 = single17;
			vertex3.g = single17;
			float single18 = single1;
			single1 = single18;
			vertex2.g = single18;
			float single19 = single1;
			single1 = single19;
			vertex1.g = single19;
			vertex.g = single1;
			float single20 = color.b;
			single1 = single20;
			vertex3.b = single20;
			float single21 = single1;
			single1 = single21;
			vertex2.b = single21;
			float single22 = single1;
			single1 = single22;
			vertex1.b = single22;
			vertex.b = single1;
			float single23 = color.a;
			single1 = single23;
			vertex3.a = single23;
			float single24 = single1;
			single1 = single24;
			vertex2.a = single24;
			float single25 = single1;
			single1 = single25;
			vertex1.a = single25;
			vertex.a = single1;
			return this.Quad(vertex, vertex1, vertex2, vertex3);
		}

		public int FastQuad(Rect uv, Color color)
		{
			return this.FastQuad(new Vector2(uv.xMin, uv.yMin), new Vector2(uv.xMax, uv.yMax), color);
		}

		public int FastQuad(Vector2 xy0, Vector2 xy1, Vector2 uv0, Vector2 uv1, Color color)
		{
			Vertex vertex = new Vertex();
			Vertex vertex1 = new Vertex();
			Vertex vertex2 = new Vertex();
			Vertex vertex3 = new Vertex();
			float single = xy1.x;
			float single1 = single;
			vertex1.x = single;
			vertex.x = single1;
			float single2 = xy1.y;
			single1 = single2;
			vertex3.y = single2;
			vertex.y = single1;
			float single3 = xy0.x;
			single1 = single3;
			vertex3.x = single3;
			vertex2.x = single1;
			float single4 = xy0.y;
			single1 = single4;
			vertex2.y = single4;
			vertex1.y = single1;
			float single5 = 0f;
			single1 = single5;
			vertex3.z = single5;
			float single6 = single1;
			single1 = single6;
			vertex2.z = single6;
			float single7 = single1;
			single1 = single7;
			vertex1.z = single7;
			vertex.z = single1;
			float single8 = uv1.x;
			single1 = single8;
			vertex1.u = single8;
			vertex.u = single1;
			float single9 = uv1.y;
			single1 = single9;
			vertex3.v = single9;
			vertex.v = single1;
			float single10 = uv0.x;
			single1 = single10;
			vertex3.u = single10;
			vertex2.u = single1;
			float single11 = uv0.y;
			single1 = single11;
			vertex2.v = single11;
			vertex1.v = single1;
			float single12 = color.r;
			single1 = single12;
			vertex3.r = single12;
			float single13 = single1;
			single1 = single13;
			vertex2.r = single13;
			float single14 = single1;
			single1 = single14;
			vertex1.r = single14;
			vertex.r = single1;
			float single15 = color.g;
			single1 = single15;
			vertex3.g = single15;
			float single16 = single1;
			single1 = single16;
			vertex2.g = single16;
			float single17 = single1;
			single1 = single17;
			vertex1.g = single17;
			vertex.g = single1;
			float single18 = color.b;
			single1 = single18;
			vertex3.b = single18;
			float single19 = single1;
			single1 = single19;
			vertex2.b = single19;
			float single20 = single1;
			single1 = single20;
			vertex1.b = single20;
			vertex.b = single1;
			float single21 = color.a;
			single1 = single21;
			vertex3.a = single21;
			float single22 = single1;
			single1 = single22;
			vertex2.a = single22;
			float single23 = single1;
			single1 = single23;
			vertex1.a = single23;
			vertex.a = single1;
			return this.Quad(vertex, vertex1, vertex2, vertex3);
		}

		public int FastQuad(Vector2 xy0, Vector2 xy1, float z, Vector2 uv0, Vector2 uv1, Color color)
		{
			Vertex vertex = new Vertex();
			Vertex vertex1 = new Vertex();
			Vertex vertex2 = new Vertex();
			Vertex vertex3 = new Vertex();
			float single = xy1.x;
			float single1 = single;
			vertex1.x = single;
			vertex.x = single1;
			float single2 = xy1.y;
			single1 = single2;
			vertex3.y = single2;
			vertex.y = single1;
			float single3 = xy0.x;
			single1 = single3;
			vertex3.x = single3;
			vertex2.x = single1;
			float single4 = xy0.y;
			single1 = single4;
			vertex2.y = single4;
			vertex1.y = single1;
			float single5 = z;
			single1 = single5;
			vertex3.z = single5;
			float single6 = single1;
			single1 = single6;
			vertex2.z = single6;
			float single7 = single1;
			single1 = single7;
			vertex1.z = single7;
			vertex.z = single1;
			float single8 = uv1.x;
			single1 = single8;
			vertex1.u = single8;
			vertex.u = single1;
			float single9 = uv1.y;
			single1 = single9;
			vertex3.v = single9;
			vertex.v = single1;
			float single10 = uv0.x;
			single1 = single10;
			vertex3.u = single10;
			vertex2.u = single1;
			float single11 = uv0.y;
			single1 = single11;
			vertex2.v = single11;
			vertex1.v = single1;
			float single12 = color.r;
			single1 = single12;
			vertex3.r = single12;
			float single13 = single1;
			single1 = single13;
			vertex2.r = single13;
			float single14 = single1;
			single1 = single14;
			vertex1.r = single14;
			vertex.r = single1;
			float single15 = color.g;
			single1 = single15;
			vertex3.g = single15;
			float single16 = single1;
			single1 = single16;
			vertex2.g = single16;
			float single17 = single1;
			single1 = single17;
			vertex1.g = single17;
			vertex.g = single1;
			float single18 = color.b;
			single1 = single18;
			vertex3.b = single18;
			float single19 = single1;
			single1 = single19;
			vertex2.b = single19;
			float single20 = single1;
			single1 = single20;
			vertex1.b = single20;
			vertex.b = single1;
			float single21 = color.a;
			single1 = single21;
			vertex3.a = single21;
			float single22 = single1;
			single1 = single22;
			vertex2.a = single22;
			float single23 = single1;
			single1 = single23;
			vertex1.a = single23;
			vertex.a = single1;
			return this.Quad(vertex, vertex1, vertex2, vertex3);
		}

		private static int Gen_Alloc<T>(int count, ref int size, ref int cap, ref T[] array, int initAllocSize, int maxAllocSize, int maxAllocSizeIncrement)
		{
			if (count <= 0)
			{
				return -1;
			}
			int num = size;
			size = size + count;
			if (size > cap)
			{
				if (cap == 0)
				{
					cap = initAllocSize;
				}
				while (cap < size)
				{
					if (cap >= maxAllocSize)
					{
						cap = cap + maxAllocSizeIncrement;
					}
					else
					{
						cap = cap * 2;
					}
				}
				Array.Resize<T>(ref array, cap);
			}
			return num;
		}

		public void Offset(float x, float y, float z)
		{
			if (x == 0f)
			{
				if (y == 0f)
				{
					if (z == 0f)
					{
						return;
					}
					for (int i = 0; i < this.vSize; i++)
					{
						this.v[i].z = this.v[i].z + z;
					}
				}
				else if (z != 0f)
				{
					for (int j = 0; j < this.vSize; j++)
					{
						this.v[j].y = this.v[j].y + y;
						this.v[j].z = this.v[j].z + z;
					}
				}
				else
				{
					for (int k = 0; k < this.vSize; k++)
					{
						this.v[k].y = this.v[k].y + y;
					}
				}
			}
			else if (y == 0f)
			{
				if (z != 0f)
				{
					for (int l = 0; l < this.vSize; l++)
					{
						this.v[l].x = this.v[l].x + x;
						this.v[l].z = this.v[l].z + z;
					}
				}
				else
				{
					for (int m = 0; m < this.vSize; m++)
					{
						this.v[m].x = this.v[m].x + x;
					}
				}
			}
			else if (z != 0f)
			{
				for (int n = 0; n < this.vSize; n++)
				{
					this.v[n].x = this.v[n].x + x;
					this.v[n].y = this.v[n].y + y;
					this.v[n].z = this.v[n].z + z;
				}
			}
			else
			{
				for (int o = 0; o < this.vSize; o++)
				{
					this.v[o].x = this.v[o].x + x;
					this.v[o].y = this.v[o].y + y;
				}
			}
		}

		public void OffsetThenTransformVertices(float x, float y, float z, float m00, float m10, float m20, float m01, float m11, float m21, float m02, float m12, float m22, float m03, float m13, float m23)
		{
			float single;
			float single1;
			float single2;
			if (x == 0f)
			{
				if (y == 0f)
				{
					if (z != 0f)
					{
						for (int i = 0; i < this.vSize; i++)
						{
							single = this.v[i].x;
							single1 = this.v[i].y;
							single2 = this.v[i].z + z;
							this.v[i].x = m00 * single + m01 * single1 + m02 * single2 + m03;
							this.v[i].y = m10 * single + m11 * single1 + m12 * single2 + m13;
							this.v[i].z = m20 * single + m21 * single1 + m22 * single2 + m23;
						}
					}
					else
					{
						for (int j = 0; j < this.vSize; j++)
						{
							single = this.v[j].x;
							single1 = this.v[j].y;
							single2 = this.v[j].z;
							this.v[j].x = m00 * single + m01 * single1 + m02 * single2 + m03;
							this.v[j].y = m10 * single + m11 * single1 + m12 * single2 + m13;
							this.v[j].z = m20 * single + m21 * single1 + m22 * single2 + m23;
						}
					}
				}
				else if (z != 0f)
				{
					for (int k = 0; k < this.vSize; k++)
					{
						single = this.v[k].x;
						single1 = this.v[k].y + y;
						single2 = this.v[k].z + z;
						this.v[k].x = m00 * single + m01 * single1 + m02 * single2 + m03;
						this.v[k].y = m10 * single + m11 * single1 + m12 * single2 + m13;
						this.v[k].z = m20 * single + m21 * single1 + m22 * single2 + m23;
					}
				}
				else
				{
					for (int l = 0; l < this.vSize; l++)
					{
						single = this.v[l].x;
						single1 = this.v[l].y + y;
						single2 = this.v[l].z;
						this.v[l].x = m00 * single + m01 * single1 + m02 * single2 + m03;
						this.v[l].y = m10 * single + m11 * single1 + m12 * single2 + m13;
						this.v[l].z = m20 * single + m21 * single1 + m22 * single2 + m23;
					}
				}
			}
			else if (y == 0f)
			{
				if (z != 0f)
				{
					for (int m = 0; m < this.vSize; m++)
					{
						single = this.v[m].x + x;
						single1 = this.v[m].y;
						single2 = this.v[m].z + z;
						this.v[m].x = m00 * single + m01 * single1 + m02 * single2 + m03;
						this.v[m].y = m10 * single + m11 * single1 + m12 * single2 + m13;
						this.v[m].z = m20 * single + m21 * single1 + m22 * single2 + m23;
					}
				}
				else
				{
					for (int n = 0; n < this.vSize; n++)
					{
						single = this.v[n].x + x;
						single1 = this.v[n].y;
						single2 = this.v[n].z;
						this.v[n].x = m00 * single + m01 * single1 + m02 * single2 + m03;
						this.v[n].y = m10 * single + m11 * single1 + m12 * single2 + m13;
						this.v[n].z = m20 * single + m21 * single1 + m22 * single2 + m23;
					}
				}
			}
			else if (z != 0f)
			{
				for (int o = 0; o < this.vSize; o++)
				{
					single = this.v[o].x + x;
					single1 = this.v[o].y + y;
					single2 = this.v[o].z + z;
					this.v[o].x = m00 * single + m01 * single1 + m02 * single2 + m03;
					this.v[o].y = m10 * single + m11 * single1 + m12 * single2 + m13;
					this.v[o].z = m20 * single + m21 * single1 + m22 * single2 + m23;
				}
			}
			else
			{
				for (int p = 0; p < this.vSize; p++)
				{
					single = this.v[p].x + x;
					single1 = this.v[p].y + y;
					single2 = this.v[p].z;
					this.v[p].x = m00 * single + m01 * single1 + m02 * single2 + m03;
					this.v[p].y = m10 * single + m11 * single1 + m12 * single2 + m13;
					this.v[p].z = m20 * single + m21 * single1 + m22 * single2 + m23;
				}
			}
		}

		public int Quad(Vertex A, Vertex B, Vertex C, Vertex D)
		{
			int num;
			int num1 = this.Alloc(PrimitiveKind.Quad, out num);
			int num2 = num1;
			int num3 = num2 + 1;
			this.v[num2] = B;
			int num4 = num3;
			num3 = num4 + 1;
			this.v[num4] = A;
			int num5 = num3;
			num3 = num5 + 1;
			this.v[num5] = C;
			int num6 = num3;
			num3 = num6 + 1;
			this.v[num6] = D;
			return num1;
		}

		public int QuadAlt(Vertex A, Vertex B, Vertex C, Vertex D)
		{
			return this.Quad(D, A, B, C);
		}

		private static bool ResizeChecked<T>(ref T[] array, int size)
		{
			if (size == 0)
			{
				if (array == null || (int)array.Length == 0)
				{
					return false;
				}
				array = null;
				return true;
			}
			if (array != null && (int)array.Length == size)
			{
				return false;
			}
			Array.Resize<T>(ref array, size);
			return true;
		}

		private bool SeekPrimitiveIndex(int start, out int i)
		{
			i = this.primSize - 1;
			while (i >= 0)
			{
				if (this.primitives[i].start <= start)
				{
					return true;
				}
				i = i - 1;
			}
			i = -1;
			return false;
		}

		public int TextureQuad(Vertex A, Vertex B, Vertex C, Vertex D)
		{
			int num;
			int num1 = this.Alloc(PrimitiveKind.Quad, out num);
			int num2 = num1;
			int num3 = num2 + 1;
			this.v[num2] = D;
			int num4 = num3;
			num3 = num4 + 1;
			this.v[num4] = A;
			int num5 = num3;
			num3 = num5 + 1;
			this.v[num5] = C;
			int num6 = num3;
			num3 = num6 + 1;
			this.v[num6] = B;
			return num1;
		}

		public void TransformThenOffsetVertices(float m00, float m10, float m20, float m01, float m11, float m21, float m02, float m12, float m22, float m03, float m13, float m23, float x, float y, float z)
		{
			float single;
			float single1;
			float single2;
			if (x == 0f)
			{
				if (y == 0f)
				{
					if (z != 0f)
					{
						for (int i = 0; i < this.vSize; i++)
						{
							single = this.v[i].x;
							single1 = this.v[i].y;
							single2 = this.v[i].z;
							this.v[i].x = m00 * single + m01 * single1 + m02 * single2 + m03;
							this.v[i].y = m10 * single + m11 * single1 + m12 * single2 + m13;
							this.v[i].z = m20 * single + m21 * single1 + m22 * single2 + m23 + z;
						}
					}
					else
					{
						for (int j = 0; j < this.vSize; j++)
						{
							single = this.v[j].x;
							single1 = this.v[j].y;
							single2 = this.v[j].z;
							this.v[j].x = m00 * single + m01 * single1 + m02 * single2 + m03;
							this.v[j].y = m10 * single + m11 * single1 + m12 * single2 + m13;
							this.v[j].z = m20 * single + m21 * single1 + m22 * single2 + m23;
						}
					}
				}
				else if (z != 0f)
				{
					for (int k = 0; k < this.vSize; k++)
					{
						single = this.v[k].x;
						single1 = this.v[k].y;
						single2 = this.v[k].z;
						this.v[k].x = m00 * single + m01 * single1 + m02 * single2 + m03;
						this.v[k].y = m10 * single + m11 * single1 + m12 * single2 + m13 + y;
						this.v[k].z = m20 * single + m21 * single1 + m22 * single2 + m23 + z;
					}
				}
				else
				{
					for (int l = 0; l < this.vSize; l++)
					{
						single = this.v[l].x;
						single1 = this.v[l].y;
						single2 = this.v[l].z;
						this.v[l].x = m00 * single + m01 * single1 + m02 * single2 + m03;
						this.v[l].y = m10 * single + m11 * single1 + m12 * single2 + m13 + y;
						this.v[l].z = m20 * single + m21 * single1 + m22 * single2 + m23;
					}
				}
			}
			else if (y == 0f)
			{
				if (z != 0f)
				{
					for (int m = 0; m < this.vSize; m++)
					{
						single = this.v[m].x;
						single1 = this.v[m].y;
						single2 = this.v[m].z;
						this.v[m].x = m00 * single + m01 * single1 + m02 * single2 + m03 + x;
						this.v[m].y = m10 * single + m11 * single1 + m12 * single2 + m13;
						this.v[m].z = m20 * single + m21 * single1 + m22 * single2 + m23 + z;
					}
				}
				else
				{
					for (int n = 0; n < this.vSize; n++)
					{
						single = this.v[n].x;
						single1 = this.v[n].y;
						single2 = this.v[n].z;
						this.v[n].x = m00 * single + m01 * single1 + m02 * single2 + m03 + x;
						this.v[n].y = m10 * single + m11 * single1 + m12 * single2 + m13;
						this.v[n].z = m20 * single + m21 * single1 + m22 * single2 + m23;
					}
				}
			}
			else if (z != 0f)
			{
				for (int o = 0; o < this.vSize; o++)
				{
					single = this.v[o].x;
					single1 = this.v[o].y;
					single2 = this.v[o].z;
					this.v[o].x = m00 * single + m01 * single1 + m02 * single2 + m03 + x;
					this.v[o].y = m10 * single + m11 * single1 + m12 * single2 + m13 + y;
					this.v[o].z = m20 * single + m21 * single1 + m22 * single2 + m23 + z;
				}
			}
			else
			{
				for (int p = 0; p < this.vSize; p++)
				{
					single = this.v[p].x;
					single1 = this.v[p].y;
					single2 = this.v[p].z;
					this.v[p].x = m00 * single + m01 * single1 + m02 * single2 + m03 + x;
					this.v[p].y = m10 * single + m11 * single1 + m12 * single2 + m13 + y;
					this.v[p].z = m20 * single + m21 * single1 + m22 * single2 + m23;
				}
			}
		}

		public void TransformVertices(float m00, float m10, float m20, float m01, float m11, float m21, float m02, float m12, float m22, float m03, float m13, float m23)
		{
			for (int i = 0; i < this.vSize; i++)
			{
				float single = this.v[i].x;
				float single1 = this.v[i].y;
				float single2 = this.v[i].z;
				this.v[i].x = m00 * single + m01 * single1 + m02 * single2 + m03;
				this.v[i].y = m10 * single + m11 * single1 + m12 * single2 + m13;
				this.v[i].z = m20 * single + m21 * single1 + m22 * single2 + m23;
			}
		}

		public int Triangle(Vertex A, Vertex B, Vertex C)
		{
			int num;
			int num1 = this.Alloc(PrimitiveKind.Triangle, out num);
			int num2 = num1;
			int num3 = num2 + 1;
			this.v[num2] = A;
			int num4 = num3;
			num3 = num4 + 1;
			this.v[num4] = B;
			int num5 = num3;
			num3 = num5 + 1;
			this.v[num5] = C;
			return num1;
		}

		public void WriteBuffers(Vector3[] transformedVertexes, MeshBuffer target)
		{
			int i;
			if (transformedVertexes == null)
			{
				this.WriteBuffers(target);
			}
			else if (this.primSize > 0)
			{
				int num = 0;
				for (i = 0; i < this.primSize - 1; i++)
				{
					this.primitives[i].Copy(ref num, this.v, transformedVertexes, (int)this.primitives[i + 1].start, target);
				}
				this.primitives[i].Copy(ref num, this.v, transformedVertexes, this.vSize, target);
			}
		}

		public void WriteBuffers(MeshBuffer target)
		{
			int i;
			if (this.primSize > 0)
			{
				int num = 0;
				for (i = 0; i < this.primSize - 1; i++)
				{
					this.primitives[i].Copy(ref num, this.v, (int)this.primitives[i + 1].start, target);
				}
				this.primitives[i].Copy(ref num, this.v, this.vSize, target);
			}
		}

		private static bool ZeroedXYScale(Transform transform)
		{
			if (!transform)
			{
				return false;
			}
			Vector3 vector3 = transform.localScale;
			return (vector3.x == 0f ? true : vector3.y == 0f);
		}

		private struct FillBuffer<T>
		{
			public T[] buf;

			public int offset;
		}
	}
}