﻿using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

internal class ProjHandler : EntityHandler<Projectile>
{
    public ProjHandler(Projectile entity) : base(entity) { }
    public override void OnCollision(DynamicTile tile, Direction dir, ref DynamicTile newAttach)
    {
        if(dir == Direction.Inside)
        {
            Entity.Kill();
        }
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
