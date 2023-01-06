using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class Gear6 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gear");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "齿轮");
        }
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
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
        Vector2 Inpos;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Inpos = player.Center - new Vector2(94 * player.direction, 64) - new Vector2(40, 0) + new Vector2(32 * Projectile.ai[0], 0);
            Projectile.position = Projectile.position * 0.498f + Inpos * 0.502f;
            Projectile.rotation += Ome;
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
                fade *= 0.98f;
            }
            if (Main.mouseLeft && player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.Legendary.MachineSkeGun>())
            {
                Projectile.timeLeft = 60;
            }
        }
        float fade = 0;

        /*public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 v0 = (Projectile.Center - player.Center) * 0.5f;
            Vector2 vPos = Projectile.Center - v0 - new Vector2(0, 1500);
            Vector2 v = (Projectile.Center - vPos) / 30f + Main.npc[(int)Projectile.ai[0]].velocity;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), vPos, v, ModContent.ProjectileType<Projectiles.Ranged.FragransArrowSuper>(), (int)Projectile.ai[1], 15, Main.myPlayer, Projectile.ai[0], 0f);
        }*/
    }
}
