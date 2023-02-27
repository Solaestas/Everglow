using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	class FragransSpear : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 130;
            Projectile.height = 130;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
        }
        private Vector2 v_1 = new Vector2(-90, -54);
        private Vector2 v2 = Vector2.Zero;
        private bool Dir = false;
        private int Pdir = 1;
        private float Prot = 0;
        private bool ExtraKnife = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Weapon.Fragrans.Fragrans>()] < 1)
            {
                Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<Weapon.Fragrans.Fragrans>(), 0, 0, player.whoAmI, 0, 0);
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
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Dir)
            {
                Pdir = Math.Sign(Main.mouseX - player.Center.X + Main.screenPosition.X);
                Vector2 vc = -(new Vector2(Main.mouseX, Main.mouseY) - player.Center + Main.screenPosition);
                Prot = (float)Math.Atan2(vc.Y, vc.X);
                if (Pdir == 1)
                {
                    Prot += (float)(Math.PI);
                }
                Dir = true;
            }
            if (Main.mouseRight && MiscItems.Projectiles.Weapon.Fragrans.Fragrans.Dashcool == 0)
            {
                MiscItems.Weapons.Fragrans.FragransSpear.CoolRarr = 60;
                MiscItems.Projectiles.Weapon.Fragrans.Fragrans.Dashcool = 60;
                Vector2 velocity = Main.MouseWorld - Projectile.Center;
                velocity.Normalize();
                Projectile.NewProjectile(null, Projectile.Center, velocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpear4>(), Projectile.damage * 5, Projectile.knockBack, player.whoAmI, 0f, 0f);
                Projectile.Kill();
            }
            Vector2 v0 = v_1.RotatedBy(1.6 / 170d * Math.PI * (200 - Projectile.timeLeft));
            if (Projectile.timeLeft < 30)
            {
                Projectile.Kill();
                v0 = v_1.RotatedBy(1.6 * Math.PI);
            }
            if (Projectile.timeLeft == 32 && !ExtraKnife)
            {
                ExtraKnife = true;
                Projectile.timeLeft = 70;
            }
            Projectile.spriteDirection = Pdir;
            v0.X *= Pdir;
            Vector2 v1 = new Vector2(v0.X, v0.Y * 0.5f).RotatedBy(Prot) - new Vector2(60, 60);
            Projectile.position = player.Center + v1;
            v2 = Projectile.Center - player.Center;
            v2.X *= Pdir;
            float Rot = (float)(Math.Atan2(v2.Y, v2.X) + Math.PI / 4d * Pdir);
            Projectile.rotation = Rot;
            Projectile.velocity = v2.RotatedBy(Math.PI / 2d) / v2.Length();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Fragrans/FragransSpear").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            if (ExtraKnife)
            {
                for (int k = 0; k < (Projectile.timeLeft - 25) * 2; k++)
                {
                    if (k % 10 == 2)
                    {
                        float K0 = (Projectile.timeLeft - 30) / 40f;
                        Vector2 v3 = v_1.RotatedBy(1.6 / 170d * Math.PI * (240 - Projectile.timeLeft * K0 - k * K0)) * 1.5f;
                        if (Projectile.timeLeft < 30)
                        {
                            v3 = v_1.RotatedBy(1.6 * Math.PI) * 1.5f;
                        }
                        v3.X *= Pdir;
                        Vector2 v4 = new Vector2(v3.X, v3.Y * 0.5f).RotatedBy(Prot);
                        Vector2 v5 = new Vector2(v3.X, v3.Y).RotatedBy(Prot);
                        Vector2 drawPos = player.Center + v4 * 1.30f - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY) - new Vector2(60);
                        Vector2 drawPosp = player.Center - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY) - new Vector2(60);
                        Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
                        float Kc = ((Projectile.timeLeft - 25) * 2 - k) / (float)((Projectile.timeLeft - 25) * 2);
                        Color color2 = new Color((int)(color.R * Kc), (int)(color.G * Kc), (int)(color.B * Kc), (int)(color.A * Kc));
                        float Rot = (float)(Math.Atan2(v5.Y, v5.X) + Math.PI / 4d * Pdir + Math.PI * (1 - Pdir) / 2d);
                        SpriteEffects S = SpriteEffects.None;
                        if (Pdir == -1)
                        {
                            S = SpriteEffects.FlipHorizontally;
                        }
                        //Main.spriteBatch.Draw(t, drawPos, null, color2, Rot, drawOrigin, Projectile.scale, S, 0f);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        List<Vertex2D> Vx = new List<Vertex2D>();
                        Vector2 vc = (drawPos - drawPosp).RotatedBy(-Math.PI / 2d) / 2f;
                        Vector2 vd = drawPosp + (drawPos - drawPosp) / 2f + vc;
                        Vector2 ve = drawPosp + (drawPos - drawPosp) / 2f - vc;
                        Vx.Add(new Vertex2D(vd, color2, new Vector3(0, 0, 0)));
                        Vx.Add(new Vertex2D(drawPos, color2, new Vector3(1, 0, 0)));
                        Vx.Add(new Vertex2D(drawPosp, color2, new Vector3(0, 1, 0)));

                        Vx.Add(new Vertex2D(drawPos, color2, new Vector3(1, 0, 0)));
                        Vx.Add(new Vertex2D(ve, color2, new Vector3(1, 1, 0)));
                        Vx.Add(new Vertex2D(drawPosp, color2, new Vector3(0, 1, 0)));
                        Main.graphics.GraphicsDevice.Textures[0] = t;
                        Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
                    }
                }
            }
            else
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    if (Projectile.oldPos[k] == Vector2.Zero)
                        break;
                    if (k % 10 == 2)
                    {
                        Vector2 v3 = v_1.RotatedBy(1.6 / 170d * Math.PI * (200 - Projectile.timeLeft - k)) * 1.5f;
                        if (Projectile.timeLeft < 30)
                        {
                            v3 = v_1.RotatedBy(1.6 * Math.PI) * 1.5f;
                        }
                        v3.X *= Pdir;
                        Vector2 v4 = new Vector2(v3.X, v3.Y * 0.5f).RotatedBy(Prot);
                        Vector2 v5 = new Vector2(v3.X, v3.Y).RotatedBy(Prot);
                        Vector2 drawPos = player.Center + v4 * 1.30f - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY) - new Vector2(60);
                        Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
                        Color color2 = new Color((int)(color.R * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.G * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.B * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.A * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
                        float Rot = (float)(Math.Atan2(v5.Y, v5.X) + Math.PI / 4d * Pdir + Math.PI * (1 - Pdir) / 2d);
                        SpriteEffects S = SpriteEffects.None;
                        if (Pdir == -1)
                        {
                            S = SpriteEffects.FlipHorizontally;
                        }
                        //Main.spriteBatch.Draw(t, drawPos, null, color2, Rot, drawOrigin, Projectile.scale, S, 0f);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        Vector2 drawPosp = player.Center - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY) - new Vector2(60);
                        List<Vertex2D> Vx = new List<Vertex2D>();
                        Vector2 vc = (drawPos - drawPosp).RotatedBy(-Math.PI / 2d) / 2f;
                        Vector2 vd = drawPosp + (drawPos - drawPosp) / 2f + vc;
                        Vector2 ve = drawPosp + (drawPos - drawPosp) / 2f - vc;
                        Vx.Add(new Vertex2D(vd, color2, new Vector3(0, 0, 0)));
                        Vx.Add(new Vertex2D(drawPos, color2, new Vector3(1, 0, 0)));
                        Vx.Add(new Vertex2D(drawPosp, color2, new Vector3(0, 1, 0)));

                        Vx.Add(new Vertex2D(drawPos, color2, new Vector3(1, 0, 0)));
                        Vx.Add(new Vertex2D(ve, color2, new Vector3(1, 1, 0)));
                        Vx.Add(new Vertex2D(drawPosp, color2, new Vector3(0, 1, 0)));
                        Main.graphics.GraphicsDevice.Textures[0] = t;
                        Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
                    }
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
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail2").Value;
            double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            double o2 = Math.Atan2(Projectile.oldVelocity.Y, Projectile.oldVelocity.X);
            double omega = Math.Abs(o2 - o1) % MathHelper.TwoPi;
            int width = 40;
            if (ExtraKnife)
            {
                for (int i = 1; i < (Projectile.timeLeft - 25) * 2; ++i)
                {
                    float K0 = (Projectile.timeLeft - 30) / 40f;
                    Vector2 v3 = v_1.RotatedBy(1.6 / 170d * Math.PI * (240 - Projectile.timeLeft * K0 - i)) * 1.5f;
                    if (Projectile.timeLeft < 30)
                    {
                        v3 = v_1.RotatedBy(1.6 * Math.PI) * 1.5f;
                    }
                    v3.X *= Pdir;
                    Vector2 v4 = new Vector2(v3.X, v3.Y * 0.5f).RotatedBy(Prot);
                    var normalDir = v_1.RotatedBy(1.6 / 170d * Math.PI * (200 - Projectile.timeLeft - i + 1)) * 1.5f - v_1.RotatedBy(1.6 / 170d * Math.PI * (200 - Projectile.timeLeft - i)) * 1.5f;
                    if (Projectile.timeLeft < 30)
                    {
                        if (i < 30 - Projectile.timeLeft)
                        {
                            normalDir = v_1.RotatedBy(1.6 / 170d * Math.PI * 170) * 1.5f - v_1.RotatedBy(1.6 / 170d * Math.PI * 169.99) * 1.5f;
                        }
                    }
                    normalDir.X *= Pdir;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(Prot);

                    var factor = i / (float)((Projectile.timeLeft - 25) * 2);
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);

                    Player player = Main.player[Projectile.owner];

                    bars.Add(new Vertex2D(player.Center + v4 + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new Vertex2D(player.Center + v4 + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                    if (!Main.gamePaused && Main.rand.NextBool(192))
                    {
                        int num90 = Dust.NewDust(player.Center + v4 + normalDir * -width * Main.rand.NextFloat(Main.rand.NextFloat(-0.5f, 0.8f), 0.8f) * player.direction, 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans>(), 0f, 0f, 0, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                        Main.dust[num90].noGravity = true;
                        Main.dust[num90].velocity = v4.RotatedBy(Math.PI / 2d * player.direction) / v4.Length();
                    }
                    if (!Main.gamePaused && Main.rand.NextBool(384))
                    {
                        int num90 = Dust.NewDust(player.Center + v4 + normalDir * -width * Main.rand.NextFloat(Main.rand.NextFloat(-0.5f, 0.8f), 0.8f) * player.direction, 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans2>(), 0f, 0f, 0, default(Color), Main.rand.NextFloat(1.0f, 1.3f));
                        Main.dust[num90].noGravity = true;
                        Main.dust[num90].velocity = v4.RotatedBy(Math.PI / 2d * player.direction) / v4.Length();
                    }
                    if (!Main.gamePaused && Projectile.timeLeft % 3 == 0 && i == 1 && !Collision.SolidCollision(player.Center + v4 + normalDir * -width * player.direction - Vector2.One * 5f, 10, 10))
                    {
                        int num90 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Center + v4 + normalDir * -width * player.direction, v4.RotatedBy(Math.PI / 2d * player.direction) / 4f, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpearfly>(), Projectile.damage / 3 * 2, Projectile.knockBack, player.whoAmI, player.GetCritChance(DamageClass.Melee) + 8, 0f);
                    }
                }
            }
            else
            {
                for (int i = 1; i < Projectile.oldPos.Length; ++i)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero)
                        break;
                    Vector2 v3 = v_1.RotatedBy(1.6 / 170d * Math.PI * (200 - Projectile.timeLeft - i)) * 1.5f;
                    if (Projectile.timeLeft < 30)
                    {
                        v3 = v_1.RotatedBy(1.6 * Math.PI) * 1.5f;
                    }
                    v3.X *= Pdir;
                    Vector2 v4 = new Vector2(v3.X, v3.Y * 0.5f).RotatedBy(Prot);
                    var normalDir = v_1.RotatedBy(1.6 / 170d * Math.PI * (200 - Projectile.timeLeft - i + 1)) * 1.5f - v_1.RotatedBy(1.6 / 170d * Math.PI * (200 - Projectile.timeLeft - i)) * 1.5f;
                    if (Projectile.timeLeft < 30)
                    {
                        if (i < 30 - Projectile.timeLeft)
                        {
                            normalDir = v_1.RotatedBy(1.6 / 170d * Math.PI * 170) * 1.5f - v_1.RotatedBy(1.6 / 170d * Math.PI * 169.99) * 1.5f;
                        }
                    }
                    normalDir.X *= Pdir;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(Prot);

                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    float h = 0;
                    if (Projectile.timeLeft > 130)
                    {
                        h = (Projectile.timeLeft - 200) / 70f + 1;
                    }
                    var w = MathHelper.Lerp(1f, 0.05f, factor + h);

                    Player player = Main.player[Projectile.owner];

                    bars.Add(new Vertex2D(player.Center + v4 + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new Vertex2D(player.Center + v4 + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                    if (!Main.gamePaused && Main.rand.NextBool(192))
                    {
                        int num90 = Dust.NewDust(player.Center + v4 + normalDir * -width * Main.rand.NextFloat(Main.rand.NextFloat(-0.5f, 0.8f), 0.8f) * player.direction, 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans>(), 0f, 0f, 0, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                        Main.dust[num90].noGravity = true;
                        Main.dust[num90].velocity = v4.RotatedBy(Math.PI / 2d * player.direction) / v4.Length();
                    }
                    if (!Main.gamePaused && Main.rand.NextBool(384))
                    {
                        int num90 = Dust.NewDust(player.Center + v4 + normalDir * -width * Main.rand.NextFloat(Main.rand.NextFloat(-0.5f, 0.8f), 0.8f) * player.direction, 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans2>(), 0f, 0f, 0, default(Color), Main.rand.NextFloat(1.0f, 1.3f));
                        Main.dust[num90].noGravity = true;
                        Main.dust[num90].velocity = v4.RotatedBy(Math.PI / 2d * player.direction) / v4.Length();
                    }
                    if (!Main.gamePaused && Projectile.timeLeft % 3 == 0 && i == 1 && !Collision.SolidCollision(player.Center + v4 + normalDir * -width * player.direction - Vector2.One * 5f, 10, 10))
                    {
                        int num90 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Center + v4 + normalDir * -width * player.direction, v4.RotatedBy(Math.PI / 2d * player.direction) / 4f, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpearfly>(), Projectile.damage / 3 * 2, Projectile.knockBack, player.whoAmI, player.GetCritChance(DamageClass.Melee) + 8, 0f);
                    }
                }
            }

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
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapFragrans").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/BloodGold" + (1 + Pdir).ToString()).Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/BloodGold" + (1 + Pdir).ToString()).Value;
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
