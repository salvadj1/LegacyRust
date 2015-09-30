using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	public UITextList.Style style;

	public UILabel textLabel;

	public float maxWidth;

	public float maxHeight;

	public int maxEntries = 50;

	public bool supportScrollWheel = true;

	protected char[] mSeparator = new char[] { '\n' };

	protected List<UITextList.Paragraph> mParagraphs = new List<UITextList.Paragraph>();

	protected float mScroll;

	protected bool mSelected;

	protected int mTotalLines;

	public UITextList()
	{
	}

	public void Add(string text)
	{
		this.Add(text, true);
	}

	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph item = null;
		if (this.mParagraphs.Count >= this.maxEntries)
		{
			item = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		else
		{
			item = new UITextList.Paragraph();
		}
		item.text = text;
		this.mParagraphs.Add(item);
		if (this.textLabel != null && this.textLabel.font != null)
		{
			UIFont uIFont = this.textLabel.font;
			List<UITextMarkup> uITextMarkups = UIFont.tempMarkup;
			string str = item.text;
			float single = this.maxWidth;
			Vector3 vector3 = this.textLabel.transform.localScale;
			item.lines = uIFont.WrapText(uITextMarkups, str, single / vector3.y, this.textLabel.maxLineCount, this.textLabel.supportEncoding, this.textLabel.symbolStyle).Split(this.mSeparator);
			this.mTotalLines = 0;
			int num = 0;
			int count = this.mParagraphs.Count;
			while (num < count)
			{
				UITextList length = this;
				length.mTotalLines = length.mTotalLines + (int)this.mParagraphs[num].lines.Length;
				num++;
			}
		}
		if (updateVisible)
		{
			this.UpdateVisibleText();
		}
	}

	private void Awake()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.textLabel != null)
		{
			this.textLabel.lineWidth = 0;
		}
		Collider collider = base.collider;
		if (collider != null)
		{
			if (this.maxHeight <= 0f)
			{
				Bounds bound = collider.bounds;
				this.maxHeight = bound.size.y / base.transform.lossyScale.y;
			}
			if (this.maxWidth <= 0f)
			{
				Bounds bound1 = collider.bounds;
				this.maxWidth = bound1.size.x / base.transform.lossyScale.x;
			}
		}
	}

	public void Clear()
	{
		this.mParagraphs.Clear();
		this.UpdateVisibleText();
	}

	private void OnScroll(float val)
	{
		if (this.mSelected && this.supportScrollWheel)
		{
			val = val * (this.style != UITextList.Style.Chat ? -10f : 10f);
			this.mScroll = Mathf.Max(0f, this.mScroll + val);
			this.UpdateVisibleText();
		}
	}

	private void OnSelect(bool selected)
	{
		this.mSelected = selected;
	}

	protected void UpdateVisibleText()
	{
		int num;
		if (this.textLabel != null && this.textLabel.font != null)
		{
			int num1 = 0;
			if (this.maxHeight <= 0f)
			{
				num = 100000;
			}
			else
			{
				float single = this.maxHeight;
				Vector3 vector3 = this.textLabel.cachedTransform.localScale;
				num = Mathf.FloorToInt(single / vector3.y);
			}
			int num2 = num;
			int num3 = Mathf.RoundToInt(this.mScroll);
			if (num2 + num3 > this.mTotalLines)
			{
				num3 = Mathf.Max(0, this.mTotalLines - num2);
				this.mScroll = (float)num3;
			}
			if (this.style == UITextList.Style.Chat)
			{
				num3 = Mathf.Max(0, this.mTotalLines - num2 - num3);
			}
			string empty = string.Empty;
			int num4 = 0;
			int count = this.mParagraphs.Count;
			while (num4 < count)
			{
				UITextList.Paragraph item = this.mParagraphs[num4];
				int num5 = 0;
				int length = (int)item.lines.Length;
				while (num5 < length)
				{
					string str = item.lines[num5];
					if (num3 <= 0)
					{
						if (empty.Length > 0)
						{
							empty = string.Concat(empty, "\n");
						}
						empty = string.Concat(empty, str);
						num1++;
						if (num1 >= num2)
						{
							break;
						}
					}
					else
					{
						num3--;
					}
					num5++;
				}
				if (num1 < num2)
				{
					num4++;
				}
				else
				{
					break;
				}
			}
			this.textLabel.text = empty;
		}
	}

	protected class Paragraph
	{
		public string text;

		public string[] lines;

		public Paragraph()
		{
		}
	}

	public enum Style
	{
		Text,
		Chat
	}
}