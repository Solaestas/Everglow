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
    public override void Update()
    {
        if (attachTile is not null)
        {
            Entity.position += new Vector2(0, Entity.gfxOffY);
            Entity.gfxOffY = 0;
        }
        base.Update();
    }
    public override void OnCollision(DynamicTile tile, Direction dir)
    {
        if(dir == Direction.Inside && !Entity.boss)
        {
            Entity.StrikeNPC(10, 0, 0);
        }
    }

}
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
        orig(self, fall, cPosition, cWidth, cHeight);
        //var modnpc = self.GetGlobalNPC<NPCColliding>();
        //modnpc.fall = fall;
        //if (modnpc.standTile is not null)
        //{
        //    self.velocity.Y = 0;
        //}
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
        orig(self, oldDryVelocity, Slowdown);
        self.GetGlobalNPC<NPCColliding>().handler.Update();
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
        orig(self);
        self.GetGlobalNPC<NPCColliding>().handler.Update();
        TileSystem.EnableDTCollision = true;

    }
}
