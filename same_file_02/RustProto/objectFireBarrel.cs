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
	public sealed class objectFireBarrel : GeneratedMessage<objectFireBarrel, objectFireBarrel.Builder>
	{
		public const int OnFireFieldNumber = 1;

		private readonly static objectFireBarrel defaultInstance;

		private readonly static string[] _objectFireBarrelFieldNames;

		private readonly static uint[] _objectFireBarrelFieldTags;

		private bool hasOnFire;

		private bool onFire_;

		private int memoizedSerializedSize = -1;

		public static objectFireBarrel DefaultInstance
		{
			get
			{
				return objectFireBarrel.defaultInstance;
			}
		}

		public override objectFireBarrel DefaultInstanceForType
		{
			get
			{
				return objectFireBarrel.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectFireBarrel__Descriptor;
			}
		}

		public bool HasOnFire
		{
			get
			{
				return this.hasOnFire;
			}
		}

		protected override FieldAccessorTable<objectFireBarrel, objectFireBarrel.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectFireBarrel__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public bool OnFire
		{
			get
			{
				return this.onFire_;
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
				if (this.hasOnFire)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeBoolSize(1, this.OnFire);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectFireBarrel ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectFireBarrel()
		{
			objectFireBarrel.defaultInstance = (new objectFireBarrel()).MakeReadOnly();
			objectFireBarrel._objectFireBarrelFieldNames = new string[] { "OnFire" };
			objectFireBarrel._objectFireBarrelFieldTags = new uint[] { 8 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectFireBarrel()
		{
		}

		public static objectFireBarrel.Builder CreateBuilder()
		{
			return new objectFireBarrel.Builder();
		}

		public static objectFireBarrel.Builder CreateBuilder(objectFireBarrel prototype)
		{
			return new objectFireBarrel.Builder(prototype);
		}

		public override objectFireBarrel.Builder CreateBuilderForType()
		{
			return new objectFireBarrel.Builder();
		}

		private objectFireBarrel MakeReadOnly()
		{
			return this;
		}

		public static objectFireBarrel ParseDelimitedFrom(Stream input)
		{
			return objectFireBarrel.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectFireBarrel ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectFireBarrel.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectFireBarrel ParseFrom(ByteString data)
		{
			return objectFireBarrel.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectFireBarrel ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectFireBarrel.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectFireBarrel ParseFrom(byte[] data)
		{
			return objectFireBarrel.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectFireBarrel ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectFireBarrel.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectFireBarrel ParseFrom(Stream input)
		{
			return objectFireBarrel.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectFireBarrel ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectFireBarrel.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectFireBarrel ParseFrom(ICodedInputStream input)
		{
			return objectFireBarrel.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectFireBarrel ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectFireBarrel.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectFireBarrel, objectFireBarrel.Builder> Recycler()
		{
			return Recycler<objectFireBarrel, objectFireBarrel.Builder>.Manufacture();
		}

		public override objectFireBarrel.Builder ToBuilder()
		{
			return objectFireBarrel.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectFireBarrel._objectFireBarrelFieldNames;
			if (this.hasOnFire)
			{
				output.WriteBool(1, strArrays[0], this.OnFire);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectFireBarrel, objectFireBarrel.Builder>
		{
			private bool resultIsReadOnly;

			private objectFireBarrel result;

			public override objectFireBarrel DefaultInstanceForType
			{
				get
				{
					return objectFireBarrel.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectFireBarrel.Descriptor;
				}
			}

			public bool HasOnFire
			{
				get
				{
					return this.result.hasOnFire;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override objectFireBarrel MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public bool OnFire
			{
				get
				{
					return this.result.OnFire;
				}
				set
				{
					this.SetOnFire(value);
				}
			}

			protected override objectFireBarrel.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectFireBarrel.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectFireBarrel cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectFireBarrel BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectFireBarrel.Builder Clear()
			{
				this.result = objectFireBarrel.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectFireBarrel.Builder ClearOnFire()
			{
				this.PrepareBuilder();
				this.result.hasOnFire = false;
				this.result.onFire_ = false;
				return this;
			}

			public override objectFireBarrel.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectFireBarrel.Builder(this.result);
				}
				return (new objectFireBarrel.Builder()).MergeFrom(this.result);
			}

			public override objectFireBarrel.Builder MergeFrom(IMessage other)
			{
				if (other is objectFireBarrel)
				{
					return this.MergeFrom((objectFireBarrel)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectFireBarrel.Builder MergeFrom(objectFireBarrel other)
			{
				if (other == objectFireBarrel.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasOnFire)
				{
					this.OnFire = other.OnFire;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectFireBarrel.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectFireBarrel.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectFireBarrel._objectFireBarrelFieldNames, str, StringComparer.Ordinal);
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
							num = objectFireBarrel._objectFireBarrelFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasOnFire = input.ReadBool(ref this.result.onFire_);
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

			private objectFireBarrel PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectFireBarrel _objectFireBarrel = this.result;
					this.result = new objectFireBarrel();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectFireBarrel);
				}
				return this.result;
			}

			public objectFireBarrel.Builder SetOnFire(bool value)
			{
				this.PrepareBuilder();
				this.result.hasOnFire = true;
				this.result.onFire_ = value;
				return this;
			}
		}
	}
}