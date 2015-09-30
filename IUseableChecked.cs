using Facepunch;

public interface IUseableChecked : IUseable, IComponentInterface<IUseable, MonoBehaviour, Useable>, IComponentInterface<IUseable, MonoBehaviour>, IComponentInterface<IUseable>
{
	UseCheck CanUse(Character user, UseEnterRequest request);
}