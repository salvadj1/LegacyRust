using UnityEngine;

public interface IComponentInterface<InterfaceType, MonoBehaviourType> : IComponentInterface<InterfaceType>
where InterfaceType : IComponentInterface<InterfaceType, MonoBehaviourType>
where MonoBehaviourType : MonoBehaviour
{

}