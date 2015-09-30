using Facepunch;
using System;

public interface IUseableNotifyDecline : IUseable, IComponentInterface<IUseable, MonoBehaviour, Useable>, IComponentInterface<IUseable, MonoBehaviour>, IComponentInterface<IUseable>
{
	void OnUseDeclined(Character user, UseResponse response, UseEnterRequest request);
}