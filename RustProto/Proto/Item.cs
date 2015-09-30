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
	public static class Item
	{
		internal static MessageDescriptor internal__static_RustProto_Item__Descriptor;

		internal static FieldAccessorTable<RustProto.Item, RustProto.Item.Builder> internal__static_RustProto_Item__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Item.descriptor;
			}
		}

		static Item()
		{
			byte[] numArray = Convert.FromBase64String("Cg9ydXN0L2l0ZW0ucHJvdG8SCVJ1c3RQcm90bxoTcnVzdC9pdGVtX21vZC5wcm90byKaAQoESXRlbRIKCgJpZBgBIAIoBRIMCgRuYW1lGAIgASgJEgwKBHNsb3QYAyABKAUSDQoFY291bnQYBCABKAUSEAoIc3Vic2xvdHMYBiABKAUSEQoJY29uZGl0aW9uGAcgASgCEhQKDG1heGNvbmRpdGlvbhgIIAEoAhIgCgdzdWJpdGVtGAUgAygLMg8uUnVzdFByb3RvLkl0ZW1CAkgB");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				RustProto.Proto.Item.descriptor = root;
				RustProto.Proto.Item.internal__static_RustProto_Item__Descriptor = RustProto.Proto.Item.Descriptor.MessageTypes[0];
				RustProto.Proto.Item.internal__static_RustProto_Item__FieldAccessorTable = new FieldAccessorTable<RustProto.Item, RustProto.Item.Builder>(RustProto.Proto.Item.internal__static_RustProto_Item__Descriptor, new string[] { "Id", "Name", "Slot", "Count", "Subslots", "Condition", "Maxcondition", "Subitem" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[] { RustProto.Proto.ItemMod.Descriptor }, internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}