using System;
using UnityEngine;

namespace NGUI.Meshing
{
	public struct Primitive
	{
		public readonly PrimitiveKind kind;

		public readonly ushort start;

		public Primitive(PrimitiveKind kind, ushort start)
		{
			this.kind = kind;
			this.start = start;
		}

		public void Copy(ref int start, Vertex[] v, int end, MeshBuffer p)
		{
			int num;
			int num1 = (end - start) / Primitive.VertexCount(this.kind);
		Label0:
			int num2 = num1;
			num1 = num2 - 1;
			if (num2 <= 0)
			{
				return;
			}
			int num3 = p.Alloc(this.kind, out num);
			while (num3 < num)
			{
				int num4 = num3;
				num3 = num4 + 1;
				int num5 = start;
				int num6 = num5;
				start = num5 + 1;
				p.v[num4] = v[num6];
			}
			goto Label0;
		}

		public void Copy(ref int start, Vertex[] v, Vector3[] transformed, int end, MeshBuffer p)
		{
			int num;
			int num1 = (end - start) / Primitive.VertexCount(this.kind);
		Label0:
			int num2 = num1;
			num1 = num2 - 1;
			if (num2 <= 0)
			{
				return;
			}
			int num3 = p.Alloc(this.kind, out num);
			while (num3 < num)
			{
				p.v[num3].x = transformed[start].x;
				p.v[num3].y = transformed[start].y;
				p.v[num3].z = transformed[start].z;
				p.v[num3].u = v[start].u;
				p.v[num3].v = v[start].v;
				p.v[num3].r = v[start].r;
				p.v[num3].g = v[start].g;
				p.v[num3].b = v[start].b;
				p.v[num3].a = v[start].a;
				num3++;
				start = start + 1;
			}
			goto Label0;
		}

		public static int IndexCount(PrimitiveKind kind)
		{
			switch (kind)
			{
				case PrimitiveKind.Triangle:
				{
					return 3;
				}
				case PrimitiveKind.Quad:
				{
					return 6;
				}
				case PrimitiveKind.Grid2x1:
				case PrimitiveKind.Grid1x2:
				{
					return 12;
				}
				case PrimitiveKind.Grid2x2:
				{
					return 24;
				}
				case PrimitiveKind.Grid1x3:
				case PrimitiveKind.Grid3x1:
				{
					return 18;
				}
				case PrimitiveKind.Grid3x2:
				case PrimitiveKind.Grid2x3:
				{
					return 36;
				}
				case PrimitiveKind.Grid3x3:
				{
					return 54;
				}
				case PrimitiveKind.Hole3x3:
				{
					return 48;
				}
			}
			throw new NotImplementedException();
		}

		public static bool JoinsInList(PrimitiveKind kind)
		{
			return true;
		}

		public void Put(int[] t, ref int v, ref int i, int end)
		{
			int num;
			int num1 = (end - this.start) / Primitive.VertexCount(this.kind);
			switch (this.kind)
			{
				case PrimitiveKind.Triangle:
				{
					while (true)
					{
						int num2 = num1;
						num1 = num2 - 1;
						if (num2 <= 0)
						{
							break;
						}
						int num3 = i;
						num = num3;
						i = num3 + 1;
						t[num] = v;
						int num4 = i;
						num = num4;
						i = num4 + 1;
						t[num] = v + 1;
						int num5 = i;
						num = num5;
						i = num5 + 1;
						t[num] = v + 2;
						v = v + 3;
					}
					break;
				}
				case PrimitiveKind.Quad:
				{
					while (true)
					{
						int num6 = num1;
						num1 = num6 - 1;
						if (num6 <= 0)
						{
							break;
						}
						int num7 = i;
						num = num7;
						i = num7 + 1;
						t[num] = v;
						int num8 = i;
						num = num8;
						i = num8 + 1;
						t[num] = v + 1;
						int num9 = i;
						num = num9;
						i = num9 + 1;
						t[num] = v + 3;
						int num10 = i;
						num = num10;
						i = num10 + 1;
						t[num] = v + 2;
						int num11 = i;
						num = num11;
						i = num11 + 1;
						t[num] = v;
						int num12 = i;
						num = num12;
						i = num12 + 1;
						t[num] = v + 3;
						v = v + 4;
					}
					break;
				}
				case PrimitiveKind.Grid2x1:
				{
					while (true)
					{
						int num13 = num1;
						num1 = num13 - 1;
						if (num13 <= 0)
						{
							break;
						}
						for (int i1 = 0; i1 < 2; i1++)
						{
							for (int j = 0; j < 1; j++)
							{
								int num14 = i;
								num = num14;
								i = num14 + 1;
								t[num] = v + i1 + j * 3;
								int num15 = i;
								num = num15;
								i = num15 + 1;
								t[num] = v + i1 + 1 + j * 3;
								int num16 = i;
								num = num16;
								i = num16 + 1;
								t[num] = v + i1 + (j + 1) * 3;
								int num17 = i;
								num = num17;
								i = num17 + 1;
								t[num] = v + i1 + 1 + j * 3;
								int num18 = i;
								num = num18;
								i = num18 + 1;
								t[num] = v + i1 + 1 + (j + 1) * 3;
								int num19 = i;
								num = num19;
								i = num19 + 1;
								t[num] = v + i1 + (j + 1) * 3;
							}
						}
						v = v + 6;
					}
					break;
				}
				case PrimitiveKind.Grid1x2:
				{
					while (true)
					{
						int num20 = num1;
						num1 = num20 - 1;
						if (num20 <= 0)
						{
							break;
						}
						for (int k = 0; k < 1; k++)
						{
							for (int l = 0; l < 2; l++)
							{
								int num21 = i;
								num = num21;
								i = num21 + 1;
								t[num] = v + k + l * 2;
								int num22 = i;
								num = num22;
								i = num22 + 1;
								t[num] = v + k + 1 + l * 2;
								int num23 = i;
								num = num23;
								i = num23 + 1;
								t[num] = v + k + (l + 1) * 2;
								int num24 = i;
								num = num24;
								i = num24 + 1;
								t[num] = v + k + 1 + l * 2;
								int num25 = i;
								num = num25;
								i = num25 + 1;
								t[num] = v + k + 1 + (l + 1) * 2;
								int num26 = i;
								num = num26;
								i = num26 + 1;
								t[num] = v + k + (l + 1) * 2;
							}
						}
						v = v + 6;
					}
					break;
				}
				case PrimitiveKind.Grid2x2:
				{
					while (true)
					{
						int num27 = num1;
						num1 = num27 - 1;
						if (num27 <= 0)
						{
							break;
						}
						for (int m = 0; m < 2; m++)
						{
							for (int n = 0; n < 2; n++)
							{
								int num28 = i;
								num = num28;
								i = num28 + 1;
								t[num] = v + m + n * 3;
								int num29 = i;
								num = num29;
								i = num29 + 1;
								t[num] = v + m + 1 + n * 3;
								int num30 = i;
								num = num30;
								i = num30 + 1;
								t[num] = v + m + (n + 1) * 3;
								int num31 = i;
								num = num31;
								i = num31 + 1;
								t[num] = v + m + 1 + n * 3;
								int num32 = i;
								num = num32;
								i = num32 + 1;
								t[num] = v + m + 1 + (n + 1) * 3;
								int num33 = i;
								num = num33;
								i = num33 + 1;
								t[num] = v + m + (n + 1) * 3;
							}
						}
						v = v + 9;
					}
					break;
				}
				case PrimitiveKind.Grid1x3:
				{
					while (true)
					{
						int num34 = num1;
						num1 = num34 - 1;
						if (num34 <= 0)
						{
							break;
						}
						for (int o = 0; o < 1; o++)
						{
							for (int p = 0; p < 3; p++)
							{
								int num35 = i;
								num = num35;
								i = num35 + 1;
								t[num] = v + o + p * 2;
								int num36 = i;
								num = num36;
								i = num36 + 1;
								t[num] = v + o + 1 + p * 2;
								int num37 = i;
								num = num37;
								i = num37 + 1;
								t[num] = v + o + (p + 1) * 2;
								int num38 = i;
								num = num38;
								i = num38 + 1;
								t[num] = v + o + 1 + p * 2;
								int num39 = i;
								num = num39;
								i = num39 + 1;
								t[num] = v + o + 1 + (p + 1) * 2;
								int num40 = i;
								num = num40;
								i = num40 + 1;
								t[num] = v + o + (p + 1) * 2;
							}
						}
						v = v + 8;
					}
					break;
				}
				case PrimitiveKind.Grid3x1:
				{
					while (true)
					{
						int num41 = num1;
						num1 = num41 - 1;
						if (num41 <= 0)
						{
							break;
						}
						for (int q = 0; q < 3; q++)
						{
							for (int r = 0; r < 1; r++)
							{
								int num42 = i;
								num = num42;
								i = num42 + 1;
								t[num] = v + q + r * 4;
								int num43 = i;
								num = num43;
								i = num43 + 1;
								t[num] = v + q + 1 + r * 4;
								int num44 = i;
								num = num44;
								i = num44 + 1;
								t[num] = v + q + (r + 1) * 4;
								int num45 = i;
								num = num45;
								i = num45 + 1;
								t[num] = v + q + 1 + r * 4;
								int num46 = i;
								num = num46;
								i = num46 + 1;
								t[num] = v + q + 1 + (r + 1) * 4;
								int num47 = i;
								num = num47;
								i = num47 + 1;
								t[num] = v + q + (r + 1) * 4;
							}
						}
						v = v + 8;
					}
					break;
				}
				case PrimitiveKind.Grid3x2:
				{
					while (true)
					{
						int num48 = num1;
						num1 = num48 - 1;
						if (num48 <= 0)
						{
							break;
						}
						for (int s = 0; s < 3; s++)
						{
							for (int t1 = 0; t1 < 2; t1++)
							{
								int num49 = i;
								num = num49;
								i = num49 + 1;
								t[num] = v + s + t1 * 4;
								int num50 = i;
								num = num50;
								i = num50 + 1;
								t[num] = v + s + 1 + t1 * 4;
								int num51 = i;
								num = num51;
								i = num51 + 1;
								t[num] = v + s + (t1 + 1) * 4;
								int num52 = i;
								num = num52;
								i = num52 + 1;
								t[num] = v + s + 1 + t1 * 4;
								int num53 = i;
								num = num53;
								i = num53 + 1;
								t[num] = v + s + 1 + (t1 + 1) * 4;
								int num54 = i;
								num = num54;
								i = num54 + 1;
								t[num] = v + s + (t1 + 1) * 4;
							}
						}
						v = v + 12;
					}
					break;
				}
				case PrimitiveKind.Grid2x3:
				{
					while (true)
					{
						int num55 = num1;
						num1 = num55 - 1;
						if (num55 <= 0)
						{
							break;
						}
						for (int u = 0; u < 2; u++)
						{
							for (int v1 = 0; v1 < 3; v1++)
							{
								int num56 = i;
								num = num56;
								i = num56 + 1;
								t[num] = v + u + v1 * 3;
								int num57 = i;
								num = num57;
								i = num57 + 1;
								t[num] = v + u + 1 + v1 * 3;
								int num58 = i;
								num = num58;
								i = num58 + 1;
								t[num] = v + u + (v1 + 1) * 3;
								int num59 = i;
								num = num59;
								i = num59 + 1;
								t[num] = v + u + 1 + v1 * 3;
								int num60 = i;
								num = num60;
								i = num60 + 1;
								t[num] = v + u + 1 + (v1 + 1) * 3;
								int num61 = i;
								num = num61;
								i = num61 + 1;
								t[num] = v + u + (v1 + 1) * 3;
							}
						}
						v = v + 12;
					}
					break;
				}
				case PrimitiveKind.Grid3x3:
				{
					while (true)
					{
						int num62 = num1;
						num1 = num62 - 1;
						if (num62 <= 0)
						{
							break;
						}
						for (int w = 0; w < 3; w++)
						{
							for (int x = 0; x < 3; x++)
							{
								int num63 = i;
								num = num63;
								i = num63 + 1;
								t[num] = v + w + x * 4;
								int num64 = i;
								num = num64;
								i = num64 + 1;
								t[num] = v + w + 1 + x * 4;
								int num65 = i;
								num = num65;
								i = num65 + 1;
								t[num] = v + w + (x + 1) * 4;
								int num66 = i;
								num = num66;
								i = num66 + 1;
								t[num] = v + w + 1 + x * 4;
								int num67 = i;
								num = num67;
								i = num67 + 1;
								t[num] = v + w + 1 + (x + 1) * 4;
								int num68 = i;
								num = num68;
								i = num68 + 1;
								t[num] = v + w + (x + 1) * 4;
							}
						}
						v = v + 16;
					}
					break;
				}
				case PrimitiveKind.Hole3x3:
				{
					while (true)
					{
						int num69 = num1;
						num1 = num69 - 1;
						if (num69 <= 0)
						{
							break;
						}
						for (int y = 0; y < 3; y++)
						{
							for (int a = 0; a < 3; a++)
							{
								if (y != 1 || a != 1)
								{
									int num70 = i;
									num = num70;
									i = num70 + 1;
									t[num] = v + y + a * 4;
									int num71 = i;
									num = num71;
									i = num71 + 1;
									t[num] = v + y + 1 + a * 4;
									int num72 = i;
									num = num72;
									i = num72 + 1;
									t[num] = v + y + (a + 1) * 4;
									int num73 = i;
									num = num73;
									i = num73 + 1;
									t[num] = v + y + 1 + a * 4;
									int num74 = i;
									num = num74;
									i = num74 + 1;
									t[num] = v + y + 1 + (a + 1) * 4;
									int num75 = i;
									num = num75;
									i = num75 + 1;
									t[num] = v + y + (a + 1) * 4;
								}
							}
						}
						v = v + 16;
					}
					break;
				}
				default:
				{
					throw new NotImplementedException();
				}
			}
		}

		public static int VertexCount(PrimitiveKind kind)
		{
			switch (kind)
			{
				case PrimitiveKind.Triangle:
				{
					return 3;
				}
				case PrimitiveKind.Quad:
				{
					return 4;
				}
				case PrimitiveKind.Grid2x1:
				case PrimitiveKind.Grid1x2:
				{
					return 6;
				}
				case PrimitiveKind.Grid2x2:
				{
					return 9;
				}
				case PrimitiveKind.Grid1x3:
				case PrimitiveKind.Grid3x1:
				{
					return 8;
				}
				case PrimitiveKind.Grid3x2:
				case PrimitiveKind.Grid2x3:
				{
					return 12;
				}
				case PrimitiveKind.Grid3x3:
				case PrimitiveKind.Hole3x3:
				{
					return 16;
				}
			}
			throw new NotImplementedException();
		}
	}
}