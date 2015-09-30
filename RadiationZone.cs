using System;
using System.Collections.Generic;
using UnityEngine;

public class RadiationZone : MonoBehaviour
{
	public float radius = 10f;

	public float exposurePerMin = 50f;

	public bool strongerAtCenter = true;

	[NonSerialized]
	private HashSet<Radiation> radiating;

	[NonSerialized]
	private bool shuttingDown;

	public RadiationZone()
	{
	}

	internal bool CanAddToRadiation(Radiation radiation)
	{
		bool flag;
		if (this.shuttingDown)
		{
			flag = false;
		}
		else
		{
			HashSet<Radiation> radiations = this.radiating;
			if (radiations == null)
			{
				HashSet<Radiation> radiations1 = new HashSet<Radiation>();
				HashSet<Radiation> radiations2 = radiations1;
				this.radiating = radiations1;
				radiations = radiations2;
			}
			flag = radiations.Add(radiation);
		}
		return flag;
	}

	public float GetExposureForPos(Vector3 pos)
	{
		if (!this.strongerAtCenter)
		{
			return this.exposurePerMin;
		}
		return this.exposurePerMin * (1f - Mathf.Clamp01(Vector3.Distance(pos, base.transform.position) / this.radius));
	}

	public Character GetFromCollider(Collider other)
	{
		IDBase dBase = IDBase.Get(other);
		if (!dBase)
		{
			return null;
		}
		return dBase.idMain as Character;
	}

	private void OnDestroy()
	{
		this.shuttingDown = true;
		if (this.radiating != null)
		{
			foreach (Radiation radiation in this.radiating)
			{
				if (!radiation)
				{
					continue;
				}
				radiation.RemoveRadiationZone(this);
			}
			this.radiating = null;
		}
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.3f, 0.5f, 0.3f, 0.25f);
		Gizmos.DrawSphere(base.transform.position, this.radius);
		Gizmos.color = Color.green;
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.3f, 0.5f, 0.3f, 0.4f);
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
		Gizmos.color = Color.green;
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}

	private void OnTriggerEnter(Collider other)
	{
		Character fromCollider = this.GetFromCollider(other);
		if (!fromCollider)
		{
			return;
		}
		Radiation local = fromCollider.GetLocal<Radiation>();
		if (local)
		{
			local.AddRadiationZone(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Character fromCollider = this.GetFromCollider(other);
		if (!fromCollider)
		{
			return;
		}
		Radiation local = fromCollider.GetLocal<Radiation>();
		if (!local)
		{
			return;
		}
		local.RemoveRadiationZone(this);
	}

	internal bool RemoveFromRadiation(Radiation radiation)
	{
		bool flag;
		if (this.shuttingDown)
		{
			flag = true;
		}
		else
		{
			flag = (this.radiating == null ? false : this.radiating.Remove(radiation));
		}
		return flag;
	}

	private void Start()
	{
		this.UpdateCollider();
	}

	[ContextMenu("Update Collider")]
	public void UpdateCollider()
	{
		base.GetComponent<SphereCollider>().radius = this.radius;
		base.collider.isTrigger = true;
	}
}