using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    public class GlowSporeBead : SlingshotAmmo
    {
        public override void SetDef()
        {
        }
        public override void AI()
        {
            if (TimeTokill >= 0 && TimeTokill <= 2)
            {
                Projectile.Kill();
            }
            if (TimeTokill <= 15 && TimeTokill > 0)
            {
                Projectile.velocity = Projectile.oldVelocity;
            }
            TimeTokill--;
            if (TimeTokill < 0)
            {
                Projectile.velocity.Y += 0.17f;
                int index = Dust.NewDust(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<Dusts.JungleSpore>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.6f, 1.1f));
                Main.dust[index].velocity = Projectile.velocity * 0.5f;

                int index2 = Dust.NewDust(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<Dusts.JungleSmogStoppedByTile> (), 0f, 0f, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
                Main.dust[index2].velocity = Projectile.velocity * 0.5f;
                Main.dust[index2].alpha = (int)(Main.dust[index2].scale * 50);

                for (float v = 0; v < Projectile.velocity.Length(); v += 1f)
                {
                    int type = ModContent.DustType<Dusts.LittleJungleSpore>();
                    if (Main.rand.NextBool(8))
                    {
                        int r2 = Dust.NewDust(Projectile.Center + Projectile.velocity - Utils.SafeNormalize(Projectile.velocity, Vector2.Zero) * v - new Vector2(4), 0, 0, type, 0, 0, 200, default, Projectile.ai[0] * 2 + 0.4f);
                        Main.dust[r2].velocity = Projectile.velocity * 0.1f;
                        Main.dust[r2].noGravity = true;
                    }
                }
            }
            else
            {
                if (TimeTokill < 10)
                {
                    Projectile.damage = 0;
                    Projectile.friendly = false;
                }
                Projectile.velocity *= 0f;
            }
        }
        public override void DrawTrail()
        {
            DrawShade();
            List<Vertex2D> bars = new List<Vertex2D>();
            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    if (i == 1)
                    {
                        return;
                    }
                    break;
                }

                TrueL = i;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                float width = 4;
                if (Projectile.timeLeft <= 30)
                {
                    width *= Projectile.timeLeft / 30f;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);

                var factor = i / (float)TrueL;
                var color = Color.Lerp(new Color(255, 255, 255, 0) * 0.6f, new Color(0, 0, 0, 0), factor);

                float fac1 = factor * 3 + (float)(-Main.timeForVisualEffects * 0.03) + 100000;
                float fac2 = ((i + 1) / (float)TrueL) * 3 + (float)(-Main.timeForVisualEffects * 0.03) + 100000;
                //TODO:925分钟之后会炸

                fac1 %= 1f;
                fac2 %= 1f;
                if (fac1 > fac2)
                {
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 0, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 1, 0)));
                    if (i < Projectile.oldPos.Length - 1)
                    {
                        float fac3 = 1 - fac1;
                        fac3 /= 3 / (float)TrueL;
                        normalDir = Projectile.oldPos[i] - Projectile.oldPos[i + 1];
                        normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);
                        bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
                        bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 1, 0)));

                        bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(0, 0, 0)));
                        bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(0, 1, 0)));
                    }
                }
                else
                {
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 0, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 1, 0)));
                }
            }

            if (bars.Count > 2)
            {
                Texture2D t = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/SporeTrace");

                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        private void DrawShade()
        {
            List<Vertex2D> bars = new List<Vertex2D>();
            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    if (i == 1)
                    {
                        return;
                    }
                    break;
                }

                TrueL = i;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                float width = 4;
                if (Projectile.timeLeft <= 30)
                {
                    width *= Projectile.timeLeft / 30f;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);

                var factor = i / (float)TrueL;
                var color = Color.Lerp(Color.White * 0.6f, new Color(0, 0, 0, 0), factor);

                float fac1 = factor * 3 + (float)(-Main.timeForVisualEffects * 0.03) + 100000;
                float fac2 = ((i + 1) / (float)TrueL) * 3 + (float)(-Main.timeForVisualEffects * 0.03) + 100000;

                fac1 %= 1f;
                fac2 %= 1f;
                if (fac1 > fac2)
                {
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 0, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 1, 0)));
                    if (i < Projectile.oldPos.Length - 1)
                    {
                        float fac3 = 1 - fac1;
                        fac3 /= 3 / (float)TrueL;
                        normalDir = Projectile.oldPos[i] - Projectile.oldPos[i + 1];
                        normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);
                        bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
                        bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 1, 0)));

                        bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(0, 0, 0)));
                        bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(0, 1, 0)));
                    }
                }
                else
                {
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 0, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 1, 0)));
                }
            }
            if (bars.Count > 2)
            {
                Texture2D t = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/SporeTraceShade");
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        public override void AmmoHit()
        {
            SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NormalHit>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.velocity.Length(), Main.rand.NextFloat(6.283f));
            float Power = Projectile.ai[0] + 0.5f;
            for (int x = 0; x < 34; x++)
            {
                int index = Dust.NewDust(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<Dusts.JungleSpore>(), 0f, 0f, 100, default, Power);
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.5f, 4f)).RotatedByRandom(6.283);
                int index2 = Dust.NewDust(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<Dusts.JungleSmogStoppedByTile>(), 0f, 0f, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
                Main.dust[index2].velocity = new Vector2(0, Main.rand.NextFloat(3.5f, 4f)).RotatedByRandom(6.283);
                Main.dust[index2].alpha = (int)(Main.dust[index2].scale * 50);

                for (float v = 0; v < Projectile.velocity.Length(); v += 1f)
                {
                    int type = ModContent.DustType<Dusts.LittleJungleSpore>();
                    if (Main.rand.NextBool(8))
                    {
                        int r2 = Dust.NewDust(Projectile.Center + Projectile.velocity - Utils.SafeNormalize(Projectile.velocity, Vector2.Zero) * v - new Vector2(4), 0, 0, type, 0, 0, 200, default, Power);
                        Main.dust[r2].velocity = new Vector2(0, Main.rand.NextFloat(3.5f, 4f)).RotatedByRandom(6.283);
                        Main.dust[r2].noGravity = true;
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }
    }
}
