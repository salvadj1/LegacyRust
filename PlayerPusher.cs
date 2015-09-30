using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerPusher : MonoBehaviour
{
	[NonSerialized]
	private Rigidbody _rigidbody;

	[NonSerialized]
	private bool _gotRigidbody;

	[NonSerialized]
	private HashSet<CCMotor> activeMotors;

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

	public PlayerPusher()
	{
	}

	private bool AddMotor(CCMotor motor)
	{
		if (this.activeMotors == null)
		{
			this.activeMotors = new HashSet<CCMotor>();
			this.activeMotors.Add(motor);
			return true;
		}
		if (this.activeMotors.Add(motor))
		{
			return true;
		}
		Debug.LogWarning("Already added motor?", this);
		return false;
	}

	private bool ContainsMotor(CCMotor motor)
	{
		return (this.activeMotors == null ? false : this.activeMotors.Contains(motor));
	}

	private static bool GetCCMotor(Collision collision, out CCMotor ccmotor)
	{
		GameObject gameObject = collision.gameObject;
		if (gameObject.layer != 16)
		{
			ccmotor = null;
			return false;
		}
		ccmotor = gameObject.GetComponent<CCMotor>();
		return ccmotor;
	}

	private void OnCollisionEnter(Collision collision)
	{
		CCMotor cCMotor;
		if (PlayerPusher.GetCCMotor(collision, out cCMotor) && this.AddMotor(cCMotor))
		{
			try
			{
				cCMotor.OnPushEnter(this.rigidbody, base.collider, collision);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		CCMotor cCMotor;
		if (PlayerPusher.GetCCMotor(collision, out cCMotor) && this.RemoveMotor(cCMotor))
		{
			try
			{
				cCMotor.OnPushExit(this.rigidbody, base.collider, collision);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		CCMotor cCMotor;
		if (PlayerPusher.GetCCMotor(collision, out cCMotor) && this.ContainsMotor(cCMotor))
		{
			try
			{
				cCMotor.OnPushStay(this.rigidbody, base.collider, collision);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
	}

	private bool RemoveMotor(CCMotor motor)
	{
		if (this.activeMotors == null || !this.activeMotors.Remove(motor))
		{
			return false;
		}
		if (this.activeMotors.Count == 0)
		{
			this.activeMotors = null;
		}
		return true;
	}
}