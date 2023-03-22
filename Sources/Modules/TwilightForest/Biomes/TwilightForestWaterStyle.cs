using Everglow.TwilightForest.Dusts;
using ReLogic.Content;

namespace Everglow.TwilightForest.Biomes
{
	public class TwilightForestWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle()
		{
			return ModContent.Find<ModWaterfallStyle>("Everglow/TwilightForestWaterfallStyle").Slot;
		}

		public override int GetSplashDust()
		{
			return ModContent.DustType<TwilightWaterSplash>();
		}

		public override int GetDropletGore()
		{
			return GoreID.AmbientAirborneCloud1;
		}

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 0.13f;
			g = 0f;
			b = 1f;
		}

		public override Color BiomeHairColor()
		{
			return new Color(0.4f, 0, 0.7f);
		}

		public override byte GetRainVariant()
		{
			return (byte)Main.rand.Next(3);
		}

		public override Asset<Texture2D> GetRainTexture()
		{
			return ModContent.Request<Texture2D>("Everglow/TwilightForest/Biomes/TwilightForestRain");
		}
	}
}