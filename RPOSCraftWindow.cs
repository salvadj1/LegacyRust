using System;
using System.Collections;
using UnityEngine;

public class RPOSCraftWindow : RPOSWindowScrollable
{
	public GameObject ingredientAnchor;

	public GameObject ingredientPlaquePrefab;

	public BlueprintDataBlock selectedItem;

	public UIButton craftButton;

	public RPOS_Craft_BlueprintList bpLister;

	public UISlider craftProgressBar;

	public UILabel amountInput;

	public UISprite amountInputBackground;

	public UIButton plusButton;

	public UIButton minusButton;

	public UILabel progressLabel;

	public UILabel requirementLabel;

	public int desiredAmount = 1;

	private bool wasCrafting;

	public AudioClip craftSound;

	[NonSerialized]
	private float _lastTimeStringValue = Single.PositiveInfinity;

	[NonSerialized]
	private string _lastTimeStringString;

	private static int amountModifier
	{
		get
		{
			int num;
			try
			{
				Event @event = Event.current;
				if (@event.control)
				{
					num = 32767;
				}
				else if (!@event.shift)
				{
					return 1;
				}
				else
				{
					num = 10;
				}
			}
			catch
			{
				return 1;
			}
			return num;
		}
	}

	public RPOSCraftWindow()
	{
	}

	public bool AtWorkbench()
	{
		return RPOS.ObservedPlayer.GetComponent<CraftingInventory>().AtWorkBench();
	}

	public new void Awake()
	{
		this.ShowCraftingOptions(false);
		UIEventListener.Get(this.craftButton.gameObject).onClick += new UIEventListener.VoidDelegate(this.CraftButtonClicked);
		UIEventListener.Get(this.plusButton.gameObject).onClick += new UIEventListener.VoidDelegate(this.PlusButtonClicked);
		UIEventListener.Get(this.minusButton.gameObject).onClick += new UIEventListener.VoidDelegate(this.MinusButtonClicked);
		this.amountInput.text = "1";
	}

	public void CraftButtonClicked(GameObject go)
	{
		if (this.selectedItem == null)
		{
			return;
		}
		Debug.Log("Crafting clicked");
		CraftingInventory component = RPOS.ObservedPlayer.GetComponent<CraftingInventory>();
		if (component == null)
		{
			Debug.Log("No local player inventory.. weird");
			return;
		}
		if (component.isCrafting)
		{
			component.CancelCrafting();
		}
		else if (component.ValidateCraftRequirements(this.selectedItem))
		{
			component.StartCrafting(this.selectedItem, this.RequestedAmount());
		}
	}

	public void ItemClicked(GameObject go)
	{
		if (RPOS.ObservedPlayer.GetComponent<CraftingInventory>().isCrafting)
		{
			return;
		}
		RPOSCraftItemEntry component = go.GetComponent<RPOSCraftItemEntry>();
		if (component == null)
		{
			return;
		}
		BlueprintDataBlock blueprintDataBlock = component.blueprint;
		if (!blueprintDataBlock)
		{
			Debug.Log("no bp by that name");
			return;
		}
		if (blueprintDataBlock != this.selectedItem)
		{
			this.SetSelectedItem(component.blueprint);
			this.UpdateIngredients();
		}
	}

	public void ItemHovered(GameObject go, bool what)
	{
	}

	public void LocalInventoryModified()
	{
		this.bpLister.UpdateItems();
		this.UpdateIngredients();
	}

	public void MinusButtonClicked(GameObject go)
	{
		this.PlusMinusClick(-RPOSCraftWindow.amountModifier);
	}

	protected override void OnWindowHide()
	{
		base.OnWindowHide();
	}

	protected override void OnWindowShow()
	{
		this.bpLister.UpdateItems();
		this.SetRequestedAmount(1);
		base.OnWindowShow();
	}

	public void PlusButtonClicked(GameObject go)
	{
		this.PlusMinusClick(RPOSCraftWindow.amountModifier);
	}

	public void PlusMinusClick(int amount)
	{
		if (amount == 0)
		{
			return;
		}
		CraftingInventory component = RPOS.ObservedPlayer.GetComponent<CraftingInventory>();
		if (component == null)
		{
			return;
		}
		if (component.isCrafting)
		{
			return;
		}
		this.SetRequestedAmount(this.desiredAmount + amount);
		this.UpdateIngredients();
	}

	public int RequestedAmount()
	{
		return this.desiredAmount;
	}

	public void SetRequestedAmount(int amount)
	{
		if (this.selectedItem)
		{
			int num = this.selectedItem.MaxAmount(RPOS.ObservedPlayer.GetComponent<Inventory>());
			this.desiredAmount = Mathf.Clamp(amount, 1, (num > 0 ? num : 1));
		}
		else
		{
			this.desiredAmount = amount;
		}
		this.amountInput.text = this.desiredAmount.ToString();
	}

	public void SetSelectedItem(BlueprintDataBlock newSel)
	{
		!this.selectedItem;
		this.selectedItem = newSel;
		this.SetRequestedAmount(1);
		!this.selectedItem;
		this.ShowCraftingOptions(this.selectedItem != null);
		this.UpdateWorkbenchRequirements();
	}

	public void ShowCraftingOptions(bool show)
	{
		this.plusButton.gameObject.SetActive(show);
		this.minusButton.gameObject.SetActive(show);
		this.amountInput.gameObject.SetActive(show);
		this.amountInputBackground.gameObject.SetActive(show);
		this.craftProgressBar.gameObject.SetActive(show);
		this.craftButton.gameObject.SetActive(show);
		this.requirementLabel.gameObject.SetActive(show);
	}

	public void Update()
	{
		if (RPOS.ObservedPlayer == null)
		{
			return;
		}
		CraftingInventory component = RPOS.ObservedPlayer.GetComponent<CraftingInventory>();
		if (component == null)
		{
			return;
		}
		bool flag = component.isCrafting;
		if (flag)
		{
			component.CraftThink();
		}
		if (!flag && this.wasCrafting)
		{
			this.UpdateIngredients();
		}
		else if (!this.wasCrafting && flag)
		{
			this.craftSound.Play();
		}
		if (this.craftButton.gameObject.activeSelf)
		{
			this.craftButton.GetComponentInChildren<UILabel>().text = (!component.isCrafting ? "Craft" : "Cancel");
		}
		if (this.craftProgressBar && this.craftProgressBar.gameObject && this.craftProgressBar.gameObject.activeSelf)
		{
			UISlider uISlider = this.craftProgressBar;
			float? nullable = component.craftingCompletePercent;
			uISlider.sliderValue = (!nullable.HasValue ? 0f : nullable.Value);
			float? nullable1 = component.craftingSecondsRemaining;
			float single = (!nullable1.HasValue ? 0f : nullable1.Value);
			if (single != this._lastTimeStringValue)
			{
				this._lastTimeStringString = single.ToString("0.0");
				this._lastTimeStringValue = single;
			}
			this.progressLabel.text = this._lastTimeStringString;
			Color color = Color.white;
			float single1 = component.craftingSpeedPerSec;
			if (single1 > 1f)
			{
				color = Color.green;
			}
			else if (single1 < 1f)
			{
				color = Color.yellow;
			}
			else if (single1 < 0.5f)
			{
				color = Color.red;
			}
			this.progressLabel.color = color;
		}
		if (this.selectedItem != null)
		{
			this.UpdateWorkbenchRequirements();
		}
		if (this.progressLabel)
		{
			this.progressLabel.enabled = flag;
		}
		this.wasCrafting = component.isCrafting;
	}

	public void UpdateIngredients()
	{
		if (this.selectedItem)
		{
			IEnumerator enumerator = this.ingredientAnchor.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					UnityEngine.Object.Destroy((enumerator.Current as Transform).gameObject);
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
			int num = this.RequestedAmount();
			int num1 = 0;
			BlueprintDataBlock.IngredientEntry[] ingredientEntryArray = this.selectedItem.ingredients;
			for (int i = 0; i < (int)ingredientEntryArray.Length; i++)
			{
				BlueprintDataBlock.IngredientEntry ingredientEntry = ingredientEntryArray[i];
				int num2 = 0;
				PlayerClient.GetLocalPlayer().controllable.GetComponent<CraftingInventory>().FindItem(ingredientEntry.Ingredient, out num2);
				int num3 = ingredientEntry.amount * num;
				GameObject gameObject = NGUITools.AddChild(this.ingredientAnchor, this.ingredientPlaquePrefab);
				gameObject.GetComponent<RPOS_Craft_IngredientPlaque>().Bind(ingredientEntry, num3, num2);
				gameObject.transform.SetLocalPositionY((float)num1);
				num1 = num1 - 12;
			}
		}
	}

	public void UpdateWorkbenchRequirements()
	{
		if (!(this.selectedItem != null) || !this.selectedItem.RequireWorkbench)
		{
			this.requirementLabel.text = string.Empty;
		}
		else
		{
			this.requirementLabel.color = (!this.AtWorkbench() ? Color.red : Color.green);
			this.requirementLabel.text = "REQUIRES WORKBENCH";
		}
	}

	public char ValidateAmountInput(string text, char ch)
	{
		Debug.Log("validating input");
		if (text.Length == 0 && ch == '0')
		{
			return '\0';
		}
		if (ch >= '0' && ch <= '9')
		{
			return ch;
		}
		return '\0';
	}
}