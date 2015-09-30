using System;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
	public bool impacted;

	public float speedPerSec = 80f;

	public float maxRange = 1000f;

	private float maxLifeTime = 4f;

	public float lastUpdateTime;

	public float spawnTime;

	private int layerMask = 406721553;

	private float distance;

	public float dropDegreesPerSec = 5f;

	private bool reported;

	public ItemRepresentation _myBow;

	public IBowWeaponItem _myItemInstance;

	public ArrowMovement()
	{
	}

	public void Init(float arrowSpeed, ItemRepresentation itemRep, IBowWeaponItem itemInstance, bool firedLocal)
	{
		this.speedPerSec = arrowSpeed;
		if (itemRep != null && itemInstance != null)
		{
			this._myBow = itemRep;
			this._myItemInstance = itemInstance;
		}
	}

	private void OnDestroy()
	{
		if (!this.impacted)
		{
			this.TryReportMiss();
		}
	}

	private void Start()
	{
		this.spawnTime = Time.time;
		this.lastUpdateTime = Time.time;
	}

	public void TryReportHit(GameObject hitGameObject)
	{
		if (this._myItemInstance != null && !this.reported)
		{
			this.reported = true;
			IDMain main = IDBase.GetMain(hitGameObject);
			this._myItemInstance.ArrowReportHit(main, this);
		}
	}

	public void TryReportMiss()
	{
		if (this._myItemInstance != null && !this.reported)
		{
			this.reported = true;
			this._myItemInstance.ArrowReportMiss(this);
		}
	}

	private void Update()
	{
		Vector3 vector3;
		Vector3 vector31;
		GameObject gameObject;
		Rigidbody rigidbody;
		if (this.impacted)
		{
			return;
		}
		float single = Time.time - this.lastUpdateTime;
		this.lastUpdateTime = Time.time;
		RaycastHit raycastHit = new RaycastHit();
		RaycastHit2 raycastHit2 = RaycastHit2.invalid;
		base.transform.Rotate(Vector3.right, this.dropDegreesPerSec * single);
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		bool flag = true;
		if (!Physics2.Raycast2(ray, out raycastHit2, this.speedPerSec * single, this.layerMask))
		{
			Transform transforms = base.transform;
			transforms.position = transforms.position + ((base.transform.forward * this.speedPerSec) * single);
			ArrowMovement arrowMovement = this;
			arrowMovement.distance = arrowMovement.distance + this.speedPerSec * single;
		}
		else
		{
			if (!flag)
			{
				vector31 = raycastHit.normal;
				vector3 = raycastHit.point;
				gameObject = raycastHit.collider.gameObject;
				rigidbody = raycastHit.rigidbody;
			}
			else
			{
				vector31 = raycastHit2.normal;
				vector3 = raycastHit2.point;
				gameObject = raycastHit2.gameObject;
				rigidbody = raycastHit2.rigidbody;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector31);
			Vector3 vector32 = Vector3.zero;
			int num = gameObject.layer;
			bool flag1 = true;
			if (rigidbody && !rigidbody.isKinematic && !rigidbody.CompareTag("Door"))
			{
				rigidbody.AddForceAtPosition(Vector3.up * 200f, vector3);
				rigidbody.AddForceAtPosition(ray.direction * 1000f, vector3);
			}
			if (num == 17 || num == 18 || num == 27 || num == 21)
			{
				flag1 = false;
			}
			else
			{
				vector32 = vector3 + (vector31 * 0.01f);
			}
			this.impacted = true;
			base.transform.position = vector3;
			this.TryReportHit(gameObject);
			base.transform.parent = gameObject.transform;
			TrailRenderer component = base.GetComponent<TrailRenderer>();
			if (component)
			{
				component.enabled = false;
			}
			base.audio.enabled = false;
			if (gameObject)
			{
				SurfaceInfoObject surfaceInfoFor = SurfaceInfo.GetSurfaceInfoFor(gameObject, vector3);
				surfaceInfoFor.GetImpactEffect(SurfaceInfoObject.ImpactType.Bullet);
				UnityEngine.Object obj = UnityEngine.Object.Instantiate(surfaceInfoFor.GetImpactEffect(SurfaceInfoObject.ImpactType.Bullet), vector3, quaternion);
				UnityEngine.Object.Destroy(obj, 1.5f);
				this.TryReportMiss();
			}
			UnityEngine.Object.Destroy(base.gameObject, 20f);
		}
		if (this.distance > this.maxRange || Time.time - this.spawnTime > this.maxLifeTime)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}