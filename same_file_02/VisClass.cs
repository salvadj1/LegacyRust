using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class VisClass : ScriptableObject
{
	[SerializeField]
	private VisClass _super;

	[SerializeField]
	private string[] keys;

	[SerializeField]
	private VisQuery[] values;

	[NonSerialized]
	private VisQuery[] instance;

	[NonSerialized]
	private Dictionary<string, int> members;

	[NonSerialized]
	private bool locked;

	[NonSerialized]
	private bool recurseLock;

	private readonly static VisQuery.Instance[] none;

	public VisClass.Handle handle
	{
		get
		{
			if (!this.locked)
			{
				this.Setup();
				if (!this.locked)
				{
					return new VisClass.Handle(null);
				}
			}
			return new VisClass.Handle(this);
		}
	}

	public VisClass superClass
	{
		get
		{
			return this._super;
		}
	}

	static VisClass()
	{
		VisClass.none = new VisQuery.Instance[0];
	}

	public VisClass()
	{
	}

	private void BuildMembers(List<VisQuery> list, HashSet<VisQuery> hset)
	{
		if (this._super)
		{
			if (this._super.recurseLock)
			{
				Debug.LogError("Recursion in setup hit itself, some VisClass has super set to something which references itself", this._super);
				return;
			}
			this._super.recurseLock = true;
			this._super.BuildMembers(list, hset);
			this._super.recurseLock = false;
		}
		if (this.values != null)
		{
			for (int i = 0; i < (int)this.values.Length; i++)
			{
				if (this.values[i] != null && hset.Remove(this.values[i]))
				{
					list.Add(this.values[i]);
				}
			}
		}
	}

	public void EditorOnly_Add(ref VisClass.Rep rep, string key, VisQuery value)
	{
		Array.Resize<string>(ref this.keys, (int)this.keys.Length + 1);
		Array.Resize<VisQuery>(ref this.values, (int)this.values.Length + 1);
		this.keys[(int)this.keys.Length - 1] = key;
		this.values[(int)this.values.Length - 1] = value;
		rep = null;
	}

	public bool EditorOnly_Apply(ref VisClass.Rep rep)
	{
		return (rep == null ? false : rep.Apply());
	}

	public void EditorOnly_Rep(ref VisClass.Rep rep)
	{
		if (this.keys == null && this.values == null)
		{
			this.keys = new string[0];
			this.values = new VisQuery[0];
		}
		VisClass.Rep.Ref(ref rep, this);
	}

	public bool EditorOnly_SetSuper(ref VisClass.Rep rep, VisClass _super)
	{
		VisClass visClass = _super;
		int num = 50;
		while (visClass != null)
		{
			if (visClass == this)
			{
				Debug.LogError("Self Reference Detected", this);
				return false;
			}
			visClass = visClass._super;
			int num1 = num - 1;
			num = num1;
			if (num1 > 0)
			{
				continue;
			}
			Debug.LogError("Circular Dependancy Detected", this);
			return false;
		}
		rep = null;
		this._super = _super;
		return true;
	}

	private void Setup()
	{
		int num;
		if (this.locked)
		{
			return;
		}
		if (this.recurseLock)
		{
			Debug.LogError("Recursion in setup hit itself, some VisClass has super set to something which references itself", this);
			return;
		}
		this.recurseLock = true;
		List<VisQuery> visQueries = new List<VisQuery>();
		HashSet<VisQuery> visQueries1 = new HashSet<VisQuery>();
		Dictionary<string, VisQuery> strs = new Dictionary<string, VisQuery>();
		if (!this._super)
		{
			for (int i = 0; i < (int)this.keys.Length; i++)
			{
				string str = this.keys[i];
				if (!string.IsNullOrEmpty(str))
				{
					VisQuery visQuery = this.values[i];
					if (visQuery != null)
					{
						strs.Add(str, visQuery);
						if (visQueries1.Add(visQuery))
						{
							visQueries.Add(visQuery);
						}
					}
				}
			}
		}
		else
		{
			this._super.Setup();
			if (this.keys != null)
			{
				for (int j = 0; j < (int)this.keys.Length; j++)
				{
					string str1 = this.keys[j];
					if (!string.IsNullOrEmpty(str1))
					{
						VisQuery visQuery1 = this.values[j];
						if (this._super.members.TryGetValue(str1, out num))
						{
							VisQuery visQuery2 = this._super.instance[num];
							if (visQuery2 == visQuery1)
							{
								if (visQuery2 != null)
								{
									visQueries1.Add(visQuery2);
									strs.Add(str1, visQuery2);
								}
							}
							else if (visQuery1 != null)
							{
								strs.Add(str1, visQuery1);
								visQueries1.Add(visQuery1);
							}
						}
						else if (visQuery1 != null)
						{
							strs.Add(str1, visQuery1);
							visQueries1.Add(visQuery1);
						}
					}
				}
			}
			this.BuildMembers(visQueries, visQueries1);
		}
		this.members = new Dictionary<string, int>(strs.Count);
		foreach (KeyValuePair<string, VisQuery> keyValuePair in strs)
		{
			this.members.Add(keyValuePair.Key, visQueries.IndexOf(keyValuePair.Value));
		}
		this.instance = visQueries.ToArray();
		this.recurseLock = false;
		this.locked = true;
	}

	public struct Handle
	{
		private readonly VisClass klass;

		private readonly VisQuery.Instance[] queries;

		private long bits;

		public VisQuery.Instance this[int i]
		{
			get
			{
				return this.queries[i];
			}
		}

		public VisQuery.Instance this[string name]
		{
			get
			{
				return this.queries[this.klass.members[name]];
			}
		}

		public int Length
		{
			get
			{
				return (int)this.klass.instance.Length;
			}
		}

		public bool valid
		{
			get
			{
				return this.queries != null;
			}
		}

		internal Handle(VisClass klass)
		{
			this.klass = klass;
			this.bits = (long)0;
			if (!klass)
			{
				this.queries = VisClass.none;
			}
			else
			{
				int num = 0;
				this.queries = new VisQuery.Instance[(int)klass.instance.Length];
				for (int i = 0; i < (int)this.queries.Length; i++)
				{
					this.queries[i] = new VisQuery.Instance(klass.instance[i], ref num);
				}
			}
		}
	}

	public class Rep
	{
		internal static VisClass nklass;

		internal VisClass klass;

		private static bool building;

		private HashSet<VisClass.Rep.Setting> modifiedSettings;

		public Dictionary<string, VisClass.Rep.Setting> dict;

		static Rep()
		{
		}

		public Rep()
		{
		}

		internal bool Apply()
		{
			if (this.modifiedSettings.Count == 0)
			{
				return false;
			}
			foreach (VisClass.Rep.Setting modifiedSetting in this.modifiedSettings)
			{
				VisClass.Rep.Action action = modifiedSetting.action;
				if (action == VisClass.Rep.Action.Revert)
				{
					this.Remove(modifiedSetting);
				}
				else if (action == VisClass.Rep.Action.Value)
				{
					if (!(modifiedSetting.valueSet == null) || modifiedSetting.isOverride)
					{
						this.Change(modifiedSetting);
					}
					else
					{
						this.Remove(modifiedSetting);
					}
				}
				modifiedSetting.action = VisClass.Rep.Action.None;
			}
			return true;
		}

		private void Change(VisClass.Rep.Setting setting)
		{
			if (!setting.isInherited)
			{
				int num = 0;
				while (num < (int)this.klass.keys.Length)
				{
					if (this.klass.keys[num] != setting.name)
					{
						num++;
					}
					else
					{
						this.klass.values[num] = setting.query;
						break;
					}
				}
			}
			else
			{
				VisQuery visQuery = setting.valueSet;
				Dictionary<string, VisClass.Rep.Setting> strs = this.dict;
				string str = setting.name;
				VisClass.Rep.Setting setting1 = setting.Override(this.klass);
				setting = setting1;
				strs[str] = setting1;
				setting.isInherited = false;
				setting.valueSet = visQuery;
				Array.Resize<string>(ref this.klass.keys, (int)this.klass.keys.Length + 1);
				Array.Resize<VisQuery>(ref this.klass.values, (int)this.klass.values.Length + 1);
				this.klass.keys[(int)this.klass.keys.Length - 1] = setting.name;
				this.klass.values[(int)this.klass.values.Length - 1] = visQuery;
			}
		}

		private static bool MarkModified(VisClass.Rep.Setting setting)
		{
			if (VisClass.Rep.building)
			{
				return false;
			}
			setting.rep.modifiedSettings.Add(setting);
			return true;
		}

		internal static void Recur(ref VisClass.Rep rep, VisClass klass)
		{
			VisClass.Rep.Setting setting;
			if (!klass._super)
			{
				rep = new VisClass.Rep()
				{
					klass = VisClass.Rep.nklass,
					dict = new Dictionary<string, VisClass.Rep.Setting>()
				};
				for (int i = 0; i < (int)klass.keys.Length; i++)
				{
					string str = klass.keys[i];
					if (!string.IsNullOrEmpty(str))
					{
						VisQuery visQuery = klass.values[i];
						if (visQuery != null)
						{
							VisClass.Rep.Setting setting1 = new VisClass.Rep.Setting(str, klass, rep)
							{
								query = visQuery
							};
							rep.dict.Add(str, setting1);
						}
					}
				}
			}
			else
			{
				VisClass.Rep.Recur(ref rep, klass._super);
				foreach (VisClass.Rep.Setting value in rep.dict.Values)
				{
					value.isInherited = true;
				}
				for (int j = 0; j < (int)klass.keys.Length; j++)
				{
					string str1 = klass.keys[j];
					if (!string.IsNullOrEmpty(str1))
					{
						VisQuery visQuery1 = klass.values[j];
						if (rep.dict.TryGetValue(str1, out setting))
						{
							Dictionary<string, VisClass.Rep.Setting> strs = rep.dict;
							VisClass.Rep.Setting setting2 = setting.Override(klass);
							setting = setting2;
							strs[str1] = setting2;
						}
						else
						{
							if (visQuery1 == null)
							{
								goto Label0;
							}
							setting = new VisClass.Rep.Setting(str1, klass, rep);
							rep.dict.Add(str1, setting);
						}
						setting.isInherited = false;
						setting.query = visQuery1;
					}
				Label0:
				}
			}
		}

		internal static void Ref(ref VisClass.Rep rep, VisClass klass)
		{
			if (rep == null)
			{
				VisClass.Rep.nklass = klass;
				VisClass.Rep.building = true;
				VisClass.Rep.Recur(ref rep, klass);
				VisClass.Rep.building = false;
				VisClass.Rep.nklass = null;
			}
		}

		private void Remove(VisClass.Rep.Setting setting)
		{
			int num = 0;
			while (num < (int)this.klass.keys.Length)
			{
				if (this.klass.keys[num] != setting.name)
				{
					num++;
				}
				else
				{
					int num1 = num;
					while (true)
					{
						int num2 = num1 + 1;
						num1 = num2;
						if (num2 >= (int)this.klass.keys.Length)
						{
							break;
						}
						this.klass.keys[num1 - 1] = this.klass.keys[num1];
						this.klass.values[num1 - 1] = this.klass.values[num1];
					}
					Array.Resize<string>(ref this.klass.keys, (int)this.klass.keys.Length - 1);
					Array.Resize<VisQuery>(ref this.klass.values, (int)this.klass.values.Length - 1);
					break;
				}
			}
			if (!setting.isOverride)
			{
				this.dict.Remove(setting.name);
			}
			else
			{
				this.dict[setting.name] = setting.MoveBack();
			}
		}

		internal enum Action
		{
			None,
			Revert,
			Value
		}

		public class Setting
		{
			internal VisClass.Rep rep;

			internal VisClass.Rep.Action action;

			private bool _unchanged;

			private bool _isInherited;

			private bool _hasSuper;

			private VisQuery _value;

			private VisQuery _valueSet;

			private VisClass _inheritedClass;

			private VisClass.Rep.Setting _inheritSetting;

			private string key;

			private VisClass inheritedClass
			{
				get
				{
					return this._inheritedClass;
				}
			}

			public bool isInherited
			{
				get
				{
					return this._isInherited;
				}
				set
				{
					if (this._isInherited != value)
					{
						this._isInherited = value;
						if (VisClass.Rep.MarkModified(this))
						{
							this.action = VisClass.Rep.Action.Revert;
						}
					}
				}
			}

			public bool isOverride
			{
				get
				{
					return this._hasSuper;
				}
			}

			internal string name
			{
				get
				{
					return this.key;
				}
			}

			public VisQuery query
			{
				get
				{
					return this._value;
				}
				set
				{
					if (this._isInherited)
					{
						VisClass.Rep.MarkModified(this);
					}
					else if (this._value == value)
					{
						return;
					}
					if (!VisClass.Rep.MarkModified(this))
					{
						this._value = value;
					}
					else
					{
						this.action = VisClass.Rep.Action.Value;
						this._valueSet = value;
					}
				}
			}

			public VisQuery superQuery
			{
				get
				{
					VisQuery visQuery;
					if (!this._hasSuper)
					{
						visQuery = null;
					}
					else
					{
						visQuery = this._inheritSetting.query;
					}
					return visQuery;
				}
			}

			internal VisQuery valueSet
			{
				get
				{
					return this._valueSet;
				}
				set
				{
					this._value = value;
				}
			}

			internal Setting(string key, VisClass klass, VisClass.Rep rep)
			{
				this.key = key;
				this.rep = rep;
				this._inheritedClass = klass;
			}

			internal VisClass.Rep.Setting MoveBack()
			{
				return this._inheritSetting;
			}

			internal VisClass.Rep.Setting Override(VisClass klass)
			{
				VisClass.Rep.Setting setting = (VisClass.Rep.Setting)this.MemberwiseClone();
				setting._inheritedClass = klass;
				setting._hasSuper = true;
				setting._inheritSetting = this;
				return setting;
			}
		}
	}
}