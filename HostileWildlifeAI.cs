using Facepunch.Procedural;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public class HostileWildlifeAI : BasicWildLifeAI
{
	protected TakeDamage _targetTD;

	public float loseTargetRange = 100f;

	public float attackRange = 1f;

	public float attackRangeMax = 3f;

	public float attackRate;

	public float attackDamageMin;

	public float attackDamageMax;

	public string lastMoveAnim;

	[SerializeField]
	protected AudioClipArray attackSounds;

	[SerializeField]
	protected AudioClipArray chaseSoundsClose;

	[SerializeField]
	protected AudioClipArray chaseSoundsFar;

	protected MillisClock nextTargetClock;

	protected MillisClock nextAttackClock;

	protected MillisClock attackStrikeClock;

	protected MillisClock chaseSoundClock;

	protected MillisClock targetReachClock;

	protected MillisClock stuckClock;

	protected MillisClock warnClock;

	protected bool wasStuck;

	public float nextScentListenTime;

	public string dropOnDeathString;

	public Character _myChar;

	public HostileWildlifeAI()
	{
	}

	[RPC]
	public void CL_Attack(uLink.NetworkMessageInfo info)
	{
		InterpTimedEvent.Queue(this, "ATK", ref info);
	}

	public void DoClientAttack()
	{
		base.animation.CrossFade(this.GetAttackAnim(), 0.1f, PlayMode.StopSameLayer);
	}

	public virtual string GetAttackAnim()
	{
		return "bite";
	}

	public void GoScentBlind(float dur)
	{
		this.nextScentListenTime = Time.time + dur;
	}

	public bool IsScentBlind()
	{
		return Time.time < this.nextScentListenTime;
	}

	protected override bool OnInterpTimedEvent()
	{
		int num;
		string tag = InterpTimedEvent.Tag;
		if (tag != null)
		{
			if (HostileWildlifeAI.<>f__switch$map8 == null)
			{
				HostileWildlifeAI.<>f__switch$map8 = new Dictionary<string, int>(1)
				{
					{ "ATK", 0 }
				};
			}
			if (HostileWildlifeAI.<>f__switch$map8.TryGetValue(tag, out num))
			{
				if (num == 0)
				{
					this.DoClientAttack();
					return true;
				}
			}
		}
		return base.OnInterpTimedEvent();
	}

	protected override bool PlaySnd(int type)
	{
		AudioClip item = null;
		float single = 1f;
		float single1 = 5f;
		float single2 = 20f;
		bool flag = false;
		if (type == 5)
		{
			if (this.chaseSoundsFar != null)
			{
				item = this.chaseSoundsFar[UnityEngine.Random.Range(0, this.chaseSoundsFar.Length)];
			}
			single = 1f;
			single1 = 0.25f;
			single2 = 25f;
			flag = true;
		}
		else if (type == 6)
		{
			if (this.chaseSoundsClose != null)
			{
				item = this.chaseSoundsClose[UnityEngine.Random.Range(0, this.chaseSoundsClose.Length)];
			}
			single = 1f;
			single1 = 0f;
			single2 = 10f;
			flag = true;
		}
		else if (type == 2)
		{
			if (this.attackSounds != null)
			{
				item = this.attackSounds[UnityEngine.Random.Range(0, this.attackSounds.Length)];
			}
			single = 1f;
			single1 = 0f;
			single2 = 10f;
			flag = true;
		}
		if (!item || !flag)
		{
			return base.PlaySnd(type);
		}
		item.PlayLocal(base.transform, Vector3.zero, single, single1, single2);
		return true;
	}
}