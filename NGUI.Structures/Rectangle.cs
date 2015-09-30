using System;
using System.Reflection;
using UnityEngine;

namespace NGUI.Structures
{
	public struct Rectangle
	{
		public const int size = 16;

		public const int halfSize = 8;

		public Vector2 a;

		public Vector2 d;

		public Vector2 b
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.d.x;
				vector2.y = this.a.y;
				return vector2;
			}
			set
			{
				this.d.x = value.x;
				this.a.y = value.y;
			}
		}

		public Vector2 c
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.a.x;
				vector2.y = this.d.y;
				return vector2;
			}
			set
			{
				this.a.x = value.x;
				this.d.y = value.y;
			}
		}

		public Vector2 center
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.a.x + (this.d.x - this.a.x) * 0.5f;
				vector2.y = this.a.y + (this.d.y - this.a.y) * 0.5f;
				return vector2;
			}
		}

		public Vector2 dim
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = this.d.x - this.a.x;
				vector2.y = this.d.y - this.a.y;
				return vector2;
			}
		}

		public float height
		{
			get
			{
				return this.d.y - this.a.y;
			}
		}

		public Vector2 this[int i]
		{
			get
			{
				Vector2 vector2 = new Vector2();
				vector2.x = ((i & 1) != 1 ? this.a.x : this.d.x);
				vector2.y = ((i & 2) != 2 ? this.a.y : this.d.y);
				return vector2;
			}
			set
			{
				if ((i & 1) != 1)
				{
					this.a.x = value.x;
				}
				else
				{
					this.d.x = value.x;
				}
				if ((i & 2) != 2)
				{
					this.a.y = value.y;
				}
				else
				{
					this.d.y = value.y;
				}
			}
		}

		public float width
		{
			get
			{
				return this.d.x - this.a.x;
			}
		}
	}
}