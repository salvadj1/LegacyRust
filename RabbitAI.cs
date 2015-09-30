using System;
using UnityEngine;

public class RabbitAI : BasicWildLifeAI
{
	protected string lastMoveAnim;

	public RabbitAI()
	{
	}

	protected void Update()
	{
		if (this._takeDamage.dead)
		{
			return;
		}
		string str = "idle1";
		float single = 1f;
		float moveSpeedForAnim = base.GetMoveSpeedForAnim();
		if (moveSpeedForAnim <= 0.001f)
		{
			str = "idle1";
		}
		else if (moveSpeedForAnim <= 2f)
		{
			str = "hop";
			single = moveSpeedForAnim / 0.75f;
		}
		else if (moveSpeedForAnim > 2f)
		{
			str = "run";
			single = moveSpeedForAnim / 3f;
		}
		if (str != this.lastMoveAnim)
		{
			base.animation.CrossFade(str, 0.25f, PlayMode.StopSameLayer);
		}
		base.animation[str].speed = single;
		this.lastMoveAnim = str;
	}
}