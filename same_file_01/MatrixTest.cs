using Facepunch.Precision;
using System;
using UnityEngine;

[AddComponentMenu("Precision/Tests/Matrix Test")]
[ExecuteInEditMode]
public class MatrixTest : MonoBehaviour
{
	private const float cellWidth = 100f;

	private const float cellHeight = 30f;

	private const string formatFloat = "0.#####";

	private GUIContent[,,] contents;

	private Rect[,,] rects;

	private Matrix4x4 unity = Matrix4x4.identity;

	private Matrix4x4 lastUnity;

	private Matrix4x4G facep = Matrix4x4G.identity;

	public MatrixTest.TRS[] transforms;

	public bool revMul;

	public bool revG;

	public ProjectionTest projection;

	private ProjectionTest lastProjectionTest;

	public MatrixTest()
	{
	}

	private void Awake()
	{
		this.contents = new GUIContent[2, 4, 4];
		this.rects = new Rect[2, 4, 4];
		float single = 20f;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				float single1 = 20f;
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
		if (this.contents[m, col, row].text == this.contents[(m + 1) % 2, col, row].text)
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
		if (this.lastUnity != this.unity || this.projection != this.lastProjectionTest)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					GUIContent str = this.contents[num, i, j];
					float item = this.unity[i, j];
					str.text = item.ToString("0.#####");
				}
			}
			num = 1;
			for (int k = 0; k < 4; k++)
			{
				for (int l = 0; l < 4; l++)
				{
					GUIContent gUIContent = this.contents[num, k, l];
					double item1 = this.facep[k, l];
					gUIContent.text = item1.ToString("0.#####");
				}
			}
			this.lastProjectionTest = this.projection;
			this.lastUnity = this.unity;
		}
		GUIStyle gUIStyle = GUI.skin.textField;
		for (int m = 0; m < 2; m++)
		{
			for (int n = 0; n < 4; n++)
			{
				for (int o = 0; o < 4; o++)
				{
					this.DrawLabel(m, n, o, gUIStyle);
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
		Matrix4x4 unityMatrix;
		Matrix4x4G matrix4x4G;
		if (this.transforms == null || (int)this.transforms.Length <= 0)
		{
			unityMatrix = Matrix4x4.identity;
			matrix4x4G = Matrix4x4G.identity;
		}
		else if (!this.revMul)
		{
			int i = 0;
			unityMatrix = this.transforms[i].unity;
			matrix4x4G = this.transforms[i].facep;
			for (i++; i < (int)this.transforms.Length; i++)
			{
				unityMatrix = unityMatrix * this.transforms[i].unity;
				matrix4x4G = this.MultG(matrix4x4G, this.transforms[i].facep);
			}
		}
		else
		{
			int length = (int)this.transforms.Length - 1;
			unityMatrix = this.transforms[length].unity;
			matrix4x4G = this.transforms[length].facep;
			for (length--; length >= 0; length--)
			{
				unityMatrix = this.transforms[length].unity * unityMatrix;
				matrix4x4G = this.MultG(this.transforms[length].facep, matrix4x4G);
			}
		}
		if (this.projection)
		{
			unityMatrix = this.projection.UnityMatrix * unityMatrix;
			matrix4x4G = this.MultG(this.projection.GMatrix, matrix4x4G);
		}
		this.unity = unityMatrix;
		this.facep = matrix4x4G;
	}

	[Serializable]
	public class TRS
	{
		public Vector3 T;

		public Vector3 eulerR;

		public Vector3 S;

		public Matrix4x4G facep
		{
			get
			{
				Matrix4x4G matrix4x4G;
				Vector3G vector3G = new Vector3G(this.T);
				QuaternionG rFacep = this.R_facep;
				Vector3G vector3G1 = new Vector3G(this.S);
				Matrix4x4G.TRS(ref vector3G, ref rFacep, ref vector3G1, out matrix4x4G);
				return matrix4x4G;
			}
		}

		public QuaternionG R_facep
		{
			get
			{
				QuaternionG quaternionG;
				Vector3G vector3G = new Vector3G(this.eulerR);
				QuaternionG.Euler(ref vector3G, out quaternionG);
				return quaternionG;
			}
		}

		public Quaternion R_unity
		{
			get
			{
				return Quaternion.Euler(this.eulerR);
			}
		}

		public Matrix4x4 unity
		{
			get
			{
				return Matrix4x4.TRS(this.T, this.R_unity, this.S);
			}
		}

		public TRS()
		{
			this.S = Vector3.one;
		}
	}
}