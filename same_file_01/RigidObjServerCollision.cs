using System;
using UnityEngine;

[AddComponentMenu("")]
public sealed class RigidObjServerCollision : MonoBehaviour
{
	public const byte Enter = 0;

	public const byte Exit = 1;

	public const byte Stay = 2;

	[NonSerialized]
	public RigidObj rigidObj;

	public RigidObjServerCollision()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (this.rigidObj)
		{
			this.rigidObj.OnServerCollision(0, collision);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (this.rigidObj)
		{
			this.rigidObj.OnServerCollision(1, collision);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (this.rigidObj)
		{
			this.rigidObj.OnServerCollision(2, collision);
		}
	}
}