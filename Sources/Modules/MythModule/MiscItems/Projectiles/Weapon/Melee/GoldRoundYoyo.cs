using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Melee
{
    public class GoldRoundYoyo : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(547);
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;
            Projectile.DamageType = DamageClass.Melee;
        }
        internal Vector2 LastPos;
        public override void AI()
        {
            if ((Projectile.Center - LastPos).Length() > 30)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<.GoldRound>(), Projectile.damage / 5, 0.2f, Projectile.owner, 0f, 0f);
                LastPos = Projectile.Center;
            }
            ProjectileExtras.YoyoAI(Projectile.whoAmI, 60f, 300f, 14f);
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = MythContent.QuickTexture("MiscItems/Projectiles/Weapon/Melee/GoldRoundYoyoGlow");
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
        }
    }
}
