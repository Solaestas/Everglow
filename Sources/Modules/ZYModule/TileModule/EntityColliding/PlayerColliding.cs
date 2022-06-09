using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Mono.Cecil.Cil;
using MonoMod.Cil;
namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;
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
            IL.Terraria.Player.WallslideMovement += Player_WallslideMovement_IL;
        }
        catch (Exception ex)
        {
            ILException.Throw("Player_WallslideMovement_Error", ex);
        }
        On.Terraria.Player.WallslideMovement += Player_WallslideMovement_On;

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
        TileSystem.EnableCollisionHook = false;
        var player = self.GetModPlayer<PlayerColliding>();
        player.handler.position = self.position;//记录位置，否则会把传送当成位移
        orig(self, fallThrough, ignorePlats);
        self.GetModPlayer<PlayerColliding>().handler.Update(ignorePlats || fallThrough);
        TileSystem.EnableCollisionHook = true;
    }
    private static void Player_WaterCollision(On.Terraria.Player.orig_WaterCollision orig, Player self, bool fallThrough, bool ignorePlats)
    {
        if (!TileSystem.Enable || self.ghost)
        {
            orig(self, fallThrough, ignorePlats);
            return;
        }

        TileSystem.EnableCollisionHook = false;
        var player = self.GetModPlayer<PlayerColliding>();
        player.handler.position = self.position;//记录位置，否则会把传送当成位移
        orig(self, fallThrough, ignorePlats);
        self.GetModPlayer<PlayerColliding>().handler.Update(ignorePlats || fallThrough);
        TileSystem.EnableCollisionHook = true;
    }
    private static void Player_HoneyCollision(On.Terraria.Player.orig_HoneyCollision orig, Player self, bool fallThrough, bool ignorePlats)
    {
        if (!TileSystem.Enable || self.ghost)
        {
            orig(self, fallThrough, ignorePlats);
            return;
        }

        TileSystem.EnableCollisionHook = false;
        var player = self.GetModPlayer<PlayerColliding>();
        player.handler.position = self.position;//记录位置，否则会把传送当成位移
        orig(self, fallThrough, ignorePlats);
        self.GetModPlayer<PlayerColliding>().handler.Update(ignorePlats || fallThrough);
        TileSystem.EnableCollisionHook = true;
    }
    private static bool Player_CanFitSpace(On.Terraria.Player.orig_CanFitSpace orig, Player self, int heightBoost)
    {
        TileSystem.EnableCollisionHook = false;
        bool flag = orig(self, heightBoost);
        TileSystem.EnableCollisionHook = true;
        return flag;
    }
    private static void Player_WallslideMovement_IL(ILContext il)
    {
        var cursor = new ILCursor(il);
        var skipControlCheck = cursor.DefineLabel();
        var skipSetFlag = cursor.DefineLabel();
        if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchStfld<Player>("sliding")))
        {
            ILException.Throw("Player_WallslideMovement_NotFound_0");
        }
        MethodInfo method = null;
        foreach(var m in typeof(Player).GetMethods(BindingFlags.Instance | BindingFlags.Public))
        {
            if(m.Name == "GetModPlayer" && m.GetParameters().Length == 0)
            {
                method = m;
            }
        }
        Debug.Assert(method != null);
        //if(player.GetModPlayer<PlayerColliding>().handler.attachType == AttachType.Grab)
        //{
        //    flag = true;
        //    goto
        //}
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.Emit(OpCodes.Call, method.MakeGenericMethod(typeof(PlayerColliding)));
        cursor.Emit(OpCodes.Ldfld, typeof(PlayerColliding).GetField(nameof(handler)));
        cursor.Emit(OpCodes.Ldfld, typeof(EntityHandler).GetField(nameof(EntityHandler.attachType)));
        cursor.Emit(OpCodes.Ldc_I4, (int)AttachType.Grab);
        cursor.Emit(OpCodes.Bne_Un, skipSetFlag);
        cursor.Emit(OpCodes.Ldc_I4, 1);
        cursor.Emit(OpCodes.Stloc_0);
        cursor.Emit(OpCodes.Br, skipControlCheck);
        cursor.MarkLabel(skipSetFlag);

        if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchStloc(0) && ins.Previous.MatchLdcI4(0)))
        {
            ILException.Throw("Player_WallslideMovement_NotFound_1");
        }
        cursor.MarkLabel(skipControlCheck);
    }
    private static void Player_WallslideMovement_On(On.Terraria.Player.orig_WallslideMovement orig, Player self)
    {
        TileSystem.EnableCollisionHook = false;
        orig(self);
        TileSystem.EnableCollisionHook = true;
    }
}
