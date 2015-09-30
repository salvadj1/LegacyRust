using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class dfFontRendererBase : IDisposable
{
	public Color32? BottomColor
	{
		get;
		set;
	}

	public int CharacterSpacing
	{
		get;
		set;
	}

	public bool ColorizeSymbols
	{
		get;
		set;
	}

	public Color32 DefaultColor
	{
		get;
		set;
	}

	public dfFontBase Font
	{
		get;
		protected set;
	}

	public int LinesRendered
	{
		get;
		internal set;
	}

	public Vector2 MaxSize
	{
		get;
		set;
	}

	public bool MultiLine
	{
		get;
		set;
	}

	public float Opacity
	{
		get;
		set;
	}

	public bool Outline
	{
		get;
		set;
	}

	public Color32 OutlineColor
	{
		get;
		set;
	}

	public int OutlineSize
	{
		get;
		set;
	}

	public bool OverrideMarkupColors
	{
		get;
		set;
	}

	public float PixelRatio
	{
		get;
		set;
	}

	public bool ProcessMarkup
	{
		get;
		set;
	}

	public Vector2 RenderedSize
	{
		get;
		internal set;
	}

	public bool Shadow
	{
		get;
		set;
	}

	public Color32 ShadowColor
	{
		get;
		set;
	}

	public Vector2 ShadowOffset
	{
		get;
		set;
	}

	public int TabSize
	{
		get;
		set;
	}

	public List<int> TabStops
	{
		get;
		set;
	}

	public TextAlignment TextAlign
	{
		get;
		set;
	}

	public float TextScale
	{
		get;
		set;
	}

	public Vector3 VectorOffset
	{
		get;
		set;
	}

	public bool WordWrap
	{
		get;
		set;
	}

	protected dfFontRendererBase()
	{
	}

	public void Dispose()
	{
		this.Release();
	}

	public abstract float[] GetCharacterWidths(string text);

	public abstract Vector2 MeasureString(string text);

	public abstract void Release();

	public abstract void Render(string text, dfRenderData destination);

	protected virtual void Reset()
	{
		this.Font = null;
		this.PixelRatio = 0f;
		this.TextScale = 1f;
		this.CharacterSpacing = 0;
		this.VectorOffset = Vector3.zero;
		this.ProcessMarkup = false;
		this.WordWrap = false;
		this.MultiLine = false;
		this.OverrideMarkupColors = false;
		this.ColorizeSymbols = false;
		this.TextAlign = TextAlignment.Left;
		this.DefaultColor = Color.white;
		this.BottomColor = null;
		this.Opacity = 1f;
		this.Outline = false;
		this.Shadow = false;
	}
}