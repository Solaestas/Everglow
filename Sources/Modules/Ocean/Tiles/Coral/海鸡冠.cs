using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
	public class 海鸡冠 : ModTile
	{
		public override void SetStaticDefaults()
		{
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileNoAttach[(int)base.Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				18
			};
            TileObjectData.newTile.CoordinateWidth = 36;
            TileObjectData.addTile((int)base.Type);
			this.DustType = 123;
            LocalizedText modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("");
            this.MineResist = 3f;
			base.SetStaticDefaults();
			// modTranslation.AddTranslation(GameCulture.Chinese, "");
            base.AddMapEntry(new Color(193, 131, 139), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Everglow.Ocean.Items.Alcyonarian>());
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.2f;
            b = 0.1f;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 2));
            Main.tile[i, j].TileFrameX = (short)(num * 36);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 36);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 36);
        }
    }
}
