using NGUI.MessageUtil;
using System;
using UnityEngine;

public static class DropNotification
{
	public const DropNotificationFlags DragDrop = DropNotificationFlags.DragDrop;

	public const DropNotificationFlags DragLand = DropNotificationFlags.DragLand;

	public const DropNotificationFlags kDragReverseBit = DropNotificationFlags.DragReverse;

	public const DropNotificationFlags AltDrop = DropNotificationFlags.AltDrop;

	public const DropNotificationFlags AltLand = DropNotificationFlags.AltLand;

	public const DropNotificationFlags kAltReverseBit = DropNotificationFlags.AltReverse;

	public const DropNotificationFlags MidDrop = DropNotificationFlags.MidDrop;

	public const DropNotificationFlags MidLand = DropNotificationFlags.MidLand;

	public const DropNotificationFlags kMidReverseBit = DropNotificationFlags.MidReverse;

	public const DropNotificationFlags DragLandOutside = DropNotificationFlags.DragLandOutside;

	private const DropNotificationFlags kInvalidNeverSet = -2147483648;

	public const DropNotificationFlags DragDropThenLand = DropNotificationFlags.DragDrop | DropNotificationFlags.DragLand;

	public const DropNotificationFlags DragLandThenDrop = DropNotificationFlags.DragDrop | DropNotificationFlags.DragLand | DropNotificationFlags.DragReverse;

	public const DropNotificationFlags AltDropThenLand = DropNotificationFlags.AltDrop | DropNotificationFlags.AltLand;

	public const DropNotificationFlags AltLandThenDrop = DropNotificationFlags.AltDrop | DropNotificationFlags.AltLand | DropNotificationFlags.AltReverse;

	public const DropNotificationFlags MidDropThenLand = DropNotificationFlags.MidDrop | DropNotificationFlags.MidLand;

	public const DropNotificationFlags MidLandThenDrop = DropNotificationFlags.MidDrop | DropNotificationFlags.MidLand | DropNotificationFlags.MidReverse;

	public const DropNotificationFlags kDefault = DropNotificationFlags.DragDrop;

	public const DropNotificationFlags kMask_Drag = DropNotificationFlags.DragDrop | DropNotificationFlags.DragLand | DropNotificationFlags.DragReverse;

	public const DropNotificationFlags kMask_Alt = DropNotificationFlags.AltDrop | DropNotificationFlags.AltLand | DropNotificationFlags.AltReverse;

	public const DropNotificationFlags kMask_Mid = DropNotificationFlags.MidDrop | DropNotificationFlags.MidLand | DropNotificationFlags.MidReverse;

	public const DropNotificationFlags kMask_Active = DropNotificationFlags.DragDrop | DropNotificationFlags.DragLand | DropNotificationFlags.AltDrop | DropNotificationFlags.AltLand | DropNotificationFlags.MidDrop | DropNotificationFlags.MidLand;

	public const DropNotificationFlags Disable = 0;

	private static GameObject scanItem;

	private static bool stopDrag;

	private static DragEventKind inDrag;

	internal static bool DropMessage(ref DropNotificationFlags flags, DragEventKind kind, GameObject Pressed, GameObject Released)
	{
		bool flag;
		bool flag1;
		string str;
		string str1;
		DropNotificationFlags dropNotificationFlag;
		DropNotificationFlags dropNotificationFlag1;
		DropNotificationFlags dropNotificationFlag2;
		switch (kind)
		{
			case DragEventKind.Drag:
			{
				flag = true;
				if (!Released)
				{
					flag1 = false;
					dropNotificationFlag = (DropNotificationFlags)-2147483648;
					dropNotificationFlag2 = DropNotificationFlags.DragLandOutside;
					dropNotificationFlag1 = DropNotificationFlags.DragLandOutside;
					str = "----";
					str1 = "OnLandOutside";
				}
				else
				{
					flag1 = true;
					dropNotificationFlag = DropNotificationFlags.DragDrop;
					dropNotificationFlag1 = DropNotificationFlags.DragLand;
					dropNotificationFlag2 = DropNotificationFlags.DragReverse;
					str = "OnDrop";
					str1 = "OnLand";
				}
				break;
			}
			case DragEventKind.Alt:
			{
				flag1 = true;
				flag = false;
				dropNotificationFlag = DropNotificationFlags.AltDrop;
				dropNotificationFlag1 = DropNotificationFlags.AltLand;
				dropNotificationFlag2 = DropNotificationFlags.AltReverse;
				str = "OnAltDrop";
				str1 = "OnAltLand";
				break;
			}
			case DragEventKind.Mid:
			{
				flag1 = true;
				flag = false;
				dropNotificationFlag = DropNotificationFlags.MidDrop;
				dropNotificationFlag1 = DropNotificationFlags.MidLand;
				dropNotificationFlag2 = DropNotificationFlags.MidReverse;
				str = "OnMidDrop";
				str1 = "OnMidLand";
				break;
			}
			default:
			{
				throw new ArgumentOutOfRangeException();
			}
		}
		if (((int)flags & (int)dropNotificationFlag2) != (int)dropNotificationFlag2)
		{
			if (((int)flags & (int)dropNotificationFlag) == (int)dropNotificationFlag)
			{
				if (!flag1)
				{
					DropNotification.Message(Released, str, Pressed, kind, ref flag);
				}
				else
				{
					DropNotification.Message(Released, Pressed, str, Pressed, kind, ref flag);
				}
			}
			if (((int)flags & (int)dropNotificationFlag1) == (int)dropNotificationFlag1)
			{
				if (!flag1)
				{
					DropNotification.Message(Pressed, str1, Pressed, kind, ref flag);
				}
				else
				{
					DropNotification.Message(Pressed, Released, str1, Pressed, kind, ref flag);
				}
			}
		}
		else
		{
			if (((int)flags & (int)dropNotificationFlag1) == (int)dropNotificationFlag1)
			{
				if (!flag1)
				{
					DropNotification.Message(Pressed, str1, Pressed, kind, ref flag);
				}
				else
				{
					DropNotification.Message(Pressed, Released, str1, Pressed, kind, ref flag);
				}
			}
			if (((int)flags & (int)dropNotificationFlag) == (int)dropNotificationFlag)
			{
				if (!flag1)
				{
					DropNotification.Message(Released, str, Pressed, kind, ref flag);
				}
				else
				{
					DropNotification.Message(Released, Pressed, str, Pressed, kind, ref flag);
				}
			}
		}
		return flag;
	}

	private static bool Message(GameObject target, GameObject parameter, string messageName, GameObject scan, DragEventKind kind, ref bool drop)
	{
		bool flag;
		if (!target)
		{
			return false;
		}
		GameObject gameObject = DropNotification.scanItem;
		bool flag1 = DropNotification.stopDrag;
		DragEventKind dragEventKind = DropNotification.inDrag;
		try
		{
			DropNotification.scanItem = scan;
			DropNotification.stopDrag = drop;
			DropNotification.inDrag = kind;
			target.NGUIMessage(messageName, parameter);
			drop = DropNotification.stopDrag;
			flag = true;
		}
		finally
		{
			DropNotification.scanItem = gameObject;
			DropNotification.stopDrag = flag1;
			DropNotification.inDrag = dragEventKind;
		}
		return flag;
	}

	private static bool Message(GameObject target, string messageName, GameObject scan, DragEventKind kind, ref bool drop)
	{
		bool flag;
		if (!target)
		{
			return false;
		}
		GameObject gameObject = DropNotification.scanItem;
		bool flag1 = DropNotification.stopDrag;
		DragEventKind dragEventKind = DropNotification.inDrag;
		try
		{
			DropNotification.scanItem = scan;
			DropNotification.stopDrag = drop;
			DropNotification.inDrag = kind;
			target.NGUIMessage(messageName);
			drop = DropNotification.stopDrag;
			flag = true;
		}
		finally
		{
			DropNotification.scanItem = gameObject;
			DropNotification.stopDrag = flag1;
			DropNotification.inDrag = dragEventKind;
		}
		return flag;
	}

	public static void StopDragging(GameObject item)
	{
		if (DropNotification.inDrag == DragEventKind.None)
		{
			Debug.LogError("StopDragging can only be called from within Drop or Land messages");
		}
		else if (item == DropNotification.scanItem)
		{
			DropNotification.stopDrag = true;
		}
		else
		{
			Debug.LogWarning("StopDragging was called with a invalid value, should have been the thing being dragged");
		}
	}
}