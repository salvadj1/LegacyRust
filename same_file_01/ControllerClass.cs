using System;
using System.Reflection;
using UnityEngine;

public class ControllerClass : ScriptableObject
{
	private const ControllerClass.Configuration kDriverMask = ControllerClass.Configuration.DynamicFreeVessel;

	private const ControllerClass.Configuration kStaticMask = ControllerClass.Configuration.StaticRoot;

	private const ControllerClass.Configuration kDriver_Root = ControllerClass.Configuration.DynamicRoot;

	private const ControllerClass.Configuration kDriver_StandaloneVessel = ControllerClass.Configuration.DynamicStandaloneVessel;

	private const ControllerClass.Configuration kDriver_DependantVessel = ControllerClass.Configuration.DynamicDependantVessel;

	private const ControllerClass.Configuration kDriver_FreeVessel = ControllerClass.Configuration.DynamicFreeVessel;

	private const ControllerClass.Configuration kStatic_Static = ControllerClass.Configuration.StaticRoot;

	private const ControllerClass.Configuration kStatic_Dynamic = ControllerClass.Configuration.DynamicRoot;

	[SerializeField]
	private string _npcName = string.Empty;

	[SerializeField]
	private ControllerClassesConfigurations classNames;

	[SerializeField]
	private ControllerClass.Configuration runtime;

	internal string npcName
	{
		get
		{
			return (!string.IsNullOrEmpty(this._npcName) ? this._npcName : base.name);
		}
	}

	internal bool root
	{
		get
		{
			return (this.runtime & ControllerClass.Configuration.DynamicFreeVessel) == ControllerClass.Configuration.DynamicRoot;
		}
	}

	internal bool staticGroup
	{
		get
		{
			return (this.runtime & ControllerClass.Configuration.StaticRoot) == ControllerClass.Configuration.StaticRoot;
		}
	}

	internal string unassignedClassName
	{
		get
		{
			return this.classNames.unassignedClassName;
		}
	}

	internal bool vessel
	{
		get
		{
			return (this.runtime & ControllerClass.Configuration.DynamicFreeVessel) != ControllerClass.Configuration.DynamicRoot;
		}
	}

	internal bool vesselDependant
	{
		get
		{
			return (this.runtime & ControllerClass.Configuration.DynamicFreeVessel) == ControllerClass.Configuration.DynamicDependantVessel;
		}
	}

	internal bool vesselFree
	{
		get
		{
			return (this.runtime & ControllerClass.Configuration.DynamicFreeVessel) == ControllerClass.Configuration.DynamicFreeVessel;
		}
	}

	internal bool vesselStandalone
	{
		get
		{
			return (this.runtime & ControllerClass.Configuration.DynamicFreeVessel) == ControllerClass.Configuration.DynamicStandaloneVessel;
		}
	}

	public ControllerClass()
	{
	}

	internal bool DefinesClass(bool player, bool local)
	{
		return !object.ReferenceEquals(this.GetClassName(player, local), null);
	}

	internal bool DefinesClass(bool player)
	{
		return !object.ReferenceEquals(this.GetClassName(player, false) ?? this.GetClassName(player, true), null);
	}

	internal string GetClassName(bool player, bool local)
	{
		string className;
		if (this.classNames != null)
		{
			className = this.classNames.GetClassName(player, local);
		}
		else
		{
			className = null;
		}
		return className;
	}

	internal bool GetClassName(bool player, bool local, out string className)
	{
		string str = this.GetClassName(player, local);
		string str1 = str;
		className = str;
		return !object.ReferenceEquals(str1, null);
	}

	public enum Configuration
	{
		DynamicRoot,
		DynamicStandaloneVessel,
		DynamicDependantVessel,
		DynamicFreeVessel,
		StaticRoot,
		StaticStandaloneVessel,
		StaticDependantVessel,
		StaticFreeVessel
	}

	public struct Merge
	{
		private int length;

		private int hash;

		private ControllerClass.Merge.Instance first;

		private ControllerClass.Merge.Instance[] classes;

		public bool any
		{
			get
			{
				return this.length > 0;
			}
		}

		public bool this[bool player, bool local]
		{
			get
			{
				if (this.length <= 0)
				{
					return false;
				}
				if (this.length == 1)
				{
					return this.first.@value.DefinesClass(player, local);
				}
				for (int i = 0; i < this.length; i++)
				{
					if (!this.classes[i].@value.DefinesClass(player, local))
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool this[bool player]
		{
			get
			{
				if (this.length <= 0)
				{
					return false;
				}
				if (this.length == 1)
				{
					return this.first.@value.DefinesClass(player);
				}
				for (int i = 0; i < this.length; i++)
				{
					if (!this.classes[i].@value.DefinesClass(player))
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool multiple
		{
			get
			{
				return this.length > 1;
			}
		}

		public bool root
		{
			get
			{
				if (this.length <= 0)
				{
					return false;
				}
				if (this.length == 1)
				{
					return this.first.@value.root;
				}
				for (int i = 0; i < this.length; i++)
				{
					if (!this.classes[i].@value.root)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool staticGroup
		{
			get
			{
				if (this.length <= 0)
				{
					return false;
				}
				if (this.length == 1)
				{
					return this.first.@value.staticGroup;
				}
				for (int i = 0; i < this.length; i++)
				{
					if (!this.classes[i].@value.staticGroup)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool vessel
		{
			get
			{
				if (this.length <= 0)
				{
					return false;
				}
				if (this.length == 1)
				{
					return this.first.@value.vessel;
				}
				for (int i = 0; i < this.length; i++)
				{
					if (!this.classes[i].@value.vessel)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool vesselDependant
		{
			get
			{
				if (this.length <= 0)
				{
					return false;
				}
				if (this.length == 1)
				{
					return this.first.@value.vesselDependant;
				}
				for (int i = 0; i < this.length; i++)
				{
					if (!this.classes[i].@value.vesselDependant)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool vesselFree
		{
			get
			{
				if (this.length <= 0)
				{
					return false;
				}
				if (this.length == 1)
				{
					return this.first.@value.vesselFree;
				}
				for (int i = 0; i < this.length; i++)
				{
					if (!this.classes[i].@value.vesselFree)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool vesselStandalone
		{
			get
			{
				if (this.length <= 0)
				{
					return false;
				}
				if (this.length == 1)
				{
					return this.first.@value.vesselStandalone;
				}
				for (int i = 0; i < this.length; i++)
				{
					if (!this.classes[i].@value.vesselStandalone)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool Add(ControllerClass @class)
		{
			ControllerClass.Merge.Instance hashCode = new ControllerClass.Merge.Instance();
			if (!@class)
			{
				return false;
			}
			hashCode.hash = @class.GetHashCode();
			hashCode.@value = @class;
			if (this.length == 1)
			{
				if (this.hash == hashCode.hash && object.ReferenceEquals(this.first.@value, hashCode.@value))
				{
					return false;
				}
			}
			else if (this.length > 1 && (this.hash & hashCode.hash) == hashCode.hash)
			{
				for (int i = 0; i < this.length; i++)
				{
					if (this.classes[i].hash == this.hash && object.ReferenceEquals(this.classes[i].@value, hashCode.@value))
					{
						return false;
					}
				}
			}
			ControllerClass.Merge merge = this;
			merge.hash = merge.hash | hashCode.hash;
			ControllerClass.Merge merge1 = this;
			int num = merge1.length;
			int num1 = num;
			merge1.length = num + 1;
			int num2 = num1;
			if (num2 == 0)
			{
				this.first = hashCode;
			}
			else if (num2 != 1)
			{
				Array.Resize<ControllerClass.Merge.Instance>(ref this.classes, this.length);
				this.classes[num2] = hashCode;
			}
			else
			{
				this.classes = new ControllerClass.Merge.Instance[] { this.first, hashCode };
				this.first.hash = 0;
				this.first.@value = null;
			}
			return true;
		}

		private struct Instance
		{
			public int hash;

			public ControllerClass @value;
		}
	}
}