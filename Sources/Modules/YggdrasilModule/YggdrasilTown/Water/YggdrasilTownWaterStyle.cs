using static Terraria.ModLoader.ModContent;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Water
{
    public class YggdrasilTownWaterStyle : ModWaterStyle
    {
        public override int ChooseWaterfallStyle() => GetInstance<YggdrasilTownWaterfallStyle>().Slot;

        public override int GetSplashDust() => ModContent.DustType<Dusts.YggdrasilTownWater>();

        public override int GetDropletGore() => base.Slot;

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 0.8f;
            g = 0.9f;
            b = 1.01f;
        }
        public override Color BiomeHairColor() => new Color(74, 100, 153);
    }
}