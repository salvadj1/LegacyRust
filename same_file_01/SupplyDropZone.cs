using Facepunch;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDropZone : Facepunch.MonoBehaviour
{
	public static List<SupplyDropZone> _dropZones;

	public float radius = 100f;

	public SupplyDropZone()
	{
	}

	public void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}
}