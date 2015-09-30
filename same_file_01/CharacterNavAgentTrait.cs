using System;
using UnityEngine;

public class CharacterNavAgentTrait : CharacterTrait
{
	[SerializeField]
	private float _radius = 0.5f;

	[SerializeField]
	private float _speed = 3f;

	[SerializeField]
	private float _acceleration = 8f;

	[SerializeField]
	private float _angularSpeed = 120f;

	[SerializeField]
	private float _stoppingDistance = 2f;

	[SerializeField]
	private bool _autoTraverseOffMeshLink = true;

	[SerializeField]
	private bool _autoBraking = true;

	[SerializeField]
	private bool _autoRepath = true;

	[SerializeField]
	private float _height = 2f;

	[SerializeField]
	private float _baseOffset;

	[SerializeField]
	private ObstacleAvoidanceType _obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;

	[SerializeField]
	private int _avoidancePriority = 50;

	[SerializeField]
	private int _walkableMask = -1;

	public float acceleration
	{
		get
		{
			return this._acceleration;
		}
	}

	public float angularSpeed
	{
		get
		{
			return this._angularSpeed;
		}
	}

	public bool autoBraking
	{
		get
		{
			return this._autoBraking;
		}
	}

	public bool autoRepath
	{
		get
		{
			return this._autoRepath;
		}
	}

	public bool autoTraverseOffMeshLink
	{
		get
		{
			return this._autoTraverseOffMeshLink;
		}
	}

	public int avoidancePriority
	{
		get
		{
			return this._avoidancePriority;
		}
	}

	public float baseOffset
	{
		get
		{
			return this._baseOffset;
		}
	}

	public float height
	{
		get
		{
			return this._height;
		}
	}

	public ObstacleAvoidanceType obstacleAvoidanceType
	{
		get
		{
			return this._obstacleAvoidanceType;
		}
	}

	public float radius
	{
		get
		{
			return this._radius;
		}
	}

	public float speed
	{
		get
		{
			return this._speed;
		}
	}

	public float stoppingDistance
	{
		get
		{
			return this._stoppingDistance;
		}
	}

	public int walkableMaks
	{
		get
		{
			return this._walkableMask;
		}
	}

	public CharacterNavAgentTrait()
	{
	}

	public void CopyTo(NavMeshAgent agent)
	{
		agent.radius = this._radius;
		agent.speed = this._speed;
		agent.acceleration = this._acceleration;
		agent.angularSpeed = this._angularSpeed;
		agent.stoppingDistance = this._stoppingDistance;
		agent.autoTraverseOffMeshLink = this._autoTraverseOffMeshLink;
		agent.autoBraking = this._autoBraking;
		agent.autoRepath = this._autoRepath;
		agent.height = this._height;
		agent.baseOffset = this._baseOffset;
		agent.obstacleAvoidanceType = this._obstacleAvoidanceType;
		agent.avoidancePriority = this._avoidancePriority;
		agent.walkableMask = this._walkableMask;
	}
}