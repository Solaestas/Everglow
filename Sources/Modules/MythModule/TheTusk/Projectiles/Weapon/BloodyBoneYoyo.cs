using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    public class BloodyBoneYoyo : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(549);
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 200f;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            ProjectileExtras.YoyoAI(Projectile.whoAmI, 60f, 200f, 14f);
            if (Projectile.oldPosition != Vector2.Zero)
            {
                for (int g = 0; g < Projectile.velocity.Length() * 2.5f; g++)
                {
                    Vector2 a0 = new Vector2(Projectile.width, Projectile.height) / 2f;
                    Vector2 v3 = Projectile.oldPosition + a0;
                    Vector2 v4 = Vector2.Normalize(Projectile.velocity) * 0.6f;
                    int num92 = Dust.NewDust(v3 + v4 * g - new Vector2(4, 4), 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 2f) * 0.4f);
                    Main.dust[num92].noGravity = true;
                    Main.dust[num92].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * 0.5f;
                }
            }
        }
        int hit = 0;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float a = Main.rand.NextFloat(0, 100f);
            Player player = Main.player[Projectile.owner];
            if (hit >= 4)
            {
                for (int y = 0; y < 5; y++)
                {
                    Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity.RotatedBy(Math.PI * y / 2.5 + a) / Projectile.velocity.Length() * 9f, ModContent.ProjectileType<Projectiles.Weapon.LittleTusk0>(), Projectile.damage / 2, Projectile.knockBack, player.whoAmI);
                }
                for (int j = 0; j < 5; j++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
                        int dus = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.VampireHeal, v2.X, v2.Y, 100, default, 0.4f);
                        Main.dust[dus].noGravity = true;
                        Main.dust[dus].velocity = v2;
                    }
                    Vector2 v1 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
                    int r = Dust.NewDust(new Vector2(target.Center.X, target.Center.Y) - new Vector2(4, 4) + v1, 0, 0, DustID.VampireHeal, v3.X, v3.Y, 0, default, 2f);
                    Main.dust[r].noGravity = true;
                    Main.dust[r].velocity = v3;
                }
                for (int i = 0; i < 10; i++)
                {
                    Vector2 v = new Vector2(0, Main.rand.NextFloat(2, 7)).RotatedByRandom(MathHelper.TwoPi);
                    Dust.NewDust(target.position, target.width, target.height, DustID.VampireHeal, v.X, v.Y, 150, default, Main.rand.NextFloat(0.5f, 1.2f));
                }
                hit = 0;
            }
            for (int y = 0; y < 16; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 2f));
                Main.dust[num90].noGravity = false;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.4f, 2.2f), 0).RotatedByRandom(Math.PI * 2d);
            }
            hit++;
        }
    }
}
