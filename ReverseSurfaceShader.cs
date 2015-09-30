using System;
using UnityEngine;

public class ReverseSurfaceShader : ScriptableObject
{
	public Shader inputShader;

	public Shader outputShader;

	public string outputShaderName;

	public bool pragmaDebug;

	public ShaderMod[] mods;

	public ReverseSurfaceShader()
	{
	}
}