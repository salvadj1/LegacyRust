using System;
using UnityEngine;

public class LootSpawnList : ScriptableObject
{
	public LootSpawnList.LootWeightedEntry[] LootPackages;

	public int minPackagesToSpawn = 1;

	public int maxPackagesToSpawn = 1;

	public bool noDuplicates;

	public bool spawnOneOfEach;

	public LootSpawnList()
	{
	}

	public void PopulateInventory(Inventory inven)
	{
		LootSpawnList.RecursiveInventoryPopulateArgs recursiveInventoryPopulateArg = new LootSpawnList.RecursiveInventoryPopulateArgs();
		recursiveInventoryPopulateArg.inventory = inven;
		recursiveInventoryPopulateArg.spawnCount = 0;
		recursiveInventoryPopulateArg.inventoryExausted = inven.noVacantSlots;
		if (!recursiveInventoryPopulateArg.inventoryExausted)
		{
			this.PopulateInventory_Recurse(ref recursiveInventoryPopulateArg);
		}
	}

	private void PopulateInventory_Recurse(ref LootSpawnList.RecursiveInventoryPopulateArgs args)
	{
		if (this.maxPackagesToSpawn > (int)this.LootPackages.Length)
		{
			this.maxPackagesToSpawn = (int)this.LootPackages.Length;
		}
		int num = 0;
		num = (!this.spawnOneOfEach ? UnityEngine.Random.Range(this.minPackagesToSpawn, this.maxPackagesToSpawn) : (int)this.LootPackages.Length);
		for (int i = 0; !args.inventoryExausted && i < num; i++)
		{
			LootSpawnList.LootWeightedEntry lootWeightedEntry = null;
			lootWeightedEntry = (!this.spawnOneOfEach ? WeightSelection.RandomPickEntry(this.LootPackages) as LootSpawnList.LootWeightedEntry : this.LootPackages[i]);
			if (lootWeightedEntry == null)
			{
				Debug.Log("Massive fuckup...");
				return;
			}
			UnityEngine.Object obj = lootWeightedEntry.obj;
			if (obj)
			{
				if (obj is ItemDataBlock)
				{
					if (!object.ReferenceEquals(args.inventory.AddItem(obj as ItemDataBlock, Inventory.Slot.Preference.Define(args.spawnCount, false, Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt), UnityEngine.Random.Range(lootWeightedEntry.amountMin, lootWeightedEntry.amountMax + 1)), null))
					{
						args.spawnCount = args.spawnCount + 1;
						if (args.inventory.noVacantSlots)
						{
							args.inventoryExausted = true;
						}
					}
				}
				else if (obj is LootSpawnList)
				{
					((LootSpawnList)obj).PopulateInventory_Recurse(ref args);
				}
			}
		}
	}

	[Serializable]
	public class LootWeightedEntry : WeightSelection.WeightedEntry
	{
		public int amountMin;

		public int amountMax;

		public LootWeightedEntry()
		{
		}
	}

	private struct RecursiveInventoryPopulateArgs
	{
		public Inventory inventory;

		public int spawnCount;

		public bool inventoryExausted;
	}
}