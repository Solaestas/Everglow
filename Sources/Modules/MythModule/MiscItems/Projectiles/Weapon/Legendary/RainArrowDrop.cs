//using MythMod.Common.Players;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    class RainArrowDrop2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 9000;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
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
        bool Release = true;
        int PdamF = 0;
        Vector2 oldPo = Vector2.Zero;
        public override void AI()
        {
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].Center;
            if (Main.mouseRight && Release)
            {
                if (Projectile.timeLeft > 8000)
                {
                    PdamF = Projectile.damage;
                }
                if (Energy <= 120)
                {
                    Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI * 0.25);
                    Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * 14f;
                    oldPo = Projectile.Center;
                }
                Projectile.Center = oldPo;
                Projectile.velocity *= 0;
                if (Energy <= 120)
                {
                    Projectile.timeLeft = 5 + Energy;
                }
                Energy++;
                float AddDam = 1 + Energy / 400f;
                Projectile.damage = (int)(PdamF * (AddDam * AddDam));
                if (Energy == 85)
                {
                    for (int h = 0; h < 25; h++)
                    {
                        Projectile.NewProjectile(null, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RainEffect>(), 0, 0, Projectile.owner);
                    }
                }
            }
            if (!Main.mouseRight && Release)
            {
                if (Energy <= 120)
                {
                    Projectile.velocity = Vector2.Normalize(v0) * 34f;
                    Projectile.friendly = true;
                    Projectile.tileCollide = true;
                }
                Projectile.ai[0] = 57;
                Release = false;
            }
            if (!Main.mouseRight && Release)
            {
                if (Energy < 120)
                {
                    Projectile.velocity.Y += 0.35f;
                    Projectile.velocity *= 0.99f;
                }
            }
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
            /*for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 30 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active && !Nul)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[1]);
                    Player player = Main.player[Projectile.owner];
                    player.addDPS( (int)(Projectile.damage * (1 + Projectile.ai[1] / 100f) * 1.0f));
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
            }*/
            if (Projectile.timeLeft < 60)
            {
                yd = Projectile.timeLeft / 60f;
            }
            if (Projectile.timeLeft > 340)
            {
                yd = (400 - Projectile.timeLeft) / 60f;
            }
            CirR0 += 0.001f;
            CirPro0 += 0.3f;
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
            return false;
        }
        bool Bomb = false;
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
        int Energy = 0;
        int TrueL = 1;
        int TrueLU = 1;
        int TrueLD = 1;
        float CirR0 = 0;
        float CirPro0 = 0;
        float yd = 1;
        Vector2[] Vlaser = new Vector2[501];
        Vector2[] VlaserU = new Vector2[501];
        Vector2[] VlaserD = new Vector2[501];
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vT0 = Main.MouseWorld - player.Center;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(vT0.Y, vT0.X) - Math.PI / 2d));
            Texture2D TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
            float a0 = Energy % 60f;
            float a1 = (60 - a0) / 60f;
            float a2 = a1 * 1.5f;
            float a3 = a2 * a2;
            float b0 = Math.Clamp(Energy, 0, 60);
            float b1 = b0 / 60f;
            float b2 = b1;
            float b3 = b2 * b2;
            if (Release)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Legendary/BlueRainBow").Value;
                Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
                SpriteEffects se = SpriteEffects.None;
                if (Projectile.Center.X < Main.player[Projectile.owner].Center.X)
                {
                    se = SpriteEffects.FlipVertically;
                    Main.player[Projectile.owner].direction = -1;
                }
                else
                {
                    Main.player[Projectile.owner].direction = 1;
                }
                Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].Center;
                Vector2 vPro = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * (14f - 12f * b3);
                Main.spriteBatch.Draw(TexMain, vPro - Main.screenPosition, null, drawColor, Projectile.rotation, new Vector2(18, 18), 1f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25), new Vector2(25, 54), 1f, se, 0);
                Texture2D texture2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/Lightball").Value;
                Color color2 = new Color(1 - a3 / 2.25f, 1 - a3 / 2.25f, 1 - a3 / 2.25f, 0);
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, color2, 0, new Vector2(250, 250), a3 / 4f, SpriteEffects.None, 0);
            }

            //水线拖尾
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            //水线拖尾2
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            if (Energy > 120)
            {

                //喷流
                float AddDam = 1 + Energy / 400f;
                Projectile.damage = (int)(PdamF * (AddDam * AddDam) * 2.5f);
                if (Projectile.timeLeft > 65)
                {
                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
                    Projectile.timeLeft = 63;
                    Projectile.extraUpdates = 3;
					//MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
					//mplayer.Shake = 3;
					ScreenShaker mplayer = player.GetModPlayer<ScreenShaker>();
					mplayer.FlyCamPosition = new Vector2(0, 56).RotatedByRandom(6.283);
					float Str = 1;
                    if ((player.Center - Projectile.Center).Length() > 100)
                    {
                        Str = 100 / (player.Center - Projectile.Center).Length();
                    }
                    mplayer.DirFlyCamPosStrength = Str;
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                List<Vertex2D> bars3 = new List<Vertex2D>();
                List<Vertex2D> bars4 = new List<Vertex2D>();
                List<Vertex2D> bars5 = new List<Vertex2D>();

                float step = 4;
                int Count = 0;
                int CountU = 0;
                int CountD = 0;
                bool Clo = false;
                bool CloU = false;
                bool CloD = false;
                for (int m = 0; m < 500; ++m)
                {
                    float width = 1f;
                    if (m <= 25)
                    {
                        width = (float)Math.Sqrt(m) / 5f;
                    }
                    Vector2 v0 = new Vector2(0, step).RotatedBy(Projectile.rotation - Math.PI * 0.75) * m;
                    Vector2 v1 = v0 + new Vector2(step, 0).RotatedBy(Projectile.rotation - Math.PI * 0.75) * 25 * width * (float)(Math.Sin(m / 10d + Main.time / 60d));//上螺旋线
                    Vector2 v2 = v0 + new Vector2(step, 0).RotatedBy(Projectile.rotation - Math.PI * 0.75) * 17 * width * (float)(Math.Sin(m / 10d + Math.PI / 2d + Main.time / 60d));//下螺旋线
                    if (Collision.SolidCollision(Projectile.Center + v0, 1, 1) && !Clo)
                    {
                        Clo = true;
                    }
                    if (!Collision.SolidCollision(Projectile.Center + v0, 1, 1) && !Clo)
                    {
                        Vlaser[m] = Projectile.Center + v0;
                        ++Count;
                    }
                    if (Collision.SolidCollision(Projectile.Center + v1, 1, 1) && !CloU)
                    {
                        CloU = true;
                    }
                    if (!Collision.SolidCollision(Projectile.Center + v1, 1, 1) && !CloU)
                    {
                        VlaserU[m] = Projectile.Center + v1;
                        ++CountU;
                    }
                    if (Collision.SolidCollision(Projectile.Center + v2, 1, 1) && !CloD)
                    {
                        CloD = true;
                    }
                    if (!Collision.SolidCollision(Projectile.Center + v2, 1, 1) && !CloD)
                    {
                        VlaserD[m] = Projectile.Center + v2;
                        ++CountD;
                    }
                    if (Clo && CloD && CloU)
                    {
                        break;
                    }
                }
                for (int i = 1; i < Count; ++i)
                {
                    if (Vlaser[i] == Vector2.Zero)
                        break;
                    var normalDir = Vlaser[i - 1] - Vlaser[i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = (float)Math.Sqrt((i + 1)) / (float)TrueL / 9f + Projectile.timeLeft / 60f;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    float width = 60;
                    if (i <= 25)
                    {
                        width = 60 * (float)Math.Sqrt(i) / 5f;
                    }

                    width *= yd;
                    Lighting.AddLight(Vlaser[i], 0, (float)(255 - Projectile.alpha) * 0.2f / 50f * yd, (float)(255 - Projectile.alpha) * 1.2f / 50f * yd);
                    if (Count - i < 5)
                    {
                        int C0 = Math.Clamp((int)(255 * (Count - i - 1) / 5f), 0, 255);
                        bars3.Add(new Vertex2D(Vlaser[i] + normalDir * width - Main.screenPosition, new Color(C0, C0, C0, 0), new Vector3(factor % 1f, 1, w)));
                        bars3.Add(new Vertex2D(Vlaser[i] + normalDir * -width - Main.screenPosition, new Color(C0, C0, C0, 0), new Vector3(factor % 1f, 0, w)));
                    }
                    else
                    {
                        bars3.Add(new Vertex2D(Vlaser[i] + normalDir * width - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor % 1f, 1, w)));
                        bars3.Add(new Vertex2D(Vlaser[i] + normalDir * -width - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor % 1f, 0, w)));
                    }
                    if (!Main.gamePaused)
                    {
                        if (i % 15 == 0)
                        {
                            for (int j = 0; j < 200; j++)
                            {
                                if ((Main.npc[j].Center - Vlaser[i]).Length() < 90 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active && !HasHit[j])
                                {
                                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[1]);
                                    Player player2 = Main.player[Projectile.owner];
                                    player2.dpsDamage += (int)(Projectile.damage * (1 + Projectile.ai[1] / 100f) * 1f);
                                    HasHit[j] = true;
                                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Main.npc[j].Center, Vector2.Zero, ModContent.ProjectileType<RainArrowDropHit2>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 0f, 0f);
                                }
                            }
                        }
                    }
                }
                for (int i = 1; i < CountU; ++i)//上旋线
                {
                    if (VlaserU[i] == Vector2.Zero)
                        break;
                    var normalDir = VlaserU[i - 1] - VlaserU[i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = (float)Math.Sqrt((i + 1)) / (float)TrueL / 9f + Projectile.timeLeft / 60f;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    float width = 20 * (float)(Math.Cos(i / 10d + Main.time / 60d) + 1);
                    if (i <= 25)
                    {
                        width = 20 * (float)Math.Sqrt(i) / 5f * (float)(Math.Cos(i / 10d + Main.time / 60d) + 1);
                    }

                    width *= yd;
                    Lighting.AddLight(VlaserU[i], 0, (float)(255 - Projectile.alpha) * 0.2f / 50f * yd, (float)(255 - Projectile.alpha) * 1.2f / 50f * yd);
                    if (CountU - i < 5)
                    {
                        int C0 = Math.Clamp((int)(255 * (CountU - i - 1) / 5f), 0, 255);
                        bars4.Add(new Vertex2D(VlaserU[i] + normalDir * width - Main.screenPosition, new Color(C0, C0, C0, 0), new Vector3(factor % 1f, 1, w)));
                        bars4.Add(new Vertex2D(VlaserU[i] + normalDir * -width - Main.screenPosition, new Color(C0, C0, C0, 0), new Vector3(factor % 1f, 0, w)));
                    }
                    else
                    {
                        bars4.Add(new Vertex2D(VlaserU[i] + normalDir * width - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor % 1f, 1, w)));
                        bars4.Add(new Vertex2D(VlaserU[i] + normalDir * -width - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor % 1f, 0, w)));
                    }
                }
                for (int i = 1; i < CountD; ++i)//下旋线
                {
                    //Main.NewText(VlaserD[i].To
                    //(),0,0,255);
                    if (VlaserD[i] == Vector2.Zero)
                        break;
                    var normalDir = VlaserD[i - 1] - VlaserD[i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = (float)Math.Sqrt((i + 1)) / (float)TrueL / 9f + Projectile.timeLeft / 60f;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    float width = 15 * (float)(Math.Cos(i / 10d + Main.time / 60d + Math.PI / 2d + Main.time / 60d) + 1);
                    if (i <= 25)
                    {
                        width = 15 * (float)Math.Sqrt(i) / 5f * (float)(Math.Cos(i / 10d + Math.PI / 2d + Main.time / 60d) + 1);
                    }

                    width *= yd;
                    Lighting.AddLight(VlaserD[i], 0, (float)(255 - Projectile.alpha) * 0.2f / 50f * yd, (float)(255 - Projectile.alpha) * 1.2f / 50f * yd);
                    if (CountD - i < 5)
                    {
                        int C0 = Math.Clamp((int)(255 * (CountD - i - 1) / 5f), 0, 255);
                        bars5.Add(new Vertex2D(VlaserD[i] + normalDir * width - Main.screenPosition, new Color(C0, C0, C0, 0), new Vector3(factor % 1f, 1, w)));
                        bars5.Add(new Vertex2D(VlaserD[i] + normalDir * -width - Main.screenPosition, new Color(C0, C0, C0, 0), new Vector3(factor % 1f, 0, w)));
                    }
                    else
                    {
                        bars5.Add(new Vertex2D(VlaserD[i] + normalDir * width - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor % 1f, 1, w)));
                        bars5.Add(new Vertex2D(VlaserD[i] + normalDir * -width - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor % 1f, 0, w)));
                    }
                }
                List<Vertex2D> Vx = new List<Vertex2D>();
                List<Vertex2D> VxU = new List<Vertex2D>();
                List<Vertex2D> VxD = new List<Vertex2D>();
                if (bars3.Count > 2)
                {
                    Vx.Add(bars3[0]);
                    var vertex = new Vertex2D((bars3[0].position + bars3[1].position) * 0.5f + new Vector2(-5, 0).RotatedBy(Projectile.rotation), new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
                    Vx.Add(bars3[1]);
                    Vx.Add(vertex);
                    for (int i = 0; i < bars3.Count - 2; i += 2)
                    {
                        Vx.Add(bars3[i]);
                        Vx.Add(bars3[i + 2]);
                        Vx.Add(bars3[i + 1]);

                        Vx.Add(bars3[i + 1]);
                        Vx.Add(bars3[i + 2]);
                        Vx.Add(bars3[i + 3]);
                    }
                }
                Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/RainArrowRay").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                if (bars4.Count > 2)
                {
                    VxU.Add(bars4[0]);
                    var vertex = new Vertex2D((bars4[0].position + bars4[1].position) * 0.5f + new Vector2(-5, 0).RotatedBy(Projectile.rotation), new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
                    VxU.Add(bars4[1]);
                    VxU.Add(vertex);
                    for (int i = 0; i < bars4.Count - 2; i += 2)
                    {
                        VxU.Add(bars4[i]);
                        VxU.Add(bars4[i + 2]);
                        VxU.Add(bars4[i + 1]);

                        VxU.Add(bars4[i + 1]);
                        VxU.Add(bars4[i + 2]);
                        VxU.Add(bars4[i + 3]);
                    }
                }
                t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/RainArrowRay2").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxU.ToArray(), 0, VxU.Count / 3);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                if (bars5.Count > 2)
                {
                    VxD.Add(bars5[0]);
                    var vertex = new Vertex2D((bars5[0].position + bars5[1].position) * 0.5f + new Vector2(-5, 0).RotatedBy(Projectile.rotation), new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
                    VxD.Add(bars5[1]);
                    VxD.Add(vertex);
                    for (int i = 0; i < bars5.Count - 2; i += 2)
                    {
                        VxD.Add(bars5[i]);
                        VxD.Add(bars5[i + 2]);
                        VxD.Add(bars5[i + 1]);

                        VxD.Add(bars5[i + 1]);
                        VxD.Add(bars5[i + 2]);
                        VxD.Add(bars5[i + 3]);
                    }
                }
                t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/RainArrowRay2").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxD.ToArray(), 0, VxD.Count / 3);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                List<Vertex2D> Vx2 = new List<Vertex2D>();

                Vector2 vf = Vlaser[Math.Clamp(Count - 1, 0, 501)] - Main.screenPosition;
                float ACircleR = 150 * yd;
                for (int h = 0; h < 100; h++)
                {
                    Color color3 = new Color(254, 254, 254, 0);
                    Vector2 v0 = new Vector2(0, ACircleR).RotatedBy(h / 50d * Math.PI + CirR0);
                    Vector2 v1 = new Vector2(0, ACircleR).RotatedBy((h + 1) / 50d * Math.PI + CirR0);
                    if (h % 20 >= 10)
                    {
                        Vx2.Add(new Vertex2D(vf + v0, color3, new Vector3(((0.999f + CirPro0) / 25f) % 1f, 0, 0)));
                        Vx2.Add(new Vertex2D(vf + v1, color3, new Vector3(((CirPro0) / 25f) % 1f, 0, 0)));
                        Vx2.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + CirPro0) / 25f) % 1f, 1, 0)));
                    }
                    else
                    {
                        Vx2.Add(new Vertex2D(vf + v0, color3, new Vector3(((CirPro0) / 25f) % 1f, 0, 0)));
                        Vx2.Add(new Vertex2D(vf + v1, color3, new Vector3(((0.999f + CirPro0) / 25f) % 1f, 0, 0)));
                        Vx2.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + CirPro0) / 25f) % 1f, 1, 0)));
                    }
                    if (!Bomb)
                    {
                        Bomb = true;
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Vlaser[Math.Clamp(Count - 1, 0, 507)], Vector2.Zero, ModContent.ProjectileType<RainArrowDropHit>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 0f, 0f);
                    }
                }
                Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightCrackBlue").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t1;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);
            }
        }
        bool[] HasHit = new bool[200];
    }
}
