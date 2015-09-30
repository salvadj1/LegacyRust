using System;
using UnityEngine;

namespace Facepunch.Procedural
{
	public struct Origin
	{
		[NonSerialized]
		public Integrated<Vector3> @value;

		[NonSerialized]
		public Vector3 delta;

		public Integration Advance(ulong millis)
		{
			Integration integration = this.@value.clock.IntegrateTime(millis);
			Integration integration1 = integration;
			if (integration1 == Integration.Moved)
			{
				double num = this.@value.clock.percent;
				this.@value.current.x = (float)((double)this.@value.begin.x + (double)this.delta.x * num);
				this.@value.current.y = (float)((double)this.@value.begin.y + (double)this.delta.y * num);
				this.@value.current.z = (float)((double)this.@value.begin.z + (double)this.delta.z * num);
			}
			else if (integration1 == Integration.MovedDestination)
			{
				this.@value.current = this.@value.end;
			}
			return integration;
		}

		public void Target(ref Vector3 target, float moveSpeed)
		{
			float single;
			if (this.@value.clock.once)
			{
				this.delta.x = target.x - this.@value.current.x;
				this.delta.y = target.y - this.@value.current.y;
				this.delta.z = target.z - this.@value.current.z;
				float single1 = this.delta.x * this.delta.x + this.delta.y * this.delta.y + this.delta.z * this.delta.z;
				float single2 = moveSpeed * 1.401298E-45f;
				if (single1 <= single2 * single2 || moveSpeed == 0f)
				{
					float single3 = 0f;
					single = single3;
					this.delta.z = single3;
					float single4 = single;
					single = single4;
					this.delta.y = single4;
					this.delta.x = single;
					this.@value.SetImmediate(ref target);
				}
				else
				{
					float single5 = Mathf.Sqrt(single1);
					this.@value.begin = this.@value.current;
					this.@value.end = target;
					if (moveSpeed < 0f)
					{
						moveSpeed = -moveSpeed;
					}
					ulong num = (ulong)Math.Ceiling((double)single5 * 1000 / (double)moveSpeed);
					ulong num1 = num;
					this.@value.clock.duration = num;
					this.@value.clock.remain = num1;
					if (this.@value.clock.remain <= (long)1)
					{
						float single6 = 0f;
						single = single6;
						this.delta.z = single6;
						float single7 = single;
						single = single7;
						this.delta.y = single7;
						this.delta.x = single;
						this.@value.SetImmediate(ref target);
					}
				}
			}
			else
			{
				float single8 = 0f;
				single = single8;
				this.delta.z = single8;
				float single9 = single;
				single = single9;
				this.delta.y = single9;
				this.delta.x = single;
				this.@value.SetImmediate(ref target);
			}
		}
	}
}