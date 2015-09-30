using System;
using UnityEngine;

public class UINode
{
	private int mVisibleFlag = -1;

	public Transform trans;

	public UIWidget widget;

	public bool lastActive;

	public Vector3 lastPos;

	public Quaternion lastRot;

	public Vector3 lastScale;

	public int changeFlag = -1;

	private GameObject mGo;

	public int visibleFlag
	{
		get
		{
			return (this.widget == null ? this.mVisibleFlag : this.widget.visibleFlag);
		}
		set
		{
			if (this.widget == null)
			{
				this.mVisibleFlag = value;
			}
			else
			{
				this.widget.visibleFlag = value;
			}
		}
	}

	public UINode(Transform t)
	{
		this.trans = t;
		this.lastPos = this.trans.localPosition;
		this.lastRot = this.trans.localRotation;
		this.lastScale = this.trans.localScale;
		this.mGo = t.gameObject;
	}

	public bool HasChanged()
	{
		bool flag;
		if (!this.mGo.activeInHierarchy)
		{
			flag = false;
		}
		else if (this.widget == null)
		{
			flag = true;
		}
		else
		{
			flag = (!this.widget.enabled ? false : this.widget.color.a > 0.001f);
		}
		bool flag1 = flag;
		if (this.lastActive == flag1 && (!flag1 || !(this.lastPos != this.trans.localPosition) && !(this.lastRot != this.trans.localRotation) && !(this.lastScale != this.trans.localScale)))
		{
			return false;
		}
		this.lastActive = flag1;
		this.lastPos = this.trans.localPosition;
		this.lastRot = this.trans.localRotation;
		this.lastScale = this.trans.localScale;
		return true;
	}
}