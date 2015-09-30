using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Helpers;
using RustProto.Proto;
using System;
using System.Diagnostics;
using System.IO;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class ItemMod : GeneratedMessage<RustProto.ItemMod, RustProto.ItemMod.Builder>
	{
		public const int IdFieldNumber = 1;

		public const int NameFieldNumber = 2;

		private readonly static RustProto.ItemMod defaultInstance;

		private readonly static string[] _itemModFieldNames;

		private readonly static uint[] _itemModFieldTags;

		private bool hasId;

		private int id_;

		private bool hasName;

		private string name_ = string.Empty;

		private int memoizedSerializedSize = -1;

		public static RustProto.ItemMod DefaultInstance
		{
			get
			{
				return RustProto.ItemMod.defaultInstance;
			}
		}

		public override RustProto.ItemMod DefaultInstanceForType
		{
			get
			{
				return RustProto.ItemMod.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.ItemMod.internal__static_RustProto_ItemMod__Descriptor;
			}
		}

		public bool HasId
		{
			get
			{
				return this.hasId;
			}
		}

		public bool HasName
		{
			get
			{
				return this.hasName;
			}
		}

		public int Id
		{
			get
			{
				return this.id_;
			}
		}

		protected override FieldAccessorTable<RustProto.ItemMod, RustProto.ItemMod.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.ItemMod.internal__static_RustProto_ItemMod__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				if (!this.hasId)
				{
					return false;
				}
				return true;
			}
		}

		public string Name
		{
			get
			{
				return this.name_;
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
				if (this.hasId)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(1, this.Id);
				}
				if (this.hasName)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeStringSize(2, this.Name);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override RustProto.ItemMod ThisMessage
		{
			get
			{
				return this;
			}
		}

		static ItemMod()
		{
			RustProto.ItemMod.defaultInstance = (new RustProto.ItemMod()).MakeReadOnly();
			RustProto.ItemMod._itemModFieldNames = new string[] { "id", "name" };
			RustProto.ItemMod._itemModFieldTags = new uint[] { 8, 18 };
			object.ReferenceEquals(RustProto.Proto.ItemMod.Descriptor, null);
		}

		private ItemMod()
		{
		}

		public static RustProto.ItemMod.Builder CreateBuilder()
		{
			return new RustProto.ItemMod.Builder();
		}

		public static RustProto.ItemMod.Builder CreateBuilder(RustProto.ItemMod prototype)
		{
			return new RustProto.ItemMod.Builder(prototype);
		}

		public override RustProto.ItemMod.Builder CreateBuilderForType()
		{
			return new RustProto.ItemMod.Builder();
		}

		private RustProto.ItemMod MakeReadOnly()
		{
			return this;
		}

		public static RustProto.ItemMod ParseDelimitedFrom(Stream input)
		{
			return RustProto.ItemMod.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static RustProto.ItemMod ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.ItemMod.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.ItemMod ParseFrom(ByteString data)
		{
			return RustProto.ItemMod.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.ItemMod ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.ItemMod.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.ItemMod ParseFrom(byte[] data)
		{
			return RustProto.ItemMod.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.ItemMod ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.ItemMod.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.ItemMod ParseFrom(Stream input)
		{
			return RustProto.ItemMod.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.ItemMod ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.ItemMod.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.ItemMod ParseFrom(ICodedInputStream input)
		{
			return RustProto.ItemMod.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.ItemMod ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.ItemMod.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<RustProto.ItemMod, RustProto.ItemMod.Builder> Recycler()
		{
			return Recycler<RustProto.ItemMod, RustProto.ItemMod.Builder>.Manufacture();
		}

		public override RustProto.ItemMod.Builder ToBuilder()
		{
			return RustProto.ItemMod.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = RustProto.ItemMod._itemModFieldNames;
			if (this.hasId)
			{
				output.WriteInt32(1, strArrays[0], this.Id);
			}
			if (this.hasName)
			{
				output.WriteString(2, strArrays[1], this.Name);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<RustProto.ItemMod, RustProto.ItemMod.Builder>
		{
			private bool resultIsReadOnly;

			private RustProto.ItemMod result;

			public override RustProto.ItemMod DefaultInstanceForType
			{
				get
				{
					return RustProto.ItemMod.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return RustProto.ItemMod.Descriptor;
				}
			}

			public bool HasId
			{
				get
				{
					return this.result.hasId;
				}
			}

			public bool HasName
			{
				get
				{
					return this.result.hasName;
				}
			}

			public int Id
			{
				get
				{
					return this.result.Id;
				}
				set
				{
					this.SetId(value);
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override RustProto.ItemMod MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public string Name
			{
				get
				{
					return this.result.Name;
				}
				set
				{
					this.SetName(value);
				}
			}

			protected override RustProto.ItemMod.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = RustProto.ItemMod.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(RustProto.ItemMod cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override RustProto.ItemMod BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override RustProto.ItemMod.Builder Clear()
			{
				this.result = RustProto.ItemMod.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public RustProto.ItemMod.Builder ClearId()
			{
				this.PrepareBuilder();
				this.result.hasId = false;
				this.result.id_ = 0;
				return this;
			}

			public RustProto.ItemMod.Builder ClearName()
			{
				this.PrepareBuilder();
				this.result.hasName = false;
				this.result.name_ = string.Empty;
				return this;
			}

			public override RustProto.ItemMod.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new RustProto.ItemMod.Builder(this.result);
				}
				return (new RustProto.ItemMod.Builder()).MergeFrom(this.result);
			}

			public override RustProto.ItemMod.Builder MergeFrom(IMessage other)
			{
				if (other is RustProto.ItemMod)
				{
					return this.MergeFrom((RustProto.ItemMod)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override RustProto.ItemMod.Builder MergeFrom(RustProto.ItemMod other)
			{
				if (other == RustProto.ItemMod.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasId)
				{
					this.Id = other.Id;
				}
				if (other.HasName)
				{
					this.Name = other.Name;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override RustProto.ItemMod.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override RustProto.ItemMod.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(RustProto.ItemMod._itemModFieldNames, str, StringComparer.Ordinal);
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
							num = RustProto.ItemMod._itemModFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasId = input.ReadInt32(ref this.result.id_);
					}
					else if (num2 == 18)
					{
						this.result.hasName = input.ReadString(ref this.result.name_);
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
					}
				}
				if (builder != null)
				{
					this.UnknownFields = builder.Build();
				}
				return this;
			}

			private RustProto.ItemMod PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					RustProto.ItemMod itemMod = this.result;
					this.result = new RustProto.ItemMod();
					this.resultIsReadOnly = false;
					this.MergeFrom(itemMod);
				}
				return this.result;
			}

			public RustProto.ItemMod.Builder SetId(int value)
			{
				this.PrepareBuilder();
				this.result.hasId = true;
				this.result.id_ = value;
				return this;
			}

			public RustProto.ItemMod.Builder SetName(string value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasName = true;
				this.result.name_ = value;
				return this;
			}
		}
	}
}