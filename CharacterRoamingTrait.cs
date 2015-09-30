using System;
using UnityEngine;

public class CharacterRoamingTrait : CharacterTrait
{
	[SerializeField]
	private float _maxRoamDistance = 20f;

	[SerializeField]
	private float _minRoamDistance = 10f;

	[SerializeField]
	private float _minRoamAngle = -180f;

	[SerializeField]
	private float _maxRoamAngle = 180f;

	[SerializeField]
	private float _maxFleeDistance = 40f;

	[SerializeField]
	private float _minFleeDistance = 21f;

	[SerializeField]
	private float _roamRadius = 80f;

	[SerializeField]
	private bool _allowed = true;

	[SerializeField]
	private int _minIdleMilliseconds = 2000;

	[SerializeField]
	private int _maxIdleMilliseconds = 8000;

	[SerializeField]
	private int _retryFromFailureMilliseconds = 800;

	[SerializeField]
	private float _fleeSpeed = 9f;

	[SerializeField]
	private float _runSpeed = 6f;

	[SerializeField]
	private float _walkSpeed = 1.8f;

	public bool allowed
	{
		get
		{
			return this._allowed;
		}
	}

	public float fleeSpeed
	{
		get
		{
			return this._fleeSpeed;
		}
	}

	public float maxFleeDistance
	{
		get
		{
			return (!this._allowed ? 0f : this._maxFleeDistance);
		}
	}

	public int maxIdleMilliseconds
	{
		get
		{
			return this._maxIdleMilliseconds;
		}
	}

	public float maxIdleSeconds
	{
		get
		{
			return (float)((double)this._maxIdleMilliseconds / 1000);
		}
	}

	public float maxRoamAngle
	{
		get
		{
			return (!this._allowed ? 0f : this._maxRoamAngle);
		}
	}

	public float maxRoamDistance
	{
		get
		{
			return (!this._allowed ? 0f : this._maxRoamDistance);
		}
	}

	public float minFleeDistance
	{
		get
		{
			return (!this._allowed ? 0f : this._minFleeDistance);
		}
	}

	public int minIdleMilliseconds
	{
		get
		{
			return this._minIdleMilliseconds;
		}
	}

	public float minIdleSeconds
	{
		get
		{
			return (float)((double)this._minIdleMilliseconds / 1000);
		}
	}

	public float minRoamAngle
	{
		get
		{
			return (!this._allowed ? 0f : this._minRoamAngle);
		}
	}

	public float minRoamDistance
	{
		get
		{
			return (!this._allowed ? 0f : this._minRoamDistance);
		}
	}

	public float randomFleeDistance
	{
		get
		{
			float single;
			if (!this._allowed)
			{
				single = 0f;
			}
			else
			{
				single = (this._minFleeDistance != this._maxFleeDistance ? this._minFleeDistance + (this._maxFleeDistance - this._minFleeDistance) * UnityEngine.Random.@value : this._minFleeDistance);
			}
			return single;
		}
	}

	public Vector3 randomFleeVector
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.y = 0f;
			if (!this._allowed)
			{
				vector3.x = 0f;
				vector3.z = 0f;
			}
			else
			{
				float single = this.randomFleeDistance;
				float single1 = this.randomRoamAngle * 0.0174532924f;
				vector3.x = Mathf.Sin(single1) * single;
				vector3.z = Mathf.Cos(single1) * single;
			}
			return vector3;
		}
	}

	public int randomIdleMilliseconds
	{
		get
		{
			int num;
			if (this._minIdleMilliseconds != this._maxIdleMilliseconds)
			{
				num = (this._minIdleMilliseconds >= this._maxIdleMilliseconds ? UnityEngine.Random.Range(this._maxIdleMilliseconds, this._minIdleMilliseconds + 1) : UnityEngine.Random.Range(this._minIdleMilliseconds, this._maxIdleMilliseconds + 1));
			}
			else
			{
				num = this._minIdleMilliseconds;
			}
			return num;
		}
	}

	public float randomIdleSeconds
	{
		get
		{
			return (float)((double)this.randomIdleMilliseconds / 1000);
		}
	}

	public float randomRoamAngle
	{
		get
		{
			float single;
			if (!this._allowed)
			{
				single = 0f;
			}
			else
			{
				single = (this._maxRoamAngle != this._minRoamAngle ? this._minRoamAngle + (this._maxRoamAngle - this._minRoamAngle) * UnityEngine.Random.@value : this._minRoamAngle);
			}
			return single;
		}
	}

	public float randomRoamDistance
	{
		get
		{
			float single;
			if (!this._allowed)
			{
				single = 0f;
			}
			else
			{
				single = (this._minRoamDistance != this._maxRoamDistance ? this._minRoamDistance + (this._maxRoamDistance - this._minRoamDistance) * UnityEngine.Random.@value : this._minRoamDistance);
			}
			return single;
		}
	}

	public Vector3 randomRoamNormal
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.y = 0f;
			if (!this._allowed)
			{
				vector3.x = 0f;
				vector3.z = 0f;
			}
			else
			{
				float single = this.randomRoamAngle * 0.0174532924f;
				vector3.x = Mathf.Sin(single);
				vector3.z = Mathf.Cos(single);
			}
			return vector3;
		}
	}

	public Vector3 randomRoamVector
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.y = 0f;
			if (!this._allowed)
			{
				vector3.x = 0f;
				vector3.z = 0f;
			}
			else
			{
				float single = this.randomRoamDistance;
				float single1 = this.randomRoamAngle * 0.0174532924f;
				vector3.x = Mathf.Sin(single1) * single;
				vector3.z = Mathf.Cos(single1) * single;
			}
			return vector3;
		}
	}

	public int retryFromFailureMilliseconds
	{
		get
		{
			return this._retryFromFailureMilliseconds;
		}
	}

	public float retryFromFailureSeconds
	{
		get
		{
			return (float)((double)this._retryFromFailureMilliseconds / 1000);
		}
	}

	public float roamRadius
	{
		get
		{
			return this._roamRadius;
		}
	}

	public float runSpeed
	{
		get
		{
			return this._runSpeed;
		}
	}

	public float walkSpeed
	{
		get
		{
			return this._walkSpeed;
		}
	}

	public CharacterRoamingTrait()
	{
	}
}