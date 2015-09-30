using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : IgnoreTimeScale
{
	public Transform target;

	public Transform sizeParent;

	public Vector3 scale = Vector3.one;

	public float scrollWheelFactor;

	public bool restrictWithinPanel;

	public bool restrictToScreen;

	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	public float momentumAmount = 35f;

	private Plane mPlane;

	private Vector3 mLastPos;

	private UIPanel mPanel;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private AABBox mBounds;

	public static RectOffset screenBorder
	{
		get
		{
			return new RectOffset(0, -64, 0, 0);
		}
	}

	public UIDragObject()
	{
	}

	private void FindPanel()
	{
		UIPanel uIPanel;
		if (this.target == null)
		{
			uIPanel = null;
		}
		else
		{
			uIPanel = UIPanel.Find(this.target.transform, false);
		}
		this.mPanel = uIPanel;
		if (this.mPanel == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	private void LateUpdate()
	{
		float single = base.UpdateRealTimeDelta();
		if (this.target == null)
		{
			return;
		}
		if (!this.mPressed)
		{
			UIDragObject uIDragObject = this;
			uIDragObject.mMomentum = uIDragObject.mMomentum + (this.scale * (-this.mScroll * 0.05f));
			this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, single);
			if (this.mMomentum.magnitude <= 0.0001f)
			{
				this.mScroll = 0f;
			}
			else
			{
				if (this.mPanel == null)
				{
					this.FindPanel();
				}
				if (this.mPanel != null)
				{
					Transform transforms = this.target;
					transforms.position = transforms.position + NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
					if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
					{
						this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, this.target);
						if (!this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
						{
							SpringPosition component = this.target.GetComponent<SpringPosition>();
							if (component != null)
							{
								component.enabled = false;
							}
						}
					}
					return;
				}
			}
		}
		else
		{
			SpringPosition springPosition = this.target.GetComponent<SpringPosition>();
			if (springPosition != null)
			{
				springPosition.enabled = false;
			}
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
	}

	private void OnDrag(Vector2 delta)
	{
		Vector2 vector2;
		if (base.enabled && base.gameObject.activeInHierarchy && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float single = 0f;
			if (this.mPlane.Raycast(ray, out single))
			{
				Vector3 point = ray.GetPoint(single);
				Vector3 vector3 = point - this.mLastPos;
				this.mLastPos = point;
				if (vector3.x != 0f || vector3.y != 0f)
				{
					vector3 = this.target.InverseTransformDirection(vector3);
					vector3.Scale(this.scale);
					vector3 = this.target.TransformDirection(vector3);
				}
				this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + (vector3 * (0.01f * this.momentumAmount)), 0.67f);
				if (this.restrictWithinPanel)
				{
					Vector3 vector31 = this.target.localPosition;
					Transform transforms = this.target;
					transforms.position = transforms.position + vector3;
					this.mBounds.center = this.mBounds.center + (this.target.localPosition - vector31);
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.mPanel.clipping != UIDrawCall.Clipping.None && this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.mMomentum = Vector3.zero;
						this.mScroll = 0f;
					}
				}
				else if (this.restrictToScreen)
				{
					Transform transforms1 = this.target;
					transforms1.position = transforms1.position + vector3;
					vector2 = (!this.sizeParent ? NGUIMath.CalculateRelativeWidgetBounds(this.target).size : this.sizeParent.transform.localScale);
					Rect rect = UIDragObject.screenBorder.Add(new Rect(0f, (float)(-Screen.height), (float)Screen.width, (float)Screen.height));
					Vector3 vector32 = this.target.localPosition;
					bool flag = true;
					if (vector32.x + vector2.x > rect.xMax)
					{
						vector32.x = rect.xMax - vector2.x;
					}
					else if (vector32.x >= rect.xMin)
					{
						flag = false;
					}
					else
					{
						vector32.x = rect.xMin;
					}
					bool flag1 = true;
					if (vector32.y > rect.yMax)
					{
						vector32.y = rect.yMax;
					}
					else if (vector32.y - vector2.y >= rect.yMin)
					{
						flag1 = false;
					}
					else
					{
						vector32.y = rect.yMin + vector2.y;
					}
					if (flag || flag1)
					{
						this.target.localPosition = vector32;
					}
				}
			}
		}
	}

	private void OnPress(bool pressed)
	{
		if (base.enabled && base.gameObject.activeInHierarchy && this.target != null)
		{
			this.mPressed = pressed;
			if (pressed)
			{
				if ((this.restrictWithinPanel || this.restrictToScreen) && this.mPanel == null)
				{
					this.FindPanel();
				}
				if (this.restrictWithinPanel)
				{
					this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, this.target);
				}
				if (this.restrictToScreen)
				{
					UICamera uICamera = UICamera.FindCameraForLayer(base.gameObject.layer);
					Rect rect = UIDragObject.screenBorder.Add(uICamera.camera.pixelRect);
					this.mBounds = AABBox.CenterAndSize(rect.center, new Vector3(rect.width, rect.height, 0f));
				}
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
				SpringPosition component = this.target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
				this.mLastPos = UICamera.lastHit.point;
				Transform transforms = UICamera.currentCamera.transform;
				this.mPlane = new Plane((this.mPanel == null ? transforms.rotation : this.mPanel.cachedTransform.rotation) * Vector3.back, this.mLastPos);
			}
			else if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
			{
				this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, false);
			}
		}
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			UIDragObject uIDragObject = this;
			uIDragObject.mScroll = uIDragObject.mScroll + delta * this.scrollWheelFactor;
		}
	}

	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}
}