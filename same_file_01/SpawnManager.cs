using System;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private const float kRandomizeSpawnRadius = 10f;

	public static SpawnManager.SpawnData[] _spawnPoints;

	public static SpawnManager _spawnMan;

	public SpawnManager()
	{
	}

	public virtual void AddPlayerSpawn(GameObject spawn)
	{
		ServerManagement.Get().AddPlayerSpawn(spawn);
	}

	private void Awake()
	{
		SpawnManager._spawnMan = this;
		this.InstallSpawns();
	}

	public SpawnManager Get()
	{
		return SpawnManager._spawnMan;
	}

	public static void GetCloseSpawn(Vector3 point, out Vector3 pos, out Quaternion rot)
	{
		Vector3 vector3 = new Vector3();
		float single = Single.PositiveInfinity;
		int num = -1;
		for (int i = 0; i < (int)SpawnManager._spawnPoints.Length; i++)
		{
			vector3.x = point.x - SpawnManager._spawnPoints[i].pos.x;
			vector3.y = point.y - SpawnManager._spawnPoints[i].pos.y;
			vector3.z = point.z - SpawnManager._spawnPoints[i].pos.z;
			float single1 = vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z;
			if (single1 < single && single1 > 40f)
			{
				single = single1;
				num = i;
			}
		}
		if (num != -1)
		{
			pos = SpawnManager._spawnPoints[num].pos;
			rot = SpawnManager._spawnPoints[num].rot;
		}
		else
		{
			SpawnManager.GetRandomSpawn(out pos, out rot);
		}
		SpawnManager.RandomizeAndScanSpawnPosition(ref pos);
	}

	public static void GetClosestSpawn(Vector3 point, out Vector3 pos, out Quaternion rot)
	{
		Vector3 vector3 = new Vector3();
		float single = Single.PositiveInfinity;
		int num = -1;
		for (int i = 0; i < (int)SpawnManager._spawnPoints.Length; i++)
		{
			vector3.x = point.x - SpawnManager._spawnPoints[i].pos.x;
			vector3.y = point.y - SpawnManager._spawnPoints[i].pos.y;
			vector3.z = point.z - SpawnManager._spawnPoints[i].pos.z;
			float single1 = vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z;
			if (single1 < single)
			{
				single = single1;
				num = i;
			}
		}
		if (num != -1)
		{
			pos = SpawnManager._spawnPoints[num].pos;
			rot = SpawnManager._spawnPoints[num].rot;
		}
		else
		{
			SpawnManager.GetRandomSpawn(out pos, out rot);
		}
		SpawnManager.RandomizeAndScanSpawnPosition(ref pos);
	}

	public static void GetFarthestSpawn(Vector3 point, out Vector3 pos, out Quaternion rot)
	{
		Vector3 vector3 = new Vector3();
		float single = Single.NegativeInfinity;
		int num = -1;
		for (int i = 0; i < (int)SpawnManager._spawnPoints.Length; i++)
		{
			vector3.x = point.x - SpawnManager._spawnPoints[i].pos.x;
			vector3.y = point.y - SpawnManager._spawnPoints[i].pos.y;
			vector3.z = point.z - SpawnManager._spawnPoints[i].pos.z;
			float single1 = vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z;
			if (single1 > single)
			{
				single = single1;
				num = i;
			}
		}
		if (num != -1)
		{
			pos = SpawnManager._spawnPoints[num].pos;
			rot = SpawnManager._spawnPoints[num].rot;
		}
		else
		{
			SpawnManager.GetRandomSpawn(out pos, out rot);
		}
		SpawnManager.RandomizeAndScanSpawnPosition(ref pos);
	}

	public static void GetRandomSpawn(out Vector3 pos, out Quaternion rot)
	{
		int num = UnityEngine.Random.Range(0, (int)SpawnManager._spawnPoints.Length);
		pos = SpawnManager._spawnPoints[num].pos;
		rot = SpawnManager._spawnPoints[num].rot;
		SpawnManager.RandomizeAndScanSpawnPosition(ref pos);
	}

	private void InstallSpawns()
	{
		SpawnManager._spawnPoints = new SpawnManager.SpawnData[base.transform.childCount];
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			SpawnManager._spawnPoints[i].pos = child.position;
			SpawnManager._spawnPoints[i].rot = child.rotation;
		}
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UnityEngine.Object.Destroy(((Transform)enumerator.Current).gameObject);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public static bool RandomizeAndScanSpawnPosition(ref Vector3 pos)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		RaycastHit raycastHit;
		Vector2 vector2 = UnityEngine.Random.insideUnitCircle * 10f;
		float single = pos.x + vector2.x;
		float single1 = single;
		vector3.x = single;
		vector31.x = single1;
		vector3.y = pos.y + 2000f;
		vector31.y = pos.y - 500f;
		float single2 = pos.z + vector2.y;
		single1 = single2;
		vector3.z = single2;
		vector31.z = single1;
		if (!Physics.Linecast(vector3, vector31, out raycastHit, 525313))
		{
			return false;
		}
		pos = raycastHit.point;
		float single3 = pos.y;
		Vector3 vector32 = raycastHit.normal;
		pos.y = single3 + vector32.y * 0.25f;
		return true;
	}

	public virtual void RemovePlayerSpawn(GameObject spawn)
	{
		ServerManagement serverManagement = ServerManagement.Get();
		if (serverManagement)
		{
			serverManagement.RemovePlayerSpawn(spawn);
		}
	}

	private void Update()
	{
	}

	public struct SpawnData
	{
		public Vector3 pos;

		public Quaternion rot;
	}
}