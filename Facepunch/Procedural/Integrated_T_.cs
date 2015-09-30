using System;

namespace Facepunch.Procedural
{
	public struct Integrated<T>
	where T : struct
	{
		[NonSerialized]
		public MillisClock clock;

		[NonSerialized]
		public T begin;

		[NonSerialized]
		public T end;

		[NonSerialized]
		public T current;

		public void SetImmediate(ref T value)
		{
			T t = value;
			T t1 = t;
			this.current = t;
			T t2 = t1;
			t1 = t2;
			this.end = t2;
			this.begin = t1;
			this.clock.SetImmediate();
		}
	}
}