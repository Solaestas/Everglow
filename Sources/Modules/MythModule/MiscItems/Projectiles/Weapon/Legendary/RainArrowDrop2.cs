using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    class RainArrowDrop : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 90;
            Projectile.alpha = 255;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }
        int Ran = -1;
        int Tokill = -1;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
        }
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
            if (Ran == -1)
            {
                Ran = Main.rand.Next(9);
            }
            int addi = 90 - Projectile.timeLeft;
            if (Projectile.timeLeft < 40)
            {
                addi = Projectile.timeLeft;
                Projectile.scale = Projectile.timeLeft / 40f;
            }
            if (Tokill <= 44 && Tokill > 0)
            {
                Projectile.position = Projectile.oldPosition;
                Projectile.velocity = Projectile.oldVelocity;
                if (Tokill < 40 && Tokill < Projectile.timeLeft)
                {
                    addi = Tokill;
                    Projectile.scale = Tokill / 40f;
                }
            }
            if (Tokill >= 0 && Tokill <= 2)
            {
                Projectile.Kill();
            }
            int Alp = 255 - addi * 5;
            if (Alp < 80)
            {
                Alp = 80;
            }
            Projectile.alpha = Alp;
            if (Projectile.ai[0] >= 1)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * (255 - Projectile.alpha) / 100f);
                int h = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default(Color), Main.rand.NextFloat(0.6f, 1.3f) * (255 - Projectile.alpha) / 100f);
                Main.dust[h].noGravity = true;
            }
            if (Projectile.timeLeft % 9 == Ran)
            {
                if (Projectile.ai[0] >= 1)
                {
                    Vector2 v = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
                    int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.RainArrow>(), Projectile.damage / 2, Projectile.knockBack * 2 / 3f, Projectile.owner, Projectile.ai[0] - 1, 0f);
                    Main.projectile[h].timeLeft = Projectile.timeLeft;
                }
            }
            if (Projectile.ai[0] < 1)
            {
                Projectile.velocity.Y += 0.35f;
                Projectile.velocity *= 0.99f;
            }
            else
            {
                Projectile.velocity.Y += 0.15f;
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 30 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active && !Nul)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[1]);
                    Player player = Main.player[Projectile.owner];
                    player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[1] / 100f) * 1.0f));
                    Projectile.friendly = false;
                    Projectile.damage = 0;
                    Projectile.tileCollide = false;
                    Projectile.ignoreWater = true;
                    Projectile.aiStyle = -1;
                    Nul = true;
                    Projectile.velocity = Projectile.oldVelocity;
                    Tokill = 45;
                    for (int i = 0; i < 2; i++)
                    {
                        int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
                    }
                    for (int ja = 0; ja < 4; ja++)
                    {
                        int num20 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
                    }
                }
            }
            Tokill--;
        }
        private bool Nul = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Projectile.oldVelocity;
            Tokill = 45;
            for (int i = 0; i < 2; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
            }
            for (int j = 0; j < 4; j++)
            {
                int num20 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
            }
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Nul = true;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Nul = true;
            Projectile.velocity = Projectile.oldVelocity;
            Tokill = 45;
            for (int i = 0; i < 2; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
            }
            for (int j = 0; j < 4; j++)
            {
                int num20 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
            }
            for (int j = 0; j < 4; j++)
            {
                int num20 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
            }
        }
        private Effect ef;
        private Effect ef2;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> barz = new List<Vertex2D>();
            ef2 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TrailB").Value;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                int width = 18;
                if (Projectile.timeLeft > 30)
                {
                    width = 18;
                }
                else
                {
                    width = Projectile.timeLeft * 3 / 5;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                if (normalDir.Length() < 0.2f)
                {
                    normalDir = Projectile.velocity / Projectile.velocity.Length();
                }
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                width = (int)(width * (1 - factor));
                barz.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(18), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                barz.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(18), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }
            List<Vertex2D> triangleLisd = new List<Vertex2D>();
            if (barz.Count > 2)
            {
                triangleLisd.Add(barz[0]);
                var vertex = new Vertex2D((barz[0].position + barz[1].position) * 0.5f + Projectile.velocity * 0.04f, Color.White, new Vector3(0, 0.5f, 1));
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
                ef2.Parameters["uTime"].SetValue(-(float)Main.time * 0.01f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlueD").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/GoldLine").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/WaterLine").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef2.CurrentTechnique.Passes[0].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleLisd.ToArray(), 0, triangleLisd.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail2").Value;
            int g = (int)(Projectile.oldPos.Length * 0.75);
            for (int i = 1; i < g; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                int width = 18;
                if (Projectile.timeLeft > 30)
                {
                    width = 18;
                }
                else
                {
                    width = Projectile.timeLeft * 3 / 5;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                if (normalDir.Length() < 0.2f)
                {
                    normalDir = Projectile.velocity / Projectile.velocity.Length();
                }
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)g;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                width = (int)(width * (1 - factor));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(18), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(18), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }
            List<Vertex2D> triangleList = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Projectile.velocity * 0.04f, Color.White, new Vector3(0, 0.5f, 1));
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
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.015f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlue").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/DarkGrey").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/WaterLine").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef.CurrentTechnique.Passes[0].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
