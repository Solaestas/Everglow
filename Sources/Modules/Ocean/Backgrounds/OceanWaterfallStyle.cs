namespace MythMod.OceanMod.Backgrounds.Ocean
{
    public class OceanWaterfallStyle : ModWaterfallStyle
    {
        public override void AddLight(int i, int j) =>
            Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), new Vector3(28, 200, 245));
    }
}