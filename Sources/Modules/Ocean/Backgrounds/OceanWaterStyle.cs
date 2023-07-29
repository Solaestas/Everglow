using static Terraria.ModLoader.ModContent;

namespace Everglow.Ocean.Backgrounds.Ocean
{
    public class OceanWaterStyle : ModWaterStyle
    {
		public override int ChooseWaterfallStyle() => Find<ModWaterfallStyle>("Everglow.Ocean.OceanWaterfallStyle").Slot;

		public override int GetSplashDust() => DustID.Snow;

        public override int GetDropletGore() => Find<ModGore>("Everglow.Ocean.AncientTangerineTreeGore5").Type;

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 0.1f;
            g = 0.6f;
            b = 0.9f;
        }

        public override Color BiomeHairColor()
            => new Color(28, 200, 245, 159);
    }
}