using Facepunch;
using Facepunch.Cursor;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RPOS : UnityEngine.MonoBehaviour
{
	public const RPOSLimitFlags kNoControllableLimitFlags = RPOSLimitFlags.KeepOff | RPOSLimitFlags.HideInventory | RPOSLimitFlags.HideContext | RPOSLimitFlags.HideSprites;

	[NonSerialized]
	private readonly ContextClientWorkingCallback _onContextMenuVisible_;

	[SerializeField]
	private UILabel _useHoverLabel;

	[SerializeField]
	private UIPanel _useHoverPanel;

	private Controllable lastUseHoverControllable;

	private IContextRequestableText lastUseHoverText;

	private IContextRequestableUpdatingText lastUseHoverUpdatingText;

	private IContextRequestablePointText lastUseHoverPointText;

	private Vector3 pointUseHoverOrigin;

	private Plane pointUseHoverPlane;

	private AABBox useHoverLabelBounds;

	private string useHoverText;

	private bool forceHideUseHoverTextCaseContextMenu;

	private bool forceHideUseHoverTextCaseLimitFlags;

	private bool queuedUseHoverText;

	private bool useHoverTextUpdatable;

	private bool useHoverTextPoint;

	private bool useHoverTextPanelVisible;

	private Vector3? useHoverTextScreenPoint;

	public static RPOS g_RPOS;

	public List<RPOSWindow> windowList;

	public RPOSBumper _bumper;

	public GameObject _closeButton;

	public GameObject _optionsButton;

	public RPOSInvCellManager _belt;

	[NonSerialized]
	public RPOSInventoryCell _clickedItemCell;

	private bool RPOSOn;

	private bool forceOff;

	private bool forceHideSprites;

	private bool forceHideInventory;

	private Controllable observedPlayer;

	private UnlockCursorNode unlocker;

	public GameObject windowAnchor;

	public GameObject bottomCenterAnchor;

	public GameObject LootPanelPrefab;

	public GameObject WorkbenchPanelPrefab;

	public GameObject InfoPanelPrefab;

	public UISlider _healthProgress;

	public UISlider _foodProgress;

	public UILabel healthLabel;

	public UISprite fadeSprite;

	public UILabel calorieLabel;

	public UILabel radLabel;

	public UISprite radSprite;

	public UIPanel actionPanel;

	public UILabel actionLabel;

	public UISlider actionProgress;

	public RPOSItemRightClickMenu rightClickMenu;

	public RPOSPlaqueManager _plaqueManager;

	public UIPanel[] keepTop;

	public UIPanel[] keepBottom;

	[HideInInspector]
	public Color nextFadeColor;

	[HideInInspector]
	public float nextFadeDuration;

	private bool awaking;

	private bool rposModeLock;

	private RPOSLimitFlags currentLimitFlags;

	private int lastScreenWidth;

	private int lastScreenHeight;

	public static TempList<RPOSWindow> AllClosedWindows
	{
		get
		{
			TempList<RPOSWindow> tempList = TempList<RPOSWindow>.New();
			foreach (RPOSWindow allWindow in RPOS.g_windows.allWindows)
			{
				if (!allWindow || !allWindow.closed)
				{
					continue;
				}
				tempList.Add(allWindow);
			}
			return tempList;
		}
	}

	public static TempList<RPOSWindow> AllHidingWindows
	{
		get
		{
			TempList<RPOSWindow> tempList = TempList<RPOSWindow>.New();
			foreach (RPOSWindow allWindow in RPOS.g_windows.allWindows)
			{
				if (!allWindow || allWindow.showing)
				{
					continue;
				}
				tempList.Add(allWindow);
			}
			return tempList;
		}
	}

	public static TempList<RPOSWindow> AllOpenWindows
	{
		get
		{
			TempList<RPOSWindow> tempList = TempList<RPOSWindow>.New();
			foreach (RPOSWindow allWindow in RPOS.g_windows.allWindows)
			{
				if (!allWindow || !allWindow.open)
				{
					continue;
				}
				tempList.Add(allWindow);
			}
			return tempList;
		}
	}

	public static TempList<RPOSWindow> AllShowingWindows
	{
		get
		{
			TempList<RPOSWindow> tempList = TempList<RPOSWindow>.New();
			foreach (RPOSWindow allWindow in RPOS.g_windows.allWindows)
			{
				if (!allWindow || !allWindow.showing)
				{
					continue;
				}
				tempList.Add(allWindow);
			}
			return tempList;
		}
	}

	public static TempList<RPOSWindow> AllWindows
	{
		get
		{
			return TempList<RPOSWindow>.New(RPOS.g_windows.allWindows);
		}
	}

	public static bool Exists
	{
		get
		{
			return RPOS.g_RPOS;
		}
	}

	private bool forceHideUseHoverText
	{
		get
		{
			return (this.forceHideUseHoverTextCaseContextMenu || this.RPOSOn ? true : this.forceHideUseHoverTextCaseLimitFlags);
		}
	}

	public static bool hideSprites
	{
		get
		{
			bool flag;
			if (!RPOS.g_RPOS)
			{
				flag = false;
			}
			else
			{
				flag = (RPOS.g_RPOS.RPOSOn ? true : RPOS.g_RPOS.forceHideSprites);
			}
			return flag;
		}
	}

	public static bool IsClosed
	{
		get
		{
			return !RPOS.IsOpen;
		}
	}

	public static bool IsOpen
	{
		get
		{
			return (!RPOS.g_RPOS || !RPOS.g_RPOS.RPOSOn ? false : !RPOS.g_RPOS.awaking);
		}
	}

	public static Controllable ObservedPlayer
	{
		get
		{
			Controllable gRPOS;
			if (!RPOS.g_RPOS)
			{
				gRPOS = null;
			}
			else
			{
				gRPOS = RPOS.g_RPOS.observedPlayer;
			}
			return gRPOS;
		}
		set
		{
			if (RPOS.g_RPOS)
			{
				RPOS.g_RPOS.SetObservedPlayer(value);
			}
		}
	}

	public static int WindowCount
	{
		get
		{
			return RPOS.g_windows.allWindows.Count;
		}
	}

	public RPOS()
	{
		this._onContextMenuVisible_ = new ContextClientWorkingCallback(this.OnContextMenuVisible);
	}

	private void Awake()
	{
		this.actionPanel.enabled = false;
		RPOS.g_RPOS = this;
		try
		{
			this.awaking = true;
			this._bumper.Populate();
			this.unlocker = LockCursorManager.CreateCursorUnlockNode(false, UnlockCursorFlags.AllowSubsetOfKeys, "RPOS UNLOCKER");
			this.SetRPOSModeNoChecks(false);
			UIEventListener.Get(this._closeButton).onClick += new UIEventListener.VoidDelegate(this.OnCloseButtonClicked);
			UIEventListener.Get(this._optionsButton).onClick += new UIEventListener.VoidDelegate(this.OnOptionsButtonClicked);
			TweenColor component = this.fadeSprite.GetComponent<TweenColor>();
			component.eventReceiver = base.gameObject;
			component.callWhenFinished = "FadeFinished";
			if (this._onContextMenuVisible_ != null)
			{
				Context.OnClientWorking += this._onContextMenuVisible_;
			}
			this.UseHoverTextInitialize();
		}
		finally
		{
			this.awaking = false;
		}
		using (TempList<RPOSWindow> tempList = TempList<RPOSWindow>.New(RPOS.g_windows.allWindows))
		{
			foreach (RPOSWindow rPOSWindow in tempList)
			{
				RPOS.InitWindow(rPOSWindow);
			}
		}
	}

	internal static void BeforeRPOSRender_Internal(UICamera uicamera)
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.UIUpdate(uicamera);
		}
	}

	internal static void BeforeSceneRender_Internal(Camera sceneCamera)
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.SceneUpdate(sceneCamera);
		}
	}

	public static bool BringToFront(RPOSWindow window)
	{
		window.EnsureAwake<RPOSWindow>();
		RPOS.g_windows.front = window;
		return RPOS.g_windows.lastPropertySetSuccess;
	}

	public static void ChangeRPOSMode(bool enable)
	{
		RPOS.g_RPOS.SetRPOSMode(enable);
	}

	private void CheckUseHoverTextEnabled()
	{
		this.SetHoverTextState((this.forceHideUseHoverText ? false : this.queuedUseHoverText), this.useHoverText);
	}

	public static void ClearFade()
	{
		RPOS.g_RPOS.fadeSprite.enabled = false;
		RPOS.g_RPOS.CancelInvoke("DoFade");
	}

	private void ClearInjury()
	{
		this._plaqueManager.SetPlaqueActive("PlaqueInjury", false);
	}

	public static void CloseLootWindow()
	{
		foreach (RPOSWindow allWindow in RPOS.g_windows.allWindows)
		{
			if (!allWindow || !(allWindow is RPOSLootWindow))
			{
				continue;
			}
			((RPOSLootWindow)allWindow).LootClosed();
			return;
		}
	}

	public static void CloseOptions()
	{
	}

	public static void CloseWindowByName(string name)
	{
		using (TempList<RPOSWindow> allWindows = RPOS.AllWindows)
		{
			foreach (RPOSWindow allWindow in allWindows)
			{
				if (!allWindow || !(allWindow.title == name))
				{
					continue;
				}
				allWindow.ExternalClose();
			}
		}
	}

	public static void CloseWorkbenchWindow()
	{
		RPOS.CloseWindowByName("Workbench");
	}

	public static void DoFade(float delay, float duration, Color col)
	{
		if (delay > 0f)
		{
			RPOS.g_RPOS.nextFadeColor = col;
			RPOS.g_RPOS.nextFadeDuration = duration;
			RPOS.g_RPOS.Invoke("Internal_DoFade", delay);
		}
		else
		{
			RPOS.DoFadeNow(duration, col);
		}
	}

	public void DoFade(float duration, Color col)
	{
		this.fadeSprite.enabled = true;
		TweenColor.Begin(this.fadeSprite.gameObject, duration, col);
	}

	public static void DoFadeNow(float duration, Color col)
	{
		RPOS.g_RPOS.DoFade(duration, col);
	}

	[Obsolete("Use RPOS.Hide()")]
	public void DoHide()
	{
		if (this.RPOSOn)
		{
			this.DoToggle();
		}
	}

	private void DoInjuryUpdate()
	{
		FallDamage component = RPOS.ObservedPlayer.GetComponent<FallDamage>();
		this._plaqueManager.SetPlaqueActive("PlaqueInjury", component.GetLegInjury() > 0f);
	}

	private void DoMetabolismUpdate()
	{
		Metabolism component = RPOS.ObservedPlayer.GetComponent<Metabolism>();
		this.calorieLabel.text = component.GetCalorieLevel().ToString("N0");
		this.radLabel.text = component.GetRadLevel().ToString("N0");
		this._foodProgress.sliderValue = Mathf.Clamp01(component.GetCalorieLevel() / 3000f);
		this._plaqueManager.SetPlaqueActive("PlaqueHunger", component.GetCalorieLevel() < 500f);
		this._plaqueManager.SetPlaqueActive("PlaqueCold", component.IsCold());
		this._plaqueManager.SetPlaqueActive("PlaqueWarm", component.IsWarm());
		this._plaqueManager.SetPlaqueActive("PlaqueRadiation", component.HasRadiationPoisoning());
		this._plaqueManager.SetPlaqueActive("PlaquePoison", component.IsPoisoned());
		if (component.GetCalorieLevel() >= 500f)
		{
			this.calorieLabel.color = Color.white;
		}
		else
		{
			this.calorieLabel.color = Color.red;
		}
	}

	[Obsolete("Use RPOS.Show()")]
	public void DoShow()
	{
		if (!this.RPOSOn)
		{
			this.DoToggle();
		}
	}

	[Obsolete("Use RPOS.Toggle()")]
	public void DoToggle()
	{
		this.SetRPOSMode(!this.RPOSOn);
	}

	private void DoTossItem(byte slot)
	{
		InventoryHolder component = RPOS.ObservedPlayer.GetComponent<InventoryHolder>();
		if (component)
		{
			component.TossItem((int)slot);
		}
		GUIHeldItem.Get().ClearHeldItem();
	}

	[Obsolete("Use RPOS.SetEquipmentDirty()")]
	public void EquipmentDirty()
	{
		RPOS.GetWindowByName<RPOSArmorWindow>("Armor").ForceUpdate();
	}

	public void FadeFinished()
	{
		if (this.fadeSprite.color.a == 0f)
		{
			this.fadeSprite.enabled = false;
		}
	}

	public static bool FocusArmor()
	{
		return RPOS.FocusListedWindow("Armor");
	}

	public static bool FocusInventory()
	{
		return RPOS.FocusListedWindow("Inventory");
	}

	public static bool FocusListedWindow(string name)
	{
		bool flag;
		if (!RPOS.g_RPOS)
		{
			return false;
		}
		if (RPOS.g_RPOS.forceHideInventory)
		{
			return false;
		}
		bool flag1 = false;
		List<RPOSWindow>.Enumerator enumerator = RPOS.g_RPOS.windowList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				RPOSWindow current = enumerator.Current;
				if (!current || !(current.title == name))
				{
					continue;
				}
				if (!RPOS.g_RPOS.RPOSOn)
				{
					RPOS.g_RPOS.SetRPOSMode(true);
					if (!RPOS.g_RPOS.RPOSOn)
					{
						flag = false;
						return flag;
					}
				}
				current.zzz___INTERNAL_FOCUS();
				flag1 = true;
			}
			return flag1;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return flag;
	}

	[Obsolete("Avoid using this", true)]
	public static RPOS Get()
	{
		return RPOS.g_RPOS;
	}

	public static IEnumerable<RPOSWindow> GetBumperWindowList()
	{
		RPOS gRPOS = RPOS.g_RPOS;
		if (!gRPOS)
		{
			UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(RPOS));
			if ((int)objArray.Length <= 0)
			{
				return new RPOSWindow[0];
			}
			gRPOS = (RPOS)objArray[0];
		}
		return gRPOS.windowList;
	}

	public static int GetIndex2D(int x, int y, int width)
	{
		return x + y * width;
	}

	[Obsolete("Use var player = RPOS.ObservedPlayer", true)]
	public Controllable GetObservedPlayer()
	{
		return this.observedPlayer;
	}

	public static bool GetObservedPlayerComponent<TComponent>(out TComponent component)
	where TComponent : Component
	{
		if (RPOS.g_RPOS)
		{
			Controllable gRPOS = RPOS.g_RPOS.observedPlayer;
			if (gRPOS)
			{
				component = gRPOS.GetComponent<TComponent>();
				return component;
			}
		}
		component = (TComponent)null;
		return false;
	}

	public static RPOSItemRightClickMenu GetRightClickMenu()
	{
		return RPOS.g_RPOS.rightClickMenu;
	}

	internal static bool GetWindowAbove(RPOSWindow window, out RPOSWindow fill)
	{
		if (!window)
		{
			throw new ArgumentNullException("window");
		}
		int num = window.order;
		if (num + 1 == RPOS.WindowCount)
		{
			fill = null;
			return false;
		}
		fill = RPOS.g_windows.allWindows[num + 1];
		return true;
	}

	internal static RPOSWindow GetWindowAbove(RPOSWindow window)
	{
		RPOSWindow rPOSWindow;
		RPOSWindow rPOSWindow1;
		if (!RPOS.GetWindowAbove(window, out rPOSWindow))
		{
			rPOSWindow1 = null;
		}
		else
		{
			rPOSWindow1 = rPOSWindow;
		}
		return rPOSWindow1;
	}

	internal static bool GetWindowBelow(RPOSWindow window, out RPOSWindow fill)
	{
		if (!window)
		{
			throw new ArgumentNullException("window");
		}
		int num = window.order;
		if (num == 0)
		{
			fill = null;
			return false;
		}
		fill = RPOS.g_windows.allWindows[num - 1];
		return true;
	}

	internal static RPOSWindow GetWindowBelow(RPOSWindow window)
	{
		RPOSWindow rPOSWindow;
		RPOSWindow rPOSWindow1;
		if (!RPOS.GetWindowAbove(window, out rPOSWindow))
		{
			rPOSWindow1 = null;
		}
		else
		{
			rPOSWindow1 = rPOSWindow;
		}
		return rPOSWindow1;
	}

	public static RPOSWindow GetWindowByName(string name)
	{
		RPOSWindow rPOSWindow;
		if (!RPOS.g_RPOS)
		{
			return null;
		}
		List<RPOSWindow>.Enumerator enumerator = RPOS.g_windows.allWindows.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				RPOSWindow current = enumerator.Current;
				if (!current || !(current.title == name))
				{
					continue;
				}
				RPOSWindow.EnsureAwake(current);
				rPOSWindow = current;
				return rPOSWindow;
			}
			Debug.Log("GetWindowByName returning null");
			return null;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return rPOSWindow;
	}

	public static TRPOSWindow GetWindowByName<TRPOSWindow>(string name)
	where TRPOSWindow : RPOSWindow
	{
		TRPOSWindow tRPOSWindow;
		if (!RPOS.g_RPOS)
		{
			return (TRPOSWindow)null;
		}
		List<RPOSWindow>.Enumerator enumerator = RPOS.g_windows.allWindows.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				RPOSWindow current = enumerator.Current;
				if (!current || !(current is TRPOSWindow) || !(current.title == name))
				{
					continue;
				}
				RPOSWindow.EnsureAwake(current);
				tRPOSWindow = (TRPOSWindow)current;
				return tRPOSWindow;
			}
			return (TRPOSWindow)null;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return tRPOSWindow;
	}

	public static void HealthUpdate(float value)
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.UpdateHealth(value);
		}
	}

	public static void Hide()
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.DoHide();
		}
	}

	private static void InitWindow(RPOSWindow window)
	{
		if (window)
		{
			window.RPOSReady();
			window.CheckDisplay();
		}
	}

	public static void InjuryUpdate()
	{
		RPOS.g_RPOS.DoInjuryUpdate();
	}

	public void Internal_DoFade()
	{
		this.DoFade(this.nextFadeDuration, this.nextFadeColor);
	}

	public static bool IsObservedPlayer(Controllable controllable)
	{
		return (!RPOS.g_RPOS || !controllable ? false : RPOS.g_RPOS.observedPlayer == controllable);
	}

	public static void Item_CellDragBegin(RPOSInventoryCell begin)
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.ItemCellDragBegin(begin);
		}
	}

	public static void Item_CellDragEnd(RPOSInventoryCell begin, RPOSInventoryCell end)
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.ItemCellDragEnd(begin, end);
		}
	}

	public static void Item_CellDrop(RPOSInventoryCell cell)
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.ItemCellDrop(cell);
		}
	}

	public static void Item_CellReset()
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.ItemCellReset();
		}
	}

	public static bool Item_IsClickedCell(RPOSInventoryCell cell)
	{
		return (!RPOS.g_RPOS || !RPOS.g_RPOS._clickedItemCell ? false : RPOS.g_RPOS._clickedItemCell == cell);
	}

	public static void ItemCellAltClicked(RPOSInventoryCell cell)
	{
	}

	[Obsolete("Use RPOS.Item_CellClicked()")]
	public void ItemCellClicked(RPOSInventoryCell cell)
	{
		RPOSInventoryCell rPOSInventoryCell = cell;
		bool flag = false;
		byte num = 0;
		byte num1 = 0;
		Inventory inventory = null;
		Inventory inventory1 = null;
		IInventoryItem inventoryItem = null;
		IInventoryItem inventoryItem1 = null;
		if (this._clickedItemCell != null)
		{
			inventory = this._clickedItemCell._displayInventory;
			num = this._clickedItemCell._mySlot;
			inventory.GetItem((int)num, out inventoryItem);
		}
		inventory1 = rPOSInventoryCell._displayInventory;
		num1 = rPOSInventoryCell._mySlot;
		inventory1.GetItem((int)num1, out inventoryItem1);
		if (inventoryItem == null && inventoryItem1 == null)
		{
			Debug.Log("wtf");
		}
		if (inventoryItem == null && inventoryItem1 != null)
		{
			this._clickedItemCell = cell;
			inventoryItem = cell._myDisplayItem;
			flag = true;
		}
		else if (inventoryItem != null && inventoryItem1 != null)
		{
			bool flag1 = Event.current.shift;
			NetEntityID netEntityID = NetEntityID.Get(inventory);
			NetEntityID netEntityID1 = NetEntityID.Get(inventory1);
			if (!flag1)
			{
				Inventory.ItemMergePredicted(netEntityID, netEntityID1, (int)num, (int)num1);
			}
			else
			{
				Inventory.ItemCombinePredicted(netEntityID, netEntityID1, (int)num, (int)num1);
			}
			inventoryItem = null;
			this._clickedItemCell = null;
		}
		else if (inventoryItem != null && inventoryItem1 == null)
		{
			NetEntityID netEntityID2 = NetEntityID.Get(inventory1);
			Inventory.ItemMovePredicted(NetEntityID.Get(inventory), netEntityID2, (int)num, (int)num1);
			this._clickedItemCell = null;
			inventoryItem = null;
			flag = true;
		}
		if (inventoryItem != GUIHeldItem.CurrentItem())
		{
			if (inventoryItem != null)
			{
				if (!flag || !GUIHeldItem.Get().SetHeldItem(cell))
				{
					GUIHeldItem.Get().SetHeldItem(inventoryItem);
				}
			}
			else if (!flag || !cell)
			{
				GUIHeldItem.Get().ClearHeldItem();
			}
			else
			{
				GUIHeldItem.Get().ClearHeldItem(cell);
			}
		}
	}

	[Obsolete("Use RPOS.Item_CellDragBegin()")]
	public void ItemCellDragBegin(RPOSInventoryCell cell)
	{
		this.ItemCellReset();
		this.ItemCellClicked(cell);
	}

	[Obsolete("Use RPOS.Item_CellDragEnd()")]
	public void ItemCellDragEnd(RPOSInventoryCell begin, RPOSInventoryCell end)
	{
		if (end)
		{
			GUIHeldItem.Get().ClearHeldItem(end);
		}
		this.ItemCellReset();
		if (begin != end && end && begin)
		{
			this._clickedItemCell = begin;
			this.ItemCellClicked(end);
		}
	}

	[Obsolete("Use RPOS.Item_CellDrop()")]
	public void ItemCellDrop(RPOSInventoryCell cell)
	{
		if (this._clickedItemCell != null)
		{
			this.ItemCellClicked(cell);
		}
	}

	[Obsolete("Use RPOS.Item_CellReset()")]
	public void ItemCellReset()
	{
		if (!this._clickedItemCell)
		{
			GUIHeldItem.Get().ClearHeldItem();
		}
		else
		{
			GUIHeldItem.Get().ClearHeldItem(this._clickedItemCell);
			this._clickedItemCell._displayInventory.MarkSlotDirty((int)this._clickedItemCell._mySlot);
		}
		this._clickedItemCell = null;
	}

	private void LimitInventory(bool limit)
	{
		this.forceHideInventory = limit;
		using (TempList<RPOSWindow> allWindows = RPOS.AllWindows)
		{
			bool flag = !limit;
			foreach (RPOSWindow allWindow in allWindows)
			{
				if (!allWindow || !allWindow.isInventoryRelated)
				{
					continue;
				}
				allWindow.bumpersEnabled = flag;
			}
			foreach (RPOSWindow rPOSWindow in allWindows)
			{
				if (!rPOSWindow)
				{
					continue;
				}
				rPOSWindow.inventoryHide = limit;
			}
		}
		if (this._belt)
		{
			this._belt.GetComponent<UIPanel>().enabled = !limit;
		}
	}

	public static void LocalInventoryModified()
	{
		RPOS.GetWindowByName("Crafting").GetComponent<RPOSCraftWindow>().LocalInventoryModified();
		RPOS.SetPlaqueActive("PlaqueCrafting", RPOS.g_RPOS.observedPlayer.GetComponent<CraftingInventory>().isCrafting);
	}

	public static void MetabolismUpdate()
	{
		RPOS.g_RPOS.DoMetabolismUpdate();
	}

	public static bool MoveDown(RPOSWindow window)
	{
		return RPOS.g_windows.MoveDown(window.EnsureAwake<RPOSWindow>());
	}

	public static bool MoveUp(RPOSWindow window)
	{
		return RPOS.g_windows.MoveUp(window.EnsureAwake<RPOSWindow>());
	}

	public void OnCloseButtonClicked(GameObject go)
	{
		this.SetRPOSMode(false);
	}

	private void OnContextMenuVisible(bool visible)
	{
		this.forceHideUseHoverTextCaseContextMenu = visible;
		this.CheckUseHoverTextEnabled();
	}

	private void OnDestroy()
	{
		if (this.unlocker != null)
		{
			this.unlocker.Dispose();
			this.unlocker = null;
		}
		if (this._onContextMenuVisible_ != null)
		{
			Context.OnClientWorking -= this._onContextMenuVisible_;
		}
	}

	public void OnOptionsButtonClicked(GameObject go)
	{
		RPOS.OpenOptions();
	}

	public static void OpenInfoWindow(ItemDataBlock itemdb)
	{
	}

	public static void OpenLootWindow(LootableObject lootObj)
	{
		if (RPOS.g_RPOS)
		{
			RPOS.CloseWindowByName("Crafting");
			Vector3 lootPanelPrefab = RPOS.g_RPOS.LootPanelPrefab.transform.localPosition;
			GameObject gameObject = null;
			gameObject = (!lootObj.lootWindowOverride ? RPOS.g_RPOS.LootPanelPrefab : lootObj.lootWindowOverride.gameObject);
			GameObject gameObject1 = NGUITools.AddChild(RPOS.g_RPOS.bottomCenterAnchor, gameObject);
			gameObject1.GetComponent<RPOSLootWindow>().SetLootable(lootObj, true);
			gameObject1.transform.localPosition = lootPanelPrefab;
			RPOS.BringToFront(gameObject1.GetComponent<RPOSWindow>());
			RPOS.g_RPOS.SetRPOSMode(true);
		}
	}

	public static void OpenOptions()
	{
	}

	public static void OpenWorkbenchWindow(WorkBench workbenchObj)
	{
		if (RPOS.g_RPOS)
		{
			GameObject gameObject = NGUITools.AddChild(RPOS.g_RPOS.windowAnchor, RPOS.g_RPOS.WorkbenchPanelPrefab);
			gameObject.GetComponent<RPOSWorkbenchWindow>().SetWorkbench(workbenchObj);
			RPOS.BringToFront(gameObject.GetComponent<RPOSWindow>());
			RPOS.g_RPOS.SetRPOSMode(true);
		}
	}

	internal static void RegisterWindow(RPOSWindow window)
	{
		if (window.zzz__index == -1)
		{
			window.zzz__index = RPOS.g_windows.allWindows.Count;
			RPOS.g_windows.allWindows.Add(window);
			if (RPOS.g_RPOS && !RPOS.g_RPOS.awaking)
			{
				RPOS.InitWindow(window);
			}
			RPOS.g_windows.orderChanged = true;
		}
	}

	private void SceneUpdate(Camera camera)
	{
		this.UseHoverTextThink(camera);
	}

	public static bool SendToBack(RPOSWindow window)
	{
		window.EnsureAwake<RPOSWindow>();
		RPOS.g_windows.back = window;
		return RPOS.g_windows.lastPropertySetSuccess;
	}

	public static void SetActionProgress(bool show, string label, float progress)
	{
		if (!show)
		{
			RPOS.g_RPOS.actionPanel.enabled = false;
		}
		else
		{
			if (string.IsNullOrEmpty(label))
			{
				RPOS.g_RPOS.actionLabel.enabled = false;
			}
			else
			{
				RPOS.g_RPOS.actionLabel.text = label;
				RPOS.g_RPOS.actionLabel.enabled = true;
			}
			RPOS.g_RPOS.actionProgress.sliderValue = progress;
			RPOS.g_RPOS.actionPanel.enabled = true;
		}
	}

	public static void SetCurrentFade(Color col)
	{
		RPOS.g_RPOS.fadeSprite.color = col;
		TweenColor component = RPOS.g_RPOS.fadeSprite.GetComponent<TweenColor>();
		component.@from = col;
		component.to = col;
		component.isFullscreen = true;
		RPOS.g_RPOS.fadeSprite.enabled = true;
	}

	public static void SetEquipmentDirty()
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.EquipmentDirty();
		}
	}

	private void SetHoverTextState(bool enable, string text)
	{
		if (!this._useHoverLabel)
		{
			return;
		}
		if (!enable || string.IsNullOrEmpty(text))
		{
			this.useHoverTextPanelVisible = false;
			if (!this._useHoverPanel)
			{
				this._useHoverLabel.enabled = false;
			}
			else
			{
				this._useHoverPanel.enabled = false;
			}
		}
		else
		{
			bool flag = false;
			this._useHoverLabel.enabled = true;
			if (this._useHoverLabel.text != text)
			{
				this._useHoverLabel.text = text;
				flag = true;
			}
			if (this._useHoverPanel)
			{
				this.useHoverTextPanelVisible = this.lastUseHoverControllable;
				if (flag || !this._useHoverPanel.enabled)
				{
					this._useHoverPanel.enabled = true;
					this._useHoverPanel.ManualPanelUpdate();
					if (flag)
					{
						this.useHoverLabelBounds = NGUIMath.CalculateRelativeWidgetBounds(this._useHoverPanel.transform, this._useHoverLabel.transform);
					}
				}
			}
		}
	}

	[Obsolete("Use RPOS.ObservedPlayer = player")]
	public void SetObservedPlayer(Controllable player)
	{
		this.observedPlayer = player;
		RPOSWindow windowByName = RPOS.GetWindowByName("Inventory");
		if (windowByName)
		{
			windowByName.GetComponentInChildren<RPOSInvCellManager>().SetInventory(player.GetComponent<Inventory>(), false);
		}
		PlayerInventory component = player.GetComponent<PlayerInventory>();
		this._belt.CellIndexStart = 30;
		this._belt.SetInventory(component, false);
		RPOSInvCellManager componentInChildren = RPOS.GetWindowByName("Armor").GetComponentInChildren<RPOSInvCellManager>();
		componentInChildren.CellIndexStart = 36;
		componentInChildren.SetInventory(component, false);
		this.SetRPOSMode(false);
		RPOS.InjuryUpdate();
	}

	public static void SetPlaqueActive(string plaqueName, bool on)
	{
		RPOS.g_RPOS._plaqueManager.SetPlaqueActive(plaqueName, on);
	}

	private void SetRPOSMode(bool enable)
	{
		if (enable != this.RPOSOn)
		{
			if (this.forceOff && enable)
			{
				return;
			}
			this.SetRPOSModeNoChecks(enable);
		}
	}

	private void SetRPOSModeNoChecks(bool enable)
	{
		if (this.rposModeLock)
		{
			if (enable != this.RPOSOn)
			{
				throw new InvalidOperationException((!enable ? "You cannot turn OFF RPOS while its being turned ON-- check callstack" : "You cannot turn ON RPOS while its being turned OFF-- check callstack"));
			}
			return;
		}
		try
		{
			this.rposModeLock = true;
			if (!this.observedPlayer)
			{
				enable = false;
			}
			bool rPOSOn = this.RPOSOn != enable;
			this.RPOSOn = enable;
			using (TempList<RPOSWindow> tempList = TempList<RPOSWindow>.New(RPOS.g_windows.allWindows))
			{
				if (enable)
				{
					foreach (RPOSWindow rPOSWindow in tempList)
					{
						if (!rPOSWindow)
						{
							continue;
						}
						rPOSWindow.RPOSOn();
					}
				}
				foreach (RPOSWindow rPOSWindow1 in tempList)
				{
					if (!rPOSWindow1)
					{
						continue;
					}
					rPOSWindow1.CheckDisplay();
				}
				if (!enable)
				{
					foreach (RPOSWindow rPOSWindow2 in tempList)
					{
						if (!rPOSWindow2)
						{
							continue;
						}
						rPOSWindow2.RPOSOff();
					}
					this._clickedItemCell = null;
					GUIHeldItem gUIHeldItem = GUIHeldItem.Get();
					if (gUIHeldItem)
					{
						gUIHeldItem.ClearHeldItem();
					}
				}
			}
			this._bumper.GetComponent<UIPanel>().enabled = enable;
			UIPanel.Find(this._closeButton.transform).enabled = enable;
			if (!this.RPOSOn)
			{
				this.unlocker.TryLock();
			}
			else
			{
				this.unlocker.On = true;
			}
			if (rPOSOn)
			{
				this.CheckUseHoverTextEnabled();
			}
		}
		finally
		{
			this.rposModeLock = false;
		}
		ItemToolTip.SetToolTip(null, null);
	}

	public static void Toggle()
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.DoToggle();
		}
	}

	public static void ToggleOptions()
	{
	}

	public static void TossItem(byte slot)
	{
		if (RPOS.g_RPOS)
		{
			RPOS.g_RPOS.DoTossItem(slot);
		}
	}

	private void UIUpdate(UICamera camera)
	{
		this.UseHoverTextPostThink(camera.cachedCamera);
	}

	internal static void UnregisterWindow(RPOSWindow window)
	{
		bool item;
		while (window.zzz__index > -1)
		{
			try
			{
				item = RPOS.g_windows.allWindows[window.zzz__index] == window;
			}
			catch (IndexOutOfRangeException indexOutOfRangeException)
			{
				item = false;
			}
			if (item)
			{
				RPOS.g_windows.allWindows.RemoveAt(window.zzz__index);
				int zzz_index = window.zzz__index;
				int count = RPOS.g_windows.allWindows.Count;
				while (zzz_index < count)
				{
					RPOS.g_windows.allWindows[zzz_index].zzz__index = zzz_index;
					zzz_index++;
				}
				RPOS.g_windows.orderChanged = true;
				break;
			}
			else
			{
				int num = RPOS.g_windows.allWindows.IndexOf(window);
				Debug.LogWarning(string.Format("Some how list maintanance failed, stored index was {0} but index of returned {1}", window.zzz__index, num), window);
				window.zzz__index = num;
			}
		}
	}

	private void Update()
	{
		HUDIndicator.Step();
		RPOSLimitFlags rPOSLimitFlag = this.currentLimitFlags;
		PlayerClient localPlayer = PlayerClient.GetLocalPlayer();
		if (!localPlayer)
		{
			this.currentLimitFlags = RPOSLimitFlags.KeepOff | RPOSLimitFlags.HideInventory | RPOSLimitFlags.HideContext | RPOSLimitFlags.HideSprites;
		}
		else
		{
			Controllable controllable = localPlayer.controllable;
			if (controllable)
			{
				Controllable controllable1 = controllable.masterControllable;
				Controllable controllable2 = controllable1;
				if (!controllable1)
				{
					goto Label1;
				}
				this.currentLimitFlags = controllable2.rposLimitFlags;
				goto Label0;
			}
		Label1:
			this.currentLimitFlags = RPOSLimitFlags.KeepOff | RPOSLimitFlags.HideInventory | RPOSLimitFlags.HideContext | RPOSLimitFlags.HideSprites;
		Label0:
		}
		if (rPOSLimitFlag != this.currentLimitFlags)
		{
			RPOSLimitFlags rPOSLimitFlag1 = rPOSLimitFlag ^ this.currentLimitFlags;
			if ((rPOSLimitFlag1 & RPOSLimitFlags.HideContext) == RPOSLimitFlags.HideContext)
			{
				this.forceHideUseHoverTextCaseLimitFlags = (this.currentLimitFlags & RPOSLimitFlags.HideContext) == RPOSLimitFlags.HideContext;
				this.CheckUseHoverTextEnabled();
			}
			if ((rPOSLimitFlag1 & RPOSLimitFlags.HideSprites) == RPOSLimitFlags.HideSprites)
			{
				this.forceHideSprites = (this.currentLimitFlags & RPOSLimitFlags.HideSprites) == RPOSLimitFlags.HideSprites;
			}
			if ((rPOSLimitFlag1 & RPOSLimitFlags.HideInventory) == RPOSLimitFlags.HideInventory)
			{
				this.LimitInventory((this.currentLimitFlags & RPOSLimitFlags.HideInventory) == RPOSLimitFlags.HideInventory);
			}
			if ((rPOSLimitFlag1 & RPOSLimitFlags.KeepOff) == RPOSLimitFlags.KeepOff)
			{
				if ((this.currentLimitFlags & RPOSLimitFlags.KeepOff) != RPOSLimitFlags.KeepOff)
				{
					this.forceOff = false;
				}
				else
				{
					if (this.RPOSOn)
					{
						this.SetRPOSMode(false);
					}
					this.forceOff = true;
				}
			}
		}
		int num = Screen.width;
		int num1 = Screen.height;
		if (RPOS.g_windows.orderChanged || num1 != this.lastScreenHeight || num != this.lastScreenWidth)
		{
			RPOS.g_windows.ProcessDepth(this.windowAnchor.transform);
			this.lastScreenHeight = num1;
			this.lastScreenWidth = num;
		}
		if (RPOS.g_RPOS.observedPlayer)
		{
			RPOS.SetPlaqueActive("PlaqueWorkbench1", RPOS.g_RPOS.observedPlayer.GetComponent<CraftingInventory>().AtWorkBench());
		}
	}

	[Obsolete("Use RPOS.HealthUpdate(amount)")]
	public void UpdateHealth(float amount)
	{
		this.healthLabel.text = amount.ToString("N0");
		this._healthProgress.sliderValue = Mathf.Clamp01(amount / 100f);
		UIFilledSprite component = this._healthProgress.foreground.GetComponent<UIFilledSprite>();
		if (amount > 75f)
		{
			component.color = Color.green;
		}
		else if (amount <= 40f)
		{
			component.color = Color.red;
		}
		else
		{
			component.color = Color.yellow;
		}
	}

	private void UpdateUseHoverTextPlane()
	{
		this.pointUseHoverPlane = new Plane(-this._useHoverPanel.transform.forward, this._useHoverPanel.transform.position);
	}

	public static void UseHoverTextClear()
	{
		RPOS.g_RPOS.useHoverText = string.Empty;
		RPOS.g_RPOS.queuedUseHoverText = false;
		RPOS.g_RPOS.lastUseHoverControllable = null;
		RPOS.g_RPOS.lastUseHoverText = null;
		RPOS.g_RPOS.lastUseHoverUpdatingText = null;
		RPOS.g_RPOS.lastUseHoverPointText = null;
		RPOS.g_RPOS.useHoverTextUpdatable = false;
		RPOS.g_RPOS.useHoverTextPoint = false;
		RPOS.g_RPOS.CheckUseHoverTextEnabled();
	}

	private void UseHoverTextInitialize()
	{
		if (this._useHoverPanel)
		{
			this.pointUseHoverOrigin = this._useHoverPanel.transform.localPosition;
			this.UpdateUseHoverTextPlane();
		}
		this.CheckUseHoverTextEnabled();
	}

	private void UseHoverTextMove(Camera sceneCamera, Vector3 worldPoint)
	{
		this.useHoverTextScreenPoint = new Vector3?(sceneCamera.WorldToScreenPoint(worldPoint));
	}

	private void UseHoverTextMoveRevert()
	{
		if (this._useHoverPanel)
		{
			this.useHoverTextScreenPoint = null;
			this._useHoverPanel.transform.localPosition = this.pointUseHoverOrigin;
		}
	}

	private void UseHoverTextPostThink(Camera panelCamera)
	{
		if (this._useHoverPanel)
		{
			this.UseHoverTextScreen(panelCamera);
		}
	}

	private void UseHoverTextScreen(Camera panelCamera)
	{
		float single;
		if (this.useHoverTextScreenPoint.HasValue)
		{
			Vector3 value = this.useHoverTextScreenPoint.Value;
			this.useHoverTextScreenPoint = null;
			Vector2 vector2 = this.useHoverLabelBounds.min + value;
			Vector2 vector21 = this.useHoverLabelBounds.max + value;
			if (vector2 != vector21)
			{
				if (vector2.x < 0f)
				{
					if (vector21.x < (float)Screen.width)
					{
						value.x = value.x - vector2.x;
					}
				}
				else if (vector21.x > (float)Screen.width)
				{
					value.x = value.x - (vector21.x - (float)Screen.width);
				}
				if (vector2.y < 0f)
				{
					if (vector21.y < (float)Screen.height)
					{
						value.y = value.y - vector2.y;
					}
				}
				else if (vector21.y > (float)Screen.height)
				{
					value.y = value.y - (vector21.y - (float)Screen.height);
				}
			}
			Ray ray = panelCamera.ScreenPointToRay(value);
			if (this.pointUseHoverPlane.Raycast(ray, out single))
			{
				this._useHoverPanel.transform.position = ray.GetPoint(single);
				this._useHoverPanel.ManualPanelUpdate();
			}
		}
	}

	public static void UseHoverTextSet(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			RPOS.g_RPOS.queuedUseHoverText = true;
			RPOS.g_RPOS.useHoverText = text;
			RPOS.g_RPOS.lastUseHoverText = null;
			RPOS.g_RPOS.lastUseHoverControllable = null;
			RPOS.g_RPOS.lastUseHoverUpdatingText = null;
			RPOS.g_RPOS.lastUseHoverPointText = null;
			RPOS.g_RPOS.useHoverTextUpdatable = false;
			RPOS.g_RPOS.useHoverTextPoint = false;
			RPOS.g_RPOS.UseHoverTextMoveRevert();
			RPOS.g_RPOS.CheckUseHoverTextEnabled();
		}
		else
		{
			RPOS.UseHoverTextClear();
		}
	}

	public static void UseHoverTextSet(Controllable localPlayerControllable, IContextRequestableText text)
	{
		if (text == null)
		{
			RPOS.UseHoverTextClear();
		}
		else if (RPOS.g_RPOS.lastUseHoverText != text)
		{
			RPOS.g_RPOS.lastUseHoverText = text;
			RPOS.g_RPOS.lastUseHoverUpdatingText = text as IContextRequestableUpdatingText;
			RPOS.g_RPOS.useHoverTextUpdatable = RPOS.g_RPOS.lastUseHoverUpdatingText != null;
			RPOS.g_RPOS.lastUseHoverPointText = text as IContextRequestablePointText;
			RPOS.g_RPOS.useHoverTextPoint = RPOS.g_RPOS.lastUseHoverPointText != null;
			if (!RPOS.g_RPOS.useHoverTextPoint)
			{
				RPOS.g_RPOS.UseHoverTextMoveRevert();
			}
			RPOS.g_RPOS.lastUseHoverControllable = localPlayerControllable;
			RPOS.g_RPOS.useHoverText = text.ContextText(localPlayerControllable);
			RPOS.g_RPOS.queuedUseHoverText = true;
			RPOS.g_RPOS.CheckUseHoverTextEnabled();
		}
	}

	private void UseHoverTextThink(Camera sceneCamera)
	{
		string str;
		Vector3 vector3;
		this.useHoverTextScreenPoint = null;
		if (!this.forceHideUseHoverText && this.queuedUseHoverText)
		{
			if (!(this.lastUseHoverText as Facepunch.MonoBehaviour))
			{
				this.lastUseHoverControllable = null;
			}
			if (!this.lastUseHoverControllable)
			{
				this.useHoverTextPanelVisible = false;
				if (this.lastUseHoverText != null)
				{
					RPOS.UseHoverTextClear();
				}
			}
			else
			{
				if (!this._useHoverLabel)
				{
					return;
				}
				str = (!this.useHoverTextUpdatable ? this.lastUseHoverText.ContextText(this.lastUseHoverControllable) : this.lastUseHoverUpdatingText.ContextTextUpdate(this.lastUseHoverControllable, this.useHoverText) ?? string.Empty);
				if (str != this.useHoverText)
				{
					this.useHoverText = str;
					this.SetHoverTextState(true, this.useHoverText);
				}
			}
			if (this.useHoverTextPanelVisible)
			{
				if (this.useHoverTextPoint)
				{
					if (!this.lastUseHoverPointText.ContextTextPoint(out vector3))
					{
						this.UseHoverTextMoveRevert();
					}
					else
					{
						this.UseHoverTextMove(sceneCamera, vector3);
					}
				}
				this._useHoverPanel.ManualPanelUpdate();
			}
		}
	}

	private static class g_windows
	{
		public static List<RPOSWindow> allWindows;

		public static bool orderChanged;

		public static bool lastPropertySetSuccess;

		public static float lastZ;

		public static RPOSWindow back
		{
			get
			{
				RPOSWindow item;
				if (RPOS.g_windows.allWindows.Count != 0)
				{
					item = RPOS.g_windows.allWindows[0];
				}
				else
				{
					item = null;
				}
				return item;
			}
			set
			{
				RPOS.g_windows.lastPropertySetSuccess = false;
				if (!value)
				{
					throw new ArgumentNullException();
				}
				if (value.zzz__index == -1)
				{
					throw new InvalidOperationException("The window was not awake");
				}
				int count = RPOS.g_windows.allWindows.Count;
				if (count == 0)
				{
					throw new InvalidOperationException("There definitely should have been a window in the list unless you passed a prefab here or didnt ensure awake.");
				}
				if (count == 1 || RPOS.g_windows.allWindows[0] == value)
				{
					return;
				}
				for (int i = value.zzz__index; i > 0; i--)
				{
					RPOSWindow item = RPOS.g_windows.allWindows[i - 1];
					RPOS.g_windows.allWindows[i] = item;
					item.zzz__index = i;
				}
				RPOS.g_windows.allWindows[0] = value;
				value.zzz__index = 0;
				RPOS.g_windows.orderChanged = true;
				RPOS.g_windows.lastPropertySetSuccess = true;
			}
		}

		public static RPOSWindow front
		{
			get
			{
				RPOSWindow item;
				int count = RPOS.g_windows.allWindows.Count;
				if (count != 0)
				{
					item = RPOS.g_windows.allWindows[count - 1];
				}
				else
				{
					item = null;
				}
				return item;
			}
			set
			{
				RPOS.g_windows.lastPropertySetSuccess = false;
				if (!value)
				{
					throw new ArgumentNullException();
				}
				if (value.zzz__index == -1)
				{
					throw new InvalidOperationException("The window was not awake");
				}
				int count = RPOS.g_windows.allWindows.Count;
				if (count == 0)
				{
					throw new InvalidOperationException("There definitely should have been a window in the list unless you passed a prefab here or didnt ensure awake.");
				}
				if (count == 1 || RPOS.g_windows.allWindows[count - 1] == value)
				{
					return;
				}
				for (int i = value.zzz__index; i < count - 1; i++)
				{
					RPOSWindow item = RPOS.g_windows.allWindows[i + 1];
					RPOS.g_windows.allWindows[i] = item;
					item.zzz__index = i;
				}
				RPOS.g_windows.allWindows[count - 1] = value;
				value.zzz__index = count - 1;
				RPOS.g_windows.orderChanged = true;
				RPOS.g_windows.lastPropertySetSuccess = true;
			}
		}

		static g_windows()
		{
			RPOS.g_windows.allWindows = new List<RPOSWindow>();
			RPOS.g_windows.orderChanged = false;
			RPOS.g_windows.lastPropertySetSuccess = false;
		}

		public static bool MoveDown(RPOSWindow window)
		{
			if (!window)
			{
				throw new ArgumentNullException();
			}
			if (window.zzz__index == -1)
			{
				throw new InvalidOperationException("The window was not awake");
			}
			int count = RPOS.g_windows.allWindows.Count;
			if (count == 0)
			{
				throw new InvalidOperationException("There definitely should have been a window in the list unless you passed a prefab here or didnt ensure awake.");
			}
			if (count == 1 || RPOS.g_windows.allWindows[0] == window)
			{
				return false;
			}
			RPOS.g_windows.allWindows.Reverse(window.zzz__index - 1, 2);
			RPOS.g_windows.allWindows[window.zzz__index].zzz__index = window.zzz__index;
			RPOSWindow zzz_index = window;
			zzz_index.zzz__index = zzz_index.zzz__index - 1;
			RPOS.g_windows.orderChanged = true;
			return true;
		}

		public static bool MoveUp(RPOSWindow window)
		{
			if (!window)
			{
				throw new ArgumentNullException();
			}
			if (window.zzz__index == -1)
			{
				throw new InvalidOperationException("The window was not awake");
			}
			int count = RPOS.g_windows.allWindows.Count;
			if (count == 0)
			{
				throw new InvalidOperationException("There definitely should have been a window in the list unless you passed a prefab here or didnt ensure awake.");
			}
			if (count == 1 || RPOS.g_windows.allWindows[count - 1] == window)
			{
				return false;
			}
			RPOS.g_windows.allWindows.Reverse(window.zzz__index, 2);
			RPOS.g_windows.allWindows[window.zzz__index].zzz__index = window.zzz__index;
			RPOSWindow zzz_index = window;
			zzz_index.zzz__index = zzz_index.zzz__index + 1;
			RPOS.g_windows.orderChanged = true;
			return true;
		}

		public static void ProcessDepth(Transform uiRoot)
		{
			Bounds bound;
			UIPanel[] gRPOS;
			UIPanel[] uIPanelArray;
			RPOS.g_windows.orderChanged = false;
			RPOS.g_windows.lastZ = 0f;
			if (!RPOS.g_RPOS)
			{
				gRPOS = null;
			}
			else
			{
				gRPOS = RPOS.g_RPOS.keepBottom;
			}
			UIPanel[] uIPanelArray1 = gRPOS;
			if (uIPanelArray1 != null)
			{
				for (int i = (int)uIPanelArray1.Length - 1; i >= 0; i--)
				{
					if (uIPanelArray1[i])
					{
						RPOS.g_windows.ProcessTransform(uIPanelArray1[i].transform, ref RPOS.g_windows.lastZ);
					}
				}
			}
			RPOS.g_windows.WindowRect[] windowRectArray = new RPOS.g_windows.WindowRect[RPOS.g_windows.allWindows.Count];
			RPOS.g_windows.WindowRect windowRect = new RPOS.g_windows.WindowRect();
			Matrix4x4 matrix4x4 = uiRoot.worldToLocalMatrix;
			int num = 0;
			foreach (RPOSWindow allWindow in RPOS.g_windows.allWindows)
			{
				if (!allWindow)
				{
					int num1 = num;
					num = num1 + 1;
					RPOS.g_windows.WindowRect windowRect1 = new RPOS.g_windows.WindowRect();
					windowRectArray[num1] = windowRect1;
				}
				else
				{
					RPOS.g_windows.ProcessTransform(ref matrix4x4, allWindow, ref RPOS.g_windows.lastZ, out bound);
					RPOS.g_windows.WindowRect windowRect2 = new RPOS.g_windows.WindowRect(bound);
					windowRect = (!windowRect.empty ? new RPOS.g_windows.WindowRect(windowRect, windowRect2) : windowRect2);
					int num2 = num;
					num = num2 + 1;
					windowRectArray[num2] = windowRect2;
				}
			}
			if (!RPOS.g_RPOS)
			{
				uIPanelArray = null;
			}
			else
			{
				uIPanelArray = RPOS.g_RPOS.keepTop;
			}
			uIPanelArray1 = uIPanelArray;
			if (uIPanelArray1 != null)
			{
				for (int j = 0; j < (int)uIPanelArray1.Length; j++)
				{
					if (uIPanelArray1[j])
					{
						RPOS.g_windows.ProcessTransform(uIPanelArray1[j].transform, ref RPOS.g_windows.lastZ);
					}
				}
			}
		}

		private static void ProcessTransform(Transform transform, ref float z)
		{
			AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(transform);
			Vector3 vector3 = transform.localPosition;
			Vector3 vector31 = aABBox.max;
			vector3.z = -(z + vector31.z);
			z = z + aABBox.size.z;
			transform.localPosition = vector3;
		}

		private static void ProcessTransform(ref Matrix4x4 toRoot, RPOSWindow window, ref float z, out Bounds bounds)
		{
			RPOS.g_windows.ProcessTransform(window.transform, ref z);
			Vector4 vector4 = window.windowDimensions;
			Matrix4x4 matrix4x4 = window.transform.localToWorldMatrix;
			bounds = new Bounds(toRoot.MultiplyPoint3x4(matrix4x4.MultiplyPoint3x4(new Vector3(vector4.x, vector4.y, 0f))), Vector3.zero);
			bounds.Encapsulate(toRoot.MultiplyPoint3x4(matrix4x4.MultiplyPoint3x4(new Vector3(vector4.x, vector4.y + vector4.w, 0f))));
			bounds.Encapsulate(toRoot.MultiplyPoint3x4(matrix4x4.MultiplyPoint3x4(new Vector3(vector4.x + vector4.z, vector4.y, 0f))));
			bounds.Encapsulate(toRoot.MultiplyPoint3x4(matrix4x4.MultiplyPoint3x4(new Vector3(vector4.x + vector4.z, vector4.y + vector4.w, 0f))));
		}

		private struct WindowRect
		{
			public int x;

			public int y;

			public ushort width;

			public ushort height;

			public int bottom
			{
				get
				{
					return this.y + this.height;
				}
			}

			public int center
			{
				get
				{
					return this.x + this.width / 2;
				}
			}

			public bool empty
			{
				get
				{
					return (this.width == 0 ? true : this.height == 0);
				}
			}

			public int left
			{
				get
				{
					return this.x;
				}
			}

			public int middle
			{
				get
				{
					return this.y + this.height / 2;
				}
			}

			public int right
			{
				get
				{
					return this.x + this.width;
				}
			}

			public int top
			{
				get
				{
					return this.y;
				}
			}

			public WindowRect(RPOS.g_windows.WindowRect a, RPOS.g_windows.WindowRect b)
			{
				int num;
				if (a.x >= b.x)
				{
					this.x = b.x;
					num = a.x + a.width - b.x;
					if (num >= b.width)
					{
						this.width = (ushort)num;
					}
					else
					{
						this.width = b.width;
					}
				}
				else
				{
					this.x = a.x;
					num = b.x + b.width - a.x;
					if (num >= a.width)
					{
						this.width = (ushort)num;
					}
					else
					{
						this.width = a.width;
					}
				}
				if (a.y >= b.y)
				{
					this.y = b.y;
					num = a.y + a.height - b.y;
					if (num >= b.height)
					{
						this.height = (ushort)num;
					}
					else
					{
						this.height = b.height;
					}
				}
				else
				{
					this.y = a.y;
					num = b.y + b.height - a.y;
					if (num >= a.height)
					{
						this.height = (ushort)num;
					}
					else
					{
						this.height = a.height;
					}
				}
			}

			public WindowRect(int x, int y, int width, int height)
			{
				if (width >= 0)
				{
					this.x = x;
					this.width = (ushort)width;
				}
				else
				{
					this.x = x + width;
					this.width = (ushort)(-width);
				}
				if (height >= 0)
				{
					this.y = y;
					this.height = (ushort)height;
				}
				else
				{
					this.y = y + height;
					this.height = (ushort)(-height);
				}
			}

			public WindowRect(int x, int y, ushort width, ushort height)
			{
				this.x = x;
				this.y = y;
				this.width = width;
				this.height = height;
			}

			public WindowRect(Bounds bounds)
			{
				Vector2 vector2 = bounds.center;
				Vector2 vector21 = bounds.extents;
				if (vector21.x >= 0f)
				{
					this.x = Mathf.FloorToInt(vector2.x - vector21.x);
					this.width = (ushort)Mathf.CeilToInt(vector2.x + vector21.x - (float)this.x);
				}
				else
				{
					this.x = Mathf.FloorToInt(vector2.x + vector21.x);
					this.width = (ushort)Mathf.CeilToInt(vector2.x - vector21.x - (float)this.x);
				}
				if (vector21.y >= 0f)
				{
					this.y = Mathf.FloorToInt(vector2.y - vector21.y);
					this.height = (ushort)Mathf.CeilToInt(vector2.y + vector21.y - (float)this.y);
				}
				else
				{
					this.y = Mathf.FloorToInt(vector2.y + vector21.y);
					this.height = (ushort)Mathf.CeilToInt(vector2.y - vector21.y - (float)this.y);
				}
			}

			public bool Contains(RPOS.g_windows.WindowRect other)
			{
				bool flag;
				bool flag1;
				if (this.x >= other.x)
				{
					flag = (this.x != other.x ? false : other.width < this.width);
				}
				else
				{
					flag = other.x + other.width - this.x <= this.width;
				}
				if (!flag)
				{
					flag1 = false;
				}
				else if (this.y >= other.y)
				{
					flag1 = (this.y != other.y ? false : other.height < this.height);
				}
				else
				{
					flag1 = other.y + other.height - this.y <= this.height;
				}
				return flag1;
			}

			public bool ContainsOrEquals(RPOS.g_windows.WindowRect other)
			{
				bool flag;
				bool flag1;
				if (other.x != this.x)
				{
					flag = (this.x >= other.x ? false : other.x + other.width - this.x <= this.width);
				}
				else
				{
					flag = other.width <= this.width;
				}
				if (!flag)
				{
					flag1 = false;
				}
				else if (other.y != this.y)
				{
					flag1 = (this.y >= other.y ? false : other.y + other.height - this.y <= this.height);
				}
				else
				{
					flag1 = other.height <= this.height;
				}
				return flag1;
			}

			public bool Equals(RPOS.g_windows.WindowRect other)
			{
				return (this.width != other.width || this.x != other.x || this.y != other.y ? false : this.height == other.height);
			}

			public bool Overlaps(RPOS.g_windows.WindowRect other)
			{
				bool flag;
				if ((other.x >= this.x ? this.x - other.x + this.width <= 0 : other.x + other.width <= this.x))
				{
					flag = false;
				}
				else
				{
					flag = (other.y >= this.y ? this.y - other.y + this.height > 0 : other.y + other.height > this.y);
				}
				return flag;
			}

			public bool OverlapsOrOutside(RPOS.g_windows.WindowRect other)
			{
				return (other.x < this.x || other.y < this.y || this.x - other.x + other.width > this.width ? true : this.y - other.y + this.height > this.height);
			}

			public bool OverlapsOrTouches(RPOS.g_windows.WindowRect other)
			{
				bool flag;
				if (other.x != this.x)
				{
					if ((other.x >= this.x ? this.x - other.x + this.width >= 0 : other.x + other.width >= this.x))
					{
						if (other.y == this.y)
						{
							flag = true;
						}
						else
						{
							flag = (other.y >= this.y ? this.y - other.y + this.height >= 0 : other.y + other.height >= this.y);
						}
						return flag;
					}
					flag = false;
					return flag;
				}
				if (other.y == this.y)
				{
					flag = true;
				}
				else
				{
					flag = (other.y >= this.y ? this.y - other.y + this.height >= 0 : other.y + other.height >= this.y);
				}
				return flag;
			}

			public bool OverlapsTouchesOrOutside(RPOS.g_windows.WindowRect other)
			{
				return (other.x <= this.x || other.y <= this.y || this.x - other.x + other.width >= this.width ? true : this.y - other.y + this.height >= this.height);
			}

			public override string ToString()
			{
				return string.Format("{{x:{0},y:{1},width:{2},height:{3}}}", new object[] { this.x, this.y, this.width, this.height });
			}
		}
	}
}