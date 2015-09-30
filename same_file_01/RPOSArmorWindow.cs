using System;
using UnityEngine;

public class RPOSArmorWindow : RPOSWindow
{
	public UILabel leftText;

	public UILabel rightText;

	public RPOSInvCellManager cellMan;

	public RPOSArmorWindow()
	{
	}

	public void ForceUpdate()
	{
		DamageTypeList damageTypeList;
		HumanBodyTakeDamage humanBodyTakeDamage;
		damageTypeList = (!RPOS.GetObservedPlayerComponent<HumanBodyTakeDamage>(out humanBodyTakeDamage) ? new DamageTypeList() : humanBodyTakeDamage.GetArmorValues());
		this.leftText.text = string.Empty;
		this.rightText.text = string.Empty;
		for (int i = 0; i < 6; i++)
		{
			if (damageTypeList[i] != 0f)
			{
				UILabel uILabel = this.leftText;
				uILabel.text = string.Concat(uILabel.text, TakeDamage.DamageIndexToString((DamageTypeIndex)i), "\n");
				UILabel uILabel1 = this.rightText;
				string str = uILabel1.text;
				uILabel1.text = string.Concat(new object[] { str, "+", (int)damageTypeList[i], "\n" });
			}
		}
	}

	protected override void WindowAwake()
	{
		base.WindowAwake();
		this.cellMan = base.GetComponentInChildren<RPOSInvCellManager>();
	}
}