using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Capture;


namespace Everglow.Ocean.Tiles.Ocean
{
	// Token: 0x02000E8F RID: 3727
    public class 幻海琉璃瓦 : ModTile
	{
		private float num5 = 0;
		private int num6 = 0;
		// Token: 0x060045D6 RID: 17878 RVA: 0x0027A6F0 File Offset: 0x002788F0
		public override void SetStaticDefaults()
		{
			Main.tileSolid[(int)base.Type] = true;
			Main.tileMergeDirt[(int)base.Type] = true;
			Main.tileBlendAll[(int)base.Type] = true;
			Main.tileBlockLight[(int)base.Type] = true;
			Main.tileShine2[(int)base.Type] = true;
			Main.tileOreFinderPriority[(int)base.Type] = 1300;
			this.MinPick = 200;
			this.DustType = 183;
			this.HitSound = 21;
			this.soundStyle/* tModPorter Note: Removed. Integrate into HitSound */ = 2;
            this.RegisterItemDrop(ModContent.ItemType<Everglow.Ocean.Items.OceanGlass>());
			Main.tileSpelunker[(int)base.Type] = true;
			LocalizedText modTranslation = base.CreateMapEntryName();
			base.AddMapEntry(new Color(0,115,231), modTranslation);
            // modTranslation.SetDefault("");
            // modTranslation.AddTranslation(GameCulture.Chinese, "");
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
		}
		private int GetDrawOffset()
		{
			int num;
			if ((float)Main.screenWidth < 1664f)
			{
				num = 193;
			}
			else
			{
				num = (int)(-0.5f * (float)Main.screenWidth + 1025f);
			}
			return num - 1;
		}
	}
}
