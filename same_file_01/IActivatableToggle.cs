using Facepunch;
using System;

public interface IActivatableToggle : IActivatable, IComponentInterface<IActivatable, MonoBehaviour, Activatable>, IComponentInterface<IActivatable, MonoBehaviour>, IComponentInterface<IActivatable>
{
	ActivationToggleState ActGetToggleState();

	ActivationResult ActTrigger(Character instigator, ActivationToggleState toggleTarget, ulong timestamp);
}