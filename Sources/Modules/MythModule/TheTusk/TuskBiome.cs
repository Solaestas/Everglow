using Terraria.Graphics.Capture;
using Everglow.Sources.Modules.MythModule.TheTusk.WorldGeneration;
using Everglow.Sources.Modules.MythModule.TheTusk.Backgrounds;

namespace Everglow.Sources.Modules.MythModule.TheTusk
{
    public class TuskBiomeBG : ModSurfaceBackgroundStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
        }
    }

    public class TuskBiome : ModBiome
    {
        public override int Music => Common.MythContent.QuickMusic("TuskBiome");
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override string BestiaryIcon => "Everglow/Sources/Modules/MythModule/TheTusk/TuskIcon";
        public override string BackgroundPath => base.BackgroundPath;
        public override string MapBackground => "Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/TuskBackground_Inside";
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.TuskWaterStyle>();
        public override Color? BackgroundColor => base.BackgroundColor;
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<TuskSurfaceBackgroundStyle>();
        //public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("Everglow/MothUndergroundBackground");

        public override void SetStaticDefaults()
        {
            //TODO:����֮�
        }

        public override bool IsBiomeActive(Player player)
        {
            bool b1 = TuskGen.TuskLandActive();
            return b1;
        }

        public override void OnInBiome(Player player)
        {
            base.OnInBiome(player);
        }
    }
}