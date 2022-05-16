using Everglow.Sources.Modules.ZY.WorldSystem;

namespace Everglow.Sources.Modules.ZY.Common
{
    internal class PlayerManager : ModPlayer
    {
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (newPlayer)
            {
                Everglow.PacketResolver.Send(new WorldVersionPacket());
            }
        }
    }
}
