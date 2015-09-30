using System;
using UnityEngine;

public class RPOSInfoWindow : RPOSWindowScrollable
{
	public GameObject addParent;

	public GameObject itemTitlePrefab;

	public GameObject sectionTitlePrefab;

	public GameObject itemDescriptionPrefab;

	public GameObject basicLabelPrefab;

	public GameObject progressStatPrefab;

	public RPOSInfoWindow()
	{
		this.neverAutoShow = true;
	}

	public GameObject AddBasicLabel(string text, float aboveSpace)
	{
		float contentHeight = this.GetContentHeight();
		GameObject gameObject = NGUITools.AddChild(this.addParent, this.basicLabelPrefab);
		gameObject.GetComponentInChildren<UILabel>().text = text;
		gameObject.transform.SetLocalPositionY(-(contentHeight + aboveSpace));
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

	public GameObject AddItemTitle(ItemDataBlock item)
	{
		return this.AddItemTitle(item, 0f);
	}

	public GameObject AddItemTitle(ItemDataBlock item, float aboveSpace)
	{
		float contentHeight = this.GetContentHeight();
		GameObject gameObject = NGUITools.AddChild(this.addParent, this.itemTitlePrefab);
		gameObject.GetComponentInChildren<UILabel>().text = item.name;
		UITexture componentInChildren = gameObject.GetComponentInChildren<UITexture>();
		componentInChildren.material = componentInChildren.material.Clone();
		componentInChildren.material.Set("_MainTex", item.GetIconTexture());
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

	public void FinishPopulating()
	{
		base.ResetScrolling();
		base.showWithRPOS = this.autoShowWithRPOS;
		base.showWithoutRPOS = this.autoShowWithoutRPOS;
	}

	public float GetContentHeight()
	{
		AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(this.addParent.transform, this.addParent.transform);
		return aABBox.size.y;
	}

	protected override void OnWindowHide()
	{
		base.OnWindowHide();
		this.SetVisible(false);
	}

	protected override void OnWindowShow()
	{
		base.OnWindowShow();
		this.SetVisible(true);
	}

	private void SetVisible(bool enable)
	{
		Debug.Log("Info RPOS opened");
		base.mainPanel.enabled = enable;
		UIPanel[] componentsInChildren = base.GetComponentsInChildren<UIPanel>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = enable;
		}
	}
}