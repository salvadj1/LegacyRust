using System;
using UnityEngine;

[NGCAutoAddScript]
public class DeployedRespawn : DeployableObject
{
	public double lastSpawnTime;

	public double spawnDelay = 240;

	protected DeployedRespawn() : base(IDFlags.Item)
	{
		this.lastSpawnTime = Double.NegativeInfinity;
	}

	public double CooldownTimeLeft()
	{
		return (double)Mathf.Clamp((float)(this.lastSpawnTime + this.spawnDelay - NetCull.time), 0f, (float)this.spawnDelay);
	}

	public virtual Vector3 GetSpawnPos()
	{
		return base.transform.position;
	}

	public virtual Quaternion GetSpawnRot()
	{
		return base.transform.rotation;
	}

	public virtual bool IsValidToSpawn()
	{
		return NetCull.time > this.lastSpawnTime + this.spawnDelay;
	}

	public virtual void MarkSpawnedOn()
	{
		this.lastSpawnTime = NetCull.time;
	}

	public virtual void NearbyRespawn()
	{
		this.lastSpawnTime = NetCull.time;
	}
}