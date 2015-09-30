using System;
using UnityEngine;

public class GameUI : MonoBehaviour
{
	public GameUI()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		Debug.Log("GameUI Loaded");
	}
}