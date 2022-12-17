namespace Everglow.Sources.Modules.MythModule.TheTusk.WorldGeneration
{
    public class TuskWaterfallStyle : ModWaterfallStyle
    {
        // Makes the waterfall provide light
        // Learn how to make a waterfall: https://terraria.gamepedia.com/Waterfall
        public override void AddLight(int i, int j) =>
            Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), Color.White.ToVector3() * 0.5f);
    }
}