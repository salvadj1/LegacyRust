using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NGUI.MessageUtil
{
	public static class Util
	{
		public static void AltClick(this GameObject recv)
		{
			Util.MSG(recv, "OnAltClick", null, false);
		}

		public static void AltDoubleClick(this GameObject recv)
		{
			Util.MSG(recv, "OnAltDoubleClick", null, false);
		}

		public static void AltPress(this GameObject recv, bool press)
		{
			Util.MSG(recv, "OnAltPress", Boxed.Box(press), true);
		}

		public static void Click(this GameObject recv)
		{
			Util.MSG(recv, "OnClick", null, false);
		}

		public static void DoubleClick(this GameObject recv)
		{
			Util.MSG(recv, "OnDoubleClick", null, false);
		}

		public static void Drag(this GameObject recv, Vector2 delta)
		{
			Util.MSG(recv, "OnDrag", Boxed.Box<Vector2>(delta), true);
		}

		public static void DragState(this GameObject recv, bool dragging)
		{
			Util.MSG(recv, "OnDragState", Boxed.Box(dragging), true);
		}

		public static void Drop(this GameObject recv, GameObject obj)
		{
			Util.MSG(recv, "OnDrop", Boxed.Box<GameObject>(obj), true);
		}

		public static void Hover(this GameObject recv, bool highlight)
		{
			Util.MSG(recv, "OnHover", Boxed.Box(highlight), true);
		}

		public static void Input(this GameObject recv, string input)
		{
			Util.MSG(recv, "OnInput", Boxed.Box<string>(input), true);
		}

		public static void Key(this GameObject recv, KeyCode key)
		{
			Util.MSG(recv, "OnKey", Boxed.Box(key), true);
		}

		public static void MidClick(this GameObject recv)
		{
			Util.MSG(recv, "OnMidClick", null, false);
		}

		public static void MidDoubleClick(this GameObject recv)
		{
			Util.MSG(recv, "OnMidDoubleClick", null, false);
		}

		public static void MidPress(this GameObject recv, bool press)
		{
			Util.MSG(recv, "OnMidPress", Boxed.Box(press), true);
		}

		private static void MSG(GameObject recv, string message, object value, bool withValue)
		{
			if (!recv)
			{
				if (withValue)
				{
					Debug.LogWarning(string.Format("((GameObject)null).SendMessage(\"{0}\", {1}, SendMessageOptions.{2})", message, value, SendMessageOptions.DontRequireReceiver));
				}
				else
				{
					Debug.LogWarning(string.Format("((GameObject)null).SendMessage(\"{0}\", SendMessageOptions.{1})", message, SendMessageOptions.DontRequireReceiver));
				}
			}
			else if (!withValue)
			{
				try
				{
					recv.SendMessage(message, SendMessageOptions.DontRequireReceiver);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Debug.LogError(string.Format("((GameObject){2}).SendMessage(\"{0}\", SendMessageOptions.{1}) threw the exception below\r\n{3}", new object[] { message, SendMessageOptions.DontRequireReceiver, recv, exception }), recv);
				}
			}
			else if (!object.ReferenceEquals(value, null))
			{
				try
				{
					recv.SendMessage(message, value, SendMessageOptions.DontRequireReceiver);
				}
				catch (Exception exception3)
				{
					Exception exception2 = exception3;
					Debug.LogError(string.Format("((GameObject){2}).SendMessage(\"{0}\", {4}({5}), SendMessageOptions.{1}) threw the exception below\r\n{3}", new object[] { message, SendMessageOptions.DontRequireReceiver, recv, exception2, value, value.GetType() }), recv);
				}
			}
			else
			{
				Debug.LogWarning(string.Format("((GameObject){2}).SendMessage(\"{0}\", SendMessageOptions.{1}, null ) was not called because of the null argument.", message, SendMessageOptions.DontRequireReceiver, recv), recv);
			}
		}

		public static void NGUIMessage(this GameObject recv, string message)
		{
			Util.MSG(recv, message, null, false);
		}

		public static void NGUIMessage(this GameObject recv, string message, bool value)
		{
			Util.MSG(recv, message, Boxed.Box(value), true);
		}

		public static void NGUIMessage(this GameObject recv, string message, int value)
		{
			Util.MSG(recv, message, Boxed.Box(value), true);
		}

		public static void NGUIMessage(this GameObject recv, string message, KeyCode value)
		{
			Util.MSG(recv, message, Boxed.Box(value), true);
		}

		public static void NGUIMessage(this GameObject recv, string message, GameObject value)
		{
			Util.MSG(recv, message, Boxed.Box<GameObject>(value), true);
		}

		public static void NGUIMessage(this GameObject recv, string message, object value)
		{
			Util.MSG(recv, message, value, true);
		}

		public static void NGUIMessage<T>(this GameObject recv, string message, T value)
		{
			Util.MSG(recv, message, Boxed.Box<T>(value), true);
		}

		public static void Press(this GameObject recv, bool press)
		{
			Util.MSG(recv, "OnPress", Boxed.Box(press), true);
		}

		public static void Scroll(this GameObject recv, float y)
		{
			Util.MSG(recv, "OnScroll", Boxed.Box<float>(y), true);
		}

		public static void ScrollX(this GameObject recv, float x)
		{
			Util.MSG(recv, "OnScrollX", Boxed.Box<float>(x), true);
		}

		public static void Select(this GameObject recv, bool selected)
		{
			Util.MSG(recv, "OnSelect", Boxed.Box(selected), true);
		}

		public static void Tooltip(this GameObject recv, bool show)
		{
			Util.MSG(recv, "OnTooltip", Boxed.Box(show), true);
		}
	}
}