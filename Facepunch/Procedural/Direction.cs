using System;
using UnityEngine;

namespace Facepunch.Procedural
{
	public struct Direction
	{
		[NonSerialized]
		public Integrated<Vector3> @value;

		public Integration Advance(ulong millis)
		{
			Integration integration = this.@value.clock.IntegrateTime(millis);
			Integration integration1 = integration;
			if (integration1 == Integration.Moved)
			{
				this.@value.current = Vector3.Slerp(this.@value.begin, this.@value.end, this.@value.clock.percentf);
			}
			else if (integration1 == Integration.MovedDestination)
			{
				this.@value.current = this.@value.end;
			}
			return integration;
		}

		public void Target(ref Vector3 target, float degreeSpeed)
		{
			if (this.@value.clock.once)
			{
				float single = Mathf.Abs(Vector3.Angle(this.@value.current, target));
				if (single < degreeSpeed * 1.401298E-45f || degreeSpeed == 0f)
				{
					this.@value.SetImmediate(ref target);
				}
				else
				{
					this.@value.begin = this.@value.current;
					this.@value.end = target;
					if (degreeSpeed < 0f)
					{
						degreeSpeed = -degreeSpeed;
					}
					ulong num = (ulong)Math.Ceiling((double)single * 1000 / (double)degreeSpeed);
					ulong num1 = num;
					this.@value.clock.remain = num;
					this.@value.clock.duration = num1;
					if (this.@value.clock.remain <= (long)1)
					{
						this.@value.SetImmediate(ref target);
					}
				}
			}
			else
			{
				this.@value.SetImmediate(ref target);
			}
		}
	}
}