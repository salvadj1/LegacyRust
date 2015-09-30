using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using uLink;
using UnityEngine;

public class CullGrid : UnityEngine.MonoBehaviour
{
	private static bool cull_prebinding;

	[SerializeField]
	private CullGridSetup setup;

	private static CullGrid.CullGridRuntime grid;

	private static bool has_grid;

	public static bool autoPrebindInInstantiate
	{
		get
		{
			return (!CullGrid.has_grid ? false : CullGrid.cull_prebinding);
		}
	}

	public static int Tall
	{
		get
		{
			return CullGrid.grid.cellsTall;
		}
	}

	public static int Wide
	{
		get
		{
			return CullGrid.grid.cellsWide;
		}
	}

	static CullGrid()
	{
		CullGrid.cull_prebinding = true;
	}

	public CullGrid()
	{
	}

	private void Awake()
	{
		CullGrid.RegisterGrid(this);
	}

	public static bool CellContainsPoint(ushort cell, ref Vector2 flatPoint)
	{
		return cell == CullGrid.grid.FlatCell(ref flatPoint);
	}

	public static bool CellContainsPoint(ushort cell, ref Vector2 flatPoint, out ushort cell_point)
	{
		cell_point = CullGrid.grid.FlatCell(ref flatPoint);
		return cell == cell_point;
	}

	public static bool CellContainsPoint(ushort cell, ref Vector3 worldPoint)
	{
		return cell == CullGrid.grid.WorldCell(ref worldPoint);
	}

	public static bool CellContainsPoint(ushort cell, ref Vector3 worldPoint, out ushort cell_point)
	{
		cell_point = CullGrid.grid.WorldCell(ref worldPoint);
		return cell_point == cell;
	}

	public static ushort CellFromGroupID(int groupID)
	{
		if (groupID < CullGrid.grid.groupBegin || groupID >= CullGrid.grid.groupEnd)
		{
			throw new ArgumentOutOfRangeException("groupID", (object)groupID, "groupID < grid.groupBegin || groupID >= grid.groupEnd");
		}
		return (ushort)(groupID - CullGrid.grid.groupBegin);
	}

	public static ushort CellFromGroupID(int groupID, out ushort x, out ushort y)
	{
		ushort num = CullGrid.CellFromGroupID(groupID);
		x = (ushort)(num % CullGrid.grid.cellsWide);
		y = (ushort)(num / CullGrid.grid.cellsWide);
		return num;
	}

	private void DrawGizmosNow()
	{
	}

	private void DrawGrid(int cell)
	{
		if (cell != -1)
		{
			this.DrawGrid(this.GetCenterSetup(cell));
		}
	}

	private void DrawGrid(int centerCell, int xOffset, int yOffset)
	{
		this.DrawGrid(centerCell + xOffset + this.setup.cellsWide * 2 * yOffset);
	}

	private void DrawGrid(Vector3 center)
	{
		Vector3 vector3 = base.transform.right * ((float)this.setup.cellSquareDimension / 2f);
		Vector3 vector31 = base.transform.forward * ((float)this.setup.cellSquareDimension / 2f);
		CullGrid.DrawQuadRayCastDown((center + vector3) + vector31, (center + vector3) - vector31, (center - vector3) - vector31, (center - vector3) + vector31);
	}

	private void DrawGrid(Vector3 center, float sizeX, float sizeY)
	{
		Vector3 vector3 = base.transform.right * (sizeX / 2f);
		Vector3 vector31 = base.transform.forward * (sizeY / 2f);
		CullGrid.DrawQuadRayCastDown((center + vector3) + vector31, (center + vector3) - vector31, (center - vector3) - vector31, (center - vector3) + vector31);
	}

	private static void DrawQuadRayCastDown(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		CullGrid.RaycastDownVect(ref a);
		CullGrid.RaycastDownVect(ref b);
		CullGrid.RaycastDownVect(ref c);
		CullGrid.RaycastDownVect(ref d);
		Gizmos.DrawLine(a, b);
		Gizmos.DrawLine(b, c);
		Gizmos.DrawLine(c, d);
		Gizmos.DrawLine(d, a);
		if (a.y > c.y)
		{
			if (b.y > d.y)
			{
				if (b.y - d.y <= a.y - c.y)
				{
					Gizmos.DrawLine(a, c);
				}
				else
				{
					Gizmos.DrawLine(b, d);
				}
			}
			else if (d.y - b.y <= a.y - c.y)
			{
				Gizmos.DrawLine(a, c);
			}
			else
			{
				Gizmos.DrawLine(d, b);
			}
		}
		else if (b.y > d.y)
		{
			if (b.y - d.y <= c.y - a.y)
			{
				Gizmos.DrawLine(c, a);
			}
			else
			{
				Gizmos.DrawLine(b, d);
			}
		}
		else if (d.y - b.y <= c.y - a.y)
		{
			Gizmos.DrawLine(c, a);
		}
		else
		{
			Gizmos.DrawLine(d, b);
		}
	}

	public static Vector2 Flat(Vector3 triD)
	{
		Vector2 vector2 = new Vector2();
		vector2.x = triD.x;
		vector2.y = triD.z;
		return vector2;
	}

	public static ushort FlatCell(Vector2 flat)
	{
		return CullGrid.grid.FlatCell(ref flat);
	}

	public static ushort FlatCell(ref Vector2 flat)
	{
		return CullGrid.grid.FlatCell(ref flat);
	}

	public static int FlatGroupID(ref Vector2 flat)
	{
		return CullGrid.grid.FlatCell(ref flat) + CullGrid.grid.groupBegin;
	}

	private Vector3 GetCenterSetup(int cell)
	{
		CullGridSetup cullGridSetup = this.setup;
		return (base.transform.position + (base.transform.forward * (((float)(cell / cullGridSetup.cellsWide) - ((float)cullGridSetup.cellsTall / 2f - (float)(2 - (cullGridSetup.cellsTall & 1)) / 2f)) * (float)cullGridSetup.cellSquareDimension))) + (base.transform.right * (((float)(cell % cullGridSetup.cellsWide) - ((float)cullGridSetup.cellsWide / 2f - (float)(2 - (cullGridSetup.cellsWide & 1)) / 2f)) * (float)cullGridSetup.cellSquareDimension));
	}

	public static bool GroupIDContainsPoint(int groupID, ref Vector2 flatPoint, out int groupID_point)
	{
		ushort num;
		if (groupID < CullGrid.grid.groupBegin || groupID >= CullGrid.grid.groupEnd)
		{
			groupID_point = NetworkGroup.unassigned.id;
			return false;
		}
		if (CullGrid.CellContainsPoint((ushort)(groupID - CullGrid.grid.groupBegin), ref flatPoint, out num))
		{
			groupID_point = groupID;
			return true;
		}
		groupID_point = num + CullGrid.grid.groupBegin;
		return false;
	}

	public static bool GroupIDContainsPoint(int groupID, ref Vector3 worldPoint, out int groupID_point)
	{
		ushort num;
		if (groupID < CullGrid.grid.groupBegin || groupID >= CullGrid.grid.groupEnd)
		{
			groupID_point = NetworkGroup.unassigned.id;
			return false;
		}
		if (CullGrid.CellContainsPoint(CullGrid.CellFromGroupID(groupID), ref worldPoint, out num))
		{
			groupID_point = groupID;
			return true;
		}
		groupID_point = CullGrid.GroupIDFromCell(num);
		return false;
	}

	public static bool GroupIDContainsPoint(int groupID, ref Vector2 flatPoint)
	{
		return (groupID < CullGrid.grid.groupBegin || groupID >= CullGrid.grid.groupEnd ? false : CullGrid.CellContainsPoint((ushort)(groupID - CullGrid.grid.groupBegin), ref flatPoint));
	}

	public static bool GroupIDContainsPoint(int groupID, ref Vector3 worldPoint)
	{
		return (groupID < CullGrid.grid.groupBegin || groupID >= CullGrid.grid.groupEnd ? false : CullGrid.CellContainsPoint((ushort)(groupID - CullGrid.grid.groupBegin), ref worldPoint));
	}

	public static int GroupIDFromCell(ushort cell)
	{
		if (cell >= CullGrid.grid.numCells)
		{
			throw new ArgumentOutOfRangeException("cell", (object)cell, "cell >= grid.numCells");
		}
		return CullGrid.grid.groupBegin + cell;
	}

	public static bool IsCellGroup(NetworkGroup group)
	{
		return CullGrid.IsCellGroupID(group.id);
	}

	public static bool IsCellGroupID(int usedGroup)
	{
		return (!CullGrid.has_grid || usedGroup < CullGrid.grid.groupBegin ? false : usedGroup < CullGrid.grid.groupEnd);
	}

	private static void RaycastDownVect(ref Vector3 a)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Vector3(a.x, 10000f, a.z), Vector3.down, out raycastHit, Single.PositiveInfinity))
		{
			a = raycastHit.point + (Vector3.up * a.y);
		}
	}

	private static void RegisterGrid(CullGrid grid)
	{
		if (grid)
		{
			CullGrid.grid = new CullGrid.CullGridRuntime(grid);
			CullGrid.has_grid = true;
		}
	}

	public static ushort WorldCell(Vector3 world)
	{
		return CullGrid.grid.WorldCell(ref world);
	}

	public static ushort WorldCell(ref Vector3 world)
	{
		return CullGrid.grid.WorldCell(ref world);
	}

	public static int WorldGroupID(ref Vector3 world)
	{
		return CullGrid.grid.WorldCell(ref world) + CullGrid.grid.groupBegin;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct CellID
	{
		[FieldOffset(-1)]
		private const ushort kInvalidID = 65535;

		[FieldOffset(0)]
		public ushort id;

		public int column
		{
			get
			{
				return (!this.valid ? -1 : this.id % CullGrid.grid.cellsWide);
			}
		}

		public CullGrid.CellID down
		{
			get
			{
				CullGrid.CellID cellID = new CullGrid.CellID();
				ushort num;
				if (!this.valid)
				{
					num = 65535;
				}
				else
				{
					num = CullGrid.CellID.NextDown(this.id);
				}
				cellID.id = num;
				return cellID;
			}
		}

		public Vector2 flatCenter
		{
			get
			{
				Vector2 vector2;
				CullGrid.grid.GetCenter((int)this.id, out vector2);
				return vector2;
			}
		}

		public Vector2 flatMax
		{
			get
			{
				Vector2 vector2;
				CullGrid.grid.GetMin((int)this.id, out vector2);
				return vector2;
			}
		}

		public Vector2 flatMin
		{
			get
			{
				Vector2 vector2;
				CullGrid.grid.GetMax((int)this.id, out vector2);
				return vector2;
			}
		}

		public Rect flatRect
		{
			get
			{
				Rect rect;
				CullGrid.grid.GetRect((int)this.id, out rect);
				return rect;
			}
		}

		public NetworkGroup @group
		{
			get
			{
				NetworkGroup networkGroup;
				if (!this.valid)
				{
					networkGroup = NetworkGroup.unassigned;
				}
				else
				{
					networkGroup = CullGrid.GroupIDFromCell(this.id);
				}
				return networkGroup;
			}
		}

		public int groupID
		{
			get
			{
				return (!this.valid ? NetworkGroup.unassigned.id : CullGrid.GroupIDFromCell(this.id));
			}
		}

		public CullGrid.CellID left
		{
			get
			{
				CullGrid.CellID cellID = new CullGrid.CellID();
				ushort num;
				if (!this.valid)
				{
					num = 65535;
				}
				else
				{
					num = CullGrid.CellID.NextLeft(this.id);
				}
				cellID.id = num;
				return cellID;
			}
		}

		public bool mostBottom
		{
			get
			{
				return (!this.valid ? false : this.id / CullGrid.grid.cellsWide == 0);
			}
		}

		public bool mostLeft
		{
			get
			{
				return (!this.valid ? false : this.id % CullGrid.grid.cellsWide == 0);
			}
		}

		public bool mostRight
		{
			get
			{
				return (!this.valid ? false : this.id % CullGrid.grid.cellsWide == CullGrid.grid.cellWideLast);
			}
		}

		public bool mostTop
		{
			get
			{
				return (!this.valid ? false : this.id / CullGrid.grid.cellsWide == CullGrid.grid.cellTallLast);
			}
		}

		public CullGrid.CellID right
		{
			get
			{
				CullGrid.CellID cellID = new CullGrid.CellID();
				ushort num;
				if (!this.valid)
				{
					num = 65535;
				}
				else
				{
					num = CullGrid.CellID.NextRight(this.id);
				}
				cellID.id = num;
				return cellID;
			}
		}

		public int row
		{
			get
			{
				return (!this.valid ? -1 : this.id / CullGrid.grid.cellsWide);
			}
		}

		public CullGrid.CellID up
		{
			get
			{
				CullGrid.CellID cellID = new CullGrid.CellID();
				ushort num;
				if (!this.valid)
				{
					num = 65535;
				}
				else
				{
					num = CullGrid.CellID.NextUp(this.id);
				}
				cellID.id = num;
				return cellID;
			}
		}

		public bool valid
		{
			get
			{
				return this.id < CullGrid.grid.numCells;
			}
		}

		public Bounds worldBounds
		{
			get
			{
				Bounds bound;
				CullGrid.grid.GetBounds((int)this.id, out bound);
				return bound;
			}
		}

		public Vector3 worldCenter
		{
			get
			{
				Vector3 vector3;
				CullGrid.grid.GetCenter((int)this.id, out vector3);
				return vector3;
			}
		}

		public Vector3 worldMax
		{
			get
			{
				Vector3 vector3;
				CullGrid.grid.GetMin((int)this.id, out vector3);
				return vector3;
			}
		}

		public Vector3 worldMin
		{
			get
			{
				Vector3 vector3;
				CullGrid.grid.GetMax((int)this.id, out vector3);
				return vector3;
			}
		}

		public CellID(ushort cellID)
		{
			this.id = cellID;
		}

		public bool ContainsFlatPoint(Vector2 flatPoint)
		{
			return CullGrid.grid.Contains((int)this.id, ref flatPoint);
		}

		public bool ContainsFlatPoint(ref Vector2 flatPoint)
		{
			return (!this.valid ? false : CullGrid.grid.Contains((int)this.id, ref flatPoint));
		}

		public bool ContainsWorldPoint(Vector3 worldPoint)
		{
			return CullGrid.grid.Contains((int)this.id, ref worldPoint);
		}

		public bool ContainsWorldPoint(ref Vector3 worldPoint)
		{
			return (!this.valid ? false : CullGrid.grid.Contains((int)this.id, ref worldPoint));
		}

		private static ushort NextDown(ushort id)
		{
			ushort num;
			if (id / CullGrid.grid.cellsWide != 0)
			{
				num = (ushort)(id - CullGrid.grid.cellsWide);
			}
			else
			{
				num = 65535;
			}
			return num;
		}

		private static ushort NextLeft(ushort id)
		{
			ushort num;
			if (id % CullGrid.grid.cellsWide != 0)
			{
				num = (ushort)(id - 1);
			}
			else
			{
				num = 65535;
			}
			return num;
		}

		private static ushort NextRight(ushort id)
		{
			ushort num;
			if (id % CullGrid.grid.cellsWide != CullGrid.grid.cellWideLast)
			{
				num = (ushort)(id + 1);
			}
			else
			{
				num = 65535;
			}
			return num;
		}

		private static ushort NextUp(ushort id)
		{
			ushort num;
			if (id / CullGrid.grid.cellsWide != CullGrid.grid.cellTallLast)
			{
				num = (ushort)(id + CullGrid.grid.cellsWide);
			}
			else
			{
				num = 65535;
			}
			return num;
		}
	}

	private class CullGridRuntime : CullGridSetup
	{
		private const double kMAX_WORLD_Y = 32000;

		private const double kMIN_WORLD_Y = -32000;

		public int groupEnd;

		public int numCells;

		public CullGrid cullGrid;

		public Transform transform;

		public double halfCellTall;

		public double halfCellWide;

		public int twoMinusOddTall;

		public int twoMinusOddWide;

		public double halfTwoMinusOddTall;

		public double halfTwoMinusOddWide;

		public double halfCellTallMinusHalfTwoMinusOddTall;

		public double halfCellWideMinusHalfTwoMinusOddWide;

		public double px;

		public double py;

		public double pz;

		public double fx;

		public double fy;

		public double fz;

		public double rx;

		public double ry;

		public double rz;

		public double flat_wide_ofs;

		public double flat_tall_ofs;

		public ushort cellWideLast;

		public ushort cellTallLast;

		public double cellWideLastTimesSquareDimension;

		public double cellTallLastTimesSquareDimension;

		public CullGridRuntime(CullGrid cullGrid) : base(cullGrid.setup)
		{
			this.cullGrid = cullGrid;
			this.transform = cullGrid.transform;
			this.halfCellTall = (double)this.cellsTall / 2;
			this.halfCellWide = (double)this.cellsWide / 2;
			this.twoMinusOddTall = 2 - (this.cellsTall & 1);
			this.twoMinusOddWide = 2 - (this.cellsWide & 1);
			this.halfTwoMinusOddTall = (double)this.twoMinusOddTall / 2;
			this.halfTwoMinusOddWide = this.halfTwoMinusOddWide / 2;
			this.halfCellTallMinusHalfTwoMinusOddTall = this.halfCellTall - this.halfTwoMinusOddTall;
			this.halfCellWideMinusHalfTwoMinusOddWide = this.halfCellWide - this.halfTwoMinusOddWide;
			Vector3 vector3 = this.transform.forward;
			Vector3 vector31 = this.transform.right;
			Vector3 vector32 = this.transform.position;
			this.fx = (double)vector3.x;
			this.fy = (double)vector3.y;
			this.fz = (double)vector3.z;
			double num = Math.Sqrt(this.fx * this.fx + this.fy * this.fy + this.fz * this.fz);
			CullGrid.CullGridRuntime cullGridRuntime = this;
			cullGridRuntime.fx = cullGridRuntime.fx / num;
			CullGrid.CullGridRuntime cullGridRuntime1 = this;
			cullGridRuntime1.fy = cullGridRuntime1.fy / num;
			CullGrid.CullGridRuntime cullGridRuntime2 = this;
			cullGridRuntime2.fz = cullGridRuntime2.fz / num;
			this.rx = (double)vector31.x;
			this.ry = (double)vector31.y;
			this.rz = (double)vector31.z;
			num = Math.Sqrt(this.rx * this.rx + this.ry * this.ry + this.rz * this.rz);
			CullGrid.CullGridRuntime cullGridRuntime3 = this;
			cullGridRuntime3.rx = cullGridRuntime3.rx / num;
			CullGrid.CullGridRuntime cullGridRuntime4 = this;
			cullGridRuntime4.ry = cullGridRuntime4.ry / num;
			CullGrid.CullGridRuntime cullGridRuntime5 = this;
			cullGridRuntime5.rz = cullGridRuntime5.rz / num;
			this.px = (double)vector32.x;
			this.py = (double)vector32.y;
			this.pz = (double)vector32.z;
			this.flat_wide_ofs = (double)this.cellSquareDimension * (this.halfCellWide - (double)(1 - (this.cellsWide & 1)) / 2);
			this.flat_tall_ofs = (double)this.cellSquareDimension * (this.halfCellTall - (double)(1 - (this.cellsTall & 1)) / 2);
			this.cellTallLast = (ushort)(this.cellsTall - 1);
			this.cellWideLast = (ushort)(this.cellsWide - 1);
			this.cellTallLastTimesSquareDimension = (double)this.cellTallLast * (double)this.cellSquareDimension;
			this.cellWideLastTimesSquareDimension = (double)this.cellWideLast * (double)this.cellSquareDimension;
			this.numCells = this.cellsTall * this.cellsWide;
			this.groupEnd = this.groupBegin + this.numCells;
		}

		public bool Contains(int cell, ref Vector2 flatPoint)
		{
			return (cell < 0 || cell >= this.numCells ? false : this.FlatCell(ref flatPoint) == cell);
		}

		public bool Contains(int cell, ref Vector3 worldPoint)
		{
			return (cell < 0 || cell >= this.numCells ? false : this.WorldCell(ref worldPoint) == cell);
		}

		public bool Contains(int x, int y, ref Vector2 flatPoint)
		{
			return this.Contains(y * this.cellsWide + x, ref flatPoint);
		}

		public bool Contains(int x, int y, ref Vector3 worldPoint)
		{
			return this.Contains(y * this.cellsWide + x, ref worldPoint);
		}

		public List<ushort> EnumerateNearbyCells(int cell)
		{
			return this.EnumerateNearbyCells(cell, cell % CullGrid.grid.cellsWide, cell / CullGrid.grid.cellsWide);
		}

		public List<ushort> EnumerateNearbyCells(int x, int y)
		{
			return this.EnumerateNearbyCells(y * this.cellsWide + x, x, y);
		}

		public List<ushort> EnumerateNearbyCells(int i, int x, int y)
		{
			if (i < 0)
			{
				throw new ArgumentOutOfRangeException("i", (object)i, "i<0");
			}
			if (x < 0)
			{
				throw new ArgumentOutOfRangeException("x", (object)x, "x<0");
			}
			if (y < 0)
			{
				throw new ArgumentOutOfRangeException("y", (object)y, "y<0");
			}
			List<ushort> nums = new List<ushort>();
			int num = -(this.gatheringCellsCenter % this.gatheringCellsWide);
			int num1 = -(this.gatheringCellsCenter / this.gatheringCellsWide);
			for (int i1 = 0; i1 < this.gatheringCellsWide; i1++)
			{
				int num2 = x + i1 + num;
				if (num2 >= 0 && num2 < this.cellsWide)
				{
					for (int j = 0; j < this.gatheringCellsTall; j++)
					{
						int num3 = y + j + num1;
						if (num3 >= 0 && num3 < this.cellsTall && base.GetGatheringBit(i1, j))
						{
							ushort num4 = (ushort)(num2 + num3 * this.cellsWide);
							if (num3 != y || num2 != x)
							{
								nums.Add(num4);
							}
							else
							{
								nums.Insert(0, num4);
							}
						}
					}
				}
			}
			return nums;
		}

		public ushort FlatCell(ref Vector2 point, out ushort x, out ushort y)
		{
			double num = (double)point.x + this.flat_wide_ofs;
			if (num <= 0)
			{
				x = 0;
			}
			else if (num < this.cellWideLastTimesSquareDimension)
			{
				x = (ushort)Math.Floor(num / (double)this.cellSquareDimension);
			}
			else
			{
				x = this.cellWideLast;
			}
			double num1 = (double)point.y + this.flat_tall_ofs;
			if (num1 <= 0)
			{
				y = 0;
			}
			else if (num1 < this.cellTallLastTimesSquareDimension)
			{
				y = (ushort)Math.Floor(num1 / (double)this.cellSquareDimension);
			}
			else
			{
				y = this.cellTallLast;
			}
			return (ushort)(y * this.cellsWide + x);
		}

		public ushort FlatCell(ref Vector2 point)
		{
			int num;
			int num1;
			double num2 = (double)point.x + this.flat_wide_ofs;
			if (num2 <= 0)
			{
				num = 0;
			}
			else if (num2 < this.cellWideLastTimesSquareDimension)
			{
				num = (int)Math.Floor(num2 / (double)this.cellSquareDimension);
			}
			else
			{
				num = this.cellWideLast;
			}
			double num3 = (double)point.y + this.flat_tall_ofs;
			if (num3 <= 0)
			{
				num1 = 0;
			}
			else if (num3 < this.cellTallLastTimesSquareDimension)
			{
				num1 = (int)Math.Floor(num3 / (double)this.cellSquareDimension);
			}
			else
			{
				num1 = this.cellTallLast;
			}
			return (ushort)(num1 * this.cellsWide + num);
		}

		public void GetBounds(int x, int y, out Bounds bounds)
		{
			Vector3 vector3;
			this.GetCenter(x, y, out vector3);
			bounds = new Bounds(vector3, new Vector3((float)this.cellSquareDimension, 64000f, (float)this.cellSquareDimension));
		}

		public void GetBounds(int cell, out Bounds bounds)
		{
			Vector3 vector3;
			int num = cell % this.cellsWide;
			this.GetCenter(num, cell / this.cellsWide, out vector3);
			bounds = new Bounds(vector3, new Vector3((float)this.cellSquareDimension, 64000f, (float)this.cellSquareDimension));
		}

		public void GetCenter(int cell, out Vector3 center)
		{
			center = new Vector3();
			double num = ((double)(cell % this.cellsWide) - this.halfCellWideMinusHalfTwoMinusOddWide) * (double)this.cellSquareDimension;
			double num1 = ((double)(cell / this.cellsWide) - this.halfCellTallMinusHalfTwoMinusOddTall) * (double)this.cellSquareDimension;
			center.x = (float)(this.px + this.fx * num1 + this.rx * num);
			center.y = (float)(this.py + this.fy * num1 + this.ry * num);
			center.z = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetCenter(int cell, out Vector2 center)
		{
			center = new Vector2();
			double num = ((double)(cell % this.cellsWide) - this.halfCellWideMinusHalfTwoMinusOddWide) * (double)this.cellSquareDimension;
			double num1 = ((double)(cell / this.cellsWide) - this.halfCellTallMinusHalfTwoMinusOddTall) * (double)this.cellSquareDimension;
			center.x = (float)(this.px + this.fx * num1 + this.rx * num);
			center.y = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetCenter(int x, int y, out Vector3 center)
		{
			center = new Vector3();
			double num = ((double)x - this.halfCellWideMinusHalfTwoMinusOddWide) * (double)this.cellSquareDimension;
			double num1 = ((double)y - this.halfCellTallMinusHalfTwoMinusOddTall) * (double)this.cellSquareDimension;
			center.x = (float)(this.px + this.fx * num1 + this.rx * num);
			center.y = (float)(this.py + this.fy * num1 + this.ry * num);
			center.z = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetCenter(int x, int y, out Vector2 center)
		{
			center = new Vector2();
			double num = ((double)x - this.halfCellWideMinusHalfTwoMinusOddWide) * (double)this.cellSquareDimension;
			double num1 = ((double)y - this.halfCellTallMinusHalfTwoMinusOddTall) * (double)this.cellSquareDimension;
			center.x = (float)(this.px + this.fx * num1 + this.rx * num);
			center.y = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetMax(int cell, out Vector3 max)
		{
			max = new Vector3();
			double num = ((double)(cell % this.cellsWide) - this.halfCellWideMinusHalfTwoMinusOddWide + 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)(cell / this.cellsWide) - this.halfCellTallMinusHalfTwoMinusOddTall + 0.5) * (double)this.cellSquareDimension;
			max.x = (float)(this.px + this.fx * num1 + this.rx * num);
			max.y = (float)(32000 + (this.py + this.fy * num1 + this.ry * num));
			max.z = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetMax(int cell, out Vector2 max)
		{
			max = new Vector2();
			double num = ((double)(cell % this.cellsWide) - this.halfCellWideMinusHalfTwoMinusOddWide + 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)(cell / this.cellsWide) - this.halfCellTallMinusHalfTwoMinusOddTall + 0.5) * (double)this.cellSquareDimension;
			max.x = (float)(this.px + this.fx * num1 + this.rx * num);
			max.y = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetMax(int x, int y, out Vector3 max)
		{
			max = new Vector3();
			double num = ((double)x - this.halfCellWideMinusHalfTwoMinusOddWide + 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)y - this.halfCellTallMinusHalfTwoMinusOddTall + 0.5) * (double)this.cellSquareDimension;
			max.x = (float)(this.px + this.fx * num1 + this.rx * num);
			max.y = (float)(32000 + (this.py + this.fy * num1 + this.ry * num));
			max.z = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetMax(int x, int y, out Vector2 max)
		{
			max = new Vector2();
			double num = ((double)x - this.halfCellTallMinusHalfTwoMinusOddTall + 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)y - this.halfCellTallMinusHalfTwoMinusOddTall + 0.5) * (double)this.cellSquareDimension;
			max.x = (float)(this.px + this.fx * num1 + this.rx * num);
			max.y = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetMin(int cell, out Vector3 min)
		{
			min = new Vector3();
			double num = ((double)(cell % this.cellsWide) - this.halfCellWideMinusHalfTwoMinusOddWide - 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)(cell / this.cellsWide) - this.halfCellTallMinusHalfTwoMinusOddTall - 0.5) * (double)this.cellSquareDimension;
			min.x = (float)(this.px + this.fx * num1 + this.rx * num);
			min.y = (float)(-32000 + (this.py + this.fy * num1 + this.ry * num));
			min.z = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetMin(int cell, out Vector2 min)
		{
			min = new Vector2();
			double num = ((double)(cell % this.cellsWide) - this.halfCellWideMinusHalfTwoMinusOddWide - 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)(cell / this.cellsWide) - this.halfCellTallMinusHalfTwoMinusOddTall - 0.5) * (double)this.cellSquareDimension;
			min.x = (float)(this.px + this.fx * num1 + this.rx * num);
			min.y = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetMin(int x, int y, out Vector3 min)
		{
			min = new Vector3();
			double num = ((double)x - this.halfCellWideMinusHalfTwoMinusOddWide - 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)y - this.halfCellTallMinusHalfTwoMinusOddTall - 0.5) * (double)this.cellSquareDimension;
			min.x = (float)(this.px + this.fx * num1 + this.rx * num);
			min.y = (float)(-32000 + (this.py + this.fy * num1 + this.ry * num));
			min.z = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetMin(int x, int y, out Vector2 min)
		{
			min = new Vector2();
			double num = ((double)x - this.halfCellWideMinusHalfTwoMinusOddWide - 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)y - this.halfCellTallMinusHalfTwoMinusOddTall - 0.5) * (double)this.cellSquareDimension;
			min.x = (float)(this.px + this.fx * num1 + this.rx * num);
			min.y = (float)(this.pz + this.fz * num1 + this.rz * num);
		}

		public void GetRect(int x, int y, out Rect rect)
		{
			float single;
			float single1;
			float single2;
			float single3;
			double num = ((double)x - this.halfCellWideMinusHalfTwoMinusOddWide - 0.5) * (double)this.cellSquareDimension;
			double num1 = ((double)y - this.halfCellTallMinusHalfTwoMinusOddTall - 0.5) * (double)this.cellSquareDimension;
			double num2 = num + (double)this.cellSquareDimension;
			double num3 = num1 + (double)this.cellSquareDimension;
			double num4 = this.px + this.fx * num1 + this.rx * num;
			double num5 = this.px + this.fx * num3 + this.ry * num2;
			if (num4 >= num5)
			{
				single = (float)num5;
				single1 = (float)(num4 - num5);
			}
			else
			{
				single = (float)num4;
				single1 = (float)(num5 - num4);
			}
			num4 = this.pz + this.fz * num1 + this.rx * num;
			num5 = this.pz + this.fz * num3 + this.rx * num2;
			if (num4 >= num5)
			{
				single2 = (float)num5;
				single3 = (float)(num4 - num5);
			}
			else
			{
				single2 = (float)num4;
				single3 = (float)(num5 - num4);
			}
			rect = new Rect(single, single2, single1, single3);
		}

		public void GetRect(int cell, out Rect rect)
		{
			float single;
			float single1;
			float single2;
			float single3;
			int num = cell % this.cellsWide;
			int num1 = cell / this.cellsWide;
			double num2 = ((double)num - this.halfCellWideMinusHalfTwoMinusOddWide - 0.5) * (double)this.cellSquareDimension;
			double num3 = ((double)num1 - this.halfCellTallMinusHalfTwoMinusOddTall - 0.5) * (double)this.cellSquareDimension;
			double num4 = num2 + (double)this.cellSquareDimension;
			double num5 = num3 + (double)this.cellSquareDimension;
			double num6 = this.px + this.fx * num3 + this.rx * num2;
			double num7 = this.px + this.fx * num5 + this.ry * num4;
			if (num6 >= num7)
			{
				single = (float)num7;
				single1 = (float)(num6 - num7);
			}
			else
			{
				single = (float)num6;
				single1 = (float)(num7 - num6);
			}
			num6 = this.pz + this.fz * num3 + this.rx * num2;
			num7 = this.pz + this.fz * num5 + this.rx * num4;
			if (num6 >= num7)
			{
				single2 = (float)num7;
				single3 = (float)(num6 - num7);
			}
			else
			{
				single2 = (float)num6;
				single3 = (float)(num7 - num6);
			}
			rect = new Rect(single, single2, single1, single3);
		}

		public ushort WorldCell(ref Vector3 point, out ushort x, out ushort y)
		{
			double num = (double)point.x + this.flat_wide_ofs;
			if (num <= 0)
			{
				x = 0;
			}
			else if (num < this.cellWideLastTimesSquareDimension)
			{
				x = (ushort)Math.Floor(num / (double)this.cellSquareDimension);
			}
			else
			{
				x = this.cellWideLast;
			}
			double num1 = (double)point.z + this.flat_tall_ofs;
			if (num1 <= 0)
			{
				y = 0;
			}
			else if (num1 < this.cellTallLastTimesSquareDimension)
			{
				y = (ushort)Math.Floor(num1 / (double)this.cellSquareDimension);
			}
			else
			{
				y = this.cellTallLast;
			}
			return (ushort)(y * this.cellsWide + x);
		}

		public ushort WorldCell(ref Vector3 point)
		{
			int num;
			int num1;
			double num2 = (double)point.x + this.flat_wide_ofs;
			if (num2 <= 0)
			{
				num = 0;
			}
			else if (num2 < this.cellWideLastTimesSquareDimension)
			{
				num = (int)Math.Floor(num2 / (double)this.cellSquareDimension);
			}
			else
			{
				num = this.cellWideLast;
			}
			double num3 = (double)point.z + this.flat_tall_ofs;
			if (num3 <= 0)
			{
				num1 = 0;
			}
			else if (num3 < this.cellTallLastTimesSquareDimension)
			{
				num1 = (int)Math.Floor(num3 / (double)this.cellSquareDimension);
			}
			else
			{
				num1 = this.cellTallLast;
			}
			return (ushort)(num1 * this.cellsWide + num);
		}
	}
}