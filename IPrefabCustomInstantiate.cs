using System;

public interface IPrefabCustomInstantiate
{
	IDMain CustomInstantiatePrefab(ref CustomInstantiationArgs args);

	bool InitializePrefabInstance(NetInstance net);
}