using Facepunch;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DatablockDictionary
{
	private const int expectedDBListLength = 14;

	private static Dictionary<string, int> _dataBlocks;

	private static Dictionary<int, int> _dataBlocksByUniqueID;

	private static ItemDataBlock[] _all;

	public static Dictionary<string, LootSpawnList> _lootSpawnLists;

	private static bool initializedAtLeastOnce;

	public static ItemDataBlock[] All
	{
		get
		{
			return DatablockDictionary._all;
		}
	}

	public DatablockDictionary()
	{
	}

	public static TArmorModel GetArmorModelByUniqueID<TArmorModel>(int uniqueID)
	where TArmorModel : ArmorModel, new()
	{
		ArmorDataBlock byUniqueID = DatablockDictionary.GetByUniqueID(uniqueID) as ArmorDataBlock;
		if (!byUniqueID)
		{
			return (TArmorModel)null;
		}
		return byUniqueID.GetArmorModel<TArmorModel>();
	}

	public static ArmorModel GetArmorModelByUniqueID(int uniqueID, ArmorModelSlot slot)
	{
		ArmorDataBlock byUniqueID = DatablockDictionary.GetByUniqueID(uniqueID) as ArmorDataBlock;
		if (!byUniqueID)
		{
			return null;
		}
		return byUniqueID.GetArmorModel(slot);
	}

	public static ItemDataBlock GetByName(string name)
	{
		int num;
		if (!DatablockDictionary._dataBlocks.TryGetValue(name, out num))
		{
			return null;
		}
		return DatablockDictionary._all[num];
	}

	public static ItemDataBlock GetByUniqueID(int uniqueID)
	{
		int num;
		if (!DatablockDictionary._dataBlocksByUniqueID.TryGetValue(uniqueID, out num))
		{
			return null;
		}
		return DatablockDictionary._all[num];
	}

	public static LootSpawnList GetLootSpawnListByName(string name)
	{
		LootSpawnList lootSpawnList;
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (!DatablockDictionary._lootSpawnLists.TryGetValue(name, out lootSpawnList))
		{
			Debug.LogError(string.Concat("Theres no loot spawn list with name ", name));
		}
		return lootSpawnList;
	}

	public static void Initialize()
	{
		DatablockDictionary._dataBlocks = new Dictionary<string, int>();
		DatablockDictionary._dataBlocksByUniqueID = new Dictionary<int, int>();
		DatablockDictionary._lootSpawnLists = new Dictionary<string, LootSpawnList>();
		List<ItemDataBlock> itemDataBlocks = new List<ItemDataBlock>();
		HashSet<ItemDataBlock> itemDataBlocks1 = new HashSet<ItemDataBlock>();
		ItemDataBlock[] itemDataBlockArray = Bundling.LoadAll<ItemDataBlock>();
		for (int i = 0; i < (int)itemDataBlockArray.Length; i++)
		{
			ItemDataBlock itemDataBlock = itemDataBlockArray[i];
			if (itemDataBlocks1.Add(itemDataBlock))
			{
				int count = itemDataBlocks.Count;
				DatablockDictionary._dataBlocks.Add(itemDataBlock.name, count);
				DatablockDictionary._dataBlocksByUniqueID.Add(itemDataBlock.uniqueID, count);
				itemDataBlocks.Add(itemDataBlock);
			}
		}
		DatablockDictionary._all = itemDataBlocks.ToArray();
		LootSpawnList[] lootSpawnListArray = Bundling.LoadAll<LootSpawnList>();
		for (int j = 0; j < (int)lootSpawnListArray.Length; j++)
		{
			LootSpawnList lootSpawnList = lootSpawnListArray[j];
			DatablockDictionary._lootSpawnLists.Add(lootSpawnList.name, lootSpawnList);
		}
		DatablockDictionary.initializedAtLeastOnce = true;
	}

	public static void TryInitialize()
	{
		if (!DatablockDictionary.initializedAtLeastOnce)
		{
			DatablockDictionary.Initialize();
		}
	}
}