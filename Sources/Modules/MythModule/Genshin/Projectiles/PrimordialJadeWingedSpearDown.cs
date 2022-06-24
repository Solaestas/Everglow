using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.Genshin.Projectiles
{
    public class PrimordialJadeWingedSpearDown : ModProjectile
    {
        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 150f;

        public Vector2 SafelyNormalize(Vector2 orig)
        {
            if (Projectile.velocity.Length() > 0)
            {
                return SafelyNormalize(orig);
            }
            else
            {
                return Vector2.One;
            }
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear);
            Projectile.extraUpdates = 6;
            Projectile.width = 80;
            Projectile.height = 80;
        }
        private bool Crash = false;
        private int kad = 0;
        public override void Kill(int timeLeft)
        {
            for (int f = 0; f < 60; f++)
            {
                OldplCen[f] = Vector2.Zero;
                FirstVel = Vector2.Zero;
                wid = new float[60];
                TrueL = 0;
                statrP = new float[4];
            }
        }
        public void GreenFlame()
        {
            if (Main.rand.NextBool(5))
            {
                Vector2 v1 = new Vector2(Main.rand.NextFloat(0.8f, 10f), 0).RotatedByRandom(6.28);
                Dust.NewDust(OldplCen[0] - new Vector2(4), 1, 1, ModContent.DustType<Dusts.XiaoDust>(), (Projectile.velocity * 0.35f + v1).X, (Projectile.velocity * 0.35f + v1).Y, 0, default(Color), Main.rand.NextFloat(0.85f, 1.25f));
            }
            if (Main.rand.NextBool(3))
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(0.8f, 10f), 0).RotatedByRandom(6.28);
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(null, OldplCen[0], Projectile.velocity * 0.35f + v0, ModContent.GoreType<Gores.XiaoDash0>(), Main.rand.NextFloat(0.65f, 1.25f));
                }
                else
                {
                    Gore.NewGore(null, OldplCen[0], Projectile.velocity * 0.35f + v0, ModContent.GoreType<Gores.XiaoDash1>(), Main.rand.NextFloat(0.65f, 1.25f));
                }
            }
        }
        public void ShakeEffect()
        {
            /*MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Shake = 5;
            mplayer.ShakeStrength = 2f;
            SoundEngine.PlaySound(new SoundStyle("MythMod/Sounds/Xiao"), Projectile.Center);*/
        }
        public override bool PreAI()
        {
            if (Projectile.timeLeft == 2)
            {
                for (int f = 0; f < 60; f++)
                {
                    OldplCen[f] = Vector2.Zero;
                    FirstVel = Vector2.Zero;
                    wid = new float[60];
                    TrueL = 0;
                    statrP = new float[4];
                }
            }

            Player player = Main.player[Projectile.owner];
            float duration = player.itemAnimationMax * 7.2f;
            OldplCen[0] = Projectile.Center - SafelyNormalize(Projectile.velocity) * 15f;//记录位置
            GreenFlame();
            kad++;
            if (kad % 10 == 1 && Projectile.timeLeft > 30)
            {
                Vector2 v = SafelyNormalize(Projectile.velocity);
                int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.XiaoBlackWave>(), 0, 0, player.whoAmI, Math.Clamp(Projectile.velocity.Length() / 8f, 0f, 4f), 0);
                Main.projectile[h].rotation = (float)(Math.Atan2(v.Y, v.X) + Math.PI / 2d);
            }
            for (int f = OldplCen.Length - 1; f > 0; f--)
            {
                OldplCen[f] = OldplCen[f - 1];
            }
            wid[0] = Math.Clamp(Projectile.velocity.Length() / 6f, 0, 60);//宽度
            for (int f = wid.Length - 1; f > 0; f--)
            {
                wid[f] = wid[f - 1];
            }
            if (player.direction == -Math.Sign(Projectile.velocity.X))
            {
                player.direction *= -1;
            }
            player.heldProj = Projectile.whoAmI;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = (int)duration;
            }
            if (Projectile.timeLeft <= 6)
            {
                if (Collision.SolidCollision(player.Top, 1, 1))
                {
                    player.position.Y -= 64;
                }
            }
            float halfDuration = duration * 0.5f;

            if (Projectile.timeLeft < halfDuration + 2 && !max)
            {
                max = true;
            }
            Projectile.velocity *= 0.995f;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                Projectile.rotation += MathHelper.ToRadians(135f);
            }
            if (Collision.SolidCollision(Projectile.Center + Projectile.velocity, 1, 1))
            {
                Projectile.velocity *= 0.5f;
                Projectile.timeLeft -= 1;
                Projectile.extraUpdates = 0;
                for (int f = wid.Length - 1; f >= 0; f--)
                {
                    wid[f] *= 0.9f;
                }
                if (Projectile.timeLeft % 30 == 1)
                {
                    for (int h = 0; h < 18; h++)
                    {
                        Vector2 v = Projectile.Center - SafelyNormalize(Projectile.velocity) * 16 * h;
                        if (Collision.SolidCollision(v, 1, 1))
                        {
                            Collision.HitTiles(v, Projectile.velocity * 20, 16, 16);
                        }
                    }
                }
                if (!Crash)
                {
                    for (int f = 0; f < 25; f++)
                    {
                        Vector2 v0 = new Vector2(Main.rand.NextFloat(0.8f, 3f), 0).RotatedByRandom(6.28);
                        float Dx = Main.rand.NextFloat(-250f, 250f);
                        Vector2 Pos = OldplCen[0] + new Vector2(Dx, 40 + Main.rand.NextFloat(-10f, 10f));
                        for (int i = 0; i < 5; i++)
                        {
                            if (Collision.SolidCollision(Pos, 1, 1))
                            {
                                Pos.Y -= 16;
                            }
                        }
                        Vector2 Dy = v0 + new Vector2(0, Main.rand.NextFloat(Math.Abs(Dx) - 278, 0) / 12f);
                        if (Main.rand.NextBool(2))
                        {
                            Gore.NewGore(null, Pos, Dy, ModContent.GoreType<Gores.XiaoDash0>(), Main.rand.NextFloat(1f, 1.5f));
                        }
                        else
                        {
                            Gore.NewGore(null, Pos, Dy, ModContent.GoreType<Gores.XiaoDash1>(), Main.rand.NextFloat(1f, 1.5f));
                        }
                    }
                    for (int f = 0; f < 8; f++)
                    {
                        Vector2 Pos = Projectile.Center + new Vector2((f - 3.5f) * 80, -300);
                        for (int i = 0; i < 75; i++)
                        {
                            if (!Collision.SolidCollision(Pos, 1, 1))
                            {
                                Pos.Y += 8;
                            }
                        }
                        int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Pos, Vector2.Zero, ModContent.ProjectileType<Projectiles.PrimordialJadeWingedSpearDownSpice>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(50f, 110f), 0);
                        Main.projectile[h].rotation = Main.rand.NextFloat((f - 3.5f) / 15f - 0.3f, (f - 3.5f) / 15f + 0.3f);
                    }

                    for (int f = 0; f < 12; f++)
                    {
                        Vector2 Pos = Projectile.Center + new Vector2((f - 5.5f) * 50, -300);
                        for (int i = 0; i < 75; i++)
                        {
                            if (!Collision.SolidCollision(Pos, 1, 1))
                            {
                                Pos.Y += 8;
                            }
                        }
                        int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Pos, Vector2.Zero, ModContent.ProjectileType<Projectiles.PrimordialJadeWingedSpearDownShake>(), 0, Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(17f, 32f) * (Math.Abs(f - 5.5f) + 0.5f), 0);
                        Main.projectile[h].rotation = Main.rand.NextFloat((f - 5.5f) / 15f - 0.1f + Math.Sign(f - 5.5f) * 0.75f, (f - 5.5f) / 15f + 0.1f + Math.Sign(f - 5.5f) * 0.75f);
                    }
                    ShakeEffect();
                    Crash = true;
                }
            }
            else
            {
                if (Projectile.velocity.Length() < 100)
                {
                    Projectile.velocity *= addK;
                }
                if (addK < 1.5f)
                {
                    addK += 0.001f;
                }
            }
            if (Projectile.timeLeft > 6 && !Collision.SolidCollision(Projectile.Center, 1, 1))
            {
                player.velocity = Projectile.velocity * 6;
            }
            if (Projectile.timeLeft < 6)
            {
                player.velocity *= 0.4f;
                Projectile.velocity *= 0.4f;
            }
            return false;
        }
        public override ModProjectile Clone(Projectile projectile)
        {
            var clone = base.Clone(projectile) as PrimordialJadeWingedSpearDown;
            clone.wid = new float[60];
            clone.OldplCen = new Vector2[60];
            clone.statrP = new float[4];
            return clone;
        }
        private float addK = 1.0f;
        private Effect ef2;
        private bool max = false;
        private Vector2 FirstVel = Vector2.Zero;
        private float[] wid;
        private Vector2[] OldplCen;
        private int TrueL = 0;
        private float[] statrP;
        public override void PostDraw(Color lightColor)//主要画绿光
        {
            ef2 = Common.MythContent.QuickEffect("Genshin/Effects/FadeCurseGreen");
            if (FirstVel == Vector2.Zero)
            {
                FirstVel = SafelyNormalize(Projectile.velocity);
            }
            Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);

            for (int i = 1; i < 60; ++i)
            {
                TrueL++;
                if (OldplCen[i] == Vector2.Zero)
                {
                    break;
                }
            }

            for (int d = 0; d < 4; d++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                List<Vertex2D> VxII = new List<Vertex2D>();
                List<Vertex2D> barsII = new List<Vertex2D>();
                Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 2d * Math.PI + Main.time / 32d);
                float widk = Vector2.Dot(SafelyNormalize(deltaPos), SafelyNormalize(Projectile.velocity)) + 1f;
                float widV = (float)Math.Clamp(1.6 - Math.Sqrt(Projectile.velocity.Length() / 16f), 0, 1.6f);
                if (d == 0)
                {
                    deltaPos *= 0;
                    statrP[d] = 1;
                }
                else
                {
                    if (statrP[d] == 0)
                    {
                        statrP[d] = Main.rand.NextFloat(1f, 2f);
                    }
                }
                for (int i = 1; i < 60; ++i)
                {
                    if (OldplCen[i] == Vector2.Zero)
                    {
                        break;
                    }

                    var factor = i / 60f;
                    var w = statrP[d] - factor;
                    if (w > 1)
                    {
                        w = 2 - w;
                    }
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] + FlipVel * wid[i] * widk * widk * 0.6f, new Color(255, 255, 255, 100), new Vector3((float)Math.Sqrt(factor), 1, w)));
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] - FlipVel * wid[i] * widk * widk * 0.6f, new Color(255, 255, 255, 100), new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                if (barsII.Count > 2)
                {
                    VxII.Add(barsII[0]);
                    if (statrP[d] > 1)
                    {
                        statrP[d] = 2 - statrP[d];
                    }
                    var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + SafelyNormalize(Projectile.velocity) * 30, new Color(255, 255, 255, 100), new Vector3(0, 0.5f, statrP[d]));
                    VxII.Add(barsII[1]);
                    VxII.Add(vertex);
                    for (int i = 0; i < barsII.Count - 2; i += 2)
                    {
                        VxII.Add(barsII[i]);
                        VxII.Add(barsII[i + 2]);
                        VxII.Add(barsII[i + 1]);

                        VxII.Add(barsII[i + 1]);
                        VxII.Add(barsII[i + 2]);
                        VxII.Add(barsII[i + 3]);
                    }
                }

                //Texture2D t0 = ModContent.Request<Texture2D>("MythMod/UIImages/heatmapShadeXiaoGreen").Value;
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                ef2.Parameters["uTransform"].SetValue(model * projection);
                ef2.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                ef2.Parameters["maxr"].SetValue(widk * widk / 36f);
                Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("UIImages/heatmapWindCyan");
                Main.graphics.GraphicsDevice.Textures[1] = MythContent.QuickTexture("UIImages/ShakeWave");
                Main.graphics.GraphicsDevice.Textures[2] = MythContent.QuickTexture("UIImages/ShakeWave");
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef2.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            }
        }
        public override bool PreDraw(ref Color lightColor)//主要画黑影
        {
            if (FirstVel == Vector2.Zero)
            {
                FirstVel = SafelyNormalize(Projectile.velocity);
            }
            Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);
            for (int d = 0; d < 7; d++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                List<Vertex2D> VxII = new List<Vertex2D>();
                List<Vertex2D> barsII = new List<Vertex2D>();
                Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 3d * Math.PI + Main.time / 7d);
                float widk = Vector2.Dot(SafelyNormalize(deltaPos), SafelyNormalize(Projectile.velocity)) + 1.2f;
                float widV = (float)Math.Clamp(1.6 - Math.Sqrt(Projectile.velocity.Length() / 16f), 0, 1.6f);
                Color c0 = Color.White;
                if (d == 0)
                {
                    deltaPos *= 0;
                    widk = 4f * Projectile.timeLeft / 60f;
                    c0 = new Color(255, 255, 255, 0);
                }
                for (int i = 1; i < 60; ++i)
                {
                    if (OldplCen[i] == Vector2.Zero)
                    {
                        break;
                    }

                    var factor = i / 60f;
                    var factor2 = i / (float)(TrueL);
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] + FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 1, 1 - factor2)));
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] - FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 0, 1 - factor2)));
                }
                if (barsII.Count > 2)
                {
                    VxII.Add(barsII[0]);
                    var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + SafelyNormalize(Projectile.velocity) * 30, new Color(255, 255, 255, 255), new Vector3(0, 0.5f, 1));
                    VxII.Add(barsII[1]);
                    VxII.Add(vertex);
                    for (int i = 0; i < barsII.Count - 2; i += 2)
                    {
                        VxII.Add(barsII[i]);
                        VxII.Add(barsII[i + 2]);
                        VxII.Add(barsII[i + 1]);

                        VxII.Add(barsII[i + 1]);
                        VxII.Add(barsII[i + 2]);
                        VxII.Add(barsII[i + 3]);
                    }
                }
                Texture2D t0 = MythContent.QuickTexture("UIImages/heatmapShadeXiao");
                if (d == 0)
                {
                    t0 = MythContent.QuickTexture("UIImages/heatmapShadeXiaoGreen");
                }
                Main.graphics.GraphicsDevice.Textures[0] = t0;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            }

            return true;
        }
        public static int CyanStrike = 0;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            CyanStrike = 1;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.XiaoHit>(), 0, 0, Projectile.owner, 0.45f);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}
