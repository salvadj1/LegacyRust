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
	public sealed class objectTakeDamage : GeneratedMessage<objectTakeDamage, objectTakeDamage.Builder>
	{
		public const int HealthFieldNumber = 1;

		private readonly static objectTakeDamage defaultInstance;

		private readonly static string[] _objectTakeDamageFieldNames;

		private readonly static uint[] _objectTakeDamageFieldTags;

		private bool hasHealth;

		private float health_;

		private int memoizedSerializedSize = -1;

		public static objectTakeDamage DefaultInstance
		{
			get
			{
				return objectTakeDamage.defaultInstance;
			}
		}

		public override objectTakeDamage DefaultInstanceForType
		{
			get
			{
				return objectTakeDamage.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectTakeDamage__Descriptor;
			}
		}

		public bool HasHealth
		{
			get
			{
				return this.hasHealth;
			}
		}

		public float Health
		{
			get
			{
				return this.health_;
			}
		}

		protected override FieldAccessorTable<objectTakeDamage, objectTakeDamage.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectTakeDamage__FieldAccessorTable;
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
				if (this.hasHealth)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(1, this.Health);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectTakeDamage ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectTakeDamage()
		{
			objectTakeDamage.defaultInstance = (new objectTakeDamage()).MakeReadOnly();
			objectTakeDamage._objectTakeDamageFieldNames = new string[] { "health" };
			objectTakeDamage._objectTakeDamageFieldTags = new uint[] { 13 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectTakeDamage()
		{
		}

		public static objectTakeDamage.Builder CreateBuilder()
		{
			return new objectTakeDamage.Builder();
		}

		public static objectTakeDamage.Builder CreateBuilder(objectTakeDamage prototype)
		{
			return new objectTakeDamage.Builder(prototype);
		}

		public override objectTakeDamage.Builder CreateBuilderForType()
		{
			return new objectTakeDamage.Builder();
		}

		private objectTakeDamage MakeReadOnly()
		{
			return this;
		}

		public static objectTakeDamage ParseDelimitedFrom(Stream input)
		{
			return objectTakeDamage.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectTakeDamage ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectTakeDamage.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectTakeDamage ParseFrom(ByteString data)
		{
			return objectTakeDamage.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectTakeDamage ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectTakeDamage.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectTakeDamage ParseFrom(byte[] data)
		{
			return objectTakeDamage.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectTakeDamage ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectTakeDamage.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectTakeDamage ParseFrom(Stream input)
		{
			return objectTakeDamage.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectTakeDamage ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectTakeDamage.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectTakeDamage ParseFrom(ICodedInputStream input)
		{
			return objectTakeDamage.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectTakeDamage ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectTakeDamage.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectTakeDamage, objectTakeDamage.Builder> Recycler()
		{
			return Recycler<objectTakeDamage, objectTakeDamage.Builder>.Manufacture();
		}

		public override objectTakeDamage.Builder ToBuilder()
		{
			return objectTakeDamage.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectTakeDamage._objectTakeDamageFieldNames;
			if (this.hasHealth)
			{
				output.WriteFloat(1, strArrays[0], this.Health);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectTakeDamage, objectTakeDamage.Builder>
		{
			private bool resultIsReadOnly;

			private objectTakeDamage result;

			public override objectTakeDamage DefaultInstanceForType
			{
				get
				{
					return objectTakeDamage.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectTakeDamage.Descriptor;
				}
			}

			public bool HasHealth
			{
				get
				{
					return this.result.hasHealth;
				}
			}

			public float Health
			{
				get
				{
					return this.result.Health;
				}
				set
				{
					this.SetHealth(value);
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override objectTakeDamage MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override objectTakeDamage.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectTakeDamage.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectTakeDamage cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectTakeDamage BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectTakeDamage.Builder Clear()
			{
				this.result = objectTakeDamage.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectTakeDamage.Builder ClearHealth()
			{
				this.PrepareBuilder();
				this.result.hasHealth = false;
				this.result.health_ = 0f;
				return this;
			}

			public override objectTakeDamage.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectTakeDamage.Builder(this.result);
				}
				return (new objectTakeDamage.Builder()).MergeFrom(this.result);
			}

			public override objectTakeDamage.Builder MergeFrom(IMessage other)
			{
				if (other is objectTakeDamage)
				{
					return this.MergeFrom((objectTakeDamage)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectTakeDamage.Builder MergeFrom(objectTakeDamage other)
			{
				if (other == objectTakeDamage.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasHealth)
				{
					this.Health = other.Health;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectTakeDamage.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectTakeDamage.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectTakeDamage._objectTakeDamageFieldNames, str, StringComparer.Ordinal);
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
							num = objectTakeDamage._objectTakeDamageFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 13)
					{
						this.result.hasHealth = input.ReadFloat(ref this.result.health_);
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

			private objectTakeDamage PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectTakeDamage _objectTakeDamage = this.result;
					this.result = new objectTakeDamage();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectTakeDamage);
				}
				return this.result;
			}

			public objectTakeDamage.Builder SetHealth(float value)
			{
				this.PrepareBuilder();
				this.result.hasHealth = true;
				this.result.health_ = value;
				return this;
			}
		}
	}
}