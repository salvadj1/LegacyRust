using Facepunch.MeshBatch;
using Facepunch.MeshBatch.Extensions;
using Facepunch.MeshBatch.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExplosionHelper
{
	private const float kMaxZero = 1E-05f;

	public static ExplosionHelper.Surface[] OverlapExplosion(Vector3 point, float explosionRadius, int findLayerMask = -1, int occludingLayerMask = -1, IDMain ignore = null)
	{
		ExplosionHelper.Point points = new ExplosionHelper.Point(point, explosionRadius, findLayerMask, occludingLayerMask, ignore);
		return points.ToArray();
	}

	public static ExplosionHelper.Surface[] OverlapExplosionSorted(Vector3 point, float explosionRadius, int findLayerMask = -1, int occludingLayerMask = -1, IDMain ignore = null)
	{
		ExplosionHelper.Surface[] surfaceArray = ExplosionHelper.OverlapExplosion(point, explosionRadius, findLayerMask, occludingLayerMask, ignore);
		if ((int)surfaceArray.Length > 1)
		{
			Array.Sort<ExplosionHelper.Surface>(surfaceArray);
		}
		return surfaceArray;
	}

	public static ExplosionHelper.Surface[] OverlapExplosionUnique(Vector3 point, float explosionRadius, int findLayerMask = -1, int occludingLayerMask = -1, IDMain ignore = null)
	{
		ExplosionHelper.Surface[] surfaceArray = ExplosionHelper.OverlapExplosion(point, explosionRadius, findLayerMask, occludingLayerMask, ignore);
		int length = (int)surfaceArray.Length;
		if (length > 1)
		{
			Array.Sort<ExplosionHelper.Surface>(surfaceArray);
			if (ExplosionHelper.Unique.Filter(surfaceArray, ref length))
			{
				Array.Resize<ExplosionHelper.Surface>(ref surfaceArray, length);
			}
		}
		return surfaceArray;
	}

	public struct Point : IEnumerable, IEnumerable<ExplosionHelper.Surface>
	{
		public readonly Vector3 point;

		public readonly float blastRadius;

		public readonly int overlapLayerMask;

		public readonly int raycastLayerMask;

		public readonly IDMain skip;

		public Point(Vector3 point, float blastRadius, int overlapLayerMask, int raycastLayerMask, IDMain skip)
		{
			this.point = point;
			this.blastRadius = blastRadius;
			this.overlapLayerMask = overlapLayerMask;
			this.raycastLayerMask = raycastLayerMask;
			this.skip = skip;
		}

		private bool BoundsWork(ref Bounds bounds, ref ExplosionHelper.Work w)
		{
			float single;
			w.boundsSquareDistance = bounds.SqrDistance(this.point);
			if (w.boundsSquareDistance > this.blastRadius * this.blastRadius)
			{
				return false;
			}
			if (w.boundsSquareDistance <= 1E-05f)
			{
				w.boundsSquareDistance = 0f;
			}
			w.center = bounds.center;
			float single1 = w.center.x;
			Vector3 vector3 = this.point;
			w.rayDir.x = single1 - vector3.x;
			float single2 = w.center.y;
			Vector3 vector31 = this.point;
			w.rayDir.y = single2 - vector31.y;
			float single3 = w.center.z;
			Vector3 vector32 = this.point;
			w.rayDir.z = single3 - vector32.z;
			w.squareDistanceToCenter = w.rayDir.x * w.rayDir.x + w.rayDir.y * w.rayDir.y + w.rayDir.z * w.rayDir.z;
			if (w.squareDistanceToCenter > this.blastRadius * this.blastRadius)
			{
				return false;
			}
			if (w.squareDistanceToCenter <= 9.99999944E-11f)
			{
				float single4 = 0f;
				single = single4;
				w.squareDistanceToCenter = single4;
				w.distanceToCenter = single;
				float single5 = 0f;
				single = single5;
				w.squareRayDistance = single5;
				w.rayDistance = single;
				w.rayTest = false;
				w.boundsExtent = bounds.size;
				w.boundsExtent.x = w.boundsExtent.x * 0.5f;
				w.boundsExtent.y = w.boundsExtent.y * 0.5f;
				w.boundsExtent.z = w.boundsExtent.z * 0.5f;
				w.boundsExtentSquareMagnitude = w.boundsExtent.x * w.boundsExtent.x + w.boundsExtent.y * w.boundsExtent.y + w.boundsExtent.z * w.boundsExtent.z;
				return true;
			}
			w.distanceToCenter = Mathf.Sqrt(w.squareDistanceToCenter);
			w.boundsExtent = bounds.size;
			w.boundsExtent.x = w.boundsExtent.x * 0.5f;
			w.boundsExtent.y = w.boundsExtent.y * 0.5f;
			w.boundsExtent.z = w.boundsExtent.z * 0.5f;
			w.boundsExtentSquareMagnitude = w.boundsExtent.x * w.boundsExtent.x + w.boundsExtent.y * w.boundsExtent.y + w.boundsExtent.z * w.boundsExtent.z;
			w.squareRayDistance = w.boundsSquareDistance + w.boundsExtentSquareMagnitude;
			if (w.squareRayDistance <= w.squareDistanceToCenter)
			{
				if (w.squareRayDistance <= 9.99999944E-11f)
				{
					float single6 = 0f;
					single = single6;
					w.squareRayDistance = single6;
					w.rayDistance = single;
					w.rayTest = false;
					return true;
				}
				w.rayDistance = Mathf.Sqrt(w.squareRayDistance);
			}
			else
			{
				w.squareRayDistance = w.squareDistanceToCenter;
				w.rayDistance = w.distanceToCenter;
			}
			w.rayTest = true;
			return true;
		}

		public ExplosionHelper.Point.Enumerator GetEnumerator()
		{
			return new ExplosionHelper.Point.Enumerator(this, false);
		}

		private bool SurfaceForCollider(Collider collider, ref ExplosionHelper.Surface surface)
		{
			RaycastHit raycastHit;
			if (!collider.enabled)
			{
				surface = new ExplosionHelper.Surface();
				return false;
			}
			surface.idBase = IDBase.Get(collider);
			if (!surface.idBase)
			{
				surface = new ExplosionHelper.Surface();
				return false;
			}
			surface.idMain = surface.idBase.idMain;
			if (!surface.idMain || surface.idMain == this.skip)
			{
				surface = new ExplosionHelper.Surface();
				return false;
			}
			surface.bounds = collider.bounds;
			if (!this.BoundsWork(ref surface.bounds, ref surface.work))
			{
				return false;
			}
			if (this.raycastLayerMask != 0)
			{
				surface.blocked = (!surface.work.rayTest || !collider.Raycast(new Ray(this.point, surface.work.rayDir), out raycastHit, surface.work.rayDistance) || !Physics.Raycast(this.point, surface.work.rayDir, out raycastHit, raycastHit.distance, this.raycastLayerMask) ? false : raycastHit.collider != collider);
			}
			return true;
		}

		private bool SurfaceForMeshBatchInstance(MeshBatchInstance instance, ref ExplosionHelper.Surface surface)
		{
			bool flag;
			MeshBatchInstance meshBatchInstance;
			surface.idBase = instance;
			surface.idMain = surface.idBase.idMain;
			if (!surface.idMain || surface.idMain == this.skip)
			{
				surface = new ExplosionHelper.Surface();
				return false;
			}
			surface.bounds = instance.physicalBounds;
			if (!this.BoundsWork(ref surface.bounds, ref surface.work))
			{
				surface = new ExplosionHelper.Surface();
				return false;
			}
			if (!surface.work.rayTest)
			{
				surface.blocked = false;
			}
			else if (this.raycastLayerMask == 0 || !Facepunch.MeshBatch.MeshBatchPhysics.Raycast(this.point, surface.work.rayDir, surface.work.rayDistance, this.raycastLayerMask, out flag, out meshBatchInstance))
			{
				surface.blocked = false;
			}
			else if (!flag || !(meshBatchInstance == instance))
			{
				surface.blocked = true;
			}
			else
			{
				surface.blocked = false;
			}
			return true;
		}

		IEnumerator<ExplosionHelper.Surface> System.Collections.Generic.IEnumerable<ExplosionHelper.Surface>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public ExplosionHelper.Surface[] ToArray()
		{
			ExplosionHelper.Point.Enumerator enumerator = new ExplosionHelper.Point.Enumerator(this, true);
			return ExplosionHelper.Point.EnumeratorToArray.Build(ref enumerator);
		}

		public struct Enumerator : IDisposable, IEnumerator, IEnumerator<ExplosionHelper.Surface>
		{
			private readonly ExplosionHelper.Point IN;

			private int colliderIndex;

			private bool inInstanceEnumerator;

			private Facepunch.MeshBatch.MeshBatchPhysicalOutput output;

			private IEnumerator<MeshBatchInstance> overlapEnumerator;

			private Collider[] overlap;

			public ExplosionHelper.Surface current;

			private readonly bool _immediate;

			public ExplosionHelper.Surface Current
			{
				get
				{
					return this.current;
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return this.current;
				}
			}

			internal Enumerator(ref ExplosionHelper.Point point, bool immediate)
			{
				this._immediate = immediate;
				this.IN = point;
				this.colliderIndex = -1;
				this.inInstanceEnumerator = false;
				this.overlapEnumerator = null;
				this.output = null;
				this.overlap = null;
				this.current = new ExplosionHelper.Surface();
			}

			public void Dispose()
			{
				this.colliderIndex = -1;
				if (this.inInstanceEnumerator)
				{
					this.inInstanceEnumerator = false;
					this.overlapEnumerator.Dispose();
				}
				this.overlapEnumerator = null;
				this.output = null;
				this.overlap = null;
				this.current = new ExplosionHelper.Surface();
			}

			public bool MoveNext()
			{
			Label0:
				while (this.inInstanceEnumerator)
				{
					if ((this._immediate || this.output) && this.overlapEnumerator.MoveNext())
					{
						MeshBatchInstance current = this.overlapEnumerator.Current;
						if (this.IN.SurfaceForMeshBatchInstance(current, ref this.current))
						{
							return true;
						}
					}
					else
					{
						this.overlapEnumerator.Dispose();
						this.overlapEnumerator = null;
						this.inInstanceEnumerator = false;
						this.output = null;
					}
				}
				ExplosionHelper.Point.Enumerator enumerator = this;
				int num = enumerator.colliderIndex;
				int num1 = num;
				enumerator.colliderIndex = num + 1;
				if (num1 == -1)
				{
					Vector3 n = this.IN.point;
					float single = this.IN.blastRadius;
					ExplosionHelper.Point points = this.IN;
					this.overlap = Physics.OverlapSphere(n, single, points.overlapLayerMask);
				}
				while (this.colliderIndex < (int)this.overlap.Length)
				{
					if (this._immediate || this.overlap[this.colliderIndex])
					{
						if (this.overlap[this.colliderIndex].GetMeshBatchPhysicalOutput<Facepunch.MeshBatch.MeshBatchPhysicalOutput>(out this.output))
						{
							this.inInstanceEnumerator = true;
							Facepunch.MeshBatch.MeshBatchPhysicalOutput meshBatchPhysicalOutput = this.output;
							Vector3 vector3 = this.IN.point;
							ExplosionHelper.Point n1 = this.IN;
							this.overlapEnumerator = meshBatchPhysicalOutput.EnumerateOverlapSphereInstances(vector3, n1.blastRadius).GetEnumerator();
							goto Label0;
						}
						else if (this.IN.SurfaceForCollider(this.overlap[this.colliderIndex], ref this.current))
						{
							return true;
						}
					}
					ExplosionHelper.Point.Enumerator enumerator1 = this;
					enumerator1.colliderIndex = enumerator1.colliderIndex + 1;
				}
				this.colliderIndex = (int)this.overlap.Length;
				this.current = new ExplosionHelper.Surface();
				return false;
			}

			public void Reset()
			{
				this.Dispose();
			}
		}

		private struct EnumeratorToArray
		{
			private ExplosionHelper.Point.Enumerator enumerator;

			private ExplosionHelper.Surface[] array;

			private int length;

			public static ExplosionHelper.Surface[] Build(ref ExplosionHelper.Point.Enumerator point_enumerator)
			{
				ExplosionHelper.Point.EnumeratorToArray pointEnumerator = new ExplosionHelper.Point.EnumeratorToArray();
				pointEnumerator.enumerator = point_enumerator;
				pointEnumerator.length = 0;
				pointEnumerator.array = null;
				pointEnumerator.RecurseInStackHeapToArray();
				return pointEnumerator.array;
			}

			private void RecurseInStackHeapToArray()
			{
				if (!this.enumerator.MoveNext())
				{
					this.array = new ExplosionHelper.Surface[this.length];
				}
				else
				{
					ExplosionHelper.Surface surface = this.enumerator.current;
					ExplosionHelper.Point.EnumeratorToArray enumeratorToArray = this;
					enumeratorToArray.length = enumeratorToArray.length + 1;
					this.RecurseInStackHeapToArray();
					ExplosionHelper.Point.EnumeratorToArray enumeratorToArray1 = this;
					int num = enumeratorToArray1.length - 1;
					int num1 = num;
					enumeratorToArray1.length = num;
					this.array[num1] = surface;
				}
			}
		}
	}

	public struct Surface : IEquatable<ExplosionHelper.Surface>, IComparable<ExplosionHelper.Surface>
	{
		public IDBase idBase;

		public IDMain idMain;

		public Bounds bounds;

		public ExplosionHelper.Work work;

		public bool blocked;

		public int CompareTo(ExplosionHelper.Surface other)
		{
			int num = this.blocked.CompareTo(other.blocked);
			if (num == 0)
			{
				num = this.work.distanceToCenter.CompareTo(other.work.distanceToCenter);
				if (num == 0)
				{
					num = this.work.boundsSquareDistance.CompareTo(other.work.squareDistanceToCenter);
					if (num == 0)
					{
						num = this.work.rayDistance.CompareTo(other.work.rayDistance);
					}
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			return (!(obj is ExplosionHelper.Surface) ? false : this.Equals((ExplosionHelper.Surface)obj));
		}

		public bool Equals(ExplosionHelper.Surface surface)
		{
			return (this.blocked != surface.blocked || !(this.bounds == surface.bounds) || !(this.idBase == surface.idBase) || !(this.idMain == surface.idMain) ? false : this.work.Equals(ref surface.work));
		}

		public bool Equals(ref ExplosionHelper.Surface surface)
		{
			return (this.blocked != surface.blocked || !(this.bounds == surface.bounds) || !(this.idBase == surface.idBase) || !(this.idMain == surface.idMain) ? false : this.work.Equals(ref surface.work));
		}

		public override int GetHashCode()
		{
			return this.bounds.GetHashCode() ^ (!this.idBase ? 0 : this.idBase.GetHashCode());
		}

		public override string ToString()
		{
			return "Surface";
		}
	}

	private static class Unique
	{
		private readonly static HashSet<IDMain> Set;

		static Unique()
		{
			ExplosionHelper.Unique.Set = new HashSet<IDMain>();
		}

		public static bool Filter(ExplosionHelper.Surface[] array, ref int length)
		{
			bool flag;
			int num = (int)array.Length;
			try
			{
				int num1 = 0;
				while (num1 < num)
				{
					IDMain dMain = array[num1].idMain;
					if (!dMain || ExplosionHelper.Unique.Set.Add(dMain))
					{
						num1++;
					}
					else
					{
						int num2 = num1;
						while (true)
						{
							int num3 = num1 + 1;
							num1 = num3;
							if (num3 >= num)
							{
								break;
							}
							dMain = array[num1].idMain;
							if (!array[num1].idMain || ExplosionHelper.Unique.Set.Add(dMain))
							{
								int num4 = num2;
								num2 = num4 + 1;
								array[num4] = array[num1];
							}
						}
						length = num2;
						flag = true;
						return flag;
					}
				}
				return false;
			}
			finally
			{
				ExplosionHelper.Unique.Set.Clear();
			}
			return flag;
		}
	}

	public struct Work
	{
		public Vector3 center;

		public Vector3 rayDir;

		public Vector3 boundsExtent;

		public float boundsExtentSquareMagnitude;

		public float boundsSquareDistance;

		public float distanceToCenter;

		public float squareDistanceToCenter;

		public float rayDistance;

		public float squareRayDistance;

		public bool rayTest;

		public bool Equals(ref ExplosionHelper.Work w)
		{
			bool flag;
			bool flag1;
			if (this.squareDistanceToCenter == w.squareDistanceToCenter && this.boundsSquareDistance == w.boundsSquareDistance && this.boundsExtentSquareMagnitude == w.boundsExtentSquareMagnitude && this.distanceToCenter == w.distanceToCenter)
			{
				if (!this.rayTest)
				{
					flag1 = !w.rayTest;
				}
				else
				{
					flag1 = (!w.rayTest || this.squareRayDistance != w.squareRayDistance || this.rayDistance != w.rayDistance ? false : this.rayDir == w.rayDir);
				}
				if (!flag1 || !(this.center == w.center))
				{
					flag = false;
					return flag;
				}
				flag = this.boundsExtent == w.boundsExtent;
				return flag;
			}
			flag = false;
			return flag;
		}
	}
}