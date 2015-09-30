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
	public static class User
	{
		internal static MessageDescriptor internal__static_RustProto_User__Descriptor;

		internal static FieldAccessorTable<RustProto.User, RustProto.User.Builder> internal__static_RustProto_User__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.User.descriptor;
			}
		}

		static User()
		{
			byte[] numArray = Convert.FromBase64String("Cg9ydXN0L3VzZXIucHJvdG8SCVJ1c3RQcm90byKKAQoEVXNlchIOCgZ1c2VyaWQYASACKAQSEwoLZGlzcGxheW5hbWUYAiACKAkSLAoJdXNlcmdyb3VwGAMgAigOMhkuUnVzdFByb3RvLlVzZXIuVXNlckdyb3VwIi8KCVVzZXJHcm91cBILCgdSRUdVTEFSEAASCgoGQkFOTkVEEAESCQoFQURNSU4QAkICSAE=");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				RustProto.Proto.User.descriptor = root;
				RustProto.Proto.User.internal__static_RustProto_User__Descriptor = RustProto.Proto.User.Descriptor.MessageTypes[0];
				RustProto.Proto.User.internal__static_RustProto_User__FieldAccessorTable = new FieldAccessorTable<RustProto.User, RustProto.User.Builder>(RustProto.Proto.User.internal__static_RustProto_User__Descriptor, new string[] { "Userid", "Displayname", "Usergroup" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[0], internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}