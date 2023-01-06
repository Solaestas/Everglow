using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class Gear9 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gear");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "齿轮");
        }
        public override void SetDefaults()
        {
            Projectile.width = 230;
            Projectile.height = 230;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0));
        }
        float Ome = 0;
        float Ang = 0;
        Vector2 Inpos;
        int AimP = -1;
        public override void AI()
        {
            if (AimP == -1)
            {
                for (int d = 0; d < Main.projectile.Length; d++)
                {
                    if (Main.projectile[d].type == ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.Gear3>())
                    {
                        if ((Main.projectile[d].Center - Projectile.Center).Length() < 20)
                        {
                            AimP = d;
                            break;
                        }
                    }
                }
            }
            Player player = Main.player[Projectile.owner];
            Inpos = player.Center - new Vector2(170 * player.direction, 150) - new Vector2(115, 0) + new Vector2(32 * Projectile.ai[0], 0);
            Projectile.position = Projectile.position * 0.498f + Inpos * 0.502f;
            Projectile.rotation -= Ome * 0.66f;
            if (Ome < 0.1f && Projectile.timeLeft > 55)
            {
                Ome += 0.0006f;
            }
            if (Projectile.timeLeft <= 55)
            {
                Ome *= 0.98f;
            }
            if (fade < 1f && Projectile.timeLeft > 55)
            {
                fade += 0.02f;
            }
            if (Projectile.timeLeft <= 55)
            {
                fade *= 0.93f;
            }
            if (Main.mouseLeft && player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.Legendary.MachineSkeGun>())
            {
                Projectile.timeLeft = 60;
            }
        }
        float fade = 0;
        float R1 = 0;
        float R2 = 0;
        float R3 = 0;
        float AR1 = 50;
        float AR2 = 70;
        float AR3 = 80;
        int[] L = new int[9];
        float[] R = new float[9];
        float adding = 1;
        public override void PostDraw(Color lightColor)
        {
            adding += 0.01f;
            if (!Main.gamePaused)
            {
                R1 = R1 * 0.95f + AR1 * 0.05f;
                R2 = R2 * 0.95f + AR2 * 0.05f;
                R3 = R3 * 0.95f + AR3 * 0.05f;
            }
            CirR0 += 0.007f;
            CirPro0 += 0.2f * Ome;
            Rota = 0;
            Vector2 va0 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + Projectile.rotation + 0.52) - Main.screenPosition;
            Vector2 va1 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + 2.0943951023932 + Projectile.rotation + 0.52) - Main.screenPosition;
            Vector2 va2 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + 4.1887902047864 + Projectile.rotation + 0.52) - Main.screenPosition;
            if (AimP != -1)
            {
                va0 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + Main.projectile[AimP].rotation + 0.52) - Main.screenPosition;
                va1 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + 2.0943951023932 + Main.projectile[AimP].rotation + 0.52) - Main.screenPosition;
                va2 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + 4.1887902047864 + Main.projectile[AimP].rotation + 0.52) - Main.screenPosition;
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Color color3 = new Color(fade * fade, fade * fade, fade * fade, 0);
            Vector2 vf = Projectile.Center - Main.screenPosition;
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/Gear9").Value;
            for (int da = 0; da < 3; da++)
            {
                for (int db = 0; db < 3; db++)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    List<Vertex2D> bars = new List<Vertex2D>();
                    float width0 = 36;
                    for (int i = 1; i < 19; ++i)
                    {
                        var factor = i / 50f;
                        float width = width0 * factor * factor + width0 * 0.1f;
                        Vector2 v0 = new Vector2(1, 1).RotatedBy(db / 1.5d * Math.PI);
                        Vector2 v1 = v0.RotatedBy(Math.PI / 2d) * (float)Math.Sin(i / 7d + adding) * 3;
                        Vector2 normalDir = Vector2.Normalize(v0.RotatedBy(Math.PI / 2d));
                        Vector2 vc2 = Vector2.Zero;
                        float dx = 1 / (Math.Clamp(frequency, 4f, 19f));
                        if (da == 0)
                        {
                            vc2 = va0;
                            bars.Add(new Vertex2D(vc2 + normalDir * -width + v0 * i + v1, new Color(255, 255, 255, 0), new Vector3(Math.Clamp(1 - (dx * ((i + Acount) % frequency)), 0, 1), 1, 0)));
                            bars.Add(new Vertex2D(vc2 + normalDir * width + v0 * i + v1, new Color(255, 255, 255, 0), new Vector3(Math.Clamp(1 - (dx * ((i + Acount) % frequency)), 0, 1), 0, 0)));
                        }
                        if (da == 1)
                        {
                            vc2 = va1;
                            bars.Add(new Vertex2D(vc2 + normalDir * -width + v0 * i + v1, new Color(255, 255, 255, 0), new Vector3(Math.Clamp(1 - (dx * ((i + Acount + 50) % frequency)), 0, 1), 1, 0)));
                            bars.Add(new Vertex2D(vc2 + normalDir * width + v0 * i + v1, new Color(255, 255, 255, 0), new Vector3(Math.Clamp(1 - (dx * ((i + Acount + 50) % frequency)), 0, 1), 0, 0)));
                        }
                        if (da == 2)
                        {
                            vc2 = va2;
                            bars.Add(new Vertex2D(vc2 + normalDir * -width + v0 * i + v1, new Color(255, 255, 255, 0), new Vector3(Math.Clamp(1 - (dx * ((i + Acount + 100) % frequency)), 0, 1), 1, 0)));
                            bars.Add(new Vertex2D(vc2 + normalDir * width + v0 * i + v1, new Color(255, 255, 255, 0), new Vector3(Math.Clamp(1 - (dx * ((i + Acount + 100) % frequency)), 0, 1), 0, 0)));
                        }
                    }
                    List<Vertex2D> Vxa = new List<Vertex2D>();
                    if (bars.Count > 2)
                    {
                        Vxa.Add(bars[0]);
                        var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f, new Color(255, 255, 255, 0), new Vector3(0, 0.5f, 1));
                        Vxa.Add(bars[1]);
                        Vxa.Add(vertex);
                        for (int i = 0; i < bars.Count - 2; i += 2)
                        {
                            Vxa.Add(bars[i]);
                            Vxa.Add(bars[i + 2]);
                            Vxa.Add(bars[i + 1]);

                            Vxa.Add(bars[i + 1]);
                            Vxa.Add(bars[i + 2]);
                            Vxa.Add(bars[i + 3]);
                        }
                    }
                    t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/Gear11").Value;
                    Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vxa.ToArray(), 0, Vxa.Count / 3);
                }
            }
            if (!Main.gamePaused)
            {
                Vector2 v0 = va0 + Main.screenPosition;
                Vector2 v1 = va1 + Main.screenPosition;
                Vector2 v2 = va2 + Main.screenPosition;
                if (fade >= 0.8)
                {
                    Acount++;
                    if (frequency > 5)
                    {
                        if (Acount % 4 == 0)
                        {
                            if (frequency > 11)
                            {
                                frequency--;
                            }
                            else
                            {
                                if (Acount % 7 == 0)
                                {
                                    frequency--;
                                }
                            }
                        }
                    }
                }
                else
                {
                    frequency++;
                }
                if ((Acount + 0) % frequency == 0)
                {
                    Vector2 v0s = Main.MouseWorld - v0;
                    v0s = v0s / v0s.Length() * 22f;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), v0, v0s, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.LaserBullet>(), Projectile.damage, 1, Main.myPlayer, Projectile.ai[0], 0f);
                }
                if ((Acount + 50) % frequency == 0)
                {
                    Vector2 v1s = Main.MouseWorld - v1;
                    v1s = v1s / v1s.Length() * 22f;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), v1, v1s, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.LaserBullet>(), Projectile.damage, 1, Main.myPlayer, Projectile.ai[0], 0f);
                }
                if ((Acount + 100) % frequency == 0)
                {
                    Vector2 v2s = Main.MouseWorld - v2;
                    v2s = v2s / v2s.Length() * 22f;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), v2, v2s, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.LaserBullet>(), Projectile.damage, 1, Main.myPlayer, Projectile.ai[0], 0f);
                }
            }
            for (int d = 0; d < 9; d++)
            {
                L[d] = (int)(15 + Math.Sin(CirPro0 * Math.Sin((4 - d) / 7f)) + (20 + d * d + Math.Cos(CirPro0 / 4 / d) / 14));
                R[d] = CirPro0 * 0.4f * d;
                List<Vertex2D> Vx = new List<Vertex2D>();
                for (int h = 0; h < L[d]; h++)
                {
                    color3.A = 0;
                    color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                    color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                    color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                    Vector2 v0 = new Vector2(0, R1 * (d / 7f + 1)).RotatedBy(h / 45d * Math.PI + CirR0 + R[d]);
                    Vector2 v1 = new Vector2(0, R1 * (d / 7f + 1)).RotatedBy((h + 1) / 45d * Math.PI + CirR0 + R[d]);
                    if (((vf + v0) * 1.001f - va0).Length() > 28 && ((vf + v1) * 1.001f - va0).Length() > 28 && ((vf + v0) * 1.001f - va1).Length() > 28 && ((vf + v1) * 1.001f - va1).Length() > 28 && ((vf + v0) * 1.001f - va2).Length() > 28 && ((vf + v1) * 1.001f - va2).Length() > 28)
                    {
                        if (d % 3 == 2)
                        {
                            float H0 = 0;
                            if (d == 8)
                            {
                                H0 = h;
                            }
                            if (h % 8 < (Math.Sin(CirPro0 + d * d + H0) + 1) * 4)
                            {
                                Vx.Add(new Vertex2D(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                                Vx.Add(new Vertex2D(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                                Vx.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
                            }
                        }
                        else
                        {
                            Vx.Add(new Vertex2D(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                            Vx.Add(new Vertex2D(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                            Vx.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
                        }
                    }

                }
                t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/Gear10").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            }

            List<Vertex2D> Vx2 = new List<Vertex2D>();
            for (int h = 0; h < 90; h++)
            {
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                Vector2 v0 = new Vector2(0, R2).RotatedBy(h / 45d * Math.PI + CirR0);
                Vector2 v1 = new Vector2(0, R2).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                if (((vf + v0) * 1.001f - va0).Length() > 28 && ((vf + v1) * 1.001f - va0).Length() > 28 && ((vf + v0) * 1.001f - va1).Length() > 28 && ((vf + v1) * 1.001f - va1).Length() > 28 && ((vf + v0) * 1.001f - va2).Length() > 28 && ((vf + v1) * 1.001f - va2).Length() > 28)
                {
                    Vx2.Add(new Vertex2D(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                    Vx2.Add(new Vertex2D(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                    Vx2.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
                }
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/Gear10").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);

            List<Vertex2D> Vx3 = new List<Vertex2D>();
            for (int h = 0; h < 15; h++)
            {
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                Vector2 v0 = new Vector2(0, R3).RotatedBy(h / 45d * Math.PI + CirR0);
                Vector2 v1 = new Vector2(0, R3).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                Vx3.Add(new Vertex2D(vf + v0, color3, new Vector3(((h) / 15f) % 1f, 0, 0)));
                Vx3.Add(new Vertex2D(vf + v1, color3, new Vector3(((0.999f + h) / 15f) % 1f, 0, 0)));
                Vx3.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + h) / 15f) % 1f, 1, 0)));
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/Gearlogo").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx3.ToArray(), 0, Vx3.Count / 3);
        }
        int frequency = 30;
        int Acount = 1;
        private Effect ef;
        float Rota = 0;
        float CirR0 = 0;
        float CirPro0 = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
