using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RenderAtDay : MonoBehaviour
{
	public TOD_Sky sky;

	private Renderer rendererComponent;

	public RenderAtDay()
	{
	}

	protected void OnEnable()
	{
		if (!this.sky)
		{
			Debug.LogError("Sky instance reference not set. Disabling script.");
			base.enabled = false;
		}
		this.rendererComponent = base.renderer;
	}

	protected void Update()
	{
		this.rendererComponent.enabled = this.sky.IsDay;
	}
}