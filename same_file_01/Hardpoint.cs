using Facepunch.MeshBatch;
using System;
using UnityEngine;

public class Hardpoint : IDRemote
{
	public Hardpoint.hardpoint_type type = Hardpoint.hardpoint_type.Generic;

	private DeployableObject holding;

	private HardpointMaster _master;

	public Hardpoint()
	{
	}

	public new void Awake()
	{
		HardpointMaster component = base.idMain.GetComponent<HardpointMaster>();
		if (component)
		{
			this.SetMaster(component);
		}
		base.Awake();
	}

	public static Hardpoint GetHardpointFromRay(Ray ray, Hardpoint.hardpoint_type type)
	{
		RaycastHit raycastHit;
		bool flag;
		MeshBatchInstance meshBatchInstance;
		if (Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray, out raycastHit, 10f, out flag, out meshBatchInstance))
		{
			IDMain dMain = (!flag ? IDBase.GetMain(raycastHit.collider) : meshBatchInstance.idMain);
			if (dMain)
			{
				HardpointMaster component = dMain.GetComponent<HardpointMaster>();
				if (component)
				{
					return component.GetHardpointNear(raycastHit.point, type);
				}
			}
		}
		return null;
	}

	public HardpointMaster GetMaster()
	{
		return this._master;
	}

	public bool IsFree()
	{
		return this.holding == null;
	}

	public new void OnDestroy()
	{
		base.OnDestroy();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube(base.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(base.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
	}

	public void SetMaster(HardpointMaster master)
	{
		this._master = master;
		master.AddHardpoint(this);
	}

	public enum hardpoint_type
	{
		None,
		Generic,
		Door,
		Turret,
		Gate,
		Window
	}
}