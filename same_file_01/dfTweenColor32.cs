using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Tweens/Color32")]
public class dfTweenColor32 : dfTweenComponent<Color32>
{
	public dfTweenColor32()
	{
	}

	public override Color32 evaluate(Color32 startValue, Color32 endValue, float time)
	{
		Vector4 vector4 = startValue;
		Vector4 vector41 = endValue;
		Vector4 vector42 = new Vector4(dfTweenComponent<Color32>.Lerp(vector4.x, vector41.x, time), dfTweenComponent<Color32>.Lerp(vector4.y, vector41.y, time), dfTweenComponent<Color32>.Lerp(vector4.z, vector41.z, time), dfTweenComponent<Color32>.Lerp(vector4.w, vector41.w, time));
		return vector42;
	}

	public override Color32 offset(Color32 lhs, Color32 rhs)
	{
		return lhs + rhs;
	}
}