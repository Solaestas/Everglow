using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Tiles
{
    public class 石榴石晶莹宝石块 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[(int)base.Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileMergeDirt[(int)base.Type] = false;
			Main.tileBlockLight[(int)base.Type] = true;
			this.DustType = 254;
            this.RegisterItemDrop(ModContent.ItemType<Items.GarnetBrick>());
			var modTranslation = base.CreateMapEntryName();
            //modTranslation.SetDefault("石榴石晶莹宝石块");
			base.AddMapEntry(new Color(223, 18, 85), modTranslation);
			this.MineResist = 5f;
			//this.soundType = 21;
			Main.tileSpelunker[(int)base.Type] = true;
            //modTranslation.AddTranslation(GameCulture.Chinese, "石榴石晶莹宝石块");
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1;
            g = 0.28f;
            b = 0.68f;
            return;
        }
    }
}
