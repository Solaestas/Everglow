using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles.Ocean
{
	// Token: 0x02000C71 RID: 3185
	public class 菊花海葵 : ModTile
	{
        private float num = 0;
        private int num2 = 0;
        private bool num3 = false;
        private bool flag2 = false;
        // Token: 0x0600400B RID: 16395 RVA: 0x0032258C File Offset: 0x0032078C
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            this.ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = base.Mod.Find<ModItem>("OrangeSeaAnemone").Type;
            TileObjectData.addTile((int)base.Type);
            LocalizedText modTranslation = base.CreateMapEntryName(null);
            // modTranslation.SetDefault("");
            base.AddMapEntry(new Color(36, 100, 100), modTranslation);
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x00013910 File Offset: 0x00011B10
		public override bool CreateDust(int i, int j, ref int type)
		{
			return true;
		}
        public override void NearbyEffects(int i, int j, bool closer)
        {
            bool flag = false;
            Player player = Main.player[Main.myPlayer];
            if((player.Center - new Vector2(i * 16, j * 16)).Length() < 70)
            {
                flag = true;
            }

            if(flag)
            {
                num += (3 / (MythWorld.菊花海葵 + 0.0001f)) * Main.rand.Next(1000, 1200) / 1000f;
                if (i % 2 == 1)
                {
                    if ((int)num % (3) == 0 && Main.tile[i, j].TileFrameY < 72)
                    {
                        Main.tile[i, j].TileFrameY += 18;
                    }
                }
                if (i % 2 == 0)
                {
                    if ((int)num % (3) == 0 && Main.tile[i, j].TileFrameY < 72)
                    {
                        Main.tile[i, j].TileFrameY += 18;
                    }
                }
                flag2 = true;
            }
            if (Main.tile[i, j].TileFrameY >= 18 && !flag)
            {
                num += 3 * Main.rand.Next(1000, 1200) / 1000f;
                if (i % 2 == 1)
                {
                    if ((int)num % (5) == 0 && Main.tile[i, j].TileFrameY > 18)
                    {
                        Main.tile[i, j].TileFrameY -= 18;
                    }
                }
                if (i % 2 == 0)
                {
                    if ((int)num % (5) == 0 && Main.tile[i, j].TileFrameY > 18)
                    {
                        Main.tile[i, j].TileFrameY -= 18;
                    }
                }
                if (Main.tile[i, j].TileFrameY < 18)
                {
                    flag2 = false;
                }
            }
        }
        // Token: 0x0600400D RID: 16397 RVA: 0x00013946 File Offset: 0x00011B46
        // Token: 0x0600400E RID: 16398 RVA: 0x00013956 File Offset: 0x00011B56
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 3) * 18);
            Main.tile[i, j].TileFrameX = num;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = 16;
            Main.spriteBatch.Draw(Mod.GetTexture("Tiles/Ocean/菊花海葵Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(55,55,55,0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
