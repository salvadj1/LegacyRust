using System;
using System.Collections;
using UnityEngine;

public class ItemToolTip : MonoBehaviour
{
	public UISlicedSprite _background;

	public static ItemToolTip _globalToolTip;

	public GameObject addParent;

	public GameObject itemTitlePrefab;

	public GameObject sectionTitlePrefab;

	public GameObject itemDescriptionPrefab;

	public GameObject basicLabelPrefab;

	public GameObject progressStatPrefab;

	public Camera uiCamera;

	private Plane planeTest;

	public ItemToolTip()
	{
	}

	public GameObject AddBasicLabel(string text, float aboveSpace)
	{
		return this.AddBasicLabel(text, aboveSpace, Color.white);
	}

	public GameObject AddBasicLabel(string text, float aboveSpace, Color col)
	{
		float contentHeight = this.GetContentHeight();
		GameObject gameObject = NGUITools.AddChild(this.addParent, this.basicLabelPrefab);
		UILabel componentInChildren = gameObject.GetComponentInChildren<UILabel>();
		componentInChildren.text = text;
		componentInChildren.color = col;
		gameObject.transform.SetLocalPositionY(-(contentHeight + aboveSpace));
		return gameObject;
	}

	public GameObject AddConditionInfo(IInventoryItem item)
	{
		if (item == null || !item.datablock.DoesLoseCondition())
		{
			return null;
		}
		Color color = Color.green;
		if (item.condition <= 0.6f)
		{
			color = Color.yellow;
		}
		else if (item.IsBroken())
		{
			color = Color.red;
		}
		float single = 100f * item.condition;
		float single1 = 100f * item.maxcondition;
		GameObject gameObject = this.AddBasicLabel(string.Concat("Condition : ", single.ToString("0"), "/", single1.ToString("0")), 15f, color);
		return gameObject;
	}

	public GameObject AddItemDescription(ItemDataBlock item, float aboveSpace)
	{
		float contentHeight = this.GetContentHeight();
		GameObject itemDescription = NGUITools.AddChild(this.addParent, this.itemDescriptionPrefab);
		itemDescription.transform.FindChild("DescText").GetComponent<UILabel>().text = item.GetItemDescription();
		itemDescription.transform.SetLocalPositionY(-(contentHeight + aboveSpace));
		return null;
	}

	public GameObject AddItemTitle(ItemDataBlock itemdb, IInventoryItem itemInstance = null, float aboveSpace = 0)
	{
		float contentHeight = this.GetContentHeight();
		GameObject gameObject = NGUITools.AddChild(this.addParent, this.itemTitlePrefab);
		gameObject.GetComponentInChildren<UILabel>().text = (itemInstance == null ? itemdb.name : itemInstance.toolTip);
		UITexture componentInChildren = gameObject.GetComponentInChildren<UITexture>();
		componentInChildren.material = componentInChildren.material.Clone();
		componentInChildren.material.Set("_MainTex", itemdb.GetIconTexture());
		componentInChildren.color = (itemInstance == null || !itemInstance.IsBroken() ? Color.white : Color.red);
		gameObject.transform.SetLocalPositionY(-(contentHeight + aboveSpace));
		return gameObject;
	}

	public GameObject AddProgressStat(string text, float currentAmount, float maxAmount, float aboveSpace)
	{
		float contentHeight = this.GetContentHeight();
		GameObject gameObject = NGUITools.AddChild(this.addParent, this.progressStatPrefab);
		UISlider componentInChildren = gameObject.GetComponentInChildren<UISlider>();
		UILabel component = FindChildHelper.FindChildByName("ProgressStatTitle", gameObject.gameObject).GetComponent<UILabel>();
		UILabel uILabel = FindChildHelper.FindChildByName("ProgressAmountLabel", gameObject.gameObject).GetComponent<UILabel>();
		component.text = text;
		uILabel.text = (currentAmount >= 1f ? currentAmount.ToString("N0") : currentAmount.ToString("N2"));
		componentInChildren.sliderValue = currentAmount / maxAmount;
		gameObject.transform.SetLocalPositionY(-(contentHeight + aboveSpace));
		return gameObject;
	}

	public GameObject AddSectionTitle(string text, float aboveSpace)
	{
		float contentHeight = this.GetContentHeight();
		GameObject gameObject = NGUITools.AddChild(this.addParent, this.sectionTitlePrefab);
		gameObject.GetComponentInChildren<UILabel>().text = text;
		gameObject.transform.SetLocalPositionY(-(contentHeight + aboveSpace));
		return gameObject;
	}

	private void Awake()
	{
		ItemToolTip._globalToolTip = this;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.planeTest = new Plane(this.uiCamera.transform.forward * 1f, new Vector3(0f, 0f, 2f));
	}

	public void ClearContents()
	{
		IEnumerator enumerator = this.addParent.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UnityEngine.Object.Destroy(((Transform)enumerator.Current).gameObject);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public void FinishPopulating()
	{
	}

	public static ItemToolTip Get()
	{
		return ItemToolTip._globalToolTip;
	}

	public float GetContentHeight()
	{
		AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(this.addParent.transform, this.addParent.transform);
		return aABBox.size.y;
	}

	public void Internal_SetToolTip(ItemDataBlock itemdb, IInventoryItem item)
	{
		this.ClearContents();
		if (itemdb == null)
		{
			this.SetVisible(false);
			return;
		}
		this.RepositionAtCursor();
		itemdb.PopulateInfoWindow(this, item);
		Transform vector3 = this._background.transform;
		float contentHeight = this.GetContentHeight();
		Vector3 vector31 = this.addParent.transform.localPosition;
		vector3.localScale = new Vector3(250f, contentHeight + Mathf.Abs(vector31.y * 2f), 1f);
		this.SetVisible(true);
	}

	public void RepositionAtCursor()
	{
		Vector3 vector3 = UICamera.lastMousePosition;
		Ray ray = this.uiCamera.ScreenPointToRay(vector3);
		float single = 0f;
		if (this.planeTest.Raycast(ray, out single))
		{
			base.transform.position = ray.GetPoint(single);
			Transform transforms = base.transform;
			float single1 = base.transform.localPosition.x;
			Vector3 vector31 = base.transform.localPosition;
			transforms.localPosition = new Vector3(single1, vector31.y, -180f);
			AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(base.transform);
			float single2 = base.transform.localPosition.x;
			Vector3 vector32 = aABBox.size;
			float single3 = single2 + vector32.x - (float)Screen.width;
			if (single3 > 0f)
			{
				Transform transforms1 = base.transform;
				Vector3 vector33 = base.transform.localPosition;
				transforms1.SetLocalPositionX(vector33.x - single3);
			}
			float single4 = base.transform.localPosition.y;
			Vector3 vector34 = aABBox.size;
			float single5 = Mathf.Abs(single4 - vector34.y) - (float)Screen.height;
			if (single5 > 0f)
			{
				Transform transforms2 = base.transform;
				Vector3 vector35 = base.transform.localPosition;
				transforms2.SetLocalPositionY(vector35.y + single5);
			}
		}
	}

	public static void SetToolTip(ItemDataBlock itemdb, IInventoryItem item = null)
	{
		ItemToolTip.Get().Internal_SetToolTip(itemdb, item);
		ItemToolTip.Get().RepositionAtCursor();
	}

	public void SetVisible(bool vis)
	{
		base.GetComponent<UIPanel>().enabled = vis;
	}

	private void Update()
	{
	}
}