using Terraria.Localization;

namespace MythMod.OceanMod.Backgrounds
{
    public class VolcanoSurfaceBiome : ModBiome
    {
        //public override bool IsPrimaryBiome => true; // Allows this biome to impact NPC prices

        // Select all the scenery
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("MythMod/OceanWaterStyle"); // Sets a water style for when inside this biome
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("MythMod/VolcanoSurfaceBackgroundStyle");
        //public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

        // Select Music
        public override int Music => MusicLoader.GetMusicSlot("MythMod/Musics/VolcanoSurface");

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Volcano");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "火山");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            bool b1 = Main.ActiveWorldFileData.Path.Contains("OcEaNMyTh");
            // Second, we will limit this biome to the inner horizontal third of the map as our second custom condition
            bool b2 = ModContent.GetInstance<Common.Systems.VolcanoTileCount>().volcanoBlockCount >= 40;
            if (b1 && b2)
            {
                Common.Players.MythPlayer.ZoneVolcano = true;
            }
            else
            {
                Common.Players.MythPlayer.ZoneVolcano = false;
            }
            return b1 && b2;
        }
        public override float GetWeight(Player player)
        {
            return 1f;
        }
        public override SceneEffectPriority Priority => base.Priority;
    }
}
