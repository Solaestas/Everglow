using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;
using Terraria.DataStructures;
namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

internal class PlayerHandler : EntityHandler<Player>
{
    public PlayerHandler(Player entity) : base(entity) { }
    public override void Update(bool ignorePlats = false)
    {
        if (attachTile is not null)
        {
            Entity.position += new Vector2(0, Entity.gfxOffY);
            Entity.gfxOffY = 0;
        }

        base.Update(ignorePlats || Entity.grapCount > 0);

    }
    public override bool CanAttach()
    {
        return Entity.grapCount == 0 && !(Entity.mount.Active && Entity.mount.CanFly());
    }
    public override Direction Ground => Entity.gravDir > 0 ? Direction.Bottom : Direction.Top;
    public override void OnAttach()
    {
        if (attachType == AttachType.Stand)
        {
            Entity.velocity.Y = 0;
        }
        else if(attachType == AttachType.Grab)
        {
            Entity.velocity.Y = Quick.AirSpeed;
        }
    }
    public override void OnCollision(DynamicTile tile, Direction dir, ref DynamicTile newAttach)
    {
        if(dir == Direction.Inside)
        {
            Entity.Hurt(PlayerDeathReason.ByCustomReason("Inside Tile"), 10, 0);
        }

        if (tile.IsGrabbable && dir.IsH())
        {
            var player = Entity;
            player.slideDir = (int)player.GetControlDirectionH().ToVector2().X;
            if (player.slideDir == 0 || player.spikedBoots <= 0 || player.mount.Active ||
                ((!player.controlLeft || player.slideDir != -1 || dir != Direction.Left) && 
                (!player.controlRight || player.slideDir != 1 || dir != Direction.Right)))
            {
                return;
            }
            newAttach = tile;
            attachDir = dir;
            attachType = AttachType.Grab;
        }
    }
}
