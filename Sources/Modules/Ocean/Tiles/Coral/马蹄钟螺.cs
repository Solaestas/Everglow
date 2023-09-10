using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
	public class 马蹄钟螺 : ModTile
	{
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateWidth = 90;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                26
            };
            TileObjectData.addTile((int)base.Type);
            this.DustType = 25;
            LocalizedText modTranslation = base.CreateMapEntryName();
            base.AddMapEntry(new Color(209, 160, 83), modTranslation);
            // modTranslation.SetDefault("");
            this.MineResist = 3f;
            base.SetStaticDefaults();
            // modTranslation.AddTranslation(GameCulture.Chinese, "");
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Everglow.Ocean.Items.Shell6>());
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 3));
            Main.tile[i, j].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 3].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 72);
        }
    }
}
