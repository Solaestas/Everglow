using Terraria.Audio;
using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class MachineFitMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MachineFitMissile");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "统帅导弹");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 64;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        private bool initialization = true;
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            if (Projectile.timeLeft < 61)
            {
                Projectile.friendly = true;
            }
            if (Projectile.timeLeft > 54)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(-6, 4) - Projectile.velocity / Projectile.velocity.Length() * 12f, 16, 16, 6, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1f, 2.6f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity *= 0.5f;
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(-6, 4) - Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, ModContent.DustType<LanternMoon.Dusts.Flame2 >(), 0, 0, 0, default(Color), 4f);
                Main.dust[r].noGravity = true;
                Main.dust[r].rotation = Main.rand.NextFloat(0, (float)(MathHelper.TwoPi));
            }
            else
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(-6, 4) - Projectile.velocity / Projectile.velocity.Length() * 24f, 16, 16, 6, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1f, 2.6f) * Projectile.timeLeft / 30f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity *= 0.5f;
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(-6, 4) - Projectile.velocity / Projectile.velocity.Length() * 24f, 0, 0, ModContent.DustType<LanternMoon.Dusts.Flame2 >(), 0, 0, 0, default(Color), Projectile.timeLeft / 7.5f);
                Main.dust[r].noGravity = true;
                Main.dust[r].rotation = Main.rand.NextFloat(0, (float)(MathHelper.TwoPi));
            }
            if (Projectile.timeLeft < 49)
            {
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
                    Projectile.velocity.X = (Projectile.velocity.X * 20f + num9) / 21f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num10) / 21f;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 600);
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft < 63)
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
                for (int j = 0; j < 15; j++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
                        Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
                        int dus = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, v2.X, v2.Y, 100, default(Color), 1.8f);
                        Main.dust[dus].noGravity = true;
                        Main.dust[dus].velocity = v2;
                    }
                    Vector2 v1 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
                    int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + v1, 0, 0, ModContent.DustType<LanternMoon.Dusts.Flame2 >(), v3.X, v3.Y, 0, default(Color), 6f);
                    Main.dust[r].noGravity = true;
                    Main.dust[r].velocity = v3;
                }
                for (int i = 0; i < 47; i++)
                {
                    Vector2 v = new Vector2(0, Main.rand.NextFloat(2, 7)).RotatedByRandom(MathHelper.TwoPi);
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 188, v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0.8f, 2.1f));
                }
                for (int j = 0; j < 200; j++)
                {
                    if ((Main.npc[j].Center - Projectile.Center).Length() < 64 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly)
                    {
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                        Player player = Main.player[Projectile.owner];
                        player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[0] / 100f)));
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    Gore.NewGore(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(0.7f, 2.2f)).RotatedByRandom(MathHelper.TwoPi), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 1.6f));
                }
            }
        }
    }
}
