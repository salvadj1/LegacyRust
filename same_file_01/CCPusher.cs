using Facepunch.Geometry;
using System;
using UnityEngine;

public class CCPusher : MonoBehaviour
{
	[SerializeField]
	private ShapeDefinition shape;

	[SerializeField]
	private Vector3 pushPoint0 = Vector3.forward;

	[SerializeField]
	private Vector3 pushDir0 = Vector3.back;

	[SerializeField]
	private Vector3 pushPoint1 = Vector3.back;

	[SerializeField]
	private Vector3 pushDir1 = Vector3.forward;

	[SerializeField]
	private float pushSpeed = 3f;

	public CCPusher()
	{
	}

	private static void DrawPushPlane(Matrix4x4 trs, Vector3 point, Vector3 dir)
	{
		point = trs.MultiplyPoint3x4(point);
		dir = trs.MultiplyVector(dir);
		Vector3 vector3 = point + (dir.normalized * 0.1f);
		Gizmos.DrawLine(point, vector3);
		Matrix4x4 matrix4x4 = Gizmos.matrix;
		Gizmos.matrix = matrix4x4 * Matrix4x4.TRS(point, Quaternion.LookRotation(dir), Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 1f, 0.0001f));
		Gizmos.matrix = matrix4x4;
	}

	private void OnDrawGizmosSelected()
	{
		Collider collider = base.collider;
		if (collider)
		{
			Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.8f);
			Shape shape = this.shape;
			Shape shape1 = shape.Transform(collider.ColliderToWorld());
			shape1.Gizmo();
			Matrix4x4 matrix4x4 = base.transform.localToWorldMatrix;
			Gizmos.color = new Color(0.9f, 0.5f, 1f, 0.8f);
			CCPusher.DrawPushPlane(matrix4x4, this.pushPoint0, this.pushDir0);
			Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.8f);
			CCPusher.DrawPushPlane(matrix4x4, this.pushPoint1, this.pushDir1);
		}
	}

	public bool Push(Facepunch.Geometry.Sphere Sphere, ref Vector3 Velocity)
	{
		Vector vector;
		if (!this.shape.Shape.Intersects(Sphere))
		{
			return false;
		}
		Facepunch.Geometry.Plane plane = Facepunch.Geometry.Plane.DirectionPoint(this.pushDir0, this.pushPoint0);
		Facepunch.Geometry.Plane plane1 = Facepunch.Geometry.Plane.DirectionPoint(this.pushDir1, this.pushPoint1);
		vector = (plane.DistanceTo(Sphere.Center + ((Normal)plane.Direction * Sphere.Radius)) <= plane1.DistanceTo(Sphere.Center + ((Normal)plane1.Direction * Sphere.Radius)) ? (Normal)plane1.Direction * (this.pushSpeed * Time.deltaTime) : (Normal)plane.Direction * (this.pushSpeed * Time.deltaTime));
		Velocity.x = Velocity.x + vector.x;
		Velocity.y = Velocity.y + vector.y;
		Velocity.z = Velocity.z + vector.z;
		return true;
	}

	private void Reset()
	{
		Shape shape;
		if (this.shape == null)
		{
			this.shape = new ShapeDefinition();
		}
		if (base.collider && base.collider.GetGeometricShapeLocal(out shape))
		{
			this.shape.Shape = shape;
		}
	}
}