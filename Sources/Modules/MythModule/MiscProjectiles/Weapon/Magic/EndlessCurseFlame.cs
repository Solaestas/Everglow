using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Magic
{
    public class EndlessCurseFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("EndlessCurseFlame");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "不断咒火");
        }
        public override void SetDefaults()
        {
            Projectile.width = 210;
            Projectile.height = 210;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 300;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(39, 60, false);
        }
        private bool boom = false;
        private bool co = false;
        private bool l = true;
        public override void AI()
        {
            if (Projectile.timeLeft > 100)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0f, 10f)).RotatedByRandom(Math.PI * 2f);
                int num3 = Dust.NewDust(Projectile.Center, 0, 0, 75, (float)v.X, (float)v.Y, 0, default(Color), 3f);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].velocity = v;
            }
            else
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0f, 10f) * (float)Projectile.timeLeft / 100f).RotatedByRandom(Math.PI * 2f);
                int num3 = Dust.NewDust(Projectile.Center, 0, 0, 75, (float)v.X, (float)v.Y, 0, default(Color), 3f * (float)Projectile.timeLeft / 100f);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].velocity = v;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(0, 0, 0, 0));
        }
    }
}
