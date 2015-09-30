using Google.ProtocolBuffers;
using System;

namespace RustProto.Helpers
{
	public sealed class Recycler<TMessage, TBuilder> : IDisposable
	where TMessage : GeneratedMessage<TMessage, TBuilder>
	where TBuilder : GeneratedBuilder<TMessage, TBuilder>, new()
	{
		private TBuilder Builder;

		private Recycler<TMessage, TBuilder> Next;

		private bool Disposed;

		private bool Cleared;

		private bool Created;

		private int OpenCount;

		private static Recycler<TMessage, TBuilder>.Holding Recovery;

		private Recycler()
		{
		}

		public void CloseBuilder(ref TBuilder builder)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Recycler");
			}
			if (this.OpenCount == 0)
			{
				throw new InvalidOperationException("Close was called more than Open for this Recycler");
			}
			if (!object.ReferenceEquals(builder, this.Builder))
			{
				throw new ArgumentOutOfRangeException("builder", "Was not opened by this recycler");
			}
			builder = (TBuilder)null;
			Recycler<TMessage, TBuilder> recycler = this;
			int openCount = recycler.OpenCount - 1;
			int num = openCount;
			recycler.OpenCount = openCount;
			if (num == 0 && !this.Cleared)
			{
				this.Builder.Clear();
				this.Cleared = true;
			}
		}

		public static Recycler<TMessage, TBuilder> Manufacture()
		{
			if (Recycler<TMessage, TBuilder>.Recovery.Count == 0)
			{
				return new Recycler<TMessage, TBuilder>();
			}
			Recycler<TMessage, TBuilder> pile = Recycler<TMessage, TBuilder>.Recovery.Pile;
			if (Recycler<TMessage, TBuilder>.Recovery.Count != 1)
			{
				Recycler<TMessage, TBuilder>.Recovery.Count = Recycler<TMessage, TBuilder>.Recovery.Count - 1;
				Recycler<TMessage, TBuilder>.Recovery.Pile = pile.Next;
				pile.Next = null;
			}
			else
			{
				Recycler<TMessage, TBuilder>.Recovery = new Recycler<TMessage, TBuilder>.Holding();
			}
			pile.Disposed = false;
			return pile;
		}

		public TBuilder OpenBuilder()
		{
			Recycler<TMessage, TBuilder> recycler = this;
			int openCount = recycler.OpenCount;
			int num = openCount;
			recycler.OpenCount = openCount + 1;
			if (num == 0)
			{
				if (this.Created)
				{
					this.Cleared = false;
				}
				else
				{
					this.Builder = Activator.CreateInstance<TBuilder>();
					this.Created = true;
				}
			}
			return this.Builder;
		}

		public TBuilder OpenBuilder(TMessage copyFrom)
		{
			TBuilder tBuilder = this.OpenBuilder();
			tBuilder.MergeFrom(copyFrom);
			return tBuilder;
		}

		void System.IDisposable.Dispose()
		{
			if (!this.Disposed)
			{
				this.Disposed = true;
				int count = Recycler<TMessage, TBuilder>.Recovery.Count;
				int num = count;
				Recycler<TMessage, TBuilder>.Recovery.Count = count + 1;
				if (num != 0)
				{
					this.Next = Recycler<TMessage, TBuilder>.Recovery.Pile;
					Recycler<TMessage, TBuilder>.Recovery.Pile = this;
				}
				else
				{
					this.Next = null;
					Recycler<TMessage, TBuilder>.Recovery.Pile = this;
				}
				this.OpenCount = 0;
				if (this.Created && !this.Cleared)
				{
					this.Builder.Clear();
					this.Cleared = true;
				}
			}
		}

		private struct Holding
		{
			public Recycler<TMessage, TBuilder> Pile;

			public int Count;
		}
	}
}