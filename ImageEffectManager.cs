using System;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffectManager : MonoBehaviour
{
	private static ImageEffectManager singleton;

	private static Dictionary<Type, bool> states;

	static ImageEffectManager()
	{
		ImageEffectManager.states = new Dictionary<Type, bool>();
	}

	public ImageEffectManager()
	{
	}

	public static bool GetEnabled<T>()
	where T : MonoBehaviour
	{
		return (!ImageEffectManager.states.ContainsKey(typeof(T)) ? true : ImageEffectManager.states[typeof(T)]);
	}

	public static T GetInstance<T>()
	where T : MonoBehaviour
	{
		return (ImageEffectManager.singleton == null ? (T)null : ImageEffectManager.singleton.GetComponent<T>());
	}

	protected void OnDisable()
	{
		if (ImageEffectManager.singleton == this)
		{
			ImageEffectManager.singleton = null;
		}
	}

	protected void OnEnable()
	{
		ImageEffectManager.singleton = this;
	}

	public static void SetEnabled<T>(bool value)
	where T : MonoBehaviour
	{
		if (ImageEffectManager.GetInstance<T>() != null)
		{
			ImageEffectManager.GetInstance<T>().enabled = value;
		}
		if (ImageEffectManager.states.ContainsKey(typeof(T)))
		{
			ImageEffectManager.states[typeof(T)] = value;
		}
		else
		{
			ImageEffectManager.states.Add(typeof(T), value);
		}
	}

	protected void Start()
	{
		MonoBehaviour[] components = base.GetComponents<MonoBehaviour>();
		for (int i = 0; i < (int)components.Length; i++)
		{
			MonoBehaviour item = components[i];
			if (!item.enabled)
			{
				return;
			}
			Type type = item.GetType();
			if (ImageEffectManager.states.ContainsKey(type))
			{
				item.enabled = ImageEffectManager.states[type];
			}
		}
	}
}