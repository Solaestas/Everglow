using Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds;
using Terraria.GameContent.Liquid;

namespace Everglow.Sources.Modules.MythModule.TheFirefly
{

    public class FireflyBiomeBG : ModSurfaceBackgroundStyle
    {
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            
        }
    }

    public class FireflyBiome : ModBiome
    {
		public override int Music => Common.MythContent.QuickMusic("MothBiome");

		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
		public override string BestiaryIcon => "Everglow/Sources/Modules/MythModule/TheFirefly/FireflyIcon";
		public override string BackgroundPath => base.BackgroundPath;
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<Backgrounds.FakeMothBackground>();
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.FireflyWaterStyle>();
		public override Color? BackgroundColor => base.BackgroundColor;
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("Everglow/FireflyWaterStyle");
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<FireflyBiomeBG>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Cocoon");
		}

        public override void Load()
        {
			//On.Terraria.Main.DrawWaters += Main_DrawWaters;
			base.Load();
        }

  //      private void Main_DrawWaters(On.Terraria.Main.orig_DrawWaters orig, Main self, bool isBackground)
  //      {
		//	var s = LoaderManager.Get<WaterStylesLoader>();
		//	int totalCount = (int)typeof(Loader).GetProperty("TotalCount", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(s);
		//	Main.drewLava = false;
		//	if (!isBackground)
		//	{
		//		Main.waterStyle = Main.CalculateWaterStyle();
		//		for (int i = 0; i < totalCount; i++)
		//		{
		//			if (Main.IsLiquidStyleWater(Main.waterStyle))
		//			{
		//				if (Main.waterStyle != i)
		//				{
		//					Main.liquidAlpha[i] = Math.Max(Main.liquidAlpha[i] - 0.2f, 0f);
		//				}
		//				else
		//				{
		//					Main.liquidAlpha[i] = Math.Min(Main.liquidAlpha[i] + 0.2f, 1f);
		//				}
		//			}
		//		}
		//		LoaderManager.Get<WaterStylesLoader>().UpdateLiquidAlphas();
		//	}
		//	if (!Main.drawToScreen && !isBackground)
		//	{
		//		Vector2 vector = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange));
		//		int val = (int)((Main.Camera.ScaledPosition.X - vector.X) / 16f - 1f);
		//		int val2 = (int)((Main.Camera.ScaledPosition.X + Main.Camera.ScaledSize.X + vector.X) / 16f) + 2;
		//		int val3 = (int)((Main.Camera.ScaledPosition.Y - vector.Y) / 16f - 1f);
		//		int val4 = (int)((Main.Camera.ScaledPosition.Y + Main.Camera.ScaledSize.Y + vector.Y) / 16f) + 5;
		//		val = Math.Max(val, 5) - 2;
		//		val3 = Math.Max(val3, 5);
		//		val2 = Math.Min(val2, Main.maxTilesX - 5) + 2;
		//		val4 = Math.Min(val4, Main.maxTilesY - 5) + 4;
		//		Rectangle drawArea = new Rectangle(val, val3, val2 - val, val4 - val3);
		//		LiquidRenderer.Instance.PrepareDraw(drawArea);
		//	}
		//	bool flag = false;

		//	var main_DrawWater = typeof(Main).GetMethod("DrawWater", BindingFlags.Instance | BindingFlags.NonPublic);
		//	for (int j = 0; j < totalCount; j++)
		//	{
		//		if (Main.IsLiquidStyleWater(j) && Main.liquidAlpha[j] > 0f && j != Main.waterStyle)
		//		{
		//			main_DrawWater.Invoke(Main.instance, new object[] { isBackground, j, isBackground ? 1f : Main.liquidAlpha[j] });
		//			flag = true;
		//		}
		//	}
		//	main_DrawWater.Invoke(Main.instance, new object[] { isBackground, Main.waterStyle, flag ? Main.liquidAlpha[Main.waterStyle] : 1f });
		//	LoaderManager.Get<WaterStylesLoader>().DrawWatersToScreen(isBackground);
		//}


        public override bool IsBiomeActive(Player player)
		{
			MothBackground mothBackground = ModContent.GetInstance<MothBackground>();
			return mothBackground.BiomeActive();
		}

        public override void OnInBiome(Player player)
        {
			base.OnInBiome(player);
        }
    }
}

