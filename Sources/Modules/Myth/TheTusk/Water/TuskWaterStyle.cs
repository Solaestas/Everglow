using static Terraria.ModLoader.ModContent;

namespace Everglow.Myth.TheTusk.Water;

public class TuskWaterStyle : ModWaterStyle
{
	public override int ChooseWaterfallStyle() => GetInstance<TuskWaterfallStyle>().Slot;

	public override int GetSplashDust() => DustID.Blood;

	public override int GetDropletGore() => Slot;

	public override void LightColorMultiplier(ref float r, ref float g, ref float b)
	{
		r = 0.8f;
		g = 0.3f;
		b = 0.0f;
	}

	public override Color BiomeHairColor()
		=> new Color(255, 0, 20, 255);
}