using Facepunch;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facepunch.Load
{
	public static class Utility
	{
		public static string GetBuildInvariantTypeName(this Type type)
		{
			string fullName = type.Assembly.FullName;
			int num = fullName.IndexOf(',');
			if (num != -1)
			{
				fullName = fullName.Substring(0, num);
			}
			return string.Concat(type.FullName, ", ", fullName);
		}

		public sealed class ReferenceCountedCoroutine : IEnumerator
		{
			private readonly Facepunch.Load.Utility.ReferenceCountedCoroutine.Runner runner;

			private readonly Facepunch.Load.Utility.ReferenceCountedCoroutine.Callback callback;

			private object tag;

			private object yieldInstruction;

			private bool skipOnce;

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return this.yieldInstruction;
				}
			}

			private ReferenceCountedCoroutine(Facepunch.Load.Utility.ReferenceCountedCoroutine.Runner runner, Facepunch.Load.Utility.ReferenceCountedCoroutine.Callback callback, object yieldInstruction, object tag, bool skipOnce)
			{
				this.runner = runner;
				this.callback = callback;
				this.yieldInstruction = yieldInstruction;
				this.tag = tag;
				this.skipOnce = skipOnce;
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				bool flag;
				if (this.skipOnce)
				{
					this.skipOnce = false;
					return true;
				}
				try
				{
					flag = this.callback(ref this.yieldInstruction, ref this.tag);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					flag = false;
					Debug.LogException(exception);
				}
				if (flag)
				{
					return true;
				}
				this.runner.Release();
				this.tag = null;
				this.yieldInstruction = null;
				return false;
			}

			void System.Collections.IEnumerator.Reset()
			{
			}

			public delegate bool Callback(ref object yieldInstruction, ref object tag);

			public sealed class Runner
			{
				private readonly string gameObjectName;

				private GameObject go;

				private Facepunch.MonoBehaviour script;

				private int refCount;

				public Runner(string gameObjectName)
				{
					this.gameObjectName = gameObjectName;
				}

				public Coroutine Install(Facepunch.Load.Utility.ReferenceCountedCoroutine.Callback callback, object tag, object defaultYieldInstruction, bool skipFirst)
				{
					this.Retain();
					return this.script.StartCoroutine(new Facepunch.Load.Utility.ReferenceCountedCoroutine(this, callback, defaultYieldInstruction, tag, skipFirst));
				}

				public void Release()
				{
					Facepunch.Load.Utility.ReferenceCountedCoroutine.Runner runner = this;
					int num = runner.refCount - 1;
					int num1 = num;
					runner.refCount = num;
					if (num1 == 0)
					{
						UnityEngine.Object.Destroy(this.go);
						UnityEngine.Object.Destroy(this.script);
						this.go = null;
						this.script = null;
					}
				}

				public void Retain()
				{
					Facepunch.Load.Utility.ReferenceCountedCoroutine.Runner runner = this;
					int num = runner.refCount;
					int num1 = num;
					runner.refCount = num + 1;
					if (num1 == 0)
					{
						this.go = new GameObject(this.gameObjectName, new Type[] { typeof(Facepunch.MonoBehaviour) });
						UnityEngine.Object.DontDestroyOnLoad(this.go);
						this.script = this.go.GetComponent<Facepunch.MonoBehaviour>();
					}
				}
			}
		}
	}
}