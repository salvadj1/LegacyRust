using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public sealed class DamageTypeList
{
	private const int kDamageIndexCount = 6;

	[SerializeField]
	private float[] damageArray;

	public float this[int index]
	{
		get
		{
			if (index < 0 || index >= 6)
			{
				throw new IndexOutOfRangeException();
			}
			return (this.damageArray == null || (int)this.damageArray.Length <= index ? 0f : this.damageArray[index]);
		}
		set
		{
			if (index < 0 || index >= 6)
			{
				throw new IndexOutOfRangeException();
			}
			if (this.damageArray == null || (int)this.damageArray.Length <= index)
			{
				Array.Resize<float>(ref this.damageArray, 6);
			}
			this.damageArray[index] = value;
		}
	}

	public float this[DamageTypeIndex index]
	{
		get
		{
			return this[(int)index];
		}
		set
		{
			this[(int)index] = value;
		}
	}

	public DamageTypeList()
	{
	}

	public DamageTypeList(DamageTypeList copyFrom) : this()
	{
		if (copyFrom == null || copyFrom.damageArray == null)
		{
			this.damageArray = new float[6];
		}
		else if ((int)copyFrom.damageArray.Length != 6)
		{
			this.damageArray = new float[6];
			if ((int)copyFrom.damageArray.Length <= 6)
			{
				for (int i = 0; i < (int)copyFrom.damageArray.Length; i++)
				{
					this.damageArray[i] = copyFrom.damageArray[i];
				}
			}
			else
			{
				for (int j = 0; j < 6; j++)
				{
					this.damageArray[j] = copyFrom.damageArray[j];
				}
			}
		}
		else
		{
			this.damageArray = (float[])copyFrom.damageArray.Clone();
		}
	}

	public DamageTypeList(float generic, float bullet, float melee, float explosion, float radiation, float cold)
	{
		this.damageArray = new float[] { generic, bullet, melee, explosion, radiation, cold };
	}

	public void SetArmorValues(DamageTypeList copyFrom)
	{
		int j;
		if (this.damageArray != null && (int)this.damageArray.Length == 6)
		{
			if (copyFrom.damageArray == null)
			{
				if (this.damageArray == null || (int)this.damageArray.Length != 6)
				{
					this.damageArray = new float[6];
				}
				else
				{
					for (int i = 0; i < 6; i++)
					{
						this.damageArray[i] = 0f;
					}
				}
			}
			else if ((int)copyFrom.damageArray.Length < 6)
			{
				for (j = 0; j < (int)copyFrom.damageArray.Length; j++)
				{
					this.damageArray[j] = copyFrom.damageArray[j];
				}
				while (j < 6)
				{
					int num = j;
					j = num + 1;
					this.damageArray[num] = 0f;
				}
			}
			else
			{
				for (int k = 0; k < 6; k++)
				{
					this.damageArray[k] = copyFrom.damageArray[k];
				}
			}
		}
		else if (copyFrom == null || copyFrom.damageArray == null)
		{
			this.damageArray = new float[6];
		}
		else if ((int)copyFrom.damageArray.Length != 6)
		{
			this.damageArray = new float[6];
			if ((int)copyFrom.damageArray.Length <= 6)
			{
				for (int l = 0; l < (int)copyFrom.damageArray.Length; l++)
				{
					this.damageArray[l] = copyFrom.damageArray[l];
				}
			}
			else
			{
				for (int m = 0; m < 6; m++)
				{
					this.damageArray[m] = copyFrom.damageArray[m];
				}
			}
		}
		else
		{
			this.damageArray = (float[])copyFrom.damageArray.Clone();
		}
	}
}