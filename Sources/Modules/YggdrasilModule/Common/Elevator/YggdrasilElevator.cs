using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

namespace Everglow.Sources.Modules.YggdrasilModule.Common.Elevator
{
    internal class YggdrasilElevator : DynamicTile
    {
        public override Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
        {
            throw new NotImplementedException();
        }
    }
}