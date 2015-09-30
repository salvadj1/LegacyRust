using System;
using UnityEngine;

[ExecuteInEditMode]
public class FPGrassPatch : MonoBehaviour, IFPGrassAsset
{
	[SerializeField]
	private Mesh mesh;

	[SerializeField]
	private float patchSize;

	[SerializeField]
	public FPGrassLevel level;

	[NonSerialized]
	private Vector3 lastRenderPosition;

	[NonSerialized]
	private Mesh runtimeMesh;

	[NonSerialized]
	private Bounds lastBounds;

	[NonSerialized]
	public Transform transform;

	public FPGrassPatch()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnEnable()
	{
		this.transform = base.transform;
		float single = Single.NegativeInfinity;
		float single1 = single;
		this.lastRenderPosition.z = single;
		float single2 = single1;
		single1 = single2;
		this.lastRenderPosition.y = single2;
		this.lastRenderPosition.x = single1;
	}

	internal void Render(ref FPGrass.RenderArguments renderArgs)
	{
		Bounds bound;
		bool flag;
		float single;
		float single1;
		float single2;
		float single3;
		float single4;
		float single5;
		if (renderArgs.terrain == null)
		{
			return;
		}
		Vector3 vector3 = renderArgs.center + this.transform.position;
		if (vector3.x != this.lastRenderPosition.x || vector3.y != this.lastRenderPosition.y || vector3.z != this.lastRenderPosition.z)
		{
			Vector3 vector31 = vector3;
			Vector3 vector32 = vector31;
			Vector3 vector33 = vector31;
			Vector3 vector34 = vector31;
			float single6 = this.patchSize * 0.5f;
			vector31.x = vector31.x - single6;
			vector31.z = vector31.z + single6;
			vector32.x = vector32.x - single6;
			vector32.z = vector32.z - single6;
			vector33.x = vector33.x + single6;
			vector33.z = vector33.z + single6;
			vector34.x = vector34.x + single6;
			vector34.z = vector34.z - single6;
			float single7 = renderArgs.terrain.SampleHeight(vector31);
			float single8 = renderArgs.terrain.SampleHeight(vector32);
			float single9 = renderArgs.terrain.SampleHeight(vector33);
			float single10 = renderArgs.terrain.SampleHeight(vector34);
			if (single7 >= single8)
			{
				single2 = single8;
				single4 = single7;
			}
			else
			{
				single2 = single7;
				single4 = single8;
			}
			if (single9 >= single10)
			{
				single3 = single10;
				single5 = single9;
			}
			else
			{
				single3 = single9;
				single5 = single10;
			}
			single = (single2 >= single3 ? single3 : single2);
			single1 = (single4 <= single5 ? single5 : single4);
			vector32.y = single - 5f;
			vector33.y = single1 + 5f;
			bound = new Bounds();
			bound.SetMinMax(vector32, vector33);
			flag = bound != this.lastBounds;
		}
		else
		{
			bound = this.lastBounds;
			flag = false;
		}
		if (!this.runtimeMesh)
		{
			this.runtimeMesh = this.mesh;
		}
		if (flag)
		{
			this.runtimeMesh.bounds = new Bounds(bound.center - vector3, bound.size);
			this.lastBounds = bound;
		}
		if (GeometryUtility.TestPlanesAABB(renderArgs.frustum, bound))
		{
			this.level.Draw(this, this.runtimeMesh, ref vector3, ref renderArgs);
		}
	}
}