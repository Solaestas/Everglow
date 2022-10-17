namespace Everglow.Sources.Modules.MythModule.TheFirefly
{
    internal class TileSpin
    {
        public static Dictionary<(int, int), Vector2> TileRotation = new Dictionary<(int, int), Vector2>();
        /// <summary>
        /// 更新旋转
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        public void Update(int i, int j, float k1 = 0.75f, float k2 = 0.13f)
        {
            if (TileRotation.ContainsKey((i, j)) && !Main.gamePaused)
            {
                float rot;
                float Omega;
                Omega = TileRotation[(i, j)].X;
                rot = TileRotation[(i, j)].Y;
                Omega = Omega * k1 - rot * k2;
                TileRotation[(i, j)] = new Vector2(Omega, rot + Omega);
                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                {
                    TileRotation.Remove((i, j));
                }
            }
        }
        /// <summary>
        /// 更新贴图旋转，并抖落蓝色荧光花粉dust
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        public void UpdateBlackShrub(int i, int j, float k1 = 0.75f, float k2 = 0.13f, Vector2 offset = new Vector2(), float Addx = 0, float Addy = 0, int width = 16, int height = 16)
        {
            if (TileRotation.ContainsKey((i, j)) && !Main.gamePaused)
            {
                float rot;
                float Omega;
                Omega = TileRotation[(i, j)].X;
                rot = TileRotation[(i, j)].Y;
                Omega = Omega * 0.75f - rot * 0.13f;
                TileRotation[(i, j)] = new Vector2(Omega, rot + Omega);
                float Strength = Math.Abs(Omega) + Math.Abs(rot);
                if(Main.rand.NextBool(Math.Clamp((int)(100 - Strength * 1200f * k1), 1, 900)))
                {
                    Dust d = Dust.NewDustDirect(new Vector2(i * 16 + Addx, j * 16 + Addy) + offset.RotatedBy(rot), width, height, ModContent.DustType<Dusts.BlueParticleDark>());
                    Dust d2 = Dust.NewDustDirect(new Vector2(i * 16 + Addx, j * 16 + Addy) + offset.RotatedBy(rot), width, height, ModContent.DustType<Dusts.BlueParticle>());
                    d.alpha = (int)(255 - Strength * 2000f * k1);
                    d2.scale = Strength * 10f;
                    d.scale *= k1;
                    d.velocity = (offset.RotatedBy(rot) - offset.RotatedBy(rot - Omega)) * k1 * 1f;
                    d2.scale *= k1;
                    d2.velocity = (offset.RotatedBy(rot) - offset.RotatedBy(rot - Omega)) * k1 * 1f;
                }
                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                {
                    TileRotation.Remove((i, j));
                }
            }
        }
        /// <summary>
        /// 专门绘制吊灯的
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="tex"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="specialColor"></param>
        /// <param name="color"></param>
        public void DrawRotatedChandelier(int i, int j, Texture2D tex, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
        {
            float rot = 0;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            var tile = Main.tile[i, j];
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Color c = Lighting.GetColor(i, j);
            if(specialColor)
            {
                c = color;
            }
            if (TileRotation.ContainsKey((i, j)))
            {
                rot = TileRotation[(i, j)].Y;
                Main.spriteBatch.Draw(tex, new Vector2(i * 16  + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX - 18, 0, 54, 48), c, rot, new Vector2(27, 0), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX - 18, 0, 54, 48), c, rot, new Vector2(27, 0), 1f, SpriteEffects.None, 0f);
            }
        }
        /// <summary>
        /// 专门绘制吊灯的
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="tex"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="specialColor"></param>
        /// <param name="color"></param>
        public void DrawRotatedLamp(int i, int j, Texture2D tex, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
        {
            float rot = 0;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            var tile = Main.tile[i, j];
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Color c = Lighting.GetColor(i, j);
            if (specialColor)
            {
                c = color;
            }
            if (TileRotation.ContainsKey((i, j)))
            {
                rot = TileRotation[(i, j)].Y;
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX, 0, 18, 34), c, rot, new Vector2(9, 0), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX, 0, 18, 34), c, rot, new Vector2(9, 0), 1f, SpriteEffects.None, 0f);
            }
        }
        /// <summary>
        /// 画旋转物块
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="tex"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="origin"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="specialColor"></param>
        /// <param name="color"></param>
        public void DrawRotatedTile(int i, int j, Texture2D tex, Rectangle sourceRectangle, Vector2 origin, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
        {
            float rot = 0;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Color c = Lighting.GetColor(i, j);
            if (specialColor)
            {
                c = color;
            }
            if (TileRotation.ContainsKey((i, j)))
            {
                rot = TileRotation[(i, j)].Y;
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot, origin, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot, origin, 1f, SpriteEffects.None, 0f);
            }
        }
        /// <summary>
        /// 画旋转物块
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="tex"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="origin"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="kRot"></param>
        /// <param name="specialColor"></param>
        /// <param name="color"></param>
        public void DrawRotatedTile(int i, int j, Texture2D tex, Rectangle sourceRectangle, Vector2 origin, float offsetX = 0, float offsetY = 0, float kRot = 1, bool specialColor = false, Color color = new Color())
        {
            float rot = 0;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Color c = Lighting.GetColor(i, j);
            if (specialColor)
            {
                c = color;
            }
            if (TileRotation.ContainsKey((i, j)))
            {
                rot = TileRotation[(i, j)].Y;
                if (specialColor)
                {
                    float maxC = Math.Max(color.R / 255f, Math.Abs(rot * 3) + Math.Abs(TileRotation[(i, j)].X * 3));
                    maxC = Math.Clamp(maxC, 0, 1);
                    c = new Color(maxC, maxC, maxC, 0);
                }
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot * kRot, origin, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot * kRot, origin, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
