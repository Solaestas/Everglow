using System;
using Everglow.Ocean.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Tiles
{
    public class 海蓝宝石晶莹宝石块 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[(int)base.Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileMergeDirt[(int)base.Type] = false;
			Main.tileBlockLight[(int)base.Type] = true;
			this.DustType = 156;
            this.RegisterItemDrop(ModContent.ItemType<AquamarineBrick>());
			var modTranslation = base.CreateMapEntryName();
            //modTranslation.SetDefault("海蓝宝石晶莹宝石块");
			base.AddMapEntry(new Color(0, 231, 236), modTranslation);
			this.MineResist = 5f;
			//this.soundType = 21;
			Main.tileSpelunker[(int)base.Type] = true;
            //modTranslation.AddTranslation(GameCulture.Chinese, "海蓝宝石晶莹宝石块");
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0;
            g = 0.9f;
            b = 0.9f;
            return;
        }
    }
}
