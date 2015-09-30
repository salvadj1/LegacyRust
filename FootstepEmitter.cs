using System;
using UnityEngine;

public class FootstepEmitter : IDLocalCharacter
{
	[NonSerialized]
	private CharacterFootstepTrait trait;

	[NonSerialized]
	private Vector3 lastFootstepPos;

	[NonSerialized]
	private float nextAllowTime;

	[NonSerialized]
	private float movedAmount;

	public bool terraincheck;

	private AudioClip lastPlayed;

	public FootstepEmitter()
	{
	}

	private Collider GetBelowObj()
	{
		RaycastHit raycastHit;
		if (!Physics.Raycast(new Ray(base.transform.position + new Vector3(0f, 0.25f, 0f), Vector3.down), out raycastHit, 1f))
		{
			return null;
		}
		return raycastHit.collider;
	}

	private void Start()
	{
		this.lastFootstepPos = base.origin;
		this.trait = base.GetTrait<CharacterFootstepTrait>();
		if (!this.trait || !this.trait.defaultFootsteps || this.trait.defaultFootsteps.Length == 0)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (this.terraincheck)
		{
			TerrainTextureHelper.GetTextureIndex(base.origin);
		}
		if (base.stateFlags.grounded)
		{
			bool flag = this.trait.timeLimited;
			bool flag1 = flag;
			if ((!flag || this.nextAllowTime <= Time.time) && (!base.masterControllable || !(base.masterControllable.idMain != base.idMain)))
			{
				bool flag2 = base.stateFlags.crouch;
				Vector3 vector3 = base.origin;
				FootstepEmitter footstepEmitter = this;
				footstepEmitter.movedAmount = footstepEmitter.movedAmount + Vector3.Distance(this.lastFootstepPos, vector3);
				this.lastFootstepPos = vector3;
				if (this.movedAmount >= this.trait.sqrStrideDist)
				{
					this.movedAmount = 0f;
					AudioClip item = null;
					if (footsteps.quality >= 2 || footsteps.quality == 1 && base.character.localControlled)
					{
						Collider belowObj = this.GetBelowObj();
						if (belowObj)
						{
							SurfaceInfoObject surfaceInfoFor = SurfaceInfo.GetSurfaceInfoFor(belowObj, vector3);
							if (surfaceInfoFor)
							{
								item = (!this.trait.animal ? surfaceInfoFor.GetFootstepBiped(this.lastPlayed) : surfaceInfoFor.GetFootstepAnimal());
								this.lastPlayed = item;
							}
						}
					}
					if (!item)
					{
						item = this.trait.defaultFootsteps[UnityEngine.Random.Range(0, this.trait.defaultFootsteps.Length)];
						if (!item)
						{
							return;
						}
					}
					float single = this.trait.minAudioDist;
					float single1 = this.trait.maxAudioDist;
					if (!flag2)
					{
						item.Play(vector3, 0.65f, UnityEngine.Random.Range(0.95f, 1.05f), single, single1, 30);
					}
					else
					{
						item.Play(vector3, 0.2f, UnityEngine.Random.Range(0.95f, 1.05f), single * 0.333f, single1 * 0.333f, 30);
					}
					if (flag1)
					{
						this.nextAllowTime = Time.time + this.trait.minInterval;
					}
				}
				return;
			}
		}
	}
}