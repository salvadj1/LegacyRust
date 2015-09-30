using System;
using uLink;
using UnityEngine;

public abstract class DeployableItem<T> : HeldItem<T>
where T : DeployableItemDataBlock
{
	protected float _nextPlaceTime;

	protected PrefabRenderer _prefabRenderer;

	[NonSerialized]
	private bool _aimHelper;

	[NonSerialized]
	private GameGizmo.Instance _aimGizmo;

	protected DeployableItem(T db) : base(db)
	{
	}

	public virtual bool CanPlace()
	{
		Vector3 vector3;
		Quaternion quaternion;
		TransCarrier transCarrier;
		if (this.datablock.ObjectToPlace == null)
		{
			return false;
		}
		T t = this.datablock;
		bool flag = t.CheckPlacement(base.character.eyesRay, out vector3, out quaternion, out transCarrier);
		this._aimGizmo.rotation = quaternion;
		this._aimGizmo.position = vector3;
		if (!flag)
		{
			return false;
		}
		return this._nextPlaceTime <= Time.time;
	}

	public virtual void DoPlace()
	{
		Vector3 vector3;
		Quaternion quaternion;
		TransCarrier transCarrier;
		Ray ray = base.character.eyesRay;
		T t = this.datablock;
		t.CheckPlacement(ray, out vector3, out quaternion, out transCarrier);
		this._nextPlaceTime = Time.time + 0.5f;
		base.itemRepresentation.Action(1, uLink.RPCMode.Server, new object[] { ray.origin, ray.direction });
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		base.ItemPreFrame(ref sample);
		if (!this._aimHelper)
		{
			return;
		}
		if (sample.attack && this.CanPlace())
		{
			Character character = base.inventory.idMain as Character;
			if (character && !character.stateFlags.grounded)
			{
				return;
			}
			this.DoPlace();
		}
	}

	protected override void OnSetActive(bool isActive)
	{
		base.OnSetActive(isActive);
		if (isActive)
		{
			if (!this._aimHelper && (T)this.datablock.aimGizmo)
			{
				this._aimHelper = (T)this.datablock.aimGizmo.Create<GameGizmo.Instance>(out this._aimGizmo);
			}
		}
		else if (this._aimHelper)
		{
			this._aimHelper = !this._aimGizmo.gameGizmo.Destroy<GameGizmo.Instance>(ref this._aimGizmo);
		}
	}

	public override void PreCameraRender()
	{
		Vector3 vector3;
		Quaternion quaternion;
		TransCarrier transCarrier;
		if (!this._aimHelper)
		{
			return;
		}
		T t = this.datablock;
		bool flag = t.CheckPlacement(base.character.eyesRay, out vector3, out quaternion, out transCarrier);
		Color color = (!flag ? this._aimGizmo.gameGizmo.badColor : this._aimGizmo.gameGizmo.goodColor);
		this._aimGizmo.propertyBlock.Clear();
		this._aimGizmo.propertyBlock.AddColor("_EmissionColor", color);
		Vector4 vector4 = new Vector4(1f, 1f, 0f, Mathf.Repeat(Time.time, 30f));
		this._aimGizmo.propertyBlock.AddVector("_MainTex_ST", vector4);
		this._aimGizmo.propertyBlock.AddVector("_GizmoWorldPos", vector3);
		if (this._aimGizmo is GameGizmoWaveAnimation.Instance)
		{
			GameGizmoWaveAnimation.Instance instance = (GameGizmoWaveAnimation.Instance)this._aimGizmo;
			if (!flag)
			{
				instance.propertyBlock.AddFloat("_PushIn", (float)instance.@value);
				instance.propertyBlock.AddFloat("_PushOut", (float)(-instance.@value));
			}
			else
			{
				instance.propertyBlock.AddFloat("_PushOut", (float)instance.@value);
			}
		}
		this.RenderDeployPreview(vector3, quaternion, transCarrier);
		this._aimGizmo.Render();
	}

	public virtual void RenderDeployPreview(Vector3 point, Quaternion rot, TransCarrier carrier)
	{
		if (this._aimGizmo != null)
		{
			this._aimGizmo.rotation = rot;
			this._aimGizmo.position = point;
		}
		if (this._prefabRenderer == null)
		{
			DeployableObject objectToPlace = this.datablock.ObjectToPlace;
			if (!objectToPlace)
			{
				return;
			}
			this._prefabRenderer = PrefabRenderer.GetOrCreateRender(objectToPlace.gameObject);
		}
		Material material = (T)this.datablock.overrideMat;
		if (!material)
		{
			PrefabRenderer prefabRenderer = this._prefabRenderer;
			Camera camera = MountedCamera.main.camera;
			T t = this.datablock;
			prefabRenderer.Render(camera, Matrix4x4.TRS(point, rot, t.ObjectToPlace.transform.localScale), this._aimGizmo.propertyBlock, null);
		}
		else
		{
			PrefabRenderer prefabRenderer1 = this._prefabRenderer;
			Camera camera1 = MountedCamera.main.camera;
			T t1 = this.datablock;
			prefabRenderer1.RenderOneMaterial(camera1, Matrix4x4.TRS(point, rot, t1.ObjectToPlace.transform.localScale), this._aimGizmo.propertyBlock, material);
		}
		if (this._aimGizmo != null)
		{
			bool flag = false;
			if (carrier)
			{
				Renderer renderer = carrier.renderer;
				if (renderer is MeshRenderer && renderer && renderer.enabled)
				{
					flag = true;
					this._aimGizmo.carrierRenderer = (MeshRenderer)renderer;
				}
			}
			if (!flag)
			{
				this._aimGizmo.carrierRenderer = null;
			}
		}
	}
}