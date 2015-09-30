using RustProto;
using System;
using UnityEngine;

public class AvatarSaveRestore : MonoBehaviour
{
	public AvatarSaveRestore()
	{
	}

	public static void CopyPersistantMessages(ref RustProto.Avatar.Builder builder, ref RustProto.Avatar avatar)
	{
		builder.ClearBlueprints();
		for (int i = 0; i < avatar.BlueprintsCount; i++)
		{
			builder.AddBlueprints(avatar.GetBlueprints(i));
		}
	}
}