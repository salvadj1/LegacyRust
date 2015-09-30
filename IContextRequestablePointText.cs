using Facepunch;
using System;
using UnityEngine;

public interface IContextRequestablePointText : IContextRequestable, IContextRequestableText, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	bool ContextTextPoint(out Vector3 worldPoint);
}