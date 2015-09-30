using Facepunch;
using System;

public interface IUseableUpdated : IUseable, IComponentInterface<IUseable, MonoBehaviour, Useable>, IComponentInterface<IUseable, MonoBehaviour>, IComponentInterface<IUseable>
{
	UseUpdateFlags UseUpdateFlags
	{
		get;
	}

	void OnUseUpdate(Useable use);
}