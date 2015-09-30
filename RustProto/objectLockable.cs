using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Collections;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class objectLockable : GeneratedMessage<objectLockable, objectLockable.Builder>
	{
		public const int PasswordFieldNumber = 1;

		public const int LockedFieldNumber = 2;

		public const int UsersFieldNumber = 3;

		private readonly static objectLockable defaultInstance;

		private readonly static string[] _objectLockableFieldNames;

		private readonly static uint[] _objectLockableFieldTags;

		private bool hasPassword;

		private string password_ = string.Empty;

		private bool hasLocked;

		private bool locked_;

		private PopsicleList<ulong> users_ = new PopsicleList<ulong>();

		private int memoizedSerializedSize = -1;

		public static objectLockable DefaultInstance
		{
			get
			{
				return objectLockable.defaultInstance;
			}
		}

		public override objectLockable DefaultInstanceForType
		{
			get
			{
				return objectLockable.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectLockable__Descriptor;
			}
		}

		public bool HasLocked
		{
			get
			{
				return this.hasLocked;
			}
		}

		public bool HasPassword
		{
			get
			{
				return this.hasPassword;
			}
		}

		protected override FieldAccessorTable<objectLockable, objectLockable.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectLockable__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public bool Locked
		{
			get
			{
				return this.locked_;
			}
		}

		public string Password
		{
			get
			{
				return this.password_;
			}
		}

		public override int SerializedSize
		{
			get
			{
				int count = this.memoizedSerializedSize;
				if (count != -1)
				{
					return count;
				}
				count = 0;
				if (this.hasPassword)
				{
					count = count + CodedOutputStream.ComputeStringSize(1, this.Password);
				}
				if (this.hasLocked)
				{
					count = count + CodedOutputStream.ComputeBoolSize(2, this.Locked);
				}
				int num = 0;
				IEnumerator<ulong> enumerator = this.UsersList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						num = num + CodedOutputStream.ComputeUInt64SizeNoTag(enumerator.Current);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				count = count + num;
				count = count + 1 * this.users_.Count;
				count = count + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = count;
				return count;
			}
		}

		protected override objectLockable ThisMessage
		{
			get
			{
				return this;
			}
		}

		public int UsersCount
		{
			get
			{
				return this.users_.Count;
			}
		}

		[CLSCompliant(false)]
		public IList<ulong> UsersList
		{
			get
			{
				return Lists.AsReadOnly<ulong>(this.users_);
			}
		}

		static objectLockable()
		{
			objectLockable.defaultInstance = (new objectLockable()).MakeReadOnly();
			objectLockable._objectLockableFieldNames = new string[] { "locked", "password", "users" };
			objectLockable._objectLockableFieldTags = new uint[] { 16, 10, 24 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectLockable()
		{
		}

		public static objectLockable.Builder CreateBuilder()
		{
			return new objectLockable.Builder();
		}

		public static objectLockable.Builder CreateBuilder(objectLockable prototype)
		{
			return new objectLockable.Builder(prototype);
		}

		public override objectLockable.Builder CreateBuilderForType()
		{
			return new objectLockable.Builder();
		}

		[CLSCompliant(false)]
		public ulong GetUsers(int index)
		{
			return this.users_[index];
		}

		private objectLockable MakeReadOnly()
		{
			this.users_.MakeReadOnly();
			return this;
		}

		public static objectLockable ParseDelimitedFrom(Stream input)
		{
			return objectLockable.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectLockable ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectLockable.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectLockable ParseFrom(ByteString data)
		{
			return objectLockable.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectLockable ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectLockable.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectLockable ParseFrom(byte[] data)
		{
			return objectLockable.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectLockable ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectLockable.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectLockable ParseFrom(Stream input)
		{
			return objectLockable.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectLockable ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectLockable.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectLockable ParseFrom(ICodedInputStream input)
		{
			return objectLockable.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectLockable ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectLockable.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectLockable, objectLockable.Builder> Recycler()
		{
			return Recycler<objectLockable, objectLockable.Builder>.Manufacture();
		}

		public override objectLockable.Builder ToBuilder()
		{
			return objectLockable.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectLockable._objectLockableFieldNames;
			if (this.hasPassword)
			{
				output.WriteString(1, strArrays[1], this.Password);
			}
			if (this.hasLocked)
			{
				output.WriteBool(2, strArrays[0], this.Locked);
			}
			if (this.users_.Count > 0)
			{
				output.WriteUInt64Array(3, strArrays[2], this.users_);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectLockable, objectLockable.Builder>
		{
			private bool resultIsReadOnly;

			private objectLockable result;

			public override objectLockable DefaultInstanceForType
			{
				get
				{
					return objectLockable.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectLockable.Descriptor;
				}
			}

			public bool HasLocked
			{
				get
				{
					return this.result.hasLocked;
				}
			}

			public bool HasPassword
			{
				get
				{
					return this.result.hasPassword;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			public bool Locked
			{
				get
				{
					return this.result.Locked;
				}
				set
				{
					this.SetLocked(value);
				}
			}

			protected override objectLockable MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public string Password
			{
				get
				{
					return this.result.Password;
				}
				set
				{
					this.SetPassword(value);
				}
			}

			protected override objectLockable.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public int UsersCount
			{
				get
				{
					return this.result.UsersCount;
				}
			}

			[CLSCompliant(false)]
			public IPopsicleList<ulong> UsersList
			{
				get
				{
					return this.PrepareBuilder().users_;
				}
			}

			public Builder()
			{
				this.result = objectLockable.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectLockable cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			[CLSCompliant(false)]
			public objectLockable.Builder AddRangeUsers(IEnumerable<ulong> values)
			{
				this.PrepareBuilder();
				this.result.users_.Add(values);
				return this;
			}

			[CLSCompliant(false)]
			public objectLockable.Builder AddUsers(ulong value)
			{
				this.PrepareBuilder();
				this.result.users_.Add(value);
				return this;
			}

			public override objectLockable BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectLockable.Builder Clear()
			{
				this.result = objectLockable.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectLockable.Builder ClearLocked()
			{
				this.PrepareBuilder();
				this.result.hasLocked = false;
				this.result.locked_ = false;
				return this;
			}

			public objectLockable.Builder ClearPassword()
			{
				this.PrepareBuilder();
				this.result.hasPassword = false;
				this.result.password_ = string.Empty;
				return this;
			}

			public objectLockable.Builder ClearUsers()
			{
				this.PrepareBuilder();
				this.result.users_.Clear();
				return this;
			}

			public override objectLockable.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectLockable.Builder(this.result);
				}
				return (new objectLockable.Builder()).MergeFrom(this.result);
			}

			[CLSCompliant(false)]
			public ulong GetUsers(int index)
			{
				return this.result.GetUsers(index);
			}

			public override objectLockable.Builder MergeFrom(IMessage other)
			{
				if (other is objectLockable)
				{
					return this.MergeFrom((objectLockable)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectLockable.Builder MergeFrom(objectLockable other)
			{
				if (other == objectLockable.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasPassword)
				{
					this.Password = other.Password;
				}
				if (other.HasLocked)
				{
					this.Locked = other.Locked;
				}
				if (other.users_.Count != 0)
				{
					this.result.users_.Add(other.users_);
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectLockable.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectLockable.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				uint num1;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
			Label1:
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num2 = Array.BinarySearch<string>(objectLockable._objectLockableFieldNames, str, StringComparer.Ordinal);
						if (num2 < 0)
						{
							if (builder == null)
							{
								builder = UnknownFieldSet.CreateBuilder(this.UnknownFields);
							}
							this.ParseUnknownField(input, builder, extensionRegistry, num, str);
							continue;
						}
						else
						{
							num = objectLockable._objectLockableFieldTags[num2];
						}
					}
					num1 = num;
					switch (num1)
					{
						case 24:
						case 26:
						{
							input.ReadUInt64Array(num, str, this.result.users_);
							continue;
						}
						default:
						{
							if (num1 == 0)
							{
								break;
							}
							else
							{
								goto Label0;
							}
						}
					}
					throw InvalidProtocolBufferException.InvalidTag();
				}
				if (builder != null)
				{
					this.UnknownFields = builder.Build();
				}
				return this;
				if (num1 == 10)
				{
					this.result.hasPassword = input.ReadString(ref this.result.password_);
					goto Label1;
				}
				else if (num1 == 16)
				{
					this.result.hasLocked = input.ReadBool(ref this.result.locked_);
					goto Label1;
				}
				else
				{
					if (WireFormat.IsEndGroupTag(num))
					{
						if (builder != null)
						{
							this.UnknownFields = builder.Build();
						}
						return this;
					}
					if (builder == null)
					{
						builder = UnknownFieldSet.CreateBuilder(this.UnknownFields);
					}
					this.ParseUnknownField(input, builder, extensionRegistry, num, str);
					goto Label1;
				}
			}

			private objectLockable PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectLockable _objectLockable = this.result;
					this.result = new objectLockable();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectLockable);
				}
				return this.result;
			}

			public objectLockable.Builder SetLocked(bool value)
			{
				this.PrepareBuilder();
				this.result.hasLocked = true;
				this.result.locked_ = value;
				return this;
			}

			public objectLockable.Builder SetPassword(string value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasPassword = true;
				this.result.password_ = value;
				return this;
			}

			[CLSCompliant(false)]
			public objectLockable.Builder SetUsers(int index, ulong value)
			{
				this.PrepareBuilder();
				this.result.users_[index] = value;
				return this;
			}
		}
	}
}