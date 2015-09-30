using System;
using UnityEngine;

public class SoundEffect : ScriptableObject
{
	public SoundEffect()
	{
	}

	public struct Levels
	{
		public float volume;

		public float pitch;

		public float pan;

		public float doppler;

		public float spread;
	}

	public struct MinMax
	{
		public float min;

		public float max;
	}

	public struct Parameters
	{
		public AudioClip clip;

		public SoundEffect.Parent parent;

		public SoundEffect.Levels levels;

		public SoundEffect.Rolloff rolloff;

		public int priority;

		public bool bypassEffects;

		public bool bypassListenerVolume;

		public Vector3 positionalValue;

		public Quaternion rotationalValue;

		public Vector3 localPosition
		{
			get
			{
				if ((this.parent.mode & SoundEffect.ParentMode.RetainWorld) != SoundEffect.ParentMode.RetainWorld)
				{
					return this.positionalValue;
				}
				return this.parent.transform.InverseTransformPoint(this.positionalValue);
			}
			set
			{
				if ((this.parent.mode & SoundEffect.ParentMode.RetainWorld) == SoundEffect.ParentMode.RetainWorld)
				{
					this.positionalValue = this.parent.transform.TransformPoint(value);
				}
				else
				{
					this.positionalValue = value;
				}
			}
		}

		public Quaternion localRotation
		{
			get
			{
				if ((this.parent.mode & SoundEffect.ParentMode.RetainWorld) != SoundEffect.ParentMode.RetainWorld)
				{
					return this.rotationalValue;
				}
				return SoundEffect.Parameters.InverseTransformQuaternion(this.parent.transform, this.rotationalValue);
			}
			set
			{
				if ((this.parent.mode & SoundEffect.ParentMode.RetainWorld) == SoundEffect.ParentMode.RetainWorld)
				{
					this.rotationalValue = SoundEffect.Parameters.TransformQuaternion(this.parent.transform, value);
				}
				else
				{
					this.rotationalValue = value;
				}
			}
		}

		public Vector3 position
		{
			get
			{
				if ((this.parent.mode & SoundEffect.ParentMode.RetainWorld) != SoundEffect.ParentMode.RetainLocal)
				{
					return this.positionalValue;
				}
				return this.parent.transform.TransformPoint(this.positionalValue);
			}
			set
			{
				if ((this.parent.mode & SoundEffect.ParentMode.RetainWorld) == SoundEffect.ParentMode.RetainLocal)
				{
					this.positionalValue = this.parent.transform.InverseTransformPoint(value);
				}
				else
				{
					this.positionalValue = value;
				}
			}
		}

		public Quaternion rotation
		{
			get
			{
				if ((this.parent.mode & SoundEffect.ParentMode.RetainWorld) != SoundEffect.ParentMode.RetainLocal)
				{
					return this.rotationalValue;
				}
				return SoundEffect.Parameters.TransformQuaternion(this.parent.transform, this.rotationalValue);
			}
			set
			{
				if ((this.parent.mode & SoundEffect.ParentMode.RetainWorld) == SoundEffect.ParentMode.RetainLocal)
				{
					this.rotationalValue = SoundEffect.Parameters.InverseTransformQuaternion(this.parent.transform, value);
				}
				else
				{
					this.rotationalValue = value;
				}
			}
		}

		private static Quaternion InverseTransformQuaternion(Transform transform, Quaternion rotation)
		{
			return rotation * Quaternion.Inverse(transform.rotation);
		}

		private static Quaternion TransformQuaternion(Transform transform, Quaternion rotation)
		{
			return transform.rotation * rotation;
		}
	}

	public struct Parent
	{
		public Transform transform;

		public SoundEffect.ParentMode mode;
	}

	public enum ParentMode
	{
		None = 0,
		RetainLocal = 1,
		RetainWorld = 3,
		StartLocally = 5,
		StartWorld = 6,
		CameraLocally = 9,
		CameraWorld = 10
	}

	public struct Rolloff
	{
		public const float kCutoffVolume = 0.001f;

		public SoundEffect.MinMax distance;

		public float? manualCutoffDistance;

		public bool logarithmic;
	}
}