using Everglow.Commons.CustomTiles;
using Everglow.Commons.CustomTiles.Tiles;

namespace Everglow.Commons.CustomTiles.EntityColliding;

public class ProjHandler : EntityHandler<Projectile>
{
	public ProjHandler(Projectile entity) : base(entity) { }
	public override void OnCollision(CustomTile tile, Direction dir, ref CustomTile newAttach)
	{
		if (dir == Direction.Inside)
			Entity.Kill();
	}
	public override void Update(bool ignorePlats = false)
	{
		if (attachTile is not null)
		{
			Entity.position += new Vector2(0, Entity.gfxOffY);
			Entity.gfxOffY = 0;
		}
		base.Update(ignorePlats);
	}
	public override void OnAttach()
	{
		Entity.velocity.Y = 0;
	}
}
