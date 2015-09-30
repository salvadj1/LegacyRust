using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Grid")]
[ExecuteInEditMode]
public class UIGrid : MonoBehaviour
{
	public UIGrid.Arrangement arrangement;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool repositionNow;

	public bool sorted;

	public bool hideInactive = true;

	private bool mStarted;

	public UIGrid()
	{
	}

	public void Reposition()
	{
		if (!this.mStarted)
		{
			this.repositionNow = true;
			return;
		}
		Transform transforms = base.transform;
		int num = 0;
		int num1 = 0;
		if (!this.sorted)
		{
			for (int i = 0; i < transforms.childCount; i++)
			{
				Transform child = transforms.GetChild(i);
				if (child.gameObject.activeInHierarchy || !this.hideInactive)
				{
					float single = child.localPosition.z;
					child.localPosition = (this.arrangement != UIGrid.Arrangement.Horizontal ? new Vector3(this.cellWidth * (float)num1, -this.cellHeight * (float)num, single) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num1, single));
					int num2 = num + 1;
					num = num2;
					if (num2 >= this.maxPerLine && this.maxPerLine > 0)
					{
						num = 0;
						num1++;
					}
				}
			}
		}
		else
		{
			List<Transform> transforms1 = new List<Transform>();
			for (int j = 0; j < transforms.childCount; j++)
			{
				Transform child1 = transforms.GetChild(j);
				if (child1)
				{
					transforms1.Add(child1);
				}
			}
			transforms1.Sort(new Comparison<Transform>(UIGrid.SortByName));
			int num3 = 0;
			int count = transforms1.Count;
			while (num3 < count)
			{
				Transform item = transforms1[num3];
				if (item.gameObject.activeInHierarchy || !this.hideInactive)
				{
					float single1 = item.localPosition.z;
					item.localPosition = (this.arrangement != UIGrid.Arrangement.Horizontal ? new Vector3(this.cellWidth * (float)num1, -this.cellHeight * (float)num, single1) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num1, single1));
					int num4 = num + 1;
					num = num4;
					if (num4 >= this.maxPerLine && this.maxPerLine > 0)
					{
						num = 0;
						num1++;
					}
				}
				num3++;
			}
		}
		UIDraggablePanel uIDraggablePanel = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
		if (uIDraggablePanel != null)
		{
			uIDraggablePanel.UpdateScrollbars(true);
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	private void Start()
	{
		this.mStarted = true;
		this.Reposition();
	}

	private void Update()
	{
		if (this.repositionNow)
		{
			this.repositionNow = false;
			this.Reposition();
		}
	}

	public enum Arrangement
	{
		Horizontal,
		Vertical
	}
}