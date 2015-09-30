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
	public static class Blueprint
	{
		internal static MessageDescriptor internal__static_RustProto_Blueprint__Descriptor;

		internal static FieldAccessorTable<RustProto.Blueprint, RustProto.Blueprint.Builder> internal__static_RustProto_Blueprint__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Blueprint.descriptor;
			}
		}

		static Blueprint()
		{
			byte[] numArray = Convert.FromBase64String("ChRydXN0L2JsdWVwcmludC5wcm90bxIJUnVzdFByb3RvIhcKCUJsdWVwcmludBIKCgJpZBgBIAIoBUICSAE=");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				RustProto.Proto.Blueprint.descriptor = root;
				RustProto.Proto.Blueprint.internal__static_RustProto_Blueprint__Descriptor = RustProto.Proto.Blueprint.Descriptor.MessageTypes[0];
				RustProto.Proto.Blueprint.internal__static_RustProto_Blueprint__FieldAccessorTable = new FieldAccessorTable<RustProto.Blueprint, RustProto.Blueprint.Builder>(RustProto.Proto.Blueprint.internal__static_RustProto_Blueprint__Descriptor, new string[] { "Id" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[0], internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}