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
	public sealed class objectNGCInstance : GeneratedMessage<objectNGCInstance, objectNGCInstance.Builder>
	{
		public const int IDFieldNumber = 1;

		public const int DataFieldNumber = 2;

		private readonly static objectNGCInstance defaultInstance;

		private readonly static string[] _objectNGCInstanceFieldNames;

		private readonly static uint[] _objectNGCInstanceFieldTags;

		private bool hasID;

		private int iD_;

		private bool hasData;

		private ByteString data_ = ByteString.Empty;

		private int memoizedSerializedSize = -1;

		public ByteString Data
		{
			get
			{
				return this.data_;
			}
		}

		public static objectNGCInstance DefaultInstance
		{
			get
			{
				return objectNGCInstance.defaultInstance;
			}
		}

		public override objectNGCInstance DefaultInstanceForType
		{
			get
			{
				return objectNGCInstance.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectNGCInstance__Descriptor;
			}
		}

		public bool HasData
		{
			get
			{
				return this.hasData;
			}
		}

		public bool HasID
		{
			get
			{
				return this.hasID;
			}
		}

		public int ID
		{
			get
			{
				return this.iD_;
			}
		}

		protected override FieldAccessorTable<objectNGCInstance, objectNGCInstance.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectNGCInstance__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
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
				if (this.hasID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(1, this.ID);
				}
				if (this.hasData)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeBytesSize(2, this.Data);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectNGCInstance ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectNGCInstance()
		{
			objectNGCInstance.defaultInstance = (new objectNGCInstance()).MakeReadOnly();
			objectNGCInstance._objectNGCInstanceFieldNames = new string[] { "ID", "data" };
			objectNGCInstance._objectNGCInstanceFieldTags = new uint[] { 8, 18 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectNGCInstance()
		{
		}

		public static objectNGCInstance.Builder CreateBuilder()
		{
			return new objectNGCInstance.Builder();
		}

		public static objectNGCInstance.Builder CreateBuilder(objectNGCInstance prototype)
		{
			return new objectNGCInstance.Builder(prototype);
		}

		public override objectNGCInstance.Builder CreateBuilderForType()
		{
			return new objectNGCInstance.Builder();
		}

		private objectNGCInstance MakeReadOnly()
		{
			return this;
		}

		public static objectNGCInstance ParseDelimitedFrom(Stream input)
		{
			return objectNGCInstance.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectNGCInstance ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectNGCInstance.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectNGCInstance ParseFrom(ByteString data)
		{
			return objectNGCInstance.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectNGCInstance ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectNGCInstance.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectNGCInstance ParseFrom(byte[] data)
		{
			return objectNGCInstance.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectNGCInstance ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectNGCInstance.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectNGCInstance ParseFrom(Stream input)
		{
			return objectNGCInstance.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectNGCInstance ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectNGCInstance.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectNGCInstance ParseFrom(ICodedInputStream input)
		{
			return objectNGCInstance.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectNGCInstance ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectNGCInstance.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectNGCInstance, objectNGCInstance.Builder> Recycler()
		{
			return Recycler<objectNGCInstance, objectNGCInstance.Builder>.Manufacture();
		}

		public override objectNGCInstance.Builder ToBuilder()
		{
			return objectNGCInstance.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectNGCInstance._objectNGCInstanceFieldNames;
			if (this.hasID)
			{
				output.WriteInt32(1, strArrays[0], this.ID);
			}
			if (this.hasData)
			{
				output.WriteBytes(2, strArrays[1], this.Data);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectNGCInstance, objectNGCInstance.Builder>
		{
			private bool resultIsReadOnly;

			private objectNGCInstance result;

			public ByteString Data
			{
				get
				{
					return this.result.Data;
				}
				set
				{
					this.SetData(value);
				}
			}

			public override objectNGCInstance DefaultInstanceForType
			{
				get
				{
					return objectNGCInstance.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectNGCInstance.Descriptor;
				}
			}

			public bool HasData
			{
				get
				{
					return this.result.hasData;
				}
			}

			public bool HasID
			{
				get
				{
					return this.result.hasID;
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

			protected override objectNGCInstance MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override objectNGCInstance.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectNGCInstance.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectNGCInstance cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectNGCInstance BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectNGCInstance.Builder Clear()
			{
				this.result = objectNGCInstance.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectNGCInstance.Builder ClearData()
			{
				this.PrepareBuilder();
				this.result.hasData = false;
				this.result.data_ = ByteString.Empty;
				return this;
			}

			public objectNGCInstance.Builder ClearID()
			{
				this.PrepareBuilder();
				this.result.hasID = false;
				this.result.iD_ = 0;
				return this;
			}

			public override objectNGCInstance.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectNGCInstance.Builder(this.result);
				}
				return (new objectNGCInstance.Builder()).MergeFrom(this.result);
			}

			public override objectNGCInstance.Builder MergeFrom(IMessage other)
			{
				if (other is objectNGCInstance)
				{
					return this.MergeFrom((objectNGCInstance)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectNGCInstance.Builder MergeFrom(objectNGCInstance other)
			{
				if (other == objectNGCInstance.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasID)
				{
					this.ID = other.ID;
				}
				if (other.HasData)
				{
					this.Data = other.Data;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectNGCInstance.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectNGCInstance.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectNGCInstance._objectNGCInstanceFieldNames, str, StringComparer.Ordinal);
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
							num = objectNGCInstance._objectNGCInstanceFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasID = input.ReadInt32(ref this.result.iD_);
					}
					else if (num2 == 18)
					{
						this.result.hasData = input.ReadBytes(ref this.result.data_);
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

			private objectNGCInstance PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectNGCInstance _objectNGCInstance = this.result;
					this.result = new objectNGCInstance();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectNGCInstance);
				}
				return this.result;
			}

			public objectNGCInstance.Builder SetData(ByteString value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasData = true;
				this.result.data_ = value;
				return this;
			}

			public objectNGCInstance.Builder SetID(int value)
			{
				this.PrepareBuilder();
				this.result.hasID = true;
				this.result.iD_ = value;
				return this;
			}
		}
	}
}