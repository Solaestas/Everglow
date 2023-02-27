using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	class FragransSpearfly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        private float K = 10;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int y = 0; y < 8; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f) * Projectile.timeLeft / 30f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d) * Projectile.velocity.Length() / 27f;
            }
            for (int y = 0; y < 3; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f) * Projectile.timeLeft / 30f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d) * Projectile.velocity.Length() / 27f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(Projectile.timeLeft / 20f, Projectile.timeLeft / 20f, Projectile.timeLeft / 20f, 0));
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num5) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num6);
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * (Main.rand.NextFloat(0.85f, 1.15f))), Projectile.knockBack, Projectile.direction, Main.rand.Next(100) < Projectile.ai[0]);
                        player.addDPS((int)(Projectile.damage * (100 + Projectile.ai[0]) / 100f));
                        Projectile.penetrate--;
                    }
                }
            }
            Projectile.velocity *= 0.96f;
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
            float num2 = Projectile.Center.X;
            float num3 = Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
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
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * (Main.rand.NextFloat(0.85f, 1.15f))), Projectile.knockBack, Projectile.direction, Main.rand.Next(100) < Projectile.ai[0]);
                        player.addDPS((int)(Projectile.damage * (100 + Projectile.ai[0]) / 100f));
                        Projectile.penetrate--;
                        NPC target = Main.npc[j];
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
                if (Projectile.velocity.Length() > 0.005f)
                {
                    Projectile.velocity *= 0.95f;
                }
            }
        }
        private Effect ef;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail").Value;
            // 把所有的点都生成出来，按照顺序
            double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            double o2 = Math.Atan2(Projectile.oldVelocity.Y, Projectile.oldVelocity.X);
            double omega = Math.Abs(o2 - o1) % MathHelper.TwoPi;
            int width = 30;
            if (Projectile.timeLeft < 10)
            {
                width = Projectile.timeLeft * 3;
            }
            if (Projectile.timeLeft > 20)
            {
                width = (30 - Projectile.timeLeft) * 3;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
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
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapFragrans").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/GoldLine").Value;
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
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int y = 0; y < 8; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f) * Projectile.timeLeft / 30f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d) * Projectile.velocity.Length() / 27f;
            }
            for (int y = 0; y < 3; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f) * Projectile.timeLeft / 30f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d) * Projectile.velocity.Length() / 27f;
            }
            Projectile.Kill();
            return true;
        }
    }
}
