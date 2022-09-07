using Everglow.Sources.Commons.Function.Vertex;
using Terraria.DataStructures;
using Terraria.GameContent;
namespace Everglow.Sources.Modules.MEACModule.NonTrueMeleeProj
{
    public class GoldShield : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
        }
        public void DrawWarp()
        {
            float WaveRange = 0.7f;
            //Texture2D BackG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/Black").Value;

            float k0 = (float)Math.Sqrt(1200 - Projectile.timeLeft) / 6f;//画方波
            if (k0 < 1 && k0 > 0)
            {
                k0 = Math.Max(k0 - 0.025f, 0);
                float k1 = 1 - k0;
                float k2 = k1 * k1;
                float k3 = (float)Math.Sqrt(k1);
                Vector2 DrawCen = Projectile.Center - Main.screenPosition;


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                KEx.CurrentTechnique.Passes[0].Apply();

                DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 150) * WaveRange, DrawCen + new Vector2(k0 * 75, -k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(k0 * 75, -k0 * 75) * WaveRange, DrawCen + new Vector2(k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 150) * WaveRange, DrawCen + new Vector2(-k0 * 75, -k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(-k0 * 75, -k0 * 75) * WaveRange, DrawCen + new Vector2(-k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));

                DrawDoubleLine(DrawCen + new Vector2(0, k0 * 150) * WaveRange, DrawCen + new Vector2(k0 * 75, k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(k0 * 75, k0 * 75) * WaveRange, DrawCen + new Vector2(k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(0, k0 * 150) * WaveRange, DrawCen + new Vector2(-k0 * 75, k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(-k0 * 75, k0 * 75) * WaveRange, DrawCen + new Vector2(-k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void DrawDoubleLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2)
        {
            Vector2 DrawCen = Projectile.Center - Main.screenPosition;
            float Wid = (Projectile.timeLeft - 1170) / 2f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;
            Vector2 WidthS = Vector2.Normalize(StartPos - DrawCen).RotatedBy(Math.PI / 2d) * Wid;
            Vector2 WidthE = Vector2.Normalize(EndPos - DrawCen).RotatedBy(Math.PI / 2d) * Wid;
            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            for (int x = 0; x < 3; x++)
            {
                vertex2Ds.Add(new Vertex2D(StartPos + WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos + WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
            }


            Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }

        public void DrawPost(Color color, int Width, float Height, float StarPos, Texture2D tex)
        {
            List<Vertex2D> vertex2Ds = new List<Vertex2D>();
            Vector2 DrawCen = Main.player[Projectile.owner].Center - Main.screenPosition - new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            if (DrawCen.Length() < 5f)
            {
                DrawCen = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            }
            else
            {
                DrawCen = Main.player[Projectile.owner].Center - Main.screenPosition;
            }

            float SPos = StarPos;
            float R = Width;
            for (int x = -Width; x < Width; x++)
            {
                float y = (float)Math.Sqrt(R * R - x * x);
                float newy = (float)Math.Sqrt(R * R - (x + 1) * (x + 1));

                float r1 = (float)(Math.Acos(Math.Clamp(x / R, -1, 1)) / Math.PI);
                float r2 = (float)(Math.Acos(Math.Clamp((x + 1) / R, -1, 1)) / Math.PI);

                float deltaY = newy - y;
                float length = (float)Math.Sqrt(deltaY * deltaY + 1);

                //vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x - 0.5f, Height), color, new Vector3(SPos, 1, 0)));
                //vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x + 0.5f, Height), color, new Vector3(SPos + length, 1, 0)));
                //vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x + 0.5f, -Height), color, new Vector3(SPos + length, 0, 0)));

                //vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x + 0.5f, -Height), color, new Vector3(SPos + length, 0, 0)));
                //vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x - 0.5f, Height), color, new Vector3(SPos, 1, 0)));
                //vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x - 0.5f, -Height), color, new Vector3(SPos, 0, 0)));

                const float scale = 0.25f;

                vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale - 0.5f, Height), color, new Vector3(r1, 1, 0)));
                vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale + 0.5f, Height), color, new Vector3(r2, 1, 0)));
                vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale + 0.5f, -Height), color, new Vector3(r2, 0, 0)));

                vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale + 0.5f, -Height), color, new Vector3(r2, 0, 0)));
                vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale - 0.5f, Height), color, new Vector3(r1, 1, 0)));
                vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale - 0.5f, -Height), color, new Vector3(r1, 0, 0)));


                SPos += length;
            }

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }
        public override void AI()
        {
            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0]--;
            }
            else
            {
                Projectile.ai[0] = 0;
            }
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.hide = true;
            Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16),0.8f,0.6f,0);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int LeftTime = 1200 - Projectile.timeLeft;
            float glowStrength = 0;
            float glowStrength2 = 0;

            if(Projectile.ai[0] > 0)
            {
                if (Projectile.ai[0] < 10)
                {
                    glowStrength2 = (float)(-Math.Cos((Projectile.ai[0]) / 5d * Math.PI) + 1) * 120f;
                }
            }


            if (LeftTime < 10)
            {
                glowStrength = (float)(-Math.Cos(LeftTime / 10d * Math.PI) + 1) * 120f;
            }
            else if(LeftTime < 40)
            {
                glowStrength = (float)(-Math.Cos((LeftTime + 75) / 30d * Math.PI) + 1) * 120f;
            }

            if (Projectile.timeLeft < 10)
            {
                glowStrength = (float)(-Math.Cos(Projectile.timeLeft / 10d * Math.PI) + 1) * 120f;
            }
            else if (Projectile.timeLeft < 40)
            {
                glowStrength = (float)(-Math.Cos((Projectile.timeLeft + 75) / 30d * Math.PI) + 1) * 120f;
            }
            if (glowStrength + glowStrength2 > 0)//光效
            {
                for(int x = 0;x < glowStrength + glowStrength2;x++)
                {
                    Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, -12) * Main.player[Projectile.owner].gravDir + new Vector2(x / 20f).RotatedBy(x), null, new Color(2, 2, 2, 0), Projectile.rotation, new Vector2(tex.Width / 2f, tex.Height / 2f), 1, SpriteEffects.None, 0);
                }
            }
            if (Projectile.timeLeft <= 9 && Projectile.timeLeft >= 0 && Projectile.timeLeft % 3 == 0)
            {
                for (int x = 0; x < 12; x++)
                {
                    float X = (float)Math.Sqrt(Main.rand.NextFloat(0, 0.5f));
                    float Y = (float)Math.Sqrt(Main.rand.NextFloat(0, 0.5f));
                    Vector2 v0 = new Vector2(X * Math.Sign(Main.rand.NextFloat(-1, 1)) * 38, Y * Math.Sign(Main.rand.NextFloat(-1, 1)) * 38);
                    int k = Dust.NewDust(Projectile.Center + v0 - new Vector2(4) + new Vector2(0, -12) * Main.player[Projectile.owner].gravDir, 0, 0, DustID.GoldFlame, 0, 0, 0, default, Main.rand.NextFloat(0.8f, 2f));
                    Main.dust[k].noGravity = true;

                }
            }
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, -12) * Main.player[Projectile.owner].gravDir, null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(tex.Width / 2f, tex.Height / 2f), 1, SpriteEffects.None, 0);





            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect Post = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/Post", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Post.Parameters["uTime"].SetValue((float)(Main.timeForVisualEffects * 0.005));
            Post.CurrentTechnique.Passes[0].Apply();

            Texture2D StoneSquire = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/GoldShieldGlowMap").Value;
            DrawPost(new Color(255, 255, 255, 0), 200, 50, 1, StoneSquire);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Post.CurrentTechnique.Passes[0].Apply();
            Texture2D StoneSquireD = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/GoldShieldDarkMap").Value;
            DrawPost(new Color(255, 255, 255, 255), 200, 50, 1, StoneSquireD);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            
            float WaveRange = 0.7f;
            //Texture2D BackG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/Black").Value;

            float k0 = (float)Math.Sqrt(1200 - Projectile.timeLeft) / 6f;//画方波
            if (k0 < 1 && k0 > 0)
            {
                k0 = Math.Max(k0 - 0.025f, 0);
                float k1 = 1 - k0;
                float k2 = k1 * k1;
                float k3 = (float)Math.Sqrt(k1);
                Vector2 DrawCen = Projectile.Center - Main.screenPosition;


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                KEx.CurrentTechnique.Passes[0].Apply();

                DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 150) * WaveRange, DrawCen + new Vector2(k0 * 75, -k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(k0 * 75, -k0 * 75) * WaveRange, DrawCen + new Vector2(k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 150) * WaveRange, DrawCen + new Vector2(-k0 * 75, -k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(-k0 * 75, -k0 * 75) * WaveRange, DrawCen + new Vector2(-k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));

                DrawDoubleLine(DrawCen + new Vector2(0, k0 * 150) * WaveRange, DrawCen + new Vector2(k0 * 75, k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(k0 * 75, k0 * 75) * WaveRange, DrawCen + new Vector2(k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(0, k0 * 150) * WaveRange, DrawCen + new Vector2(-k0 * 75, k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(-k0 * 75, k0 * 75) * WaveRange, DrawCen + new Vector2(-k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                KEx.CurrentTechnique.Passes[0].Apply();

                DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 120) * WaveRange, DrawCen + new Vector2(k0 * 60, -k0 * 60) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(k0 * 60, -k0 * 60) * WaveRange, DrawCen + new Vector2(k0 * 120, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 120) * WaveRange, DrawCen + new Vector2(-k0 * 60, -k0 * 60) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(-k0 * 60, -k0 * 60) * WaveRange, DrawCen + new Vector2(-k0 * 120, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));

                DrawDoubleLine(DrawCen + new Vector2(0, k0 * 120) * WaveRange, DrawCen + new Vector2(k0 * 60, k0 * 60) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(k0 * 60, k0 * 60) * WaveRange, DrawCen + new Vector2(k0 * 120, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(0, k0 * 120) * WaveRange, DrawCen + new Vector2(-k0 * 60, k0 * 60) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + new Vector2(-k0 * 60, k0 * 60) * WaveRange, DrawCen + new Vector2(-k0 * 120, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }
    public class GlodShieldPlayer : ModPlayer
    {
        public int immuneTime = 0;
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (immuneTime > 0)
            {
                return false;
            }
            immuneTime = 30;
            if (Player.longInvince)
            {
                immuneTime = 45;
            }
            for (int x = 0; x < Main.projectile.Length; x++)
            {
                if (Main.projectile[x].owner == Player.whoAmI)
                {
                    if (Main.projectile[x].type == ModContent.ProjectileType<GoldShield>() && Main.projectile[x].active)
                    {

                        if (Main.projectile[x].ai[1] >= damage)
                        {
                            Main.projectile[x].ai[1] -= damage;
                            Main.projectile[x].ai[0] = 10;
                            return false;
                        }
                        else
                        {
                            damage -= (int)Main.projectile[x].ai[1];
                            Main.projectile[x].ai[1] = 0;
                            Main.projectile[x].timeLeft = 14;
                        }
                    }
                }

            }

            return true;
        }
        public int DownUpdate(int value)
        {
            if (value > 0)
            {
                value--;
            }
            else
            {
                value = 0;
            }
            return value;
        }
        public override void PostUpdate()
        {
            immuneTime = DownUpdate(immuneTime);
            if(immuneTime > 0)
            {
                Player.immune = true;
            }
            base.PostUpdate();
        }
    }
}