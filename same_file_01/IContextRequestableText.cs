using Facepunch;
using System;

public interface IContextRequestableText : IContextRequestable, IComponentInterface<IContextRequestable, MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	string ContextText(Controllable localControllable);
}