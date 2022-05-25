using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

internal class NPCColliding : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public IDynamicTile standTile;
    public bool fall;
    public override void Load()
    {
        On.Terraria.NPC.Collision_MoveWhileDry += NPC_Collision_MoveWhileDry;
        On.Terraria.NPC.Collision_MoveWhileWet += NPC_Collision_MoveWhileWet;
        On.Terraria.NPC.ApplyTileCollision += NPC_ApplyTileCollision;
    }
    private static void NPC_ApplyTileCollision(On.Terraria.NPC.orig_ApplyTileCollision orig, NPC self, bool fall, Vector2 cPosition, int cWidth, int cHeight)
    {
        TileSystem.EnableDTCollision = false;
        orig(self, fall, cPosition, cWidth, cHeight);
        var modnpc = self.GetGlobalNPC<NPCColliding>();
        modnpc.fall = fall;
        if (modnpc.standTile is not null)
        {
            self.velocity.Y = 0;
        }
        TileSystem.EnableDTCollision = true;
    }
    private static void NPC_Collision_MoveWhileWet(On.Terraria.NPC.orig_Collision_MoveWhileWet orig, NPC self, Vector2 oldDryVelocity, float Slowdown)
    {
        if (!TileSystem.Enable || self.noTileCollide)
        {
            orig(self, oldDryVelocity, Slowdown);
            return;
        }

        TileSystem.EnableDTCollision = false;
        var npc = self.GetGlobalNPC<NPCColliding>();
        Vector2 last = self.position;
        orig(self, oldDryVelocity, Slowdown);
        Vector2 move = self.position - last;
        self.position = last;
        if (npc.standTile is not null)
        {
            if (npc.standTile.Active is false || !npc.standTile.OnTile(self, npc.fall))
            {
                self.position -= self.velocity;
                npc.standTile.StandingLeaving(self);
                self.position += self.velocity;
                npc.standTile = null;
                self.position.Y += 0.001f;
            }
            else
            {
                npc.standTile.StandingMoving(self);
            }
        }
        var result = TileSystem.MoveColliding(self, move, npc.fall);

        foreach (var (tile, info) in result)
        {
            if (info == Direction.Bottom)
            {
                npc.standTile = tile;
                tile.StandingBegin(self);
            }
            else if (info == Direction.Inside && !self.boss)
            {
                self.StrikeNPC(10, 0, 0);
            }
        }
        TileSystem.EnableDTCollision = true;
    }
    private static void NPC_Collision_MoveWhileDry(On.Terraria.NPC.orig_Collision_MoveWhileDry orig, NPC self)
    {
        if (!TileSystem.Enable || self.noTileCollide)
        {
            orig(self);
            return;
        }

        TileSystem.EnableDTCollision = false;
        var npc = self.GetGlobalNPC<NPCColliding>();
        Vector2 last = self.position;
        orig(self);
        Vector2 move = self.position - last;
        self.position = last;
        if (npc.standTile is not null)
        {
            if (npc.standTile.Active is false || !npc.standTile.OnTile(self, npc.fall))
            {
                self.position -= self.velocity;
                npc.standTile.StandingLeaving(self);
                self.position += self.velocity;
                npc.standTile = null;
                self.position.Y += 0.001f;
            }
            else
            {
                npc.standTile.StandingMoving(self);
            }
        }
        var result = TileSystem.MoveColliding(self, move, npc.fall);

        foreach (var (tile, info) in result)
        {
            if (info == Direction.Bottom)
            {
                npc.standTile = tile;
                tile.StandingBegin(self);
            }
            else if (info == Direction.Inside && !self.boss)
            {
                self.StrikeNPC(10, 0, 0);
            }
        }
        TileSystem.EnableDTCollision = true;
    }
}
