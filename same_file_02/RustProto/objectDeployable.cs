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
	public sealed class objectDeployable : GeneratedMessage<objectDeployable, objectDeployable.Builder>
	{
		public const int CreatorIDFieldNumber = 1;

		public const int OwnerIDFieldNumber = 2;

		private readonly static objectDeployable defaultInstance;

		private readonly static string[] _objectDeployableFieldNames;

		private readonly static uint[] _objectDeployableFieldTags;

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

		public static objectDeployable DefaultInstance
		{
			get
			{
				return objectDeployable.defaultInstance;
			}
		}

		public override objectDeployable DefaultInstanceForType
		{
			get
			{
				return objectDeployable.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectDeployable__Descriptor;
			}
		}

		public bool HasCreatorID
		{
			get
			{
				return this.hasCreatorID;
			}
		}

		public bool HasOwnerID
		{
			get
			{
				return this.hasOwnerID;
			}
		}

		protected override FieldAccessorTable<objectDeployable, objectDeployable.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectDeployable__FieldAccessorTable;
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
				if (this.hasCreatorID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeUInt64Size(1, this.CreatorID);
				}
				if (this.hasOwnerID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeUInt64Size(2, this.OwnerID);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectDeployable ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectDeployable()
		{
			objectDeployable.defaultInstance = (new objectDeployable()).MakeReadOnly();
			objectDeployable._objectDeployableFieldNames = new string[] { "CreatorID", "OwnerID" };
			objectDeployable._objectDeployableFieldTags = new uint[] { 8, 16 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectDeployable()
		{
		}

		public static objectDeployable.Builder CreateBuilder()
		{
			return new objectDeployable.Builder();
		}

		public static objectDeployable.Builder CreateBuilder(objectDeployable prototype)
		{
			return new objectDeployable.Builder(prototype);
		}

		public override objectDeployable.Builder CreateBuilderForType()
		{
			return new objectDeployable.Builder();
		}

		private objectDeployable MakeReadOnly()
		{
			return this;
		}

		public static objectDeployable ParseDelimitedFrom(Stream input)
		{
			return objectDeployable.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectDeployable ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectDeployable.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectDeployable ParseFrom(ByteString data)
		{
			return objectDeployable.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectDeployable ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectDeployable.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectDeployable ParseFrom(byte[] data)
		{
			return objectDeployable.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectDeployable ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectDeployable.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectDeployable ParseFrom(Stream input)
		{
			return objectDeployable.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectDeployable ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectDeployable.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectDeployable ParseFrom(ICodedInputStream input)
		{
			return objectDeployable.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectDeployable ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectDeployable.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectDeployable, objectDeployable.Builder> Recycler()
		{
			return Recycler<objectDeployable, objectDeployable.Builder>.Manufacture();
		}

		public override objectDeployable.Builder ToBuilder()
		{
			return objectDeployable.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectDeployable._objectDeployableFieldNames;
			if (this.hasCreatorID)
			{
				output.WriteUInt64(1, strArrays[0], this.CreatorID);
			}
			if (this.hasOwnerID)
			{
				output.WriteUInt64(2, strArrays[1], this.OwnerID);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectDeployable, objectDeployable.Builder>
		{
			private bool resultIsReadOnly;

			private objectDeployable result;

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

			public override objectDeployable DefaultInstanceForType
			{
				get
				{
					return objectDeployable.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectDeployable.Descriptor;
				}
			}

			public bool HasCreatorID
			{
				get
				{
					return this.result.hasCreatorID;
				}
			}

			public bool HasOwnerID
			{
				get
				{
					return this.result.hasOwnerID;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override objectDeployable MessageBeingBuilt
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

			protected override objectDeployable.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectDeployable.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectDeployable cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectDeployable BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectDeployable.Builder Clear()
			{
				this.result = objectDeployable.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectDeployable.Builder ClearCreatorID()
			{
				this.PrepareBuilder();
				this.result.hasCreatorID = false;
				this.result.creatorID_ = (ulong)0;
				return this;
			}

			public objectDeployable.Builder ClearOwnerID()
			{
				this.PrepareBuilder();
				this.result.hasOwnerID = false;
				this.result.ownerID_ = (ulong)0;
				return this;
			}

			public override objectDeployable.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectDeployable.Builder(this.result);
				}
				return (new objectDeployable.Builder()).MergeFrom(this.result);
			}

			public override objectDeployable.Builder MergeFrom(IMessage other)
			{
				if (other is objectDeployable)
				{
					return this.MergeFrom((objectDeployable)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectDeployable.Builder MergeFrom(objectDeployable other)
			{
				if (other == objectDeployable.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
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

			public override objectDeployable.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectDeployable.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectDeployable._objectDeployableFieldNames, str, StringComparer.Ordinal);
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
							num = objectDeployable._objectDeployableFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasCreatorID = input.ReadUInt64(ref this.result.creatorID_);
					}
					else if (num2 == 16)
					{
						this.result.hasOwnerID = input.ReadUInt64(ref this.result.ownerID_);
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

			private objectDeployable PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectDeployable _objectDeployable = this.result;
					this.result = new objectDeployable();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectDeployable);
				}
				return this.result;
			}

			[CLSCompliant(false)]
			public objectDeployable.Builder SetCreatorID(ulong value)
			{
				this.PrepareBuilder();
				this.result.hasCreatorID = true;
				this.result.creatorID_ = value;
				return this;
			}

			[CLSCompliant(false)]
			public objectDeployable.Builder SetOwnerID(ulong value)
			{
				this.PrepareBuilder();
				this.result.hasOwnerID = true;
				this.result.ownerID_ = value;
				return this;
			}
		}
	}
}