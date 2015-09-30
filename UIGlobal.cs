using System;
using UnityEngine;

[AddComponentMenu("")]
public class UIGlobal : MonoBehaviour
{
	private static UIGlobal g;

	public UIGlobal()
	{
	}

	public static void EnsureGlobal()
	{
		if (Application.isPlaying && !UIGlobal.g)
		{
			GameObject gameObject = new GameObject("__UIGlobal", new Type[] { typeof(UIGlobal) });
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			UIGlobal.g = gameObject.GetComponent<UIGlobal>();
		}
	}

	private void LateUpdate()
	{
		UIWidget.GlobalUpdate();
		UIPanel.GlobalUpdate();
	}
}