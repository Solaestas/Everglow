using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;
internal class PlayerHandler : EntityHandler<Player>
{
    public PlayerHandler(Player entity) : base(entity) { }
    public override void Update()
    {
        if (attachTile is not null)
        {
            Entity.position += new Vector2(0, Entity.gfxOffY);
            Entity.gfxOffY = 0;
        }
        base.Update();
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

        if (dir.IsH())
        {
            var player = Entity;
            player.slideDir = (int)player.GetControlDirectionH().ToVector2().X;
            if (player.slideDir == 0 || player.spikedBoots <= 0 || player.mount.Active ||
                ((!player.controlLeft || player.slideDir != -1) && (!player.controlRight || player.slideDir != 1)))
            {
                return;
            }
            newAttach = tile;
            attachDir = dir;
            attachType = AttachType.Grab;
        }
    }
}
internal class PlayerColliding : ModPlayer
{
    public PlayerHandler handler;
    protected override bool CloneNewInstances => true;
    public override bool IsCloneable => true;
    public override void Load()
    {
        On.Terraria.Player.CanFitSpace += Player_CanFitSpace;
        On.Terraria.Player.DryCollision += Player_DryCollision;
        On.Terraria.Player.WaterCollision += Player_WaterCollision;
        //On.Terraria.Player.ItemCheck_UseMiningTools_ActuallyUseMiningTool += Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool;
        On.Terraria.Player.HoneyCollision += Player_HoneyCollision;
        try
        {
            IL.Terraria.Player.WallslideMovement += Player_WallslideMovement;
        }
        catch (Exception ex)
        {
            ILException.Throw("Player_WallslideMovement_Error", ex);
        }
        
    }

    public override ModPlayer Clone(Player newEntity)
    {
        var clone = base.Clone(newEntity) as PlayerColliding;
        clone.handler = new PlayerHandler(newEntity);
        return clone;
    }
    private static void Player_DryCollision(On.Terraria.Player.orig_DryCollision orig, Player self, bool fallThrough, bool ignorePlats)
    {
        if (!TileSystem.Enable || self.ghost)
        {
            orig(self, fallThrough, ignorePlats);
            return;
        }

        TileSystem.EnableDTCollision = false;
        orig(self, fallThrough, ignorePlats);
        self.GetModPlayer<PlayerColliding>().handler.Update();
        TileSystem.EnableDTCollision = true;
    }
    private static void Player_WaterCollision(On.Terraria.Player.orig_WaterCollision orig, Player self, bool fallThrough, bool ignorePlats)
    {
        if (!TileSystem.Enable || self.ghost)
        {
            orig(self, fallThrough, ignorePlats);
            return;
        }

        TileSystem.EnableDTCollision = false;
        orig(self, fallThrough, ignorePlats);
        self.GetModPlayer<PlayerColliding>().handler.Update();
        TileSystem.EnableDTCollision = true;
    }
    private static void Player_HoneyCollision(On.Terraria.Player.orig_HoneyCollision orig, Player self, bool fallThrough, bool ignorePlats)
    {
        if (!TileSystem.Enable || self.ghost)
        {
            orig(self, fallThrough, ignorePlats);
            return;
        }

        TileSystem.EnableDTCollision = false;
        orig(self, fallThrough, ignorePlats);
        self.GetModPlayer<PlayerColliding>().handler.Update();
        TileSystem.EnableDTCollision = true;
    }
    private static bool Player_CanFitSpace(On.Terraria.Player.orig_CanFitSpace orig, Player self, int heightBoost)
    {
        TileSystem.EnableDTCollision = false;
        bool flag = orig(self, heightBoost);
        TileSystem.EnableDTCollision = true;
        return flag;
    }
    private static void Player_WallslideMovement(ILContext il)
    {
        var cursor = new ILCursor(il);
        var label = cursor.DefineLabel();
        if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchStfld<Player>("sliding")))
        {
            ILException.Throw("Player_WallslideMovement_NotFound_0");
        }
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.EmitDelegate((Player player) =>
        {
            return player.GetModPlayer<PlayerColliding>().handler.attachType == AttachType.Grab;
        });
        cursor.Emit(OpCodes.Brtrue, label);


        if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchStloc(0) && ins.Previous.MatchLdcI4(0)))
        {
            ILException.Throw("Player_WallslideMovement_NotFound_1");
        }
        cursor.MarkLabel(label);
        cursor.Emit(OpCodes.Ldc_I4_1);
        cursor.Emit(OpCodes.Stloc_0);
    }
    //private static void Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool(On.Terraria.Player.orig_ItemCheck_UseMiningTools_ActuallyUseMiningTool orig, Player self, Item sItem, out bool canHitWalls, int x, int y)
    //{
    //    //TODO 大概联机会炸……
    //    if (!TileSystem.Enable || sItem.pick == 0)
    //    {
    //        orig(self, sItem, out canHitWalls, x, y);
    //        return;
    //    }

    //    CPoint mouse = new CPoint(Main.MouseWorld);
    //    foreach (var tile in TileSystem.GetTiles<IPickable>())
    //    {
    //        IDynamicTile dynamicTile = tile as IDynamicTile;
    //        if (dynamicTile.Collision(mouse))
    //        {
    //            tile.PickTile(sItem.pick);
    //            canHitWalls = false;
    //            return;
    //        }
    //    }
    //    orig(self, sItem, out canHitWalls, x, y);
    //}
}
