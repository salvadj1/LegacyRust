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
	public sealed class objectICarriableTrans : GeneratedMessage<objectICarriableTrans, objectICarriableTrans.Builder>
	{
		public const int TransCarrierIDFieldNumber = 1;

		private readonly static objectICarriableTrans defaultInstance;

		private readonly static string[] _objectICarriableTransFieldNames;

		private readonly static uint[] _objectICarriableTransFieldTags;

		private bool hasTransCarrierID;

		private int transCarrierID_;

		private int memoizedSerializedSize = -1;

		public static objectICarriableTrans DefaultInstance
		{
			get
			{
				return objectICarriableTrans.defaultInstance;
			}
		}

		public override objectICarriableTrans DefaultInstanceForType
		{
			get
			{
				return objectICarriableTrans.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectICarriableTrans__Descriptor;
			}
		}

		public bool HasTransCarrierID
		{
			get
			{
				return this.hasTransCarrierID;
			}
		}

		protected override FieldAccessorTable<objectICarriableTrans, objectICarriableTrans.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectICarriableTrans__FieldAccessorTable;
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
				if (this.hasTransCarrierID)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(1, this.TransCarrierID);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectICarriableTrans ThisMessage
		{
			get
			{
				return this;
			}
		}

		public int TransCarrierID
		{
			get
			{
				return this.transCarrierID_;
			}
		}

		static objectICarriableTrans()
		{
			objectICarriableTrans.defaultInstance = (new objectICarriableTrans()).MakeReadOnly();
			objectICarriableTrans._objectICarriableTransFieldNames = new string[] { "transCarrierID" };
			objectICarriableTrans._objectICarriableTransFieldTags = new uint[] { 8 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectICarriableTrans()
		{
		}

		public static objectICarriableTrans.Builder CreateBuilder()
		{
			return new objectICarriableTrans.Builder();
		}

		public static objectICarriableTrans.Builder CreateBuilder(objectICarriableTrans prototype)
		{
			return new objectICarriableTrans.Builder(prototype);
		}

		public override objectICarriableTrans.Builder CreateBuilderForType()
		{
			return new objectICarriableTrans.Builder();
		}

		private objectICarriableTrans MakeReadOnly()
		{
			return this;
		}

		public static objectICarriableTrans ParseDelimitedFrom(Stream input)
		{
			return objectICarriableTrans.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectICarriableTrans ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectICarriableTrans.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectICarriableTrans ParseFrom(ByteString data)
		{
			return objectICarriableTrans.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectICarriableTrans ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectICarriableTrans.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectICarriableTrans ParseFrom(byte[] data)
		{
			return objectICarriableTrans.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectICarriableTrans ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectICarriableTrans.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectICarriableTrans ParseFrom(Stream input)
		{
			return objectICarriableTrans.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectICarriableTrans ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectICarriableTrans.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectICarriableTrans ParseFrom(ICodedInputStream input)
		{
			return objectICarriableTrans.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectICarriableTrans ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectICarriableTrans.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectICarriableTrans, objectICarriableTrans.Builder> Recycler()
		{
			return Recycler<objectICarriableTrans, objectICarriableTrans.Builder>.Manufacture();
		}

		public override objectICarriableTrans.Builder ToBuilder()
		{
			return objectICarriableTrans.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectICarriableTrans._objectICarriableTransFieldNames;
			if (this.hasTransCarrierID)
			{
				output.WriteInt32(1, strArrays[0], this.TransCarrierID);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectICarriableTrans, objectICarriableTrans.Builder>
		{
			private bool resultIsReadOnly;

			private objectICarriableTrans result;

			public override objectICarriableTrans DefaultInstanceForType
			{
				get
				{
					return objectICarriableTrans.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectICarriableTrans.Descriptor;
				}
			}

			public bool HasTransCarrierID
			{
				get
				{
					return this.result.hasTransCarrierID;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override objectICarriableTrans MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override objectICarriableTrans.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public int TransCarrierID
			{
				get
				{
					return this.result.TransCarrierID;
				}
				set
				{
					this.SetTransCarrierID(value);
				}
			}

			public Builder()
			{
				this.result = objectICarriableTrans.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectICarriableTrans cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectICarriableTrans BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectICarriableTrans.Builder Clear()
			{
				this.result = objectICarriableTrans.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectICarriableTrans.Builder ClearTransCarrierID()
			{
				this.PrepareBuilder();
				this.result.hasTransCarrierID = false;
				this.result.transCarrierID_ = 0;
				return this;
			}

			public override objectICarriableTrans.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectICarriableTrans.Builder(this.result);
				}
				return (new objectICarriableTrans.Builder()).MergeFrom(this.result);
			}

			public override objectICarriableTrans.Builder MergeFrom(IMessage other)
			{
				if (other is objectICarriableTrans)
				{
					return this.MergeFrom((objectICarriableTrans)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectICarriableTrans.Builder MergeFrom(objectICarriableTrans other)
			{
				if (other == objectICarriableTrans.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasTransCarrierID)
				{
					this.TransCarrierID = other.TransCarrierID;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectICarriableTrans.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectICarriableTrans.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectICarriableTrans._objectICarriableTransFieldNames, str, StringComparer.Ordinal);
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
							num = objectICarriableTrans._objectICarriableTransFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasTransCarrierID = input.ReadInt32(ref this.result.transCarrierID_);
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

			private objectICarriableTrans PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectICarriableTrans objectICarriableTran = this.result;
					this.result = new objectICarriableTrans();
					this.resultIsReadOnly = false;
					this.MergeFrom(objectICarriableTran);
				}
				return this.result;
			}

			public objectICarriableTrans.Builder SetTransCarrierID(int value)
			{
				this.PrepareBuilder();
				this.result.hasTransCarrierID = true;
				this.result.transCarrierID_ = value;
				return this;
			}
		}
	}
}