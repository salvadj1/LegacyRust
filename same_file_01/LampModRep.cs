using System;
using UnityEngine;

public class LampModRep : WeaponModRep
{
	private const float kVolume = 1f;

	private const float kMinAudioDistance = 1f;

	private const float kMaxAudioDistance = 4f;

	private const AudioRolloffMode kRolloffMode = AudioRolloffMode.Logarithmic;

	private Light[] lights;

	protected LampModRep(ItemModRepresentation.Caps caps, bool defaultOn) : base(caps, defaultOn)
	{
	}

	public LampModRep() : this(ItemModRepresentation.Caps.BindStateFlags, false)
	{
	}

	protected LampModRep(ItemModRepresentation.Caps caps) : this(caps, false)
	{
	}

	protected override void BindStateFlags(CharacterStateFlags flags, ItemModRepresentation.Reason reason)
	{
		base.BindStateFlags(flags, reason);
		base.SetOn(flags.lamp, reason);
	}

	protected override void DisableMod(ItemModRepresentation.Reason reason)
	{
		Light light = null;
		Light[] lightArray = this.lights;
		for (int i = 0; i < (int)lightArray.Length; i++)
		{
			Light light1 = lightArray[i];
			if (light1)
			{
				light1.enabled = false;
				light = light1;
			}
		}
		if (reason == ItemModRepresentation.Reason.Explicit)
		{
			this.PlaySound(light, base.modDataBlock.offSound);
		}
	}

	protected override void EnableMod(ItemModRepresentation.Reason reason)
	{
		Light light = null;
		Light[] lightArray = this.lights;
		for (int i = 0; i < (int)lightArray.Length; i++)
		{
			Light light1 = lightArray[i];
			if (light1)
			{
				light1.enabled = true;
				light = light1;
			}
		}
		if (reason == ItemModRepresentation.Reason.Explicit)
		{
			this.PlaySound(light, base.modDataBlock.onSound);
		}
	}

	protected override void OnAddAttached()
	{
		this.lights = base.attached.GetComponentsInChildren<Light>();
	}

	protected override void OnRemoveAttached()
	{
		this.lights = null;
	}

	private void PlaySound(Light anyLight, AudioClip clip)
	{
		if (!anyLight)
		{
			clip.PlayLocal(base.itemRep.transform, Vector3.zero, 1f, AudioRolloffMode.Logarithmic, 1f, 4f);
		}
		else
		{
			clip.PlayLocal(anyLight.transform, Vector3.zero, 1f, AudioRolloffMode.Logarithmic, 1f, 4f);
		}
	}

	protected override bool VerifyCompatible(GameObject attachment)
	{
		return attachment.GetComponentInChildren<Light>();
	}
}