using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facepunch.Progress
{
	public static class IProgressUtility
	{
		public static bool Poll(this Facepunch.Progress.IProgress IProgress, out float progress)
		{
			bool flag;
			flag = (!(IProgress is UnityEngine.Object) ? object.ReferenceEquals(IProgress, null) : !(UnityEngine.Object)IProgress);
			if (flag)
			{
				progress = 0f;
				return false;
			}
			float single = IProgress.progress;
			if (single >= 1f)
			{
				progress = 1f;
			}
			else if (single > 0f)
			{
				progress = single;
			}
			else
			{
				progress = 0f;
			}
			return true;
		}
	}
}