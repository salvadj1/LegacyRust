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
	public sealed class GameError : GeneratedMessage<GameError, GameError.Builder>
	{
		public const int ErrorFieldNumber = 1;

		public const int TraceFieldNumber = 2;

		private readonly static GameError defaultInstance;

		private readonly static string[] _gameErrorFieldNames;

		private readonly static uint[] _gameErrorFieldTags;

		private bool hasError;

		private string error_ = string.Empty;

		private bool hasTrace;

		private string trace_ = string.Empty;

		private int memoizedSerializedSize = -1;

		public static GameError DefaultInstance
		{
			get
			{
				return GameError.defaultInstance;
			}
		}

		public override GameError DefaultInstanceForType
		{
			get
			{
				return GameError.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Error.internal__static_RustProto_GameError__Descriptor;
			}
		}

		public string Error
		{
			get
			{
				return this.error_;
			}
		}

		public bool HasError
		{
			get
			{
				return this.hasError;
			}
		}

		public bool HasTrace
		{
			get
			{
				return this.hasTrace;
			}
		}

		protected override FieldAccessorTable<GameError, GameError.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.Error.internal__static_RustProto_GameError__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				if (!this.hasError)
				{
					return false;
				}
				if (!this.hasTrace)
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
				if (this.hasError)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeStringSize(1, this.Error);
				}
				if (this.hasTrace)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeStringSize(2, this.Trace);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override GameError ThisMessage
		{
			get
			{
				return this;
			}
		}

		public string Trace
		{
			get
			{
				return this.trace_;
			}
		}

		static GameError()
		{
			GameError.defaultInstance = (new GameError()).MakeReadOnly();
			GameError._gameErrorFieldNames = new string[] { "error", "trace" };
			GameError._gameErrorFieldTags = new uint[] { 10, 18 };
			object.ReferenceEquals(RustProto.Proto.Error.Descriptor, null);
		}

		private GameError()
		{
		}

		public static GameError.Builder CreateBuilder()
		{
			return new GameError.Builder();
		}

		public static GameError.Builder CreateBuilder(GameError prototype)
		{
			return new GameError.Builder(prototype);
		}

		public override GameError.Builder CreateBuilderForType()
		{
			return new GameError.Builder();
		}

		private GameError MakeReadOnly()
		{
			return this;
		}

		public static GameError ParseDelimitedFrom(Stream input)
		{
			return GameError.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static GameError ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return GameError.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static GameError ParseFrom(ByteString data)
		{
			return GameError.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static GameError ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return GameError.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static GameError ParseFrom(byte[] data)
		{
			return GameError.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static GameError ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return GameError.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static GameError ParseFrom(Stream input)
		{
			return GameError.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static GameError ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return GameError.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static GameError ParseFrom(ICodedInputStream input)
		{
			return GameError.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static GameError ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return GameError.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public override GameError.Builder ToBuilder()
		{
			return GameError.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = GameError._gameErrorFieldNames;
			if (this.hasError)
			{
				output.WriteString(1, strArrays[0], this.Error);
			}
			if (this.hasTrace)
			{
				output.WriteString(2, strArrays[1], this.Trace);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<GameError, GameError.Builder>
		{
			private bool resultIsReadOnly;

			private GameError result;

			public override GameError DefaultInstanceForType
			{
				get
				{
					return GameError.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return GameError.Descriptor;
				}
			}

			public string Error
			{
				get
				{
					return this.result.Error;
				}
				set
				{
					this.SetError(value);
				}
			}

			public bool HasError
			{
				get
				{
					return this.result.hasError;
				}
			}

			public bool HasTrace
			{
				get
				{
					return this.result.hasTrace;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override GameError MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override GameError.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public string Trace
			{
				get
				{
					return this.result.Trace;
				}
				set
				{
					this.SetTrace(value);
				}
			}

			public Builder()
			{
				this.result = GameError.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(GameError cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override GameError BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override GameError.Builder Clear()
			{
				this.result = GameError.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public GameError.Builder ClearError()
			{
				this.PrepareBuilder();
				this.result.hasError = false;
				this.result.error_ = string.Empty;
				return this;
			}

			public GameError.Builder ClearTrace()
			{
				this.PrepareBuilder();
				this.result.hasTrace = false;
				this.result.trace_ = string.Empty;
				return this;
			}

			public override GameError.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new GameError.Builder(this.result);
				}
				return (new GameError.Builder()).MergeFrom(this.result);
			}

			public override GameError.Builder MergeFrom(IMessage other)
			{
				if (other is GameError)
				{
					return this.MergeFrom((GameError)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override GameError.Builder MergeFrom(GameError other)
			{
				if (other == GameError.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasError)
				{
					this.Error = other.Error;
				}
				if (other.HasTrace)
				{
					this.Trace = other.Trace;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override GameError.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override GameError.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(GameError._gameErrorFieldNames, str, StringComparer.Ordinal);
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
							num = GameError._gameErrorFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 10)
					{
						this.result.hasError = input.ReadString(ref this.result.error_);
					}
					else if (num2 == 18)
					{
						this.result.hasTrace = input.ReadString(ref this.result.trace_);
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

			private GameError PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					GameError gameError = this.result;
					this.result = new GameError();
					this.resultIsReadOnly = false;
					this.MergeFrom(gameError);
				}
				return this.result;
			}

			public GameError.Builder SetError(string value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasError = true;
				this.result.error_ = value;
				return this;
			}

			public GameError.Builder SetTrace(string value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasTrace = true;
				this.result.trace_ = value;
				return this;
			}
		}
	}
}