using System;
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
public class TestTransformFind : MonoBehaviour
{
	public string find;

	public Transform foundFind;

	public Transform foundIter;

	public float findTime;

	public float iterTime;

	private Stopwatch findSW;

	private Stopwatch iterSW;

	public TestTransformFind()
	{
	}

	private void Update()
	{
		if (!string.IsNullOrEmpty(this.find))
		{
			if (this.findSW == null)
			{
				this.findSW = new Stopwatch();
			}
			this.findSW.Reset();
			this.findSW.Start();
			this.foundFind = base.transform.Find(this.find);
			this.findSW.Stop();
			this.findTime = (float)this.findSW.Elapsed.TotalMilliseconds;
			if (this.iterSW == null)
			{
				this.iterSW = new Stopwatch();
			}
			this.iterSW.Reset();
			this.iterSW.Start();
			this.foundIter = FindChildHelper.FindChildByName(this.find, this);
			this.iterSW.Stop();
			this.iterTime = (float)this.iterSW.Elapsed.TotalMilliseconds;
		}
		else
		{
			this.foundFind = null;
			this.foundIter = null;
		}
	}
}