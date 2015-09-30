using System;
using UnityEngine;

public class LaserModRep : WeaponModRep
{
	private const float kVolume = 1f;

	private const float kMinAudioDistance = 1f;

	private const float kMaxAudioDistance = 4f;

	private const AudioRolloffMode kRolloffMode = AudioRolloffMode.Logarithmic;

	private bool is_vm;

	private LaserBeam[] beams;

	private static bool allow_3rd_lasers;

	static LaserModRep()
	{
		LaserModRep.allow_3rd_lasers = true;
	}

	protected LaserModRep(ItemModRepresentation.Caps caps, bool defaultOn) : base(caps, defaultOn)
	{
	}

	public LaserModRep() : this(ItemModRepresentation.Caps.BindStateFlags, false)
	{
	}

	protected LaserModRep(ItemModRepresentation.Caps caps) : this(caps, false)
	{
	}

	protected override void BindStateFlags(CharacterStateFlags flags, ItemModRepresentation.Reason reason)
	{
		base.BindStateFlags(flags, reason);
		base.SetOn(flags.laser, reason);
	}

	protected override void DisableMod(ItemModRepresentation.Reason reason)
	{
		LaserBeam laserBeam = null;
		LaserBeam[] laserBeamArray = this.beams;
		for (int i = 0; i < (int)laserBeamArray.Length; i++)
		{
			LaserBeam laserBeam1 = laserBeamArray[i];
			if (laserBeam1)
			{
				laserBeam = laserBeam1;
				laserBeam1.enabled = false;
			}
		}
		if (reason == ItemModRepresentation.Reason.Explicit)
		{
			this.PlaySound(laserBeam, base.modDataBlock.offSound);
		}
	}

	protected override void EnableMod(ItemModRepresentation.Reason reason)
	{
		LaserBeam laserBeam = null;
		LaserBeam[] laserBeamArray = this.beams;
		for (int i = 0; i < (int)laserBeamArray.Length; i++)
		{
			LaserBeam laserBeam1 = laserBeamArray[i];
			if (laserBeam1)
			{
				laserBeam = laserBeam1;
				laserBeam1.enabled = (this.is_vm ? true : LaserModRep.allow_3rd_lasers);
			}
		}
		if (reason == ItemModRepresentation.Reason.Explicit)
		{
			this.PlaySound(laserBeam, base.modDataBlock.onSound);
		}
	}

	protected override void OnAddAttached()
	{
		this.beams = base.attached.GetComponentsInChildren<LaserBeam>();
	}

	protected override void OnRemoveAttached()
	{
		this.beams = null;
	}

	private void PlaySound(LaserBeam anyBeam, AudioClip clip)
	{
		if (!anyBeam)
		{
			clip.PlayLocal(base.itemRep.transform, Vector3.zero, 1f, AudioRolloffMode.Logarithmic, 1f, 4f);
		}
		else
		{
			clip.PlayLocal(anyBeam.transform, Vector3.zero, 1f, AudioRolloffMode.Logarithmic, 1f, 4f);
		}
	}

	public override void SetAttached(GameObject attached, bool vm)
	{
		this.is_vm = vm;
		base.SetAttached(attached, vm);
	}

	protected override bool VerifyCompatible(GameObject attachment)
	{
		return attachment.GetComponentInChildren<LaserBeam>();
	}
}