using Facepunch;
using Facepunch.Cursor;
using System;
using UnityEngine;

public sealed class ContextProbe : IDLocalCharacterAddon
{
	private const IDLocalCharacterAddon.AddonFlags ContextProbeAddonFlags = IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake | IDLocalCharacterAddon.AddonFlags.FireOnWillRemoveAddon;

	[NonSerialized]
	private float raycastLength;

	[NonSerialized]
	private UnityEngine.MonoBehaviour lastUseHighlight;

	[NonSerialized]
	private bool hasHighlight;

	private static ContextProbe singleton;

	public static bool aProbeIsHighlighting
	{
		get
		{
			return (!ContextProbe.singleton ? false : ContextProbe.singleton.hasHighlight);
		}
	}

	public bool isHighlighting
	{
		get
		{
			return this.hasHighlight;
		}
	}

	public ContextProbe() : this(IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake | IDLocalCharacterAddon.AddonFlags.FireOnWillRemoveAddon)
	{
	}

	private ContextProbe(IDLocalCharacterAddon.AddonFlags addonFlags) : base(addonFlags)
	{
	}

	private bool ClientCheckUse(Ray ray, bool press)
	{
		RaycastHit raycastHit;
		Facepunch.MonoBehaviour monoBehaviour;
		UnityEngine.MonoBehaviour monoBehaviour1;
		NetEntityID netEntityID;
		NetEntityID.Kind kind;
		Contextual contextual;
		if (!Physics.Raycast(ray, out raycastHit, this.raycastLength, -201523205))
		{
			monoBehaviour = null;
		}
		else
		{
			Transform transforms = raycastHit.transform;
			Transform transforms1 = transforms.parent;
			while (true)
			{
				NetEntityID.Kind kind1 = NetEntityID.Of(transforms, out netEntityID, out monoBehaviour1);
				kind = kind1;
				if ((int)kind1 != 0 || !transforms1)
				{
					break;
				}
				transforms = transforms1;
				transforms1 = transforms.parent;
			}
			if ((int)kind == 0)
			{
				monoBehaviour = null;
			}
			else if (!Contextual.ContextOf(monoBehaviour1, out contextual))
			{
				monoBehaviour = null;
			}
			else
			{
				monoBehaviour = contextual.implementor;
				if (press)
				{
					Context.BeginQuery(contextual);
				}
			}
		}
		if (monoBehaviour != this.lastUseHighlight)
		{
			this.lastUseHighlight = monoBehaviour;
			if (!monoBehaviour)
			{
				RPOS.UseHoverTextClear();
			}
			else
			{
				IContextRequestableText contextRequestableText = monoBehaviour as IContextRequestableText;
				if (contextRequestableText == null)
				{
					RPOS.UseHoverTextSet(monoBehaviour.name);
				}
				else
				{
					RPOS.UseHoverTextSet(base.controllable, contextRequestableText);
				}
			}
		}
		return monoBehaviour;
	}

	protected override void OnAddonAwake()
	{
		ContextProbe.singleton = this;
		this.raycastLength = base.GetTrait<CharacterContextProbeTrait>().rayLength;
	}

	protected override void OnWillRemoveAddon()
	{
		if (ContextProbe.singleton == this)
		{
			ContextProbe.singleton = null;
		}
		this.hasHighlight = false;
		this.lastUseHighlight = null;
	}

	private void Update()
	{
		bool buttonDown;
		bool buttonUp;
		if (base.dead)
		{
			return;
		}
		if (!LockCursorManager.IsLocked())
		{
			buttonDown = false;
			buttonUp = (!Context.WorkingInMenu ? false : Context.ButtonUp);
		}
		else
		{
			buttonDown = Context.ButtonDown;
			buttonUp = Context.ButtonUp;
		}
		this.hasHighlight = this.ClientCheckUse(base.eyesRay, buttonDown);
		if (Context.ButtonUp)
		{
			Context.EndQuery();
		}
	}
}