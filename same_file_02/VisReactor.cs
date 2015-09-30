using System;
using UnityEngine;

public abstract class VisReactor : MonoBehaviour
{
	[PrefetchComponent]
	[SerializeField]
	private VisNode _visNode;

	internal VisNode __visNode
	{
		set
		{
			this._visNode = value;
		}
	}

	public VisNode node
	{
		get
		{
			return this._visNode;
		}
	}

	protected VisNode self
	{
		get
		{
			return this._visNode;
		}
	}

	protected VisReactor()
	{
	}

	internal void AWARE_ENTER()
	{
		try
		{
			this.React_AwareEnter();
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, this);
		}
	}

	internal void AWARE_EXIT()
	{
		try
		{
			this.React_AwareExit();
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, this);
		}
	}

	protected virtual void React_AwareEnter()
	{
	}

	protected virtual void React_AwareExit()
	{
	}

	protected virtual void React_SeeAdd(VisNode spotted)
	{
	}

	protected virtual void React_SeeRemove(VisNode lost)
	{
	}

	protected virtual void React_SpectatedEnter()
	{
	}

	protected virtual void React_SpectatedExit()
	{
	}

	protected virtual void React_SpectatorAdd(VisNode spectator)
	{
	}

	protected virtual void React_SpectatorRemove(VisNode spectator)
	{
	}

	protected void Reset()
	{
		this._visNode = base.GetComponent<VisNode>();
		if (this._visNode)
		{
			this._visNode.__reactor = this;
		}
	}

	internal void SEE_ADD(VisNode spotted)
	{
		try
		{
			this.React_SeeAdd(spotted);
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, this);
		}
	}

	internal void SEE_REMOVE(VisNode lost)
	{
		try
		{
			this.React_SeeRemove(lost);
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, this);
		}
	}

	internal void SPECTATED_ENTER()
	{
		try
		{
			this.React_SpectatedEnter();
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, this);
		}
	}

	internal void SPECTATED_EXIT()
	{
		try
		{
			this.React_SpectatedExit();
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, this);
		}
	}

	internal void SPECTATOR_ADD(VisNode spectator)
	{
		try
		{
			this.React_SpectatorAdd(spectator);
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, this);
		}
	}

	internal void SPECTATOR_REMOVE(VisNode spectator)
	{
		try
		{
			this.React_SpectatorRemove(spectator);
		}
		catch (Exception exception)
		{
			Debug.LogError(exception, this);
		}
	}
}