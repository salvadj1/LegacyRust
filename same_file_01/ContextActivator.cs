using Facepunch;
using System;
using UnityEngine;

public sealed class ContextActivator : Facepunch.MonoBehaviour, IContextRequestable, IContextRequestableQuick, IContextRequestableStatus, IContextRequestableText, IContextRequestablePointText, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	[SerializeField]
	private Activatable mainAction;

	[SerializeField]
	private ContextActivator.ActivationMode activationMode;

	[SerializeField]
	private Activatable[] extraActions;

	[SerializeField]
	private string defaultText;

	[SerializeField]
	private string onText;

	[SerializeField]
	private string offText;

	[SerializeField]
	private Vector3 defaultTextPoint;

	[SerializeField]
	private Vector3 onTextPoint;

	[SerializeField]
	private Vector3 offTextPoint;

	[SerializeField]
	private bool useTextPoint;

	[SerializeField]
	private bool useSpriteTextPoint;

	[SerializeField]
	private ContextActivator.SpriteQuickMode defaultSprite;

	[SerializeField]
	private ContextActivator.SpriteQuickMode onSprite;

	[SerializeField]
	private ContextActivator.SpriteQuickMode offSprite;

	[SerializeField]
	private bool isSwitch;

	private bool isToggle;

	private ActivationToggleState toggleState
	{
		get
		{
			if (!this.mainAction)
			{
				return ActivationToggleState.Unspecified;
			}
			return this.mainAction.toggleState;
		}
	}

	public ContextActivator()
	{
	}

	private ActivationResult ApplyActivatable(Activatable activatable, Character instigator, ulong timestamp, bool extra)
	{
		ActivationResult activationResult;
		if (!activatable)
		{
			activationResult = ActivationResult.Error_Destroyed;
		}
		else
		{
			ContextActivator.ActivationMode activationMode = this.activationMode;
			if (activationMode == ContextActivator.ActivationMode.TurnOn)
			{
				activationResult = activatable.Activate(true, instigator, timestamp);
			}
			else
			{
				activationResult = (activationMode == ContextActivator.ActivationMode.TurnOff ? activatable.Activate(false, instigator, timestamp) : activatable.Activate(instigator, timestamp));
			}
		}
		return activationResult;
	}

	bool IContextRequestablePointText.ContextTextPoint(out Vector3 worldPoint)
	{
		if (!this.useTextPoint)
		{
			worldPoint = new Vector3();
			return false;
		}
		if (!this.useSpriteTextPoint || !ContextRequestable.PointUtil.SpriteOrOrigin(this, out worldPoint))
		{
			if (!this.isSwitch)
			{
				worldPoint = this.defaultTextPoint;
			}
			else
			{
				ActivationToggleState activationToggleState = this.toggleState;
				if (activationToggleState == ActivationToggleState.On)
				{
					worldPoint = this.onTextPoint;
				}
				else if (activationToggleState == ActivationToggleState.Off)
				{
					worldPoint = this.offTextPoint;
				}
				else
				{
					worldPoint = this.defaultTextPoint;
				}
			}
			worldPoint = base.transform.TransformPoint(worldPoint);
		}
		return true;
	}

	ContextStatusFlags IContextRequestableStatus.ContextStatusPoll()
	{
		ContextActivator.SpriteQuickMode spriteQuickMode;
		if (!this.isSwitch)
		{
			spriteQuickMode = this.defaultSprite;
		}
		else
		{
			ActivationToggleState activationToggleState = this.toggleState;
			if (activationToggleState == ActivationToggleState.On)
			{
				spriteQuickMode = this.onSprite;
			}
			else
			{
				spriteQuickMode = (activationToggleState == ActivationToggleState.Off ? this.offSprite : this.defaultSprite);
			}
		}
		switch (spriteQuickMode)
		{
			case ContextActivator.SpriteQuickMode.Faded:
			{
				return ContextStatusFlags.SpriteFlag0;
			}
			case ContextActivator.SpriteQuickMode.AlwaysVisible:
			{
				return ContextStatusFlags.SpriteFlag0 | ContextStatusFlags.SpriteFlag1;
			}
			case ContextActivator.SpriteQuickMode.NeverVisisble:
			{
				return ContextStatusFlags.SpriteFlag1;
			}
		}
		return 0;
	}

	string IContextRequestableText.ContextText(Controllable localControllable)
	{
		if (this.isSwitch)
		{
			ActivationToggleState activationToggleState = this.toggleState;
			if (activationToggleState == ActivationToggleState.On)
			{
				return this.onText;
			}
			if (activationToggleState == ActivationToggleState.Off)
			{
				return this.offText;
			}
		}
		return this.defaultText;
	}

	private enum ActivationMode
	{
		ActivateOrToggle,
		TurnOn,
		TurnOff
	}

	private enum SpriteQuickMode
	{
		Default,
		Faded,
		AlwaysVisible,
		NeverVisisble
	}
}