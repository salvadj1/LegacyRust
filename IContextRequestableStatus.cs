using Facepunch;

public interface IContextRequestableStatus : IContextRequestable, IComponentInterface<IContextRequestable, MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	ContextStatusFlags ContextStatusPoll();
}