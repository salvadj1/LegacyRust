using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Collections;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Helpers;
using RustProto.Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class Item : GeneratedMessage<RustProto.Item, RustProto.Item.Builder>
	{
		public const int IdFieldNumber = 1;

		public const int NameFieldNumber = 2;

		public const int SlotFieldNumber = 3;

		public const int CountFieldNumber = 4;

		public const int SubslotsFieldNumber = 6;

		public const int ConditionFieldNumber = 7;

		public const int MaxconditionFieldNumber = 8;

		public const int SubitemFieldNumber = 5;

		private readonly static RustProto.Item defaultInstance;

		private readonly static string[] _itemFieldNames;

		private readonly static uint[] _itemFieldTags;

		private bool hasId;

		private int id_;

		private bool hasName;

		private string name_ = string.Empty;

		private bool hasSlot;

		private int slot_;

		private bool hasCount;

		private int count_;

		private bool hasSubslots;

		private int subslots_;

		private bool hasCondition;

		private float condition_;

		private bool hasMaxcondition;

		private float maxcondition_;

		private PopsicleList<RustProto.Item> subitem_ = new PopsicleList<RustProto.Item>();

		private int memoizedSerializedSize = -1;

		public float Condition
		{
			get
			{
				return this.condition_;
			}
		}

		public int Count
		{
			get
			{
				return this.count_;
			}
		}

		public static RustProto.Item DefaultInstance
		{
			get
			{
				return RustProto.Item.defaultInstance;
			}
		}

		public override RustProto.Item DefaultInstanceForType
		{
			get
			{
				return RustProto.Item.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Item.internal__static_RustProto_Item__Descriptor;
			}
		}

		public bool HasCondition
		{
			get
			{
				return this.hasCondition;
			}
		}

		public bool HasCount
		{
			get
			{
				return this.hasCount;
			}
		}

		public bool HasId
		{
			get
			{
				return this.hasId;
			}
		}

		public bool HasMaxcondition
		{
			get
			{
				return this.hasMaxcondition;
			}
		}

		public bool HasName
		{
			get
			{
				return this.hasName;
			}
		}

		public bool HasSlot
		{
			get
			{
				return this.hasSlot;
			}
		}

		public bool HasSubslots
		{
			get
			{
				return this.hasSubslots;
			}
		}

		public int Id
		{
			get
			{
				return this.id_;
			}
		}

		protected override FieldAccessorTable<RustProto.Item, RustProto.Item.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.Item.internal__static_RustProto_Item__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				bool flag;
				if (!this.hasId)
				{
					return false;
				}
				IEnumerator<RustProto.Item> enumerator = this.SubitemList.GetEnumerator();
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
					return true;
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				return flag;
			}
		}

		public float Maxcondition
		{
			get
			{
				return this.maxcondition_;
			}
		}

		public string Name
		{
			get
			{
				return this.name_;
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
				if (this.hasName)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeStringSize(2, this.Name);
				}
				if (this.hasSlot)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(3, this.Slot);
				}
				if (this.hasCount)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(4, this.Count);
				}
				if (this.hasSubslots)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(6, this.Subslots);
				}
				if (this.hasCondition)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(7, this.Condition);
				}
				if (this.hasMaxcondition)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(8, this.Maxcondition);
				}
				IEnumerator<RustProto.Item> enumerator = this.SubitemList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						RustProto.Item current = enumerator.Current;
						serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(5, current);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		public int Slot
		{
			get
			{
				return this.slot_;
			}
		}

		public int SubitemCount
		{
			get
			{
				return this.subitem_.Count;
			}
		}

		public IList<RustProto.Item> SubitemList
		{
			get
			{
				return this.subitem_;
			}
		}

		public int Subslots
		{
			get
			{
				return this.subslots_;
			}
		}

		protected override RustProto.Item ThisMessage
		{
			get
			{
				return this;
			}
		}

		static Item()
		{
			RustProto.Item.defaultInstance = (new RustProto.Item()).MakeReadOnly();
			RustProto.Item._itemFieldNames = new string[] { "condition", "count", "id", "maxcondition", "name", "slot", "subitem", "subslots" };
			RustProto.Item._itemFieldTags = new uint[] { 61, 32, 8, 69, 18, 24, 42, 48 };
			object.ReferenceEquals(RustProto.Proto.Item.Descriptor, null);
		}

		private Item()
		{
		}

		public static RustProto.Item.Builder CreateBuilder()
		{
			return new RustProto.Item.Builder();
		}

		public static RustProto.Item.Builder CreateBuilder(RustProto.Item prototype)
		{
			return new RustProto.Item.Builder(prototype);
		}

		public override RustProto.Item.Builder CreateBuilderForType()
		{
			return new RustProto.Item.Builder();
		}

		public RustProto.Item GetSubitem(int index)
		{
			return this.subitem_[index];
		}

		private RustProto.Item MakeReadOnly()
		{
			this.subitem_.MakeReadOnly();
			return this;
		}

		public static RustProto.Item ParseDelimitedFrom(Stream input)
		{
			return RustProto.Item.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static RustProto.Item ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Item.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Item ParseFrom(ByteString data)
		{
			return RustProto.Item.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Item ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Item.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Item ParseFrom(byte[] data)
		{
			return RustProto.Item.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Item ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Item.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Item ParseFrom(Stream input)
		{
			return RustProto.Item.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Item ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Item.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Item ParseFrom(ICodedInputStream input)
		{
			return RustProto.Item.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Item ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Item.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<RustProto.Item, RustProto.Item.Builder> Recycler()
		{
			return Recycler<RustProto.Item, RustProto.Item.Builder>.Manufacture();
		}

		public override RustProto.Item.Builder ToBuilder()
		{
			return RustProto.Item.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = RustProto.Item._itemFieldNames;
			if (this.hasId)
			{
				output.WriteInt32(1, strArrays[2], this.Id);
			}
			if (this.hasName)
			{
				output.WriteString(2, strArrays[4], this.Name);
			}
			if (this.hasSlot)
			{
				output.WriteInt32(3, strArrays[5], this.Slot);
			}
			if (this.hasCount)
			{
				output.WriteInt32(4, strArrays[1], this.Count);
			}
			if (this.subitem_.Count > 0)
			{
				output.WriteMessageArray<RustProto.Item>(5, strArrays[6], this.subitem_);
			}
			if (this.hasSubslots)
			{
				output.WriteInt32(6, strArrays[7], this.Subslots);
			}
			if (this.hasCondition)
			{
				output.WriteFloat(7, strArrays[0], this.Condition);
			}
			if (this.hasMaxcondition)
			{
				output.WriteFloat(8, strArrays[3], this.Maxcondition);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<RustProto.Item, RustProto.Item.Builder>
		{
			private bool resultIsReadOnly;

			private RustProto.Item result;

			public float Condition
			{
				get
				{
					return this.result.Condition;
				}
				set
				{
					this.SetCondition(value);
				}
			}

			public int Count
			{
				get
				{
					return this.result.Count;
				}
				set
				{
					this.SetCount(value);
				}
			}

			public override RustProto.Item DefaultInstanceForType
			{
				get
				{
					return RustProto.Item.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return RustProto.Item.Descriptor;
				}
			}

			public bool HasCondition
			{
				get
				{
					return this.result.hasCondition;
				}
			}

			public bool HasCount
			{
				get
				{
					return this.result.hasCount;
				}
			}

			public bool HasId
			{
				get
				{
					return this.result.hasId;
				}
			}

			public bool HasMaxcondition
			{
				get
				{
					return this.result.hasMaxcondition;
				}
			}

			public bool HasName
			{
				get
				{
					return this.result.hasName;
				}
			}

			public bool HasSlot
			{
				get
				{
					return this.result.hasSlot;
				}
			}

			public bool HasSubslots
			{
				get
				{
					return this.result.hasSubslots;
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

			public float Maxcondition
			{
				get
				{
					return this.result.Maxcondition;
				}
				set
				{
					this.SetMaxcondition(value);
				}
			}

			protected override RustProto.Item MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public string Name
			{
				get
				{
					return this.result.Name;
				}
				set
				{
					this.SetName(value);
				}
			}

			public int Slot
			{
				get
				{
					return this.result.Slot;
				}
				set
				{
					this.SetSlot(value);
				}
			}

			public int SubitemCount
			{
				get
				{
					return this.result.SubitemCount;
				}
			}

			public IPopsicleList<RustProto.Item> SubitemList
			{
				get
				{
					return this.PrepareBuilder().subitem_;
				}
			}

			public int Subslots
			{
				get
				{
					return this.result.Subslots;
				}
				set
				{
					this.SetSubslots(value);
				}
			}

			protected override RustProto.Item.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = RustProto.Item.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(RustProto.Item cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public RustProto.Item.Builder AddRangeSubitem(IEnumerable<RustProto.Item> values)
			{
				this.PrepareBuilder();
				this.result.subitem_.Add(values);
				return this;
			}

			public RustProto.Item.Builder AddSubitem(RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.subitem_.Add(value);
				return this;
			}

			public RustProto.Item.Builder AddSubitem(RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.subitem_.Add(builderForValue.Build());
				return this;
			}

			public override RustProto.Item BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override RustProto.Item.Builder Clear()
			{
				this.result = RustProto.Item.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public RustProto.Item.Builder ClearCondition()
			{
				this.PrepareBuilder();
				this.result.hasCondition = false;
				this.result.condition_ = 0f;
				return this;
			}

			public RustProto.Item.Builder ClearCount()
			{
				this.PrepareBuilder();
				this.result.hasCount = false;
				this.result.count_ = 0;
				return this;
			}

			public RustProto.Item.Builder ClearId()
			{
				this.PrepareBuilder();
				this.result.hasId = false;
				this.result.id_ = 0;
				return this;
			}

			public RustProto.Item.Builder ClearMaxcondition()
			{
				this.PrepareBuilder();
				this.result.hasMaxcondition = false;
				this.result.maxcondition_ = 0f;
				return this;
			}

			public RustProto.Item.Builder ClearName()
			{
				this.PrepareBuilder();
				this.result.hasName = false;
				this.result.name_ = string.Empty;
				return this;
			}

			public RustProto.Item.Builder ClearSlot()
			{
				this.PrepareBuilder();
				this.result.hasSlot = false;
				this.result.slot_ = 0;
				return this;
			}

			public RustProto.Item.Builder ClearSubitem()
			{
				this.PrepareBuilder();
				this.result.subitem_.Clear();
				return this;
			}

			public RustProto.Item.Builder ClearSubslots()
			{
				this.PrepareBuilder();
				this.result.hasSubslots = false;
				this.result.subslots_ = 0;
				return this;
			}

			public override RustProto.Item.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new RustProto.Item.Builder(this.result);
				}
				return (new RustProto.Item.Builder()).MergeFrom(this.result);
			}

			public RustProto.Item GetSubitem(int index)
			{
				return this.result.GetSubitem(index);
			}

			public override RustProto.Item.Builder MergeFrom(IMessage other)
			{
				if (other is RustProto.Item)
				{
					return this.MergeFrom((RustProto.Item)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override RustProto.Item.Builder MergeFrom(RustProto.Item other)
			{
				if (other == RustProto.Item.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasId)
				{
					this.Id = other.Id;
				}
				if (other.HasName)
				{
					this.Name = other.Name;
				}
				if (other.HasSlot)
				{
					this.Slot = other.Slot;
				}
				if (other.HasCount)
				{
					this.Count = other.Count;
				}
				if (other.HasSubslots)
				{
					this.Subslots = other.Subslots;
				}
				if (other.HasCondition)
				{
					this.Condition = other.Condition;
				}
				if (other.HasMaxcondition)
				{
					this.Maxcondition = other.Maxcondition;
				}
				if (other.subitem_.Count != 0)
				{
					this.result.subitem_.Add(other.subitem_);
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override RustProto.Item.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override RustProto.Item.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(RustProto.Item._itemFieldNames, str, StringComparer.Ordinal);
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
							num = RustProto.Item._itemFieldTags[num1];
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
					else if (num2 == 18)
					{
						this.result.hasName = input.ReadString(ref this.result.name_);
					}
					else if (num2 == 24)
					{
						this.result.hasSlot = input.ReadInt32(ref this.result.slot_);
					}
					else if (num2 == 32)
					{
						this.result.hasCount = input.ReadInt32(ref this.result.count_);
					}
					else if (num2 == 42)
					{
						input.ReadMessageArray<RustProto.Item>(num, str, this.result.subitem_, RustProto.Item.DefaultInstance, extensionRegistry);
					}
					else if (num2 == 48)
					{
						this.result.hasSubslots = input.ReadInt32(ref this.result.subslots_);
					}
					else if (num2 == 61)
					{
						this.result.hasCondition = input.ReadFloat(ref this.result.condition_);
					}
					else if (num2 == 69)
					{
						this.result.hasMaxcondition = input.ReadFloat(ref this.result.maxcondition_);
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

			private RustProto.Item PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					RustProto.Item item = this.result;
					this.result = new RustProto.Item();
					this.resultIsReadOnly = false;
					this.MergeFrom(item);
				}
				return this.result;
			}

			public RustProto.Item.Builder SetCondition(float value)
			{
				this.PrepareBuilder();
				this.result.hasCondition = true;
				this.result.condition_ = value;
				return this;
			}

			public RustProto.Item.Builder SetCount(int value)
			{
				this.PrepareBuilder();
				this.result.hasCount = true;
				this.result.count_ = value;
				return this;
			}

			public RustProto.Item.Builder SetId(int value)
			{
				this.PrepareBuilder();
				this.result.hasId = true;
				this.result.id_ = value;
				return this;
			}

			public RustProto.Item.Builder SetMaxcondition(float value)
			{
				this.PrepareBuilder();
				this.result.hasMaxcondition = true;
				this.result.maxcondition_ = value;
				return this;
			}

			public RustProto.Item.Builder SetName(string value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasName = true;
				this.result.name_ = value;
				return this;
			}

			public RustProto.Item.Builder SetSlot(int value)
			{
				this.PrepareBuilder();
				this.result.hasSlot = true;
				this.result.slot_ = value;
				return this;
			}

			public RustProto.Item.Builder SetSubitem(int index, RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.subitem_[index] = value;
				return this;
			}

			public RustProto.Item.Builder SetSubitem(int index, RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.subitem_[index] = builderForValue.Build();
				return this;
			}

			public RustProto.Item.Builder SetSubslots(int value)
			{
				this.PrepareBuilder();
				this.result.hasSubslots = true;
				this.result.subslots_ = value;
				return this;
			}
		}
	}
}