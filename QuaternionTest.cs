using Facepunch.Precision;
using System;
using UnityEngine;

[AddComponentMenu("Precision/Tests/Quaternion Test")]
[ExecuteInEditMode]
public class QuaternionTest : MonoBehaviour
{
	private const float cellWidth = 100f;

	private const float cellHeight = 30f;

	private const string formatFloat = "0.######";

	private GUIContent[,] contents;

	private Rect[,] rects;

	private Quaternion unity = Quaternion.identity;

	private Quaternion lastUnity;

	private QuaternionG facep = QuaternionG.identity;

	public Vector3[] R;

	public bool revMul;

	public bool nonHomogenous;

	private bool nonHomogenousWas;

	public QuaternionTest()
	{
	}

	private void Awake()
	{
		this.contents = new GUIContent[3, 4];
		this.rects = new Rect[3, 4];
		float single = 400f;
		for (int i = 0; i < 3; i++)
		{
			float single1 = 20f;
			for (int j = 0; j < 4; j++)
			{
				this.contents[i, j] = new GUIContent();
				this.rects[i, j] = new Rect(single1, single, 100f, 30f);
				single1 = single1 + 102f;
			}
			single = single + 32f;
		}
		this.contents[2, 0].text = "Degrees:";
	}

	private void OnGUI()
	{
		Vector3G vector3G;
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		if (this.contents == null)
		{
			this.Awake();
		}
		if (this.lastUnity != this.unity || this.nonHomogenous != this.nonHomogenousWas)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				GUIContent str = this.contents[num, i];
				float item = this.unity[i];
				str.text = item.ToString("0.######");
			}
			num = 1;
			for (int j = 0; j < 4; j++)
			{
				GUIContent gUIContent = this.contents[num, j];
				double item1 = this.facep[j];
				gUIContent.text = item1.ToString("0.######");
			}
			num = 2;
			if (!this.nonHomogenous)
			{
				QuaternionG.ToEuler(ref this.facep, out vector3G);
			}
			else
			{
				QuaternionG.ToEulerNonHomogenious(ref this.facep, out vector3G);
			}
			for (int k = 1; k < 4; k++)
			{
				GUIContent str1 = this.contents[num, k];
				double num1 = vector3G[k - 1];
				str1.text = num1.ToString("0.######");
			}
			this.nonHomogenousWas = this.nonHomogenous;
			this.lastUnity = this.unity;
		}
		GUIStyle gUIStyle = GUI.skin.textField;
		for (int l = 0; l < 3; l++)
		{
			for (int m = 0; m < 4; m++)
			{
				GUI.Label(this.rects[l, m], this.contents[l, m], gUIStyle);
			}
		}
	}

	private void Update()
	{
		QuaternionG quaternionG;
		QuaternionG quaternionG1;
		if (this.R == null || (int)this.R.Length == 0)
		{
			this.unity = Quaternion.identity;
			this.facep = QuaternionG.identity;
		}
		else if (!this.revMul)
		{
			int i = 0;
			this.unity = Quaternion.Euler(this.R[i]);
			Vector3G vector3G = new Vector3G(this.R[i]);
			QuaternionG.Euler(ref vector3G, out this.facep);
			for (i++; i < (int)this.R.Length; i++)
			{
				QuaternionTest quaternionTest = this;
				quaternionTest.unity = quaternionTest.unity * Quaternion.Euler(this.R[i]);
				vector3G.f = this.R[i];
				QuaternionG.Euler(ref vector3G, out quaternionG1);
				QuaternionG quaternionG2 = this.facep;
				QuaternionG.Mult(ref quaternionG2, ref quaternionG1, out this.facep);
			}
		}
		else
		{
			int length = (int)this.R.Length - 1;
			this.unity = Quaternion.Euler(this.R[length]);
			Vector3G r = new Vector3G(this.R[length]);
			QuaternionG.Euler(ref r, out this.facep);
			for (length--; length >= 0; length--)
			{
				this.unity = Quaternion.Euler(this.R[length]) * this.unity;
				r.f = this.R[length];
				QuaternionG.Euler(ref r, out quaternionG);
				QuaternionG.Mult(ref quaternionG, ref this.facep, out this.facep);
			}
		}
	}
}