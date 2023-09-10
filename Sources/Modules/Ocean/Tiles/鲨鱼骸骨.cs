using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
	public class 鲨鱼骸骨 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileLavaDeath[(int)base.Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile((int)base.Type);
			this.DustType = 7;
			TileID.Sets.DisableSmartCursor[Type] = true;
			LocalizedText modTranslation = base.CreateMapEntryName();
			// modTranslation.SetDefault("鲨鱼骸骨");
			base.AddMapEntry(new Color(0, 24, 123), modTranslation);
			// modTranslation.AddTranslation(GameCulture.Chinese, "鲨鱼骸骨");
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(null, i * 16, j * 16, 48, 48, ModContent.ItemType<Everglow.Ocean.Items.SharkBone>(), 1, false, 0, false, false);
		}
	}
}
