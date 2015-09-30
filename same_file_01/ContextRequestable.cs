using System;
using UnityEngine;

public static class ContextRequestable
{
	public const ContextExecution AllExecutionFlags = ContextExecution.Quick | ContextExecution.Menu;

	public static bool UseableForwardFromContext(IContextRequestable requestable, Controllable controllable, Useable useable)
	{
		MonoBehaviour monoBehaviour = requestable as MonoBehaviour;
		if (!useable)
		{
			useable = monoBehaviour.GetComponent<Useable>();
		}
		Character character = controllable.idMain;
		return (!character || !useable ? false : useable.EnterFromContext(character).Succeeded());
	}

	private static bool UseableForwardFromContext(IContextRequestable requestable, Controllable controllable)
	{
		return ContextRequestable.UseableForwardFromContext(requestable, controllable, null);
	}

	public static ContextResponse UseableForwardFromContextRespond(IContextRequestable requestable, Controllable controllable, Useable useable)
	{
		return (!ContextRequestable.UseableForwardFromContext(requestable, controllable, useable) ? ContextResponse.FailBreak : ContextResponse.DoneBreak);
	}

	public static ContextResponse UseableForwardFromContextRespond(IContextRequestable requestable, Controllable controllable)
	{
		return (!ContextRequestable.UseableForwardFromContext(requestable, controllable, null) ? ContextResponse.FailBreak : ContextResponse.DoneBreak);
	}

	public static class PointUtil
	{
		public static bool SpriteOrOrigin(Component component, out Vector3 worldPoint)
		{
			ContextSprite contextSprite;
			if (ContextSprite.FindSprite(component, out contextSprite))
			{
				worldPoint = contextSprite.transform.position;
				return true;
			}
			worldPoint = component.transform.position;
			return false;
		}
	}
}