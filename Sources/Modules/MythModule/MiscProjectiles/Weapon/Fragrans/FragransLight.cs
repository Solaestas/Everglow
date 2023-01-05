using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Fragrans
{
	class FragransLight : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        float ka = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Range += 3;
            dx = 200 - Projectile.timeLeft;
            fx = (float)((-(1 / (dx / 4d + 0.25) + Math.Log(dx / 4d + 0.4)) + 3.95) * 40f);
            Projectile.velocity *= 0.99f;
            if (ka == 0)
            {
                ka = Main.rand.NextFloat(Main.rand.NextFloat(0.15f, 1f), 1f);
            }
        }

        private Effect ef;
        private Effect ef2;
        private float Range = 60;
        private float dx = 0;
        private float fx = 40;
        bool[] haihit = new bool[240];
        public override void PostDraw(Color lightColor)
        {
            double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            int width = (int)fx;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail2").Value;



            for (int i = 0; i < Range; ++i)
            {
                Player player = Main.player[Projectile.owner];
                Vector2 v = new Vector2(Range, 0).RotatedBy((i - Range / 2d) / (float)Range * 2);
                Vector2 v2 = new Vector2(v.X, v.Y * Projectile.ai[0]);
                Vector2 v3 = v2.RotatedBy(o1);
                /*for (int j = 0; j < 150; j++)
                {
                    if (Collision.SolidCollision(v3 * (1 + width / 200f) / (float)(Math.Sqrt(ka)) + Projectile.Center - Vector2.One * 5f, 10, 10))
                    {
                        v3 *= 0.95f;
                        if (!Main.gamePaused)
                        {
                            Projectile.velocity *= 0.999f;
                        }
                    }
                    else
                    {
                        break;
                    }
                }*/
                if (!Main.gamePaused)
                {
                    if (i % 15 == 0)
                    {
                        for (int j = 0; j < 200; j++)
                        {
                            if (!haihit[j])
                            {
                                if ((Main.npc[j].Center - (v3 * (1 + width / 200f) / (float)(Math.Sqrt(ka)) + Projectile.Center)).Length() < 40 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].life > 0)
                                {
                                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 15, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[1]);
                                    player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[1] / 100f)));
                                    haihit[j] = true;
                                    if (player.ownedProjectileCounts[ModContent.ProjectileType<Weapon.Fragrans.Fragrans>()] >= 1)
                                    {
                                        MiscProjectiles.Weapon.Fragrans.Fragrans.Reset = 300;
                                        if (Main.npc[j].type == NPCID.TargetDummy)
                                        {
                                            MiscProjectiles.Weapon.Fragrans.Fragrans.Dummy = true;
                                        }
                                        if (!hi)
                                        {
                                            Projectile.NewProjectile(null, player.Center - new Vector2(58), Vector2.Zero, ModContent.ProjectileType<Weapon.Fragrans.Fragrans3>(), 0, 0, player.whoAmI, 0, 0);
                                        }
                                    }
                                    hi = true;
                                }
                            }
                        }
                    }
                }
                var factor = i / (float)Range;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, 0.1f);
                if (i % 8 == 0)
                {
                    float h0 = 1;
                    if (Projectile.timeLeft < 60f)
                    {
                        h0 = Projectile.timeLeft / 60f;
                    }
                    float k0 = (float)(255 - Projectile.alpha) * (float)(Math.Sin(factor * Math.PI)) / 555f * h0;
                    Lighting.AddLight(Projectile.Center + v3 * (1 + width / 200f) / (float)(Math.Sqrt(ka)), k0 * 0.8f, k0 * 0.78f, 0);
                    Lighting.AddLight(Projectile.Center + v3 * (0.9f + width / 200f) / (float)(Math.Sqrt(ka)), k0 * 0.37f, k0 * 0.35f, k0 * 0.51f);
                }

                bars.Add(new Vertex2D(Projectile.Center + v3 * (1 - width / 200f) / (float)(Math.Sqrt(ka)), color, new Vector3((float)factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.Center + v3 * (1 + width / 200f) / (float)(Math.Sqrt(ka)), color, new Vector3((float)factor, 0, w)));
                if (!Main.gamePaused && Main.rand.Next(30 * (int)(dx + 2)) == 1)
                {
                    int num90 = Dust.NewDust(Projectile.Center + v3 * (1 + width / 200f) / (float)(Math.Sqrt(ka)), 0, 0, ModContent.DustType<MiscDusts.Fragrans.Fragrans>(), 0f, 0f, 0, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                    Main.dust[num90].noGravity = true;
                    Main.dust[num90].velocity = Projectile.velocity * 5;
                }
                if (!Main.gamePaused && Main.rand.Next(60 * (int)(dx + 2)) == 1)
                {
                    int num90 = Dust.NewDust(Projectile.Center + v3 * (1 + width / 200f) / (float)(Math.Sqrt(ka)), 0, 0, ModContent.DustType<MiscDusts.Fragrans.Fragrans2>(), 0f, 0f, 0, default(Color), Main.rand.NextFloat(1.0f, 1.3f));
                    Main.dust[num90].noGravity = true;
                    Main.dust[num90].velocity = Projectile.velocity * 5;
                }
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
                ef.Parameters["uTime"].SetValue(0);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapFragrans").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/MoonLight").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Grey").Value;
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
        bool hi = false;
    }
}
