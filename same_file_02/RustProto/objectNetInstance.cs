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
	public sealed class objectNetInstance : GeneratedMessage<objectNetInstance, objectNetInstance.Builder>
	{
		public const int ServerPrefabFieldNumber = 1;

		public const int OwnerPrefabFieldNumber = 2;

		public const int ProxyPrefabFieldNumber = 3;

		public const int GroupIDFieldNumber = 4;

		private readonly static objectNetInstance defaultInstance;

		private readonly static string[] _objectNetInstanceFieldNames;

		private readonly static uint[] _objectNetInstanceFieldTags;

		private bool hasServerPrefab;

		private int serverPrefab_;

		private bool hasOwnerPrefab;

		private int ownerPrefab_;

		private bool hasProxyPrefab;

		private int proxyPrefab_;

		private bool hasGroupID;

		private int groupID_;

		private int memoizedSerializedSize = -1;

		public static objectNetInstance DefaultInstance
		{
			get
			{
				return objectNetInstance.defaultInstance;
			}
		}

		public override objectNetInstance DefaultInstanceForType
		{
			get
			{
				return objectNetInstance.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectNetInstance__Descriptor;
			}
		}

		public int GroupID
		{
			get
			{
				return this.groupID_;
			}
		}

		public bool HasGroupID
		{
			get
			{
				return this.hasGroupID;
			}
		}

		public bool HasOwnerPrefab
		{
			get
			{
				return this.hasOwnerPrefab;
			}
		}

		public bool HasProxyPrefab
		{
			get
			{
				return this.hasProxyPrefab;
			}
		}

		public bool HasServerPrefab
		{
			get
			{
				return this.hasServerPrefab;
			}
		}

		protected override FieldAccessorTable<objectNetInstance, objectNetInstance.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectNetInstance__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public int OwnerPrefab
		{
			get
			{
				return this.ownerPrefab_;
			}
		}

		public int ProxyPrefab
		{
			get
			{
				return this.proxyPrefab_;
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
				if (this.hasServerPrefab)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(1, this.ServerPrefab);
				}
				if (this.hasOwnerPrefab)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(2, this.OwnerPrefab);
				}
				if (this.hasProxyPrefab)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(3, this.ProxyPrefab);
				}
				if (this.hasGroupID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(4, this.GroupID);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		public int ServerPrefab
		{
			get
			{
				return this.serverPrefab_;
			}
		}

		protected override objectNetInstance ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectNetInstance()
		{
			objectNetInstance.defaultInstance = (new objectNetInstance()).MakeReadOnly();
			objectNetInstance._objectNetInstanceFieldNames = new string[] { "groupID", "ownerPrefab", "proxyPrefab", "serverPrefab" };
			objectNetInstance._objectNetInstanceFieldTags = new uint[] { 32, 16, 24, 8 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectNetInstance()
		{
		}

		public static objectNetInstance.Builder CreateBuilder()
		{
			return new objectNetInstance.Builder();
		}

		public static objectNetInstance.Builder CreateBuilder(objectNetInstance prototype)
		{
			return new objectNetInstance.Builder(prototype);
		}

		public override objectNetInstance.Builder CreateBuilderForType()
		{
			return new objectNetInstance.Builder();
		}

		private objectNetInstance MakeReadOnly()
		{
			return this;
		}

		public static objectNetInstance ParseDelimitedFrom(Stream input)
		{
			return objectNetInstance.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectNetInstance ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectNetInstance.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectNetInstance ParseFrom(ByteString data)
		{
			return objectNetInstance.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectNetInstance ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectNetInstance.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectNetInstance ParseFrom(byte[] data)
		{
			return objectNetInstance.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectNetInstance ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectNetInstance.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectNetInstance ParseFrom(Stream input)
		{
			return objectNetInstance.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectNetInstance ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectNetInstance.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectNetInstance ParseFrom(ICodedInputStream input)
		{
			return objectNetInstance.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectNetInstance ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectNetInstance.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectNetInstance, objectNetInstance.Builder> Recycler()
		{
			return Recycler<objectNetInstance, objectNetInstance.Builder>.Manufacture();
		}

		public override objectNetInstance.Builder ToBuilder()
		{
			return objectNetInstance.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectNetInstance._objectNetInstanceFieldNames;
			if (this.hasServerPrefab)
			{
				output.WriteInt32(1, strArrays[3], this.ServerPrefab);
			}
			if (this.hasOwnerPrefab)
			{
				output.WriteInt32(2, strArrays[1], this.OwnerPrefab);
			}
			if (this.hasProxyPrefab)
			{
				output.WriteInt32(3, strArrays[2], this.ProxyPrefab);
			}
			if (this.hasGroupID)
			{
				output.WriteInt32(4, strArrays[0], this.GroupID);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectNetInstance, objectNetInstance.Builder>
		{
			private bool resultIsReadOnly;

			private objectNetInstance result;

			public override objectNetInstance DefaultInstanceForType
			{
				get
				{
					return objectNetInstance.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectNetInstance.Descriptor;
				}
			}

			public int GroupID
			{
				get
				{
					return this.result.GroupID;
				}
				set
				{
					this.SetGroupID(value);
				}
			}

			public bool HasGroupID
			{
				get
				{
					return this.result.hasGroupID;
				}
			}

			public bool HasOwnerPrefab
			{
				get
				{
					return this.result.hasOwnerPrefab;
				}
			}

			public bool HasProxyPrefab
			{
				get
				{
					return this.result.hasProxyPrefab;
				}
			}

			public bool HasServerPrefab
			{
				get
				{
					return this.result.hasServerPrefab;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override objectNetInstance MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public int OwnerPrefab
			{
				get
				{
					return this.result.OwnerPrefab;
				}
				set
				{
					this.SetOwnerPrefab(value);
				}
			}

			public int ProxyPrefab
			{
				get
				{
					return this.result.ProxyPrefab;
				}
				set
				{
					this.SetProxyPrefab(value);
				}
			}

			public int ServerPrefab
			{
				get
				{
					return this.result.ServerPrefab;
				}
				set
				{
					this.SetServerPrefab(value);
				}
			}

			protected override objectNetInstance.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectNetInstance.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectNetInstance cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectNetInstance BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectNetInstance.Builder Clear()
			{
				this.result = objectNetInstance.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectNetInstance.Builder ClearGroupID()
			{
				this.PrepareBuilder();
				this.result.hasGroupID = false;
				this.result.groupID_ = 0;
				return this;
			}

			public objectNetInstance.Builder ClearOwnerPrefab()
			{
				this.PrepareBuilder();
				this.result.hasOwnerPrefab = false;
				this.result.ownerPrefab_ = 0;
				return this;
			}

			public objectNetInstance.Builder ClearProxyPrefab()
			{
				this.PrepareBuilder();
				this.result.hasProxyPrefab = false;
				this.result.proxyPrefab_ = 0;
				return this;
			}

			public objectNetInstance.Builder ClearServerPrefab()
			{
				this.PrepareBuilder();
				this.result.hasServerPrefab = false;
				this.result.serverPrefab_ = 0;
				return this;
			}

			public override objectNetInstance.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectNetInstance.Builder(this.result);
				}
				return (new objectNetInstance.Builder()).MergeFrom(this.result);
			}

			public override objectNetInstance.Builder MergeFrom(IMessage other)
			{
				if (other is objectNetInstance)
				{
					return this.MergeFrom((objectNetInstance)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectNetInstance.Builder MergeFrom(objectNetInstance other)
			{
				if (other == objectNetInstance.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasServerPrefab)
				{
					this.ServerPrefab = other.ServerPrefab;
				}
				if (other.HasOwnerPrefab)
				{
					this.OwnerPrefab = other.OwnerPrefab;
				}
				if (other.HasProxyPrefab)
				{
					this.ProxyPrefab = other.ProxyPrefab;
				}
				if (other.HasGroupID)
				{
					this.GroupID = other.GroupID;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectNetInstance.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectNetInstance.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectNetInstance._objectNetInstanceFieldNames, str, StringComparer.Ordinal);
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
							num = objectNetInstance._objectNetInstanceFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasServerPrefab = input.ReadInt32(ref this.result.serverPrefab_);
					}
					else if (num2 == 16)
					{
						this.result.hasOwnerPrefab = input.ReadInt32(ref this.result.ownerPrefab_);
					}
					else if (num2 == 24)
					{
						this.result.hasProxyPrefab = input.ReadInt32(ref this.result.proxyPrefab_);
					}
					else if (num2 == 32)
					{
						this.result.hasGroupID = input.ReadInt32(ref this.result.groupID_);
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

			private objectNetInstance PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectNetInstance _objectNetInstance = this.result;
					this.result = new objectNetInstance();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectNetInstance);
				}
				return this.result;
			}

			public objectNetInstance.Builder SetGroupID(int value)
			{
				this.PrepareBuilder();
				this.result.hasGroupID = true;
				this.result.groupID_ = value;
				return this;
			}

			public objectNetInstance.Builder SetOwnerPrefab(int value)
			{
				this.PrepareBuilder();
				this.result.hasOwnerPrefab = true;
				this.result.ownerPrefab_ = value;
				return this;
			}

			public objectNetInstance.Builder SetProxyPrefab(int value)
			{
				this.PrepareBuilder();
				this.result.hasProxyPrefab = true;
				this.result.proxyPrefab_ = value;
				return this;
			}

			public objectNetInstance.Builder SetServerPrefab(int value)
			{
				this.PrepareBuilder();
				this.result.hasServerPrefab = true;
				this.result.serverPrefab_ = value;
				return this;
			}
		}
	}
}