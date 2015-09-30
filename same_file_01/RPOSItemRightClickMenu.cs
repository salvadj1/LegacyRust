using System;
using UnityEngine;

public class RPOSItemRightClickMenu : MonoBehaviour
{
	private IInventoryItem _observedItem;

	public GameObject _buttonPrefab;

	public Camera uiCamera;

	private Plane planeTest;

	public float lastHeight;

	private readonly static InventoryItem.MenuItem[] menuItemBuffer;

	static RPOSItemRightClickMenu()
	{
		RPOSItemRightClickMenu.menuItemBuffer = new InventoryItem.MenuItem[30];
	}

	public RPOSItemRightClickMenu()
	{
	}

	public void AddRightClickEntry(string entry)
	{
		GameObject gameObject = NGUITools.AddChild(base.gameObject, this._buttonPrefab);
		gameObject.GetComponentInChildren<UILabel>().text = entry;
		UIEventListener.Get(gameObject).onClick += new UIEventListener.VoidDelegate(this.EntryClicked);
		gameObject.name = entry;
		Vector3 vector3 = gameObject.transform.localPosition;
		vector3.y = this.lastHeight;
		gameObject.transform.localPosition = vector3;
		RPOSItemRightClickMenu rPOSItemRightClickMenu = this;
		float single = rPOSItemRightClickMenu.lastHeight;
		Vector3 componentInChildren = gameObject.GetComponentInChildren<UISlicedSprite>().transform.localScale;
		rPOSItemRightClickMenu.lastHeight = single - componentInChildren.y;
	}

	public void Awake()
	{
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.planeTest = new Plane(this.uiCamera.transform.forward * 1f, new Vector3(0f, 0f, 2f));
		base.GetComponent<UIPanel>().enabled = false;
	}

	public void ClearChildren()
	{
		UIButton[] componentsInChildren = base.GetComponentsInChildren<UIButton>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		this.lastHeight = 0f;
	}

	public void EntryClicked(GameObject go)
	{
		InventoryItem.MenuItem? nullable;
		try
		{
			try
			{
				if (this._observedItem != null)
				{
					try
					{
						nullable = new InventoryItem.MenuItem?((InventoryItem.MenuItem)((byte)Enum.Parse(typeof(InventoryItem.MenuItem), go.name, true)));
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						nullable = null;
						Debug.LogException(exception);
					}
					if (nullable.HasValue)
					{
						this._observedItem.OnMenuOption(nullable.Value);
					}
				}
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
		}
		finally
		{
			UICamera.UnPopupPanel(base.GetComponent<UIPanel>());
		}
	}

	private void PopupEnd()
	{
		base.GetComponent<UIPanel>().enabled = false;
	}

	private void PopupStart()
	{
		this.RepositionAtCursor();
		base.GetComponent<UIPanel>().enabled = true;
	}

	public void RepositionAtCursor()
	{
		Vector3 vector3 = UICamera.lastMousePosition;
		Ray ray = this.uiCamera.ScreenPointToRay(vector3);
		float single = 0f;
		if (this.planeTest.Raycast(ray, out single))
		{
			base.transform.position = ray.GetPoint(single);
			AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(base.transform);
			float single1 = base.transform.localPosition.x;
			Vector3 vector31 = aABBox.size;
			float single2 = single1 + vector31.x - (float)Screen.width;
			if (single2 > 0f)
			{
				Transform transforms = base.transform;
				Vector3 vector32 = base.transform.localPosition;
				transforms.SetLocalPositionX(vector32.x - single2);
			}
			float single3 = base.transform.localPosition.y;
			Vector3 vector33 = aABBox.size;
			float single4 = single3 + vector33.y - (float)Screen.height;
			if (single4 > 0f)
			{
				Transform transforms1 = base.transform;
				Vector3 vector34 = base.transform.localPosition;
				transforms1.SetLocalPositionY(vector34.y - single4);
			}
			Transform transforms2 = base.transform;
			float single5 = base.transform.localPosition.x;
			Vector3 vector35 = base.transform.localPosition;
			transforms2.localPosition = new Vector3(single5, vector35.y, -180f);
		}
	}

	public virtual void SetItem(IInventoryItem item)
	{
		this.ClearChildren();
		this._observedItem = item;
		int num = item.datablock.RetreiveMenuOptions(item, RPOSItemRightClickMenu.menuItemBuffer, 0);
		for (int i = 0; i < num; i++)
		{
			this.AddRightClickEntry(RPOSItemRightClickMenu.menuItemBuffer[i].ToString());
		}
		UICamera.PopupPanel(base.GetComponent<UIPanel>());
	}
}