using NGUI.Meshing;
using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Filled)")]
[ExecuteInEditMode]
public class UIFilledSprite : UISprite
{
	[HideInInspector]
	[SerializeField]
	private UIFilledSprite.FillDirection mFillDirection = UIFilledSprite.FillDirection.Radial360;

	[HideInInspector]
	[SerializeField]
	private float mFillAmount = 1f;

	[HideInInspector]
	[SerializeField]
	private bool mInvert;

	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float single = Mathf.Clamp01(value);
			if (this.mFillAmount != single)
			{
				this.mFillAmount = single;
				base.ChangedAuto();
			}
		}
	}

	public UIFilledSprite.FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				base.ChangedAuto();
			}
		}
	}

	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				base.ChangedAuto();
			}
		}
	}

	public UIFilledSprite()
	{
	}

	private bool AdjustRadial(Vector2[] xy, Vector2[] uv, float fill, bool invert)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float single = Mathf.Clamp01(fill);
		if (!invert)
		{
			single = 1f - single;
		}
		single = single * 1.57079637f;
		float single1 = Mathf.Sin(single);
		float single2 = Mathf.Cos(single);
		if (single1 > single2)
		{
			single2 = single2 * (1f / single1);
			single1 = 1f;
			if (!invert)
			{
				xy[0].y = Mathf.Lerp(xy[2].y, xy[0].y, single2);
				xy[3].y = xy[0].y;
				uv[0].y = Mathf.Lerp(uv[2].y, uv[0].y, single2);
				uv[3].y = uv[0].y;
			}
		}
		else if (single2 <= single1)
		{
			single1 = 1f;
			single2 = 1f;
		}
		else
		{
			single1 = single1 * (1f / single2);
			single2 = 1f;
			if (invert)
			{
				xy[0].x = Mathf.Lerp(xy[2].x, xy[0].x, single1);
				xy[1].x = xy[0].x;
				uv[0].x = Mathf.Lerp(uv[2].x, uv[0].x, single1);
				uv[1].x = uv[0].x;
			}
		}
		if (!invert)
		{
			xy[3].x = Mathf.Lerp(xy[2].x, xy[0].x, single1);
			uv[3].x = Mathf.Lerp(uv[2].x, uv[0].x, single1);
		}
		else
		{
			xy[1].y = Mathf.Lerp(xy[2].y, xy[0].y, single2);
			uv[1].y = Mathf.Lerp(uv[2].y, uv[0].y, single2);
		}
		return true;
	}

	public override void OnFill(MeshBuffer m)
	{
		float single;
		float single1;
		Vertex vertex = new Vertex();
		Vertex vertex1 = new Vertex();
		Vertex vertex2 = new Vertex();
		Vertex vertex3 = new Vertex();
		float single2 = 0f;
		float single3 = 0f;
		float single4 = 1f;
		float single5 = -1f;
		float single6 = this.mOuterUV.xMin;
		float single7 = this.mOuterUV.yMin;
		float single8 = this.mOuterUV.xMax;
		float single9 = this.mOuterUV.yMax;
		if (this.mFillDirection == UIFilledSprite.FillDirection.Horizontal || this.mFillDirection == UIFilledSprite.FillDirection.Vertical)
		{
			float single10 = (single8 - single6) * this.mFillAmount;
			float single11 = (single9 - single7) * this.mFillAmount;
			if (this.fillDirection == UIFilledSprite.FillDirection.Horizontal)
			{
				if (!this.mInvert)
				{
					single4 = single4 * this.mFillAmount;
					single8 = single6 + single10;
				}
				else
				{
					single2 = 1f - this.mFillAmount;
					single6 = single8 - single10;
				}
			}
			else if (this.fillDirection == UIFilledSprite.FillDirection.Vertical)
			{
				if (!this.mInvert)
				{
					single3 = -(1f - this.mFillAmount);
					single9 = single7 + single11;
				}
				else
				{
					single5 = single5 * this.mFillAmount;
					single7 = single9 - single11;
				}
			}
		}
		Vector2[] vector2 = new Vector2[4];
		Vector2[] vector2Array = new Vector2[4];
		vector2[0] = new Vector2(single4, single3);
		vector2[1] = new Vector2(single4, single5);
		vector2[2] = new Vector2(single2, single5);
		vector2[3] = new Vector2(single2, single3);
		vector2Array[0] = new Vector2(single8, single9);
		vector2Array[1] = new Vector2(single8, single7);
		vector2Array[2] = new Vector2(single6, single7);
		vector2Array[3] = new Vector2(single6, single9);
		Color color = base.color;
		if (this.fillDirection != UIFilledSprite.FillDirection.Radial90)
		{
			if (this.fillDirection == UIFilledSprite.FillDirection.Radial180)
			{
				Vector2[] vector21 = new Vector2[4];
				Vector2[] vector2Array1 = new Vector2[4];
				for (int i = 0; i < 2; i++)
				{
					vector21[0] = new Vector2(0f, 0f);
					vector21[1] = new Vector2(0f, 1f);
					vector21[2] = new Vector2(1f, 1f);
					vector21[3] = new Vector2(1f, 0f);
					vector2Array1[0] = new Vector2(0f, 0f);
					vector2Array1[1] = new Vector2(0f, 1f);
					vector2Array1[2] = new Vector2(1f, 1f);
					vector2Array1[3] = new Vector2(1f, 0f);
					if (this.mInvert)
					{
						if (i > 0)
						{
							this.Rotate(vector21, i);
							this.Rotate(vector2Array1, i);
						}
					}
					else if (i < 1)
					{
						this.Rotate(vector21, 1 - i);
						this.Rotate(vector2Array1, 1 - i);
					}
					if (i != 1)
					{
						single = (!this.mInvert ? 0.5f : 1f);
						single1 = (!this.mInvert ? 1f : 0.5f);
					}
					else
					{
						single = (!this.mInvert ? 1f : 0.5f);
						single1 = (!this.mInvert ? 0.5f : 1f);
					}
					vector21[1].y = Mathf.Lerp(single, single1, vector21[1].y);
					vector21[2].y = Mathf.Lerp(single, single1, vector21[2].y);
					vector2Array1[1].y = Mathf.Lerp(single, single1, vector2Array1[1].y);
					vector2Array1[2].y = Mathf.Lerp(single, single1, vector2Array1[2].y);
					float single12 = this.mFillAmount * 2f - (float)i;
					bool flag = i % 2 == 1;
					if (this.AdjustRadial(vector21, vector2Array1, single12, !flag))
					{
						if (this.mInvert)
						{
							flag = !flag;
						}
						if (!flag)
						{
							int num = m.Alloc(PrimitiveKind.Quad);
							for (int j = 3; j > -1; j--)
							{
								m.v[num].x = Mathf.Lerp(vector2[0].x, vector2[2].x, vector21[j].x);
								m.v[num].y = Mathf.Lerp(vector2[0].y, vector2[2].y, vector21[j].y);
								m.v[num].z = 0f;
								m.v[num].u = Mathf.Lerp(vector2Array[0].x, vector2Array[2].x, vector2Array1[j].x);
								m.v[num].v = Mathf.Lerp(vector2Array[0].y, vector2Array[2].y, vector2Array1[j].y);
								m.v[num].r = color.r;
								m.v[num].g = color.g;
								m.v[num].b = color.b;
								m.v[num].a = color.a;
								num++;
							}
						}
						else
						{
							int num1 = m.Alloc(PrimitiveKind.Quad);
							for (int k = 0; k < 4; k++)
							{
								m.v[num1].x = Mathf.Lerp(vector2[0].x, vector2[2].x, vector21[k].x);
								m.v[num1].y = Mathf.Lerp(vector2[0].y, vector2[2].y, vector21[k].y);
								m.v[num1].z = 0f;
								m.v[num1].u = Mathf.Lerp(vector2Array[0].x, vector2Array[2].x, vector2Array1[k].x);
								m.v[num1].v = Mathf.Lerp(vector2Array[0].y, vector2Array[2].y, vector2Array1[k].y);
								m.v[num1].r = color.r;
								m.v[num1].g = color.g;
								m.v[num1].b = color.b;
								m.v[num1].a = color.a;
								num1++;
							}
						}
					}
				}
				return;
			}
			if (this.fillDirection == UIFilledSprite.FillDirection.Radial360)
			{
				float[] singleArray = new float[] { 0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 0f, 0.5f, 0f, 0.5f };
				Vector2[] vector22 = new Vector2[4];
				Vector2[] vector2Array2 = new Vector2[4];
				for (int l = 0; l < 4; l++)
				{
					vector22[0] = new Vector2(0f, 0f);
					vector22[1] = new Vector2(0f, 1f);
					vector22[2] = new Vector2(1f, 1f);
					vector22[3] = new Vector2(1f, 0f);
					vector2Array2[0] = new Vector2(0f, 0f);
					vector2Array2[1] = new Vector2(0f, 1f);
					vector2Array2[2] = new Vector2(1f, 1f);
					vector2Array2[3] = new Vector2(1f, 0f);
					if (this.mInvert)
					{
						if (l > 0)
						{
							this.Rotate(vector22, l);
							this.Rotate(vector2Array2, l);
						}
					}
					else if (l < 3)
					{
						this.Rotate(vector22, 3 - l);
						this.Rotate(vector2Array2, 3 - l);
					}
					for (int m1 = 0; m1 < 4; m1++)
					{
						int num2 = (!this.mInvert ? l * 4 : (3 - l) * 4);
						float single13 = singleArray[num2];
						float single14 = singleArray[num2 + 1];
						float single15 = singleArray[num2 + 2];
						float single16 = singleArray[num2 + 3];
						vector22[m1].x = Mathf.Lerp(single13, single14, vector22[m1].x);
						vector22[m1].y = Mathf.Lerp(single15, single16, vector22[m1].y);
						vector2Array2[m1].x = Mathf.Lerp(single13, single14, vector2Array2[m1].x);
						vector2Array2[m1].y = Mathf.Lerp(single15, single16, vector2Array2[m1].y);
					}
					float single17 = this.mFillAmount * 4f - (float)l;
					bool flag1 = l % 2 == 1;
					if (this.AdjustRadial(vector22, vector2Array2, single17, !flag1))
					{
						if (this.mInvert)
						{
							flag1 = !flag1;
						}
						if (!flag1)
						{
							int num3 = m.Alloc(PrimitiveKind.Quad);
							for (int n = 3; n > -1; n--)
							{
								m.v[num3].x = Mathf.Lerp(vector2[0].x, vector2[2].x, vector22[n].x);
								m.v[num3].y = Mathf.Lerp(vector2[0].y, vector2[2].y, vector22[n].y);
								m.v[num3].z = 0f;
								m.v[num3].u = Mathf.Lerp(vector2Array[0].x, vector2Array[2].x, vector2Array2[n].x);
								m.v[num3].v = Mathf.Lerp(vector2Array[0].y, vector2Array[2].y, vector2Array2[n].y);
								m.v[num3].r = color.r;
								m.v[num3].g = color.g;
								m.v[num3].b = color.b;
								m.v[num3].a = color.a;
								num3++;
							}
						}
						else
						{
							int num4 = m.Alloc(PrimitiveKind.Quad);
							for (int o = 0; o < 4; o++)
							{
								m.v[num4].x = Mathf.Lerp(vector2[0].x, vector2[2].x, vector22[o].x);
								m.v[num4].y = Mathf.Lerp(vector2[0].y, vector2[2].y, vector22[o].y);
								m.v[num4].z = 0f;
								m.v[num4].u = Mathf.Lerp(vector2Array[0].x, vector2Array[2].x, vector2Array2[o].x);
								m.v[num4].v = Mathf.Lerp(vector2Array[0].y, vector2Array[2].y, vector2Array2[o].y);
								m.v[num4].r = color.r;
								m.v[num4].g = color.g;
								m.v[num4].b = color.b;
								m.v[num4].a = color.a;
								num4++;
							}
						}
					}
				}
				return;
			}
		}
		else if (!this.AdjustRadial(vector2, vector2Array, this.mFillAmount, this.mInvert))
		{
			return;
		}
		vertex.x = vector2[0].x;
		vertex.y = vector2[0].y;
		vertex.u = vector2Array[0].x;
		vertex.v = vector2Array[0].y;
		vertex1.x = vector2[1].x;
		vertex1.y = vector2[1].y;
		vertex1.u = vector2Array[1].x;
		vertex1.v = vector2Array[1].y;
		vertex2.x = vector2[2].x;
		vertex2.y = vector2[2].y;
		vertex2.u = vector2Array[2].x;
		vertex2.v = vector2Array[2].y;
		vertex3.x = vector2[3].x;
		vertex3.y = vector2[3].y;
		vertex3.u = vector2Array[3].x;
		vertex3.v = vector2Array[3].y;
		float single18 = 0f;
		float single19 = single18;
		vertex3.z = single18;
		float single20 = single19;
		single19 = single20;
		vertex2.z = single20;
		float single21 = single19;
		single19 = single21;
		vertex1.z = single21;
		vertex.z = single19;
		float single22 = color.r;
		single19 = single22;
		vertex3.r = single22;
		float single23 = single19;
		single19 = single23;
		vertex2.r = single23;
		float single24 = single19;
		single19 = single24;
		vertex1.r = single24;
		vertex.r = single19;
		float single25 = color.g;
		single19 = single25;
		vertex3.g = single25;
		float single26 = single19;
		single19 = single26;
		vertex2.g = single26;
		float single27 = single19;
		single19 = single27;
		vertex1.g = single27;
		vertex.g = single19;
		float single28 = color.b;
		single19 = single28;
		vertex3.b = single28;
		float single29 = single19;
		single19 = single29;
		vertex2.b = single29;
		float single30 = single19;
		single19 = single30;
		vertex1.b = single30;
		vertex.b = single19;
		float single31 = color.a;
		single19 = single31;
		vertex3.a = single31;
		float single32 = single19;
		single19 = single32;
		vertex2.a = single32;
		float single33 = single19;
		single19 = single33;
		vertex1.a = single33;
		vertex.a = single19;
		m.Quad(vertex, vertex1, vertex2, vertex3);
	}

	private void Rotate(Vector2[] v, int offset)
	{
		for (int i = 0; i < offset; i++)
		{
			Vector2 vector2 = new Vector2(v[3].x, v[3].y);
			v[3].x = v[2].y;
			v[3].y = v[2].x;
			v[2].x = v[1].y;
			v[2].y = v[1].x;
			v[1].x = v[0].y;
			v[1].y = v[0].x;
			v[0].x = vector2.y;
			v[0].y = vector2.x;
		}
	}

	public enum FillDirection
	{
		Horizontal,
		Vertical,
		Radial90,
		Radial180,
		Radial360
	}
}