using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CraterMaker : MonoBehaviour
{
	public Terrain MyTerrain;

	public int insidetextureindex;

	public CraterMaker()
	{
	}

	public void Create(Vector3 position, float radius, float depth, float noise)
	{
		this.Create(new Vector2(position.x, position.z), radius, depth, noise);
	}

	public void Create(Vector2 position, float radius, float depth, float noise)
	{
		base.StartCoroutine(this.RealCreate(position, radius, depth, noise));
	}

	[DebuggerHidden]
	public IEnumerator RealCreate(Vector2 position, float radius, float depth, float noise)
	{
		CraterMaker.<RealCreate>c__Iterator4D variable = null;
		return variable;
	}
}