using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

internal class PlayerColliding : ModPlayer
{
    public DynamicTile standTile;
    public DynamicTile grabTile;

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
    public override void PostUpdateRunSpeeds()
    {
        if(standTile != null)
        {
            Player.gravity = 0;
        }
    }
    private static void Player_DryCollision(On.Terraria.Player.orig_DryCollision orig, Player self, bool fallThrough, bool ignorePlats)
    {
        if (!TileSystem.Enable || self.ghost)
        {
            orig(self, fallThrough, ignorePlats);
            return;
        }
        TileSystem.EnableDTCollision = false;

        var player = self.GetModPlayer<PlayerColliding>();
        if (player.standTile is not null)
        {
            self.position += new Vector2(0, self.gfxOffY);
            self.gfxOffY = 0;
        }
        Vector2 oldpos = self.position;

        orig(self, fallThrough, ignorePlats);
        Vector2 move = self.position - oldpos;
        self.position = oldpos;
        var result = TileSystem.MoveCollision(self, move, fallThrough);
        DynamicTile newStand = null;
        if (player.standTile != null && !player.standTile.Active)
        {
            player.standTile = null;
        }

        foreach (var (tile, info) in result)
        {
            if (info == Direction.Inside)
            {
                self.Hurt(PlayerDeathReason.ByCustomReason("物块内部"), 10, 1);
            }
            Direction GroundEdge = self.gravDir > 0 ? Direction.Bottom : Direction.Top;
            if (info == GroundEdge && self.grapCount == 0 && !(self.mount.Active && self.mount.CanFly()))
            {
                newStand = tile;
            }
        }

        if (newStand == null)
        {
            if (player.standTile != null)
            {
                player.standTile.Leave(self);
                self.GetModPlayer<PlayerManager>().Jump();
                player.standTile = null;
            }
        }
        else if (player.standTile != newStand)
        {
            player.standTile?.Leave(self);
            newStand.Stand(self, true);
            player.standTile = newStand;
            self.velocity.Y = 0;
        }
        else if (player.standTile != null && player.standTile == newStand)
        {
            self.velocity.Y = 0;
            newStand.Stand(self, false);
        }
        Main.NewText(self.velocity);
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

        var player = self.GetModPlayer<PlayerColliding>();
        if (player.standTile is not null)
        {
            self.position += new Vector2(0, self.gfxOffY);
            self.gfxOffY = 0;
        }
        Vector2 oldpos = self.position;

        orig(self, fallThrough, ignorePlats);
        Vector2 move = self.position - oldpos;
        self.position = oldpos;
        var result = TileSystem.MoveCollision(self, move, fallThrough);
        DynamicTile newStand = null;
        if (player.standTile != null && !player.standTile.Active)
        {
            player.standTile = null;
        }

        foreach (var (tile, info) in result)
        {
            if (info == Direction.Inside)
            {
                self.Hurt(PlayerDeathReason.ByCustomReason("物块内部"), 10, 1);
            }
            Direction GroundEdge = self.gravDir > 0 ? Direction.Bottom : Direction.Top;
            if (info == GroundEdge && self.grapCount == 0 && !(self.mount.Active && self.mount.CanFly()))
            {
                player.standTile = tile;
            }
        }

        if (newStand == null)
        {
            if (player.standTile != null)
            {
                player.standTile.Leave(self);
                self.GetModPlayer<PlayerManager>().Jump();
            }
        }
        else if (player.standTile != newStand)
        {

            player.standTile.Leave(self);
            self.GetModPlayer<PlayerManager>().Jump();
            newStand.Stand(self, true);
            player.standTile = newStand;
        }
        else if (player.standTile == newStand)
        {
            newStand.Stand(self, false);
        }

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

        var player = self.GetModPlayer<PlayerColliding>();
        if (player.standTile is not null)
        {
            self.position += new Vector2(0, self.gfxOffY);
            self.gfxOffY = 0;
        }
        Vector2 oldpos = self.position;

        orig(self, fallThrough, ignorePlats);
        Vector2 move = self.position - oldpos;
        self.position = oldpos;
        var result = TileSystem.MoveCollision(self, move, fallThrough);
        DynamicTile newStand = null;
        if (player.standTile != null && !player.standTile.Active)
        {
            player.standTile = null;
        }

        foreach (var (tile, info) in result)
        {
            if (info == Direction.Inside)
            {
                self.Hurt(PlayerDeathReason.ByCustomReason("物块内部"), 10, 1);
            }
            Direction GroundEdge = self.gravDir > 0 ? Direction.Bottom : Direction.Top;
            if (info == GroundEdge && self.grapCount == 0 && !(self.mount.Active && self.mount.CanFly()))
            {
                player.standTile = tile;
            }
        }

        if (newStand == null)
        {
            if (player.standTile != null)
            {
                player.standTile.Leave(self);
                self.GetModPlayer<PlayerManager>().Jump();
            }
        }
        else if (player.standTile != newStand)
        {
            player?.standTile.Leave(self);
            self.GetModPlayer<PlayerManager>().Jump();
            newStand.Stand(self, true);
            player.standTile = newStand;
        }
        else if (player.standTile == newStand)
        {
            newStand.Stand(self, false);
        }

        if(player.standTile != null)
        {
            self.velocity.Y = 0;
        }

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
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.EmitDelegate((Player player) =>
        {
            player.slideDir = (int)player.GetControlDirectionH().ToVector2().X;
        });
        if (!cursor.TryGotoNext(MoveType.Before, ins => ins.MatchRet()))
        {
            ILException.Throw("Player_WallslideMovement_NotFound_0");
        }
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.EmitDelegate((Player player) =>
        {
            var modplayer = player.GetModPlayer<PlayerColliding>();
            if (modplayer.grabTile is not null)
            {
                (modplayer.grabTile as IGrabbable).EndGrab(player);
                modplayer.grabTile = null;
            }
        });

        //cursor.GotoNext(MoveType.Before, ins => ins.OpCode == OpCodes.Ldc_I4_0);
        //cursor.GotoNext(MoveType.Before, ins => ins.OpCode == OpCodes.Stloc_0);
        //cursor.GotoNext(MoveType.Before, ins => ins.OpCode == OpCodes.Ldarg_0);
        if (!cursor.TryGotoNext(MoveType.Before, ins => ins.MatchLdcI4(0) &&
            (ins.Next?.MatchStloc(0) ?? false) &&
            (ins.Next.Next?.MatchLdarg(0) ?? false)))
        {
            ILException.Throw("Player_WallslideMovement_NotFound_1");
        }
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.EmitDelegate((Player player) =>
        {
            var modplayer = player.GetModPlayer<PlayerColliding>();
            foreach (var grabbable in TileSystem.GetTiles<IGrabbable>())
            {
                if (grabbable.CanGrab(player))
                {
                    modplayer.grabTile = grabbable as DynamicTile;
                    grabbable.OnGrab(player);
                    return true;
                }
            }
            if (modplayer.grabTile is not null)
            {
                (modplayer.grabTile as IGrabbable).EndGrab(player);
                modplayer.grabTile = null;
            }
            return false;
        });
        cursor.Emit(OpCodes.Stloc_0);//flag
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
