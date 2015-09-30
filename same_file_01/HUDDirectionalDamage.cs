using System;
using UnityEngine;

public class HUDDirectionalDamage : HUDIndicator
{
	private const string materialProp_MIN = "_MinChannels";

	private const string materialProp_MAX = "_MaxChannels";

	[SerializeField]
	private Material skeletonMaterial;

	private Vector4 lastBoundMin;

	private Vector4 lastBoundMax;

	[NonSerialized]
	public Vector3 worldDirection = Vector3.left;

	[NonSerialized]
	public double damageTime;

	[NonSerialized]
	public double duration;

	[NonSerialized]
	public double damageAmount;

	private Vector4 randMin;

	private Vector4 randMax;

	private double speedModX;

	private double speedModY;

	private double speedModZ;

	private double speedModW;

	private bool swapX;

	private bool swapY;

	private bool swapZ;

	private bool inverseX;

	private bool inverseY;

	private bool inverseZ;

	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private UITexture texture;

	private UIPanelMaterialPropertyBlock propBlock = new UIPanelMaterialPropertyBlock();

	public HUDDirectionalDamage()
	{
	}

	private void Awake()
	{
		this.lastBoundMin = this.skeletonMaterial.GetVector("_MinChannels");
		this.lastBoundMax = this.skeletonMaterial.GetVector("_MaxChannels");
	}

	protected sealed override bool Continue()
	{
		Vector4 vector4 = new Vector4();
		Vector4 vector41 = new Vector4();
		float single;
		if (this.duration <= 0)
		{
			return false;
		}
		double num = HUDIndicator.stepTime - this.damageTime;
		if (num > this.duration)
		{
			return false;
		}
		this.propBlock.Clear();
		double num1 = num / this.duration;
		double num2 = num1 * this.speedModX;
		double num3 = num1 * this.speedModY;
		double num4 = num1 * this.speedModZ;
		if (num2 > 1)
		{
			num2 = 1 - (num2 - 1);
		}
		if (num3 > 1)
		{
			num3 = 1 - (num3 - 1);
		}
		if (num4 > 1)
		{
			num4 = 1 - (num4 - 1);
		}
		double num5 = TransitionFunctions.Spline(num2, 1, 0);
		double num6 = (num3 >= 0.100000001490116 ? TransitionFunctions.Spline((num3 - 0.1) / 0.9, 1, 0) : TransitionFunctions.Spline(num3 / 0.1, 0, 1));
		double num7 = Math.Cos(num4 * 1.5707963267949);
		vector4.x = (float)num7;
		vector4.y = (float)num5;
		vector4.z = (float)num6;
		vector4.w = (float)num1;
		vector41.x = (float)(num7 / this.speedModX);
		vector41.y = (float)(num5 / this.speedModY);
		vector41.z = (float)(num6 / this.speedModZ);
		vector41.w = (float)(1 - num1);
		if (this.inverseX)
		{
			vector41.x = 1f - vector41.x;
		}
		if (this.inverseY)
		{
			vector41.y = 1f - vector41.y;
		}
		if (this.inverseZ)
		{
			vector41.z = 1f - vector41.z;
		}
		if (this.swapX)
		{
			single = vector41.x;
			vector41.x = vector4.x;
			vector4.x = single;
		}
		if (this.swapY)
		{
			single = vector41.y;
			vector41.y = vector4.y;
			vector4.y = single;
		}
		if (this.swapZ)
		{
			single = vector41.z;
			vector41.z = vector4.z;
			vector4.z = single;
		}
		if (vector4 != this.lastBoundMin)
		{
			this.lastBoundMin = vector4;
			this.propBlock.Set("_MinChannels", this.lastBoundMin);
		}
		if (vector41 != this.lastBoundMax)
		{
			this.lastBoundMax = vector41;
			this.propBlock.Set("_MaxChannels", this.lastBoundMax);
		}
		Vector3 vector3 = HUDIndicator.worldToCameraLocalMatrix.MultiplyVector(this.worldDirection);
		vector3.Normalize();
		if (vector3.y * vector3.y <= 0.99f)
		{
			base.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vector3.z, vector3.x) * 57.29578f);
		}
		this.panel.propertyBlock = this.propBlock;
		return true;
	}

	public static void CreateIndicator(Vector3 worldDamageDirection, double damageAmount, double timestamp, double duration, HUDDirectionalDamage prefab)
	{
		HUDDirectionalDamage hUDDirectionalDamage = (HUDDirectionalDamage)HUDIndicator.InstantiateIndicator(HUDIndicator.ScratchTarget.CenteredFixed3000Tall, prefab, HUDIndicator.PlacementSpace.DoNotModify, Vector3.zero);
		hUDDirectionalDamage.damageTime = timestamp;
		hUDDirectionalDamage.duration = duration;
		hUDDirectionalDamage.damageAmount = damageAmount;
		hUDDirectionalDamage.worldDirection = -worldDamageDirection;
		hUDDirectionalDamage.transform.localPosition = Vector3.zero;
		hUDDirectionalDamage.transform.localRotation = Quaternion.identity;
		hUDDirectionalDamage.transform.localScale = Vector3.one;
		hUDDirectionalDamage.texture.ForceReloadMaterial();
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
	}

	protected new void Start()
	{
		this.randMin.x = UnityEngine.Random.Range(0f, 0.06f);
		this.randMin.y = UnityEngine.Random.Range(0f, 0.06f);
		this.randMin.z = UnityEngine.Random.Range(0f, 0.06f);
		this.randMin.w = UnityEngine.Random.Range(0f, 0.06f);
		this.randMax.x = UnityEngine.Random.Range(0.94f, 1f);
		this.randMax.y = UnityEngine.Random.Range(0.94f, 1f);
		this.randMax.z = UnityEngine.Random.Range(0.94f, 1f);
		this.randMax.w = UnityEngine.Random.Range(0.94f, 1f);
		int num = UnityEngine.Random.Range(0, 64);
		this.swapX = (num & 1) == 1;
		this.inverseX = (num & 8) == 8;
		this.swapY = (num & 2) == 2;
		this.inverseY = (num & 16) == 16;
		this.swapZ = (num & 4) == 4;
		this.inverseZ = (num & 32) == 32;
		this.speedModX = 1.12 - (1 - ((double)this.randMax.x - (double)this.randMin.x));
		this.speedModY = 1.12 - (1 - ((double)this.randMax.y - (double)this.randMin.y));
		this.speedModZ = 1.12 - (1 - ((double)this.randMax.z - (double)this.randMin.z));
		this.speedModW = 1.12 - (1 - ((double)this.randMax.w - (double)this.randMin.w));
		HUDDirectionalDamage hUDDirectionalDamage = this;
		hUDDirectionalDamage.speedModX = hUDDirectionalDamage.speedModX / this.speedModW;
		HUDDirectionalDamage hUDDirectionalDamage1 = this;
		hUDDirectionalDamage1.speedModY = hUDDirectionalDamage1.speedModY / this.speedModW;
		HUDDirectionalDamage hUDDirectionalDamage2 = this;
		hUDDirectionalDamage2.speedModZ = hUDDirectionalDamage2.speedModZ / this.speedModW;
		this.speedModW = 1;
		base.Start();
	}
}