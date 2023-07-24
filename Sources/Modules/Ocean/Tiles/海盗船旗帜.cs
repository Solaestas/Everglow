using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace MythMod.Tiles.Ocean
{
    public class 海盗船旗帜 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 72;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData((Terraria.Enums.AnchorType)1, 1, 1);
            TileObjectData.addTile((int)base.Type);
            this.dustType = 7;
            this.disableSmartCursor = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("海盗船旗帜");
            base.AddMapEntry(new Color(0, 24, 123), modTranslation);
            modTranslation.AddTranslation(GameCulture.Chinese, "海盗船旗帜");
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("PirateFlag"), 1, false, 0, false, false);
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 4));
            Main.tile[i, j].frameX = (short)(num * 72);
            Main.tile[i, j + 1].frameX = (short)(num * 72);
            Main.tile[i, j + 2].frameX = (short)(num * 72);
            Main.tile[i, j + 3].frameX = (short)(num * 72);
            Main.tile[i, j + 4].frameX = (short)(num * 72);
        }
    }
}
