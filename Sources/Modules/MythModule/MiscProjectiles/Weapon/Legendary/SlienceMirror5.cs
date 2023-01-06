using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    class SlienceMirror5 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 360;
            Projectile.height = 360;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 340;
            Projectile.extraUpdates = 30;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
        }
        private Vector2 v_1 = new Vector2(15, -56);
        private Vector2 v2 = Vector2.Zero;
        private bool Dir = false;
        private int Pdir = -1;
        private float Prot = 0;
        private float Siort = 1f;
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
                Siort = 0.4f;
                Vector2 vc = -(new Vector2(Main.mouseX, Main.mouseY) - player.Center + Main.screenPosition);
                Prot = (float)Math.Atan2(vc.Y, vc.X);
                if (Pdir == 1)
                {
                    Prot += (float)(Math.PI);
                }
                Dir = true;
            }
            Vector2 v0 = v_1.RotatedBy(1.1 / 170d * Math.PI * (340 - Projectile.timeLeft));
            if (Projectile.timeLeft < 30)
            {
                Projectile.Kill();
                v0 = v_1.RotatedBy(1.6 * Math.PI);
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
            if (Projectile.timeLeft == 120)
            {
                if (Main.mouseLeft)
                {

                }
                Projectile.Kill();
            }
            if (Projectile.timeLeft == 306)
            {
                Vector2 vnp0 = player.Center + new Vector2(0, Main.rand.NextFloat(140f, 245f)).RotatedBy(0);
                Vector2 vnp1 = Main.MouseWorld - vnp0;
                vnp1 = vnp1 / vnp1.Length() * 10f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), vnp0 - vnp1 * 70f, vnp1 * 4.6f, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SilenCrack5>(), Projectile.damage, Projectile.knockBack, player.whoAmI, player.GetCritChance(DamageClass.Melee) + 25, 0f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
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
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail2").Value;
            double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            double o2 = Math.Atan2(Projectile.oldVelocity.Y, Projectile.oldVelocity.X);
            double omega = Math.Abs(o2 - o1) % MathHelper.TwoPi;
            int width = 84;
            for (int i = 1; i < 60; ++i)
            {

                Vector2 v3 = (v_1 * 1.0f).RotatedBy(1.1 / 170d * Math.PI * (340 - Projectile.timeLeft - i)) * 7.6f;
                if (Projectile.timeLeft < 30)
                {
                    v3 = (v_1 * 1.0f).RotatedBy(1.6 * Math.PI) * 7.6f;
                }
                v3.X *= Pdir;
                Vector2 v4 = new Vector2(v3.X, v3.Y * Siort).RotatedBy(Prot);
                var normalDir = (v_1 * 1.0f).RotatedBy(1.1 / 170d * Math.PI * (340 - Projectile.timeLeft - i + 1)) * 7.6f - (v_1 * 1.0f).RotatedBy(1.1 / 170d * Math.PI * (340 - Projectile.timeLeft - i)) * 7.6f;
                if (Projectile.timeLeft < 30)
                {
                    if (i < 30 - Projectile.timeLeft)
                    {
                        normalDir = (v_1 * 1.0f).RotatedBy(1.1 / 170d * Math.PI * 170) * 7.6f - (v_1 * 1.0f).RotatedBy(1.1 / 170d * Math.PI * 169.99) * 7.6f;
                    }
                }
                normalDir.X *= Pdir;
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(Prot);

                var factor = i / 60f;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                if (Projectile.timeLeft < 120)
                {
                    width = 0;
                }
                bars.Add(new Vertex2D(player.Center + v4 + normalDir * width * 2, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(player.Center + v4 + normalDir * -width * 2, color, new Vector3((float)Math.Sqrt(factor), 0, w)));

            }
            Vector2 v32 = (v_1 * 1.0f).RotatedBy(1.1 / 170d * Math.PI * (340 - Projectile.timeLeft)) * 7.6f;
            v32.X *= Pdir;
            Vector2 v42 = new Vector2(v32.X, v32.Y * Siort).RotatedBy(Prot);
            List<Vertex2D> triangleList = new List<Vertex2D>();
            if (!Main.gamePaused)
            {
                Projectile.NewProjectile(null, player.Center + v42 * 0.75f, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.MeleeHit>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0, 0f);
            }
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
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapRed").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ForgeWave").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Mirror" + (1 + Pdir).ToString()).Value;
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
