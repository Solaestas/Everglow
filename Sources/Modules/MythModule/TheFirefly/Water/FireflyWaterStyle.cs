using static Terraria.ModLoader.ModContent;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Water
{
    public class FireflyWaterStyle : ModWaterStyle
    {
        public override int ChooseWaterfallStyle() => GetInstance<FireflyWaterfallStyle>().Slot;

        public override int GetSplashDust() => DustType<MothBlue2>();

        public override int GetDropletGore() => base.Slot;

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 0.8f;
            g = 0.9f;
            b = 1.01f;
        }

        public override Color BiomeHairColor()
            => new Color(11, 0, 38, 255);
    }
}