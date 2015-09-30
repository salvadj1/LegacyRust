using System;
using UnityEngine;

public class CCDispatchTest : MonoBehaviour
{
	[SerializeField]
	private CCTotemPole totemPole;

	[SerializeField]
	private Camera fpsCam;

	public float crouchHeight = 1.3f;

	public float standingHeight = 2f;

	public float crouchSmoothing = 0.02f;

	public float crouchMaxSpeed = 5f;

	public float horizonalScalar = 4f;

	public float downwardSpeed = 10f;

	[NonSerialized]
	private float crouchVelocity;

	[NonSerialized]
	private float currentHeight;

	public CCDispatchTest()
	{
	}

	private void Awake()
	{
		this.currentHeight = this.crouchHeight;
		this.totemPole.OnBindPosition += new CCTotem.PositionBinder(this.BindPositions);
	}

	private void BindPositions(ref CCTotem.PositionPlacement placement, object Tag)
	{
		base.transform.position = placement.bottom;
		this.fpsCam.transform.position = placement.top - new Vector3(0f, 0.25f, 0f);
	}

	private void OnDestroy()
	{
		if (this.totemPole)
		{
			this.totemPole.OnBindPosition -= new CCTotem.PositionBinder(this.BindPositions);
		}
	}

	private void Update()
	{
		float single;
		float single1 = Time.deltaTime;
		Vector3 vector3 = new Vector3(Input.GetAxis("Horizontal") * this.horizonalScalar, -this.downwardSpeed, Input.GetAxis("Vertical") * this.horizonalScalar);
		float single2 = vector3.x * vector3.x + vector3.z * vector3.z;
		single = (single2 <= this.horizonalScalar * this.horizonalScalar ? single1 : this.horizonalScalar / Mathf.Sqrt(single2) * single1);
		vector3.x = vector3.x * single;
		vector3.z = vector3.z * single;
		vector3.y = vector3.y * single1;
		float single3 = (!Input.GetButton("Crouch") ? this.standingHeight : this.crouchHeight);
		this.currentHeight = Mathf.SmoothDamp(this.currentHeight, single3, ref this.crouchVelocity, this.crouchSmoothing, this.crouchMaxSpeed, single1);
		this.totemPole.Move(vector3, this.currentHeight);
	}
}