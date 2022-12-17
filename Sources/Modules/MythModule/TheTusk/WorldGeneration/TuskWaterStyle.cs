using static Terraria.ModLoader.ModContent;

namespace Everglow.Sources.Modules.MythModule.TheTusk.WorldGeneration
{
    public class TuskWaterStyle : ModWaterStyle
    {
        public override int ChooseWaterfallStyle() => Find<ModWaterfallStyle>("Everglow/Sources/Modules/MythModule/TheTusk/WorldGeneration/TuskWaterfallStyle").Slot;

        public override int GetSplashDust() => DustID.BloodWater;

        public override int GetDropletGore() => Find<ModGore>("Everglow/Sources/Modules/MythModule/TheTusk/Gores/StonePanBreak8").Type;

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override Color BiomeHairColor()
            => Color.Crimson;
    }
}