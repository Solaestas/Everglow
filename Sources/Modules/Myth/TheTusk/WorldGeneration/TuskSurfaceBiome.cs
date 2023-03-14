using Everglow.Sources.Modules.MythModule.TheTusk.Backgrounds;
using Everglow.Sources.Modules.MythModule.TheTusk.Water;
using Terraria.Graphics.Capture;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.WorldGeneration
{
	// Shows setting up two basic biomes. For a more complicated example, please request.
	public class TuskSurfaceBiome : ModBiome
	{
		//public override bool IsPrimaryBiome => true; // Allows this biome to impact NPC prices

		// Select all the scenery
		public override ModWaterStyle WaterStyle => ModContent.GetInstance<TuskWaterStyle>();//ModContent.Find<ModWaterStyle>("Everglow/Sources/Modules/MythModule/TheTusk/WorldGeneration/TuskWaterStyle"); // Sets a water style for when inside this biome
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<TuskSurfaceBackgroundStyle>();//ModContent.Find<ModSurfaceBackgroundStyle>("Everglow/Sources/Modules/MythModule/TheTusk/Background/TuskSurfaceBackgroundStyle");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;

		// Use SetStaticDefaults to assign the display name
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cursed Jaw");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "诅咒之颌");
		}
		public override void Load()
		{
			//On.Terraria.Main.DrawWaters += Main_DrawWaters;
			base.Load();
		}
		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
		{

			bool b1 = TheTusk.Backgrounds.TuskBiomeSky.Open;
			/*if(b1)
            {
				MythMod.MiscItems.Projectiles.Weapon.Fragrans.Fragrans.ZoneTusk = 2;
			}*/
			return b1;
		}
		public override void OnInBiome(Player player)
		{
			base.OnInBiome(player);
		}
	}
}
