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
	public sealed class objectStructComponent : GeneratedMessage<objectStructComponent, objectStructComponent.Builder>
	{
		public const int IDFieldNumber = 1;

		public const int MasterIDFieldNumber = 2;

		public const int MasterViewIDFieldNumber = 3;

		private readonly static objectStructComponent defaultInstance;

		private readonly static string[] _objectStructComponentFieldNames;

		private readonly static uint[] _objectStructComponentFieldTags;

		private bool hasID;

		private int iD_;

		private bool hasMasterID;

		private int masterID_;

		private bool hasMasterViewID;

		private int masterViewID_;

		private int memoizedSerializedSize = -1;

		public static objectStructComponent DefaultInstance
		{
			get
			{
				return objectStructComponent.defaultInstance;
			}
		}

		public override objectStructComponent DefaultInstanceForType
		{
			get
			{
				return objectStructComponent.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectStructComponent__Descriptor;
			}
		}

		public bool HasID
		{
			get
			{
				return this.hasID;
			}
		}

		public bool HasMasterID
		{
			get
			{
				return this.hasMasterID;
			}
		}

		public bool HasMasterViewID
		{
			get
			{
				return this.hasMasterViewID;
			}
		}

		public int ID
		{
			get
			{
				return this.iD_;
			}
		}

		protected override FieldAccessorTable<objectStructComponent, objectStructComponent.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectStructComponent__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public int MasterID
		{
			get
			{
				return this.masterID_;
			}
		}

		public int MasterViewID
		{
			get
			{
				return this.masterViewID_;
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
				if (this.hasMasterID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(2, this.MasterID);
				}
				if (this.hasMasterViewID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(3, this.MasterViewID);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectStructComponent ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectStructComponent()
		{
			objectStructComponent.defaultInstance = (new objectStructComponent()).MakeReadOnly();
			objectStructComponent._objectStructComponentFieldNames = new string[] { "ID", "MasterID", "MasterViewID" };
			objectStructComponent._objectStructComponentFieldTags = new uint[] { 8, 16, 24 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectStructComponent()
		{
		}

		public static objectStructComponent.Builder CreateBuilder()
		{
			return new objectStructComponent.Builder();
		}

		public static objectStructComponent.Builder CreateBuilder(objectStructComponent prototype)
		{
			return new objectStructComponent.Builder(prototype);
		}

		public override objectStructComponent.Builder CreateBuilderForType()
		{
			return new objectStructComponent.Builder();
		}

		private objectStructComponent MakeReadOnly()
		{
			return this;
		}

		public static objectStructComponent ParseDelimitedFrom(Stream input)
		{
			return objectStructComponent.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectStructComponent ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectStructComponent.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectStructComponent ParseFrom(ByteString data)
		{
			return objectStructComponent.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectStructComponent ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectStructComponent.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectStructComponent ParseFrom(byte[] data)
		{
			return objectStructComponent.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectStructComponent ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectStructComponent.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectStructComponent ParseFrom(Stream input)
		{
			return objectStructComponent.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectStructComponent ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectStructComponent.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectStructComponent ParseFrom(ICodedInputStream input)
		{
			return objectStructComponent.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectStructComponent ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectStructComponent.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectStructComponent, objectStructComponent.Builder> Recycler()
		{
			return Recycler<objectStructComponent, objectStructComponent.Builder>.Manufacture();
		}

		public override objectStructComponent.Builder ToBuilder()
		{
			return objectStructComponent.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectStructComponent._objectStructComponentFieldNames;
			if (this.hasID)
			{
				output.WriteInt32(1, strArrays[0], this.ID);
			}
			if (this.hasMasterID)
			{
				output.WriteInt32(2, strArrays[1], this.MasterID);
			}
			if (this.hasMasterViewID)
			{
				output.WriteInt32(3, strArrays[2], this.MasterViewID);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectStructComponent, objectStructComponent.Builder>
		{
			private bool resultIsReadOnly;

			private objectStructComponent result;

			public override objectStructComponent DefaultInstanceForType
			{
				get
				{
					return objectStructComponent.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectStructComponent.Descriptor;
				}
			}

			public bool HasID
			{
				get
				{
					return this.result.hasID;
				}
			}

			public bool HasMasterID
			{
				get
				{
					return this.result.hasMasterID;
				}
			}

			public bool HasMasterViewID
			{
				get
				{
					return this.result.hasMasterViewID;
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

			public int MasterID
			{
				get
				{
					return this.result.MasterID;
				}
				set
				{
					this.SetMasterID(value);
				}
			}

			public int MasterViewID
			{
				get
				{
					return this.result.MasterViewID;
				}
				set
				{
					this.SetMasterViewID(value);
				}
			}

			protected override objectStructComponent MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override objectStructComponent.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectStructComponent.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectStructComponent cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectStructComponent BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectStructComponent.Builder Clear()
			{
				this.result = objectStructComponent.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectStructComponent.Builder ClearID()
			{
				this.PrepareBuilder();
				this.result.hasID = false;
				this.result.iD_ = 0;
				return this;
			}

			public objectStructComponent.Builder ClearMasterID()
			{
				this.PrepareBuilder();
				this.result.hasMasterID = false;
				this.result.masterID_ = 0;
				return this;
			}

			public objectStructComponent.Builder ClearMasterViewID()
			{
				this.PrepareBuilder();
				this.result.hasMasterViewID = false;
				this.result.masterViewID_ = 0;
				return this;
			}

			public override objectStructComponent.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectStructComponent.Builder(this.result);
				}
				return (new objectStructComponent.Builder()).MergeFrom(this.result);
			}

			public override objectStructComponent.Builder MergeFrom(IMessage other)
			{
				if (other is objectStructComponent)
				{
					return this.MergeFrom((objectStructComponent)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectStructComponent.Builder MergeFrom(objectStructComponent other)
			{
				if (other == objectStructComponent.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasID)
				{
					this.ID = other.ID;
				}
				if (other.HasMasterID)
				{
					this.MasterID = other.MasterID;
				}
				if (other.HasMasterViewID)
				{
					this.MasterViewID = other.MasterViewID;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectStructComponent.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectStructComponent.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectStructComponent._objectStructComponentFieldNames, str, StringComparer.Ordinal);
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
							num = objectStructComponent._objectStructComponentFieldTags[num1];
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
					else if (num2 == 16)
					{
						this.result.hasMasterID = input.ReadInt32(ref this.result.masterID_);
					}
					else if (num2 == 24)
					{
						this.result.hasMasterViewID = input.ReadInt32(ref this.result.masterViewID_);
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

			private objectStructComponent PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectStructComponent _objectStructComponent = this.result;
					this.result = new objectStructComponent();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectStructComponent);
				}
				return this.result;
			}

			public objectStructComponent.Builder SetID(int value)
			{
				this.PrepareBuilder();
				this.result.hasID = true;
				this.result.iD_ = value;
				return this;
			}

			public objectStructComponent.Builder SetMasterID(int value)
			{
				this.PrepareBuilder();
				this.result.hasMasterID = true;
				this.result.masterID_ = value;
				return this;
			}

			public objectStructComponent.Builder SetMasterViewID(int value)
			{
				this.PrepareBuilder();
				this.result.hasMasterViewID = true;
				this.result.masterViewID_ = value;
				return this;
			}
		}
	}
}