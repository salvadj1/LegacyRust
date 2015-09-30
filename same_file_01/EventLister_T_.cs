using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public struct EventLister<T>
{
	public readonly static Type InvokeCallType;

	public readonly static Type CalleeType;

	public readonly static MethodInfo CalleeMethod;

	private EventLister<T>.Node node;

	public bool empty
	{
		get
		{
			return object.ReferenceEquals(this.node, null);
		}
	}

	static EventLister()
	{
		if (!typeof(T).IsSubclassOf(typeof(Delegate)))
		{
			throw new InvalidOperationException("T is not a delegate");
		}
		EventListerInvokeAttribute customAttribute = (EventListerInvokeAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EventListerInvokeAttribute), false);
		EventLister<T>.InvokeCallType = customAttribute.InvokeCall;
		EventLister<T>.CalleeType = customAttribute.InvokeClass;
		MethodInfo method = EventLister<T>.InvokeCallType.GetMethod("Invoke");
		ParameterInfo[] parameters = method.GetParameters();
		Type[] parameterType = new Type[(int)parameters.Length];
		for (int i = 0; i < (int)parameters.Length; i++)
		{
			parameterType[i] = parameters[i].ParameterType;
		}
		EventLister<T>.CalleeMethod = EventLister<T>.CalleeType.GetMethod(customAttribute.InvokeMember, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, method.CallingConvention, parameterType, null);
		ParameterInfo[] parameterInfoArray = EventLister<T>.CalleeMethod.GetParameters();
		for (int j = 0; j < (int)parameterInfoArray.Length; j++)
		{
			if ((parameterInfoArray[j].Attributes & parameters[j].Attributes) != parameters[j].Attributes)
			{
				throw new InvalidOperationException(string.Concat("Parameter does not match the InvokeCall ", parameterInfoArray[j]));
			}
		}
	}

	public bool Add(T callback)
	{
		if (object.ReferenceEquals(this.node, null))
		{
			this.node = new EventLister<T>.Node(callback);
			return true;
		}
		if (!this.node.hashSet.Add(callback))
		{
			return false;
		}
		this.node.list.Add(callback);
		EventLister<T>.Node node = this.node;
		node.count = node.count + 1;
		return true;
	}

	public void Clear()
	{
		this.node = null;
	}

	public bool Invoke<C>(C caller)
	{
		if (object.ReferenceEquals(this.node, null))
		{
			return false;
		}
		if (this.node.invoking)
		{
			throw new InvalidOperationException("This lister is invoking already");
		}
		EventLister<T>.ExecCall<C> invoke = EventLister<T>.Invocation<C>.Invoke;
		try
		{
			this.node.invoking = true;
			this.node.iter = 0;
			while (this.node.iter < this.node.count)
			{
				List<T> ts = this.node.list;
				EventLister<T>.Node node = this.node;
				int num = node.iter;
				int num1 = num;
				node.iter = num + 1;
				T item = ts[num1];
				try
				{
					invoke(caller, item);
				}
				catch (Exception exception)
				{
					Debug.LogError(exception);
				}
			}
		}
		finally
		{
			if (this.node.count != 0)
			{
				this.node.invoking = false;
			}
			else
			{
				this.node = null;
			}
		}
		return true;
	}

	public bool Invoke<C, D>(C caller, ref D data)
	{
		if (object.ReferenceEquals(this.node, null))
		{
			return false;
		}
		if (this.node.invoking)
		{
			throw new InvalidOperationException("This lister is invoking already");
		}
		EventLister<T>.ExecCall<C, D> invoke = EventLister<T>.Invocation<C, D>.Invoke;
		try
		{
			this.node.invoking = true;
			this.node.iter = 0;
			while (this.node.iter < this.node.count)
			{
				List<T> ts = this.node.list;
				EventLister<T>.Node node = this.node;
				int num = node.iter;
				int num1 = num;
				node.iter = num + 1;
				T item = ts[num1];
				try
				{
					invoke(caller, ref data, item);
				}
				catch (Exception exception)
				{
					Debug.LogError(exception);
				}
			}
		}
		finally
		{
			if (this.node.count != 0)
			{
				this.node.invoking = false;
			}
			else
			{
				this.node = null;
			}
		}
		return true;
	}

	public void InvokeManual<C>(T callback, C caller)
	{
		try
		{
			EventLister<T>.Invocation<C>.Invoke(caller, callback);
		}
		catch (Exception exception)
		{
			Debug.LogError(exception);
		}
	}

	public void InvokeManual<C, D>(T callback, C caller, ref D data)
	{
		try
		{
			EventLister<T>.Invocation<C, D>.Invoke(caller, ref data, callback);
		}
		catch (Exception exception)
		{
			Debug.LogError(exception);
		}
	}

	public bool Remove(T callback)
	{
		if (object.ReferenceEquals(this.node, null) || !this.node.hashSet.Remove(callback))
		{
			return false;
		}
		EventLister<T>.Node node = this.node;
		int num = node.count - 1;
		int num1 = num;
		node.count = num;
		if (num1 != 0 || this.node.invoking)
		{
			int num2 = this.node.list.IndexOf(callback);
			this.node.list.RemoveAt(num2);
			if (this.node.iter > num2)
			{
				EventLister<T>.Node node1 = this.node;
				node1.iter = node1.iter - 1;
			}
		}
		else
		{
			this.node = null;
		}
		return true;
	}

	public delegate void ExecCall<C>(C caller, T callback);

	public delegate void ExecCall<C, D>(C caller, ref D data, T callback);

	internal static class Invocation<C>
	{
		public readonly static EventLister<T>.ExecCall<C> Invoke;

		static Invocation()
		{
			if (EventLister<T>.InvokeCallType != typeof(EventLister<>.ExecCall<T, C>))
			{
				throw new InvalidOperationException(string.Concat(EventLister<T>.InvokeCallType.Name, " should have been used."));
			}
			EventLister<T>.Invocation<C>.Invoke = (EventLister<T>.ExecCall<C>)Delegate.CreateDelegate(typeof(EventLister<>.ExecCall<T, C>), EventLister<T>.CalleeMethod);
		}
	}

	internal static class Invocation<C, D>
	{
		public readonly static EventLister<T>.ExecCall<C, D> Invoke;

		static Invocation()
		{
			if (EventLister<T>.InvokeCallType != typeof(EventLister<>.ExecCall<T, C, D>))
			{
				throw new InvalidOperationException(string.Concat(EventLister<T>.InvokeCallType.Name, " should have been used."));
			}
			EventLister<T>.Invocation<C, D>.Invoke = (EventLister<T>.ExecCall<C, D>)Delegate.CreateDelegate(typeof(EventLister<>.ExecCall<T, C, D>), EventLister<T>.CalleeMethod);
		}
	}

	private sealed class Node
	{
		internal readonly HashSet<T> hashSet;

		internal readonly List<T> list;

		internal int count;

		internal int iter;

		internal bool invoking;

		internal Node(T callback)
		{
			this.hashSet.Add(callback);
			this.list.Add(callback);
			this.count = 1;
		}
	}
}