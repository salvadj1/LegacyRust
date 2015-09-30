using System;
using UnityEngine;

public class TransformLerpTest : MonoBehaviour
{
	public Transform a;

	public Transform b;

	public float t;

	public float angleXY;

	public float angleYZ;

	public float angleZX;

	public bool cap;

	public bool inverse0;

	public bool transpose;

	public bool inverse1;

	[SerializeField]
	private TransformLerpTest.SlerpMode mode;

	private bool ready
	{
		get
		{
			bool flag;
			bool flag1;
			switch (this.mode)
			{
				case TransformLerpTest.SlerpMode.TransformSlerp:
				case TransformLerpTest.SlerpMode.TransformLerp:
				{
					if (!this.a)
					{
						flag = false;
					}
					else
					{
						flag = this.b;
					}
					return flag;
				}
				case TransformLerpTest.SlerpMode.WorldToCameraSlerp:
				case TransformLerpTest.SlerpMode.WorldToCameraLerp:
				case TransformLerpTest.SlerpMode.CameraToWorldSlerp:
				case TransformLerpTest.SlerpMode.CameraToWorldLerp:
				{
					if (!this.a || !this.b || !this.a.camera)
					{
						flag1 = false;
					}
					else
					{
						flag1 = this.b.camera;
					}
					return flag1;
				}
				default:
				{
					if (!this.a)
					{
						flag = false;
					}
					else
					{
						flag = this.b;
					}
					return flag;
				}
			}
		}
	}

	public TransformLerpTest()
	{
	}

	private static void DrawAxes(Matrix4x4 m, float alpha)
	{
		Vector3 vector3 = m.MultiplyPoint(Vector3.zero);
		Gizmos.color = new Color(1f, 1f, 1f, alpha);
		Gizmos.DrawSphere(vector3, 0.01f);
		Gizmos.color = new Color(1f, 0f, 0f, alpha);
		Gizmos.DrawLine(vector3, m.MultiplyPoint(Vector3.right));
		Gizmos.color = new Color(0f, 1f, 0f, alpha);
		Gizmos.DrawLine(vector3, m.MultiplyPoint(Vector3.up));
		Gizmos.color = new Color(0f, 0f, 1f, alpha);
		Gizmos.DrawLine(vector3, m.MultiplyPoint(Vector3.forward));
	}

	private Matrix4x4 GetMatrix(Transform a)
	{
		switch (this.mode)
		{
			case TransformLerpTest.SlerpMode.TransformSlerp:
			case TransformLerpTest.SlerpMode.TransformLerp:
			{
				if (!a.camera)
				{
					return a.localToWorldMatrix;
				}
				return a.camera.worldToCameraMatrix * a.localToWorldMatrix;
			}
			case TransformLerpTest.SlerpMode.WorldToCameraSlerp:
			case TransformLerpTest.SlerpMode.WorldToCameraLerp:
			case TransformLerpTest.SlerpMode.WorldToCameraSlerp2:
			{
				return a.camera.worldToCameraMatrix;
			}
			case TransformLerpTest.SlerpMode.CameraToWorldSlerp:
			case TransformLerpTest.SlerpMode.CameraToWorldLerp:
			{
				return a.camera.cameraToWorldMatrix;
			}
			default:
			{
				if (!a.camera)
				{
					return a.localToWorldMatrix;
				}
				return a.camera.worldToCameraMatrix * a.localToWorldMatrix;
			}
		}
	}

	private Matrix4x4 Interp(float t, Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 camera;
		switch (this.mode)
		{
			case TransformLerpTest.SlerpMode.TransformSlerp:
			case TransformLerpTest.SlerpMode.WorldToCameraSlerp:
			case TransformLerpTest.SlerpMode.CameraToWorldSlerp:
			{
				camera = TransitionFunctions.Slerp(t, a, b);
				break;
			}
			case TransformLerpTest.SlerpMode.TransformLerp:
			case TransformLerpTest.SlerpMode.WorldToCameraLerp:
			case TransformLerpTest.SlerpMode.CameraToWorldLerp:
			{
				camera = TransitionFunctions.Linear(t, a, b);
				break;
			}
			case TransformLerpTest.SlerpMode.WorldToCameraSlerp2:
			{
				camera = TransitionFunctions.SlerpWorldToCamera(t, a, b);
				break;
			}
			default:
			{
				goto case TransformLerpTest.SlerpMode.CameraToWorldSlerp;
			}
		}
		if (!this.inverse0)
		{
			if (!this.transpose)
			{
				if (!this.inverse1)
				{
					return camera;
				}
				return camera.inverse;
			}
			if (!this.inverse1)
			{
				return camera.transpose;
			}
			return camera.transpose.inverse;
		}
		if (!this.transpose)
		{
			if (!this.inverse1)
			{
				return camera.inverse;
			}
			return camera.inverse.inverse;
		}
		if (!this.inverse1)
		{
			return camera.inverse.transpose;
		}
		return camera.inverse.transpose.inverse;
	}

	private void OnDrawGizmos()
	{
		if (this.ready)
		{
			Matrix4x4 matrix = this.GetMatrix(this.a);
			Matrix4x4 matrix4x4 = this.GetMatrix(this.b);
			float single = (!this.cap ? this.t : Mathf.Clamp01(this.t));
			Matrix4x4 matrix4x41 = this.Interp(0f, matrix, matrix4x4);
			TransformLerpTest.DrawAxes(matrix4x41, 0.5f);
			for (int i = 1; i <= 32; i++)
			{
				Matrix4x4 matrix4x42 = this.Interp((float)i / 32f, matrix, matrix4x4);
				Gizmos.color = Color.white * 0.5f;
				Gizmos.DrawLine(matrix4x41.MultiplyPoint(Vector3.zero), matrix4x42.MultiplyPoint(Vector3.zero));
				Gizmos.color = Color.red * 0.5f;
				Gizmos.DrawLine(matrix4x41.MultiplyPoint(Vector3.right), matrix4x42.MultiplyPoint(Vector3.right));
				Gizmos.color = Color.green * 0.5f;
				Gizmos.DrawLine(matrix4x41.MultiplyPoint(Vector3.up), matrix4x42.MultiplyPoint(Vector3.up));
				Gizmos.color = Color.blue * 0.5f;
				Gizmos.DrawLine(matrix4x41.MultiplyPoint(Vector3.forward), matrix4x42.MultiplyPoint(Vector3.forward));
				matrix4x41 = matrix4x42;
			}
			TransformLerpTest.DrawAxes(matrix4x41, 0.5f);
			matrix4x41 = this.Interp(single, matrix, matrix4x4);
			TransformLerpTest.DrawAxes(matrix4x41, 1f);
			this.angleXY = Vector3.Angle(matrix4x41.MultiplyVector(Vector3.right), matrix4x41.MultiplyVector(Vector3.up));
			this.angleYZ = Vector3.Angle(matrix4x41.MultiplyVector(Vector3.up), matrix4x41.MultiplyVector(Vector3.forward));
			this.angleZX = Vector3.Angle(matrix4x41.MultiplyVector(Vector3.forward), matrix4x41.MultiplyVector(Vector3.right));
		}
	}

	private enum SlerpMode
	{
		TransformSlerp,
		TransformLerp,
		WorldToCameraSlerp,
		WorldToCameraLerp,
		CameraToWorldSlerp,
		CameraToWorldLerp,
		WorldToCameraSlerp2
	}
}