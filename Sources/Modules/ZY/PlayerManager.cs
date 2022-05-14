using Everglow;
using Everglow.Sources.Commons.Core;
namespace Everglow.Sources.Modules.ZY
{
    internal class PlayerManager : ModPlayer
    {
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (newPlayer)
            {
                Debug.Assert(Main.netMode == NetmodeID.MultiplayerClient);
                var pack = Everglow.Instance.GetPacket();
                pack.Write((byte)PackageType.WorldType);
                pack.Send();
            }
        }
    }
}
