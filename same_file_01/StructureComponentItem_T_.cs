using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

public abstract class StructureComponentItem<T> : HeldItem<T>
where T : StructureComponentDataBlock
{
	protected bool validLocation;

	protected float _nextPlaceTime;

	protected StructureMaster _master;

	protected Vector3 _placePos;

	protected Quaternion _placeRot;

	protected PrefabRenderer _prefabRenderer;

	protected MaterialPropertyBlock _materialProps;

	protected bool lastFrameAttack2;

	public Quaternion desiredRotation;

	private static bool informedPreRender;

	private static bool informedPreFrame;

	protected StructureComponentItem(T db) : base(db)
	{
	}

	public virtual bool CanPlace()
	{
		return (!this.validLocation ? false : this._nextPlaceTime <= Time.time);
	}

	public virtual void DoPlace()
	{
		this._nextPlaceTime = Time.time + 0.5f;
		Character character = base.character;
		if (character == null)
		{
			Debug.Log("NO char for placement");
			return;
		}
		Ray ray = character.eyesRay;
		ItemRepresentation itemRepresentation = base.itemRepresentation;
		object[] objArray = new object[] { ray.origin, ray.direction, this._placePos, this._placeRot, null };
		objArray[4] = (this._master == null ? uLink.NetworkViewID.unassigned : this._master.networkView.viewID);
		itemRepresentation.Action(1, uLink.RPCMode.Server, objArray);
	}

	private void InformException(Exception e, string title, ref bool informedOnce, UnityEngine.Object obj = null)
	{
		if (informedOnce)
		{
			Debug.LogException(e);
		}
		else
		{
			Debug.LogError(string.Concat(title, "\n", e), obj);
			informedOnce = true;
		}
	}

	public bool IsValidLocation()
	{
		return false;
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		base.ItemPreFrame(ref sample);
		try
		{
			if (sample.attack2 && !this.lastFrameAttack2)
			{
				this.desiredRotation = this.desiredRotation * Quaternion.AngleAxis(90f, Vector3.up);
			}
			if (sample.attack && this.CanPlace())
			{
				this.DoPlace();
			}
			this.lastFrameAttack2 = sample.attack2;
		}
		catch (Exception exception)
		{
			this.InformException(exception, "in ItemPreFrame", ref StructureComponentItem<T>.informedPreFrame, null);
		}
	}

	public override void PreCameraRender()
	{
		try
		{
			this.RenderPlacementHelpers();
		}
		catch (Exception exception)
		{
			this.InformException(exception, "in PreCameraRender()", ref StructureComponentItem<T>.informedPreRender, null);
		}
	}

	public virtual void RenderDeployPreview(Vector3 point, Quaternion rot)
	{
		if (this._prefabRenderer == null)
		{
			StructureComponent structureComponent = this.datablock.structureToPlacePrefab;
			if (!structureComponent)
			{
				return;
			}
			this._prefabRenderer = PrefabRenderer.GetOrCreateRender(structureComponent.gameObject);
			this._materialProps = new MaterialPropertyBlock();
		}
		Material material = (T)this.datablock.overrideMat;
		if (!material)
		{
			this._prefabRenderer.Render(MountedCamera.main.camera, Matrix4x4.TRS(point, rot, Vector3.one), this._materialProps, null);
		}
		else
		{
			this._prefabRenderer.RenderOneMaterial(MountedCamera.main.camera, Matrix4x4.TRS(point, rot, Vector3.one), this._materialProps, material);
		}
	}

	protected void RenderPlacementHelpers()
	{
		RaycastHit raycastHit;
		int num;
		int num1;
		int num2;
		StructureComponent structureComponent = this.datablock.structureToPlacePrefab;
		this._master = null;
		this._placePos = Vector3.zero;
		this._placeRot = Quaternion.identity;
		this.validLocation = false;
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			this.desiredRotation = this.desiredRotation * Quaternion.AngleAxis(90f, Vector3.up);
		}
		else if (axis < 0f)
		{
			this.desiredRotation = this.desiredRotation * Quaternion.AngleAxis(-90f, Vector3.up);
		}
		Character character = base.character;
		if (character == null)
		{
			return;
		}
		Ray ray = character.eyesRay;
		float single = (structureComponent.type != StructureComponent.StructureComponentType.Ceiling ? 8f : 4f);
		Vector3 vector3 = Vector3.zero;
		Vector3 vector31 = Vector3.up;
		Vector3 vector32 = Vector3.zero;
		bool flag = false;
		if (!Physics.Raycast(ray, out raycastHit, single))
		{
			flag = false;
			Vector3 vector33 = ray.origin + (ray.direction * single);
			vector3 = vector33;
			vector32 = vector33;
		}
		else
		{
			Vector3 vector34 = raycastHit.point;
			vector3 = vector34;
			vector32 = vector34;
			vector31 = raycastHit.normal;
			flag = true;
		}
		switch (structureComponent.type)
		{
			case StructureComponent.StructureComponentType.Ceiling:
			case StructureComponent.StructureComponentType.Foundation:
			case StructureComponent.StructureComponentType.Ramp:
			{
				vector3.y = vector3.y - 3.5f;
				goto case StructureComponent.StructureComponentType.WindowWall;
			}
			case StructureComponent.StructureComponentType.Stairs:
			case StructureComponent.StructureComponentType.WindowWall:
			{
				bool flag1 = false;
				bool flag2 = false;
				Vector3 vector35 = vector3;
				Angle2 angle2 = character.eyesAngles;
				Quaternion quaternion = TransformHelpers.LookRotationForcedUp(angle2.forward, Vector3.up) * this.desiredRotation;
				StructureMaster[] structureMasterArray = StructureMaster.RayTestStructures(ray);
				for (int i = 0; i < (int)structureMasterArray.Length; i++)
				{
					StructureMaster structureMaster = structureMasterArray[i];
					if (structureMaster)
					{
						structureMaster.GetStructureSize(out num, out num1, out num2);
						this._placePos = StructureMaster.SnapToGrid(structureMaster.transform, vector3, true);
						this._placeRot = TransformHelpers.LookRotationForcedUp(structureMaster.transform.forward, structureMaster.transform.transform.up) * this.desiredRotation;
						if (!flag2)
						{
							vector35 = this._placePos;
							quaternion = this._placeRot;
							flag2 = true;
						}
						if (structureComponent.CheckLocation(structureMaster, this._placePos, this._placeRot))
						{
							this._master = structureMaster;
							flag1 = true;
							break;
						}
					}
				}
				if (flag1)
				{
					this.validLocation = true;
				}
				else if (structureComponent.type != StructureComponent.StructureComponentType.Foundation)
				{
					this._placePos = vector35;
					this._placeRot = quaternion;
					this.validLocation = false;
				}
				else if (!flag || !(raycastHit.collider is TerrainCollider))
				{
					this._placePos = vector35;
					this._placeRot = quaternion;
					this.validLocation = false;
				}
				else
				{
					bool flag3 = false;
					foreach (StructureMaster allStructuresWithBound in StructureMaster.AllStructuresWithBounds)
					{
						if (!allStructuresWithBound.containedBounds.Intersects(new Bounds(vector3, new Vector3(5f, 5f, 4f))))
						{
							continue;
						}
						flag3 = true;
						break;
					}
					if (!flag3)
					{
						this._placePos = vector3;
						Angle2 angle21 = character.eyesAngles;
						this._placeRot = TransformHelpers.LookRotationForcedUp(angle21.forward, Vector3.up) * this.desiredRotation;
						this.validLocation = true;
					}
				}
				if (!this.datablock.CheckBlockers(this._placePos))
				{
					this.validLocation = false;
				}
				Color color = Color.red;
				if (this.validLocation)
				{
					color = Color.green;
				}
				color.a = 0.5f + Mathf.Abs(Mathf.Sin(Time.time * 8f)) * 0.25f;
				if (this._materialProps != null)
				{
					this._materialProps.Clear();
					this._materialProps.AddColor("_EmissionColor", color);
					this._materialProps.AddVector("_MainTex_ST", new Vector4(1f, 1f, 0f, Mathf.Repeat(Time.time, 30f)));
				}
				if (!this.validLocation)
				{
					this._placePos = vector3;
				}
				this.RenderDeployPreview(this._placePos, this._placeRot);
				return;
			}
			default:
			{
				goto case StructureComponent.StructureComponentType.WindowWall;
			}
		}
	}
}