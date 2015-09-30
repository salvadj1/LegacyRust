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
	public sealed class Blueprint : GeneratedMessage<RustProto.Blueprint, RustProto.Blueprint.Builder>
	{
		public const int IdFieldNumber = 1;

		private readonly static RustProto.Blueprint defaultInstance;

		private readonly static string[] _blueprintFieldNames;

		private readonly static uint[] _blueprintFieldTags;

		private bool hasId;

		private int id_;

		private int memoizedSerializedSize = -1;

		public static RustProto.Blueprint DefaultInstance
		{
			get
			{
				return RustProto.Blueprint.defaultInstance;
			}
		}

		public override RustProto.Blueprint DefaultInstanceForType
		{
			get
			{
				return RustProto.Blueprint.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Blueprint.internal__static_RustProto_Blueprint__Descriptor;
			}
		}

		public bool HasId
		{
			get
			{
				return this.hasId;
			}
		}

		public int Id
		{
			get
			{
				return this.id_;
			}
		}

		protected override FieldAccessorTable<RustProto.Blueprint, RustProto.Blueprint.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.Blueprint.internal__static_RustProto_Blueprint__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				if (!this.hasId)
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
				if (this.hasId)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(1, this.Id);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override RustProto.Blueprint ThisMessage
		{
			get
			{
				return this;
			}
		}

		static Blueprint()
		{
			RustProto.Blueprint.defaultInstance = (new RustProto.Blueprint()).MakeReadOnly();
			RustProto.Blueprint._blueprintFieldNames = new string[] { "id" };
			RustProto.Blueprint._blueprintFieldTags = new uint[] { 8 };
			object.ReferenceEquals(RustProto.Proto.Blueprint.Descriptor, null);
		}

		private Blueprint()
		{
		}

		public static RustProto.Blueprint.Builder CreateBuilder()
		{
			return new RustProto.Blueprint.Builder();
		}

		public static RustProto.Blueprint.Builder CreateBuilder(RustProto.Blueprint prototype)
		{
			return new RustProto.Blueprint.Builder(prototype);
		}

		public override RustProto.Blueprint.Builder CreateBuilderForType()
		{
			return new RustProto.Blueprint.Builder();
		}

		private RustProto.Blueprint MakeReadOnly()
		{
			return this;
		}

		public static RustProto.Blueprint ParseDelimitedFrom(Stream input)
		{
			return RustProto.Blueprint.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static RustProto.Blueprint ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Blueprint.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Blueprint ParseFrom(ByteString data)
		{
			return RustProto.Blueprint.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Blueprint ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Blueprint.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Blueprint ParseFrom(byte[] data)
		{
			return RustProto.Blueprint.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Blueprint ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Blueprint.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Blueprint ParseFrom(Stream input)
		{
			return RustProto.Blueprint.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Blueprint ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Blueprint.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Blueprint ParseFrom(ICodedInputStream input)
		{
			return RustProto.Blueprint.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Blueprint ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Blueprint.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<RustProto.Blueprint, RustProto.Blueprint.Builder> Recycler()
		{
			return Recycler<RustProto.Blueprint, RustProto.Blueprint.Builder>.Manufacture();
		}

		public override RustProto.Blueprint.Builder ToBuilder()
		{
			return RustProto.Blueprint.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = RustProto.Blueprint._blueprintFieldNames;
			if (this.hasId)
			{
				output.WriteInt32(1, strArrays[0], this.Id);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<RustProto.Blueprint, RustProto.Blueprint.Builder>
		{
			private bool resultIsReadOnly;

			private RustProto.Blueprint result;

			public override RustProto.Blueprint DefaultInstanceForType
			{
				get
				{
					return RustProto.Blueprint.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return RustProto.Blueprint.Descriptor;
				}
			}

			public bool HasId
			{
				get
				{
					return this.result.hasId;
				}
			}

			public int Id
			{
				get
				{
					return this.result.Id;
				}
				set
				{
					this.SetId(value);
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override RustProto.Blueprint MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override RustProto.Blueprint.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = RustProto.Blueprint.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(RustProto.Blueprint cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override RustProto.Blueprint BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override RustProto.Blueprint.Builder Clear()
			{
				this.result = RustProto.Blueprint.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public RustProto.Blueprint.Builder ClearId()
			{
				this.PrepareBuilder();
				this.result.hasId = false;
				this.result.id_ = 0;
				return this;
			}

			public override RustProto.Blueprint.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new RustProto.Blueprint.Builder(this.result);
				}
				return (new RustProto.Blueprint.Builder()).MergeFrom(this.result);
			}

			public override RustProto.Blueprint.Builder MergeFrom(IMessage other)
			{
				if (other is RustProto.Blueprint)
				{
					return this.MergeFrom((RustProto.Blueprint)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override RustProto.Blueprint.Builder MergeFrom(RustProto.Blueprint other)
			{
				if (other == RustProto.Blueprint.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasId)
				{
					this.Id = other.Id;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override RustProto.Blueprint.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override RustProto.Blueprint.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(RustProto.Blueprint._blueprintFieldNames, str, StringComparer.Ordinal);
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
							num = RustProto.Blueprint._blueprintFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasId = input.ReadInt32(ref this.result.id_);
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

			private RustProto.Blueprint PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					RustProto.Blueprint blueprint = this.result;
					this.result = new RustProto.Blueprint();
					this.resultIsReadOnly = false;
					this.MergeFrom(blueprint);
				}
				return this.result;
			}

			public RustProto.Blueprint.Builder SetId(int value)
			{
				this.PrepareBuilder();
				this.result.hasId = true;
				this.result.id_ = value;
				return this;
			}
		}
	}
}