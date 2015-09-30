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
	public sealed class Avatar : GeneratedMessage<RustProto.Avatar, RustProto.Avatar.Builder>
	{
		public const int PosFieldNumber = 1;

		public const int AngFieldNumber = 2;

		public const int VitalsFieldNumber = 3;

		public const int BlueprintsFieldNumber = 4;

		public const int InventoryFieldNumber = 5;

		public const int WearableFieldNumber = 6;

		public const int BeltFieldNumber = 7;

		public const int AwayEventFieldNumber = 8;

		private readonly static RustProto.Avatar defaultInstance;

		private readonly static string[] _avatarFieldNames;

		private readonly static uint[] _avatarFieldTags;

		private bool hasPos;

		private Vector pos_;

		private bool hasAng;

		private Quaternion ang_;

		private bool hasVitals;

		private RustProto.Vitals vitals_;

		private PopsicleList<RustProto.Blueprint> blueprints_ = new PopsicleList<RustProto.Blueprint>();

		private PopsicleList<RustProto.Item> inventory_ = new PopsicleList<RustProto.Item>();

		private PopsicleList<RustProto.Item> wearable_ = new PopsicleList<RustProto.Item>();

		private PopsicleList<RustProto.Item> belt_ = new PopsicleList<RustProto.Item>();

		private bool hasAwayEvent;

		private RustProto.AwayEvent awayEvent_;

		private int memoizedSerializedSize = -1;

		public Quaternion Ang
		{
			get
			{
				return this.ang_ ?? Quaternion.DefaultInstance;
			}
		}

		public RustProto.AwayEvent AwayEvent
		{
			get
			{
				return this.awayEvent_ ?? RustProto.AwayEvent.DefaultInstance;
			}
		}

		public int BeltCount
		{
			get
			{
				return this.belt_.Count;
			}
		}

		public IList<RustProto.Item> BeltList
		{
			get
			{
				return this.belt_;
			}
		}

		public int BlueprintsCount
		{
			get
			{
				return this.blueprints_.Count;
			}
		}

		public IList<RustProto.Blueprint> BlueprintsList
		{
			get
			{
				return this.blueprints_;
			}
		}

		public static RustProto.Avatar DefaultInstance
		{
			get
			{
				return RustProto.Avatar.defaultInstance;
			}
		}

		public override RustProto.Avatar DefaultInstanceForType
		{
			get
			{
				return RustProto.Avatar.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Avatar.internal__static_RustProto_Avatar__Descriptor;
			}
		}

		public bool HasAng
		{
			get
			{
				return this.hasAng;
			}
		}

		public bool HasAwayEvent
		{
			get
			{
				return this.hasAwayEvent;
			}
		}

		public bool HasPos
		{
			get
			{
				return this.hasPos;
			}
		}

		public bool HasVitals
		{
			get
			{
				return this.hasVitals;
			}
		}

		protected override FieldAccessorTable<RustProto.Avatar, RustProto.Avatar.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.Avatar.internal__static_RustProto_Avatar__FieldAccessorTable;
			}
		}

		public int InventoryCount
		{
			get
			{
				return this.inventory_.Count;
			}
		}

		public IList<RustProto.Item> InventoryList
		{
			get
			{
				return this.inventory_;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				bool flag;
				IEnumerator<RustProto.Blueprint> enumerator = this.BlueprintsList.GetEnumerator();
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
				IEnumerator<RustProto.Item> enumerator1 = this.InventoryList.GetEnumerator();
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
				}
				finally
				{
					if (enumerator1 == null)
					{
					}
					enumerator1.Dispose();
				}
				IEnumerator<RustProto.Item> enumerator2 = this.WearableList.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.IsInitialized)
						{
							continue;
						}
						flag = false;
						return flag;
					}
				}
				finally
				{
					if (enumerator2 == null)
					{
					}
					enumerator2.Dispose();
				}
				IEnumerator<RustProto.Item> enumerator3 = this.BeltList.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.IsInitialized)
						{
							continue;
						}
						flag = false;
						return flag;
					}
					if (this.HasAwayEvent && !this.AwayEvent.IsInitialized)
					{
						return false;
					}
					return true;
				}
				finally
				{
					if (enumerator3 == null)
					{
					}
					enumerator3.Dispose();
				}
				return flag;
			}
		}

		public Vector Pos
		{
			get
			{
				return this.pos_ ?? Vector.DefaultInstance;
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
				if (this.hasAng)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(2, this.Ang);
				}
				if (this.hasVitals)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(3, this.Vitals);
				}
				IEnumerator<RustProto.Blueprint> enumerator = this.BlueprintsList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						RustProto.Blueprint current = enumerator.Current;
						serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(4, current);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				IEnumerator<RustProto.Item> enumerator1 = this.InventoryList.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						RustProto.Item item = enumerator1.Current;
						serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(5, item);
					}
				}
				finally
				{
					if (enumerator1 == null)
					{
					}
					enumerator1.Dispose();
				}
				IEnumerator<RustProto.Item> enumerator2 = this.WearableList.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						RustProto.Item current1 = enumerator2.Current;
						serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(6, current1);
					}
				}
				finally
				{
					if (enumerator2 == null)
					{
					}
					enumerator2.Dispose();
				}
				IEnumerator<RustProto.Item> enumerator3 = this.BeltList.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						RustProto.Item item1 = enumerator3.Current;
						serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(7, item1);
					}
				}
				finally
				{
					if (enumerator3 == null)
					{
					}
					enumerator3.Dispose();
				}
				if (this.hasAwayEvent)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(8, this.AwayEvent);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		protected override RustProto.Avatar ThisMessage
		{
			get
			{
				return this;
			}
		}

		public RustProto.Vitals Vitals
		{
			get
			{
				return this.vitals_ ?? RustProto.Vitals.DefaultInstance;
			}
		}

		public int WearableCount
		{
			get
			{
				return this.wearable_.Count;
			}
		}

		public IList<RustProto.Item> WearableList
		{
			get
			{
				return this.wearable_;
			}
		}

		static Avatar()
		{
			RustProto.Avatar.defaultInstance = (new RustProto.Avatar()).MakeReadOnly();
			RustProto.Avatar._avatarFieldNames = new string[] { "ang", "awayEvent", "belt", "blueprints", "inventory", "pos", "vitals", "wearable" };
			RustProto.Avatar._avatarFieldTags = new uint[] { 18, 66, 58, 34, 42, 10, 26, 50 };
			object.ReferenceEquals(RustProto.Proto.Avatar.Descriptor, null);
		}

		private Avatar()
		{
		}

		public static RustProto.Avatar.Builder CreateBuilder()
		{
			return new RustProto.Avatar.Builder();
		}

		public static RustProto.Avatar.Builder CreateBuilder(RustProto.Avatar prototype)
		{
			return new RustProto.Avatar.Builder(prototype);
		}

		public override RustProto.Avatar.Builder CreateBuilderForType()
		{
			return new RustProto.Avatar.Builder();
		}

		public RustProto.Item GetBelt(int index)
		{
			return this.belt_[index];
		}

		public RustProto.Blueprint GetBlueprints(int index)
		{
			return this.blueprints_[index];
		}

		public RustProto.Item GetInventory(int index)
		{
			return this.inventory_[index];
		}

		public RustProto.Item GetWearable(int index)
		{
			return this.wearable_[index];
		}

		private RustProto.Avatar MakeReadOnly()
		{
			this.blueprints_.MakeReadOnly();
			this.inventory_.MakeReadOnly();
			this.wearable_.MakeReadOnly();
			this.belt_.MakeReadOnly();
			return this;
		}

		public static RustProto.Avatar ParseDelimitedFrom(Stream input)
		{
			return RustProto.Avatar.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static RustProto.Avatar ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Avatar.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Avatar ParseFrom(ByteString data)
		{
			return RustProto.Avatar.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Avatar ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Avatar.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Avatar ParseFrom(byte[] data)
		{
			return RustProto.Avatar.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Avatar ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Avatar.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Avatar ParseFrom(Stream input)
		{
			return RustProto.Avatar.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Avatar ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Avatar.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Avatar ParseFrom(ICodedInputStream input)
		{
			return RustProto.Avatar.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Avatar ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Avatar.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<RustProto.Avatar, RustProto.Avatar.Builder> Recycler()
		{
			return Recycler<RustProto.Avatar, RustProto.Avatar.Builder>.Manufacture();
		}

		public override RustProto.Avatar.Builder ToBuilder()
		{
			return RustProto.Avatar.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = RustProto.Avatar._avatarFieldNames;
			if (this.hasPos)
			{
				output.WriteMessage(1, strArrays[5], this.Pos);
			}
			if (this.hasAng)
			{
				output.WriteMessage(2, strArrays[0], this.Ang);
			}
			if (this.hasVitals)
			{
				output.WriteMessage(3, strArrays[6], this.Vitals);
			}
			if (this.blueprints_.Count > 0)
			{
				output.WriteMessageArray<RustProto.Blueprint>(4, strArrays[3], this.blueprints_);
			}
			if (this.inventory_.Count > 0)
			{
				output.WriteMessageArray<RustProto.Item>(5, strArrays[4], this.inventory_);
			}
			if (this.wearable_.Count > 0)
			{
				output.WriteMessageArray<RustProto.Item>(6, strArrays[7], this.wearable_);
			}
			if (this.belt_.Count > 0)
			{
				output.WriteMessageArray<RustProto.Item>(7, strArrays[2], this.belt_);
			}
			if (this.hasAwayEvent)
			{
				output.WriteMessage(8, strArrays[1], this.AwayEvent);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<RustProto.Avatar, RustProto.Avatar.Builder>
		{
			private bool resultIsReadOnly;

			private RustProto.Avatar result;

			public Quaternion Ang
			{
				get
				{
					return this.result.Ang;
				}
				set
				{
					this.SetAng(value);
				}
			}

			public RustProto.AwayEvent AwayEvent
			{
				get
				{
					return this.result.AwayEvent;
				}
				set
				{
					this.SetAwayEvent(value);
				}
			}

			public int BeltCount
			{
				get
				{
					return this.result.BeltCount;
				}
			}

			public IPopsicleList<RustProto.Item> BeltList
			{
				get
				{
					return this.PrepareBuilder().belt_;
				}
			}

			public int BlueprintsCount
			{
				get
				{
					return this.result.BlueprintsCount;
				}
			}

			public IPopsicleList<RustProto.Blueprint> BlueprintsList
			{
				get
				{
					return this.PrepareBuilder().blueprints_;
				}
			}

			public override RustProto.Avatar DefaultInstanceForType
			{
				get
				{
					return RustProto.Avatar.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return RustProto.Avatar.Descriptor;
				}
			}

			public bool HasAng
			{
				get
				{
					return this.result.hasAng;
				}
			}

			public bool HasAwayEvent
			{
				get
				{
					return this.result.hasAwayEvent;
				}
			}

			public bool HasPos
			{
				get
				{
					return this.result.hasPos;
				}
			}

			public bool HasVitals
			{
				get
				{
					return this.result.hasVitals;
				}
			}

			public int InventoryCount
			{
				get
				{
					return this.result.InventoryCount;
				}
			}

			public IPopsicleList<RustProto.Item> InventoryList
			{
				get
				{
					return this.PrepareBuilder().inventory_;
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override RustProto.Avatar MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
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

			protected override RustProto.Avatar.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public RustProto.Vitals Vitals
			{
				get
				{
					return this.result.Vitals;
				}
				set
				{
					this.SetVitals(value);
				}
			}

			public int WearableCount
			{
				get
				{
					return this.result.WearableCount;
				}
			}

			public IPopsicleList<RustProto.Item> WearableList
			{
				get
				{
					return this.PrepareBuilder().wearable_;
				}
			}

			public Builder()
			{
				this.result = RustProto.Avatar.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(RustProto.Avatar cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public RustProto.Avatar.Builder AddBelt(RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.belt_.Add(value);
				return this;
			}

			public RustProto.Avatar.Builder AddBelt(RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.belt_.Add(builderForValue.Build());
				return this;
			}

			public RustProto.Avatar.Builder AddBlueprints(RustProto.Blueprint value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.blueprints_.Add(value);
				return this;
			}

			public RustProto.Avatar.Builder AddBlueprints(RustProto.Blueprint.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.blueprints_.Add(builderForValue.Build());
				return this;
			}

			public RustProto.Avatar.Builder AddInventory(RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.inventory_.Add(value);
				return this;
			}

			public RustProto.Avatar.Builder AddInventory(RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.inventory_.Add(builderForValue.Build());
				return this;
			}

			public RustProto.Avatar.Builder AddRangeBelt(IEnumerable<RustProto.Item> values)
			{
				this.PrepareBuilder();
				this.result.belt_.Add(values);
				return this;
			}

			public RustProto.Avatar.Builder AddRangeBlueprints(IEnumerable<RustProto.Blueprint> values)
			{
				this.PrepareBuilder();
				this.result.blueprints_.Add(values);
				return this;
			}

			public RustProto.Avatar.Builder AddRangeInventory(IEnumerable<RustProto.Item> values)
			{
				this.PrepareBuilder();
				this.result.inventory_.Add(values);
				return this;
			}

			public RustProto.Avatar.Builder AddRangeWearable(IEnumerable<RustProto.Item> values)
			{
				this.PrepareBuilder();
				this.result.wearable_.Add(values);
				return this;
			}

			public RustProto.Avatar.Builder AddWearable(RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.wearable_.Add(value);
				return this;
			}

			public RustProto.Avatar.Builder AddWearable(RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.wearable_.Add(builderForValue.Build());
				return this;
			}

			public override RustProto.Avatar BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override RustProto.Avatar.Builder Clear()
			{
				this.result = RustProto.Avatar.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public RustProto.Avatar.Builder ClearAng()
			{
				this.PrepareBuilder();
				this.result.hasAng = false;
				this.result.ang_ = null;
				return this;
			}

			public RustProto.Avatar.Builder ClearAwayEvent()
			{
				this.PrepareBuilder();
				this.result.hasAwayEvent = false;
				this.result.awayEvent_ = null;
				return this;
			}

			public RustProto.Avatar.Builder ClearBelt()
			{
				this.PrepareBuilder();
				this.result.belt_.Clear();
				return this;
			}

			public RustProto.Avatar.Builder ClearBlueprints()
			{
				this.PrepareBuilder();
				this.result.blueprints_.Clear();
				return this;
			}

			public RustProto.Avatar.Builder ClearInventory()
			{
				this.PrepareBuilder();
				this.result.inventory_.Clear();
				return this;
			}

			public RustProto.Avatar.Builder ClearPos()
			{
				this.PrepareBuilder();
				this.result.hasPos = false;
				this.result.pos_ = null;
				return this;
			}

			public RustProto.Avatar.Builder ClearVitals()
			{
				this.PrepareBuilder();
				this.result.hasVitals = false;
				this.result.vitals_ = null;
				return this;
			}

			public RustProto.Avatar.Builder ClearWearable()
			{
				this.PrepareBuilder();
				this.result.wearable_.Clear();
				return this;
			}

			public override RustProto.Avatar.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new RustProto.Avatar.Builder(this.result);
				}
				return (new RustProto.Avatar.Builder()).MergeFrom(this.result);
			}

			public RustProto.Item GetBelt(int index)
			{
				return this.result.GetBelt(index);
			}

			public RustProto.Blueprint GetBlueprints(int index)
			{
				return this.result.GetBlueprints(index);
			}

			public RustProto.Item GetInventory(int index)
			{
				return this.result.GetInventory(index);
			}

			public RustProto.Item GetWearable(int index)
			{
				return this.result.GetWearable(index);
			}

			public RustProto.Avatar.Builder MergeAng(Quaternion value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasAng || this.result.ang_ == Quaternion.DefaultInstance)
				{
					this.result.ang_ = value;
				}
				else
				{
					this.result.ang_ = Quaternion.CreateBuilder(this.result.ang_).MergeFrom(value).BuildPartial();
				}
				this.result.hasAng = true;
				return this;
			}

			public RustProto.Avatar.Builder MergeAwayEvent(RustProto.AwayEvent value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasAwayEvent || this.result.awayEvent_ == RustProto.AwayEvent.DefaultInstance)
				{
					this.result.awayEvent_ = value;
				}
				else
				{
					this.result.awayEvent_ = RustProto.AwayEvent.CreateBuilder(this.result.awayEvent_).MergeFrom(value).BuildPartial();
				}
				this.result.hasAwayEvent = true;
				return this;
			}

			public override RustProto.Avatar.Builder MergeFrom(IMessage other)
			{
				if (other is RustProto.Avatar)
				{
					return this.MergeFrom((RustProto.Avatar)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override RustProto.Avatar.Builder MergeFrom(RustProto.Avatar other)
			{
				if (other == RustProto.Avatar.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasPos)
				{
					this.MergePos(other.Pos);
				}
				if (other.HasAng)
				{
					this.MergeAng(other.Ang);
				}
				if (other.HasVitals)
				{
					this.MergeVitals(other.Vitals);
				}
				if (other.blueprints_.Count != 0)
				{
					this.result.blueprints_.Add(other.blueprints_);
				}
				if (other.inventory_.Count != 0)
				{
					this.result.inventory_.Add(other.inventory_);
				}
				if (other.wearable_.Count != 0)
				{
					this.result.wearable_.Add(other.wearable_);
				}
				if (other.belt_.Count != 0)
				{
					this.result.belt_.Add(other.belt_);
				}
				if (other.HasAwayEvent)
				{
					this.MergeAwayEvent(other.AwayEvent);
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override RustProto.Avatar.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override RustProto.Avatar.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(RustProto.Avatar._avatarFieldNames, str, StringComparer.Ordinal);
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
							num = RustProto.Avatar._avatarFieldTags[num1];
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
						Quaternion.Builder builder2 = Quaternion.CreateBuilder();
						if (this.result.hasAng)
						{
							builder2.MergeFrom(this.Ang);
						}
						input.ReadMessage(builder2, extensionRegistry);
						this.Ang = builder2.BuildPartial();
					}
					else if (num2 == 26)
					{
						RustProto.Vitals.Builder builder3 = RustProto.Vitals.CreateBuilder();
						if (this.result.hasVitals)
						{
							builder3.MergeFrom(this.Vitals);
						}
						input.ReadMessage(builder3, extensionRegistry);
						this.Vitals = builder3.BuildPartial();
					}
					else if (num2 == 34)
					{
						input.ReadMessageArray<RustProto.Blueprint>(num, str, this.result.blueprints_, RustProto.Blueprint.DefaultInstance, extensionRegistry);
					}
					else if (num2 == 42)
					{
						input.ReadMessageArray<RustProto.Item>(num, str, this.result.inventory_, RustProto.Item.DefaultInstance, extensionRegistry);
					}
					else if (num2 == 50)
					{
						input.ReadMessageArray<RustProto.Item>(num, str, this.result.wearable_, RustProto.Item.DefaultInstance, extensionRegistry);
					}
					else if (num2 == 58)
					{
						input.ReadMessageArray<RustProto.Item>(num, str, this.result.belt_, RustProto.Item.DefaultInstance, extensionRegistry);
					}
					else if (num2 == 66)
					{
						RustProto.AwayEvent.Builder builder4 = RustProto.AwayEvent.CreateBuilder();
						if (this.result.hasAwayEvent)
						{
							builder4.MergeFrom(this.AwayEvent);
						}
						input.ReadMessage(builder4, extensionRegistry);
						this.AwayEvent = builder4.BuildPartial();
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

			public RustProto.Avatar.Builder MergePos(Vector value)
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

			public RustProto.Avatar.Builder MergeVitals(RustProto.Vitals value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasVitals || this.result.vitals_ == RustProto.Vitals.DefaultInstance)
				{
					this.result.vitals_ = value;
				}
				else
				{
					this.result.vitals_ = RustProto.Vitals.CreateBuilder(this.result.vitals_).MergeFrom(value).BuildPartial();
				}
				this.result.hasVitals = true;
				return this;
			}

			private RustProto.Avatar PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					RustProto.Avatar avatar = this.result;
					this.result = new RustProto.Avatar();
					this.resultIsReadOnly = false;
					this.MergeFrom(avatar);
				}
				return this.result;
			}

			public RustProto.Avatar.Builder SetAng(Quaternion value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasAng = true;
				this.result.ang_ = value;
				return this;
			}

			public RustProto.Avatar.Builder SetAng(Quaternion.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasAng = true;
				this.result.ang_ = builderForValue.Build();
				return this;
			}

			public RustProto.Avatar.Builder SetAwayEvent(RustProto.AwayEvent value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasAwayEvent = true;
				this.result.awayEvent_ = value;
				return this;
			}

			public RustProto.Avatar.Builder SetAwayEvent(RustProto.AwayEvent.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasAwayEvent = true;
				this.result.awayEvent_ = builderForValue.Build();
				return this;
			}

			public RustProto.Avatar.Builder SetBelt(int index, RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.belt_[index] = value;
				return this;
			}

			public RustProto.Avatar.Builder SetBelt(int index, RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.belt_[index] = builderForValue.Build();
				return this;
			}

			public RustProto.Avatar.Builder SetBlueprints(int index, RustProto.Blueprint value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.blueprints_[index] = value;
				return this;
			}

			public RustProto.Avatar.Builder SetBlueprints(int index, RustProto.Blueprint.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.blueprints_[index] = builderForValue.Build();
				return this;
			}

			public RustProto.Avatar.Builder SetInventory(int index, RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.inventory_[index] = value;
				return this;
			}

			public RustProto.Avatar.Builder SetInventory(int index, RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.inventory_[index] = builderForValue.Build();
				return this;
			}

			public RustProto.Avatar.Builder SetPos(Vector value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasPos = true;
				this.result.pos_ = value;
				return this;
			}

			public RustProto.Avatar.Builder SetPos(Vector.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasPos = true;
				this.result.pos_ = builderForValue.Build();
				return this;
			}

			public RustProto.Avatar.Builder SetVitals(RustProto.Vitals value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasVitals = true;
				this.result.vitals_ = value;
				return this;
			}

			public RustProto.Avatar.Builder SetVitals(RustProto.Vitals.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasVitals = true;
				this.result.vitals_ = builderForValue.Build();
				return this;
			}

			public RustProto.Avatar.Builder SetWearable(int index, RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.wearable_[index] = value;
				return this;
			}

			public RustProto.Avatar.Builder SetWearable(int index, RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.wearable_[index] = builderForValue.Build();
				return this;
			}
		}
	}
}