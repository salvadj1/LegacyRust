using System;
using UnityEngine;

[NGCAutoAddScript]
public class MovingDoor : BasicDoor
{
	private const float kFixedTimeElapsedMinimum = 1.5f;

	[SerializeField]
	protected Vector3 closedPositionPivot;

	[SerializeField]
	protected Vector3 openMovement = Vector3.up;

	[SerializeField]
	protected Vector3 rotationAxis = Vector3.up;

	[SerializeField]
	protected Vector3 rotationCross = Vector3.right;

	[SerializeField]
	protected float degrees;

	[SerializeField]
	protected bool rotationFirst;

	[SerializeField]
	protected bool smooth;

	[SerializeField]
	protected bool movementABS;

	[SerializeField]
	protected bool rotationABS;

	[SerializeField]
	protected bool slerpMovementByDegrees;

	[NonSerialized]
	private Vector3 lastLocalPosition;

	[NonSerialized]
	private Quaternion lastLocalRotation;

	[NonSerialized]
	private bool onceMoved;

	[NonSerialized]
	private int timesBoundKinematic;

	[NonSerialized]
	private float kinematicFrameStart;

	[NonSerialized]
	protected Rigidbody _rigidbody;

	[NonSerialized]
	protected bool _gotRigidbody;

	private Quaternion baseRot;

	public new Rigidbody rigidbody
	{
		get
		{
			if (!this._gotRigidbody)
			{
				this._rigidbody = base.rigidbody;
				this._gotRigidbody = true;
			}
			return this._rigidbody;
		}
	}

	public MovingDoor()
	{
	}

	private static void DrawOpenGizmo(Vector3 closedPositionPivot, Vector3 rotationCross, Vector3 rotationAxis, float degrees, Vector3 openMovement, bool movementABS, bool rotationABS, bool rotationFirst, bool reversed)
	{
		Vector3 vector3;
		Color color = Gizmos.color;
		Vector3 vector31 = closedPositionPivot;
		Vector3 vector32 = vector31 + rotationCross;
		bool flag = !Mathf.Approximately(degrees, 0f);
		bool flag1 = !Mathf.Approximately(openMovement.magnitude, 0f);
		Quaternion quaternion = Quaternion.identity;
		for (int i = 1; i < 21; i++)
		{
			float single = (float)i / 20f;
			Quaternion quaternion1 = Quaternion.AngleAxis(degrees * (!reversed || rotationABS ? single : -single), rotationAxis);
			vector3 = (!rotationFirst ? closedPositionPivot + (openMovement * (!reversed || movementABS ? single : -single)) : closedPositionPivot + (quaternion1 * openMovement * (!reversed || movementABS ? single : -single)));
			if (flag1)
			{
				Gizmos.color = Color.Lerp(Color.red, (!reversed ? Color.green : Color.yellow), single);
				Gizmos.DrawLine(vector31, vector3);
			}
			vector31 = vector3;
			Vector3 vector33 = vector3 + (quaternion1 * rotationCross);
			if (flag)
			{
				Gizmos.color = Color.Lerp(Color.blue, (!reversed ? Color.cyan : Color.magenta), single);
				Gizmos.DrawLine(vector32, vector33);
			}
			vector32 = vector33;
			quaternion = quaternion1;
		}
		if (flag)
		{
			Vector3 vector34 = closedPositionPivot + (!rotationFirst ? openMovement : quaternion * openMovement);
			Gizmos.color = new Color(1f, (!reversed ? 0f : 1f), 0f, 0.5f);
			Gizmos.DrawLine(Vector3.Lerp(vector34, vector32, 0.5f), vector32);
		}
		Gizmos.color = color;
	}

	protected override BasicDoor.IdealSide IdealSideForPoint(Vector3 worldPoint)
	{
		float single = Vector3.Dot(base.transform.InverseTransformPoint(worldPoint), Vector3.Cross(this.rotationCross, this.rotationAxis));
		if (float.IsInfinity(single) || float.IsNaN(single) || Mathf.Approximately(0f, single))
		{
			return BasicDoor.IdealSide.Unknown;
		}
		return (single <= 0f ? BasicDoor.IdealSide.Reverse | BasicDoor.IdealSide.Forward : BasicDoor.IdealSide.Forward);
	}

	protected override void OnDoorFraction(double fractionOpen)
	{
		this.UpdateMovement(fractionOpen);
	}

	protected void UpdateMovement(double openFraction, out Vector3 localPosition, out Quaternion localRotation)
	{
		double num;
		Quaternion quaternion;
		if (openFraction == 0)
		{
			localPosition = this.originalLocalPosition;
			localRotation = this.originalLocalRotation;
			return;
		}
		if (this.smooth)
		{
			openFraction = (double)((openFraction >= 0 ? Mathf.SmoothStep(0f, 1f, (float)openFraction) : Mathf.SmoothStep(0f, -1f, (float)(-openFraction))));
		}
		if (this.slerpMovementByDegrees && this.degrees != 0f && openFraction != 0 && openFraction != 1)
		{
			double num1 = (double)this.degrees * 3.14159265358979 / 180;
			double num2 = num1;
			double num3 = Math.Sin(num1);
			double num4 = num3;
			if (num3 == 0)
			{
				goto Label1;
			}
			num = Math.Sin(openFraction * num2) / num4;
			goto Label0;
		}
	Label1:
		num = openFraction;
	Label0:
		quaternion = (openFraction != 0 ? Quaternion.AngleAxis((float)((double)this.degrees * (!this.rotationABS ? openFraction : Math.Abs(openFraction))), this.rotationAxis) : Quaternion.identity);
		Quaternion quaternion1 = quaternion;
		Vector3 vector3 = this.openMovement * (float)((!this.movementABS ? num : Math.Abs(num)));
		Vector3 vector31 = (quaternion1 * -this.closedPositionPivot) + this.closedPositionPivot;
		if (!this.rotationFirst)
		{
			localPosition = this.originalLocalPosition + (this.originalLocalRotation * (vector31 + vector3));
		}
		else
		{
			localPosition = this.originalLocalPosition + (this.originalLocalRotation * (vector31 + (quaternion1 * vector3)));
		}
		localRotation = (openFraction != 0 ? this.originalLocalRotation * quaternion1 : this.originalLocalRotation);
	}

	protected void UpdateMovement(double openFraction)
	{
		Vector3 vector3;
		Quaternion quaternion;
		this.UpdateMovement(openFraction, out vector3, out quaternion);
		base.transform.localPosition = vector3;
		base.transform.localRotation = quaternion;
	}
}