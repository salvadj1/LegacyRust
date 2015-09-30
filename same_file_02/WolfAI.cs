using System;
using UnityEngine;

public class WolfAI : HostileWildlifeAI
{
	public Renderer wolfRenderer;

	public Material[] mats;

	public WolfAI()
	{
	}

	public override string GetAttackAnim()
	{
		return "bite";
	}

	public void Start()
	{
		this.wolfRenderer.material = this.mats[UnityEngine.Random.Range(0, (int)this.mats.Length)];
	}

	protected void Update()
	{
		if (this._takeDamage.dead)
		{
			return;
		}
		string str = "idle";
		float walkAnimScalar = 1f;
		float moveSpeedForAnim = base.GetMoveSpeedForAnim();
		if (moveSpeedForAnim <= 0.001f)
		{
			str = "idle";
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