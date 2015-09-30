using System;
using System.Collections;
using UnityEngine;

public class RPOSPlaqueManager : MonoBehaviour
{
	public GameObject coldPlaque;

	public GameObject bleedingPlaque;

	public RPOSPlaqueManager()
	{
	}

	public void Awake()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				((Transform)enumerator.Current).gameObject.SetActive(false);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public void SetPlaqueActive(string plaqueName, bool on)
	{
		GameObject gameObject = null;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (current.name != plaqueName)
				{
					continue;
				}
				gameObject = current.gameObject;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		if (gameObject && gameObject.activeSelf != on)
		{
			gameObject.SetActive(on);
			float single = 21f;
			IEnumerator enumerator1 = base.transform.GetEnumerator();
			try
			{
				while (enumerator1.MoveNext())
				{
					Transform transforms = (Transform)enumerator1.Current;
					if (!transforms.gameObject.activeSelf)
					{
						continue;
					}
					transforms.SetLocalPositionY(single);
					single = single + 28f;
				}
			}
			finally
			{
				IDisposable disposable1 = enumerator1 as IDisposable;
				if (disposable1 == null)
				{
				}
				disposable1.Dispose();
			}
		}
	}
}