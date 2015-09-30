using System;
using UnityEngine;

public interface IComponentInterfaceDriver<InterfaceType, MonoBehaviourType, InterfaceDriverType>
where InterfaceType : IComponentInterface<InterfaceType, MonoBehaviourType, InterfaceDriverType>
where MonoBehaviourType : MonoBehaviour
where InterfaceDriverType : MonoBehaviour, IComponentInterfaceDriver<InterfaceType, MonoBehaviourType, InterfaceDriverType>
{
	InterfaceDriverType driver
	{
		get;
	}

	bool exists
	{
		get;
	}

	MonoBehaviourType implementor
	{
		get;
	}

	InterfaceType @interface
	{
		get;
	}
}