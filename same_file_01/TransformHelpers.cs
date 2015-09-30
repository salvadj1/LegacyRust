using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public static class TransformHelpers
{
	private readonly static Vector2[] upHeightTests;

	static TransformHelpers()
	{
		Vector2[] vector2Array = new Vector2[4];
		Vector2 vector2 = new Vector2()
		{
			y = -1000f
		};
		vector2Array[0] = vector2;
		Vector2 vector21 = new Vector2()
		{
			x = 5f,
			y = -1000f
		};
		vector2Array[1] = vector21;
		Vector2 vector22 = new Vector2()
		{
			x = 30f,
			y = -2000f
		};
		vector2Array[2] = vector22;
		Vector2 vector23 = new Vector2()
		{
			x = 200f,
			y = -4000f
		};
		vector2Array[3] = vector23;
		TransformHelpers.upHeightTests = vector2Array;
	}

	public static float Dist2D(Vector3 a, Vector3 b)
	{
		Vector2 vector2 = new Vector2();
		vector2.x = b.x - a.x;
		vector2.y = b.z - a.z;
		return Mathf.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y);
	}

	public static void DropToGround(this Transform transform, bool useNormal)
	{
		Vector3 vector3;
		Vector3 vector31;
		if (transform.GetGroundInfo(out vector3, out vector31))
		{
			transform.position = vector3;
			if (useNormal)
			{
				transform.rotation = Quaternion.LookRotation(vector31);
			}
		}
	}

	public static bool GetGroundInfo(this Transform transform, out Vector3 pos, out Vector3 normal)
	{
		return TransformHelpers.GetGroundInfoNoTransform(transform.position, out pos, out normal);
	}

	public static bool GetGroundInfo(Vector3 startPos, out Vector3 pos, out Vector3 normal)
	{
		return TransformHelpers.GetGroundInfo(startPos, 100f, out pos, out normal);
	}

	public static bool GetGroundInfo(Vector3 startPos, float range, out Vector3 pos, out Vector3 normal)
	{
		RaycastHit raycastHit;
		startPos.y = startPos.y + 0.25f;
		Ray ray = new Ray(startPos, Vector3.down);
		if (!Physics.Raycast(ray, out raycastHit, range, -472317957))
		{
			pos = startPos;
			normal = Vector3.up;
			return false;
		}
		pos = raycastHit.point;
		normal = raycastHit.normal;
		return true;
	}

	private static bool GetGroundInfoNavMesh(Vector3 startPos, out NavMeshHit hit, float maxVariationFallback, int acceptMask)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		int num = ~acceptMask;
		float single = startPos.x;
		float single1 = single;
		vector3.x = single;
		vector31.x = single1;
		float single2 = startPos.z;
		single1 = single2;
		vector3.z = single2;
		vector31.z = single1;
		for (int i = 0; i < (int)TransformHelpers.upHeightTests.Length; i++)
		{
			vector3.y = startPos.y + TransformHelpers.upHeightTests[i].x;
			vector31.y = startPos.y + TransformHelpers.upHeightTests[i].y;
			if (NavMesh.Raycast(vector3, vector31, out hit, num))
			{
				return true;
			}
		}
		if (NavMesh.SamplePosition(startPos, out hit, maxVariationFallback, acceptMask))
		{
			return true;
		}
		return false;
	}

	public static bool GetGroundInfoNavMesh(Vector3 startPos, out Vector3 pos, float maxVariationFallback, int acceptMask)
	{
		NavMeshHit navMeshHit;
		if (!TransformHelpers.GetGroundInfoNavMesh(startPos, out navMeshHit, maxVariationFallback, acceptMask))
		{
			pos = startPos;
			return false;
		}
		pos = navMeshHit.position;
		return true;
	}

	public static bool GetGroundInfoNavMesh(Vector3 startPos, out Vector3 pos, float maxVariationFallback)
	{
		return TransformHelpers.GetGroundInfoNavMesh(startPos, out pos, maxVariationFallback, -1);
	}

	public static bool GetGroundInfoNavMesh(Vector3 startPos, out Vector3 pos)
	{
		return TransformHelpers.GetGroundInfoNavMesh(startPos, out pos, 200f);
	}

	public static bool GetGroundInfoNoTransform(Vector3 transformOrigin, out Vector3 pos, out Vector3 normal)
	{
		RaycastHit raycastHit;
		Vector3 vector3 = transformOrigin;
		vector3.y = vector3.y + 0.25f;
		Ray ray = new Ray(vector3, Vector3.down);
		if (!Physics.Raycast(ray, out raycastHit, 1000f))
		{
			pos = transformOrigin;
			normal = Vector3.up;
			return false;
		}
		pos = raycastHit.point;
		normal = raycastHit.normal;
		return true;
	}

	public static Quaternion GetGroundInfoRotation(Quaternion ang, Vector3 y)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		float single = y.magnitude;
		if (Mathf.Approximately(single, 0f))
		{
			y = Vector3.up;
			single = 0f;
		}
		float single1 = 0f;
		float single2 = single1;
		vector3.y = single1;
		float single3 = single2;
		single2 = single3;
		vector3.x = single3;
		float single4 = single2;
		single2 = single4;
		vector31.z = single4;
		vector31.y = single2;
		float single5 = single;
		single2 = single5;
		vector3.z = single5;
		vector31.x = single2;
		vector31 = ang * vector31;
		vector3 = ang * vector3;
		float single6 = vector3.x * y.x + vector3.y * y.y + vector3.z * y.z;
		float single7 = vector31.x * y.x + vector31.y * y.y + vector31.z * y.z;
		if (single6 * single6 > single7 * single7)
		{
			return TransformHelpers.LookRotationForcedUp(vector31, y);
		}
		return TransformHelpers.LookRotationForcedUp(vector3, y);
	}

	public static bool GetGroundInfoTerrainOnly(Vector3 startPos, float range, out Vector3 pos, out Vector3 normal)
	{
		RaycastHit raycastHit;
		startPos.y = startPos.y + 0.25f;
		Ray ray = new Ray(startPos, Vector3.down);
		if (!Physics.Raycast(ray, out raycastHit, range + 0.25f) || !(raycastHit.collider is TerrainCollider))
		{
			pos = startPos;
			normal = Vector3.up;
			return false;
		}
		pos = raycastHit.point;
		normal = raycastHit.normal;
		return true;
	}

	public static bool GetIDBaseFromCollider(Collider collider, out IDBase id)
	{
		if (!collider)
		{
			id = null;
			return false;
		}
		id = IDBase.Get(collider);
		if (id)
		{
			return true;
		}
		Rigidbody rigidbody = collider.attachedRigidbody;
		if (!rigidbody)
		{
			return false;
		}
		id = rigidbody.GetComponent<IDBase>();
		return id;
	}

	public static bool GetIDMainFromCollider(Collider collider, out IDMain main)
	{
		IDBase dBase;
		if (!TransformHelpers.GetIDBaseFromCollider(collider, out dBase))
		{
			main = null;
			return false;
		}
		main = dBase.idMain;
		return main;
	}

	private static float InvSqrt(float x)
	{
		return 1f / Mathf.Sqrt(x);
	}

	private static float InvSqrt(float x, float y)
	{
		return 1f / Mathf.Sqrt(x * x + y * y);
	}

	private static float InvSqrt(float x, float y, float z)
	{
		return 1f / Mathf.Sqrt(x * x + y * y + z * z);
	}

	private static float InvSqrt(float x, float y, float z, float w)
	{
		return 1f / Mathf.Sqrt(x * x + y * y + z * z + w * w);
	}

	[DebuggerHidden]
	private static IEnumerable<Transform> IterateChildren(Transform parent, int iChild)
	{
		TransformHelpers.<IterateChildren>c__Iterator24 variable = null;
		return variable;
	}

	public static List<Transform> ListDecendantsByDepth(this Transform root)
	{
		return (root.childCount != 0 ? new List<Transform>(TransformHelpers.IterateChildren(root, 0)) : new List<Transform>(0));
	}

	public static Quaternion LookRotationForcedUp(Vector3 forward, Vector3 up)
	{
		if (forward == up)
		{
			return Quaternion.LookRotation(up);
		}
		Vector3 vector3 = Vector3.Cross(forward, up);
		forward = Vector3.Cross(up, vector3);
		if (forward == Vector3.zero)
		{
			forward = Vector3.forward;
		}
		return Quaternion.LookRotation(forward, up);
	}

	public static Quaternion LookRotationForcedUp(Quaternion rotation, Vector3 up)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		Vector3 vector32 = new Vector3();
		Vector3 vector33 = new Vector3();
		Vector3 vector34 = new Vector3();
		float single = up.x * up.x + up.y * up.y + up.z * up.z;
		if (single < 1.401298E-45f)
		{
			return rotation;
		}
		float single1 = TransformHelpers.InvSqrt(single);
		up.x = up.x * single1;
		up.y = up.y * single1;
		up.z = up.z * single1;
		vector33.x = up.x;
		vector33.y = up.y;
		vector33.z = up.z;
		float single2 = 1f;
		float single3 = single2;
		vector34.x = single2;
		vector3.z = single3;
		float single4 = 0f;
		single3 = single4;
		vector34.y = single4;
		float single5 = single3;
		single3 = single5;
		vector34.z = single5;
		float single6 = single3;
		single3 = single6;
		vector3.x = single6;
		vector3.y = single3;
		vector3 = rotation * vector3;
		vector34 = rotation * vector34;
		float single7 = vector3.x * vector33.x + vector3.y * vector33.y + vector3.z * vector33.z;
		float single8 = vector34.x * vector33.x + vector34.y * vector33.y + vector34.z * vector33.z;
		if (single7 * single7 <= single8 * single8)
		{
			vector31.x = vector3.x;
			vector31.y = vector3.y;
			vector31.z = vector3.z;
		}
		else
		{
			vector31.x = vector33.x;
			vector31.y = vector33.y;
			vector31.z = vector33.z;
			vector32.x = vector34.x;
			vector32.y = vector34.y;
			vector32.z = vector34.z;
			vector3.x = -(vector31.y * vector32.z - vector31.z * vector32.y);
			vector3.y = -(vector31.z * vector32.x - vector31.x * vector32.z);
			vector3.z = -(vector31.x * vector32.y - vector31.y * vector32.x);
			float single9 = TransformHelpers.InvSqrt(vector3.x, vector3.y, vector3.z);
			vector31.x = single9 * vector3.x;
			vector31.y = single9 * vector3.y;
			vector31.z = single9 * vector3.z;
		}
		vector32.x = vector33.x;
		vector32.y = vector33.y;
		vector32.z = vector33.z;
		vector34.x = vector31.y * vector32.z - vector31.z * vector32.y;
		vector34.y = vector31.z * vector32.x - vector31.x * vector32.z;
		vector34.z = vector31.x * vector32.y - vector31.y * vector32.x;
		float single10 = TransformHelpers.InvSqrt(vector34.x, vector34.y, vector34.z);
		vector32.x = vector34.x * single10;
		vector32.y = vector34.y * single10;
		vector32.z = vector34.z * single10;
		vector31.x = vector33.x;
		vector31.y = vector33.y;
		vector31.z = vector33.z;
		vector3.x = vector31.y * vector32.z - vector31.z * vector32.y;
		vector3.y = vector31.z * vector32.x - vector31.x * vector32.z;
		vector3.z = vector31.x * vector32.y - vector31.y * vector32.x;
		if (vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z < 1.401298E-45f)
		{
			return rotation;
		}
		return Quaternion.LookRotation(vector3, up);
	}

	public static void SetLocalPositionX(this Transform transform, float x)
	{
		Vector3 vector3 = transform.localPosition;
		vector3.x = x;
		transform.localPosition = vector3;
	}

	public static void SetLocalPositionY(this Transform transform, float y)
	{
		Vector3 vector3 = transform.localPosition;
		vector3.y = y;
		transform.localPosition = vector3;
	}

	public static void SetLocalPositionZ(this Transform transform, float z)
	{
		Vector3 vector3 = transform.localPosition;
		vector3.z = z;
		transform.localPosition = vector3;
	}

	public static Vector3 TestBoxCorners(Vector3 origin, Quaternion rotation, Vector3 boxCenter, Vector3 boxSize, int layerMask = 1024, int iterations = 7)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		Vector3 vector32 = new Vector3();
		Vector3 vector33 = new Vector3();
		RaycastHit raycastHit;
		RaycastHit raycastHit1;
		RaycastHit raycastHit2;
		RaycastHit raycastHit3;
		boxSize.x = Mathf.Abs(boxSize.x) * 0.5f;
		boxSize.y = Mathf.Abs(boxSize.y) * 0.5f;
		boxSize.z = Mathf.Abs(boxSize.z) * 0.5f;
		float single = boxCenter.x - boxSize.x;
		float single1 = single;
		vector31.x = single;
		vector3.x = single1;
		float single2 = boxCenter.x + boxSize.x;
		single1 = single2;
		vector33.x = single2;
		vector32.x = single1;
		float single3 = boxCenter.z - boxSize.z;
		single1 = single3;
		vector33.z = single3;
		vector31.z = single1;
		float single4 = boxCenter.z + boxSize.z;
		single1 = single4;
		vector32.z = single4;
		vector3.z = single1;
		float single5 = boxCenter.y + boxSize.y;
		single1 = single5;
		vector33.y = single5;
		float single6 = single1;
		single1 = single6;
		vector31.y = single6;
		float single7 = single1;
		single1 = single7;
		vector32.y = single7;
		vector3.y = single1;
		vector3 = rotation * vector3;
		vector31 = rotation * vector31;
		vector32 = rotation * vector32;
		vector33 = rotation * vector33;
		float single8 = vector3.magnitude;
		float single9 = vector31.magnitude;
		float single10 = vector32.magnitude;
		float single11 = vector33.magnitude;
		float single12 = 1f / single8;
		float single13 = 1f / single9;
		float single14 = 1f / single10;
		float single15 = 1f / single11;
		Vector3 vector34 = vector3 * single12;
		Vector3 vector35 = vector31 * single13;
		Vector3 vector36 = vector32 * single14;
		Vector3 vector37 = vector33 * single15;
		Vector3 vector38 = Vector3.Lerp(Vector3.Lerp(vector3, vector33, 0.5f), Vector3.Lerp(vector32, vector31, 0.5f), 0.5f);
		int num = 0;
		while (num < iterations)
		{
			Vector3 vector39 = origin + vector3;
			Vector3 vector310 = origin + vector31;
			Vector3 vector311 = origin + vector32;
			Vector3 vector312 = origin + vector33;
			bool flag = Physics.Raycast(vector39, -vector34, out raycastHit, single8, layerMask);
			bool flag1 = Physics.Raycast(vector310, -vector35, out raycastHit1, single9, layerMask);
			bool flag2 = Physics.Raycast(vector311, -vector36, out raycastHit2, single10, layerMask);
			bool flag3 = Physics.Raycast(vector312, -vector37, out raycastHit3, single11, layerMask);
			if (flag || flag1 || flag2 || flag3)
			{
				Vector3 vector313 = (!flag ? vector3 : raycastHit.point - origin);
				Vector3 vector314 = (!flag1 ? vector31 : raycastHit1.point - origin);
				Vector3 vector315 = (!flag2 ? vector32 : raycastHit2.point - origin);
				Vector3 vector316 = (!flag3 ? vector33 : raycastHit3.point - origin);
				Vector3 vector317 = Vector3.Lerp(Vector3.Lerp(vector313, vector316, 0.5f), Vector3.Lerp(vector315, vector314, 0.5f), 0.5f);
				Vector3 vector318 = vector317 - vector38;
				vector318.y = 0f;
				origin = origin + (vector318 * 2.15f);
				num++;
			}
			else
			{
				break;
			}
		}
		return origin;
	}

	public static Quaternion UpRotation(Vector3 up)
	{
		Vector3 vector3;
		float single = Vector3.Dot(up, Vector3.forward);
		float single1 = Vector3.Dot(up, Vector3.right);
		vector3 = (single * single >= single1 * single1 ? Vector3.Cross(up, Vector3.right) : Vector3.Cross(up, Vector3.forward));
		return Quaternion.LookRotation(vector3, up);
	}
}