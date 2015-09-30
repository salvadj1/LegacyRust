using System;

namespace NGUI.Meshing
{
	public enum PrimitiveKind : byte
	{
		Triangle = 0,
		Grid1x1 = 1,
		Quad = 1,
		Grid2x1 = 2,
		Grid1x2 = 3,
		Grid2x2 = 4,
		Grid1x3 = 5,
		Grid3x1 = 6,
		Grid3x2 = 7,
		Grid2x3 = 8,
		Grid3x3 = 9,
		Hole3x3 = 10,
		Invalid = 255
	}
}