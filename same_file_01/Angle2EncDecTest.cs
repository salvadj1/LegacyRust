using System;
using UnityEngine;

public class Angle2EncDecTest : MonoBehaviour
{
	public float rate = 360f;

	public GUIContent[] contents;

	private int contentIndex;

	private Angle2 a;

	private Angle2? dec;

	public Angle2EncDecTest()
	{
	}

	private void OnGUI()
	{
		if (!this.dec.HasValue)
		{
			this.dec = new Angle2?(this.a.decoded);
			GUIContent[] gUIContentArray = this.contents;
			Angle2EncDecTest angle2EncDecTest = this;
			int num = angle2EncDecTest.contentIndex;
			int num1 = num;
			angle2EncDecTest.contentIndex = num + 1;
			GUIContent gUIContent = gUIContentArray[num1];
			object[] value = new object[] { "Enc:\t", this.a.x, "\tDec:\t", null, null, null };
			value[3] = this.dec.Value.x;
			value[4] = "\tRED:\t";
			Angle2 angle2 = this.dec.Value;
			value[5] = angle2.decoded.x;
			gUIContent.text = string.Concat(value);
			Angle2EncDecTest length = this;
			length.contentIndex = length.contentIndex % (int)this.contents.Length;
		}
		GUIContent[] gUIContentArray1 = this.contents;
		for (int i = 0; i < (int)gUIContentArray1.Length; i++)
		{
			GUILayout.Label(gUIContentArray1[i], new GUILayoutOption[0]);
		}
	}

	private void Update()
	{
		float single = Time.deltaTime * this.rate;
		if (single != 0f)
		{
			this.a.x = this.a.x + single;
			while (this.a.x > 360f)
			{
				this.a.x = this.a.x - 360f;
			}
			while (this.a.x < 0f)
			{
				this.a.x = this.a.x + 360f;
			}
			this.dec = null;
		}
	}
}