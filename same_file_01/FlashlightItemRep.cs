using System;
using UnityEngine;

public class FlashlightItemRep : ItemRepresentation
{
	private GameObject lightEffect;

	public GameObject lightEffectPrefab1P;

	public GameObject lightEffectPrefab3P;

	public FlashlightItemRep()
	{
	}

	public virtual void SetLightOn(bool on)
	{
		bool flag;
		flag = (base.networkViewOwner != NetCull.player ? false : true);
		if (!on)
		{
			UnityEngine.Object.Destroy(this.lightEffect);
		}
		else if (!flag)
		{
			Vector3 vector3 = base.transform.position;
			Quaternion quaternion = base.transform.rotation;
			this.lightEffect = UnityEngine.Object.Instantiate(this.lightEffectPrefab3P, vector3, quaternion) as GameObject;
			this.lightEffect.transform.localPosition = vector3;
			this.lightEffect.transform.localRotation = quaternion;
		}
	}

	protected override void StateSignalReceive(Character character, bool treatedAsFirst)
	{
		this.SetLightOn(character.stateFlags.lamp);
	}
}