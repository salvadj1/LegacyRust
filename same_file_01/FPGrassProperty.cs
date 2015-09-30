using System;
using UnityEngine;

[ExecuteInEditMode]
public class FPGrassProperty : ScriptableObject, IFPGrassAsset
{
	[SerializeField]
	private Color color1 = Color.white;

	[SerializeField]
	private Color color2 = Color.white;

	[SerializeField]
	private float minWidth = 1f;

	[SerializeField]
	private float maxWidth = 1f;

	[SerializeField]
	private float minHeight = 1f;

	[SerializeField]
	private float maxHeight = 1f;

	public Color Color1
	{
		get
		{
			return this.color1;
		}
		set
		{
			this.color1 = value;
		}
	}

	public Color Color2
	{
		get
		{
			return this.color2;
		}
		set
		{
			this.color2 = value;
		}
	}

	public float MaxHeight
	{
		get
		{
			return this.maxHeight;
		}
		set
		{
			this.maxHeight = value;
		}
	}

	public float MaxWidth
	{
		get
		{
			return this.maxWidth;
		}
		set
		{
			this.maxWidth = value;
		}
	}

	public float MinHeight
	{
		get
		{
			return this.minHeight;
		}
		set
		{
			this.minHeight = value;
		}
	}

	public float MinWidth
	{
		get
		{
			return this.minWidth;
		}
		set
		{
			this.minWidth = value;
		}
	}

	public FPGrassProperty()
	{
	}
}