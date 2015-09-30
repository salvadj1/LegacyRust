using Facepunch;
using System;

public interface IUseable : IComponentInterface<IUseable, MonoBehaviour, Useable>, IComponentInterface<IUseable, MonoBehaviour>, IComponentInterface<IUseable>
{
	void OnUseEnter(Useable use);

	void OnUseExit(Useable use, UseExitReason reason);
}