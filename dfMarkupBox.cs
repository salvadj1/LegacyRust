using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class dfMarkupBox
{
	public Vector2 Position = Vector2.zero;

	public Vector2 Size = Vector2.zero;

	public dfMarkupDisplayType Display;

	public dfMarkupBorders Margins = new dfMarkupBorders(0, 0, 0, 0);

	public dfMarkupBorders Padding = new dfMarkupBorders(0, 0, 0, 0);

	public dfMarkupStyle Style;

	public bool IsNewline;

	public int Baseline;

	private List<dfMarkupBox> children = new List<dfMarkupBox>();

	private dfMarkupBox currentLine;

	private int currentLinePos;

	public List<dfMarkupBox> Children
	{
		get
		{
			return this.children;
		}
	}

	public dfMarkupElement Element
	{
		get;
		protected set;
	}

	public int Height
	{
		get
		{
			return (int)this.Size.y;
		}
		set
		{
			this.Size = new Vector2(this.Size.x, (float)value);
		}
	}

	public dfMarkupBox Parent
	{
		get;
		protected set;
	}

	public int Width
	{
		get
		{
			return (int)this.Size.x;
		}
		set
		{
			this.Size = new Vector2((float)value, this.Size.y);
		}
	}

	private dfMarkupBox()
	{
		throw new NotImplementedException();
	}

	public dfMarkupBox(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style)
	{
		this.Element = element;
		this.Display = display;
		this.Style = style;
		this.Baseline = style.FontSize;
	}

	private void addBlock(dfMarkupBox box)
	{
		if (this.currentLine != null)
		{
			this.currentLine.IsNewline = true;
			this.endCurrentLine(true);
		}
		dfMarkupBox containingBlock = this.GetContainingBlock();
		if (box.Size.sqrMagnitude <= 1.401298E-45f)
		{
			box.Size = new Vector2(containingBlock.Size.x - (float)box.Margins.horizontal, (float)this.Style.FontSize);
		}
		int verticalPosition = this.getVerticalPosition(box.Margins.top);
		box.Position = new Vector2((float)box.Margins.left, (float)verticalPosition);
		this.Size = new Vector2(this.Size.x, Mathf.Max(this.Size.y, box.Position.y + box.Size.y));
		box.Parent = this;
		this.children.Add(box);
	}

	public virtual void AddChild(dfMarkupBox box)
	{
		dfMarkupDisplayType display = box.Display;
		if ((display == dfMarkupDisplayType.block || display == dfMarkupDisplayType.table || display == dfMarkupDisplayType.listItem ? false : display != dfMarkupDisplayType.tableRow))
		{
			this.addInline(box);
		}
		else
		{
			this.addBlock(box);
		}
	}

	private void addInline(dfMarkupBox box)
	{
		bool flag;
		dfMarkupBorders margins = box.Margins;
		if (this.Style.Preformatted)
		{
			flag = false;
		}
		else
		{
			flag = (this.currentLine == null ? false : (float)this.currentLinePos + box.Size.x > this.currentLine.Size.x);
		}
		if (this.currentLine == null || flag)
		{
			this.endCurrentLine(false);
			int verticalPosition = this.getVerticalPosition(margins.top);
			dfMarkupBox containingBlock = this.GetContainingBlock();
			if (containingBlock == null)
			{
				Debug.LogError("Containing block not found");
				return;
			}
			dfDynamicFont font = this.Style.Font ?? this.Style.Host.Font;
			float fontSize = (float)font.FontSize / (float)font.FontSize;
			float baseline = (float)font.Baseline * fontSize;
			dfMarkupBox _dfMarkupBox = new dfMarkupBox(this.Element, dfMarkupDisplayType.block, this.Style)
			{
				Size = new Vector2(containingBlock.Size.x, (float)this.Style.LineHeight),
				Position = new Vector2(0f, (float)verticalPosition),
				Parent = this,
				Baseline = (int)baseline
			};
			this.currentLine = _dfMarkupBox;
			this.children.Add(this.currentLine);
		}
		if (this.currentLinePos == 0 && !box.Style.PreserveWhitespace && box is dfMarkupBoxText && (box as dfMarkupBoxText).IsWhitespace)
		{
			return;
		}
		Vector2 vector2 = new Vector2((float)(this.currentLinePos + margins.left), (float)margins.top);
		box.Position = vector2;
		box.Parent = this.currentLine;
		this.currentLine.children.Add(box);
		this.currentLinePos = (int)(vector2.x + box.Size.x + (float)box.Margins.right);
		float single = Mathf.Max(this.currentLine.Size.x, vector2.x + box.Size.x);
		float single1 = Mathf.Max(this.currentLine.Size.y, vector2.y + box.Size.y);
		this.currentLine.Size = new Vector2(single, single1);
	}

	internal void AddLineBreak()
	{
		if (this.currentLine != null)
		{
			this.currentLine.IsNewline = true;
		}
		int verticalPosition = this.getVerticalPosition(0);
		this.endCurrentLine(false);
		dfMarkupBox containingBlock = this.GetContainingBlock();
		dfMarkupBox _dfMarkupBox = new dfMarkupBox(this.Element, dfMarkupDisplayType.block, this.Style)
		{
			Size = new Vector2(containingBlock.Size.x, (float)this.Style.FontSize),
			Position = new Vector2(0f, (float)verticalPosition),
			Parent = this
		};
		this.currentLine = _dfMarkupBox;
		this.children.Add(this.currentLine);
	}

	private void doHorizontalAlignment()
	{
		if (this.Style.Align == dfMarkupTextAlign.Left || this.children.Count == 0)
		{
			return;
		}
		int count = this.children.Count - 1;
		while (count > 0)
		{
			dfMarkupBoxText item = this.children[count] as dfMarkupBoxText;
			if (item == null || !item.IsWhitespace)
			{
				break;
			}
			else
			{
				count--;
			}
		}
		if (this.Style.Align == dfMarkupTextAlign.Center)
		{
			float size = 0f;
			for (int i = 0; i <= count; i++)
			{
				size = size + this.children[i].Size.x;
			}
			float single = (this.Size.x - (float)this.Padding.horizontal - size) * 0.5f;
			for (int j = 0; j <= count; j++)
			{
				Vector2 position = this.children[j].Position;
				position.x = position.x + single;
				this.children[j].Position = position;
			}
		}
		else if (this.Style.Align != dfMarkupTextAlign.Right)
		{
			if (this.Style.Align != dfMarkupTextAlign.Justify)
			{
				throw new NotImplementedException(string.Concat("text-align: ", this.Style.Align, " is not implemented"));
			}
			if (this.children.Count <= 1)
			{
				return;
			}
			if (this.IsNewline || this.children[this.children.Count - 1].IsNewline)
			{
				return;
			}
			float single1 = 0f;
			for (int k = 0; k <= count; k++)
			{
				dfMarkupBox _dfMarkupBox = this.children[k];
				single1 = Mathf.Max(single1, _dfMarkupBox.Position.x + _dfMarkupBox.Size.x);
			}
			float size1 = (this.Size.x - (float)this.Padding.horizontal - single1) / (float)this.children.Count;
			for (int l = 1; l <= count; l++)
			{
				dfMarkupBox item1 = this.children[l];
				item1.Position = item1.Position + new Vector2((float)l * size1, 0f);
			}
			dfMarkupBox _dfMarkupBox1 = this.children[count];
			Vector2 vector2 = _dfMarkupBox1.Position;
			vector2.x = this.Size.x - (float)this.Padding.horizontal - _dfMarkupBox1.Size.x;
			_dfMarkupBox1.Position = vector2;
		}
		else
		{
			float size2 = this.Size.x - (float)this.Padding.horizontal;
			for (int m = count; m >= 0; m--)
			{
				Vector2 position1 = this.children[m].Position;
				position1.x = size2 - this.children[m].Size.x;
				this.children[m].Position = position1;
				size2 = size2 - this.children[m].Size.x;
			}
		}
	}

	private void doVerticalAlignment()
	{
		if (this.children.Count == 0)
		{
			return;
		}
		float single = Single.MinValue;
		float single1 = Single.MaxValue;
		float single2 = Single.MinValue;
		this.Baseline = (int)(this.Size.y * 0.95f);
		for (int i = 0; i < this.children.Count; i++)
		{
			dfMarkupBox item = this.children[i];
			single2 = Mathf.Max(single2, item.Position.y + (float)item.Baseline);
		}
		for (int j = 0; j < this.children.Count; j++)
		{
			dfMarkupBox _dfMarkupBox = this.children[j];
			dfMarkupVerticalAlign verticalAlign = _dfMarkupBox.Style.VerticalAlign;
			Vector2 position = _dfMarkupBox.Position;
			if (verticalAlign == dfMarkupVerticalAlign.Baseline)
			{
				position.y = single2 - (float)_dfMarkupBox.Baseline;
			}
			_dfMarkupBox.Position = position;
		}
		for (int k = 0; k < this.children.Count; k++)
		{
			dfMarkupBox item1 = this.children[k];
			Vector2 vector2 = item1.Position;
			Vector2 size = item1.Size;
			single1 = Mathf.Min(single1, vector2.y);
			single = Mathf.Max(single, vector2.y + size.y);
		}
		for (int l = 0; l < this.children.Count; l++)
		{
			dfMarkupBox _dfMarkupBox1 = this.children[l];
			dfMarkupVerticalAlign _dfMarkupVerticalAlign = _dfMarkupBox1.Style.VerticalAlign;
			Vector2 position1 = _dfMarkupBox1.Position;
			Vector2 size1 = _dfMarkupBox1.Size;
			if (_dfMarkupVerticalAlign == dfMarkupVerticalAlign.Top)
			{
				position1.y = single1;
			}
			else if (_dfMarkupVerticalAlign == dfMarkupVerticalAlign.Bottom)
			{
				position1.y = single - size1.y;
			}
			else if (_dfMarkupVerticalAlign == dfMarkupVerticalAlign.Middle)
			{
				position1.y = (this.Size.y - size1.y) * 0.5f;
			}
			_dfMarkupBox1.Position = position1;
		}
		int num = 2147483647;
		for (int m = 0; m < this.children.Count; m++)
		{
			num = Mathf.Min(num, (int)this.children[m].Position.y);
		}
		for (int n = 0; n < this.children.Count; n++)
		{
			Vector2 vector21 = this.children[n].Position;
			vector21.y = vector21.y - (float)num;
			this.children[n].Position = vector21;
		}
	}

	private void endCurrentLine(bool removeEmpty = false)
	{
		if (this.currentLine == null)
		{
			return;
		}
		if (this.currentLinePos != 0)
		{
			this.currentLine.doHorizontalAlignment();
			this.currentLine.doVerticalAlignment();
		}
		else if (removeEmpty)
		{
			this.children.Remove(this.currentLine);
		}
		this.currentLine = null;
		this.currentLinePos = 0;
	}

	public void FitToContents(bool recursive = false)
	{
		if (this.children.Count == 0)
		{
			this.Size = new Vector2(this.Size.x, 0f);
			return;
		}
		this.endCurrentLine(false);
		Vector2 vector2 = Vector2.zero;
		for (int i = 0; i < this.children.Count; i++)
		{
			dfMarkupBox item = this.children[i];
			vector2 = Vector2.Max(vector2, item.Position + item.Size);
		}
		this.Size = vector2;
	}

	private dfMarkupBox GetContainingBlock()
	{
		for (dfMarkupBox i = this; i != null; i = i.Parent)
		{
			dfMarkupDisplayType display = i.Display;
			if ((display == dfMarkupDisplayType.block || display == dfMarkupDisplayType.inlineBlock || display == dfMarkupDisplayType.listItem || display == dfMarkupDisplayType.table || display == dfMarkupDisplayType.tableRow ? true : display == dfMarkupDisplayType.tableCell))
			{
				return i;
			}
		}
		return null;
	}

	public virtual Vector2 GetOffset()
	{
		Vector2 position = Vector2.zero;
		for (dfMarkupBox i = this; i != null; i = i.Parent)
		{
			position = position + i.Position;
		}
		return position;
	}

	private int getVerticalPosition(int topMargin)
	{
		if (this.children.Count == 0)
		{
			return topMargin;
		}
		int num = 0;
		int num1 = 0;
		for (int i = 0; i < this.children.Count; i++)
		{
			dfMarkupBox item = this.children[i];
			float position = item.Position.y + item.Size.y + (float)item.Margins.bottom;
			if (position > (float)num)
			{
				num = (int)position;
				num1 = i;
			}
		}
		dfMarkupBox _dfMarkupBox = this.children[num1];
		int num2 = Mathf.Max(_dfMarkupBox.Margins.bottom, topMargin);
		return (int)(_dfMarkupBox.Position.y + _dfMarkupBox.Size.y + (float)num2);
	}

	internal dfMarkupBox HitTest(Vector2 point)
	{
		Vector2 offset = this.GetOffset();
		Vector2 size = offset + this.Size;
		if (point.x < offset.x || point.x > size.x || point.y < offset.y || point.y > size.y)
		{
			return null;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			dfMarkupBox _dfMarkupBox = this.children[i].HitTest(point);
			if (_dfMarkupBox != null)
			{
				return _dfMarkupBox;
			}
		}
		return this;
	}

	protected virtual dfRenderData OnRebuildRenderData()
	{
		return null;
	}

	public virtual void Release()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].Release();
		}
		this.children.Clear();
		this.Element = null;
		this.Parent = null;
		this.Margins = new dfMarkupBorders();
	}

	internal dfRenderData Render()
	{
		dfRenderData dfRenderDatum;
		try
		{
			this.endCurrentLine(false);
			dfRenderDatum = this.OnRebuildRenderData();
		}
		finally
		{
		}
		return dfRenderDatum;
	}

	protected void renderDebugBox(dfRenderData renderData)
	{
		Vector3 vector3 = Vector3.zero;
		Vector3 size = vector3 + (Vector3.right * this.Size.x);
		Vector3 size1 = size + (Vector3.down * this.Size.y);
		Vector3 vector31 = vector3 + (Vector3.down * this.Size.y);
		renderData.Vertices.Add(vector3);
		renderData.Vertices.Add(size);
		renderData.Vertices.Add(size1);
		renderData.Vertices.Add(vector31);
		renderData.Triangles.AddRange(new int[] { 0, 1, 3, 3, 1, 2 });
		renderData.UV.Add(Vector2.zero);
		renderData.UV.Add(Vector2.zero);
		renderData.UV.Add(Vector2.zero);
		renderData.UV.Add(Vector2.zero);
		Color backgroundColor = this.Style.BackgroundColor;
		renderData.Colors.Add(backgroundColor);
		renderData.Colors.Add(backgroundColor);
		renderData.Colors.Add(backgroundColor);
		renderData.Colors.Add(backgroundColor);
	}
}