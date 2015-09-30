using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Tweens/Color")]
public class dfTweenColor : dfTweenComponent<Color>
{
	public dfTweenColor()
	{
	}

	public override Color evaluate(Color startValue, Color endValue, float time)
	{
		Vector4 vector4 = startValue;
		Vector4 vector41 = endValue;
		Vector4 vector42 = new Vector4(dfTweenComponent<Color>.Lerp(vector4.x, vector41.x, time), dfTweenComponent<Color>.Lerp(vector4.y, vector41.y, time), dfTweenComponent<Color>.Lerp(vector4.z, vector41.z, time), dfTweenComponent<Color>.Lerp(vector4.w, vector41.w, time));
		return vector42;
	}

	public override Color offset(Color lhs, Color rhs)
	{
		return lhs + rhs;
	}
}