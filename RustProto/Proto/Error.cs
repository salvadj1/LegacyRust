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
	public static class Error
	{
		internal static MessageDescriptor internal__static_RustProto_Error__Descriptor;

		internal static FieldAccessorTable<RustProto.Error, RustProto.Error.Builder> internal__static_RustProto_Error__FieldAccessorTable;

		internal static MessageDescriptor internal__static_RustProto_GameError__Descriptor;

		internal static FieldAccessorTable<GameError, GameError.Builder> internal__static_RustProto_GameError__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Error.descriptor;
			}
		}

		static Error()
		{
			byte[] numArray = Convert.FromBase64String("ChBydXN0L2Vycm9yLnByb3RvEglSdXN0UHJvdG8iKAoFRXJyb3ISDgoGc3RhdHVzGAEgAigJEg8KB21lc3NhZ2UYAiACKAkiKQoJR2FtZUVycm9yEg0KBWVycm9yGAEgAigJEg0KBXRyYWNlGAIgAigJQgJIAQ==");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				RustProto.Proto.Error.descriptor = root;
				RustProto.Proto.Error.internal__static_RustProto_Error__Descriptor = RustProto.Proto.Error.Descriptor.MessageTypes[0];
				RustProto.Proto.Error.internal__static_RustProto_Error__FieldAccessorTable = new FieldAccessorTable<RustProto.Error, RustProto.Error.Builder>(RustProto.Proto.Error.internal__static_RustProto_Error__Descriptor, new string[] { "Status", "Message" });
				RustProto.Proto.Error.internal__static_RustProto_GameError__Descriptor = RustProto.Proto.Error.Descriptor.MessageTypes[1];
				RustProto.Proto.Error.internal__static_RustProto_GameError__FieldAccessorTable = new FieldAccessorTable<GameError, GameError.Builder>(RustProto.Proto.Error.internal__static_RustProto_GameError__Descriptor, new string[] { "Error", "Trace" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[0], internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}