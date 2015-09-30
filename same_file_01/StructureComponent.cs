using Facepunch;
using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

[NGCAutoAddScript]
[RequireComponent(typeof(TakeDamage))]
public class StructureComponent : IDMain, IServerSaveable
{
	public GameObject deathEffect;

	public StructureMaster _master;

	protected float oldHealth;

	[NonSerialized]
	private bool addedDestroyCallback;

	public float Width = 5f;

	public float Height = 1f;

	public StructureMaster.StructureMaterialType _materialType;

	public StructureComponent.StructureComponentType type;

	[NonSerialized]
	private HealthDimmer healthDimmer;

	private static bool logFailures;

	static StructureComponent()
	{
	}

	public StructureComponent() : base(IDFlags.Unknown)
	{
	}

	public virtual bool CheckLocation(StructureMaster master, Vector3 placePos, Quaternion placeRot)
	{
		bool flag;
		bool flag1;
		bool flag2;
		bool flag3 = false;
		bool flag4 = false;
		Vector3 vector3 = master.transform.InverseTransformPoint(placePos);
		if (master.GetMaterialType() != StructureMaster.StructureMaterialType.UNSET && master.GetMaterialType() != this.GetMaterialType())
		{
			if (StructureComponent.logFailures)
			{
				Debug.Log(string.Concat("Not proper material type, master is :", master.GetMaterialType()));
			}
			return false;
		}
		StructureComponent componentFromPositionWorld = master.GetComponentFromPositionWorld(placePos);
		if (componentFromPositionWorld != null)
		{
			if (StructureComponent.logFailures)
			{
				Debug.Log("Occupied space", componentFromPositionWorld);
			}
			flag3 = true;
		}
		StructureComponent structureComponent = master.CompByLocal(vector3 - new Vector3(0f, StructureMaster.gridSpacingY, 0f));
		if (this.type != StructureComponent.StructureComponentType.Foundation && master.GetFoundationForPoint(placePos))
		{
			flag4 = true;
		}
		if (this.type == StructureComponent.StructureComponentType.Wall || this.type == StructureComponent.StructureComponentType.Doorway || this.type == StructureComponent.StructureComponentType.WindowWall)
		{
			if (flag3)
			{
				return false;
			}
			Vector3 vector31 = placePos + ((placeRot * -Vector3.right) * 2.5f);
			StructureComponent componentFromPositionWorld1 = master.GetComponentFromPositionWorld(vector31);
			Vector3 vector32 = placePos + ((placeRot * Vector3.right) * 2.5f);
			StructureComponent structureComponent1 = master.GetComponentFromPositionWorld(vector32);
			if (StructureComponent.logFailures)
			{
				Debug.DrawLine(vector31, vector32, Color.cyan);
			}
			if (!componentFromPositionWorld1 || !structureComponent1)
			{
				if (StructureComponent.logFailures)
				{
					if (!componentFromPositionWorld1)
					{
						Debug.Log("Did not find left");
					}
					if (!structureComponent1)
					{
						Debug.Log("Did not find right");
					}
				}
				return false;
			}
			if (componentFromPositionWorld1.type == StructureComponent.StructureComponentType.Pillar)
			{
				flag = true;
			}
			else
			{
				if (StructureComponent.logFailures)
				{
					Debug.Log("Left was not acceptable", componentFromPositionWorld1);
				}
				flag = false;
			}
			if (structureComponent1.type == StructureComponent.StructureComponentType.Pillar)
			{
				flag1 = true;
			}
			else
			{
				if (StructureComponent.logFailures)
				{
					Debug.Log("Right was not acceptable", structureComponent1);
				}
				flag1 = false;
			}
			return (!flag ? false : flag1);
		}
		if (this.type == StructureComponent.StructureComponentType.Foundation)
		{
			List<StructureMaster>.Enumerator enumerator = StructureMaster.AllStructuresWithBounds.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StructureMaster current = enumerator.Current;
					if (current != master)
					{
						if (!current.containedBounds.Intersects(new Bounds(placePos, new Vector3(5f, 5f, 4f))))
						{
							continue;
						}
						if (StructureComponent.logFailures)
						{
							Debug.Log("Too close to something");
						}
						flag2 = false;
						return flag2;
					}
				}
				bool flag5 = master.IsValidFoundationSpot(placePos);
				if (StructureComponent.logFailures)
				{
					Debug.Log(string.Concat(new object[] { "returning here : mastervalid:", flag5, "compinplace", componentFromPositionWorld }));
				}
				return (!flag5 ? false : !componentFromPositionWorld);
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return flag2;
		}
		if (this.type == StructureComponent.StructureComponentType.Ramp)
		{
			if (componentFromPositionWorld == null && (master.IsValidFoundationSpot(placePos) || structureComponent && (structureComponent.type == StructureComponent.StructureComponentType.Ceiling || structureComponent.type == StructureComponent.StructureComponentType.Foundation)))
			{
				return true;
			}
			return false;
		}
		if (this.type == StructureComponent.StructureComponentType.Pillar)
		{
			return ((!structureComponent || structureComponent.type != StructureComponent.StructureComponentType.Pillar) && !flag4 ? false : !flag3);
		}
		if (this.type != StructureComponent.StructureComponentType.Stairs && this.type != StructureComponent.StructureComponentType.Ceiling)
		{
			return false;
		}
		if (flag3)
		{
			return false;
		}
		Vector3[] vector3Array = new Vector3[] { new Vector3(-2.5f, 0f, -2.5f), new Vector3(2.5f, 0f, 2.5f), new Vector3(-2.5f, 0f, 2.5f), new Vector3(2.5f, 0f, -2.5f) };
		for (int i = 0; i < (int)vector3Array.Length; i++)
		{
			Vector3 vector33 = vector3Array[i];
			StructureComponent structureComponent2 = master.CompByLocal(vector3 + vector33);
			if (structureComponent2 == null || structureComponent2.type != StructureComponent.StructureComponentType.Pillar)
			{
				return false;
			}
		}
		return true;
	}

	private void cl_predestroy(NGCView view)
	{
		if (this._master)
		{
			this._master.CullComponent(this);
		}
	}

	[RPC]
	public void ClientHealthUpdate(float newHealth)
	{
		this.healthDimmer.UpdateHealthAmount(this, newHealth, false);
	}

	[RPC]
	public void ClientKilled()
	{
		if (this.deathEffect)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.deathEffect, base.transform.position, base.transform.rotation) as GameObject;
			UnityEngine.Object.Destroy(gameObject, 5f);
		}
	}

	public StructureMaster.StructureMaterialType GetMaterialType()
	{
		return this._materialType;
	}

	public bool IsPillar()
	{
		return this.type == StructureComponent.StructureComponentType.Pillar;
	}

	public bool IsType(StructureComponent.StructureComponentType checkType)
	{
		return this.type == checkType;
	}

	public bool IsWallType()
	{
		return (this.type == StructureComponent.StructureComponentType.Wall || this.type == StructureComponent.StructureComponentType.Doorway ? true : this.type == StructureComponent.StructureComponentType.WindowWall);
	}

	public void OnHurt(DamageEvent damage)
	{
	}

	protected internal virtual void OnOwnedByMasterStructure(StructureMaster master)
	{
		this._master = master;
		NGCView component = base.GetComponent<NGCView>();
		if (component && !this.addedDestroyCallback && component)
		{
			this.addedDestroyCallback = true;
			component.OnPreDestroy += new NGC.EventCallback(this.cl_predestroy);
		}
	}

	protected void OnPoolRetire()
	{
		this.oldHealth = 0f;
		this.addedDestroyCallback = false;
		this.healthDimmer.Reset();
	}

	public void OnRepair()
	{
	}

	[Obsolete("Do not call manually", true)]
	[RPC]
	protected void SMSet(uLink.NetworkViewID masterViewID)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(masterViewID);
		if (!networkView)
		{
			Debug.LogWarning("Couldnt find master view", this);
		}
		else
		{
			StructureMaster component = networkView.GetComponent<StructureMaster>();
			if (!component)
			{
				Debug.LogWarning("No Master On GO", networkView);
			}
			else
			{
				component.AppendStructureComponent(this);
			}
		}
	}

	public void Touched()
	{
		this._master.Touched();
	}

	[Serializable]
	public enum StructureComponentType
	{
		Pillar,
		Wall,
		Doorway,
		Ceiling,
		Stairs,
		Foundation,
		WindowWall,
		Ramp,
		Last
	}
}