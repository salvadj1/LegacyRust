using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RustProto.Proto
{
	[DebuggerNonUserCode]
	public static class Avatar
	{
		internal static MessageDescriptor internal__static_RustProto_Avatar__Descriptor;

		internal static FieldAccessorTable<RustProto.Avatar, RustProto.Avatar.Builder> internal__static_RustProto_Avatar__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_AwayEvent__Descriptor;

		internal static FieldAccessorTable<AwayEvent, AwayEvent.Builder> internal__static_RustProto_AwayEvent__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Avatar.descriptor;
			}
		}

		static Avatar()
		{
			byte[] numArray = Convert.FromBase64String("ChFydXN0L2F2YXRhci5wcm90bxIJUnVzdFByb3RvGhRydXN0L2JsdWVwcmludC5wcm90bxoPcnVzdC9pdGVtLnByb3RvGhFydXN0L2NvbW1vbi5wcm90bxoRcnVzdC92aXRhbHMucHJvdG8iqAIKBkF2YXRhchIeCgNwb3MYASABKAsyES5SdXN0UHJvdG8uVmVjdG9yEiIKA2FuZxgCIAEoCzIVLlJ1c3RQcm90by5RdWF0ZXJuaW9uEiEKBnZpdGFscxgDIAEoCzIRLlJ1c3RQcm90by5WaXRhbHMSKAoKYmx1ZXByaW50cxgEIAMoCzIULlJ1c3RQcm90by5CbHVlcHJpbnQSIgoJaW52ZW50b3J5GAUgAygLMg8uUnVzdFByb3RvLkl0ZW0SIQoId2VhcmFibGUYBiADKAsyDy5SdXN0UHJvdG8uSXRlbRIdCgRiZWx0GAcgAygLMg8uUnVzdFByb3RvLkl0ZW0SJwoJYXdheUV2ZW50GAggASgLMhQuUnVzdFByb3RvLkF3YXlFdmVudCKZAQoJQXdheUV2ZW50EjAKBHR5cGUYASACKA4yIi5SdXN0UHJvdG8uQXdheUV2ZW50LkF3YXlFdmVudFR5cGUSEQoJdGltZXN0YW1wGAIgAigFEhIKCmluc3RpZ2F0b3IYAyABKAQiMwoNQXdheUV2ZW50VHlwZRILCgdVTktOT1dOEAASCwoHU0xVTUJFUhABEggKBERJRUQQAkICSAE=");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				RustProto.Proto.Avatar.descriptor = root;
				RustProto.Proto.Avatar.internal__static_RustProto_Avatar__Descriptor = RustProto.Proto.Avatar.Descriptor.MessageTypes[0];
				RustProto.Proto.Avatar.internal__static_RustProto_Avatar__FieldAccessorTable = new FieldAccessorTable<RustProto.Avatar, RustProto.Avatar.Builder>(RustProto.Proto.Avatar.internal__static_RustProto_Avatar__Descriptor, new string[] { "Pos", "Ang", "Vitals", "Blueprints", "Inventory", "Wearable", "Belt", "AwayEvent" });
				RustProto.Proto.Avatar.internal__static_RustProto_AwayEvent__Descriptor = RustProto.Proto.Avatar.Descriptor.MessageTypes[1];
				RustProto.Proto.Avatar.internal__static_RustProto_AwayEvent__FieldAccessorTable = new FieldAccessorTable<AwayEvent, AwayEvent.Builder>(RustProto.Proto.Avatar.internal__static_RustProto_AwayEvent__Descriptor, new string[] { "Type", "Timestamp", "Instigator" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[] { RustProto.Proto.Blueprint.Descriptor, RustProto.Proto.Item.Descriptor, Common.Descriptor, RustProto.Proto.Vitals.Descriptor }, internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}