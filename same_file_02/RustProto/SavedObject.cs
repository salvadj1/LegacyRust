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
	public sealed class SavedObject : GeneratedMessage<SavedObject, SavedObject.Builder>
	{
		public const int IdFieldNumber = 1;

		public const int DoorFieldNumber = 2;

		public const int InventoryFieldNumber = 3;

		public const int DeployableFieldNumber = 4;

		public const int StructMasterFieldNumber = 5;

		public const int StructComponentFieldNumber = 6;

		public const int FireBarrelFieldNumber = 7;

		public const int NetInstanceFieldNumber = 8;

		public const int CoordsFieldNumber = 9;

		public const int NgcInstanceFieldNumber = 10;

		public const int CarriableTransFieldNumber = 11;

		public const int TakeDamageFieldNumber = 12;

		public const int SortOrderFieldNumber = 13;

		public const int SleepingAvatarFieldNumber = 14;

		public const int LockableFieldNumber = 15;

		private readonly static SavedObject defaultInstance;

		private readonly static string[] _savedObjectFieldNames;

		private readonly static uint[] _savedObjectFieldTags;

		private bool hasId;

		private int id_;

		private bool hasDoor;

		private objectDoor door_;

		private PopsicleList<RustProto.Item> inventory_ = new PopsicleList<RustProto.Item>();

		private bool hasDeployable;

		private objectDeployable deployable_;

		private bool hasStructMaster;

		private objectStructMaster structMaster_;

		private bool hasStructComponent;

		private objectStructComponent structComponent_;

		private bool hasFireBarrel;

		private objectFireBarrel fireBarrel_;

		private bool hasNetInstance;

		private objectNetInstance netInstance_;

		private bool hasCoords;

		private objectCoords coords_;

		private bool hasNgcInstance;

		private objectNGCInstance ngcInstance_;

		private bool hasCarriableTrans;

		private objectICarriableTrans carriableTrans_;

		private bool hasTakeDamage;

		private objectTakeDamage takeDamage_;

		private bool hasSortOrder;

		private int sortOrder_;

		private bool hasSleepingAvatar;

		private objectSleepingAvatar sleepingAvatar_;

		private bool hasLockable;

		private objectLockable lockable_;

		private int memoizedSerializedSize = -1;

		public objectICarriableTrans CarriableTrans
		{
			get
			{
				return this.carriableTrans_ ?? objectICarriableTrans.DefaultInstance;
			}
		}

		public objectCoords Coords
		{
			get
			{
				return this.coords_ ?? objectCoords.DefaultInstance;
			}
		}

		public static SavedObject DefaultInstance
		{
			get
			{
				return SavedObject.defaultInstance;
			}
		}

		public override SavedObject DefaultInstanceForType
		{
			get
			{
				return SavedObject.DefaultInstance;
			}
		}

		public objectDeployable Deployable
		{
			get
			{
				return this.deployable_ ?? objectDeployable.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return Worldsave.internal__static_RustProto_SavedObject__Descriptor;
			}
		}

		public objectDoor Door
		{
			get
			{
				return this.door_ ?? objectDoor.DefaultInstance;
			}
		}

		public objectFireBarrel FireBarrel
		{
			get
			{
				return this.fireBarrel_ ?? objectFireBarrel.DefaultInstance;
			}
		}

		public bool HasCarriableTrans
		{
			get
			{
				return this.hasCarriableTrans;
			}
		}

		public bool HasCoords
		{
			get
			{
				return this.hasCoords;
			}
		}

		public bool HasDeployable
		{
			get
			{
				return this.hasDeployable;
			}
		}

		public bool HasDoor
		{
			get
			{
				return this.hasDoor;
			}
		}

		public bool HasFireBarrel
		{
			get
			{
				return this.hasFireBarrel;
			}
		}

		public bool HasId
		{
			get
			{
				return this.hasId;
			}
		}

		public bool HasLockable
		{
			get
			{
				return this.hasLockable;
			}
		}

		public bool HasNetInstance
		{
			get
			{
				return this.hasNetInstance;
			}
		}

		public bool HasNgcInstance
		{
			get
			{
				return this.hasNgcInstance;
			}
		}

		public bool HasSleepingAvatar
		{
			get
			{
				return this.hasSleepingAvatar;
			}
		}

		public bool HasSortOrder
		{
			get
			{
				return this.hasSortOrder;
			}
		}

		public bool HasStructComponent
		{
			get
			{
				return this.hasStructComponent;
			}
		}

		public bool HasStructMaster
		{
			get
			{
				return this.hasStructMaster;
			}
		}

		public bool HasTakeDamage
		{
			get
			{
				return this.hasTakeDamage;
			}
		}

		public int Id
		{
			get
			{
				return this.id_;
			}
		}

		protected override FieldAccessorTable<SavedObject, SavedObject.Builder> InternalFieldAccessors
		{
			get
			{
				return Worldsave.internal__static_RustProto_SavedObject__FieldAccessorTable;
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
				IEnumerator<RustProto.Item> enumerator = this.InventoryList.GetEnumerator();
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

		public objectLockable Lockable
		{
			get
			{
				return this.lockable_ ?? objectLockable.DefaultInstance;
			}
		}

		public objectNetInstance NetInstance
		{
			get
			{
				return this.netInstance_ ?? objectNetInstance.DefaultInstance;
			}
		}

		public objectNGCInstance NgcInstance
		{
			get
			{
				return this.ngcInstance_ ?? objectNGCInstance.DefaultInstance;
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
				if (this.hasDoor)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(2, this.Door);
				}
				IEnumerator<RustProto.Item> enumerator = this.InventoryList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						RustProto.Item current = enumerator.Current;
						serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(3, current);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				if (this.hasDeployable)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(4, this.Deployable);
				}
				if (this.hasStructMaster)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(5, this.StructMaster);
				}
				if (this.hasStructComponent)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(6, this.StructComponent);
				}
				if (this.hasFireBarrel)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(7, this.FireBarrel);
				}
				if (this.hasNetInstance)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(8, this.NetInstance);
				}
				if (this.hasCoords)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(9, this.Coords);
				}
				if (this.hasNgcInstance)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(10, this.NgcInstance);
				}
				if (this.hasCarriableTrans)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(11, this.CarriableTrans);
				}
				if (this.hasTakeDamage)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(12, this.TakeDamage);
				}
				if (this.hasSortOrder)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeInt32Size(13, this.SortOrder);
				}
				if (this.hasSleepingAvatar)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(14, this.SleepingAvatar);
				}
				if (this.hasLockable)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeMessageSize(15, this.Lockable);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		public objectSleepingAvatar SleepingAvatar
		{
			get
			{
				return this.sleepingAvatar_ ?? objectSleepingAvatar.DefaultInstance;
			}
		}

		public int SortOrder
		{
			get
			{
				return this.sortOrder_;
			}
		}

		public objectStructComponent StructComponent
		{
			get
			{
				return this.structComponent_ ?? objectStructComponent.DefaultInstance;
			}
		}

		public objectStructMaster StructMaster
		{
			get
			{
				return this.structMaster_ ?? objectStructMaster.DefaultInstance;
			}
		}

		public objectTakeDamage TakeDamage
		{
			get
			{
				return this.takeDamage_ ?? objectTakeDamage.DefaultInstance;
			}
		}

		protected override SavedObject ThisMessage
		{
			get
			{
				return this;
			}
		}

		static SavedObject()
		{
			SavedObject.defaultInstance = (new SavedObject()).MakeReadOnly();
			SavedObject._savedObjectFieldNames = new string[] { "carriableTrans", "coords", "deployable", "door", "fireBarrel", "id", "inventory", "lockable", "netInstance", "ngcInstance", "sleepingAvatar", "sortOrder", "structComponent", "structMaster", "takeDamage" };
			SavedObject._savedObjectFieldTags = new uint[] { 90, 74, 34, 18, 58, 8, 26, 122, 66, 82, 114, 104, 50, 42, 98 };
			object.ReferenceEquals(Worldsave.Descriptor, null);
		}

		private SavedObject()
		{
		}

		public static SavedObject.Builder CreateBuilder()
		{
			return new SavedObject.Builder();
		}

		public static SavedObject.Builder CreateBuilder(SavedObject prototype)
		{
			return new SavedObject.Builder(prototype);
		}

		public override SavedObject.Builder CreateBuilderForType()
		{
			return new SavedObject.Builder();
		}

		public RustProto.Item GetInventory(int index)
		{
			return this.inventory_[index];
		}

		private SavedObject MakeReadOnly()
		{
			this.inventory_.MakeReadOnly();
			return this;
		}

		public static SavedObject ParseDelimitedFrom(Stream input)
		{
			return SavedObject.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static SavedObject ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return SavedObject.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static SavedObject ParseFrom(ByteString data)
		{
			return SavedObject.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static SavedObject ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return SavedObject.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static SavedObject ParseFrom(byte[] data)
		{
			return SavedObject.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static SavedObject ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return SavedObject.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static SavedObject ParseFrom(Stream input)
		{
			return SavedObject.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static SavedObject ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return SavedObject.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static SavedObject ParseFrom(ICodedInputStream input)
		{
			return SavedObject.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static SavedObject ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return SavedObject.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<SavedObject, SavedObject.Builder> Recycler()
		{
			return Recycler<SavedObject, SavedObject.Builder>.Manufacture();
		}

		public override SavedObject.Builder ToBuilder()
		{
			return SavedObject.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = SavedObject._savedObjectFieldNames;
			if (this.hasId)
			{
				output.WriteInt32(1, strArrays[5], this.Id);
			}
			if (this.hasDoor)
			{
				output.WriteMessage(2, strArrays[3], this.Door);
			}
			if (this.inventory_.Count > 0)
			{
				output.WriteMessageArray<RustProto.Item>(3, strArrays[6], this.inventory_);
			}
			if (this.hasDeployable)
			{
				output.WriteMessage(4, strArrays[2], this.Deployable);
			}
			if (this.hasStructMaster)
			{
				output.WriteMessage(5, strArrays[13], this.StructMaster);
			}
			if (this.hasStructComponent)
			{
				output.WriteMessage(6, strArrays[12], this.StructComponent);
			}
			if (this.hasFireBarrel)
			{
				output.WriteMessage(7, strArrays[4], this.FireBarrel);
			}
			if (this.hasNetInstance)
			{
				output.WriteMessage(8, strArrays[8], this.NetInstance);
			}
			if (this.hasCoords)
			{
				output.WriteMessage(9, strArrays[1], this.Coords);
			}
			if (this.hasNgcInstance)
			{
				output.WriteMessage(10, strArrays[9], this.NgcInstance);
			}
			if (this.hasCarriableTrans)
			{
				output.WriteMessage(11, strArrays[0], this.CarriableTrans);
			}
			if (this.hasTakeDamage)
			{
				output.WriteMessage(12, strArrays[14], this.TakeDamage);
			}
			if (this.hasSortOrder)
			{
				output.WriteInt32(13, strArrays[11], this.SortOrder);
			}
			if (this.hasSleepingAvatar)
			{
				output.WriteMessage(14, strArrays[10], this.SleepingAvatar);
			}
			if (this.hasLockable)
			{
				output.WriteMessage(15, strArrays[7], this.Lockable);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<SavedObject, SavedObject.Builder>
		{
			private bool resultIsReadOnly;

			private SavedObject result;

			public objectICarriableTrans CarriableTrans
			{
				get
				{
					return this.result.CarriableTrans;
				}
				set
				{
					this.SetCarriableTrans(value);
				}
			}

			public objectCoords Coords
			{
				get
				{
					return this.result.Coords;
				}
				set
				{
					this.SetCoords(value);
				}
			}

			public override SavedObject DefaultInstanceForType
			{
				get
				{
					return SavedObject.DefaultInstance;
				}
			}

			public objectDeployable Deployable
			{
				get
				{
					return this.result.Deployable;
				}
				set
				{
					this.SetDeployable(value);
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return SavedObject.Descriptor;
				}
			}

			public objectDoor Door
			{
				get
				{
					return this.result.Door;
				}
				set
				{
					this.SetDoor(value);
				}
			}

			public objectFireBarrel FireBarrel
			{
				get
				{
					return this.result.FireBarrel;
				}
				set
				{
					this.SetFireBarrel(value);
				}
			}

			public bool HasCarriableTrans
			{
				get
				{
					return this.result.hasCarriableTrans;
				}
			}

			public bool HasCoords
			{
				get
				{
					return this.result.hasCoords;
				}
			}

			public bool HasDeployable
			{
				get
				{
					return this.result.hasDeployable;
				}
			}

			public bool HasDoor
			{
				get
				{
					return this.result.hasDoor;
				}
			}

			public bool HasFireBarrel
			{
				get
				{
					return this.result.hasFireBarrel;
				}
			}

			public bool HasId
			{
				get
				{
					return this.result.hasId;
				}
			}

			public bool HasLockable
			{
				get
				{
					return this.result.hasLockable;
				}
			}

			public bool HasNetInstance
			{
				get
				{
					return this.result.hasNetInstance;
				}
			}

			public bool HasNgcInstance
			{
				get
				{
					return this.result.hasNgcInstance;
				}
			}

			public bool HasSleepingAvatar
			{
				get
				{
					return this.result.hasSleepingAvatar;
				}
			}

			public bool HasSortOrder
			{
				get
				{
					return this.result.hasSortOrder;
				}
			}

			public bool HasStructComponent
			{
				get
				{
					return this.result.hasStructComponent;
				}
			}

			public bool HasStructMaster
			{
				get
				{
					return this.result.hasStructMaster;
				}
			}

			public bool HasTakeDamage
			{
				get
				{
					return this.result.hasTakeDamage;
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

			public objectLockable Lockable
			{
				get
				{
					return this.result.Lockable;
				}
				set
				{
					this.SetLockable(value);
				}
			}

			protected override SavedObject MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public objectNetInstance NetInstance
			{
				get
				{
					return this.result.NetInstance;
				}
				set
				{
					this.SetNetInstance(value);
				}
			}

			public objectNGCInstance NgcInstance
			{
				get
				{
					return this.result.NgcInstance;
				}
				set
				{
					this.SetNgcInstance(value);
				}
			}

			public objectSleepingAvatar SleepingAvatar
			{
				get
				{
					return this.result.SleepingAvatar;
				}
				set
				{
					this.SetSleepingAvatar(value);
				}
			}

			public int SortOrder
			{
				get
				{
					return this.result.SortOrder;
				}
				set
				{
					this.SetSortOrder(value);
				}
			}

			public objectStructComponent StructComponent
			{
				get
				{
					return this.result.StructComponent;
				}
				set
				{
					this.SetStructComponent(value);
				}
			}

			public objectStructMaster StructMaster
			{
				get
				{
					return this.result.StructMaster;
				}
				set
				{
					this.SetStructMaster(value);
				}
			}

			public objectTakeDamage TakeDamage
			{
				get
				{
					return this.result.TakeDamage;
				}
				set
				{
					this.SetTakeDamage(value);
				}
			}

			protected override SavedObject.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = SavedObject.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(SavedObject cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public SavedObject.Builder AddInventory(RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.inventory_.Add(value);
				return this;
			}

			public SavedObject.Builder AddInventory(RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.inventory_.Add(builderForValue.Build());
				return this;
			}

			public SavedObject.Builder AddRangeInventory(IEnumerable<RustProto.Item> values)
			{
				this.PrepareBuilder();
				this.result.inventory_.Add(values);
				return this;
			}

			public override SavedObject BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override SavedObject.Builder Clear()
			{
				this.result = SavedObject.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public SavedObject.Builder ClearCarriableTrans()
			{
				this.PrepareBuilder();
				this.result.hasCarriableTrans = false;
				this.result.carriableTrans_ = null;
				return this;
			}

			public SavedObject.Builder ClearCoords()
			{
				this.PrepareBuilder();
				this.result.hasCoords = false;
				this.result.coords_ = null;
				return this;
			}

			public SavedObject.Builder ClearDeployable()
			{
				this.PrepareBuilder();
				this.result.hasDeployable = false;
				this.result.deployable_ = null;
				return this;
			}

			public SavedObject.Builder ClearDoor()
			{
				this.PrepareBuilder();
				this.result.hasDoor = false;
				this.result.door_ = null;
				return this;
			}

			public SavedObject.Builder ClearFireBarrel()
			{
				this.PrepareBuilder();
				this.result.hasFireBarrel = false;
				this.result.fireBarrel_ = null;
				return this;
			}

			public SavedObject.Builder ClearId()
			{
				this.PrepareBuilder();
				this.result.hasId = false;
				this.result.id_ = 0;
				return this;
			}

			public SavedObject.Builder ClearInventory()
			{
				this.PrepareBuilder();
				this.result.inventory_.Clear();
				return this;
			}

			public SavedObject.Builder ClearLockable()
			{
				this.PrepareBuilder();
				this.result.hasLockable = false;
				this.result.lockable_ = null;
				return this;
			}

			public SavedObject.Builder ClearNetInstance()
			{
				this.PrepareBuilder();
				this.result.hasNetInstance = false;
				this.result.netInstance_ = null;
				return this;
			}

			public SavedObject.Builder ClearNgcInstance()
			{
				this.PrepareBuilder();
				this.result.hasNgcInstance = false;
				this.result.ngcInstance_ = null;
				return this;
			}

			public SavedObject.Builder ClearSleepingAvatar()
			{
				this.PrepareBuilder();
				this.result.hasSleepingAvatar = false;
				this.result.sleepingAvatar_ = null;
				return this;
			}

			public SavedObject.Builder ClearSortOrder()
			{
				this.PrepareBuilder();
				this.result.hasSortOrder = false;
				this.result.sortOrder_ = 0;
				return this;
			}

			public SavedObject.Builder ClearStructComponent()
			{
				this.PrepareBuilder();
				this.result.hasStructComponent = false;
				this.result.structComponent_ = null;
				return this;
			}

			public SavedObject.Builder ClearStructMaster()
			{
				this.PrepareBuilder();
				this.result.hasStructMaster = false;
				this.result.structMaster_ = null;
				return this;
			}

			public SavedObject.Builder ClearTakeDamage()
			{
				this.PrepareBuilder();
				this.result.hasTakeDamage = false;
				this.result.takeDamage_ = null;
				return this;
			}

			public override SavedObject.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new SavedObject.Builder(this.result);
				}
				return (new SavedObject.Builder()).MergeFrom(this.result);
			}

			public RustProto.Item GetInventory(int index)
			{
				return this.result.GetInventory(index);
			}

			public SavedObject.Builder MergeCarriableTrans(objectICarriableTrans value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasCarriableTrans || this.result.carriableTrans_ == objectICarriableTrans.DefaultInstance)
				{
					this.result.carriableTrans_ = value;
				}
				else
				{
					this.result.carriableTrans_ = objectICarriableTrans.CreateBuilder(this.result.carriableTrans_).MergeFrom(value).BuildPartial();
				}
				this.result.hasCarriableTrans = true;
				return this;
			}

			public SavedObject.Builder MergeCoords(objectCoords value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasCoords || this.result.coords_ == objectCoords.DefaultInstance)
				{
					this.result.coords_ = value;
				}
				else
				{
					this.result.coords_ = objectCoords.CreateBuilder(this.result.coords_).MergeFrom(value).BuildPartial();
				}
				this.result.hasCoords = true;
				return this;
			}

			public SavedObject.Builder MergeDeployable(objectDeployable value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasDeployable || this.result.deployable_ == objectDeployable.DefaultInstance)
				{
					this.result.deployable_ = value;
				}
				else
				{
					this.result.deployable_ = objectDeployable.CreateBuilder(this.result.deployable_).MergeFrom(value).BuildPartial();
				}
				this.result.hasDeployable = true;
				return this;
			}

			public SavedObject.Builder MergeDoor(objectDoor value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasDoor || this.result.door_ == objectDoor.DefaultInstance)
				{
					this.result.door_ = value;
				}
				else
				{
					this.result.door_ = objectDoor.CreateBuilder(this.result.door_).MergeFrom(value).BuildPartial();
				}
				this.result.hasDoor = true;
				return this;
			}

			public SavedObject.Builder MergeFireBarrel(objectFireBarrel value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasFireBarrel || this.result.fireBarrel_ == objectFireBarrel.DefaultInstance)
				{
					this.result.fireBarrel_ = value;
				}
				else
				{
					this.result.fireBarrel_ = objectFireBarrel.CreateBuilder(this.result.fireBarrel_).MergeFrom(value).BuildPartial();
				}
				this.result.hasFireBarrel = true;
				return this;
			}

			public override SavedObject.Builder MergeFrom(IMessage other)
			{
				if (other is SavedObject)
				{
					return this.MergeFrom((SavedObject)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override SavedObject.Builder MergeFrom(SavedObject other)
			{
				if (other == SavedObject.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasId)
				{
					this.Id = other.Id;
				}
				if (other.HasDoor)
				{
					this.MergeDoor(other.Door);
				}
				if (other.inventory_.Count != 0)
				{
					this.result.inventory_.Add(other.inventory_);
				}
				if (other.HasDeployable)
				{
					this.MergeDeployable(other.Deployable);
				}
				if (other.HasStructMaster)
				{
					this.MergeStructMaster(other.StructMaster);
				}
				if (other.HasStructComponent)
				{
					this.MergeStructComponent(other.StructComponent);
				}
				if (other.HasFireBarrel)
				{
					this.MergeFireBarrel(other.FireBarrel);
				}
				if (other.HasNetInstance)
				{
					this.MergeNetInstance(other.NetInstance);
				}
				if (other.HasCoords)
				{
					this.MergeCoords(other.Coords);
				}
				if (other.HasNgcInstance)
				{
					this.MergeNgcInstance(other.NgcInstance);
				}
				if (other.HasCarriableTrans)
				{
					this.MergeCarriableTrans(other.CarriableTrans);
				}
				if (other.HasTakeDamage)
				{
					this.MergeTakeDamage(other.TakeDamage);
				}
				if (other.HasSortOrder)
				{
					this.SortOrder = other.SortOrder;
				}
				if (other.HasSleepingAvatar)
				{
					this.MergeSleepingAvatar(other.SleepingAvatar);
				}
				if (other.HasLockable)
				{
					this.MergeLockable(other.Lockable);
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override SavedObject.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override SavedObject.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(SavedObject._savedObjectFieldNames, str, StringComparer.Ordinal);
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
							num = SavedObject._savedObjectFieldTags[num1];
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
						objectDoor.Builder builder1 = objectDoor.CreateBuilder();
						if (this.result.hasDoor)
						{
							builder1.MergeFrom(this.Door);
						}
						input.ReadMessage(builder1, extensionRegistry);
						this.Door = builder1.BuildPartial();
					}
					else if (num2 == 26)
					{
						input.ReadMessageArray<RustProto.Item>(num, str, this.result.inventory_, RustProto.Item.DefaultInstance, extensionRegistry);
					}
					else if (num2 == 34)
					{
						objectDeployable.Builder builder2 = objectDeployable.CreateBuilder();
						if (this.result.hasDeployable)
						{
							builder2.MergeFrom(this.Deployable);
						}
						input.ReadMessage(builder2, extensionRegistry);
						this.Deployable = builder2.BuildPartial();
					}
					else if (num2 == 42)
					{
						objectStructMaster.Builder builder3 = objectStructMaster.CreateBuilder();
						if (this.result.hasStructMaster)
						{
							builder3.MergeFrom(this.StructMaster);
						}
						input.ReadMessage(builder3, extensionRegistry);
						this.StructMaster = builder3.BuildPartial();
					}
					else if (num2 == 50)
					{
						objectStructComponent.Builder builder4 = objectStructComponent.CreateBuilder();
						if (this.result.hasStructComponent)
						{
							builder4.MergeFrom(this.StructComponent);
						}
						input.ReadMessage(builder4, extensionRegistry);
						this.StructComponent = builder4.BuildPartial();
					}
					else if (num2 == 58)
					{
						objectFireBarrel.Builder builder5 = objectFireBarrel.CreateBuilder();
						if (this.result.hasFireBarrel)
						{
							builder5.MergeFrom(this.FireBarrel);
						}
						input.ReadMessage(builder5, extensionRegistry);
						this.FireBarrel = builder5.BuildPartial();
					}
					else if (num2 == 66)
					{
						objectNetInstance.Builder builder6 = objectNetInstance.CreateBuilder();
						if (this.result.hasNetInstance)
						{
							builder6.MergeFrom(this.NetInstance);
						}
						input.ReadMessage(builder6, extensionRegistry);
						this.NetInstance = builder6.BuildPartial();
					}
					else if (num2 == 74)
					{
						objectCoords.Builder builder7 = objectCoords.CreateBuilder();
						if (this.result.hasCoords)
						{
							builder7.MergeFrom(this.Coords);
						}
						input.ReadMessage(builder7, extensionRegistry);
						this.Coords = builder7.BuildPartial();
					}
					else if (num2 == 82)
					{
						objectNGCInstance.Builder builder8 = objectNGCInstance.CreateBuilder();
						if (this.result.hasNgcInstance)
						{
							builder8.MergeFrom(this.NgcInstance);
						}
						input.ReadMessage(builder8, extensionRegistry);
						this.NgcInstance = builder8.BuildPartial();
					}
					else if (num2 == 90)
					{
						objectICarriableTrans.Builder builder9 = objectICarriableTrans.CreateBuilder();
						if (this.result.hasCarriableTrans)
						{
							builder9.MergeFrom(this.CarriableTrans);
						}
						input.ReadMessage(builder9, extensionRegistry);
						this.CarriableTrans = builder9.BuildPartial();
					}
					else if (num2 == 98)
					{
						objectTakeDamage.Builder builder10 = objectTakeDamage.CreateBuilder();
						if (this.result.hasTakeDamage)
						{
							builder10.MergeFrom(this.TakeDamage);
						}
						input.ReadMessage(builder10, extensionRegistry);
						this.TakeDamage = builder10.BuildPartial();
					}
					else if (num2 == 104)
					{
						this.result.hasSortOrder = input.ReadInt32(ref this.result.sortOrder_);
					}
					else if (num2 == 114)
					{
						objectSleepingAvatar.Builder builder11 = objectSleepingAvatar.CreateBuilder();
						if (this.result.hasSleepingAvatar)
						{
							builder11.MergeFrom(this.SleepingAvatar);
						}
						input.ReadMessage(builder11, extensionRegistry);
						this.SleepingAvatar = builder11.BuildPartial();
					}
					else if (num2 == 122)
					{
						objectLockable.Builder builder12 = objectLockable.CreateBuilder();
						if (this.result.hasLockable)
						{
							builder12.MergeFrom(this.Lockable);
						}
						input.ReadMessage(builder12, extensionRegistry);
						this.Lockable = builder12.BuildPartial();
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

			public SavedObject.Builder MergeLockable(objectLockable value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasLockable || this.result.lockable_ == objectLockable.DefaultInstance)
				{
					this.result.lockable_ = value;
				}
				else
				{
					this.result.lockable_ = objectLockable.CreateBuilder(this.result.lockable_).MergeFrom(value).BuildPartial();
				}
				this.result.hasLockable = true;
				return this;
			}

			public SavedObject.Builder MergeNetInstance(objectNetInstance value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasNetInstance || this.result.netInstance_ == objectNetInstance.DefaultInstance)
				{
					this.result.netInstance_ = value;
				}
				else
				{
					this.result.netInstance_ = objectNetInstance.CreateBuilder(this.result.netInstance_).MergeFrom(value).BuildPartial();
				}
				this.result.hasNetInstance = true;
				return this;
			}

			public SavedObject.Builder MergeNgcInstance(objectNGCInstance value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasNgcInstance || this.result.ngcInstance_ == objectNGCInstance.DefaultInstance)
				{
					this.result.ngcInstance_ = value;
				}
				else
				{
					this.result.ngcInstance_ = objectNGCInstance.CreateBuilder(this.result.ngcInstance_).MergeFrom(value).BuildPartial();
				}
				this.result.hasNgcInstance = true;
				return this;
			}

			public SavedObject.Builder MergeSleepingAvatar(objectSleepingAvatar value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasSleepingAvatar || this.result.sleepingAvatar_ == objectSleepingAvatar.DefaultInstance)
				{
					this.result.sleepingAvatar_ = value;
				}
				else
				{
					this.result.sleepingAvatar_ = objectSleepingAvatar.CreateBuilder(this.result.sleepingAvatar_).MergeFrom(value).BuildPartial();
				}
				this.result.hasSleepingAvatar = true;
				return this;
			}

			public SavedObject.Builder MergeStructComponent(objectStructComponent value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasStructComponent || this.result.structComponent_ == objectStructComponent.DefaultInstance)
				{
					this.result.structComponent_ = value;
				}
				else
				{
					this.result.structComponent_ = objectStructComponent.CreateBuilder(this.result.structComponent_).MergeFrom(value).BuildPartial();
				}
				this.result.hasStructComponent = true;
				return this;
			}

			public SavedObject.Builder MergeStructMaster(objectStructMaster value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasStructMaster || this.result.structMaster_ == objectStructMaster.DefaultInstance)
				{
					this.result.structMaster_ = value;
				}
				else
				{
					this.result.structMaster_ = objectStructMaster.CreateBuilder(this.result.structMaster_).MergeFrom(value).BuildPartial();
				}
				this.result.hasStructMaster = true;
				return this;
			}

			public SavedObject.Builder MergeTakeDamage(objectTakeDamage value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				if (!this.result.hasTakeDamage || this.result.takeDamage_ == objectTakeDamage.DefaultInstance)
				{
					this.result.takeDamage_ = value;
				}
				else
				{
					this.result.takeDamage_ = objectTakeDamage.CreateBuilder(this.result.takeDamage_).MergeFrom(value).BuildPartial();
				}
				this.result.hasTakeDamage = true;
				return this;
			}

			private SavedObject PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					SavedObject savedObject = this.result;
					this.result = new SavedObject();
					this.resultIsReadOnly = false;
					this.MergeFrom(savedObject);
				}
				return this.result;
			}

			public SavedObject.Builder SetCarriableTrans(objectICarriableTrans value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasCarriableTrans = true;
				this.result.carriableTrans_ = value;
				return this;
			}

			public SavedObject.Builder SetCarriableTrans(objectICarriableTrans.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasCarriableTrans = true;
				this.result.carriableTrans_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetCoords(objectCoords value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasCoords = true;
				this.result.coords_ = value;
				return this;
			}

			public SavedObject.Builder SetCoords(objectCoords.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasCoords = true;
				this.result.coords_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetDeployable(objectDeployable value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasDeployable = true;
				this.result.deployable_ = value;
				return this;
			}

			public SavedObject.Builder SetDeployable(objectDeployable.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasDeployable = true;
				this.result.deployable_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetDoor(objectDoor value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasDoor = true;
				this.result.door_ = value;
				return this;
			}

			public SavedObject.Builder SetDoor(objectDoor.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasDoor = true;
				this.result.door_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetFireBarrel(objectFireBarrel value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasFireBarrel = true;
				this.result.fireBarrel_ = value;
				return this;
			}

			public SavedObject.Builder SetFireBarrel(objectFireBarrel.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasFireBarrel = true;
				this.result.fireBarrel_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetId(int value)
			{
				this.PrepareBuilder();
				this.result.hasId = true;
				this.result.id_ = value;
				return this;
			}

			public SavedObject.Builder SetInventory(int index, RustProto.Item value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.inventory_[index] = value;
				return this;
			}

			public SavedObject.Builder SetInventory(int index, RustProto.Item.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.inventory_[index] = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetLockable(objectLockable value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasLockable = true;
				this.result.lockable_ = value;
				return this;
			}

			public SavedObject.Builder SetLockable(objectLockable.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasLockable = true;
				this.result.lockable_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetNetInstance(objectNetInstance value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasNetInstance = true;
				this.result.netInstance_ = value;
				return this;
			}

			public SavedObject.Builder SetNetInstance(objectNetInstance.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasNetInstance = true;
				this.result.netInstance_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetNgcInstance(objectNGCInstance value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasNgcInstance = true;
				this.result.ngcInstance_ = value;
				return this;
			}

			public SavedObject.Builder SetNgcInstance(objectNGCInstance.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasNgcInstance = true;
				this.result.ngcInstance_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetSleepingAvatar(objectSleepingAvatar value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasSleepingAvatar = true;
				this.result.sleepingAvatar_ = value;
				return this;
			}

			public SavedObject.Builder SetSleepingAvatar(objectSleepingAvatar.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasSleepingAvatar = true;
				this.result.sleepingAvatar_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetSortOrder(int value)
			{
				this.PrepareBuilder();
				this.result.hasSortOrder = true;
				this.result.sortOrder_ = value;
				return this;
			}

			public SavedObject.Builder SetStructComponent(objectStructComponent value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasStructComponent = true;
				this.result.structComponent_ = value;
				return this;
			}

			public SavedObject.Builder SetStructComponent(objectStructComponent.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasStructComponent = true;
				this.result.structComponent_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetStructMaster(objectStructMaster value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasStructMaster = true;
				this.result.structMaster_ = value;
				return this;
			}

			public SavedObject.Builder SetStructMaster(objectStructMaster.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasStructMaster = true;
				this.result.structMaster_ = builderForValue.Build();
				return this;
			}

			public SavedObject.Builder SetTakeDamage(objectTakeDamage value)
			{
				ThrowHelper.ThrowIfNull(value, "value");
				this.PrepareBuilder();
				this.result.hasTakeDamage = true;
				this.result.takeDamage_ = value;
				return this;
			}

			public SavedObject.Builder SetTakeDamage(objectTakeDamage.Builder builderForValue)
			{
				ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
				this.PrepareBuilder();
				this.result.hasTakeDamage = true;
				this.result.takeDamage_ = builderForValue.Build();
				return this;
			}
		}
	}
}