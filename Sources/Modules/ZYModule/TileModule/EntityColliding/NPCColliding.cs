using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

internal class NPCColliding : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public DynamicTile standTile;
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
        if (npc.standTile is not null)
        {
            self.position += new Vector2(0, self.gfxOffY);
            self.gfxOffY = 0;
        }
        Vector2 oldpos = self.position;

        orig(self, oldDryVelocity, Slowdown);
        Vector2 move = self.position - oldpos;
        self.position = oldpos;
        var result = TileSystem.MoveCollision(self, move, npc.fall);
        DynamicTile newStand = null;
        if (npc.standTile != null && !npc.standTile.Active)
        {
            npc.standTile = null;
        }

        foreach (var (tile, info) in result)
        {
            if (info == Direction.Inside)
            {
                self.StrikeNPC(10, 0, 0);
            }

            if (info == Direction.Bottom)
            {
                npc.standTile = tile;
            }
        }

        if (newStand == null)
        {
            if (npc.standTile != null)
            {
                npc.standTile.Leave(self);
            }
        }
        else if (npc.standTile != newStand)
        {
            npc.standTile?.Leave(self);
            newStand.Stand(self, true);
            npc.standTile = newStand;
        }
        else if (npc.standTile == newStand)
        {
            newStand.Stand(self, false);
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
        if (npc.standTile is not null)
        {
            self.position += new Vector2(0, self.gfxOffY);
            self.gfxOffY = 0;
        }
        Vector2 oldpos = self.position;

        orig(self);
        Vector2 move = self.position - oldpos;
        self.position = oldpos;
        var result = TileSystem.MoveCollision(self, move, npc.fall);
        DynamicTile newStand = null;
        if (npc.standTile != null && !npc.standTile.Active)
        {
            npc.standTile = null;
        }

        foreach (var (tile, info) in result)
        {
            if (info == Direction.Inside)
            {
                self.StrikeNPC(10, 0, 0);
            }

            if (info == Direction.Bottom)
            {
                npc.standTile = tile;
            }
        }

        if (newStand == null)
        {
            if (npc.standTile != null)
            {
                npc.standTile.Leave(self);
            }
        }
        else if (npc.standTile != newStand)
        {
            npc.standTile?.Leave(self);
            newStand.Stand(self, true);
            npc.standTile = newStand;
        }
        else if (npc.standTile == newStand)
        {
            newStand.Stand(self, false);
        }

        TileSystem.EnableDTCollision = true;
    }
}
