using System;
using UnityEngine;

public class OfflineMode : MonoBehaviour
{
	[SerializeField]
	private CharacterPrefab characterPrefab;

	[SerializeField]
	private OfflinePlayer offlinePlayer;

	[SerializeField]
	private MountedCamera sceneCameraPrefab;

	[SerializeField]
	private bool useSceneViewWhenAvailable;

	[SerializeField]
	private bool paused;

	[SerializeField]
	private bool respawn;

	[SerializeField]
	private bool teleport;

	[SerializeField]
	private float timeScale = 1f;

	public OfflineMode()
	{
	}

	private void Start()
	{
	}
}