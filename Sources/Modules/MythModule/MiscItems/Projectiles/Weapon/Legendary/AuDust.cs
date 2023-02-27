using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class AuDust : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("金火粒");
        }
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }
        private bool Dar = false;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.5f / 255f, (float)(255 - Projectile.alpha) * 0.3f / 255f, (float)(255 - Projectile.alpha) * 0f / 255f);

            if (Projectile.velocity.Length() < 0.5f)
            {
                Dar = true;
                Projectile.velocity *= 0.99f;
            }
            else
            {
                Projectile.velocity *= 0.9f;
            }
            float num2 = Projectile.Center.X;
            float num3 = Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(Projectile.Center, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num5) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num2 = num5;
                        num3 = num6;
                        flag = true;
                    }
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), Projectile.knockBack, Projectile.direction, Main.rand.Next(100) < Projectile.ai[0]);
                        Projectile.penetrate--;
                        NPC target = Main.npc[j];
                        Player player = Main.player[Projectile.owner];
                        player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[0] / 100f)));
                        if (Projectile.penetrate == 0)
                        {
                            Projectile.Kill();
                        }
                    }
                }
            }
            if (flag)
            {
                float num8 = 50f;
                Vector2 vector1 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num9 = num2 - vector1.X;
                float num10 = num3 - vector1.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                Projectile.velocity.X = (Projectile.velocity.X * 20f + num9) / 21f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num10) / 21f;
                Projectile.velocity *= 0.95f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(0, 0, 0, 0));
        }
        private int DelOme = 0;
        private Effect ef;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail").Value;
            double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            double o2 = Math.Atan2(Projectile.oldVelocity.Y, Projectile.oldVelocity.X);
            double omega = Math.Abs(o2 - o1) % MathHelper.TwoPi;
            int width = 0;
            width = 12;
            if (Dar && Projectile.velocity.Length() < 0.5f)
            {
                width = (int)(width * (2 * Projectile.velocity.Length()));
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(4, 4), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(4, 4), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }
            List<Vertex2D> triangleList = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Projectile.velocity, Color.White, new Vector3(0, 0.5f, 1));
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
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapDarkRedGold2").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
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
}
