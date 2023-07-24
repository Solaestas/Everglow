using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Waters
{
	public class CoralWater : ModWaterStyle
	{
		public override bool ChooseWaterStyle()
		{
			return Main.LocalPlayer.GetModPlayer<MythPlayer>().ZoneDeepocean;
		}
		public override int ChooseWaterfallStyle()
		{
			return base.mod.GetWaterfallStyleSlot("CoralWaterflow");
		}
		public override int GetSplashDust()
		{
			return mod.DustType("Wave");
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
