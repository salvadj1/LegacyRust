using Facepunch;
using System;

public interface IContextRequestableUpdatingText : IContextRequestable, IContextRequestableText, IComponentInterface<IContextRequestable, MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	string ContextTextUpdate(Controllable localControllable, string lastText);
}