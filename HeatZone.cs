using System;
using UnityEngine;

public class HeatZone : MonoBehaviour
{
	private bool _isOn;

	public HeatZone()
	{
	}

	public Metabolism GetFromCollider(Collider other)
	{
		return other.gameObject.GetComponent<Metabolism>();
	}

	public void SetOn(bool on)
	{
		this._isOn = on;
	}
}