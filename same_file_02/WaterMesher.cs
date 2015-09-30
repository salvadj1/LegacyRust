using System;
using UnityEngine;

[AddComponentMenu("Water/Mesher")]
public class WaterMesher : MonoBehaviour
{
	private const int kPoints = 16;

	public WaterMesher next;

	public WaterMesher prev;

	public Vector2 inTangent;

	public Vector2 outTangent;

	public bool isRoot;

	public Vector2 position
	{
		get
		{
			Vector3 vector3 = base.transform.position;
			vector3.y = vector3.z;
			vector3.z = 0f;
			return new Vector2(vector3.x, vector3.y);
		}
		set
		{
			Vector3 vector3 = base.transform.position;
			vector3.x = value.x;
			vector3.z = value.y;
			base.transform.position = vector3;
		}
	}

	public Vector3 position3
	{
		get
		{
			return base.transform.position;
		}
	}

	public Vector2 smoothInTangent
	{
		get
		{
			return (!this.prev ? this.inTangent : (this.inTangent - this.prev.outTangent) / 2f);
		}
	}

	public Vector2 smoothOutTangent
	{
		get
		{
			return (!this.next ? this.inTangent : (this.outTangent - this.next.inTangent) / 2f);
		}
	}

	public WaterMesher()
	{
	}

	public Vector2 Point(float t, Vector2 p3)
	{
		Vector2 vector2 = this.position;
		Vector2 vector21 = vector2 + this.inTangent;
		Vector2 vector22 = p3 + this.outTangent;
		float single = 1f - t;
		vector2.x = vector2.x * single + vector21.x * t;
		vector2.y = vector2.y * single + vector21.y * t;
		vector21.x = vector21.x * single + vector22.x * t;
		vector21.y = vector21.y * single + vector22.y * t;
		vector22.x = vector22.x * single + p3.x * t;
		vector22.y = vector22.y * single + p3.y * t;
		vector2.x = vector2.x * single + vector21.x * t;
		vector2.y = vector2.y * single + vector21.y * t;
		vector21.x = vector21.x * single + vector22.x * t;
		vector21.y = vector21.y * single + vector22.y * t;
		vector2.x = vector2.x * single + vector21.x * t;
		vector2.y = vector2.y * single + vector21.y * t;
		return vector2;
	}

	public Vector3 Point3(float t, Vector2 p3)
	{
		Vector3 vector3 = new Vector3();
		Vector2 vector2 = this.Point(t, p3);
		vector3.x = vector2.x;
		vector3.y = base.transform.position.y;
		vector3.z = vector2.y;
		return vector3;
	}

	public Vector2 SmoothPoint(float t, Vector2 p3)
	{
		Vector2 vector2 = this.position;
		Vector2 vector21 = vector2 + this.smoothInTangent;
		Vector2 vector22 = p3 + this.smoothOutTangent;
		float single = 1f - t;
		vector2.x = vector2.x * single + vector21.x * t;
		vector2.y = vector2.y * single + vector21.y * t;
		vector21.x = vector21.x * single + vector22.x * t;
		vector21.y = vector21.y * single + vector22.y * t;
		vector22.x = vector22.x * single + p3.x * t;
		vector22.y = vector22.y * single + p3.y * t;
		vector2.x = vector2.x * single + vector21.x * t;
		vector2.y = vector2.y * single + vector21.y * t;
		vector21.x = vector21.x * single + vector22.x * t;
		vector21.y = vector21.y * single + vector22.y * t;
		vector2.x = vector2.x * single + vector21.x * t;
		vector2.y = vector2.y * single + vector21.y * t;
		return vector2;
	}
}