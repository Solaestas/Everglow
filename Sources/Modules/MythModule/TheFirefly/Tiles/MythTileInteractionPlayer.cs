using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
