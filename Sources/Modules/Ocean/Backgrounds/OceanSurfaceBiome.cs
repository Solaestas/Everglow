using Everglow.Ocean.Common;
using Terraria.Localization;

namespace Everglow.Ocean.Backgrounds
{
    public class OceanSurfaceBiome : ModBiome
    {
        //public override bool IsPrimaryBiome => true; // Allows this biome to impact NPC prices

        // Select all the scenery
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("Everglow.Ocean.OceanWaterStyle"); // Sets a water style for when inside this biome
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("Everglow.Ocean.Backgrounds.OceanSurfaceBgStyle");
		//public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;
		public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("Everglow.Ocean.Backgrounds.OceanUgBgStyle");
		// Select Music
		public override int Music => MusicLoader.GetMusicSlot("Everglow.Ocean.Musics/Ocean");

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Far Ocean");
            // DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "远洋");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
			OceanContentPlayer ocplayer = player.GetModPlayer<OceanContentPlayer>();
            bool b1 = Main.ActiveWorldFileData.Path.Contains("OcEaNMyTh"); // string needs to change to a more appropriate text
            bool b2 = Main.LocalPlayer.position.X / 16d <= Main.maxTilesX * 0.8778 || Main.LocalPlayer.position.X / 16d >= Main.maxTilesX * 0.9456;
            bool b3 = !ocplayer.ZoneVolcano;
            return b1 && b2 && b3;
        }
        public override float GetWeight(Player player)
        {
            return 0f;
        }
        public override SceneEffectPriority Priority => base.Priority;
    }
}
