using Everglow.Commons.Physics.DataStructures;

namespace Everglow.Commons.Physics.Abstracts;

public interface IPolygonalCollider2D : ICollider2D
{
	Polygon GetPolygon();
}