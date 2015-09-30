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
	public sealed class objectCoords : GeneratedMessage<objectCoords, objectCoords.Builder>
	{
		public const int PosFieldNumber = 1;

		public const int OldPosFieldNumber = 2;

		public const int RotFieldNumber = 3;

		public const int OldRotFieldNumber = 4;

		private readonly static objectCoords defaultInstance;

		private readonly static string[] _objectCoordsFieldNames;

		private readonly static uint[] _objectCoordsFieldTags;

		private bool hasPos;

		private Vector pos_;

		private bool hasOldPos;

		private Vector oldPos_;

		private bool hasRot;

		private Quaternion rot_;

		private bool hasOldRot;

		private Quaternion oldRot_;

		private int memoizedSerializedSize = -1;

		public static objectCoords DefaultInstance
		{
			get
			{
				return objectCoords.defaultInstance;
			}
		}

		public override objectCoords DefaultInstanceForType
		{
			get
			{
				return objectCoords.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectCoords__Descriptor;
			}
		}

		public bool HasOldPos
		{
			get
			{
				return this.hasOldPos;
			}
		}

		public bool HasOldRot
		{
			get
			{
				return this.hasOldRot;
			}
		}

		public bool HasPos
		{
			get
			{
				return this.hasPos;
			}
		}

		public bool HasRot
		{
			get
			{
				return this.hasRot;
			}
		}

		protected override FieldAccessorTable<objectCoords, objectCoords.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_objectCoords__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public Vector OldPos
		{
			get
			{
				return this.oldPos_ ?? Vector.DefaultInstance;
			}
		}

		public Quaternion OldRot
		{
			get
			{
				return this.oldRot_ ?? Quaternion.DefaultInstance;
			}
		}

		public Vector Pos
		{
			get
			{
				return this.pos_ ?? Vector.DefaultInstance;
			}
		}

		public Quaternion Rot
		{
			get
			{
				return this.rot_ ?? Quaternion.DefaultInstance;
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
				if (this.hasPos)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(1, this.Pos);
				}
				if (this.hasOldPos)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(2, this.OldPos);
				}
				if (this.hasRot)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(3, this.Rot);
				}
				if (this.hasOldRot)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(4, this.OldRot);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override objectCoords ThisMessage
		{
			get
			{
				return this;
			}
		}

		static objectCoords()
		{
			objectCoords.defaultInstance = (new objectCoords()).MakeReadOnly();
			objectCoords._objectCoordsFieldNames = new string[] { "oldPos", "oldRot", "pos", "rot" };
			objectCoords._objectCoordsFieldTags = new uint[] { 18, 34, 10, 26 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private objectCoords()
		{
		}

		public static objectCoords.Builder CreateBuilder()
		{
			return new objectCoords.Builder();
		}

		public static objectCoords.Builder CreateBuilder(objectCoords prototype)
		{
			return new objectCoords.Builder(prototype);
		}

		public override objectCoords.Builder CreateBuilderForType()
		{
			return new objectCoords.Builder();
		}

		private objectCoords MakeReadOnly()
		{
			return this;
		}

		public static objectCoords ParseDelimitedFrom(Stream input)
		{
			return objectCoords.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static objectCoords ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectCoords.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectCoords ParseFrom(ByteString data)
		{
			return objectCoords.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectCoords ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return objectCoords.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectCoords ParseFrom(byte[] data)
		{
			return objectCoords.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static objectCoords ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return objectCoords.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static objectCoords ParseFrom(Stream input)
		{
			return objectCoords.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectCoords ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return objectCoords.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static objectCoords ParseFrom(ICodedInputStream input)
		{
			return objectCoords.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static objectCoords ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return objectCoords.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<objectCoords, objectCoords.Builder> Recycler()
		{
			return Recycler<objectCoords, objectCoords.Builder>.Manufacture();
		}

		public override objectCoords.Builder ToBuilder()
		{
			return objectCoords.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = objectCoords._objectCoordsFieldNames;
			if (this.hasPos)
			{
				output.WriteMessage(1, strArrays[2], this.Pos);
			}
			if (this.hasOldPos)
			{
				output.WriteMessage(2, strArrays[0], this.OldPos);
			}
			if (this.hasRot)
			{
				output.WriteMessage(3, strArrays[3], this.Rot);
			}
			if (this.hasOldRot)
			{
				output.WriteMessage(4, strArrays[1], this.OldRot);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<objectCoords, objectCoords.Builder>
		{
			private bool resultIsReadOnly;

			private objectCoords result;

			public override objectCoords DefaultInstanceForType
			{
				get
				{
					return objectCoords.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return objectCoords.Descriptor;
				}
			}

			public bool HasOldPos
			{
				get
				{
					return this.result.hasOldPos;
				}
			}

			public bool HasOldRot
			{
				get
				{
					return this.result.hasOldRot;
				}
			}

			public bool HasPos
			{
				get
				{
					return this.result.hasPos;
				}
			}

			public bool HasRot
			{
				get
				{
					return this.result.hasRot;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override objectCoords MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public Vector OldPos
			{
				get
				{
					return this.result.OldPos;
				}
				set
				{
					this.SetOldPos(value);
				}
			}

			public Quaternion OldRot
			{
				get
				{
					return this.result.OldRot;
				}
				set
				{
					this.SetOldRot(value);
				}
			}

			public Vector Pos
			{
				get
				{
					return this.result.Pos;
				}
				set
				{
					this.SetPos(value);
				}
			}

			public Quaternion Rot
			{
				get
				{
					return this.result.Rot;
				}
				set
				{
					this.SetRot(value);
				}
			}

			protected override objectCoords.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = objectCoords.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(objectCoords cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override objectCoords BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override objectCoords.Builder Clear()
			{
				this.result = objectCoords.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public objectCoords.Builder ClearOldPos()
			{
				this.PrepareBuilder();
				this.result.hasOldPos = false;
				this.result.oldPos_ = null;
				return this;
			}

			public objectCoords.Builder ClearOldRot()
			{
				this.PrepareBuilder();
				this.result.hasOldRot = false;
				this.result.oldRot_ = null;
				return this;
			}

			public objectCoords.Builder ClearPos()
			{
				this.PrepareBuilder();
				this.result.hasPos = false;
				this.result.pos_ = null;
				return this;
			}

			public objectCoords.Builder ClearRot()
			{
				this.PrepareBuilder();
				this.result.hasRot = false;
				this.result.rot_ = null;
				return this;
			}

			public override objectCoords.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new objectCoords.Builder(this.result);
				}
				return (new objectCoords.Builder()).MergeFrom(this.result);
			}

			public override objectCoords.Builder MergeFrom(IMessage other)
			{
				if (other is objectCoords)
				{
					return this.MergeFrom((objectCoords)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override objectCoords.Builder MergeFrom(objectCoords other)
			{
				if (other == objectCoords.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasPos)
				{
					this.MergePos(other.Pos);
				}
				if (other.HasOldPos)
				{
					this.MergeOldPos(other.OldPos);
				}
				if (other.HasRot)
				{
					this.MergeRot(other.Rot);
				}
				if (other.HasOldRot)
				{
					this.MergeOldRot(other.OldRot);
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override objectCoords.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override objectCoords.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(objectCoords._objectCoordsFieldNames, str, StringComparer.Ordinal);
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
							num = objectCoords._objectCoordsFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 10)
					{
						Vector.Builder builder1 = Vector.CreateBuilder();
						if (this.result.hasPos)
						{
							builder1.MergeFrom(this.Pos);
						}
						input.ReadMessage(builder1, extensionRegistry);
						this.Pos = builder1.BuildPartial();
					}
					else if (num2 == 18)
					{
						Vector.Builder builder2 = Vector.CreateBuilder();
						if (this.result.hasOldPos)
						{
							builder2.MergeFrom(this.OldPos);
						}
						input.ReadMessage(builder2, extensionRegistry);
						this.OldPos = builder2.BuildPartial();
					}
					else if (num2 == 26)
					{
						Quaternion.Builder builder3 = Quaternion.CreateBuilder();
						if (this.result.hasRot)
						{
							builder3.MergeFrom(this.Rot);
						}
						input.ReadMessage(builder3, extensionRegistry);
						this.Rot = builder3.BuildPartial();
					}
					else if (num2 == 34)
					{
						Quaternion.Builder builder4 = Quaternion.CreateBuilder();
						if (this.result.hasOldRot)
						{
							builder4.MergeFrom(this.OldRot);
						}
						input.ReadMessage(builder4, extensionRegistry);
						this.OldRot = builder4.BuildPartial();
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

			public objectCoords.Builder MergeOldPos(Vector value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasOldPos || this.result.oldPos_ == Vector.DefaultInstance)
				{
					this.result.oldPos_ = value;
				}
				else
				{
					this.result.oldPos_ = Vector.CreateBuilder(this.result.oldPos_).MergeFrom(value).BuildPartial();
				}
				this.result.hasOldPos = true;
				return this;
			}

			public objectCoords.Builder MergeOldRot(Quaternion value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasOldRot || this.result.oldRot_ == Quaternion.DefaultInstance)
				{
					this.result.oldRot_ = value;
				}
				else
				{
					this.result.oldRot_ = Quaternion.CreateBuilder(this.result.oldRot_).MergeFrom(value).BuildPartial();
				}
				this.result.hasOldRot = true;
				return this;
			}

			public objectCoords.Builder MergePos(Vector value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasPos || this.result.pos_ == Vector.DefaultInstance)
				{
					this.result.pos_ = value;
				}
				else
				{
					this.result.pos_ = Vector.CreateBuilder(this.result.pos_).MergeFrom(value).BuildPartial();
				}
				this.result.hasPos = true;
				return this;
			}

			public objectCoords.Builder MergeRot(Quaternion value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasRot || this.result.rot_ == Quaternion.DefaultInstance)
				{
					this.result.rot_ = value;
				}
				else
				{
					this.result.rot_ = Quaternion.CreateBuilder(this.result.rot_).MergeFrom(value).BuildPartial();
				}
				this.result.hasRot = true;
				return this;
			}

			private objectCoords PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					objectCoords objectCoord = this.result;
					this.result = new objectCoords();
					this.resultIsReadOnly = false;
					this.MergeFrom(objectCoord);
				}
				return this.result;
			}

			public objectCoords.Builder SetOldPos(Vector value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasOldPos = true;
				this.result.oldPos_ = value;
				return this;
			}

			public objectCoords.Builder SetOldPos(Vector.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasOldPos = true;
				this.result.oldPos_ = builderForValue.Build();
				return this;
			}

			public objectCoords.Builder SetOldRot(Quaternion value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasOldRot = true;
				this.result.oldRot_ = value;
				return this;
			}

			public objectCoords.Builder SetOldRot(Quaternion.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasOldRot = true;
				this.result.oldRot_ = builderForValue.Build();
				return this;
			}

			public objectCoords.Builder SetPos(Vector value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasPos = true;
				this.result.pos_ = value;
				return this;
			}

			public objectCoords.Builder SetPos(Vector.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasPos = true;
				this.result.pos_ = builderForValue.Build();
				return this;
			}

			public objectCoords.Builder SetRot(Quaternion value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasRot = true;
				this.result.rot_ = value;
				return this;
			}

			public objectCoords.Builder SetRot(Quaternion.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasRot = true;
				this.result.rot_ = builderForValue.Build();
				return this;
			}
		}
	}
}