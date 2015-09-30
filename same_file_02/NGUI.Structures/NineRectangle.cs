using NGUI.Meshing;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NGUI.Structures
{
	[StructLayout(LayoutKind.Explicit)]
	public struct NineRectangle
	{
		[FieldOffset(-1)]
		private const PrimitiveKind GRID_3ROWS_3COLUMNS = PrimitiveKind.Grid3x3;

		[FieldOffset(-1)]
		private const PrimitiveKind GRID_3ROWS_2COLUMNS = PrimitiveKind.Grid2x3;

		[FieldOffset(-1)]
		private const PrimitiveKind GRID_3ROWS_1COLUMNS = PrimitiveKind.Grid1x3;

		[FieldOffset(-1)]
		private const PrimitiveKind GRID_2ROWS_3COLUMNS = PrimitiveKind.Grid3x2;

		[FieldOffset(-1)]
		private const PrimitiveKind GRID_2ROWS_2COLUMNS = PrimitiveKind.Grid2x2;

		[FieldOffset(-1)]
		private const PrimitiveKind GRID_2ROWS_1COLUMNS = PrimitiveKind.Grid1x2;

		[FieldOffset(-1)]
		private const PrimitiveKind GRID_1ROWS_3COLUMNS = PrimitiveKind.Grid3x1;

		[FieldOffset(-1)]
		private const PrimitiveKind GRID_1ROWS_2COLUMNS = PrimitiveKind.Grid2x1;

		[FieldOffset(-1)]
		private const PrimitiveKind GRID_1ROWS_1COLUMNS = PrimitiveKind.Quad;

		[FieldOffset(0)]
		public Vector2 xx;

		[FieldOffset(8)]
		public Vector2 yy;

		[FieldOffset(16)]
		public Vector2 zz;

		[FieldOffset(24)]
		public Vector2 ww;

		public Vector2 this[int i]
		{
			get
			{
				switch (i)
				{
					case 0:
					{
						return this.xx;
					}
					case 1:
					{
						return this.yy;
					}
					case 2:
					{
						return this.zz;
					}
					case 3:
					{
						return this.ww;
					}
				}
				throw new IndexOutOfRangeException();
			}
		}

		public float this[int i, int a]
		{
			get
			{
				switch (i)
				{
					case 0:
					{
						return this.xx[a];
					}
					case 1:
					{
						return this.yy[a];
					}
					case 2:
					{
						return this.zz[a];
					}
					case 3:
					{
						return this.ww[a];
					}
				}
				throw new IndexOutOfRangeException();
			}
		}

		public Vector2 wx
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.ww.x;
				vector2.y = this.xx.y;
				return vector2;
			}
			set
			{
				this.ww.x = value.x;
				this.xx.y = value.y;
			}
		}

		public Vector2 wy
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.ww.x;
				vector2.y = this.yy.y;
				return vector2;
			}
			set
			{
				this.ww.x = value.x;
				this.zz.y = value.y;
			}
		}

		public Vector2 wz
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.ww.x;
				vector2.y = this.zz.y;
				return vector2;
			}
			set
			{
				this.ww.x = value.x;
				this.zz.y = value.y;
			}
		}

		public Vector2 xw
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.xx.x;
				vector2.y = this.ww.y;
				return vector2;
			}
			set
			{
				this.xx.x = value.x;
				this.ww.y = value.y;
			}
		}

		public Vector2 xy
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.xx.x;
				vector2.y = this.yy.y;
				return vector2;
			}
			set
			{
				this.xx.x = value.x;
				this.yy.y = value.y;
			}
		}

		public Vector2 xz
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.xx.x;
				vector2.y = this.zz.y;
				return vector2;
			}
			set
			{
				this.xx.x = value.x;
				this.zz.y = value.y;
			}
		}

		public Vector2 yw
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.yy.x;
				vector2.y = this.ww.y;
				return vector2;
			}
			set
			{
				this.yy.x = value.x;
				this.ww.y = value.y;
			}
		}

		public Vector2 yx
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.yy.x;
				vector2.y = this.xx.y;
				return vector2;
			}
			set
			{
				this.yy.x = value.x;
				this.xx.y = value.y;
			}
		}

		public Vector2 yz
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.yy.x;
				vector2.y = this.zz.y;
				return vector2;
			}
			set
			{
				this.yy.x = value.x;
				this.zz.y = value.y;
			}
		}

		public Vector2 zw
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.zz.x;
				vector2.y = this.ww.y;
				return vector2;
			}
			set
			{
				this.zz.x = value.x;
				this.ww.y = value.y;
			}
		}

		public Vector2 zx
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.zz.x;
				vector2.y = this.xx.y;
				return vector2;
			}
			set
			{
				this.zz.x = value.x;
				this.xx.y = value.y;
			}
		}

		public Vector2 zy
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.zz.x;
				vector2.y = this.yy.y;
				return vector2;
			}
			set
			{
				this.zz.x = value.x;
				this.zz.y = value.y;
			}
		}

		public static void Calculate(UIWidget.Pivot pivot, float pixelSize, Texture tex, ref Vector4 minMaxX, ref Vector4 minMaxY, ref Vector2 scale, out NineRectangle nqV, out NineRectangle nqT)
		{
			nqV = new NineRectangle();
			nqT = new NineRectangle();
			Vector2 vector2 = new Vector2();
			Vector2 vector21 = new Vector2();
			float single;
			if (!tex || pixelSize == 0f)
			{
				float single1 = 0f;
				float single2 = single1;
				nqV.yy.y = single1;
				float single3 = single2;
				single2 = single3;
				nqV.xx.y = single3;
				float single4 = single2;
				single2 = single4;
				nqV.yy.x = single4;
				nqV.xx.x = single2;
				float single5 = 1f;
				single2 = single5;
				nqV.ww.x = single5;
				nqV.zz.x = single2;
				float single6 = -1f;
				single2 = single6;
				nqV.ww.y = single6;
				nqV.zz.y = single2;
				nqT = new NineRectangle();
			}
			else
			{
				float single7 = (minMaxX.y - minMaxX.x) * pixelSize;
				float single8 = (minMaxX.w - minMaxX.z) * pixelSize;
				float single9 = (minMaxY.z - minMaxY.w) * pixelSize;
				float single10 = (minMaxY.x - minMaxY.y) * pixelSize;
				if (scale.x >= 0f)
				{
					float single11 = (float)(1 / ((double)scale.x / (double)tex.width));
					vector2.x = single7 * single11;
					vector21.x = single8 * single11;
				}
				else
				{
					scale.x = 0f;
					vector2.x = single7 / 0f;
					vector21.x = single8 / 0f;
				}
				if (scale.y >= 0f)
				{
					float single12 = (float)(1 / ((double)scale.y / (double)tex.height));
					vector2.y = single9 * single12;
					vector21.y = single10 * single12;
				}
				else
				{
					scale.y = 0f;
					vector2.y = single9 / 0f;
					vector21.y = single10 / 0f;
				}
				UIWidget.Pivot pivot1 = pivot;
				switch (pivot1)
				{
					case UIWidget.Pivot.TopRight:
					case UIWidget.Pivot.Right:
					{
						single = vector21.x + vector2.x;
						if (single > 1f)
						{
							nqV.xx.x = 1f - single;
							nqV.yy.x = nqV.xx.x + vector2.x;
							nqV.ww.x = nqV.xx.x + single;
							single = 1f - vector21.x;
							float single13 = nqV.xx.x;
							nqV.zz.x = single13 + (single <= vector2.x ? vector2.x : single);
						}
						else
						{
							nqV.xx.x = 0f;
							nqV.ww.x = 1f;
							nqV.yy.x = vector2.x;
							single = 1f - vector21.x;
							nqV.zz.x = (single <= vector2.x ? vector2.x : single);
						}
						break;
					}
					default:
					{
						if (pivot1 == UIWidget.Pivot.BottomRight)
						{
							goto case UIWidget.Pivot.Right;
						}
						nqV.xx.x = 0f;
						nqV.yy.x = vector2.x;
						single = 1f - vector21.x;
						nqV.zz.x = (single <= vector2.x ? vector2.x : single);
						single = vector2.x + vector21.x;
						nqV.ww.x = (single <= 1f ? 1f : single);
						break;
					}
				}
				pivot1 = pivot;
				switch (pivot1)
				{
					case UIWidget.Pivot.BottomLeft:
					case UIWidget.Pivot.Bottom:
					case UIWidget.Pivot.BottomRight:
					{
						single = -1f - vector21.y + vector2.y;
						if (single > 0f)
						{
							nqV.xx.y = single;
							nqV.yy.y = nqV.xx.y + vector2.x;
							single = -1f - vector21.y;
							float single14 = nqV.xx.y;
							nqV.zz.y = single14 + (single >= vector2.y ? vector2.y : single);
							single = vector2.y + vector21.y;
							float single15 = nqV.xx.y;
							nqV.ww.y = single15 + (single >= -1f ? -1f : single);
						}
						else
						{
							nqV.xx.y = 0f;
							nqV.yy.y = vector2.y;
							single = -1f - vector21.y;
							nqV.zz.y = (single >= vector2.y ? vector2.y : single);
							single = vector2.y + vector21.y;
							nqV.ww.y = (single >= -1f ? -1f : single);
						}
						break;
					}
					default:
					{
						nqV.xx.y = 0f;
						nqV.yy.y = vector2.y;
						single = -1f - vector21.y;
						nqV.zz.y = (single >= vector2.y ? vector2.y : single);
						single = vector21.y + vector2.y;
						nqV.ww.y = (single >= -1f ? -1f : single);
						break;
					}
				}
				nqT.xx.x = minMaxX.x;
				nqT.yy.x = minMaxX.y;
				nqT.zz.x = minMaxX.z;
				nqT.ww.x = minMaxX.w;
				nqT.xx.y = minMaxY.w;
				nqT.yy.y = minMaxY.z;
				nqT.zz.y = minMaxY.y;
				nqT.ww.y = minMaxY.x;
			}
		}

		private static void Commit3x3(int start, ref NineRectangle nqV, ref NineRectangle nqT, ref Color color, MeshBuffer m)
		{
			m.v[start].x = nqV.xx.x;
			m.v[start].y = nqV.xx.y;
			m.v[start].u = nqT.xx.x;
			m.v[start].v = nqT.xx.y;
			Vector2 vector2 = nqV.yx;
			m.v[start + 1].x = vector2.x;
			Vector2 vector21 = nqV.yx;
			m.v[start + 1].y = vector21.y;
			Vector2 vector22 = nqT.yx;
			m.v[start + 1].u = vector22.x;
			Vector2 vector23 = nqT.yx;
			m.v[start + 1].v = vector23.y;
			Vector2 vector24 = nqV.zx;
			m.v[start + 2].x = vector24.x;
			Vector2 vector25 = nqV.zx;
			m.v[start + 2].y = vector25.y;
			Vector2 vector26 = nqT.zx;
			m.v[start + 2].u = vector26.x;
			Vector2 vector27 = nqT.zx;
			m.v[start + 2].v = vector27.y;
			Vector2 vector28 = nqV.wx;
			m.v[start + 3].x = vector28.x;
			Vector2 vector29 = nqV.wx;
			m.v[start + 3].y = vector29.y;
			Vector2 vector210 = nqT.wx;
			m.v[start + 3].u = vector210.x;
			Vector2 vector211 = nqT.wx;
			m.v[start + 3].v = vector211.y;
			Vector2 vector212 = nqV.xy;
			m.v[start + 4].x = vector212.x;
			Vector2 vector213 = nqV.xy;
			m.v[start + 4].y = vector213.y;
			Vector2 vector214 = nqT.xy;
			m.v[start + 4].u = vector214.x;
			Vector2 vector215 = nqT.xy;
			m.v[start + 4].v = vector215.y;
			m.v[start + 1 + 4].x = nqV.yy.x;
			m.v[start + 1 + 4].y = nqV.yy.y;
			m.v[start + 1 + 4].u = nqT.yy.x;
			m.v[start + 1 + 4].v = nqT.yy.y;
			Vector2 vector216 = nqV.zy;
			m.v[start + 2 + 4].x = vector216.x;
			Vector2 vector217 = nqV.zy;
			m.v[start + 2 + 4].y = vector217.y;
			Vector2 vector218 = nqT.zy;
			m.v[start + 2 + 4].u = vector218.x;
			Vector2 vector219 = nqT.zy;
			m.v[start + 2 + 4].v = vector219.y;
			Vector2 vector220 = nqV.wy;
			m.v[start + 3 + 4].x = vector220.x;
			Vector2 vector221 = nqV.wy;
			m.v[start + 3 + 4].y = vector221.y;
			Vector2 vector222 = nqT.wy;
			m.v[start + 3 + 4].u = vector222.x;
			Vector2 vector223 = nqT.wy;
			m.v[start + 3 + 4].v = vector223.y;
			Vector2 vector224 = nqV.xz;
			m.v[start + 8].x = vector224.x;
			Vector2 vector225 = nqV.xz;
			m.v[start + 8].y = vector225.y;
			Vector2 vector226 = nqT.xz;
			m.v[start + 8].u = vector226.x;
			Vector2 vector227 = nqT.xz;
			m.v[start + 8].v = vector227.y;
			Vector2 vector228 = nqV.yz;
			m.v[start + 1 + 8].x = vector228.x;
			Vector2 vector229 = nqV.yz;
			m.v[start + 1 + 8].y = vector229.y;
			Vector2 vector230 = nqT.yz;
			m.v[start + 1 + 8].u = vector230.x;
			Vector2 vector231 = nqT.yz;
			m.v[start + 1 + 8].v = vector231.y;
			m.v[start + 2 + 8].x = nqV.zz.x;
			m.v[start + 2 + 8].y = nqV.zz.y;
			m.v[start + 2 + 8].u = nqT.zz.x;
			m.v[start + 2 + 8].v = nqT.zz.y;
			Vector2 vector232 = nqV.wz;
			m.v[start + 3 + 8].x = vector232.x;
			Vector2 vector233 = nqV.wz;
			m.v[start + 3 + 8].y = vector233.y;
			Vector2 vector234 = nqT.wz;
			m.v[start + 3 + 8].u = vector234.x;
			Vector2 vector235 = nqT.wz;
			m.v[start + 3 + 8].v = vector235.y;
			Vector2 vector236 = nqV.xw;
			m.v[start + 12].x = vector236.x;
			Vector2 vector237 = nqV.xw;
			m.v[start + 12].y = vector237.y;
			Vector2 vector238 = nqT.xw;
			m.v[start + 12].u = vector238.x;
			Vector2 vector239 = nqT.xw;
			m.v[start + 12].v = vector239.y;
			Vector2 vector240 = nqV.yw;
			m.v[start + 1 + 12].x = vector240.x;
			Vector2 vector241 = nqV.yw;
			m.v[start + 1 + 12].y = vector241.y;
			Vector2 vector242 = nqT.yw;
			m.v[start + 1 + 12].u = vector242.x;
			Vector2 vector243 = nqT.yw;
			m.v[start + 1 + 12].v = vector243.y;
			Vector2 vector244 = nqV.zw;
			m.v[start + 2 + 12].x = vector244.x;
			Vector2 vector245 = nqV.zw;
			m.v[start + 2 + 12].y = vector245.y;
			Vector2 vector246 = nqT.zw;
			m.v[start + 2 + 12].u = vector246.x;
			Vector2 vector247 = nqT.zw;
			m.v[start + 2 + 12].v = vector247.y;
			m.v[start + 3 + 12].x = nqV.ww.x;
			m.v[start + 3 + 12].y = nqV.ww.y;
			m.v[start + 3 + 12].u = nqT.ww.x;
			m.v[start + 3 + 12].v = nqT.ww.y;
			for (int i = 0; i < 16; i++)
			{
				m.v[start + i].z = 0f;
				m.v[start + i].r = color.r;
				m.v[start + i].g = color.g;
				m.v[start + i].b = color.b;
				m.v[start + i].a = color.a;
			}
		}

		public static void Fill8(ref NineRectangle nqV, ref NineRectangle nqT, ref Color color, MeshBuffer m)
		{
			NineRectangle.Commit3x3(m.Alloc(PrimitiveKind.Hole3x3), ref nqV, ref nqT, ref color, m);
		}

		public static void Fill9(ref NineRectangle nqV, ref NineRectangle nqT, ref Color color, MeshBuffer m)
		{
			if (nqV.xx.x == nqV.yy.x)
			{
				if (nqV.yy.x == nqV.zz.x)
				{
					if (nqV.zz.x != nqT.ww.x)
					{
						NineRectangle.FillColumn1(ref nqV, ref nqT, 2, ref color, m);
					}
				}
				else if (nqV.zz.x != nqT.ww.x)
				{
					NineRectangle.FillColumn2(ref nqV, ref nqT, 1, ref color, m);
				}
				else
				{
					NineRectangle.FillColumn1(ref nqV, ref nqT, 1, ref color, m);
				}
			}
			else if (nqV.yy.x == nqV.zz.x)
			{
				if (nqV.zz.x != nqV.ww.x)
				{
					NineRectangle.FillColumn2(ref nqV, ref nqT, 2, ref color, m);
				}
				else
				{
					NineRectangle.FillColumn1(ref nqV, ref nqT, 1, ref color, m);
				}
			}
			else if (nqV.zz.x != nqV.ww.x)
			{
				NineRectangle.FillColumn3(ref nqV, ref nqT, ref color, m);
			}
			else
			{
				NineRectangle.FillColumn2(ref nqV, ref nqT, 0, ref color, m);
			}
		}

		private static void FillColumn1(ref NineRectangle nqV, ref NineRectangle nqT, int columnStart, ref Color color, MeshBuffer m)
		{
			if (nqV.xx.y == nqV.yy.y)
			{
				if (nqV.yy.y == nqV.zz.y)
				{
					if (nqV.zz.y != nqV.ww.y)
					{
						switch (columnStart)
						{
							case 0:
							{
								m.FastCell(nqV.xz, nqV.yw, nqT.xz, nqT.yw, ref color);
								break;
							}
							case 1:
							{
								m.FastCell(nqV.yz, nqV.zw, nqT.yz, nqT.zw, ref color);
								break;
							}
							case 2:
							{
								m.FastCell(nqV.zz, nqV.ww, nqT.zz, nqT.ww, ref color);
								break;
							}
						}
					}
				}
				else if (nqV.zz.y != nqV.ww.y)
				{
					int num = m.Alloc(PrimitiveKind.Grid1x2, 0f, color);
					switch (columnStart)
					{
						case 0:
						{
							m.v[num].x = nqV.xx.x;
							m.v[num].y = nqV.yy.y;
							m.v[num].u = nqT.xx.x;
							m.v[num].v = nqT.yy.y;
							m.v[num + 1].x = nqV.yy.x;
							m.v[num + 1].y = nqV.yy.y;
							m.v[num + 1].u = nqT.yy.x;
							m.v[num + 1].v = nqT.yy.y;
							m.v[num + 2].x = nqV.xx.x;
							m.v[num + 2].y = nqV.zz.y;
							m.v[num + 2].u = nqT.xx.x;
							m.v[num + 2].v = nqT.zz.y;
							m.v[num + 3].x = nqV.yy.x;
							m.v[num + 3].y = nqV.zz.y;
							m.v[num + 3].u = nqT.yy.x;
							m.v[num + 3].v = nqT.zz.y;
							m.v[num + 4].x = nqV.xx.x;
							m.v[num + 4].y = nqV.ww.y;
							m.v[num + 4].u = nqT.xx.x;
							m.v[num + 4].v = nqT.ww.y;
							m.v[num + 5].x = nqV.yy.x;
							m.v[num + 5].y = nqV.ww.y;
							m.v[num + 5].u = nqT.yy.x;
							m.v[num + 5].v = nqT.ww.y;
							break;
						}
						case 1:
						{
							m.v[num].x = nqV.yy.x;
							m.v[num].y = nqV.yy.y;
							m.v[num].u = nqT.yy.x;
							m.v[num].v = nqT.yy.y;
							m.v[num + 1].x = nqV.zz.x;
							m.v[num + 1].y = nqV.yy.y;
							m.v[num + 1].u = nqT.zz.x;
							m.v[num + 1].v = nqT.yy.y;
							m.v[num + 2].x = nqV.yy.x;
							m.v[num + 2].y = nqV.zz.y;
							m.v[num + 2].u = nqT.yy.x;
							m.v[num + 2].v = nqT.zz.y;
							m.v[num + 3].x = nqV.zz.x;
							m.v[num + 3].y = nqV.zz.y;
							m.v[num + 3].u = nqT.zz.x;
							m.v[num + 3].v = nqT.zz.y;
							m.v[num + 4].x = nqV.yy.x;
							m.v[num + 4].y = nqV.ww.y;
							m.v[num + 4].u = nqT.yy.x;
							m.v[num + 4].v = nqT.ww.y;
							m.v[num + 5].x = nqV.zz.x;
							m.v[num + 5].y = nqV.ww.y;
							m.v[num + 5].u = nqT.zz.x;
							m.v[num + 5].v = nqT.ww.y;
							break;
						}
						case 2:
						{
							m.v[num].x = nqV.zz.x;
							m.v[num].y = nqV.yy.y;
							m.v[num].u = nqT.zz.x;
							m.v[num].v = nqT.yy.y;
							m.v[num + 1].x = nqV.ww.x;
							m.v[num + 1].y = nqV.yy.y;
							m.v[num + 1].u = nqT.ww.x;
							m.v[num + 1].v = nqT.yy.y;
							m.v[num + 2].x = nqV.zz.x;
							m.v[num + 2].y = nqV.zz.y;
							m.v[num + 2].u = nqT.zz.x;
							m.v[num + 2].v = nqT.zz.y;
							m.v[num + 3].x = nqV.ww.x;
							m.v[num + 3].y = nqV.zz.y;
							m.v[num + 3].u = nqT.ww.x;
							m.v[num + 3].v = nqT.zz.y;
							m.v[num + 4].x = nqV.zz.x;
							m.v[num + 4].y = nqV.ww.y;
							m.v[num + 4].u = nqT.zz.x;
							m.v[num + 4].v = nqT.ww.y;
							m.v[num + 5].x = nqV.ww.x;
							m.v[num + 5].y = nqV.ww.y;
							m.v[num + 5].u = nqT.ww.x;
							m.v[num + 5].v = nqT.ww.y;
							break;
						}
					}
				}
				else
				{
					switch (columnStart)
					{
						case 0:
						{
							m.FastCell(nqV.xy, nqV.yz, nqT.xy, nqT.yz, ref color);
							break;
						}
						case 1:
						{
							m.FastCell(nqV.yy, nqV.zz, nqT.yy, nqT.zz, ref color);
							break;
						}
						case 2:
						{
							m.FastCell(nqV.zy, nqV.wz, nqT.zy, nqT.wz, ref color);
							break;
						}
					}
				}
			}
			else if (nqV.yy.y == nqV.zz.y)
			{
				if (nqV.zz.y != nqV.ww.y)
				{
					switch (columnStart)
					{
						case 0:
						{
							m.FastCell(nqV.xx, nqV.yy, nqT.xx, nqT.yy, ref color);
							m.FastCell(nqV.xz, nqV.yw, nqT.xz, nqT.yw, ref color);
							break;
						}
						case 1:
						{
							m.FastCell(nqV.yx, nqV.zy, nqT.yx, nqT.zy, ref color);
							m.FastCell(nqV.yz, nqV.zw, nqT.yz, nqT.zw, ref color);
							break;
						}
						case 2:
						{
							m.FastCell(nqV.zx, nqV.wy, nqT.zx, nqT.wy, ref color);
							m.FastCell(nqV.zz, nqV.ww, nqT.zz, nqT.ww, ref color);
							break;
						}
					}
				}
				else
				{
					switch (columnStart)
					{
						case 0:
						{
							m.FastCell(nqV.xx, nqV.yy, nqT.xx, nqT.yy, ref color);
							break;
						}
						case 1:
						{
							m.FastCell(nqV.yx, nqV.zy, nqT.yx, nqT.zy, ref color);
							break;
						}
						case 2:
						{
							m.FastCell(nqV.zx, nqV.wy, nqT.zx, nqT.wy, ref color);
							break;
						}
					}
				}
			}
			else if (nqV.zz.y != nqV.ww.y)
			{
				int num1 = m.Alloc(PrimitiveKind.Grid1x2, 0f, color);
				switch (columnStart)
				{
					case 0:
					{
						m.v[num1].x = nqV.xx.x;
						m.v[num1].y = nqV.xx.y;
						m.v[num1].u = nqT.xx.x;
						m.v[num1].v = nqT.xx.y;
						m.v[num1 + 1].x = nqV.yy.x;
						m.v[num1 + 1].y = nqV.xx.y;
						m.v[num1 + 1].u = nqT.yy.x;
						m.v[num1 + 1].v = nqT.xx.y;
						m.v[num1 + 2].x = nqV.xx.x;
						m.v[num1 + 2].y = nqV.yy.y;
						m.v[num1 + 2].u = nqT.xx.x;
						m.v[num1 + 2].v = nqT.yy.y;
						m.v[num1 + 3].x = nqV.yy.x;
						m.v[num1 + 3].y = nqV.yy.y;
						m.v[num1 + 3].u = nqT.yy.x;
						m.v[num1 + 3].v = nqT.yy.y;
						m.v[num1 + 4].x = nqV.xx.x;
						m.v[num1 + 4].y = nqV.zz.y;
						m.v[num1 + 4].u = nqT.xx.x;
						m.v[num1 + 4].v = nqT.zz.y;
						m.v[num1 + 5].x = nqV.yy.x;
						m.v[num1 + 5].y = nqV.zz.y;
						m.v[num1 + 5].u = nqT.yy.x;
						m.v[num1 + 5].v = nqT.zz.y;
						m.v[num1 + 6].x = nqV.xx.x;
						m.v[num1 + 6].y = nqV.ww.y;
						m.v[num1 + 6].u = nqT.xx.x;
						m.v[num1 + 6].v = nqT.ww.y;
						m.v[num1 + 7].x = nqV.yy.x;
						m.v[num1 + 7].y = nqV.ww.y;
						m.v[num1 + 7].u = nqT.yy.x;
						m.v[num1 + 7].v = nqT.ww.y;
						break;
					}
					case 1:
					{
						m.v[num1].x = nqV.yy.x;
						m.v[num1].y = nqV.xx.y;
						m.v[num1].u = nqT.yy.x;
						m.v[num1].v = nqT.xx.y;
						m.v[num1 + 1].x = nqV.zz.x;
						m.v[num1 + 1].y = nqV.xx.y;
						m.v[num1 + 1].u = nqT.zz.x;
						m.v[num1 + 1].v = nqT.xx.y;
						m.v[num1 + 2].x = nqV.yy.x;
						m.v[num1 + 2].y = nqV.yy.y;
						m.v[num1 + 2].u = nqT.yy.x;
						m.v[num1 + 2].v = nqT.yy.y;
						m.v[num1 + 3].x = nqV.zz.x;
						m.v[num1 + 3].y = nqV.yy.y;
						m.v[num1 + 3].u = nqT.zz.x;
						m.v[num1 + 3].v = nqT.yy.y;
						m.v[num1 + 4].x = nqV.yy.x;
						m.v[num1 + 4].y = nqV.zz.y;
						m.v[num1 + 4].u = nqT.yy.x;
						m.v[num1 + 4].v = nqT.zz.y;
						m.v[num1 + 5].x = nqV.zz.x;
						m.v[num1 + 5].y = nqV.zz.y;
						m.v[num1 + 5].u = nqT.zz.x;
						m.v[num1 + 5].v = nqT.zz.y;
						m.v[num1 + 6].x = nqV.yy.x;
						m.v[num1 + 6].y = nqV.ww.y;
						m.v[num1 + 6].u = nqT.yy.x;
						m.v[num1 + 6].v = nqT.ww.y;
						m.v[num1 + 7].x = nqV.zz.x;
						m.v[num1 + 7].y = nqV.ww.y;
						m.v[num1 + 7].u = nqT.zz.x;
						m.v[num1 + 7].v = nqT.ww.y;
						break;
					}
					case 2:
					{
						m.v[num1].x = nqV.zz.x;
						m.v[num1].y = nqV.xx.y;
						m.v[num1].u = nqT.zz.x;
						m.v[num1].v = nqT.xx.y;
						m.v[num1 + 1].x = nqV.ww.x;
						m.v[num1 + 1].y = nqV.xx.y;
						m.v[num1 + 1].u = nqT.ww.x;
						m.v[num1 + 1].v = nqT.xx.y;
						m.v[num1 + 2].x = nqV.zz.x;
						m.v[num1 + 2].y = nqV.yy.y;
						m.v[num1 + 2].u = nqT.zz.x;
						m.v[num1 + 2].v = nqT.yy.y;
						m.v[num1 + 3].x = nqV.ww.x;
						m.v[num1 + 3].y = nqV.yy.y;
						m.v[num1 + 3].u = nqT.ww.x;
						m.v[num1 + 3].v = nqT.yy.y;
						m.v[num1 + 4].x = nqV.zz.x;
						m.v[num1 + 4].y = nqV.zz.y;
						m.v[num1 + 4].u = nqT.zz.x;
						m.v[num1 + 4].v = nqT.zz.y;
						m.v[num1 + 5].x = nqV.ww.x;
						m.v[num1 + 5].y = nqV.zz.y;
						m.v[num1 + 5].u = nqT.ww.x;
						m.v[num1 + 5].v = nqT.zz.y;
						m.v[num1 + 6].x = nqV.zz.x;
						m.v[num1 + 6].y = nqV.ww.y;
						m.v[num1 + 6].u = nqT.zz.x;
						m.v[num1 + 6].v = nqT.ww.y;
						m.v[num1 + 7].x = nqV.ww.x;
						m.v[num1 + 7].y = nqV.ww.y;
						m.v[num1 + 7].u = nqT.ww.x;
						m.v[num1 + 7].v = nqT.ww.y;
						break;
					}
				}
			}
			else
			{
				int num2 = m.Alloc(PrimitiveKind.Grid1x2, 0f, color);
				switch (columnStart)
				{
					case 0:
					{
						m.v[num2].x = nqV.xx.x;
						m.v[num2].y = nqV.xx.y;
						m.v[num2].u = nqT.xx.x;
						m.v[num2].v = nqT.xx.y;
						m.v[num2 + 1].x = nqV.yy.x;
						m.v[num2 + 1].y = nqV.xx.y;
						m.v[num2 + 1].u = nqT.yy.x;
						m.v[num2 + 1].v = nqT.xx.y;
						m.v[num2 + 2].x = nqV.xx.x;
						m.v[num2 + 2].y = nqV.yy.y;
						m.v[num2 + 2].u = nqT.xx.x;
						m.v[num2 + 2].v = nqT.yy.y;
						m.v[num2 + 3].x = nqV.yy.x;
						m.v[num2 + 3].y = nqV.yy.y;
						m.v[num2 + 3].u = nqT.yy.x;
						m.v[num2 + 3].v = nqT.yy.y;
						m.v[num2 + 4].x = nqV.xx.x;
						m.v[num2 + 4].y = nqV.zz.y;
						m.v[num2 + 4].u = nqT.xx.x;
						m.v[num2 + 4].v = nqT.zz.y;
						m.v[num2 + 5].x = nqV.yy.x;
						m.v[num2 + 5].y = nqV.zz.y;
						m.v[num2 + 5].u = nqT.yy.x;
						m.v[num2 + 5].v = nqT.zz.y;
						break;
					}
					case 1:
					{
						m.v[num2].x = nqV.yy.x;
						m.v[num2].y = nqV.xx.y;
						m.v[num2].u = nqT.yy.x;
						m.v[num2].v = nqT.xx.y;
						m.v[num2 + 1].x = nqV.zz.x;
						m.v[num2 + 1].y = nqV.xx.y;
						m.v[num2 + 1].u = nqT.zz.x;
						m.v[num2 + 1].v = nqT.xx.y;
						m.v[num2 + 2].x = nqV.yy.x;
						m.v[num2 + 2].y = nqV.yy.y;
						m.v[num2 + 2].u = nqT.yy.x;
						m.v[num2 + 2].v = nqT.yy.y;
						m.v[num2 + 3].x = nqV.zz.x;
						m.v[num2 + 3].y = nqV.yy.y;
						m.v[num2 + 3].u = nqT.zz.x;
						m.v[num2 + 3].v = nqT.yy.y;
						m.v[num2 + 4].x = nqV.yy.x;
						m.v[num2 + 4].y = nqV.zz.y;
						m.v[num2 + 4].u = nqT.yy.x;
						m.v[num2 + 4].v = nqT.zz.y;
						m.v[num2 + 5].x = nqV.zz.x;
						m.v[num2 + 5].y = nqV.zz.y;
						m.v[num2 + 5].u = nqT.zz.x;
						m.v[num2 + 5].v = nqT.zz.y;
						break;
					}
					case 2:
					{
						m.v[num2].x = nqV.zz.x;
						m.v[num2].y = nqV.xx.y;
						m.v[num2].u = nqT.zz.x;
						m.v[num2].v = nqT.xx.y;
						m.v[num2 + 1].x = nqV.ww.x;
						m.v[num2 + 1].y = nqV.xx.y;
						m.v[num2 + 1].u = nqT.ww.x;
						m.v[num2 + 1].v = nqT.xx.y;
						m.v[num2 + 2].x = nqV.zz.x;
						m.v[num2 + 2].y = nqV.yy.y;
						m.v[num2 + 2].u = nqT.zz.x;
						m.v[num2 + 2].v = nqT.yy.y;
						m.v[num2 + 3].x = nqV.ww.x;
						m.v[num2 + 3].y = nqV.yy.y;
						m.v[num2 + 3].u = nqT.ww.x;
						m.v[num2 + 3].v = nqT.yy.y;
						m.v[num2 + 4].x = nqV.zz.x;
						m.v[num2 + 4].y = nqV.zz.y;
						m.v[num2 + 4].u = nqT.zz.x;
						m.v[num2 + 4].v = nqT.zz.y;
						m.v[num2 + 5].x = nqV.ww.x;
						m.v[num2 + 5].y = nqV.zz.y;
						m.v[num2 + 5].u = nqT.ww.x;
						m.v[num2 + 5].v = nqT.zz.y;
						break;
					}
				}
			}
		}

		private static void FillColumn2(ref NineRectangle nqV, ref NineRectangle nqT, int columnStart, ref Color color, MeshBuffer m)
		{
			int num;
			int num1;
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			if (nqV.xx.y == nqV.yy.y)
			{
				if (nqV.yy.y == nqV.zz.y)
				{
					if (nqV.zz.y != nqV.ww.y)
					{
						switch (columnStart)
						{
							case 0:
							{
								num = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
								m.v[num].x = nqV.xx.x;
								m.v[num].y = nqV.zz.y;
								m.v[num].u = nqT.xx.x;
								m.v[num].v = nqT.zz.y;
								m.v[num + 1].x = nqV.yy.x;
								m.v[num + 1].y = nqV.zz.y;
								m.v[num + 1].u = nqT.yy.x;
								m.v[num + 1].v = nqT.zz.y;
								m.v[num + 2].x = nqV.zz.x;
								m.v[num + 2].y = nqV.zz.y;
								m.v[num + 2].u = nqT.zz.x;
								m.v[num + 2].v = nqT.zz.y;
								m.v[num + 3].x = nqV.xx.x;
								m.v[num + 3].y = nqV.ww.y;
								m.v[num + 3].u = nqT.xx.x;
								m.v[num + 3].v = nqT.ww.y;
								m.v[num + 4].x = nqV.yy.x;
								m.v[num + 4].y = nqV.ww.y;
								m.v[num + 4].u = nqT.yy.x;
								m.v[num + 4].v = nqT.ww.y;
								m.v[num + 5].x = nqV.zz.x;
								m.v[num + 5].y = nqV.ww.y;
								m.v[num + 5].u = nqT.zz.x;
								m.v[num + 5].v = nqT.ww.y;
								break;
							}
							case 1:
							{
								num = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
								m.v[num].x = nqV.yy.x;
								m.v[num].y = nqV.zz.y;
								m.v[num].u = nqT.yy.x;
								m.v[num].v = nqT.zz.y;
								m.v[num + 1].x = nqV.zz.x;
								m.v[num + 1].y = nqV.zz.y;
								m.v[num + 1].u = nqT.zz.x;
								m.v[num + 1].v = nqT.zz.y;
								m.v[num + 2].x = nqV.ww.x;
								m.v[num + 2].y = nqV.zz.y;
								m.v[num + 2].u = nqT.ww.x;
								m.v[num + 2].v = nqT.zz.y;
								m.v[num + 3].x = nqV.yy.x;
								m.v[num + 3].y = nqV.ww.y;
								m.v[num + 3].u = nqT.yy.x;
								m.v[num + 3].v = nqT.ww.y;
								m.v[num + 4].x = nqV.zz.x;
								m.v[num + 4].y = nqV.ww.y;
								m.v[num + 4].u = nqT.zz.x;
								m.v[num + 4].v = nqT.ww.y;
								m.v[num + 5].x = nqV.ww.x;
								m.v[num + 5].y = nqV.ww.y;
								m.v[num + 5].u = nqT.ww.x;
								m.v[num + 5].v = nqT.ww.y;
								break;
							}
							case 2:
							{
								m.FastCell(nqV.xz, nqV.yw, nqT.xz, nqT.yw, ref color);
								m.FastCell(nqV.zz, nqV.ww, nqT.zz, nqT.ww, ref color);
								break;
							}
						}
					}
				}
				else if (nqV.zz.y != nqV.ww.y)
				{
					switch (columnStart)
					{
						case 0:
						{
							num2 = m.Alloc(PrimitiveKind.Grid2x2, 0f, color);
							m.v[num2].x = nqV.xx.x;
							m.v[num2].y = nqV.yy.y;
							m.v[num2].u = nqT.xx.x;
							m.v[num2].v = nqT.yy.y;
							m.v[num2 + 1].x = nqV.yy.x;
							m.v[num2 + 1].y = nqV.yy.y;
							m.v[num2 + 1].u = nqT.yy.x;
							m.v[num2 + 1].v = nqT.yy.y;
							m.v[num2 + 2].x = nqV.zz.x;
							m.v[num2 + 2].y = nqV.yy.y;
							m.v[num2 + 2].u = nqT.zz.x;
							m.v[num2 + 2].v = nqT.yy.y;
							m.v[num2 + 3].x = nqV.xx.x;
							m.v[num2 + 3].y = nqV.zz.y;
							m.v[num2 + 3].u = nqT.xx.x;
							m.v[num2 + 3].v = nqT.zz.y;
							m.v[num2 + 4].x = nqV.yy.x;
							m.v[num2 + 4].y = nqV.zz.y;
							m.v[num2 + 4].u = nqT.yy.x;
							m.v[num2 + 4].v = nqT.zz.y;
							m.v[num2 + 5].x = nqV.zz.x;
							m.v[num2 + 5].y = nqV.zz.y;
							m.v[num2 + 5].u = nqT.zz.x;
							m.v[num2 + 5].v = nqT.zz.y;
							m.v[num2 + 6].x = nqV.xx.x;
							m.v[num2 + 6].y = nqV.ww.y;
							m.v[num2 + 6].u = nqT.xx.x;
							m.v[num2 + 6].v = nqT.ww.y;
							m.v[num2 + 7].x = nqV.yy.x;
							m.v[num2 + 7].y = nqV.ww.y;
							m.v[num2 + 7].u = nqT.yy.x;
							m.v[num2 + 7].v = nqT.ww.y;
							m.v[num2 + 8].x = nqV.zz.x;
							m.v[num2 + 8].y = nqV.ww.y;
							m.v[num2 + 8].u = nqT.zz.x;
							m.v[num2 + 8].v = nqT.ww.y;
							break;
						}
						case 1:
						{
							num2 = m.Alloc(PrimitiveKind.Grid2x2, 0f, color);
							m.v[num2].x = nqV.yy.x;
							m.v[num2].y = nqV.yy.y;
							m.v[num2].u = nqT.yy.x;
							m.v[num2].v = nqT.yy.y;
							m.v[num2 + 1].x = nqV.zz.x;
							m.v[num2 + 1].y = nqV.yy.y;
							m.v[num2 + 1].u = nqT.zz.x;
							m.v[num2 + 1].v = nqT.yy.y;
							m.v[num2 + 2].x = nqV.ww.x;
							m.v[num2 + 2].y = nqV.yy.y;
							m.v[num2 + 2].u = nqT.ww.x;
							m.v[num2 + 2].v = nqT.yy.y;
							m.v[num2 + 3].x = nqV.yy.x;
							m.v[num2 + 3].y = nqV.zz.y;
							m.v[num2 + 3].u = nqT.yy.x;
							m.v[num2 + 3].v = nqT.zz.y;
							m.v[num2 + 4].x = nqV.zz.x;
							m.v[num2 + 4].y = nqV.zz.y;
							m.v[num2 + 4].u = nqT.zz.x;
							m.v[num2 + 4].v = nqT.zz.y;
							m.v[num2 + 5].x = nqV.ww.x;
							m.v[num2 + 5].y = nqV.zz.y;
							m.v[num2 + 5].u = nqT.ww.x;
							m.v[num2 + 5].v = nqT.zz.y;
							m.v[num2 + 6].x = nqV.yy.x;
							m.v[num2 + 6].y = nqV.ww.y;
							m.v[num2 + 6].u = nqT.yy.x;
							m.v[num2 + 6].v = nqT.ww.y;
							m.v[num2 + 7].x = nqV.zz.x;
							m.v[num2 + 7].y = nqV.ww.y;
							m.v[num2 + 7].u = nqT.zz.x;
							m.v[num2 + 7].v = nqT.ww.y;
							m.v[num2 + 8].x = nqV.ww.x;
							m.v[num2 + 8].y = nqV.ww.y;
							m.v[num2 + 8].u = nqT.ww.x;
							m.v[num2 + 8].v = nqT.ww.y;
							break;
						}
						case 2:
						{
							num2 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num2].x = nqV.xx.x;
							m.v[num2].y = nqV.yy.y;
							m.v[num2].u = nqT.xx.x;
							m.v[num2].v = nqT.yy.y;
							m.v[num2 + 1].x = nqV.yy.x;
							m.v[num2 + 1].y = nqV.yy.y;
							m.v[num2 + 1].u = nqT.yy.x;
							m.v[num2 + 1].v = nqT.yy.y;
							m.v[num2 + 2].x = nqV.xx.x;
							m.v[num2 + 2].y = nqV.zz.y;
							m.v[num2 + 2].u = nqT.xx.x;
							m.v[num2 + 2].v = nqT.zz.y;
							m.v[num2 + 3].x = nqV.yy.x;
							m.v[num2 + 3].y = nqV.zz.y;
							m.v[num2 + 3].u = nqT.yy.x;
							m.v[num2 + 3].v = nqT.zz.y;
							m.v[num2 + 4].x = nqV.yy.x;
							m.v[num2 + 4].y = nqV.ww.y;
							m.v[num2 + 4].u = nqT.yy.x;
							m.v[num2 + 4].v = nqT.ww.y;
							m.v[num2 + 5].x = nqV.zz.x;
							m.v[num2 + 5].y = nqV.ww.y;
							m.v[num2 + 5].u = nqT.zz.x;
							m.v[num2 + 5].v = nqT.ww.y;
							num2 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num2].x = nqV.zz.x;
							m.v[num2].y = nqV.yy.y;
							m.v[num2].u = nqT.zz.x;
							m.v[num2].v = nqT.yy.y;
							m.v[num2 + 1].x = nqV.ww.x;
							m.v[num2 + 1].y = nqV.yy.y;
							m.v[num2 + 1].u = nqT.ww.x;
							m.v[num2 + 1].v = nqT.yy.y;
							m.v[num2 + 2].x = nqV.zz.x;
							m.v[num2 + 2].y = nqV.zz.y;
							m.v[num2 + 2].u = nqT.zz.x;
							m.v[num2 + 2].v = nqT.zz.y;
							m.v[num2 + 3].x = nqV.ww.x;
							m.v[num2 + 3].y = nqV.zz.y;
							m.v[num2 + 3].u = nqT.ww.x;
							m.v[num2 + 3].v = nqT.zz.y;
							m.v[num2 + 4].x = nqV.zz.x;
							m.v[num2 + 4].y = nqV.ww.y;
							m.v[num2 + 4].u = nqT.zz.x;
							m.v[num2 + 4].v = nqT.ww.y;
							m.v[num2 + 5].x = nqV.ww.x;
							m.v[num2 + 5].y = nqV.ww.y;
							m.v[num2 + 5].u = nqT.ww.x;
							m.v[num2 + 5].v = nqT.ww.y;
							break;
						}
					}
				}
				else
				{
					switch (columnStart)
					{
						case 0:
						{
							num1 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num1].x = nqV.xx.x;
							m.v[num1].y = nqV.yy.y;
							m.v[num1].u = nqT.xx.x;
							m.v[num1].v = nqT.yy.y;
							m.v[num1 + 1].x = nqV.yy.x;
							m.v[num1 + 1].y = nqV.yy.y;
							m.v[num1 + 1].u = nqT.yy.x;
							m.v[num1 + 1].v = nqT.yy.y;
							m.v[num1 + 2].x = nqV.zz.x;
							m.v[num1 + 2].y = nqV.yy.y;
							m.v[num1 + 2].u = nqT.zz.x;
							m.v[num1 + 2].v = nqT.yy.y;
							m.v[num1 + 3].x = nqV.xx.x;
							m.v[num1 + 3].y = nqV.zz.y;
							m.v[num1 + 3].u = nqT.xx.x;
							m.v[num1 + 3].v = nqT.zz.y;
							m.v[num1 + 4].x = nqV.yy.x;
							m.v[num1 + 4].y = nqV.zz.y;
							m.v[num1 + 4].u = nqT.yy.x;
							m.v[num1 + 4].v = nqT.zz.y;
							m.v[num1 + 5].x = nqV.zz.x;
							m.v[num1 + 5].y = nqV.zz.y;
							m.v[num1 + 5].u = nqT.zz.x;
							m.v[num1 + 5].v = nqT.zz.y;
							break;
						}
						case 1:
						{
							num1 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num1].x = nqV.yy.x;
							m.v[num1].y = nqV.yy.y;
							m.v[num1].u = nqT.yy.x;
							m.v[num1].v = nqT.yy.y;
							m.v[num1 + 1].x = nqV.zz.x;
							m.v[num1 + 1].y = nqV.yy.y;
							m.v[num1 + 1].u = nqT.zz.x;
							m.v[num1 + 1].v = nqT.yy.y;
							m.v[num1 + 2].x = nqV.ww.x;
							m.v[num1 + 2].y = nqV.yy.y;
							m.v[num1 + 2].u = nqT.ww.x;
							m.v[num1 + 2].v = nqT.yy.y;
							m.v[num1 + 3].x = nqV.yy.x;
							m.v[num1 + 3].y = nqV.zz.y;
							m.v[num1 + 3].u = nqT.yy.x;
							m.v[num1 + 3].v = nqT.zz.y;
							m.v[num1 + 4].x = nqV.zz.x;
							m.v[num1 + 4].y = nqV.zz.y;
							m.v[num1 + 4].u = nqT.zz.x;
							m.v[num1 + 4].v = nqT.zz.y;
							m.v[num1 + 5].x = nqV.ww.x;
							m.v[num1 + 5].y = nqV.zz.y;
							m.v[num1 + 5].u = nqT.ww.x;
							m.v[num1 + 5].v = nqT.zz.y;
							break;
						}
						case 2:
						{
							m.FastCell(nqV.xy, nqV.yz, nqT.xy, nqT.yz, ref color);
							m.FastCell(nqV.zy, nqV.wz, nqT.zy, nqT.wz, ref color);
							break;
						}
					}
				}
			}
			else if (nqV.yy.y == nqV.zz.y)
			{
				if (nqV.zz.y != nqV.ww.y)
				{
					switch (columnStart)
					{
						case 0:
						{
							num4 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num4].x = nqV.xx.x;
							m.v[num4].y = nqV.xx.y;
							m.v[num4].u = nqT.xx.x;
							m.v[num4].v = nqT.xx.y;
							m.v[num4 + 1].x = nqV.yy.x;
							m.v[num4 + 1].y = nqV.xx.y;
							m.v[num4 + 1].u = nqT.yy.x;
							m.v[num4 + 1].v = nqT.xx.y;
							m.v[num4 + 2].x = nqV.zz.x;
							m.v[num4 + 2].y = nqV.xx.y;
							m.v[num4 + 2].u = nqT.zz.x;
							m.v[num4 + 2].v = nqT.xx.y;
							m.v[num4 + 3].x = nqV.xx.x;
							m.v[num4 + 3].y = nqV.yy.y;
							m.v[num4 + 3].u = nqT.xx.x;
							m.v[num4 + 3].v = nqT.yy.y;
							m.v[num4 + 4].x = nqV.yy.x;
							m.v[num4 + 4].y = nqV.yy.y;
							m.v[num4 + 4].u = nqT.yy.x;
							m.v[num4 + 4].v = nqT.yy.y;
							m.v[num4 + 5].x = nqV.zz.x;
							m.v[num4 + 5].y = nqV.yy.y;
							m.v[num4 + 5].u = nqT.zz.x;
							m.v[num4 + 5].v = nqT.yy.y;
							num4 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num4].x = nqV.xx.x;
							m.v[num4].y = nqV.zz.y;
							m.v[num4].u = nqT.xx.x;
							m.v[num4].v = nqT.zz.y;
							m.v[num4 + 1].x = nqV.yy.x;
							m.v[num4 + 1].y = nqV.zz.y;
							m.v[num4 + 1].u = nqT.yy.x;
							m.v[num4 + 1].v = nqT.zz.y;
							m.v[num4 + 2].x = nqV.zz.x;
							m.v[num4 + 2].y = nqV.zz.y;
							m.v[num4 + 2].u = nqT.zz.x;
							m.v[num4 + 2].v = nqT.zz.y;
							m.v[num4 + 3].x = nqV.xx.x;
							m.v[num4 + 3].y = nqV.ww.y;
							m.v[num4 + 3].u = nqT.xx.x;
							m.v[num4 + 3].v = nqT.ww.y;
							m.v[num4 + 4].x = nqV.yy.x;
							m.v[num4 + 4].y = nqV.ww.y;
							m.v[num4 + 4].u = nqT.yy.x;
							m.v[num4 + 4].v = nqT.ww.y;
							m.v[num4 + 5].x = nqV.zz.x;
							m.v[num4 + 5].y = nqV.ww.y;
							m.v[num4 + 5].u = nqT.zz.x;
							m.v[num4 + 5].v = nqT.ww.y;
							break;
						}
						case 1:
						{
							num4 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num4].x = nqV.yy.x;
							m.v[num4].y = nqV.xx.y;
							m.v[num4].u = nqT.yy.x;
							m.v[num4].v = nqT.xx.y;
							m.v[num4 + 1].x = nqV.zz.x;
							m.v[num4 + 1].y = nqV.xx.y;
							m.v[num4 + 1].u = nqT.zz.x;
							m.v[num4 + 1].v = nqT.xx.y;
							m.v[num4 + 2].x = nqV.ww.x;
							m.v[num4 + 2].y = nqV.xx.y;
							m.v[num4 + 2].u = nqT.ww.x;
							m.v[num4 + 2].v = nqT.xx.y;
							m.v[num4 + 3].x = nqV.yy.x;
							m.v[num4 + 3].y = nqV.yy.y;
							m.v[num4 + 3].u = nqT.yy.x;
							m.v[num4 + 3].v = nqT.yy.y;
							m.v[num4 + 4].x = nqV.zz.x;
							m.v[num4 + 4].y = nqV.yy.y;
							m.v[num4 + 4].u = nqT.zz.x;
							m.v[num4 + 4].v = nqT.yy.y;
							m.v[num4 + 5].x = nqV.ww.x;
							m.v[num4 + 5].y = nqV.yy.y;
							m.v[num4 + 5].u = nqT.ww.x;
							m.v[num4 + 5].v = nqT.yy.y;
							num4 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num4].x = nqV.yy.x;
							m.v[num4].y = nqV.zz.y;
							m.v[num4].u = nqT.yy.x;
							m.v[num4].v = nqT.zz.y;
							m.v[num4 + 1].x = nqV.zz.x;
							m.v[num4 + 1].y = nqV.zz.y;
							m.v[num4 + 1].u = nqT.zz.x;
							m.v[num4 + 1].v = nqT.zz.y;
							m.v[num4 + 2].x = nqV.ww.x;
							m.v[num4 + 2].y = nqV.zz.y;
							m.v[num4 + 2].u = nqT.ww.x;
							m.v[num4 + 2].v = nqT.zz.y;
							m.v[num4 + 3].x = nqV.yy.x;
							m.v[num4 + 3].y = nqV.ww.y;
							m.v[num4 + 3].u = nqT.yy.x;
							m.v[num4 + 3].v = nqT.ww.y;
							m.v[num4 + 4].x = nqV.zz.x;
							m.v[num4 + 4].y = nqV.ww.y;
							m.v[num4 + 4].u = nqT.zz.x;
							m.v[num4 + 4].v = nqT.ww.y;
							m.v[num4 + 5].x = nqV.ww.x;
							m.v[num4 + 5].y = nqV.ww.y;
							m.v[num4 + 5].u = nqT.ww.x;
							m.v[num4 + 5].v = nqT.ww.y;
							break;
						}
						case 2:
						{
							m.FastCell(nqV.xx, nqV.yy, nqT.xx, nqT.yy, ref color);
							m.FastCell(nqV.zx, nqV.wy, nqT.zx, nqT.wy, ref color);
							m.FastCell(nqV.xz, nqV.yw, nqT.xz, nqT.yw, ref color);
							m.FastCell(nqV.zz, nqV.ww, nqT.zz, nqT.ww, ref color);
							break;
						}
					}
				}
				else
				{
					switch (columnStart)
					{
						case 0:
						{
							num3 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num3].x = nqV.xx.x;
							m.v[num3].y = nqV.xx.y;
							m.v[num3].u = nqT.xx.x;
							m.v[num3].v = nqT.xx.y;
							m.v[num3 + 1].x = nqV.yy.x;
							m.v[num3 + 1].y = nqV.xx.y;
							m.v[num3 + 1].u = nqT.yy.x;
							m.v[num3 + 1].v = nqT.xx.y;
							m.v[num3 + 2].x = nqV.zz.x;
							m.v[num3 + 2].y = nqV.xx.y;
							m.v[num3 + 2].u = nqT.zz.x;
							m.v[num3 + 2].v = nqT.xx.y;
							m.v[num3 + 3].x = nqV.xx.x;
							m.v[num3 + 3].y = nqV.yy.y;
							m.v[num3 + 3].u = nqT.xx.x;
							m.v[num3 + 3].v = nqT.yy.y;
							m.v[num3 + 4].x = nqV.yy.x;
							m.v[num3 + 4].y = nqV.yy.y;
							m.v[num3 + 4].u = nqT.yy.x;
							m.v[num3 + 4].v = nqT.yy.y;
							m.v[num3 + 5].x = nqV.zz.x;
							m.v[num3 + 5].y = nqV.yy.y;
							m.v[num3 + 5].u = nqT.zz.x;
							m.v[num3 + 5].v = nqT.yy.y;
							break;
						}
						case 1:
						{
							num3 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
							m.v[num3].x = nqV.yy.x;
							m.v[num3].y = nqV.xx.y;
							m.v[num3].u = nqT.yy.x;
							m.v[num3].v = nqT.xx.y;
							m.v[num3 + 1].x = nqV.zz.x;
							m.v[num3 + 1].y = nqV.xx.y;
							m.v[num3 + 1].u = nqT.zz.x;
							m.v[num3 + 1].v = nqT.xx.y;
							m.v[num3 + 2].x = nqV.ww.x;
							m.v[num3 + 2].y = nqV.xx.y;
							m.v[num3 + 2].u = nqT.ww.x;
							m.v[num3 + 2].v = nqT.xx.y;
							m.v[num3 + 3].x = nqV.yy.x;
							m.v[num3 + 3].y = nqV.yy.y;
							m.v[num3 + 3].u = nqT.yy.x;
							m.v[num3 + 3].v = nqT.yy.y;
							m.v[num3 + 4].x = nqV.zz.x;
							m.v[num3 + 4].y = nqV.yy.y;
							m.v[num3 + 4].u = nqT.zz.x;
							m.v[num3 + 4].v = nqT.yy.y;
							m.v[num3 + 5].x = nqV.ww.x;
							m.v[num3 + 5].y = nqV.yy.y;
							m.v[num3 + 5].u = nqT.ww.x;
							m.v[num3 + 5].v = nqT.yy.y;
							break;
						}
						case 2:
						{
							m.FastCell(nqV.xx, nqV.yy, nqT.xx, nqT.yy, ref color);
							m.FastCell(nqV.zx, nqV.wy, nqT.zx, nqT.wy, ref color);
							break;
						}
					}
				}
			}
			else if (nqV.zz.y != nqV.ww.y)
			{
				switch (columnStart)
				{
					case 0:
					{
						num6 = m.Alloc(PrimitiveKind.Grid2x3, 0f, color);
						m.v[num6].x = nqV.xx.x;
						m.v[num6].y = nqV.xx.y;
						m.v[num6].u = nqT.xx.x;
						m.v[num6].v = nqT.xx.y;
						m.v[num6 + 1].x = nqV.yy.x;
						m.v[num6 + 1].y = nqV.xx.y;
						m.v[num6 + 1].u = nqT.yy.x;
						m.v[num6 + 1].v = nqT.xx.y;
						m.v[num6 + 2].x = nqV.zz.x;
						m.v[num6 + 2].y = nqV.xx.y;
						m.v[num6 + 2].u = nqT.zz.x;
						m.v[num6 + 2].v = nqT.xx.y;
						m.v[num6 + 3].x = nqV.xx.x;
						m.v[num6 + 3].y = nqV.yy.y;
						m.v[num6 + 3].u = nqT.xx.x;
						m.v[num6 + 3].v = nqT.yy.y;
						m.v[num6 + 4].x = nqV.yy.x;
						m.v[num6 + 4].y = nqV.yy.y;
						m.v[num6 + 4].u = nqT.yy.x;
						m.v[num6 + 4].v = nqT.yy.y;
						m.v[num6 + 5].x = nqV.zz.x;
						m.v[num6 + 5].y = nqV.yy.y;
						m.v[num6 + 5].u = nqT.zz.x;
						m.v[num6 + 5].v = nqT.yy.y;
						m.v[num6 + 6].x = nqV.xx.x;
						m.v[num6 + 6].y = nqV.zz.y;
						m.v[num6 + 6].u = nqT.xx.x;
						m.v[num6 + 6].v = nqT.zz.y;
						m.v[num6 + 7].x = nqV.yy.x;
						m.v[num6 + 7].y = nqV.zz.y;
						m.v[num6 + 7].u = nqT.yy.x;
						m.v[num6 + 7].v = nqT.zz.y;
						m.v[num6 + 8].x = nqV.zz.x;
						m.v[num6 + 8].y = nqV.zz.y;
						m.v[num6 + 8].u = nqT.zz.x;
						m.v[num6 + 8].v = nqT.zz.y;
						m.v[num6 + 9].x = nqV.xx.x;
						m.v[num6 + 9].y = nqV.ww.y;
						m.v[num6 + 9].u = nqT.xx.x;
						m.v[num6 + 9].v = nqT.ww.y;
						m.v[num6 + 10].x = nqV.yy.x;
						m.v[num6 + 10].y = nqV.ww.y;
						m.v[num6 + 10].u = nqT.yy.x;
						m.v[num6 + 10].v = nqT.ww.y;
						m.v[num6 + 11].x = nqV.zz.x;
						m.v[num6 + 11].y = nqV.ww.y;
						m.v[num6 + 11].u = nqT.zz.x;
						m.v[num6 + 11].v = nqT.ww.y;
						break;
					}
					case 1:
					{
						num6 = m.Alloc(PrimitiveKind.Grid2x3, 0f, color);
						m.v[num6].x = nqV.yy.x;
						m.v[num6].y = nqV.xx.y;
						m.v[num6].u = nqT.yy.x;
						m.v[num6].v = nqT.xx.y;
						m.v[num6 + 1].x = nqV.zz.x;
						m.v[num6 + 1].y = nqV.xx.y;
						m.v[num6 + 1].u = nqT.zz.x;
						m.v[num6 + 1].v = nqT.xx.y;
						m.v[num6 + 2].x = nqV.ww.x;
						m.v[num6 + 2].y = nqV.xx.y;
						m.v[num6 + 2].u = nqT.ww.x;
						m.v[num6 + 2].v = nqT.xx.y;
						m.v[num6 + 3].x = nqV.yy.x;
						m.v[num6 + 3].y = nqV.yy.y;
						m.v[num6 + 3].u = nqT.yy.x;
						m.v[num6 + 3].v = nqT.yy.y;
						m.v[num6 + 4].x = nqV.zz.x;
						m.v[num6 + 4].y = nqV.yy.y;
						m.v[num6 + 4].u = nqT.zz.x;
						m.v[num6 + 4].v = nqT.yy.y;
						m.v[num6 + 5].x = nqV.ww.x;
						m.v[num6 + 5].y = nqV.yy.y;
						m.v[num6 + 5].u = nqT.ww.x;
						m.v[num6 + 5].v = nqT.yy.y;
						m.v[num6 + 6].x = nqV.yy.x;
						m.v[num6 + 6].y = nqV.zz.y;
						m.v[num6 + 6].u = nqT.yy.x;
						m.v[num6 + 6].v = nqT.zz.y;
						m.v[num6 + 7].x = nqV.zz.x;
						m.v[num6 + 7].y = nqV.zz.y;
						m.v[num6 + 7].u = nqT.zz.x;
						m.v[num6 + 7].v = nqT.zz.y;
						m.v[num6 + 8].x = nqV.ww.x;
						m.v[num6 + 8].y = nqV.zz.y;
						m.v[num6 + 8].u = nqT.ww.x;
						m.v[num6 + 8].v = nqT.zz.y;
						m.v[num6 + 9].x = nqV.yy.x;
						m.v[num6 + 9].y = nqV.ww.y;
						m.v[num6 + 9].u = nqT.yy.x;
						m.v[num6 + 9].v = nqT.ww.y;
						m.v[num6 + 10].x = nqV.zz.x;
						m.v[num6 + 10].y = nqV.ww.y;
						m.v[num6 + 10].u = nqT.zz.x;
						m.v[num6 + 10].v = nqT.ww.y;
						m.v[num6 + 11].x = nqV.ww.x;
						m.v[num6 + 11].y = nqV.ww.y;
						m.v[num6 + 11].u = nqT.ww.x;
						m.v[num6 + 11].v = nqT.ww.y;
						break;
					}
					case 2:
					{
						num6 = m.Alloc(PrimitiveKind.Grid1x3, 0f, color);
						m.v[num6].x = nqV.xx.x;
						m.v[num6].y = nqV.xx.y;
						m.v[num6].u = nqT.xx.x;
						m.v[num6].v = nqT.xx.y;
						m.v[num6 + 1].x = nqV.yy.x;
						m.v[num6 + 1].y = nqV.xx.y;
						m.v[num6 + 1].u = nqT.yy.x;
						m.v[num6 + 1].v = nqT.xx.y;
						m.v[num6 + 2].x = nqV.xx.x;
						m.v[num6 + 2].y = nqV.yy.y;
						m.v[num6 + 2].u = nqT.xx.x;
						m.v[num6 + 2].v = nqT.yy.y;
						m.v[num6 + 3].x = nqV.yy.x;
						m.v[num6 + 3].y = nqV.yy.y;
						m.v[num6 + 3].u = nqT.yy.x;
						m.v[num6 + 3].v = nqT.yy.y;
						m.v[num6 + 4].x = nqV.xx.x;
						m.v[num6 + 4].y = nqV.zz.y;
						m.v[num6 + 4].u = nqT.xx.x;
						m.v[num6 + 4].v = nqT.zz.y;
						m.v[num6 + 5].x = nqV.yy.x;
						m.v[num6 + 5].y = nqV.zz.y;
						m.v[num6 + 5].u = nqT.yy.x;
						m.v[num6 + 5].v = nqT.zz.y;
						m.v[num6 + 6].x = nqV.xx.x;
						m.v[num6 + 6].y = nqV.ww.y;
						m.v[num6 + 6].u = nqT.xx.x;
						m.v[num6 + 6].v = nqT.ww.y;
						m.v[num6 + 7].x = nqV.yy.x;
						m.v[num6 + 7].y = nqV.ww.y;
						m.v[num6 + 7].u = nqT.yy.x;
						m.v[num6 + 7].v = nqT.ww.y;
						num6 = m.Alloc(PrimitiveKind.Grid1x3, 0f, color);
						m.v[num6].x = nqV.zz.x;
						m.v[num6].y = nqV.xx.y;
						m.v[num6].u = nqT.zz.x;
						m.v[num6].v = nqT.xx.y;
						m.v[num6 + 1].x = nqV.ww.x;
						m.v[num6 + 1].y = nqV.xx.y;
						m.v[num6 + 1].u = nqT.ww.x;
						m.v[num6 + 1].v = nqT.xx.y;
						m.v[num6 + 2].x = nqV.zz.x;
						m.v[num6 + 2].y = nqV.yy.y;
						m.v[num6 + 2].u = nqT.zz.x;
						m.v[num6 + 2].v = nqT.yy.y;
						m.v[num6 + 3].x = nqV.ww.x;
						m.v[num6 + 3].y = nqV.yy.y;
						m.v[num6 + 3].u = nqT.ww.x;
						m.v[num6 + 3].v = nqT.yy.y;
						m.v[num6 + 4].x = nqV.zz.x;
						m.v[num6 + 4].y = nqV.zz.y;
						m.v[num6 + 4].u = nqT.zz.x;
						m.v[num6 + 4].v = nqT.zz.y;
						m.v[num6 + 5].x = nqV.ww.x;
						m.v[num6 + 5].y = nqV.zz.y;
						m.v[num6 + 5].u = nqT.ww.x;
						m.v[num6 + 5].v = nqT.zz.y;
						m.v[num6 + 6].x = nqV.zz.x;
						m.v[num6 + 6].y = nqV.ww.y;
						m.v[num6 + 6].u = nqT.zz.x;
						m.v[num6 + 6].v = nqT.ww.y;
						m.v[num6 + 7].x = nqV.ww.x;
						m.v[num6 + 7].y = nqV.ww.y;
						m.v[num6 + 7].u = nqT.ww.x;
						m.v[num6 + 7].v = nqT.ww.y;
						break;
					}
				}
			}
			else
			{
				switch (columnStart)
				{
					case 0:
					{
						num5 = m.Alloc(PrimitiveKind.Grid2x2, 0f, color);
						m.v[num5].x = nqV.xx.x;
						m.v[num5].y = nqV.xx.y;
						m.v[num5].u = nqT.xx.x;
						m.v[num5].v = nqT.xx.y;
						m.v[num5 + 1].x = nqV.yy.x;
						m.v[num5 + 1].y = nqV.xx.y;
						m.v[num5 + 1].u = nqT.yy.x;
						m.v[num5 + 1].v = nqT.xx.y;
						m.v[num5 + 2].x = nqV.zz.x;
						m.v[num5 + 2].y = nqV.xx.y;
						m.v[num5 + 2].u = nqT.zz.x;
						m.v[num5 + 2].v = nqT.xx.y;
						m.v[num5 + 3].x = nqV.xx.x;
						m.v[num5 + 3].y = nqV.yy.y;
						m.v[num5 + 3].u = nqT.xx.x;
						m.v[num5 + 3].v = nqT.zz.y;
						m.v[num5 + 4].x = nqV.yy.x;
						m.v[num5 + 4].y = nqV.yy.y;
						m.v[num5 + 4].u = nqT.yy.x;
						m.v[num5 + 4].v = nqT.yy.y;
						m.v[num5 + 5].x = nqV.zz.x;
						m.v[num5 + 5].y = nqV.yy.y;
						m.v[num5 + 5].u = nqT.zz.x;
						m.v[num5 + 5].v = nqT.yy.y;
						m.v[num5 + 6].x = nqV.xx.x;
						m.v[num5 + 6].y = nqV.zz.y;
						m.v[num5 + 6].u = nqT.xx.x;
						m.v[num5 + 6].v = nqT.zz.y;
						m.v[num5 + 7].x = nqV.yy.x;
						m.v[num5 + 7].y = nqV.zz.y;
						m.v[num5 + 7].u = nqT.yy.x;
						m.v[num5 + 7].v = nqT.zz.y;
						m.v[num5 + 8].x = nqV.zz.x;
						m.v[num5 + 8].y = nqV.zz.y;
						m.v[num5 + 8].u = nqT.zz.x;
						m.v[num5 + 8].v = nqT.zz.y;
						break;
					}
					case 1:
					{
						num5 = m.Alloc(PrimitiveKind.Grid2x2, 0f, color);
						m.v[num5].x = nqV.yy.x;
						m.v[num5].y = nqV.xx.y;
						m.v[num5].u = nqT.yy.x;
						m.v[num5].v = nqT.xx.y;
						m.v[num5 + 1].x = nqV.zz.x;
						m.v[num5 + 1].y = nqV.xx.y;
						m.v[num5 + 1].u = nqT.zz.x;
						m.v[num5 + 1].v = nqT.xx.y;
						m.v[num5 + 2].x = nqV.ww.x;
						m.v[num5 + 2].y = nqV.xx.y;
						m.v[num5 + 2].u = nqT.ww.x;
						m.v[num5 + 2].v = nqT.xx.y;
						m.v[num5 + 3].x = nqV.yy.x;
						m.v[num5 + 3].y = nqV.yy.y;
						m.v[num5 + 3].u = nqT.yy.x;
						m.v[num5 + 3].v = nqT.yy.y;
						m.v[num5 + 4].x = nqV.zz.x;
						m.v[num5 + 4].y = nqV.yy.y;
						m.v[num5 + 4].u = nqT.zz.x;
						m.v[num5 + 4].v = nqT.yy.y;
						m.v[num5 + 5].x = nqV.ww.x;
						m.v[num5 + 5].y = nqV.yy.y;
						m.v[num5 + 5].u = nqT.ww.x;
						m.v[num5 + 5].v = nqT.yy.y;
						m.v[num5 + 6].x = nqV.yy.x;
						m.v[num5 + 6].y = nqV.zz.y;
						m.v[num5 + 6].u = nqT.yy.x;
						m.v[num5 + 6].v = nqT.zz.y;
						m.v[num5 + 7].x = nqV.zz.x;
						m.v[num5 + 7].y = nqV.zz.y;
						m.v[num5 + 7].u = nqT.zz.x;
						m.v[num5 + 7].v = nqT.zz.y;
						m.v[num5 + 8].x = nqV.ww.x;
						m.v[num5 + 8].y = nqV.zz.y;
						m.v[num5 + 8].u = nqT.ww.x;
						m.v[num5 + 8].v = nqT.zz.y;
						break;
					}
					case 2:
					{
						num5 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
						m.v[num5].x = nqV.xx.x;
						m.v[num5].y = nqV.xx.y;
						m.v[num5].u = nqT.xx.x;
						m.v[num5].v = nqT.xx.y;
						m.v[num5 + 1].x = nqV.yy.x;
						m.v[num5 + 1].y = nqV.xx.y;
						m.v[num5 + 1].u = nqT.yy.x;
						m.v[num5 + 1].v = nqT.xx.y;
						m.v[num5 + 2].x = nqV.xx.x;
						m.v[num5 + 2].y = nqV.yy.y;
						m.v[num5 + 2].u = nqT.xx.x;
						m.v[num5 + 2].v = nqT.yy.y;
						m.v[num5 + 3].x = nqV.yy.x;
						m.v[num5 + 3].y = nqV.yy.y;
						m.v[num5 + 3].u = nqT.yy.x;
						m.v[num5 + 3].v = nqT.yy.y;
						m.v[num5 + 4].x = nqV.yy.x;
						m.v[num5 + 4].y = nqV.zz.y;
						m.v[num5 + 4].u = nqT.yy.x;
						m.v[num5 + 4].v = nqT.zz.y;
						m.v[num5 + 5].x = nqV.zz.x;
						m.v[num5 + 5].y = nqV.zz.y;
						m.v[num5 + 5].u = nqT.zz.x;
						m.v[num5 + 5].v = nqT.zz.y;
						num5 = m.Alloc(PrimitiveKind.Grid2x1, 0f, color);
						m.v[num5].x = nqV.zz.x;
						m.v[num5].y = nqV.xx.y;
						m.v[num5].u = nqT.zz.x;
						m.v[num5].v = nqT.xx.y;
						m.v[num5 + 1].x = nqV.ww.x;
						m.v[num5 + 1].y = nqV.xx.y;
						m.v[num5 + 1].u = nqT.ww.x;
						m.v[num5 + 1].v = nqT.xx.y;
						m.v[num5 + 2].x = nqV.zz.x;
						m.v[num5 + 2].y = nqV.yy.y;
						m.v[num5 + 2].u = nqT.zz.x;
						m.v[num5 + 2].v = nqT.yy.y;
						m.v[num5 + 3].x = nqV.ww.x;
						m.v[num5 + 3].y = nqV.yy.y;
						m.v[num5 + 3].u = nqT.ww.x;
						m.v[num5 + 3].v = nqT.yy.y;
						m.v[num5 + 4].x = nqV.zz.x;
						m.v[num5 + 4].y = nqV.zz.y;
						m.v[num5 + 4].u = nqT.zz.x;
						m.v[num5 + 4].v = nqT.zz.y;
						m.v[num5 + 5].x = nqV.ww.x;
						m.v[num5 + 5].y = nqV.zz.y;
						m.v[num5 + 5].u = nqT.ww.x;
						m.v[num5 + 5].v = nqT.zz.y;
						break;
					}
				}
			}
		}

		private static void FillColumn3(ref NineRectangle nqV, ref NineRectangle nqT, ref Color color, MeshBuffer m)
		{
			if (nqV.xx.y == nqV.yy.y)
			{
				if (nqV.yy.y == nqV.zz.y)
				{
					if (nqV.zz.y != nqV.ww.y)
					{
						int num = m.Alloc(PrimitiveKind.Grid3x1, 0f, color);
						m.v[num].x = nqV.xx.x;
						m.v[num].y = nqV.zz.y;
						m.v[num].u = nqT.xx.x;
						m.v[num].v = nqT.zz.y;
						m.v[num + 1].x = nqV.yy.x;
						m.v[num + 1].y = nqV.zz.y;
						m.v[num + 1].u = nqT.yy.x;
						m.v[num + 1].v = nqT.zz.y;
						m.v[num + 2].x = nqV.zz.x;
						m.v[num + 2].y = nqV.zz.y;
						m.v[num + 2].u = nqT.zz.x;
						m.v[num + 2].v = nqT.zz.y;
						m.v[num + 3].x = nqV.ww.x;
						m.v[num + 3].y = nqV.zz.y;
						m.v[num + 3].u = nqT.ww.x;
						m.v[num + 3].v = nqT.zz.y;
						m.v[num + 4].x = nqV.xx.x;
						m.v[num + 4].y = nqV.ww.y;
						m.v[num + 4].u = nqT.xx.x;
						m.v[num + 4].v = nqT.ww.y;
						m.v[num + 5].x = nqV.yy.x;
						m.v[num + 5].y = nqV.ww.y;
						m.v[num + 5].u = nqT.yy.x;
						m.v[num + 5].v = nqT.ww.y;
						m.v[num + 6].x = nqV.zz.x;
						m.v[num + 6].y = nqV.ww.y;
						m.v[num + 6].u = nqT.zz.x;
						m.v[num + 6].v = nqT.ww.y;
						m.v[num + 7].x = nqV.ww.x;
						m.v[num + 7].y = nqV.ww.y;
						m.v[num + 7].u = nqT.ww.x;
						m.v[num + 7].v = nqT.ww.y;
					}
				}
				else if (nqV.zz.y != nqV.ww.y)
				{
					int num1 = m.Alloc(PrimitiveKind.Grid3x2, 0f, color);
					m.v[num1].x = nqV.xx.x;
					m.v[num1].y = nqV.yy.y;
					m.v[num1].u = nqT.xx.x;
					m.v[num1].v = nqT.yy.y;
					m.v[num1 + 1].x = nqV.yy.x;
					m.v[num1 + 1].y = nqV.yy.y;
					m.v[num1 + 1].u = nqT.yy.x;
					m.v[num1 + 1].v = nqT.yy.y;
					m.v[num1 + 2].x = nqV.zz.x;
					m.v[num1 + 2].y = nqV.yy.y;
					m.v[num1 + 2].u = nqT.zz.x;
					m.v[num1 + 2].v = nqT.yy.y;
					m.v[num1 + 3].x = nqV.ww.x;
					m.v[num1 + 3].y = nqV.yy.y;
					m.v[num1 + 3].u = nqT.ww.x;
					m.v[num1 + 3].v = nqT.yy.y;
					m.v[num1 + 4].x = nqV.xx.x;
					m.v[num1 + 4].y = nqV.zz.y;
					m.v[num1 + 4].u = nqT.xx.x;
					m.v[num1 + 4].v = nqT.zz.y;
					m.v[num1 + 5].x = nqV.yy.x;
					m.v[num1 + 5].y = nqV.zz.y;
					m.v[num1 + 5].u = nqT.yy.x;
					m.v[num1 + 5].v = nqT.zz.y;
					m.v[num1 + 6].x = nqV.zz.x;
					m.v[num1 + 6].y = nqV.zz.y;
					m.v[num1 + 6].u = nqT.zz.x;
					m.v[num1 + 6].v = nqT.zz.y;
					m.v[num1 + 7].x = nqV.ww.x;
					m.v[num1 + 7].y = nqV.zz.y;
					m.v[num1 + 7].u = nqT.ww.x;
					m.v[num1 + 7].v = nqT.zz.y;
					m.v[num1 + 8].x = nqV.xx.x;
					m.v[num1 + 8].y = nqV.ww.y;
					m.v[num1 + 8].u = nqT.xx.x;
					m.v[num1 + 8].v = nqT.ww.y;
					m.v[num1 + 9].x = nqV.yy.x;
					m.v[num1 + 9].y = nqV.ww.y;
					m.v[num1 + 9].u = nqT.yy.x;
					m.v[num1 + 9].v = nqT.ww.y;
					m.v[num1 + 10].x = nqV.zz.x;
					m.v[num1 + 10].y = nqV.ww.y;
					m.v[num1 + 10].u = nqT.zz.x;
					m.v[num1 + 10].v = nqT.ww.y;
					m.v[num1 + 11].x = nqV.ww.x;
					m.v[num1 + 11].y = nqV.ww.y;
					m.v[num1 + 11].u = nqT.ww.x;
					m.v[num1 + 11].v = nqT.ww.y;
				}
				else
				{
					int num2 = m.Alloc(PrimitiveKind.Grid3x1, 0f, color);
					m.v[num2].x = nqV.xx.x;
					m.v[num2].y = nqV.yy.y;
					m.v[num2].u = nqT.xx.x;
					m.v[num2].v = nqT.yy.y;
					m.v[num2 + 1].x = nqV.yy.x;
					m.v[num2 + 1].y = nqV.yy.y;
					m.v[num2 + 1].u = nqT.yy.x;
					m.v[num2 + 1].v = nqT.yy.y;
					m.v[num2 + 2].x = nqV.zz.x;
					m.v[num2 + 2].y = nqV.yy.y;
					m.v[num2 + 2].u = nqT.zz.x;
					m.v[num2 + 2].v = nqT.yy.y;
					m.v[num2 + 3].x = nqV.ww.x;
					m.v[num2 + 3].y = nqV.yy.y;
					m.v[num2 + 3].u = nqT.ww.x;
					m.v[num2 + 3].v = nqT.yy.y;
					m.v[num2 + 4].x = nqV.xx.x;
					m.v[num2 + 4].y = nqV.zz.y;
					m.v[num2 + 4].u = nqT.xx.x;
					m.v[num2 + 4].v = nqT.zz.y;
					m.v[num2 + 5].x = nqV.yy.x;
					m.v[num2 + 5].y = nqV.zz.y;
					m.v[num2 + 5].u = nqT.yy.x;
					m.v[num2 + 5].v = nqT.zz.y;
					m.v[num2 + 6].x = nqV.zz.x;
					m.v[num2 + 6].y = nqV.zz.y;
					m.v[num2 + 6].u = nqT.zz.x;
					m.v[num2 + 6].v = nqT.zz.y;
					m.v[num2 + 7].x = nqV.ww.x;
					m.v[num2 + 7].y = nqV.zz.y;
					m.v[num2 + 7].u = nqT.ww.x;
					m.v[num2 + 7].v = nqT.zz.y;
				}
			}
			else if (nqV.yy.y == nqV.zz.y)
			{
				if (nqV.zz.y != nqV.ww.y)
				{
					int num3 = m.Alloc(PrimitiveKind.Grid3x1, 0f, color);
					m.v[num3].x = nqV.xx.x;
					m.v[num3].y = nqV.xx.y;
					m.v[num3].u = nqT.xx.x;
					m.v[num3].v = nqT.xx.y;
					m.v[num3 + 1].x = nqV.yy.x;
					m.v[num3 + 1].y = nqV.xx.y;
					m.v[num3 + 1].u = nqT.yy.x;
					m.v[num3 + 1].v = nqT.xx.y;
					m.v[num3 + 2].x = nqV.zz.x;
					m.v[num3 + 2].y = nqV.xx.y;
					m.v[num3 + 2].u = nqT.zz.x;
					m.v[num3 + 2].v = nqT.xx.y;
					m.v[num3 + 3].x = nqV.ww.x;
					m.v[num3 + 3].y = nqV.xx.y;
					m.v[num3 + 3].u = nqT.ww.x;
					m.v[num3 + 3].v = nqT.xx.y;
					m.v[num3 + 4].x = nqV.xx.x;
					m.v[num3 + 4].y = nqV.yy.y;
					m.v[num3 + 4].u = nqT.xx.x;
					m.v[num3 + 4].v = nqT.yy.y;
					m.v[num3 + 5].x = nqV.yy.x;
					m.v[num3 + 5].y = nqV.yy.y;
					m.v[num3 + 5].u = nqT.yy.x;
					m.v[num3 + 5].v = nqT.yy.y;
					m.v[num3 + 6].x = nqV.zz.x;
					m.v[num3 + 6].y = nqV.yy.y;
					m.v[num3 + 6].u = nqT.zz.x;
					m.v[num3 + 6].v = nqT.yy.y;
					m.v[num3 + 7].x = nqV.ww.x;
					m.v[num3 + 7].y = nqV.yy.y;
					m.v[num3 + 7].u = nqT.ww.x;
					m.v[num3 + 7].v = nqT.yy.y;
					num3 = m.Alloc(PrimitiveKind.Grid3x1, 0f, color);
					m.v[num3].x = nqV.xx.x;
					m.v[num3].y = nqV.zz.y;
					m.v[num3].u = nqT.xx.x;
					m.v[num3].v = nqT.zz.y;
					m.v[num3 + 1].x = nqV.yy.x;
					m.v[num3 + 1].y = nqV.zz.y;
					m.v[num3 + 1].u = nqT.yy.x;
					m.v[num3 + 1].v = nqT.zz.y;
					m.v[num3 + 2].x = nqV.zz.x;
					m.v[num3 + 2].y = nqV.zz.y;
					m.v[num3 + 2].u = nqT.zz.x;
					m.v[num3 + 2].v = nqT.zz.y;
					m.v[num3 + 3].x = nqV.ww.x;
					m.v[num3 + 3].y = nqV.zz.y;
					m.v[num3 + 3].u = nqT.ww.x;
					m.v[num3 + 3].v = nqT.zz.y;
					m.v[num3 + 4].x = nqV.xx.x;
					m.v[num3 + 4].y = nqV.ww.y;
					m.v[num3 + 4].u = nqT.xx.x;
					m.v[num3 + 4].v = nqT.ww.y;
					m.v[num3 + 5].x = nqV.yy.x;
					m.v[num3 + 5].y = nqV.ww.y;
					m.v[num3 + 5].u = nqT.yy.x;
					m.v[num3 + 5].v = nqT.ww.y;
					m.v[num3 + 6].x = nqV.zz.x;
					m.v[num3 + 6].y = nqV.ww.y;
					m.v[num3 + 6].u = nqT.zz.x;
					m.v[num3 + 6].v = nqT.ww.y;
					m.v[num3 + 7].x = nqV.ww.x;
					m.v[num3 + 7].y = nqV.ww.y;
					m.v[num3 + 7].u = nqT.ww.x;
					m.v[num3 + 7].v = nqT.ww.y;
				}
				else
				{
					int num4 = m.Alloc(PrimitiveKind.Grid3x1, 0f, color);
					m.v[num4].x = nqV.xx.x;
					m.v[num4].y = nqV.xx.y;
					m.v[num4].u = nqT.xx.x;
					m.v[num4].v = nqT.xx.y;
					m.v[num4 + 1].x = nqV.yy.x;
					m.v[num4 + 1].y = nqV.xx.y;
					m.v[num4 + 1].u = nqT.yy.x;
					m.v[num4 + 1].v = nqT.xx.y;
					m.v[num4 + 2].x = nqV.zz.x;
					m.v[num4 + 2].y = nqV.xx.y;
					m.v[num4 + 2].u = nqT.zz.x;
					m.v[num4 + 2].v = nqT.xx.y;
					m.v[num4 + 3].x = nqV.ww.x;
					m.v[num4 + 3].y = nqV.xx.y;
					m.v[num4 + 3].u = nqT.ww.x;
					m.v[num4 + 3].v = nqT.xx.y;
					m.v[num4 + 4].x = nqV.xx.x;
					m.v[num4 + 4].y = nqV.yy.y;
					m.v[num4 + 4].u = nqT.xx.x;
					m.v[num4 + 4].v = nqT.yy.y;
					m.v[num4 + 5].x = nqV.yy.x;
					m.v[num4 + 5].y = nqV.yy.y;
					m.v[num4 + 5].u = nqT.yy.x;
					m.v[num4 + 5].v = nqT.yy.y;
					m.v[num4 + 6].x = nqV.zz.x;
					m.v[num4 + 6].y = nqV.yy.y;
					m.v[num4 + 6].u = nqT.zz.x;
					m.v[num4 + 6].v = nqT.yy.y;
					m.v[num4 + 7].x = nqV.ww.x;
					m.v[num4 + 7].y = nqV.yy.y;
					m.v[num4 + 7].u = nqT.ww.x;
					m.v[num4 + 7].v = nqT.yy.y;
				}
			}
			else if (nqV.zz.y != nqV.ww.y)
			{
				NineRectangle.Commit3x3(m.Alloc(PrimitiveKind.Grid3x3), ref nqV, ref nqT, ref color, m);
			}
			else
			{
				int num5 = m.Alloc(PrimitiveKind.Grid3x2, 0f, color);
				m.v[num5].x = nqV.xx.x;
				m.v[num5].y = nqV.xx.y;
				m.v[num5].u = nqT.xx.x;
				m.v[num5].v = nqT.xx.y;
				m.v[num5 + 1].x = nqV.yy.x;
				m.v[num5 + 1].y = nqV.xx.y;
				m.v[num5 + 1].u = nqT.yy.x;
				m.v[num5 + 1].v = nqT.xx.y;
				m.v[num5 + 2].x = nqV.zz.x;
				m.v[num5 + 2].y = nqV.xx.y;
				m.v[num5 + 2].u = nqT.zz.x;
				m.v[num5 + 2].v = nqT.xx.y;
				m.v[num5 + 3].x = nqV.ww.x;
				m.v[num5 + 3].y = nqV.xx.y;
				m.v[num5 + 3].u = nqT.ww.x;
				m.v[num5 + 3].v = nqT.xx.y;
				m.v[num5 + 4].x = nqV.xx.x;
				m.v[num5 + 4].y = nqV.yy.y;
				m.v[num5 + 4].u = nqT.xx.x;
				m.v[num5 + 4].v = nqT.yy.y;
				m.v[num5 + 5].x = nqV.yy.x;
				m.v[num5 + 5].y = nqV.yy.y;
				m.v[num5 + 5].u = nqT.yy.x;
				m.v[num5 + 5].v = nqT.yy.y;
				m.v[num5 + 6].x = nqV.zz.x;
				m.v[num5 + 6].y = nqV.yy.y;
				m.v[num5 + 6].u = nqT.zz.x;
				m.v[num5 + 6].v = nqT.yy.y;
				m.v[num5 + 7].x = nqV.ww.x;
				m.v[num5 + 7].y = nqV.yy.y;
				m.v[num5 + 7].u = nqT.ww.x;
				m.v[num5 + 7].v = nqT.yy.y;
				m.v[num5 + 8].x = nqV.xx.x;
				m.v[num5 + 8].y = nqV.zz.y;
				m.v[num5 + 8].u = nqT.xx.x;
				m.v[num5 + 8].v = nqT.zz.y;
				m.v[num5 + 9].x = nqV.yy.x;
				m.v[num5 + 9].y = nqV.zz.y;
				m.v[num5 + 9].u = nqT.yy.x;
				m.v[num5 + 9].v = nqT.zz.y;
				m.v[num5 + 10].x = nqV.zz.x;
				m.v[num5 + 10].y = nqV.zz.y;
				m.v[num5 + 10].u = nqT.zz.x;
				m.v[num5 + 10].v = nqT.zz.y;
				m.v[num5 + 11].x = nqV.ww.x;
				m.v[num5 + 11].y = nqV.zz.y;
				m.v[num5 + 11].u = nqT.ww.x;
				m.v[num5 + 11].v = nqT.zz.y;
			}
		}
	}
}