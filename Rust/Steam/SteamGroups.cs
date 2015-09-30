using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rust.Steam
{
	public static class SteamGroups
	{
		public static List<SteamGroups.Group> groupList;

		static SteamGroups()
		{
			SteamGroups.groupList = new List<SteamGroups.Group>();
		}

		public static void Init()
		{
			SteamGroups.groupList.Clear();
			int num = SteamGroups.SteamGroup_GetCount();
			for (int i = 0; i < num; i++)
			{
				SteamGroups.Group group = new SteamGroups.Group()
				{
					steamid = SteamGroups.SteamGroup_GetSteamID(i)
				};
				SteamGroups.groupList.Add(group);
			}
		}

		public static bool MemberOf(ulong iGroupID)
		{
			return SteamGroups.groupList.Any<SteamGroups.Group>((SteamGroups.Group item) => item.steamid == iGroupID);
		}

		[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int SteamGroup_GetCount();

		[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern ulong SteamGroup_GetSteamID(int iCount);

		public class Group
		{
			public ulong steamid;

			public Group()
			{
			}
		}
	}
}