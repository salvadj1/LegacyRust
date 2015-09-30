using System;
using UnityEngine;

public class FitRequirements : ScriptableObject
{
	[SerializeField]
	private FitRequirements.Condition[] Conditions;

	[HideInInspector]
	[SerializeField]
	private string assetPreview;

	public FitRequirements()
	{
	}

	public bool Test(Matrix4x4 placePosition)
	{
		if (!object.ReferenceEquals(this.Conditions, null))
		{
			FitRequirements.Condition[] conditions = this.Conditions;
			for (int i = 0; i < (int)conditions.Length; i++)
			{
				if (!conditions[i].Check(ref placePosition))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool Test(Vector3 origin, Quaternion rotation, Vector3 scale)
	{
		return this.Test(Matrix4x4.TRS(origin, rotation, scale));
	}

	public bool Test(Vector3 origin, Quaternion rotation)
	{
		return this.Test(Matrix4x4.TRS(origin, rotation, Vector3.one));
	}

	[Serializable]
	public class Condition
	{
		[SerializeField]
		private FitRequirements.Instruction instruction;

		[SerializeField]
		private Color flt0;

		[SerializeField]
		private Color flt1;

		[SerializeField]
		private Color flt2;

		[SerializeField]
		private LayerMask mask;

		[SerializeField]
		private bool failPass;

		public Vector3 capEnd
		{
			get
			{
				return this.flt2;
			}
			set
			{
				this.flt2.r = value.x;
				this.flt2.g = value.y;
				this.flt2.b = value.z;
			}
		}

		public Vector3 capStart
		{
			get
			{
				return this.flt0;
			}
			set
			{
				this.flt0.r = value.x;
				this.flt0.g = value.y;
				this.flt0.b = value.z;
			}
		}

		public Vector3 center
		{
			get
			{
				return this.flt0;
			}
			set
			{
				this.flt0.r = value.x;
				this.flt0.g = value.y;
				this.flt0.b = value.z;
			}
		}

		public Vector3 direction
		{
			get
			{
				return this.flt1;
			}
			set
			{
				this.flt1.r = value.x;
				this.flt1.g = value.y;
				this.flt1.b = value.z;
			}
		}

		public float distance
		{
			get
			{
				return this.flt0.a;
			}
			set
			{
				this.flt0.a = value;
			}
		}

		public bool passOnFail
		{
			get
			{
				return this.failPass;
			}
			set
			{
				this.failPass = value;
			}
		}

		public float radius
		{
			get
			{
				return this.flt1.a;
			}
			set
			{
				this.flt1.a = value;
			}
		}

		public Condition()
		{
			this.flt0.a = 1f;
			this.flt1 = Vector3.up;
			this.flt1.a = 0.5f;
			this.flt2 = Vector3.up;
			this.mask = 536871936;
		}

		public bool Check(ref Matrix4x4 matrix)
		{
			bool flag;
			Vector3 vector3;
			float single;
			switch (this.instruction)
			{
				case FitRequirements.Instruction.Raycast:
				{
					Vector3 vector31 = matrix.MultiplyPoint3x4(this.center);
					Vector3 vector32 = matrix.MultiplyVector(this.direction);
					vector3 = vector32;
					Vector3 vector33 = matrix.MultiplyVector(vector3.normalized);
					flag = Physics.Raycast(vector31, vector32, vector33.magnitude * this.distance, this.mask);
					break;
				}
				case FitRequirements.Instruction.SphereCast:
				{
					Ray ray = new Ray(matrix.MultiplyPoint3x4(this.center), matrix.MultiplyVector(this.direction));
					single = matrix.MultiplyVector(ray.direction).magnitude;
					flag = Physics.SphereCast(ray, single * this.radius, single * this.distance, this.mask);
					break;
				}
				case FitRequirements.Instruction.CapsuleCast:
				{
					vector3 = matrix.MultiplyVector(this.direction);
					single = matrix.MultiplyVector(vector3.normalized).magnitude;
					flag = Physics.CapsuleCast(matrix.MultiplyPoint3x4(this.capStart), matrix.MultiplyPoint3x4(this.capEnd), single * this.radius, vector3, single * this.distance, this.mask);
					break;
				}
				case FitRequirements.Instruction.CheckCapsule:
				{
					Vector3 vector34 = matrix.MultiplyPoint3x4(this.capStart);
					Vector3 vector35 = matrix.MultiplyPoint3x4(this.capEnd);
					Vector3 vector36 = matrix.MultiplyVector(Vector3.one);
					Vector3 vector37 = matrix.MultiplyVector(vector36.normalized);
					flag = Physics.CheckCapsule(vector34, vector35, vector37.magnitude * this.radius, this.mask);
					break;
				}
				case FitRequirements.Instruction.CheckSphere:
				{
					Vector3 vector38 = matrix.MultiplyPoint3x4(this.center);
					Vector3 vector39 = matrix.MultiplyVector(Vector3.one);
					Vector3 vector310 = matrix.MultiplyVector(vector39.normalized);
					flag = Physics.CheckSphere(vector38, vector310.magnitude * this.radius, this.mask);
					break;
				}
				default:
				{
					return true;
				}
			}
			return flag != this.passOnFail;
		}

		public void DrawGizmo(ref Matrix4x4 matrix)
		{
			switch (this.instruction)
			{
				case FitRequirements.Instruction.Raycast:
				{
					Vector3 vector3 = matrix.MultiplyPoint3x4(this.center);
					Vector3 vector31 = matrix.MultiplyVector(this.direction).normalized;
					Vector3 vector32 = matrix.MultiplyVector(vector31);
					Gizmos.DrawLine(vector3, vector3 + (vector31 * (vector32.magnitude * this.distance)));
					break;
				}
				case FitRequirements.Instruction.SphereCast:
				{
					float? nullable = null;
					float? nullable1 = null;
					FitRequirements.Condition.GizmoCapsuleAxis(ref matrix, this.center, this.radius, this.distance, this.direction, nullable, nullable1);
					break;
				}
				case FitRequirements.Instruction.CapsuleCast:
				{
					float? nullable2 = null;
					float? nullable3 = null;
					FitRequirements.Condition.GizmoCapsuleAxis(ref matrix, this.capStart, this.radius, this.distance, this.direction, nullable2, nullable3);
					float? nullable4 = null;
					float? nullable5 = null;
					FitRequirements.Condition.GizmoCapsuleAxis(ref matrix, this.capEnd, this.radius, this.distance, this.direction, nullable4, nullable5);
					FitRequirements.Condition.GizmoCapsulePoles(ref matrix, this.capStart, this.radius, this.capEnd);
					break;
				}
				case FitRequirements.Instruction.CheckCapsule:
				{
					FitRequirements.Condition.GizmoCapsulePoles(ref matrix, this.capStart, this.radius, this.capEnd);
					break;
				}
				case FitRequirements.Instruction.CheckSphere:
				{
					Vector3 vector33 = matrix.MultiplyPoint3x4(this.center);
					Vector3 vector34 = matrix.MultiplyVector(Vector3.one);
					Vector3 vector35 = matrix.MultiplyVector(vector34.normalized);
					Gizmos.DrawSphere(vector33, vector35.magnitude * this.radius);
					break;
				}
			}
		}

		private static void GizmoCapsuleAxis(ref Matrix4x4 matrix, Vector3 start, float radius, float distance, Vector3 direction, float? unitValueRadius = null, float? unitValueHeight = null)
		{
			float value;
			float single;
			Vector3 vector3 = matrix.MultiplyPoint3x4(start);
			Vector3 vector31 = matrix.MultiplyVector(direction).normalized;
			float? nullable = null;
			if (!unitValueRadius.HasValue)
			{
				Vector3 vector32 = matrix.MultiplyVector(vector31);
				float? nullable1 = new float?(vector32.magnitude);
				nullable = nullable1;
				value = nullable1.Value;
			}
			else
			{
				value = unitValueRadius.Value;
			}
			float single1 = value;
			if (!unitValueHeight.HasValue)
			{
				single = (!nullable.HasValue ? matrix.MultiplyVector(vector31).magnitude : nullable.Value);
			}
			else
			{
				single = unitValueHeight.Value;
			}
			float single2 = single;
			Matrix4x4 matrix4x4 = Gizmos.matrix;
			Gizmos.matrix = matrix4x4 * Matrix4x4.TRS(vector3, Quaternion.LookRotation(vector31, matrix.MultiplyVector(Vector3.up)), Vector3.one);
			radius = single1 * radius;
			float single3 = single2 * (distance + radius * 2f);
			Gizmos2.DrawWireCapsule(new Vector3(0f, 0f, single3 * 0.5f - radius), radius, single3, 2);
			Gizmos.matrix = matrix4x4;
		}

		private static void GizmoCapsulePoles(ref Matrix4x4 matrix, Vector3 start, float radius, Vector3 end)
		{
			Vector3 vector3 = (end - start).normalized;
			start = matrix.MultiplyPoint3x4(start);
			end = matrix.MultiplyPoint3x4(end);
			Vector3 vector31 = matrix.MultiplyVector(vector3);
			radius = radius * vector31.magnitude;
			vector3 = (end - start).normalized;
			start = start - (vector3 * radius);
			end = end + (vector3 * radius);
			vector3 = end - start;
			Matrix4x4 matrix4x4 = Gizmos.matrix;
			Gizmos.matrix = matrix4x4 * Matrix4x4.TRS(start, Quaternion.LookRotation(vector3, matrix.MultiplyVector(Vector3.up)), Vector3.one);
			float single = (end - start).magnitude;
			Gizmos2.DrawWireCapsule(new Vector3(0f, 0f, single * 0.5f), radius, single, 2);
			Gizmos.matrix = matrix4x4;
		}
	}

	public enum Instruction
	{
		Raycast,
		SphereCast,
		CapsuleCast,
		CheckCapsule,
		CheckSphere
	}
}