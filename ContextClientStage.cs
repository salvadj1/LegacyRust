using System;
using System.Text;

internal struct ContextClientStage
{
	[NonSerialized]
	public ContextClientStageMenuItem[] option;

	[NonSerialized]
	public int length;

	public void Set(ContextMenuData data)
	{
		if (this.length >= data.options_length)
		{
			while (this.length > data.options_length)
			{
				ContextClientStage contextClientStage = this;
				int num = contextClientStage.length - 1;
				int num1 = num;
				contextClientStage.length = num;
				this.option[num1].text = null;
			}
		}
		else
		{
			this.option = new ContextClientStageMenuItem[data.options_length];
			this.length = data.options_length;
		}
		for (int i = 0; i < data.options_length; i++)
		{
			this.option[i].name = data.options[i].name;
			if (data.options[i].utf8_length != 0)
			{
				this.option[i].text = Encoding.UTF8.GetString(data.options[i].utf8_text, 0, data.options[i].utf8_length);
			}
			else
			{
				this.option[i].text = string.Empty;
			}
		}
	}
}