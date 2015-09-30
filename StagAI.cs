using System;
using UnityEngine;

public class StagAI : BasicWildLifeAI
{
	protected string lastMoveAnim;

	public StagAI()
	{
	}

	protected void Update()
	{
		if (this._takeDamage.dead)
		{
			return;
		}
		string str = "idle1";
		float walkAnimScalar = 1f;
		float moveSpeedForAnim = base.GetMoveSpeedForAnim();
		if (moveSpeedForAnim <= 0.001f)
		{
			str = "idle1";
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