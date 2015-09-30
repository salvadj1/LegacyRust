using System;
using UnityEngine;

namespace NGUIHack
{
	public class Event : IDisposable
	{
		public static int pressed;

		public static int unpressed;

		public static int held;

		private readonly UnityEngine.Event @event;

		private readonly EventType originalType;

		private readonly EventType originalRawType;

		private readonly EventType overrideType;

		private readonly Vector2 screenPosition;

		public bool alt
		{
			get
			{
				return this.@event.alt;
			}
		}

		public int button
		{
			get
			{
				return this.@event.button;
			}
		}

		public bool capsLock
		{
			get
			{
				return this.@event.capsLock;
			}
		}

		public char character
		{
			get
			{
				return this.@event.character;
			}
		}

		public bool control
		{
			get
			{
				return this.@event.control;
			}
		}

		public Vector2 delta
		{
			get
			{
				return this.@event.delta;
			}
		}

		public KeyCode keyCode
		{
			get
			{
				return this.@event.keyCode;
			}
		}

		public EventModifiers modifiers
		{
			get
			{
				return this.@event.modifiers;
			}
		}

		public Vector2 mousePosition
		{
			get
			{
				return this.screenPosition;
			}
		}

		public EventType rawType
		{
			get
			{
				return (this.overrideType != EventType.Used ? this.overrideType : this.@event.rawType);
			}
		}

		internal UnityEngine.Event real
		{
			get
			{
				return this.@event;
			}
		}

		public bool shift
		{
			get
			{
				return this.@event.shift;
			}
		}

		public EventType type
		{
			get
			{
				return (this.overrideType != EventType.Used ? this.overrideType : this.@event.type);
			}
		}

		public EventType unityOriginalRawType
		{
			get
			{
				return this.originalRawType;
			}
		}

		internal Event(UnityEngine.Event @event)
		{
			this.@event = @event;
			this.originalType = @event.type;
			this.originalRawType = @event.rawType;
			this.overrideType = EventType.Used;
			this.screenPosition = Input.mousePosition;
		}

		internal Event(UnityEngine.Event @event, EventType overrideType) : this(@event)
		{
			this.overrideType = overrideType;
		}

		public void Dispose()
		{
			if (this.overrideType != EventType.Used && this.@event.type == this.overrideType)
			{
				this.@event.type = this.originalType;
			}
		}

		public void Use()
		{
			if (this.overrideType == EventType.Used)
			{
				this.@event.Use();
			}
		}
	}
}