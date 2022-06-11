using static Terraria.ModLoader.ModContent;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Water
{
    public class FireflyWaterStyle : ModWaterStyle
    {
        public override int ChooseWaterfallStyle() => Find<ModWaterfallStyle>("Everglow/FireflyWaterfallStyle").Slot;

        public override int GetSplashDust() => DustType<MothBlue2>();

        public override int GetDropletGore() => base.Slot;

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 0.12f;
            g = 0.25f;
            b = 1.0f;
        }

        public override Color BiomeHairColor()
            => new Color(11, 0, 38, 255);
    }
}