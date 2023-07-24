using Terraria.Localization;

namespace MythMod.OceanMod.Backgrounds
{
    //Shows setting up two basic biomes. For a more complicated example, please request.
    public class TownSurfaceBiome : ModBiome
    {
        //public override bool IsPrimaryBiome => true; // Allows this biome to impact NPC prices

        // Select all the scenery
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("MythMod/OceanWaterStyle"); // Sets a water style for when inside this biome
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("MythMod/TownSurfaceBackgroundStyle");
        //public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

        // Select Music
        //public override int Music => MusicLoader.GetMusicSlot("ExampleMod/Assets/Music/MysteriousMystery");

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Marine Town");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "海滨渔村");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            bool b1 = Main.ActiveWorldFileData.Path.Contains("OcEaNMyTh");
            bool b2 = Main.LocalPlayer.position.X / 16d >= Main.maxTilesX * 0.8778 && Main.LocalPlayer.position.X / 16d <= Main.maxTilesX * 0.9456;
            return b1 && b2;
        }
        public override float GetWeight(Player player)
        {
            return 0f;
        }
        public override SceneEffectPriority Priority => base.Priority;
    }
}
