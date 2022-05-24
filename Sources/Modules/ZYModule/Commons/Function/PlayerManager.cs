using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.ZY.WorldModule;
using Everglow.Sources.Modules.ZY.ZYPacket;

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
