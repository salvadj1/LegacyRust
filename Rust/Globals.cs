using System;

namespace Rust
{
	public static class Globals
	{
		public static string currentLevel;

		public static bool isPlaying;

		public static bool isLoading;

		static Globals()
		{
			Globals.currentLevel = string.Empty;
		}
	}
}