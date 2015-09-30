using System;

public class gfx : ConsoleSystem
{
	[Client]
	public static bool all
	{
		get
		{
			return (!gfx.ssaa || !gfx.bloom || !gfx.grain || !gfx.ssao || !gfx.tonemap || !gfx.shafts ? false : gfx.damage);
		}
		set
		{
			bool flag = value;
			gfx.damage = flag;
			flag = flag;
			gfx.shafts = flag;
			flag = flag;
			gfx.tonemap = flag;
			flag = flag;
			gfx.ssao = flag;
			flag = flag;
			gfx.grain = flag;
			flag = flag;
			gfx.bloom = flag;
			gfx.ssaa = flag;
		}
	}

	[Client]
	[Saved]
	public static bool bloom
	{
		get
		{
			return ImageEffectManager.GetEnabled<Bloom>();
		}
		set
		{
			ImageEffectManager.SetEnabled<Bloom>(value);
		}
	}

	[Client]
	[Saved]
	public static bool damage
	{
		get
		{
			return ImageEffectManager.GetEnabled<GameFullscreen>();
		}
		set
		{
			ImageEffectManager.SetEnabled<GameFullscreen>(value);
		}
	}

	[Client]
	[Saved]
	public static bool grain
	{
		get
		{
			return ImageEffectManager.GetEnabled<NoiseAndGrain>();
		}
		set
		{
			ImageEffectManager.SetEnabled<NoiseAndGrain>(value);
		}
	}

	[Client]
	[Saved]
	public static bool shafts
	{
		get
		{
			return ImageEffectManager.GetEnabled<TOD_SunShafts>();
		}
		set
		{
			ImageEffectManager.SetEnabled<TOD_SunShafts>(value);
		}
	}

	[Client]
	[Saved]
	public static bool ssaa
	{
		get
		{
			return ImageEffectManager.GetEnabled<AntialiasingAsPostEffect>();
		}
		set
		{
			ImageEffectManager.SetEnabled<AntialiasingAsPostEffect>(value);
		}
	}

	[Client]
	[Saved]
	public static bool ssao
	{
		get
		{
			return ImageEffectManager.GetEnabled<SSAOEffect>();
		}
		set
		{
			ImageEffectManager.SetEnabled<SSAOEffect>(value);
		}
	}

	[Client]
	[Saved]
	public static bool tonemap
	{
		get
		{
			return ImageEffectManager.GetEnabled<Tonemapping>();
		}
		set
		{
			ImageEffectManager.SetEnabled<Tonemapping>(value);
		}
	}

	public gfx()
	{
	}
}