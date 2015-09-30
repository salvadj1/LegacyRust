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
	public static class Vitals
	{
		internal static MessageDescriptor internal__static_RustProto_Vitals__Descriptor;

		internal static FieldAccessorTable<RustProto.Vitals, RustProto.Vitals.Builder> internal__static_RustProto_Vitals__FieldAccessorTable;

		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Vitals.descriptor;
			}
		}

		static Vitals()
		{
			byte[] numArray = Convert.FromBase64String("ChFydXN0L3ZpdGFscy5wcm90bxIJUnVzdFByb3RvIu8BCgZWaXRhbHMSEwoGaGVhbHRoGAEgASgCOgMxMDASFQoJaHlkcmF0aW9uGAIgASgCOgIzMBIWCghjYWxvcmllcxgDIAEoAjoEMTAwMBIUCglyYWRpYXRpb24YBCABKAI6ATASGQoOcmFkaWF0aW9uX2FudGkYBSABKAI6ATASFgoLYmxlZWRfc3BlZWQYBiABKAI6ATASFAoJYmxlZWRfbWF4GAcgASgCOgEwEhUKCmhlYWxfc3BlZWQYCCABKAI6ATASEwoIaGVhbF9tYXgYCSABKAI6ATASFgoLdGVtcGVyYXR1cmUYCiABKAI6ATBCAkgB");
			FileDescriptor.InternalDescriptorAssigner internalDescriptorAssigner = (FileDescriptor root) => {
				RustProto.Proto.Vitals.descriptor = root;
				RustProto.Proto.Vitals.internal__static_RustProto_Vitals__Descriptor = RustProto.Proto.Vitals.Descriptor.MessageTypes[0];
				RustProto.Proto.Vitals.internal__static_RustProto_Vitals__FieldAccessorTable = new FieldAccessorTable<RustProto.Vitals, RustProto.Vitals.Builder>(RustProto.Proto.Vitals.internal__static_RustProto_Vitals__Descriptor, new string[] { "Health", "Hydration", "Calories", "Radiation", "RadiationAnti", "BleedSpeed", "BleedMax", "HealSpeed", "HealMax", "Temperature" });
				return null;
			};
			FileDescriptor.InternalBuildGeneratedFileFrom(numArray, new FileDescriptor[0], internalDescriptorAssigner);
		}

		public static void RegisterAllExtensions(ExtensionRegistry registry)
		{
		}
	}
}