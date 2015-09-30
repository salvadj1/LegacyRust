using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Draggable Panel")]
[ExecuteInEditMode]
[RequireComponent(typeof(UIPanel))]
public class UIDraggablePanel : IgnoreTimeScale
{
	public bool restrictWithinPanel = true;

	public bool restrictWithinPanelWithScroll = true;

	public bool disableDragIfFits;

	public UIDraggablePanel.DragEffect dragEffect = UIDraggablePanel.DragEffect.MomentumAndSpring;

	public Vector3 scale = Vector3.one;

	public float scrollWheelFactor;

	public float momentumAmount = 35f;

	public Vector2 relativePositionOnReset = Vector2.zero;

	public bool repositionClipping;

	public UIScrollBar horizontalScrollBar;

	public UIScrollBar verticalScrollBar;

	public UIDraggablePanel.ShowCondition showScrollBars = UIDraggablePanel.ShowCondition.OnlyIfNeeded;

	[SerializeField]
	private bool _calculateBoundsEveryChange = true;

	private bool _panelMayNeedBoundCalculation;

	private Transform mTrans;

	private UIPanel mPanel;

	private Plane mPlane;

	private Vector3 mLastPos;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private AABBox mBounds;

	private bool mCalculatedBounds;

	private bool mShouldMove;

	private bool mIgnoreCallbacks;

	private bool mStartedManually;

	private bool mStartedAutomatically;

	private int mTouches;

	private bool _calculateNextChange;

	public bool respondHoverScroll = true;

	private UIDraggablePanel.CalculatedNextChangeCallback calculatedNextChangeCallback;

	public AABBox bounds
	{
		get
		{
			if (!this.mCalculatedBounds)
			{
				this.mCalculatedBounds = true;
				this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
			}
			return this.mBounds;
		}
	}

	public bool calculateBoundsEveryChange
	{
		get
		{
			return this._calculateBoundsEveryChange;
		}
		set
		{
			if (!value)
			{
				this._calculateBoundsEveryChange = false;
			}
			else if (!this._calculateBoundsEveryChange)
			{
				this.CalculateBoundsIfNeeded();
				this._calculateBoundsEveryChange = true;
			}
		}
	}

	public bool calculateNextChange
	{
		set
		{
			if (value)
			{
				this._calculateNextChange = true;
			}
		}
	}

	public Vector3 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
		}
	}

	public bool panelMayNeedBoundsCalculated
	{
		get
		{
			return this._panelMayNeedBoundCalculation;
		}
	}

	private bool shouldMove
	{
		get
		{
			if (!this.disableDragIfFits)
			{
				return true;
			}
			if (this.mPanel == null)
			{
				this.mPanel = base.GetComponent<UIPanel>();
			}
			Vector4 vector4 = this.mPanel.clipRange;
			AABBox aABBox = this.bounds;
			float single = vector4.z * 0.5f;
			float single1 = vector4.w * 0.5f;
			if (!Mathf.Approximately(this.scale.x, 0f))
			{
				if (aABBox.min.x < vector4.x - single)
				{
					return true;
				}
				if (aABBox.max.x > vector4.x + single)
				{
					return true;
				}
			}
			if (!Mathf.Approximately(this.scale.y, 0f))
			{
				if (aABBox.min.y < vector4.y - single1)
				{
					return true;
				}
				if (aABBox.max.y > vector4.y + single1)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool shouldMoveHorizontally
	{
		get
		{
			float single = this.bounds.size.x;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				Vector2 vector2 = this.mPanel.clipSoftness;
				single = single + vector2.x * 2f;
			}
			return single > this.mPanel.clipRange.z;
		}
	}

	public bool shouldMoveVertically
	{
		get
		{
			float single = this.bounds.size.y;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				Vector2 vector2 = this.mPanel.clipSoftness;
				single = single + vector2.y * 2f;
			}
			return single > this.mPanel.clipRange.w;
		}
	}

	public UIDraggablePanel()
	{
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		this.mPanel = base.GetComponent<UIPanel>();
	}

	public bool CalculateBoundsIfNeeded()
	{
		if (!this._panelMayNeedBoundCalculation)
		{
			return false;
		}
		this.UpdateScrollbars(true);
		return !this._panelMayNeedBoundCalculation;
	}

	public void DisableSpring()
	{
		SpringPanel component = base.GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public void Drag(Vector2 delta)
	{
		if (base.enabled && base.gameObject.activeInHierarchy && this.mShouldMove)
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
					vector3 = this.mTrans.InverseTransformDirection(vector3);
					vector3.Scale(this.scale);
					vector3 = this.mTrans.TransformDirection(vector3);
				}
				this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + (vector3 * (0.01f * this.momentumAmount)), 0.67f);
				this.MoveAbsolute(vector3);
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect != UIDraggablePanel.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(false);
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (!this.mPanel.enabled)
		{
			this.mMomentum = Vector3.zero;
			return;
		}
		if (this.mPanel.changedLastFrame)
		{
			this.OnPanelChanged();
		}
		if (this.repositionClipping)
		{
			this.repositionClipping = false;
			this.mCalculatedBounds = false;
			this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
		}
		if (!Application.isPlaying)
		{
			return;
		}
		float single = base.UpdateRealTimeDelta();
		if (this.showScrollBars != UIDraggablePanel.ShowCondition.Always)
		{
			bool flag = false;
			bool flag1 = false;
			if (this.showScrollBars != UIDraggablePanel.ShowCondition.WhenDragging || this.mTouches > 0)
			{
				flag = this.shouldMoveVertically;
				flag1 = this.shouldMoveHorizontally;
			}
			if (this.verticalScrollBar)
			{
				float single1 = this.verticalScrollBar.alpha;
				single1 = single1 + (!flag ? -single * 3f : single * 6f);
				single1 = Mathf.Clamp01(single1);
				if (this.verticalScrollBar.alpha != single1)
				{
					this.verticalScrollBar.alpha = single1;
				}
			}
			if (this.horizontalScrollBar)
			{
				float single2 = this.horizontalScrollBar.alpha;
				single2 = single2 + (!flag1 ? -single * 3f : single * 6f);
				single2 = Mathf.Clamp01(single2);
				if (this.horizontalScrollBar.alpha != single2)
				{
					this.horizontalScrollBar.alpha = single2;
				}
			}
		}
		if (!this.mShouldMove || this.mPressed)
		{
			this.mScroll = 0f;
		}
		else
		{
			UIDraggablePanel uIDraggablePanel = this;
			uIDraggablePanel.mMomentum = uIDraggablePanel.mMomentum + (this.scale * (-this.mScroll * 0.05f));
			if (this.mMomentum.magnitude > 0.0001f)
			{
				this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, single);
				Vector3 vector3 = NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
				this.MoveAbsolute(vector3);
				if ((this.restrictWithinPanel || this.restrictWithinPanelWithScroll) && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					this.RestrictWithinBounds(false);
				}
				return;
			}
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
	}

	public bool ManualStart()
	{
		if (!this.mStartedManually)
		{
			if (!this.mStartedAutomatically)
			{
				this.Start();
				this.mStartedManually = true;
				return true;
			}
		}
		return false;
	}

	private void MoveAbsolute(Vector3 absolute)
	{
		Vector3 vector3 = this.mTrans.InverseTransformPoint(absolute);
		Vector3 vector31 = this.mTrans.InverseTransformPoint(Vector3.zero);
		this.MoveRelative(vector3 - vector31);
	}

	private void MoveRelative(Vector3 relative)
	{
		Transform transforms = this.mTrans;
		transforms.localPosition = transforms.localPosition + relative;
		Vector4 vector4 = this.mPanel.clipRange;
		vector4.x = vector4.x - relative.x;
		vector4.y = vector4.y - relative.y;
		this.mPanel.clipRange = vector4;
		this.UpdateScrollbars(false);
	}

	private void OnHorizontalBar(UIScrollBar sb)
	{
		if (!this.mIgnoreCallbacks)
		{
			float single = (this.horizontalScrollBar == null ? 0f : this.horizontalScrollBar.scrollValue);
			this.SetDragAmount(single, (this.verticalScrollBar == null ? 0f : this.verticalScrollBar.scrollValue), false);
		}
	}

	private void OnHoverScroll(float y)
	{
		if (this.respondHoverScroll)
		{
			this.Scroll(y);
		}
	}

	private void OnPanelChanged()
	{
		if (this._calculateNextChange)
		{
			this._calculateNextChange = false;
			this.UpdateScrollbars(true);
			if (this.calculatedNextChangeCallback != null)
			{
				UIDraggablePanel.CalculatedNextChangeCallback calculatedNextChangeCallback = this.calculatedNextChangeCallback;
				this.calculatedNextChangeCallback = null;
				calculatedNextChangeCallback();
			}
		}
		else if (!Application.isPlaying || this._calculateBoundsEveryChange)
		{
			this.UpdateScrollbars(true);
		}
		else
		{
			this._panelMayNeedBoundCalculation = true;
		}
	}

	private void OnVerticalBar(UIScrollBar sb)
	{
		if (!this.mIgnoreCallbacks)
		{
			float single = (this.horizontalScrollBar == null ? 0f : this.horizontalScrollBar.scrollValue);
			this.SetDragAmount(single, (this.verticalScrollBar == null ? 0f : this.verticalScrollBar.scrollValue), false);
		}
	}

	public void Press(bool pressed)
	{
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			UIDraggablePanel uIDraggablePanel = this;
			uIDraggablePanel.mTouches = uIDraggablePanel.mTouches + (!pressed ? -1 : 1);
			this.mCalculatedBounds = false;
			this.mShouldMove = this.shouldMove;
			if (!this.mShouldMove)
			{
				return;
			}
			this.mPressed = pressed;
			if (pressed)
			{
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
				this.DisableSpring();
				this.mLastPos = UICamera.lastHit.point;
				this.mPlane = new Plane(this.mTrans.rotation * Vector3.back, this.mLastPos);
			}
			else if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect == UIDraggablePanel.DragEffect.MomentumAndSpring)
			{
				this.RestrictWithinBounds(false);
			}
		}
	}

	public void ResetPosition()
	{
		this.mCalculatedBounds = false;
		this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, false);
		this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
	}

	public void RestrictWithinBounds(bool instant)
	{
		UIPanel uIPanel = this.mPanel;
		AABBox aABBox = this.bounds;
		Vector3 vector3 = uIPanel.CalculateConstrainOffset(aABBox.min, this.bounds.max);
		if (vector3.magnitude <= 0.001f)
		{
			this.DisableSpring();
		}
		else if (instant || this.dragEffect != UIDraggablePanel.DragEffect.MomentumAndSpring)
		{
			this.MoveRelative(vector3);
			this.mMomentum = Vector3.zero;
			this.mScroll = 0f;
		}
		else
		{
			SpringPanel.Begin(this.mPanel.gameObject, this.mTrans.localPosition + vector3, 13f);
		}
	}

	public void Scroll(float delta)
	{
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			this.mShouldMove = this.shouldMove;
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			UIDraggablePanel uIDraggablePanel = this;
			uIDraggablePanel.mScroll = uIDraggablePanel.mScroll + delta * this.scrollWheelFactor;
		}
	}

	public void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		this.DisableSpring();
		AABBox aABBox = this.bounds;
		if (aABBox.min.x == aABBox.max.x || aABBox.min.y == aABBox.max.x)
		{
			return;
		}
		Vector4 vector4 = this.mPanel.clipRange;
		float single = vector4.z * 0.5f;
		float single1 = vector4.w * 0.5f;
		float single2 = aABBox.min.x + single;
		float single3 = aABBox.max.x - single;
		float single4 = aABBox.min.y + single1;
		float single5 = aABBox.max.y - single1;
		if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			single2 = single2 - this.mPanel.clipSoftness.x;
			single3 = single3 + this.mPanel.clipSoftness.x;
			single4 = single4 - this.mPanel.clipSoftness.y;
			single5 = single5 + this.mPanel.clipSoftness.y;
		}
		float single6 = Mathf.Lerp(single2, single3, x);
		float single7 = Mathf.Lerp(single5, single4, y);
		if (!updateScrollbars)
		{
			Vector3 vector3 = this.mTrans.localPosition;
			if (this.scale.x != 0f)
			{
				vector3.x = vector3.x + (vector4.x - single6);
			}
			if (this.scale.y != 0f)
			{
				vector3.y = vector3.y + (vector4.y - single7);
			}
			this.mTrans.localPosition = vector3;
		}
		vector4.x = single6;
		vector4.y = single7;
		this.mPanel.clipRange = vector4;
		if (updateScrollbars)
		{
			this.UpdateScrollbars(false);
		}
	}

	private void Start()
	{
		if (this.mStartedManually)
		{
			return;
		}
		this.UpdateScrollbars(true);
		if (this.horizontalScrollBar != null)
		{
			this.horizontalScrollBar.onChange += new UIScrollBar.OnScrollBarChange(this.OnHorizontalBar);
			this.horizontalScrollBar.alpha = (this.showScrollBars == UIDraggablePanel.ShowCondition.Always || this.shouldMoveHorizontally ? 1f : 0f);
		}
		if (this.verticalScrollBar != null)
		{
			this.verticalScrollBar.onChange += new UIScrollBar.OnScrollBarChange(this.OnVerticalBar);
			this.verticalScrollBar.alpha = (this.showScrollBars == UIDraggablePanel.ShowCondition.Always || this.shouldMoveVertically ? 1f : 0f);
		}
		this.mStartedAutomatically = true;
	}

	public void UpdateScrollbars(bool recalculateBounds)
	{
		if (this.mPanel == null)
		{
			return;
		}
		if (this.horizontalScrollBar != null || this.verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				this.mCalculatedBounds = false;
				this._panelMayNeedBoundCalculation = false;
				this.mShouldMove = this.shouldMove;
			}
			if (this.horizontalScrollBar != null)
			{
				AABBox aABBox = this.bounds;
				Vector3 vector3 = aABBox.size;
				if (vector3.x > 0f)
				{
					Vector4 vector4 = this.mPanel.clipRange;
					float single = vector4.z * 0.5f;
					Vector3 vector31 = aABBox.min;
					float single1 = vector4.x - single - vector31.x;
					Vector3 vector32 = aABBox.max;
					float single2 = vector32.x - single - vector4.x;
					if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
					{
						single1 = single1 + this.mPanel.clipSoftness.x;
						single2 = single2 - this.mPanel.clipSoftness.x;
					}
					single1 = Mathf.Clamp01(single1 / vector3.x);
					single2 = Mathf.Clamp01(single2 / vector3.x);
					float single3 = single1 + single2;
					this.mIgnoreCallbacks = true;
					this.horizontalScrollBar.barSize = 1f - single3;
					this.horizontalScrollBar.scrollValue = (single3 <= 0.001f ? 0f : single1 / single3);
					this.mIgnoreCallbacks = false;
				}
			}
			if (this.verticalScrollBar != null)
			{
				AABBox aABBox1 = this.bounds;
				Vector3 vector33 = aABBox1.size;
				if (vector33.y > 0f)
				{
					Vector4 vector41 = this.mPanel.clipRange;
					float single4 = vector41.w * 0.5f;
					Vector3 vector34 = aABBox1.min;
					float single5 = vector41.y - single4 - vector34.y;
					Vector3 vector35 = aABBox1.max;
					float single6 = vector35.y - single4 - vector41.y;
					if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
					{
						single5 = single5 + this.mPanel.clipSoftness.y;
						single6 = single6 - this.mPanel.clipSoftness.y;
					}
					single5 = Mathf.Clamp01(single5 / vector33.y);
					single6 = Mathf.Clamp01(single6 / vector33.y);
					float single7 = single5 + single6;
					this.mIgnoreCallbacks = true;
					this.verticalScrollBar.barSize = 1f - single7;
					this.verticalScrollBar.scrollValue = (single7 <= 0.001f ? 0f : 1f - single5 / single7);
					this.mIgnoreCallbacks = false;
				}
			}
		}
		else if (recalculateBounds)
		{
			this.mCalculatedBounds = false;
			this._panelMayNeedBoundCalculation = false;
		}
	}

	public event UIDraggablePanel.CalculatedNextChangeCallback onNextChangeCallback
	{
		add
		{
			this.calculatedNextChangeCallback += value;
		}
		remove
		{
			this.calculatedNextChangeCallback -= value;
		}
	}

	public delegate void CalculatedNextChangeCallback();

	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public enum ShowCondition
	{
		Always,
		OnlyIfNeeded,
		WhenDragging
	}
}