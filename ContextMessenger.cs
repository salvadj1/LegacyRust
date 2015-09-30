using Facepunch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ContextMessenger : Facepunch.MonoBehaviour, IContextRequestable, IContextRequestableMenu, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	public string[] messageOptions;

	public ContextMessenger()
	{
	}

	public ContextExecution ContextQuery(Controllable controllable, ulong timestamp)
	{
		return (this.messageOptions == null || (int)this.messageOptions.Length == 0 ? ContextExecution.NotAvailable : ContextExecution.Menu);
	}

	[DebuggerHidden]
	public IEnumerable<ContextActionPrototype> ContextQueryMenu(Controllable controllable, ulong timestamp)
	{
		ContextMessenger.<ContextQueryMenu>c__Iterator38 variable = null;
		return variable;
	}

	public ContextResponse ContextRespondMenu(Controllable controllable, ContextActionPrototype action, ulong timestamp)
	{
		base.SendMessage(((ContextMessenger.MessageAction)action).message, controllable);
		return ContextResponse.DoneBreak;
	}

	private class MessageAction : ContextActionPrototype
	{
		public string message;

		public MessageAction(int name, string text, string message)
		{
			this.name = name;
			this.text = text;
			this.message = message;
		}
	}
}