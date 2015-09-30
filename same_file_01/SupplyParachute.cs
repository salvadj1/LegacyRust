using Facepunch;
using System;
using UnityEngine;

public class SupplyParachute : Facepunch.MonoBehaviour
{
	[NonSerialized]
	private Vector3 targetScale;

	public SupplyParachute()
	{
	}

	public void Landed()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Start()
	{
		this.targetScale = base.transform.localScale;
		base.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
	}

	private void Update()
	{
		base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, this.targetScale, Time.deltaTime * 2f);
		if (base.transform.localScale == this.targetScale)
		{
			base.enabled = false;
		}
	}
}