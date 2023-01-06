using Everglow.Sources.Modules.MythModule.TheTusk.Projectiles;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Melee
{
    public class GoldRoundYoyo : ModProjectile
    {
        private bool M = false;
        private float X = 0;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(547);
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;
            Projectile.DamageType = DamageClass.Melee;
        }
        private int K = 0;
        private Vector2 Prepos;
        public override void AI()
        {
            if (K == 0)
            {
                Prepos = Projectile.Center;
                K += 1;
            }
            if ((Projectile.Center - Prepos).Length() > 30)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.GoldRound>(), Projectile.damage / 5, 0.2f, Projectile.owner, 0f, 0f);
                Prepos = Projectile.Center;
            }
            ProjectileExtras.YoyoAI(Projectile.whoAmI, 60f, 300f, 14f);
            /*if (K % 15 == 0)
            {
                Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0.4f, base.mod.ProjectileType("LightEsclipse"), (int)((double)Projectile.damage * 3f), Projectile.knockBack, Projectile.owner, 0f, 0f);
            }*/
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Melee/GoldRoundYoyoGlow").Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
        }
    }
}
