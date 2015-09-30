using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Object")]
public class UICheckboxControlledObject : MonoBehaviour
{
	public GameObject target;

	public bool inverse;

	public UICheckboxControlledObject()
	{
	}

	private void OnActivate(bool isActive)
	{
		if (this.target != null)
		{
			NGUITools.SetActive(this.target, (!this.inverse ? isActive : !isActive));
		}
	}
}