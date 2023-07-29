using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Waters
{
	public class CoralWater : ModWaterStyle
	{
		//public override bool ChooseWaterStyle()
		//{
		//	return Main.LocalPlayer.GetModPlayer<OceanContentPlayer>().ZoneDeepocean;
		//}
		public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("Everglow.Ocean.Waters.CoralWaterflow").Slot;
		public override int GetSplashDust()
		{
			return ModContent.DustType<Everglow.Ocean.Dusts.Wave>();
		}
		public override int GetDropletGore()
		{
			return 713;
		}

		public override Color BiomeHairColor()
		{
			return Color.Blue;
		}
    }
}
