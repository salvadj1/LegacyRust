using System;
using UnityEngine;

public class Pagination : MonoBehaviour
{
	public GameObject clickableButton;

	public GameObject spacerLabel;

	public int buttonGroups = 2;

	protected int pageCount;

	protected int pageCurrent;

	private Pagination.SwitchToPage OnPageSwitch;

	public Pagination()
	{
	}

	public void DropSpacer(ref Vector3 vPos)
	{
		if (!this.spacerLabel)
		{
			return;
		}
		dfControl component = base.GetComponent<dfControl>();
		dfControl _dfControl = ((GameObject)UnityEngine.Object.Instantiate(this.spacerLabel)).GetComponent<dfControl>();
		component.AddControl(_dfControl);
		_dfControl.Position = vPos;
		vPos.x = vPos.x + (_dfControl.Width + 5f);
	}

	public void OnButtonClicked(dfControl control, dfMouseEventArgs mouseEvent)
	{
		int num = int.Parse(control.Tooltip);
		this.Setup(this.pageCount, num);
		if (this.OnPageSwitch != null)
		{
			this.OnPageSwitch(num);
		}
	}

	public void Setup(int iPages, int iCurrentPage)
	{
		if (this.pageCount == iPages && this.pageCurrent == iCurrentPage)
		{
			return;
		}
		this.pageCount = iPages;
		this.pageCurrent = iCurrentPage;
		dfControl[] componentsInChildren = base.gameObject.GetComponentsInChildren<dfControl>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			dfControl _dfControl = componentsInChildren[i];
			if (_dfControl.gameObject != base.gameObject)
			{
				UnityEngine.Object.Destroy(_dfControl.gameObject);
			}
		}
		if (this.pageCount <= 1)
		{
			return;
		}
		dfControl component = base.GetComponent<dfControl>();
		bool flag = true;
		Vector3 vector3 = new Vector3(0f, 0f, 0f);
		for (int j = 0; j < this.pageCount; j++)
		{
			if (this.buttonGroups - j > 0 || j >= this.pageCount - this.buttonGroups || Math.Abs(j - this.pageCurrent) <= this.buttonGroups / 2)
			{
				dfButton str = ((GameObject)UnityEngine.Object.Instantiate(this.clickableButton)).GetComponent<dfButton>();
				component.AddControl(str);
				str.Tooltip = j.ToString();
				str.MouseDown += new MouseEventHandler(this.OnButtonClicked);
				str.Text = (j + 1).ToString();
				str.Invalidate();
				if (j == this.pageCurrent)
				{
					str.Disable();
				}
				str.Position = vector3;
				vector3.x = vector3.x + (str.Width + 5f);
				flag = true;
			}
			else
			{
				if (flag)
				{
					this.DropSpacer(ref vector3);
				}
				flag = false;
			}
		}
		component.Width = vector3.x;
	}

	public event Pagination.SwitchToPage OnPageSwitch
	{
		add
		{
			this.OnPageSwitch += value;
		}
		remove
		{
			this.OnPageSwitch -= value;
		}
	}

	public delegate void SwitchToPage(int iPage);
}