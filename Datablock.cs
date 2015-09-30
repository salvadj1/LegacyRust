using Facepunch.Build;
using Facepunch.Hash;
using System;
using uLink;
using UnityEngine;

[UniqueBundleScriptableObject]
public class Datablock : ScriptableObject, IComparable<Datablock>
{
	[HideInInspector]
	[SerializeField]
	private int _uniqueID;

	public int uniqueID
	{
		get
		{
			return this._uniqueID;
		}
	}

	public Datablock()
	{
	}

	public int CompareTo(Datablock other)
	{
		if (object.ReferenceEquals(other, this))
		{
			return 0;
		}
		if (!other)
		{
			return -1;
		}
		int num = this._uniqueID.CompareTo(other._uniqueID);
		if (num != 0)
		{
			return num;
		}
		return base.name.CompareTo(other.name);
	}

	public override int GetHashCode()
	{
		return this._uniqueID;
	}

	public uint SecureHash()
	{
		return this.SecureHash(0);
	}

	public uint SecureHash(uint seed)
	{
		uLink.BitStream bitStream = new uLink.BitStream(true);
		try
		{
			this.SecureWriteMemberValues(bitStream);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return MurmurHash2.UINT(bitStream.GetDataByteArray(), seed);
	}

	protected virtual void SecureWriteMemberValues(uLink.BitStream stream)
	{
		stream.WriteInt32(this._uniqueID);
	}

	public struct Ident : IEquatable<Datablock.Ident>, IEquatable<Datablock>
	{
		private const byte TYPE_NULL = 0;

		private const byte TYPE_DATABLOCK = 1;

		private const byte TYPE_INVENTORY_ITEM = 2;

		private const byte TYPE_STRING = 3;

		private const byte TYPE_HASH = 4;

		private const int FLAG_UNCONFIRMED = 128;

		private const int MASK_TYPE = 127;

		private const byte TYPE_STRING_UNCONFIRMED = 131;

		private const byte TYPE_HASH_UNCONFIRMED = 132;

		private const byte TYPE_INVENTORY_ITEM_UNCONFIRMED = 130;

		private const byte TYPE_DATABLOCK_UNCONFIRMED = 129;

		private readonly object refValue;

		private readonly int uid;

		private readonly byte type_f;

		public Datablock datablock
		{
			get
			{
				if ((this.type_f & 128) == 128)
				{
					this.Confirm();
				}
				return (Datablock)this.refValue;
			}
		}

		public bool exists
		{
			get
			{
				bool flag;
				if ((this.type_f & 128) == 128)
				{
					this.Confirm();
				}
				if (this.type_f == 0)
				{
					flag = false;
				}
				else
				{
					flag = (Datablock)this.refValue;
				}
				return flag;
			}
		}

		public string name
		{
			get
			{
				if ((this.type_f & 128) == 128)
				{
					this.Confirm();
				}
				if (this.type_f != 1)
				{
					return string.Empty;
				}
				Datablock datablock = (Datablock)this.refValue;
				if (!datablock)
				{
					return string.Empty;
				}
				return datablock.name;
			}
		}

		public int uniqueID
		{
			get
			{
				if ((this.type_f & 128) == 128)
				{
					this.Confirm();
				}
				return this.uid;
			}
		}

		public int? uniqueIDIfExists
		{
			get
			{
				if ((this.type_f & 128) == 128)
				{
					this.Confirm();
				}
				if (this.type_f != 0)
				{
					return new int?(this.uid);
				}
				return null;
			}
		}

		private Ident(object refValue, int uniqueID, byte type_f)
		{
			this.refValue = refValue;
			this.uid = uniqueID;
			this.type_f = type_f;
		}

		private Ident(object referenceValue, bool isNull, byte type)
		{
			if (!isNull)
			{
				this.refValue = referenceValue;
				this.uid = 0;
				this.type_f = type;
			}
			else
			{
				this = new Datablock.Ident();
			}
		}

		private Ident(object referenceValue, byte type) : this(referenceValue, !object.ReferenceEquals(referenceValue, null), type)
		{
		}

		private Ident(Datablock db) : this(db, db, 129)
		{
		}

		private Ident(InventoryItem item) : this(item, 130)
		{
		}

		private Ident(string name) : this(name, string.IsNullOrEmpty(name), 131)
		{
		}

		private Ident(int uniqueID)
		{
			this.refValue = null;
			this.type_f = 132;
			this.uid = uniqueID;
		}

		private void Confirm()
		{
			Datablock byName;
			switch (this.type_f & 127)
			{
				case 1:
				{
					byName = (Datablock)this.refValue;
					break;
				}
				case 2:
				{
					byName = ((InventoryItem)this.refValue).datablock;
					break;
				}
				case 3:
				{
					byName = DatablockDictionary.GetByName((string)this.refValue);
					break;
				}
				case 4:
				{
					byName = DatablockDictionary.GetByUniqueID(this.uid);
					break;
				}
				default:
				{
					this = new Datablock.Ident();
					return;
				}
			}
			if (!byName)
			{
				this = new Datablock.Ident();
			}
			else
			{
				this = new Datablock.Ident(byName, byName.uniqueID, 1);
			}
		}

		public bool Equals(Datablock.Ident other)
		{
			if ((this.type_f & 128) == 128)
			{
				this.Confirm();
			}
			if ((other.type_f & 128) == 128)
			{
				other.Confirm();
			}
			return object.Equals(this.refValue, other.refValue);
		}

		public bool Equals(Datablock datablock)
		{
			if ((this.type_f & 128) == 128)
			{
				this.Confirm();
			}
			return object.Equals(this.refValue, datablock);
		}

		public override bool Equals(object obj)
		{
			if (obj is Datablock.Ident)
			{
				return this.Equals((Datablock.Ident)obj);
			}
			if (!(obj is Datablock))
			{
				return false;
			}
			return this.Equals((Datablock)obj);
		}

		public bool GetDatablock(out Datablock datablock)
		{
			if ((this.type_f & 128) == 128)
			{
				this.Confirm();
			}
			if (this.type_f == 0)
			{
				datablock = null;
				return false;
			}
			datablock = (Datablock)this.refValue;
			return datablock;
		}

		public bool GetDatablock<TDatablock>(out TDatablock datablock)
		where TDatablock : Datablock
		{
			if ((this.type_f & 128) == 128)
			{
				this.Confirm();
			}
			if (this.type_f == 0)
			{
				datablock = (TDatablock)null;
				return false;
			}
			datablock = (TDatablock)((Datablock)this.refValue as TDatablock);
			return datablock;
		}

		public Datablock GetDatablock()
		{
			if ((this.type_f & 128) == 128)
			{
				this.Confirm();
			}
			if (this.type_f == 0)
			{
				return null;
			}
			return (Datablock)this.refValue;
		}

		public Datablock GetDatablock<TDatablock>()
		where TDatablock : Datablock
		{
			if ((this.type_f & 128) == 128)
			{
				this.Confirm();
			}
			if (this.type_f == 0)
			{
				throw new MissingReferenceException("this identifier is not valid");
			}
			return (object)((TDatablock)this.refValue);
		}

		public override int GetHashCode()
		{
			return this.uid;
		}

		public static bool operator ==(Datablock.Ident ident, Datablock.Ident other)
		{
			return ident.Equals(other);
		}

		public static bool operator ==(Datablock.Ident ident, Datablock other)
		{
			return ident.Equals(other);
		}

		public static bool operator ==(Datablock.Ident ident, string other)
		{
			if (string.IsNullOrEmpty(other))
			{
				return !ident.exists;
			}
			return ident.name == other;
		}

		public static bool operator ==(Datablock.Ident ident, int hash)
		{
			int? nullable = ident.uniqueIDIfExists;
			return (nullable.GetValueOrDefault() != hash ? false : nullable.HasValue);
		}

		public static bool operator ==(Datablock.Ident ident, uint hash)
		{
			return ident.uniqueID == hash;
		}

		public static bool operator ==(Datablock.Ident ident, ushort hash)
		{
			int? nullable = ident.uniqueIDIfExists;
			return (nullable.GetValueOrDefault() != hash ? false : nullable.HasValue);
		}

		public static bool operator ==(Datablock.Ident ident, short hash)
		{
			return ident.uniqueID == hash;
		}

		public static bool operator ==(Datablock.Ident ident, byte hash)
		{
			int? nullable = ident.uniqueIDIfExists;
			return (nullable.GetValueOrDefault() != hash ? false : nullable.HasValue);
		}

		public static bool operator ==(Datablock.Ident ident, sbyte hash)
		{
			return ident.uniqueID == (int)hash;
		}

		public static explicit operator Ident(ulong dbHash)
		{
			return new Datablock.Ident((int)dbHash);
		}

		public static explicit operator Ident(long dbHash)
		{
			return new Datablock.Ident((int)dbHash);
		}

		public static explicit operator Ident(InventoryItem item)
		{
			return new Datablock.Ident(item);
		}

		public static explicit operator Ident(Datablock db)
		{
			if (!db)
			{
				return new Datablock.Ident();
			}
			return new Datablock.Ident(db, db.uniqueID, 1);
		}

		public static bool operator @false(Datablock.Ident ident)
		{
			return !ident.exists;
		}

		public static implicit operator Ident(string dbName)
		{
			return new Datablock.Ident(dbName);
		}

		public static implicit operator Ident(int dbHash)
		{
			return new Datablock.Ident(dbHash);
		}

		public static implicit operator Ident(uint dbHash)
		{
			return new Datablock.Ident((int)dbHash);
		}

		[Obsolete("Make sure your wanting to get a dbhash from a ushort here.")]
		public static implicit operator Ident(ushort dbHash)
		{
			return new Datablock.Ident((int)dbHash);
		}

		[Obsolete("Make sure your wanting to get a dbhash from a short here.")]
		public static implicit operator Ident(short dbHash)
		{
			return new Datablock.Ident(dbHash);
		}

		[Obsolete("Make sure your wanting to get a dbhash from a byte here.")]
		public static implicit operator Ident(byte dbHash)
		{
			return new Datablock.Ident((int)dbHash);
		}

		[Obsolete("Make sure your wanting to get a dbhash from a sbyte here.")]
		public static implicit operator Ident(sbyte dbHash)
		{
			return new Datablock.Ident((int)dbHash);
		}

		public static bool operator !=(Datablock.Ident ident, Datablock.Ident other)
		{
			return !ident.Equals(other);
		}

		public static bool operator !=(Datablock.Ident ident, Datablock other)
		{
			return !ident.Equals(other);
		}

		public static bool operator !=(Datablock.Ident ident, string other)
		{
			if (string.IsNullOrEmpty(other))
			{
				return ident.exists;
			}
			return ident.name != other;
		}

		public static bool operator !=(Datablock.Ident ident, int hash)
		{
			int? nullable = ident.uniqueIDIfExists;
			return (nullable.GetValueOrDefault() != hash ? true : !nullable.HasValue);
		}

		public static bool operator !=(Datablock.Ident ident, uint hash)
		{
			return ident.uniqueID != hash;
		}

		public static bool operator !=(Datablock.Ident ident, ushort hash)
		{
			int? nullable = ident.uniqueIDIfExists;
			return (nullable.GetValueOrDefault() != hash ? true : !nullable.HasValue);
		}

		public static bool operator !=(Datablock.Ident ident, short hash)
		{
			return ident.uniqueID != hash;
		}

		public static bool operator !=(Datablock.Ident ident, byte hash)
		{
			int? nullable = ident.uniqueIDIfExists;
			return (nullable.GetValueOrDefault() != hash ? true : !nullable.HasValue);
		}

		public static bool operator !=(Datablock.Ident ident, sbyte hash)
		{
			return ident.uniqueID != (int)hash;
		}

		public static bool operator @true(Datablock.Ident ident)
		{
			return ident.exists;
		}

		public static Datablock.Ident operator +(Datablock.Ident ident)
		{
			if ((ident.type_f & 128) == 128)
			{
				ident.Confirm();
			}
			return ident;
		}

		public override string ToString()
		{
			string str;
			if ((this.type_f & 128) == 128)
			{
				this.Confirm();
			}
			if (this.type_f != 0)
			{
				Datablock datablock = (Datablock)this.refValue;
				Datablock datablock1 = datablock;
				if (!datablock)
				{
					str = "null";
					return str;
				}
				str = datablock1.name;
				return str;
			}
			str = "null";
			return str;
		}
	}
}