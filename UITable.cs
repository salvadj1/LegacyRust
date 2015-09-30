using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Table")]
[ExecuteInEditMode]
public class UITable : MonoBehaviour
{
	public int columns;

	public UITable.Direction direction;

	public Vector2 padding = Vector2.zero;

	public bool sorted;

	public bool hideInactive = true;

	public bool repositionNow;

	public bool keepWithinPanel;

	public UITable.OnReposition onReposition;

	private UIPanel mPanel;

	private UIDraggablePanel mDrag;

	private bool mStarted;

	public UITable()
	{
	}

	private void LateUpdate()
	{
		if (this.repositionNow)
		{
			this.repositionNow = false;
			this.Reposition();
			if (this.onReposition != null)
			{
				this.onReposition();
			}
		}
	}

	public void Reposition()
	{
		if (!this.mStarted)
		{
			this.repositionNow = true;
		}
		else
		{
			Transform transforms = base.transform;
			List<Transform> transforms1 = new List<Transform>();
			for (int i = 0; i < transforms.childCount; i++)
			{
				Transform child = transforms.GetChild(i);
				if (child && (!this.hideInactive || child.gameObject.activeInHierarchy))
				{
					transforms1.Add(child);
				}
			}
			if (this.sorted)
			{
				transforms1.Sort(new Comparison<Transform>(UITable.SortByName));
			}
			if (transforms1.Count > 0)
			{
				this.RepositionVariableSize(transforms1);
			}
			if (this.mPanel != null && this.mDrag == null)
			{
				this.mPanel.ConstrainTargetToBounds(transforms, true);
			}
			if (this.mDrag != null)
			{
				this.mDrag.UpdateScrollbars(true);
			}
		}
	}

	private void RepositionVariableSize(List<Transform> children)
	{
		float single = 0f;
		float single1 = 0f;
		int num = (this.columns <= 0 ? 1 : children.Count / this.columns + 1);
		int num1 = (this.columns <= 0 ? children.Count : this.columns);
		AABBox[,] aABBoxArray = new AABBox[num, num1];
		AABBox[] aABBoxArray1 = new AABBox[num1];
		AABBox[] aABBoxArray2 = new AABBox[num];
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int count = children.Count;
		while (num4 < count)
		{
			Transform item = children[num4];
			AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(item);
			Vector3 vector3 = item.localScale;
			aABBox.SetMinMax(Vector3.Scale(aABBox.min, vector3), Vector3.Scale(aABBox.max, vector3));
			aABBoxArray[num3, num2] = aABBox;
			aABBoxArray1[num2].Encapsulate(aABBox);
			aABBoxArray2[num3].Encapsulate(aABBox);
			int num5 = num2 + 1;
			num2 = num5;
			if (num5 >= this.columns && this.columns > 0)
			{
				num2 = 0;
				num3++;
			}
			num4++;
		}
		num2 = 0;
		num3 = 0;
		int num6 = 0;
		int count1 = children.Count;
		while (num6 < count1)
		{
			Transform transforms = children[num6];
			AABBox aABBox1 = aABBoxArray[num3, num2];
			AABBox aABBox2 = aABBoxArray1[num2];
			AABBox aABBox3 = aABBoxArray2[num3];
			Vector3 vector31 = transforms.localPosition;
			Vector3 vector32 = aABBox1.min;
			Vector3 vector33 = aABBox1.max;
			Vector3 vector34 = aABBox1.size * 0.5f;
			Vector3 vector35 = aABBox1.center;
			Vector3 vector36 = aABBox3.min;
			Vector3 vector37 = aABBox3.max;
			Vector3 vector38 = aABBox2.min;
			vector31.x = single + vector34.x - vector35.x;
			vector31.x = vector31.x + (vector32.x - vector38.x + this.padding.x);
			if (this.direction != UITable.Direction.Down)
			{
				vector31.y = single1 + vector34.y - vector35.y;
				vector31.y = vector31.y + ((vector33.y - vector32.y - vector37.y + vector36.y) * 0.5f - this.padding.y);
			}
			else
			{
				vector31.y = -single1 - vector34.y - vector35.y;
				vector31.y = vector31.y + ((vector33.y - vector32.y - vector37.y + vector36.y) * 0.5f - this.padding.y);
			}
			single = single + (vector38.x - vector38.x + this.padding.x * 2f);
			transforms.localPosition = vector31;
			int num7 = num2 + 1;
			num2 = num7;
			if (num7 >= this.columns && this.columns > 0)
			{
				num2 = 0;
				num3++;
				single = 0f;
				single1 = single1 + (vector34.y * 2f + this.padding.y * 2f);
			}
			num6++;
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.keepWithinPanel)
		{
			this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			this.mDrag = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
		}
		this.Reposition();
	}

	public enum Direction
	{
		Down,
		Up
	}

	public delegate void OnReposition();
}