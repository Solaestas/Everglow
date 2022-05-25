using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

internal class ProjColliding : GlobalProjectile
{
    public static HashSet<Projectile> callFromHook = new HashSet<Projectile>();
    public override bool InstancePerEntity => true;
    public IDynamicTile standTile;
    public override void Load()
    {
        On.Terraria.Projectile.AI_007_GrapplingHooks += Projectile_AI_007_GrapplingHooks_On;
        On.Terraria.Projectile.HandleMovement += Projectile_HandleMovement;
        try
        {
            IL.Terraria.Projectile.AI_007_GrapplingHooks += Projectile_AI_007_GrapplingHooks_IL;
        }
        catch (Exception ex)
        {
            ILException.Throw("AI_007_GrapplingHooks_Error", ex);
        }
    }

    private static void Projectile_HandleMovement(On.Terraria.Projectile.orig_HandleMovement orig, Projectile self, Vector2 wetVelocity, out int overrideWidth, out int overrideHeight)
    {
        if (!TileSystem.Enable || !self.tileCollide || self.aiStyle == 7)
        {
            orig(self, wetVelocity, out overrideWidth, out overrideHeight);
            return;
        }

        orig(self, wetVelocity, out overrideWidth, out overrideHeight);
        TileSystem.EnableDTCollision = false;
        var proj = self.GetGlobalProjectile<ProjColliding>();
        Vector2 last = self.position;
        Vector2 move = self.position - last;
        self.position = last;
        if (proj.standTile is not null)
        {
            if (proj.standTile.Active is false || !proj.standTile.OnTile(self, true))
            {
                self.position -= self.velocity;
                proj.standTile.StandingLeaving(self);
                self.position += self.velocity;
                proj.standTile = null;
                self.position.Y += 0.001f;
            }
            else
            {
                proj.standTile.StandingMoving(self);
            }
        }
        var result = TileSystem.MoveColliding(self, move, true);

        foreach (var (tile, info) in result)
        {
            if (info == Direction.Bottom)
            {
                proj.standTile = tile;
                tile.StandingBegin(self);
            }
        }

        TileSystem.EnableDTCollision = true;
    }
    private static void Projectile_AI_007_GrapplingHooks_On(On.Terraria.Projectile.orig_AI_007_GrapplingHooks orig, Projectile self)
    {
        if (!TileSystem.Enable)
        {
            orig(self);
            return;
        }

        Player player = Main.player[self.owner];
        int numHooks = 3;
        if (self.type == 165)
        {
            numHooks = 8;
        }
        else if (self.type == 256)
        {
            numHooks = 2;
        }
        else if (self.type == 372)
        {
            numHooks = 2;
        }
        else if (self.type == 652)
        {
            numHooks = 1;
        }
        else if (self.type >= 646 && self.type <= 649)
        {
            numHooks = 4;
        }

        int grapCount = 0;
        int leastTime = int.MaxValue;
        int leastIndex = self.whoAmI;
        foreach (var proj in Main.projectile)
        {
            if (proj.active && proj.aiStyle == 7 && proj.owner == self.owner)
            {
                grapCount++;
                if (proj.timeLeft < leastTime)
                {
                    leastTime = proj.timeLeft;
                    leastIndex = proj.whoAmI;
                }
            }
        }
        ProjectileLoader.NumGrappleHooks(self, player, ref numHooks);
        if (grapCount > numHooks)
        {
            Main.projectile[leastIndex].Kill();
        }
        if (self.ai[0] != 1)
        {
            var gproj = self.GetGlobalProjectile<ProjColliding>();
            if (gproj.standTile is null)
            {
                foreach (var tile in TileSystem.DynamicTiles)
                {
                    if (tile is IHookable hookable)
                    {
                        CRectangle c = self.GetCollider();
                        if (tile.Collision(c))
                        {
                            self.position = hookable.GetSafeHookPosition(self);
                            self.position += hookable.GetHookMovement(self);
                            if (self.type == 935 && self.alpha == 0 && Main.myPlayer == self.owner)
                            {
                                player.DoQueenSlimeHookTeleport(hookable.GetSafePlayerPosition(self));
                                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, self.owner);
                            }
                            callFromHook.Add(self);
                            self.ai[0] = 2;
                            self.netUpdate = true;
                            self.velocity *= 0;
                            if (self.alpha == 0)
                            {
                                self.alpha = 1;
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, self.Center);
                            }
                            gproj.standTile = tile;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (gproj.standTile.Active)
                {
                    callFromHook.Add(self);
                    self.position += (gproj.standTile as IHookable).GetHookMovement(self);
                }
                else
                {
                    gproj.standTile = null;
                }
            }
        }
        orig(self);
        callFromHook.Remove(self);
    }
    private static void Projectile_AI_007_GrapplingHooks_IL(ILContext il)
    {
        //TODO offset多次修改会炸，要改
        var cursor = new ILCursor(il);
        if (!cursor.TryGotoNext(MoveType.After, ins => (ins.Previous?.MatchLdcI4(1) ?? false) &&
            ins.MatchStloc(44) &&
            (ins.Next?.MatchLdsflda<Main>(nameof(Main.tile)) ?? false)))
        {
            ILException.Throw("Projectile_AI_007_GrapplingHooks_NotFound_0");
        }
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.EmitDelegate((Projectile proj) => callFromHook.Contains(proj));
        var label = il.DefineLabel();
        cursor.Emit(OpCodes.Brtrue, label);
        if (!cursor.TryGotoNext(MoveType.Before, ins => ins.MatchLdsfld<Main>(nameof(Main.player)) &&
            ins.Previous.MatchRet()))
        {
            ILException.Throw("Projectile_AI_007_GrapplingHooks_NotFound_1");
        }
        cursor.MarkLabel(label);
    }
}
