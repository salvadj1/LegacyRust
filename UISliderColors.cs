using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Slider Colors")]
[ExecuteInEditMode]
[RequireComponent(typeof(UISlider))]
public class UISliderColors : MonoBehaviour
{
	public UISprite sprite;

	public Color[] colors = new Color[] { Color.red, Color.yellow, Color.green };

	private UISlider mSlider;

	public UISliderColors()
	{
	}

	private void Start()
	{
		this.mSlider = base.GetComponent<UISlider>();
		this.Update();
	}

	private void Update()
	{
		if (this.sprite == null || (int)this.colors.Length == 0)
		{
			return;
		}
		float length = this.mSlider.sliderValue;
		length = length * (float)((int)this.colors.Length - 1);
		int num = Mathf.FloorToInt(length);
		Color color = this.colors[0];
		if (num >= 0)
		{
			if (num + 1 >= (int)this.colors.Length)
			{
				color = (num >= (int)this.colors.Length ? this.colors[(int)this.colors.Length - 1] : this.colors[num]);
			}
			else
			{
				float single = length - (float)num;
				color = Color.Lerp(this.colors[num], this.colors[num + 1], single);
			}
		}
		color.a = this.sprite.color.a;
		this.sprite.color = color;
	}
}