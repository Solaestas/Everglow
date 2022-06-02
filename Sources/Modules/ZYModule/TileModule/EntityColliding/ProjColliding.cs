using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;
using Mono.Cecil.Cil;
using MonoMod.Cil;

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
    public override void Update()
    {
        if (attachTile is not null)
        {
            Entity.position += new Vector2(0, Entity.gfxOffY);
            Entity.gfxOffY = 0;
        }
        base.Update();
    }
    public override void OnAttach()
    {
        Entity.velocity.Y = 0;
    }
}
internal class ProjColliding : GlobalProjectile
{
    public ProjHandler handler;
    public static HashSet<Projectile> callFromHook = new HashSet<Projectile>();
    public const int HookAiStyle = 7;
    public override bool InstancePerEntity => true;
    protected override bool CloneNewInstances => true;
    public override bool IsCloneable => true;
    public override GlobalProjectile Clone(Projectile from, Projectile to)
    {
        var clone = base.Clone(from, to) as ProjColliding;
        clone.handler = new ProjHandler(to);
        return clone;
    }
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
        if (!TileSystem.Enable || !self.tileCollide || self.aiStyle == HookAiStyle)
        {
            orig(self, wetVelocity, out overrideWidth, out overrideHeight);
            return;
        }


        TileSystem.EnableDTCollision = false;
        orig(self, wetVelocity, out overrideWidth, out overrideHeight);
        self.GetGlobalProjectile<ProjColliding>().handler.Update();
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
            if (gproj.handler.attachTile is null)
            {
                foreach (var tile in TileSystem.DynamicTiles)
                {
                    if (tile is IHookable hookable)
                    {
                        CAABB c = new CAABB(new AABB(self.position, self.Size));
                        if (tile.Collision(c))
                        {
                            hookable.SetHookPosition(self);
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
                            gproj.handler.attachTile = tile;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (gproj.handler.attachTile.Active)
                {
                    callFromHook.Add(self);
                    (gproj.handler.attachTile as IHookable).SetHookPosition(self);
                }
                else
                {
                    gproj.handler.attachTile = null;
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
