using Everglow.Commons.Physics.DataStructures;

namespace Everglow.Commons.Physics.Abstracts;

public partial interface ICollider2D
{
	Vector2 Position { get; set; }

	[Obsolete("Rotation not implemented.", true)]
	float Rotation { get; set; }

	Vector2 Size { get; set; }

	AABB AABB { get; }

	bool Intersect(ICollider2D other);

	bool Contains(Vector2 position);

	float Distance(Vector2 position);
}