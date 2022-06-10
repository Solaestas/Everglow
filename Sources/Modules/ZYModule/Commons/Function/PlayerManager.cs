using Everglow.Sources.Modules.ZYModule.WorldModule;
using Everglow.Sources.Modules.ZYModule.ZYPacket;
using Terraria.GameInput;

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
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        ControlLeft.Update(Player.controlLeft);
        ControlRight.Update(Player.controlRight);
        ControlUp.Update(Player.controlUp);
        ControlDown.Update(Player.controlDown);
        ControlJump.Update(Player.controlJump);
        ControlUseItem.Update(Player.controlUseItem);
        ControlUseTile.Update(Player.controlUseTile);
        if (Main.myPlayer == Player.whoAmI)
        {
            MouseLeft.Update(Main.mouseLeft);
            MouseRight.Update(Main.mouseRight);
            MouseWorld = Main.MouseWorld;
            Everglow.PacketResolver.Send<MousePacketToServer>();
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
}
