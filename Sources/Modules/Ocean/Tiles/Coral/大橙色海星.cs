using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MythMod.Tiles.Ocean
{
	public class 大橙色海星 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileLavaDeath[(int)base.Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile((int)base.Type);
			this.dustType = 7;
			this.disableSmartCursor = true;
			ModTranslation modTranslation = base.CreateMapEntryName(null);
			modTranslation.SetDefault("大橙色海星");
			base.AddMapEntry(new Color(120, 85, 60), modTranslation);
			modTranslation.AddTranslation(GameCulture.Chinese, "大橙色海星");
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
		   Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("HugeOrangeStarfish"), 1, false, 0, false, false);
		}
	}
}
