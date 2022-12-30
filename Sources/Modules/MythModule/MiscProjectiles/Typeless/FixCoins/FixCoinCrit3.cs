using Everglow.Sources.Modules.MythModule.TheTusk;
using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Typeless.FixCoins
{
    public class FixCoinCrit3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fix Coin");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "附魔币");
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 150;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }



        private float b;
        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                if (IniV[i] == Vector2.Zero)
                {
                    IniV[i] = new Vector2(0, Main.rand.NextFloat(3f, 9f)).RotatedByRandom(6.283);
                }
            }
            Projectile.rotation = 0;
            Projectile.velocity *= 0.98f * Projectile.timeLeft / 150f;
            if (Projectile.velocity.Length() > 0.3f)
            {
                Projectile.velocity.Y -= 0.75f * Projectile.timeLeft / 150f;
            }
            if (Projectile.timeLeft > 149)
            {
                b = Main.rand.Next(200);
                Projectile.ai[0] = b;
            }
            if (Projectile.timeLeft > 50 && Projectile.timeLeft < 120)
            {
                Stre2 += 1 / 70f;
            }
            else
            {
                Stre2 *= 0.95f;
            }
            Stre += 1 / 150f;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center);
            for (int h = 0; h < 20; h++)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<TheFirefly.Dusts.PureBlue>(), 0, 0, 0, default, 15f * Main.rand.NextFloat(0.4f, 1.1f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3;
            }
            for (int h = 0; h < 5; h++)
            {
                Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 2f)).RotatedByRandom(3.14159);
                //Gore.NewGore(null, Projectile.position, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/MiscGore/CoinFrame" + (h % 3 + 1).ToString()).Type, 1f);
            }
            Player player = Main.player[Projectile.owner];
            int X0 = Main.rand.Next(58);
            for (int x = X0; x < 58; x++)
            {
                if (player.inventory[x].accessory)
                {
                    player.inventory[x].prefix = PrefixID.Lucky;
                    SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, player.Center);
                    for (int h = 0; h < 20; h++)
                    {
                        Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                        int r = Dust.NewDust(new Vector2(player.Center.X, player.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<TheFirefly.Dusts.PureBlue>(), 0, 0, 0, default, 15f * Main.rand.NextFloat(0.4f, 1.1f));
                        Main.dust[r].noGravity = true;
                        Main.dust[r].velocity = v3;
                        //Main.dust[r].dustIndex = (int)(Math.Cos(h * Math.PI / 10d + player.ai[0]) * 100d);
                    }
                    string tex1 = "Your ";
                    string tex2 = " get prefix";
                    if (Language.ActiveCulture.Name == "zh-Hans")
                    {
                        tex1 = "你的[";
                        tex2 = "]得到了附魔";
                    }
                    CombatText.NewText(new Rectangle((int)player.Center.X - 10, (int)player.Center.Y - 10, 20, 20), Color.Blue, tex1 + player.inventory[x].Name + tex2);
                    return;
                }

            }
            for (int x = X0; x >= 0; x--)
            {
                if (player.inventory[x].accessory)
                {
                    player.inventory[x].prefix = PrefixID.Lucky;
                    SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, player.Center);
                    for (int h = 0; h < 20; h++)
                    {
                        Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                        int r = Dust.NewDust(new Vector2(player.Center.X, player.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<TheFirefly.Dusts.PureBlue>(), 0, 0, 0, default, 15f * Main.rand.NextFloat(0.4f, 1.1f));
                        Main.dust[r].noGravity = true;
                        Main.dust[r].velocity = v3;
                        //Main.dust[r].dustIndex = (int)(Math.Cos(h * Math.PI / 10d + player.ai[0]) * 100d);
                    }
                    string tex1 = "Your ";
                    string tex2 = " get prefix";
                    if (Language.ActiveCulture.Name == "zh-Hans")
                    {
                        tex1 = "你的[";
                        tex2 = "]得到了附魔";
                    }
                    CombatText.NewText(new Rectangle((int)player.Center.X - 10, (int)player.Center.Y - 10, 20, 20), Color.Blue, tex1 + player.inventory[x].Name + tex2);
                    return;
                }
            }
            string tex3 = "Please put at lease 1 accessory item in your inventory";
            if (Language.ActiveCulture.Name == "zh-Hans")
            {
                tex3 = "你的背包中没有饰品";
            }
            Item.NewItem(null, Projectile.Center, ModContent.ItemType<MiscItems.FixCoins.FixCoinCrit3>());
            CombatText.NewText(new Rectangle((int)player.Center.X - 10, (int)player.Center.Y - 10, 20, 20), Color.White, tex3);
        }
        float Stre = 0;
        float Stre2 = 0;
        private Effect ef;
        Vector2[] IniV = new Vector2[5];
        public override void PostDraw(Color lightColor)
        {
            for (int j = 0; j < 5; j++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
                                ef = Common.MythContent.QuickEffect("Effects/Trail");
                Player player = Main.player[Projectile.owner];
                Vector2 v0 = Projectile.Center;
                Vector2 Vi = IniV[j];
                for (int i = 1; i < 300; ++i)
                {
                    Vector2 v1 = player.Center - v0;
                    if (v1.Length() < 5)
                    {
                        break;
                    }
                    v1 /= v1.Length();
                    Vector2 v2 = v0;
                    v0 += Vi + v1 * 5;
                    Vi *= 0.99f;
                    int width = (int)(20 * Stre2);
                    var normalDir = v2 - v0;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = Math.Abs(((i / 23f) % 1) - 0.5f);
                    var color2 = Color.Lerp(Color.White, Color.Blue, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    bars.Add(new VertexBase.CustomVertexInfo(v0 + normalDir * width, color2, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new VertexBase.CustomVertexInfo(v0 + normalDir * -width, color2, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                List<VertexBase.CustomVertexInfo> triangleList = new List<VertexBase.CustomVertexInfo>();
                if (bars.Count > 2)
                {
                    triangleList.Add(bars[0]);
                    var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 0.5f, Color.White, new Vector3(0, 0.5f, 1));
                    triangleList.Add(bars[1]);
                    triangleList.Add(vertex);
                    for (int i = 0; i < bars.Count - 2; i += 2)
                    {
                        triangleList.Add(bars[i]);
                        triangleList.Add(bars[i + 2]);
                        triangleList.Add(bars[i + 1]);

                        triangleList.Add(bars[i + 1]);
                        triangleList.Add(bars[i + 2]);
                        triangleList.Add(bars[i + 3]);
                    }
                    RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                    var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                    var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                    ef.Parameters["uTransform"].SetValue(model * projection);
                    ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f + Projectile.ai[0]);
                    Texture2D Blue = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlue2").Value;
                    Texture2D Shape = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                    Texture2D Mask = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/IceTrace").Value;
                    Main.graphics.GraphicsDevice.Textures[0] = Blue;
                    Main.graphics.GraphicsDevice.Textures[1] = Shape;
                    Main.graphics.GraphicsDevice.Textures[2] = Mask;
                    Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                    Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                    Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                    ef.CurrentTechnique.Passes[0].Apply();
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                    Main.graphics.GraphicsDevice.RasterizerState = originalState;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
            Texture2D LightE = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value;
            Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(0, 0, 0.3f * Stre * Stre, 0), -(float)(Math.Sin(Main.time / 26d)) + 0.6f, new Vector2(128f, 128f), 0.5f + (float)(0.25 * Math.Sin(Main.time / 26d)), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(0, 0, 1f * Stre * Stre, 0), (float)(Math.Sin(Main.time / 12d + 2)) + 1.6f, new Vector2(128f, 128f), 0.5f + (float)(0.25 * Math.Sin(Main.time / 26d)), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(0, 0, 0.3f * Stre * Stre, 0), (float)Math.PI / 2f + (float)(Main.time / 9d), new Vector2(128f, 128f), 0.5f + (float)(0.25 * Math.Sin(Main.time / 26d + 1.57)), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(0, 0, 1f * Stre * Stre, 0), (float)(Main.time / 26d), new Vector2(128f, 128f), 0.5f + (float)(0.25 * Math.Sin(Main.time / 26d + 3.14)), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(0, 0, 1f * Stre * Stre, 0), -(float)(Main.time / 26d), new Vector2(128f, 128f), 0.5f + (float)(0.25 * Math.Sin(Main.time / 26d + 4.71)), SpriteEffects.None, 0);
            Texture2D Ball = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Texture2D Circle = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Typeless/FixCoinFramework").Value;
            Texture2D Light = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Typeless/FixCoinLight3").Value;
            Color color = Lighting.GetColor((int)(Projectile.Center.X / 16d), (int)(Projectile.Center.Y / 16d));
            color = Projectile.GetAlpha(color) * ((255 - Projectile.alpha) / 255f);
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(255 * Stre2), (int)(255 * Stre2), (int)(255 * Stre2), 0), Projectile.rotation, new Vector2(56f, 56f), Projectile.scale, SpriteEffects.None, 0);
            if (Projectile.timeLeft > 50)
            {
                Main.spriteBatch.Draw(Ball, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255 + color.R, 255 + color.G, 255 + color.B, color.A), Projectile.rotation, new Vector2(14f, 14f), Projectile.scale, SpriteEffects.None, 0);
            }
            else
            {
                Main.spriteBatch.Draw(Ball, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(255 * Stre2) + color.R, (int)(255 * Stre2) + color.G, (int)(255 * Stre2) + color.B, +color.A), Projectile.rotation, new Vector2(14f, 14f), Projectile.scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(Circle, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, 28 * ((int)(Main.time / 6d + Projectile.ai[0]) % 5), 28, 28), color, Projectile.rotation, new Vector2(14f, 14f), Projectile.scale, SpriteEffects.None, 0);
        }
    }
}