using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Scroll Bar")]
[ExecuteInEditMode]
public class UIScrollBar : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private UISprite mBG;

	[HideInInspector]
	[SerializeField]
	private UISprite mFG;

	[HideInInspector]
	[SerializeField]
	private UIScrollBar.Direction mDir;

	[HideInInspector]
	[SerializeField]
	private bool mInverted;

	[HideInInspector]
	[SerializeField]
	private float mScroll;

	[HideInInspector]
	[SerializeField]
	private float mSize = 1f;

	private Transform mTrans;

	private bool mIsDirty;

	private Camera mCam;

	private Vector2 mScreenPos = Vector2.zero;

	public UIScrollBar.OnScrollBarChange onChange;

	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG == null)
			{
				return 0f;
			}
			return this.mBG.alpha;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				this.mFG.gameObject.SetActive(!NGUITools.ZeroAlpha(this.mFG.alpha));
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				this.mBG.gameObject.SetActive(!NGUITools.ZeroAlpha(this.mFG.alpha));
			}
		}
	}

	public UISprite background
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	public float barSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			float single = Mathf.Clamp01(value);
			if (this.mSize != single)
			{
				this.mSize = single;
				this.mIsDirty = true;
				if (this.onChange != null)
				{
					this.onChange(this);
				}
			}
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return this.mCam;
		}
	}

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public UIScrollBar.Direction direction
	{
		get
		{
			return this.mDir;
		}
		set
		{
			if (this.mDir != value)
			{
				this.mDir = value;
				this.mIsDirty = true;
				if (this.mBG != null)
				{
					Transform transforms = this.mBG.cachedTransform;
					Vector3 vector3 = transforms.localScale;
					if (this.mDir == UIScrollBar.Direction.Vertical && vector3.x > vector3.y || this.mDir == UIScrollBar.Direction.Horizontal && vector3.x < vector3.y)
					{
						float single = vector3.x;
						vector3.x = vector3.y;
						vector3.y = single;
						transforms.localScale = vector3;
						this.ForceUpdate();
						if (this.mBG.collider != null)
						{
							NGUITools.AddWidgetHotSpot(this.mBG.gameObject);
						}
						if (this.mFG.collider != null)
						{
							NGUITools.AddWidgetHotSpot(this.mFG.gameObject);
						}
					}
				}
			}
		}
	}

	public UISprite foreground
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	public bool inverted
	{
		get
		{
			return this.mInverted;
		}
		set
		{
			if (this.mInverted != value)
			{
				this.mInverted = value;
				this.mIsDirty = true;
			}
		}
	}

	public float scrollValue
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			float single = Mathf.Clamp01(value);
			if (this.mScroll != single)
			{
				this.mScroll = single;
				this.mIsDirty = true;
				if (this.onChange != null)
				{
					this.onChange(this);
				}
			}
		}
	}

	public UIScrollBar()
	{
	}

	private void CenterOnPos(Vector2 localPos)
	{
		if (this.mBG == null || this.mFG == null)
		{
			return;
		}
		AABBox aABBox = NGUIMath.CalculateRelativeInnerBounds(this.cachedTransform, this.mBG);
		AABBox aABBox1 = NGUIMath.CalculateRelativeInnerBounds(this.cachedTransform, this.mFG);
		if (this.mDir != UIScrollBar.Direction.Horizontal)
		{
			float single = aABBox.size.y - aABBox1.size.y;
			float single1 = aABBox.center.y - single * 0.5f;
			float single2 = (single <= 0f ? 0f : 1f - (localPos.y - single1) / single);
			this.scrollValue = (!this.mInverted ? single2 : 1f - single2);
		}
		else
		{
			float single3 = aABBox.size.x - aABBox1.size.x;
			float single4 = aABBox.center.x - single3 * 0.5f;
			float single5 = (single3 <= 0f ? 0f : (localPos.x - single4) / single3);
			this.scrollValue = (!this.mInverted ? single5 : 1f - single5);
		}
	}

	public void ForceUpdate()
	{
		this.mIsDirty = false;
		if (this.mBG != null && this.mFG != null)
		{
			this.mSize = Mathf.Clamp01(this.mSize);
			this.mScroll = Mathf.Clamp01(this.mScroll);
			Vector4 vector4 = this.mBG.border;
			Vector4 vector41 = this.mFG.border;
			Vector3 vector3 = this.mBG.cachedTransform.localScale;
			float single = Mathf.Max(0f, vector3.x - vector4.x - vector4.z);
			Vector3 vector31 = this.mBG.cachedTransform.localScale;
			Vector2 vector2 = new Vector2(single, Mathf.Max(0f, vector31.y - vector4.y - vector4.w));
			float single1 = (!this.mInverted ? this.mScroll : 1f - this.mScroll);
			if (this.mDir != UIScrollBar.Direction.Horizontal)
			{
				Vector2 vector21 = new Vector2(vector2.x, vector2.y * this.mSize);
				this.mFG.pivot = UIWidget.Pivot.Top;
				this.mBG.pivot = UIWidget.Pivot.Top;
				this.mBG.cachedTransform.localPosition = Vector3.zero;
				this.mFG.cachedTransform.localPosition = new Vector3(0f, -vector4.y + vector41.y - (vector2.y - vector21.y) * single1, 0f);
				this.mFG.cachedTransform.localScale = new Vector3(vector21.x + vector41.x + vector41.z, vector21.y + vector41.y + vector41.w, 1f);
				if (single1 < 0.999f && single1 > 0.001f)
				{
					this.mFG.MakePixelPerfect();
				}
			}
			else
			{
				Vector2 vector22 = new Vector2(vector2.x * this.mSize, vector2.y);
				this.mFG.pivot = UIWidget.Pivot.Left;
				this.mBG.pivot = UIWidget.Pivot.Left;
				this.mBG.cachedTransform.localPosition = Vector3.zero;
				this.mFG.cachedTransform.localPosition = new Vector3(vector4.x - vector41.x + (vector2.x - vector22.x) * single1, 0f, 0f);
				this.mFG.cachedTransform.localScale = new Vector3(vector22.x + vector41.x + vector41.z, vector22.y + vector41.y + vector41.w, 1f);
				if (single1 < 0.999f && single1 > 0.001f)
				{
					this.mFG.MakePixelPerfect();
				}
			}
		}
	}

	private void OnDragBackground(GameObject go, Vector2 delta)
	{
		this.mCam = UICamera.currentCamera;
		this.Reposition(UICamera.lastTouchPosition);
	}

	private void OnDragForeground(GameObject go, Vector2 delta)
	{
		this.mCam = UICamera.currentCamera;
		this.Reposition(this.mScreenPos + UICamera.currentTouch.totalDelta);
	}

	private void OnPressBackground(GameObject go, bool isPressed)
	{
		this.mCam = UICamera.currentCamera;
		this.Reposition(UICamera.lastTouchPosition);
	}

	private void OnPressForeground(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.mCam = UICamera.currentCamera;
			AABBox aABBox = NGUIMath.CalculateAbsoluteWidgetBounds(this.mFG.cachedTransform);
			this.mScreenPos = this.mCam.WorldToScreenPoint(aABBox.center);
		}
	}

	private void Reposition(Vector2 screenPos)
	{
		float single;
		Transform transforms = this.cachedTransform;
		Plane plane = new Plane(transforms.rotation * Vector3.back, transforms.position);
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		if (!plane.Raycast(ray, out single))
		{
			return;
		}
		this.CenterOnPos(transforms.InverseTransformPoint(ray.GetPoint(single)));
	}

	private void Start()
	{
		if (this.background != null && NGUITools.HasMeansOfClicking(this.background))
		{
			UIEventListener uIEventListener = UIEventListener.Get(this.background.gameObject);
			uIEventListener.onPress += new UIEventListener.BoolDelegate(this.OnPressBackground);
			uIEventListener.onDrag += new UIEventListener.VectorDelegate(this.OnDragBackground);
		}
		if (this.foreground != null && NGUITools.HasMeansOfClicking(this.foreground))
		{
			UIEventListener uIEventListener1 = UIEventListener.Get(this.foreground.gameObject);
			uIEventListener1.onPress += new UIEventListener.BoolDelegate(this.OnPressForeground);
			uIEventListener1.onDrag += new UIEventListener.VectorDelegate(this.OnDragForeground);
		}
		this.ForceUpdate();
	}

	private void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	public enum Direction
	{
		Horizontal,
		Vertical
	}

	public delegate void OnScrollBarChange(UIScrollBar sb);
}