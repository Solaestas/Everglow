namespace Everglow.Commons.CustomTiles;
/// <summary>
/// 
/// </summary>
/// <param name="Collider">碰撞体</param>
/// <param name="Normal">内法向</param>
/// <param name="Velocity">下一帧的速度</param>
/// <param name="Stride">当前帧的步长</param>
public record struct CollisionResult(
	RigidEntity Collider,
	Vector2 Normal,
	Vector2 Velocity,
	Vector2 Stride);