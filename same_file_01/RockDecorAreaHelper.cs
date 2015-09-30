using System;
using UnityEngine;

public class RockDecorAreaHelper : MonoBehaviour
{
	public RockDecorAreaHelper()
	{
	}

	private void DrawBounds()
	{
		Color color = Gizmos.color;
		color.a = 0.25f;
		Gizmos.color = color;
		Gizmos.DrawCube(base.transform.position, base.transform.localScale);
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(base.transform.position, base.transform.localScale);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.grey;
		this.DrawBounds();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		this.DrawBounds();
	}
}