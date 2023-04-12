using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Geometries;
internal struct BoundaryBox
{
	public float XMin;
	public float YMin;
	public float ZMin;
	public float XMax;
	public float YMax;
	public float ZMax;
	public Vector3 Center => new Vector3(XMin + XMax, YMin + YMax, ZMin + ZMax) / 2;
	public BoundaryBox(Vector3[] vs)
	{
		foreach (Vector3 v in vs)
		{
			if (v.X < XMin)
			{
				XMin = v.X;
			}
			if (v.X > XMax)
			{
				XMax = v.X;
			}
			if (v.Y < YMin)
			{
				YMin = v.Y;
			}
			if (v.Y > YMax)
			{
				YMax = v.Y;
			}
			if (v.Z < ZMin)
			{
				ZMin = v.Z;
			}
			if (v.Z > ZMax)
			{
				ZMax = v.Z;
			}
		}
	}
	public bool Intersect(BoundaryBox other)
	{
		return Intersect(this, other);
	}
	public static bool Intersect(BoundaryBox box1, BoundaryBox box2)
	{
		if ((box2.XMin > box1.XMin && box2.XMin < box1.XMax)|| (box2.XMax > box1.XMin && box2.XMax < box1.XMax))
		{
			return true;
		}
		if ((box2.YMin > box1.YMin && box2.YMin < box1.YMax) || (box2.YMax > box1.YMin && box2.YMax < box1.YMax))
		{
			return true;
		}
		if ((box2.ZMin > box1.ZMin && box2.ZMin < box1.ZMax) || (box2.ZMax > box1.ZMin && box2.ZMax < box1.ZMax))
		{
			return true;
		}
		return false;
	}
}