using System;
using uLink;
using UnityEngine;

public class ChickenAI : BasicWildLifeAI
{
	protected bool isMale;

	[SerializeField]
	protected Material roosterMat;

	[SerializeField]
	protected Material ChickenMatA;

	[SerializeField]
	protected Material ChickenMatB;

	[SerializeField]
	protected Renderer chickenRenderer;

	protected string lastMoveAnim;

	public ChickenAI()
	{
	}

	protected void SetGender(bool male, bool alt)
	{
		this.isMale = male;
		if (this.isMale)
		{
			this.chickenRenderer.material = this.roosterMat;
		}
		else if (alt)
		{
			this.chickenRenderer.material = this.ChickenMatB;
		}
		else
		{
			this.chickenRenderer.material = this.ChickenMatA;
		}
	}

	protected new void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		int num = info.networkView.viewID.id;
		this.SetGender((num & 14) >> 1 <= 2, (num & 1) == 1);
		base.uLink_OnNetworkInstantiate(info);
	}

	protected void Update()
	{
		if (this._takeDamage.dead)
		{
			return;
		}
		string str = "idleEat";
		float single = 1f;
		float moveSpeedForAnim = base.GetMoveSpeedForAnim();
		if (moveSpeedForAnim <= 0.001f)
		{
			str = "idleEat";
		}
		else if (moveSpeedForAnim <= 2f)
		{
			str = "walk";
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