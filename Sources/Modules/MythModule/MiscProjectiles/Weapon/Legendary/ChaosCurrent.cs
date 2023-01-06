using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class ChaosCurrent : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ChaosCurrent");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "混沌爆流");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1080;
            Projectile.extraUpdates = 200;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
        }
        public override void AI()
        {
            if ((Projectile.Center - Main.MouseWorld).Length() < 16)
            {
                Projectile.Kill();
            }
            if (Projectile.timeLeft > 1072)
                return;
            /*Vector2 v = new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(Math.PI * 2f);
            int num3 = Dust.NewDust(Projectile.Center, 0, 0, 114, (float)v.X, (float)v.Y, 0, default(Color), 2f);
            Main.dust[num3].noGravity = true;
            Main.dust[num3].velocity = v;*/
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/ElectricCurrency2"), Main.player[Projectile.owner].Center);
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChaosCurrent2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0], Projectile.ai[1]);
        }
    }
}