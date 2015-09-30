using Facepunch;
using System;

public interface IActivatableFill : IActivatable, IComponentInterface<IActivatable, MonoBehaviour, Activatable>, IComponentInterface<IActivatable, MonoBehaviour>, IComponentInterface<IActivatable>
{
	void ActivatableChanged(Activatable activatable, bool nonNull);
}