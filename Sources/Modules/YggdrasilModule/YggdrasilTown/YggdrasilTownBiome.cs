namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown
{
    public class YggdrasilTownBiome : ModBiome
    {

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override string BestiaryIcon => "Everglow/Sources/Modules/YggdrasilModule/YggdrasilTown/YggdrasilTownIcon";
		public override string BackgroundPath => base.BackgroundPath;
		public override string MapBackground => "Everglow/Sources/Modules/YggdrasilModule/YggdrasilTown/Backgrounds/YggdrasilTown_MapBackground";
		public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.YggdrasilTownWaterStyle>();
		public override Color? BackgroundColor => base.BackgroundColor;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yggdrasil Town");
		}

        public override void Load()
        {
			base.Load();
        }


        public override bool IsBiomeActive(Player player)
		{
			return Background.MineRoadBackground.BiomeActive();
		}

        public override void OnInBiome(Player player)
        {
			base.OnInBiome(player);
        }
    }
}

