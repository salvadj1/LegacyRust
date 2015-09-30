using System;
using UnityEngine;

public class dfEasingFunctions
{
	public dfEasingFunctions()
	{
	}

	private static float bounce(float start, float end, float time)
	{
		time = time / 1f;
		end = end - start;
		if (time < 0.363636374f)
		{
			return end * (7.5625f * time * time) + start;
		}
		if (time < 0.727272749f)
		{
			time = time - 0.545454562f;
			return end * (7.5625f * time * time + 0.75f) + start;
		}
		if ((double)time < 0.909090909090909)
		{
			time = time - 0.8181818f;
			return end * (7.5625f * time * time + 0.9375f) + start;
		}
		time = time - 0.954545438f;
		return end * (7.5625f * time * time + 0.984375f) + start;
	}

	private static float clerp(float start, float end, float time)
	{
		float single = 0f;
		float single1 = 360f;
		float single2 = Mathf.Abs((single1 - single) / 2f);
		float single3 = 0f;
		float single4 = 0f;
		if (end - start < -single2)
		{
			single4 = (single1 - start + end) * time;
			single3 = start + single4;
		}
		else if (end - start <= single2)
		{
			single3 = start + (end - start) * time;
		}
		else
		{
			single4 = -(single1 - end + start) * time;
			single3 = start + single4;
		}
		return single3;
	}

	private static float easeInBack(float start, float end, float time)
	{
		end = end - start;
		time = time / 1f;
		float single = 1.70158f;
		return end * time * time * ((single + 1f) * time - single) + start;
	}

	private static float easeInCirc(float start, float end, float time)
	{
		end = end - start;
		return -end * (Mathf.Sqrt(1f - time * time) - 1f) + start;
	}

	private static float easeInCubic(float start, float end, float time)
	{
		end = end - start;
		return end * time * time * time + start;
	}

	private static float easeInExpo(float start, float end, float time)
	{
		end = end - start;
		return end * Mathf.Pow(2f, 10f * (time / 1f - 1f)) + start;
	}

	private static float easeInOutBack(float start, float end, float time)
	{
		float single = 1.70158f;
		end = end - start;
		time = time / 0.5f;
		if (time < 1f)
		{
			single = single * 1.525f;
			return end / 2f * (time * time * ((single + 1f) * time - single)) + start;
		}
		time = time - 2f;
		single = single * 1.525f;
		return end / 2f * (time * time * ((single + 1f) * time + single) + 2f) + start;
	}

	private static float easeInOutCirc(float start, float end, float time)
	{
		time = time / 0.5f;
		end = end - start;
		if (time < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - time * time) - 1f) + start;
		}
		time = time - 2f;
		return end / 2f * (Mathf.Sqrt(1f - time * time) + 1f) + start;
	}

	private static float easeInOutCubic(float start, float end, float time)
	{
		time = time / 0.5f;
		end = end - start;
		if (time < 1f)
		{
			return end / 2f * time * time * time + start;
		}
		time = time - 2f;
		return end / 2f * (time * time * time + 2f) + start;
	}

	private static float easeInOutExpo(float start, float end, float time)
	{
		time = time / 0.5f;
		end = end - start;
		if (time < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (time - 1f)) + start;
		}
		time = time - 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * time) + 2f) + start;
	}

	private static float easeInOutQuad(float start, float end, float time)
	{
		time = time / 0.5f;
		end = end - start;
		if (time < 1f)
		{
			return end / 2f * time * time + start;
		}
		time = time - 1f;
		return -end / 2f * (time * (time - 2f) - 1f) + start;
	}

	private static float easeInOutQuart(float start, float end, float time)
	{
		time = time / 0.5f;
		end = end - start;
		if (time < 1f)
		{
			return end / 2f * time * time * time * time + start;
		}
		time = time - 2f;
		return -end / 2f * (time * time * time * time - 2f) + start;
	}

	private static float easeInOutQuint(float start, float end, float time)
	{
		time = time / 0.5f;
		end = end - start;
		if (time < 1f)
		{
			return end / 2f * time * time * time * time * time + start;
		}
		time = time - 2f;
		return end / 2f * (time * time * time * time * time + 2f) + start;
	}

	private static float easeInOutSine(float start, float end, float time)
	{
		end = end - start;
		return -end / 2f * (Mathf.Cos(3.14159274f * time / 1f) - 1f) + start;
	}

	private static float easeInQuad(float start, float end, float time)
	{
		end = end - start;
		return end * time * time + start;
	}

	private static float easeInQuart(float start, float end, float time)
	{
		end = end - start;
		return end * time * time * time * time + start;
	}

	private static float easeInQuint(float start, float end, float time)
	{
		end = end - start;
		return end * time * time * time * time * time + start;
	}

	private static float easeInSine(float start, float end, float time)
	{
		end = end - start;
		return -end * Mathf.Cos(time / 1f * 1.57079637f) + end + start;
	}

	private static float easeOutBack(float start, float end, float time)
	{
		float single = 1.70158f;
		end = end - start;
		time = time / 1f - 1f;
		return end * (time * time * ((single + 1f) * time + single) + 1f) + start;
	}

	private static float easeOutCirc(float start, float end, float time)
	{
		time = time - 1f;
		end = end - start;
		return end * Mathf.Sqrt(1f - time * time) + start;
	}

	private static float easeOutCubic(float start, float end, float time)
	{
		time = time - 1f;
		end = end - start;
		return end * (time * time * time + 1f) + start;
	}

	private static float easeOutExpo(float start, float end, float time)
	{
		end = end - start;
		return end * (-Mathf.Pow(2f, -10f * time / 1f) + 1f) + start;
	}

	private static float easeOutQuad(float start, float end, float time)
	{
		end = end - start;
		return -end * time * (time - 2f) + start;
	}

	private static float easeOutQuart(float start, float end, float time)
	{
		time = time - 1f;
		end = end - start;
		return -end * (time * time * time * time - 1f) + start;
	}

	private static float easeOutQuint(float start, float end, float time)
	{
		time = time - 1f;
		end = end - start;
		return end * (time * time * time * time * time + 1f) + start;
	}

	private static float easeOutSine(float start, float end, float time)
	{
		end = end - start;
		return end * Mathf.Sin(time / 1f * 1.57079637f) + start;
	}

	public static dfEasingFunctions.EasingFunction GetFunction(dfEasingType easeType)
	{
		switch (easeType)
		{
			case dfEasingType.Linear:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.linear);
			}
			case dfEasingType.Bounce:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.bounce);
			}
			case dfEasingType.BackEaseIn:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInBack);
			}
			case dfEasingType.BackEaseOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutBack);
			}
			case dfEasingType.BackEaseInOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutBack);
			}
			case dfEasingType.CircEaseIn:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInCirc);
			}
			case dfEasingType.CircEaseOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutCirc);
			}
			case dfEasingType.CircEaseInOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutCirc);
			}
			case dfEasingType.CubicEaseIn:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInCubic);
			}
			case dfEasingType.CubicEaseOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutCubic);
			}
			case dfEasingType.CubicEaseInOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutCubic);
			}
			case dfEasingType.ExpoEaseIn:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInExpo);
			}
			case dfEasingType.ExpoEaseOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutExpo);
			}
			case dfEasingType.ExpoEaseInOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutExpo);
			}
			case dfEasingType.QuadEaseIn:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInQuad);
			}
			case dfEasingType.QuadEaseOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutQuad);
			}
			case dfEasingType.QuadEaseInOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutQuad);
			}
			case dfEasingType.QuartEaseIn:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInQuart);
			}
			case dfEasingType.QuartEaseOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutQuart);
			}
			case dfEasingType.QuartEaseInOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutQuart);
			}
			case dfEasingType.QuintEaseIn:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInQuint);
			}
			case dfEasingType.QuintEaseOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutQuint);
			}
			case dfEasingType.QuintEaseInOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutQuint);
			}
			case dfEasingType.SineEaseIn:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInSine);
			}
			case dfEasingType.SineEaseOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutSine);
			}
			case dfEasingType.SineEaseInOut:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutSine);
			}
			case dfEasingType.Spring:
			{
				return new dfEasingFunctions.EasingFunction(dfEasingFunctions.spring);
			}
		}
		throw new NotImplementedException();
	}

	private static float linear(float start, float end, float time)
	{
		return Mathf.Lerp(start, end, time);
	}

	private static float punch(float amplitude, float time)
	{
		float single = 9f;
		if (time == 0f)
		{
			return 0f;
		}
		if (time == 1f)
		{
			return 0f;
		}
		float single1 = 0.3f;
		single = single1 / 6.28318548f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * time) * Mathf.Sin((time * 1f - single) * 6.28318548f / single1);
	}

	private static float spring(float start, float end, float time)
	{
		time = Mathf.Clamp01(time);
		time = (Mathf.Sin(time * 3.14159274f * (0.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + 1.2f * (1f - time));
		return start + (end - start) * time;
	}

	public delegate float EasingFunction(float start, float end, float time);
}