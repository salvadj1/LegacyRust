using NGUI.Meshing;
using System;
using UnityEngine;

public class UIGeometry
{
	[NonSerialized]
	public MeshBuffer meshBuffer = new MeshBuffer();

	private bool vertsTransformed;

	private Vector3 lastPivotOffset;

	private Matrix4x4 lastWidgetToPanel;

	public bool hasTransformed
	{
		get
		{
			return (this.vertsTransformed ? true : this.meshBuffer.vSize == 0);
		}
	}

	public bool hasVertices
	{
		get
		{
			return this.meshBuffer.vSize > 0;
		}
	}

	public UIGeometry()
	{
	}

	public void Apply(ref Matrix4x4 widgetToPanel)
	{
		if (!this.vertsTransformed)
		{
			Debug.LogWarning("This overload of apply suggests you have called the other overload once before");
		}
		this.Apply(ref this.lastPivotOffset, ref widgetToPanel);
	}

	public void Apply(ref Vector3 pivotOffset, ref Matrix4x4 widgetToPanel)
	{
		if (!this.vertsTransformed)
		{
			this.meshBuffer.OffsetThenTransformVertices(pivotOffset.x, pivotOffset.y, pivotOffset.z, widgetToPanel.m00, widgetToPanel.m10, widgetToPanel.m20, widgetToPanel.m01, widgetToPanel.m11, widgetToPanel.m21, widgetToPanel.m02, widgetToPanel.m12, widgetToPanel.m22, widgetToPanel.m03, widgetToPanel.m13, widgetToPanel.m23);
			this.lastWidgetToPanel = widgetToPanel;
			this.lastPivotOffset = pivotOffset;
			this.vertsTransformed = true;
		}
		else if (pivotOffset != this.lastPivotOffset)
		{
			Debug.LogWarning("Verts were transformed more than once");
			Matrix4x4 matrix4x4 = this.lastWidgetToPanel.inverse;
			this.meshBuffer.TransformThenOffsetVertices(matrix4x4.m00, matrix4x4.m10, matrix4x4.m20, matrix4x4.m01, matrix4x4.m11, matrix4x4.m21, matrix4x4.m02, matrix4x4.m12, matrix4x4.m22, matrix4x4.m03, matrix4x4.m13, matrix4x4.m23, -this.lastPivotOffset.x, -this.lastPivotOffset.y, -this.lastPivotOffset.z);
			this.meshBuffer.OffsetThenTransformVertices(pivotOffset.x, pivotOffset.y, pivotOffset.z, widgetToPanel.m00, widgetToPanel.m10, widgetToPanel.m20, widgetToPanel.m01, widgetToPanel.m11, widgetToPanel.m21, widgetToPanel.m02, widgetToPanel.m12, widgetToPanel.m22, widgetToPanel.m03, widgetToPanel.m13, widgetToPanel.m23);
			this.lastWidgetToPanel = widgetToPanel;
			this.lastPivotOffset = pivotOffset;
		}
		else
		{
			if (widgetToPanel == this.lastWidgetToPanel)
			{
				return;
			}
			Matrix4x4 matrix4x41 = this.lastWidgetToPanel.inverse;
			this.lastWidgetToPanel = widgetToPanel;
			matrix4x41 = widgetToPanel * matrix4x41;
			this.meshBuffer.TransformVertices(matrix4x41.m00, matrix4x41.m10, matrix4x41.m20, matrix4x41.m01, matrix4x41.m11, matrix4x41.m21, matrix4x41.m02, matrix4x41.m12, matrix4x41.m22, matrix4x41.m03, matrix4x41.m13, matrix4x41.m23);
		}
	}

	public void Clear()
	{
		this.meshBuffer.Clear();
		this.vertsTransformed = false;
	}

	public void WriteToBuffers(MeshBuffer m)
	{
		this.meshBuffer.WriteBuffers(m);
	}
}