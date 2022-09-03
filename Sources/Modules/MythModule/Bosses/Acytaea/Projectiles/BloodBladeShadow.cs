using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles
{
    public class BloodBladeShadow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BloodBlade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.scale = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }

        private float K = 10;

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(1f - 1f / Projectile.velocity.Length(), 1f - 1f / Projectile.velocity.Length(), 1f - 1f / Projectile.velocity.Length(), 0));
        }

        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
            Projectile.velocity *= 1 + 0.5f / Projectile.velocity.Length();
            if (K >= 40)
            {
                K *= 0.96f;
            }
            if (K <= 6)
            {
                K *= 1.05f;
            }
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            K += Main.rand.NextFloat(-0.025f, 0.025f);
            /*int num = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default(Color), 1f);
            Main.dust[num].noGravity = false;
            Main.dust[num].velocity *= 0;
            int num20 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) - new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default(Color), 1f);
            Main.dust[num20].noGravity = false;
            Main.dust[num20].velocity *= 0;
            int num21 = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default(Color), 1.5f);
            Main.dust[num21].velocity *= 0;
            int num22 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 8f)).RotatedByRandom(Math.PI * 2), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default(Color), 1.5f);
            Main.dust[num22].velocity *= 0.2f;
            if(Collision.CanHit(Projectile.Center, 1, 1, Main.player[Player.FindClosest(Projectile.Center, 60, 60)].Center, 1, 1))
            {
                Projectile.tileCollide = true;
            }*/
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i <= 32; i++)
            {
                float num4 = Main.rand.Next(500, 8000) * ((600 - timeLeft) / 600f + 0.4f);
                double num1 = Main.rand.Next(0, 1000) / 500f;
                double num2 = Math.Sin((double)num1 * Math.PI) * num4 / 40f;
                double num3 = Math.Cos((double)num1 * Math.PI) * num4 / 40f;
                /*int num5 = Projectile.NewProjectile(base.Projectile.Center.X, base.Projectile.Center.Y, (float)num2, (float)num3, base.mod.ProjectileType("RedGemDust"), 0, 0, base.Projectile.owner, 0f, 0f);
                Main.Projectile[num5].scale = Main.rand.Next(1150, 2200) / 1000f;*/
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(61, 61), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            Effect ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail").Value;
            // 把所有的点都生成出来，按照顺序
            int width = 40;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }
                //spriteBatch.Draw(Main.magicPixel, Projectile.oldPos[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(30, 30), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(30, 30), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<Vertex2D> triangleList = new List<Vertex2D>();

            if (bars.Count > 2)
            {
                // 按照顺序连接三角形
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
                // 干掉注释掉就可以只显示三角形栅格
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                // 把变换和所需信息丢给shader
                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/VisualTextures/heatmapGrey").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/VisualTextures/DarkGrey").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/VisualTextures/BladeShadow").Value;
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