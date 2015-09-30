using System;
using System.Collections.Generic;
using UnityEngine;

public static class NGUIMath
{
	public static Vector3 ApplyHalfPixelOffset(Vector3 pos)
	{
		RuntimePlatform runtimePlatform = Application.platform;
		if (runtimePlatform == RuntimePlatform.WindowsPlayer || runtimePlatform == RuntimePlatform.WindowsWebPlayer || runtimePlatform == RuntimePlatform.WindowsEditor)
		{
			pos.x = pos.x - 0.5f;
			pos.y = pos.y + 0.5f;
		}
		return pos;
	}

	public static Vector3 ApplyHalfPixelOffset(Vector3 pos, Vector3 scale)
	{
		RuntimePlatform runtimePlatform = Application.platform;
		if (runtimePlatform == RuntimePlatform.WindowsPlayer || runtimePlatform == RuntimePlatform.WindowsWebPlayer || runtimePlatform == RuntimePlatform.WindowsEditor)
		{
			if (Mathf.RoundToInt(scale.x) == Mathf.RoundToInt(scale.x * 0.5f) * 2)
			{
				pos.x = pos.x - 0.5f;
			}
			if (Mathf.RoundToInt(scale.y) == Mathf.RoundToInt(scale.y * 0.5f) * 2)
			{
				pos.y = pos.y + 0.5f;
			}
		}
		return pos;
	}

	public static AABBox CalculateAbsoluteWidgetBounds(Transform trans)
	{
		Vector2 vector2;
		Vector2 vector21;
		AABBox aABBox;
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		AABBox aABBox1;
		using (NGUIMath.WidgetList widgetsInChildren = NGUIMath.GetWidgetsInChildren(trans))
		{
			if (!widgetsInChildren.empty)
			{
				AABBox aABBox2 = new AABBox();
				bool flag = true;
				foreach (UIWidget widgetsInChild in widgetsInChildren)
				{
					widgetsInChild.GetPivotOffsetAndRelativeSize(out vector21, out vector2);
					vector3.x = (vector21.x + 0.5f) * vector2.x;
					vector3.y = (vector21.y - 0.5f) * vector2.y;
					vector31.x = vector3.x + vector2.x * 0.5f;
					vector31.y = vector3.y + vector2.y * 0.5f;
					vector3.x = vector3.x - vector2.x * 0.5f;
					vector3.y = vector3.y - vector2.y * 0.5f;
					vector3.z = 0f;
					vector31.z = 0f;
					AABBox aABBox3 = new AABBox(ref vector3, ref vector31);
					Matrix4x4 matrix4x4 = widgetsInChild.cachedTransform.localToWorldMatrix;
					aABBox3.TransformedAABB3x4(ref matrix4x4, out aABBox);
					if (!flag)
					{
						aABBox2.Encapsulate(ref aABBox);
					}
					else
					{
						aABBox2 = aABBox;
						flag = false;
					}
				}
				aABBox1 = (!flag ? aABBox2 : new AABBox(trans.position));
			}
			else
			{
				aABBox1 = new AABBox();
			}
		}
		return aABBox1;
	}

	public static AABBox CalculateRelativeInnerBounds(Transform root, UISlicedSprite sprite)
	{
		Vector2 vector2;
		Vector2 vector21;
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		AABBox aABBox;
		Transform transforms = sprite.cachedTransform;
		Matrix4x4 matrix4x4 = root.worldToLocalMatrix * transforms.localToWorldMatrix;
		sprite.GetPivotOffsetAndRelativeSize(out vector21, out vector2);
		float single = (vector21.x + 0.5f) * vector2.x;
		float single1 = (vector21.y - 0.5f) * vector2.y;
		vector2 = vector2 * 0.5f;
		Vector3 vector32 = transforms.localScale;
		float single2 = vector32.x;
		float single3 = vector32.y;
		Vector4 vector4 = sprite.border;
		if (single2 != 0f)
		{
			vector4.x = vector4.x / single2;
			vector4.z = vector4.z / single2;
		}
		if (single3 != 0f)
		{
			vector4.y = vector4.y / single3;
			vector4.w = vector4.w / single3;
		}
		vector3.x = single - vector2.x + vector4.x;
		vector31.x = single + vector2.x - vector4.z;
		vector3.y = single1 - vector2.y + vector4.y;
		vector31.y = single1 + vector2.y - vector4.w;
		float single4 = 0f;
		float single5 = single4;
		vector31.z = single4;
		vector3.z = single5;
		AABBox aABBox1 = new AABBox(ref vector3, ref vector31);
		aABBox1.TransformedAABB3x4(ref matrix4x4, out aABBox);
		return aABBox;
	}

	public static AABBox CalculateRelativeInnerBounds(Transform root, UISprite sprite)
	{
		if (sprite is UISlicedSprite)
		{
			return NGUIMath.CalculateRelativeInnerBounds(root, sprite as UISlicedSprite);
		}
		return NGUIMath.CalculateRelativeWidgetBounds(root, sprite.cachedTransform);
	}

	public static AABBox CalculateRelativeWidgetBounds(Transform root, Transform child)
	{
		Vector2 vector2;
		Vector2 vector21;
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		AABBox aABBox;
		AABBox aABBox1;
		using (NGUIMath.WidgetList widgetsInChildren = NGUIMath.GetWidgetsInChildren(child))
		{
			if (!widgetsInChildren.empty)
			{
				bool flag = true;
				AABBox aABBox2 = new AABBox();
				Matrix4x4 matrix4x4 = root.worldToLocalMatrix;
				foreach (UIWidget widgetsInChild in widgetsInChildren)
				{
					widgetsInChild.GetPivotOffsetAndRelativeSize(out vector21, out vector2);
					vector3.x = (vector21.x + 0.5f) * vector2.x;
					vector3.y = (vector21.x - 0.5f) * vector2.y;
					vector3.z = 0f;
					vector31.x = vector3.x + vector2.x * 0.5f;
					vector31.y = vector3.y + vector2.y * 0.5f;
					vector31.z = 0f;
					vector3.x = vector3.x - vector2.x * 0.5f;
					vector3.y = vector3.y - vector2.y * 0.5f;
					Matrix4x4 matrix4x41 = matrix4x4 * widgetsInChild.cachedTransform.localToWorldMatrix;
					AABBox aABBox3 = new AABBox(ref vector3, ref vector31);
					aABBox3.TransformedAABB3x4(ref matrix4x41, out aABBox);
					if (!flag)
					{
						aABBox2.Encapsulate(ref aABBox);
					}
					else
					{
						aABBox2 = aABBox;
						flag = false;
					}
				}
				aABBox1 = aABBox2;
			}
			else
			{
				aABBox1 = new AABBox();
			}
		}
		return aABBox1;
	}

	public static AABBox CalculateRelativeWidgetBounds(Transform trans)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(trans, trans);
	}

	public static int ColorToInt(Color c)
	{
		int num = 0;
		num = num | Mathf.RoundToInt(c.r * 255f) << 24;
		num = num | Mathf.RoundToInt(c.g * 255f) << 16;
		num = num | Mathf.RoundToInt(c.b * 255f) << 8;
		num = num | Mathf.RoundToInt(c.a * 255f);
		return num;
	}

	public static Vector2 ConstrainRect(Vector2 minRect, Vector2 maxRect, Vector2 minArea, Vector2 maxArea)
	{
		Vector2 vector2 = Vector2.zero;
		float single = maxRect.x - minRect.x;
		float single1 = maxRect.y - minRect.y;
		float single2 = maxArea.x - minArea.x;
		float single3 = maxArea.y - minArea.y;
		if (single > single2)
		{
			float single4 = single - single2;
			minArea.x = minArea.x - single4;
			maxArea.x = maxArea.x + single4;
		}
		if (single1 > single3)
		{
			float single5 = single1 - single3;
			minArea.y = minArea.y - single5;
			maxArea.y = maxArea.y + single5;
		}
		if (minRect.x < minArea.x)
		{
			vector2.x = vector2.x + (minArea.x - minRect.x);
		}
		if (maxRect.x > maxArea.x)
		{
			vector2.x = vector2.x - (maxRect.x - maxArea.x);
		}
		if (minRect.y < minArea.y)
		{
			vector2.y = vector2.y + (minArea.y - minRect.y);
		}
		if (maxRect.y > maxArea.y)
		{
			vector2.y = vector2.y - (maxRect.y - maxArea.y);
		}
		return vector2;
	}

	public static Rect ConvertToPixels(Rect rect, int width, int height, bool round)
	{
		Rect num = rect;
		if (!round)
		{
			num.xMin = rect.xMin * (float)width;
			num.xMax = rect.xMax * (float)width;
			num.yMin = (1f - rect.yMax) * (float)height;
			num.yMax = (1f - rect.yMin) * (float)height;
		}
		else
		{
			num.xMin = (float)Mathf.RoundToInt(rect.xMin * (float)width);
			num.xMax = (float)Mathf.RoundToInt(rect.xMax * (float)width);
			num.yMin = (float)Mathf.RoundToInt((1f - rect.yMax) * (float)height);
			num.yMax = (float)Mathf.RoundToInt((1f - rect.yMin) * (float)height);
		}
		return num;
	}

	public static Rect ConvertToTexCoords(Rect rect, int width, int height)
	{
		Rect rect1 = rect;
		if ((float)width != 0f && (float)height != 0f)
		{
			rect1.xMin = rect.xMin / (float)width;
			rect1.xMax = rect.xMax / (float)width;
			rect1.yMin = 1f - rect.yMax / (float)height;
			rect1.yMax = 1f - rect.yMin / (float)height;
		}
		return rect1;
	}

	private static void FillWidgetListWithChildren(Transform trans, ref NGUIMath.WidgetList list, ref bool madeList)
	{
		UIWidget component = trans.GetComponent<UIWidget>();
		if (component)
		{
			if (!madeList)
			{
				list = NGUIMath.WidgetList.Generate();
				madeList = true;
			}
			list.Add(component);
		}
		int num = trans.childCount;
		while (true)
		{
			int num1 = num;
			num = num1 - 1;
			if (num1 <= 0)
			{
				break;
			}
			NGUIMath.FillWidgetListWithChildren(trans.GetChild(num), ref list, ref madeList);
		}
	}

	private static NGUIMath.WidgetList GetWidgetsInChildren(Transform trans)
	{
		if (trans)
		{
			bool flag = false;
			NGUIMath.WidgetList widgetList = null;
			NGUIMath.FillWidgetListWithChildren(trans, ref widgetList, ref flag);
			if (flag)
			{
				return widgetList;
			}
		}
		return NGUIMath.WidgetList.Empty;
	}

	public static Color HexToColor(uint val)
	{
		return NGUIMath.IntToColor((int)val);
	}

	public static int HexToDecimal(char ch)
	{
		char chr = ch;
		switch (chr)
		{
			case '0':
			{
				return 0;
			}
			case '1':
			{
				return 1;
			}
			case '2':
			{
				return 2;
			}
			case '3':
			{
				return 3;
			}
			case '4':
			{
				return 4;
			}
			case '5':
			{
				return 5;
			}
			case '6':
			{
				return 6;
			}
			case '7':
			{
				return 7;
			}
			case '8':
			{
				return 8;
			}
			case '9':
			{
				return 9;
			}
			case 'A':
			{
				return 10;
			}
			case 'B':
			{
				return 11;
			}
			case 'C':
			{
				return 12;
			}
			case 'D':
			{
				return 13;
			}
			case 'E':
			{
				return 14;
			}
			case 'F':
			{
				return 15;
			}
			default:
			{
				switch (chr)
				{
					case 'a':
					{
						return 10;
					}
					case 'b':
					{
						return 11;
					}
					case 'c':
					{
						return 12;
					}
					case 'd':
					{
						return 13;
					}
					case 'e':
					{
						return 14;
					}
					case 'f':
					{
						return 15;
					}
				}
				return 15;
			}
		}
	}

	public static string IntToBinary(int val, int bits)
	{
		string empty = string.Empty;
		int num = bits;
		while (num > 0)
		{
			if (num == 8 || num == 16 || num == 24)
			{
				empty = string.Concat(empty, " ");
			}
			string str = empty;
			int num1 = num - 1;
			num = num1;
			empty = string.Concat(str, ((val & 1 << (num1 & 31 & 31)) == 0 ? '0' : '1'));
		}
		return empty;
	}

	public static Color IntToColor(int val)
	{
		float single = 0.003921569f;
		Color color = Color.black;
		color.r = single * (float)(val >> 24 & 255);
		color.g = single * (float)(val >> 16 & 255);
		color.b = single * (float)(val >> 8 & 255);
		color.a = single * (float)(val & 255);
		return color;
	}

	public static Rect MakePixelPerfect(Rect rect)
	{
		rect.xMin = (float)Mathf.RoundToInt(rect.xMin);
		rect.yMin = (float)Mathf.RoundToInt(rect.yMin);
		rect.xMax = (float)Mathf.RoundToInt(rect.xMax);
		rect.yMax = (float)Mathf.RoundToInt(rect.yMax);
		return rect;
	}

	public static Rect MakePixelPerfect(Rect rect, int width, int height)
	{
		rect = NGUIMath.ConvertToPixels(rect, width, height, true);
		rect.xMin = (float)Mathf.RoundToInt(rect.xMin);
		rect.yMin = (float)Mathf.RoundToInt(rect.yMin);
		rect.xMax = (float)Mathf.RoundToInt(rect.xMax);
		rect.yMax = (float)Mathf.RoundToInt(rect.yMax);
		return NGUIMath.ConvertToTexCoords(rect, width, height);
	}

	public static float RotateTowards(float from, float to, float maxAngle)
	{
		float single = NGUIMath.WrapAngle(to - from);
		if (Mathf.Abs(single) > maxAngle)
		{
			single = maxAngle * Mathf.Sign(single);
		}
		return from + single;
	}

	public static Vector3 SpringDampen(ref Vector3 velocity, float strength, float deltaTime)
	{
		if (Mathf.Approximately(velocity.x, 0f) && Mathf.Approximately(velocity.y, 0f) && Mathf.Approximately(velocity.z, 0f))
		{
			velocity = Vector3.zero;
			return Vector3.zero;
		}
		float single = 1f - strength * 0.001f;
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		Vector3 vector3 = Vector3.zero;
		for (int i = 0; i < num; i++)
		{
			vector3 = vector3 + (velocity * 0.06f);
			velocity = velocity * single;
		}
		return vector3;
	}

	public static Vector2 SpringDampen(ref Vector2 velocity, float strength, float deltaTime)
	{
		float single = 1f - strength * 0.001f;
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		Vector2 vector2 = Vector2.zero;
		for (int i = 0; i < num; i++)
		{
			vector2 = vector2 + (velocity * 0.06f);
			velocity = velocity * single;
		}
		return vector2;
	}

	public static float SpringLerp(float strength, float deltaTime)
	{
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		deltaTime = 0.001f * strength;
		float single = 0f;
		for (int i = 0; i < num; i++)
		{
			single = Mathf.Lerp(single, 1f, deltaTime);
		}
		return single;
	}

	public static float SpringLerp(float from, float to, float strength, float deltaTime)
	{
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		deltaTime = 0.001f * strength;
		for (int i = 0; i < num; i++)
		{
			from = Mathf.Lerp(from, to, deltaTime);
		}
		return from;
	}

	public static Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime)
	{
		return Vector2.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime)
	{
		return Vector3.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	public static Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime)
	{
		return Quaternion.Slerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	public static float WrapAngle(float angle)
	{
		while (angle > 180f)
		{
			angle = angle - 360f;
		}
		while (angle < -180f)
		{
			angle = angle + 360f;
		}
		return angle;
	}

	private class WidgetList : List<UIWidget>, IDisposable
	{
		private readonly bool staticEmpty;

		private bool disposed;

		private static int tempWidgetListsSize;

		private static Queue<NGUIMath.WidgetList> tempWidgetLists;

		public readonly static NGUIMath.WidgetList Empty;

		public bool empty
		{
			get
			{
				return this.staticEmpty;
			}
		}

		static WidgetList()
		{
			NGUIMath.WidgetList.tempWidgetLists = new Queue<NGUIMath.WidgetList>();
			NGUIMath.WidgetList.Empty = new NGUIMath.WidgetList(true);
		}

		private WidgetList(bool staticEmpty)
		{
			this.staticEmpty = staticEmpty;
		}

		public void Add(UIWidget widget)
		{
			if (this.staticEmpty)
			{
				throw new InvalidOperationException();
			}
			base.Add(widget);
		}

		public void Dispose()
		{
			if (!this.disposed && !this.staticEmpty)
			{
				this.Clear();
				NGUIMath.WidgetList.tempWidgetLists.Enqueue(this);
				NGUIMath.WidgetList.tempWidgetListsSize = NGUIMath.WidgetList.tempWidgetListsSize + 1;
				this.disposed = true;
			}
		}

		public static NGUIMath.WidgetList Generate()
		{
			if (NGUIMath.WidgetList.tempWidgetListsSize == 0)
			{
				return new NGUIMath.WidgetList(false);
			}
			NGUIMath.WidgetList widgetList = NGUIMath.WidgetList.tempWidgetLists.Dequeue();
			widgetList.disposed = false;
			NGUIMath.WidgetList.tempWidgetListsSize = NGUIMath.WidgetList.tempWidgetListsSize - 1;
			return widgetList;
		}
	}
}