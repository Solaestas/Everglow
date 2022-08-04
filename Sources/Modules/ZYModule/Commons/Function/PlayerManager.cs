using Everglow.Sources.Modules.ZYModule.Items;
using Everglow.Sources.Modules.ZYModule.ZYPacket;
using Terraria.Audio;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function;


internal class PlayerManager : ModPlayer
{
    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        if (newPlayer)
        {
            Everglow.PacketResolver.Send<WorldVersionPacket>();
        }
    }

    public VirtualKey ControlLeft { get; private set; } = new VirtualKey();
    public VirtualKey ControlRight { get; private set; } = new VirtualKey();
    public VirtualKey ControlUp { get; private set; } = new VirtualKey();
    public VirtualKey ControlDown { get; private set; } = new VirtualKey();
    public VirtualKey ControlJump { get; private set; } = new VirtualKey();
    public VirtualKey ControlUseItem { get; private set; } = new VirtualKey();
    public VirtualKey ControlUseTile { get; private set; } = new VirtualKey();
    public VirtualKey MouseLeft { get; private set; } = new VirtualKey();
    public VirtualKey MouseRight { get; private set; } = new VirtualKey();
    public Vector2 MouseWorld { get; internal set; }
    public override void PostUpdate()
    {
        ControlLeft.Update(Player.controlLeft);
        ControlRight.Update(Player.controlRight);
        ControlUp.Update(Player.controlUp);
        ControlDown.Update(Player.controlDown);
        ControlJump.Update(Player.controlJump);
        ControlUseItem.Update(Player.controlUseItem);
        if (Main.myPlayer == Player.whoAmI)
        {
            MouseLeft.Update(Main.mouseLeft);
            MouseRight.Update(Main.mouseRight);
            ControlUseTile.Update(Player.controlUseTile);
            MouseWorld = Main.MouseWorld;
            Everglow.PacketResolver.Send<InputPacketToServer>();
        }
    }
    private float jumpSpeed;
    private int jumpTime;
    public void Jump() => Jump(Player.jump, Player.velocity.Y);
    public void Jump(int time, float speed)
    {
        jumpTime = time;
        jumpSpeed = speed;
    }
    public override void Load()
    {
        On.Terraria.Player.JumpMovement += Player_JumpMovement;
    }
    internal static void Player_JumpMovement(On.Terraria.Player.orig_JumpMovement orig, Player self)
    {
        var player = self.GetModPlayer<PlayerManager>();
        if (player.jumpTime > 0)
        {
            if (self.jump != 0)
            {
                self.jump = 0;
            }
            if (!self.controlJump)
            {
                player.jumpTime = 1;
            }
            self.velocity.Y = player.jumpSpeed;
            player.jumpTime--;
        }
        orig(self);
    }


    public WoodShieldProj shield;
    public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
    {
        if (shield is null)
        {
            return;
        }
        if (!shield.Projectile.active)
        {
            shield = null;
            return;
        }
        if (shield.IsDefending && npc.Hitbox.Intersects(shield.Projectile.Hitbox))
        {
            crit = false;
            shield.DefendDamage(npc, ref damage);
        }
    }
    public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
    {
        if (shield is null)
        {
            return;
        }
        if (!shield.Projectile.active)
        {
            shield = null;
            return;
        }
        if (shield.IsDefending && proj.Colliding(proj.Hitbox, shield.Projectile.Hitbox))
        {
            crit = false;
            shield.DefendDamage(proj, ref damage);
        }
    }
}
