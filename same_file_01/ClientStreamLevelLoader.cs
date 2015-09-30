using System;
using UnityEngine;

public class ClientStreamLevelLoader : MonoBehaviour
{
	[SerializeField]
	private RustLoader loaderPrefab;

	public ClientStreamLevelLoader()
	{
	}

	private void Start()
	{
		RustLoader rustLoader = (RustLoader)UnityEngine.Object.Instantiate(this.loaderPrefab);
		base.enabled = false;
	}
}