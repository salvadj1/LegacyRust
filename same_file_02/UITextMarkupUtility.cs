using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class UITextMarkupUtility
{
	public static string MarkUp(this List<UITextMarkup> list, string input)
	{
		if (list != null)
		{
			int count = list.Count;
			int num = count;
			if (count != 0)
			{
				int item = list[0].index;
				StringBuilder stringBuilder = new StringBuilder(input, 0, item, input.Length + num);
				int num1 = 0;
				UITextMarkup uITextMarkup = list[num1];
				for (int i = uITextMarkup.index; i < input.Length; i++)
				{
					char chr = input[i];
					if (i == uITextMarkup.index)
					{
						do
						{
							switch (uITextMarkup.mod)
							{
								case UITextMod.End:
								{
									i = input.Length + 1;
									chr = '\0';
									break;
								}
								case UITextMod.Removed:
								{
									chr = '\0';
									break;
								}
								case UITextMod.Replaced:
								{
									stringBuilder.Append(uITextMarkup.@value);
									chr = '\0';
									break;
								}
								case UITextMod.Added:
								{
									stringBuilder.Append(uITextMarkup.@value);
									break;
								}
							}
							int num2 = num1 + 1;
							num1 = num2;
							if (num2 != num)
							{
								uITextMarkup = list[num1];
							}
							else if (i < input.Length)
							{
								if (chr == 0)
								{
									int num3 = i + 1;
									i = num3;
									if (num3 >= input.Length)
									{
										break;
									}
									else
									{
										stringBuilder.Append(input, i, input.Length - i);
									}
								}
								else
								{
									stringBuilder.Append(input, i, input.Length - i);
								}
								i = input.Length + 1;
								break;
							}
						}
						while (uITextMarkup.index == i);
						if (chr != 0)
						{
							stringBuilder.Append(chr);
						}
					}
					else if (chr != 0)
					{
						stringBuilder.Append(chr);
					}
				}
				while (true)
				{
					int num4 = num1 + 1;
					num1 = num4;
					if (num4 >= num)
					{
						break;
					}
					switch (uITextMarkup.mod)
					{
						case UITextMod.End:
						{
							continue;
						}
						case UITextMod.Removed:
						case UITextMod.Replaced:
						{
							Debug.Log(string.Concat("Unsupported end markup ", uITextMarkup));
							continue;
						}
						case UITextMod.Added:
						{
							stringBuilder.Append(uITextMarkup.@value);
							continue;
						}
						default:
						{
							goto case UITextMod.Replaced;
						}
					}
				}
				return stringBuilder.ToString();
			}
		}
		return input;
	}

	public static void SortMarkup(this List<UITextMarkup> list)
	{
		list.Sort((UITextMarkup x, UITextMarkup y) => {
			int num = x.index.CompareTo(y.index);
			return (num != 0 ? num : x.mod.CompareTo((byte)y.mod));
		});
	}
}