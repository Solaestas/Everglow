namespace Everglow.Commons.Physics.Abstracts;

public partial interface ICollider2D
{
	[Obsolete("Not implemented.", true)]
	Vector3 GetSDFWithGradient(Vector2 position);
}