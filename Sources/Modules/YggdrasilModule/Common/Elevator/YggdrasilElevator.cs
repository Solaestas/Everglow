using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

namespace Everglow.Sources.Modules.YggdrasilModule.Common.Elevator
{
    internal class YggdrasilElevator : DBlock
    {
        public override void AI()
        {
            size = new Vector2(192, 32);
            velocity = new Vector2(0, -0.5f);
            base.AI();
        }
    }
    public class ElevatorSummonSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if(Main.mouseRight && Main.mouseRightRelease)
            {
               
            }
        }
    }
}