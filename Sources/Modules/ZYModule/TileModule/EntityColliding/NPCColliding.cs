namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;
internal class NPCColliding : GlobalNPC
{
    public NPCHandler handler;
    public override bool InstancePerEntity => true;
    protected override bool CloneNewInstances => true;
    public override bool IsCloneable => true;
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return !entity.noTileCollide;
    }
    public override GlobalNPC Clone(NPC from, NPC to)
    {
        var clone = base.Clone(from, to) as NPCColliding;
        clone.handler = new NPCHandler(to);
        return clone;
    }
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
        self.GetGlobalNPC<NPCColliding>().fall = fall;
        orig(self, fall, cPosition, cWidth, cHeight);
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
        npc.handler.position = self.position;
        orig(self, oldDryVelocity, Slowdown);
        npc.handler.Update(npc.fall);
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
        npc.handler.position = self.position;
        orig(self);
        npc.handler.Update(npc.fall);
        TileSystem.EnableDTCollision = true;

    }
}
