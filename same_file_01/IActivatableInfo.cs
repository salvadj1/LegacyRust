using Facepunch;
using System;

public interface IActivatableInfo : IActivatable, IComponentInterface<IActivatable, MonoBehaviour, Activatable>, IComponentInterface<IActivatable, MonoBehaviour>, IComponentInterface<IActivatable>
{
	void ActInfo(out ActivatableInfo info);
}