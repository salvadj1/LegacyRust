using System;
using UnityEngine;

public class WeightSelection
{
	public WeightSelection()
	{
	}

	public static object RandomPick(WeightSelection.WeightedEntry[] array)
	{
		return WeightSelection.RandomPickEntry(array).obj;
	}

	public static T RandomPick<T>(WeightSelection.WeightedEntry<T>[] array)
	{
		return WeightSelection.RandomPickEntry<T>(array).obj;
	}

	public static WeightSelection.WeightedEntry RandomPickEntry(WeightSelection.WeightedEntry[] array)
	{
		float single = 0f;
		WeightSelection.WeightedEntry[] weightedEntryArray = array;
		for (int i = 0; i < (int)weightedEntryArray.Length; i++)
		{
			single = single + weightedEntryArray[i].weight;
		}
		if (single == 0f)
		{
			return null;
		}
		float single1 = UnityEngine.Random.Range(0f, single);
		WeightSelection.WeightedEntry[] weightedEntryArray1 = array;
		for (int j = 0; j < (int)weightedEntryArray1.Length; j++)
		{
			WeightSelection.WeightedEntry weightedEntry = weightedEntryArray1[j];
			float single2 = single1 - weightedEntry.weight;
			single1 = single2;
			if (single2 <= 0f)
			{
				return weightedEntry;
			}
		}
		return array[(int)array.Length - 1];
	}

	public static WeightSelection.WeightedEntry<T> RandomPickEntry<T>(WeightSelection.WeightedEntry<T>[] array)
	{
		float single = 0f;
		WeightSelection.WeightedEntry<T>[] weightedEntryArray = array;
		for (int i = 0; i < (int)weightedEntryArray.Length; i++)
		{
			single = single + weightedEntryArray[i].weight;
		}
		if (single == 0f)
		{
			return null;
		}
		float single1 = UnityEngine.Random.Range(0f, single);
		WeightSelection.WeightedEntry<T>[] weightedEntryArray1 = array;
		for (int j = 0; j < (int)weightedEntryArray1.Length; j++)
		{
			WeightSelection.WeightedEntry<T> weightedEntry = weightedEntryArray1[j];
			float single2 = single1 - weightedEntry.weight;
			single1 = single2;
			if (single2 <= 0f)
			{
				return weightedEntry;
			}
		}
		return array[(int)array.Length - 1];
	}

	[Serializable]
	public class WeightedEntry
	{
		public float weight;

		public UnityEngine.Object obj;

		public WeightedEntry()
		{
		}
	}

	[Serializable]
	public class WeightedEntry<T>
	{
		public float weight;

		public T obj;

		public WeightedEntry()
		{
		}
	}
}