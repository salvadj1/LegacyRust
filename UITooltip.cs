using System;
using UnityEngine;

[AddComponentMenu("Game/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	private static UITooltip mInstance;

	public Camera uiCamera;

	public UILabel text;

	public UISlicedSprite background;

	public float appearSpeed = 10f;

	public bool scalingTransitions = true;

	private Transform mTrans;

	private float mTarget;

	private float mCurrent;

	private Vector3 mPos;

	private Vector3 mSize;

	private UIWidget[] mWidgets;

	public UITooltip()
	{
	}

	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	private void SetAlpha(float val)
	{
		int num = 0;
		int length = (int)this.mWidgets.Length;
		while (num < length)
		{
			UIWidget uIWidget = this.mWidgets[num];
			Color color = uIWidget.color;
			color.a = val;
			uIWidget.color = color;
			num++;
		}
	}

	private void SetText(string tooltipText)
	{
		if (!(this.text != null) || string.IsNullOrEmpty(tooltipText))
		{
			this.mTarget = 0f;
		}
		else
		{
			this.mTarget = 1f;
			if (this.text != null)
			{
				this.text.text = tooltipText;
			}
			this.mPos = Input.mousePosition;
			if (this.background != null)
			{
				Transform transforms = this.background.transform;
				Transform transforms1 = this.text.transform;
				Vector3 vector3 = transforms1.localPosition;
				Vector3 vector31 = transforms1.localScale;
				this.mSize = this.text.relativeSize;
				this.mSize.x = this.mSize.x * vector31.x;
				this.mSize.y = this.mSize.y * vector31.y;
				float single = this.mSize.x;
				float single1 = this.background.border.x + this.background.border.z;
				float single2 = vector3.x;
				Vector4 vector4 = this.background.border;
				this.mSize.x = single + (single1 + (single2 - vector4.x) * 2f);
				float single3 = this.mSize.y;
				float single4 = this.background.border.y + this.background.border.w;
				float single5 = -vector3.y;
				Vector4 vector41 = this.background.border;
				this.mSize.y = single3 + (single4 + (single5 - vector41.y) * 2f);
				this.mSize.z = 1f;
				transforms.localScale = this.mSize;
			}
			if (this.uiCamera == null)
			{
				if (this.mPos.x + this.mSize.x > (float)Screen.width)
				{
					this.mPos.x = (float)Screen.width - this.mSize.x;
				}
				if (this.mPos.y - this.mSize.y < 0f)
				{
					this.mPos.y = this.mSize.y;
				}
				this.mPos.x = this.mPos.x - (float)Screen.width * 0.5f;
				this.mPos.y = this.mPos.y - (float)Screen.height * 0.5f;
			}
			else
			{
				this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
				this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
				float single6 = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
				float single7 = (float)Screen.height * 0.5f / single6;
				Vector2 vector2 = new Vector2(single7 * this.mSize.x / (float)Screen.width, single7 * this.mSize.y / (float)Screen.height);
				this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector2.x);
				this.mPos.y = Mathf.Max(this.mPos.y, vector2.y);
				this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
				this.mPos = this.mTrans.localPosition;
				this.mPos.x = Mathf.Round(this.mPos.x);
				this.mPos.y = Mathf.Round(this.mPos.y);
				this.mTrans.localPosition = this.mPos;
			}
		}
	}

	public static void ShowText(string tooltipText)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(tooltipText);
		}
	}

	private void Start()
	{
		this.mTrans = base.transform;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.localPosition;
		this.mSize = this.mTrans.localScale;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.SetAlpha(0f);
	}

	private void Update()
	{
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, Time.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 vector3 = this.mSize * 0.25f;
				vector3.y = -vector3.y;
				Vector3 vector31 = Vector3.one * (1.5f - this.mCurrent * 0.5f);
				Vector3 vector32 = Vector3.Lerp(this.mPos - vector3, this.mPos, this.mCurrent);
				this.mTrans.localPosition = vector32;
				this.mTrans.localScale = vector31;
			}
		}
	}
}