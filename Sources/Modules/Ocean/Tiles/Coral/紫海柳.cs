using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles.Ocean
{
	// Token: 0x02000DD9 RID: 3545
	public class 紫海柳 : ModTile
	{
		// Token: 0x0600489C RID: 18588 RVA: 0x003496D4 File Offset: 0x003478D4
		public override void SetStaticDefaults()
		{
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
			this.DustType = 253;
            LocalizedText modTranslation = base.CreateMapEntryName(null);
            // modTranslation.SetDefault("");
            base.AddMapEntry(new Color(86, 54, 130), modTranslation);
            this.MineResist = 3f;
			base.SetStaticDefaults();
			modTranslation.AddTranslation(GameCulture.Chinese, "");
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, base.Mod.Find<ModItem>("PurpleSeaWillow").Type);
        }
        // Token: 0x0600489D RID: 18589 RVA: 0x000138D5 File Offset: 0x00011AD5
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 4));
            Main.tile[i, j].TileFrameX = (short)(num * 36);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 36);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 36);
        }
    }
}
