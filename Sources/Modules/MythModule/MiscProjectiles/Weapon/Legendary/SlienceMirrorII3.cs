using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    class SlienceMirrorII3 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 360;
            Projectile.height = 360;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 680;
            Projectile.extraUpdates = 8;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 160;
        }
        private Vector2 v_1 = new Vector2(15, -56);
        private Vector2 v2 = Vector2.Zero;
        private bool Dir = false;
        private int Pdir = -1;
        private float Prot = 0;
        private float Siort = 1f;
        private bool ExtraKnife = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (v_1 == new Vector2(15, -56))
            {
                v_1 = new Vector2(8, -25);
            }
            if (!Dir)
            {
                Pdir = -Math.Sign(Main.mouseX - player.Center.X + Main.screenPosition.X);
                Siort = Main.rand.NextFloat(0.5f);
                Vector2 vc = -(new Vector2(Main.mouseX, Main.mouseY) - player.Center + Main.screenPosition);
                Prot = (float)Math.Atan2(vc.Y, vc.X);
                if (Pdir == 1)
                {
                    Prot += (float)(Math.PI);
                }
                Dir = true;
            }
            Vector2 v0 = v_1.RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft));
            if (ExtraKnife)
            {
                v0 = v_1.RotatedBy(1.2 / 170d * Math.PI * (650));
            }
            if (Projectile.timeLeft == 32 && !ExtraKnife)
            {
                ExtraKnife = true;
                Projectile.timeLeft = 170;
            }
            if (Projectile.timeLeft < 30)
            {
                Projectile.Kill();
                v0 = v_1.RotatedBy(1.6 * Math.PI);
            }
            if (Projectile.timeLeft > 560 && Projectile.timeLeft % 4 == 0)
            {
                Projectile.extraUpdates++;
            }
            if (Projectile.timeLeft < 170 && Projectile.timeLeft % 4 == 0 && ExtraKnife)
            {
                Projectile.extraUpdates--;
            }
            Projectile.spriteDirection = Pdir;
            v0.X *= Pdir;
            Vector2 v1 = new Vector2(v0.X, v0.Y * Siort).RotatedBy(Prot) - new Vector2(80, 80);
            Projectile.position = player.Center + v1 * 1.3f;
            v2 = Projectile.Center - player.Center;
            v2.X *= Pdir;
            float Rot = (float)(Math.Atan2(v2.Y, v2.X) + Math.PI / 4d * Pdir);
            Projectile.rotation = Rot;
            Projectile.velocity = v2.RotatedBy(Math.PI / 2d) / v2.Length();

            /*if (Projectile.timeLeft == 306)
            {
                Vector2 vnp0 = player.Center + new Vector2(0, Main.rand.NextFloat(140f, 245f)).RotatedByRandom(Math.PI * 2d);
                Vector2 vnp1 = Main.MouseWorld - vnp0;
                vnp1 = vnp1 / vnp1.Length() * 10f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), vnp0 - vnp1 * 70f, vnp1 * 4.6f, ModContent.ProjectileType<Projectiles.Melee.SilenCrack2>(), Projectile.damage, Projectile.knockBack, player.whoAmI, player.GetCritChance(DamageClass.Melee) + 25, 0f);
            }*/
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D t = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            if (ExtraKnife)
            {
                for (int k = 0; k < Projectile.timeLeft - 120; k++)
                {
                    if (k % 15 == 2)
                    {
                        Vector2 v3 = v_1.RotatedBy(1.2 / 170d * Math.PI * (650 - k) + 0.4) * 4.5f * (1 - (1 - Siort) / 2f);
                        if (Projectile.timeLeft < 30)
                        {
                            v3 = v_1.RotatedBy(1.6 * Math.PI) * 3.5f * (1 - (1 - Siort) / 2f);
                        }
                        v3.X *= Pdir;
                        Vector2 v4 = new Vector2(v3.X, v3.Y * Siort).RotatedBy(Prot);
                        Vector2 v5 = new Vector2(v3.X, v3.Y).RotatedBy(Prot);
                        Vector2 drawPos = player.Center + v4 - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY) - new Vector2(80);
                        Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
                        Color color2 = new Color((int)(color.R * (50 - k) / (float)50), (int)(color.G * (50 - k) / (float)50), (int)(color.B * (50 - k) / (float)50), (int)(color.A * (50 - k) / (float)50));
                        float Rot = (float)(Math.Atan2(v5.Y, v5.X) + Math.PI / 4d * Pdir + Math.PI * (1 - Pdir) / 2d);
                        SpriteEffects S = SpriteEffects.None;
                        if (Pdir == -1)
                        {
                            S = SpriteEffects.FlipHorizontally;
                        }
                        Main.spriteBatch.Draw(t, drawPos, null, color2, Rot, drawOrigin, Projectile.scale, S, 0f);
                    }
                }
                return false;
            }
            for (int k = 0; k < 50; k++)
            {
                if (k % 15 == 2)
                {
                    Vector2 v3 = v_1.RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft - k) + 0.4) * 4.5f * (1 - (1 - Siort) / 2f);
                    if (Projectile.timeLeft < 30)
                    {
                        v3 = v_1.RotatedBy(1.6 * Math.PI) * 3.5f * (1 - (1 - Siort) / 2f);
                    }
                    v3.X *= Pdir;
                    Vector2 v4 = new Vector2(v3.X, v3.Y * Siort).RotatedBy(Prot);
                    Vector2 v5 = new Vector2(v3.X, v3.Y).RotatedBy(Prot);
                    Vector2 drawPos = player.Center + v4 - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY) - new Vector2(80);
                    Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
                    Color color2 = new Color((int)(color.R * (50 - k) / (float)50), (int)(color.G * (50 - k) / (float)50), (int)(color.B * (50 - k) / (float)50), (int)(color.A * (50 - k) / (float)50));
                    float Rot = (float)(Math.Atan2(v5.Y, v5.X) + Math.PI / 4d * Pdir + Math.PI * (1 - Pdir) / 2d);
                    SpriteEffects S = SpriteEffects.None;
                    if (Pdir == -1)
                    {
                        S = SpriteEffects.FlipHorizontally;
                    }
                    Main.spriteBatch.Draw(t, drawPos, null, color2, Rot, drawOrigin, Projectile.scale, S, 0f);
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
        }
        //private int DelOme = 0;
        private Effect ef;
        private Effect ef2;
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail2").Value;
            double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            double o2 = Math.Atan2(Projectile.oldVelocity.Y, Projectile.oldVelocity.X);
            double omega = Math.Abs(o2 - o1) % MathHelper.TwoPi;
            int width = 84;
            int Maxz = 160;
            if (680 - Projectile.timeLeft < 160)
            {
                Maxz = 680 - Projectile.timeLeft;
            }
            if (ExtraKnife)
            {
                Maxz = Projectile.timeLeft - 10;
            }
            if (ExtraKnife)
            {
                for (int i = 1; i < Maxz; ++i)
                {

                    Vector2 v3 = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (650 - i)) * 7.6f;
                    if (Projectile.timeLeft < 30)
                    {
                        v3 = (v_1 * 1.0f).RotatedBy(1.6 * Math.PI) * 7.6f;
                    }
                    v3.X *= Pdir;
                    Vector2 v4 = new Vector2(v3.X, v3.Y * Siort).RotatedBy(Prot);
                    var normalDir = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (650 - i + 1)) * 7.6f - (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (650 - i)) * 7.6f;
                    if (Projectile.timeLeft < 30)
                    {
                        if (i < 30 - Projectile.timeLeft)
                        {
                            normalDir = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * 170) * 7.6f - (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * 169.99) * 7.6f;
                        }
                    }
                    normalDir.X *= Pdir;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(Prot);

                    var factor = i / (float)Maxz;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    bars.Add(new Vertex2D(player.Center + v4 + normalDir * width * 2, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new Vertex2D(player.Center + v4 + normalDir * -width * 2, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                    if (!Main.gamePaused)
                    {
                        if (Main.rand.Next(25) == 1)
                        {
                            Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Center + v4 + normalDir, normalDir.RotatedBy(1.57) * -10f, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorIIBroken>(), Projectile.damage, 1, Main.myPlayer, Projectile.ai[0], 0f);
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i < Maxz; ++i)
                {

                    Vector2 v3 = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft - i)) * 7.6f;
                    if (Projectile.timeLeft < 30)
                    {
                        v3 = (v_1 * 1.0f).RotatedBy(1.6 * Math.PI) * 7.6f;
                    }
                    v3.X *= Pdir;
                    Vector2 v4 = new Vector2(v3.X, v3.Y * Siort).RotatedBy(Prot);
                    var normalDir = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft - i + 1)) * 7.6f - (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft - i)) * 7.6f;
                    if (Projectile.timeLeft < 30)
                    {
                        if (i < 30 - Projectile.timeLeft)
                        {
                            normalDir = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * 170) * 7.6f - (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * 169.99) * 7.6f;
                        }
                    }
                    normalDir.X *= Pdir;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(Prot);

                    var factor = i / (float)Maxz;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);

                    if (Projectile.timeLeft < 120)
                    {
                        width = 0;
                    }
                    bars.Add(new Vertex2D(player.Center + v4 + normalDir * width * 2, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new Vertex2D(player.Center + v4 + normalDir * -width * 2, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                    if (!Main.gamePaused)
                    {
                        if (Main.rand.Next(25) == 1)
                        {
                            Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Center + v4 + normalDir, normalDir.RotatedBy(1.57) * -10f, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorIIBroken>(), Projectile.damage, 1, Main.myPlayer, Projectile.ai[0], 0f);
                        }
                    }
                    if (!Main.gamePaused)
                    {
                        if (i % 15 == 0)
                        {
                            for (int j = 0; j < 200; j++)
                            {
                                if ((Main.npc[j].Center - (player.Center + v4 + normalDir * width * 2)).Length() < 75 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].life > 0)
                                {
                                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 15, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < 25 + player.GetCritChance(DamageClass.Melee));
                                    player.addDPS((int)(Projectile.damage * (1 + (25 + player.GetCritChance(DamageClass.Melee)) / 100f)));

                                }
                            }
                        }
                    }
                }
            }
            Vector2 v32 = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft)) * 7.6f;
            v32.X *= Pdir;
            Vector2 v42 = new Vector2(v32.X, v32.Y * Siort).RotatedBy(Prot);

            List<Vertex2D> triangleList = new List<Vertex2D>();

            if (bars.Count > 2)
            {
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
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(0);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlue").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ForgeWave").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/MirrorB" + (1 + Pdir).ToString()).Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef.CurrentTechnique.Passes[0].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> barz = new List<Vertex2D>();
            ef2 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlueFlame").Value;

            int widtf = 84;
            int Maxh = 240;
            if (680 - Projectile.timeLeft < 240)
            {
                Maxh = 680 - Projectile.timeLeft;
            }
            if (ExtraKnife)
            {
                Maxh = 240;
            }
            if (ExtraKnife)
            {
                for (int i = 1; i < Maxh; ++i)
                {
                    float kMax = 1 + i / 160f + (170 - Projectile.timeLeft) / 170f;
                    Vector2 v3 = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (650 - i)) * 7.6f;
                    if (Projectile.timeLeft < 30)
                    {
                        v3 = (v_1 * 1.0f).RotatedBy(1.6 * Math.PI) * 7.6f;
                    }
                    v3.X *= Pdir;
                    Vector2 v4 = new Vector2(v3.X, v3.Y * Siort).RotatedBy(Prot);
                    var normalDir = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (650 - i + 1)) * 7.6f - (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (650 - i)) * 7.6f;
                    if (Projectile.timeLeft < 30)
                    {
                        if (i < 30 - Projectile.timeLeft)
                        {
                            normalDir = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * 170) * 7.6f - (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * 169.99) * 7.6f;
                        }
                    }
                    normalDir.X *= Pdir;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(Prot);

                    var factor = i / (float)Maxh;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    barz.Add(new Vertex2D(player.Center + v4 * kMax + normalDir * widtf * 2, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    barz.Add(new Vertex2D(player.Center + v4 * kMax + normalDir * -widtf * 2, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }
            else
            {
                for (int i = 1; i < Maxh; ++i)
                {
                    float kMax = 1 + i / 160f;
                    Vector2 v3 = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft - i)) * 7.6f;
                    if (Projectile.timeLeft < 30)
                    {
                        v3 = (v_1 * 1.0f).RotatedBy(1.6 * Math.PI) * 7.6f;
                    }
                    v3.X *= Pdir;
                    Vector2 v4 = new Vector2(v3.X, v3.Y * Siort).RotatedBy(Prot);
                    var normalDir = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft - i + 1)) * 7.6f - (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft - i)) * 7.6f;
                    if (Projectile.timeLeft < 30)
                    {
                        if (i < 30 - Projectile.timeLeft)
                        {
                            normalDir = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * 170) * 7.6f - (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * 169.99) * 7.6f;
                        }
                    }
                    normalDir.X *= Pdir;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(Prot);

                    var factor = i / (float)Maxh;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    if (!Main.gamePaused)
                    {
                        if (i % 15 == 0)
                        {
                            for (int j = 0; j < 200; j++)
                            {
                                if ((Main.npc[j].Center - (player.Center + v4 * kMax + normalDir * widtf * 2)).Length() < 75 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].life > 0)
                                {
                                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.45f, 0.85f)), 15, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < 25 + player.GetCritChance(DamageClass.Melee));
                                    player.addDPS((int)(Projectile.damage * (0.65 + (25 + player.GetCritChance(DamageClass.Melee)) / 100f)));

                                }
                            }
                        }
                    }
                    barz.Add(new Vertex2D(player.Center + v4 * kMax + normalDir * widtf * 2, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    barz.Add(new Vertex2D(player.Center + v4 * kMax + normalDir * -widtf * 2, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }

            Vector2 v32a = (v_1 * 1.0f).RotatedBy(1.2 / 170d * Math.PI * (680 - Projectile.timeLeft)) * 7.6f;
            v32a.X *= Pdir;
            Vector2 v42a = new Vector2(v32a.X, v32a.Y * Siort).RotatedBy(Prot);

            List<Vertex2D> triangleLisd = new List<Vertex2D>();

            if (barz.Count > 2)
            {
                triangleLisd.Add(barz[0]);
                var vertex = new Vertex2D((barz[0].position + barz[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 3, Color.White, new Vector3(0, 0.5f, 1));
                triangleLisd.Add(barz[1]);
                triangleLisd.Add(vertex);
                for (int i = 0; i < barz.Count - 2; i += 2)
                {
                    triangleLisd.Add(barz[i]);
                    triangleLisd.Add(barz[i + 2]);
                    triangleLisd.Add(barz[i + 1]);

                    triangleLisd.Add(barz[i + 1]);
                    triangleLisd.Add(barz[i + 2]);
                    triangleLisd.Add(barz[i + 3]);
                }
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                ef2.Parameters["uTransform"].SetValue(model * projection);
                ef2.Parameters["uTime"].SetValue(Projectile.ai[1]);
                float Str = 1;
                if (ExtraKnife)
                {
                    Str = Projectile.timeLeft / 170f;
                }
                ef2.Parameters["Stre"].SetValue(Str);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlue").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceBeta3").Value;//
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceBeta2").Value;
                Main.graphics.GraphicsDevice.Textures[3] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/MirrorB" + (1 + Pdir).ToString()).Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[3] = SamplerState.PointWrap;
                ef2.CurrentTechnique.Passes[0].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleLisd.ToArray(), 0, triangleLisd.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
