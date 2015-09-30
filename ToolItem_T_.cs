using System;

public abstract class ToolItem<T> : InventoryItem<T>
where T : ToolDataBlock
{
	public virtual bool canWork
	{
		get
		{
			T t = this.datablock;
			return t.CanWork(this.iface as IToolItem, base.inventory);
		}
	}

	public virtual float workDuration
	{
		get
		{
			return this.datablock.GetWorkDuration(this.iface as IToolItem);
		}
	}

	protected ToolItem(T db) : base(db)
	{
	}

	public virtual void CancelWork()
	{
	}

	public virtual void CompleteWork()
	{
		T t = this.datablock;
		t.CompleteWork(this.iface as IToolItem, base.inventory);
	}

	public virtual void StartWork()
	{
	}
}