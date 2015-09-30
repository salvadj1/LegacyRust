using System;

public struct UIHighlight
{
	public UIHighlight.Node a;

	public UIHighlight.Node b;

	public bool any
	{
		get
		{
			return this.a.i != this.b.i;
		}
	}

	public int characterCount
	{
		get
		{
			return this.b.i - this.a.i;
		}
	}

	public UIHighlight.Node delta
	{
		get
		{
			UIHighlight.Node l = new UIHighlight.Node();
			l.i = this.b.i - this.a.i;
			l.L = this.b.L - this.a.L;
			l.C = this.b.C - this.a.C;
			return l;
		}
	}

	public bool empty
	{
		get
		{
			return this.a.i == this.b.i;
		}
	}

	public static UIHighlight invalid
	{
		get
		{
			UIHighlight uIHighlight = new UIHighlight()
			{
				a = new UIHighlight.Node()
				{
					i = -1
				},
				b = new UIHighlight.Node()
				{
					i = -1
				}
			};
			return uIHighlight;
		}
	}

	public int lineCount
	{
		get
		{
			return (this.a.i == this.b.i ? 0 : this.b.L - this.a.L + 1);
		}
	}

	public int lineSpan
	{
		get
		{
			return this.b.L - this.a.L;
		}
	}

	public struct Node
	{
		public int i;

		public int L;

		public int C;

		public override string ToString()
		{
			return string.Format("[{0}({1}:{2})]", this.i, this.L, this.C);
		}
	}
}