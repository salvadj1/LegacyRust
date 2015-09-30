using System;
using UnityEngine;

public class GameTip : MonoBehaviour
{
	public string text = "Tooltip Text";

	public bool lineOfSight = true;

	public Vector3 testOffset = new Vector3(0f, 0f, 0f);

	public Vector3 positionOffset = new Vector3(0f, 0f, 0f);

	public Color textColor = Color.white;

	public float maxDistance = 16f;

	public Collider TargetCollider;

	public GameTip()
	{
	}

	public void OnWillRenderObject()
	{
		if (Camera.main != Camera.current)
		{
			return;
		}
		float single = Vector3.Distance(base.transform.position, Camera.main.transform.position);
		if (single > this.maxDistance)
		{
			return;
		}
		if (this.lineOfSight && this.TestLineOfSight(single))
		{
			return;
		}
		float single1 = 1f - single / this.maxDistance;
		float single2 = 2f / (single * (2f * Mathf.Tan((float)Camera.main.fieldOfView / 2f * 0.0174532924f)));
		GameTooltipManager.Singleton.UpdateTip(base.gameObject, this.text, base.transform.position + this.positionOffset, this.textColor, single1, single2);
	}

	public bool TestLineOfSight(float fDistance)
	{
		RaycastHit raycastHit;
		Vector3 vector3 = (base.transform.position + this.testOffset) - Camera.main.transform.position;
		vector3.Normalize();
		Ray ray = new Ray(Camera.main.transform.position, vector3);
		if (!Physics.Raycast(ray, out raycastHit, fDistance))
		{
			return false;
		}
		if (raycastHit.distance > fDistance - 0.5f)
		{
			return false;
		}
		if (this.TargetCollider && raycastHit.collider == this.TargetCollider)
		{
			return false;
		}
		if (base.transform.IsChildOf(raycastHit.collider.gameObject.transform))
		{
			return false;
		}
		if (raycastHit.collider.gameObject.transform.IsChildOf(base.transform))
		{
			return false;
		}
		return true;
	}
}