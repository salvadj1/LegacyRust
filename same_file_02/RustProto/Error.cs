using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Proto;
using System;
using System.Diagnostics;
using System.IO;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class Error : GeneratedMessage<RustProto.Error, RustProto.Error.Builder>
	{
		public const int StatusFieldNumber = 1;

		public const int MessageFieldNumber = 2;

		private readonly static RustProto.Error defaultInstance;

		private readonly static string[] _errorFieldNames;

		private readonly static uint[] _errorFieldTags;

		private bool hasStatus;

		private string status_ = string.Empty;

		private bool hasMessage;

		private string message_ = string.Empty;

		private int memoizedSerializedSize = -1;

		public static RustProto.Error DefaultInstance
		{
			get
			{
				return RustProto.Error.defaultInstance;
			}
		}

		public override RustProto.Error DefaultInstanceForType
		{
			get
			{
				return RustProto.Error.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Error.internal__static_RustProto_Error__Descriptor;
			}
		}

		public bool HasMessage
		{
			get
			{
				return this.hasMessage;
			}
		}

		public bool HasStatus
		{
			get
			{
				return this.hasStatus;
			}
		}

		protected override FieldAccessorTable<RustProto.Error, RustProto.Error.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.Error.internal__static_RustProto_Error__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				if (!this.hasStatus)
				{
					return false;
				}
				if (!this.hasMessage)
				{
					return false;
				}
				return true;
			}
		}

		public string Message
		{
			get
			{
				return this.message_;
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
				if (this.hasStatus)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeStringSize(1, this.Status);
				}
				if (this.hasMessage)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeStringSize(2, this.Message);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		public string Status
		{
			get
			{
				return this.status_;
			}
		}

		protected override RustProto.Error ThisMessage
		{
			get
			{
				return this;
			}
		}

		static Error()
		{
			RustProto.Error.defaultInstance = (new RustProto.Error()).MakeReadOnly();
			RustProto.Error._errorFieldNames = new string[] { "message", "status" };
			RustProto.Error._errorFieldTags = new uint[] { 18, 10 };
			object.ReferenceEquals(RustProto.Proto.Error.Descriptor, null);
		}

		private Error()
		{
		}

		public static RustProto.Error.Builder CreateBuilder()
		{
			return new RustProto.Error.Builder();
		}

		public static RustProto.Error.Builder CreateBuilder(RustProto.Error prototype)
		{
			return new RustProto.Error.Builder(prototype);
		}

		public override RustProto.Error.Builder CreateBuilderForType()
		{
			return new RustProto.Error.Builder();
		}

		private RustProto.Error MakeReadOnly()
		{
			return this;
		}

		public static RustProto.Error ParseDelimitedFrom(Stream input)
		{
			return RustProto.Error.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static RustProto.Error ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Error.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Error ParseFrom(ByteString data)
		{
			return RustProto.Error.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Error ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Error.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Error ParseFrom(byte[] data)
		{
			return RustProto.Error.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Error ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Error.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Error ParseFrom(Stream input)
		{
			return RustProto.Error.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Error ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Error.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Error ParseFrom(ICodedInputStream input)
		{
			return RustProto.Error.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Error ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Error.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public override RustProto.Error.Builder ToBuilder()
		{
			return RustProto.Error.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = RustProto.Error._errorFieldNames;
			if (this.hasStatus)
			{
				output.WriteString(1, strArrays[1], this.Status);
			}
			if (this.hasMessage)
			{
				output.WriteString(2, strArrays[0], this.Message);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<RustProto.Error, RustProto.Error.Builder>
		{
			private bool resultIsReadOnly;

			private RustProto.Error result;

			public override RustProto.Error DefaultInstanceForType
			{
				get
				{
					return RustProto.Error.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return RustProto.Error.Descriptor;
				}
			}

			public bool HasMessage
			{
				get
				{
					return this.result.hasMessage;
				}
			}

			public bool HasStatus
			{
				get
				{
					return this.result.hasStatus;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			public string Message
			{
				get
				{
					return this.result.Message;
				}
				set
				{
					this.SetMessage(value);
				}
			}

			protected override RustProto.Error MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public string Status
			{
				get
				{
					return this.result.Status;
				}
				set
				{
					this.SetStatus(value);
				}
			}

			protected override RustProto.Error.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = RustProto.Error.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(RustProto.Error cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override RustProto.Error BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override RustProto.Error.Builder Clear()
			{
				this.result = RustProto.Error.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public RustProto.Error.Builder ClearMessage()
			{
				this.PrepareBuilder();
				this.result.hasMessage = false;
				this.result.message_ = string.Empty;
				return this;
			}

			public RustProto.Error.Builder ClearStatus()
			{
				this.PrepareBuilder();
				this.result.hasStatus = false;
				this.result.status_ = string.Empty;
				return this;
			}

			public override RustProto.Error.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new RustProto.Error.Builder(this.result);
				}
				return (new RustProto.Error.Builder()).MergeFrom(this.result);
			}

			public override RustProto.Error.Builder MergeFrom(IMessage other)
			{
				if (other is RustProto.Error)
				{
					return this.MergeFrom((RustProto.Error)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override RustProto.Error.Builder MergeFrom(RustProto.Error other)
			{
				if (other == RustProto.Error.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasStatus)
				{
					this.Status = other.Status;
				}
				if (other.HasMessage)
				{
					this.Message = other.Message;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override RustProto.Error.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override RustProto.Error.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(RustProto.Error._errorFieldNames, str, StringComparer.Ordinal);
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
							num = RustProto.Error._errorFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 10)
					{
						this.result.hasStatus = input.ReadString(ref this.result.status_);
					}
					else if (num2 == 18)
					{
						this.result.hasMessage = input.ReadString(ref this.result.message_);
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

			private RustProto.Error PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					RustProto.Error error = this.result;
					this.result = new RustProto.Error();
					this.resultIsReadOnly = false;
					this.MergeFrom(error);
				}
				return this.result;
			}

			public RustProto.Error.Builder SetMessage(string value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasMessage = true;
				this.result.message_ = value;
				return this;
			}

			public RustProto.Error.Builder SetStatus(string value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasStatus = true;
				this.result.status_ = value;
				return this;
			}
		}
	}
}