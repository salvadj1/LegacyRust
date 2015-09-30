using System;
using UnityEngine;

[ExecuteInEditMode]
public class FPGrassDisplacementCamera : MonoBehaviour
{
	[NonSerialized]
	public Material blitMat;

	public static FPGrassDisplacementCamera singleton
	{
		get
		{
			return FPGrassDisplacementCamera.Global.singleton;
		}
	}

	public FPGrassDisplacementCamera()
	{
	}

	public void Awake()
	{
	}

	public static FPGrassDisplacementCamera Get()
	{
		return FPGrassDisplacementCamera.singleton;
	}

	public static Material GetBlitMat()
	{
		return FPGrassDisplacementCamera.singleton.blitMat;
	}

	public static RenderTexture GetRT()
	{
		return FPGrassDisplacementCamera.singleton.camera.targetTexture;
	}

	public void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(base.camera.targetTexture);
		UnityEngine.Object.DestroyImmediate(this.blitMat);
	}

	private static class Global
	{
		public static FPGrassDisplacementCamera singleton;

		static Global()
		{
			GameObject gameObject = GameObject.FindWithTag("DisplacementCamera");
			if (gameObject)
			{
				FPGrassDisplacementCamera.Global.singleton = gameObject.GetComponent<FPGrassDisplacementCamera>();
				if (FPGrassDisplacementCamera.Global.singleton)
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
				FPGrassDisplacementCamera.Global.singleton = null;
			}
			GameObject color = new GameObject("FPGrassDisplacementCamera")
			{
				hideFlags = HideFlags.DontSave
			};
			color.AddComponent<Camera>();
			color.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
			color.camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
			color.camera.clearFlags = CameraClearFlags.Color;
			color.camera.orthographic = true;
			color.camera.orthographicSize = 50f;
			color.camera.nearClipPlane = 0.3f;
			color.camera.farClipPlane = 1000f;
			color.camera.renderingPath = RenderingPath.VertexLit;
			color.camera.enabled = false;
			color.camera.cullingMask = 1 << (LayerMask.NameToLayer("GrassDisplacement") & 31);
			color.camera.tag = "DisplacementCamera";
			FPGrassDisplacementCamera.Global.singleton = color.AddComponent<FPGrassDisplacementCamera>();
			RenderTexture renderTexture = new RenderTexture(512, 512, 0, FPGrass.Support.ProbabilityRenderTextureFormat1Channel)
			{
				hideFlags = HideFlags.DontSave
			};
			RenderTexture renderTexture1 = renderTexture;
			renderTexture1.Create();
			renderTexture1.name = "FPGrassDisplacement_RT";
			color.camera.targetTexture = renderTexture1;
			FPGrassDisplacementCamera fPGrassDisplacementCamera = FPGrassDisplacementCamera.Global.singleton;
			Material material = new Material(Shader.Find("Custom/DisplacementBlit"))
			{
				hideFlags = HideFlags.DontSave
			};
			fPGrassDisplacementCamera.blitMat = material;
		}
	}
}