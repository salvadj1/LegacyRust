using System;
using System.Runtime.CompilerServices;
using uLink;

public static class CharacterStateFlagsExtenders
{
	public static CharacterStateFlags ReadCharacterStateFlags(this BitStream stream)
	{
		CharacterStateFlags characterStateFlag = new CharacterStateFlags();
		characterStateFlag.flags = stream.ReadUInt16();
		return characterStateFlag;
	}

	public static void Serialize(this BitStream stream, ref CharacterStateFlags v)
	{
		stream.Serialize<ushort>(ref v.flags, new object[0]);
	}

	public static void WriteCharacterStateFlags(this BitStream stream, CharacterStateFlags v)
	{
		stream.WriteUInt16(v.flags);
	}
}