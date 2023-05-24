using Everglow.Commons.CustomTiles.Tiles;

namespace Everglow.Commons.CustomTiles.EntityColliding;

public class NPCHandler : EntityHandler<NPC>
{
	public NPCHandler(NPC entity) : base(entity) { }
	public override Direction Ground => Direction.Bottom;
	public override void OnAttach()
	{
		Entity.velocity.Y = 0;
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
	public override void OnCollision(CustomTile tile, Direction dir, ref CustomTile newAttach)
	{
		if (dir == Direction.Inside && !Entity.boss)
			Entity.StrikeNPC(10, 0, 0);
	}

}
