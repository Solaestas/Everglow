using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	public class FragransAim : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Aim");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "瞄准");
        }
        public override void SetDefaults()
        {
            Projectile.width = 116;
            Projectile.height = 116;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((1 / (fade * fade)), (1 / (fade * fade)), (1 / (fade * fade)), 0));
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = Main.npc[(int)Projectile.ai[0]].Center - new Vector2(58);
            if (Projectile.timeLeft < 30)
            {
                fade = (Projectile.timeLeft) / 5f + 1;
            }
            Projectile.scale = fade * 0.5f;
        }
        float fade = 4;

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 v0 = (Projectile.Center - player.Center) * 0.5f;
            Vector2 vPos = Projectile.Center - v0 - new Vector2(0, 1500);
            Vector2 v = (Projectile.Center - vPos) / 30f + Main.npc[(int)Projectile.ai[0]].velocity;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), vPos, v, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransArrowSuper>(), (int)Projectile.ai[1], 15, Main.myPlayer, Projectile.ai[0], 0f);
        }
    }
}
