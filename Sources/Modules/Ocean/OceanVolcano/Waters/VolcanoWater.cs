using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Waters
{
	// Token: 0x02000E0B RID: 3595
	public class VolcanoWater : ModWaterStyle
	{
		// Token: 0x06004983 RID: 18819 RVA: 0x0001442F File Offset: 0x0001262F
		public override bool ChooseWaterStyle()
		{
			return Main.LocalPlayer.GetModPlayer<MythPlayer>().ZoneVolcano;
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00014446 File Offset: 0x00012646
		public override int ChooseWaterfallStyle()
		{
			return base.mod.GetWaterfallStyleSlot("VolcanoWaterflow");
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00014458 File Offset: 0x00012658
		public override int GetSplashDust()
		{
			return 33;
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x0001445C File Offset: 0x0001265C
		public override int GetDropletGore()
		{
			return 713;
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00014463 File Offset: 0x00012663
		public override Color BiomeHairColor()
		{
			return Color.Blue;
		}
	}
}
