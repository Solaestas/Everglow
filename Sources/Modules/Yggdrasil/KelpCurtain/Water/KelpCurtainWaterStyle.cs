using static Terraria.ModLoader.ModContent;
using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Water;

public class KelpCurtainWaterStyle : ModWaterStyle
{
	public override int ChooseWaterfallStyle() => GetInstance<KelpCurtainWaterfallStyle>().Slot;

	public override int GetSplashDust() => ModContent.DustType<KelpCurtainWater>();

	public override int GetDropletGore() => base.Slot;

	public override void LightColorMultiplier(ref float r, ref float g, ref float b)
	{
		r = 0.05f;
		g = 0.6f;
		b = 0.05f;
		base.LightColorMultiplier(ref r, ref g, ref b);
	}

	public override Color BiomeHairColor() => new Color(0, 150, 40);
}