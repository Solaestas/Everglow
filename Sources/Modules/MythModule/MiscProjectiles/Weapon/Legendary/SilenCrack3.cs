using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Graphics.Effects;
using Terraria.Localization;


namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class SilenCrack3 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silence Mirror");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "寂镜之空切");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 60;
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.tileCollide = false;
        }
        private Effect ef2;
        public override void AI()
        {
            if (Projectile.velocity.Length() > 46)
            {
                Projectile.velocity = Projectile.velocity / Projectile.velocity.Length() * 46;
            }
            if (Projectile.timeLeft > 50)
            {
                if (!Filters.Scene["SLECR3"].IsActive())
                {
                    Filters.Scene.Activate("SLECR3");
                }
            }
            if (Projectile.timeLeft < 25)
            {
                if (Filters.Scene["SLECR3"].IsActive())
                {
                    Filters.Scene.Deactivate("SLECR3");
                    ef2.Parameters["Strds"].SetValue(0);
                    ef2.Parameters["DMax"].SetValue(0.001f);
                    Projectile.Kill();
                }
            }
            ef2 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
            float k0 = Projectile.velocity.Y / (float)Projectile.velocity.X;
            k0 *= (float)Main.screenWidth / (float)Main.screenHeight;
            ef2.Parameters["k0"].SetValue(k0);
            Vector2 v0 = Projectile.Center - Main.screenPosition;
            float Correc = (float)Main.screenWidth / (float)Main.screenHeight;
            float x0 = v0.X / (float)Main.screenWidth;
            float y0 = v0.Y / (float)Main.screenHeight;
            float b0 = y0 - k0 * x0;
            ef2.Parameters["b0"].SetValue(b0);
            ef2.Parameters["x1"].SetValue(x0);
            ef2.Parameters["y1"].SetValue(y0);
            ef2.Parameters["uTime"].SetValue((float)Main.time * 0.06f);
            float zoom = (Projectile.timeLeft - 25) / 250f * (Projectile.timeLeft - 25) / 50f;
            float DMax = (Projectile.timeLeft - 25) / 450f;
            ef2.Parameters["DMax"].SetValue(DMax);
            if (zoom < 0)
            {
                zoom = 0;
            }

            ef2.Parameters["Strds"].SetValue((0.098f - zoom) / (DMax * DMax * DMax * DMax) / 28000f * Math.Sign(Projectile.velocity.X));

            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 80 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    Player player = Main.player[Projectile.owner];
                    player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[0] / 100f) * 1.6f));
                }
            }

        }

        private bool fi = true;
        private Effect ef;
        private bool max = false;
        private int pre = 0;
        private Vector2[] vpos = new Vector2[15];
        private Vector2 sTp;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail").Value;
            // 把所有的点都生成出来，按照顺序
            int width = (Projectile.timeLeft - 25) * 3;
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;
            if (Projectile.timeLeft == duration - 8)
            {
                Vector2 vp = (player.Center - sTp).RotatedBy(Math.PI / 2d);
                float Ve = vp.X * Projectile.velocity.X + vp.Y * Projectile.velocity.Y;
                if (Math.Abs(Ve) <= 5f)
                {
                    for (int i = 1; i < Projectile.oldPos.Length; ++i)
                    {
                        vpos[i] = Projectile.oldPos[i];
                    }
                }
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                if (vpos[i] != Vector2.Zero)
                {
                    var normalDir = Projectile.velocity;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    Vector2 deltaPos = Projectile.position - vpos[1];
                    bars.Add(new Vertex2D(vpos[i] + normalDir * width + new Vector2(40) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new Vertex2D(vpos[i] + normalDir * -width + new Vector2(40) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                else
                {
                    var normalDir = Projectile.velocity;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(40), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(40), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }

            List<Vertex2D> triangleList = new List<Vertex2D>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                Vector2 vo = (bars[0].position - bars[1].position) / (bars[0].position - bars[1].position).Length();
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + vo.RotatedBy(-Math.PI / 2d) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
                // 干掉注释掉就可以只显示三角形栅格
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                // 把变换和所需信息丢给shader
                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(0);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBloodTusk").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/GoldLine2").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

                ef.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
