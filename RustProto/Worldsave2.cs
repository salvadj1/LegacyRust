using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Proto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RustProto
{
	[DebuggerNonUserCode]
	public static class Worldsave
	{
		internal static MessageDescriptor internal__static_RustProto_objectDoor__Descriptor;

		internal static FieldAccessorTable<objectDoor, objectDoor.Builder> internal__static_RustProto_objectDoor__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectDeployable__Descriptor;

		internal static FieldAccessorTable<objectDeployable, objectDeployable.Builder> internal__static_RustProto_objectDeployable__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectStructMaster__Descriptor;

		internal static FieldAccessorTable<objectStructMaster, objectStructMaster.Builder> internal__static_RustProto_objectStructMaster__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectStructComponent__Descriptor;

		internal static FieldAccessorTable<objectStructComponent, objectStructComponent.Builder> internal__static_RustProto_objectStructComponent__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectFireBarrel__Descriptor;

		internal static FieldAccessorTable<objectFireBarrel, objectFireBarrel.Builder> internal__static_RustProto_objectFireBarrel__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectNetInstance__Descriptor;

		internal static FieldAccessorTable<objectNetInstance, objectNetInstance.Builder> internal__static_RustProto_objectNetInstance__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectNGCInstance__Descriptor;

		internal static FieldAccessorTable<objectNGCInstance, objectNGCInstance.Builder> internal__static_RustProto_objectNGCInstance__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectCoords__Descriptor;

		internal static FieldAccessorTable<objectCoords, objectCoords.Builder> internal__static_RustProto_objectCoords__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectICarriableTrans__Descriptor;

		internal static FieldAccessorTable<objectICarriableTrans, objectICarriableTrans.Builder> internal__static_RustProto_objectICarriableTrans__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectTakeDamage__Descriptor;

		internal static FieldAccessorTable<objectTakeDamage, objectTakeDamage.Builder> internal__static_RustProto_objectTakeDamage__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectSleepingAvatar__Descriptor;

		internal static FieldAccessorTable<objectSleepingAvatar, objectSleepingAvatar.Builder> internal__static_RustProto_objectSleepingAvatar__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_objectLockable__Descriptor;

		internal static FieldAccessorTable<objectLockable, objectLockable.Builder> internal__static_RustProto_objectLockable__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_SavedObject__Descriptor;

		internal static FieldAccessorTable<SavedObject, SavedObject.Builder> internal__static_RustProto_SavedObject__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_WorldSave__Descriptor;

		internal static FieldAccessorTable<WorldSave, WorldSave.Builder> internal__static_RustProto_WorldSave__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return Worldsave.descriptor;
			}
		}

		static Worldsave()
		{
			byte[] numArray = Convert.FromBase64String("ChRydXN0L3dvcmxkc2F2ZS5wcm90bxIJUnVzdFByb3RvGg9ydXN0L2l0ZW0ucHJvdG8aEXJ1c3QvY29tbW9uLnByb3RvGhFydXN0L3ZpdGFscy5wcm90byIpCgpvYmplY3REb29yEg0KBVN0YXRlGAEgASgFEgwKBE9wZW4YAiABKAgiNgoQb2JqZWN0RGVwbG95YWJsZRIRCglDcmVhdG9ySUQYASABKAQSDwoHT3duZXJJRBgCIAEoBCJYChJvYmplY3RTdHJ1Y3RNYXN0ZXISCgoCSUQYASABKAUSEgoKRGVjYXlEZWxheRgCIAEoAhIRCglDcmVhdG9ySUQYAyABKAQSDwoHT3duZXJJRBgEIAEoBCJLChVvYmplY3RTdHJ1Y3RDb21wb25lbnQSCgoCSUQYASABKAUSEAoITWFzdGVySUQYAiABKAUSFAoMTWFzdGVyVmlld0lEGAMgASgFIiIKEG9iamVjdEZpcmVCYXJyZWwSDgoGT25GaXJlGAEgASgIImQKEW9iamVjdE5ldEluc3RhbmNlEhQKDHNlcnZlclByZWZhYhgBIAEoBRITCgtvd25lclByZWZhYhgCIAEoBRITCgtwcm94eVByZWZhYhgDIAEoBRIPCgdncm91cElEGAQgASgFIi0KEW9iamVjdE5HQ0luc3RhbmNlEgoKAklEGAEgASgFEgwKBGRhdGEYAiABKAwinAEKDG9iamVjdENvb3JkcxIeCgNwb3MYASABKAsyES5SdXN0UHJvdG8uVmVjdG9yEiEKBm9sZFBvcxgCIAEoCzIRLlJ1c3RQcm90by5WZWN0b3ISIgoDcm90GAMgASgLMhUuUnVzdFByb3RvLlF1YXRlcm5pb24SJQoGb2xkUm90GAQgASgLMhUuUnVzdFByb3RvLlF1YXRlcm5pb24iLwoVb2JqZWN0SUNhcnJpYWJsZVRyYW5zEhYKDnRyYW5zQ2FycmllcklEGAEgASgFIiIKEG9iamVjdFRha2VEYW1hZ2USDgoGaGVhbHRoGAEgASgCIpgBChRvYmplY3RTbGVlcGluZ0F2YXRhchIRCglmb290QXJtb3IYASABKAUSEAoIbGVnQXJtb3IYAiABKAUSEgoKdG9yc29Bcm1vchgDIAEoBRIRCgloZWFkQXJtb3IYBCABKAUSEQoJdGltZXN0YW1wGAUgASgFEiEKBnZpdGFscxgGIAEoCzIRLlJ1c3RQcm90by5WaXRhbHMiQQoOb2JqZWN0TG9ja2FibGUSEAoIcGFzc3dvcmQYASABKAkSDgoGbG9ja2VkGAIgASgIEg0KBXVzZXJzGAMgAygEIqcFCgtTYXZlZE9iamVjdBIKCgJpZBgBIAEoBRIjCgRkb29yGAIgASgLMhUuUnVzdFByb3RvLm9iamVjdERvb3ISIgoJaW52ZW50b3J5GAMgAygLMg8uUnVzdFByb3RvLkl0ZW0SLwoKZGVwbG95YWJsZRgEIAEoCzIbLlJ1c3RQcm90by5vYmplY3REZXBsb3lhYmxlEjMKDHN0cnVjdE1hc3RlchgFIAEoCzIdLlJ1c3RQcm90by5vYmplY3RTdHJ1Y3RNYXN0ZXISOQoPc3RydWN0Q29tcG9uZW50GAYgASgLMiAuUnVzdFByb3RvLm9iamVjdFN0cnVjdENvbXBvbmVudBIvCgpmaXJlQmFycmVsGAcgASgLMhsuUnVzdFByb3RvLm9iamVjdEZpcmVCYXJyZWwSMQoLbmV0SW5zdGFuY2UYCCABKAsyHC5SdXN0UHJvdG8ub2JqZWN0TmV0SW5zdGFuY2USJwoGY29vcmRzGAkgASgLMhcuUnVzdFByb3RvLm9iamVjdENvb3JkcxIxCgtuZ2NJbnN0YW5jZRgKIAEoCzIcLlJ1c3RQcm90by5vYmplY3ROR0NJbnN0YW5jZRI4Cg5jYXJyaWFibGVUcmFucxgLIAEoCzIgLlJ1c3RQcm90by5vYmplY3RJQ2FycmlhYmxlVHJhbnMSLwoKdGFrZURhbWFnZRgMIAEoCzIbLlJ1c3RQcm90by5vYmplY3RUYWtlRGFtYWdlEhEKCXNvcnRPcmRlchgNIAEoBRI3Cg5zbGVlcGluZ0F2YXRhchgOIAEoCzIfLlJ1c3RQcm90by5vYmplY3RTbGVlcGluZ0F2YXRhchIrCghsb2NrYWJsZRgPIAEoCzIZLlJ1c3RQcm90by5vYmplY3RMb2NrYWJsZSJoCglXb3JsZFNhdmUSKwoLc2NlbmVPYmplY3QYASADKAsyFi5SdXN0UHJvdG8uU2F2ZWRPYmplY3QSLgoOaW5zdGFuY2VPYmplY3QYAiADKAsyFi5SdXN0UHJvdG8uU2F2ZWRPYmplY3RCAkgB");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				Worldsave.descriptor = root;
				Worldsave.internal__static_RustProto_objectDoor__Descriptor = Worldsave.Descriptor.MessageTypes[0];
				Worldsave.internal__static_RustProto_objectDoor__FieldAccessorTable = new FieldAccessorTable<objectDoor, objectDoor.Builder>(Worldsave.internal__static_RustProto_objectDoor__Descriptor, new string[] { "State", "Open" });
				Worldsave.internal__static_RustProto_objectDeployable__Descriptor = Worldsave.Descriptor.MessageTypes[1];
				Worldsave.internal__static_RustProto_objectDeployable__FieldAccessorTable = new FieldAccessorTable<objectDeployable, objectDeployable.Builder>(Worldsave.internal__static_RustProto_objectDeployable__Descriptor, new string[] { "CreatorID", "OwnerID" });
				Worldsave.internal__static_RustProto_objectStructMaster__Descriptor = Worldsave.Descriptor.MessageTypes[2];
				Worldsave.internal__static_RustProto_objectStructMaster__FieldAccessorTable = new FieldAccessorTable<objectStructMaster, objectStructMaster.Builder>(Worldsave.internal__static_RustProto_objectStructMaster__Descriptor, new string[] { "ID", "DecayDelay", "CreatorID", "OwnerID" });
				Worldsave.internal__static_RustProto_objectStructComponent__Descriptor = Worldsave.Descriptor.MessageTypes[3];
				Worldsave.internal__static_RustProto_objectStructComponent__FieldAccessorTable = new FieldAccessorTable<objectStructComponent, objectStructComponent.Builder>(Worldsave.internal__static_RustProto_objectStructComponent__Descriptor, new string[] { "ID", "MasterID", "MasterViewID" });
				Worldsave.internal__static_RustProto_objectFireBarrel__Descriptor = Worldsave.Descriptor.MessageTypes[4];
				Worldsave.internal__static_RustProto_objectFireBarrel__FieldAccessorTable = new FieldAccessorTable<objectFireBarrel, objectFireBarrel.Builder>(Worldsave.internal__static_RustProto_objectFireBarrel__Descriptor, new string[] { "OnFire" });
				Worldsave.internal__static_RustProto_objectNetInstance__Descriptor = Worldsave.Descriptor.MessageTypes[5];
				Worldsave.internal__static_RustProto_objectNetInstance__FieldAccessorTable = new FieldAccessorTable<objectNetInstance, objectNetInstance.Builder>(Worldsave.internal__static_RustProto_objectNetInstance__Descriptor, new string[] { "ServerPrefab", "OwnerPrefab", "ProxyPrefab", "GroupID" });
				Worldsave.internal__static_RustProto_objectNGCInstance__Descriptor = Worldsave.Descriptor.MessageTypes[6];
				Worldsave.internal__static_RustProto_objectNGCInstance__FieldAccessorTable = new FieldAccessorTable<objectNGCInstance, objectNGCInstance.Builder>(Worldsave.internal__static_RustProto_objectNGCInstance__Descriptor, new string[] { "ID", "Data" });
				Worldsave.internal__static_RustProto_objectCoords__Descriptor = Worldsave.Descriptor.MessageTypes[7];
				Worldsave.internal__static_RustProto_objectCoords__FieldAccessorTable = new FieldAccessorTable<objectCoords, objectCoords.Builder>(Worldsave.internal__static_RustProto_objectCoords__Descriptor, new string[] { "Pos", "OldPos", "Rot", "OldRot" });
				Worldsave.internal__static_RustProto_objectICarriableTrans__Descriptor = Worldsave.Descriptor.MessageTypes[8];
				Worldsave.internal__static_RustProto_objectICarriableTrans__FieldAccessorTable = new FieldAccessorTable<objectICarriableTrans, objectICarriableTrans.Builder>(Worldsave.internal__static_RustProto_objectICarriableTrans__Descriptor, new string[] { "TransCarrierID" });
				Worldsave.internal__static_RustProto_objectTakeDamage__Descriptor = Worldsave.Descriptor.MessageTypes[9];
				Worldsave.internal__static_RustProto_objectTakeDamage__FieldAccessorTable = new FieldAccessorTable<objectTakeDamage, objectTakeDamage.Builder>(Worldsave.internal__static_RustProto_objectTakeDamage__Descriptor, new string[] { "Health" });
				Worldsave.internal__static_RustProto_objectSleepingAvatar__Descriptor = Worldsave.Descriptor.MessageTypes[10];
				Worldsave.internal__static_RustProto_objectSleepingAvatar__FieldAccessorTable = new FieldAccessorTable<objectSleepingAvatar, objectSleepingAvatar.Builder>(Worldsave.internal__static_RustProto_objectSleepingAvatar__Descriptor, new string[] { "FootArmor", "LegArmor", "TorsoArmor", "HeadArmor", "Timestamp", "Vitals" });
				Worldsave.internal__static_RustProto_objectLockable__Descriptor = Worldsave.Descriptor.MessageTypes[11];
				Worldsave.internal__static_RustProto_objectLockable__FieldAccessorTable = new FieldAccessorTable<objectLockable, objectLockable.Builder>(Worldsave.internal__static_RustProto_objectLockable__Descriptor, new string[] { "Password", "Locked", "Users" });
				Worldsave.internal__static_RustProto_SavedObject__Descriptor = Worldsave.Descriptor.MessageTypes[12];
				Worldsave.internal__static_RustProto_SavedObject__FieldAccessorTable = new FieldAccessorTable<SavedObject, SavedObject.Builder>(Worldsave.internal__static_RustProto_SavedObject__Descriptor, new string[] { "Id", "Door", "Inventory", "Deployable", "StructMaster", "StructComponent", "FireBarrel", "NetInstance", "Coords", "NgcInstance", "CarriableTrans", "TakeDamage", "SortOrder", "SleepingAvatar", "Lockable" });
				Worldsave.internal__static_RustProto_WorldSave__Descriptor = Worldsave.Descriptor.MessageTypes[13];
				Worldsave.internal__static_RustProto_WorldSave__FieldAccessorTable = new FieldAccessorTable<WorldSave, WorldSave.Builder>(Worldsave.internal__static_RustProto_WorldSave__Descriptor, new string[] { "SceneObject", "InstanceObject" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[] { RustProto.Proto.Item.Descriptor, Common.Descriptor, RustProto.Proto.Vitals.Descriptor }, internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}