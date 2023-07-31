using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles.Coral
{
	public class 大蓝色海星 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileLavaDeath[(int)base.Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile((int)base.Type);
			DustType = 7;
			TileID.Sets.DisableSmartCursor[Type] = true;
			var modTranslation = base.CreateMapEntryName();
			// modTranslation.SetDefault("大蓝色海星");
			base.AddMapEntry(new Color(120, 85, 60), modTranslation);
			// modTranslation.AddTranslation(GameCulture.Chinese, "大蓝色海星");
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
		   Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("HugeBlueStarfish"), 1, false, 0, false, false);
		}
	}
}
