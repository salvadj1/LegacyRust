using Facepunch.Precision;
using System;

public struct PerspectiveMatrixBuilder
{
	public double fieldOfView;

	public double aspectRatio;

	public double nearPlane;

	public double farPlane;

	public void ToProjectionMatrix(out Matrix4x4G proj)
	{
		Matrix4x4G.Perspective(ref this.fieldOfView, ref this.aspectRatio, ref this.nearPlane, ref this.farPlane, out proj);
	}
}