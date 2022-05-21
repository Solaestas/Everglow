using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.ZY.WorldModule;
using Everglow.Sources.Modules.ZY.ZYPacket;

namespace Everglow.Sources.Modules.ZY.Common;

[ProfilerMeasure]
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
