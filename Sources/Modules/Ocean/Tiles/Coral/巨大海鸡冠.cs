using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Everglow.Ocean.Tiles.Ocean
{
	public class 巨大海鸡冠 : ModTile
	{
		public override void SetStaticDefaults()
		{
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileNoAttach[(int)base.Type] = true;
            this.MinPick = 300;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
                16,
                16,
                16,
                22
			};
            TileObjectData.newTile.CoordinateWidth = 144;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorTop = default(AnchorData);
            TileObjectData.addTile((int)base.Type);
			this.DustType = 123;
            LocalizedText modTranslation = base.CreateMapEntryName(null);
            base.AddMapEntry(new Color(193, 131, 139), modTranslation);
            // modTranslation.SetDefault("");
            this.MineResist = 3f;
			base.SetStaticDefaults();
			modTranslation.AddTranslation(GameCulture.Chinese, "");
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, base.Mod.Find<ModItem>("HugeAlcyonarian").Type);
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
            short num = (short)(Main.rand.Next(0, 4));
            Main.tile[i, j].TileFrameX = (short)(num * 144);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 144);
            Main.tile[i, j + 5].TileFrameX = (short)(num * 144);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 144);
            Main.tile[i, j + 4].TileFrameX = (short)(num * 144);
            Main.tile[i, j + 3].TileFrameX = (short)(num * 144);
        }
    }
}
