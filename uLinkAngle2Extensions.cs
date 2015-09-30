using System;
using System.Runtime.CompilerServices;
using uLink;

public static class uLinkAngle2Extensions
{
	private const BitStreamTypeCode bitStreamTypeCode = BitStreamTypeCode.Int32;

	private readonly static BitStreamCodec int32Codec;

	private readonly static BitStreamCodec.Deserializer deserializer;

	private readonly static BitStreamCodec.Serializer serializer;

	static uLinkAngle2Extensions()
	{
		uLinkAngle2Extensions.int32Codec = BitStreamCodec.Find(typeof(int).TypeHandle);
		uLinkAngle2Extensions.serializer = new BitStreamCodec.Serializer(uLinkAngle2Extensions.Serializer);
		uLinkAngle2Extensions.deserializer = new BitStreamCodec.Deserializer(uLinkAngle2Extensions.Deserializer);
		BitStreamCodec.Add<Angle2>(uLinkAngle2Extensions.deserializer, uLinkAngle2Extensions.serializer, BitStreamTypeCode.Int32, false);
	}

	private static object Deserializer(BitStream stream, params object[] codecOptions)
	{
		object obj = uLinkAngle2Extensions.int32Codec.deserializer(stream, codecOptions);
		if (!(obj is int))
		{
			return obj;
		}
		return new Angle2()
		{
			encoded = (int)obj
		};
	}

	public static Angle2 ReadAngle2(this BitStream stream)
	{
		return new Angle2()
		{
			encoded = stream.ReadInt32()
		};
	}

	public static void Register()
	{
	}

	public static void Serialize(this BitStream stream, ref Angle2 value, params object[] codecOptions)
	{
		int num = value.encoded;
		int num1 = num;
		stream.Serialize(ref num1, codecOptions);
		if (num1 != num)
		{
			value.encoded = num1;
		}
	}

	private static void Serializer(BitStream stream, object value, params object[] codecOptions)
	{
		uLinkAngle2Extensions.int32Codec.serializer(stream, ((Angle2)value).encoded, codecOptions);
	}

	public static void WriteAngle2(this BitStream stream, Angle2 value)
	{
		stream.WriteInt32(value.encoded);
	}
}