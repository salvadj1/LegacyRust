using System;
using UnityEngine;

public class GUIHeldItem : MonoBehaviour
{
	private const float kOffsetSpeed = 600f;

	private const float kFadeSpeed = 50f;

	private const float kOffsetSmoothTime = 0.06f;

	private const float kFadeSmoothTime = 0.1f;

	private static GUIHeldItem _guiHeldItem;

	public UITexture _icon;

	private UIMaterial _myMaterial;

	public Camera uiCamera;

	private Transform mTrans;

	private Plane planeTest;

	private IInventoryItem _itemHolding;

	private Vector3 offsetPoint;

	private Vector3 offsetVelocity;

	private float lastTime;

	private bool hasItem;

	private bool fadingOut;

	private Vector3 fadeOutPointStart;

	private Vector3 fadeOutPointEnd;

	private Vector3 fadeOutPoint;

	private Vector3 fadeOutVelocity;

	private Vector3 fadeOutPointNormal;

	private float fadeOutPointDistance;

	private float fadeOutPointMagnitude;

	private float fadeOutAlpha;

	private Color startingIconColor = Color.white;

	private bool started;

	public GUIHeldItem()
	{
	}

	public void ClearHeldItem()
	{
		if (this.hasItem)
		{
			this.SetHeldItem((IInventoryItem)null);
			if (!this.fadingOut)
			{
				this.Opaque();
			}
		}
	}

	public void ClearHeldItem(RPOSInventoryCell fadeToCell)
	{
		Vector3 vector3;
		if (this.hasItem)
		{
			this.fadingOut = true;
			this.ClearHeldItem();
			try
			{
				if (NGUITools.GetCentroid(fadeToCell, out vector3))
				{
					this.FadeOutToPoint(vector3);
				}
				return;
			}
			catch
			{
			}
			this.Opaque();
		}
	}

	public static IInventoryItem CurrentItem()
	{
		return GUIHeldItem.Get()._itemHolding;
	}

	public void FadeOutToPoint(Vector3 worldPoint)
	{
		this.Opaque();
		this.fadeOutPointStart = this.mTrans.position;
		this.fadeOutPointEnd = new Vector3(worldPoint.x, worldPoint.y, worldPoint.z);
		if (this.fadeOutPointStart == this.fadeOutPointEnd)
		{
			this.fadeOutPointEnd.z = this.fadeOutPointEnd.z + 1f;
		}
		this.fadeOutPointNormal = this.fadeOutPointEnd - this.fadeOutPointStart;
		this.fadeOutPointMagnitude = this.fadeOutPointNormal.magnitude;
		GUIHeldItem gUIHeldItem = this;
		gUIHeldItem.fadeOutPointNormal = gUIHeldItem.fadeOutPointNormal / this.fadeOutPointMagnitude;
		this.fadeOutPointDistance = Vector3.Dot(this.fadeOutPointNormal, this.fadeOutPointStart);
		this.fadeOutAlpha = 1f;
		this.fadingOut = true;
		this._icon.enabled = true;
		this.fadeOutPoint = this.fadeOutPointStart;
	}

	public static GUIHeldItem Get()
	{
		return GUIHeldItem._guiHeldItem;
	}

	private void MakeEmpty()
	{
		if (this._icon)
		{
			this._icon.enabled = false;
		}
		this._itemHolding = null;
		this.hasItem = false;
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this._myMaterial);
	}

	private void Opaque()
	{
		this.fadeOutAlpha = 1f;
		this.fadeOutPointStart = Vector3.zero;
		this.fadeOutPointEnd = Vector3.right;
		this.fadeOutPointDistance = 1f;
		this.fadeOutPointMagnitude = 1f;
		this.fadeOutPointNormal = Vector3.right;
		this.fadeOutVelocity = Vector3.zero;
		this.fadingOut = false;
		if (this.started)
		{
			this._icon.color = this.startingIconColor;
		}
	}

	public bool SetHeldItem(IInventoryItem item)
	{
		if (item == null)
		{
			this.MakeEmpty();
			if (!this.fadingOut)
			{
				this.Opaque();
			}
			return false;
		}
		this.hasItem = true;
		Texture texture = item.datablock.iconTex;
		ItemDataBlock.LoadIconOrUnknown<Texture>(item.datablock.icon, ref texture);
		this._icon.enabled = true;
		this._myMaterial.Set("_MainTex", texture);
		this._itemHolding = item;
		Vector3 vector3 = new Vector2();
		Vector3 vector31 = vector3;
		this.offsetPoint = vector3;
		this.offsetVelocity = vector31;
		this.Opaque();
		return true;
	}

	public bool SetHeldItem(RPOSInventoryCell cell)
	{
		Vector3 vector3;
		IInventoryItem inventoryItem;
		if (!cell)
		{
			inventoryItem = null;
		}
		else
		{
			inventoryItem = cell.slotItem;
		}
		if (!this.SetHeldItem(inventoryItem))
		{
			return false;
		}
		try
		{
			if (NGUITools.GetCentroid(cell, out vector3))
			{
				Vector2 screenPoint = UICamera.FindCameraForLayer(cell.gameObject.layer).cachedCamera.WorldToScreenPoint(vector3);
				this.offsetPoint = screenPoint - UICamera.lastMousePosition;
			}
		}
		catch
		{
			this.offsetPoint = Vector3.zero;
		}
		return true;
	}

	private void SetPosition(Vector3 world)
	{
		Vector3 vector3 = this.mTrans.localPosition + this.mTrans.InverseTransformPoint(world);
		vector3.z = -190f;
		this.mTrans.localPosition = vector3;
	}

	private void Start()
	{
		this.startingIconColor = this._icon.color;
		this._icon.enabled = false;
		GUIHeldItem._guiHeldItem = this;
		this._myMaterial = this._icon.material.Clone();
		this._icon.material = this._myMaterial;
		this.mTrans = base.transform;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.planeTest = new Plane(this.uiCamera.transform.forward * 1f, new Vector3(0f, 0f, 2f));
		this.started = true;
	}

	private void Update()
	{
		if (this.hasItem)
		{
			Vector3 vector3 = UICamera.lastMousePosition + this.offsetPoint;
			Ray ray = this.uiCamera.ScreenPointToRay(vector3);
			float single = 0f;
			if (this.planeTest.Raycast(ray, out single))
			{
				this.SetPosition(ray.GetPoint(single));
			}
			this.offsetPoint = Vector3.SmoothDamp(this.offsetPoint, Vector3.zero, ref this.offsetVelocity, 0.06f, 600f);
		}
		else if (this.fadingOut)
		{
			this.fadeOutPoint = Vector3.SmoothDamp(this.fadeOutPoint, this.fadeOutPointEnd, ref this.fadeOutVelocity, 0.1f, 50f);
			this.fadeOutAlpha = this.startingIconColor.a * (1f - Mathf.Clamp01(Mathf.Abs(Vector3.Dot(this.fadeOutPointNormal, this.fadeOutPoint) - this.fadeOutPointDistance) / this.fadeOutPointMagnitude));
			if ((double)this.fadeOutAlpha > 0.00390625)
			{
				Color color = this._icon.color;
				this.SetPosition(this.fadeOutPoint);
				color.a = this.fadeOutAlpha;
				this._icon.color = color;
			}
			else
			{
				this.fadingOut = false;
				this.MakeEmpty();
			}
		}
	}
}