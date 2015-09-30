using System;
using UnityEngine;

public class IgnoreColliders : MonoBehaviour
{
	public Collider[] a;

	public Collider[] b;

	public IgnoreColliders()
	{
	}

	private void Awake()
	{
		if (this.a != null && this.b != null)
		{
			int num = Mathf.Min((int)this.a.Length, (int)this.b.Length);
			for (int i = 0; i < num; i++)
			{
				if (this.a[i] && this.b[i] && this.b[i] != this.a[i])
				{
					Physics.IgnoreCollision(this.a[i], this.b[i]);
				}
			}
			this.a = null;
			this.b = null;
		}
	}
}