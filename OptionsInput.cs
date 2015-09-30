using System;
using UnityEngine;

public class OptionsInput : MonoBehaviour
{
	public GameObject lineObject;

	public OptionsInput()
	{
	}

	private void Start()
	{
		GameInput.GameButton[] buttons = GameInput.Buttons;
		for (int i = 0; i < (int)buttons.Length; i++)
		{
			GameInput.GameButton gameButton = buttons[i];
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.lineObject);
			gameObject.transform.parent = base.transform;
			gameObject.GetComponent<OptionsKeyBinding>().Setup(gameButton);
		}
	}
}