using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGizmo : ScriptableObject
{
	[SerializeField]
	private Mesh _mesh;

	[SerializeField]
	private Material[] _materials;

	[SerializeField]
	private bool _castShadows;

	[SerializeField]
	private bool _receiveShadows;

	[SerializeField]
	private int _layer;

	[SerializeField]
	private Color _good = Color.green;

	[SerializeField]
	private Color _bad = Color.red;

	[SerializeField]
	private float _minAlpha = 0.9f;

	[SerializeField]
	private float _maxAlpha = 1f;

	[SerializeField]
	private Vector3 alternateArrowDirection;

	private HashSet<GameGizmo.Instance> _instances;

	public Color badColor
	{
		get
		{
			return this._bad;
		}
	}

	public Color goodColor
	{
		get
		{
			return this._good;
		}
	}

	public float maxAlpha
	{
		get
		{
			return this._maxAlpha;
		}
	}

	public float minAlpha
	{
		get
		{
			return this._minAlpha;
		}
	}

	public GameGizmo()
	{
	}

	protected virtual GameGizmo.Instance ConstructInstance()
	{
		return new GameGizmo.Instance(this);
	}

	public bool Create<TInstance>(out TInstance instance)
	where TInstance : GameGizmo.Instance
	{
		GameGizmo.Instance instance1;
		if (this.CreateInstance(out instance1, typeof(TInstance)))
		{
			instance = (TInstance)instance1;
			return true;
		}
		instance = (TInstance)null;
		return false;
	}

	private bool CreateInstance(out GameGizmo.Instance instance, Type type)
	{
		bool flag;
		try
		{
			instance = this.ConstructInstance();
			if (!object.ReferenceEquals(instance, null))
			{
				if (this._instances == null)
				{
					this._instances = new HashSet<GameGizmo.Instance>();
				}
				this._instances.Add(instance);
				if (!type.IsAssignableFrom(instance.GetType()))
				{
					this.DestroyInstance(instance);
					throw new InvalidCastException();
				}
				return true;
			}
			else
			{
				flag = false;
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
			instance = null;
			flag = false;
		}
		return flag;
	}

	protected virtual void DeconstructInstance(GameGizmo.Instance instance)
	{
	}

	public bool Destroy<TInstance>(ref TInstance instance)
	where TInstance : GameGizmo.Instance
	{
		if (!this.DestroyInstance(instance))
		{
			return false;
		}
		instance = (TInstance)null;
		return true;
	}

	private bool DestroyInstance(GameGizmo.Instance instance)
	{
		if (object.ReferenceEquals(instance, null) || this._instances == null || !this._instances.Remove(instance))
		{
			return false;
		}
		try
		{
			instance.ClearResources();
			this.DeconstructInstance(instance);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
		}
		return true;
	}

	public class Instance
	{
		[NonSerialized]
		public readonly GameGizmo gameGizmo;

		[NonSerialized]
		public readonly MaterialPropertyBlock propertyBlock;

		[NonSerialized]
		public Vector3 localPosition;

		[NonSerialized]
		public Quaternion localRotation;

		[NonSerialized]
		public Vector3 localScale;

		[NonSerialized]
		public Matrix4x4? overrideMatrix;

		[NonSerialized]
		public MeshRenderer carrierRenderer;

		protected Matrix4x4? ultimateMatrix;

		protected bool hideMesh;

		private List<UnityEngine.Object> resources;

		private Transform _parent;

		protected bool castShadows
		{
			get
			{
				return this.gameGizmo._castShadows;
			}
		}

		protected int layer
		{
			get
			{
				return this.gameGizmo._layer;
			}
		}

		public Transform parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				if (value != this._parent)
				{
					if (!value)
					{
						this._parent = null;
					}
					else
					{
						this.localPosition = value.InverseTransformPoint(this.position);
						this.localRotation = Quaternion.Inverse(value.rotation) * this.rotation;
						this._parent = value;
					}
				}
			}
		}

		public Vector3 position
		{
			get
			{
				return (!this._parent ? this.localPosition : this._parent.TransformPoint(this.localPosition));
			}
			set
			{
				if (!this._parent)
				{
					this.localPosition = value;
				}
				else
				{
					this.localPosition = this._parent.InverseTransformPoint(value);
				}
			}
		}

		protected bool receiveShadows
		{
			get
			{
				return this.gameGizmo._receiveShadows;
			}
		}

		public Quaternion rotation
		{
			get
			{
				return (!this._parent ? this.localRotation : this.localRotation * this._parent.rotation);
			}
			set
			{
				if (!this._parent)
				{
					this.localRotation = value;
				}
				else
				{
					this.localRotation = Quaternion.Inverse(this._parent.rotation) * value;
				}
			}
		}

		protected internal Instance(GameGizmo gizmo)
		{
			this.localPosition = Vector3.zero;
			this.localRotation = Quaternion.identity;
			this.localScale = Vector3.one;
			this.gameGizmo = gizmo;
			this.propertyBlock = new MaterialPropertyBlock();
		}

		public void AddResourceToDelete(UnityEngine.Object resource)
		{
			if (resource)
			{
				List<UnityEngine.Object> objs = this.resources;
				if (objs == null)
				{
					List<UnityEngine.Object> objs1 = new List<UnityEngine.Object>();
					List<UnityEngine.Object> objs2 = objs1;
					this.resources = objs1;
					objs = objs2;
				}
				objs.Add(resource);
			}
		}

		internal void ClearResources()
		{
			List<UnityEngine.Object> objs = this.resources;
			if (objs != null)
			{
				this.resources = null;
				foreach (UnityEngine.Object obj in objs)
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		protected Matrix4x4 DefaultMatrix()
		{
			Matrix4x4 matrix4x4 = Matrix4x4.TRS(this.localPosition, this.localRotation, this.localScale);
			if (this._parent)
			{
				matrix4x4 = this._parent.localToWorldMatrix * matrix4x4;
			}
			return matrix4x4;
		}

		public void Render()
		{
			this.Render(false, null);
		}

		public void Render(Camera camera)
		{
			this.Render(camera, camera);
		}

		protected virtual void Render(bool useCamera, Camera camera)
		{
			Matrix4x4 value;
			if (this.hideMesh)
			{
				return;
			}
			Mesh mesh = this.gameGizmo._mesh;
			if (!mesh)
			{
				return;
			}
			Material[] materialArray = this.gameGizmo._materials;
			if (materialArray != null)
			{
				int length = (int)materialArray.Length;
				int num = length;
				if (length != 0)
				{
					Matrix4x4? nullable = this.ultimateMatrix;
					if (!nullable.HasValue)
					{
						Matrix4x4? nullable1 = this.overrideMatrix;
						value = (!nullable1.HasValue ? this.DefaultMatrix() : nullable1.Value);
					}
					else
					{
						value = nullable.Value;
					}
					Matrix4x4 matrix4x4 = value;
					if (this.gameGizmo.alternateArrowDirection != Vector3.zero)
					{
						matrix4x4 = matrix4x4 * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(this.gameGizmo.alternateArrowDirection), Vector3.one);
					}
					for (int i = 0; i < mesh.subMeshCount; i++)
					{
						Graphics.DrawMesh(mesh, matrix4x4, materialArray[i % num], this.gameGizmo._layer, camera, i, this.propertyBlock, this.gameGizmo._castShadows, this.gameGizmo._receiveShadows);
					}
					return;
				}
			}
		}
	}
}