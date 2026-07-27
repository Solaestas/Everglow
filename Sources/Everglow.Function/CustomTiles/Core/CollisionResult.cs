namespace Everglow.Commons.CustomTiles.Core;

/// <summary>
/// A structure representing the result of a collision between two rigid entities.
/// </summary>
/// <param name="Collider"></param>
/// <param name="Normal">Inward normal</param>
/// <param name="Velocity">Velocity in next frame</param>
/// <param name="Stride">Pace in current frame</param>
public record struct CollisionResult(
	RigidEntity Collider,
	Vector2 Normal,
	Vector2 Velocity,
	Vector2 Stride);