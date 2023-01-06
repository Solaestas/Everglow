using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    public class ToothSpear2 : ModProjectile
    {
        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 150f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tooth Spear");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血龙脊");

        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.  
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
            Projectile.extraUpdates = 6;
            Projectile.width = 80;
            Projectile.height = 80;
        }
        private bool fi = true;
        public override bool PreAI()
        {

            Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            float duration = player.itemAnimationMax * 7.2f; // Define the duration the projectile will exist in frames
            player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

            // Reset projectile time left if necessary
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = (int)duration;
            }

            // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

            float halfDuration = duration * 0.5f;
            float progress;

            if (Projectile.timeLeft < halfDuration + 2 && !max)
            {
                Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity * 2f, ModContent.ProjectileType<TuskSpear3>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                max = true;
            }
            // Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation. 
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }
            // Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
            //Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            Projectile.velocity *= 0.98f;
            // Apply proper rotation to the sprite.
            if (Projectile.spriteDirection == -1)
            {
                // If sprite is facing left, rotate 45 degrees
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                // If sprite is facing right, rotate 135 degrees
                Projectile.rotation += MathHelper.ToRadians(135f);
            }
            float dx = Projectile.timeLeft / (duration);
            if (Collision.SolidCollision(Projectile.Center, 1, 1))
            {
                Projectile.velocity *= 0.9f;
                player.velocity *= 0.9f;
                bool HasHi = false;
                if (Projectile.timeLeft % 30 == 1)
                {
                    for (int h = 0; h < 18; h++)
                    {
                        Vector2 v = Projectile.Center - Vector2.Normalize(Projectile.velocity) * 16 * h;
                        if (Collision.SolidCollision(v, 1, 1))
                        {
                            Collision.HitTiles(v, Projectile.velocity * 20, 16, 16);
                            HasHi = true;
                        }
                    }
                }
            }
            if (Projectile.timeLeft > 6 && !Collision.SolidCollision(Projectile.Center, 1, 1))
            {
                player.velocity = Projectile.velocity * 6;
            }
            if (Projectile.timeLeft < 6)
            {
                player.velocity *= 0.8f;
                Projectile.velocity *= 0.8f;
            }

            // Avoid spawning dusts on dedicated servers
            if (!Main.dedServ)
            {
            }
            if (fi)
            {
                sTp = player.Center;
                fi = false;
            }
            Common.MythContentPlayer.IMMUNE = 15;
            return false; // Don't execute vanilla AI.
        }
        private Effect ef;
        private Effect ef2;
        private bool max = false;
        private int pre = 0;
        private Vector2[] vpos = new Vector2[65];
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float a = Main.rand.NextFloat(0, 100f);
            Player player = Main.player[Projectile.owner];
            pre--;
            if (pre > -3)
            {
                for (int y = 0; y < 5; y++)
                {
                    Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity.RotatedBy(Math.PI * y / 2.5 + a) / Projectile.velocity.Length() * 15f, ModContent.ProjectileType<Projectiles.Weapon.TuskSpear3>(), Projectile.damage / 4, Projectile.knockBack, player.whoAmI);
                }
            }
            for (int j = 0; j < 5; j++)
            {
                for (int z = 0; z < 4; z++)
                {
                    Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
                    Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
                    int dus = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 183, v2.X, v2.Y, 100, default(Color), 1.8f);
                    Main.dust[dus].noGravity = true;
                    Main.dust[dus].velocity = v2;
                }
                Vector2 v1 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
                int r = Dust.NewDust(new Vector2(target.Center.X, target.Center.Y) - new Vector2(4, 4) + v1, 0, 0, 183, v3.X, v3.Y, 0, default(Color), 6f);
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3;
            }
            for (int i = 0; i < 10; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2, 7)).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(target.position, target.width, target.height, 183, v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0.8f, 2.1f));
            }
            for (int y = 0; y < 48; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.Blood, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4f));
                Main.dust[num90].noGravity = false;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.4f, 1.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
            }
        }

        private Vector2 sTp;
        float MaxR = 0.1f;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TrailTuskRush").Value;

            int Wid = 120;
            if (Projectile.timeLeft < 110)
            {
                MaxR = (110 - Projectile.timeLeft) / 120f + 0.1f;
            }

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
                int width = Wid;

                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                if (vpos[i] != Vector2.Zero)
                {
                    var normalDir = Projectile.velocity;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    float h = 0;
                    if (Projectile.timeLeft > player.itemAnimationMax * 7.2f - 60)
                    {
                        h = (Projectile.timeLeft - player.itemAnimationMax * 7.2f) / 60f + 1;
                    }
                    var w = MathHelper.Lerp(1f, 0.05f, factor + h);
                    Vector2 deltaPos = Projectile.position - vpos[1];
                    double dw = i;
                    if (dw < 40)
                    {
                        width = (int)(Wid * Math.Sqrt(dw / 40d));
                    }
                    bars.Add(new VertexBase.CustomVertexInfo(vpos[i] + normalDir * width + new Vector2(47) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    dw += 0.5;
                    if (dw < 40)
                    {
                        width = (int)(Wid * Math.Sqrt(dw / 40d));
                    }
                    bars.Add(new VertexBase.CustomVertexInfo(vpos[i] + normalDir * -width + new Vector2(47) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                else
                {
                    var normalDir = Projectile.velocity;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    float h = 0;
                    if (Projectile.timeLeft > player.itemAnimationMax * 7.2f - 60)
                    {
                        h = (Projectile.timeLeft - player.itemAnimationMax * 7.2f) / 60f + 1;
                    }
                    var w = MathHelper.Lerp(1f, 0.05f, factor + h);

                    double dw = i;
                    if (dw < 40)
                    {
                        width = (int)(int)(Wid * Math.Sqrt(dw / 40d));
                    }
                    bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + new Vector2(47), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    dw += 0.5;
                    if (dw < 40)
                    {
                        width = (int)(Wid * Math.Sqrt(dw / 40d));
                    }
                    bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + new Vector2(47), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }

            List<VertexBase.CustomVertexInfo> triangleList = new List<VertexBase.CustomVertexInfo>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                Vector2 vo = (bars[0].Position - bars[1].Position) * 3f;
                var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + vo, Color.White, new Vector3(0, 0.5f, 1));
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
                ef.Parameters["uTime"].SetValue(Adding * 0.3f);
                ef.Parameters["maxr"].SetValue(MaxR);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapRedBeta").Value;
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



            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<VertexBase.CustomVertexInfo> barz = new List<VertexBase.CustomVertexInfo>();
            ef2 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TrailTuskRush").Value;

            int Wit = 120;

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
                int width = Wit;

                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                if (vpos[i] != Vector2.Zero)
                {
                    var normalDir = Projectile.velocity;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    float h = 0;
                    if (Projectile.timeLeft > player.itemAnimationMax * 7.2f - 60)
                    {
                        h = (Projectile.timeLeft - player.itemAnimationMax * 7.2f) / 60f + 1;
                    }
                    var w = MathHelper.Lerp(1f, 0.05f, factor + h);
                    Vector2 deltaPos = Projectile.position - vpos[1];
                    double dw = i;
                    if (dw < 40)
                    {
                        width = (int)(Wit * Math.Sqrt(dw / 40d));
                    }
                    barz.Add(new VertexBase.CustomVertexInfo(vpos[i] + normalDir * width + new Vector2(47) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    dw += 0.5;
                    if (dw < 40)
                    {
                        width = (int)(Wit * Math.Sqrt(dw / 40d));
                    }
                    barz.Add(new VertexBase.CustomVertexInfo(vpos[i] + normalDir * -width + new Vector2(47) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                else
                {
                    var normalDir = Projectile.velocity;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    float h = 0;
                    if (Projectile.timeLeft > player.itemAnimationMax * 7.2f - 60)
                    {
                        h = (Projectile.timeLeft - player.itemAnimationMax * 7.2f) / 60f + 1;
                    }
                    var w = MathHelper.Lerp(1f, 0.05f, factor + h);
                    double dw = i;
                    if (dw < 40)
                    {
                        width = (int)(int)(Wit * Math.Sqrt(dw / 40d));
                    }
                    barz.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + new Vector2(47), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    dw += 0.5;
                    if (dw < 40)
                    {
                        width = (int)(Wit * Math.Sqrt(dw / 40d));
                    }
                    barz.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + new Vector2(47), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }

            List<VertexBase.CustomVertexInfo> triangleLisd = new List<VertexBase.CustomVertexInfo>();

            if (barz.Count > 2)
            {

                // 按照顺序连接三角形
                triangleLisd.Add(barz[0]);
                Vector2 vo = (barz[0].Position - barz[1].Position) * 3f;
                var vertex = new VertexBase.CustomVertexInfo((barz[0].Position + barz[1].Position) * 0.5f + vo, Color.White, new Vector3(0, 0.5f, 1));
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
                // 干掉注释掉就可以只显示三角形栅格
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                // 把变换和所需信息丢给shader
                ef2.Parameters["uTransform"].SetValue(model * projection);
                ef2.Parameters["uTime"].SetValue(Adding * 1.5f);
                ef2.Parameters["maxr"].SetValue(MaxR);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapRedBeta").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ClubShaderFlame2").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTrace").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

                ef2.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleLisd.ToArray(), 0, triangleLisd.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            Adding -= 0.04f;
            if (Adding > 3141592.65f)
            {
                Adding = 0;
            }
        }
        float Adding = 0;
    }
}
