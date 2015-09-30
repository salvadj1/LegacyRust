using System;

public abstract class BlueprintItem<T> : ToolItem<T>
where T : BlueprintDataBlock
{
	public override float workDuration
	{
		get
		{
			return this.datablock.GetWorkDuration(this.iface as IToolItem);
		}
	}

	protected BlueprintItem(T db) : base(db)
	{
	}
}