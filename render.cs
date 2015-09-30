using System;
using UnityEngine;

public class render : ConsoleSystem
{
	private static float distance_real;

	private static int frames_real;

	private static int fov_real;

	private static bool vsync_real;

	[Client]
	[Help("The relative render distance. (0-1)", "")]
	[Saved]
	public static float distance
	{
		get
		{
			return render.distance_real;
		}
		set
		{
			render.distance_real = Mathf.Clamp01(value);
			render.update();
		}
	}

	[Client]
	[Help("The field of view. (60-120, default 60)", "")]
	[Saved]
	public static int fov
	{
		get
		{
			return render.fov_real;
		}
		set
		{
			render.fov_real = Mathf.Clamp(value, 60, 120);
			render.update();
		}
	}

	[Client]
	[Help("The limit for how many frames may be rendered per second. (default -1 for no fps limit)", "")]
	[Saved]
	public static int frames
	{
		get
		{
			return render.frames_real;
		}
		set
		{
			render.frames_real = value;
			render.update();
		}
	}

	[Client]
	[Help("The render quality level. (0-1)", "")]
	public static float level
	{
		get
		{
			return (float)QualitySettings.GetQualityLevel() / (float)((int)QualitySettings.names.Length - 1);
		}
		set
		{
			int num = Mathf.RoundToInt(value * (float)((int)QualitySettings.names.Length - 1));
			QualitySettings.SetQualityLevel(num, true);
			render.update();
		}
	}

	[Client]
	[Help("Whether VSync should be enabled or disabled", "")]
	[Saved]
	public static bool vsync
	{
		get
		{
			return render.vsync_real;
		}
		set
		{
			render.vsync_real = value;
			render.update();
		}
	}

	static render()
	{
		render.distance_real = 0.2f;
		render.frames_real = -1;
		render.fov_real = 60;
	}

	public render()
	{
	}

	[Client]
	[Help("Makes sure settings match their convar values. You shouldn't need to call this manually.", "")]
	public static void update(ref ConsoleSystem.Arg args)
	{
		render.update();
	}

	private static void update()
	{
		QualitySettings.vSyncCount = (!render.vsync_real ? 0 : 1);
		Application.targetFrameRate = render.frames_real;
		int qualityLevel = QualitySettings.GetQualityLevel();
		if (PlayerPrefs.GetInt("UnityGraphicsQualityBackup", -1) != qualityLevel)
		{
			PlayerPrefs.SetInt("UnityGraphicsQualityBackup", qualityLevel);
			switch (qualityLevel)
			{
				case 0:
				case 1:
				case 2:
				{
					gfx.ssaa = false;
					gfx.bloom = false;
					gfx.ssao = false;
					gfx.tonemap = false;
					gfx.shafts = false;
					break;
				}
				case 3:
				{
					gfx.ssaa = false;
					gfx.bloom = false;
					gfx.ssao = false;
					gfx.tonemap = false;
					gfx.shafts = true;
					break;
				}
				case 4:
				{
					break;
				}
				default:
				{
					gfx.ssaa = true;
					gfx.bloom = true;
					gfx.ssao = true;
					gfx.tonemap = true;
					gfx.shafts = true;
					break;
				}
			}
		}
		GameEvent.DoQualitySettingsRefresh();
	}
}