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
	public sealed class AwayEvent : GeneratedMessage<AwayEvent, AwayEvent.Builder>
	{
		public const int TypeFieldNumber = 1;

		public const int TimestampFieldNumber = 2;

		public const int InstigatorFieldNumber = 3;

		private readonly static AwayEvent defaultInstance;

		private readonly static string[] _awayEventFieldNames;

		private readonly static uint[] _awayEventFieldTags;

		private bool hasType;

		private AwayEvent.Types.AwayEventType type_;

		private bool hasTimestamp;

		private int timestamp_;

		private bool hasInstigator;

		private ulong instigator_;

		private int memoizedSerializedSize = -1;

		public static AwayEvent DefaultInstance
		{
			get
			{
				return AwayEvent.defaultInstance;
			}
		}

		public override AwayEvent DefaultInstanceForType
		{
			get
			{
				return AwayEvent.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Avatar.internal__static_RustProto_AwayEvent__Descriptor;
			}
		}

		public bool HasInstigator
		{
			get
			{
				return this.hasInstigator;
			}
		}

		public bool HasTimestamp
		{
			get
			{
				return this.hasTimestamp;
			}
		}

		public bool HasType
		{
			get
			{
				return this.hasType;
			}
		}

		[CLSCompliant(false)]
		public ulong Instigator
		{
			get
			{
				return this.instigator_;
			}
		}

		protected override FieldAccessorTable<AwayEvent, AwayEvent.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.Avatar.internal__static_RustProto_AwayEvent__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				if (!this.hasType)
				{
					return false;
				}
				if (!this.hasTimestamp)
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
				if (this.hasType)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeEnumSize(1, (int)this.Type);
				}
				if (this.hasTimestamp)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(2, this.Timestamp);
				}
				if (this.hasInstigator)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeUInt64Size(3, this.Instigator);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override AwayEvent ThisMessage
		{
			get
			{
				return this;
			}
		}

		public int Timestamp
		{
			get
			{
				return this.timestamp_;
			}
		}

		public AwayEvent.Types.AwayEventType Type
		{
			get
			{
				return this.type_;
			}
		}

		static AwayEvent()
		{
			AwayEvent.defaultInstance = (new AwayEvent()).MakeReadOnly();
			AwayEvent._awayEventFieldNames = new string[] { "instigator", "timestamp", "type" };
			AwayEvent._awayEventFieldTags = new uint[] { 24, 16, 8 };
			object.ReferenceEquals(RustProto.Proto.Avatar.Descriptor, null);
		}

		private AwayEvent()
		{
		}

		public static AwayEvent.Builder CreateBuilder()
		{
			return new AwayEvent.Builder();
		}

		public static AwayEvent.Builder CreateBuilder(AwayEvent prototype)
		{
			return new AwayEvent.Builder(prototype);
		}

		public override AwayEvent.Builder CreateBuilderForType()
		{
			return new AwayEvent.Builder();
		}

		private AwayEvent MakeReadOnly()
		{
			return this;
		}

		public static AwayEvent ParseDelimitedFrom(Stream input)
		{
			return AwayEvent.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static AwayEvent ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return AwayEvent.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static AwayEvent ParseFrom(ByteString data)
		{
			return AwayEvent.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static AwayEvent ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return AwayEvent.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static AwayEvent ParseFrom(byte[] data)
		{
			return AwayEvent.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static AwayEvent ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return AwayEvent.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static AwayEvent ParseFrom(Stream input)
		{
			return AwayEvent.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static AwayEvent ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return AwayEvent.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static AwayEvent ParseFrom(ICodedInputStream input)
		{
			return AwayEvent.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static AwayEvent ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return AwayEvent.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<AwayEvent, AwayEvent.Builder> Recycler()
		{
			return Recycler<AwayEvent, AwayEvent.Builder>.Manufacture();
		}

		public override AwayEvent.Builder ToBuilder()
		{
			return AwayEvent.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = AwayEvent._awayEventFieldNames;
			if (this.hasType)
			{
				output.WriteEnum(1, strArrays[2], (int)this.Type, this.Type);
			}
			if (this.hasTimestamp)
			{
				output.WriteInt32(2, strArrays[1], this.Timestamp);
			}
			if (this.hasInstigator)
			{
				output.WriteUInt64(3, strArrays[0], this.Instigator);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<AwayEvent, AwayEvent.Builder>
		{
			private bool resultIsReadOnly;

			private AwayEvent result;

			public override AwayEvent DefaultInstanceForType
			{
				get
				{
					return AwayEvent.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return AwayEvent.Descriptor;
				}
			}

			public bool HasInstigator
			{
				get
				{
					return this.result.hasInstigator;
				}
			}

			public bool HasTimestamp
			{
				get
				{
					return this.result.hasTimestamp;
				}
			}

			public bool HasType
			{
				get
				{
					return this.result.hasType;
				}
			}

			[CLSCompliant(false)]
			public ulong Instigator
			{
				get
				{
					return this.result.Instigator;
				}
				set
				{
					this.SetInstigator(value);
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override AwayEvent MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override AwayEvent.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public int Timestamp
			{
				get
				{
					return this.result.Timestamp;
				}
				set
				{
					this.SetTimestamp(value);
				}
			}

			public AwayEvent.Types.AwayEventType Type
			{
				get
				{
					return this.result.Type;
				}
				set
				{
					this.SetType(value);
				}
			}

			public Builder()
			{
				this.result = AwayEvent.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(AwayEvent cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override AwayEvent BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override AwayEvent.Builder Clear()
			{
				this.result = AwayEvent.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public AwayEvent.Builder ClearInstigator()
			{
				this.PrepareBuilder();
				this.result.hasInstigator = false;
				this.result.instigator_ = (ulong)0;
				return this;
			}

			public AwayEvent.Builder ClearTimestamp()
			{
				this.PrepareBuilder();
				this.result.hasTimestamp = false;
				this.result.timestamp_ = 0;
				return this;
			}

			public AwayEvent.Builder ClearType()
			{
				this.PrepareBuilder();
				this.result.hasType = false;
				this.result.type_ = AwayEvent.Types.AwayEventType.UNKNOWN;
				return this;
			}

			public override AwayEvent.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new AwayEvent.Builder(this.result);
				}
				return (new AwayEvent.Builder()).MergeFrom(this.result);
			}

			public override AwayEvent.Builder MergeFrom(IMessage other)
			{
				if (other is AwayEvent)
				{
					return this.MergeFrom((AwayEvent)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override AwayEvent.Builder MergeFrom(AwayEvent other)
			{
				if (other == AwayEvent.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasType)
				{
					this.Type = other.Type;
				}
				if (other.HasTimestamp)
				{
					this.Timestamp = other.Timestamp;
				}
				if (other.HasInstigator)
				{
					this.Instigator = other.Instigator;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override AwayEvent.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override AwayEvent.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
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
						int num1 = Array.BinarySearch<string>(AwayEvent._awayEventFieldNames, str, StringComparer.Ordinal);
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
							num = AwayEvent._awayEventFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 != 8)
					{
						if (num2 == 16)
						{
							this.result.hasTimestamp = input.ReadInt32(ref this.result.timestamp_);
						}
						else if (num2 == 24)
						{
							this.result.hasInstigator = input.ReadUInt64(ref this.result.instigator_);
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
					else if (input.ReadEnum<AwayEvent.Types.AwayEventType>(ref this.result.type_, out obj))
					{
						this.result.hasType = true;
					}
					else if (obj is int)
					{
						if (builder == null)
						{
							builder = UnknownFieldSet.CreateBuilder(this.UnknownFields);
						}
						builder.MergeVarintField(1, (ulong)((int)obj));
					}
				}
				if (builder != null)
				{
					this.UnknownFields = builder.Build();
				}
				return this;
			}

			private AwayEvent PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					AwayEvent awayEvent = this.result;
					this.result = new AwayEvent();
					this.resultIsReadOnly = false;
					this.MergeFrom(awayEvent);
				}
				return this.result;
			}

			[CLSCompliant(false)]
			public AwayEvent.Builder SetInstigator(ulong value)
			{
				this.PrepareBuilder();
				this.result.hasInstigator = true;
				this.result.instigator_ = value;
				return this;
			}

			public AwayEvent.Builder SetTimestamp(int value)
			{
				this.PrepareBuilder();
				this.result.hasTimestamp = true;
				this.result.timestamp_ = value;
				return this;
			}

			public AwayEvent.Builder SetType(AwayEvent.Types.AwayEventType value)
			{
				this.PrepareBuilder();
				this.result.hasType = true;
				this.result.type_ = value;
				return this;
			}
		}

		[DebuggerNonUserCode]
		public static class Types
		{
			public enum AwayEventType
			{
				UNKNOWN,
				SLUMBER,
				DIED
			}
		}
	}
}