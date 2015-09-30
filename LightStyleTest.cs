using System;
using System.Collections.Generic;
using UnityEngine;

public class LightStyleTest : MonoBehaviour
{
	public LightStylist stylist;

	public string[] tests;

	public float spacebarTargetWeight = 1f;

	public float spacebarFadeLength = 1.3f;

	public float enterCrossfadeLength = 0.3f;

	private int index;

	private List<float> weights;

	public LightStyleTest()
	{
	}

	private void OnGUI()
	{
		for (int i = 0; i < (int)this.tests.Length; i++)
		{
			if (this.index != i)
			{
				GUILayout.Label(this.tests[i], new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Box(this.tests[i], new GUILayoutOption[0]);
			}
		}
		if (Event.current.type == EventType.Repaint)
		{
			if (this.weights != null)
			{
				this.weights.Clear();
			}
			else
			{
				this.weights = new List<float>();
			}
			this.weights.AddRange(this.stylist.Weights);
			int count = this.weights.Count;
			for (int j = 0; j < count; j++)
			{
				Rect rect = new Rect((float)(Screen.width / count * j), (float)(Screen.height - 120), (float)(Screen.width / count), 120f * this.weights[j]);
				float item = this.weights[j];
				GUI.Box(rect, item.ToString());
			}
			GUI.Label(new Rect((float)(Screen.width - 400), 0f, 400f, 100f), "\nPress up and down to change light style.\nHold space to apply it through LightStylist.Blend\nPress enter to apply it through LightStylist.CrossFade\nPress ctrl to apply it through LightStylist.Play");
		}
	}

	private void Reset()
	{
		this.tests = new string[] { "pulsate" };
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.index = (this.index + 1) % (int)this.tests.Length;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.index = (this.index + ((int)this.tests.Length - 1)) % (int)this.tests.Length;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			this.stylist.Blend(this.tests[this.index], this.spacebarTargetWeight, this.spacebarFadeLength);
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			this.stylist.CrossFade(this.tests[this.index], this.enterCrossfadeLength);
		}
		if (Input.GetKeyDown(KeyCode.LeftControl) | Input.GetKeyDown(KeyCode.RightControl))
		{
			this.stylist.Play(this.tests[this.index]);
		}
	}
}