using Facepunch;
using System;
using uLink;
using UnityEngine;

public class CullCell : Facepunch.MonoBehaviour
{
	private const float kMaxDistance = 150f;

	private const float kFadeDistance = 10f;

	[NonSerialized]
	public int groupID;

	[NonSerialized]
	public Vector2 center;

	[NonSerialized]
	public float extent;

	[NonSerialized]
	public float size;

	[NonSerialized]
	public float y_xy;

	[NonSerialized]
	public float y_Xy;

	[NonSerialized]
	public float y_xY;

	[NonSerialized]
	public float y_XY;

	[NonSerialized]
	public float y_mc;

	[NonSerialized]
	public float y_my;

	[NonSerialized]
	public float y_mY;

	[NonSerialized]
	public float y_xc;

	[NonSerialized]
	public float y_Xc;

	[NonSerialized]
	public Bounds bounds;

	[NonSerialized]
	private Transform t_xy;

	[NonSerialized]
	private Transform t_XY;

	[NonSerialized]
	private Transform t_Xy;

	[NonSerialized]
	private Transform t_xY;

	[NonSerialized]
	private Transform t_xc;

	[NonSerialized]
	private Transform t_Xc;

	[NonSerialized]
	private Transform t_my;

	[NonSerialized]
	private Transform t_mY;

	[NonSerialized]
	private Transform t_mc;

	private string groupString;

	public static Quaternion instantiateRotation
	{
		get
		{
			return new Quaternion(0f, 0.7071068f, 0.7071068f, 0f);
		}
	}

	public CullCell()
	{
	}

	private static float HeightCast(Vector2 point)
	{
		RaycastHit raycastHit;
		return (!Physics.Raycast(new Vector3(point.x, 5000f, point.y), Vector3.down, out raycastHit, Single.PositiveInfinity, CullCell.g.terrainMask) ? 0f : raycastHit.point.y);
	}

	private void OnGUI()
	{
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		Vector3 tMc = this.t_mc.position;
		Camera camera = Camera.main;
		if (camera)
		{
			Vector3 screenPoint = camera.WorldToScreenPoint(tMc);
			if (screenPoint.z > 0f && screenPoint.z < 150f)
			{
				Vector2 gUIPoint = GUIUtility.ScreenToGUIPoint(screenPoint);
				gUIPoint.y = (float)Screen.height - (gUIPoint.y + 1f);
				if (screenPoint.z > 10f)
				{
					GUI.color = GUI.color * new Color(1f, 1f, 1f, 1f - (screenPoint.z - 10f) / 140f);
				}
				Rect rect = new Rect(gUIPoint.x - 64f, gUIPoint.y - 12f, 128f, 24f);
				if (string.IsNullOrEmpty(this.groupString))
				{
					this.groupString = base.networkView.@group.ToString();
				}
				GUI.Label(rect, this.groupString);
			}
		}
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		ushort num;
		ushort num1;
		uLink.NetworkView networkView = info.networkView;
		this.groupID = info.networkView.@group;
		this.center = CullGrid.Flat(networkView.position);
		this.size = networkView.position.y;
		this.extent = this.size / 2f;
		CullGrid.CellFromGroupID(this.groupID, out num, out num1);
		base.name = string.Format("GRID-CELL:{0:00000}-[{1},{2}]", this.groupID, num, num1);
		this.y_mc = CullCell.HeightCast(this.center);
		this.y_xy = CullCell.HeightCast(new Vector2(this.center.x - this.extent, this.center.y - this.extent));
		this.y_XY = CullCell.HeightCast(new Vector2(this.center.x + this.extent, this.center.y + this.extent));
		this.y_Xy = CullCell.HeightCast(new Vector2(this.center.x + this.extent, this.center.y - this.extent));
		this.y_xY = CullCell.HeightCast(new Vector2(this.center.x - this.extent, this.center.y + this.extent));
		this.y_xc = CullCell.HeightCast(new Vector2(this.center.x - this.extent, this.center.y));
		this.y_Xc = CullCell.HeightCast(new Vector2(this.center.x + this.extent, this.center.y));
		this.y_my = CullCell.HeightCast(new Vector2(this.center.x, this.center.y - this.extent));
		this.y_mY = CullCell.HeightCast(new Vector2(this.center.x, this.center.y + this.extent));
		base.transform.position = new Vector3(this.center.x, this.y_mc, this.center.y);
		float single = Mathf.Min(new float[] { this.y_xy, this.y_XY, this.y_Xy, this.y_xY, this.y_xc, this.y_Xc, this.y_my, this.y_mY, this.y_mc });
		float single1 = Mathf.Max(new float[] { this.y_xy, this.y_XY, this.y_Xy, this.y_xY, this.y_xc, this.y_Xc, this.y_my, this.y_mY, this.y_mc }) - single;
		this.bounds = new Bounds(new Vector3(this.center.x, single + single1 * 0.5f, this.center.y), new Vector3(this.size, single1, this.size));
		Transform bound = base.transform;
		this.t_xy = bound.FindChild("BL");
		this.t_XY = bound.FindChild("FR");
		this.t_Xy = bound.FindChild("BR");
		this.t_xY = bound.FindChild("FL");
		this.t_xc = bound.FindChild("ML");
		this.t_Xc = bound.FindChild("MR");
		this.t_my = bound.FindChild("BC");
		this.t_mY = bound.FindChild("FC");
		this.t_mc = bound.FindChild("MC");
		this.t_xy.position = new Vector3(this.center.x - this.extent, this.y_xy, this.center.y - this.extent);
		this.t_XY.position = new Vector3(this.center.x + this.extent, this.y_XY, this.center.y + this.extent);
		this.t_Xy.position = new Vector3(this.center.x + this.extent, this.y_Xy, this.center.y - this.extent);
		this.t_xY.position = new Vector3(this.center.x - this.extent, this.y_xY, this.center.y + this.extent);
		this.t_xc.position = new Vector3(this.center.x - this.extent, this.y_xc, this.center.y);
		this.t_Xc.position = new Vector3(this.center.x + this.extent, this.y_Xc, this.center.y);
		this.t_my.position = new Vector3(this.center.x, this.y_my, this.center.y - this.extent);
		this.t_mY.position = new Vector3(this.center.x, this.y_mY, this.center.y + this.extent);
		this.t_mc.position = new Vector3(this.center.x, this.y_mc, this.center.y);
		bound.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().localBounds = new Bounds(new Vector3(0f, this.y_mc - (single + single1 * 0.5f), 0f), new Vector3(this.size, single1, this.size));
	}

	private static class g
	{
		public readonly static int terrainMask;

		static g()
		{
			CullCell.g.terrainMask = 1 << (LayerMask.NameToLayer("Terrain") & 31);
		}
	}
}