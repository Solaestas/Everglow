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
        }
        public override Color MapColor => new Color(122, 91, 79);
        public override void Draw()
        {
            Main.spriteBatch.Draw(YggdrasilContent.QuickTexture("Common/Elevator/SkyTreeLiftLarge"), position - Main.screenPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
        public override void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
        {
            base.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
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