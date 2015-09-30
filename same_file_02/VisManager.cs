using System;
using UnityEngine;

[AddComponentMenu("")]
public class VisManager : MonoBehaviour
{
	private static bool isUpdatingVisiblity;

	public static bool guardedUpdate
	{
		get
		{
			return VisManager.isUpdatingVisiblity;
		}
	}

	public VisManager()
	{
	}

	private void Reset()
	{
		Debug.LogError("REMOVE ME NOW, I GET GENERATED AT RUN TIME", this);
	}

	private void Update()
	{
		if (!VisManager.isUpdatingVisiblity)
		{
			VisManager.isUpdatingVisiblity = true;
			try
			{
				try
				{
					VisNode.Process();
				}
				catch (Exception exception)
				{
					Debug.LogError(string.Format("{0}\n-- Vis data potentially compromised\n", exception));
				}
			}
			finally
			{
				VisManager.isUpdatingVisiblity = false;
			}
		}
	}
}