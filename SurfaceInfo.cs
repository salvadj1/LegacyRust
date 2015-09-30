using System;
using UnityEngine;

public class SurfaceInfo : MonoBehaviour
{
	public SurfaceInfoObject surface;

	public SurfaceInfo()
	{
	}

	public static void DoImpact(GameObject go, SurfaceInfoObject.ImpactType type, Vector3 worldPos, Quaternion rotation)
	{
		SurfaceInfoObject surfaceInfoFor = SurfaceInfo.GetSurfaceInfoFor(go, worldPos);
		UnityEngine.Object obj = UnityEngine.Object.Instantiate(surfaceInfoFor.GetImpactEffect(type), worldPos, rotation);
		UnityEngine.Object.Destroy(obj, 1f);
	}

	public static SurfaceInfoObject GetSurfaceInfoFor(Collider collider, Vector3 worldPos)
	{
		return SurfaceInfo.GetSurfaceInfoFor(collider.gameObject, worldPos);
	}

	public static SurfaceInfoObject GetSurfaceInfoFor(GameObject obj, Vector3 worldPos)
	{
		SurfaceInfo component = obj.GetComponent<SurfaceInfo>();
		if (component)
		{
			return component.SurfaceObj(worldPos);
		}
		IDBase dBase = obj.GetComponent<IDBase>();
		if (dBase)
		{
			SurfaceInfo surfaceInfo = dBase.idMain.GetComponent<SurfaceInfo>();
			if (surfaceInfo)
			{
				return surfaceInfo.SurfaceObj(worldPos);
			}
		}
		return SurfaceInfoObject.GetDefault();
	}

	public virtual SurfaceInfoObject SurfaceObj()
	{
		return this.surface;
	}

	public virtual SurfaceInfoObject SurfaceObj(Vector3 worldPos)
	{
		return this.surface;
	}
}