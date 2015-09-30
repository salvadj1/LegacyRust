using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TOD_Animation : MonoBehaviour
{
	public float WindDegrees;

	public float WindSpeed = 3f;

	private TOD_Sky sky;

	internal Vector4 CloudUV
	{
		get;
		set;
	}

	internal Vector4 OffsetUV
	{
		get
		{
			Vector3 vector3 = base.transform.position;
			Vector3 vector31 = base.transform.lossyScale;
			Vector3 vector32 = new Vector3(vector3.x / vector31.x, 0f, vector3.z / vector31.z);
			vector32 = -base.transform.TransformDirection(vector32);
			return new Vector4(vector32.x, vector32.z, vector32.x, vector32.z);
		}
	}

	public TOD_Animation()
	{
	}

	protected void Start()
	{
		this.sky = base.GetComponent<TOD_Sky>();
	}

	protected void Update()
	{
		Vector2 vector2 = new Vector2(Mathf.Cos(0.0174532924f * (this.WindDegrees + 15f)), Mathf.Sin(0.0174532924f * (this.WindDegrees + 15f)));
		Vector2 vector21 = new Vector2(Mathf.Cos(0.0174532924f * (this.WindDegrees - 15f)), Mathf.Sin(0.0174532924f * (this.WindDegrees - 15f)));
		Vector4 windSpeed = this.WindSpeed / 100f * new Vector4(vector2.x, vector2.y, vector21.x, vector21.y);
		TOD_Animation cloudUV = this;
		cloudUV.CloudUV = cloudUV.CloudUV + (Time.deltaTime * windSpeed);
		Vector4 vector4 = this.CloudUV;
		float scale1 = vector4.x % this.sky.Clouds.Scale1.x;
		Vector4 cloudUV1 = this.CloudUV;
		float single = cloudUV1.y % this.sky.Clouds.Scale1.y;
		Vector4 vector41 = this.CloudUV;
		float scale2 = vector41.z % this.sky.Clouds.Scale2.x;
		Vector4 cloudUV2 = this.CloudUV;
		this.CloudUV = new Vector4(scale1, single, scale2, cloudUV2.w % this.sky.Clouds.Scale2.y);
	}
}