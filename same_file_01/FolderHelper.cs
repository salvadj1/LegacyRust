using System;
using UnityEngine;

public class FolderHelper : MonoBehaviour
{
	public FolderHelper()
	{
	}

	private void Awake()
	{
		base.transform.DetachChildren();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}