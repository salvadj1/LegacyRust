using System;

public struct UITextSelection
{
	private const UITextSelection.Change kSelectChange_None = UITextSelection.Change.None;

	private const UITextSelection.Change kSelectChange_DropCarrat = UITextSelection.Change.CarratToNone;

	private const UITextSelection.Change kSelectChange_MoveCarrat = UITextSelection.Change.CarratMove;

	private const UITextSelection.Change kSelectChange_NewCarrat = UITextSelection.Change.NoneToCarrat;

	private const UITextSelection.Change kSelectChange_DropSelection = UITextSelection.Change.SelectionToCarrat;

	private const UITextSelection.Change kSelectChange_MoveSelection = UITextSelection.Change.SelectionAdjusted;

	private const UITextSelection.Change kSelectChange_NewSelection = UITextSelection.Change.CarratToSelection;

	private const UITextSelection.Change kSelectChange_DropAll = UITextSelection.Change.SelectionToNone;

	private const UITextSelection.Change kSelectChange_NewAll = UITextSelection.Change.NoneToSelection;

	public UITextPosition carratPos;

	public UITextPosition selectPos;

	public int carratIndex
	{
		get
		{
			if (this.carratPos.position != this.selectPos.position && this.selectPos.valid || !this.carratPos.valid)
			{
				return -1;
			}
			return this.carratPos.position;
		}
	}

	public bool hasSelection
	{
		get
		{
			return (!this.carratPos.valid || !this.selectPos.valid ? false : this.carratPos.position != this.selectPos.position);
		}
	}

	public int highlightBegin
	{
		get
		{
			if (!this.carratPos.valid || !this.selectPos.valid || this.selectPos.position == this.carratPos.position)
			{
				return -1;
			}
			return (this.selectPos.position >= this.carratPos.position ? this.carratPos.position : this.selectPos.position);
		}
	}

	public int highlightEnd
	{
		get
		{
			if (!this.carratPos.valid || !this.selectPos.valid || this.selectPos.position == this.carratPos.position)
			{
				return -1;
			}
			return (this.selectPos.position >= this.carratPos.position ? this.selectPos.position : this.carratPos.position);
		}
	}

	public int selectIndex
	{
		get
		{
			if (!this.carratPos.valid || !this.selectPos.valid || this.selectPos.position == this.carratPos.position)
			{
				return -1;
			}
			return this.selectPos.position;
		}
	}

	public bool showCarrat
	{
		get
		{
			bool flag;
			if (!this.carratPos.valid)
			{
				flag = false;
			}
			else
			{
				flag = (!this.selectPos.valid ? true : this.selectPos.position == this.carratPos.position);
			}
			return flag;
		}
	}

	public bool valid
	{
		get
		{
			return this.carratPos.valid;
		}
	}

	public UITextSelection(UITextPosition carratPos, UITextPosition selectPos)
	{
		this.carratPos = carratPos;
		this.selectPos = selectPos;
	}

	public UITextSelection.Change GetChangesTo(ref UITextSelection value)
	{
		UITextSelection.Change change;
		if (!this.carratPos.valid)
		{
			if (!value.carratPos.valid)
			{
				change = UITextSelection.Change.None;
			}
			else
			{
				change = (!value.hasSelection ? UITextSelection.Change.NoneToCarrat : UITextSelection.Change.NoneToSelection);
			}
		}
		else if (!value.carratPos.valid)
		{
			change = (!this.hasSelection ? UITextSelection.Change.CarratToNone : UITextSelection.Change.SelectionToNone);
		}
		else if (this.hasSelection)
		{
			if (value.hasSelection)
			{
				change = (value.carratPos.position != this.carratPos.position || value.selectPos.position != this.selectPos.position ? UITextSelection.Change.SelectionAdjusted : UITextSelection.Change.None);
			}
			else
			{
				change = UITextSelection.Change.SelectionToCarrat;
			}
		}
		else if (!value.hasSelection)
		{
			change = (value.carratPos.position == this.carratPos.position ? UITextSelection.Change.None : UITextSelection.Change.CarratMove);
		}
		else
		{
			change = UITextSelection.Change.CarratToSelection;
		}
		return change;
	}

	public bool GetHighlight(out UIHighlight h)
	{
		h = new UIHighlight();
		if (this.selectPos.position < this.carratPos.position)
		{
			if (this.carratPos.valid && this.selectPos.valid)
			{
				h.a.i = this.selectPos.position;
				h.a.L = this.selectPos.line;
				h.a.C = this.selectPos.column;
				h.b.i = this.carratPos.position;
				h.b.L = this.carratPos.line;
				h.b.C = this.carratPos.column;
				return true;
			}
		}
		else if (this.selectPos.position > this.carratPos.position && this.carratPos.valid && this.selectPos.valid)
		{
			h.b.i = this.selectPos.position;
			h.b.L = this.selectPos.line;
			h.b.C = this.selectPos.column;
			h.a.i = this.carratPos.position;
			h.a.L = this.carratPos.line;
			h.a.C = this.carratPos.column;
			return true;
		}
		h = UIHighlight.invalid;
		return false;
	}

	public override string ToString()
	{
		return string.Format("[hasSelection={0}, showCarrat={1}, highlight=[{2}->{3}], carratPos={4}, selectPos={5}]", new object[] { this.hasSelection, this.showCarrat, this.highlightBegin, this.highlightEnd, this.carratPos.ToString(), this.selectPos.ToString() });
	}

	public enum Change : sbyte
	{
		None,
		NoneToCarrat,
		CarratMove,
		CarratToNone,
		CarratToSelection,
		SelectionAdjusted,
		SelectionToCarrat,
		NoneToSelection,
		SelectionToNone
	}
}