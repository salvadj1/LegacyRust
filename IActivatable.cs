using Facepunch;
using System;

public interface IActivatable : IComponentInterface<IActivatable, MonoBehaviour, Activatable>, IComponentInterface<IActivatable, MonoBehaviour>, IComponentInterface<IActivatable>
{
	ActivationResult ActTrigger(Character instigator, ulong timestamp);
}