using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace Everglow.Ocean.Tiles
{
	public class 贝壳风铃 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileLavaDeath[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 48;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData((Terraria.Enums.AnchorType)1, 1, 1);
            TileObjectData.addTile((int)base.Type);
			this.DustType = 7;
			TileID.Sets.DisableSmartCursor[Type] = true;
			LocalizedText modTranslation = base.CreateMapEntryName();
			// modTranslation.SetDefault("贝壳风铃");
			base.AddMapEntry(new Color(0, 24, 123), modTranslation);
			// modTranslation.AddTranslation(GameCulture.Chinese, "贝壳风铃");
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(null, i * 16, j * 16, 48, 48, ModContent.ItemType<Everglow.Ocean.Items.ShellBell>(), 1, false, 0, false, false);
		}
	}
}
