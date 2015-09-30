using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

public class dfMarkupBoxText : dfMarkupBox
{
	private static int[] TRIANGLE_INDICES;

	private static Queue<dfMarkupBoxText> objectPool;

	private static Regex whitespacePattern;

	private dfRenderData renderData = new dfRenderData(32);

	private bool isWhitespace;

	public bool IsWhitespace
	{
		get
		{
			return this.isWhitespace;
		}
	}

	public string Text
	{
		get;
		private set;
	}

	static dfMarkupBoxText()
	{
		dfMarkupBoxText.TRIANGLE_INDICES = new int[] { 0, 1, 2, 0, 2, 3 };
		dfMarkupBoxText.objectPool = new Queue<dfMarkupBoxText>();
		dfMarkupBoxText.whitespacePattern = new Regex("\\s+");
	}

	public dfMarkupBoxText(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style) : base(element, display, style)
	{
	}

	private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] tRIANGLEINDICES = dfMarkupBoxText.TRIANGLE_INDICES;
		for (int i = 0; i < (int)tRIANGLEINDICES.Length; i++)
		{
			triangles.Add(count + tRIANGLEINDICES[i]);
		}
	}

	public static dfMarkupBoxText Obtain(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style)
	{
		if (dfMarkupBoxText.objectPool.Count <= 0)
		{
			return new dfMarkupBoxText(element, display, style);
		}
		dfMarkupBoxText fontSize = dfMarkupBoxText.objectPool.Dequeue();
		fontSize.Element = element;
		fontSize.Display = display;
		fontSize.Style = style;
		fontSize.Position = Vector2.zero;
		fontSize.Size = Vector2.zero;
		fontSize.Baseline = (int)((float)style.FontSize * 1.1f);
		fontSize.Margins = new dfMarkupBorders();
		fontSize.Padding = new dfMarkupBorders();
		return fontSize;
	}

	protected override dfRenderData OnRebuildRenderData()
	{
		this.renderData.Clear();
		if (this.Style.Font == null)
		{
			return null;
		}
		if (this.Style.TextDecoration == dfMarkupTextDecoration.Underline)
		{
			this.renderUnderline();
		}
		this.renderText(this.Text);
		return this.renderData;
	}

	public override void Release()
	{
		base.Release();
		this.Text = string.Empty;
		this.renderData.Clear();
		dfMarkupBoxText.objectPool.Enqueue(this);
	}

	private void renderText(string text)
	{
		dfDynamicFont font = this.Style.Font;
		int fontSize = this.Style.FontSize;
		FontStyle fontStyle = this.Style.FontStyle;
		dfList<Vector3> vertices = this.renderData.Vertices;
		dfList<int> triangles = this.renderData.Triangles;
		dfList<Vector2> uV = this.renderData.UV;
		dfList<Color32> colors = this.renderData.Colors;
		float single = (float)fontSize / (float)font.FontSize;
		float descent = (float)font.Descent * single;
		float num = 0f;
		CharacterInfo[] characterInfoArray = font.RequestCharacters(text, fontSize, fontStyle);
		this.renderData.Material = font.Material;
		for (int i = 0; i < text.Length; i++)
		{
			CharacterInfo characterInfo = characterInfoArray[i];
			dfMarkupBoxText.addTriangleIndices(vertices, triangles);
			float fontSize1 = (float)font.FontSize + characterInfo.vert.y - (float)fontSize + descent;
			float single1 = num + characterInfo.vert.x;
			float single2 = fontSize1;
			float single3 = single1 + characterInfo.vert.width;
			float single4 = single2 + characterInfo.vert.height;
			Vector3 vector3 = new Vector3(single1, single2);
			Vector3 vector31 = new Vector3(single3, single2);
			Vector3 vector32 = new Vector3(single3, single4);
			Vector3 vector33 = new Vector3(single1, single4);
			vertices.Add(vector3);
			vertices.Add(vector31);
			vertices.Add(vector32);
			vertices.Add(vector33);
			Color color = this.Style.Color;
			colors.Add(color);
			colors.Add(color);
			colors.Add(color);
			colors.Add(color);
			Rect rect = characterInfo.uv;
			float single5 = rect.x;
			float single6 = rect.y + rect.height;
			float single7 = single5 + rect.width;
			float single8 = rect.y;
			if (!characterInfo.flipped)
			{
				uV.Add(new Vector2(single5, single6));
				uV.Add(new Vector2(single7, single6));
				uV.Add(new Vector2(single7, single8));
				uV.Add(new Vector2(single5, single8));
			}
			else
			{
				uV.Add(new Vector2(single7, single8));
				uV.Add(new Vector2(single7, single6));
				uV.Add(new Vector2(single5, single6));
				uV.Add(new Vector2(single5, single8));
			}
			num = num + (float)Mathf.CeilToInt(characterInfo.vert.x + characterInfo.vert.width);
		}
	}

	private void renderUnderline()
	{
	}

	internal void SetText(string text)
	{
		this.Text = text;
		if (this.Style.Font == null)
		{
			return;
		}
		this.isWhitespace = dfMarkupBoxText.whitespacePattern.IsMatch(this.Text);
		string str = (this.Style.PreserveWhitespace || !this.isWhitespace ? this.Text : " ");
		CharacterInfo[] characterInfoArray = this.Style.Font.RequestCharacters(str, this.Style.FontSize, this.Style.FontStyle);
		int fontSize = this.Style.FontSize;
		Vector2 vector2 = new Vector2(0f, (float)this.Style.LineHeight);
		for (int i = 0; i < str.Length; i++)
		{
			CharacterInfo characterInfo = characterInfoArray[i];
			float single = characterInfo.vert.x + characterInfo.vert.width;
			if (str[i] == ' ')
			{
				single = Mathf.Max(single, (float)fontSize * 0.33f);
			}
			else if (str[i] == '\t')
			{
				single = single + (float)(fontSize * 3);
			}
			vector2.x = vector2.x + single;
		}
		this.Size = vector2;
		dfDynamicFont font = this.Style.Font;
		float fontSize1 = (float)fontSize / (float)font.FontSize;
		this.Baseline = Mathf.CeilToInt((float)font.Baseline * fontSize1);
	}
}