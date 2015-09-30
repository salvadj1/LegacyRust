using System;
using uLink;
using UnityEngine;

public class FallDamage : IDLocalCharacter
{
	private const string kRPCName_InjuryInfo = "fIo";

	private const string kRPCName_ReadFallImpactClient = "fIc";

	private const string kRPCName_ReadFallImpactServer = "fIm";

	public AudioClip legBreakSound;

	public BobEffect fallbob;

	private float injuryLevel;

	private float injuredTime;

	public FallDamage()
	{
	}

	public void AddLegInjury(float inj)
	{
		this.SetLegInjury(this.GetLegInjury() + inj);
	}

	public void ClearInjury()
	{
		this.SetLegInjury(0f);
	}

	public void FallImpact(float fallspeed)
	{
		this.legBreakSound.Play(base.transform.position, 1f, 3f, 10f);
		if (base.localControlled)
		{
			HeadBob component = CameraMount.current.GetComponent<HeadBob>();
			component.AddEffect(this.fallbob);
		}
	}

	[RPC]
	protected void fIc(float fallspeed)
	{
		this.FallImpact(fallspeed);
	}

	[RPC]
	protected void fIm(Vector3 velocity, uLink.NetworkMessageInfo info)
	{
	}

	[RPC]
	protected void fIo(float injAmount)
	{
		this.SetLegInjury(injAmount);
	}

	public float GetLegInjury()
	{
		return this.injuryLevel;
	}

	[RPC]
	protected void ReadFallImpact(Vector3 velocity, uLink.NetworkMessageInfo info)
	{
	}

	public void ResetInjuryTime()
	{
		base.CancelInvoke("ClearInjury");
		float injuryLength = falldamage.injury_length * UnityEngine.Random.Range(0.9f, 1.1f);
		base.Invoke("ClearInjury", injuryLength);
	}

	public void SendFallImpact(Vector3 velocity)
	{
		if (velocity.y > -18f)
		{
			return;
		}
		base.networkView.RPC<Vector3>("fIm", uLink.RPCMode.Server, velocity);
	}

	public void SetLegInjury(float injAmount)
	{
		this.injuryLevel = injAmount;
		if (base.character.localPlayerControlled)
		{
			RPOS.InjuryUpdate();
		}
	}
}