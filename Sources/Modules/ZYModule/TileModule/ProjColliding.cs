using Everglow.Sources.Modules.ZY.Commons.Function;
using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Everglow.Sources.Modules.ZYModule.TileModule;

internal class ProjColliding : GlobalProjectile
{
    public static HashSet<Projectile> callFromHook = new HashSet<Projectile>();
    public override bool InstancePerEntity => true;
    public IDynamicTile standTile;
    public override void Load()
    {
        On.Terraria.Projectile.AI_007_GrapplingHooks += Projectile_AI_007_GrapplingHooks;
        IL.Terraria.Projectile.AI_007_GrapplingHooks += Projectile_AI_007_GrapplingHooks1;
        On.Terraria.Projectile.HandleMovement += Projectile_HandleMovement;
    }

    internal static void Projectile_HandleMovement(On.Terraria.Projectile.orig_HandleMovement orig, Projectile self, Vector2 wetVelocity, out int overrideWidth, out int overrideHeight)
    {
        if (TileSystem.Enable && self.tileCollide && self.aiStyle != 7)
        {
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
            return;
        }
        orig(self, wetVelocity, out overrideWidth, out overrideHeight);
    }
    internal static void Projectile_AI_007_GrapplingHooks(On.Terraria.Projectile.orig_AI_007_GrapplingHooks orig, Projectile self)
    {
        if (TileSystem.Enable)
        {
            Player player = Main.player[self.owner];
            int numHooks = 3;
            //time to replicate retarded vanilla hardcoding, wheee
            if (self.type == 165)
            {
                numHooks = 8;
            }

            if (self.type == 256)
            {
                numHooks = 2;
            }

            if (self.type == 372)
            {
                numHooks = 2;
            }

            if (self.type == 652)
            {
                numHooks = 1;
            }

            if (self.type >= 646 && self.type <= 649)
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
            var gproj = self.GetGlobalProjectile<ProjColliding>();
            if (!gproj.standTile.Active)
            {
                gproj.standTile = null;
            }
            if (gproj.standTile is null)
            {
                foreach (var tile in TileSystem.DynamicTiles)
                {
                    if (self.ai[0] != 1 && tile is IHookable hookable)
                    {
                        CRectangle c = self.GetCollider();
                        if (tile.Collision(c))
                        {
                            self.position = hookable.GetSafeHookPosition(self);
                            self.position += hookable.GetHookMovement(self);
                            if (self.type == 935 && self.alpha == 0)
                            {
                                player.DoQueenSlimeHookTeleport(hookable.GetSafePlayerPosition(self));
                                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, self.owner);
                            }
                            callFromHook.Add(self);
                            self.ai[0] = 2;
                            self.netUpdate = true;
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
                callFromHook.Add(self);
                self.position += (gproj.standTile as IHookable).GetHookMovement(self);
            }
            orig(self);
            callFromHook.Remove(self);
            return;
        }
        orig(self);
    }
    internal static void Projectile_AI_007_GrapplingHooks1(ILContext il)
    {
        //TODO offset多次修改会炸，要改
        var cursor = new ILCursor(il);
        cursor.TryGotoNext(MoveType.After, ins => ins.Offset == 0x0E6F);
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.EmitDelegate((Projectile proj) => callFromHook.Contains(proj));
        var label = il.DefineLabel();
        cursor.Emit(OpCodes.Brtrue, label);
        cursor.TryGotoNext(MoveType.Before, ins => ins.Offset == 0x0EC9);
        cursor.MarkLabel(label);
    }
}
