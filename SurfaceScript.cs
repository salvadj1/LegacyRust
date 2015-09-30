using System;
using UnityEngine;

public class SurfaceScript : MonoBehaviour
{
	public SurfaceScript()
	{
	}

	private void Start()
	{
		Material material;
		material = (base.transform.parent.GetComponent<MarkerScript>().objectScript.materialType != 0 ? (Material)UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("surfaceAlphaMaterial", typeof(Material))) : (Material)UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("surfaceMaterial", typeof(Material))));
		Color component = material.color;
		component.a = base.transform.parent.GetComponent<MarkerScript>().objectScript.surfaceOpacity;
		base.gameObject.renderer.sharedMaterial = material;
	}
}