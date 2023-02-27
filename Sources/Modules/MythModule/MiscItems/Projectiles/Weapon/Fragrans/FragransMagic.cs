using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	public class FragransMagic : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Magic");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "桂花魔法");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1800;
            Projectile.alpha = 0;
            Projectile.penetrate = 3;
            Projectile.scale = 0.7f;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        private float K = 10;
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            for (int y = 0; y < 6; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 3; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 7.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 50 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 8, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[0] / 100f) * 1.0f));
                }
            }
            float ad = Main.rand.NextFloat(0f, 6.28f);
            float Sc = Main.rand.NextFloat(0.85f, 1.15f);
            Projectile.NewProjectileDirect(null, Projectile.Center, new Vector2(0, 0.7f * Sc).RotatedByRandom(2d * Math.PI), ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransPedal>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Sc);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.Fragrans>()] < 1)
            {
                Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.Fragrans>(), 0, 0, player.whoAmI, 0, 0);
                player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragrans>(), 300);
            }
            else
            {
                MiscItems.Projectiles.Weapon.Fragrans.Fragrans.Reset = 300;
                if (target.type == NPCID.TargetDummy)
                {
                    MiscItems.Projectiles.Weapon.Fragrans.Fragrans.Dummy = true;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(0, 0, 0, 0));
        }
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.5);
            if (Projectile.timeLeft < 19)
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.timeLeft > 1000f)
            {
                Projectile.frame = Main.rand.Next(4);
                Projectile.timeLeft = Main.rand.Next(150, 200);
            }
            if (Projectile.velocity.Length() > 0.1f)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }
            if (Main.rand.NextBool(6))
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = Projectile.velocity * Main.rand.NextFloat(0.2f, 0.5f);
            }
            Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0] * Projectile.ai[0] * Projectile.ai[0] / 2d);
            Projectile.velocity *= 0.99f;
            if (Projectile.timeLeft < 30)
            {
                Projectile.scale = 0.5f + Projectile.timeLeft / 60f;
            }
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
                }
            }
            if (flag)
            {
                float num8 = 20f;
                Vector2 vector1 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num9 = num2 - vector1.X;
                float num10 = num3 - vector1.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                Projectile.velocity.X = (Projectile.velocity.X * 20f + num9) / 21f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num10) / 21f;
            }
            Player player = Main.player[Projectile.owner];
        }
        private Effect ef;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TrailB").Value;
            // 把所有的点都生成出来，按照顺序
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                //spriteBatch.Draw(Main.magicPixel, Projectile.oldPos[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                int width = 24;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Blue, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(13, 13) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(13, 13) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<Vertex2D> triangleList = new List<Vertex2D>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 3, Color.White, new Vector3(0, 0.5f, 1));
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
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f + Projectile.ai[0]);
                Texture2D Blue = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapFragrans").Value;
                Texture2D Shape = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Texture2D Mask = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Main.graphics.GraphicsDevice.Textures[0] = Blue;
                Main.graphics.GraphicsDevice.Textures[1] = Shape;
                Main.graphics.GraphicsDevice.Textures[2] = Mask;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

                ef.CurrentTechnique.Passes[0].Apply();


                //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null);
                //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}