using System;

namespace Facepunch.Collections
{
	public abstract class StaticQueue<KEY, T>
	where T : class
	{
		private static bool reg_made;

		private static int count;

		protected static int num
		{
			get
			{
				return StaticQueue<KEY, T>.count;
			}
		}

		protected StaticQueue()
		{
		}

		protected static bool contains(ref StaticQueue<KEY, T>.Entry state)
		{
			return state.inside;
		}

		protected static bool dequeue(T instance, ref StaticQueue<KEY, T>.Entry state)
		{
			if (!state.inside)
			{
				return false;
			}
			state.inside = false;
			return StaticQueue<KEY, T>.reg.dispose(ref state.node);
		}

		protected static void drain()
		{
			if (StaticQueue<KEY, T>.reg_made)
			{
				StaticQueue<KEY, T>.reg.drain();
			}
		}

		protected static bool enqueue(T instance, ref StaticQueue<KEY, T>.Entry state)
		{
			if (state.inside)
			{
				return false;
			}
			state.inside = true;
			state.node = StaticQueue<KEY, T>.reg.insert_end(StaticQueue<KEY, T>.reg.make_node(instance));
			return true;
		}

		protected static bool enrequeue(T instance, ref StaticQueue<KEY, T>.Entry state)
		{
			return (!state.inside ? StaticQueue<KEY, T>.enqueue(instance, ref state) : StaticQueue<KEY, T>.requeue(instance, ref state));
		}

		protected static bool requeue(T instance, ref StaticQueue<KEY, T>.Entry state)
		{
			if (!state.inside)
			{
				return false;
			}
			if (object.ReferenceEquals(StaticQueue<KEY, T>.reg.last, state.node))
			{
				return true;
			}
			state.node = StaticQueue<KEY, T>.reg.insert_end(state.node);
			return true;
		}

		protected static bool requeue(T instance, ref StaticQueue<KEY, T>.Entry state, bool enqueue_if_missing)
		{
			return (!enqueue_if_missing ? StaticQueue<KEY, T>.enqueue(instance, ref state) : StaticQueue<KEY, T>.enrequeue(instance, ref state));
		}

		protected static bool validate(T instance, ref StaticQueue<KEY, T>.Entry state, bool must_be_contained = false)
		{
			bool mustBeContained;
			if (!state.inside)
			{
				mustBeContained = !must_be_contained;
			}
			else
			{
				mustBeContained = (object.ReferenceEquals(state.node, null) ? false : object.ReferenceEquals(state.node.v, instance));
			}
			return mustBeContained;
		}

		protected enum act
		{
			none,
			front,
			back,
			delist
		}

		public struct Entry
		{
			internal bool inside;

			internal StaticQueue<KEY, T>.node node;
		}

		internal struct fork
		{
			public StaticQueue<KEY, T>.way p;

			public StaticQueue<KEY, T>.way n;
		}

		protected struct iterator
		{
			private int attempts;

			private int fail_left;

			private int position;

			private StaticQueue<KEY, T>.node node;

			private StaticQueue<KEY, T>.node next;

			public iterator(int maxIterations, int maxFailedIterations)
			{
				StaticQueue<KEY, T>.node _node;
				if (maxIterations == 0 || maxIterations > StaticQueue<KEY, T>.count)
				{
					this.attempts = StaticQueue<KEY, T>.count;
					this.fail_left = 0;
				}
				else if (maxIterations == StaticQueue<KEY, T>.count)
				{
					this.attempts = StaticQueue<KEY, T>.count;
					this.fail_left = 0;
				}
				else if (maxIterations + maxFailedIterations <= StaticQueue<KEY, T>.count)
				{
					this.attempts = maxIterations;
					this.fail_left = maxFailedIterations;
				}
				else
				{
					this.attempts = maxIterations;
					this.fail_left = StaticQueue<KEY, T>.count - maxIterations;
				}
				this.position = 0;
				this.node = null;
				if (!StaticQueue<KEY, T>.reg_made)
				{
					_node = null;
				}
				else
				{
					_node = StaticQueue<KEY, T>.reg.first;
				}
				this.next = _node;
			}

			public iterator(int maxIter) : this(maxIter, (maxIter >= StaticQueue<KEY, T>.count ? 0 : StaticQueue<KEY, T>.count - maxIter))
			{
			}

			public bool MissingNext(out T v)
			{
				StaticQueue<KEY, T>.iterator _iterator = this;
				int failLeft = _iterator.fail_left;
				int num = failLeft;
				_iterator.fail_left = failLeft - 1;
				if (num > 0)
				{
					StaticQueue<KEY, T>.iterator _iterator1 = this;
					_iterator1.position = _iterator1.position - 1;
				}
				StaticQueue<KEY, T>.reg.dispose(ref this.node);
				return this.Start(out v);
			}

			public bool Next(ref StaticQueue<KEY, T>.Entry prev_key, StaticQueue<KEY, T>.act cmd, out T v)
			{
				bool flag = object.ReferenceEquals(prev_key.node, null);
				if (!flag && !object.ReferenceEquals(prev_key.node, this.node))
				{
					throw new ArgumentException("prev_key did not match that of what was expected", "prev_key");
				}
				if (flag)
				{
					prev_key.inside = false;
				}
				if (prev_key.inside)
				{
					switch (cmd)
					{
						case (StaticQueue<KEY, T>.act)StaticQueue<KEY, T>.act.front:
						{
							if (StaticQueue<KEY, T>.reg.deref(this.node))
							{
								prev_key.node = StaticQueue<KEY, T>.reg.insert_begin(this.node);
							}
							break;
						}
						case (StaticQueue<KEY, T>.act)StaticQueue<KEY, T>.act.back:
						{
							if (StaticQueue<KEY, T>.reg.deref(this.node))
							{
								prev_key.node = StaticQueue<KEY, T>.reg.insert_end(this.node);
							}
							break;
						}
						case (StaticQueue<KEY, T>.act)StaticQueue<KEY, T>.act.delist:
						{
							StaticQueue<KEY, T>.reg.dispose(ref this.node);
							prev_key.node = null;
							break;
						}
					}
				}
				else
				{
					cmd = StaticQueue<KEY, T>.act.delist;
					StaticQueue<KEY, T>.iterator _iterator = this;
					int failLeft = _iterator.fail_left;
					int num = failLeft;
					_iterator.fail_left = failLeft - 1;
					if (num > 0)
					{
						StaticQueue<KEY, T>.iterator _iterator1 = this;
						_iterator1.position = _iterator1.position - 1;
					}
					if (!flag)
					{
						StaticQueue<KEY, T>.reg.dispose(ref prev_key.node);
					}
					else
					{
						StaticQueue<KEY, T>.reg.dispose(ref this.node);
					}
				}
				return this.Start(out v);
			}

			public bool Start(out T v)
			{
				StaticQueue<KEY, T>.iterator _iterator = this;
				int num = _iterator.position;
				int num1 = num;
				_iterator.position = num + 1;
				if (num1 >= this.attempts)
				{
					object obj = null;
					StaticQueue<KEY, T>.node _node = (StaticQueue<KEY, T>.node)obj;
					this.next = (StaticQueue<KEY, T>.node)obj;
					this.node = _node;
					v = (T)null;
					return false;
				}
				this.node = this.next;
				this.next = this.node.w.n.v;
				v = this.node.v;
				return true;
			}

			public bool Validate(ref StaticQueue<KEY, T>.Entry key)
			{
				return key.inside;
			}
		}

		internal class node
		{
			public T v;

			public bool e;

			public StaticQueue<KEY, T>.fork w;

			public node()
			{
			}
		}

		private static class reg
		{
			private static int dump_size;

			private static StaticQueue<KEY, T>.node dump;

			internal static StaticQueue<KEY, T>.node first;

			internal static StaticQueue<KEY, T>.node last;

			static reg()
			{
				StaticQueue<KEY, T>.reg_made = true;
			}

			internal static void delete(StaticQueue<KEY, T>.node r)
			{
				r.v = (T)null;
				StaticQueue<KEY, T>.way _way = new StaticQueue<KEY, T>.way();
				r.w.n = _way;
				r.e = false;
				int dumpSize = StaticQueue<KEY, T>.reg.dump_size;
				StaticQueue<KEY, T>.reg.dump_size = dumpSize + 1;
				r.w.p.e = dumpSize > 0;
				r.w.p.v = StaticQueue<KEY, T>.reg.dump;
				StaticQueue<KEY, T>.reg.dump = r;
			}

			internal static bool deref(StaticQueue<KEY, T>.node node)
			{
				if (object.ReferenceEquals(node, null))
				{
					return false;
				}
				if (!node.e)
				{
					return false;
				}
				int num = StaticQueue<KEY, T>.count - 1;
				StaticQueue<KEY, T>.count = num;
				if (num != 0)
				{
					if (node.w.p.e)
					{
						node.w.p.v.w.n = node.w.n;
					}
					else if (node.w.n.e)
					{
						StaticQueue<KEY, T>.reg.first = node.w.n.v;
					}
					if (node.w.n.e)
					{
						node.w.n.v.w.p = node.w.p;
					}
					else if (node.w.p.e)
					{
						StaticQueue<KEY, T>.reg.last = node.w.p.v;
					}
					node.w = new StaticQueue<KEY, T>.fork();
				}
				else
				{
					object obj = null;
					StaticQueue<KEY, T>.reg.last = (StaticQueue<KEY, T>.node)obj;
					StaticQueue<KEY, T>.reg.first = (StaticQueue<KEY, T>.node)obj;
				}
				node.e = false;
				return true;
			}

			internal static bool dispose(ref StaticQueue<KEY, T>.node node)
			{
				if (!StaticQueue<KEY, T>.reg.deref(node))
				{
					return false;
				}
				node.v = (T)null;
				StaticQueue<KEY, T>.reg.delete(node);
				node = null;
				return true;
			}

			public static void drain()
			{
				StaticQueue<KEY, T>.reg.dump = null;
				StaticQueue<KEY, T>.reg.dump_size = 0;
			}

			internal static StaticQueue<KEY, T>.node insert_begin(StaticQueue<KEY, T>.node node)
			{
				if (node.e)
				{
					StaticQueue<KEY, T>.reg.deref(node);
				}
				int num = StaticQueue<KEY, T>.count;
				StaticQueue<KEY, T>.count = num + 1;
				if (num != 0)
				{
					node.w.n.e = true;
					node.w.n.v = StaticQueue<KEY, T>.reg.first;
					StaticQueue<KEY, T>.reg.first.w.p.e = true;
					StaticQueue<KEY, T>.reg.first.w.p.v = node;
					StaticQueue<KEY, T>.reg.first = node;
				}
				else
				{
					StaticQueue<KEY, T>.node _node = node;
					StaticQueue<KEY, T>.reg.first = _node;
					StaticQueue<KEY, T>.reg.last = _node;
				}
				node.e = true;
				return StaticQueue<KEY, T>.reg.first;
			}

			internal static StaticQueue<KEY, T>.node insert_end(StaticQueue<KEY, T>.node node)
			{
				if (node.e)
				{
					StaticQueue<KEY, T>.reg.deref(node);
				}
				int num = StaticQueue<KEY, T>.count;
				StaticQueue<KEY, T>.count = num + 1;
				if (num != 0)
				{
					node.w.p.e = true;
					node.w.p.v = StaticQueue<KEY, T>.reg.last;
					StaticQueue<KEY, T>.reg.last.w.n.e = true;
					StaticQueue<KEY, T>.reg.last.w.n.v = node;
					StaticQueue<KEY, T>.reg.last = node;
				}
				else
				{
					StaticQueue<KEY, T>.node _node = node;
					StaticQueue<KEY, T>.reg.first = _node;
					StaticQueue<KEY, T>.reg.last = _node;
				}
				node.e = true;
				return StaticQueue<KEY, T>.reg.last;
			}

			internal static StaticQueue<KEY, T>.node make_node(T v)
			{
				StaticQueue<KEY, T>.node _node;
				if (StaticQueue<KEY, T>.reg.dump_size <= 0)
				{
					_node = new StaticQueue<KEY, T>.node();
				}
				else
				{
					StaticQueue<KEY, T>.reg.dump_size = StaticQueue<KEY, T>.reg.dump_size - 1;
					_node = StaticQueue<KEY, T>.reg.dump;
					StaticQueue<KEY, T>.reg.dump = _node.w.p.v;
					_node.w = new StaticQueue<KEY, T>.fork();
				}
				_node.v = v;
				_node.e = false;
				return _node;
			}
		}

		internal struct way
		{
			public StaticQueue<KEY, T>.node v;

			public bool e;
		}
	}
}