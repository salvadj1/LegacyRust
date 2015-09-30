using System;
using UnityEngine;

public class HUDHitIndicator : HUDIndicator
{
	private const float kMIN = 0.003921569f;

	private const float kMAX = 0.996078432f;

	public UITexture texture;

	public AnimationCurve curve;

	private UIMaterial material;

	private double startTime;

	private Vector3 worldPosition;

	private bool followPoint;

	public HUDHitIndicator()
	{
	}

	private void Awake()
	{
		this.startTime = NetCull.time;
		this.material = this.texture.material.Clone();
		this.texture.material = this.material;
	}

	protected override bool Continue()
	{
		float single;
		float single1 = (float)(HUDIndicator.stepTime - this.startTime);
		if (single1 > this.curve[this.curve.length - 1].time)
		{
			return false;
		}
		this.material.Set("_AlphaValue", Mathf.Clamp(this.curve.Evaluate(single1), 0.003921569f, 0.996078432f));
		if (this.followPoint)
		{
			Vector3 vector3 = base.transform.position;
			Vector3 point = base.GetPoint(HUDIndicator.PlacementSpace.World, this.worldPosition);
			if (vector3.z != point.z)
			{
				Plane plane = new Plane(-base.transform.forward, vector3);
				Ray ray = new Ray(point, Vector3.forward);
				if (!plane.Raycast(ray, out single))
				{
					ray.direction = -ray.direction;
					point = (!plane.Raycast(ray, out single) ? vector3 : ray.GetPoint(single));
				}
				else
				{
					point = ray.GetPoint(single);
				}
			}
			if (point != vector3)
			{
				base.transform.position = point;
			}
		}
		return true;
	}

	public static void CreateIndicator(Vector3 worldPoint, bool followPoint, HUDHitIndicator prefab)
	{
		HUDHitIndicator hUDHitIndicator = (HUDHitIndicator)HUDIndicator.InstantiateIndicator(HUDIndicator.ScratchTarget.CenteredAuto, prefab, HUDIndicator.PlacementSpace.World, worldPoint);
		hUDHitIndicator.worldPosition = worldPoint;
		hUDHitIndicator.followPoint = followPoint;
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		if (this.material)
		{
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}
	}
}