namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain
{
    public class KelpCurtainBiome : ModBiome
    {
        public override int Music => Common.YggdrasilContent.QuickMusic("KelpCurtainBGM");
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override string BestiaryIcon => "Everglow/Sources/Modules/YggdrasilModule/KelpCurtain/KelpCurtainIcon";
		public override string BackgroundPath => base.BackgroundPath;
		public override string MapBackground => "Everglow/Sources/Modules/YggdrasilModule/KelpCurtain/Backgrounds/KelpCurtain_MapBackground";
		public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.KelpCurtainWaterStyle>();
		public override Color? BackgroundColor => base.BackgroundColor;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kelp Curtain");
		}

        public override void Load()
        {
			base.Load();
        }
        public override bool IsBiomeActive(Player player)
        {
            return Background.KelpCurtainBackground.BiomeActive();
        }

        public override void OnInBiome(Player player)
        {
            base.OnInBiome(player);
        }
    }
    public class KelpCurtainSystem:ModSystem
    {
        //������
        public readonly Vector3 ambient = new Vector3(0f, 0f, 0f);
        /// <summary>
        /// ������Ĺ���
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="outputColor"></param>
        private void TileLightScanner_GetTileLight(On.Terraria.Graphics.Light.TileLightScanner.orig_GetTileLight orig, Terraria.Graphics.Light.TileLightScanner self, int x, int y, out Vector3 outputColor)
        {
            orig(self, x, y, out outputColor);
            KelpCurtainBiome KelpCurtainBiome = new KelpCurtainBiome();
            if (KelpCurtainBiome.IsBiomeActive(Main.LocalPlayer))
            {
                outputColor += ambient;
            }
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public override void OnModLoad()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                On.Terraria.Graphics.Light.TileLightScanner.GetTileLight += TileLightScanner_GetTileLight;
            }
        }
    }
}

