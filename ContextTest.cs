using Facepunch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ContextTest : Facepunch.MonoBehaviour, IContextRequestable, IContextRequestableMenu, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	public ContextTest()
	{
	}

	public ContextExecution ContextQuery(Controllable controllable, ulong timestamp)
	{
		return ContextExecution.Quick | ContextExecution.Menu;
	}

	[DebuggerHidden]
	public IEnumerable<ContextActionPrototype> ContextQueryMenu(Controllable controllable, ulong timestamp)
	{
		ContextTest.<ContextQueryMenu>c__Iterator37 variable = null;
		return variable;
	}

	public ContextResponse ContextRespondMenu(Controllable controllable, ContextActionPrototype action, ulong timestamp)
	{
		return ((ContextTest.ContextCallback)action).func(controllable);
	}

	private ContextResponse Option1(Controllable control)
	{
		UnityEngine.Debug.Log("Wee option 1");
		return ContextResponse.DoneBreak;
	}

	private ContextResponse Option2(Controllable control)
	{
		UnityEngine.Debug.Log("Wee option 2");
		return ContextResponse.DoneBreak;
	}

	private delegate ContextResponse CallbackFunction(Controllable controllable);

	private class ContextCallback : ContextActionPrototype
	{
		public ContextTest.CallbackFunction func;

		public ContextCallback(int name, string text, ContextTest.CallbackFunction function)
		{
			this.name = name;
			this.text = text;
			this.func = function;
		}
	}
}