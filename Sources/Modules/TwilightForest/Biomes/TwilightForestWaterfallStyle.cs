namespace Everglow.TwilightForest.Biomes
{
	public class TwilightForestWaterfallStyle : ModWaterfallStyle
	{
		public override void AddLight(int i, int j) =>
			Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), new Vector3(0.1f, 0f, 0.9f));
	}
}