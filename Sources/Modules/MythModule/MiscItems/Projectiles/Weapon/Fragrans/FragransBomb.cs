using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	public class FragransBomb : ModProjectile
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
            Projectile.timeLeft = 5;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            for (int y = 0; y < 30; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 6; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
        }
        private bool boom = false;
        private bool co = false;
        private bool l = true;
        public override void AI()
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(0, 0, 0, 0));
        }
    }
}
