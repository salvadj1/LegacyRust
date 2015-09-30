using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Popup List")]
[ExecuteInEditMode]
public class UIPopupList : MonoBehaviour
{
	private const float animSpeed = 0.15f;

	public UIAtlas atlas;

	public UIFont font;

	public UILabel textLabel;

	public string backgroundSprite;

	public string highlightSprite;

	public UIPopupList.Position position;

	public List<string> items = new List<string>();

	public Vector2 padding = new Vector3(4f, 4f);

	public float textScale = 1f;

	public Color textColor = Color.white;

	public Color backgroundColor = Color.white;

	public Color highlightColor = new Color(0.596078455f, 1f, 0.2f, 1f);

	public bool isAnimated = true;

	public bool isLocalized;

	public GameObject eventReceiver;

	public string functionName = "OnSelectionChange";

	[HideInInspector]
	[SerializeField]
	private string mSelectedItem;

	private UIPanel mPanel;

	private GameObject mChild;

	private UISprite mBackground;

	private UISprite mHighlight;

	private UILabel mHighlightedLabel;

	private List<UILabel> mLabelList = new List<UILabel>();

	private float mBgBorder;

	private bool handleEvents
	{
		get
		{
			UIButtonKeys component = base.GetComponent<UIButtonKeys>();
			return (component == null ? true : !component.enabled);
		}
		set
		{
			UIButtonKeys component = base.GetComponent<UIButtonKeys>();
			if (component != null)
			{
				component.enabled = !value;
			}
		}
	}

	public bool isOpen
	{
		get
		{
			return this.mChild != null;
		}
	}

	public string selection
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			if (this.mSelectedItem != value)
			{
				this.mSelectedItem = value;
				if (this.textLabel != null)
				{
					this.textLabel.text = (!this.isLocalized || !(Localization.instance != null) ? value : Localization.instance.Get(value));
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName) && Application.isPlaying)
				{
					this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public UIPopupList()
	{
	}

	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	private void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 vector3;
		Vector3 vector31 = widget.cachedTransform.localPosition;
		vector3 = (!placeAbove ? new Vector3(vector31.x, 0f, vector31.z) : new Vector3(vector31.x, bottom, vector31.z));
		widget.cachedTransform.localPosition = vector3;
		GameObject gameObject = widget.gameObject;
		TweenPosition.Begin(gameObject, 0.15f, vector31).method = UITweener.Method.EaseOut;
	}

	private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.gameObject;
		Transform vector3 = widget.cachedTransform;
		float single = (float)this.font.size * this.textScale + this.mBgBorder * 2f;
		Vector3 vector31 = vector3.localScale;
		vector3.localScale = new Vector3(vector31.x, single, vector31.z);
		TweenScale.Begin(gameObject, 0.15f, vector31).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 vector32 = vector3.localPosition;
			vector3.localPosition = new Vector3(vector32.x, vector32.y - vector31.y + single, vector32.z);
			TweenPosition.Begin(gameObject, 0.15f, vector32).method = UITweener.Method.EaseOut;
		}
	}

	private void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			TweenPosition component = lbl.GetComponent<TweenPosition>();
			if (component != null && component.enabled)
			{
				return;
			}
			this.mHighlightedLabel = lbl;
			UIAtlas.Sprite sprite = this.mHighlight.sprite;
			float single = sprite.inner.xMin - sprite.outer.xMin;
			float single1 = sprite.inner.yMin - sprite.outer.yMin;
			Vector3 vector3 = lbl.cachedTransform.localPosition + new Vector3(-single, single1, 0f);
			if (instant || !this.isAnimated)
			{
				this.mHighlight.cachedTransform.localPosition = vector3;
			}
			else
			{
				TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, vector3).method = UITweener.Method.EaseOut;
			}
		}
	}

	private void OnClick()
	{
		if (!(this.mChild == null) || !(this.atlas != null) || !(this.font != null) || this.items.Count <= 1)
		{
			this.OnSelect(false);
		}
		else
		{
			this.mLabelList.Clear();
			this.handleEvents = true;
			if (this.mPanel == null)
			{
				this.mPanel = UIPanel.Find(base.transform, true);
			}
			Transform transforms = base.transform;
			AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(transforms.parent, transforms);
			this.mChild = new GameObject("Drop-down List")
			{
				layer = base.gameObject.layer
			};
			Transform vector3 = this.mChild.transform;
			vector3.parent = transforms.parent;
			vector3.localPosition = aABBox.min;
			vector3.localRotation = Quaternion.identity;
			vector3.localScale = Vector3.one;
			this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
			this.mBackground.pivot = UIWidget.Pivot.TopLeft;
			this.mBackground.depth = NGUITools.CalculateNextDepth(this.mPanel.gameObject);
			this.mBackground.color = this.backgroundColor;
			Vector4 vector4 = this.mBackground.border;
			this.mBgBorder = vector4.y;
			this.mBackground.cachedTransform.localPosition = new Vector3(0f, vector4.y, 0f);
			this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
			this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
			this.mHighlight.color = this.highlightColor;
			UIAtlas.Sprite sprite = this.mHighlight.sprite;
			float single = sprite.inner.yMin - sprite.outer.yMin;
			float single1 = (float)this.font.size * this.textScale;
			float single2 = 0f;
			float single3 = -this.padding.y;
			List<UILabel> uILabels = new List<UILabel>();
			int num = 0;
			int count = this.items.Count;
			while (num < count)
			{
				string item = this.items[num];
				UILabel uILabel = NGUITools.AddWidget<UILabel>(this.mChild);
				uILabel.pivot = UIWidget.Pivot.TopLeft;
				uILabel.font = this.font;
				uILabel.text = (!this.isLocalized || !(Localization.instance != null) ? item : Localization.instance.Get(item));
				uILabel.color = this.textColor;
				uILabel.cachedTransform.localPosition = new Vector3(vector4.x, single3, 0f);
				uILabel.MakePixelPerfect();
				if (this.textScale != 1f)
				{
					Vector3 vector31 = uILabel.cachedTransform.localScale;
					uILabel.cachedTransform.localScale = vector31 * this.textScale;
				}
				uILabels.Add(uILabel);
				single3 = single3 - single1;
				single3 = single3 - this.padding.y;
				Vector2 vector2 = uILabel.relativeSize;
				single2 = Mathf.Max(single2, vector2.x * single1);
				UIEventListener boolDelegate = UIEventListener.Get(uILabel.gameObject);
				boolDelegate.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
				boolDelegate.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
				boolDelegate.parameter = item;
				if (this.mSelectedItem == item)
				{
					this.Highlight(uILabel, true);
				}
				this.mLabelList.Add(uILabel);
				num++;
			}
			Vector3 vector32 = aABBox.size;
			single2 = Mathf.Max(single2, vector32.x - vector4.x * 2f);
			Vector3 vector33 = new Vector3(single2 * 0.5f / single1, -0.5f, 0f);
			Vector3 vector34 = new Vector3(single2 / single1, (single1 + this.padding.y) / single1, 1f);
			int num1 = 0;
			int count1 = uILabels.Count;
			while (num1 < count1)
			{
				UIHotSpot uIHotSpot = NGUITools.AddWidgetHotSpot(uILabels[num1].gameObject);
				vector33.z = uIHotSpot.center.z;
				uIHotSpot.center = vector33;
				uIHotSpot.size = vector34;
				num1++;
			}
			single2 = single2 + vector4.x * 2f;
			single3 = single3 - vector4.y;
			this.mBackground.cachedTransform.localScale = new Vector3(single2, -single3 + vector4.y, 1f);
			this.mHighlight.cachedTransform.localScale = new Vector3(single2 - vector4.x * 2f + (sprite.inner.xMin - sprite.outer.xMin) * 2f, single1 + single * 2f, 1f);
			bool flag = this.position == UIPopupList.Position.Above;
			if (this.position == UIPopupList.Position.Auto)
			{
				UICamera uICamera = UICamera.FindCameraForLayer(base.gameObject.layer);
				if (uICamera != null)
				{
					Vector3 viewportPoint = uICamera.cachedCamera.WorldToViewportPoint(transforms.position);
					flag = viewportPoint.y < 0.5f;
				}
			}
			if (this.isAnimated)
			{
				float single4 = single3 + single1;
				this.Animate(this.mHighlight, flag, single4);
				int num2 = 0;
				int count2 = uILabels.Count;
				while (num2 < count2)
				{
					this.Animate(uILabels[num2], flag, single4);
					num2++;
				}
				this.AnimateColor(this.mBackground);
				this.AnimateScale(this.mBackground, flag, single4);
			}
			if (flag)
			{
				float single5 = aABBox.min.x;
				Vector3 vector35 = aABBox.max;
				float single6 = vector35.y - single3 - vector4.y;
				Vector3 vector36 = aABBox.min;
				vector3.localPosition = new Vector3(single5, single6, vector36.z);
			}
		}
	}

	private void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			this.Highlight(go.GetComponent<UILabel>(), false);
		}
	}

	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
		}
	}

	private void OnKey(KeyCode key)
	{
		if (base.enabled && base.gameObject.activeInHierarchy && this.handleEvents)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (key == KeyCode.UpArrow)
			{
				if (num > 0)
				{
					int num1 = num - 1;
					num = num1;
					this.Select(this.mLabelList[num1], false);
				}
			}
			else if (key == KeyCode.DownArrow)
			{
				if (num + 1 < this.mLabelList.Count)
				{
					int num2 = num + 1;
					num = num2;
					this.Select(this.mLabelList[num2], false);
				}
			}
			else if (key == KeyCode.Escape)
			{
				this.OnSelect(false);
			}
		}
	}

	private void OnLocalize(Localization loc)
	{
		if (this.isLocalized && this.textLabel != null)
		{
			this.textLabel.text = loc.Get(this.mSelectedItem);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (!isSelected && this.mChild != null)
		{
			this.mLabelList.Clear();
			this.handleEvents = false;
			if (!this.isAnimated)
			{
				UnityEngine.Object.Destroy(this.mChild);
			}
			else
			{
				UIWidget[] componentsInChildren = this.mChild.GetComponentsInChildren<UIWidget>();
				int num = 0;
				int length = (int)componentsInChildren.Length;
				while (num < length)
				{
					UIWidget uIWidget = componentsInChildren[num];
					Color color = uIWidget.color;
					color.a = 0f;
					TweenColor.Begin(uIWidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
					num++;
				}
				NGUITools.SetAllowClickChildren(this.mChild, false);
				UpdateManager.AddDestroy(this.mChild, 0.15f);
			}
			this.mBackground = null;
			this.mHighlight = null;
			this.mChild = null;
		}
	}

	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
		this.selection = lbl.gameObject.GetComponent<UIEventListener>().parameter as string;
		UIButtonSound[] components = base.GetComponents<UIButtonSound>();
		int num = 0;
		int length = (int)components.Length;
		while (num < length)
		{
			UIButtonSound uIButtonSound = components[num];
			if (uIButtonSound.trigger == UIButtonSound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uIButtonSound.audioClip, uIButtonSound.volume, 1f);
			}
			num++;
		}
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(this.mSelectedItem))
		{
			string str = this.mSelectedItem;
			this.mSelectedItem = null;
			this.selection = str;
		}
		else if (this.items.Count > 0)
		{
			this.selection = this.items[0];
		}
	}

	public enum Position
	{
		Auto,
		Above,
		Below
	}
}