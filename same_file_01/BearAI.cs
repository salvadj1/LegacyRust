using System;
using UnityEngine;

public class BearAI : HostileWildlifeAI
{
	public BearAI()
	{
	}

	public override string GetAttackAnim()
	{
		int num = UnityEngine.Random.Range(0, 3);
		if (num == 0)
		{
			return "4LegsClawsAttackL";
		}
		if (num == 1)
		{
			return "4LegsClawsAttackR";
		}
		return "4LegsBiteAttack";
	}

	public override string GetDeathAnim()
	{
		return "4LegsDeath";
	}

	protected void Update()
	{
		if (this._takeDamage.dead)
		{
			return;
		}
		string str = "idle4legs";
		float walkAnimScalar = 1f;
		float moveSpeedForAnim = base.GetMoveSpeedForAnim();
		if (moveSpeedForAnim <= 0.001f)
		{
			str = "idle4Legs";
		}
		else if (moveSpeedForAnim <= 2f)
		{
			str = "walk";
			walkAnimScalar = moveSpeedForAnim / base.GetWalkAnimScalar();
		}
		else if (moveSpeedForAnim > 2f)
		{
			str = "run";
			walkAnimScalar = moveSpeedForAnim / base.GetRunAnimScalar();
		}
		if (str != this.lastMoveAnim)
		{
			base.animation.CrossFade(str, 0.25f, PlayMode.StopSameLayer);
		}
		base.animation[str].speed = walkAnimScalar;
		this.lastMoveAnim = str;
	}
}