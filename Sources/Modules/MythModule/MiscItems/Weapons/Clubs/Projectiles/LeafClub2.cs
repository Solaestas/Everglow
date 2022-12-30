using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class LeafClub2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LeafClub2");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "叶绿棍");
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 720;
            Projectile.localNPCHitCooldown = 0;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 55;
        }
        public override void AI()
        {
            Omega *= 0.994f;
            Projectile.alpha = (int)(55 + (float)(400 - (float)Projectile.timeLeft) / 2);
            Projectile.rotation += Omega;
            Projectile.velocity.X *= 0.99f;
            Projectile.velocity.Y *= 0.99f;
            Lighting.AddLight(Projectile.Center, (float)Projectile.timeLeft / 1200f * 0 / 255f, (float)Projectile.timeLeft / 1200f * 205 / 255f, (float)Projectile.timeLeft / 1200f * 100 / 255f);
            float num2 = Projectile.Center.X;
            float num3 = Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1) && Omega > 0.1f)
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
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), Projectile.knockBack, Projectile.direction, Main.rand.Next(100) < Projectile.ai[0]);
                        Player p = Main.player[Projectile.owner];
                        p.dpsDamage += (int)(Projectile.damage * (100 + Projectile.ai[0]) / 100d);
                        Projectile.penetrate--;
                        NPC target = Main.npc[j];
                    }
                }
            }
            if (flag)
            {
                float num8 = 50f;
                Vector2 vector1 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num9 = num2 - vector1.X;
                float num10 = num3 - vector1.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                Projectile.velocity.X = (Projectile.velocity.X * 20f + num9) / 21f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num10) / 21f;
                Projectile.velocity *= 0.65f;
            }
            if (Stopping)
            {
                Projectile.velocity = Projectile.oldVelocity * 0.95f;
                Projectile.timeLeft -= 5;
                Omega *= 0.98f;
            }
            if (Projectile.timeLeft == 710)
            {
                Projectile.tileCollide = true;
            }
        }
        public override void Kill(int timeLeft)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft > 60)
            {
                return new Color?(new Color(255, 255, 255, 0));
            }
            else
            {
                return new Color?(new Color((float)Projectile.timeLeft / 60f, (float)Projectile.timeLeft / 60f, (float)Projectile.timeLeft / 60f, 0));
            }
        }
        bool Stopping = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            Stopping = true;
            Projectile.velocity = Projectile.oldVelocity * 0.95f;
            Projectile.timeLeft -= 15;
            Omega *= 0.95f;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player p = Main.player[Projectile.owner];
            Vector2 vk = new Vector2(0, 5).RotatedByRandom(6.28);
            for (int g = 0; g < 8; g++)
            {
                vk = vk / vk.Length() * 5f;
                vk = vk.RotatedBy(g / 4d * Math.PI);
                int i = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + vk, vk, ModContent.ProjectileType<Projectiles.LeafClub3>(), Projectile.damage / 6, Projectile.knockBack * 0.5f, p.whoAmI, p.GetCritChance(DamageClass.Melee), 0);
                Main.projectile[i].rotation = Main.rand.NextFloat(0f, 6.283f);
            }
            Omega *= 0.5f;
            Projectile.timeLeft -= 120;
            Projectile.position -= Projectile.velocity * 2;
            Projectile.velocity *= -0.5f;
        }
        private float Omega = 0.4f;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            float im = Omega * 50;
            if (im >= 1)
            {
                for (int i = 0; i < im; i++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity * i / im, null, new Color(i / im * Omega * 0.5f, i / im * Omega * 2.5f, i / im * Omega * 0.5f, i / im * Projectile.alpha / 255f * Omega * 2.5f), Projectile.rotation + Omega * i * 0.4f, new Vector2(32, 32), Projectile.scale, effects, 0f);
                }
            }
            return false;
        }
    }
}