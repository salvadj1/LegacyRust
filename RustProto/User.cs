using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Proto;
using System;
using System.Diagnostics;
using System.IO;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class User : GeneratedMessage<RustProto.User, RustProto.User.Builder>
	{
		public const int UseridFieldNumber = 1;

		public const int DisplaynameFieldNumber = 2;

		public const int UsergroupFieldNumber = 3;

		private readonly static RustProto.User defaultInstance;

		private readonly static string[] _userFieldNames;

		private readonly static uint[] _userFieldTags;

		private bool hasUserid;

		private ulong userid_;

		private bool hasDisplayname;

		private string displayname_ = string.Empty;

		private bool hasUsergroup;

		private RustProto.User.Types.UserGroup usergroup_;

		private int memoizedSerializedSize = -1;

		public static RustProto.User DefaultInstance
		{
			get
			{
				return RustProto.User.defaultInstance;
			}
		}

		public override RustProto.User DefaultInstanceForType
		{
			get
			{
				return RustProto.User.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.User.internal__static_RustProto_User__Descriptor;
			}
		}

		public string Displayname
		{
			get
			{
				return this.displayname_;
			}
		}

		public bool HasDisplayname
		{
			get
			{
				return this.hasDisplayname;
			}
		}

		public bool HasUsergroup
		{
			get
			{
				return this.hasUsergroup;
			}
		}

		public bool HasUserid
		{
			get
			{
				return this.hasUserid;
			}
		}

		protected override FieldAccessorTable<RustProto.User, RustProto.User.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.User.internal__static_RustProto_User__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				if (!this.hasUserid)
				{
					return false;
				}
				if (!this.hasDisplayname)
				{
					return false;
				}
				if (!this.hasUsergroup)
				{
					return false;
				}
				return true;
			}
		}

		public override int SerializedSize
		{
			get
			{
				int serializedSize = this.memoizedSerializedSize;
				if (serializedSize != -1)
				{
					return serializedSize;
				}
				serializedSize = 0;
				if (this.hasUserid)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeUInt64Size(1, this.Userid);
				}
				if (this.hasDisplayname)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeStringSize(2, this.Displayname);
				}
				if (this.hasUsergroup)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeEnumSize(3, (int)this.Usergroup);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override RustProto.User ThisMessage
		{
			get
			{
				return this;
			}
		}

		public RustProto.User.Types.UserGroup Usergroup
		{
			get
			{
				return this.usergroup_;
			}
		}

		[CLSCompliant(false)]
		public ulong Userid
		{
			get
			{
				return this.userid_;
			}
		}

		static User()
		{
			RustProto.User.defaultInstance = (new RustProto.User()).MakeReadOnly();
			RustProto.User._userFieldNames = new string[] { "displayname", "usergroup", "userid" };
			RustProto.User._userFieldTags = new uint[] { 18, 24, 8 };
			object.ReferenceEquals(RustProto.Proto.User.Descriptor, null);
		}

		private User()
		{
		}

		public static RustProto.User.Builder CreateBuilder()
		{
			return new RustProto.User.Builder();
		}

		public static RustProto.User.Builder CreateBuilder(RustProto.User prototype)
		{
			return new RustProto.User.Builder(prototype);
		}

		public override RustProto.User.Builder CreateBuilderForType()
		{
			return new RustProto.User.Builder();
		}

		private RustProto.User MakeReadOnly()
		{
			return this;
		}

		public static RustProto.User ParseDelimitedFrom(Stream input)
		{
			return RustProto.User.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static RustProto.User ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.User.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.User ParseFrom(ByteString data)
		{
			return RustProto.User.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.User ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.User.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.User ParseFrom(byte[] data)
		{
			return RustProto.User.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.User ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.User.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.User ParseFrom(Stream input)
		{
			return RustProto.User.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.User ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.User.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.User ParseFrom(ICodedInputStream input)
		{
			return RustProto.User.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.User ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.User.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public override RustProto.User.Builder ToBuilder()
		{
			return RustProto.User.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = RustProto.User._userFieldNames;
			if (this.hasUserid)
			{
				output.WriteUInt64(1, strArrays[2], this.Userid);
			}
			if (this.hasDisplayname)
			{
				output.WriteString(2, strArrays[0], this.Displayname);
			}
			if (this.hasUsergroup)
			{
				output.WriteEnum(3, strArrays[1], (int)this.Usergroup, this.Usergroup);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<RustProto.User, RustProto.User.Builder>
		{
			private bool resultIsReadOnly;

			private RustProto.User result;

			public override RustProto.User DefaultInstanceForType
			{
				get
				{
					return RustProto.User.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return RustProto.User.Descriptor;
				}
			}

			public string Displayname
			{
				get
				{
					return this.result.Displayname;
				}
				set
				{
					this.SetDisplayname(value);
				}
			}

			public bool HasDisplayname
			{
				get
				{
					return this.result.hasDisplayname;
				}
			}

			public bool HasUsergroup
			{
				get
				{
					return this.result.hasUsergroup;
				}
			}

			public bool HasUserid
			{
				get
				{
					return this.result.hasUserid;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override RustProto.User MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override RustProto.User.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public RustProto.User.Types.UserGroup Usergroup
			{
				get
				{
					return this.result.Usergroup;
				}
				set
				{
					this.SetUsergroup(value);
				}
			}

			[CLSCompliant(false)]
			public ulong Userid
			{
				get
				{
					return this.result.Userid;
				}
				set
				{
					this.SetUserid(value);
				}
			}

			public Builder()
			{
				this.result = RustProto.User.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(RustProto.User cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override RustProto.User BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override RustProto.User.Builder Clear()
			{
				this.result = RustProto.User.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public RustProto.User.Builder ClearDisplayname()
			{
				this.PrepareBuilder();
				this.result.hasDisplayname = false;
				this.result.displayname_ = string.Empty;
				return this;
			}

			public RustProto.User.Builder ClearUsergroup()
			{
				this.PrepareBuilder();
				this.result.hasUsergroup = false;
				this.result.usergroup_ = RustProto.User.Types.UserGroup.REGULAR;
				return this;
			}

			public RustProto.User.Builder ClearUserid()
			{
				this.PrepareBuilder();
				this.result.hasUserid = false;
				this.result.userid_ = (ulong)0;
				return this;
			}

			public override RustProto.User.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new RustProto.User.Builder(this.result);
				}
				return (new RustProto.User.Builder()).MergeFrom(this.result);
			}

			public override RustProto.User.Builder MergeFrom(IMessage other)
			{
				if (other is RustProto.User)
				{
					return this.MergeFrom((RustProto.User)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override RustProto.User.Builder MergeFrom(RustProto.User other)
			{
				if (other == RustProto.User.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasUserid)
				{
					this.Userid = other.Userid;
				}
				if (other.HasDisplayname)
				{
					this.Displayname = other.Displayname;
				}
				if (other.HasUsergroup)
				{
					this.Usergroup = other.Usergroup;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override RustProto.User.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override RustProto.User.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				object obj;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(RustProto.User._userFieldNames, str, StringComparer.Ordinal);
						if (num1 < 0)
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
							num = RustProto.User._userFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasUserid = input.ReadUInt64(ref this.result.userid_);
					}
					else if (num2 == 18)
					{
						this.result.hasDisplayname = input.ReadString(ref this.result.displayname_);
					}
					else if (num2 != 24)
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
					}
					else if (input.ReadEnum<RustProto.User.Types.UserGroup>(ref this.result.usergroup_, out obj))
					{
						this.result.hasUsergroup = true;
					}
					else if (obj is int)
					{
						if (builder == null)
						{
							builder = UnknownFieldSet.CreateBuilder(this.UnknownFields);
						}
						builder.MergeVarintField(3, (ulong)((int)obj));
					}
				}
				if (builder != null)
				{
					this.UnknownFields = builder.Build();
				}
				return this;
			}

			private RustProto.User PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					RustProto.User user = this.result;
					this.result = new RustProto.User();
					this.resultIsReadOnly = false;
					this.MergeFrom(user);
				}
				return this.result;
			}

			public RustProto.User.Builder SetDisplayname(string value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasDisplayname = true;
				this.result.displayname_ = value;
				return this;
			}

			public RustProto.User.Builder SetUsergroup(RustProto.User.Types.UserGroup value)
			{
				this.PrepareBuilder();
				this.result.hasUsergroup = true;
				this.result.usergroup_ = value;
				return this;
			}

			[CLSCompliant(false)]
			public RustProto.User.Builder SetUserid(ulong value)
			{
				this.PrepareBuilder();
				this.result.hasUserid = true;
				this.result.userid_ = value;
				return this;
			}
		}

		[DebuggerNonUserCode]
		public static class Types
		{
			public enum UserGroup
			{
				REGULAR,
				BANNED,
				ADMIN
			}
		}
	}
}