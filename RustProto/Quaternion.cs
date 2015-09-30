using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class Quaternion : GeneratedMessage<RustProto.Quaternion, RustProto.Quaternion.Builder>
	{
		public const int XFieldNumber = 1;

		public const int YFieldNumber = 2;

		public const int ZFieldNumber = 3;

		public const int WFieldNumber = 4;

		private readonly static RustProto.Quaternion defaultInstance;

		private readonly static string[] _quaternionFieldNames;

		private readonly static uint[] _quaternionFieldTags;

		private bool hasX;

		private float x_;

		private bool hasY;

		private float y_;

		private bool hasZ;

		private float z_;

		private bool hasW;

		private float w_;

		private int memoizedSerializedSize = -1;

		public static RustProto.Quaternion DefaultInstance
		{
			get
			{
				return RustProto.Quaternion.defaultInstance;
			}
		}

		public override RustProto.Quaternion DefaultInstanceForType
		{
			get
			{
				return RustProto.Quaternion.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Common.internal__static_RustProto_Quaternion__Descriptor;
			}
		}

		public bool HasW
		{
			get
			{
				return this.hasW;
			}
		}

		public bool HasX
		{
			get
			{
				return this.hasX;
			}
		}

		public bool HasY
		{
			get
			{
				return this.hasY;
			}
		}

		public bool HasZ
		{
			get
			{
				return this.hasZ;
			}
		}

		protected override FieldAccessorTable<RustProto.Quaternion, RustProto.Quaternion.Builder> InternalFieldAccessors
		{
			get
			{
				return Common.internal__static_RustProto_Quaternion__FieldAccessorTable;
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
				if (this.hasX)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(1, this.X);
				}
				if (this.hasY)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(2, this.Y);
				}
				if (this.hasZ)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(3, this.Z);
				}
				if (this.hasW)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(4, this.W);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override RustProto.Quaternion ThisMessage
		{
			get
			{
				return this;
			}
		}

		public float W
		{
			get
			{
				return this.w_;
			}
		}

		public float X
		{
			get
			{
				return this.x_;
			}
		}

		public float Y
		{
			get
			{
				return this.y_;
			}
		}

		public float Z
		{
			get
			{
				return this.z_;
			}
		}

		static Quaternion()
		{
			RustProto.Quaternion.defaultInstance = (new RustProto.Quaternion()).MakeReadOnly();
			RustProto.Quaternion._quaternionFieldNames = new string[] { "w", "x", "y", "z" };
			RustProto.Quaternion._quaternionFieldTags = new uint[] { 37, 13, 21, 29 };
			object.ReferenceEquals(Common.Descriptor, null);
		}

		private Quaternion()
		{
		}

		public static RustProto.Quaternion.Builder CreateBuilder()
		{
			return new RustProto.Quaternion.Builder();
		}

		public static RustProto.Quaternion.Builder CreateBuilder(RustProto.Quaternion prototype)
		{
			return new RustProto.Quaternion.Builder(prototype);
		}

		public override RustProto.Quaternion.Builder CreateBuilderForType()
		{
			return new RustProto.Quaternion.Builder();
		}

		private RustProto.Quaternion MakeReadOnly()
		{
			return this;
		}

		public static implicit operator Quaternion(UnityEngine.Quaternion v)
		{
			RustProto.Quaternion quaternion;
			using (Recycler<RustProto.Quaternion, RustProto.Quaternion.Builder> recycler = RustProto.Quaternion.Recycler())
			{
				RustProto.Quaternion.Builder builder = recycler.OpenBuilder();
				builder.SetX(v.x);
				builder.SetY(v.y);
				builder.SetZ(v.z);
				builder.SetW(v.w);
				quaternion = builder.Build();
			}
			return quaternion;
		}

		public static RustProto.Quaternion ParseDelimitedFrom(Stream input)
		{
			return RustProto.Quaternion.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static RustProto.Quaternion ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Quaternion.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Quaternion ParseFrom(ByteString data)
		{
			return RustProto.Quaternion.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Quaternion ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Quaternion.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Quaternion ParseFrom(byte[] data)
		{
			return RustProto.Quaternion.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Quaternion ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Quaternion.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Quaternion ParseFrom(Stream input)
		{
			return RustProto.Quaternion.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Quaternion ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Quaternion.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Quaternion ParseFrom(ICodedInputStream input)
		{
			return RustProto.Quaternion.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Quaternion ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Quaternion.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<RustProto.Quaternion, RustProto.Quaternion.Builder> Recycler()
		{
			return Recycler<RustProto.Quaternion, RustProto.Quaternion.Builder>.Manufacture();
		}

		public override RustProto.Quaternion.Builder ToBuilder()
		{
			return RustProto.Quaternion.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = RustProto.Quaternion._quaternionFieldNames;
			if (this.hasX)
			{
				output.WriteFloat(1, strArrays[1], this.X);
			}
			if (this.hasY)
			{
				output.WriteFloat(2, strArrays[2], this.Y);
			}
			if (this.hasZ)
			{
				output.WriteFloat(3, strArrays[3], this.Z);
			}
			if (this.hasW)
			{
				output.WriteFloat(4, strArrays[0], this.W);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<RustProto.Quaternion, RustProto.Quaternion.Builder>
		{
			private bool resultIsReadOnly;

			private RustProto.Quaternion result;

			public override RustProto.Quaternion DefaultInstanceForType
			{
				get
				{
					return RustProto.Quaternion.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return RustProto.Quaternion.Descriptor;
				}
			}

			public bool HasW
			{
				get
				{
					return this.result.hasW;
				}
			}

			public bool HasX
			{
				get
				{
					return this.result.hasX;
				}
			}

			public bool HasY
			{
				get
				{
					return this.result.hasY;
				}
			}

			public bool HasZ
			{
				get
				{
					return this.result.hasZ;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override RustProto.Quaternion MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override RustProto.Quaternion.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public float W
			{
				get
				{
					return this.result.W;
				}
				set
				{
					this.SetW(value);
				}
			}

			public float X
			{
				get
				{
					return this.result.X;
				}
				set
				{
					this.SetX(value);
				}
			}

			public float Y
			{
				get
				{
					return this.result.Y;
				}
				set
				{
					this.SetY(value);
				}
			}

			public float Z
			{
				get
				{
					return this.result.Z;
				}
				set
				{
					this.SetZ(value);
				}
			}

			public Builder()
			{
				this.result = RustProto.Quaternion.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(RustProto.Quaternion cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override RustProto.Quaternion BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override RustProto.Quaternion.Builder Clear()
			{
				this.result = RustProto.Quaternion.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public RustProto.Quaternion.Builder ClearW()
			{
				this.PrepareBuilder();
				this.result.hasW = false;
				this.result.w_ = 0f;
				return this;
			}

			public RustProto.Quaternion.Builder ClearX()
			{
				this.PrepareBuilder();
				this.result.hasX = false;
				this.result.x_ = 0f;
				return this;
			}

			public RustProto.Quaternion.Builder ClearY()
			{
				this.PrepareBuilder();
				this.result.hasY = false;
				this.result.y_ = 0f;
				return this;
			}

			public RustProto.Quaternion.Builder ClearZ()
			{
				this.PrepareBuilder();
				this.result.hasZ = false;
				this.result.z_ = 0f;
				return this;
			}

			public override RustProto.Quaternion.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new RustProto.Quaternion.Builder(this.result);
				}
				return (new RustProto.Quaternion.Builder()).MergeFrom(this.result);
			}

			public override RustProto.Quaternion.Builder MergeFrom(IMessage other)
			{
				if (other is RustProto.Quaternion)
				{
					return this.MergeFrom((RustProto.Quaternion)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override RustProto.Quaternion.Builder MergeFrom(RustProto.Quaternion other)
			{
				if (other == RustProto.Quaternion.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasX)
				{
					this.X = other.X;
				}
				if (other.HasY)
				{
					this.Y = other.Y;
				}
				if (other.HasZ)
				{
					this.Z = other.Z;
				}
				if (other.HasW)
				{
					this.W = other.W;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override RustProto.Quaternion.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override RustProto.Quaternion.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(RustProto.Quaternion._quaternionFieldNames, str, StringComparer.Ordinal);
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
							num = RustProto.Quaternion._quaternionFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 13)
					{
						this.result.hasX = input.ReadFloat(ref this.result.x_);
					}
					else if (num2 == 21)
					{
						this.result.hasY = input.ReadFloat(ref this.result.y_);
					}
					else if (num2 == 29)
					{
						this.result.hasZ = input.ReadFloat(ref this.result.z_);
					}
					else if (num2 == 37)
					{
						this.result.hasW = input.ReadFloat(ref this.result.w_);
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

			private RustProto.Quaternion PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					RustProto.Quaternion quaternion = this.result;
					this.result = new RustProto.Quaternion();
					this.resultIsReadOnly = false;
					this.MergeFrom(quaternion);
				}
				return this.result;
			}

			public void Set(UnityEngine.Quaternion value)
			{
				this.SetX(value.x);
				this.SetY(value.y);
				this.SetZ(value.z);
				this.SetW(value.w);
			}

			public RustProto.Quaternion.Builder SetW(float value)
			{
				this.PrepareBuilder();
				this.result.hasW = true;
				this.result.w_ = value;
				return this;
			}

			public RustProto.Quaternion.Builder SetX(float value)
			{
				this.PrepareBuilder();
				this.result.hasX = true;
				this.result.x_ = value;
				return this;
			}

			public RustProto.Quaternion.Builder SetY(float value)
			{
				this.PrepareBuilder();
				this.result.hasY = true;
				this.result.y_ = value;
				return this;
			}

			public RustProto.Quaternion.Builder SetZ(float value)
			{
				this.PrepareBuilder();
				this.result.hasZ = true;
				this.result.z_ = value;
				return this;
			}
		}
	}
}