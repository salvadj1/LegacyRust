using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Slider")]
[ExecuteInEditMode]
public class UISlider : IgnoreTimeScale
{
	public static UISlider current;

	public Transform foreground;

	public Transform thumb;

	public UISlider.Direction direction;

	public Vector2 fullSize = Vector2.zero;

	public GameObject eventReceiver;

	public string functionName = "OnSliderChange";

	public int numberOfSteps;

	[HideInInspector]
	[SerializeField]
	private float rawValue = 1f;

	private float mStepValue = 1f;

	private BoxCollider mCol;

	private Transform mTrans;

	private Transform mFGTrans;

	private UIWidget mFGWidget;

	private UIFilledSprite mFGFilled;

	private bool mInitDone;

	public float sliderValue
	{
		get
		{
			return this.mStepValue;
		}
		set
		{
			this.Set(value, false);
		}
	}

	public UISlider()
	{
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		this.mCol = base.collider as BoxCollider;
	}

	public void ForceUpdate()
	{
		this.Set(this.rawValue, true);
	}

	private void Init()
	{
		UIFilledSprite uIFilledSprite;
		this.mInitDone = true;
		if (this.foreground != null)
		{
			this.mFGWidget = this.foreground.GetComponent<UIWidget>();
			if (this.mFGWidget == null)
			{
				uIFilledSprite = null;
			}
			else
			{
				uIFilledSprite = this.mFGWidget as UIFilledSprite;
			}
			this.mFGFilled = uIFilledSprite;
			this.mFGTrans = this.foreground.transform;
			if (this.fullSize == Vector2.zero)
			{
				this.fullSize = this.foreground.localScale;
			}
		}
		else if (this.mCol == null)
		{
			Debug.LogWarning("UISlider expected to find a foreground object or a box collider to work with", this);
		}
		else if (this.fullSize == Vector2.zero)
		{
			this.fullSize = this.mCol.size;
		}
	}

	private void OnDrag(Vector2 delta)
	{
		this.UpdateDrag();
	}

	private void OnDragThumb(GameObject go, Vector2 delta)
	{
		this.UpdateDrag();
	}

	private void OnKey(KeyCode key)
	{
		float single = ((float)this.numberOfSteps <= 1f ? 0.125f : 1f / (float)(this.numberOfSteps - 1));
		if (this.direction == UISlider.Direction.Horizontal)
		{
			if (key == KeyCode.LeftArrow)
			{
				this.Set(this.rawValue - single, false);
			}
			else if (key == KeyCode.RightArrow)
			{
				this.Set(this.rawValue + single, false);
			}
		}
		else if (key == KeyCode.DownArrow)
		{
			this.Set(this.rawValue - single, false);
		}
		else if (key == KeyCode.UpArrow)
		{
			this.Set(this.rawValue + single, false);
		}
	}

	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			this.UpdateDrag();
		}
	}

	private void OnPressThumb(GameObject go, bool pressed)
	{
		if (pressed)
		{
			this.UpdateDrag();
		}
	}

	private void Set(float input, bool force)
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		float single = Mathf.Clamp01(input);
		if (single < 0.001f)
		{
			single = 0f;
		}
		this.rawValue = single;
		if (this.numberOfSteps > 1)
		{
			single = Mathf.Round(single * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
		}
		if (force || this.mStepValue != single)
		{
			this.mStepValue = single;
			Vector3 vector3 = this.fullSize;
			if (this.direction != UISlider.Direction.Horizontal)
			{
				vector3.y = vector3.y * this.mStepValue;
			}
			else
			{
				vector3.x = vector3.x * this.mStepValue;
			}
			if (this.mFGFilled != null)
			{
				this.mFGFilled.fillAmount = this.mStepValue;
			}
			else if (this.foreground != null)
			{
				this.mFGTrans.localScale = vector3;
				if (this.mFGWidget != null)
				{
					if (single <= 0.001f)
					{
						this.mFGWidget.enabled = false;
					}
					else
					{
						this.mFGWidget.enabled = true;
						this.mFGWidget.MarkAsChanged();
					}
				}
			}
			if (this.thumb != null)
			{
				Vector3 vector31 = this.thumb.localPosition;
				if (this.mFGFilled == null)
				{
					if (this.direction != UISlider.Direction.Horizontal)
					{
						vector31.y = vector3.y;
					}
					else
					{
						vector31.x = vector3.x;
					}
				}
				else if (this.mFGFilled.fillDirection == UIFilledSprite.FillDirection.Horizontal)
				{
					vector31.x = (!this.mFGFilled.invert ? vector3.x : this.fullSize.x - vector3.x);
				}
				else if (this.mFGFilled.fillDirection == UIFilledSprite.FillDirection.Vertical)
				{
					vector31.y = (!this.mFGFilled.invert ? vector3.y : this.fullSize.y - vector3.y);
				}
				this.thumb.localPosition = vector31;
			}
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName) && Application.isPlaying)
			{
				UISlider.current = this;
				this.eventReceiver.SendMessage(this.functionName, this.mStepValue, SendMessageOptions.DontRequireReceiver);
				UISlider.current = null;
			}
		}
	}

	private void Start()
	{
		this.Init();
		if (Application.isPlaying && this.thumb != null && NGUITools.HasMeansOfClicking(this.thumb))
		{
			UIEventListener uIEventListener = UIEventListener.Get(this.thumb.gameObject);
			uIEventListener.onPress += new UIEventListener.BoolDelegate(this.OnPressThumb);
			uIEventListener.onDrag += new UIEventListener.VectorDelegate(this.OnDragThumb);
		}
		this.Set(this.rawValue, true);
	}

	private void UpdateDrag()
	{
		float single;
		if (this.mCol == null || UICamera.currentCamera == null || !UICamera.IsPressing)
		{
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
		Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		Plane plane = new Plane(this.mTrans.rotation * Vector3.back, this.mTrans.position);
		if (!plane.Raycast(ray, out single))
		{
			return;
		}
		Vector3 vector3 = (this.mTrans.localPosition + this.mCol.center) - (this.mCol.size * 0.5f);
		Vector3 vector31 = this.mTrans.localPosition - vector3;
		Vector3 vector32 = this.mTrans.InverseTransformPoint(ray.GetPoint(single));
		Vector3 vector33 = vector32 + vector31;
		this.Set((this.direction != UISlider.Direction.Horizontal ? vector33.y / this.mCol.size.y : vector33.x / this.mCol.size.x), false);
	}

	public enum Direction
	{
		Horizontal,
		Vertical
	}
}