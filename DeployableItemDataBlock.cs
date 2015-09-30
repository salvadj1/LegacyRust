using Facepunch.MeshBatch;
using System;
using uLink;
using UnityEngine;

public class DeployableItemDataBlock : HeldItemDataBlock
{
	public GameGizmo aimGizmo;

	[NonSerialized]
	private DeployableObject _deployableObject;

	[NonSerialized]
	private bool _loadedDeployableObject;

	public string DeployableObjectPrefabName;

	public Material overrideMat;

	public bool uprightOnly;

	public DeployableOrientationMode orientationMode;

	public bool CanStackOnDeployables = true;

	public float minCastRadius = 0.25f;

	public bool StructureOnly;

	public bool TerrainOnly;

	public float spacingRadius;

	public float placeRange = 8f;

	public bool requireHardpoint;

	public Hardpoint.hardpoint_type hardpointType;

	public bool checkPlacementZones;

	public bool forcePlaceable;

	public bool neverGrabCarrier;

	public DeployableItemDataBlock.CarrierSphereCastMode carrierSphereCastMode;

	public FitRequirements fitRequirements;

	public bool fitTestForcedUp;

	public DeployableObject ObjectToPlace
	{
		get
		{
			if (!this._loadedDeployableObject && Application.isPlaying)
			{
				NetCull.LoadPrefabScript<DeployableObject>(this.DeployableObjectPrefabName, out this._deployableObject);
				this._loadedDeployableObject = true;
			}
			return this._deployableObject;
		}
	}

	public DeployableItemDataBlock()
	{
	}

	public bool CheckPlacement(Ray ray, out Vector3 pos, out Quaternion rot, out TransCarrier carrier)
	{
		DeployableItemDataBlock.DeployPlaceResults deployPlaceResult;
		this.CheckPlacementResults(ray, out pos, out rot, out carrier, out deployPlaceResult);
		return deployPlaceResult.Valid();
	}

	public void CheckPlacementResults(Ray ray, out Vector3 pos, out Quaternion rot, out TransCarrier carrier, out DeployableItemDataBlock.DeployPlaceResults results)
	{
		results = new DeployableItemDataBlock.DeployPlaceResults();
		RaycastHit raycastHit;
		Vector3 vector3;
		Vector3 vector31;
		bool flag;
		MeshBatchInstance meshBatchInstance;
		Ray ray1;
		float single;
		RaycastHit raycastHit1;
		bool flag1;
		MeshBatchInstance meshBatchInstance1;
		Quaternion quaternion;
		bool flag2;
		float single1 = this.placeRange;
		IDMain dMain = null;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		DeployableObject deployableObject = null;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = this.minCastRadius >= 1.401298E-45f;
		bool flag9 = (!flag8 ? Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray, out raycastHit, single1, -472317957, out flag, out meshBatchInstance) : Facepunch.MeshBatch.MeshBatchPhysics.SphereCast(ray, this.minCastRadius, out raycastHit, single1, -472317957, out flag, out meshBatchInstance));
		Vector3 point = ray.GetPoint(single1);
		if (!flag9)
		{
			Vector3 vector32 = point;
			vector32.y = vector32.y + 0.5f;
			flag6 = Facepunch.MeshBatch.MeshBatchPhysics.Raycast(vector32, Vector3.down, out raycastHit, 5f, -472317957, out flag, out meshBatchInstance);
		}
		if (flag9 || flag6)
		{
			dMain = (!flag ? IDBase.GetMain(raycastHit.collider) : meshBatchInstance.idMain);
			flag5 = (dMain is StructureComponent ? true : dMain is StructureMaster);
			vector3 = raycastHit.point;
			vector31 = raycastHit.normal;
			if (flag5)
			{
				flag2 = false;
			}
			else
			{
				DeployableObject deployableObject1 = dMain as DeployableObject;
				deployableObject = deployableObject1;
				flag2 = deployableObject1;
			}
			flag3 = flag2;
			if (this.carrierSphereCastMode == DeployableItemDataBlock.CarrierSphereCastMode.Allowed || !flag9 || !flag8 || DeployableItemDataBlock.NonVariantSphereCast(ray, vector3))
			{
				carrier = ((!dMain ? raycastHit.collider.gameObject : dMain.gameObject)).GetComponent<TransCarrier>();
			}
			else
			{
				if (this.carrierSphereCastMode != DeployableItemDataBlock.CarrierSphereCastMode.AdjustedRay)
				{
					single = single1 + this.minCastRadius;
					ray1 = ray;
				}
				else
				{
					Vector3 vector33 = ray.origin;
					Vector3 vector34 = raycastHit.point - vector33;
					single = vector34.magnitude + this.minCastRadius * 2f;
					ray1 = new Ray(vector33, vector34);
					Debug.DrawLine(ray.origin, ray.GetPoint(single), Color.cyan);
				}
				bool flag10 = Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray1, out raycastHit1, single, -472317957, out flag1, out meshBatchInstance1);
				bool flag11 = flag10;
				if (!flag10)
				{
					Vector3 vector35 = vector3;
					vector35.y = vector35.y + 0.5f;
					flag11 = Facepunch.MeshBatch.MeshBatchPhysics.Raycast(vector35, Vector3.down, out raycastHit1, 5f, -472317957, out flag1, out meshBatchInstance1);
				}
				if (!flag11)
				{
					carrier = null;
				}
				else
				{
					IDMain dMain1 = (!flag1 ? IDBase.GetMain(raycastHit1.collider) : meshBatchInstance1.idMain);
					carrier = (!dMain1 ? raycastHit1.collider.GetComponent<TransCarrier>() : dMain1.GetLocal<TransCarrier>());
				}
			}
			flag4 = (raycastHit.collider is TerrainCollider ? true : raycastHit.collider.gameObject.layer == 10);
			flag7 = true;
		}
		else
		{
			vector3 = point;
			vector31 = Vector3.up;
			carrier = null;
		}
		bool flag12 = false;
		Hardpoint hardpointFromRay = null;
		if (this.hardpointType != Hardpoint.hardpoint_type.None)
		{
			hardpointFromRay = Hardpoint.GetHardpointFromRay(ray, this.hardpointType);
			if (hardpointFromRay)
			{
				flag12 = true;
				vector3 = hardpointFromRay.transform.position;
				vector31 = hardpointFromRay.transform.up;
				carrier = hardpointFromRay.GetMaster().GetTransCarrier();
				flag7 = true;
			}
		}
		bool flag13 = false;
		if (this.spacingRadius > 0f)
		{
			Collider[] colliderArray = Physics.OverlapSphere(vector3, this.spacingRadius);
			int num = 0;
			while (num < (int)colliderArray.Length)
			{
				Collider collider = colliderArray[num];
				GameObject gameObject = collider.gameObject;
				IDBase component = collider.gameObject.GetComponent<IDBase>();
				if (component != null)
				{
					gameObject = component.idMain.gameObject;
				}
				if (!gameObject.CompareTag(this.ObjectToPlace.gameObject.tag) || Vector3.Distance(vector3, gameObject.transform.position) >= this.spacingRadius)
				{
					num++;
				}
				else
				{
					flag13 = true;
					break;
				}
			}
		}
		bool flag14 = false;
		if (flag3 && !this.forcePlaceable && deployableObject.cantPlaceOn)
		{
			flag14 = true;
		}
		pos = vector3;
		if (this.orientationMode == DeployableOrientationMode.Default)
		{
			if (!this.uprightOnly)
			{
				this.orientationMode = DeployableOrientationMode.NormalUp;
			}
			else
			{
				this.orientationMode = DeployableOrientationMode.Upright;
			}
		}
		switch (this.orientationMode)
		{
			case DeployableOrientationMode.NormalUp:
			{
				quaternion = TransformHelpers.LookRotationForcedUp(ray.direction, vector31);
				break;
			}
			case DeployableOrientationMode.Upright:
			{
				quaternion = TransformHelpers.LookRotationForcedUp(ray.direction, Vector3.up);
				break;
			}
			case DeployableOrientationMode.NormalForward:
			{
				Vector3 vector36 = Vector3.Cross(ray.direction, Vector3.up);
				quaternion = TransformHelpers.LookRotationForcedUp(vector36, vector31);
				break;
			}
			case DeployableOrientationMode.HardpointPosRot:
			{
				quaternion = (!flag12 ? TransformHelpers.LookRotationForcedUp(ray.direction, Vector3.up) : hardpointFromRay.transform.rotation);
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
		}
		rot = quaternion * this.ObjectToPlace.transform.localRotation;
		bool flag15 = false;
		if (this.checkPlacementZones)
		{
			flag15 = NoPlacementZone.ValidPos(pos);
		}
		float single2 = Vector3.Angle(vector31, Vector3.up);
		results.falseFromDeployable = (this.CanStackOnDeployables || !flag3 ? flag14 : true);
		results.falseFromTerrian = (!this.TerrainOnly ? false : !flag4);
		results.falseFromClose = (this.spacingRadius <= 0f ? false : flag13);
		results.falseFromHardpoint = (!this.requireHardpoint ? false : !flag12);
		results.falseFromAngle = (this.requireHardpoint ? false : single2 >= this.ObjectToPlace.maxSlope);
		results.falseFromPlacementZone = (!this.checkPlacementZones ? false : !flag15);
		results.falseFromHittingNothing = !flag7;
		results.falseFromStructure = (!this.StructureOnly ? false : !flag5);
		results.falseFromFitRequirements = (this.fitRequirements == null ? false : !this.fitRequirements.Test(pos, (!this.fitTestForcedUp ? rot : TransformHelpers.LookRotationForcedUp(rot, Vector3.up)), this.ObjectToPlace.transform.localScale));
	}

	protected override IInventoryItem ConstructItem()
	{
		return new DeployableItemDataBlock.ITEM_TYPE(this);
	}

	public override void DoAction1(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
	{
	}

	private static bool NonVariantSphereCast(Ray r, Vector3 p)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = r.origin;
		Vector3 vector32 = r.direction;
		float single = vector32.x * p.x + vector32.y * p.y + vector32.z * p.z - (vector32.x * vector31.x + vector32.y * vector31.y + vector32.z * vector31.z);
		vector3.x = p.x - (vector32.x * single + vector31.x);
		vector3.y = p.y - (vector32.y * single + vector31.y);
		vector3.z = p.z - (vector32.z * single + vector31.z);
		return vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z < 0.001f;
	}

	public enum CarrierSphereCastMode
	{
		Allowed,
		AdjustedRay,
		InputRay
	}

	public struct DeployPlaceResults
	{
		public bool falseFromDeployable;

		public bool falseFromTerrian;

		public bool falseFromClose;

		public bool falseFromHardpoint;

		public bool falseFromAngle;

		public bool falseFromPlacementZone;

		public bool falseFromFitRequirements;

		public bool falseFromHittingNothing;

		public bool falseFromStructure;

		public void PrintResults()
		{
			if (!this.Valid())
			{
				string str = string.Format("Results ang:{0} dep:{1} ter:{2} close:{3} hardpoint:{4} npz:{5} fit:{6} nothin:{7} struct:{8}", new object[] { this.falseFromAngle, this.falseFromDeployable, this.falseFromTerrian, this.falseFromClose, this.falseFromHardpoint, this.falseFromPlacementZone, this.falseFromFitRequirements, this.falseFromHittingNothing, this.falseFromStructure });
				Debug.Log(string.Concat("FAIL! - ", str));
			}
			else
			{
				Debug.Log("VALID!");
			}
		}

		public bool Valid()
		{
			return (this.falseFromAngle || this.falseFromDeployable || this.falseFromTerrian || this.falseFromClose || this.falseFromHardpoint || this.falseFromPlacementZone || this.falseFromFitRequirements || this.falseFromHittingNothing ? false : !this.falseFromStructure);
		}
	}

	private sealed class ITEM_TYPE : DeployableItem<DeployableItemDataBlock>, IDeployableItem, IHeldItem, IInventoryItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(DeployableItemDataBlock BLOCK) : base(BLOCK)
		{
		}

		// privatescope
		internal void IHeldItem.AddMod(ItemModDataBlock mod)
		{
			base.AddMod(mod);
		}

		// privatescope
		internal int IHeldItem.FindMod(ItemModDataBlock mod)
		{
			return base.FindMod(mod);
		}

		// privatescope
		internal bool IHeldItem.get_canActivate()
		{
			return base.canActivate;
		}

		// privatescope
		internal bool IHeldItem.get_canDeactivate()
		{
			return base.canDeactivate;
		}

		// privatescope
		internal int IHeldItem.get_freeModSlots()
		{
			return base.freeModSlots;
		}

		// privatescope
		internal ItemModDataBlock[] IHeldItem.get_itemMods()
		{
			return base.itemMods;
		}

		// privatescope
		internal ItemRepresentation IHeldItem.get_itemRepresentation()
		{
			return base.itemRepresentation;
		}

		// privatescope
		internal ItemModFlags IHeldItem.get_modFlags()
		{
			return base.modFlags;
		}

		// privatescope
		internal int IHeldItem.get_totalModSlots()
		{
			return base.totalModSlots;
		}

		// privatescope
		internal int IHeldItem.get_usedModSlots()
		{
			return base.usedModSlots;
		}

		// privatescope
		internal ViewModel IHeldItem.get_viewModelInstance()
		{
			return base.viewModelInstance;
		}

		// privatescope
		internal void IHeldItem.OnActivate()
		{
			base.OnActivate();
		}

		// privatescope
		internal void IHeldItem.OnDeactivate()
		{
			base.OnDeactivate();
		}

		// privatescope
		internal void IHeldItem.set_itemRepresentation(ItemRepresentation value)
		{
			base.itemRepresentation = value;
		}

		// privatescope
		internal void IHeldItem.SetTotalModSlotCount(int count)
		{
			base.SetTotalModSlotCount(count);
		}

		// privatescope
		internal void IHeldItem.SetUsedModSlotCount(int count)
		{
			base.SetUsedModSlotCount(count);
		}

		// privatescope
		internal int IInventoryItem.AddUses(int count)
		{
			return base.AddUses(count);
		}

		// privatescope
		internal bool IInventoryItem.Consume(ref int count)
		{
			return base.Consume(ref count);
		}

		// privatescope
		internal void IInventoryItem.Deserialize(uLink.BitStream stream)
		{
			base.Deserialize(stream);
		}

		// privatescope
		internal bool IInventoryItem.get_active()
		{
			return base.active;
		}

		// privatescope
		internal Character IInventoryItem.get_character()
		{
			return base.character;
		}

		// privatescope
		internal float IInventoryItem.get_condition()
		{
			return base.condition;
		}

		// privatescope
		internal Controllable IInventoryItem.get_controllable()
		{
			return base.controllable;
		}

		// privatescope
		internal Controller IInventoryItem.get_controller()
		{
			return base.controller;
		}

		// privatescope
		internal bool IInventoryItem.get_dirty()
		{
			return base.dirty;
		}

		// privatescope
		internal bool IInventoryItem.get_doNotSave()
		{
			return base.doNotSave;
		}

		// privatescope
		internal IDMain IInventoryItem.get_idMain()
		{
			return base.idMain;
		}

		// privatescope
		internal Inventory IInventoryItem.get_inventory()
		{
			return base.inventory;
		}

		// privatescope
		internal bool IInventoryItem.get_isInLocalInventory()
		{
			return base.isInLocalInventory;
		}

		// privatescope
		internal float IInventoryItem.get_lastUseTime()
		{
			return base.lastUseTime;
		}

		// privatescope
		internal float IInventoryItem.get_maxcondition()
		{
			return base.maxcondition;
		}

		// privatescope
		internal int IInventoryItem.get_slot()
		{
			return base.slot;
		}

		// privatescope
		internal int IInventoryItem.get_uses()
		{
			return base.uses;
		}

		// privatescope
		internal float IInventoryItem.GetConditionPercent()
		{
			return base.GetConditionPercent();
		}

		// privatescope
		internal bool IInventoryItem.IsBroken()
		{
			return base.IsBroken();
		}

		// privatescope
		internal bool IInventoryItem.IsDamaged()
		{
			return base.IsDamaged();
		}

		// privatescope
		internal bool IInventoryItem.MarkDirty()
		{
			return base.MarkDirty();
		}

		// privatescope
		internal void IInventoryItem.Serialize(uLink.BitStream stream)
		{
			base.Serialize(stream);
		}

		// privatescope
		internal void IInventoryItem.set_lastUseTime(float value)
		{
			base.lastUseTime = value;
		}

		// privatescope
		internal void IInventoryItem.SetCondition(float condition)
		{
			base.SetCondition(condition);
		}

		// privatescope
		internal void IInventoryItem.SetMaxCondition(float condition)
		{
			base.SetMaxCondition(condition);
		}

		// privatescope
		internal void IInventoryItem.SetUses(int count)
		{
			base.SetUses(count);
		}

		// privatescope
		internal bool IInventoryItem.TryConditionLoss(float probability, float percentLoss)
		{
			return base.TryConditionLoss(probability, percentLoss);
		}
	}
}