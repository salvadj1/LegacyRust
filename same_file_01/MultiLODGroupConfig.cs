using System;
using UnityEngine;

public class MultiLODGroupConfig : ScriptableObject
{
	public const string LODGroupArray = "a";

	public const string LODFractionArray = "l";

	[SerializeField]
	private LODGroup[] a;

	[SerializeField]
	public float[] l;

	public MultiLODGroupConfig()
	{
	}
}