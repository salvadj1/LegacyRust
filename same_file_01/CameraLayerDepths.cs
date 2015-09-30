using System;
using System.Reflection;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraLayerDepths : MonoBehaviour
{
	[SerializeField]
	private float layer00;

	[SerializeField]
	private float layer01;

	[SerializeField]
	private float layer02;

	[SerializeField]
	private float layer03;

	[SerializeField]
	private float layer04;

	[SerializeField]
	private float layer05;

	[SerializeField]
	private float layer06;

	[SerializeField]
	private float layer07;

	[SerializeField]
	private float layer08;

	[SerializeField]
	private float layer09;

	[SerializeField]
	private float layer10;

	[SerializeField]
	private float layer11;

	[SerializeField]
	private float layer12;

	[SerializeField]
	private float layer13;

	[SerializeField]
	private float layer14;

	[SerializeField]
	private float layer15;

	[SerializeField]
	private float layer16;

	[SerializeField]
	private float layer17;

	[SerializeField]
	private float layer18;

	[SerializeField]
	private float layer19;

	[SerializeField]
	private float layer20;

	[SerializeField]
	private float layer21;

	[SerializeField]
	private float layer22;

	[SerializeField]
	private float layer23;

	[SerializeField]
	private float layer24;

	[SerializeField]
	private float layer25;

	[SerializeField]
	private float layer26;

	[SerializeField]
	private float layer27;

	[SerializeField]
	private float layer28;

	[SerializeField]
	private float layer29;

	[SerializeField]
	private float layer30;

	[SerializeField]
	private float layer31;

	[SerializeField]
	private bool spherical;

	[NonSerialized]
	private float layer00_;

	[NonSerialized]
	private float layer01_;

	[NonSerialized]
	private float layer02_;

	[NonSerialized]
	private float layer03_;

	[NonSerialized]
	private float layer04_;

	[NonSerialized]
	private float layer05_;

	[NonSerialized]
	private float layer06_;

	[NonSerialized]
	private float layer07_;

	[NonSerialized]
	private float layer08_;

	[NonSerialized]
	private float layer09_;

	[NonSerialized]
	private float layer10_;

	[NonSerialized]
	private float layer11_;

	[NonSerialized]
	private float layer12_;

	[NonSerialized]
	private float layer13_;

	[NonSerialized]
	private float layer14_;

	[NonSerialized]
	private float layer15_;

	[NonSerialized]
	private float layer16_;

	[NonSerialized]
	private float layer17_;

	[NonSerialized]
	private float layer18_;

	[NonSerialized]
	private float layer19_;

	[NonSerialized]
	private float layer20_;

	[NonSerialized]
	private float layer21_;

	[NonSerialized]
	private float layer22_;

	[NonSerialized]
	private float layer23_;

	[NonSerialized]
	private float layer24_;

	[NonSerialized]
	private float layer25_;

	[NonSerialized]
	private float layer26_;

	[NonSerialized]
	private float layer27_;

	[NonSerialized]
	private float layer28_;

	[NonSerialized]
	private float layer29_;

	[NonSerialized]
	private float layer30_;

	[NonSerialized]
	private float layer31_;

	[NonSerialized]
	private bool spherical_;

	public float this[int layer]
	{
		get
		{
			switch (layer)
			{
				case 0:
				{
					return this.layer00;
				}
				case 1:
				{
					return this.layer01;
				}
				case 2:
				{
					return this.layer02;
				}
				case 3:
				{
					return this.layer03;
				}
				case 4:
				{
					return this.layer04;
				}
				case 5:
				{
					return this.layer05;
				}
				case 6:
				{
					return this.layer06;
				}
				case 7:
				{
					return this.layer07;
				}
				case 8:
				{
					return this.layer08;
				}
				case 9:
				{
					return this.layer09;
				}
				case 10:
				{
					return this.layer10;
				}
				case 11:
				{
					return this.layer11;
				}
				case 12:
				{
					return this.layer12;
				}
				case 13:
				{
					return this.layer13;
				}
				case 14:
				{
					return this.layer14;
				}
				case 15:
				{
					return this.layer15;
				}
				case 16:
				{
					return this.layer16;
				}
				case 17:
				{
					return this.layer17;
				}
				case 18:
				{
					return this.layer18;
				}
				case 19:
				{
					return this.layer19;
				}
				case 20:
				{
					return this.layer20;
				}
				case 21:
				{
					return this.layer21;
				}
				case 22:
				{
					return this.layer22;
				}
				case 23:
				{
					return this.layer23;
				}
				case 24:
				{
					return this.layer24;
				}
				case 25:
				{
					return this.layer25;
				}
				case 26:
				{
					return this.layer26;
				}
				case 27:
				{
					return this.layer27;
				}
				case 28:
				{
					return this.layer28;
				}
				case 29:
				{
					return this.layer29;
				}
				case 30:
				{
					return this.layer30;
				}
				case 31:
				{
					return this.layer31;
				}
			}
			throw new ArgumentOutOfRangeException();
		}
		set
		{
			bool flag;
			switch (layer)
			{
				case 0:
				{
					flag = CameraLayerDepths.Set(ref this.layer00, value);
					break;
				}
				case 1:
				{
					flag = CameraLayerDepths.Set(ref this.layer01, value);
					break;
				}
				case 2:
				{
					flag = CameraLayerDepths.Set(ref this.layer02, value);
					break;
				}
				case 3:
				{
					flag = CameraLayerDepths.Set(ref this.layer03, value);
					break;
				}
				case 4:
				{
					flag = CameraLayerDepths.Set(ref this.layer04, value);
					break;
				}
				case 5:
				{
					flag = CameraLayerDepths.Set(ref this.layer05, value);
					break;
				}
				case 6:
				{
					flag = CameraLayerDepths.Set(ref this.layer06, value);
					break;
				}
				case 7:
				{
					flag = CameraLayerDepths.Set(ref this.layer07, value);
					break;
				}
				case 8:
				{
					flag = CameraLayerDepths.Set(ref this.layer08, value);
					break;
				}
				case 9:
				{
					flag = CameraLayerDepths.Set(ref this.layer09, value);
					break;
				}
				case 10:
				{
					flag = CameraLayerDepths.Set(ref this.layer10, value);
					break;
				}
				case 11:
				{
					flag = CameraLayerDepths.Set(ref this.layer11, value);
					break;
				}
				case 12:
				{
					flag = CameraLayerDepths.Set(ref this.layer12, value);
					break;
				}
				case 13:
				{
					flag = CameraLayerDepths.Set(ref this.layer13, value);
					break;
				}
				case 14:
				{
					flag = CameraLayerDepths.Set(ref this.layer14, value);
					break;
				}
				case 15:
				{
					flag = CameraLayerDepths.Set(ref this.layer15, value);
					break;
				}
				case 16:
				{
					flag = CameraLayerDepths.Set(ref this.layer16, value);
					break;
				}
				case 17:
				{
					flag = CameraLayerDepths.Set(ref this.layer17, value);
					break;
				}
				case 18:
				{
					flag = CameraLayerDepths.Set(ref this.layer18, value);
					break;
				}
				case 19:
				{
					flag = CameraLayerDepths.Set(ref this.layer19, value);
					break;
				}
				case 20:
				{
					flag = CameraLayerDepths.Set(ref this.layer20, value);
					break;
				}
				case 21:
				{
					flag = CameraLayerDepths.Set(ref this.layer21, value);
					break;
				}
				case 22:
				{
					flag = CameraLayerDepths.Set(ref this.layer22, value);
					break;
				}
				case 23:
				{
					flag = CameraLayerDepths.Set(ref this.layer23, value);
					break;
				}
				case 24:
				{
					flag = CameraLayerDepths.Set(ref this.layer24, value);
					break;
				}
				case 25:
				{
					flag = CameraLayerDepths.Set(ref this.layer25, value);
					break;
				}
				case 26:
				{
					flag = CameraLayerDepths.Set(ref this.layer26, value);
					break;
				}
				case 27:
				{
					flag = CameraLayerDepths.Set(ref this.layer27, value);
					break;
				}
				case 28:
				{
					flag = CameraLayerDepths.Set(ref this.layer28, value);
					break;
				}
				case 29:
				{
					flag = CameraLayerDepths.Set(ref this.layer29, value);
					break;
				}
				case 30:
				{
					flag = CameraLayerDepths.Set(ref this.layer30, value);
					break;
				}
				case 31:
				{
					flag = CameraLayerDepths.Set(ref this.layer31, value);
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException();
				}
			}
			if (flag)
			{
				this.Awake();
			}
		}
	}

	public CameraLayerDepths()
	{
	}

	private void Awake()
	{
		this.layer00_ = this.layer00;
		this.layer01_ = this.layer01;
		this.layer02_ = this.layer02;
		this.layer03_ = this.layer03;
		this.layer04_ = this.layer04;
		this.layer05_ = this.layer05;
		this.layer06_ = this.layer06;
		this.layer07_ = this.layer07;
		this.layer08_ = this.layer08;
		this.layer09_ = this.layer09;
		this.layer10_ = this.layer10;
		this.layer11_ = this.layer11;
		this.layer12_ = this.layer12;
		this.layer13_ = this.layer13;
		this.layer14_ = this.layer14;
		this.layer15_ = this.layer15;
		this.layer16_ = this.layer16;
		this.layer17_ = this.layer17;
		this.layer18_ = this.layer18;
		this.layer19_ = this.layer19;
		this.layer20_ = this.layer20;
		this.layer21_ = this.layer21;
		this.layer22_ = this.layer22;
		this.layer23_ = this.layer23;
		this.layer24_ = this.layer24;
		this.layer25_ = this.layer25;
		this.layer26_ = this.layer26;
		this.layer27_ = this.layer27;
		this.layer28_ = this.layer28;
		this.layer29_ = this.layer29;
		this.layer30_ = this.layer30;
		this.layer31_ = this.layer31;
		float[] singleArray = new float[] { this.layer00, this.layer01, this.layer02, this.layer03, this.layer04, this.layer05, this.layer06, this.layer07, this.layer08, this.layer09, this.layer10, this.layer11, this.layer12, this.layer13, this.layer14, this.layer15, this.layer16, this.layer17, this.layer18, this.layer19, this.layer20, this.layer21, this.layer22, this.layer23, this.layer24, this.layer25, this.layer26, this.layer27, this.layer28, this.layer29, this.layer30, this.layer31 };
		base.camera.layerCullDistances = singleArray;
		base.camera.layerCullSpherical = this.spherical;
	}

	[ContextMenu("Ensure Layer Depths Set")]
	private void EnsureLayerDepthsSet()
	{
		float[] singleArray = base.camera.layerCullDistances;
		if (singleArray == null)
		{
			this.Awake();
		}
		else if ((int)singleArray.Length == 32)
		{
			bool flag = false;
			int num = 0;
			while (num < 32)
			{
				if (singleArray[num] != this[num])
				{
					flag = true;
					this.Awake();
					break;
				}
				else
				{
					num++;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		else
		{
			this.Awake();
		}
		if (this.spherical == base.camera.layerCullSpherical)
		{
			return;
		}
		this.Awake();
		Debug.Log("Layer Depths Were Not Set", this);
	}

	private void OnPreCull()
	{
		if (this.spherical != this.spherical_ || this.layer00 != this.layer00_ || this.layer01 != this.layer01_ || this.layer02 != this.layer02_ || this.layer03 != this.layer03_ || this.layer04 != this.layer04_ || this.layer05 != this.layer05_ || this.layer06 != this.layer06_ || this.layer07 != this.layer07_ || this.layer08 != this.layer08_ || this.layer09 != this.layer09_ || this.layer10 != this.layer10_ || this.layer11 != this.layer11_ || this.layer12 != this.layer12_ || this.layer13 != this.layer13_ || this.layer14 != this.layer14_ || this.layer15 != this.layer15_ || this.layer16 != this.layer16_ || this.layer17 != this.layer17_ || this.layer18 != this.layer18_ || this.layer19 != this.layer19_ || this.layer20 != this.layer20_ || this.layer21 != this.layer21_ || this.layer22 != this.layer22_ || this.layer23 != this.layer23_ || this.layer24 != this.layer24_ || this.layer25 != this.layer25_ || this.layer26 != this.layer26_ || this.layer27 != this.layer27_ || this.layer28 != this.layer28_ || this.layer29 != this.layer29_ || this.layer30 != this.layer30_ || this.layer31 != this.layer31_)
		{
			this.Awake();
		}
	}

	private static bool Set(ref float m, float v)
	{
		if (m == v)
		{
			return false;
		}
		m = v;
		return true;
	}
}