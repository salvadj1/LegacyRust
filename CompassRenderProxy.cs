using System;
using UnityEngine;

[ExecuteInEditMode]
public class CompassRenderProxy : MonoBehaviour
{
	public float scalar = 0.7f;

	public Vector3 north = Vector3.up;

	public Vector3 forward = Vector3.forward;

	public float back = 0.3f;

	public bool bindNorth;

	public bool bindWest;

	public bool bindForward;

	private MaterialPropertyBlock propBlock;

	public CompassRenderProxy()
	{
	}

	private void BindFrame()
	{
		if (this.propBlock == null)
		{
			this.propBlock = new MaterialPropertyBlock();
		}
		else
		{
			this.propBlock.Clear();
		}
		Vector2 vector2 = base.transform.worldToLocalMatrix.MultiplyVector(this.north);
		vector2.Normalize();
		Vector2 vector21 = new Vector2(-vector2.y, vector2.x);
		vector21 = vector21 * this.scalar;
		vector2 = vector2 * this.scalar;
		if (this.bindNorth)
		{
			this.propBlock.AddVector(CompassRenderProxy.g.kPropLensUp, vector2);
		}
		if (this.bindWest)
		{
			this.propBlock.AddVector(CompassRenderProxy.g.kPropLensRight, vector21);
		}
		if (this.bindForward)
		{
			this.propBlock.AddVector(CompassRenderProxy.g.kPropLensDir, this.forward);
		}
		base.renderer.SetPropertyBlock(this.propBlock);
	}

	private void LateUpdate()
	{
		this.BindFrame();
	}

	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	private void OnBecameVisible()
	{
		base.enabled = true;
		this.BindFrame();
	}

	private static class g
	{
		public readonly static int kPropLensRight;

		public readonly static int kPropLensUp;

		public readonly static int kPropLensDir;

		static g()
		{
			CompassRenderProxy.g.kPropLensRight = Shader.PropertyToID("_LensRight");
			CompassRenderProxy.g.kPropLensUp = Shader.PropertyToID("_LensUp");
			CompassRenderProxy.g.kPropLensDir = Shader.PropertyToID("_LensForward");
		}
	}
}