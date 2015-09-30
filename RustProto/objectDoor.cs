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
	public sealed class objectDoor : GeneratedMessage<objectDoor, objectDoor.Builder>
	{
		public const int StateFieldNumber = 1;

		public const int OpenFieldNumber = 2;

		private readonly static objectDoor defaultInstance;

		private readonly static string[] _objectDoorFieldNames;

		private readonly static uint[] _objectDoorFieldTags;

		private bool hasState;

		private int state_;

		private bool hasOpen;

		private bool open_;

		private int memoizedSerializedSize = -1;

		public static objectDoor DefaultInstance
		{
			get
			{
				return objectDoor.defaultInstance;
			}
		}

		public override objectDoor DefaultInstanceForType
		{
			get
			{
				return objectDoor.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectDoor__Descriptor;
			}
		}

		public bool HasOpen
		{
			get
			{
				return this.hasOpen;
			}
		}

		public bool HasState
		{
			get
			{
				return this.hasState;
			}
		}

		protected override FieldAccessorTable<objectDoor, objectDoor.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectDoor__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public bool Open
		{
			get
			{
				return this.open_;
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
				if (this.hasState)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(1, this.State);
				}
				if (this.hasOpen)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeBoolSize(2, this.Open);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		public int State
		{
			get
			{
				return this.state_;
			}
		}

		protected override objectDoor ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectDoor()
		{
			objectDoor.defaultInstance = (new objectDoor()).MakeReadOnly();
			objectDoor._objectDoorFieldNames = new string[] { "Open", "State" };
			objectDoor._objectDoorFieldTags = new uint[] { 16, 8 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectDoor()
		{
		}

		public static objectDoor.Builder CreateBuilder()
		{
			return new objectDoor.Builder();
		}

		public static objectDoor.Builder CreateBuilder(objectDoor prototype)
		{
			return new objectDoor.Builder(prototype);
		}

		public override objectDoor.Builder CreateBuilderForType()
		{
			return new objectDoor.Builder();
		}

		private objectDoor MakeReadOnly()
		{
			return this;
		}

		public static objectDoor ParseDelimitedFrom(Stream input)
		{
			return objectDoor.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectDoor ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectDoor.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectDoor ParseFrom(ByteString data)
		{
			return objectDoor.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectDoor ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectDoor.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectDoor ParseFrom(byte[] data)
		{
			return objectDoor.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectDoor ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectDoor.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectDoor ParseFrom(Stream input)
		{
			return objectDoor.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectDoor ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectDoor.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectDoor ParseFrom(ICodedInputStream input)
		{
			return objectDoor.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectDoor ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectDoor.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectDoor, objectDoor.Builder> Recycler()
		{
			return Recycler<objectDoor, objectDoor.Builder>.Manufacture();
		}

		public override objectDoor.Builder ToBuilder()
		{
			return objectDoor.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectDoor._objectDoorFieldNames;
			if (this.hasState)
			{
				output.WriteInt32(1, strArrays[1], this.State);
			}
			if (this.hasOpen)
			{
				output.WriteBool(2, strArrays[0], this.Open);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectDoor, objectDoor.Builder>
		{
			private bool resultIsReadOnly;

			private objectDoor result;

			public override objectDoor DefaultInstanceForType
			{
				get
				{
					return objectDoor.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectDoor.Descriptor;
				}
			}

			public bool HasOpen
			{
				get
				{
					return this.result.hasOpen;
				}
			}

			public bool HasState
			{
				get
				{
					return this.result.hasState;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override objectDoor MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public bool Open
			{
				get
				{
					return this.result.Open;
				}
				set
				{
					this.SetOpen(value);
				}
			}

			public int State
			{
				get
				{
					return this.result.State;
				}
				set
				{
					this.SetState(value);
				}
			}

			protected override objectDoor.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectDoor.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectDoor cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectDoor BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectDoor.Builder Clear()
			{
				this.result = objectDoor.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectDoor.Builder ClearOpen()
			{
				this.PrepareBuilder();
				this.result.hasOpen = false;
				this.result.open_ = false;
				return this;
			}

			public objectDoor.Builder ClearState()
			{
				this.PrepareBuilder();
				this.result.hasState = false;
				this.result.state_ = 0;
				return this;
			}

			public override objectDoor.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectDoor.Builder(this.result);
				}
				return (new objectDoor.Builder()).MergeFrom(this.result);
			}

			public override objectDoor.Builder MergeFrom(IMessage other)
			{
				if (other is objectDoor)
				{
					return this.MergeFrom((objectDoor)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectDoor.Builder MergeFrom(objectDoor other)
			{
				if (other == objectDoor.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasState)
				{
					this.State = other.State;
				}
				if (other.HasOpen)
				{
					this.Open = other.Open;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectDoor.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectDoor.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectDoor._objectDoorFieldNames, str, StringComparer.Ordinal);
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
							num = objectDoor._objectDoorFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 8)
					{
						this.result.hasState = input.ReadInt32(ref this.result.state_);
					}
					else if (num2 == 16)
					{
						this.result.hasOpen = input.ReadBool(ref this.result.open_);
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

			private objectDoor PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectDoor _objectDoor = this.result;
					this.result = new objectDoor();
					this.resultIsReadOnly = false;
					this.MergeFrom(_objectDoor);
				}
				return this.result;
			}

			public objectDoor.Builder SetOpen(bool value)
			{
				this.PrepareBuilder();
				this.result.hasOpen = true;
				this.result.open_ = value;
				return this;
			}

			public objectDoor.Builder SetState(int value)
			{
				this.PrepareBuilder();
				this.result.hasState = true;
				this.result.state_ = value;
				return this;
			}
		}
	}
}