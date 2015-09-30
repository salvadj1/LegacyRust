using UnityEngine;

public interface IComponentInterface<InterfaceType, MonoBehaviourType, InterfaceDriverType> : IComponentInterface<InterfaceType, MonoBehaviourType>, IComponentInterface<InterfaceType>
where InterfaceType : IComponentInterface<InterfaceType, MonoBehaviourType, InterfaceDriverType>
where MonoBehaviourType : MonoBehaviour
where InterfaceDriverType : MonoBehaviour, IComponentInterfaceDriver<InterfaceType, MonoBehaviourType, InterfaceDriverType>
{

}