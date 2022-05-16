using Everglow.Sources.Modules.ZY.WorldSystem;
using Everglow.Sources.Modules.ZY.ZYPacket;

namespace Everglow.Sources.Modules.ZY.Common
{
    internal class PlayerManager : ModPlayer
    {
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (newPlayer)
            {
                Everglow.PacketResolver.Send<WorldVersionPacket>();
            }
        }
    }
}
