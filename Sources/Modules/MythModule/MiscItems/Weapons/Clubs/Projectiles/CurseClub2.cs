using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class CurseClub2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CurseClub");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "咒火棍");
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = false;
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
            if (Projectile.timeLeft < 710)
            {
                Projectile.friendly = true;
            }
            if (Projectile.timeLeft > 100)
            {
                float li = Main.rand.NextFloat(-32f, 32f);
                int num25 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(li, li).RotatedBy(Projectile.rotation), 0, 0, 75, 0, 0, 0, default(Color), 1.8f * Omega * 4f);
                Main.dust[num25].noGravity = true;
                Main.dust[num25].noLight = true;
                Main.dust[num25].velocity = new Vector2(li, li).RotatedBy(Math.PI / 2 + Projectile.rotation) * 0.3f * Omega * 2.5f;
            }
            else
            {
                float li = Main.rand.NextFloat(-32f, 32f);
                int num25 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(li, li).RotatedBy(Projectile.rotation), 0, 0, 75, 0, 0, 0, default(Color), 1.8f * Projectile.timeLeft / 100f * Omega * 4f);
                Main.dust[num25].noGravity = true;
                Main.dust[num25].noLight = true;
                Main.dust[num25].velocity = new Vector2(li, li).RotatedBy(Math.PI / 2 + Projectile.rotation) * 0.3f * Omega * 2.5f;
            }
            Lighting.AddLight(Projectile.Center, (float)Projectile.timeLeft / 1200f * 0 / 255f, (float)Projectile.timeLeft / 1200f * 255 / 255f, (float)Projectile.timeLeft / 1200f * 0 / 255f);
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
                        int Dam = (int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)) - Main.npc[j].defDefense;
                        if (Dam < 1)
                        {
                            Dam = 1;
                        }
                        Main.npc[j].StrikeNPC(Dam, Projectile.knockBack, Projectile.direction, Main.rand.Next(100) < Projectile.ai[0]);
                        Player p = Main.player[Projectile.owner];
                        p.dpsDamage += (int)(Dam * (100 + Projectile.ai[0]) / 100d);
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
            target.AddBuff(69, 600);
            for (int y = 0; y < 40; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, 75, 0f, 0f, 100, default(Color), Main.rand.NextFloat(3.6f, 7f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 2.5f)).RotatedByRandom(Math.PI * 2d) * Omega * 7f;
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
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity * i / im, null, new Color(i / im * Omega * 2.5f, i / im * Omega * 2.5f, i / im * Omega * 2.5f, i / im * Projectile.alpha / 255f * Omega * 2.5f), Projectile.rotation + Omega * i * 0.4f, new Vector2(32, 32), Projectile.scale, effects, 0f);
                }
            }
            return false;
        }
    }
}