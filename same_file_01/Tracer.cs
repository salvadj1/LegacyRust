using System;
using UnityEngine;

public class Tracer : MonoBehaviour
{
	public float speedPerSec;

	public float lastUpdateTime;

	public GameObject impactPrefab;

	public GameObject fleshImpactPrefab;

	public GameObject decalPrefab;

	public GameObject bloodDecalPrefab;

	public GameObject myMesh;

	public Vector3 startScale;

	public float distance;

	public float startTime;

	public float fadeDistStart = 0.15f;

	public float fadeDistLength = 0.25f;

	public AudioClip[] impactSounds;

	public AudioClip[] bodyImpactSounds;

	private Collider colliderToHit;

	private bool thereIsACollider;

	private bool thereIsABodyPart;

	private bool allowBlood;

	private int layerMask = 406721553;

	private float maxRange = 800f;

	public Tracer()
	{
	}

	private void Awake()
	{
		this.startTime = Time.time;
		float single = UnityEngine.Random.Range(0.75f, 1f);
		Vector3 vector3 = base.transform.localScale;
		Vector3 vector31 = base.transform.localScale;
		Vector3 vector32 = base.transform.localScale;
		this.startScale = new Vector3(vector3.x * single, vector31.y * single, vector32.z * UnityEngine.Random.Range(0.5f, 1f));
		base.transform.localScale = new Vector3(0f, 0f, this.startScale.z);
	}

	public void Init(Component component, int layerMask, float range, bool allowBlood)
	{
		Collider collider;
		this.layerMask = layerMask;
		if (!(component is Collider))
		{
			collider = null;
		}
		else
		{
			collider = (Collider)component;
		}
		this.colliderToHit = collider;
		this.thereIsACollider = base.collider;
		this.maxRange = range;
		this.allowBlood = allowBlood;
	}

	private void Start()
	{
		this.lastUpdateTime = Time.time;
	}

	private void Update()
	{
		Vector3 vector3;
		Vector3 vector31;
		GameObject gameObject;
		Rigidbody rigidbody;
		float single = Time.time - this.lastUpdateTime;
		this.lastUpdateTime = Time.time;
		if (this.distance > this.fadeDistStart)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.startScale, Mathf.Clamp((this.distance - this.fadeDistStart) / this.fadeDistLength, 0f, 1f));
		}
		RaycastHit raycastHit = new RaycastHit();
		RaycastHit2 raycastHit2 = RaycastHit2.invalid;
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		float single1 = this.speedPerSec * single;
		bool flag = (!this.thereIsACollider || !this.colliderToHit ? 0 : (int)this.colliderToHit.enabled) == 0;
		if ((!flag ? !this.colliderToHit.Raycast(ray, out raycastHit, single1) : !Physics2.Raycast2(ray, out raycastHit2, this.speedPerSec * single, this.layerMask)))
		{
			Transform transforms = base.transform;
			transforms.position = transforms.position + ((base.transform.forward * this.speedPerSec) * single);
			Tracer tracer = this;
			tracer.distance = tracer.distance + this.speedPerSec * single;
		}
		else
		{
			if (Vector3.Distance(Camera.main.transform.position, base.transform.position) > 75f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
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
			int num = gameObject.layer;
			GameObject gameObject1 = this.impactPrefab;
			bool flag1 = true;
			if (rigidbody && !rigidbody.isKinematic && !rigidbody.CompareTag("Door"))
			{
				rigidbody.AddForceAtPosition(Vector3.up * 200f, vector3);
				rigidbody.AddForceAtPosition(ray.direction * 1000f, vector3);
			}
			SurfaceInfo.DoImpact(gameObject, SurfaceInfoObject.ImpactType.Bullet, vector3 + (vector31 * 0.01f), quaternion);
			if (num == 17 || num == 18 || num == 27 || num == 21)
			{
				flag1 = false;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			if (flag1)
			{
				this.impactSounds[UnityEngine.Random.Range(0, (int)this.impactSounds.Length)].Play(vector3, 1f, 2f, 15f, 180);
				GameObject gameObject2 = UnityEngine.Object.Instantiate(this.decalPrefab, vector3 + (vector31 * UnityEngine.Random.Range(0.01f, 0.03f)), quaternion * Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(-30, 30))) as GameObject;
				if (gameObject)
				{
					gameObject2.transform.parent = gameObject.transform;
				}
				UnityEngine.Object.Destroy(gameObject2, 15f);
			}
		}
		if (this.distance > this.maxRange)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}