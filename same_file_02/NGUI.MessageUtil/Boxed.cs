using System;
using UnityEngine;

namespace NGUI.MessageUtil
{
	public static class Boxed
	{
		public readonly static object @true;

		public readonly static object @false;

		public readonly static object int_0;

		public readonly static object int_1;

		public readonly static object int_2;

		public readonly static object key_escape;

		public readonly static object key_left;

		public readonly static object key_right;

		public readonly static object key_up;

		public readonly static object key_down;

		public readonly static object key_tab;

		public readonly static object key_none;

		static Boxed()
		{
			Boxed.@true = true;
			Boxed.@false = false;
			Boxed.int_0 = 0;
			Boxed.int_1 = 1;
			Boxed.int_2 = 2;
			Boxed.key_escape = KeyCode.Escape;
			Boxed.key_left = KeyCode.LeftArrow;
			Boxed.key_right = KeyCode.RightArrow;
			Boxed.key_up = KeyCode.UpArrow;
			Boxed.key_down = KeyCode.DownArrow;
			Boxed.key_tab = KeyCode.Tab;
			Boxed.key_none = KeyCode.None;
		}

		public static object Box(bool b)
		{
			return (!b ? Boxed.@false : Boxed.@true);
		}

		public static object Box(int i)
		{
			switch (i)
			{
				case 0:
				{
					return Boxed.int_0;
				}
				case 1:
				{
					return Boxed.int_1;
				}
				case 2:
				{
					return Boxed.int_2;
				}
			}
			return i;
		}

		public static object Box<T>(T o)
		{
			return o;
		}

		public static object Box(KeyCode k)
		{
			KeyCode keyCode = k;
			switch (keyCode)
			{
				case KeyCode.UpArrow:
				{
					return Boxed.key_up;
				}
				case KeyCode.DownArrow:
				{
					return Boxed.key_down;
				}
				case KeyCode.RightArrow:
				{
					return Boxed.key_right;
				}
				case KeyCode.LeftArrow:
				{
					return Boxed.key_left;
				}
				default:
				{
					if (keyCode == KeyCode.None)
					{
						break;
					}
					else
					{
						if (keyCode == KeyCode.Tab)
						{
							return Boxed.key_tab;
						}
						if (keyCode == KeyCode.Escape)
						{
							return Boxed.key_escape;
						}
						return k;
					}
				}
			}
			return Boxed.key_none;
		}
	}
}