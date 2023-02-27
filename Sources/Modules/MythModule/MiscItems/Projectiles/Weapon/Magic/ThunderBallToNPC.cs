using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Magic
{
    public class ThunderBallToNPC : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ThunderBall");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "雷暴球");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 10;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            if (Main.npc[(int)(Projectile.ai[1])].active)
            {
                Projectile.Center = Main.npc[(int)(Projectile.ai[1])].Center;
            }
            else
            {
                Projectile.extraUpdates = 20;
            }
            streng = (int)(Projectile.timeLeft / 30f);
        }
        private Effect ef;
        Vector2[,] vP = new Vector2[8, 600];
        Vector2[,] vvP = new Vector2[8, 600];
        Vector2[] VagueV = new Vector2[8];
        int streng = 9;
        public override void PostDraw(Color lightColor)
        {
            if (vP[0, 0] == Vector2.Zero)
            {
                for (int a = 0; a < 2; ++a)
                {
                    for (int i = 0; i < 600; ++i)
                    {
                        vP[a, i] = new Vector2(0, Main.rand.NextFloat(0.1f, 2.4f)).RotatedByRandom(6.283);
                    }
                }
            }
            if (vvP[0, 0] == Vector2.Zero)
            {
                for (int a = 0; a < 2; ++a)
                {
                    for (int i = 0; i < 600; ++i)
                    {
                        vvP[a, i] = new Vector2(0, Main.rand.NextFloat(0.03f, 0.4f)).RotatedByRandom(6.283);
                    }
                }
            }
            for (int a = 0; a < 2; ++a)
            {
                for (int i = 0; i < 600; ++i)
                {
                    if (vP[a, i].Length() < 3f)
                    {
                        vP[a, i] += vvP[a, i];
                    }
                    else
                    {
                        vvP[a, i] = new Vector2(0, Main.rand.NextFloat(0.03f, 0.4f)).RotatedByRandom(6.283);
                        vP[a, i] *= 0.95f;
                    }
                }
            }
            for (int a = 0; a < streng; ++a)
            {
                Player player = Main.player[Projectile.owner];
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                List<Vertex2D> bars = new List<Vertex2D>();
                ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
                float widx = Projectile.timeLeft / 120f;
                float widxM = 1f - widx;
                float width = widx * widx * 3f;
                Vector2 VStart = Projectile.Center;
                for (int i = 0; i < 600; ++i)
                {
                    Vector2 WholeLeng = Main.projectile[(int)(Projectile.ai[0])].Center - VStart;
                    if (WholeLeng.Length() < 4)
                    {
                        break;
                    }
                    Vector2 NDpos = Vector2.Normalize(Main.projectile[(int)(Projectile.ai[0])].Center - VStart);
                    Vector2 vDp = NDpos.RotatedBy(Math.PI / 2d);
                    var normalDir = Vector2.Normalize(vDp);

                    VStart += NDpos * 4 + vP[0, i];
                    var factor = i / 50f;
                    var color = Color.Lime;
                    var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                    Vector2 va = Vector2.Zero;
                    if (a > 1)
                    {
                        va = new Vector2(0, 1).RotatedBy(a / 4d * Math.PI);
                    }
                    bars.Add(new Vertex2D(VStart + normalDir * width + va, color, new Vector3(factor, 1, w)));
                    bars.Add(new Vertex2D(VStart + normalDir * -width + va, color, new Vector3(factor, 0, w)));
                }

                List<Vertex2D> triangleList = new List<Vertex2D>();

                if (bars.Count > 2)
                {
                    triangleList.Add(bars[0]);
                    var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
                    ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                    ef.Parameters["maxr"].SetValue(widxM * widxM);
                    Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlue2").Value;
                    Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/EShootDark").Value;
                    Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/EShootDark").Value;
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
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(0, 0, 0, 0));
        }
    }
}
