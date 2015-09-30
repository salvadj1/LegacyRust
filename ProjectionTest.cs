using Facepunch.Precision;
using System;
using UnityEngine;

[AddComponentMenu("Precision/Tests/Projection Test")]
[ExecuteInEditMode]
public class ProjectionTest : MonoBehaviour
{
	private const float cellWidth = 100f;

	private const float cellHeight = 30f;

	private GUIContent[,,] contents;

	private Rect[,,] rects;

	private Matrix4x4 unity = Matrix4x4.identity;

	private Matrix4x4 lastUnity;

	private Matrix4x4G facep = Matrix4x4G.identity;

	private Matrix4x4 unity2;

	public float near = 1f;

	public float aspect = -1f;

	public float far = 1000f;

	public float fov = 60f;

	public bool revMul;

	public bool revG;

	public Matrix4x4G GMatrix
	{
		get
		{
			return this.facep;
		}
	}

	public Matrix4x4 UnityMatrix
	{
		get
		{
			return this.unity;
		}
	}

	public Matrix4x4 UnityMatrixCasted
	{
		get
		{
			return this.unity2;
		}
	}

	public ProjectionTest()
	{
	}

	private void Awake()
	{
		this.contents = new GUIContent[3, 4, 4];
		this.rects = new Rect[3, 4, 4];
		float single = 20f;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				float single1 = 600f;
				for (int k = 0; k < 4; k++)
				{
					this.contents[i, j, k] = new GUIContent();
					this.rects[i, j, k] = new Rect(single1, single, 100f, 30f);
					single1 = single1 + 102f;
				}
				single = single + 32f;
			}
			single = single + 10f;
		}
	}

	private void DrawLabel(int m, int col, int row, GUIStyle style)
	{
		if (this.contents[m, col, row].text == this.contents[0, col, row].text)
		{
			GUI.contentColor = Color.white;
		}
		else
		{
			GUI.contentColor = this.RCCol(col, row);
		}
		GUI.Label(this.rects[m, col, row], this.contents[m, col, row], style);
	}

	private Matrix4x4G MultG(Matrix4x4G a, Matrix4x4G b)
	{
		Matrix4x4G matrix4x4G;
		if (!this.revG)
		{
			Matrix4x4G.Mult(ref a, ref b, out matrix4x4G);
		}
		else
		{
			Matrix4x4G.Mult(ref b, ref a, out matrix4x4G);
		}
		return matrix4x4G;
	}

	private void OnGUI()
	{
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		if (this.contents == null)
		{
			this.Awake();
		}
		if (this.lastUnity != this.unity)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					GUIContent str = this.contents[num, i, j];
					float item = this.unity[i, j];
					str.text = item.ToString();
				}
			}
			num = 1;
			for (int k = 0; k < 4; k++)
			{
				for (int l = 0; l < 4; l++)
				{
					GUIContent gUIContent = this.contents[num, k, l];
					double item1 = this.facep[k, l];
					gUIContent.text = item1.ToString();
				}
			}
			num = 2;
			for (int m = 0; m < 4; m++)
			{
				for (int n = 0; n < 4; n++)
				{
					GUIContent str1 = this.contents[num, m, n];
					float single = this.unity2[m, n];
					str1.text = single.ToString();
				}
			}
			this.lastUnity = this.unity;
		}
		GUIStyle gUIStyle = GUI.skin.textField;
		for (int o = 0; o < 3; o++)
		{
			for (int p = 0; p < 4; p++)
			{
				for (int q = 0; q < 4; q++)
				{
					this.DrawLabel(o, p, q, gUIStyle);
				}
			}
		}
	}

	private Color RCCol(int col, int row)
	{
		Color color;
		switch (row | col % 2 << 2)
		{
			case 0:
			{
				color = Color.red;
				break;
			}
			case 1:
			{
				color = Color.green;
				break;
			}
			case 2:
			{
				color = Color.blue;
				break;
			}
			case 3:
			{
				color = Color.magenta;
				break;
			}
			case 4:
			{
				color = Color.cyan;
				break;
			}
			case 5:
			{
				color = Color.yellow;
				break;
			}
			case 6:
			{
				color = Color.gray;
				break;
			}
			case 7:
			{
				color = Color.black;
				break;
			}
			default:
			{
				color = Color.clear;
				break;
			}
		}
		if (col >= 2)
		{
			color.r = color.r + 0.25f;
			color.g = color.g + 0.25f;
			color.b = color.b + 0.25f;
		}
		return color;
	}

	private void Update()
	{
		double num = (this.aspect <= 0f ? (double)Screen.height / (double)Screen.width : (double)this.aspect);
		this.unity = Matrix4x4.Perspective(this.fov, (float)num, this.near, this.far);
		double num1 = (double)this.fov;
		double num2 = (double)this.near;
		double num3 = (double)this.far;
		Matrix4x4G.Perspective(ref num1, ref num, ref num2, ref num3, out this.facep);
		this.unity2 = this.facep.f;
	}
}