using System;

public abstract class IngredientList
{
	public const uint seed = 4027449069;

	protected static int[] tempHash;

	static IngredientList()
	{
		IngredientList.tempHash = new int[16];
	}

	protected IngredientList()
	{
	}
}