using System;

public class actor : ConsoleSystem
{
	private static float last3rdPersonDistance;

	private static float last3rdPersonYaw;

	private static float last3rdPersonHeight;

	private static float last3rdPersonOffset;

	public static bool forceThirdPerson;

	static actor()
	{
		actor.last3rdPersonDistance = 2f;
		actor.last3rdPersonHeight = -0.5f;
	}

	public actor()
	{
	}

	private static bool GetCharacterStuff(ref ConsoleSystem.Arg args, out Character character, out CameraMount camera, out ItemRepresentation itemRep, out ArmorModelRenderer armor)
	{
		character = null;
		itemRep = null;
		armor = null;
		camera = CameraMount.current;
		if (!camera)
		{
			args.ReplyWith("Theres no active camera mount.");
			return false;
		}
		character = IDBase.GetMain(camera) as Character;
		if (!character)
		{
			args.ReplyWith("theres no character for the current mounted camera");
			return false;
		}
		armor = character.GetLocal<ArmorModelRenderer>();
		InventoryHolder local = character.GetLocal<InventoryHolder>();
		if (local)
		{
			itemRep = local.itemRepresentation;
		}
		return true;
	}
}