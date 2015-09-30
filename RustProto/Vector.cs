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
	public sealed class Vector : GeneratedMessage<Vector, Vector.Builder>
	{
		public const int XFieldNumber = 1;

		public const int YFieldNumber = 2;

		public const int ZFieldNumber = 3;

		private readonly static Vector defaultInstance;

		private readonly static string[] _vectorFieldNames;

		private readonly static uint[] _vectorFieldTags;

		private bool hasX;

		private float x_;

		private bool hasY;

		private float y_;

		private bool hasZ;

		private float z_;

		private int memoizedSerializedSize = -1;

		public static Vector DefaultInstance
		{
			get
			{
				return Vector.defaultInstance;
			}
		}

		public override Vector DefaultInstanceForType
		{
			get
			{
				return Vector.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Common.internal__static_RustProto_Vector__Descriptor;
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

		protected override FieldAccessorTable<Vector, Vector.Builder> InternalFieldAccessors
		{
			get
			{
				return Common.internal__static_RustProto_Vector__FieldAccessorTable;
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
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override Vector ThisMessage
		{
			get
			{
				return this;
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

		static Vector()
		{
			Vector.defaultInstance = (new Vector()).MakeReadOnly();
			Vector._vectorFieldNames = new string[] { "x", "y", "z" };
			Vector._vectorFieldTags = new uint[] { 13, 21, 29 };
			object.ReferenceEquals(Common.Descriptor, null);
		}

		private Vector()
		{
		}

		public static Vector.Builder CreateBuilder()
		{
			return new Vector.Builder();
		}

		public static Vector.Builder CreateBuilder(Vector prototype)
		{
			return new Vector.Builder(prototype);
		}

		public override Vector.Builder CreateBuilderForType()
		{
			return new Vector.Builder();
		}

		private Vector MakeReadOnly()
		{
			return this;
		}

		public static implicit operator Vector(Vector3 v)
		{
			Vector vector;
			using (Recycler<Vector, Vector.Builder> recycler = Vector.Recycler())
			{
				Vector.Builder builder = recycler.OpenBuilder();
				builder.SetX(v.x);
				builder.SetY(v.y);
				builder.SetZ(v.z);
				vector = builder.Build();
			}
			return vector;
		}

		public static Vector ParseDelimitedFrom(Stream input)
		{
			return Vector.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static Vector ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return Vector.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static Vector ParseFrom(ByteString data)
		{
			return Vector.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static Vector ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return Vector.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static Vector ParseFrom(byte[] data)
		{
			return Vector.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static Vector ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return Vector.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static Vector ParseFrom(Stream input)
		{
			return Vector.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static Vector ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return Vector.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Vector ParseFrom(ICodedInputStream input)
		{
			return Vector.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static Vector ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return Vector.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<Vector, Vector.Builder> Recycler()
		{
			return Recycler<Vector, Vector.Builder>.Manufacture();
		}

		public override Vector.Builder ToBuilder()
		{
			return Vector.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = Vector._vectorFieldNames;
			if (this.hasX)
			{
				output.WriteFloat(1, strArrays[0], this.X);
			}
			if (this.hasY)
			{
				output.WriteFloat(2, strArrays[1], this.Y);
			}
			if (this.hasZ)
			{
				output.WriteFloat(3, strArrays[2], this.Z);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<Vector, Vector.Builder>
		{
			private bool resultIsReadOnly;

			private Vector result;

			public override Vector DefaultInstanceForType
			{
				get
				{
					return Vector.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return Vector.Descriptor;
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

			protected override Vector MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			protected override Vector.Builder ThisBuilder
			{
				get
				{
					return this;
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
				this.result = Vector.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(Vector cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override Vector BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override Vector.Builder Clear()
			{
				this.result = Vector.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public Vector.Builder ClearX()
			{
				this.PrepareBuilder();
				this.result.hasX = false;
				this.result.x_ = 0f;
				return this;
			}

			public Vector.Builder ClearY()
			{
				this.PrepareBuilder();
				this.result.hasY = false;
				this.result.y_ = 0f;
				return this;
			}

			public Vector.Builder ClearZ()
			{
				this.PrepareBuilder();
				this.result.hasZ = false;
				this.result.z_ = 0f;
				return this;
			}

			public override Vector.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new Vector.Builder(this.result);
				}
				return (new Vector.Builder()).MergeFrom(this.result);
			}

			public override Vector.Builder MergeFrom(IMessage other)
			{
				if (other is Vector)
				{
					return this.MergeFrom((Vector)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override Vector.Builder MergeFrom(Vector other)
			{
				if (other == Vector.DefaultInstance)
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
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override Vector.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override Vector.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(Vector._vectorFieldNames, str, StringComparer.Ordinal);
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
							num = Vector._vectorFieldTags[num1];
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

			private Vector PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					Vector vector = this.result;
					this.result = new Vector();
					this.resultIsReadOnly = false;
					this.MergeFrom(vector);
				}
				return this.result;
			}

			public void Set(Vector3 value)
			{
				this.SetX(value.x);
				this.SetY(value.y);
				this.SetZ(value.z);
			}

			public Vector.Builder SetX(float value)
			{
				this.PrepareBuilder();
				this.result.hasX = true;
				this.result.x_ = value;
				return this;
			}

			public Vector.Builder SetY(float value)
			{
				this.PrepareBuilder();
				this.result.hasY = true;
				this.result.y_ = value;
				return this;
			}

			public Vector.Builder SetZ(float value)
			{
				this.PrepareBuilder();
				this.result.hasZ = true;
				this.result.z_ = value;
				return this;
			}
		}
	}
}