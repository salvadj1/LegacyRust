using System;
using UnityEngine;

public class DummyHelper : MonoBehaviour
{
	public DummyHelper()
	{
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}
}