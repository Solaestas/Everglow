using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
	// Token: 0x02000C76 RID: 3190
	public class 烈焰彩虹 : ModTile
	{
		// Token: 0x06004027 RID: 16423 RVA: 0x00322CBC File Offset: 0x00320EBC
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileLavaDeath[(int)base.Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile((int)base.Type);
			this.DustType = 7;
			TileID.Sets.DisableSmartCursor[Type] = true;
			LocalizedText modTranslation = base.CreateMapEntryName();
			// modTranslation.SetDefault("烈焰彩虹");
			base.AddMapEntry(new Color(0, 24, 123), modTranslation);
			// modTranslation.AddTranslation(GameCulture.Chinese, "烈焰彩虹");
		}
        // Token: 0x06004028 RID: 16424 RVA: 0x00322D58 File Offset: 0x00320F58
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(null, i * 16, j * 16, 48, 48, ModContent.ItemType<Everglow.Ocean.Items.BurningRainbow>(), 1, false, 0, false, false);
		}
	}
}
