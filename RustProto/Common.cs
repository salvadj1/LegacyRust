using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RustProto
{
	[DebuggerNonUserCode]
	public static class Common
	{
		internal static MessageDescriptor internal__static_RustProto_Vector__Descriptor;

		internal static FieldAccessorTable<Vector, Vector.Builder> internal__static_RustProto_Vector__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_Quaternion__Descriptor;

		internal static FieldAccessorTable<Quaternion, Quaternion.Builder> internal__static_RustProto_Quaternion__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return Common.descriptor;
			}
		}

		static Common()
		{
			byte[] numArray = Convert.FromBase64String("ChFydXN0L2NvbW1vbi5wcm90bxIJUnVzdFByb3RvIjIKBlZlY3RvchIMCgF4GAEgASgCOgEwEgwKAXkYAiABKAI6ATASDAoBehgDIAEoAjoBMCJECgpRdWF0ZXJuaW9uEgwKAXgYASABKAI6ATASDAoBeRgCIAEoAjoBMBIMCgF6GAMgASgCOgEwEgwKAXcYBCABKAI6ATBCAkgB");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				Common.descriptor = root;
				Common.internal__static_RustProto_Vector__Descriptor = Common.Descriptor.MessageTypes[0];
				Common.internal__static_RustProto_Vector__FieldAccessorTable = new FieldAccessorTable<Vector, Vector.Builder>(Common.internal__static_RustProto_Vector__Descriptor, new string[] { "X", "Y", "Z" });
				Common.internal__static_RustProto_Quaternion__Descriptor = Common.Descriptor.MessageTypes[1];
				Common.internal__static_RustProto_Quaternion__FieldAccessorTable = new FieldAccessorTable<Quaternion, Quaternion.Builder>(Common.internal__static_RustProto_Quaternion__Descriptor, new string[] { "X", "Y", "Z", "W" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[0], internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}