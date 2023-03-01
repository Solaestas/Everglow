using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    internal class MythTileInteractionPlayer : ModPlayer
    {
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (newPlayer)
            {
                //新玩家进入世界是发送请求
                Everglow.PacketResolver.Send(new MothPositionPacket());
            }
        }
    }
}