using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Component")]
public class UICheckboxControlledComponent : MonoBehaviour
{
	public MonoBehaviour target;

	public bool inverse;

	public UICheckboxControlledComponent()
	{
	}

	private void OnActivate(bool isActive)
	{
		if (base.enabled && this.target != null)
		{
			this.target.enabled = (!this.inverse ? isActive : !isActive);
		}
	}
}