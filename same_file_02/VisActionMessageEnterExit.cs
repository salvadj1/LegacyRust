using System;
using UnityEngine;

public class VisActionMessageEnterExit : VisActionMessageEnter
{
	[SerializeField]
	protected string exitSelfMessage = string.Empty;

	[SerializeField]
	protected string exitInstigatorMessage = string.Empty;

	[SerializeField]
	protected bool exitWithOtherAsArg = true;

	[SerializeField]
	protected bool exitSwapMessageOrder;

	[SerializeField]
	protected bool exitSelfNonNull;

	[SerializeField]
	protected bool exitInstigatorNonNull;

	public VisActionMessageEnterExit()
	{
	}

	public override void UnAcomplish(IDMain self, IDMain instigator)
	{
		string str;
		string str1;
		bool flag = !self;
		bool flag1 = !instigator;
		if (flag)
		{
			if (!flag1)
			{
				if (this.exitSelfNonNull)
				{
					return;
				}
				Debug.LogWarning("Self is null!", this);
			}
			else
			{
				Debug.LogError("Self and instgator are null", this);
			}
		}
		else if (flag1)
		{
			if (this.exitInstigatorNonNull)
			{
				return;
			}
			Debug.LogWarning("Instigator is null!", this);
		}
		if (!this.exitSwapMessageOrder)
		{
			str = this.exitSelfMessage;
			str1 = this.exitInstigatorMessage;
		}
		else
		{
			IDMain dMain = self;
			self = instigator;
			instigator = dMain;
			str = this.exitInstigatorMessage;
			str1 = this.exitSelfMessage;
			bool flag2 = flag;
			flag = flag1;
			flag1 = flag2;
		}
		if (!this.exitWithOtherAsArg)
		{
			if (!flag && !string.IsNullOrEmpty(str))
			{
				self.SendMessage(str, SendMessageOptions.DontRequireReceiver);
			}
			if (!flag1 && !string.IsNullOrEmpty(str1))
			{
				instigator.SendMessage(str1, SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			if (!flag && !string.IsNullOrEmpty(str))
			{
				self.SendMessage(str, instigator, SendMessageOptions.DontRequireReceiver);
			}
			if (!flag1 && !string.IsNullOrEmpty(str1))
			{
				instigator.SendMessage(str1, self, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}