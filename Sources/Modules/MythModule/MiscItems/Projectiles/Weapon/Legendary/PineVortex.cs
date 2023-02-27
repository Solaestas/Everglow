//using MythMod.Common.Players;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;
using Terraria.Localization;


namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class PineVortex : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pine Vortex");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "松针爆弹");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 10000000;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        private bool Outofcontrol = false;
        private int Jnj = 15;
        private int Jnj2 = 6;
        private float St = 0;
        private int[] Aimpro = new int[1000];
        private float dmg = 0;
        public override void AI()
        {
            if (Aimpro[0] == 0)
            {
                for (int g = 0; g < 1000; g++)
                {
                    Aimpro[g] = -1;
                }
            }
            Projectile.ai[1] = addi;
            Player p = Main.player[Projectile.owner];

            if (Main.mouseRight && St < 4000)
            {
                St += 50f;
            }
            if (!Main.mouseRight && St > 0)
            {
                St -= 300f;
            }
            if (Main.mouseRight)
            {
                if (!Outofcontrol)
                {
                    Projectile.Center = Main.MouseWorld;
                }

                Projectile.velocity = Projectile.position - Projectile.oldPosition;
                for (int g = 0; g < 1000; g++)
                {
                    if (Main.projectile[g].type == 336)
                    {
                        if (Main.projectile[g].friendly)
                        {
                            Vector2 v = Projectile.Center;
                            Vector2 v0 = ((v - Main.projectile[g].Center) / ((v - Main.projectile[g].Center).Length() * (v - Main.projectile[g].Center).Length()) * St).RotatedBy((Math.PI / 2d) / (v - Main.projectile[g].Center).Length() * 5f);
                            Main.projectile[g].velocity = v0 * 0.15f + Main.projectile[g].velocity * 0.85f;
                            if (Main.projectile[g].aiStyle > -1)
                            {
                                Aimpro[g] = Main.projectile[g].aiStyle;
                            }
                            Main.projectile[g].aiStyle = -1;
                            Main.projectile[g].rotation = (float)(Math.Atan2(Main.projectile[g].velocity.Y, Main.projectile[g].velocity.X) + Math.PI / 2d);
                            Vector2 v1 = Projectile.Center - Main.projectile[g].Center;
                            if (v1.Length() <= 15)
                            {
                                Main.projectile[g].Kill();
                                dmg += Main.projectile[g].damage / 30f;
                            }
                        }
                    }
                    if (Main.projectile[g].type == 337)
                    {
                        if (Main.projectile[g].friendly)
                        {
                            Vector2 v = Projectile.Center;
                            Vector2 v0 = ((v - Main.projectile[g].Center) / ((v - Main.projectile[g].Center).Length() * (v - Main.projectile[g].Center).Length()) * St).RotatedBy((Math.PI / 2d) / (v - Main.projectile[g].Center).Length() * 5f);
                            Main.projectile[g].velocity = v0 * 0.15f + Main.projectile[g].velocity * 0.85f;
                            if (Main.projectile[g].aiStyle > -1)
                            {
                                Aimpro[g] = Main.projectile[g].aiStyle;
                            }
                            Main.projectile[g].aiStyle = -1;
                            Main.projectile[g].tileCollide = true;
                            float rot0 = (float)(Math.Atan2(Main.projectile[g].velocity.Y, Main.projectile[g].velocity.X) + Math.PI / 2d);
                            if (Main.projectile[g].timeLeft < 1195)
                            {
                                Main.projectile[g].rotation = rot0 * 0.5f + Main.projectile[g].rotation * 0.5f;
                            }

                            Vector2 v1 = Projectile.Center - Main.projectile[g].Center;
                            if (v1.Length() <= 15)
                            {
                                Main.projectile[g].Kill();
                                dmg += Main.projectile[g].damage / 30f;
                            }
                        }
                    }
                }
            }
            else
            {
                Projectile.friendly = true;
                Outofcontrol = true;
                if (Projectile.velocity.Length() > 20)
                {
                    Projectile.velocity = Projectile.velocity / Projectile.velocity.Length() * 19.99f;
                }
                if (St < 30)
                {
                    Projectile.Kill();
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
                    Projectile.velocity.X = (Projectile.velocity.X * 90f + num9) / 91f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 90f + num10) / 91f;
                }
            }
            if (Projectile.velocity.Length() > 10)
            {
                for (int h = 0; h < Projectile.velocity.Length(); h += 5)
                {
                    Vector2 v0 = -Vector2.Normalize(Projectile.velocity);

                    Vector2 vz = new Vector2(0, Main.rand.NextFloat(0.1f, 1.2f)).RotatedByRandom(MathHelper.TwoPi);
                    int r1 = Dust.NewDust(Projectile.Center + h * v0 - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, DustID.GreenMoss, vz.X, vz.Y, 0, default(Color), St / 7000f);
                    Main.dust[r1].noGravity = true;
                    vz = new Vector2(0, Main.rand.NextFloat(0.1f, 1.2f)).RotatedByRandom(MathHelper.TwoPi);
                    int r = Dust.NewDust(Projectile.Center + h * v0 - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, ModContent.DustType<MiscItems.Dusts.Freeze>(), vz.X, vz.Y, 0, default(Color), St / 700f);
                    Main.dust[r].noGravity = true;

                }
            }
            else
            {
                Vector2 vz = new Vector2(0, Main.rand.NextFloat(0.1f, 1.2f)).RotatedByRandom(MathHelper.TwoPi);
                int r1 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, DustID.GreenMoss, vz.X, vz.Y, 0, default(Color), St / 7000f);
                Main.dust[r1].noGravity = true;
                vz = new Vector2(0, Main.rand.NextFloat(0.1f, 1.2f)).RotatedByRandom(MathHelper.TwoPi);
                int r = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, ModContent.DustType<MiscItems.Dusts.Freeze>(), vz.X, vz.Y, 0, default(Color), St / 700f);
                Main.dust[r].noGravity = true;
            }

            dmg *= 0.994f;
            Projectile.damage = (int)dmg;
            if (Main.mouseRight)
            {
                if (addi < 50)
                {
                    addi += 1;
                }
            }
            else
            {
                addi *= 0.95f;
            }
            addj += 0.01f;
            Addrot += addi * 0.0013;
        }
        float addi = 0;
        private static float addj = 0;
        public override void Kill(int timeLeft)
        {
            for (int g = 0; g < 1000; g++)
            {
                if (Aimpro[g] != -1)
                {
                    Main.projectile[g].aiStyle = Aimpro[g];
                }
            }
            if (St < 200)
            {
                return;
            }
			Player player = Main.LocalPlayer;
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
            SoundEngine.PlaySound(SoundID.Item36, Projectile.Center);
            /*for (int j = 0; j < 15; j++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(6.2f, 7f)).RotatedByRandom(Math.PI * 2) * Main.rand.Next(1500, 2000) / 1000f;
                int zi = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<Projectiles.Ranged.Cyanline>(), (int)((double)Projectile.damage) / 3, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }*/
            /*for (int i = 0; i < 47; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2, 7)).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 188, v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0.8f, 5f));
            }*/
            int num10 = 0;
            for (int i = 0; i < 40; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2.9f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2) * St / 4000f;
                int num5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<MiscItems.Dusts.Freeze>(), 0f, 0f, 100, Color.White, (float)(4f * Math.Log10(Projectile.damage)) * St / 4000f);
                Main.dust[num5].velocity = v;
            }
            for (int i = 0; i < 30; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2) * St / 4000f;
                int num5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) + v * 20f, Projectile.width, Projectile.height, ModContent.DustType<MiscItems.Dusts.Freeze>(), 0f, 0f, 100, Color.White, (float)(12f * Math.Log10(Projectile.damage)) * St / 4000f);
                Main.dust[num5].velocity = v * 0.1f;
                Main.dust[num5].rotation = Main.rand.NextFloat(0, (float)(MathHelper.TwoPi));
            }
            for (int h = 0; h < 70; h++)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) * 4 + 15).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<TheFirefly.Dusts.PureBlue>(), 0, 0, 0, default(Color), 15f * Main.rand.NextFloat(0.4f, 1.1f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3;
                //Main.dust[r].dustIndex = (int)(Math.Cos(h * Math.PI / 10d + Projectile.ai[0]) * 100d);
            }
            for (int i = 0; i < 27; i++)
            {
                Gore.NewGore(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(2, 7) * St / 4000f).RotatedByRandom(MathHelper.TwoPi), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 3.2f) * St / 4000f);
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 150 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    Player player2 = Main.player[Projectile.owner];
                    player2.dpsDamage += (int)(Projectile.damage * (1 + Projectile.ai[0] / 100f));
                }
            }
        }
        private static Vector2[,] Vlaser = new Vector2[30, 200];
        private static double Addrot = 0;
        private static float[] RanDot = new float[15];
        public override void PostDraw(Color lightColor)
        {
            if (RanDot[0] == 0)
            {
                for (int k = 0; k < 15; ++k)
                {
                    RanDot[k] = Main.rand.NextFloat(0, 10f);
                }
            }
            for (int k = 0; k < 15; ++k)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                List<Vertex2D> bars = new List<Vertex2D>();

                float step = 0.4f;
                float fx = (Math.Clamp(addi, 0f, 50f)) / 50f;
                int Count = (int)(fx * fx * 200);
                for (int m = 0; m < Count; ++m)
                {
                    Vlaser[k, m] = new Vector2(0, step).RotatedBy(k / 7.5d * Math.PI) * m;
                }
                for (int i = 1; i < Count; ++i)
                {
                    double rotfx = addi / (double)(i + 50) * 2;
                    if (Vlaser[k, i] == Vector2.Zero)
                        break;
                    var normalDir = Vlaser[k, i - 1] - Vlaser[k, i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(-rotfx * rotfx + Addrot * 0.25);

                    var factor = (float)Math.Pow((Count - i), 0.2) / 1f + Math.Clamp(addi, 0f, 200f) / 6f;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    float width = (addi / 5f) * (float)(Math.Cos(i / (double)Count * Math.PI) + 1);

                    Lighting.AddLight(Vlaser[k, i], 0, Math.Clamp((int)(127 * (Count - i - 1) / 5f), 0, 127) / 50f, Math.Clamp((int)(255 * (Count - i - 1) / 5f), 0, 255) / 50f);
                    if (Count - i < 5)
                    {
                        bars.Add(new Vertex2D(Projectile.Center + Vlaser[k, i].RotatedBy(-rotfx * rotfx + Addrot * 0.25) + normalDir * width - Main.screenPosition, new Color(0, Math.Clamp((int)(127 * (Count - i - 1) / 5f), 0, 127), Math.Clamp((int)(255 * (Count - i - 1) / 5f), 0, 255), 0), new Vector3((factor - addj + RanDot[k]) % 1f, 1, w)));
                        bars.Add(new Vertex2D(Projectile.Center + Vlaser[k, i].RotatedBy(-rotfx * rotfx + Addrot * 0.25) + normalDir * -width - Main.screenPosition, new Color(0, Math.Clamp((int)(127 * (Count - i - 1) / 5f), 0, 127), Math.Clamp((int)(255 * (Count - i - 1) / 5f), 0, 255), 0), new Vector3((factor - addj + RanDot[k]) % 1f, 0, w)));
                    }
                    else
                    {
                        bars.Add(new Vertex2D(Projectile.Center + Vlaser[k, i].RotatedBy(-rotfx * rotfx + Addrot * 0.25) + normalDir * width - Main.screenPosition, new Color(0, 127, 255, 0), new Vector3((factor - addj + RanDot[k]) % 1f, 1, w)));
                        bars.Add(new Vertex2D(Projectile.Center + Vlaser[k, i].RotatedBy(-rotfx * rotfx + Addrot * 0.25) + normalDir * -width - Main.screenPosition, new Color(0, 127, 255, 0), new Vector3((factor - addj + RanDot[k]) % 1f, 0, w)));
                    }
                }
                List<Vertex2D> Vx = new List<Vertex2D>();
                if (bars.Count > 2)
                {
                    Vx.Add(bars[0]);
                    var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f, new Color(0, 127, 255, 0), new Vector3(0, 0.5f, 1));
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
                Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ForgeWave2").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            }
        }
        public static void DrawAll(SpriteBatch sb)
        {
            for (int s = 0; s < Main.projectile.Length; s++)
            {
                if (Main.projectile[s].active)
                {
                    if (Main.projectile[s].type == ModContent.ProjectileType<PineVortex>())
                    {
                        if (RanDot[0] == 0)
                        {
                            for (int k = 0; k < 15; ++k)
                            {
                                RanDot[k] = Main.rand.NextFloat(0, 10f);
                            }
                        }
                        for (int k = 0; k < 15; ++k)
                        {
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                            List<Vertex2D> bars = new List<Vertex2D>();

                            float step = 2;
                            float fx = (Math.Clamp(Main.projectile[s].ai[1], 0f, 50f)) / 50f;
                            int Count = (int)(fx * fx * 200);
                            for (int m = 0; m < Count; ++m)
                            {
                                Vlaser[k, m] = new Vector2(0, step).RotatedBy(k / 7.5d * Math.PI) * m;
                            }
                            for (int i = 1; i < Count; ++i)
                            {
                                double rotfx = Main.projectile[s].ai[1] / (double)(i + 50) * 2;
                                if (Vlaser[k, i] == Vector2.Zero)
                                    break;
                                var normalDir = Vlaser[k, i - 1] - Vlaser[k, i];
                                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)).RotatedBy(-rotfx * rotfx - Addrot);

                                var factor = (float)Math.Pow((Count - i), 0.2) / 1f + Math.Clamp(Main.projectile[s].ai[1], 0f, 200f) / 6f;
                                var w = MathHelper.Lerp(1f, 0.05f, factor);
                                float width = (Main.projectile[s].ai[1] / 5f) * (float)(Math.Cos(i / (double)Count * Math.PI) + 1);
                                if (Count - i < 5)
                                {
                                    bars.Add(new Vertex2D(Main.projectile[s].Center + Vlaser[k, i].RotatedBy(-rotfx * rotfx - Addrot) + normalDir * width - Main.screenPosition, Color.Blue, new Vector3((factor - addj + RanDot[k]) % 1f, 1, w)));
                                    bars.Add(new Vertex2D(Main.projectile[s].Center + Vlaser[k, i].RotatedBy(-rotfx * rotfx - Addrot) + normalDir * -width - Main.screenPosition, Color.Blue, new Vector3((factor - addj + RanDot[k]) % 1f, 0, w)));
                                }
                                else
                                {
                                    bars.Add(new Vertex2D(Main.projectile[s].Center + Vlaser[k, i].RotatedBy(-rotfx * rotfx - Addrot) + normalDir * width - Main.screenPosition, Color.Blue, new Vector3((factor - addj + RanDot[k]) % 1f, 1, w)));
                                    bars.Add(new Vertex2D(Main.projectile[s].Center + Vlaser[k, i].RotatedBy(-rotfx * rotfx - Addrot) + normalDir * -width - Main.screenPosition, Color.Blue, new Vector3((factor - addj + RanDot[k]) % 1f, 0, w)));
                                }
                            }
                            List<Vertex2D> Vx = new List<Vertex2D>();
                            if (bars.Count > 2)
                            {
                                Vx.Add(bars[0]);
                                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f, Color.Blue, new Vector3(0, 0.5f, 1));
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
                            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ForgeWave2").Value;
                            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
                        }
                    }
                }
            }
        }
    }
}
