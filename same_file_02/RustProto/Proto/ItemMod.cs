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
	public static class ItemMod
	{
		internal static MessageDescriptor internal__static_RustProto_ItemMod__Descriptor;

		internal static FieldAccessorTable<RustProto.ItemMod, RustProto.ItemMod.Builder> internal__static_RustProto_ItemMod__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.ItemMod.descriptor;
			}
		}

		static ItemMod()
		{
			byte[] numArray = Convert.FromBase64String("ChNydXN0L2l0ZW1fbW9kLnByb3RvEglSdXN0UHJvdG8iIwoHSXRlbU1vZBIKCgJpZBgBIAIoBRIMCgRuYW1lGAIgASgJQgJIAQ==");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				RustProto.Proto.ItemMod.descriptor = root;
				RustProto.Proto.ItemMod.internal__static_RustProto_ItemMod__Descriptor = RustProto.Proto.ItemMod.Descriptor.MessageTypes[0];
				RustProto.Proto.ItemMod.internal__static_RustProto_ItemMod__FieldAccessorTable = new FieldAccessorTable<RustProto.ItemMod, RustProto.ItemMod.Builder>(RustProto.Proto.ItemMod.internal__static_RustProto_ItemMod__Descriptor, new string[] { "Id", "Name" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[0], internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}