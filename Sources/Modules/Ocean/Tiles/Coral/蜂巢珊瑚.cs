using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles.Ocean
{
	public class 蜂巢珊瑚 : ModTile
	{
		public override void SetStaticDefaults()
		{
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileNoAttach[(int)base.Type] = true;
            this.MinPick = 300;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                18
            };
            TileObjectData.newTile.CoordinateWidth = 54;
            TileObjectData.addTile((int)base.Type);
			this.DustType = 253;
            LocalizedText modTranslation = base.CreateMapEntryName(null);
            // modTranslation.SetDefault("");
            base.AddMapEntry(new Color(42, 91, 10), modTranslation);
			this.MineResist = 3f;
			base.SetStaticDefaults();
			modTranslation.AddTranslation(GameCulture.Chinese, "");
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.35f;
            g = 0.55f;
            b = 0.08f;
            return;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = 38;
            //Main.spriteBatch.Draw(mod.GetTexture("Tiles/Ocean/蜂巢珊瑚Glow"), new Vector2(i * 16 - 18 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 52, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
        public override void NearbyEffects(int i, int j, bool closer)
		{
            Lighting.AddLight(i * 16, j * 16, 99, 152, 20);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 48, Mod.Find<ModItem>("Favosites").Type, 1, false, 0, false, false);
        }
    }
}
