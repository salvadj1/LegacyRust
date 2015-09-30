using Facepunch;
using System;
using UnityEngine;

public class BloodHelper : UnityEngine.MonoBehaviour
{
	private static GameObject bloodDecalPrefab;

	static BloodHelper()
	{
	}

	public BloodHelper()
	{
	}

	private static void BleedDir(Vector3 startPos, Vector3 dir, int hitMask)
	{
		RaycastHit raycastHit;
		Ray ray = new Ray(startPos + (dir * 0.25f), dir);
		if (Physics.Raycast(ray, out raycastHit, 4f, hitMask))
		{
			if (BloodHelper.bloodDecalPrefab == null && !Bundling.Load<GameObject>("content/effect/BloodDecal", out BloodHelper.bloodDecalPrefab))
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(raycastHit.normal);
			GameObject gameObject = UnityEngine.Object.Instantiate(BloodHelper.bloodDecalPrefab, raycastHit.point + (raycastHit.normal * UnityEngine.Random.Range(0.025f, 0.035f)), quaternion * Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(0, 360))) as GameObject;
			UnityEngine.Object.Destroy(gameObject, 12f);
		}
	}
}