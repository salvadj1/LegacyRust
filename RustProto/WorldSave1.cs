using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Collections;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class WorldSave : GeneratedMessage<WorldSave, WorldSave.Builder>
	{
		public const int SceneObjectFieldNumber = 1;

		public const int InstanceObjectFieldNumber = 2;

		private readonly static WorldSave defaultInstance;

		private readonly static string[] _worldSaveFieldNames;

		private readonly static uint[] _worldSaveFieldTags;

		private PopsicleList<SavedObject> sceneObject_ = new PopsicleList<SavedObject>();

		private PopsicleList<SavedObject> instanceObject_ = new PopsicleList<SavedObject>();

		private int memoizedSerializedSize = -1;

		public static WorldSave DefaultInstance
		{
			get
			{
				return WorldSave.defaultInstance;
			}
		}

		public override WorldSave DefaultInstanceForType
		{
			get
			{
				return WorldSave.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_WorldSave__Descriptor;
			}
		}

		public int InstanceObjectCount
		{
			get
			{
				return this.instanceObject_.Count;
			}
		}

		public IList<SavedObject> InstanceObjectList
		{
			get
			{
				return this.instanceObject_;
			}
		}

		protected override FieldAccessorTable<WorldSave, WorldSave.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_WorldSave__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				bool flag;
				IEnumerator<SavedObject> enumerator = this.SceneObjectList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsInitialized)
						{
							continue;
						}
						flag = false;
						return flag;
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				IEnumerator<SavedObject> enumerator1 = this.InstanceObjectList.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						if (enumerator1.Current.IsInitialized)
						{
							continue;
						}
						flag = false;
						return flag;
					}
					return true;
				}
				finally
				{
					if (enumerator1 == null)
					{
					}
					enumerator1.Dispose();
				}
				return flag;
			}
		}

		public int SceneObjectCount
		{
			get
			{
				return this.sceneObject_.Count;
			}
		}

		public IList<SavedObject> SceneObjectList
		{
			get
			{
				return this.sceneObject_;
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
				IEnumerator<SavedObject> enumerator = this.SceneObjectList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						SavedObject current = enumerator.Current;
						serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(1, current);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				IEnumerator<SavedObject> enumerator1 = this.InstanceObjectList.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						SavedObject savedObject = enumerator1.Current;
						serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(2, savedObject);
					}
				}
				finally
				{
					if (enumerator1 == null)
					{
					}
					enumerator1.Dispose();
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override WorldSave ThisMessage
		{
			get
			{
				return this;
			}
		}

		static WorldSave()
		{
			WorldSave.defaultInstance = (new WorldSave()).MakeReadOnly();
			WorldSave._worldSaveFieldNames = new string[] { "instanceObject", "sceneObject" };
			WorldSave._worldSaveFieldTags = new uint[] { 18, 10 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private WorldSave()
		{
		}

		public static WorldSave.Builder CreateBuilder()
		{
			return new WorldSave.Builder();
		}

		public static WorldSave.Builder CreateBuilder(WorldSave prototype)
		{
			return new WorldSave.Builder(prototype);
		}

		public override WorldSave.Builder CreateBuilderForType()
		{
			return new WorldSave.Builder();
		}

		public SavedObject GetInstanceObject(int index)
		{
			return this.instanceObject_[index];
		}

		public SavedObject GetSceneObject(int index)
		{
			return this.sceneObject_[index];
		}

		private WorldSave MakeReadOnly()
		{
			this.sceneObject_.MakeReadOnly();
			this.instanceObject_.MakeReadOnly();
			return this;
		}

		public static WorldSave ParseDelimitedFrom(Stream input)
		{
			return WorldSave.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static WorldSave ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return WorldSave.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static WorldSave ParseFrom(ByteString data)
		{
			return WorldSave.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static WorldSave ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return WorldSave.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static WorldSave ParseFrom(byte[] data)
		{
			return WorldSave.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static WorldSave ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return WorldSave.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static WorldSave ParseFrom(Stream input)
		{
			return WorldSave.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static WorldSave ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return WorldSave.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static WorldSave ParseFrom(ICodedInputStream input)
		{
			return WorldSave.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static WorldSave ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return WorldSave.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<WorldSave, WorldSave.Builder> Recycler()
		{
			return Recycler<WorldSave, WorldSave.Builder>.Manufacture();
		}

		public override WorldSave.Builder ToBuilder()
		{
			return WorldSave.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = WorldSave._worldSaveFieldNames;
			if (this.sceneObject_.Count > 0)
			{
				output.WriteMessageArray<SavedObject>(1, strArrays[1], this.sceneObject_);
			}
			if (this.instanceObject_.Count > 0)
			{
				output.WriteMessageArray<SavedObject>(2, strArrays[0], this.instanceObject_);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<WorldSave, WorldSave.Builder>
		{
			private bool resultIsReadOnly;

			private WorldSave result;

			public override WorldSave DefaultInstanceForType
			{
				get
				{
					return WorldSave.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return WorldSave.Descriptor;
				}
			}

			public int InstanceObjectCount
			{
				get
				{
					return this.result.InstanceObjectCount;
				}
			}

			public IPopsicleList<SavedObject> InstanceObjectList
			{
				get
				{
					return this.PrepareBuilder().instanceObject_;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override WorldSave MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public int SceneObjectCount
			{
				get
				{
					return this.result.SceneObjectCount;
				}
			}

			public IPopsicleList<SavedObject> SceneObjectList
			{
				get
				{
					return this.PrepareBuilder().sceneObject_;
				}
			}

			protected override WorldSave.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = WorldSave.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(WorldSave cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public WorldSave.Builder AddInstanceObject(SavedObject value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.instanceObject_.Add(value);
				return this;
			}

			public WorldSave.Builder AddInstanceObject(SavedObject.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.instanceObject_.Add(builderForValue.Build());
				return this;
			}

			public WorldSave.Builder AddRangeInstanceObject(IEnumerable<SavedObject> values)
			{
				this.PrepareBuilder();
				this.result.instanceObject_.Add(values);
				return this;
			}

			public WorldSave.Builder AddRangeSceneObject(IEnumerable<SavedObject> values)
			{
				this.PrepareBuilder();
				this.result.sceneObject_.Add(values);
				return this;
			}

			public WorldSave.Builder AddSceneObject(SavedObject value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.sceneObject_.Add(value);
				return this;
			}

			public WorldSave.Builder AddSceneObject(SavedObject.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.sceneObject_.Add(builderForValue.Build());
				return this;
			}

			public override WorldSave BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override WorldSave.Builder Clear()
			{
				this.result = WorldSave.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public WorldSave.Builder ClearInstanceObject()
			{
				this.PrepareBuilder();
				this.result.instanceObject_.Clear();
				return this;
			}

			public WorldSave.Builder ClearSceneObject()
			{
				this.PrepareBuilder();
				this.result.sceneObject_.Clear();
				return this;
			}

			public override WorldSave.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new WorldSave.Builder(this.result);
				}
				return (new WorldSave.Builder()).MergeFrom(this.result);
			}

			public SavedObject GetInstanceObject(int index)
			{
				return this.result.GetInstanceObject(index);
			}

			public SavedObject GetSceneObject(int index)
			{
				return this.result.GetSceneObject(index);
			}

			public override WorldSave.Builder MergeFrom(IMessage other)
			{
				if (other is WorldSave)
				{
					return this.MergeFrom((WorldSave)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override WorldSave.Builder MergeFrom(WorldSave other)
			{
				if (other == WorldSave.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.sceneObject_.Count != 0)
				{
					this.result.sceneObject_.Add(other.sceneObject_);
				}
				if (other.instanceObject_.Count != 0)
				{
					this.result.instanceObject_.Add(other.instanceObject_);
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override WorldSave.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override WorldSave.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(WorldSave._worldSaveFieldNames, str, StringComparer.Ordinal);
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
							num = WorldSave._worldSaveFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 10)
					{
						input.ReadMessageArray<SavedObject>(num, str, this.result.sceneObject_, SavedObject.DefaultInstance, extensionRegistry);
					}
					else if (num2 == 18)
					{
						input.ReadMessageArray<SavedObject>(num, str, this.result.instanceObject_, SavedObject.DefaultInstance, extensionRegistry);
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

			private WorldSave PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					WorldSave worldSave = this.result;
					this.result = new WorldSave();
					this.resultIsReadOnly = false;
					this.MergeFrom(worldSave);
				}
				return this.result;
			}

			public WorldSave.Builder SetInstanceObject(int index, SavedObject value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.instanceObject_[index] = value;
				return this;
			}

			public WorldSave.Builder SetInstanceObject(int index, SavedObject.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.instanceObject_[index] = builderForValue.Build();
				return this;
			}

			public WorldSave.Builder SetSceneObject(int index, SavedObject value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.sceneObject_[index] = value;
				return this;
			}

			public WorldSave.Builder SetSceneObject(int index, SavedObject.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.sceneObject_[index] = builderForValue.Build();
				return this;
			}
		}
	}
}