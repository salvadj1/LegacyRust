using System;
using UnityEngine;

public class TestFlagsScript : MonoBehaviour
{
	public TestFlagsScript.E1 flags;

	public TestFlagsScript()
	{
	}

	[Flags]
	public enum E1
	{
		bit1 = 1,
		bit3 = 4,
		bit5 = 16
	}
}