using System;
using Everglow.Ocean.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Waters
{
	public class VolcanoWater : ModWaterStyle
	{
		//public override bool ChooseWaterStyle()
		//{
		//	return Main.LocalPlayer.GetModPlayer<OceanContentPlayer>().ZoneVolcano;
		//}

		public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("Everglow.Ocean.OceanVolcano.Waters.VolcanoWaterFlow").Slot;

		public override int GetSplashDust()
		{
			return 33;
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
