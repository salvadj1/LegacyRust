using NGUI.Meshing;
using NGUI.Structures;
using System;
using UnityEngine;

public class UISpriteWheel : UISlicedSprite
{
	[HideInInspector]
	[SerializeField]
	private float _innerRadius = 0.5f;

	[HideInInspector]
	[SerializeField]
	private Vector2 _center;

	[HideInInspector]
	[SerializeField]
	private int _slices;

	[HideInInspector]
	[SerializeField]
	private float _sliceDegrees;

	[HideInInspector]
	[SerializeField]
	private float _targetDegreeResolution = 10f;

	[HideInInspector]
	[SerializeField]
	private float _sliceFill = 1f;

	[HideInInspector]
	[SerializeField]
	private float _degreesOfRotation = 360f;

	[HideInInspector]
	[SerializeField]
	private float _facialRotationOffset;

	[HideInInspector]
	[SerializeField]
	private float _addDegrees;

	public float additionalRotation
	{
		get
		{
			return this._addDegrees;
		}
		set
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				return;
			}
			while (value > 180f)
			{
				value = value - 360f;
			}
			while (value <= -180f)
			{
				value = value + 360f;
			}
			if (value != this._addDegrees)
			{
				this._addDegrees = value;
				this.MarkAsChanged();
			}
		}
	}

	public Vector2 center
	{
		get
		{
			return this._center;
		}
		set
		{
			if (this._center != value)
			{
				this._center = value;
				this.MarkAsChanged();
			}
		}
	}

	public float circumferenceFillRatio
	{
		get
		{
			return this._sliceFill;
		}
		set
		{
			if (value < 0.05f)
			{
				value = 0.05f;
			}
			else if (value > 1f)
			{
				value = 1f;
			}
			if (this._sliceFill != value)
			{
				this._sliceFill = value;
				this.MarkAsChanged();
			}
		}
	}

	public float degreesOfRotation
	{
		get
		{
			return this._degreesOfRotation;
		}
		set
		{
			if (value < 0.01f)
			{
				value = 0.01f;
			}
			else if (value > 360f)
			{
				value = 360f;
			}
			if (value != this._degreesOfRotation)
			{
				this._degreesOfRotation = value;
				this.MarkAsChanged();
			}
		}
	}

	public float facialCrank
	{
		get
		{
			return this._facialRotationOffset;
		}
		set
		{
			if (value < -1f)
			{
				value = -1f;
			}
			else if (value > 1f)
			{
				value = 1f;
			}
			if (value != this._facialRotationOffset)
			{
				this._facialRotationOffset = value;
				this.MarkAsChanged();
			}
		}
	}

	public float innerRadius
	{
		get
		{
			return this._innerRadius;
		}
		set
		{
			if (value < 0f)
			{
				if (this._innerRadius != 0f)
				{
					this._innerRadius = 0f;
					this.MarkAsChanged();
				}
			}
			else if (value > 1f)
			{
				if (this._innerRadius != 1f)
				{
					this._innerRadius = 1f;
					this.MarkAsChanged();
				}
			}
			else if (this._innerRadius != value)
			{
				this._innerRadius = value;
				this.MarkAsChanged();
			}
		}
	}

	public float outerRadius
	{
		get
		{
			return 1f - this._innerRadius;
		}
		set
		{
			this.innerRadius = 1f - value;
		}
	}

	public float sliceDegrees
	{
		get
		{
			return this._sliceDegrees;
		}
		set
		{
			if (value < 0f)
			{
				value = 0f;
			}
			if (this._sliceDegrees != value)
			{
				this._sliceDegrees = value;
				this.MarkAsChanged();
			}
		}
	}

	public int slices
	{
		get
		{
			return this._slices;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			else if (value > 360)
			{
				value = 360;
			}
			if (this._slices != value)
			{
				this._slices = value;
				this.MarkAsChanged();
			}
		}
	}

	public float targetDegreeResolution
	{
		get
		{
			return this._targetDegreeResolution;
		}
		set
		{
			if (0.5f > value)
			{
				value = 0.5f;
			}
			if (this._targetDegreeResolution != value)
			{
				this._targetDegreeResolution = value;
				this.MarkAsChanged();
			}
		}
	}

	public UISpriteWheel()
	{
	}

	public override void OnFill(MeshBuffer m)
	{
		NineRectangle nineRectangle;
		NineRectangle nineRectangle1 = new NineRectangle();
		NineRectangle nineRectangle2;
		NineRectangle nineRectangle3 = new NineRectangle();
		Vector4 vector4 = new Vector4();
		Vector4 vector41 = new Vector4();
		float single = this._degreesOfRotation * 0.0174532924f;
		float single1 = this._sliceDegrees * 0.0174532924f;
		float single2 = this._sliceFill;
		int num = this.slices + 1;
		float single3 = (single - single1 * (float)this.slices) * single2;
		float single4 = single3 / (float)num;
		float single5 = single3 / 6.28318548f;
		float single6 = (single - single3) / (float)num;
		float3 _float3 = new float3()
		{
			xyz = base.cachedTransform.localScale
		};
		float single7 = (_float3.x >= _float3.y ? _float3.x : _float3.y);
		_float3.xy.x = 3.14159274f * single7 / (float)num * single5;
		_float3.xy.y = single7 * (this.outerRadius * 0.5f);
		vector4.x = this.mOuterUV.xMin;
		vector4.y = this.mInnerUV.xMin;
		vector4.z = this.mInnerUV.xMax;
		vector4.w = this.mOuterUV.xMax;
		vector41.x = this.mOuterUV.yMin;
		vector41.y = this.mInnerUV.yMin;
		vector41.z = this.mInnerUV.yMax;
		vector41.w = this.mOuterUV.yMax;
		NineRectangle.Calculate(UIWidget.Pivot.Center, base.atlas.pixelSize, base.mainTexture, ref vector4, ref vector41, ref _float3.xy, out nineRectangle, out nineRectangle2);
		if (this.innerRadius <= 0f || Mathf.Approximately(nineRectangle.zz.x - nineRectangle.yy.x, 0f))
		{
			nineRectangle1.xx.x = nineRectangle.xx.x;
			nineRectangle1.xx.y = nineRectangle.xx.y;
			nineRectangle1.yy.x = nineRectangle.yy.x;
			nineRectangle1.yy.y = nineRectangle.yy.y;
			nineRectangle1.zz.x = nineRectangle.zz.x;
			nineRectangle1.zz.y = nineRectangle.zz.y;
			nineRectangle1.ww.x = nineRectangle.ww.x;
			nineRectangle1.ww.y = nineRectangle.ww.y;
			nineRectangle3.xx.x = nineRectangle2.xx.x;
			nineRectangle3.xx.y = nineRectangle2.xx.y;
			nineRectangle3.yy.x = nineRectangle2.yy.x;
			nineRectangle3.yy.y = nineRectangle2.yy.y;
			nineRectangle3.zz.x = nineRectangle2.zz.x;
			nineRectangle3.zz.y = nineRectangle2.zz.y;
			nineRectangle3.ww.x = nineRectangle2.ww.x;
			nineRectangle3.ww.y = nineRectangle2.ww.y;
		}
		else
		{
			_float3.xy.x = 3.14159274f * single7 * this.innerRadius / (float)num * single5;
			NineRectangle.Calculate(UIWidget.Pivot.Center, base.atlas.pixelSize, base.mainTexture, ref vector4, ref vector41, ref _float3.xy, out nineRectangle1, out nineRectangle3);
			float single8 = (nineRectangle.yy.x + nineRectangle.zz.x) * 0.5f;
			if (nineRectangle1.yy.x > single8)
			{
				float single9 = (nineRectangle1.yy.x - single8) / (nineRectangle.ww.x - single8);
				if (single9 < 1f)
				{
					float single10 = 1f - single9;
					nineRectangle1.xx.y = nineRectangle.xx.y * single9 + nineRectangle1.xx.y * single10;
					nineRectangle1.yy.x = nineRectangle.yy.x * single9 + 0.5f * single10;
					nineRectangle1.yy.y = nineRectangle.yy.y * single9 + nineRectangle1.yy.y * single10;
					nineRectangle1.zz.x = nineRectangle.zz.x * single9 + 0.5f * single10;
					nineRectangle1.zz.y = nineRectangle.zz.y * single9 + nineRectangle1.zz.y * single10;
					nineRectangle1.ww.y = nineRectangle.ww.y * single9 + nineRectangle1.ww.y * single10;
					nineRectangle1.ww.x = nineRectangle.ww.x;
					nineRectangle1.xx.x = nineRectangle.xx.x;
				}
				else
				{
					nineRectangle1.xx.x = nineRectangle.xx.x;
					nineRectangle1.xx.y = nineRectangle.xx.y;
					nineRectangle1.yy.x = nineRectangle.yy.x;
					nineRectangle1.yy.y = nineRectangle.yy.y;
					nineRectangle1.zz.x = nineRectangle.zz.x;
					nineRectangle1.zz.y = nineRectangle.zz.y;
					nineRectangle1.ww.x = nineRectangle.ww.x;
					nineRectangle1.ww.y = nineRectangle.ww.y;
					nineRectangle3.xx.x = nineRectangle2.xx.x;
					nineRectangle3.xx.y = nineRectangle2.xx.y;
					nineRectangle3.yy.x = nineRectangle2.yy.x;
					nineRectangle3.yy.y = nineRectangle2.yy.y;
					nineRectangle3.zz.x = nineRectangle2.zz.x;
					nineRectangle3.zz.y = nineRectangle2.zz.y;
					nineRectangle3.ww.x = nineRectangle2.ww.x;
					nineRectangle3.ww.y = nineRectangle2.ww.y;
				}
			}
		}
		float single11 = Mathf.Abs(nineRectangle.ww.x - nineRectangle.xx.x);
		float single12 = single4 / single11;
		if (single1 > 0f)
		{
			single11 = single11 + single1 / single12;
			single12 = single4 / single11;
		}
		float single13 = this.innerRadius * 0.5f;
		float single14 = this.outerRadius * 0.5f;
		float single15 = Mathf.Min(nineRectangle.xx.y, nineRectangle.ww.y);
		float single16 = Mathf.Max(nineRectangle.ww.y, nineRectangle.xx.y) - single15;
		Color color = base.color;
		int num1 = m.vSize;
		float single17 = single6 + single4;
		float single18 = single6 * -0.5f + (this._facialRotationOffset * 0.5f + 0.5f) * single4 + this._addDegrees * 0.0174532924f;
		while (true)
		{
			Vertex[] vertexArray = m.v;
			int num2 = m.vSize;
			for (int i = num1; i < num2; i++)
			{
				float single19 = single13 + (vertexArray[i].y - single15) / single16 * single14;
				float single20 = vertexArray[i].x * single12 + single18;
				vertexArray[i].x = 0.5f + Mathf.Sin(single20) * single19;
				vertexArray[i].y = -0.5f + Mathf.Cos(single20) * single19;
			}
			int num3 = num - 1;
			num = num3;
			if (num3 <= 0)
			{
				break;
			}
			single18 = single18 + single17;
			num1 = num2;
		}
	}
}