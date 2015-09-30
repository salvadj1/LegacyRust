using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.FieldAccess;
using RustProto.Helpers;
using RustProto.Proto;
using System;
using System.Diagnostics;
using System.IO;

namespace RustProto
{
	[DebuggerNonUserCode]
	public sealed class Vitals : GeneratedMessage<RustProto.Vitals, RustProto.Vitals.Builder>
	{
		public const int HealthFieldNumber = 1;

		public const int HydrationFieldNumber = 2;

		public const int CaloriesFieldNumber = 3;

		public const int RadiationFieldNumber = 4;

		public const int RadiationAntiFieldNumber = 5;

		public const int BleedSpeedFieldNumber = 6;

		public const int BleedMaxFieldNumber = 7;

		public const int HealSpeedFieldNumber = 8;

		public const int HealMaxFieldNumber = 9;

		public const int TemperatureFieldNumber = 10;

		private readonly static RustProto.Vitals defaultInstance;

		private readonly static string[] _vitalsFieldNames;

		private readonly static uint[] _vitalsFieldTags;

		private bool hasHealth;

		private float health_ = 100f;

		private bool hasHydration;

		private float hydration_ = 30f;

		private bool hasCalories;

		private float calories_ = 1000f;

		private bool hasRadiation;

		private float radiation_;

		private bool hasRadiationAnti;

		private float radiationAnti_;

		private bool hasBleedSpeed;

		private float bleedSpeed_;

		private bool hasBleedMax;

		private float bleedMax_;

		private bool hasHealSpeed;

		private float healSpeed_;

		private bool hasHealMax;

		private float healMax_;

		private bool hasTemperature;

		private float temperature_;

		private int memoizedSerializedSize = -1;

		public float BleedMax
		{
			get
			{
				return this.bleedMax_;
			}
		}

		public float BleedSpeed
		{
			get
			{
				return this.bleedSpeed_;
			}
		}

		public float Calories
		{
			get
			{
				return this.calories_;
			}
		}

		public static RustProto.Vitals DefaultInstance
		{
			get
			{
				return RustProto.Vitals.defaultInstance;
			}
		}

		public override RustProto.Vitals DefaultInstanceForType
		{
			get
			{
				return RustProto.Vitals.DefaultInstance;
			}
		}

		public static MessageDescriptor Descriptor
		{
			get
			{
				return RustProto.Proto.Vitals.internal__static_RustProto_Vitals__Descriptor;
			}
		}

		public bool HasBleedMax
		{
			get
			{
				return this.hasBleedMax;
			}
		}

		public bool HasBleedSpeed
		{
			get
			{
				return this.hasBleedSpeed;
			}
		}

		public bool HasCalories
		{
			get
			{
				return this.hasCalories;
			}
		}

		public bool HasHealMax
		{
			get
			{
				return this.hasHealMax;
			}
		}

		public bool HasHealSpeed
		{
			get
			{
				return this.hasHealSpeed;
			}
		}

		public bool HasHealth
		{
			get
			{
				return this.hasHealth;
			}
		}

		public bool HasHydration
		{
			get
			{
				return this.hasHydration;
			}
		}

		public bool HasRadiation
		{
			get
			{
				return this.hasRadiation;
			}
		}

		public bool HasRadiationAnti
		{
			get
			{
				return this.hasRadiationAnti;
			}
		}

		public bool HasTemperature
		{
			get
			{
				return this.hasTemperature;
			}
		}

		public float HealMax
		{
			get
			{
				return this.healMax_;
			}
		}

		public float HealSpeed
		{
			get
			{
				return this.healSpeed_;
			}
		}

		public float Health
		{
			get
			{
				return this.health_;
			}
		}

		public float Hydration
		{
			get
			{
				return this.hydration_;
			}
		}

		protected override FieldAccessorTable<RustProto.Vitals, RustProto.Vitals.Builder> InternalFieldAccessors
		{
			get
			{
				return RustProto.Proto.Vitals.internal__static_RustProto_Vitals__FieldAccessorTable;
			}
		}

		public override bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public float Radiation
		{
			get
			{
				return this.radiation_;
			}
		}

		public float RadiationAnti
		{
			get
			{
				return this.radiationAnti_;
			}
		}

		public override int SerializedSize
		{
			get
			{
				int serializedSize = this.memoizedSerializedSize;
				if (serializedSize != -1)
				{
					return serializedSize;
				}
				serializedSize = 0;
				if (this.hasHealth)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(1, this.Health);
				}
				if (this.hasHydration)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(2, this.Hydration);
				}
				if (this.hasCalories)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(3, this.Calories);
				}
				if (this.hasRadiation)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(4, this.Radiation);
				}
				if (this.hasRadiationAnti)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(5, this.RadiationAnti);
				}
				if (this.hasBleedSpeed)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(6, this.BleedSpeed);
				}
				if (this.hasBleedMax)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(7, this.BleedMax);
				}
				if (this.hasHealSpeed)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(8, this.HealSpeed);
				}
				if (this.hasHealMax)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(9, this.HealMax);
				}
				if (this.hasTemperature)
				{
					serializedSize = serializedSize + CodedOutputStream.ComputeFloatSize(10, this.Temperature);
				}
				serializedSize = serializedSize + this.UnknownFields.SerializedSize;
				this.memoizedSerializedSize = serializedSize;
				return serializedSize;
			}
		}

		public float Temperature
		{
			get
			{
				return this.temperature_;
			}
		}

		protected override RustProto.Vitals ThisMessage
		{
			get
			{
				return this;
			}
		}

		static Vitals()
		{
			RustProto.Vitals.defaultInstance = (new RustProto.Vitals()).MakeReadOnly();
			RustProto.Vitals._vitalsFieldNames = new string[] { "bleed_max", "bleed_speed", "calories", "heal_max", "heal_speed", "health", "hydration", "radiation", "radiation_anti", "temperature" };
			RustProto.Vitals._vitalsFieldTags = new uint[] { 61, 53, 29, 77, 69, 13, 21, 37, 45, 85 };
			object.ReferenceEquals(RustProto.Proto.Vitals.Descriptor, null);
		}

		private Vitals()
		{
		}

		public static RustProto.Vitals.Builder CreateBuilder()
		{
			return new RustProto.Vitals.Builder();
		}

		public static RustProto.Vitals.Builder CreateBuilder(RustProto.Vitals prototype)
		{
			return new RustProto.Vitals.Builder(prototype);
		}

		public override RustProto.Vitals.Builder CreateBuilderForType()
		{
			return new RustProto.Vitals.Builder();
		}

		private RustProto.Vitals MakeReadOnly()
		{
			return this;
		}

		public static RustProto.Vitals ParseDelimitedFrom(Stream input)
		{
			return RustProto.Vitals.CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
		}

		public static RustProto.Vitals ParseDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Vitals.CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Vitals ParseFrom(ByteString data)
		{
			return RustProto.Vitals.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Vitals ParseFrom(ByteString data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Vitals.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Vitals ParseFrom(byte[] data)
		{
			return RustProto.Vitals.CreateBuilder().MergeFrom(data).BuildParsed();
		}

		public static RustProto.Vitals ParseFrom(byte[] data, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Vitals.CreateBuilder().MergeFrom(data, extensionRegistry).BuildParsed();
		}

		public static RustProto.Vitals ParseFrom(Stream input)
		{
			return RustProto.Vitals.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Vitals ParseFrom(Stream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Vitals.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static RustProto.Vitals ParseFrom(ICodedInputStream input)
		{
			return RustProto.Vitals.CreateBuilder().MergeFrom(input).BuildParsed();
		}

		public static RustProto.Vitals ParseFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
		{
			return RustProto.Vitals.CreateBuilder().MergeFrom(input, extensionRegistry).BuildParsed();
		}

		public static Recycler<RustProto.Vitals, RustProto.Vitals.Builder> Recycler()
		{
			return Recycler<RustProto.Vitals, RustProto.Vitals.Builder>.Manufacture();
		}

		public override RustProto.Vitals.Builder ToBuilder()
		{
			return RustProto.Vitals.CreateBuilder(this);
		}

		public override void WriteTo(ICodedOutputStream output)
		{
			int serializedSize = this.SerializedSize;
			string[] strArrays = RustProto.Vitals._vitalsFieldNames;
			if (this.hasHealth)
			{
				output.WriteFloat(1, strArrays[5], this.Health);
			}
			if (this.hasHydration)
			{
				output.WriteFloat(2, strArrays[6], this.Hydration);
			}
			if (this.hasCalories)
			{
				output.WriteFloat(3, strArrays[2], this.Calories);
			}
			if (this.hasRadiation)
			{
				output.WriteFloat(4, strArrays[7], this.Radiation);
			}
			if (this.hasRadiationAnti)
			{
				output.WriteFloat(5, strArrays[8], this.RadiationAnti);
			}
			if (this.hasBleedSpeed)
			{
				output.WriteFloat(6, strArrays[1], this.BleedSpeed);
			}
			if (this.hasBleedMax)
			{
				output.WriteFloat(7, strArrays[0], this.BleedMax);
			}
			if (this.hasHealSpeed)
			{
				output.WriteFloat(8, strArrays[4], this.HealSpeed);
			}
			if (this.hasHealMax)
			{
				output.WriteFloat(9, strArrays[3], this.HealMax);
			}
			if (this.hasTemperature)
			{
				output.WriteFloat(10, strArrays[9], this.Temperature);
			}
			this.UnknownFields.WriteTo(output);
		}

		[DebuggerNonUserCode]
		public sealed class Builder : GeneratedBuilder<RustProto.Vitals, RustProto.Vitals.Builder>
		{
			private bool resultIsReadOnly;

			private RustProto.Vitals result;

			public float BleedMax
			{
				get
				{
					return this.result.BleedMax;
				}
				set
				{
					this.SetBleedMax(value);
				}
			}

			public float BleedSpeed
			{
				get
				{
					return this.result.BleedSpeed;
				}
				set
				{
					this.SetBleedSpeed(value);
				}
			}

			public float Calories
			{
				get
				{
					return this.result.Calories;
				}
				set
				{
					this.SetCalories(value);
				}
			}

			public override RustProto.Vitals DefaultInstanceForType
			{
				get
				{
					return RustProto.Vitals.DefaultInstance;
				}
			}

			public override MessageDescriptor DescriptorForType
			{
				get
				{
					return RustProto.Vitals.Descriptor;
				}
			}

			public bool HasBleedMax
			{
				get
				{
					return this.result.hasBleedMax;
				}
			}

			public bool HasBleedSpeed
			{
				get
				{
					return this.result.hasBleedSpeed;
				}
			}

			public bool HasCalories
			{
				get
				{
					return this.result.hasCalories;
				}
			}

			public bool HasHealMax
			{
				get
				{
					return this.result.hasHealMax;
				}
			}

			public bool HasHealSpeed
			{
				get
				{
					return this.result.hasHealSpeed;
				}
			}

			public bool HasHealth
			{
				get
				{
					return this.result.hasHealth;
				}
			}

			public bool HasHydration
			{
				get
				{
					return this.result.hasHydration;
				}
			}

			public bool HasRadiation
			{
				get
				{
					return this.result.hasRadiation;
				}
			}

			public bool HasRadiationAnti
			{
				get
				{
					return this.result.hasRadiationAnti;
				}
			}

			public bool HasTemperature
			{
				get
				{
					return this.result.hasTemperature;
				}
			}

			public float HealMax
			{
				get
				{
					return this.result.HealMax;
				}
				set
				{
					this.SetHealMax(value);
				}
			}

			public float HealSpeed
			{
				get
				{
					return this.result.HealSpeed;
				}
				set
				{
					this.SetHealSpeed(value);
				}
			}

			public float Health
			{
				get
				{
					return this.result.Health;
				}
				set
				{
					this.SetHealth(value);
				}
			}

			public float Hydration
			{
				get
				{
					return this.result.Hydration;
				}
				set
				{
					this.SetHydration(value);
				}
			}

			public override bool IsInitialized
			{
				get
				{
					return this.result.IsInitialized;
				}
			}

			protected override RustProto.Vitals MessageBeingBuilt
			{
				get
				{
					return this.PrepareBuilder();
				}
			}

			public float Radiation
			{
				get
				{
					return this.result.Radiation;
				}
				set
				{
					this.SetRadiation(value);
				}
			}

			public float RadiationAnti
			{
				get
				{
					return this.result.RadiationAnti;
				}
				set
				{
					this.SetRadiationAnti(value);
				}
			}

			public float Temperature
			{
				get
				{
					return this.result.Temperature;
				}
				set
				{
					this.SetTemperature(value);
				}
			}

			protected override RustProto.Vitals.Builder ThisBuilder
			{
				get
				{
					return this;
				}
			}

			public Builder()
			{
				this.result = RustProto.Vitals.DefaultInstance;
				this.resultIsReadOnly = true;
			}

			internal Builder(RustProto.Vitals cloneFrom)
			{
				this.result = cloneFrom;
				this.resultIsReadOnly = true;
			}

			public override RustProto.Vitals BuildPartial()
			{
				if (this.resultIsReadOnly)
				{
					return this.result;
				}
				this.resultIsReadOnly = true;
				return this.result.MakeReadOnly();
			}

			public override RustProto.Vitals.Builder Clear()
			{
				this.result = RustProto.Vitals.DefaultInstance;
				this.resultIsReadOnly = true;
				return this;
			}

			public RustProto.Vitals.Builder ClearBleedMax()
			{
				this.PrepareBuilder();
				this.result.hasBleedMax = false;
				this.result.bleedMax_ = 0f;
				return this;
			}

			public RustProto.Vitals.Builder ClearBleedSpeed()
			{
				this.PrepareBuilder();
				this.result.hasBleedSpeed = false;
				this.result.bleedSpeed_ = 0f;
				return this;
			}

			public RustProto.Vitals.Builder ClearCalories()
			{
				this.PrepareBuilder();
				this.result.hasCalories = false;
				this.result.calories_ = 1000f;
				return this;
			}

			public RustProto.Vitals.Builder ClearHealMax()
			{
				this.PrepareBuilder();
				this.result.hasHealMax = false;
				this.result.healMax_ = 0f;
				return this;
			}

			public RustProto.Vitals.Builder ClearHealSpeed()
			{
				this.PrepareBuilder();
				this.result.hasHealSpeed = false;
				this.result.healSpeed_ = 0f;
				return this;
			}

			public RustProto.Vitals.Builder ClearHealth()
			{
				this.PrepareBuilder();
				this.result.hasHealth = false;
				this.result.health_ = 100f;
				return this;
			}

			public RustProto.Vitals.Builder ClearHydration()
			{
				this.PrepareBuilder();
				this.result.hasHydration = false;
				this.result.hydration_ = 30f;
				return this;
			}

			public RustProto.Vitals.Builder ClearRadiation()
			{
				this.PrepareBuilder();
				this.result.hasRadiation = false;
				this.result.radiation_ = 0f;
				return this;
			}

			public RustProto.Vitals.Builder ClearRadiationAnti()
			{
				this.PrepareBuilder();
				this.result.hasRadiationAnti = false;
				this.result.radiationAnti_ = 0f;
				return this;
			}

			public RustProto.Vitals.Builder ClearTemperature()
			{
				this.PrepareBuilder();
				this.result.hasTemperature = false;
				this.result.temperature_ = 0f;
				return this;
			}

			public override RustProto.Vitals.Builder Clone()
			{
				if (this.resultIsReadOnly)
				{
					return new RustProto.Vitals.Builder(this.result);
				}
				return (new RustProto.Vitals.Builder()).MergeFrom(this.result);
			}

			public override RustProto.Vitals.Builder MergeFrom(IMessage other)
			{
				if (other is RustProto.Vitals)
				{
					return this.MergeFrom((RustProto.Vitals)other);
				}
				base.MergeFrom(other);
				return this;
			}

			public override RustProto.Vitals.Builder MergeFrom(RustProto.Vitals other)
			{
				if (other == RustProto.Vitals.DefaultInstance)
				{
					return this;
				}
				this.PrepareBuilder();
				if (other.HasHealth)
				{
					this.Health = other.Health;
				}
				if (other.HasHydration)
				{
					this.Hydration = other.Hydration;
				}
				if (other.HasCalories)
				{
					this.Calories = other.Calories;
				}
				if (other.HasRadiation)
				{
					this.Radiation = other.Radiation;
				}
				if (other.HasRadiationAnti)
				{
					this.RadiationAnti = other.RadiationAnti;
				}
				if (other.HasBleedSpeed)
				{
					this.BleedSpeed = other.BleedSpeed;
				}
				if (other.HasBleedMax)
				{
					this.BleedMax = other.BleedMax;
				}
				if (other.HasHealSpeed)
				{
					this.HealSpeed = other.HealSpeed;
				}
				if (other.HasHealMax)
				{
					this.HealMax = other.HealMax;
				}
				if (other.HasTemperature)
				{
					this.Temperature = other.Temperature;
				}
				this.MergeUnknownFields(other.UnknownFields);
				return this;
			}

			public override RustProto.Vitals.Builder MergeFrom(ICodedInputStream input)
			{
				return this.MergeFrom(input, ExtensionRegistry.Empty);
			}

			public override RustProto.Vitals.Builder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry)
			{
				uint num;
				string str;
				this.PrepareBuilder();
				UnknownFieldSet.Builder builder = null;
				while (input.ReadTag(out num, out str))
				{
					if (num == 0 && str != null)
					{
						int num1 = Array.BinarySearch<string>(RustProto.Vitals._vitalsFieldNames, str, StringComparer.Ordinal);
						if (num1 < 0)
						{
							if (builder == null)
							{
								builder = UnknownFieldSet.CreateBuilder(this.UnknownFields);
							}
							this.ParseUnknownField(input, builder, extensionRegistry, num, str);
							continue;
						}
						else
						{
							num = RustProto.Vitals._vitalsFieldTags[num1];
						}
					}
					uint num2 = num;
					if (num2 == 0)
					{
						throw InvalidProtocolBufferException.InvalidTag();
					}
					if (num2 == 13)
					{
						this.result.hasHealth = input.ReadFloat(ref this.result.health_);
					}
					else if (num2 == 21)
					{
						this.result.hasHydration = input.ReadFloat(ref this.result.hydration_);
					}
					else if (num2 == 29)
					{
						this.result.hasCalories = input.ReadFloat(ref this.result.calories_);
					}
					else if (num2 == 37)
					{
						this.result.hasRadiation = input.ReadFloat(ref this.result.radiation_);
					}
					else if (num2 == 45)
					{
						this.result.hasRadiationAnti = input.ReadFloat(ref this.result.radiationAnti_);
					}
					else if (num2 == 53)
					{
						this.result.hasBleedSpeed = input.ReadFloat(ref this.result.bleedSpeed_);
					}
					else if (num2 == 61)
					{
						this.result.hasBleedMax = input.ReadFloat(ref this.result.bleedMax_);
					}
					else if (num2 == 69)
					{
						this.result.hasHealSpeed = input.ReadFloat(ref this.result.healSpeed_);
					}
					else if (num2 == 77)
					{
						this.result.hasHealMax = input.ReadFloat(ref this.result.healMax_);
					}
					else if (num2 == 85)
					{
						this.result.hasTemperature = input.ReadFloat(ref this.result.temperature_);
					}
					else
					{
						if (WireFormat.IsEndGroupTag(num))
						{
							if (builder != null)
							{
								this.UnknownFields = builder.Build();
							}
							return this;
						}
						if (builder == null)
						{
							builder = UnknownFieldSet.CreateBuilder(this.UnknownFields);
						}
						this.ParseUnknownField(input, builder, extensionRegistry, num, str);
					}
				}
				if (builder != null)
				{
					this.UnknownFields = builder.Build();
				}
				return this;
			}

			private RustProto.Vitals PrepareBuilder()
			{
				if (this.resultIsReadOnly)
				{
					RustProto.Vitals vital = this.result;
					this.result = new RustProto.Vitals();
					this.resultIsReadOnly = false;
					this.MergeFrom(vital);
				}
				return this.result;
			}

			public RustProto.Vitals.Builder SetBleedMax(float value)
			{
				this.PrepareBuilder();
				this.result.hasBleedMax = true;
				this.result.bleedMax_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetBleedSpeed(float value)
			{
				this.PrepareBuilder();
				this.result.hasBleedSpeed = true;
				this.result.bleedSpeed_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetCalories(float value)
			{
				this.PrepareBuilder();
				this.result.hasCalories = true;
				this.result.calories_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetHealMax(float value)
			{
				this.PrepareBuilder();
				this.result.hasHealMax = true;
				this.result.healMax_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetHealSpeed(float value)
			{
				this.PrepareBuilder();
				this.result.hasHealSpeed = true;
				this.result.healSpeed_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetHealth(float value)
			{
				this.PrepareBuilder();
				this.result.hasHealth = true;
				this.result.health_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetHydration(float value)
			{
				this.PrepareBuilder();
				this.result.hasHydration = true;
				this.result.hydration_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetRadiation(float value)
			{
				this.PrepareBuilder();
				this.result.hasRadiation = true;
				this.result.radiation_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetRadiationAnti(float value)
			{
				this.PrepareBuilder();
				this.result.hasRadiationAnti = true;
				this.result.radiationAnti_ = value;
				return this;
			}

			public RustProto.Vitals.Builder SetTemperature(float value)
			{
				this.PrepareBuilder();
				this.result.hasTemperature = true;
				this.result.temperature_ = value;
				return this;
			}
		}
	}
}