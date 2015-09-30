using System;
using System.Runtime.CompilerServices;
using System.Threading;

public static class StringEx
{
	private const int maxLockSize = 1024;

	private static uint lockCount;

	private readonly static char[] cb;

	private readonly static object cbLock;

	static StringEx()
	{
		StringEx.cb = new char[1024];
		StringEx.cbLock = new object();
	}

	private static string c2s(char[] c, int l)
	{
		return (l != 0 ? new string(c, 0, l) : string.Empty);
	}

	private static string c2s(int l, char[] c)
	{
		return (l != 0 ? new string(c, 0, l) : string.Empty);
	}

	public static string MakeNice(this string s)
	{
		char[] upper;
		string str;
		if (s != null)
		{
			int length = s.Length;
			int num = length;
			if (length > 1)
			{
				int num1 = -1;
				do
				{
					int num2 = num1 + 1;
					num1 = num2;
					if (num2 < num)
					{
						continue;
					}
					return string.Empty;
				}
				while (!char.IsLetterOrDigit(s, num1));
				if (num1 == num - 1)
				{
					return s.Substring(num - 1, 1);
				}
				bool flag = char.IsDigit(s, num1);
				bool flag1 = true;
				bool flag2 = true;
				int num3 = 0;
				StringEx.L l = StringEx.S(s, num - num1, (num - (num1 + 1)) * 2, out upper);
				try
				{
					if (l.V)
					{
						if (flag)
						{
							int num4 = num3;
							num3 = num4 + 1;
							upper[num4] = s[num1];
						}
						else
						{
							int num5 = num3;
							num3 = num5 + 1;
							upper[num5] = char.ToUpper(s[num1]);
						}
						while (true)
						{
							int num6 = num1 + 1;
							num1 = num6;
							if (num6 >= num)
							{
								break;
							}
							if (flag != char.IsNumber(s, num1))
							{
								flag = !flag;
								if (flag2)
								{
									flag2 = false;
								}
								else
								{
									int num7 = num3;
									num3 = num7 + 1;
									upper[num7] = ' ';
								}
								char[] chrArray = upper;
								int num8 = num3;
								num3 = num8 + 1;
								chrArray[num8] = (!flag ? char.ToUpperInvariant(s[num1]) : s[num1]);
								flag1 = true;
							}
							else if (flag)
							{
								int num9 = num3;
								num3 = num9 + 1;
								upper[num9] = s[num1];
							}
							else if (char.IsUpper(s, num1))
							{
								if (!flag1)
								{
									if (flag2)
									{
										flag2 = false;
									}
									else
									{
										int num10 = num3;
										num3 = num10 + 1;
										upper[num10] = ' ';
									}
									flag1 = true;
								}
								int num11 = num3;
								num3 = num11 + 1;
								upper[num11] = s[num1];
							}
							else if (char.IsLower(s, num1))
							{
								int num12 = num3;
								num3 = num12 + 1;
								upper[num12] = s[num1];
								flag1 = false;
							}
							else if (!flag2)
							{
								int num13 = num3;
								num3 = num13 + 1;
								upper[num13] = ' ';
								flag2 = true;
							}
						}
						str = StringEx.c2s(upper, (!flag2 ? num3 : num3 - 1));
					}
					else
					{
						str = s;
					}
				}
				finally
				{
					((IDisposable)(object)l).Dispose();
				}
				return str;
			}
		}
		return s;
	}

	[Obsolete("You gotta specify at least one char", true)]
	public static string RemoveChars(this string s)
	{
		return s;
	}

	public static string RemoveChars(this string s, params char[] rem)
	{
		char[] chrArray;
		string str;
		int length = (int)rem.Length;
		if (length == 0)
		{
			return s;
		}
		int num = (s != null ? s.Length : 0);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < length; j++)
			{
				if (s[i] == rem[j])
				{
					if (i == num - 1)
					{
						return s.Remove(num - 1);
					}
					StringEx.L l = StringEx.S(s, num, out chrArray);
					try
					{
						if (l.V)
						{
							int num1 = i;
							while (true)
							{
								int num2 = i + 1;
								i = num2;
								if (num2 >= num)
								{
									break;
								}
								for (j = 0; j < length; j++)
								{
									if (rem[j] == chrArray[i])
									{
									}
								}
								int num3 = num1;
								num1 = num3 + 1;
								chrArray[num3] = chrArray[i];
							}
							str = StringEx.c2s(chrArray, num1);
						}
						else
						{
							str = s;
						}
					}
					finally
					{
						((IDisposable)(object)l).Dispose();
					}
					return str;
				}
			}
		}
		return s;
	}

	public static string RemoveChars(this string s, char rem)
	{
		char[] chrArray;
		string str;
		int num = (s != null ? s.Length : 0);
		for (int i = 0; i < num; i++)
		{
			if (s[i] == rem)
			{
				if (i == num - 1)
				{
					return s.Remove(num - 1);
				}
				StringEx.L l = StringEx.S(s, num, out chrArray);
				try
				{
					if (l.V)
					{
						int num1 = i;
						while (true)
						{
							int num2 = i + 1;
							i = num2;
							if (num2 >= num)
							{
								break;
							}
							if (chrArray[i] != rem)
							{
								int num3 = num1;
								num1 = num3 + 1;
								chrArray[num3] = chrArray[i];
							}
						}
						str = StringEx.c2s(chrArray, num1);
					}
					else
					{
						str = s;
					}
				}
				finally
				{
					((IDisposable)(object)l).Dispose();
				}
				return str;
			}
		}
		return s;
	}

	public static string RemoveWhiteSpaces(this string s)
	{
		char[] chrArray;
		string str;
		int num = (s != null ? s.Length : 0);
		for (int i = 0; i < num; i++)
		{
			if (char.IsWhiteSpace(s[i]))
			{
				if (i == num - 1)
				{
					return s.Remove(num - 1);
				}
				StringEx.L l = StringEx.S(s, num, out chrArray);
				try
				{
					if (l.V)
					{
						int num1 = i;
						while (true)
						{
							int num2 = i + 1;
							i = num2;
							if (num2 >= num)
							{
								break;
							}
							if (!char.IsWhiteSpace(chrArray[i]))
							{
								int num3 = num1;
								num1 = num3 + 1;
								chrArray[num3] = chrArray[i];
							}
						}
						str = StringEx.c2s(chrArray, num1);
					}
					else
					{
						str = s;
					}
				}
				finally
				{
					((IDisposable)(object)l).Dispose();
				}
				return str;
			}
		}
		return s;
	}

	private static StringEx.L S(string s, int l, out char[] buffer)
	{
		if (s == null || l <= 0)
		{
			buffer = null;
			return new StringEx.L();
		}
		StringEx.L l1 = new StringEx.L(l <= 1024);
		if (!l1.locked)
		{
			buffer = s.ToCharArray();
		}
		else
		{
			char[] chrArray = StringEx.cb;
			char[] chrArray1 = chrArray;
			buffer = chrArray;
			s.CopyTo(0, chrArray1, 0, l);
		}
		return l1;
	}

	private static StringEx.L S(string s, int l, int minSafeSize, out char[] buffer)
	{
		if (s == null || l <= 0)
		{
			buffer = null;
			return new StringEx.L();
		}
		StringEx.L l1 = new StringEx.L(minSafeSize <= 1024);
		if (!l1.locked)
		{
			buffer = s.ToCharArray();
		}
		else
		{
			char[] chrArray = StringEx.cb;
			char[] chrArray1 = chrArray;
			buffer = chrArray;
			s.CopyTo(0, chrArray1, 0, l);
		}
		return l1;
	}

	public static string ToLowerEx(this string s)
	{
		char[] lowerInvariant;
		string str;
		int num;
		int num1 = (s != null ? s.Length : 0);
		for (int i = 0; i < num1; i++)
		{
			if (char.IsUpper(s, i))
			{
				StringEx.L l = StringEx.S(s, num1, out lowerInvariant);
				try
				{
					if (l.V)
					{
						do
						{
							lowerInvariant[i] = char.ToLowerInvariant(lowerInvariant[i]);
							num = i + 1;
							i = num;
						}
						while (num < num1);
						str = StringEx.c2s(lowerInvariant, num1);
					}
					else
					{
						str = s;
					}
				}
				finally
				{
					((IDisposable)(object)l).Dispose();
				}
				return str;
			}
		}
		return s;
	}

	public static string ToUpperEx(this string s)
	{
		char[] upperInvariant;
		string str;
		int num;
		int num1 = (s != null ? s.Length : 0);
		for (int i = 0; i < num1; i++)
		{
			if (char.IsLower(s, i))
			{
				StringEx.L l = StringEx.S(s, num1, out upperInvariant);
				try
				{
					if (l.V)
					{
						do
						{
							upperInvariant[i] = char.ToUpperInvariant(upperInvariant[i]);
							num = i + 1;
							i = num;
						}
						while (num < num1);
						str = StringEx.c2s(upperInvariant, num1);
					}
					else
					{
						str = s;
					}
				}
				finally
				{
					((IDisposable)(object)l).Dispose();
				}
				return str;
			}
		}
		return s;
	}

	private struct L : IDisposable
	{
		private bool _locked;

		private bool _valid;

		public bool locked
		{
			get
			{
				return this._locked;
			}
		}

		public bool V
		{
			get
			{
				return this._valid;
			}
		}

		public L(bool locked)
		{
			this._locked = (!locked ? false : Monitor.TryEnter(StringEx.cbLock));
			this._valid = true;
		}

		public void Dispose()
		{
			if (this._locked)
			{
				Monitor.Exit(StringEx.cbLock);
				this._locked = false;
			}
		}
	}
}