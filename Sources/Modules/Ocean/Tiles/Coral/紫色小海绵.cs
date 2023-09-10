using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
	// Token: 0x02000C71 RID: 3185
	public class 紫色小海绵 : ModTile
	{
		// Token: 0x0600400B RID: 16395 RVA: 0x0032258C File Offset: 0x0032078C
		public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            Main.tileWaterDeath[(int)base.Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                18
            };
            TileObjectData.newTile.Direction = (Terraria.Enums.TileObjectDirection)1;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = (Terraria.Enums.TileObjectDirection)2;
            TileObjectData.addAlternate(1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile((int)base.Type);
            TileID.Sets.DisableSmartCursor[Type] = true;
            this.AdjTiles = new int[]
            {
                15
            };
            this.DustType = 50;
            LocalizedText modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("");
            base.AddMapEntry(new Color(153, 107, 0), modTranslation);
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x00013910 File Offset: 0x00011B10
		public override bool CreateDust(int i, int j, ref int type)
		{
			return true;
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Everglow.Ocean.Items.PurpleLittleSponge>());
        }
        // Token: 0x0600400D RID: 16397 RVA: 0x00013946 File Offset: 0x00011B46
        // Token: 0x0600400E RID: 16398 RVA: 0x00013956 File Offset: 0x00011B56
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 7) * 18);
            Main.tile[i, j].TileFrameX = num;
            Main.tile[i, j - 1].TileFrameX = num;
        }
    }
}
