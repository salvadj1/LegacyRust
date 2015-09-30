using System;
using System.Collections.Generic;
using UnityEngine;

public struct RPOSWindowMessageCenter
{
	public const RPOSWindowMessage kBegin = RPOSWindowMessage.WillShow;

	public const RPOSWindowMessage kLast = RPOSWindowMessage.DidHide;

	public const RPOSWindowMessage kEnd = RPOSWindowMessage.WillClose;

	public const int kMessageCount = 4;

	private RPOSWindowMessageCenter.RPOSWindowMessageResponder[] responders;

	private bool init;

	private readonly static RPOSWindowMessageHandler[] none;

	static RPOSWindowMessageCenter()
	{
		RPOSWindowMessageCenter.none = new RPOSWindowMessageHandler[0];
	}

	public bool Add(RPOSWindowMessage message, RPOSWindowMessageHandler handler)
	{
		if (message < RPOSWindowMessage.WillShow || message > RPOSWindowMessage.DidHide || handler == null)
		{
			return false;
		}
		if (!this.init)
		{
			this.responders = new RPOSWindowMessageCenter.RPOSWindowMessageResponder[4];
			this.init = true;
		}
		return this.responders[(int)message - (int)RPOSWindowMessage.WillShow].Add(handler);
	}

	public int CountHandlers(RPOSWindowMessage message)
	{
		return (!this.init || message < RPOSWindowMessage.WillShow || message > RPOSWindowMessage.DidHide ? 0 : this.responders[(int)message - (int)RPOSWindowMessage.WillShow].count);
	}

	public IEnumerable<RPOSWindowMessageHandler> EnumerateHandlers(RPOSWindowMessage message)
	{
		if (!this.init || message < RPOSWindowMessage.WillShow || message > RPOSWindowMessage.DidHide)
		{
			return RPOSWindowMessageCenter.none;
		}
		int num = (int)message - (int)RPOSWindowMessage.WillShow;
		if (!this.responders[num].init || this.responders[num].count == 0)
		{
			return RPOSWindowMessageCenter.none;
		}
		return this.responders[num].handlers;
	}

	public void Fire(RPOSWindow window, RPOSWindowMessage message)
	{
		if (this.init && message >= RPOSWindowMessage.WillShow && message <= RPOSWindowMessage.DidHide)
		{
			this.responders[(int)message - (int)RPOSWindowMessage.WillShow].Invoke(window, message);
		}
	}

	public bool Remove(RPOSWindowMessage message, RPOSWindowMessageHandler handler)
	{
		if (!this.init || message < RPOSWindowMessage.WillShow || message > RPOSWindowMessage.DidHide || handler == null)
		{
			return false;
		}
		return this.responders[(int)message - (int)RPOSWindowMessage.WillShow].Remove(handler);
	}

	private struct RPOSWindowMessageResponder
	{
		public HashSet<RPOSWindowMessageHandler> handlerGate;

		public List<RPOSWindowMessageHandler> handlers;

		public int count;

		public bool init;

		public bool Add(RPOSWindowMessageHandler handler)
		{
			if (handler == null)
			{
				return false;
			}
			if (!this.init)
			{
				this.handlerGate = new HashSet<RPOSWindowMessageHandler>();
				this.handlers = new List<RPOSWindowMessageHandler>();
				this.init = true;
				this.handlerGate.Add(handler);
			}
			else if (!this.handlerGate.Add(handler))
			{
				return false;
			}
			this.handlers.Add(handler);
			RPOSWindowMessageCenter.RPOSWindowMessageResponder rPOSWindowMessageResponder = this;
			rPOSWindowMessageResponder.count = rPOSWindowMessageResponder.count + 1;
			return true;
		}

		public void Invoke(RPOSWindow window, RPOSWindowMessage message)
		{
			if (!this.init || this.count == 0)
			{
				return;
			}
			if (((int)message - (int)RPOSWindowMessage.WillShow & (int)RPOSWindowMessage.DidOpen) != (int)RPOSWindowMessage.DidOpen)
			{
				for (int i = 0; i < this.count; i++)
				{
					if (!this.TryInvoke(window, message, i))
					{
						this.handlerGate.Remove(this.handlers[i]);
						int num = i;
						i = num - 1;
						this.handlers.RemoveAt(num);
						RPOSWindowMessageCenter.RPOSWindowMessageResponder rPOSWindowMessageResponder = this;
						rPOSWindowMessageResponder.count = rPOSWindowMessageResponder.count - 1;
					}
				}
			}
			else
			{
				for (int j = this.count - 1; j >= 0; j--)
				{
					if (!this.TryInvoke(window, message, j))
					{
						this.handlerGate.Remove(this.handlers[j]);
						this.handlers.RemoveAt(j);
						RPOSWindowMessageCenter.RPOSWindowMessageResponder rPOSWindowMessageResponder1 = this;
						rPOSWindowMessageResponder1.count = rPOSWindowMessageResponder1.count - 1;
					}
				}
			}
		}

		public bool Remove(RPOSWindowMessageHandler handler)
		{
			if (!this.init || handler == null || !this.handlerGate.Remove(handler))
			{
				return false;
			}
			this.handlers.Remove(handler);
			RPOSWindowMessageCenter.RPOSWindowMessageResponder rPOSWindowMessageResponder = this;
			rPOSWindowMessageResponder.count = rPOSWindowMessageResponder.count - 1;
			return true;
		}

		private bool TryInvoke(RPOSWindow window, RPOSWindowMessage message, int i)
		{
			bool flag;
			RPOSWindowMessageHandler item = this.handlers[i];
			try
			{
				flag = item(window, message);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Debug.LogError(string.Concat(new object[] { "handler ", item, " threw exception with message ", message, " on window ", window, " and will no longer execute. The exception is below\r\n", exception }), window);
				flag = false;
			}
			return flag;
		}
	}
}