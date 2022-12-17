using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    public class ToothMagic : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tooth Magic Ball");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血色法球");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1080;
            Projectile.alpha = 0;
            Projectile.penetrate = 3;
            Projectile.scale = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 45;
        }
        int MaxP = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            /*int num91 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(12, 12) - Projectile.velocity, 16, 16, DustID.Blood, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1f, 2.6f));
            Main.dust[num91].noGravity = true;
            Main.dust[num91].velocity *= 0.5f;
            int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(12, 12) - Projectile.velocity, 16, 16, 183, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1f, 2.6f));
            Main.dust[num90].noGravity = true;
            Main.dust[num90].velocity *= 0.5f;*/

            float num2 = Projectile.Center.X;
            float num3 = Projectile.Center.Y;
            float num4 = 600f;
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
                if (MaxP < 4)
                {
                    if (Main.rand.Next(13) == 1)
                    {
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.Weapon.ToothMagicSplit>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, player.whoAmI);
                        MaxP++;
                    }
                }
            }
            if (Collision.SolidCollision(Projectile.Center + Projectile.velocity * 30f, 1, 1))
            {
                if (MaxP < 4)
                {
                    if (Main.rand.Next(13) == 1)
                    {
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.Weapon.ToothMagicSplit>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, player.whoAmI);
                        MaxP++;
                    }
                }
            }
            if (Tokill >= 0 && Tokill <= 2)
            {
                Projectile.Kill();
            }
            if (Tokill <= 44 && Tokill > 0)
            {
                Projectile.position = Projectile.oldPosition;
                Projectile.velocity = Projectile.oldVelocity;
            }
            Tokill--;
            int r2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Vector2.Normalize(Projectile.velocity), 0, 0, DustID.Blood, 0, 0, 0, default(Color), 1f);
            Main.dust[r2].noGravity = true;
            addi++;
        }
        int Tokill = -1;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Weapon.ToothMagicHit>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Weapon.ToothMagicHit2>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 0f, 0f);
            for (int d = 0; d < 35; d++)
            {
                int r2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Vector2.Normalize(Projectile.velocity), 0, 0, DustID.Blood, 0, 0, 0, default(Color), 1f);
                Main.dust[r2].noGravity = true;
                Main.dust[r2].velocity = new Vector2(0, Main.rand.NextFloat(4f, 10f)).RotatedByRandom(6.283);
            }
            for (int d = 0; d < 35; d++)
            {
                int r2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Vector2.Normalize(Projectile.velocity), 0, 0, 183, 0, 0, 0, default(Color), 1f);
                Main.dust[r2].noGravity = true;
                Main.dust[r2].velocity = new Vector2(0, Main.rand.NextFloat(4f, 10f)).RotatedByRandom(6.283);
            }
            Projectile.velocity = Projectile.oldVelocity;
            Tokill = 45;//0.75s后消掉
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Weapon.ToothMagicHit>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Weapon.ToothMagicHit2>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 0f, 0f);
            for (int d = 0; d < 35; d++)
            {
                int r2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Vector2.Normalize(Projectile.velocity), 0, 0, DustID.Blood, 0, 0, 0, default(Color), 1f);
                Main.dust[r2].noGravity = true;
                Main.dust[r2].velocity = new Vector2(0, Main.rand.NextFloat(4f, 10f)).RotatedByRandom(6.283);
            }
            for (int d = 0; d < 35; d++)
            {
                int r2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Vector2.Normalize(Projectile.velocity), 0, 0, 183, 0, 0, 0, default(Color), 1f);
                Main.dust[r2].noGravity = true;
                Main.dust[r2].velocity = new Vector2(0, Main.rand.NextFloat(4f, 10f)).RotatedByRandom(6.283);
            }
            Projectile.velocity = Projectile.oldVelocity;
            Tokill = 45;//0.75s后消掉
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
        }
        int addi = 0;
        private Effect ef;
        int TrueL = 1;
        Vector2 ovel = Vector2.One;
        float DelX = -1;
        int frameC = 0;
        public override void PostDraw(Color lightColor)
        {
            if (Tokill < 0)
            {
                Texture2D texture2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Projectiles/Weapon/BloodProj").Value;
                Color Cc = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16));
                Cc.A = 250;
                if (addi % 8 == 0 && !Main.gamePaused)
                {
                    if (frameC < 6)
                    {
                        frameC++;
                    }
                    else
                    {
                        frameC = 0;
                    }
                }
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, new Rectangle(0, frameC * 32, 32, 32), Cc, 0, new Vector2(16, 16), 0.7f, SpriteEffects.None, 0);
            }
            if (DelX == -1)
            {
                DelX = Main.rand.NextFloat(1f, 40f);
            }

            for (int f = 0; f < 4; f++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
                float width = 2;
                if (Projectile.timeLeft < 60)
                {
                    width = Projectile.timeLeft / 30f;
                }
                TrueL = 0;
                for (int i = 1; i < Projectile.oldPos.Length; ++i)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero)
                        break;
                    if (i == 1)
                    {
                        for (int j = 0; j < Projectile.oldPos.Length - 2; ++j)
                        {
                            if (Projectile.oldPos[i] == Projectile.oldPos[i - 1])
                            {
                                i++;
                            }
                            else
                            {
                                //i+=2;
                                break;
                            }
                        }
                    }
                    TrueL++;
                }
                if (Projectile.velocity.Length() > 0.05f)
                {
                    ovel = Projectile.velocity;
                }
                for (int i = 1; i < Projectile.oldPos.Length; ++i)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero)
                        break;
                    if (i == 1)
                    {
                        for (int j = 0; j < Projectile.oldPos.Length - 2; ++j)
                        {
                            if (Projectile.oldPos[i] == Projectile.oldPos[i - 1])
                            {
                                i++;
                            }
                            else
                            {
                                //i+=2;
                                break;
                            }
                        }
                    }
                    var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = i / (float)TrueL;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);

                    float WaveScale = 0;//摆幅
                    float WaveFreq = 4;//轨迹摆频
                    float TimeFreq = 10;//时间摆频
                    float CosWid = 9;//粗细
                    if (f == 1)
                    {
                        WaveScale = 8;
                        WaveFreq = 2.7f;
                        TimeFreq = 12.1f;
                        CosWid = 1.7f;
                    }
                    if (f == 2)
                    {
                        WaveScale = 5;
                        WaveFreq = 5.7f;
                        TimeFreq = 8.3f;
                        CosWid = 1.9f;
                    }
                    if (f == 3)
                    {
                        WaveScale = 12;
                        WaveFreq = 4;
                        TimeFreq = 10;
                        CosWid = 1;
                    }
                    if (TrueL - i < 25)
                    {
                        CosWid *= (float)(Math.Cos((25 - Math.Clamp(TrueL - i, 0, 25)) / 25d * Math.PI) + 1) / 2f;
                        WaveScale *= (float)(Math.Cos((25 - Math.Clamp(TrueL - i, 0, 25)) / 25d * Math.PI) + 1) / 2f;
                    }
                    float SinFx0 = (float)(Math.Sin(Main.time / TimeFreq + f / 3d * 2 * Math.PI + (i * i) / WaveFreq / 12f + DelX)) * WaveScale;//摆动函数
                    float CosFx0 = (float)(Math.Cos(Main.time / TimeFreq + f / 3d * 2 * Math.PI + (i * i) / WaveFreq / 12f + DelX) + 1) * CosWid;//求导简便计算透视投影
                    if (f == 0)
                    {
                        CosFx0 = 0.5f * CosWid;
                    }
                    Vector2 P0 = Projectile.oldPos[i] + normalDir * SinFx0 + normalDir * width * CosFx0 + new Vector2(9, 9);
                    Vector2 P1 = Projectile.oldPos[i] + normalDir * SinFx0 + normalDir * -width * CosFx0 + new Vector2(9, 9);
                    Color c0 = Lighting.GetColor((int)(P0.X / 16f), (int)(P0.Y / 16f));
                    Color c1 = Lighting.GetColor((int)(P1.X / 16f), (int)(P1.Y / 16f));
                    if (f != 0)
                    {
                        c0.A = 250;
                        c1.A = 250;
                    }
                    bars.Add(new VertexBase.CustomVertexInfo(P0 - Main.screenPosition - Vector2.Normalize(ovel) * 30, c0, new Vector3(factor, 1, w)));
                    bars.Add(new VertexBase.CustomVertexInfo(P1 - Main.screenPosition - Vector2.Normalize(ovel) * 30, c1, new Vector3(factor, 0, w)));
                }
                List<VertexBase.CustomVertexInfo> Vx = new List<VertexBase.CustomVertexInfo>();
                if (bars.Count > 2)
                {
                    Vx.Add(bars[0]);
                    Vector2 P2 = (bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(ovel) * 30;
                    Color c2 = Lighting.GetColor((int)(P2.X / 16f), (int)(P2.Y / 16f));
                    var vertex = new VertexBase.CustomVertexInfo(P2, c2, new Vector3(0, 0.5f, 1));
                    Vx.Add(bars[1]);
                    Vx.Add(vertex);
                    for (int i = 0; i < bars.Count - 2; i += 2)
                    {
                        Vx.Add(bars[i]);
                        Vx.Add(bars[i + 2]);
                        Vx.Add(bars[i + 1]);

                        Vx.Add(bars[i + 1]);
                        Vx.Add(bars[i + 2]);
                        Vx.Add(bars[i + 3]);
                    }
                }
                Texture2D t = ModContent.Request<Texture2D>("MythMod/UIimages/BloodBallLine").Value;
                if (f == 0)
                {
                    t = ModContent.Request<Texture2D>("MythMod/UIimages/BloodLine").Value;
                }
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            }
        }
    }
}