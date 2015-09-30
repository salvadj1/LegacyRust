using System;
using UnityEngine;

public class GUIHide : MonoBehaviour
{
	public GUIHide()
	{
	}

	public static void SetVisible(bool bShow)
	{
		UnityEngine.Object[] objArray = Resources.FindObjectsOfTypeAll(typeof(GUIHide));
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			GUIHide gUIHide = (GUIHide)objArray[i];
			if (gUIHide.gameObject == null)
			{
				return;
			}
			gUIHide.gameObject.SetActive(bShow);
		}
	}
}