using Terraria.Localization;

namespace Everglow.Ocean.Tiles
{
    public class Basalt : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[(int)base.Type] = true;
            Main.tileMergeDirt[(int)base.Type] = true;
            Main.tileBlendAll[(int)base.Type] = true;
            Main.tileBlockLight[(int)base.Type] = true;
            Main.tileShine2[(int)base.Type] = false;
            DustType = 6;
            MinPick = 270;
            SoundType = 21;
            SoundStyle = 2;
            ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Basalt>();
            Main.tileSpelunker[(int)base.Type] = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(28, 28, 28), modTranslation);
            modTranslation.SetDefault("");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        /*public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (!Lighting.NotRetro)
            {
                return;
            }
            int num = (int)Main.tile[i, j].TileFrameX;
            int num2 = (int)Main.tile[i, j].TileFrameY;
            int num3 = i % 1;
            int num4 = j % 1;
            num3 *= 288;
            num4 *= 270;
            num += num3;
            num2 += num4;
            Texture2D texture = ModContent.Request<Texture2D>("Everglow.Ocean.OceanMod/Tiles/BasaltGlow").Value;
            Vector2 position = new Vector2((float)(i * 16) - Main.screenPosition.X + (float)this.GetDrawOffset(), (float)(j * 16) - Main.screenPosition.Y + (float)this.GetDrawOffset());
            if (CaptureManager.Instance.IsCapturing)
            {
                position = new Vector2((float)(i * 16) - Main.screenPosition.X, (float)(j * 16) - Main.screenPosition.Y);
            }
            float l = 0;
            for(int x = i - 4;x < i + 4; x++)
            {
                for (int y = j - 4; y < j + 4; y++)
                {
                    if(Main.tile[x,y].LiquidType == 1 && new Vector2(i - x, j - y).Length() <= 4)
                    {
                        l += 0.99f;
                    }
                }
            }
            spriteBatch.Draw(texture, position, new Rectangle?(new Rectangle(num, num2, 18, 18)), new Color((int)(l / 24f * 255), (int)(l / 24f * 255), (int)(l / 24f * 255), 0), 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
        }*/
        private int GetDrawOffset()
        {
            int num;
            if ((float)Main.screenWidth < 1664f)
            {
                num = 193;
            }
            else
            {
                num = (int)(-0.5f * (float)Main.screenWidth + 1025f);
            }
            return num - 1;
        }
    }
}
