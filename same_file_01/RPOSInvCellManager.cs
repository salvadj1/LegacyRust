using System;
using UnityEngine;

public class RPOSInvCellManager : MonoBehaviour
{
	public bool SpawnCells;

	private bool generatedCells;

	public int NumCellsHorizontal;

	public int NumCellsVertical;

	public int CellOffsetX;

	public int CellOffsetY;

	public int CellSize = 96;

	public int CellSpacing = 10;

	public int CellIndexStart;

	public bool CenterFromCells;

	public bool NumberedCells;

	public GameObject CellPrefab;

	public Inventory displayInventory;

	public RPOSInventoryCell[] _inventoryCells;

	public RPOSInvCellManager()
	{
	}

	private void Awake()
	{
		if (this.SpawnCells)
		{
			this.CreateCellsOnGameObject(null, base.gameObject);
		}
	}

	protected virtual void CreateCellsOnGameObject(Inventory inven, GameObject parent)
	{
		int numCells;
		int end;
		Inventory.Slot.Range range;
		if (!inven)
		{
			numCells = this.GetNumCells();
			end = 2147483647;
		}
		else
		{
			inven.GetSlotsOfKind(Inventory.Slot.Kind.Default, out range);
			numCells = range.Count;
			end = range.End;
		}
		Array.Resize<RPOSInventoryCell>(ref this._inventoryCells, numCells);
		Vector3 component = this.CellPrefab.GetComponent<RPOSInventoryCell>()._background.transform.localScale;
		float single = component.x;
		Vector3 vector3 = this.CellPrefab.GetComponent<RPOSInventoryCell>()._background.transform.localScale;
		float single1 = vector3.y;
		for (int i = 0; i < this.NumCellsVertical; i++)
		{
			for (int j = 0; j < this.NumCellsHorizontal; j++)
			{
				byte cellIndexStart = (byte)(this.CellIndexStart + RPOSInvCellManager.GetIndex2D(j, i, this.NumCellsHorizontal));
				if (cellIndexStart >= end)
				{
					return;
				}
				GameObject gameObject = NGUITools.AddChild(parent, this.CellPrefab);
				RPOSInventoryCell str = gameObject.GetComponent<RPOSInventoryCell>();
				str._mySlot = cellIndexStart;
				str._displayInventory = inven;
				if (this.NumberedCells)
				{
					int index2D = RPOSInvCellManager.GetIndex2D(j, i, this.NumCellsHorizontal) + 1;
					str._numberLabel.text = index2D.ToString();
				}
				gameObject.transform.localPosition = new Vector3((float)this.CellOffsetX + ((float)j * single + (float)(j * this.CellSpacing)), -((float)this.CellOffsetY + ((float)i * single1 + (float)(i * this.CellSpacing))), -2f);
				this._inventoryCells[RPOS.GetIndex2D(j, i, this.NumCellsHorizontal)] = gameObject.GetComponent<RPOSInventoryCell>();
			}
		}
		if (this.CenterFromCells)
		{
			if (this.NumCellsHorizontal > 1)
			{
				base.transform.localPosition = new Vector3((float)(this.CellOffsetX + this.NumCellsHorizontal * this.CellSize + (this.NumCellsHorizontal - 1) * this.CellSpacing) * -0.5f, (float)this.CellSize, 0f);
			}
			else if (this.NumCellsVertical > 1)
			{
				base.transform.localPosition = new Vector3((float)(-this.CellSize), (float)(this.CellOffsetY + this.NumCellsVertical * this.CellSize) * 0.5f, 0f);
			}
		}
	}

	public static int GetIndex2D(int x, int y, int width)
	{
		return x + y * width;
	}

	public int GetNumCells()
	{
		if (!this.SpawnCells && !this.generatedCells)
		{
			return (int)this._inventoryCells.Length;
		}
		return this.NumCellsHorizontal * this.NumCellsVertical;
	}

	public void SetInventory(Inventory newInv, bool spawnNewCells)
	{
		this.displayInventory = newInv;
		if (spawnNewCells && this.SpawnCells)
		{
			this.generatedCells = true;
			for (int i = 0; i < (int)this._inventoryCells.Length; i++)
			{
				UnityEngine.Object.Destroy(this._inventoryCells[i].gameObject);
				this._inventoryCells[i] = null;
			}
			this.NumCellsVertical = Mathf.CeilToInt((float)newInv.slotCount / 3f);
			this.CreateCellsOnGameObject(newInv, base.gameObject);
		}
		int num = 0;
		RPOSInventoryCell[] rPOSInventoryCellArray = this._inventoryCells;
		for (int j = 0; j < (int)rPOSInventoryCellArray.Length; j++)
		{
			RPOSInventoryCell cellIndexStart = rPOSInventoryCellArray[j];
			cellIndexStart._displayInventory = newInv;
			cellIndexStart._mySlot = (byte)(this.CellIndexStart + num);
			newInv.MarkSlotDirty((int)cellIndexStart._mySlot);
			num++;
		}
	}
}