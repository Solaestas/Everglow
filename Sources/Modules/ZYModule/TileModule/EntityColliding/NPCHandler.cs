using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

internal class NPCHandler : EntityHandler<NPC>
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
    public override void OnCollision(DynamicTile tile, Direction dir, ref DynamicTile newAttach)
    {
        if(dir == Direction.Inside && !Entity.boss)
        {
            Entity.StrikeNPC(10, 0, 0);
        }
    }

}
