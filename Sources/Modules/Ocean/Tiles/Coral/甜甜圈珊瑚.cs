using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles.Ocean
{
	// Token: 0x02000C71 RID: 3185
	public class 甜甜圈珊瑚 : ModTile
	{
		// Token: 0x0600400B RID: 16395 RVA: 0x0032258C File Offset: 0x0032078C
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                18
            };
            TileObjectData.newTile.CoordinateWidth = 18;
            TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile((int)base.Type);
            this.ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = base.Mod.Find<ModItem>("AnnularCoral").Type;
            LocalizedText modTranslation = base.CreateMapEntryName(null);
            // modTranslation.SetDefault("");
            base.AddMapEntry(new Color(63, 187, 161), modTranslation);
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x00013910 File Offset: 0x00011B10
		public override bool CreateDust(int i, int j, ref int type)
		{
			return true;
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x00013946 File Offset: 0x00011B46
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			WorldGen.Check1x1(i, j, (int)base.Type);
			return true;
		}
        // Token: 0x0600400E RID: 16398 RVA: 0x00013956 File Offset: 0x00011B56
        public override void PlaceInWorld(int i, int j, Item item)
		{
			Main.tile[i, j].TileFrameX = 0;
			Main.tile[i, j].TileFrameY = 0;
		}
	}
}
