using System;

public struct UITextPosition
{
	public int line;

	public int column;

	public int position;

	public short deformed;

	public UITextRegion region;

	public UITextRegion uniformRegion;

	public int uniformPosition
	{
		get
		{
			return this.position + this.deformed;
		}
		set
		{
			this.deformed = (short)(value - this.position);
		}
	}

	public bool valid
	{
		get
		{
			return this.region != UITextRegion.Invalid;
		}
	}

	public UITextPosition(UITextRegion beforeOrPre)
	{
		this.line = 0;
		this.column = 0;
		this.position = 0;
		this.deformed = 0;
		this.region = beforeOrPre;
		this.uniformRegion = beforeOrPre;
	}

	public UITextPosition(int line, int column, int position, UITextRegion region)
	{
		this.line = line;
		this.column = column;
		this.position = position;
		this.deformed = 0;
		this.region = region;
		this.uniformRegion = region;
	}

	public override string ToString()
	{
		return string.Format("[{0} pos={1}{{{2}:{3}}} uniform={{{4}-{5}}}]", new object[] { this.region, this.position, this.line, this.column, this.uniformPosition, this.uniformRegion });
	}
}