using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/GUI Camera")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[Serializable]
public class dfGUICamera : MonoBehaviour
{
	public dfGUICamera()
	{
	}

	public void Awake()
	{
	}

	public void OnEnable()
	{
	}

	public void Start()
	{
		base.camera.transparencySortMode = TransparencySortMode.Orthographic;
		base.camera.useOcclusionCulling = false;
		Camera camera = base.camera;
		camera.eventMask = camera.eventMask & ~base.camera.cullingMask;
	}
}