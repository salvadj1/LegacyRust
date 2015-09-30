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
	public sealed class objectStructMaster : GeneratedMessage<objectStructMaster, objectStructMaster.Builder>
	{
		public const int IDFieldNumber = 1;

		public const int DecayDelayFieldNumber = 2;

		public const int CreatorIDFieldNumber = 3;

		public const int OwnerIDFieldNumber = 4;

		private readonly static objectStructMaster defaultInstance;

		private readonly static string[] _objectStructMasterFieldNames;

		private readonly static uint[] _objectStructMasterFieldTags;

		private bool hasID;

		private int iD_;

		private bool hasDecayDelay;

		private float decayDelay_;

		private bool hasCreatorID;

		private ulong creatorID_;

		private bool hasOwnerID;

		private ulong ownerID_;

		private int memoizedSerializedSize = -1;

		[CLSCompliant(false)]
		public ulong CreatorID
		{
			get
			{
				return this.creatorID_;
			}
		}

		public float DecayDelay
		{
			get
			{
				return this.decayDelay_;
			}
		}

		public static objectStructMaster DefaultInstance
		{
			get
			{
				return objectStructMaster.defaultInstance;
			}
		}

		public override objectStructMaster DefaultInstanceForType
		{
			get
			{
				return objectStructMaster.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectStructMaster__Descriptor;
			}
		}

		public bool HasCreatorID
		{
			get
			{
				return this.hasCreatorID;
			}
		}

		public bool HasDecayDelay
		{
			get
			{
				return this.hasDecayDelay;
			}
		}

		public bool HasID
		{
			get
			{
				return this.hasID;
			}
		}

		public bool HasOwnerID
		{
			get
			{
				return this.hasOwnerID;
			}
		}

		public int ID
		{
			get
			{
				return this.iD_;
			}
		}

		protected override FieldAccessorTable<objectStructMaster, objectStructMaster.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectStructMaster__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		[CLSCompliant(false)]
		public ulong OwnerID
		{
			get
			{
				return this.ownerID_;
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
				if (this.hasID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(1, this.ID);
				}
				if (this.hasDecayDelay)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(2, this.DecayDelay);
				}
				if (this.hasCreatorID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeUInt64Size(3, this.CreatorID);
				}
				if (this.hasOwnerID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeUInt64Size(4, this.OwnerID);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectStructMaster ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectStructMaster()
		{
			objectStructMaster.defaultInstance = (new objectStructMaster()).MakeReadOnly();
			objectStructMaster._objectStructMasterFieldNames = new string[] { "CreatorID", "DecayDelay", "ID", "OwnerID" };
			objectStructMaster._objectStructMasterFieldTags = new uint[] { 24, 21, 8, 32 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectStructMaster()
		{
		}

		public static objectStructMaster.Builder CreateBuilder()
		{
			return new objectStructMaster.Builder();
		}

		public static objectStructMaster.Builder CreateBuilder(objectStructMaster prototype)
		{
			return new objectStructMaster.Builder(prototype);
		}

		public override objectStructMaster.Builder CreateBuilderForType()
		{
			return new objectStructMaster.Builder();
		}

		private objectStructMaster MakeReadOnly()
		{
			return this;
		}

		public static objectStructMaster ParseDelimitedFrom(Stream input)
		{
			return objectStructMaster.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectStructMaster ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectStructMaster.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectStructMaster ParseFrom(ByteString data)
		{
			return objectStructMaster.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectStructMaster ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectStructMaster.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectStructMaster ParseFrom(byte[] data)
		{
			return objectStructMaster.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectStructMaster ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectStructMaster.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectStructMaster ParseFrom(Stream input)
		{
			return objectStructMaster.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectStructMaster ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectStructMaster.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectStructMaster ParseFrom(ICodedInputStream input)
		{
			return objectStructMaster.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectStructMaster ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectStructMaster.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectStructMaster, objectStructMaster.Builder> Recycler()
		{
			return Recycler<objectStructMaster, objectStructMaster.Builder>.Manufacture();
		}

		public override objectStructMaster.Builder ToBuilder()
		{
			return objectStructMaster.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectStructMaster._objectStructMasterFieldNames;
			if (this.hasID)
			{
				output.WriteInt32(1, strArrays[2], this.ID);
			}
			if (this.hasDecayDelay)
			{
				output.WriteFloat(2, strArrays[1], this.DecayDelay);
			}
			if (this.hasCreatorID)
			{
				output.WriteUInt64(3, strArrays[0], this.CreatorID);
			}
			if (this.hasOwnerID)
			{
				output.WriteUInt64(4, strArrays[3], this.OwnerID);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectStructMaster, objectStructMaster.Builder>
		{
			private bool resultIsReadOnly;

			private objectStructMaster result;

			[CLSCompliant(false)]
			public ulong CreatorID
			{
				get
				{
					return this.result.CreatorID;
				}
				set
				{
					this.SetCreatorID(value);
				}
			}

			public float DecayDelay
			{
				get
				{
					return this.result.DecayDelay;
				}
				set
				{
					this.SetDecayDelay(value);
				}
			}

			public override objectStructMaster DefaultInstanceForType
			{
				get
				{
					return objectStructMaster.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectStructMaster.Descriptor;
				}
			}

			public bool HasCreatorID
			{
				get
				{
					return this.result.hasCreatorID;
				}
			}

			public bool HasDecayDelay
			{
				get
				{
					return this.result.hasDecayDelay;
				}
			}

			public bool HasID
			{
				get
				{
					return this.result.hasID;
				}
			}

			public bool HasOwnerID
			{
				get
				{
					return this.result.hasOwnerID;
				}
			}

			public int ID
			{
				get
				{
					return this.result.ID;
				}
				set
				{
					this.SetID(value);
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override objectStructMaster MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			[CLSCompliant(false)]
			public ulong OwnerID
			{
				get
				{
					return this.result.OwnerID;
				}
				set
				{
					this.SetOwnerID(value);
				}
			}

			protected override objectStructMaster.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectStructMaster.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectStructMaster cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectStructMaster BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectStructMaster.Builder Clear()
			{
				this.result = objectStructMaster.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectStructMaster.Builder ClearCreatorID()
			{
				this.PrepareBuilder();
				this.result.hasCreatorID = false;
				this.result.creatorID_ = (ulong)0;
				return this;
			}

			public objectStructMaster.Builder ClearDecayDelay()
			{
				this.PrepareBuilder();
				this.result.hasDecayDelay = false;
				this.result.decayDelay_ = 0f;
				return this;
			}

			public objectStructMaster.Builder ClearID()
			{
				this.PrepareBuilder();
				this.result.hasID = false;
				this.result.iD_ = 0;
				return this;
			}

			public objectStructMaster.Builder ClearOwnerID()
			{
				this.PrepareBuilder();
				this.result.hasOwnerID = false;
				this.result.ownerID_ = (ulong)0;
				return this;
			}

			public override objectStructMaster.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectStructMaster.Builder(this.result);
				}
				return (new objectStructMaster.Builder()).MergeFrom(this.result);
			}

			public override objectStructMaster.Builder MergeFrom(IMessage other)
			{
				if (other is objectStructMaster)
				{
					return this.MergeFrom((objectStructMaster)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectStructMaster.Builder MergeFrom(objectStructMaster other)
			{
				if (other == objectStructMaster.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasID)
				{
					this.ID = other.ID;
				}
				if (other.HasDecayDelay)
				{
					this.DecayDelay = other.DecayDelay;
				}
				if (other.HasCreatorID)
				{
					this.CreatorID = other.CreatorID;
				}
				if (other.HasOwnerID)
				{
					this.OwnerID = other.OwnerID;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectStructMaster.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectStructMaster.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
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
						int num2 = Array.BinarySearch<string>(objectStructMaster._objectStructMasterFieldNames, str, StringComparer.Ordinal);
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
							num = objectStructMaster._objectStructMasterFieldTags[num2];
						}
					}
					num1 = num;
					switch (num1)
					{
						case 21:
						{
							this.result.hasDecayDelay = input.ReadFloat(ref this.result.decayDelay_);
							continue;
						}
						case 24:
						{
							this.result.hasCreatorID = input.ReadUInt64(ref this.result.creatorID_);
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
				if (num1 == 8)
				{
					this.result.hasID = input.ReadInt32(ref this.result.iD_);
					goto Label1;
				}
				else if (num1 == 32)
				{
					this.result.hasOwnerID = input.ReadUInt64(ref this.result.ownerID_);
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

			private objectStructMaster PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectStructMaster _objectStructMaster = this.result;
					this.result = new objectStructMaster();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectStructMaster);
				}
				return this.result;
			}

			[CLSCompliant(false)]
			public objectStructMaster.Builder SetCreatorID(ulong value)
			{
				this.PrepareBuilder();
				this.result.hasCreatorID = true;
				this.result.creatorID_ = value;
				return this;
			}

			public objectStructMaster.Builder SetDecayDelay(float value)
			{
				this.PrepareBuilder();
				this.result.hasDecayDelay = true;
				this.result.decayDelay_ = value;
				return this;
			}

			public objectStructMaster.Builder SetID(int value)
			{
				this.PrepareBuilder();
				this.result.hasID = true;
				this.result.iD_ = value;
				return this;
			}

			[CLSCompliant(false)]
			public objectStructMaster.Builder SetOwnerID(ulong value)
			{
				this.PrepareBuilder();
				this.result.hasOwnerID = true;
				this.result.ownerID_ = value;
				return this;
			}
		}
	}
}