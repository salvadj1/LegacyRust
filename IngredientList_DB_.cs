using Facepunch.Hash;
using System;
using System.Reflection;

public sealed class IngredientList<DB> : IngredientList, IEquatable<IngredientList<DB>>
where DB : Datablock, IComparable<DB>
{
	public DB[] unsorted;

	private DB[] sorted;

	private bool madeHashCode;

	private uint hash;

	private string lastToString;

	public uint hashCode
	{
		get
		{
			DB[] dBArray;
			if (this.madeHashCode)
			{
				if (!this.needReSort)
				{
					return this.hash;
				}
				this.ReSort();
				dBArray = this.sorted;
			}
			else
			{
				dBArray = this.ordered;
			}
			int length = (int)dBArray.Length;
			if (length > (int)IngredientList.tempHash.Length)
			{
				Array.Resize<int>(ref IngredientList.tempHash, length);
			}
			for (int i = 0; i < length; i++)
			{
				IngredientList.tempHash[i] = dBArray[i].uniqueID;
			}
			this.hash = MurmurHash2.UINT(IngredientList.tempHash, length, -267518227);
			this.madeHashCode = true;
			return this.hash;
		}
	}

	private bool needReSort
	{
		get
		{
			if (this.sorted != null && (int)this.sorted.Length == (int)this.unsorted.Length)
			{
				return false;
			}
			return true;
		}
	}

	public DB[] ordered
	{
		get
		{
			if (this.needReSort)
			{
				this.ReSort();
			}
			return this.sorted;
		}
	}

	public string text
	{
		get
		{
			string str = this.lastToString;
			if (str == null)
			{
				string str1 = IngredientList<DB>.Combine(this.ordered);
				string str2 = str1;
				this.lastToString = str1;
				str = str2;
			}
			return str;
		}
	}

	public IngredientList(DB[] unsorted)
	{
		this.unsorted = unsorted ?? new DB[0];
		if ((int)unsorted.Length > 255)
		{
			throw new ArgumentException("items in list cannot exceed 255");
		}
		this.sorted = null;
		this.lastToString = null;
	}

	private static string Combine(DB[] dbs)
	{
		string[] str = new string[(int)dbs.Length];
		for (int i = 0; i < (int)dbs.Length; i++)
		{
			str[i] = Convert.ToString(dbs[i]);
		}
		return string.Join(",", str);
	}

	public IngredientList<DB> EnsureContents(DB[] original)
	{
		if (this.unsorted != original)
		{
			this.sorted = null;
			this.lastToString = null;
			this.madeHashCode = false;
			this.unsorted = original;
		}
		return this;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is IngredientList<DB>))
		{
			return false;
		}
		return this.Equals((IngredientList<DB>)obj);
	}

	public bool Equals(IngredientList<DB> other)
	{
		if (object.ReferenceEquals(other, this))
		{
			return true;
		}
		if (other == null || (int)this.unsorted.Length != (int)other.unsorted.Length || this.hashCode != other.hashCode)
		{
			return false;
		}
		DB[] dBArray = this.sorted;
		DB[] dBArray1 = other.sorted;
		int num = 0;
		int length = (int)dBArray.Length;
		while (num < length)
		{
			if (dBArray[num] != dBArray1[num])
			{
				return false;
			}
			int num1 = length - 1;
			length = num1;
			if (num1 <= num)
			{
				break;
			}
			else
			{
				if (dBArray[length] != dBArray1[length])
				{
					return false;
				}
				num++;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		return (int)this.hashCode;
	}

	private void ReSort()
	{
		int length = (int)this.unsorted.Length;
		Array.Resize<DB>(ref this.sorted, length);
		Array.Copy(this.unsorted, this.sorted, length);
		if (length > 255)
		{
			throw new InvalidOperationException("There can't be more than 255 ingredients per blueprint");
		}
		Array.Sort<DB>(this.sorted);
	}

	public override string ToString()
	{
		return string.Format("[IngredientList<{0}>[{1}]{2:X}:{3}]", new object[] { typeof(DB).Name, (int)this.unsorted.Length, this.hashCode, this.text });
	}
}