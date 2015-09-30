using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShaderMod : ScriptableObject
{
	public ShaderMod.DICT replaceIncludes;

	public ShaderMod.DICT replaceQueues;

	public ShaderMod.DICT macroDefines;

	public string[] preIncludes;

	public string[] postIncludes;

	public ShaderMod.DICT this[ShaderMod.Replacement replacement]
	{
		get
		{
			switch (replacement)
			{
				case ShaderMod.Replacement.Include:
				{
					return this.replaceIncludes;
				}
				case ShaderMod.Replacement.Queue:
				{
					return this.replaceQueues;
				}
				case ShaderMod.Replacement.Define:
				{
					return this.macroDefines;
				}
			}
			return null;
		}
	}

	public ShaderMod()
	{
	}

	public bool Replace(ShaderMod.Replacement replacement, string incoming, ref string outgoing)
	{
		ShaderMod.DICT item = this[replacement];
		return (item == null ? false : item.Replace(replacement, incoming, ref outgoing));
	}

	[Serializable]
	public class DICT
	{
		public ShaderMod.KV[] keyValues;

		public string this[string key]
		{
			get
			{
				ShaderMod.KV[] kVArray = this.keyValues;
				for (int i = 0; i < (int)kVArray.Length; i++)
				{
					ShaderMod.KV kV = kVArray[i];
					if (kV.key == key)
					{
						return kV.@value;
					}
				}
				return null;
			}
			set
			{
				int num = -1;
				while (true)
				{
					int num1 = num + 1;
					num = num1;
					if (num1 >= (int)this.keyValues.Length)
					{
						break;
					}
					if (this.keyValues[num].key == key)
					{
						if (value != null)
						{
							this.keyValues[num].@value = value;
						}
						else
						{
							this.keyValues[num] = this.keyValues[(int)this.keyValues.Length - 1];
							Array.Resize<ShaderMod.KV>(ref this.keyValues, (int)this.keyValues.Length - 1);
						}
					}
				}
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				Array.Resize<ShaderMod.KV>(ref this.keyValues, (int)this.keyValues.Length + 1);
				this.keyValues[(int)this.keyValues.Length - 1] = new ShaderMod.KV(key, value);
			}
		}

		public DICT()
		{
		}

		public bool Replace(ShaderMod.Replacement replacement, string incoming, ref string outgoing)
		{
			if (this.keyValues != null)
			{
				if (replacement == ShaderMod.Replacement.Queue)
				{
					for (int i = 0; i < (int)this.keyValues.Length; i++)
					{
						if (ShaderMod.QueueCompare.Equals(this.keyValues[i].key, incoming))
						{
							outgoing = this.keyValues[i].@value;
							return true;
						}
					}
				}
				else
				{
					for (int j = 0; j < (int)this.keyValues.Length; j++)
					{
						if (string.Equals(this.keyValues[j].key, incoming, StringComparison.InvariantCultureIgnoreCase))
						{
							outgoing = this.keyValues[j].@value;
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	[Serializable]
	public class KV
	{
		public string key;

		public string @value;

		public KV()
		{
			this.key = string.Empty;
			this.@value = string.Empty;
		}

		public KV(string key, string value)
		{
			this.key = key;
			this.@value = value;
		}

		public override int GetHashCode()
		{
			return (this.key != null ? this.key.GetHashCode() : 0);
		}
	}

	public static class QueueCompare
	{
		public const int kBackground = 1000;

		public const int kGeometry = 2000;

		public const int kAlphaTest = 2450;

		public const int kTransparent = 3000;

		public const int kOverlay = 4000;

		public const int kDefault = 2000;

		private readonly static char[] signChars;

		static QueueCompare()
		{
			ShaderMod.QueueCompare.signChars = new char[] { '-', '+' };
		}

		public static bool Equals(string queue1, string queue2)
		{
			return ShaderMod.QueueCompare.ToInt32(queue1) == ShaderMod.QueueCompare.ToInt32(queue2);
		}

		public static int ToInt32(string queue)
		{
			int num;
			int num1;
			if (queue == null || queue.Length == 0)
			{
				return 2000;
			}
			int num2 = queue.IndexOfAny(ShaderMod.QueueCompare.signChars);
			if (num2 == -1)
			{
				num = 0;
			}
			else
			{
				queue = queue.Substring(0, num2);
				num = int.Parse(queue.Substring(num2));
			}
			string str = queue.Trim();
			queue = str;
			string lowerInvariant = str.ToLowerInvariant();
			if (lowerInvariant != null)
			{
				if (ShaderMod.QueueCompare.<>f__switch$map6 == null)
				{
					Dictionary<string, int> strs = new Dictionary<string, int>(5)
					{
						{ "geometry", 0 },
						{ "alphatest", 1 },
						{ "transparent", 2 },
						{ "background", 3 },
						{ "overlay", 4 }
					};
					ShaderMod.QueueCompare.<>f__switch$map6 = strs;
				}
				if (ShaderMod.QueueCompare.<>f__switch$map6.TryGetValue(lowerInvariant, out num1))
				{
					switch (num1)
					{
						case 0:
						{
							return 2000 + num;
						}
						case 1:
						{
							return 2450 + num;
						}
						case 2:
						{
							return 3000 + num;
						}
						case 3:
						{
							return 1000 + num;
						}
						case 4:
						{
							return 4000 + num;
						}
					}
				}
			}
			return (!int.TryParse(queue, out num) ? 2000 : num);
		}
	}

	public enum Replacement
	{
		Include,
		Queue,
		Define
	}
}