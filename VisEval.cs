using System;
using System.Reflection;
using UnityEngine;

public class VisEval : ScriptableObject
{
	[SerializeField]
	private int[] data;

	[NonSerialized]
	private bool expanded;

	[NonSerialized]
	private Vis.Rule[] rules;

	private int dataCount
	{
		get
		{
			return (this.data != null ? (int)this.data.Length : 0);
		}
	}

	public Vis.Rule this[int i]
	{
		get
		{
			return Vis.Rule.Decode(this.data, i * 4);
		}
		set
		{
			Vis.Rule.Encode(ref value, this.data, i * 4);
			if (this.expanded)
			{
				this.rules[i] = value;
			}
		}
	}

	public int ruleCount
	{
		get
		{
			return this.dataCount / 4;
		}
	}

	public VisEval()
	{
	}

	public bool EditorOnly_Clear()
	{
		if (this.data == null)
		{
			return false;
		}
		this.data = null;
		return true;
	}

	public bool EditorOnly_Clone(int index)
	{
		if (index < 0 || index >= this.ruleCount)
		{
			return false;
		}
		this.EditorOnly_New();
		for (int i = this.ruleCount - 1; i > index; i--)
		{
			int num = i * 4;
			int num1 = (i - 1) * 4;
			for (int j = 0; j < 4; j++)
			{
				this.data[num] = this.data[i];
				num++;
				num1++;
			}
		}
		return true;
	}

	public bool EditorOnly_Delete(int index)
	{
		if (index < 0 || index >= this.ruleCount)
		{
			return false;
		}
		for (int i = index; i < this.ruleCount - 1; i++)
		{
			int num = i * 4;
			int num1 = (i + 1) * 4;
			for (int j = 0; j < 4; j++)
			{
				this.data[num] = this.data[i];
				num++;
				num1++;
			}
		}
		if (this.ruleCount != 1)
		{
			Array.Resize<int>(ref this.data, (int)this.data.Length - 4);
		}
		else
		{
			this.data = null;
		}
		return true;
	}

	public bool EditorOnly_MoveBottom(int index)
	{
		int num = index;
		index = num - 1;
		if (!this.EditorOnly_MoveUp(num))
		{
			return false;
		}
		while (true)
		{
			int num1 = index;
			index = num1 - 1;
			if (!this.EditorOnly_MoveUp(num1))
			{
				break;
			}
		}
		return true;
	}

	public bool EditorOnly_MoveDown(int index)
	{
		if (index >= this.ruleCount - 1)
		{
			return false;
		}
		this.Swap((index + 1) * 4, index * 4);
		return true;
	}

	public bool EditorOnly_MoveTop(int index)
	{
		int num = index;
		index = num - 1;
		if (!this.EditorOnly_MoveUp(num))
		{
			return false;
		}
		while (true)
		{
			int num1 = index;
			index = num1 - 1;
			if (!this.EditorOnly_MoveUp(num1))
			{
				break;
			}
		}
		return true;
	}

	public bool EditorOnly_MoveUp(int index)
	{
		if (index == 0)
		{
			return false;
		}
		if (index >= this.ruleCount)
		{
			return false;
		}
		this.Swap((index - 1) * 4, index * 4);
		return true;
	}

	public bool EditorOnly_New()
	{
		Array.Resize<int>(ref this.data, this.dataCount + 4);
		return true;
	}

	public bool GetMessage(Vis.Mask current, ref Vis.Mask previous, Vis.Mask other)
	{
		return false;
	}

	public bool Pass(Vis.Mask self, Vis.Mask instigator)
	{
		if (!this.expanded)
		{
			int num = this.ruleCount;
			if (num <= 0)
			{
				return true;
			}
			this.rules = new Vis.Rule[num];
			for (int i = 0; i < num; i++)
			{
				this.rules[i] = Vis.Rule.Decode(this.data, i * 4);
			}
			this.expanded = true;
		}
		for (int j = (int)this.rules.Length - 1; j >= 0; j--)
		{
			if (this.rules[j].Pass(self, instigator) != Vis.Rule.Failure.None)
			{
				return false;
			}
		}
		return true;
	}

	private void Swap(int i, int j)
	{
		int num = this.data[j];
		int num1 = j;
		j = num1 + 1;
		this.data[num1] = this.data[i];
		int num2 = i;
		i = num2 + 1;
		this.data[num2] = num;
		num = this.data[j];
		int num3 = j;
		j = num3 + 1;
		this.data[num3] = this.data[i];
		int num4 = i;
		i = num4 + 1;
		this.data[num4] = num;
		num = this.data[j];
		int num5 = j;
		j = num5 + 1;
		this.data[num5] = this.data[i];
		int num6 = i;
		i = num6 + 1;
		this.data[num6] = num;
	}
}