using Facepunch.Progress;
using Rust;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class RustLevel
{
	private static void BroadcastGlobalMessage(string messageName)
	{
		foreach (GameObject gameObject in RustLevel.CollectRootGameObjects())
		{
			if (!gameObject)
			{
				continue;
			}
			gameObject.BroadcastMessage(messageName, SendMessageOptions.DontRequireReceiver);
		}
	}

	private static List<GameObject> CollectRootGameObjects()
	{
		HashSet<Transform> transforms = new HashSet<Transform>();
		List<GameObject> gameObjects = new List<GameObject>();
		UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(Transform));
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			UnityEngine.Object obj = objArray[i];
			if (obj)
			{
				Transform transforms1 = ((Transform)obj).root;
				if (transforms.Add(transforms1))
				{
					gameObjects.Add(transforms1.gameObject);
				}
			}
		}
		return gameObjects;
	}

	public static void LevelLoadLog(byte iStage)
	{
	}

	public static Coroutine Load(string levelName, out GameObject loader)
	{
		Globals.currentLevel = levelName;
		loader = new GameObject(string.Concat("Loading Level:", levelName), new Type[] { typeof(MonoBehaviour) });
		UnityEngine.Object.DontDestroyOnLoad(loader);
		MonoBehaviour component = loader.GetComponent<MonoBehaviour>();
		return component.StartCoroutine(RustLevel.LoadRoutine(component, levelName));
	}

	public static Coroutine Load(string levelName)
	{
		GameObject gameObject;
		return RustLevel.Load(levelName, out gameObject);
	}

	[DebuggerHidden]
	private static IEnumerator LoadRoutine(MonoBehaviour script, string levelName)
	{
		RustLevel.<LoadRoutine>c__Iterator30 variable = null;
		return variable;
	}

	private static Coroutine WaitForCondition(MonoBehaviour script, Func<bool> condition, string requestLabel)
	{
		return script.StartCoroutine(RustLevel.WaitForCondition(condition, requestLabel));
	}

	[DebuggerHidden]
	private static IEnumerator WaitForCondition(Func<bool> condition, string requestLabel)
	{
		RustLevel.<WaitForCondition>c__Iterator31 variable = null;
		return variable;
	}
}