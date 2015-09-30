using System;
using UnityEngine;

public class PopupInventory : MonoBehaviour
{
	public dfRichTextLabel labelText;

	public dfTweenVector3 tweenOut;

	private static int iYPos;

	static PopupInventory()
	{
	}

	public PopupInventory()
	{
	}

	public void PlayOut()
	{
		this.tweenOut.Play();
		UnityEngine.Object.Destroy(base.gameObject, this.tweenOut.Length);
	}

	public void Setup(float fSeconds, string strText)
	{
		Vector2 size = base.transform.parent.GetComponent<dfPanel>().Size;
		dfPanel component = base.GetComponent<dfPanel>();
		Vector2 vector2 = this.labelText.Font.MeasureText(strText, this.labelText.FontSize, this.labelText.FontStyle);
		this.labelText.Width = vector2.x + 16f;
		Vector3 relativePosition = this.labelText.RelativePosition;
		component.Width = relativePosition.x + this.labelText.Width + 8f;
		Vector2 vector21 = new Vector2()
		{
			x = size.x + UnityEngine.Random.Range(-16f, 16f),
			y = size.y * 0.7f + UnityEngine.Random.Range(-16f, 16f)
		};
		vector21.y = vector21.y + ((float)PopupInventory.iYPos / 6f - 0.5f) * size.y * 0.2f;
		component.RelativePosition = vector21;
		PopupInventory.iYPos = PopupInventory.iYPos + 1;
		if (PopupInventory.iYPos > 5)
		{
			PopupInventory.iYPos = 0;
		}
		Vector3 endValue = this.tweenOut.EndValue;
		endValue.y = UnityEngine.Random.Range(-100f, 100f);
		this.tweenOut.EndValue = endValue;
		component.BringToFront();
		this.labelText.Text = strText;
		base.Invoke("PlayOut", fSeconds);
	}
}