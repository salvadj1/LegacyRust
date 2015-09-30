using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public sealed class InterpTimedEvent : IDisposable
{
	internal static InterpTimedEvent current;

	public UnityEngine.MonoBehaviour component;

	public uLink.NetworkMessageInfo info;

	public InterpTimedEvent.ArgList args;

	public string tag;

	private bool disposed;

	private bool inlist;

	private static int dumpCount;

	private static InterpTimedEvent.Dir dump;

	private static InterpTimedEvent.LList queue;

	internal InterpTimedEvent.Dir next;

	internal InterpTimedEvent.Dir prev;

	private static bool _forceCatchupToDate;

	private readonly static object[] emptyArgs;

	public static object[] ArgumentArray
	{
		get
		{
			return InterpTimedEvent.current.args.parameters;
		}
	}

	public static int ArgumentCount
	{
		get
		{
			return InterpTimedEvent.current.args.length;
		}
	}

	public static Type[] ArgumentTypeArray
	{
		get
		{
			return InterpTimedEvent.current.args.types;
		}
	}

	public static NetworkFlags Flags
	{
		get
		{
			return InterpTimedEvent.current.info.flags;
		}
	}

	public static uLink.NetworkMessageInfo Info
	{
		get
		{
			return InterpTimedEvent.current.info;
		}
	}

	public static uLink.NetworkView NetworkView
	{
		get
		{
			return InterpTimedEvent.current.info.networkView;
		}
	}

	public IInterpTimedEventReceiver receiver
	{
		get
		{
			return this.component as IInterpTimedEventReceiver;
		}
	}

	public static uLink.NetworkPlayer Sender
	{
		get
		{
			return InterpTimedEvent.current.info.sender;
		}
	}

	public static bool syncronizationPaused
	{
		get
		{
			return InterpTimedEventSyncronizer.paused;
		}
		set
		{
			InterpTimedEventSyncronizer.paused = value;
		}
	}

	public static string Tag
	{
		get
		{
			return InterpTimedEvent.current.tag;
		}
	}

	public static UnityEngine.MonoBehaviour Target
	{
		get
		{
			return InterpTimedEvent.current.component;
		}
	}

	public static double Timestamp
	{
		get
		{
			return InterpTimedEvent.current.info.timestamp;
		}
	}

	public static ulong TimestampInMilliseconds
	{
		get
		{
			return InterpTimedEvent.current.info.timestampInMillis;
		}
	}

	static InterpTimedEvent()
	{
		InterpTimedEvent.emptyArgs = new object[0];
	}

	private InterpTimedEvent()
	{
	}

	public static object Argument(int index)
	{
		return InterpTimedEvent.current.args.parameters[index];
	}

	public static T Argument<T>(int index)
	{
		Type type = InterpTimedEvent.current.args.types[index];
		if (!typeof(T).IsAssignableFrom(type) && (typeof(void) != type || typeof(T).IsValueType))
		{
			throw new InvalidCastException(string.Format("Argument #{0} was a {1} and {2} is not assignable by {1}", index, InterpTimedEvent.current.args.types[index], typeof(T)));
		}
		return (T)InterpTimedEvent.current.args.parameters[index];
	}

	public static bool ArgumentIs<T>(int index)
	{
		bool flag;
		Type type = InterpTimedEvent.current.args.types[index];
		if (typeof(T).IsAssignableFrom(type))
		{
			flag = true;
		}
		else
		{
			flag = (type != typeof(void) ? false : !typeof(T).IsValueType);
		}
		return flag;
	}

	public static bool ArgumentIs(int index, Type comptype)
	{
		bool flag;
		Type type = InterpTimedEvent.current.args.types[index];
		if (comptype.IsAssignableFrom(InterpTimedEvent.current.args.types[index]))
		{
			flag = true;
		}
		else
		{
			flag = (type != typeof(void) ? false : !comptype.IsValueType);
		}
		return flag;
	}

	public static Type ArgumentType(int index)
	{
		return InterpTimedEvent.current.args.types[index];
	}

	public static void Catchup()
	{
		InterpTimedEvent.Catchup(Interpolation.timeInMillis);
	}

	public static void Catchup(ulong playhead)
	{
		InterpTimedEvent._forceCatchupToDate = false;
		while (InterpTimedEvent.queue.Dequeue(playhead, out InterpTimedEvent.current))
		{
			InterpTimedEvent.Invoke();
		}
	}

	public static void Clear()
	{
		InterpTimedEvent.Clear(false);
	}

	public static void Clear(bool invokePending)
	{
		InterpTimedEvent interpTimedEvent;
		InterpTimedEvent.LList.Iterator iterator = new InterpTimedEvent.LList.Iterator();
		if (!invokePending)
		{
			while (InterpTimedEvent.queue.Dequeue((ulong)-1, out interpTimedEvent, ref iterator))
			{
				interpTimedEvent.Dispose();
			}
		}
		else
		{
			while (InterpTimedEvent.queue.Dequeue((ulong)-1, out interpTimedEvent, ref iterator))
			{
				InterpTimedEvent.InvokeDirect(interpTimedEvent);
			}
		}
	}

	public void Dispose()
	{
		if (this.inlist)
		{
			InterpTimedEvent.queue.Remove(this);
		}
		if (!this.disposed)
		{
			this.prev = new InterpTimedEvent.Dir();
			this.next = InterpTimedEvent.dump;
			InterpTimedEvent.dump.has = true;
			InterpTimedEvent.dump.node = this;
			this.component = null;
			this.args.Dispose();
			this.args = null;
			this.info = null;
			this.tag = null;
			InterpTimedEvent.dumpCount = InterpTimedEvent.dumpCount + 1;
			this.disposed = true;
		}
	}

	public static void EMERGENCY_DUMP(bool TRY_TO_EXECUTE)
	{
		Debug.LogWarning(string.Concat("RUNNING EMERGENCY DUMP: TRY TO EXECUTE=", TRY_TO_EXECUTE));
		try
		{
			try
			{
				if (!TRY_TO_EXECUTE)
				{
					InterpTimedEvent.queue.EmergencyDump(false);
				}
				else
				{
					try
					{
						foreach (InterpTimedEvent interpTimedEvent in InterpTimedEvent.queue.EmergencyDump(true))
						{
							try
							{
								try
								{
									InterpTimedEvent.InvokeDirect(interpTimedEvent);
								}
								catch (Exception exception)
								{
									Debug.LogException(exception);
								}
							}
							finally
							{
								try
								{
									interpTimedEvent.Dispose();
								}
								catch (Exception exception1)
								{
									Debug.LogException(exception1);
								}
							}
						}
					}
					catch (Exception exception2)
					{
						Debug.LogException(exception2);
					}
				}
			}
			catch (Exception exception3)
			{
				Debug.LogException(exception3);
			}
		}
		finally
		{
			InterpTimedEvent.queue = new InterpTimedEvent.LList();
			InterpTimedEvent.dump = new InterpTimedEvent.Dir();
			InterpTimedEvent.dumpCount = 0;
		}
		Debug.LogWarning(string.Concat("END OF EMERGENCY DUMP: TRY TO EXECUTE=", TRY_TO_EXECUTE));
	}

	public static void EmergencyDump()
	{
	}

	public static bool Execute(IInterpTimedEventReceiver receiver, string tag, ref uLink.NetworkMessageInfo info)
	{
		return InterpTimedEvent.QueueOrExecute(receiver, true, tag, ref info, InterpTimedEvent.emptyArgs);
	}

	public static bool Execute(IInterpTimedEventReceiver receiver, string tag, ref uLink.NetworkMessageInfo info, params object[] args)
	{
		return InterpTimedEvent.QueueOrExecute(receiver, true, tag, ref info, args);
	}

	public static void ForceCatchupToDate()
	{
		InterpTimedEvent._forceCatchupToDate = true;
	}

	private static void Invoke()
	{
		UnityEngine.MonoBehaviour monoBehaviour = InterpTimedEvent.current.component;
		if (!monoBehaviour)
		{
			Debug.LogWarning(string.Concat("A component implementing IInterpTimeEventReceiver was destroyed without properly calling InterpEvent.Remove() in OnDestroy!\r\n", (!string.IsNullOrEmpty(InterpTimedEvent.current.tag) ? string.Concat("The tag was \"", InterpTimedEvent.current.tag, "\"") : "There was no tag set")));
		}
		else
		{
			IInterpTimedEventReceiver interpTimedEventReceiver = monoBehaviour as IInterpTimedEventReceiver;
			try
			{
				interpTimedEventReceiver.OnInterpTimedEvent();
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception thrown during catchup \r\n", exception), monoBehaviour);
			}
		}
		InterpTimedEvent.current.Dispose();
	}

	private static void InvokeDirect(InterpTimedEvent evnt)
	{
		InterpTimedEvent interpTimedEvent = InterpTimedEvent.current;
		InterpTimedEvent.current = evnt;
		InterpTimedEvent.Invoke();
		InterpTimedEvent.current = interpTimedEvent;
	}

	public static void MarkUnhandled()
	{
		Debug.LogWarning(string.Concat("Unhandled Timed Event :", (!string.IsNullOrEmpty(InterpTimedEvent.current.tag) ? InterpTimedEvent.current.tag : " without a tag")), InterpTimedEvent.current.component);
	}

	internal static InterpTimedEvent New(UnityEngine.MonoBehaviour receiver, string tag, ref uLink.NetworkMessageInfo info, object[] args, bool immediate)
	{
		InterpTimedEvent interpTimedEvent;
		if (!receiver)
		{
			Debug.LogError("receiver is null or has been destroyed", receiver);
			return null;
		}
		if (!(receiver is IInterpTimedEventReceiver))
		{
			Debug.LogError(string.Concat("receiver of type ", receiver.GetType(), " does not implement IInterpTimedEventReceiver"), receiver);
			return null;
		}
		if (!InterpTimedEvent.dump.has)
		{
			interpTimedEvent = new InterpTimedEvent();
		}
		else
		{
			InterpTimedEvent.dumpCount = InterpTimedEvent.dumpCount - 1;
			interpTimedEvent = InterpTimedEvent.dump.node;
			InterpTimedEvent.dump = interpTimedEvent.next;
			interpTimedEvent.next = new InterpTimedEvent.Dir();
			interpTimedEvent.prev = new InterpTimedEvent.Dir();
			interpTimedEvent.disposed = false;
		}
		interpTimedEvent.args = InterpTimedEvent.ArgList.New(args);
		interpTimedEvent.tag = tag;
		interpTimedEvent.component = receiver;
		interpTimedEvent.info = info;
		if (!immediate)
		{
			InterpTimedEvent.queue.Insert(interpTimedEvent);
		}
		return interpTimedEvent;
	}

	public static bool Queue(IInterpTimedEventReceiver receiver, string tag, ref uLink.NetworkMessageInfo info)
	{
		return InterpTimedEvent.QueueOrExecute(receiver, false, tag, ref info, InterpTimedEvent.emptyArgs);
	}

	public static bool Queue(IInterpTimedEventReceiver receiver, string tag, ref uLink.NetworkMessageInfo info, params object[] args)
	{
		return InterpTimedEvent.QueueOrExecute(receiver, false, tag, ref info, args);
	}

	public static bool QueueOrExecute(IInterpTimedEventReceiver receiver, bool immediate, string tag, ref uLink.NetworkMessageInfo info)
	{
		return InterpTimedEvent.QueueOrExecute(receiver, immediate, tag, ref info, InterpTimedEvent.emptyArgs);
	}

	public static bool QueueOrExecute(IInterpTimedEventReceiver receiver, bool immediate, string tag, ref uLink.NetworkMessageInfo info, params object[] args)
	{
		UnityEngine.MonoBehaviour monoBehaviour = receiver as UnityEngine.MonoBehaviour;
		InterpTimedEvent interpTimedEvent = InterpTimedEvent.New(monoBehaviour, tag, ref info, args, immediate);
		if (interpTimedEvent == null)
		{
			return false;
		}
		if (immediate)
		{
			InterpTimedEvent.InvokeDirect(interpTimedEvent);
		}
		else if (!InterpTimedEventSyncronizer.available)
		{
			Debug.LogWarning(string.Concat("Not running event because theres no syncronizer available. ", tag), receiver as UnityEngine.Object);
			return false;
		}
		return true;
	}

	public static void Remove(UnityEngine.MonoBehaviour receiver)
	{
		InterpTimedEvent.Remove(receiver, false);
	}

	public static void Remove(UnityEngine.MonoBehaviour receiver, bool invokePending)
	{
		InterpTimedEvent interpTimedEvent;
		InterpTimedEvent.LList.Iterator iterator = new InterpTimedEvent.LList.Iterator();
		if (!invokePending)
		{
			while (InterpTimedEvent.queue.Dequeue(receiver, (ulong)-1, out interpTimedEvent, ref iterator))
			{
				interpTimedEvent.Dispose();
			}
		}
		else
		{
			while (InterpTimedEvent.queue.Dequeue(receiver, (ulong)-1, out interpTimedEvent, ref iterator))
			{
				InterpTimedEvent.InvokeDirect(interpTimedEvent);
			}
		}
	}

	public sealed class ArgList : IDisposable
	{
		public readonly object[] parameters;

		public readonly Type[] types;

		public readonly int length;

		private bool disposed;

		private static InterpTimedEvent.ArgList voidParameters;

		private static InterpTimedEvent.ArgList.Dump[] dumps;

		private InterpTimedEvent.ArgList dumpNext;

		static ArgList()
		{
			InterpTimedEvent.ArgList.voidParameters = new InterpTimedEvent.ArgList(0);
			InterpTimedEvent.ArgList.dumps = new InterpTimedEvent.ArgList.Dump[4];
		}

		private ArgList(int length)
		{
			this.length = length;
			this.parameters = new object[length];
			this.types = new Type[length];
		}

		private void AddToDump(ref InterpTimedEvent.ArgList.Dump dump)
		{
			this.dumpNext = dump.last;
			dump.count = dump.count + 1;
			dump.last = this;
		}

		public void Dispose()
		{
			if (!this.disposed && this.length != 0)
			{
				for (int i = 0; i < this.length; i++)
				{
					this.types[i] = null;
					this.parameters[i] = null;
				}
				if ((int)InterpTimedEvent.ArgList.dumps.Length <= this.length)
				{
					Array.Resize<InterpTimedEvent.ArgList.Dump>(ref InterpTimedEvent.ArgList.dumps, this.length + 1);
				}
				this.AddToDump(ref InterpTimedEvent.ArgList.dumps[this.length]);
				this.disposed = true;
			}
		}

		public static InterpTimedEvent.ArgList New(object[] args)
		{
			InterpTimedEvent.ArgList argList;
			int num = (args != null ? (int)args.Length : 0);
			if (num == 0)
			{
				return InterpTimedEvent.ArgList.voidParameters;
			}
			argList = ((int)InterpTimedEvent.ArgList.dumps.Length <= num ? new InterpTimedEvent.ArgList(num) : InterpTimedEvent.ArgList.Recycle(ref InterpTimedEvent.ArgList.dumps[num], num));
			for (int i = 0; i < num; i++)
			{
				object obj = args[i];
				argList.parameters[i] = obj;
				argList.types[i] = (obj != null ? obj.GetType() : typeof(void));
			}
			return argList;
		}

		private static InterpTimedEvent.ArgList Recycle(ref InterpTimedEvent.ArgList.Dump dump, int length)
		{
			if (dump.count <= 0)
			{
				return new InterpTimedEvent.ArgList(length);
			}
			InterpTimedEvent.ArgList argList = dump.last;
			dump.last = argList.dumpNext;
			dump.count = dump.count - 1;
			argList.dumpNext = null;
			argList.disposed = false;
			return argList;
		}

		private struct Dump
		{
			public InterpTimedEvent.ArgList last;

			public int count;
		}
	}

	internal struct Dir
	{
		public bool has;

		public InterpTimedEvent node;
	}

	internal struct LList
	{
		public InterpTimedEvent.Dir first;

		public InterpTimedEvent.Dir last;

		public int count;

		private HashSet<InterpTimedEvent> FAIL_SAFE_SET;

		public bool Dequeue(ulong playhead, out InterpTimedEvent node)
		{
			InterpTimedEvent.LList.Iterator iterator = new InterpTimedEvent.LList.Iterator();
			return this.Dequeue(playhead, out node, ref iterator);
		}

		public bool Dequeue(ulong playhead, out InterpTimedEvent node, ref InterpTimedEvent.LList.Iterator iter_)
		{
			if (this.count <= 0)
			{
				node = null;
				return false;
			}
			InterpTimedEvent.Dir dir = (!iter_.started ? this.first : iter_.d);
			if (dir.has)
			{
				if ((double)((float)playhead) >= dir.node.info.timestamp)
				{
					node = dir.node;
					iter_.d = node.next;
					iter_.started = true;
					this.Remove(node);
					return true;
				}
			}
			iter_.d = new InterpTimedEvent.Dir();
			iter_.started = true;
			node = null;
			return false;
		}

		public bool Dequeue(UnityEngine.MonoBehaviour script, ulong playhead, out InterpTimedEvent node)
		{
			InterpTimedEvent.LList.Iterator iterator = new InterpTimedEvent.LList.Iterator();
			return this.Dequeue(script, playhead, out node, ref iterator);
		}

		public bool Dequeue(UnityEngine.MonoBehaviour script, ulong playhead, out InterpTimedEvent node, ref InterpTimedEvent.LList.Iterator iter_)
		{
			if (this.count <= 0)
			{
				node = null;
				return false;
			}
			InterpTimedEvent.Dir dir = (!iter_.started ? this.first : iter_.d);
			while (dir.has)
			{
				if ((double)((float)playhead) >= dir.node.info.timestamp)
				{
					if (dir.node.component == script)
					{
						node = dir.node;
						iter_.d = node.next;
						iter_.started = true;
						this.Remove(node);
						return true;
					}
					dir = dir.node.next;
				}
				else
				{
					break;
				}
			}
			iter_.d = new InterpTimedEvent.Dir();
			iter_.started = true;
			node = null;
			return false;
		}

		public List<InterpTimedEvent> EmergencyDump(bool botherSorting)
		{
			InterpTimedEvent interpTimedEvent;
			bool flag;
			HashSet<InterpTimedEvent> interpTimedEvents = new HashSet<InterpTimedEvent>();
			InterpTimedEvent.LList.Iterator iterator = new InterpTimedEvent.LList.Iterator();
			do
			{
				try
				{
					flag = this.Dequeue((ulong)-1, out interpTimedEvent, ref iterator);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					break;
				}
				if (!flag)
				{
					continue;
				}
				interpTimedEvents.Add(interpTimedEvent);
			}
			while (flag);
			InterpTimedEvent.Dir dir = new InterpTimedEvent.Dir();
			InterpTimedEvent.Dir dir1 = dir;
			this.last = dir;
			this.first = dir1;
			this.count = 0;
			HashSet<InterpTimedEvent> fAILSAFESET = this.FAIL_SAFE_SET;
			this.FAIL_SAFE_SET = null;
			if (fAILSAFESET != null)
			{
				interpTimedEvents.UnionWith(fAILSAFESET);
			}
			List<InterpTimedEvent> interpTimedEvents1 = new List<InterpTimedEvent>(interpTimedEvents);
			if (botherSorting)
			{
				try
				{
					interpTimedEvents1.Sort((InterpTimedEvent x, InterpTimedEvent y) => {
						if (x == null)
						{
							if (y == null)
							{
								return 0;
							}
							return 0.CompareTo(1);
						}
						if (y == null)
						{
							return 1.CompareTo(0);
						}
						return x.info.timestampInMillis.CompareTo(y.info.timestampInMillis);
					});
				}
				catch (Exception exception1)
				{
					Debug.LogException(exception1);
				}
			}
			return interpTimedEvents1;
		}

		private bool Insert(ref InterpTimedEvent.Dir ent)
		{
			InterpTimedEvent.Dir dir;
			if (ent.node == null)
			{
				return false;
			}
			if (ent.node.inlist)
			{
				return false;
			}
			if (this.count == 0)
			{
				InterpTimedEvent.Dir dir1 = ent;
				InterpTimedEvent.Dir dir2 = dir1;
				this.last = dir1;
				this.first = dir2;
			}
			else if (this.last.node.info.timestampInMillis <= ent.node.info.timestampInMillis)
			{
				if (this.count != 1)
				{
					ent.node.prev = this.last;
					this.last.node.next = ent;
					this.last = ent;
				}
				else
				{
					this.first = this.last;
					this.last = ent;
					ent.node.prev = this.first;
					this.first.node.next = this.last;
				}
			}
			else if (this.count == 1)
			{
				this.first = ent;
				this.first.node.next = this.last;
				this.last.node.prev = this.first;
			}
			else if (this.first.node.info.timestampInMillis <= ent.node.info.timestampInMillis)
			{
				if (this.first.node.info.timestampInMillis != ent.node.info.timestampInMillis)
				{
					dir = this.last;
					do
					{
						if (!dir.node.prev.has)
						{
							goto Label0;
						}
						dir = dir.node.prev;
					}
					while (dir.node.info.timestampInMillis > ent.node.info.timestampInMillis);
				}
				else
				{
					dir = this.first;
					while (dir.node.next.has)
					{
						if (dir.node.next.node.info.timestampInMillis <= ent.node.info.timestampInMillis)
						{
							dir = dir.node.next;
						}
						else
						{
							break;
						}
					}
				}
			Label0:
				ent.node.next = dir.node.next;
				ent.node.prev = dir;
			}
			else
			{
				ent.node.next = this.first;
				this.first.node.prev = ent;
				this.first = ent;
			}
			InterpTimedEvent.LList lList = this;
			lList.count = lList.count + 1;
			ent.node.inlist = true;
			if (this.FAIL_SAFE_SET == null)
			{
				this.FAIL_SAFE_SET = new HashSet<InterpTimedEvent>();
			}
			this.FAIL_SAFE_SET.Add(ent.node);
			return true;
		}

		public bool Insert(InterpTimedEvent node)
		{
			InterpTimedEvent.Dir dir = new InterpTimedEvent.Dir();
			dir.node = node;
			dir.has = true;
			return this.Insert(ref dir);
		}

		internal bool Remove(InterpTimedEvent node)
		{
			if (!this.RemoveUnsafe(node))
			{
				return false;
			}
			if (this.FAIL_SAFE_SET != null)
			{
				this.FAIL_SAFE_SET.Remove(node);
			}
			return true;
		}

		private bool RemoveUnsafe(InterpTimedEvent node)
		{
			InterpTimedEvent.Dir dir;
			InterpTimedEvent.Dir dir1;
			InterpTimedEvent.Dir dir2;
			if (this.count > 0 && node != null && node.inlist)
			{
				if (node.prev.has)
				{
					if (!node.next.has)
					{
						this.last = node.prev;
						InterpTimedEvent interpTimedEvent = this.last.node;
						dir = new InterpTimedEvent.Dir();
						interpTimedEvent.next = dir;
						InterpTimedEvent.LList lList = this;
						lList.count = lList.count - 1;
						InterpTimedEvent.Dir dir3 = new InterpTimedEvent.Dir();
						dir1 = dir3;
						node.next = dir3;
						node.prev = dir1;
						node.inlist = false;
						return true;
					}
					node.next.node.prev = node.prev;
					node.prev.node.next = node.next;
					InterpTimedEvent.LList lList1 = this;
					lList1.count = lList1.count - 1;
					InterpTimedEvent.Dir dir4 = new InterpTimedEvent.Dir();
					dir = dir4;
					node.next = dir4;
					node.prev = dir;
					node.inlist = false;
					return true;
				}
				if (node.next.has)
				{
					this.first = node.next;
					InterpTimedEvent interpTimedEvent1 = this.first.node;
					dir1 = new InterpTimedEvent.Dir();
					interpTimedEvent1.prev = dir1;
					InterpTimedEvent.LList lList2 = this;
					lList2.count = lList2.count - 1;
					InterpTimedEvent.Dir dir5 = new InterpTimedEvent.Dir();
					dir2 = dir5;
					node.next = dir5;
					node.prev = dir2;
					node.inlist = false;
					return true;
				}
				if (this.first.node == node)
				{
					dir2 = new InterpTimedEvent.Dir();
					InterpTimedEvent.Dir dir6 = dir2;
					InterpTimedEvent.Dir dir7 = dir6;
					this.last = dir6;
					this.first = dir7;
					this.count = 0;
					dir7 = new InterpTimedEvent.Dir();
					InterpTimedEvent.Dir dir8 = dir7;
					InterpTimedEvent.Dir dir9 = dir8;
					node.next = dir8;
					node.prev = dir9;
					node.inlist = false;
					return true;
				}
			}
			return false;
		}

		public struct Iterator
		{
			internal InterpTimedEvent.Dir d;

			internal bool started;
		}
	}
}