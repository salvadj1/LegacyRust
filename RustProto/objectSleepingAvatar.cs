using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Helpers;
using System;
using System.Diagnostics;
using System.IO;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class objectSleepingAvatar : GeneratedMessage<objectSleepingAvatar, objectSleepingAvatar.Builder>
	{
		public const int FootArmorFieldNumber = 1;

		public const int LegArmorFieldNumber = 2;

		public const int TorsoArmorFieldNumber = 3;

		public const int HeadArmorFieldNumber = 4;

		public const int TimestampFieldNumber = 5;

		public const int VitalsFieldNumber = 6;

		private readonly static objectSleepingAvatar defaultInstance;

		private readonly static string[] _objectSleepingAvatarFieldNames;

		private readonly static uint[] _objectSleepingAvatarFieldTags;

		private bool hasFootArmor;

		private int footArmor_;

		private bool hasLegArmor;

		private int legArmor_;

		private bool hasTorsoArmor;

		private int torsoArmor_;

		private bool hasHeadArmor;

		private int headArmor_;

		private bool hasTimestamp;

		private int timestamp_;

		private bool hasVitals;

		private RustProto.Vitals vitals_;

		private int memoizedSerializedSize = -1;

		public static objectSleepingAvatar DefaultInstance
		{
			get
			{
				return objectSleepingAvatar.defaultInstance;
			}
		}

		public override objectSleepingAvatar DefaultInstanceForType
		{
			get
			{
				return objectSleepingAvatar.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectSleepingAvatar__Descriptor;
			}
		}

		public int FootArmor
		{
			get
			{
				return this.footArmor_;
			}
		}

		public bool HasFootArmor
		{
			get
			{
				return this.hasFootArmor;
			}
		}

		public bool HasHeadArmor
		{
			get
			{
				return this.hasHeadArmor;
			}
		}

		public bool HasLegArmor
		{
			get
			{
				return this.hasLegArmor;
			}
		}

		public bool HasTimestamp
		{
			get
			{
				return this.hasTimestamp;
			}
		}

		public bool HasTorsoArmor
		{
			get
			{
				return this.hasTorsoArmor;
			}
		}

		public bool HasVitals
		{
			get
			{
				return this.hasVitals;
			}
		}

		public int HeadArmor
		{
			get
			{
				return this.headArmor_;
			}
		}

		protected override FieldAccessorTable<objectSleepingAvatar, objectSleepingAvatar.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectSleepingAvatar__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public int LegArmor
		{
			get
			{
				return this.legArmor_;
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
				if (this.hasFootArmor)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(1, this.FootArmor);
				}
				if (this.hasLegArmor)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(2, this.LegArmor);
				}
				if (this.hasTorsoArmor)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(3, this.TorsoArmor);
				}
				if (this.hasHeadArmor)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(4, this.HeadArmor);
				}
				if (this.hasTimestamp)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(5, this.Timestamp);
				}
				if (this.hasVitals)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(6, this.Vitals);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectSleepingAvatar ThisMessage
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

		public int TorsoArmor
		{
			get
			{
				return this.torsoArmor_;
			}
		}

		public RustProto.Vitals Vitals
		{
			get
			{
				return this.vitals_ ?? RustProto.Vitals.DefaultInstance;
			}
		}

		static objectSleepingAvatar()
		{
			objectSleepingAvatar.defaultInstance = (new objectSleepingAvatar()).MakeReadOnly();
			objectSleepingAvatar._objectSleepingAvatarFieldNames = new string[] { "footArmor", "headArmor", "legArmor", "timestamp", "torsoArmor", "vitals" };
			objectSleepingAvatar._objectSleepingAvatarFieldTags = new uint[] { 8, 32, 16, 40, 24, 50 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectSleepingAvatar()
		{
		}

		public static objectSleepingAvatar.Builder CreateBuilder()
		{
			return new objectSleepingAvatar.Builder();
		}

		public static objectSleepingAvatar.Builder CreateBuilder(objectSleepingAvatar prototype)
		{
			return new objectSleepingAvatar.Builder(prototype);
		}

		public override objectSleepingAvatar.Builder CreateBuilderForType()
		{
			return new objectSleepingAvatar.Builder();
		}

		private objectSleepingAvatar MakeReadOnly()
		{
			return this;
		}

		public static objectSleepingAvatar ParseDelimitedFrom(Stream input)
		{
			return objectSleepingAvatar.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectSleepingAvatar ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectSleepingAvatar.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectSleepingAvatar ParseFrom(ByteString data)
		{
			return objectSleepingAvatar.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectSleepingAvatar ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectSleepingAvatar.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectSleepingAvatar ParseFrom(byte[] data)
		{
			return objectSleepingAvatar.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectSleepingAvatar ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectSleepingAvatar.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectSleepingAvatar ParseFrom(Stream input)
		{
			return objectSleepingAvatar.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectSleepingAvatar ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectSleepingAvatar.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectSleepingAvatar ParseFrom(ICodedInputStream input)
		{
			return objectSleepingAvatar.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectSleepingAvatar ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectSleepingAvatar.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectSleepingAvatar, objectSleepingAvatar.Builder> Recycler()
		{
			return Recycler<objectSleepingAvatar, objectSleepingAvatar.Builder>.Manufacture();
		}

		public override objectSleepingAvatar.Builder ToBuilder()
		{
			return objectSleepingAvatar.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectSleepingAvatar._objectSleepingAvatarFieldNames;
			if (this.hasFootArmor)
			{
				output.WriteInt32(1, strArrays[0], this.FootArmor);
			}
			if (this.hasLegArmor)
			{
				output.WriteInt32(2, strArrays[2], this.LegArmor);
			}
			if (this.hasTorsoArmor)
			{
				output.WriteInt32(3, strArrays[4], this.TorsoArmor);
			}
			if (this.hasHeadArmor)
			{
				output.WriteInt32(4, strArrays[1], this.HeadArmor);
			}
			if (this.hasTimestamp)
			{
				output.WriteInt32(5, strArrays[3], this.Timestamp);
			}
			if (this.hasVitals)
			{
				output.WriteMessage(6, strArrays[5], this.Vitals);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectSleepingAvatar, objectSleepingAvatar.Builder>
		{
			private bool resultIsReadOnly;

			private objectSleepingAvatar result;

			public override objectSleepingAvatar DefaultInstanceForType
			{
				get
				{
					return objectSleepingAvatar.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectSleepingAvatar.Descriptor;
				}
			}

			public int FootArmor
			{
				get
				{
					return this.result.FootArmor;
				}
				set
				{
					this.SetFootArmor(value);
				}
			}

			public bool HasFootArmor
			{
				get
				{
					return this.result.hasFootArmor;
				}
			}

			public bool HasHeadArmor
			{
				get
				{
					return this.result.hasHeadArmor;
				}
			}

			public bool HasLegArmor
			{
				get
				{
					return this.result.hasLegArmor;
				}
			}

			public bool HasTimestamp
			{
				get
				{
					return this.result.hasTimestamp;
				}
			}

			public bool HasTorsoArmor
			{
				get
				{
					return this.result.hasTorsoArmor;
				}
			}

			public bool HasVitals
			{
				get
				{
					return this.result.hasVitals;
				}
			}

			public int HeadArmor
			{
				get
				{
					return this.result.HeadArmor;
				}
				set
				{
					this.SetHeadArmor(value);
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			public int LegArmor
			{
				get
				{
					return this.result.LegArmor;
				}
				set
				{
					this.SetLegArmor(value);
				}
			}

			protected override objectSleepingAvatar MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override objectSleepingAvatar.Builder ThisBuilder
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

			public int TorsoArmor
			{
				get
				{
					return this.result.TorsoArmor;
				}
				set
				{
					this.SetTorsoArmor(value);
				}
			}

			public RustProto.Vitals Vitals
			{
				get
				{
					return this.result.Vitals;
				}
				set
				{
					this.SetVitals(value);
				}
			}

			public Builder()
			{
				this.result = objectSleepingAvatar.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectSleepingAvatar cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectSleepingAvatar BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectSleepingAvatar.Builder Clear()
			{
				this.result = objectSleepingAvatar.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectSleepingAvatar.Builder ClearFootArmor()
			{
				this.PrepareBuilder();
				this.result.hasFootArmor = false;
				this.result.footArmor_ = 0;
				return this;
			}

			public objectSleepingAvatar.Builder ClearHeadArmor()
			{
				this.PrepareBuilder();
				this.result.hasHeadArmor = false;
				this.result.headArmor_ = 0;
				return this;
			}

			public objectSleepingAvatar.Builder ClearLegArmor()
			{
				this.PrepareBuilder();
				this.result.hasLegArmor = false;
				this.result.legArmor_ = 0;
				return this;
			}

			public objectSleepingAvatar.Builder ClearTimestamp()
			{
				this.PrepareBuilder();
				this.result.hasTimestamp = false;
				this.result.timestamp_ = 0;
				return this;
			}

			public objectSleepingAvatar.Builder ClearTorsoArmor()
			{
				this.PrepareBuilder();
				this.result.hasTorsoArmor = false;
				this.result.torsoArmor_ = 0;
				return this;
			}

			public objectSleepingAvatar.Builder ClearVitals()
			{
				this.PrepareBuilder();
				this.result.hasVitals = false;
				this.result.vitals_ = null;
				return this;
			}

			public override objectSleepingAvatar.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectSleepingAvatar.Builder(this.result);
				}
				return (new objectSleepingAvatar.Builder()).MergeFrom(this.result);
			}

			public override objectSleepingAvatar.Builder MergeFrom(IMessage other)
			{
				if (other is objectSleepingAvatar)
				{
					return this.MergeFrom((objectSleepingAvatar)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectSleepingAvatar.Builder MergeFrom(objectSleepingAvatar other)
			{
				if (other == objectSleepingAvatar.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasFootArmor)
				{
					this.FootArmor = other.FootArmor;
				}
				if (other.HasLegArmor)
				{
					this.LegArmor = other.LegArmor;
				}
				if (other.HasTorsoArmor)
				{
					this.TorsoArmor = other.TorsoArmor;
				}
				if (other.HasHeadArmor)
				{
					this.HeadArmor = other.HeadArmor;
				}
				if (other.HasTimestamp)
				{
					this.Timestamp = other.Timestamp;
				}
				if (other.HasVitals)
				{
					this.MergeVitals(other.Vitals);
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectSleepingAvatar.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectSleepingAvatar.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectSleepingAvatar._objectSleepingAvatarFieldNames, str, StringComparer.Ordinal);
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
							num = objectSleepingAvatar._objectSleepingAvatarFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasFootArmor = input.ReadInt32(ref this.result.footArmor_);
					}
					else if (num2 == 16)
					{
						this.result.hasLegArmor = input.ReadInt32(ref this.result.legArmor_);
					}
					else if (num2 == 24)
					{
						this.result.hasTorsoArmor = input.ReadInt32(ref this.result.torsoArmor_);
					}
					else if (num2 == 32)
					{
						this.result.hasHeadArmor = input.ReadInt32(ref this.result.headArmor_);
					}
					else if (num2 == 40)
					{
						this.result.hasTimestamp = input.ReadInt32(ref this.result.timestamp_);
					}
					else if (num2 == 50)
					{
						RustProto.Vitals.Builder builder1 = RustProto.Vitals.CreateBuilder();
						if (this.result.hasVitals)
						{
							builder1.MergeFrom(this.Vitals);
						}
						input.ReadMessage(builder1, extensionRegistry);
						this.Vitals = builder1.BuildPartial();
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

			public objectSleepingAvatar.Builder MergeVitals(RustProto.Vitals value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasVitals || this.result.vitals_ == RustProto.Vitals.DefaultInstance)
				{
					this.result.vitals_ = value;
				}
				else
				{
					this.result.vitals_ = RustProto.Vitals.CreateBuilder(this.result.vitals_).MergeFrom(value).BuildPartial();
				}
				this.result.hasVitals = true;
				return this;
			}

			private objectSleepingAvatar PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectSleepingAvatar _objectSleepingAvatar = this.result;
					this.result = new objectSleepingAvatar();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectSleepingAvatar);
				}
				return this.result;
			}

			public objectSleepingAvatar.Builder SetFootArmor(int value)
			{
				this.PrepareBuilder();
				this.result.hasFootArmor = true;
				this.result.footArmor_ = value;
				return this;
			}

			public objectSleepingAvatar.Builder SetHeadArmor(int value)
			{
				this.PrepareBuilder();
				this.result.hasHeadArmor = true;
				this.result.headArmor_ = value;
				return this;
			}

			public objectSleepingAvatar.Builder SetLegArmor(int value)
			{
				this.PrepareBuilder();
				this.result.hasLegArmor = true;
				this.result.legArmor_ = value;
				return this;
			}

			public objectSleepingAvatar.Builder SetTimestamp(int value)
			{
				this.PrepareBuilder();
				this.result.hasTimestamp = true;
				this.result.timestamp_ = value;
				return this;
			}

			public objectSleepingAvatar.Builder SetTorsoArmor(int value)
			{
				this.PrepareBuilder();
				this.result.hasTorsoArmor = true;
				this.result.torsoArmor_ = value;
				return this;
			}

			public objectSleepingAvatar.Builder SetVitals(RustProto.Vitals value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasVitals = true;
				this.result.vitals_ = value;
				return this;
			}

			public objectSleepingAvatar.Builder SetVitals(RustProto.Vitals.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasVitals = true;
				this.result.vitals_ = builderForValue.Build();
				return this;
			}
		}
	}
}