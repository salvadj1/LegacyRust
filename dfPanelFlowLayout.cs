using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Panel Addon/Flow Layout")]
[ExecuteInEditMode]
[RequireComponent(typeof(dfPanel))]
public class dfPanelFlowLayout : MonoBehaviour
{
	[SerializeField]
	protected RectOffset borderPadding = new RectOffset();

	[SerializeField]
	protected Vector2 itemSpacing = new Vector2();

	[SerializeField]
	protected dfControlOrientation flowDirection;

	[SerializeField]
	protected bool hideClippedControls;

	private dfPanel panel;

	public RectOffset BorderPadding
	{
		get
		{
			if (this.borderPadding == null)
			{
				this.borderPadding = new RectOffset();
			}
			return this.borderPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.borderPadding))
			{
				this.borderPadding = value;
				this.performLayout();
			}
		}
	}

	public dfControlOrientation Direction
	{
		get
		{
			return this.flowDirection;
		}
		set
		{
			if (value != this.flowDirection)
			{
				this.flowDirection = value;
				this.performLayout();
			}
		}
	}

	public bool HideClippedControls
	{
		get
		{
			return this.hideClippedControls;
		}
		set
		{
			if (value != this.hideClippedControls)
			{
				this.hideClippedControls = value;
				this.performLayout();
			}
		}
	}

	public Vector2 ItemSpacing
	{
		get
		{
			return this.itemSpacing;
		}
		set
		{
			value = Vector2.Max(value, Vector2.zero);
			if (!object.Equals(value, this.itemSpacing))
			{
				this.itemSpacing = value;
				this.performLayout();
			}
		}
	}

	public dfPanelFlowLayout()
	{
	}

	private bool canShowControlUnclipped(dfControl control)
	{
		if (!this.hideClippedControls)
		{
			return true;
		}
		Vector3 relativePosition = control.RelativePosition;
		if (relativePosition.x + control.Width >= this.panel.Width - (float)this.borderPadding.right)
		{
			return false;
		}
		if (relativePosition.y + control.Height >= this.panel.Height - (float)this.borderPadding.bottom)
		{
			return false;
		}
		return true;
	}

	private void child_SizeChanged(dfControl control, Vector2 value)
	{
		this.performLayout();
	}

	private void child_ZOrderChanged(dfControl control, int value)
	{
		this.performLayout();
	}

	public void OnControlAdded(dfControl container, dfControl child)
	{
		child.ZOrderChanged += new PropertyChangedEventHandler<int>(this.child_ZOrderChanged);
		child.SizeChanged += new PropertyChangedEventHandler<Vector2>(this.child_SizeChanged);
		this.performLayout();
	}

	public void OnControlRemoved(dfControl container, dfControl child)
	{
		child.ZOrderChanged -= new PropertyChangedEventHandler<int>(this.child_ZOrderChanged);
		child.SizeChanged -= new PropertyChangedEventHandler<Vector2>(this.child_SizeChanged);
		this.performLayout();
	}

	public void OnEnable()
	{
		this.panel = base.GetComponent<dfPanel>();
		this.panel.SizeChanged += new PropertyChangedEventHandler<Vector2>(this.OnSizeChanged);
	}

	public void OnSizeChanged(dfControl control, Vector2 value)
	{
		this.performLayout();
	}

	private void performLayout()
	{
		if (this.panel == null)
		{
			this.panel = base.GetComponent<dfPanel>();
		}
		Vector3 vector3 = new Vector3((float)this.borderPadding.left, (float)this.borderPadding.top);
		bool flag = true;
		float width = this.panel.Width - (float)this.borderPadding.right;
		float height = this.panel.Height - (float)this.borderPadding.bottom;
		int num = 0;
		IList<dfControl> controls = this.panel.Controls;
		int num1 = 0;
		while (num1 < controls.Count)
		{
			if (!flag)
			{
				if (this.flowDirection != dfControlOrientation.Horizontal)
				{
					vector3.y = vector3.y + this.itemSpacing.y;
				}
				else
				{
					vector3.x = vector3.x + this.itemSpacing.x;
				}
			}
			dfControl item = controls[num1];
			if (this.flowDirection == dfControlOrientation.Horizontal)
			{
				if (!flag && vector3.x + item.Width >= width)
				{
					vector3.x = (float)this.borderPadding.left;
					vector3.y = vector3.y + (float)num;
					num = 0;
					flag = true;
				}
			}
			else if (!flag && vector3.y + item.Height >= height)
			{
				vector3.y = (float)this.borderPadding.top;
				vector3.x = vector3.x + (float)num;
				num = 0;
				flag = true;
			}
			item.RelativePosition = vector3;
			if (this.flowDirection != dfControlOrientation.Horizontal)
			{
				vector3.y = vector3.y + item.Height;
				num = Mathf.Max(Mathf.CeilToInt(item.Width + this.itemSpacing.x), num);
			}
			else
			{
				vector3.x = vector3.x + item.Width;
				num = Mathf.Max(Mathf.CeilToInt(item.Height + this.itemSpacing.y), num);
			}
			item.IsVisible = this.canShowControlUnclipped(item);
			num1++;
			flag = false;
		}
	}
}