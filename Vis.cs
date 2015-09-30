using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Vis
{
	public const Vis.Trait kTraitBegin = Vis.Trait.Alive;

	public const Vis.Trait kTraitEnd = 32;

	public const Vis.Trait kLifeFirst = Vis.Trait.Alive;

	public const Vis.Trait kLifeLast = Vis.Trait.Dead;

	public const Vis.Trait kStatusFirst = Vis.Trait.Casual;

	public const Vis.Trait kStatusLast = Vis.Trait.Attacking;

	public const Vis.Trait kRoleFirst = Vis.Trait.Citizen;

	public const Vis.Trait kRoleLast = Vis.Trait.Animal;

	public const Vis.Trait kLifeBegin = Vis.Trait.Alive;

	public const Vis.Trait kLifeEnd = Vis.Trait.Unconcious | Vis.Trait.Dead;

	public const Vis.Trait kStatusBegin = Vis.Trait.Casual;

	public const Vis.Trait kStatusEnd = Vis.Trait.Unconcious | Vis.Trait.Dead | Vis.Trait.Casual | Vis.Trait.Hurt | Vis.Trait.Curious | Vis.Trait.Alert | Vis.Trait.Search | Vis.Trait.Armed | Vis.Trait.Attacking;

	public const Vis.Trait kRoleBegin = Vis.Trait.Citizen;

	public const Vis.Trait kRoleEnd = 32;

	public const int kLifeCount = 3;

	public const int kStatusCount = 7;

	public const int kRoleCount = 8;

	private const uint one = 1;

	public const int kLifeMask = 7;

	public const int kStatusMask = 32512;

	public const int kRoleMask = -16777216;

	private const int OpZero = 3;

	private const int mask24b = 16777215;

	private const int mask31b = 2147483647;

	private const int mask24o7b = 16777216;

	private const int mask31o1b = -2147483648;

	private const byte mask7b = 127;

	private const byte mask7o1b = 128;

	public const Vis.Life kLifeNone = 0;

	public const Vis.Life kLifeAll = Vis.Life.Alive | Vis.Life.Unconcious | Vis.Life.Dead;

	public const Vis.Status kStatusNone = 0;

	public const Vis.Status kStatusAll = Vis.Status.Casual | Vis.Status.Hurt | Vis.Status.Curious | Vis.Status.Alert | Vis.Status.Search | Vis.Status.Armed | Vis.Status.Attacking;

	public const Vis.Role kRoleNone = 0;

	public const Vis.Role kRoleAll = Vis.Role.Citizen | Vis.Role.Criminal | Vis.Role.Authority | Vis.Role.Target | Vis.Role.Entourage | Vis.Role.Player | Vis.Role.Vehicle | Vis.Role.Animal;

	public const int kFlagRelative = 1;

	public const int kFlagTarget = 4;

	public const int kFlagSelf = 8;

	public const int kComparisonStealthy = 5;

	public const int kComparisonPrey = 9;

	public const int kComparisonIsSelf = 12;

	public const int kComparisonOblivious = 1;

	public const int kComparisonContact = 13;

	public static int CountSeen(this Vis.Comparison comparison)
	{
		int num = 0;
		int num1 = (int)comparison;
		if ((num1 & 1) == 1)
		{
			if ((num1 & 4) == 4)
			{
				num1++;
			}
			if ((num1 & 8) == 8)
			{
				num1++;
			}
		}
		return num;
	}

	public static bool DoesSeeTarget(this Vis.Comparison comparison)
	{
		return ((int)comparison & 4) == 4;
	}

	internal static bool Evaluate(Vis.Op op, int f, int m)
	{
		switch (op)
		{
			case Vis.Op.Always:
			{
				return true;
			}
			case Vis.Op.Equals:
			{
				return m == f;
			}
			case Vis.Op.All:
			{
				return (m & f) == f;
			}
			case Vis.Op.Any:
			{
				return (m & f) != 0;
			}
			case Vis.Op.None:
			{
				return (m & f) == 0;
			}
			case Vis.Op.NotEquals:
			{
				return m != f;
			}
			case Vis.Op.Never:
			{
				return false;
			}
			default:
			{
				return false;
			}
		}
	}

	public static VisNode.Search.Radial.Enumerator GetNodesInRadius(Vector3 point, float radius)
	{
		return new VisNode.Search.Radial.Enumerator(new VisNode.Search.PointRadiusData(point, radius));
	}

	public static VisNode.Search.Point.Visual.Enumerator GetNodesWhoCanSee(Vector3 point)
	{
		return new VisNode.Search.Point.Visual.Enumerator(new VisNode.Search.PointVisibilityData(point));
	}

	public static int GetStealth(this Vis.Comparison comparison)
	{
		int num = (int)(comparison & Vis.Comparison.IsSelf);
		if (num == 4)
		{
			return 1;
		}
		if (num == 8)
		{
			return -1;
		}
		return 0;
	}

	public static bool IsOneWay(this Vis.Comparison comparison)
	{
		return (comparison & Vis.Comparison.IsSelf) != Vis.Comparison.IsSelf;
	}

	public static bool IsSeenByTarget(this Vis.Comparison comparison)
	{
		return ((int)comparison & 8) == 8;
	}

	public static bool IsTwoWay(this Vis.Comparison comparison)
	{
		return (comparison & Vis.Comparison.Contact) != Vis.Comparison.IsSelf;
	}

	public static bool IsZeroWay(this Vis.Comparison comparison)
	{
		return (comparison & Vis.Comparison.Contact) == Vis.Comparison.Oblivious;
	}

	public static Vis.Op Negate(Vis.Op op)
	{
		return (int)Vis.Op.Any - ((int)op - (int)Vis.Op.Any);
	}

	public static Vis.Op<TFlags> Negate<TFlags>(Vis.Op<TFlags> op)
	where TFlags : struct, IConvertible, IFormattable, IComparable
	{
		op.op = Vis.Negate(op.op);
		return op;
	}

	public static void RadialMessage(Vector3 point, float radius, string message, object arg)
	{
		VisNode.Search.Radial.Enumerator nodesInRadius = Vis.GetNodesInRadius(point, radius);
		while (nodesInRadius.MoveNext())
		{
			nodesInRadius.Current.SendMessage(message, arg, SendMessageOptions.DontRequireReceiver);
		}
	}

	public static void RadialMessage(Vector3 point, float radius, string message)
	{
		VisNode.Search.Radial.Enumerator nodesInRadius = Vis.GetNodesInRadius(point, radius);
		while (nodesInRadius.MoveNext())
		{
			nodesInRadius.Current.SendMessage(message, SendMessageOptions.DontRequireReceiver);
		}
	}

	private static int SetTrue(Vis.Op op, int f, ref BitVector32 bits, BitVector32.Section sect)
	{
		int item = bits[sect];
		int num = Vis.SetTrue(op, f, item);
		int num1 = num;
		if (item != num)
		{
			bits[sect] = item;
		}
		return num1;
	}

	private static int SetTrue(Vis.Op op, int f, int m)
	{
		switch (op)
		{
			case Vis.Op.Always:
			case Vis.Op.Never:
			{
				return m;
			}
			case Vis.Op.Equals:
			{
				return f;
			}
			case Vis.Op.All:
			{
				return m | f;
			}
			case Vis.Op.Any:
			{
				if ((m & f) != 0)
				{
					return m;
				}
				return m | f;
			}
			case Vis.Op.None:
			{
				return m & ~f;
			}
			case Vis.Op.NotEquals:
			{
				return ~f;
			}
			default:
			{
				return m;
			}
		}
	}

	public static void VisibleMessage(Vector3 point, string message, object arg)
	{
		VisNode.Search.Point.Visual.Enumerator nodesWhoCanSee = Vis.GetNodesWhoCanSee(point);
		while (nodesWhoCanSee.MoveNext())
		{
			nodesWhoCanSee.Current.SendMessage(message, arg, SendMessageOptions.DontRequireReceiver);
		}
	}

	public static void VisibleMessage(Vector3 point, string message)
	{
		VisNode.Search.Point.Visual.Enumerator nodesWhoCanSee = Vis.GetNodesWhoCanSee(point);
		while (nodesWhoCanSee.MoveNext())
		{
			nodesWhoCanSee.Current.SendMessage(message, SendMessageOptions.DontRequireReceiver);
		}
	}

	public enum Comparison
	{
		Oblivious = 1,
		Stealthy = 5,
		Prey = 9,
		IsSelf = 12,
		Contact = 13
	}

	private static class EnumUtil<TEnum>
	where TEnum : struct, IConvertible, IFormattable, IComparable
	{
		public static int ToInt(TEnum val)
		{
			return Convert.ToInt32(val);
		}
	}

	[Flags]
	public enum Flag
	{
		Zero = 0,
		Relative = 1,
		Target = 4,
		Self = 8
	}

	[Flags]
	public enum Life
	{
		Alive = 1,
		Unconcious = 2,
		Dead = 4
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Mask
	{
		[FieldOffset(-1)]
		public const int kAlive = 1;

		[FieldOffset(-1)]
		public const int kUnconcious = 2;

		[FieldOffset(-1)]
		public const int kDead = 4;

		[FieldOffset(-1)]
		public const int kCasual = 256;

		[FieldOffset(-1)]
		public const int kHurt = 512;

		[FieldOffset(-1)]
		public const int kCurious = 1024;

		[FieldOffset(-1)]
		public const int kAlert = 2048;

		[FieldOffset(-1)]
		public const int kSearch = 4096;

		[FieldOffset(-1)]
		public const int kArmed = 8192;

		[FieldOffset(-1)]
		public const int kAttacking = 16384;

		[FieldOffset(-1)]
		public const int kCriminal = 33554432;

		[FieldOffset(-1)]
		public const int kAuthority = 67108864;

		[FieldOffset(-1)]
		private static BitVector32.Section s_life;

		[FieldOffset(-1)]
		private static BitVector32.Section s_stat;

		[FieldOffset(-1)]
		private static BitVector32.Section s_role;

		[FieldOffset(0)]
		public BitVector32 bits;

		[FieldOffset(0)]
		public int data;

		[FieldOffset(0)]
		public uint udata;

		[FieldOffset(-1)]
		public readonly static Vis.Mask zero;

		public bool this[Vis.Life mask]
		{
			get
			{
				return (int)(this.life & mask) != 0;
			}
			set
			{
				if (!value)
				{
					Vis.Mask mask1 = this;
					mask1.life = mask1.life & ~mask;
				}
				else
				{
					Vis.Mask mask2 = this;
					mask2.life = mask2.life | mask;
				}
			}
		}

		public bool this[Vis.Status mask]
		{
			get
			{
				return (int)(this.stat & mask) != 0;
			}
			set
			{
				if (!value)
				{
					Vis.Mask mask1 = this;
					mask1.stat = mask1.stat & ~mask;
				}
				else
				{
					Vis.Mask mask2 = this;
					mask2.stat = mask2.stat | mask;
				}
			}
		}

		public bool this[Vis.Role mask]
		{
			get
			{
				return (int)(this.role & mask) != 0;
			}
			set
			{
				if (!value)
				{
					Vis.Mask mask1 = this;
					mask1.role = mask1.role & ~mask;
				}
				else
				{
					Vis.Mask mask2 = this;
					mask2.role = mask2.role | mask;
				}
			}
		}

		public bool this[Vis.Op op, Vis.Life val]
		{
			get
			{
				return this.Eval(op, val);
			}
			set
			{
				this.Apply(op, val);
			}
		}

		public bool this[Vis.Op op, Vis.Status val]
		{
			get
			{
				return this.Eval(op, val);
			}
			set
			{
				this.Apply(op, val);
			}
		}

		public bool this[Vis.Op op, Vis.Role val]
		{
			get
			{
				return this.Eval(op, val);
			}
			set
			{
				this.Apply(op, val);
			}
		}

		public Vis.Op<Vis.Life>.Res this[Vis.Op<Vis.Life> op]
		{
			get
			{
				return op.Eval(this.bits[Vis.Mask.s_life]);
			}
		}

		public Vis.Op<Vis.Status>.Res this[Vis.Op<Vis.Status> op]
		{
			get
			{
				return op.Eval(this.bits[Vis.Mask.s_stat]);
			}
		}

		public Vis.Op<Vis.Role>.Res this[Vis.Op<Vis.Role> op]
		{
			get
			{
				return op.Eval(this.bits[Vis.Mask.s_role]);
			}
		}

		public bool this[Vis.Trait trait]
		{
			get
			{
				return this.bits[(int)Vis.Trait.Unconcious << (int)(trait & Vis.Trait.Animal)];
			}
		}

		public bool this[int mask]
		{
			get
			{
				return this.bits[mask];
			}
		}

		public Vis.Life life
		{
			get
			{
				return (Vis.Life)this.bits[Vis.Mask.s_life];
			}
			set
			{
				this.bits[Vis.Mask.s_life] = (short)value & Vis.Mask.s_life.Mask;
			}
		}

		public Vis.Role role
		{
			get
			{
				return (Vis.Role)this.bits[Vis.Mask.s_role];
			}
			set
			{
				this.bits[Vis.Mask.s_role] = (short)value & Vis.Mask.s_role.Mask;
			}
		}

		public Vis.Status stat
		{
			get
			{
				return (Vis.Status)this.bits[Vis.Mask.s_stat];
			}
			set
			{
				this.bits[Vis.Mask.s_stat] = (short)value & Vis.Mask.s_stat.Mask;
			}
		}

		static Mask()
		{
			Vis.Mask.zero = new Vis.Mask();
			int num = 0;
			Vis.Mask.sect(0, ref num);
			BitVector32.Section? nullable = new BitVector32.Section?(Vis.Mask.sect(3, ref num));
			Vis.Mask.sect(5, ref num);
			BitVector32.Section? nullable1 = new BitVector32.Section?(Vis.Mask.sect(7, ref num));
			Vis.Mask.sect(9, ref num);
			BitVector32.Section? nullable2 = new BitVector32.Section?(Vis.Mask.sect(8, ref num));
			Vis.Mask.s_life = nullable.GetValueOrDefault();
			Vis.Mask.s_stat = nullable1.GetValueOrDefault();
			Vis.Mask.s_role = nullable2.GetValueOrDefault();
		}

		public bool All(Vis.Life f)
		{
			return (this.life & f) == f;
		}

		public bool All(Vis.Role f)
		{
			return (this.role & f) == f;
		}

		public bool All(Vis.Status f)
		{
			return (this.stat & f) == f;
		}

		public bool AllMore(Vis.Life f)
		{
			Vis.Life life = this.life;
			return (life <= f ? false : (life & f) == f);
		}

		public bool AllMore(Vis.Role f)
		{
			Vis.Role role = this.role;
			return (role <= f ? false : (role & f) == f);
		}

		public bool AllMore(Vis.Status f)
		{
			Vis.Status statu = this.stat;
			return (statu <= f ? false : (statu & f) == f);
		}

		public bool Any(Vis.Life f)
		{
			return (int)(this.life & f) > 0;
		}

		public bool Any(Vis.Role f)
		{
			return (int)(this.role & f) > 0;
		}

		public bool Any(Vis.Status f)
		{
			return (int)(this.stat & f) > 0;
		}

		public bool AnyLess(Vis.Life f)
		{
			return (this.life & f) < f;
		}

		public bool AnyLess(Vis.Role f)
		{
			return (this.role & f) < f;
		}

		public bool AnyLess(Vis.Status f)
		{
			return (this.stat & f) < f;
		}

		public void Append(Vis.Life f)
		{
			Vis.Mask mask = this;
			mask.life = mask.life | f;
		}

		public void Append(Vis.Role f)
		{
			Vis.Mask mask = this;
			mask.role = mask.role | f;
		}

		public void Append(Vis.Status f)
		{
			Vis.Mask mask = this;
			mask.stat = mask.stat | f;
		}

		public Vis.Life AppendNot(Vis.Life f)
		{
			Vis.Life life = (this.life ^ f) & f;
			Vis.Mask mask = this;
			mask.life = mask.life | life;
			return life;
		}

		public Vis.Role AppendNot(Vis.Role f)
		{
			Vis.Role role = (this.role ^ f) & f;
			Vis.Mask mask = this;
			mask.role = mask.role | role;
			return role;
		}

		public Vis.Status AppendNot(Vis.Status f)
		{
			Vis.Status statu = (this.stat ^ f) & f;
			Vis.Mask mask = this;
			mask.stat = mask.stat | statu;
			return statu;
		}

		public Vis.Life Apply(Vis.Op op, Vis.Life f)
		{
			return (Vis.Life)Vis.SetTrue(op, (int)f, ref this.bits, Vis.Mask.s_life);
		}

		public Vis.Life Apply(Vis.Op<Vis.Life> op)
		{
			return (Vis.Life)Vis.SetTrue((Vis.Op)op, op.intvalue, ref this.bits, Vis.Mask.s_life);
		}

		public Vis.Status Apply(Vis.Op op, Vis.Status f)
		{
			return (Vis.Status)Vis.SetTrue(op, (int)f, ref this.bits, Vis.Mask.s_stat);
		}

		public Vis.Status Apply(Vis.Op<Vis.Status> op)
		{
			return (Vis.Status)Vis.SetTrue((Vis.Op)op, op.intvalue, ref this.bits, Vis.Mask.s_stat);
		}

		public Vis.Role Apply(Vis.Op op, Vis.Role f)
		{
			return (Vis.Role)Vis.SetTrue(op, (int)f, ref this.bits, Vis.Mask.s_role);
		}

		public Vis.Role Apply(Vis.Op<Vis.Role> op)
		{
			return (Vis.Role)Vis.SetTrue((Vis.Op)op, op.intvalue, ref this.bits, Vis.Mask.s_role);
		}

		public bool Equals(Vis.Life f)
		{
			return this.life == f;
		}

		public bool Equals(Vis.Role f)
		{
			return this.role == f;
		}

		public bool Equals(Vis.Status f)
		{
			return this.stat == f;
		}

		public bool Eval(Vis.Op op, Vis.Life f)
		{
			return Vis.Evaluate(op, (int)f, this.bits[Vis.Mask.s_life]);
		}

		public bool Eval(Vis.Op<Vis.Life> op)
		{
			return op == this.life;
		}

		public bool Eval(Vis.Op op, Vis.Status f)
		{
			return Vis.Evaluate(op, (int)f, this.bits[Vis.Mask.s_stat]);
		}

		public bool Eval(Vis.Op<Vis.Status> op)
		{
			return op == this.stat;
		}

		public bool Eval(Vis.Op op, Vis.Role f)
		{
			return Vis.Evaluate(op, (int)f, this.bits[Vis.Mask.s_role]);
		}

		public bool Eval(Vis.Op<Vis.Role> op)
		{
			return op == this.role;
		}

		public Vis.Life Not(Vis.Life f)
		{
			return (this.life ^ f) & f;
		}

		public Vis.Role Not(Vis.Role f)
		{
			return (this.role ^ f) & f;
		}

		public Vis.Status Not(Vis.Status f)
		{
			return (this.stat ^ f) & f;
		}

		public void Remove(Vis.Life f)
		{
			Vis.Mask mask = this;
			mask.life = mask.life & ~f;
		}

		public void Remove(Vis.Role f)
		{
			Vis.Mask mask = this;
			mask.role = mask.role & ~f;
		}

		public void Remove(Vis.Status f)
		{
			Vis.Mask mask = this;
			mask.stat = mask.stat & ~f;
		}

		private static BitVector32.Section sect(int count, ref int i)
		{
			return Vis.Mask.sect_(count, ref i);
		}

		private static BitVector32.Section sect_(int count, ref int i)
		{
			BitVector32.Section section;
			if (count == 0)
			{
				return new BitVector32.Section();
			}
			if (i == 0)
			{
				BitVector32.Section section1 = BitVector32.CreateSection((short)((1 << (count & 31)) - 1));
				i = i + count;
				return section1;
			}
			int num = i;
			if (num < 8)
			{
				section = BitVector32.CreateSection((short)((1 << (num & 31)) - 1));
			}
			else
			{
				section = BitVector32.CreateSection(255);
				for (num = num - 8; num >= 8; num = num - 8)
				{
					section = BitVector32.CreateSection(255, section);
				}
				if (num > 0)
				{
					section = BitVector32.CreateSection((short)((1 << (num & 31)) - 1), section);
				}
			}
			BitVector32.Section section2 = BitVector32.CreateSection((short)((1 << (count & 31)) - 1), section);
			i = i + count;
			return section2;
		}
	}

	public enum Op
	{
		Always,
		Equals,
		All,
		Any,
		None,
		NotEquals,
		Never
	}

	public struct Op<TFlags> : IEquatable<Vis.Op>, IEquatable<Vis.Op<TFlags>>
	where TFlags : struct, IConvertible, IFormattable, IComparable
	{
		private Vis.OpBase _;

		private byte _op
		{
			get
			{
				return this._._op;
			}
			set
			{
				this._._op = value;
			}
		}

		private int _val
		{
			get
			{
				return this._._val;
			}
			set
			{
				this._._val = value;
			}
		}

		public int data
		{
			get
			{
				return this._val;
			}
			set
			{
				this._val = value;
			}
		}

		public int intvalue
		{
			get
			{
				return this._val & 16777216;
			}
			set
			{
				this._val = this._val & 16777216 | value & 16777215;
			}
		}

		public Vis.Op op
		{
			get
			{
				return (Vis.Op)(this._op & 127);
			}
			set
			{
				this._op = (byte)(this._op & 128 | (byte)value & 127);
			}
		}

		public TFlags @value
		{
			get
			{
				return (TFlags)Enum.ToObject(typeof(TFlags), this._val & 16777215);
			}
			set
			{
				this._val = this._val & 16777216 | Vis.Op<TFlags>.ToInt(value) & 16777215;
			}
		}

		internal Op(byte op, int val)
		{
			this._ = new Vis.OpBase(op, val);
		}

		public Op(Vis.Op op, TFlags flags) : this((byte)op, Convert.ToInt32(flags))
		{
		}

		internal Op(int op, int flags) : this((byte)op, flags)
		{
		}

		public override bool Equals(object obj)
		{
			if (obj is Vis.Op<TFlags>)
			{
				return this.Equals((Vis.Op<TFlags>)obj);
			}
			if (obj as Vis.Op != Vis.Op.Always)
			{
				return this.Equals((Vis.Op)((int)obj));
			}
			return obj.Equals((Vis.Op<TFlags>)this);
		}

		public bool Equals(Vis.Op<TFlags> other)
		{
			return other._val == this;
		}

		public bool Equals(Vis.Op other)
		{
			return other == this.op;
		}

		public Vis.Op<TFlags>.Res Eval(int flags)
		{
			return new Vis.Op<TFlags>.Res(this, (TFlags)Enum.ToObject(typeof(TFlags), flags), flags);
		}

		public Vis.Op<TFlags>.Res Eval(TFlags flags)
		{
			return new Vis.Op<TFlags>.Res(this, flags, Vis.Op<TFlags>.ToInt(flags));
		}

		public override int GetHashCode()
		{
			return this._val & 2147483647;
		}

		public static Vis.Op<TFlags>.Res operator +(Vis.Op<TFlags> op, TFlags flags)
		{
			return op.Eval(flags);
		}

		public static Vis.Op<TFlags>.Res operator +(Vis.Op<TFlags> op, int flags)
		{
			return op.Eval(flags);
		}

		public static bool operator ==(Vis.Op<TFlags> op, TFlags flags)
		{
			return Vis.Evaluate(op.op, op._val & 16777215, Vis.Op<TFlags>.ToInt(flags));
		}

		public static bool operator ==(TFlags flags, Vis.Op<TFlags> op)
		{
			return Vis.Evaluate(op.op, op._val & 16777215, Vis.Op<TFlags>.ToInt(flags));
		}

		public static bool operator ==(Vis.Op<TFlags> L, Vis.Op R)
		{
			return L._op == (int)((sbyte)R);
		}

		public static bool operator ==(Vis.Op L, Vis.Op<TFlags> R)
		{
			return R._op == (int)((sbyte)L);
		}

		public static bool operator ==(Vis.Op<TFlags> L, int R)
		{
			return L._val == R;
		}

		public static bool operator ==(int R, Vis.Op<TFlags> L)
		{
			return L._val == R;
		}

		public static bool operator ==(Vis.Op<TFlags> L, Vis.Op<TFlags> R)
		{
			return L._val == R._val;
		}

		public static implicit operator Op<TFlags>(int data)
		{
			return new Vis.Op<TFlags>()
			{
				_val = data
			};
		}

		public static implicit operator Int32(Vis.Op<TFlags> op)
		{
			return op._val;
		}

		public static implicit operator Op(Vis.Op<TFlags> op)
		{
			return op.op;
		}

		public static bool operator !=(Vis.Op<TFlags> op, TFlags flags)
		{
			return !Vis.Evaluate(op.op, op._val & 16777215, Vis.Op<TFlags>.ToInt(flags));
		}

		public static bool operator !=(TFlags flags, Vis.Op<TFlags> op)
		{
			return !Vis.Evaluate(op.op, op._val & 16777215, Vis.Op<TFlags>.ToInt(flags));
		}

		public static bool operator !=(Vis.Op<TFlags> L, Vis.Op R)
		{
			return L._op != (int)((sbyte)R);
		}

		public static bool operator !=(Vis.Op L, Vis.Op<TFlags> R)
		{
			return R._op != (int)((sbyte)L);
		}

		public static bool operator !=(Vis.Op<TFlags> L, int R)
		{
			return L._val != R;
		}

		public static bool operator !=(int R, Vis.Op<TFlags> L)
		{
			return L._val != R;
		}

		public static bool operator !=(Vis.Op<TFlags> L, Vis.Op<TFlags> R)
		{
			return L._val != R._val;
		}

		public static Vis.Op<TFlags>.Res operator -(Vis.Op<TFlags> op, TFlags flags)
		{
			return op.Eval(flags);
		}

		public static Vis.Op<TFlags>.Res operator -(Vis.Op<TFlags> op, int flags)
		{
			return op.Eval(flags);
		}

		public static Vis.Op<TFlags> operator -(Vis.Op<TFlags> op)
		{
			op.op = Vis.Negate(op.op);
			return op;
		}

		private static int ToInt(TFlags f)
		{
			return Vis.EnumUtil<TFlags>.ToInt(f);
		}

		public override string ToString()
		{
			return string.Concat(this.op, ':', this.@value);
		}

		public struct Res
		{
			public readonly TFlags query;

			private readonly Vis.Op<TFlags> _op;

			public int data
			{
				get
				{
					return this._op._val;
				}
			}

			public bool failed
			{
				get
				{
					return (this._op._val & -2147483648) != -2147483648;
				}
			}

			public int intvalue
			{
				get
				{
					return this._op.intvalue;
				}
			}

			public Vis.Op<TFlags> operation
			{
				get
				{
					return this._op;
				}
			}

			public bool passed
			{
				get
				{
					return (this._op._val & -2147483648) == -2147483648;
				}
			}

			public TFlags @value
			{
				get
				{
					return this._op.@value;
				}
			}

			internal Res(Vis.Op<TFlags> op, TFlags flags, int flagsint)
			{
				this._op = op;
				this.query = flags;
				if (!Vis.Evaluate(op.op, op.intvalue, flagsint))
				{
					this._op._val = this._op._val & 2147483647;
				}
				else
				{
					this._op._val = this._op._val | -2147483648;
				}
			}

			public override int GetHashCode()
			{
				Vis.Op<TFlags> op = this._op;
				return (-2147483648 | op._val) ^ Vis.Op<TFlags>.ToInt(this.query);
			}

			public static implicit operator Boolean(Vis.Op<TFlags>.Res r)
			{
				return r.passed;
			}

			public static bool operator !(Vis.Op<TFlags>.Res r)
			{
				return r.failed;
			}

			public override string ToString()
			{
				return string.Format("{0}({1}) == {2}", this.operation, this.query, this.passed);
			}
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	private struct OpBase
	{
		[FieldOffset(0)]
		public int _val;

		[FieldOffset(3)]
		public byte _op;

		public OpBase(byte _op, int _val)
		{
			this._val = _val;
			this._op = _op;
		}
	}

	public enum Region
	{
		Life,
		Status,
		Role
	}

	[Flags]
	public enum Role
	{
		Citizen = 1,
		Criminal = 2,
		Authority = 4,
		Target = 8,
		Entourage = 16,
		Player = 32,
		Vehicle = 64,
		Animal = 128
	}

	public struct Rule
	{
		public Vis.Rule.Setup setup;

		public Vis.Mask reject;

		public Vis.Mask accept;

		public Vis.Mask conditional;

		public Vis.Op<Vis.Life> acceptLife
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.life;
				return new Vis.Op<Vis.Life>((byte)regionSetup.accept, (int)this.accept.life);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.life;
				regionSetup.accept = value.op;
				this.setup.life = regionSetup;
				this.accept.life = value.@value;
			}
		}

		public Vis.Op<Vis.Role> acceptRole
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.role;
				return new Vis.Op<Vis.Role>((byte)regionSetup.accept, (int)this.accept.role);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.role;
				regionSetup.accept = value.op;
				this.setup.role = regionSetup;
				this.accept.role = value.@value;
			}
		}

		public Vis.Op<Vis.Status> acceptStatus
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.stat;
				return new Vis.Op<Vis.Status>((byte)regionSetup.accept, (int)this.accept.stat);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.stat;
				regionSetup.accept = value.op;
				this.setup.stat = regionSetup;
				this.accept.stat = value.@value;
			}
		}

		public Vis.Op<Vis.Life> conditionalLife
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.life;
				return new Vis.Op<Vis.Life>((byte)regionSetup.conditional, (int)this.conditional.life);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.life;
				regionSetup.conditional = value.op;
				this.setup.life = regionSetup;
				this.conditional.life = value.@value;
			}
		}

		public Vis.Op<Vis.Role> conditionalRole
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.role;
				return new Vis.Op<Vis.Role>((byte)regionSetup.conditional, (int)this.conditional.role);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.role;
				regionSetup.conditional = value.op;
				this.setup.role = regionSetup;
				this.conditional.role = value.@value;
			}
		}

		public Vis.Op<Vis.Status> conditionalStatus
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.stat;
				return new Vis.Op<Vis.Status>((byte)regionSetup.conditional, (int)this.conditional.stat);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.stat;
				regionSetup.conditional = value.op;
				this.setup.stat = regionSetup;
				this.conditional.stat = value.@value;
			}
		}

		public Vis.Mask this[Vis.Rule.Step step]
		{
			get
			{
				switch (step)
				{
					case Vis.Rule.Step.Accept:
					{
						return this.accept;
					}
					case Vis.Rule.Step.Conditional:
					{
						return this.conditional;
					}
					case Vis.Rule.Step.Reject:
					{
						return this.reject;
					}
				}
				throw new ArgumentOutOfRangeException("step");
			}
			set
			{
				switch (step)
				{
					case Vis.Rule.Step.Accept:
					{
						this.accept = value;
						break;
					}
					case Vis.Rule.Step.Conditional:
					{
						this.conditional = value;
						break;
					}
					case Vis.Rule.Step.Reject:
					{
						this.reject = value;
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException("step");
					}
				}
			}
		}

		public Vis.Op<Vis.Life> rejectLife
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.life;
				return new Vis.Op<Vis.Life>((byte)regionSetup.reject, (int)this.reject.life);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.life;
				regionSetup.reject = value.op;
				this.setup.life = regionSetup;
				this.reject.life = value.@value;
			}
		}

		public Vis.Op<Vis.Role> rejectRole
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.role;
				return new Vis.Op<Vis.Role>((byte)regionSetup.reject, (int)this.reject.role);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.life;
				regionSetup.reject = value.op;
				this.setup.life = regionSetup;
				this.reject.role = value.@value;
			}
		}

		public Vis.Op<Vis.Status> rejectStatus
		{
			get
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.stat;
				return new Vis.Op<Vis.Status>((byte)regionSetup.reject, (int)this.reject.stat);
			}
			set
			{
				Vis.Rule.RegionSetup regionSetup = this.setup.life;
				regionSetup.reject = value.op;
				this.setup.life = regionSetup;
				this.reject.stat = value.@value;
			}
		}

		private Vis.Rule.Failure Accept(Vis.Mask mask)
		{
			if (!this.setup.checkAccept)
			{
				return Vis.Rule.Failure.None;
			}
			Vis.Rule.Failure failure = Vis.Rule.Failure.None;
			if (!mask.Eval(this.acceptLife))
			{
				failure = failure | Vis.Rule.Failure.Conditional | Vis.Rule.Failure.Life;
			}
			if (!mask.Eval(this.acceptRole))
			{
				failure = failure | Vis.Rule.Failure.Conditional | Vis.Rule.Failure.Role;
			}
			if (!mask.Eval(this.acceptStatus))
			{
				failure = failure | Vis.Rule.Failure.Conditional | Vis.Rule.Failure.Status;
			}
			return failure;
		}

		private Vis.Rule.Failure Check(Vis.Mask a, Vis.Mask c, Vis.Mask r)
		{
			Vis.Rule.Failure failure = this.Accept(a);
			if (failure != Vis.Rule.Failure.None)
			{
				return failure;
			}
			failure = this.Conditional(c);
			if (failure != Vis.Rule.Failure.None)
			{
				return failure;
			}
			failure = this.Reject(r);
			return failure;
		}

		private Vis.Rule.Failure Conditional(Vis.Mask mask)
		{
			if (!this.setup.checkConditional)
			{
				return Vis.Rule.Failure.None;
			}
			Vis.Rule.Failure failure = Vis.Rule.Failure.None;
			if (!mask.Eval(this.conditionalLife))
			{
				failure = failure | Vis.Rule.Failure.Conditional | Vis.Rule.Failure.Life;
			}
			if (!mask.Eval(this.conditionalRole))
			{
				failure = failure | Vis.Rule.Failure.Conditional | Vis.Rule.Failure.Role;
			}
			if (!mask.Eval(this.conditionalStatus))
			{
				failure = failure | Vis.Rule.Failure.Conditional | Vis.Rule.Failure.Status;
			}
			return failure;
		}

		public static Vis.Rule Decode(int[] data, int index)
		{
			Vis.Rule rule = new Vis.Rule();
			int num = index;
			index = num + 1;
			rule.setup.data = data[num];
			int num1 = index;
			index = num1 + 1;
			rule.accept.data = data[num1];
			int num2 = index;
			index = num2 + 1;
			rule.conditional.data = data[num2];
			rule.reject.data = data[index];
			return rule;
		}

		public static void Encode(ref Vis.Rule rule, int[] data, int index)
		{
			int num = index;
			index = num + 1;
			data[num] = rule.setup.data;
			int num1 = index;
			index = num1 + 1;
			data[num1] = rule.accept.data;
			int num2 = index;
			index = num2 + 1;
			data[num2] = rule.conditional.data;
			int num3 = index;
			index = num3 + 1;
			data[num3] = rule.reject.data;
		}

		public Vis.Rule.Failure Pass(Vis.Mask self, Vis.Mask other)
		{
			switch (this.setup.option)
			{
				case Vis.Rule.Setup.Option.Default:
				{
					return this.Check(other, self, other);
				}
				case Vis.Rule.Setup.Option.Inverse:
				{
					return this.Check(self, other, self);
				}
				case Vis.Rule.Setup.Option.NoConditional:
				{
					return this.Check(other, other, other);
				}
				case Vis.Rule.Setup.Option.AllConditional:
				{
					return this.Check(self, self, self);
				}
				default:
				{
					return this.Check(other, self, other);
				}
			}
		}

		private Vis.Rule.Failure Reject(Vis.Mask mask)
		{
			if (!this.setup.checkReject)
			{
				return Vis.Rule.Failure.None;
			}
			Vis.Rule.Failure failure = Vis.Rule.Failure.None;
			if (mask.Eval(this.rejectLife))
			{
				failure = failure | Vis.Rule.Failure.Reject | Vis.Rule.Failure.Life;
			}
			if (mask.Eval(this.rejectRole))
			{
				failure = failure | Vis.Rule.Failure.Reject | Vis.Rule.Failure.Role;
			}
			if (mask.Eval(this.rejectStatus))
			{
				failure = failure | Vis.Rule.Failure.Reject | Vis.Rule.Failure.Status;
			}
			return failure;
		}

		public enum Clearance
		{
			Outside,
			Enter,
			Stay,
			Exit
		}

		[Flags]
		public enum Failure
		{
			None = 0,
			Accept = 1,
			Conditional = 2,
			Reject = 4,
			Life = 8,
			Role = 16,
			Status = 32
		}

		public struct RegionSetup
		{
			private readonly static BitVector32.Section s_apt;

			private readonly static BitVector32.Section s_cnd;

			private readonly static BitVector32.Section s_rej;

			private readonly static BitVector32.Section s_whole;

			private readonly static BitVector32.Section s_region;

			private BitVector32 _;

			public Vis.Op accept
			{
				get
				{
					return (Vis.Op)this._[Vis.Rule.RegionSetup.s_apt];
				}
				set
				{
					this._[Vis.Rule.RegionSetup.s_apt] = (int)value;
				}
			}

			public Vis.Op conditional
			{
				get
				{
					return (Vis.Op)this._[Vis.Rule.RegionSetup.s_cnd];
				}
				set
				{
					this._[Vis.Rule.RegionSetup.s_cnd] = (int)value;
				}
			}

			internal int dat
			{
				get
				{
					return this._[Vis.Rule.RegionSetup.s_whole];
				}
			}

			public Vis.Op this[Vis.Rule.Step step]
			{
				get
				{
					switch (step)
					{
						case Vis.Rule.Step.Accept:
						{
							return this.accept;
						}
						case Vis.Rule.Step.Conditional:
						{
							return this.conditional;
						}
						case Vis.Rule.Step.Reject:
						{
							return this.reject;
						}
					}
					throw new ArgumentOutOfRangeException("step");
				}
				set
				{
					switch (step)
					{
						case Vis.Rule.Step.Accept:
						{
							this.accept = value;
							break;
						}
						case Vis.Rule.Step.Conditional:
						{
							this.conditional = value;
							break;
						}
						case Vis.Rule.Step.Reject:
						{
							this.reject = value;
							break;
						}
						default:
						{
							throw new ArgumentOutOfRangeException("step");
						}
					}
				}
			}

			public Vis.Region region
			{
				get
				{
					return (Vis.Region)this._[Vis.Rule.RegionSetup.s_region];
				}
				set
				{
					this._[Vis.Rule.RegionSetup.s_region] = (int)value;
				}
			}

			public Vis.Op reject
			{
				get
				{
					return (Vis.Op)this._[Vis.Rule.RegionSetup.s_rej];
				}
				set
				{
					this._[Vis.Rule.RegionSetup.s_rej] = (int)value;
				}
			}

			static RegionSetup()
			{
				BitVector32.Section section = BitVector32.CreateSection(7);
				Vis.Rule.RegionSetup.s_apt = section;
				BitVector32.Section section1 = BitVector32.CreateSection(7, section);
				Vis.Rule.RegionSetup.s_cnd = section1;
				BitVector32.Section section2 = BitVector32.CreateSection(7, section1);
				Vis.Rule.RegionSetup.s_rej = section2;
				BitVector32.Section section3 = section2;
				Vis.Rule.RegionSetup.s_whole = BitVector32.CreateSection(511);
				section3 = BitVector32.CreateSection(7, section3);
				BitVector32.Section section4 = BitVector32.CreateSection(3, section3);
				Vis.Rule.RegionSetup.s_region = section4;
				section3 = section4;
			}

			internal RegionSetup(int value, Vis.Region reg)
			{
				BitVector32.Section sRegion = Vis.Rule.RegionSetup.s_region;
				this._ = new BitVector32(value | (int)reg << (sRegion.Offset & 31));
			}
		}

		public struct Setup
		{
			private readonly static BitVector32.Section s_life;

			private readonly static BitVector32.Section[] s_life_;

			private readonly static BitVector32.Section s_stat;

			private readonly static BitVector32.Section[] s_stat_;

			private readonly static BitVector32.Section s_role;

			private readonly static BitVector32.Section[] s_role_;

			private readonly static BitVector32.Section s_options;

			private readonly static BitVector32.Section s_check;

			private readonly static BitVector32.Section[] s_check_;

			private BitVector32 _;

			public Vis.Rule.StepSetup accept
			{
				get
				{
					return this.Get(0);
				}
				set
				{
					this.Set(0, value);
				}
			}

			public Vis.Rule.StepCheck check
			{
				get
				{
					return new Vis.Rule.StepCheck(this._[Vis.Rule.Setup.s_check]);
				}
				set
				{
					this._[Vis.Rule.Setup.s_check] = value.@value;
				}
			}

			public bool checkAccept
			{
				get
				{
					return this._[Vis.Rule.Setup.s_check_[0]] != 0;
				}
				set
				{
					BitVector32.Section sCheck_ = Vis.Rule.Setup.s_check_[0];
					this._[sCheck_] = (!value ? 0 : 1);
				}
			}

			public bool checkConditional
			{
				get
				{
					return this._[Vis.Rule.Setup.s_check_[1]] != 0;
				}
				set
				{
					BitVector32.Section sCheck_ = Vis.Rule.Setup.s_check_[1];
					this._[sCheck_] = (!value ? 0 : 1);
				}
			}

			public bool checkReject
			{
				get
				{
					return this._[Vis.Rule.Setup.s_check_[2]] != 0;
				}
				set
				{
					BitVector32.Section sCheck_ = Vis.Rule.Setup.s_check_[2];
					this._[sCheck_] = (!value ? 0 : 1);
				}
			}

			public Vis.Rule.StepSetup conditional
			{
				get
				{
					return this.Get(1);
				}
				set
				{
					this.Set(1, value);
				}
			}

			internal int data
			{
				get
				{
					return this._.Data;
				}
				set
				{
					this._ = new BitVector32(value);
				}
			}

			public Vis.Rule.RegionSetup this[Vis.Region region]
			{
				get
				{
					switch (region)
					{
						case Vis.Region.Life:
						{
							return this.life;
						}
						case Vis.Region.Status:
						{
							return this.stat;
						}
						case Vis.Region.Role:
						{
							return this.role;
						}
					}
					throw new ArgumentOutOfRangeException("region");
				}
				set
				{
					switch (region)
					{
						case Vis.Region.Life:
						{
							this.life = value;
							break;
						}
						case Vis.Region.Status:
						{
							this.stat = value;
							break;
						}
						case Vis.Region.Role:
						{
							this.role = value;
							break;
						}
						default:
						{
							throw new ArgumentOutOfRangeException("region");
						}
					}
				}
			}

			public Vis.Rule.StepSetup this[Vis.Rule.Step step]
			{
				get
				{
					switch (step)
					{
						case Vis.Rule.Step.Accept:
						{
							return this.accept;
						}
						case Vis.Rule.Step.Conditional:
						{
							return this.conditional;
						}
						case Vis.Rule.Step.Reject:
						{
							return this.reject;
						}
					}
					throw new ArgumentOutOfRangeException("step");
				}
				set
				{
					switch (step)
					{
						case Vis.Rule.Step.Accept:
						{
							this.accept = value;
							break;
						}
						case Vis.Rule.Step.Conditional:
						{
							this.conditional = value;
							break;
						}
						case Vis.Rule.Step.Reject:
						{
							this.reject = value;
							break;
						}
						default:
						{
							throw new ArgumentOutOfRangeException("step");
						}
					}
				}
			}

			public Vis.Rule.RegionSetup life
			{
				get
				{
					return new Vis.Rule.RegionSetup(this._[Vis.Rule.Setup.s_life], Vis.Region.Life);
				}
				set
				{
					this._[Vis.Rule.Setup.s_life] = value.dat;
				}
			}

			public Vis.Rule.Setup.Option option
			{
				get
				{
					return (Vis.Rule.Setup.Option)this._[Vis.Rule.Setup.s_options];
				}
				set
				{
					this._[Vis.Rule.Setup.s_options] = (int)value;
				}
			}

			public Vis.Rule.StepSetup reject
			{
				get
				{
					return this.Get(2);
				}
				set
				{
					this.Set(2, value);
				}
			}

			public Vis.Rule.RegionSetup role
			{
				get
				{
					return new Vis.Rule.RegionSetup(this._[Vis.Rule.Setup.s_role], Vis.Region.Role);
				}
				set
				{
					this._[Vis.Rule.Setup.s_role] = value.dat;
				}
			}

			public Vis.Rule.RegionSetup stat
			{
				get
				{
					return new Vis.Rule.RegionSetup(this._[Vis.Rule.Setup.s_stat], Vis.Region.Status);
				}
				set
				{
					this._[Vis.Rule.Setup.s_stat] = value.dat;
				}
			}

			static Setup()
			{
				BitVector32.Section section = BitVector32.CreateSection(511);
				Vis.Rule.Setup.s_life = section;
				BitVector32.Section section1 = BitVector32.CreateSection(511, section);
				Vis.Rule.Setup.s_stat = section1;
				BitVector32.Section section2 = BitVector32.CreateSection(511, section1);
				Vis.Rule.Setup.s_role = section2;
				BitVector32.Section section3 = BitVector32.CreateSection(3, section2);
				Vis.Rule.Setup.s_options = section3;
				BitVector32.Section section4 = BitVector32.CreateSection(7, section3);
				Vis.Rule.Setup.s_check = section4;
				Vis.Rule.Setup.s_life_ = new BitVector32.Section[3];
				Vis.Rule.Setup.s_stat_ = new BitVector32.Section[3];
				Vis.Rule.Setup.s_role_ = new BitVector32.Section[3];
				Vis.Rule.Setup.s_check_ = new BitVector32.Section[3];
				int i = 0;
				Vis.Rule.Setup.s_life_[i] = BitVector32.CreateSection(7);
				Vis.Rule.Setup.s_stat_[i] = BitVector32.CreateSection(7, Vis.Rule.Setup.s_life);
				Vis.Rule.Setup.s_role_[i] = BitVector32.CreateSection(7, Vis.Rule.Setup.s_stat);
				Vis.Rule.Setup.s_check_[i] = BitVector32.CreateSection(1, Vis.Rule.Setup.s_options);
				for (i++; i < 3; i++)
				{
					Vis.Rule.Setup.s_life_[i] = BitVector32.CreateSection(7, Vis.Rule.Setup.s_life_[i - 1]);
					Vis.Rule.Setup.s_stat_[i] = BitVector32.CreateSection(7, Vis.Rule.Setup.s_stat_[i - 1]);
					Vis.Rule.Setup.s_role_[i] = BitVector32.CreateSection(7, Vis.Rule.Setup.s_role_[i - 1]);
					Vis.Rule.Setup.s_check_[i] = BitVector32.CreateSection(1, Vis.Rule.Setup.s_check_[i - 1]);
				}
			}

			private Vis.Rule.StepSetup Get(int i)
			{
				return new Vis.Rule.StepSetup(this._[Vis.Rule.Setup.s_life_[i]], this._[Vis.Rule.Setup.s_stat_[i]], this._[Vis.Rule.Setup.s_role_[i]], i, this._[Vis.Rule.Setup.s_check_[i]]);
			}

			private void Set(int i, Vis.Rule.StepSetup step)
			{
				this._[Vis.Rule.Setup.s_life_[i]] = (short)step.life & Vis.Rule.Setup.s_life_[i].Mask;
				this._[Vis.Rule.Setup.s_stat_[i]] = (short)step.stat & Vis.Rule.Setup.s_stat_[i].Mask;
				this._[Vis.Rule.Setup.s_role_[i]] = (short)step.role & Vis.Rule.Setup.s_role_[i].Mask;
				BitVector32.Section sCheck_ = Vis.Rule.Setup.s_check_[i];
				this._[sCheck_] = (!step.enabled ? 0 : 1);
			}

			public void SetSetup(Vis.Rule.RegionSetup operations)
			{
				this[operations.region] = operations;
			}

			public void SetSetup(Vis.Rule.StepSetup operations)
			{
				this[operations.step] = operations;
			}

			public enum Option
			{
				Default,
				Inverse,
				NoConditional,
				AllConditional
			}
		}

		public enum Step
		{
			Accept,
			Conditional,
			Reject
		}

		public struct StepCheck
		{
			private byte bits;

			public bool accept
			{
				get
				{
					return (this.bits & 1) == 1;
				}
				set
				{
					this.bits = (byte)((!value ? this.bits & 6 : this.bits | 1));
				}
			}

			public bool conditional
			{
				get
				{
					return (this.bits & 2) == 2;
				}
				set
				{
					this.bits = (byte)((!value ? this.bits & 5 : this.bits | 2));
				}
			}

			public bool this[Vis.Rule.Step step]
			{
				get
				{
					switch (step)
					{
						case Vis.Rule.Step.Accept:
						{
							return this.accept;
						}
						case Vis.Rule.Step.Conditional:
						{
							return this.conditional;
						}
						case Vis.Rule.Step.Reject:
						{
							return this.reject;
						}
					}
					throw new ArgumentOutOfRangeException("step");
				}
				set
				{
					switch (step)
					{
						case Vis.Rule.Step.Accept:
						{
							this.accept = value;
							break;
						}
						case Vis.Rule.Step.Conditional:
						{
							this.conditional = value;
							break;
						}
						case Vis.Rule.Step.Reject:
						{
							this.reject = value;
							break;
						}
						default:
						{
							throw new ArgumentOutOfRangeException("step");
						}
					}
				}
			}

			public bool reject
			{
				get
				{
					return (this.bits & 4) == 4;
				}
				set
				{
					this.bits = (byte)((!value ? this.bits & 3 : this.bits | 4));
				}
			}

			internal int @value
			{
				get
				{
					return this.bits;
				}
			}

			internal StepCheck(int i)
			{
				this.bits = (byte)i;
			}
		}

		public struct StepSetup
		{
			private readonly static BitVector32.Section s_life;

			private readonly static BitVector32.Section s_stat;

			private readonly static BitVector32.Section s_role;

			private readonly static BitVector32.Section s_step;

			private readonly static BitVector32.Section s_enable;

			private BitVector32 _;

			public bool enabled
			{
				get
				{
					return this._[Vis.Rule.StepSetup.s_enable] != 0;
				}
				set
				{
					BitVector32.Section sEnable = Vis.Rule.StepSetup.s_enable;
					this._[sEnable] = (!value ? 0 : 1);
				}
			}

			public Vis.Op this[Vis.Region region]
			{
				get
				{
					switch (region)
					{
						case Vis.Region.Life:
						{
							return this.life;
						}
						case Vis.Region.Status:
						{
							return this.stat;
						}
						case Vis.Region.Role:
						{
							return this.role;
						}
					}
					throw new ArgumentOutOfRangeException("region");
				}
				set
				{
					switch (region)
					{
						case Vis.Region.Life:
						{
							this.life = value;
							break;
						}
						case Vis.Region.Status:
						{
							this.stat = value;
							break;
						}
						case Vis.Region.Role:
						{
							this.role = value;
							break;
						}
						default:
						{
							throw new ArgumentOutOfRangeException("region");
						}
					}
				}
			}

			public Vis.Op life
			{
				get
				{
					return (Vis.Op)this._[Vis.Rule.StepSetup.s_life];
				}
				set
				{
					this._[Vis.Rule.StepSetup.s_life] = (int)value;
				}
			}

			public Vis.Op role
			{
				get
				{
					return (Vis.Op)this._[Vis.Rule.StepSetup.s_role];
				}
				set
				{
					this._[Vis.Rule.StepSetup.s_role] = (int)value;
				}
			}

			public Vis.Op stat
			{
				get
				{
					return (Vis.Op)this._[Vis.Rule.StepSetup.s_stat];
				}
				set
				{
					this._[Vis.Rule.StepSetup.s_stat] = (int)value;
				}
			}

			public Vis.Rule.Step step
			{
				get
				{
					return (Vis.Rule.Step)this._[Vis.Rule.StepSetup.s_step];
				}
				set
				{
					this._[Vis.Rule.StepSetup.s_step] = (int)value;
				}
			}

			static StepSetup()
			{
				BitVector32.Section section = BitVector32.CreateSection(7);
				Vis.Rule.StepSetup.s_life = section;
				BitVector32.Section section1 = BitVector32.CreateSection(255, section);
				Vis.Rule.StepSetup.s_step = section1;
				BitVector32.Section section2 = BitVector32.CreateSection(1, section1);
				Vis.Rule.StepSetup.s_enable = section2;
				BitVector32.Section section3 = BitVector32.CreateSection(7, section2);
				Vis.Rule.StepSetup.s_stat = section3;
				BitVector32.Section section4 = BitVector32.CreateSection(511, section3);
				BitVector32.Section section5 = BitVector32.CreateSection(7, section4);
				Vis.Rule.StepSetup.s_role = section5;
				section4 = section5;
			}

			internal StepSetup(int life, int stat, int role, int step, int enable)
			{
				this = new Vis.Rule.StepSetup();
				this._[Vis.Rule.StepSetup.s_life] = life;
				this._[Vis.Rule.StepSetup.s_stat] = stat;
				this._[Vis.Rule.StepSetup.s_role] = role;
				this._[Vis.Rule.StepSetup.s_step] = step;
				this._[Vis.Rule.StepSetup.s_enable] = enable;
			}
		}
	}

	public struct Stamp
	{
		public Vector3 position;

		public Vector4 plane;

		public Quaternion rotation;

		public Vector3 forward
		{
			get
			{
				return new Vector3(this.plane.x, this.plane.y, this.plane.z);
			}
		}

		public Stamp(Transform transform)
		{
			this.position = transform.position;
			this.rotation = transform.rotation;
			Vector3 vector3 = transform.forward;
			this.plane.x = vector3.x;
			this.plane.y = vector3.y;
			this.plane.z = vector3.z;
			this.plane.w = this.position.x * this.plane.x + this.position.y * this.plane.y + this.position.z * this.plane.z;
		}

		public void Collect(Transform transform)
		{
			this.position = transform.position;
			this.rotation = transform.rotation;
			Vector3 vector3 = transform.forward;
			this.plane.x = vector3.x;
			this.plane.y = vector3.y;
			this.plane.z = vector3.z;
			float single = this.position.x * this.forward.x + this.position.y * this.forward.y;
			float single1 = this.position.z;
			Vector3 vector31 = this.forward;
			this.plane.w = single + single1 * vector31.z;
		}
	}

	[Flags]
	public enum Status
	{
		Casual = 1,
		Hurt = 2,
		Curious = 4,
		Alert = 8,
		Search = 16,
		Armed = 32,
		Attacking = 64
	}

	public enum Trait
	{
		Alive = 0,
		Unconcious = 1,
		Dead = 2,
		Casual = 8,
		Hurt = 9,
		Curious = 10,
		Alert = 11,
		Search = 12,
		Armed = 13,
		Attacking = 14,
		Citizen = 24,
		Criminal = 25,
		Authority = 26,
		Target = 27,
		Entourage = 28,
		Player = 29,
		Vehicle = 30,
		Animal = 31
	}
}