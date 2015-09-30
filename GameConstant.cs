using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameConstant
{
	public static GameConstant.Tag GetTag(this GameObject gameObject)
	{
		return (GameConstant.Tag)gameObject;
	}

	public static GameConstant.Tag GetTag(this Component component)
	{
		return (GameConstant.Tag)component;
	}

	public static class Layer
	{
		public const int kMask_BloodSplatter = 525313;

		public const int kMask_BulletImpactWorld = 1840145;

		public const int kMask_BulletImpactCharacter = 402784256;

		public const int kMask_BulletImpact = 406721553;

		public const int kMask_BlocksSprite = 525313;

		public const int kMask_InfoLabel = -67174405;

		public const int kMask_Use = -201523205;

		public const int kMask_SpawnLand = 525313;

		public const int kMask_ClientExplosion = 134217728;

		public const int kMask_ServerExplosion = 271975425;

		public const int kMask_Deployable = -472317957;

		public const int kMask_WildlifeMove = -472317957;

		public const int kMask_PlayerMovement = 538444803;

		public const int kMask_PlayerPusher = 1310720;

		public const int kMask_Melee = 406721553;

		public static class CharacterCollision
		{
			public const string name = "Character Collision";

			public const int index = 16;

			public const int mask = 65536;
		}

		public static class CullStatic
		{
			public const string name = "CullStatic";

			public const int index = 12;

			public const int mask = 4096;
		}

		public static class Debris
		{
			public const string name = "Debris";

			public const int index = 18;

			public const int mask = 262144;
		}

		public static class Default
		{
			public const string name = "Default";

			public const int index = 0;

			public const int mask = 1;
		}

		public static class GameUI
		{
			public const string name = "GameUI";

			public const int index = 31;

			public const int mask = -2147483648;
		}

		public static class Hitbox
		{
			public const string name = "Hitbox";

			public const int index = 17;

			public const int mask = 131072;
		}

		public static class HitOnly
		{
			public const string name = "HitOnly";

			public const int index = 21;

			public const int mask = 2097152;
		}

		public static class IgnoreRaycast
		{
			public const string name = "Ignore Raycast";

			public const int index = 2;

			public const int mask = 4;
		}

		public static class Mechanical
		{
			public const string name = "Mechanical";

			public const int index = 20;

			public const int mask = 1048576;
		}

		public static class MeshBatched
		{
			public const string name = "MeshBatched";

			public const int index = 22;

			public const int mask = 4194304;
		}

		public static class NGUILayer
		{
			public const string name = "NGUILayer";

			public const int index = 8;

			public const int mask = 256;
		}

		public static class NGUILayer2D
		{
			public const string name = "NGUILayer2D";

			public const int index = 9;

			public const int mask = 512;
		}

		public static class PlayerClip
		{
			public const string name = "PlayerClip";

			public const int index = 29;

			public const int mask = 536870912;
		}

		public static class Ragdoll
		{
			public const string name = "Ragdoll";

			public const int index = 27;

			public const int mask = 134217728;
		}

		public static class Skybox
		{
			public const string name = "Skybox";

			public const int index = 23;

			public const int mask = 8388608;
		}

		public static class Sprite
		{
			public const string name = "Sprite";

			public const int index = 11;

			public const int mask = 2048;
		}

		public static class Static
		{
			public const string name = "Static";

			public const int index = 10;

			public const int mask = 1024;
		}

		public static class Terrain
		{
			public const string name = "Terrain";

			public const int index = 19;

			public const int mask = 524288;
		}

		public static class TransparentFX
		{
			public const string name = "TransparentFX";

			public const int index = 1;

			public const int mask = 2;
		}

		public static class Vehicle
		{
			public const string name = "Vehicle";

			public const int index = 28;

			public const int mask = 268435456;
		}

		public static class ViewModel
		{
			public const string name = "View Model";

			public const int index = 13;

			public const int mask = 8192;
		}

		public static class Water
		{
			public const string name = "Water";

			public const int index = 4;

			public const int mask = 16;
		}

		public static class Zone
		{
			public const string name = "Zone";

			public const int index = 26;

			public const int mask = 67108864;
		}
	}

	public struct Tag
	{
		private const int kBuiltinTagCount = 7;

		public const int kTagCount = 23;

		public const int kCustomTagCount = 16;

		public readonly int tagNumber;

		private readonly static GameConstant.TagInfo[] Info;

		private readonly static Dictionary<string, int> Dictionary;

		public bool builtin
		{
			get
			{
				return GameConstant.Tag.Info[this.tagNumber].builtin;
			}
		}

		public string tag
		{
			get
			{
				return GameConstant.Tag.Info[this.tagNumber].tag;
			}
		}

		static Tag()
		{
			int num;
			GameConstant.Tag.Info = new GameConstant.TagInfo[23];
			GameConstant.Tag.Dictionary = new Dictionary<string, int>(23);
			Type[] nestedTypes = typeof(GameConstant.Tag).GetNestedTypes();
			for (int i = 0; i < (int)nestedTypes.Length; i++)
			{
				Type type = nestedTypes[i];
				FieldInfo field = type.GetField("tag", BindingFlags.Static | BindingFlags.Public);
				FieldInfo fieldInfo = type.GetField("tagNumber", BindingFlags.Static | BindingFlags.Public);
				FieldInfo field1 = type.GetField("builtin", BindingFlags.Static | BindingFlags.Public);
				if (field != null && fieldInfo != null && field1 != null)
				{
					try
					{
						int value = (int)fieldInfo.GetValue(null);
						string str = (string)field.GetValue(null);
						bool flag = (bool)field1.GetValue(null);
						GameConstant.Tag.Info[value] = new GameConstant.TagInfo(str, value, flag);
					}
					catch (Exception exception)
					{
						Debug.LogError(exception);
					}
				}
			}
			for (int j = 0; j < 23; j++)
			{
				if (!GameConstant.Tag.Info[j].valid)
				{
					Debug.LogWarning(string.Format("Theres no tag specified for index {0}", j));
				}
				else if (!GameConstant.Tag.Dictionary.TryGetValue(GameConstant.Tag.Info[j].tag, out num))
				{
					GameConstant.Tag.Dictionary.Add(GameConstant.Tag.Info[j].tag, j);
				}
				else
				{
					Debug.LogWarning(string.Format("Duplicate tag at index {0} will be overriden by predicessor at index {1}", j, num));
				}
			}
		}

		private Tag(int tagNumber)
		{
			this.tagNumber = tagNumber;
		}

		public bool Contains(GameObject gameObject)
		{
			return (!gameObject ? false : gameObject.CompareTag(GameConstant.Tag.Info[this.tagNumber].tag));
		}

		public bool Contains(Component component)
		{
			return (!component ? false : component.CompareTag(GameConstant.Tag.Info[this.tagNumber].tag));
		}

		public static int Index(GameObject gameObject)
		{
			for (int i = 0; i < 23; i++)
			{
				if (gameObject.CompareTag(GameConstant.Tag.Info[i].tag))
				{
					return i;
				}
			}
			throw new InvalidProgramException(string.Format("There is a tag missing in this class for \"{0}\"", gameObject.tag));
		}

		public static int Index(Component component)
		{
			GameObject gameObject = component.gameObject;
			for (int i = 0; i < 23; i++)
			{
				if (gameObject.CompareTag(GameConstant.Tag.Info[i].tag))
				{
					return i;
				}
			}
			throw new InvalidProgramException(string.Format("There is a tag missing in this class for \"{0}\"", gameObject.tag));
		}

		public static int Index(string tag)
		{
			int num;
			if (!GameConstant.Tag.Dictionary.TryGetValue(tag, out num))
			{
				throw new InvalidProgramException(string.Format("There is a tag missing in this class for \"{0}\"", tag));
			}
			return num;
		}

		public static explicit operator Tag(GameObject gameObject)
		{
			return new GameConstant.Tag(GameConstant.Tag.Index(gameObject));
		}

		public static explicit operator Tag(Component component)
		{
			return new GameConstant.Tag(GameConstant.Tag.Index(component));
		}

		public static class Barricade
		{
			public const string tag = "Barricade";

			public const int tagNumber = 13;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static Barricade()
			{
				GameConstant.Tag.Barricade.@value = new GameConstant.Tag(13);
			}
		}

		public static class ClientFolder
		{
			public const string tag = "Client Folder";

			public const int tagNumber = 22;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static ClientFolder()
			{
				GameConstant.Tag.ClientFolder.@value = new GameConstant.Tag(22);
			}
		}

		public static class ClientOnly
		{
			public const string tag = "RPOS Camera";

			public const int tagNumber = 19;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static ClientOnly()
			{
				GameConstant.Tag.ClientOnly.@value = new GameConstant.Tag(19);
			}
		}

		public static class Door
		{
			public const string tag = "Door";

			public const int tagNumber = 12;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static Door()
			{
				GameConstant.Tag.Door.@value = new GameConstant.Tag(12);
			}
		}

		public static class EditorOnly
		{
			public const string tag = "EditorOnly";

			public const int tagNumber = 3;

			public const bool builtin = true;

			public readonly static GameConstant.Tag @value;

			static EditorOnly()
			{
				GameConstant.Tag.EditorOnly.@value = new GameConstant.Tag(3);
			}
		}

		public static class Finish
		{
			public const string tag = "Finish";

			public const int tagNumber = 2;

			public const bool builtin = true;

			public readonly static GameConstant.Tag @value;

			static Finish()
			{
				GameConstant.Tag.Finish.@value = new GameConstant.Tag(2);
			}
		}

		public static class Folder
		{
			public const string tag = "Folder";

			public const int tagNumber = 20;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static Folder()
			{
				GameConstant.Tag.Folder.@value = new GameConstant.Tag(20);
			}
		}

		public static class FPGrass
		{
			public const string tag = "FPGrass";

			public const int tagNumber = 17;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static FPGrass()
			{
				GameConstant.Tag.FPGrass.@value = new GameConstant.Tag(17);
			}
		}

		public static class GameController
		{
			public const string tag = "GameController";

			public const int tagNumber = 6;

			public const bool builtin = true;

			public readonly static GameConstant.Tag @value;

			static GameController()
			{
				GameConstant.Tag.GameController.@value = new GameConstant.Tag(6);
			}
		}

		public static class MainCamera
		{
			public const string tag = "MainCamera";

			public const int tagNumber = 4;

			public const bool builtin = true;

			public readonly static GameConstant.Tag @value;

			static MainCamera()
			{
				GameConstant.Tag.MainCamera.@value = new GameConstant.Tag(4);
			}
		}

		public static class MainTerrain
		{
			public const string tag = "Main Terrain";

			public const int tagNumber = 8;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static MainTerrain()
			{
				GameConstant.Tag.MainTerrain.@value = new GameConstant.Tag(8);
			}
		}

		public static class Meat
		{
			public const string tag = "Meat";

			public const int tagNumber = 10;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static Meat()
			{
				GameConstant.Tag.Meat.@value = new GameConstant.Tag(10);
			}
		}

		public static class MeshBatched
		{
			public const string tag = "mBC";

			public const int tagNumber = 15;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static MeshBatched()
			{
				GameConstant.Tag.MeshBatched.@value = new GameConstant.Tag(15);
			}
		}

		public static class Player
		{
			public const string tag = "Player";

			public const int tagNumber = 5;

			public const bool builtin = true;

			public readonly static GameConstant.Tag @value;

			static Player()
			{
				GameConstant.Tag.Player.@value = new GameConstant.Tag(5);
			}
		}

		public static class Respawn
		{
			public const string tag = "Respawn";

			public const int tagNumber = 1;

			public const bool builtin = true;

			public readonly static GameConstant.Tag @value;

			static Respawn()
			{
				GameConstant.Tag.Respawn.@value = new GameConstant.Tag(1);
			}
		}

		public static class RPOSCamera
		{
			public const string tag = "RPOS Camera";

			public const int tagNumber = 16;

			public const bool builtin = false;
		}

		public static class ServerFolder
		{
			public const string tag = "Server Folder";

			public const int tagNumber = 21;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static ServerFolder()
			{
				GameConstant.Tag.ServerFolder.@value = new GameConstant.Tag(21);
			}
		}

		public static class ServerOnly
		{
			public const string tag = "Server Only";

			public const int tagNumber = 18;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static ServerOnly()
			{
				GameConstant.Tag.ServerOnly.@value = new GameConstant.Tag(18);
			}
		}

		public static class Shelter
		{
			public const string tag = "Shelter";

			public const int tagNumber = 11;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static Shelter()
			{
				GameConstant.Tag.Shelter.@value = new GameConstant.Tag(11);
			}
		}

		public static class SkyboxCamera
		{
			public const string tag = "Skybox Camera";

			public const int tagNumber = 7;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static SkyboxCamera()
			{
				GameConstant.Tag.SkyboxCamera.@value = new GameConstant.Tag(7);
			}
		}

		public static class StorageBox
		{
			public const string tag = "StorageBox";

			public const int tagNumber = 14;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static StorageBox()
			{
				GameConstant.Tag.StorageBox.@value = new GameConstant.Tag(14);
			}
		}

		public static class TreeCollider
		{
			public const string tag = "Tree Collider";

			public const int tagNumber = 9;

			public const bool builtin = false;

			public readonly static GameConstant.Tag @value;

			static TreeCollider()
			{
				GameConstant.Tag.TreeCollider.@value = new GameConstant.Tag(9);
			}
		}

		public static class Untagged
		{
			public const string tag = "Untagged";

			public const int tagNumber = 0;

			public const bool builtin = true;

			public readonly static GameConstant.Tag @value;

			static Untagged()
			{
				GameConstant.Tag.Untagged.@value = new GameConstant.Tag(0);
			}
		}
	}

	private struct TagInfo
	{
		public readonly string tag;

		public readonly int tagNumber;

		public readonly bool builtin;

		public readonly bool valid;

		public TagInfo(string tag, int tagNumber, bool builtin)
		{
			this.tag = tag;
			this.tagNumber = tagNumber;
			this.builtin = builtin;
			this.valid = true;
		}
	}
}