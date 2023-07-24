using Terraria.Localization;

namespace MythMod.OceanMod.Backgrounds
{
    public class OceanSurfaceBiome : ModBiome
    {
        //public override bool IsPrimaryBiome => true; // Allows this biome to impact NPC prices

        // Select all the scenery
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("MythMod/OceanWaterStyle"); // Sets a water style for when inside this biome
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("MythMod/OceanSurfaceBgStyle");
        //public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

        // Select Music
        public override int Music => MusicLoader.GetMusicSlot("MythMod/Musics/Ocean");

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Far Ocean");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "远洋");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            bool b1 = Main.ActiveWorldFileData.Path.Contains("OcEaNMyTh");
            bool b2 = Main.LocalPlayer.position.X / 16d <= Main.maxTilesX * 0.8778 || Main.LocalPlayer.position.X / 16d >= Main.maxTilesX * 0.9456;
            bool b3 = !Common.Players.MythPlayer.ZoneVolcano;
            return b1 && b2 && b3;
        }
        public override float GetWeight(Player player)
        {
            return 0f;
        }
        public override SceneEffectPriority Priority => base.Priority;
    }
}
