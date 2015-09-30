using System;
using UnityEngine;

public class VisActionMessageEnter : VisAction
{
	[SerializeField]
	protected string selfMessage = string.Empty;

	[SerializeField]
	protected string instigatorMessage = string.Empty;

	[SerializeField]
	protected bool withOtherAsArg = true;

	[SerializeField]
	protected bool swapMessageOrder;

	[SerializeField]
	protected bool selfNonNull;

	[SerializeField]
	protected bool instigatorNonNull;

	public VisActionMessageEnter()
	{
	}

	public override void Accomplish(IDMain self, IDMain instigator)
	{
		string str;
		string str1;
		bool flag = !self;
		bool flag1 = !instigator;
		if (flag)
		{
			if (!flag1)
			{
				if (this.selfNonNull)
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
			if (this.instigatorNonNull)
			{
				return;
			}
			Debug.LogWarning("Instigator is null!", this);
		}
		if (!this.swapMessageOrder)
		{
			str = this.selfMessage;
			str1 = this.instigatorMessage;
		}
		else
		{
			IDMain dMain = self;
			self = instigator;
			instigator = dMain;
			str = this.instigatorMessage;
			str1 = this.selfMessage;
			bool flag2 = flag;
			flag = flag1;
			flag1 = flag2;
		}
		if (!this.withOtherAsArg)
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

	public override void UnAcomplish(IDMain self, IDMain instigator)
	{
	}
}