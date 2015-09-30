using System;
using UnityEngine;

[Serializable]
public class BobAntiOutput
{
	public BobAntiOutputAxes positionalAxes;

	public Vector3 positional;

	public BobAntiOutputAxes rotationalAxes;

	public Vector3 rotational;

	private bool wasAdded;

	private Vector3 lastPos;

	private Vector3 lastRot;

	public BobAntiOutput()
	{
	}

	public void Add(Transform transform, ref Vector3 lp, ref Vector3 lr)
	{
		if (!this.wasAdded)
		{
			this.lastPos = Vector3.Scale(BobAntiOutput.GetVector3(ref lp, this.positionalAxes), this.positional);
			transform.localPosition = this.lastPos;
			this.lastRot = Vector3.Scale(BobAntiOutput.GetVector3(ref lr, this.rotationalAxes), this.rotational);
			transform.localEulerAngles = this.lastRot;
			this.wasAdded = true;
		}
	}

	private static Vector3 GetVector3(ref Vector3 v, BobAntiOutputAxes axes)
	{
		Vector3 vector3 = new Vector3();
		switch ((int)axes & 3)
		{
			case 1:
			{
				vector3.x = v.x;
				break;
			}
			case 2:
			{
				vector3.x = v.y;
				break;
			}
			case 3:
			{
				vector3.x = v.z;
				break;
			}
			default:
			{
				goto case 1;
			}
		}
		switch (((int)axes & 12) >> 2)
		{
			case 1:
			{
				vector3.y = v.x;
				break;
			}
			case 2:
			{
				vector3.y = v.y;
				break;
			}
			case 3:
			{
				vector3.y = v.z;
				break;
			}
			default:
			{
				goto case 2;
			}
		}
		switch (((int)axes & 48) >> 4)
		{
			case 1:
			{
				vector3.z = v.x;
				break;
			}
			case 2:
			{
				vector3.z = v.y;
				break;
			}
			case 3:
			{
				vector3.z = v.z;
				break;
			}
			default:
			{
				goto case 3;
			}
		}
		return vector3;
	}

	public Vector3 Positional(Vector3 v)
	{
		return Vector3.Scale(BobAntiOutput.GetVector3(ref v, this.positionalAxes), this.positional);
	}

	public void Reset()
	{
		this.wasAdded = false;
	}

	public Vector3 Rotational(Vector3 v)
	{
		return Vector3.Scale(BobAntiOutput.GetVector3(ref v, this.rotationalAxes), this.rotational);
	}

	public void Subtract(Transform transform)
	{
		if (this.wasAdded)
		{
			Transform transforms = transform;
			transforms.localPosition = transforms.localPosition - this.lastPos;
			Transform transforms1 = transform;
			transforms1.localEulerAngles = transforms1.localEulerAngles - this.lastRot;
			this.wasAdded = false;
		}
	}
}