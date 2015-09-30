using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Resize Handle")]
[ExecuteInEditMode]
[Serializable]
public class dfResizeHandle : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string backgroundSprite = string.Empty;

	[SerializeField]
	protected dfResizeHandle.ResizeEdge edges = dfResizeHandle.ResizeEdge.Right | dfResizeHandle.ResizeEdge.Bottom;

	private Vector3 mouseAnchorPos;

	private Vector3 startPosition;

	private Vector2 startSize;

	private Vector2 minEdgePos;

	private Vector2 maxEdgePos;

	public dfAtlas Atlas
	{
		get
		{
			if (this.atlas == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					dfAtlas defaultAtlas = manager.DefaultAtlas;
					dfAtlas dfAtla = defaultAtlas;
					this.atlas = defaultAtlas;
					return dfAtla;
				}
			}
			return this.atlas;
		}
		set
		{
			if (!dfAtlas.Equals(value, this.atlas))
			{
				this.atlas = value;
				this.Invalidate();
			}
		}
	}

	public string BackgroundSprite
	{
		get
		{
			return this.backgroundSprite;
		}
		set
		{
			if (value != this.backgroundSprite)
			{
				this.backgroundSprite = value;
				this.Invalidate();
			}
		}
	}

	public dfResizeHandle.ResizeEdge Edges
	{
		get
		{
			return this.edges;
		}
		set
		{
			this.edges = value;
		}
	}

	public dfResizeHandle()
	{
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		args.Use();
		Plane plane = new Plane(this.parent.transform.TransformDirection(Vector3.back), this.parent.transform.position);
		Ray ray = args.Ray;
		float single = 0f;
		plane.Raycast(args.Ray, out single);
		this.mouseAnchorPos = ray.origin + (ray.direction * single);
		this.startSize = this.parent.Size;
		this.startPosition = this.parent.RelativePosition;
		this.minEdgePos = this.startPosition;
		this.maxEdgePos = this.startPosition + this.startSize;
		Vector2 vector2 = this.parent.CalculateMinimumSize();
		Vector2 maximumSize = this.parent.MaximumSize;
		if (maximumSize.magnitude <= 1.401298E-45f)
		{
			maximumSize = Vector2.one * 2048f;
		}
		if ((this.Edges & dfResizeHandle.ResizeEdge.Left) == dfResizeHandle.ResizeEdge.Left)
		{
			this.minEdgePos.x = this.maxEdgePos.x - maximumSize.x;
			this.maxEdgePos.x = this.maxEdgePos.x - vector2.x;
		}
		else if ((this.Edges & dfResizeHandle.ResizeEdge.Right) == dfResizeHandle.ResizeEdge.Right)
		{
			this.minEdgePos.x = this.startPosition.x + vector2.x;
			this.maxEdgePos.x = this.startPosition.x + maximumSize.x;
		}
		if ((this.Edges & dfResizeHandle.ResizeEdge.Top) == dfResizeHandle.ResizeEdge.Top)
		{
			this.minEdgePos.y = this.maxEdgePos.y - maximumSize.y;
			this.maxEdgePos.y = this.maxEdgePos.y - vector2.y;
		}
		else if ((this.Edges & dfResizeHandle.ResizeEdge.Bottom) == dfResizeHandle.ResizeEdge.Bottom)
		{
			this.minEdgePos.y = this.startPosition.y + vector2.y;
			this.maxEdgePos.y = this.startPosition.y + maximumSize.y;
		}
	}

	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if (!args.Buttons.IsSet(dfMouseButtons.Left) || this.Edges == dfResizeHandle.ResizeEdge.None)
		{
			return;
		}
		args.Use();
		Ray ray = args.Ray;
		float single = 0f;
		Vector3 vector3 = base.GetCamera().transform.TransformDirection(Vector3.back);
		Plane plane = new Plane(vector3, this.mouseAnchorPos);
		plane.Raycast(ray, out single);
		float units = base.PixelsToUnits();
		Vector3 vector31 = ray.origin + (ray.direction * single);
		Vector3 vector32 = (vector31 - this.mouseAnchorPos) / units;
		vector32.y = vector32.y * -1f;
		float single1 = this.startPosition.x;
		float single2 = this.startPosition.y;
		float single3 = single1 + this.startSize.x;
		float single4 = single2 + this.startSize.y;
		if ((this.Edges & dfResizeHandle.ResizeEdge.Left) == dfResizeHandle.ResizeEdge.Left)
		{
			single1 = Mathf.Min(this.maxEdgePos.x, Mathf.Max(this.minEdgePos.x, single1 + vector32.x));
		}
		else if ((this.Edges & dfResizeHandle.ResizeEdge.Right) == dfResizeHandle.ResizeEdge.Right)
		{
			single3 = Mathf.Min(this.maxEdgePos.x, Mathf.Max(this.minEdgePos.x, single3 + vector32.x));
		}
		if ((this.Edges & dfResizeHandle.ResizeEdge.Top) == dfResizeHandle.ResizeEdge.Top)
		{
			single2 = Mathf.Min(this.maxEdgePos.y, Mathf.Max(this.minEdgePos.y, single2 + vector32.y));
		}
		else if ((this.Edges & dfResizeHandle.ResizeEdge.Bottom) == dfResizeHandle.ResizeEdge.Bottom)
		{
			single4 = Mathf.Min(this.maxEdgePos.y, Mathf.Max(this.minEdgePos.y, single4 + vector32.y));
		}
		this.parent.Size = new Vector2(single3 - single1, single4 - single2);
		this.parent.RelativePosition = new Vector3(single1, single2, 0f);
		if (this.parent.GetManager().PixelPerfectMode)
		{
			this.parent.MakePixelPerfect(true);
		}
	}

	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		base.Parent.MakePixelPerfect(true);
		args.Use();
		base.OnMouseUp(args);
	}

	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null || string.IsNullOrEmpty(this.backgroundSprite))
		{
			return;
		}
		dfAtlas.ItemInfo item = this.Atlas[this.backgroundSprite];
		if (item == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
		Color32 color32 = base.ApplyOpacity((!base.IsEnabled ? this.disabledColor : this.color));
		dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
		dfSprite.RenderOptions upperLeft = renderOption;
		upperLeft.atlas = this.atlas;
		upperLeft.color = color32;
		upperLeft.fillAmount = 1f;
		upperLeft.offset = this.pivot.TransformToUpperLeft(base.Size);
		upperLeft.pixelsToUnits = base.PixelsToUnits();
		upperLeft.size = base.Size;
		upperLeft.spriteInfo = item;
		renderOption = upperLeft;
		if (item.border.horizontal != 0 || item.border.vertical != 0)
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOption);
		}
		else
		{
			dfSprite.renderSprite(this.renderData, renderOption);
		}
	}

	public override void Start()
	{
		base.Start();
		if (base.Size.magnitude <= 1.401298E-45f)
		{
			base.Size = new Vector2(25f, 25f);
			if (base.Parent != null)
			{
				base.RelativePosition = base.Parent.Size - base.Size;
				base.Anchor = dfAnchorStyle.Bottom | dfAnchorStyle.Right;
			}
		}
	}

	[Flags]
	public enum ResizeEdge
	{
		None = 0,
		Left = 1,
		Right = 2,
		Top = 4,
		Bottom = 8
	}
}