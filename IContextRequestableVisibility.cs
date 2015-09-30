using Facepunch;
using System;

public interface IContextRequestableVisibility : IContextRequestable, IComponentInterface<IContextRequestable, MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	void OnContextVisibilityChanged(ContextSprite sprite, bool nowVisible);
}